using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server.Tests.TestSupport.Services.DTOs
{
    public class ColumnDTO
    {
        public string Database { get; }
        public string Schema { get; }
        public string TableName { get; }
        public string ColumnName { get; }
        public int Order { get; }
        public string DefaultValue { get; }
        public bool IsNullable { get; }
        public string DataType { get; }
        public int MaxLength { get; }
        public int NumericPrecision { get; }
        public int NumericRadix { get; }
        public int NumericScale { get; }
        public int DateTimePrecision { get; }
        public string Collation { get; }

        public ColumnDTO(string database, string schema, string tableName, string columnName, int order, string defaultValue, bool isNullable, string dataType, int maxLength, int numericPrecision, int numericRadix, int numericScale, int dateTimePrecision, string collation)
        {
            Database = database;
            Schema = schema;
            TableName = tableName;
            ColumnName = columnName;
            Order = order;
            DefaultValue = defaultValue;
            IsNullable = isNullable;
            DataType = dataType;
            MaxLength = maxLength;
            NumericPrecision = numericPrecision;
            NumericRadix = numericRadix;
            NumericScale = numericScale;
            DateTimePrecision = dateTimePrecision;
            Collation = collation;
        }

        public bool IsDifferentFrom(ColumnDTO column)
        {
            return DefaultValue != column.DefaultValue || IsNullable != column.IsNullable || DataType != column.DataType || MaxLength != column.MaxLength
                   || NumericPrecision != column.NumericPrecision || NumericRadix != column.NumericRadix || NumericScale != column.NumericScale || DateTimePrecision != column.DateTimePrecision
                   || Collation != column.Collation;
        }
    }
}