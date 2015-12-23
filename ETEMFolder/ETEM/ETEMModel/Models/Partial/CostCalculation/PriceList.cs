using ETEMModel.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models
{
    public partial class PriceList : Identifiable, IModifiable
    {
        #region Identifiable Members
        public int EntityID
        {
            get { return this.idPriceList; }
        }

        public string ValidationErrorsAsString { get; set; }
        public List<string> ValidateEntity(CallContext outputContext)
        {
            List<string> result = new List<string>();

            outputContext.ResultCode = UMSEnums.ResultEnum.Success;

            ValidationErrorsAsString = string.Empty;
            
            

            return result;
        }
        #endregion

        #region IModifiable Members
        public void SetCreationData(CallContext outputContext)
        {
            this.idCreateUser = Int32.Parse(outputContext.CurrentConsumerID);
            this.dCreate = DateTime.Now;
        }

        public void SetModificationData(CallContext outputContext)
        {
            this.idModifyUser = Int32.Parse(outputContext.CurrentConsumerID);
            this.dModify = DateTime.Now;
        }
        #endregion
    }
}