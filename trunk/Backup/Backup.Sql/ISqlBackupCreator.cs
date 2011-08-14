using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backup.Sql {
    public interface ISqlBackupCreator {

        /// <summary>
        /// Creates backup for the database
        /// </summary>
        /// <returns>Returns file complete path where database backup was created</returns>
        string CreateBackup();
    }
}
