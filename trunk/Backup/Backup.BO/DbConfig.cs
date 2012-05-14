namespace Backup.BO {

    /// <summary>
    /// This will store database configuration for db to backup and upload via ftp
    /// </summary>
    public class DbConfig {
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Server { get; set; }
        public string DbName { get; set; }
    }
}
