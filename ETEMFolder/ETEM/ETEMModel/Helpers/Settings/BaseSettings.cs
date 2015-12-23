using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers.Settings
{
    public class BaseSettings
    {
        public ETEMEnums.GroupSecuritySettings GroupSecuritySetting { get; set; }
        public ETEMEnums.SecuritySettings SecuritySetting { get; set; }
        public string FrendlyName { get; set; }
        public string Description { get; set; }
    }
}