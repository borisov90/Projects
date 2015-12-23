using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Helpers;

namespace ETEMModel.Models
{
    public partial class Module : Identifiable
    {

        public int EntityID
        {
            get { return this.idModule; }
        }

        public List<string> ValidateEntity(CallContext outputContext)
        {
            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;

            ETEMDataModelEntities dbContext = new ETEMDataModelEntities();

            ValidationErrorsAsString = string.Empty;
            List<string> result = new List<string>();


            if (string.IsNullOrEmpty(this.ModuleName))
            {
                result.Add(string.Format(BaseHelper.GetCaptionString("Entity_Common_Field_Mandatory"), BaseHelper.GetCaptionString("Entity_Module_ModuleName")));
            }

            if (string.IsNullOrEmpty(this.ModuleSysName))
            {
                result.Add(string.Format(BaseHelper.GetCaptionString("Entity_Common_Field_Mandatory"), BaseHelper.GetCaptionString("Entity_Module_ModuleSysName")));
            }

            var existIP = dbContext.Modules.Where(u => u.ModuleSysName == this.ModuleSysName && u.idModule != this.idModule).FirstOrDefault();

            if (existIP != null)
            {
                result.Add(BaseHelper.GetCaptionString("Entity__Module_ModuleSysName_Exist"));
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