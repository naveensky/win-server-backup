using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Backup.BO;
using Backup.FS;
using Backup.Ftp;
using Backup.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Backup.Tests {

    [TestClass]
    public class BackupTest {

        [TestMethod]
        public void Can_Create_MsSql_Backup() {
            var backupCreator = new MsSqlBackupCreator(DateTime.Now) {
                Credentials = new NetworkCredential("sa", "asdf"),
                DatabaseName = "CCE.HolyInnocents",
                FilePath = @"D:\",
                HostName = @".\sqlexpress"
            };
            var file = backupCreator.CreateBackup();
            Assert.IsTrue(File.Exists(file));
        }

        [TestMethod]
        public void Can_Create_FileSystem_Backup() {
            IFsBackupCreator creator = new FsBackupCreator(DateTime.Now);
            var file = creator.CreateBackup(@"E:\projects\De Cristal", @"E:\projects\De Cristal");
            Assert.IsTrue(File.Exists(file));
        }

        [TestMethod]
        public void Can_FTP_Backup() {
            var manager = new FtpManager {
                Hostname = "118.139.186.1",
                Credential = new NetworkCredential("softwareftp", "Asdf1234")
            };
            IList<String> files = new List<string> { @"C:\Users\admin\Desktop\chat.xps" };
            Assert.IsTrue(manager.TransferFile(files, @"\2011.08.20\FS"));
        }

        [TestMethod]
        public void Can_FTP_Delete_Folder() {
            var manager = new FtpManager {
                Hostname = "118.139.186.1",
                Credential = new NetworkCredential("softwareftp", "Asdf1234")
            };

            Assert.IsTrue(manager.DeleteFtpDirectory(@"/2011.08.23"));
        }


        [TestMethod]
        public void Can_Schedule_Backup_Files() {
            var runner = Runner.Instance;
            runner.Settings = new Settings {
                Databases = new List<DatabaseConfig> { new DatabaseConfig { DatabaseName = "CCE.Login", Password = "asdf", Server = @".\sqlexpress", Username = "sa" } },
                Directories = new List<string> { @"E:\myneta\html" },
                FtpCredentials = new NetworkCredential("softwareftp", "Asdf1234"),
                FtpHostname = "118.139.186.1",
                FtpRoot = "",
                TempDirectory = @"c:\temp",
                ApplicationName = "tests",
                BackupRetentionDays = 3                        //how old a backup should be kept
            };

            runner.Run();

        }

        [TestMethod]
        public void Get_Root_Files() {
            var manager = new FtpManager {
                Hostname = "118.139.186.1",
                Credential = new NetworkCredential("softwareftp", "Asdf1234")
            };

            Assert.IsTrue(manager.GetListing("").Any());
        }


    }
}
