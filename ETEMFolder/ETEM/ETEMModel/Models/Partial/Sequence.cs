using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Helpers;

namespace ETEMModel.Models
{
    public partial class Sequence : Identifiable
    {
        #region Identifiable Members

        public int EntityID
        {
            get { return this.idSequence; }
        }

        public string ValidationErrorsAsString { get; set; }
        public List<string> ValidateEntity(CallContext outputContext)
        {
            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;
            ETEMDataModelEntities dbContext = new ETEMDataModelEntities();

            ValidationErrorsAsString = string.Empty;
            List<string> result = new List<string>();

            if (string.IsNullOrEmpty(this.Resource))
            {
                result.Add(String.Format(BaseHelper.GetCaptionString("Entity_Common_Field_Mandatory"), BaseHelper.GetCaptionString("Entity_Sequence_Resource")));
            }
            if (this.nextVal <= 0)
            {
                result.Add(String.Format(BaseHelper.GetCaptionString("Entity_Common_Field_Mandatory"), BaseHelper.GetCaptionString("Entity_Sequence_NextVal_Grater_Than_Zero")));
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