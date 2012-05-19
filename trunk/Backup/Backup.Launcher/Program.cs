using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Backup.BO;

namespace Backup.Launcher {

    class Program {
        static void Main(string[] args) {
            var xmlReader = new ConfigReader();
            var executingDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            var dic = xmlReader.ReadXml(executingDirectory + "\\XMLFile.xml");
            foreach (var application in dic) {
                var runner = Runner.Instance;
                runner.Settings = application;
                runner.Run();
            }
        }
    }
}
