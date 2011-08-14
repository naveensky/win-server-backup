﻿using System;
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
                DatabaseName = "decristal",
                FilePath = @"E:\projects\De Cristal\",
                HostName = @"laptop-001\sqlexpress"
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
            manager.TransferFile(files, "");
        }

        [TestMethod]
        public void Scheduler_Test() {
            var scheduler = new Scheduler();
            scheduler.Hour = 12;
            scheduler.Minute = 0;
            scheduler.Second = 0;
            var currentTime = DateTime.Now;
            Assert.IsFalse(scheduler.IsRunRequired(currentTime.AddHours(-24), currentTime));
        }

        [TestMethod]
        public void Can_Schedule_Backup_Files() {
            var runner = Runner.Instance;
            runner.Settings = new Settings {
                DatabasePassword = "asdf",
                Databases = new List<string> { "DeCristal" },
                DatabaseServer = @"laptop-001\sqlexpress",
                DatabaseUsername = "sa",
                Directories = new List<string> { @"E:\projects\De Cristal\portal\deploy" },
                FtpCredentials = new NetworkCredential("softwareftp", "Asdf1234"),
                FtpHostname = "118.139.186.1",
                FtpRoot = ""
            };

            runner.Run();

        }


    }
}
