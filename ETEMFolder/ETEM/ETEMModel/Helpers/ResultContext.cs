using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers
{
    public class CallContext
    {
        public ETEMEnums.ResultEnum ResultCode { get; set; }
        public string Result { get; set; }
        public string Message { get; set; }
        public string EntityID { get; set; }
        public string CustomID { get; set; }
        public string CurrentConsumerID { get; set; }
        public string CurrentPersonID { get; set; }
        public string CurrentConsumerLang { get; set; }
        public string CurrentConsumerNames { get; set; }
        public string CurrentConsumerSessionId { get; set; }
        public string ActionName { get; set; }
        public int CurrentYear { get; set; }
        public int CurrentPeriod { get; set; }
        public string Page { get; set; }
        public string SecuritySetting { get; set; }
        public string SecuritySettingBG { get; set; }

        public ETEMEnums.PersonTypeEnum PersonType { get; set; }
        public List<KeyValuePair<string, int>> listKvEntityID { get; set; }
        public List<KeyValuePair<string, object>> ListKvParams { get; set; }
        public ETEMEnums.SecuritySettings securitySettings { get; set; }

        public CallContext()
        {
            this.Result = string.Empty;
            this.Message = string.Empty;
            this.EntityID = string.Empty;
            this.CustomID = string.Empty;
            this.PersonType = ETEMEnums.PersonTypeEnum.Aall;
            this.listKvEntityID = new List<KeyValuePair<string, int>>();
            this.ListKvParams = new List<KeyValuePair<string, object>>();
        }
    }
}