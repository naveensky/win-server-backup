using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using p = Backup.BO;

namespace Backup.BO {
    public class ConfigReader {
        public IEnumerable<Settings> ReadXml(string path) {
            var applications = XDocument.Load(path).Descendants("application");
            //var dictionary = new Dictionary<string, ApplicationConfig>();
            foreach (var application in applications) {

                var ftpCredentials = new List<FtpCredential>();

                var databases = application.Descendants("db");
                var ftps = application.Descendants("ftp");
                var fs = application.Elements("fs");
                var defaultFtpCredentials = ReadXmlForDefaultFtpCredentials(path);
                var defaultDbCredentials = ReadXmlForDefaultDbCredentials(path);
                var retentionDayCount = 0;
                var applicationName = application.Attribute("name") == null
                                          ? string.Empty
                                          : application.Attribute("name").Value;

                if (application.Element("retention_days") != null) {
                    int.TryParse(application.Element("retention_days").Value, out retentionDayCount);
                }

                var dbCredentials = from data in databases
                                    let password =
                                        data.Element("db_password") == null
                                            ? defaultDbCredentials.Password
                                            : data.Element("db_password").Value
                                    let server =
                                        data.Element("db_server") == null
                                            ? defaultDbCredentials.Server
                                            : data.Element("db_server").Value
                                    let user =
                                        data.Element("db_user") == null
                                            ? defaultDbCredentials.Username
                                            : data.Element("db_user").Value
                                    let dbName = data.Element("db_name").Value
                                    select
                                        new DatabaseConfig { DatabaseName = dbName, Server = server, Password = password, Username = user };

                foreach (var ftp in ftps) {
                    var password = ftp.Element("ftp_password") == null ? defaultFtpCredentials.Password : ftp.Element("ftp_password").Value;
                    var username = ftp.Element("ftp_user") == null ? defaultFtpCredentials.Username : ftp.Element("ftp_user").Value; ;
                    var host = ftp.Element("ftp_host") == null ? defaultFtpCredentials.Host : ftp.Element("ftp_host").Value; ;
                    var directory = ftp.Element("ftp_directory") == null ? defaultFtpCredentials.Directory : ftp.Element("ftp_directory").Value; ;
                    var ftpCredential = new FtpCredential { Username = username, Password = password, Host = host, Directory = directory };
                    ftpCredentials.Add(ftpCredential);
                }

                //if no credential is present, then add default credential
                if (!ftpCredentials.Any())
                    ftpCredentials.Add(defaultFtpCredentials);

                yield return Settings.Create(
                    applicationName,
                    dbCredentials,
                    fs.Select(xElement => xElement.Value).ToList(),
                    new NetworkCredential(ftpCredentials.First().Username, ftpCredentials.First().Password),
                    ftpCredentials.First().Directory,
                    ftpCredentials.First().Host,
                    retentionDayCount);

            }
        }

        public DatabaseConfig ReadXmlForDefaultDbCredentials(string path) {
            var defaultConfigXml = XDocument.Load(path).Descendants("default").FirstOrDefault();

            if (defaultConfigXml == null)
                return null;

            var defaultDbXml = defaultConfigXml.Descendants("db").FirstOrDefault();
            if (defaultDbXml == null)
                return null;

            var password = defaultDbXml.Element("db_password").Value;
            var server = defaultDbXml.Element("db_server").Value;
            var user = defaultDbXml.Element("db_user").Value;
            var dbCredentials = new DatabaseConfig { Server = server, Password = password, Username = user };

            return dbCredentials;
        }

        public FtpCredential ReadXmlForDefaultFtpCredentials(string path) {
            var defaultXmlConfig = XDocument.Load(path).Descendants("default").FirstOrDefault();

            if (defaultXmlConfig == null)
                return null;

            var defaultFtpConfigXml = defaultXmlConfig.Descendants("ftp").FirstOrDefault();

            if (defaultFtpConfigXml == null)
                return null;

            var password = defaultFtpConfigXml.Element("ftp_password").Value;
            var host = defaultFtpConfigXml.Element("ftp_host").Value;
            var user = defaultFtpConfigXml.Element("ftp_user").Value;
            var port = defaultFtpConfigXml.Element("ftp_directory").Value;
            var ftpCredentials = new FtpCredential { Password = password, Username = user, Host = host, Directory = port };

            return ftpCredentials;
        }
    }
}