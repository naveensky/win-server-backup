using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading;
using Backup.FS;
using Backup.Ftp;
using Backup.Sql;
using Backup.Util;

namespace Backup.BO {
    public class Runner {
        private static readonly Runner _instance = new Runner();

        private Runner() { }

        public static Runner Instance {
            get { return _instance; }
        }

        public bool IsRunning { get; private set; }
        public Settings Settings { get; set; }

        public void Run() {
            //TODO : make mechanism to set state of IsRunning
            var backupExecuter = new RunnerThread(Settings);
            backupExecuter.Run();
            //var runnerThread = new Thread(new ThreadStart(backupExecuter.Run));
            //runnerThread.Start();
            //runnerThread.Join();
        }

    }

    internal class RunnerThread {
        private readonly Settings _settings;
        private DateTime _backupTime;

        internal RunnerThread(Settings settings) {
            _settings = settings;
            _backupTime = DateTime.Now;
        }

        public void Run() {
            var fsDataPath = string.Format(@"{0}\{1}\fs", _settings.TempDirectory, _backupTime.ToAppDateToString());
            var sqlDataPath = string.Format(@"{0}\{1}\sql", _settings.TempDirectory, _backupTime.ToAppDateToString());

            if (!Directory.Exists(fsDataPath))
                Directory.CreateDirectory(fsDataPath);

            if (!Directory.Exists(sqlDataPath))
                Directory.CreateDirectory(sqlDataPath);

            var fsBackupFiles = new List<string>();
            var sqlBackupFiles = new List<string>();


            //create zip files for directories first
            var fsCreator = new FsBackupCreator(_backupTime);
            foreach (var directory in _settings.Directories) {
                fsBackupFiles.Add(fsCreator.CreateBackup(directory, fsDataPath));
            }

            //create database backups
            foreach (var database in _settings.Databases) {
                var creator = new MsSqlBackupCreator(_backupTime) {
                    DatabaseName = database,
                    Credentials =
                        new NetworkCredential(_settings.DatabaseUsername, _settings.DatabasePassword),
                    HostName = _settings.DatabaseServer,
                    FilePath = sqlDataPath
                };
                sqlBackupFiles.Add(creator.CreateBackup());
            }

            //start ftp transaction
            var ftpManager = new FtpManager { Credential = _settings.FtpCredentials, Hostname = _settings.FtpHostname };

            var ftpFsRootPath = string.Format(@"{0}\{1}\{2}", _settings.FtpRoot, _backupTime.ToAppDateToString(), "FS");
            var ftpSqlRootPath = string.Format(@"{0}\{1}\{2}", _settings.FtpRoot, _backupTime.ToAppDateToString(), "SQL");

            ftpManager.TransferFile(fsBackupFiles, ftpFsRootPath);
            ftpManager.TransferFile(sqlBackupFiles, ftpSqlRootPath);
        }


        private string GetDirectory() {
            return Path.GetTempPath();
        }
    }
}
