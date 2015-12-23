using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Helpers;

namespace ETEMModel.Models
{
    public partial class KeyType : Identifiable
    {

        #region Identifiable

        public int EntityID { get { return this.idKeyType; } }
        public string ValidationErrorsAsString { get; set; }
        public List<string> ValidateEntity(CallContext outputContext)
        {
            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;

            ETEMDataModelEntities dbContext = new ETEMDataModelEntities();

            ValidationErrorsAsString = string.Empty;
            List<string> result = new List<string>();

            var existEntity = dbContext.KeyTypes.Where(e => e.KeyTypeIntCode == this.KeyTypeIntCode && e.idKeyType != this.idKeyType).FirstOrDefault();

            if (existEntity != null)
            {
                result.Add(BaseHelper.GetCaptionString("Entity_KeyType_KeyTypeIntCode_Exist"));
                outputContext.ResultCode = ETEMEnums.ResultEnum.Error;
            }

            ValidationErrorsAsString = string.Join(",", result.ToArray());


            outputContext.Message = this.ValidationErrorsAsString;
            outputContext.EntityID = this.EntityID.ToString();

            return result;
        }

        #endregion

        public bool IsSystemBool
        {
            get
            {

                if (this != null)
                {
                    return (this.IsSystem == 1) ? true : false;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                this.IsSystem = (byte)((value) ? 1 : 2);
            }
        }

        public string IsSystemAsString
        {
            get { return (IsSystemBool) ? BaseHelper.GetCaptionString("UI_YES") : BaseHelper.GetCaptionString("UI_NO"); }
        }


       
    
        
        


    }
}