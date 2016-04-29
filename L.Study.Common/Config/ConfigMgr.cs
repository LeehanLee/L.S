using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.Study.Common.Config
{
    using System.Configuration;
    public class ConfigMgr
    {
        
        public static bool GetAppSettingBool(string key) 
        {
            var value = ConfigurationManager.AppSettings[key];
            bool result = false;
            if (Boolean.TryParse(value, out result)) { return result; }
            else { return result; }
        }
        public static int GetAppSettingInt(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            int result = 0;
            if (Int32.TryParse(value, out result)) { return result; }
            else { return result; }
        }
        public static string GetAppSettingString(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            return value.ToString();
        }
    }
}
