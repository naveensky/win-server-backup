using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Backup.Util;
using Ionic.Zip;

namespace Backup.FS {
    public class FsBackupCreator : IFsBackupCreator {
        private DateTime _runtime;

        public FsBackupCreator(DateTime runTime) {
            _runtime = runTime;
        }

        public string CreateBackup(string sourceDirectoryPath, string destinationDirectory) {
            //return if no directory exists
            if (!Directory.Exists(sourceDirectoryPath))
                return null;

            var archiveFilename = AppUtil.GetArchiveName(GetArchivePath(sourceDirectoryPath, destinationDirectory));

            using (var zipFile = new ZipFile()) {
                zipFile.AddDirectory(sourceDirectoryPath);
                zipFile.Save(archiveFilename);
            }

            return archiveFilename;
        }

        private string GetArchivePath(string sourceDirectoryPath, string destinationDirectory) {
            var directoryName = sourceDirectoryPath.Split('\\').Last();
            return string.Format(@"{0}\{1}", destinationDirectory, directoryName);
        }
    }
}
