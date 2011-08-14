using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backup.BO {
    public class Profile {
        public List<string> NotificationEmails { get; set; }
        public List<string> FolderList { get; set; }
        public List<string> Databases { get; set; }
    }
}
