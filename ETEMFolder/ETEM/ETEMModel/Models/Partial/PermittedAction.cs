using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Helpers;

namespace ETEMModel.Models
{
    public partial class PermittedAction : Identifiable
    {
        public int EntityID
        {
            get { return this.idPermittedAction; }
        }

        public string ValidationErrorsAsString { get; set; }
        public List<string> ValidateEntity(CallContext outputContext)
        {
            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;
            ETEMDataModelEntities dbContext = new ETEMDataModelEntities();

            ValidationErrorsAsString = string.Empty;
            List<string> result = new List<string>();



            var existEntity = dbContext.PermittedActions.Where(
                e => e.GroupSecuritySetting == this.GroupSecuritySetting &&
                     e.SecuritySetting == this.SecuritySetting &&
                     e.idPermittedAction != this.idPermittedAction
                     ).FirstOrDefault();

            if (existEntity != null)
            {
                result.Add(BaseHelper.GetCaptionString("Entity_PermittedAction_GroupSecuritySetting_SecuritySetting_Exist"));
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


    }
}