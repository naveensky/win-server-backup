using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backup.BO {
    public class Scheduler {
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }

        public bool IsRunRequired(DateTime lastRuntime, DateTime nextRuntime) {
            if (nextRuntime <= lastRuntime)
                return false;

            if (nextRuntime.Subtract(lastRuntime).Hours > 24)
                return true;

            throw new NotImplementedException();
        }
    }
}
