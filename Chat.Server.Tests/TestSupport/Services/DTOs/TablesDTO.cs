using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server.Tests.TestSupport.Services.DTOs
{
    public class TablesDTO
    {
        private readonly IList<ColumnDTO> _columns = new List<ColumnDTO>();
        private readonly IList<IndexDTO> _indexes = new List<IndexDTO>();
        private readonly IList<ConstraintDTO> _constraints = new List<ConstraintDTO>();
        private readonly IList<ForeignKeyDTO> _foreignKeys = new List<ForeignKeyDTO>();

        public string DatabaseName { get; }
        public string Schema { get; }
        public string TableName { get; }
        public string TableType { get; }
        public IEnumerable<ColumnDTO> Columns => _columns;
        public IEnumerable<IndexDTO> Indexes => _indexes;
        public IEnumerable<ConstraintDTO> Constraints => _constraints;
        public IEnumerable<ForeignKeyDTO> ForeignKeys => _foreignKeys;
        
        public TablesDTO(string databaseName, string schema, string tableName, string tableType)
        {
            DatabaseName = databaseName;
            Schema = schema;
            TableName = tableName;
            TableType = tableType;
        }

        public void AddColumns(IEnumerable<ColumnDTO> columns)
        {
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));

            foreach (var column in columns)
            {
                AddColumn(column);
            }
        }

        private void AddColumn(ColumnDTO column)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));

            _columns.Add(column);
        }

        internal void AddIndexes(IEnumerable<IndexDTO> indexes)
        {
            if (indexes == null)
                throw new ArgumentNullException(nameof(indexes));

            foreach (var constraint in indexes)
            {
                AddIndex(constraint);
            }
        }

        private void AddIndex(IndexDTO index)
        {
            if (index == null)
                throw new ArgumentNullException(nameof(index));

            _indexes.Add(index);
        }

        public void AddConstraints(IEnumerable<ConstraintDTO> constraints)
        {
            if (constraints == null)
                throw new ArgumentNullException(nameof(constraints));

            foreach (var constraint in constraints)
            {
                AddConstraint(constraint);
            }
        }

        private void AddConstraint(ConstraintDTO constraint)
        {
            if (constraint == null)
                throw new ArgumentNullException(nameof(constraint));

            _constraints.Add(constraint);
        }

        public void AddForeignKeys(IEnumerable<ForeignKeyDTO> keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));

            foreach (var key in keys)
            {
                AddForeignKey(key);
            }
        }

        private void AddForeignKey(ForeignKeyDTO key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            _foreignKeys.Add(key);
        }
    }
}
