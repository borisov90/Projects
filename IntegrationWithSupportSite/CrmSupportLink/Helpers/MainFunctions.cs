using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace CRMServices.Helpers
{
    public class MainFunctions
    {
        public static string GetValueFromWebConfig(string key)
        {
            return WebConfigurationManager.AppSettings.Get(key);
        }
    }
}
