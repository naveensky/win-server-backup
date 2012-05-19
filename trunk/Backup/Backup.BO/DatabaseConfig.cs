namespace Backup.BO {

    /// <summary>
    /// This will store database configuration for db to backup and upload via ftp
    /// </summary>
    public class DatabaseConfig {
        public string Password { get; set; }
        public string Username { get; set; }
        public string Server { get; set; }
        public string DatabaseName { get; set; }
    }
}
