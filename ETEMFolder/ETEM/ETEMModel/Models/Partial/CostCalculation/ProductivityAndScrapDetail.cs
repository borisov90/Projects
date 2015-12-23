using ETEMModel.Helpers;
using ETEMModel.Helpers.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models
{
    public partial class ProductivityAndScrapDetail : Identifiable, IModifiable
    {
        #region Identifiable Members
        public int EntityID
        {
            get { return this.idProductivityAndScrapDetail; }
        }

        public string ValidationErrorsAsString { get; set; }
        public List<string> ValidateEntity(CallContext outputContext)
        {
            List<string> result = new List<string>();
            ValidationErrorsAsString = string.Empty;

            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;



            return result;
        }
        #endregion

        #region IModifiable Members
        public void SetCreationData(CallContext outputContext)
        {
            int tmpValue = Constants.INVALID_ID;
            if (Int32.TryParse(outputContext.CurrentConsumerID, out tmpValue))
            {
                this.idCreateUser = tmpValue;
            }
            this.dCreate = DateTime.Now;
        }

        public void SetModificationData(CallContext outputContext)
        {
            int tmpValue = Constants.INVALID_ID;
            if (Int32.TryParse(outputContext.CurrentConsumerID, out tmpValue))
            {
                this.idModifyUser = tmpValue;
            }
            this.dModify = DateTime.Now;
        }
        #endregion

        public string SumOfHours_String
        {
            get
            {
                return this.SumOfHours.ToStringNotFormatted();
            }
        }
        public string SumOfHours_RoundString
        {
            get
            {
                return (this.SumOfHours.HasValue ? Math.Round(this.SumOfHours.Value, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted() : string.Empty);
            }
        }
        public string SumOfHours_Formatted
        {
            get
            {
                return this.SumOfHours.ToStringFormatted();
            }
        }
        public string SumOfHours_RoundFormatted
        {
            get
            {
                return (this.SumOfHours.HasValue ? Math.Round(this.SumOfHours.Value, 2, MidpointRounding.AwayFromZero).ToStringFormatted() : string.Empty);
            }
        }
        public string SumOfConsumption_String
        {
            get
            {
                return this.SumOfConsumption.ToStringNotFormatted();
            }
        }
        public string SumOfConsumption_RoundString
        {
            get
            {
                return (this.SumOfConsumption.HasValue ? Math.Round(this.SumOfConsumption.Value, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted() : string.Empty);
            }
        }
        public string SumOfConsumption_Formatted
        {
            get
            {
                return this.SumOfConsumption.ToStringFormatted();
            }
        }
        public string SumOfConsumption_RoundFormatted
        {
            get
            {
                return (this.SumOfConsumption.HasValue ? Math.Round(this.SumOfConsumption.Value, 2, MidpointRounding.AwayFromZero).ToStringFormatted() : string.Empty);
            }
        }
        public string SumOfProduction_String
        {
            get
            {
                return this.SumOfProduction.ToStringNotFormatted();
            }
        }
        public string SumOfProduction_RoundString
        {
            get
            {
                return (this.SumOfProduction.HasValue ? Math.Round(this.SumOfProduction.Value, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted() : string.Empty);
            }
        }
        public string SumOfProduction_Formatted
        {
            get
            {
                return this.SumOfProduction.ToStringFormatted();
            }
        }
        public string SumOfProduction_RoundFormatted
        {
            get
            {
                return (this.SumOfProduction.HasValue ? Math.Round(this.SumOfProduction.Value, 2, MidpointRounding.AwayFromZero).ToStringFormatted() : string.Empty);
            }
        }
        public string ProductivityKGh_String
        {
            get
            {
                return this.ProductivityKGh.ToStringNotFormatted();
            }
        }
        public string ProductivityKGh_RoundString
        {
            get
            {
                return (this.ProductivityKGh.HasValue ? Math.Round(this.ProductivityKGh.Value, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted() : string.Empty);
            }
        }
        public string ProductivityKGh_Formatted
        {
            get
            {
                return this.ProductivityKGh.ToStringFormatted();
            }
        }
        public string ProductivityKGh_RoundFormatted
        {
            get
            {
                return (this.ProductivityKGh.HasValue ? Math.Round(this.ProductivityKGh.Value, 2, MidpointRounding.AwayFromZero).ToStringFormatted() : string.Empty);
            }
        }
        public string ScrapRate_String
        {
            get
            {
                return (this.ScrapRate.HasValue ? this.ScrapRate.Value.ToStringNotFormatted() : string.Empty);
            }
        }
        public string ScrapRate_RoundString
        {
            get
            {
                return (this.ScrapRate.HasValue ? Math.Round(this.ScrapRate.Value, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted() : string.Empty);
            }
        }
        public string ScrapRate_Formatted
        {
            get
            {
                return (this.ScrapRate.HasValue ? this.ScrapRate.Value.ToStringFormatted() : string.Empty);
            }
        }
        public string ScrapRate_RoundFormatted
        {
            get
            {
                return (this.ScrapRate.HasValue ? Math.Round(this.ScrapRate.Value, 2, MidpointRounding.AwayFromZero).ToStringFormatted() : string.Empty);
            }
        }
        public decimal? ScrapRatePercent
        {
            get
            {
                return this.ScrapRate.HasValue ? (this.ScrapRate.Value * 100) : new Nullable<decimal>();
            }
        }
        public string ScrapRatePercent_String
        {
            get
            {
                return (this.ScrapRate.HasValue ? (this.ScrapRate.Value * 100).ToStringNotFormatted() : string.Empty);
            }
        }
        public string ScrapRatePercent_RoundString
        {
            get
            {
                return (this.ScrapRate.HasValue ? Math.Round((this.ScrapRate.Value * 100), 2, MidpointRounding.AwayFromZero).ToStringNotFormatted() : string.Empty);
            }
        }
        public string ScrapRatePercent_Formatted
        {
            get
            {
                return (this.ScrapRate.HasValue ? (this.ScrapRate.Value * 100).ToStringFormatted() : string.Empty);
            }
        }
        public string ScrapRatePercent_RoundFormatted
        {
            get
            {
                return (this.ScrapRate.HasValue ? Math.Round((this.ScrapRate.Value * 100), 2, MidpointRounding.AwayFromZero).ToStringFormatted() : string.Empty);
            }
        }
    }
}