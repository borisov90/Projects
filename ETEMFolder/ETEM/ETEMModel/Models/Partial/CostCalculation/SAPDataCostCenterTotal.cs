using ETEMModel.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models 
{
    public partial class SAPDataCostCenterTotal : Identifiable
    {
        #region Identifiable Members

        public int EntityID
        {
            get
            {
                return this.idSAPDataCostCenterTotal;
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