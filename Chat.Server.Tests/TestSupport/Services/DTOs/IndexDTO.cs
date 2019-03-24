using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server.Tests.TestSupport.Services.DTOs
{
    public class IndexDTO
    {
        public string SchemaName { get; }
        public string TableName { get; }
        public string ColumnName { get; }
        public string IndexName { get; }
        public string IndexType { get; }
        public bool IsUnique { get; }
        public bool IsDescending { get; }
        public bool IsIncluded { get; }
        public bool IsFiltered { get; }
        public string FilterDefinition { get; }

        public IndexDTO(string schemaName, string tableName, string columnName, string indexName, string indexType, bool isUnique, bool isDescending, bool isIncluded, bool isFiltered, string filterDefinition)
        {
            SchemaName = schemaName;
            TableName = tableName;
            ColumnName = columnName;
            IndexName = indexName;
            IndexType = indexType;
            IsUnique = isUnique;
            IsDescending = isDescending;
            IsIncluded = isIncluded;
            IsFiltered = isFiltered;
            FilterDefinition = filterDefinition;
        }
    }
}
