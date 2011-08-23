using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;

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

        public static bool CreateArchive(string filePath, string zipFilePath) {
            if (!File.Exists(filePath))
                return false;
            using (var zipFile = new ZipFile()) {
                zipFile.AddFile(filePath);
                zipFile.Save(zipFilePath);
            }
            return true;
        }

    }
}
