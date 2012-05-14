using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using Backup.Util;

namespace Backup.Sql {
    public class MsSqlBackupCreator : ISqlBackupCreator {
        private const string DbBackupScript = @"BACKUP DATABASE [{0}] TO DISK = '{1}'";
        private const string _connString = @"Data Source={0};Initial Catalog={1};User ID={2};Password={3}";
        private SqlConnection _connection;
        private string _fileCompletePath;
        private DateTime _runtime;

        public MsSqlBackupCreator(DateTime runtime) {
            this._runtime = runtime;
        }

        public NetworkCredential Credentials { get; set; }
        public string HostName { get; set; }
        public string DatabaseName { get; set; }
        
        /// <summary>
        /// Directory where backup shall be created
        /// </summary>
        public string FilePath { get; set; }
        public string ConnectionString {
            get { return string.Format(_connString, HostName, DatabaseName, Credentials.UserName, Credentials.Password); }
        }

        public string CreateBackup() {
            var command = GetBackupCommand();
            command.ExecuteNonQuery();
            var archivePath = string.Format("{0}.zip", _fileCompletePath);
            AppUtil.CreateArchive(_fileCompletePath, archivePath);
            return archivePath;
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
            _fileCompletePath = string.Format(@"{0}\{1}.{2}.bak", FilePath, DatabaseName,
                                              _runtime.ToAppDateToString());
            return string.Format(DbBackupScript, DatabaseName, _fileCompletePath);
        }
    }
}