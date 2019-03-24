using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server.Tests.TestSupport.Services.DTOs
{
    public class ConstraintDTO
    {
        public string DatabaseName { get; }
        public string SchemaName { get; }
        public string TableName { get; }
        public string ColumnName { get; }
        public string ConstraintName { get; }
        public string ConstraintType { get; }

        public ConstraintDTO(string databaseName, string schemaName, string tableName, string columnName, string constraintName, string constraintType)
        {
            DatabaseName = databaseName;
            SchemaName = schemaName;
            TableName = tableName;
            ColumnName = columnName;
            ConstraintName = constraintName;
            ConstraintType = constraintType;
        }
    }
}
