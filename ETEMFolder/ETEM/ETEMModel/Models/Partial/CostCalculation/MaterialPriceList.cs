using ETEMModel.Helpers;
using ETEMModel.Helpers.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models
{
    public partial class MaterialPriceList : Identifiable, IModifiable
    {
        #region Identifiable Members
        public int EntityID
        {
            get { return this.idMaterialPriceList; }
        }

        public string ValidationErrorsAsString { get; set; }
        public List<string> ValidateEntity(CallContext outputContext)
        {
            List<string> result = new List<string>();
            ValidationErrorsAsString = string.Empty;

            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;

            if (this.LME == int.MinValue)
            {
                result.Add("The field `LME (EUR/ton)` is required!");
            }
            if (this.Premium == int.MinValue)
            {
                result.Add("The field `PREMIUM (EUR/ton)` is required!");
            }
            if (this.DateFrom == DateTime.MinValue)
            {
                result.Add("The field `Valid from` is required!");
            }
            if (this.DateTo.HasValue && this.DateTo.Value < this.DateFrom)
            {
                result.Add("The field `Valid to` must be grater than or equal the field `Date from`!");
            }

            ETEMDataModelEntities dbContext = new ETEMDataModelEntities();

            var checkMaterialPriceList = dbContext.MaterialPriceLists.Where(w => w.idMaterialPriceList != this.idMaterialPriceList &&
                                                                            ((this.DateTo.HasValue && w.DateTo.HasValue &&
                                                                              (this.DateTo.Value >= w.DateFrom && this.DateTo.Value <= w.DateTo.Value ||
                                                                               this.DateFrom >= w.DateFrom && this.DateFrom <= w.DateTo.Value)) ||
                                                                             (!this.DateTo.HasValue && w.DateTo.HasValue &&
                                                                              (this.DateFrom <= w.DateTo.Value && this.DateFrom >= w.DateFrom ||
                                                                               this.DateFrom <= w.DateFrom)) ||
                                                                             (this.DateTo.HasValue && !w.DateTo.HasValue &&
                                                                              this.DateTo.Value >= w.DateFrom && this.DateFrom <= w.DateFrom) ||
                                                                             (!this.DateTo.HasValue && !w.DateTo.HasValue &&
                                                                              this.DateFrom == w.DateFrom))
                                                                           ).ToList();

            if (checkMaterialPriceList.Count > 0)
            {
                result.Add("Material price lists with overlapping date from/to interval persist in the data base!");
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

        public string LME_String
        {
            get
            {
                return this.LME.ToStringNotFormatted();
            }
        }
        public string LME_RoundString
        {
            get
            {
                return Math.Round(this.LME, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted();
            }
        }
        public string LME_RoundFourString
        {
            get
            {
                return Math.Round(this.LME, 4, MidpointRounding.AwayFromZero).ToStringNotFormatted4();
            }
        }
        public string LME_Formatted
        {
            get
            {
                return this.LME.ToStringFormatted();
            }
        }
        public string LME_RoundFormatted
        {
            get
            {
                return Math.Round(this.LME, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
            }
        }
        public string LME_RoundFourFormatted
        {
            get
            {
                return Math.Round(this.LME, 4, MidpointRounding.AwayFromZero).ToStringFormatted4();
            }
        }
        public string PremiumString
        {
            get
            {
                return this.Premium.ToStringNotFormatted();
            }
        }
        public string PremiumRoundString
        {
            get
            {
                return Math.Round(this.Premium, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted();
            }
        }
        public string PremiumRoundFourString
        {
            get
            {
                return Math.Round(this.Premium, 4, MidpointRounding.AwayFromZero).ToStringNotFormatted4();
            }
        }
        public string PremiumFormatted
        {
            get
            {
                return this.Premium.ToStringFormatted();
            }
        }
        public string PremiumRoundFormatted
        {
            get
            {
                return Math.Round(this.Premium, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
            }
        }
        public string PremiumRoundFourFormatted
        {
            get
            {
                return Math.Round(this.Premium, 4, MidpointRounding.AwayFromZero).ToStringFormatted4();
            }
        }
        public string DateFromString
        {
            get
            {
                return this.DateFrom.ToString(Constants.SHORT_DATE_PATTERN);
            }
        }
        public string DateToString
        {
            get
            {
                if (this.DateTo.HasValue)
                {
                    return this.DateTo.Value.ToString(Constants.SHORT_DATE_PATTERN);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}