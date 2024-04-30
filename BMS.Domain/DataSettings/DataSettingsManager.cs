using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BMS.Shared.Common;

namespace BMS.Domain.DataSettings
{
    public static class DataSettingsManager
    {
        private static DBSettings _dataSettings;

        private static bool? _databaseIsInstalled;

        /// <summary>
        /// Load settings
        /// </summary>
        public static (DBSettings, bool) LoadSettings(bool reloadSettings = false)
        {
            if (!reloadSettings && _dataSettings != null)
                return (_dataSettings, false);

            if (!File.Exists(CommonPath.SettingsPath))
            {
                DBSettings settings = new DBSettings();
                settings.connectionString = DefaultSettings.connectionString;
                settings.dbName = DefaultSettings.dbName;

                return (settings, true);
            }

            try
            {
                var text = File.ReadAllText(CommonPath.SettingsPath);
                _dataSettings = JsonSerializer.Deserialize<DBSettings>(text);
            }
            catch
            {
                //Try to read file
                var connectionString = File.ReadLines(CommonPath.SettingsPath).FirstOrDefault();
                _dataSettings = new DBSettings() { connectionString = connectionString };

            }
            return (_dataSettings, false);
        }
        public static async Task SaveSettings(DBSettings settings)
        {
            var filePath = CommonPath.SettingsPath;
            if (!File.Exists(filePath))
            {
                using FileStream fs = File.Create(filePath);
            }
            var data = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, data);
        }


        /// <summary>
        /// Returns a value indicating whether database is already installed
        /// </summary>
        /// <returns></returns>
        public static bool DatabaseIsInstalled()
        {
            if (!_databaseIsInstalled.HasValue)
            {
                bool defaultsetting = false;
                var settings = _dataSettings;

                if (_dataSettings == null)
                    (settings, defaultsetting) = LoadSettings();

                _databaseIsInstalled = (settings != null && !string.IsNullOrEmpty(settings.connectionString) && !defaultsetting);
            }
            return _databaseIsInstalled.Value;
        }

        public static void ResetCache()
        {
            _databaseIsInstalled = false;
        }

    }
}
