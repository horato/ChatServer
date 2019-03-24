using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Chat.Server.Database;
using Chat.Server.Tests.TestSupport.Services.DTOs;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace Chat.Server.Tests.TestSupport.Services
{
    public class DatabaseCompareService : IDatabaseCompareService
    {
        private readonly IDatabaseHelperService _databaseHelperService;

        public DatabaseCompareService(IDatabaseHelperService databaseHelperService)
        {
            _databaseHelperService = databaseHelperService;
        }

        public string CompareDatabasesAndGenerateChangescripts(string sourceDatabaseName, string targetDatabaseName)
        {
            return _databaseHelperService.RunInTransaction(() => CompareDatabasesAndGenerateChangescriptsInternal(sourceDatabaseName, targetDatabaseName));
        }

        private string CompareDatabasesAndGenerateChangescriptsInternal(string sourceDatabaseName, string targetDatabaseName)
        {
            var sb = new StringBuilder();
            var sourceTables = GetAllTables(sourceDatabaseName).ToList();
            var targetTables = GetAllTables(targetDatabaseName).ToList();

            var removedTables = sourceTables.Where(x => targetTables.All(y => x.Schema != y.Schema || x.TableName != y.TableName)).ToList();
            var addedTables = targetTables.Where(x => sourceTables.All(y => x.Schema != y.Schema || x.TableName != y.TableName)).ToList();

            removedTables.ForEach(x => sourceTables.Remove(x));
            addedTables.ForEach(x => targetTables.Remove(x));

            AppendCreateTable(addedTables, sb);
            //TODO: Drop table 
            AppendAlterTable(sourceTables, targetTables, sb);
            AppendConstraints(sourceTables, targetTables, addedTables, removedTables, sb);
            AppendForeignKeys(sourceTables, targetTables, addedTables, removedTables, sb);
            AppendIndexes(sourceTables, targetTables, addedTables, removedTables, sb);

            return sb.ToString();
        }

        #region Data load

        private IEnumerable<TablesDTO> GetAllTables(string databaseName)
        {
            var tables = LoadTables(databaseName);
            LoadColumns(tables, databaseName);
            LoadIndexes(tables, databaseName);
            LoadConstraints(tables, databaseName);
            LoadForeignKeys(tables, databaseName);

            return tables;
        }

        #region Tables

        private IEnumerable<TablesDTO> LoadTables(string databaseName)
        {
            var query = $@"
USE {databaseName};

SELECT TABLE_CATALOG as DatabaseName,
TABLE_SCHEMA as [Schema],
TABLE_NAME as TableName,
TABLE_TYPE as TableType
FROM INFORMATION_SCHEMA.TABLES";

            var tables = SessionContainer.Session
                .CreateSQLQuery(query)
                .SetResultTransformer(new AliasToBeanConstructorResultTransformer(typeof(TablesDTO).GetConstructors().Single()))
                .List<TablesDTO>();

            return tables;
        }

        #endregion

        #region Columns

        private void LoadColumns(IEnumerable<TablesDTO> tables, string databaseName)
        {
            var query = $@"
USE {databaseName};

SELECT TABLE_CATALOG as [Database],
TABLE_SCHEMA as [Schema],
TABLE_NAME as TableName,
COLUMN_NAME as ColumnName,
ORDINAL_POSITION as [Order],
COLUMN_DEFAULT as DefaultValue,
cast((case IS_NULLABLE when 'YES' then 1 else 0 end) as bit) as IsNullable,
DATA_TYPE as DataType,
CHARACTER_MAXIMUM_LENGTH as [MaxLength],
NUMERIC_PRECISION as NumericPrecision,
NUMERIC_PRECISION_RADIX as NumericRadix,
NUMERIC_SCALE as NumericScale,
DATETIME_PRECISION as DateTimePrecision,
COLLATION_NAME as Collation
FROM INFORMATION_SCHEMA.COLUMNS";

            var columns = SessionContainer.Session
                .CreateSQLQuery(query)
                .SetResultTransformer(new AliasToBeanConstructorResultTransformer(typeof(ColumnDTO).GetConstructors().Single()))
                .List<ColumnDTO>();

            foreach (var table in tables)
            {
                table.AddColumns(columns.Where(x => x.Database == table.DatabaseName && x.Schema == table.Schema && x.TableName == table.TableName));
            }
        }

        #endregion

        #region Indexes

        private void LoadIndexes(IEnumerable<TablesDTO> tables, string databaseName)
        {
            var query = $@" 
USE {databaseName};

SELECT
    OBJECT_SCHEMA_NAME(i.[object_id]) as SchemaName,
    OBJECT_NAME(i.[object_id]) as TableName,
    c.Name as ColumnName, 
    i.name as IndexName,
    i.type_desc as IndexType,
    i.is_unique as IsUnique,
    ic.is_descending_key as IsDescending,
    ic.is_included_column as IsIncluded,
    i.has_filter as IsFiltered,
    i.filter_definition as FilterDefinition
FROM
    sys.indexes AS i 
INNER JOIN 
    sys.index_columns AS ic 
    ON i.[object_id] = ic.[object_id] 
    AND i.index_id = ic.index_id
INNER JOIN 
    sys.columns c
    ON ic.column_id = c.column_id
    AND ic.[object_id] = c.[object_id]
WHERE 
    objectProperty(i.[object_id], 'IsUserTable') = 1 AND
    i.type in (1, 2) AND
    i.is_primary_key = 0 AND
    i.is_unique_constraint = 0 AND
    i.is_disabled = 0 AND
    i.is_hypothetical = 0
";

            var indexes = SessionContainer.Session
                .CreateSQLQuery(query)
                .SetResultTransformer(new AliasToBeanConstructorResultTransformer(typeof(IndexDTO).GetConstructors().Single()))
                .List<IndexDTO>();

            foreach (var table in tables)
            {
                table.AddIndexes(indexes.Where(x => x.SchemaName == table.Schema && x.TableName == table.TableName));
            }
        }

        #endregion

        #region Constraints

        private void LoadConstraints(IEnumerable<TablesDTO> tables, string databaseName)
        {
            var query = $@"
USE {databaseName};

SELECT 
	const.TABLE_CATALOG as DatabaseName, 
	const.TABLE_SCHEMA as TableSchema,
	const.TABLE_NAME as TableName, 
	col.COLUMN_NAME as ColumnName,
	const.CONSTRAINT_NAME as ConstraintName, 
	const.CONSTRAINT_TYPE as ConstraintType
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS const
JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE col
on
	const.CONSTRAINT_CATALOG = col.CONSTRAINT_CATALOG AND
	const.CONSTRAINT_SCHEMA = col.CONSTRAINT_SCHEMA AND 
	const.CONSTRAINT_NAME = col.CONSTRAINT_NAME
WHERE 
	const.CONSTRAINT_TYPE IN ('UNIQUE','PRIMARY KEY') 
";

            var constraints = SessionContainer.Session
                .CreateSQLQuery(query)
                .SetResultTransformer(new AliasToBeanConstructorResultTransformer(typeof(ConstraintDTO).GetConstructors().Single()))
                .List<ConstraintDTO>();

            foreach (var table in tables)
            {
                table.AddConstraints(constraints.Where(x => x.DatabaseName == table.DatabaseName && x.SchemaName == table.Schema && x.TableName == table.TableName));
            }

        }

        #endregion

        #region ForeginKeys

        private void LoadForeignKeys(IEnumerable<TablesDTO> tables, string databaseName)
        {
            var query = $@"
USE {databaseName};

SELECT 
   OBJECT_SCHEMA_NAME(F.parent_object_id) AS SchemaName,
   OBJECT_NAME(f.parent_object_id) AS TableName,
   COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ColumnName,
   f.name AS ForeignKeyName,
   OBJECT_SCHEMA_NAME(F.referenced_object_id) AS ReferenceSchemaName,
   OBJECT_NAME (f.referenced_object_id) AS ReferenceTableName,
   COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS ReferenceColumnName
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc
ON f.OBJECT_ID = fc.constraint_object_id";

            var constraints = SessionContainer.Session
                .CreateSQLQuery(query)
                .SetResultTransformer(new AliasToBeanConstructorResultTransformer(typeof(ForeignKeyDTO).GetConstructors().Single()))
                .List<ForeignKeyDTO>();

            foreach (var table in tables)
            {
                table.AddForeignKeys(constraints.Where(x => x.SchemaName == table.Schema && x.TableName == table.TableName));
            }
        }

        #endregion

        #endregion

        #region Changes

        #region CreateTable

        private void AppendCreateTable(IEnumerable<TablesDTO> tables, StringBuilder sb)
        {
            foreach (var table in tables)
            {
                sb.AppendLine($"CREATE TABLE [{table.Schema}].[{table.TableName}]");
                sb.AppendLine("(");
                foreach (var column in table.Columns.OrderBy(x => x.Order))
                {
                    if (column.Order > 1)
                        sb.AppendLine(",");

                    AppendColumnDefinition(column, sb);
                }

                sb.AppendLine();
                sb.AppendLine(");");
                sb.AppendLine();
            }
        }

        private void AppendColumnDefinition(ColumnDTO column, StringBuilder sb)
        {
            sb.Append($" [{column.ColumnName}] {column.DataType}{(column.MaxLength > 0 ? $"({column.MaxLength}) " : column.MaxLength == -1 ? "(max) " : "")}{(string.IsNullOrWhiteSpace(column.Collation) ? "" : $"COLLATE {column.Collation}")}{(column.IsNullable ? "" : " NOT")} NULL{(string.IsNullOrWhiteSpace(column.DefaultValue) ? "" : $" DEFAULT {column.DefaultValue}")}");
        }

        #endregion

        #region AlterTable

        private void AppendAlterTable(IEnumerable<TablesDTO> sourceTables, IEnumerable<TablesDTO> targetTables, StringBuilder sb)
        {
            foreach (var sourceTable in sourceTables)
            {
                var targetTable = targetTables.Single(x => x.Schema == sourceTable.Schema && x.TableName == sourceTable.TableName);

                var sourceColumns = sourceTable.Columns.ToList();
                var targetColumns = targetTable.Columns.ToList();

                var removedColumns = sourceColumns.Where(x => targetColumns.All(y => x.ColumnName != y.ColumnName)).ToList();
                var addedColumns = targetColumns.Where(x => sourceColumns.All(y => x.ColumnName != y.ColumnName)).ToList();

                removedColumns.ForEach(x => sourceColumns.Remove(x));
                addedColumns.ForEach(x => targetColumns.Remove(x));

                AppendAddColumns(addedColumns, targetTable, sb);
                AppendRemoveColumns(removedColumns, targetTable, sb);
                AppendAlterColumns(sourceColumns, targetColumns, targetTable, sb);
            }
        }

        private void AppendAddColumns(IList<ColumnDTO> addedColumns, TablesDTO targetTable, StringBuilder sb)
        {
            if (!addedColumns.Any())
                return;

            sb.AppendLine($"ALTER TABLE {targetTable.TableName}");
            sb.AppendLine("ADD");
            foreach (var column in addedColumns)
            {
                if (addedColumns.IndexOf(column) > 0)
                    sb.AppendLine(",");

                AppendColumnDefinition(column, sb);
            }

            sb.AppendLine();
        }

        private void AppendRemoveColumns(IList<ColumnDTO> removedColumns, TablesDTO targetTable, StringBuilder sb)
        {
            if (!removedColumns.Any())
                return;

            sb.Append($"!!!!ALTER TABLE {targetTable.TableName} DROP COLUMN ");
            foreach (var column in removedColumns)
            {
                if (removedColumns.IndexOf(column) > 0)
                    sb.AppendLine(",");

                sb.Append(column.ColumnName);
            }

            sb.AppendLine();
        }

        private void AppendAlterColumns(IEnumerable<ColumnDTO> sourceColumns, IEnumerable<ColumnDTO> targetColumns, TablesDTO targetTable, StringBuilder sb)
        {
            foreach (var sourceColumn in sourceColumns)
            {
                var targetColumn = targetColumns.Single(x => x.ColumnName == sourceColumn.ColumnName);
                if (!sourceColumn.IsDifferentFrom(targetColumn))
                    continue;

                sb.AppendLine($"ALTER TABLE {targetTable.TableName}");
                sb.Append($"ALTER COLUMN {targetColumn.ColumnName}");
                if (sourceColumn.DataType != targetColumn.DataType || sourceColumn.MaxLength != targetColumn.MaxLength)
                {
                    sb.Append($" {targetColumn.DataType}{(targetColumn.MaxLength > 0 ? $"({targetColumn.MaxLength})" : targetColumn.MaxLength == -1 ? "(max)" : "")}");
                }

                if (sourceColumn.Collation != targetColumn.Collation)
                {
                    sb.Append($" COLLATE {targetColumn.Collation}");
                }

                if (sourceColumn.IsNullable != targetColumn.IsNullable)
                {
                    sb.Append($"{(targetColumn.IsNullable ? "" : " NOT")} NULL");
                }

                // This needs to go last
                if (sourceColumn.DefaultValue != targetColumn.DefaultValue)
                {
                    if (string.IsNullOrWhiteSpace(targetColumn.DefaultValue))
                    {
                        var query = $@"
USE {sourceColumn.Database}
GO
-- returns name of a column's default value constraint 
SELECT
    default_constraints.name
FROM 
    sys.all_columns

        INNER JOIN
    sys.tables
        ON all_columns.object_id = tables.object_id

        INNER JOIN 
    sys.schemas
        ON tables.schema_id = schemas.schema_id

        INNER JOIN
    sys.default_constraints
        ON all_columns.default_object_id = default_constraints.object_id

WHERE 
        schemas.name = '{sourceColumn.Schema}'
    AND tables.name = '{sourceColumn.TableName}'
    AND all_columns.name = '{sourceColumn.ColumnName}'";

                        var constraintName = SessionContainer.Session.CreateSQLQuery(query).UniqueResult<string>();
                        sb.AppendLine();
                        sb.AppendLine($"ALTER TABLE {targetTable.TableName} DROP CONSTRAINT {constraintName};");
                    }
                    else
                    {
                        sb.AppendLine($" DEFAULT {targetColumn.DefaultValue}");
                    }
                }
            }
        }

        #endregion

        #region Constraints (PK/UNIQUE)

        private void AppendConstraints(IEnumerable<TablesDTO> sourceTables, IEnumerable<TablesDTO> targetTables, IEnumerable<TablesDTO> addedTables, IEnumerable<TablesDTO> removedTables, StringBuilder sb)
        {
            foreach (var table in removedTables)
            {
                foreach (var constraint in table.Constraints)
                {
                    AppendDropConstraint(constraint, sb);
                }
            }

            foreach (var table in addedTables)
            {
                foreach (var constraint in table.Constraints)
                {
                    AppendCreateConstraint(constraint, sb);
                }
            }

            foreach (var sourceTable in sourceTables)
            {
                var targetTable = targetTables.Single(x => x.Schema == sourceTable.Schema && x.TableName == sourceTable.TableName);
                AppendConstraints(sourceTable.Constraints, targetTable.Constraints, sb);
            }
        }

        private void AppendConstraints(IEnumerable<ConstraintDTO> sourceConstraints, IEnumerable<ConstraintDTO> targetConstraints, StringBuilder sb)
        {
            var source = sourceConstraints.ToList();
            var target = targetConstraints.ToList();

            var removed = source.Where(x => target.All(y => x.ConstraintType != y.ConstraintType || x.ColumnName != y.ColumnName)).ToList();
            var added = target.Where(x => source.All(y => x.ConstraintType != y.ConstraintType || x.ColumnName != y.ColumnName)).ToList();

            removed.ForEach(x => source.Remove(x));
            added.ForEach(x => target.Remove(x));

            foreach (var constraint in removed)
            {
                AppendDropConstraint(constraint, sb);
            }

            foreach (var constraint in added)
            {
                AppendCreateConstraint(constraint, sb);
            }

            foreach (var sourceConstraint in source)
            {
                var targetConstraint = target.Single(x => x.ConstraintType == sourceConstraint.ConstraintType && x.ColumnName == sourceConstraint.ColumnName);
                if (sourceConstraint.ColumnName != targetConstraint.ColumnName || sourceConstraint.ConstraintType != targetConstraint.ConstraintType)
                {
                    AppendDropConstraint(targetConstraint, sb);
                    AppendCreateConstraint(targetConstraint, sb);
                }
            }
        }

        private void AppendDropConstraint(ConstraintDTO constraint, StringBuilder sb)
        {
            sb.AppendLine($"ALTER TABLE [{constraint.SchemaName}].[{constraint.TableName}] DROP CONSTRAINT {constraint.ConstraintName}");
        }

        private void AppendCreateConstraint(ConstraintDTO constraint, StringBuilder sb)
        {
            sb.AppendLine($"ALTER TABLE [{constraint.SchemaName}].[{constraint.TableName}] ADD CONSTRAINT {constraint.ConstraintName} {constraint.ConstraintType} ({constraint.ColumnName})");
        }

        #endregion

        #region FK

        private void AppendForeignKeys(IEnumerable<TablesDTO> sourceTables, IEnumerable<TablesDTO> targetTables, IEnumerable<TablesDTO> addedTables, IEnumerable<TablesDTO> removedTables, StringBuilder sb)
        {
            foreach (var table in removedTables)
            {
                foreach (var foreignKey in table.ForeignKeys)
                {
                    AppendDropForeignKey(foreignKey, sb);
                }
            }

            foreach (var table in addedTables)
            {
                foreach (var foreignKey in table.ForeignKeys)
                {
                    AppendCreateForeignKey(foreignKey, sb);
                }
            }

            foreach (var sourceTable in sourceTables)
            {
                var targetTable = targetTables.Single(x => x.Schema == sourceTable.Schema && x.TableName == sourceTable.TableName);
                AppendForeignKeys(sourceTable.ForeignKeys, targetTable.ForeignKeys, sb);
            }
        }

        private void AppendForeignKeys(IEnumerable<ForeignKeyDTO> sourceKeys, IEnumerable<ForeignKeyDTO> targetKeys, StringBuilder sb)
        {
            var source = sourceKeys.ToList();
            var target = targetKeys.ToList();

            var removed = source.Where(x => target.All(y => x.ForeignKeyName != y.ForeignKeyName)).ToList();
            var added = target.Where(x => source.All(y => x.ForeignKeyName != y.ForeignKeyName)).ToList();

            removed.ForEach(x => source.Remove(x));
            added.ForEach(x => target.Remove(x));

            foreach (var foreignKey in removed)
            {
                AppendDropForeignKey(foreignKey, sb);
            }

            foreach (var foreignKey in added)
            {
                AppendCreateForeignKey(foreignKey, sb);
            }

            foreach (var sourceKey in source)
            {
                var targetKey = target.Single(x => x.ForeignKeyName == sourceKey.ForeignKeyName);
                if (sourceKey.SchemaName != targetKey.SchemaName
                    || sourceKey.TableName != targetKey.TableName
                    || sourceKey.ColumnName != targetKey.ColumnName
                    || sourceKey.ReferenceSchemaName != targetKey.ReferenceSchemaName
                    || sourceKey.ReferenceTableName != targetKey.ReferenceTableName
                    || sourceKey.ReferenceColumnName != targetKey.ReferenceColumnName)
                {
                    AppendDropForeignKey(targetKey, sb);
                    AppendCreateForeignKey(targetKey, sb);
                }
            }
        }

        private void AppendDropForeignKey(ForeignKeyDTO key, StringBuilder sb)
        {
            sb.AppendLine($"ALTER TABLE [{key.SchemaName}].[{key.TableName}] DROP CONSTRAINT {key.ForeignKeyName}");
        }

        private void AppendCreateForeignKey(ForeignKeyDTO key, StringBuilder sb)
        {
            sb.AppendLine($"ALTER TABLE [{key.SchemaName}].[{key.TableName}] ADD CONSTRAINT {key.ForeignKeyName} FOREIGN KEY ({key.ColumnName}) REFERENCES [{key.ReferenceSchemaName}].[{key.ReferenceTableName}] ([{key.ReferenceColumnName}])");
        }

        #endregion

        #region Indexes

        private void AppendIndexes(IEnumerable<TablesDTO> sourceTables, IEnumerable<TablesDTO> targetTables, IEnumerable<TablesDTO> addedTables, IEnumerable<TablesDTO> removedTables, StringBuilder sb)
        {
            foreach (var table in removedTables)
            {
                foreach (var index in table.Indexes)
                {
                    AppendDropIndex(index, sb);
                }
            }

            foreach (var table in addedTables)
            {
                foreach (var index in table.Indexes)
                {
                    AppendCreateIndex(index, sb);
                }
            }

            foreach (var sourceTable in sourceTables)
            {
                var targetTable = targetTables.Single(x => x.Schema == sourceTable.Schema && x.TableName == sourceTable.TableName);
                AppendIndex(sourceTable.Indexes, targetTable.Indexes, sb);
            }
        }

        private void AppendIndex(IEnumerable<IndexDTO> sourceIndexes, IEnumerable<IndexDTO> targetIndexes, StringBuilder sb)
        {
            var source = sourceIndexes.ToList();
            var target = targetIndexes.ToList();

            var removed = source.Where(x => target.All(y => x.IndexName != y.IndexName)).ToList();
            var added = target.Where(x => source.All(y => x.IndexName != y.IndexName)).ToList();

            removed.ForEach(x => source.Remove(x));
            added.ForEach(x => target.Remove(x));

            foreach (var index in removed)
            {
                AppendDropIndex(index, sb);
            }

            foreach (var index in added)
            {
                AppendCreateIndex(index, sb);
            }

            foreach (var sourceIndex in source)
            {
                var targetIndex = target.Single(x => x.IndexName == sourceIndex.IndexName);
                if (sourceIndex.SchemaName != targetIndex.SchemaName
                    || sourceIndex.TableName != targetIndex.TableName
                    || sourceIndex.ColumnName != targetIndex.ColumnName
                    || sourceIndex.IndexType != targetIndex.IndexType
                    || sourceIndex.IsUnique != targetIndex.IsUnique
                    || sourceIndex.IsDescending != targetIndex.IsDescending
                    || sourceIndex.IsIncluded != targetIndex.IsIncluded
                    || sourceIndex.IsFiltered != targetIndex.IsFiltered
                    || sourceIndex.FilterDefinition != targetIndex.FilterDefinition)
                {
                    AppendDropIndex(targetIndex, sb);
                    AppendCreateIndex(targetIndex, sb);
                }
            }
        }

        private void AppendDropIndex(IndexDTO constraint, StringBuilder sb)
        {
            sb.AppendLine($"DROP INDEX {constraint.IndexName} ON [{constraint.SchemaName}].[{constraint.ColumnName}]");
        }

        private void AppendCreateIndex(IndexDTO constraint, StringBuilder sb)
        {
            sb.AppendLine($"CREATE {(constraint.IsUnique ? "UNIQUE " : "")}{constraint.IndexType} INDEX {constraint.IndexName} ON [{constraint.SchemaName}].[{constraint.TableName}] ({constraint.ColumnName} {(constraint.IsDescending ? "DESC" : "ASC")}){(constraint.IsFiltered ? $" WHERE {constraint.FilterDefinition}" : "")}");
        }

        #endregion

        #endregion
    }
}
