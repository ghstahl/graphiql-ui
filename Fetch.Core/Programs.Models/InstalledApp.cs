using Microsoft.Win32;

namespace Programs.Models
{
    public class InstalledApp
    {
        public InstalledApp() { }
        public InstalledApp(RegistryKey uninstallKey, string keyName)
        {

            RegistryKey key = uninstallKey.OpenSubKey(keyName, false);

            try
            {
                var d = key.GetValue("DisplayName");
                if (d != null)
                {
                    DisplayName = d.ToString();
                }

                var s = key.GetValue("UninstallString");
                if (s != null)
                    UnInstallPath = s.ToString();

            }
            finally
            {
                key.Close();
            }


        }
        public string DisplayName { get; set; }
        public string UnInstallPath { get; set; }
    }
}
