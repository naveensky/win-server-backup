using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.FtpClient;
using System.Text;

namespace Backup.Ftp {

    /// <summary>
    /// The class manages all the ftp operations.
    /// </summary>
    public class FtpManager {
        public NetworkCredential Credential { get; set; }
        public string Hostname { get; set; }
        public int Port { get; set; }

        public FtpManager() {
            //This is default port for ftp.
            Port = 21;
        }

        public bool TransferFile(IEnumerable<string> files, string ftpDirectoryPath) {
            CreateFtpDirectory(ftpDirectoryPath);
            using (var ftpClient = CreateClient()) {
                foreach (var file in files) {
                    if (file == null)
                        continue;
                    var remotePath = string.Format(@"{0}\{1}", ftpDirectoryPath, file.Split(new[] { '\\' }).Last());
                    ftpClient.Upload(file, remotePath, FtpDataType.Binary);
                }
            }
            return true;
        }

        private void CreateFtpDirectory(string ftpDirectoryPath) {
            var directoryPath = ftpDirectoryPath.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

            using (var ftpclient = CreateClient()) {
                foreach (var path in directoryPath) {
                    if (!ftpclient.DirectoryExists(path))
                        ftpclient.CreateDirectory(path);
                    ftpclient.SetWorkingDirectory(path);
                }
            }
        }

        private FtpClient CreateClient() {
            return new FtpClient() {
                Username = Credential.UserName,
                Password = Credential.Password,
                Server = Hostname,
                Port = Port,
                SslMode = FtpSslMode.None,
                //DataChannelEncryption = true,
                //DataChannelType = FtpDataChannelType.Active
            };
        }

        public IEnumerable<string> GetListing(string ftpPath) {
            using (var ftpClient = CreateClient()) {
                if (string.IsNullOrEmpty(ftpPath) || ftpPath.Equals(".") || ftpPath.Equals("/"))
                    return ftpClient.GetListing(ftpPath).Select(x => x.Name);

                if (ftpClient.DirectoryExists(ftpPath))
                    return ftpClient.GetListing(ftpPath).Select(x => x.Name);
            }
            return new List<string>();
        }

        public bool DeleteFtpDirectory(string ftpPath) {
            using (var ftpClient = CreateClient()) {
                if (ftpClient.DirectoryExists(ftpPath)) {
                    var directoryContents = ftpClient.GetListing(ftpPath);
                    foreach (var file in directoryContents.Where(x => x.Type == FtpObjectType.File)) {
                        ftpClient.RemoveFile(string.Format("{0}/{1}", ftpPath, file.Name));
                    }

                    foreach (var file in directoryContents.Where(x => x.Type == FtpObjectType.Directory)) {
                        DeleteFtpDirectory(string.Format("{0}/{1}", ftpPath, file.Name));
                    }

                    ftpClient.RemoveDirectory(ftpPath);
                } else if (ftpClient.FileExists(ftpPath)) {
                    ftpClient.RemoveFile(ftpPath);
                }
            }

            return true;
        }
    }
}
