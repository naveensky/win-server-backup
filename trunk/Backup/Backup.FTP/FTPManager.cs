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
    }
}
