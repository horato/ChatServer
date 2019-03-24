using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Chat.Server.Database;
using Chat.Server.Tests.TestSupport.Services;
using FluentNHibernate.Cfg.Db;

namespace Chat.Server.Tests.TestSupport
{
    public class ReferenceDatabase : IDisposable
    {
        private readonly string _server;
        private readonly string _database;
        private readonly string _login;
        private readonly string _password;
        private readonly string _connectionString;
        private readonly SqlConnection _connection;

        public ReferenceDatabase(string server, string database, string login, string password)
        {
            _server = server;
            _database = database;
            _login = login;
            _password = password;
            _connectionString = $"Data Source={server};Initial Catalog={database};Integrated Security=false;User ID={login};Password={password}";
            _connection = new SqlConnection(_connectionString);
            _connection.Open();

        }

        public void RemoveAllDatabaseContent()
        {
            var query = $@"
USE {_database}
GO

DECLARE @Sql NVARCHAR(500) DECLARE @Cursor CURSOR

SET @Cursor = CURSOR FAST_FORWARD FOR
SELECT DISTINCT sql = 'ALTER TABLE [' + tc2.TABLE_SCHEMA + '].[' +  tc2.TABLE_NAME + '] DROP [' + rc1.CONSTRAINT_NAME + '];'
FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc1
LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc2 ON tc2.CONSTRAINT_NAME =rc1.CONSTRAINT_NAME

OPEN @Cursor FETCH NEXT FROM @Cursor INTO @Sql

WHILE (@@FETCH_STATUS = 0)
BEGIN
Exec sp_executesql @Sql
FETCH NEXT FROM @Cursor INTO @Sql
END

CLOSE @Cursor DEALLOCATE @Cursor
GO

EXEC sp_MSforeachtable 'DROP TABLE ?'
GO
";

            ExecuteQueryWithGoKeywords(query);
        }

        public void CreateDatabase()
        {
            RunSqlScript("../../../SQL/CreateChangescriptsTable.sql");
        }

        public void ApplyChangescripts()
        {
            RunSqlScript("../../../SQL/Changescripts.sql");
        }

        private void RunSqlScript(string path)
        {
            var fullPath = Path.Combine(Environment.CurrentDirectory, path);
            if (!File.Exists(fullPath))
                throw new InvalidOperationException($"File {fullPath} does not exist.");

            var script = File.ReadAllText(fullPath);
            ExecuteQueryWithGoKeywords(script);
        }

        private object ExecuteQueryWithGoKeywords(string query)
        {
            var parts = Regex.Split(query, $"{Environment.NewLine}GO{Environment.NewLine}");
            foreach (var part in parts)
            {
                if (string.IsNullOrWhiteSpace(part))
                    continue;

                ËxecuteQuery(part);
            }

            return null;
        }

        private void ËxecuteQuery(string query)
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}