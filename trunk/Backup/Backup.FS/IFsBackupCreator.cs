using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backup.FS {
    public interface IFsBackupCreator {
        void CreateBackup(string sourceDirectoryPath, string destinationDirectory);
    }
}
