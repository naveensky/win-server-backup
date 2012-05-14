using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Backup.BO;

namespace Backup.Launcher {

    class Program {
        static void Main(string[] args) {
            var xmlReader = new ConfigReader();
            var executingDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            var dic = xmlReader.ReadXml(executingDirectory + "\\XMLFile.xml");
            foreach (var application in dic) {
                var dbpassword = application.Value.DbConfigs.ElementAt(0).Password;
                var dbServer = application.Value.DbConfigs.ElementAt(0).Server;
                var dbNames = application.Value.DbConfigs.Select(x => x.DbName).ToList();
                var dbUserName = application.Value.DbConfigs.ElementAt(0).UserName;
                var ftpusername = application.Value.FtpCredentials.ElementAt(0).UserName;
                var ftpPassword = application.Value.FtpCredentials.ElementAt(0).PassWord;
                var ftpHost = application.Value.FtpCredentials.ElementAt(0).Host;
                var ftpDirectories = application.Value.FileDirectories;
                var ftproot = application.Value.FtpCredentials.ElementAt(0).Directory;
                var runner = Runner.Instance;
                runner.Settings = new Settings {
                    DatabasePassword = dbpassword,
                    Databases = dbNames,
                    DatabaseServer = dbServer,
                    DatabaseUsername = dbUserName,
                    Directories = ftpDirectories,
                    FtpCredentials = new NetworkCredential(ftpusername, ftpPassword),
                    FtpHostname = ftpHost,
                    FtpRoot = "",
                    TempDirectory = @"c:\temp"
                };

                runner.Run();

            }


        }
    }
}
