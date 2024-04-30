using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Shared.Common
{
    public class CommonPath
    {
        private static string AppData => "App_Data";
        private static string LogDirectory => "Logs";

        public static string LogInfoFilePath
        {
            get
            {
                return Path.Combine(BaseDirectory, AppData, LogDirectory, "ErrorLogs.txt");
            }
        }

        public static string LogErrorFilePath
        {
            get
            {
                return Path.Combine(BaseDirectory, AppData, LogDirectory, "InfoLogs.txt");
            }
        }

        public static string SettingsPath
        {
            get
            {
                return Path.Combine(BaseDirectory, AppData, "settings.dat");
            }
        }
        public static string BaseDirectory { get; set; }
    }
}
