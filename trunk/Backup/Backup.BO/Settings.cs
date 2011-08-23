using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Backup.BO {
    public class Settings {
        public IEnumerable<string> Databases { get; set; }
        public IEnumerable<string> Directories { get; set; }

        public NetworkCredential FtpCredentials { get; set; }
        public string FtpRoot { get; set; }

        public string FtpHostname { get; set; }
        public string DatabaseServer { get; set; }
        public string DatabaseUsername { get; set; }
        public string DatabasePassword { get; set; }
        public string TempDirectory { get; set; }

        public static Settings Create(IEnumerable<string> databases, IEnumerable<string> directories,
            NetworkCredential credential, string ftpRoot, string ftpHostname, string databaseUsername, string databaseServer, string databasePassword) {

            return new Settings {
                DatabasePassword = databasePassword,
                Databases = databases,
                DatabaseServer = databaseServer,
                DatabaseUsername = databaseUsername,
                Directories = directories,
                FtpCredentials = credential,
                FtpHostname = ftpHostname,
                FtpRoot = ftpRoot
            };
        }
    }
}
