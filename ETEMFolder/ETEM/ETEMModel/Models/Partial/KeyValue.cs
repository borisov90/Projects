using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Helpers;

namespace ETEMModel.Models
{
    public partial class KeyValue : Identifiable
    {
        public string AdditionalInfo { get; set; }

        public int EntityID
        {
            get { return this.idKeyValue; }
        }

        public string IdEntityString
        {
            get { return this.idKeyValue.ToString(); }
        }

        public string idKeyValueString { get; set; }

        public string ValidationErrorsAsString { get; set; }
        public List<string> ValidateEntity(CallContext outputContext)
        {
            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;
            ETEMDataModelEntities dbContext = new ETEMDataModelEntities();

            ValidationErrorsAsString = string.Empty;
            List<string> result = new List<string>();



            var existEntity = dbContext.KeyValues.Where(
                e => e.KeyValueIntCode == this.KeyValueIntCode &&
                     e.idKeyType == this.idKeyType &&
                     e.idKeyValue != this.idKeyValue
                     ).FirstOrDefault();

            if (existEntity != null)
            {
                result.Add(BaseHelper.GetCaptionString("Entity_KeyValue_KeyValueIntCode_Exist"));
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