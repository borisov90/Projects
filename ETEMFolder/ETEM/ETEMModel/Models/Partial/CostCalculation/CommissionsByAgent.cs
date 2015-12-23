using ETEMModel.Helpers;
using ETEMModel.Helpers.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models
{
    public partial class CommissionsByAgent : Identifiable, IModifiable
    {
        #region Identifiable Members
        public int EntityID
        {
            get { return this.idCommissionsByAgent; }
        }

        public string ValidationErrorsAsString { get; set; }
        public List<string> ValidateEntity(CallContext outputContext)
        {
            List<string> result = new List<string>();
            ValidationErrorsAsString = string.Empty;

            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;

            if (this.idAgent == Constants.INVALID_ID || this.idAgent == Constants.INVALID_ID_ZERO)
            {
                result.Add("The field `Agent` is required!");
            }
            if (this.DateFrom == DateTime.MinValue)
            {
                result.Add("The field `Valid from` is required!");
            }
            if (this.DateTo.HasValue && this.DateTo.Value < this.DateFrom)
            {
                result.Add("The field `Valid to` must be grater than or equal the field `Date from`!");
            }
            if (!this.FixedCommission.HasValue)
            {
                result.Add("The field `Fixed commission (EUR/ton)` is required!");
            }
            if (!this.CommissionPercent.HasValue)
            {
                result.Add("The field `Commission % (% of final invoice value)` is required!");
            }

            ETEMDataModelEntities dbContext = new ETEMDataModelEntities();

            var checkDiePriceList = dbContext.CommissionsByAgents.Where(w => w.idCommissionsByAgent != this.idCommissionsByAgent &&
                                                                        w.idAgent == this.idAgent &&
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

            if (checkDiePriceList.Count > 0)
            {
                result.Add("Commissions with the same agent and period already exists in the database!");
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

        public string FixedCommissionString
        {
            get
            {
                return (this.FixedCommission.HasValue ? this.FixedCommission.Value : decimal.Zero).ToStringNotFormatted();
            }
        }
        public string FixedCommissionRoundString
        {
            get
            {
                return Math.Round((this.FixedCommission.HasValue ? this.FixedCommission.Value : decimal.Zero), 2, MidpointRounding.AwayFromZero).ToStringNotFormatted();
            }
        }
        public string FixedCommissionRoundFourString
        {
            get
            {
                return Math.Round((this.FixedCommission.HasValue ? this.FixedCommission.Value : decimal.Zero), 4, MidpointRounding.AwayFromZero).ToStringNotFormatted4();
            }
        }
        public string FixedCommissionFormatted
        {
            get
            {
                return (this.FixedCommission.HasValue ? this.FixedCommission.Value : decimal.Zero).ToStringFormatted();
            }
        }
        public string FixedCommissionRoundFormatted
        {
            get
            {
                return Math.Round((this.FixedCommission.HasValue ? this.FixedCommission.Value : decimal.Zero), 2, MidpointRounding.AwayFromZero).ToStringFormatted();
            }
        }
        public string FixedCommissionRoundFourFormatted
        {
            get
            {
                return Math.Round((this.FixedCommission.HasValue ? this.FixedCommission.Value : decimal.Zero), 4, MidpointRounding.AwayFromZero).ToStringFormatted4();
            }
        }
        public string CommissionPercentString
        {
            get
            {
                return (this.CommissionPercent.HasValue ? this.CommissionPercent.Value : decimal.Zero).ToStringNotFormatted();
            }
        }
        public string CommissionPercentRoundString
        {
            get
            {
                return Math.Round((this.CommissionPercent.HasValue ? this.CommissionPercent.Value : decimal.Zero), 2, MidpointRounding.AwayFromZero).ToStringNotFormatted();
            }
        }
        public string CommissionPercentRoundFourString
        {
            get
            {
                return Math.Round((this.CommissionPercent.HasValue ? this.CommissionPercent.Value : decimal.Zero), 4, MidpointRounding.AwayFromZero).ToStringNotFormatted4();
            }
        }
        public string CommissionPercentFormatted
        {
            get
            {
                return (this.CommissionPercent.HasValue ? this.CommissionPercent.Value : decimal.Zero).ToStringFormatted();
            }
        }
        public string CommissionPercentRoundFormatted
        {
            get
            {
                return Math.Round((this.CommissionPercent.HasValue ? this.CommissionPercent.Value : decimal.Zero), 2, MidpointRounding.AwayFromZero).ToStringFormatted();
            }
        }
        public string CommissionPercentRoundFourFormatted
        {
            get
            {
                return Math.Round((this.CommissionPercent.HasValue ? this.CommissionPercent.Value : decimal.Zero), 4, MidpointRounding.AwayFromZero).ToStringFormatted4();
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

        public string Status
        {
            get
            {
                if (this.DateTo.HasValue && this.DateFrom <= DateTime.Today && this.DateTo.Value >= DateTime.Today)
                {
                    return "Active";
                }
                else if (!this.DateTo.HasValue && this.DateFrom <= DateTime.Today)
                {
                    return "Active";
                }
                else
                {
                    return "Inactive";
                }
            }
        }
    }
}