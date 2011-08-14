using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backup.UI.ViewModel {
    public class SettingViewModel : BaseViewModelBase {

        private IList<string> _fileLocations;
        private IList<string> _databaseNames;

        //File locations
        public string FileLocations {
            get { return string.Join(Environment.NewLine, _fileLocations); }
            set { _fileLocations = value.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries); }
        }

        //database names
        public string DatabaseNames {
            get { return string.Join(Environment.NewLine, _databaseNames); }
            set { _databaseNames = value.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries); }
        }

        //ftp credentials
        public string FtpHost { get; set; }
        public string FtpUsername { get; set; }
        public string FtpPassword { get; set; }
        public string FtpDirectory { get; set; }


    }
}
