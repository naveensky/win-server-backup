using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backup.FS {
    public interface IFsBackupCreator {
        string CreateBackup(string sourceDirectoryPath, string destinationDirectory);
    }
}
