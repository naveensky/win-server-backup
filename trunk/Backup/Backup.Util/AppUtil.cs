using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backup.Util {
    public static class AppUtil {

        /// <summary>
        /// Get archive name for a file
        /// </summary>
        /// <param name="filePath">Path of the file</param>
        /// <returns>7z archive name for file</returns>
        public static string GetArchiveName(string filePath) {
            return string.Format("{0}.{1}.zip", filePath, DateTime.Now.ToString("yyyyMMdd"));
        }

    }
}
