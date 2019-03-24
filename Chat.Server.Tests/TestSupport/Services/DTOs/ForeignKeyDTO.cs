using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server.Tests.TestSupport.Services.DTOs
{
    public class ForeignKeyDTO
    {
        public string SchemaName { get; }
        public string TableName { get; }
        public string ColumnName { get; }
        public string ForeignKeyName { get; }
        public string ReferenceSchemaName { get; }
        public string ReferenceTableName { get; }
        public string ReferenceColumnName { get; }

        public ForeignKeyDTO(string schemaName, string tableName, string columnName, string foreignKeyName, string referenceSchemaName, string referenceTableName, string referenceColumnName)
        {
            SchemaName = schemaName;
            TableName = tableName;
            ColumnName = columnName;
            ForeignKeyName = foreignKeyName;
            ReferenceSchemaName = referenceSchemaName;
            ReferenceTableName = referenceTableName;
            ReferenceColumnName = referenceColumnName;
        }
    }
}
