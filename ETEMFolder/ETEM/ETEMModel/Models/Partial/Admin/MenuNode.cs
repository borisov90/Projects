using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Helpers;
using System.Linq.Expressions;

namespace ETEMModel.Models
{
    public partial class MenuNode : Identifiable
    {

        #region Identifiable Members

        public int EntityID
        {
            get { return this.idNode; }
        }



        public string ValidationErrorsAsString { get; set; }
        public List<string> ValidateEntity(CallContext outputContext)
        {
            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;

            ETEMDataModelEntities dbContext = new ETEMDataModelEntities();

            ValidationErrorsAsString = string.Empty;
            List<string> result = new List<string>();
            if (string.IsNullOrEmpty(this.name))
            {
                result.Add(String.Format(BaseHelper.GetCaptionString("Entity_Common_Field_Mandatory"),
                    BaseHelper.GetCaptionString("Entity_MenuNode_Name")));
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

        #endregion
    }
}