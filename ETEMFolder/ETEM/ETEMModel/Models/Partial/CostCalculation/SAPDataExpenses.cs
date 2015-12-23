using ETEMModel.Helpers;
using ETEMModel.Helpers.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models
{
    public partial class SAPDataExpense : Identifiable
    {
        #region Identifiable Members
        public int EntityID
        {
            get { return this.idSAPDataExpense; }
        }

        public string ValidationErrorsAsString { get; set; }
        public List<string> ValidateEntity(CallContext outputContext)
        {
            List<string> result = new List<string>();
            ValidationErrorsAsString = string.Empty;

            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;

            if (this.idCostCenter == Constants.INVALID_ID || this.idCostCenter == Constants.INVALID_ID_ZERO)
            {
                result.Add("The field `Cost center` is required!");
            }
            if (this.idExpensesType == Constants.INVALID_ID || this.idExpensesType == Constants.INVALID_ID_ZERO)
            {
                result.Add("The field `Type of expense` is required!");
            }
            if (this.ValueData == decimal.MinValue)
            {
                result.Add("The field `Value for the last 12 months (EUR)` is required!");
            }

            this.ValidationErrorsAsString = string.Join(Constants.ERROR_MESSAGES_SEPARATOR, result.ToArray());

            if (!string.IsNullOrEmpty(this.ValidationErrorsAsString))
            {
                outputContext.ResultCode = ETEMEnums.ResultEnum.Error;
            }

            outputContext.Message = this.ValidationErrorsAsString;
            if (result.Count > 0)
            {
                outputContext.EntityID = "";
            }
            else
            {
                outputContext.EntityID = this.EntityID.ToString();
            }

            return result;
        }
        #endregion

        public string ValueDataString
        {
            get
            {
                return this.ValueData.ToStringNotFormatted();
            }
        }
        public string ValueDataRoundString
        {
            get
            {
                return Math.Round(this.ValueData, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted();
            }
        }
        public string ValueDataRoundFourString
        {
            get
            {
                return Math.Round(this.ValueData, 4, MidpointRounding.AwayFromZero).ToStringNotFormatted4();
            }
        }
        public string ValueDataFormatted
        {
            get
            {
                return this.ValueData.ToStringFormatted();
            }
        }
        public string ValueDataRoundFormatted
        {
            get
            {
                return Math.Round(this.ValueData, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
            }
        }
        public string ValueDataRoundFourFormatted
        {
            get
            {
                return Math.Round(this.ValueData, 4, MidpointRounding.AwayFromZero).ToStringFormatted4();
            }
        }
    }
}