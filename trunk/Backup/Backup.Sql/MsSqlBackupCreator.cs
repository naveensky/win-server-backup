using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;

namespace Backup.Sql {
    public class MsSqlBackupCreator : ISqlBackupCreator {
        private static string _dbBackupScript = @"BACKUP DATABASE {0} TO DISK = '{1}\{0}.{2}.bak'";
        private const string _connString = @"Data Source={0};Initial Catalog={1};User ID={2};Password={3}";
        private SqlConnection _connection;

        public NetworkCredential Credentials { get; set; }
        public string HostName { get; set; }
        public string DatabaseName { get; set; }
        public string FilePath { get; set; }
        public string ConnectionString {
            get { return string.Format(_connString, HostName, DatabaseName, Credentials.UserName, Credentials.Password); }
        }

        public void CreateBackup() {
            var command = GetBackupCommand();
            command.ExecuteNonQuery();
        }

        private SqlConnection GetConnection() {
            if (_connection == null)
                _connection = new SqlConnection(ConnectionString);

            if (_connection.State == ConnectionState.Closed)
                _connection.Open();

            return _connection;
        }

        private SqlCommand GetBackupCommand() {
            var command = new SqlCommand(GetBackupSqlScript(), GetConnection()) { CommandType = CommandType.Text };
            return command;
        }

        private string GetBackupSqlScript() {
            return string.Format(_dbBackupScript, DatabaseName, FilePath, DateTime.Now.ToString("yyyyMMdd"));
        }
    }
}
