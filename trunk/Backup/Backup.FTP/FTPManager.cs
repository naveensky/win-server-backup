using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

        public void TransferFile(IEnumerable<string> files, string ftpDirectoryPath) {
            foreach (var file in files) {
                //if file doesnt exists, dont ftp anything
                if (!File.Exists(file))
                    continue;

                var fileName = new FileInfo(file).Name;
                var uri = new UriBuilder {
                    Host = string.Format("{0}", Hostname),
                    Path = string.Format("{0}/{1}", ftpDirectoryPath, fileName),
                    Port = Port,
                    Scheme = "ftp"
                };

                var request = (FtpWebRequest)WebRequest.Create(uri.Uri);

                request.Credentials = Credential;
                request.KeepAlive = true;
                request.UseBinary = true;

                request.KeepAlive = true;
                request.UseBinary = true;
                request.Method = WebRequestMethods.Ftp.UploadFile;
                var ftpstream = request.GetRequestStream();


                var fs = File.OpenRead(file);
                var buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                ftpstream.Write(buffer, 0, buffer.Length);

                ftpstream.Close();
            }
        }
    }
}
