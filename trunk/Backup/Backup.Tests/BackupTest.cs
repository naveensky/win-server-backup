﻿using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Backup.FS;
using Backup.Ftp;
using Backup.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Backup.Tests {

    [TestClass]
    public class BackupTest {

        [TestMethod]
        public void Can_Create_MsSql_Backup() {
            var backupCreator = new MsSqlBackupCreator {
                Credentials = new NetworkCredential("sa", "asdf"),
                DatabaseName = "decristal",
                FilePath = @"E:\projects\De Cristal\",
                HostName = @"laptop-001\sqlexpress"
            };
            backupCreator.CreateBackup();
            Assert.AreEqual(true, File.Exists(string.Format(@"E:\projects\De Cristal\decristal.{0}.bak", DateTime.Now.ToString("yyyyMMdd"))));

        }

        [TestMethod]
        public void Can_Create_FileSystem_Backup() {
            IFsBackupCreator creator = new FsBackupCreator();
            creator.CreateBackup(@"E:\projects\De Cristal", @"E:\projects\De Cristal");
            Assert.IsTrue(
                File.Exists(string.Format(@"E:\projects\De Cristal\De Cristal.{0}.zip",
                                          DateTime.Now.ToString("yyyyMMdd"))));
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
    }
}
