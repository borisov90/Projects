using ETEMModel.Helpers;
using ETEMModel.Helpers.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models
{
    public partial class ProductivityAndScrap : Identifiable, IModifiable
    {
        #region Identifiable Members
        public int EntityID
        {
            get { return this.idProductivityAndScrap; }
        }

        public string ValidationErrorsAsString { get; set; }
        public List<string> ValidateEntity(CallContext outputContext)
        {
            List<string> result = new List<string>();
            ValidationErrorsAsString = string.Empty;

            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;

            if (this.DateFrom == DateTime.MinValue)
            {
                result.Add("The field `Valid from` is required!");
            }
            if (this.DateTo.HasValue && this.DateTo.Value < this.DateFrom)
            {
                result.Add("The field `Valid to` must be grater than or equal the field `Date from`!");
            }

            ETEMDataModelEntities dbContext = new ETEMDataModelEntities();

            var checkProductivityAndScrapList = dbContext.ProductivityAndScraps.Where(w => w.idProductivityAndScrap != this.idProductivityAndScrap &&
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

            if (checkProductivityAndScrapList.Count > 0)
            {
                result.Add("Productivity & Scrap list with overlapping date from/to interval persist in the data base!");
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
                
        #region Get Average Productivity KG/h and Scrap Rate by Press
        public decimal GetAverageProductivityKGhByPress(int idPress)
        {
            decimal result = decimal.Zero;

            if (this.ProductivityAndScrapDetails != null && this.ProductivityAndScrapDetails.Count > 0)
            {
                result = this.ProductivityAndScrapDetails.Where(w => w.idCostCenter == idPress && w.ProductivityKGh.HasValue).Average(a => a.ProductivityKGh.Value);
            }

            return result;
        }

        public decimal GetAverageScrapRateByPress(int idPress)
        {
            decimal result = decimal.Zero;

            if (this.ProductivityAndScrapDetails != null && this.ProductivityAndScrapDetails.Count > 0)
            {
                result = this.ProductivityAndScrapDetails.Where(w => w.idCostCenter == idPress && w.ScrapRate.HasValue).Average(a => a.ScrapRate.Value);
            }

            return result;
        }

        public decimal GetAverageProductivityKGhByPress(string pressIntCode)
        {
            decimal result = decimal.Zero;

            if (this.ProductivityAndScrapDetails != null && this.ProductivityAndScrapDetails.Count > 0)
            {
                result = this.ProductivityAndScrapDetails.Where(w => w.KeyValue != null && w.KeyValue.KeyValueIntCode == pressIntCode && w.ProductivityKGh.HasValue).Average(a => a.ProductivityKGh.Value);
            }

            return result;
        }

        public decimal GetAverageScrapRateByPress(string pressIntCode)
        {
            decimal result = decimal.Zero;

            if (this.ProductivityAndScrapDetails != null && this.ProductivityAndScrapDetails.Count > 0)
            {
                result = this.ProductivityAndScrapDetails.Where(w => w.KeyValue != null && w.KeyValue.KeyValueIntCode == pressIntCode && w.ScrapRate.HasValue).Average(a => a.ScrapRate.Value);
            }

            return result;
        }

        public decimal AverageProductivityKGhForPressSMS1
        {
            get
            {
                return this.GetAverageProductivityKGhByPress(ETEMEnums.CostCenterEnum.SMS1.ToString());
            }
        }
        public string AverageProductivityKGhForPressSMS1_RoundNotFormatted
        {
            get
            {
                return Math.Round(this.AverageProductivityKGhForPressSMS1, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted();
            }
        }
        public string AverageProductivityKGhForPressSMS1_RoundFormatted
        {
            get
            {
                return Math.Round(this.AverageProductivityKGhForPressSMS1, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
            }
        }

        public decimal AverageScrapRateForPressSMS1
        {
            get
            {
                return this.GetAverageScrapRateByPress(ETEMEnums.CostCenterEnum.SMS1.ToString());
            }
        }
        public string AverageScrapRateForPressSMS1_RoundNotFormatted
        {
            get
            {
                return Math.Round(this.AverageScrapRateForPressSMS1, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted();
            }
        }
        public string AverageScrapRateForPressSMS1_RoundFormatted
        {
            get
            {
                return Math.Round(this.AverageScrapRateForPressSMS1, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
            }
        }

        public decimal AverageProductivityKGhForPressSMS2
        {
            get
            {
                return this.GetAverageProductivityKGhByPress(ETEMEnums.CostCenterEnum.SMS2.ToString());
            }
        }
        public string AverageProductivityKGhForPressSMS2_RoundNotFormatted
        {
            get
            {
                return Math.Round(this.AverageProductivityKGhForPressSMS2, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted();
            }
        }
        public string AverageProductivityKGhForPressSMS2_RoundFormatted
        {
            get
            {
                return Math.Round(this.AverageProductivityKGhForPressSMS2, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
            }
        }

        public decimal AverageScrapRateForPressSMS2
        {
            get
            {
                return this.GetAverageScrapRateByPress(ETEMEnums.CostCenterEnum.SMS2.ToString());
            }
        }
        public string AverageScrapRateForPressSMS2_RoundNotFormatted
        {
            get
            {
                return Math.Round(this.AverageScrapRateForPressSMS2, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted();
            }
        }
        public string AverageScrapRateForPressSMS2_RoundFormatted
        {
            get
            {
                return Math.Round(this.AverageScrapRateForPressSMS2, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
            }
        }

        public decimal AverageProductivityKGhForPressBREDA
        {
            get
            {
                return this.GetAverageProductivityKGhByPress(ETEMEnums.CostCenterEnum.Breda.ToString());
            }
        }
        public string AverageProductivityKGhForPressBREDA_RoundNotFormatted
        {
            get
            {
                return Math.Round(this.AverageProductivityKGhForPressBREDA, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted();
            }
        }
        public string AverageProductivityKGhForPressBREDA_RoundFormatted
        {
            get
            {
                return Math.Round(this.AverageProductivityKGhForPressBREDA, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
            }
        }

        public decimal AverageScrapRateForPressBREDA
        {
            get
            {
                return this.GetAverageScrapRateByPress(ETEMEnums.CostCenterEnum.Breda.ToString());
            }
        }
        public string AverageScrapRateForPressBREDA_RoundNotFormatted
        {
            get
            {
                return Math.Round(this.AverageScrapRateForPressBREDA, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted();
            }
        }
        public string AverageScrapRateForPressBREDA_RoundFormatted
        {
            get
            {
                return Math.Round(this.AverageScrapRateForPressBREDA, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
            }
        }

        public decimal AverageProductivityKGhForPressFARREL
        {
            get
            {
                return this.GetAverageProductivityKGhByPress(ETEMEnums.CostCenterEnum.Farrel.ToString());
            }
        }
        public string AverageProductivityKGhForPressFARREL_RoundNotFormatted
        {
            get
            {
                return Math.Round(this.AverageProductivityKGhForPressFARREL, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted();
            }
        }
        public string AverageProductivityKGhForPressFARREL_RoundFormatted
        {
            get
            {
                return Math.Round(this.AverageProductivityKGhForPressFARREL, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
            }
        }

        public decimal AverageScrapRateForPressFARREL
        {
            get
            {
                return this.GetAverageScrapRateByPress(ETEMEnums.CostCenterEnum.Farrel.ToString());
            }
        }
        public string AverageScrapRateForPressFARREL_RoundNotFormatted
        {
            get
            {
                return Math.Round(this.AverageScrapRateForPressFARREL, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted();
            }
        }
        public string AverageScrapRateForPressFARREL_RoundFormatted
        {
            get
            {
                return Math.Round(this.AverageScrapRateForPressFARREL, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
            }
        }
        #endregion
    }
}