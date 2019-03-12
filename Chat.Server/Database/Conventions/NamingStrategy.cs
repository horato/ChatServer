using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Cfg;

namespace Chat.Server.Database.Conventions
{
    public class NamingStrategy : INamingStrategy
    {
        public string ClassToTableName(string className)
        {
            return className;
        }

        public string PropertyToColumnName(string propertyName)
        {
            return propertyName;
        }

        public string TableName(string tableName)
        {
            return tableName;
        }

        public string ColumnName(string columnName)
        {
            return columnName.Replace("_id", "Id");
        }

        public string PropertyToTableName(string className, string propertyName)
        {
            throw new InvalidOperationException("Not supported");
        }

        public string LogicalColumnName(string columnName, string propertyName)
        {
            throw new InvalidOperationException("Not supported");
        }
    }
}
