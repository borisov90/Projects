using ETEMModel.Helpers;
using ETEMModel.Helpers.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models
{
    public partial class DiePriceListDetail : Identifiable
    {
        #region Identifiable Members
        public int EntityID
        {
            get { return this.idDiePriceListDetail; }
        }

        public string ValidationErrorsAsString { get; set; }
        public List<string> ValidateEntity(CallContext outputContext)
        {
            List<string> result = new List<string>();
            ValidationErrorsAsString = string.Empty;

            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;

            if (this.idDiePriceList == Constants.INVALID_ID || this.idDiePriceList == Constants.INVALID_ID_ZERO)
            {
                result.Add("The field `Vendor die price list` is required!");
            }
            if (this.idNumberOfCavities == Constants.INVALID_ID || this.idNumberOfCavities == Constants.INVALID_ID_ZERO)
            {
                result.Add("The field `Number of cavities` is required!");
            }
            if (this.idNumberOfCavities == Constants.INVALID_ID || this.idNumberOfCavities == Constants.INVALID_ID_ZERO)
            {
                result.Add("The field `Profile category` is required!");
            }
            if (this.idNumberOfCavities == Constants.INVALID_ID || this.idNumberOfCavities == Constants.INVALID_ID_ZERO)
            {
                result.Add("The field `Profile complexity` is required!");
            }
            if (this.DimensionA == int.MinValue)
            {
                result.Add("The field `Dimension A (mm)` is required!");
            }
            if (this.DimensionB == int.MinValue)
            {
                result.Add("The field `Dimension B (mm)` is required!");
            }
            if (this.Price == decimal.MinValue)
            {
                result.Add("The field `Die price (EUR)` is required!");
            }
            if (this.Lifespan == decimal.MinValue)
            {
                result.Add("The field `Lifespan (ton)` is required!");
            }

            ETEMDataModelEntities dbContext = new ETEMDataModelEntities();

            var checkDiePriceListDetail = dbContext.DiePriceListDetails.Where(w => w.idDiePriceListDetail != this.idDiePriceListDetail &&
                                                                              w.idDiePriceList == this.idDiePriceList &&
                                                                              w.idNumberOfCavities == this.idNumberOfCavities &&
                                                                              w.idProfileComplexity == this.idProfileComplexity &&
                                                                              w.idProfileCategory == this.idProfileCategory &&
                                                                              w.Price == this.Price &&
                                                                              w.DimensionA == this.DimensionA &&
                                                                              w.DimensionB == this.DimensionB).ToList();

            if (checkDiePriceListDetail.Count > 0)
            {
                result.Add("Die price for the same vendor and dimensions already exists in the database!");
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

        public string DimensionA_String
        {
            get
            {
                return Convert.ToInt32(this.DimensionA).ToString();
            }
        }
        public string DimensionB_String
        {
            get
            {
                return Convert.ToInt32(this.DimensionB).ToString();
            }
        }

        public string PriceString
        {
            get
            {
                return this.Price.ToStringNotFormatted();
            }
        }
        public string PriceRoundString
        {
            get
            {
                return Math.Round(this.Price, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted();
            }
        }
        public string PriceRoundFourString
        {
            get
            {
                return Math.Round(this.Price, 4, MidpointRounding.AwayFromZero).ToStringNotFormatted4();
            }
        }
        public string PriceFormatted
        {
            get
            {
                return this.Price.ToStringFormatted();
            }
        }
        public string PriceRoundFormatted
        {
            get
            {
                return Math.Round(this.Price, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
            }
        }
        public string PriceRoundFourFormatted
        {
            get
            {
                return Math.Round(this.Price, 4, MidpointRounding.AwayFromZero).ToStringFormatted4();
            }
        }
        public string LifespanString
        {
            get
            {
                return this.Lifespan.ToStringNotFormatted();
            }
        }
        public string LifespanRoundString
        {
            get
            {
                return Math.Round(this.Lifespan, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted();
            }
        }
        public string LifespanRoundFourString
        {
            get
            {
                return Math.Round(this.Lifespan, 4, MidpointRounding.AwayFromZero).ToStringNotFormatted4();
            }
        }
        public string LifespanFormatted
        {
            get
            {
                return this.Lifespan.ToStringFormatted();
            }
        }
        public string LifespanRoundFormatted
        {
            get
            {
                return Math.Round(this.Lifespan, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
            }
        }
        public string LifespanRoundFourFormatted
        {
            get
            {
                return Math.Round(this.Lifespan, 4, MidpointRounding.AwayFromZero).ToStringFormatted4();
            }
        }
    }
}