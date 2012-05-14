using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backup.BO {
    public class ApplicationConfig {

        public ApplicationConfig() {
            FtpCredentials = new List<FtpCredential>();
            DbConfigs = new List<DbConfig>();
            FileDirectories = new List<string>();
        }

        public IList<FtpCredential> FtpCredentials { get; set; }
        public IList<DbConfig> DbConfigs { get; set; }
        public IList<string> FileDirectories { get; set; }
    }
}
