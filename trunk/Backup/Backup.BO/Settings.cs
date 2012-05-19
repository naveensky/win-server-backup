using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Backup.BO {
    public class Settings {

        public Settings() {
            //by default temp directory will be on c:
            TempDirectory = @"c:\temp\";
        }

        private string _applicationName;
        private const string DefaultAppName = "Others";

        public IEnumerable<DatabaseConfig> Databases { get; set; }
        public IEnumerable<string> Directories { get; set; }

        public string ApplicationName {
            get {
                return string.IsNullOrEmpty(_applicationName) ? DefaultAppName : _applicationName;
            }
            set { _applicationName = value; }
        }

        public NetworkCredential FtpCredentials { get; set; }
        public string FtpRoot { get; set; }

        public string FtpHostname { get; set; }
        public string TempDirectory { get; set; }

        /// <summary>
        /// No of days for which a backup should be kept on server, beyond which backups will be deleted.
        /// If this is set to zero (0), then backups are never deleted.
        /// </summary>
        public int BackupRetentionDays { get; set; }

        public static Settings Create(string applicationName, IEnumerable<DatabaseConfig> databases, IEnumerable<string> directories,
            NetworkCredential credential, string ftpRoot, string ftpHostname, int backupRetentionDays = 0) {

            return new Settings {
                ApplicationName = applicationName,
                Databases = databases,
                Directories = directories,
                FtpCredentials = credential,
                FtpHostname = ftpHostname,
                FtpRoot = ftpRoot,
                BackupRetentionDays = backupRetentionDays
            };
        }
    }
}
