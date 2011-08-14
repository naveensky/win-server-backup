using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backup.Util {
    public static class AppExtensions {
        public static string ToAppDateToString(this DateTime date) {
            return date.ToString("dd-MM-yyyy");
        }
    }
}
