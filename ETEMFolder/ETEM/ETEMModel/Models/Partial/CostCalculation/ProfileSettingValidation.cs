using ETEMModel.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models
{
    public partial class ProfileSettingValidation : Identifiable
    {
        #region Identifiable Members

        public int EntityID
        {
            get
            {
                return this.idProfileSettingValidation;
            }
        }

        public string ValidationErrorsAsString { get; set; }

        public List<string> ValidateEntity(CallContext outputContext)
        {
            List<string> result = new List<string>();

            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;

            ValidationErrorsAsString = string.Empty;

            return result;
        }

        #endregion
    }
}