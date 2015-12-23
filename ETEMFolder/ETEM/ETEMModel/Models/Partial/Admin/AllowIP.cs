using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Helpers;

namespace ETEMModel.Models
{
    public partial class AllowIP : Identifiable
    {
        public int EntityID
        {
            get { return this.idAllowIP; }
        }

        public List<string> ValidateEntity(CallContext outputContext)
        {
            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;

            ETEMDataModelEntities dbContext = new ETEMDataModelEntities();

            ValidationErrorsAsString = string.Empty;
            List<string> result = new List<string>();


            if (string.IsNullOrEmpty(this.IP))
            {
                result.Add(string.Format(BaseHelper.GetCaptionString("Entity_Common_Field_Mandatory"), BaseHelper.GetCaptionString("Entity_AllowIP_IP")));
            }

            var existIP = dbContext.AllowIPs.Where(u => u.IP == this.IP && u.idAllowIP != this.idAllowIP).FirstOrDefault();

            if (existIP != null)
            {
                result.Add(BaseHelper.GetCaptionString("Entity_AllowIP_IP_Exist"));
            }


            ValidationErrorsAsString = string.Join(",", result.ToArray());

            if (!string.IsNullOrEmpty(ValidationErrorsAsString))
            {
                outputContext.ResultCode = ETEMEnums.ResultEnum.Error;
            }

            outputContext.Message = this.ValidationErrorsAsString;
            outputContext.EntityID = this.EntityID.ToString();

            return result;
        }

        public string ValidationErrorsAsString { get; set; }
    }
}