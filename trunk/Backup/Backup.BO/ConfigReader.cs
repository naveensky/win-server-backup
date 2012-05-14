using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using p=Backup.BO;

namespace Backup.BO {
    public class ConfigReader {
        public IDictionary<string,ApplicationConfig> ReadXml(string path) {
            var applications = XDocument.Load(path).Descendants("application");
            var dictionary = new Dictionary<string, ApplicationConfig>();
            foreach (var application in applications) {
                var dbCredentials = new List<DbConfig>();
                var ftpCredentials = new List<FtpCredential>();
                var files = new List<string>();
                var datas = application.Descendants("db");
                var ftps = application.Descendants("ftp");
                var fs = application.Elements("fs");
                var defautlFtpCredentials= new List<FtpCredential>();
                var defaultDbCredentials = ReadDefaultXmlForDb(path);
                if (datas.Any()) {
                    foreach (var data in datas) {
                        var password = data.Element("db_password") == null ? defaultDbCredentials.ElementAt(0).Password : data.Element("db_password").Value;
                        var server = data.Element("db_server") == null ? defaultDbCredentials.ElementAt(0).Server : data.Element("db_server").Value;
                        var user = data.Element("db_user") == null ? defaultDbCredentials.ElementAt(0).UserName : data.Element("db_user").Value;
                        var dbName = data.Element("db_name").Value;
                        var db = new DbConfig {DbName = dbName, Server = server, Password = password, UserName = user};
                        dbCredentials.Add(db);
                    }
                }
                defautlFtpCredentials = ReadDefaultXmlForFtp(path);
                if (!ftps.Any()) {
                    ftpCredentials = ReadDefaultXmlForFtp(path);
                   
                } else {
                    foreach (var ftp in ftps) {
                        var password = ftp.Element("ftp_password")==null?defautlFtpCredentials.ElementAt(0).PassWord:ftp.Element("ftp_password").Value;
                        var username = ftp.Element("ftp_user") == null ? defautlFtpCredentials.ElementAt(0).UserName : ftp.Element("ftp_user").Value; ;
                        var host = ftp.Element("ftp_host") == null ? defautlFtpCredentials.ElementAt(0).Host : ftp.Element("ftp_host").Value; ;
                        var port = ftp.Element("ftp_directory")== null ? defautlFtpCredentials.ElementAt(0).Directory : ftp.Element("ftp_directory").Value; ;
                        var ftpCredential = new FtpCredential
                                            {UserName = username, PassWord = password, Host = host, Directory = port};
                        ftpCredentials.Add(ftpCredential);
                    }
                }
                foreach (var xElement in fs) {
                    var value = xElement.Value;
                    files.Add(value);
                }
               
                dictionary.Add(application.Attribute("name").Value,
                                   new ApplicationConfig {DbConfigs = dbCredentials, FtpCredentials = ftpCredentials,FileDirectories = files});
                
            }
           return dictionary;
        }
        public List<DbConfig> ReadDefaultXmlForDb(string name) {
            var applications = XDocument.Load(name).Descendants("default");
            var dbCredntialses = new List<DbConfig>();
            foreach (var application in applications) {
               
               
                var datas = application.Descendants("db");
                
                foreach (var data in datas) {
                    var password = data.Element("db_password").Value;
                    var server = data.Element("db_server").Value;
                    var user = data.Element("db_user").Value;
                    var db = new DbConfig {  Server = server, Password = password, UserName = user };
                    dbCredntialses.Add(db);
                }
             }
            return dbCredntialses;  
        }

        public List<FtpCredential> ReadDefaultXmlForFtp(string name) {
            var applications = XDocument.Load(name).Descendants("default");
            var ftpCredentials = new List<FtpCredential>();
            foreach (var application in applications) {


                var datas = application.Descendants("ftp");

                foreach (var data in datas) {
                    var password = data.Element("ftp_password").Value;
                    var host = data.Element("ftp_host").Value;
                    var user = data.Element("ftp_user").Value;
                    var port = data.Element("ftp_directory").Value;
                    var db = new FtpCredential {  PassWord = password, UserName = user ,Host = host,Directory = port};
                    ftpCredentials.Add(db);
                }
            }
            return ftpCredentials;
        } 
    }
}