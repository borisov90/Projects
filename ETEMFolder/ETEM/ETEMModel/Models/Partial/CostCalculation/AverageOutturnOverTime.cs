using ETEMModel.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Helpers.Extentions;

namespace ETEMModel.Models
{
    public partial class AverageOutturnOverTime : Identifiable, IModifiable
    {
         #region Identifiable Members
        public int EntityID
        {
            get { return this.idAverageOutturnOverTime; }
        }

        public string ValidationErrorsAsString { get; set; }
        public List<string> ValidateEntity(CallContext outputContext)
        {
            List<string> result = new List<string>();
            ValidationErrorsAsString = string.Empty;

            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;

            if (this.ValueOfPressSMS1 == int.MinValue)
            {
                result.Add("The field `Value of press SMS1` is required!");
            }
            if (this.ValueOfPressSMS2 == int.MinValue)
            {
                result.Add("The field `Value of press SMS2` is required!");
            }

            if (this.ValueOfPressBREDA== int.MinValue)
            {
                result.Add("The field `Value of press BREDA` is required!");
            }

             if (this.ValueOfPressFARREL == int.MinValue)
            {
                result.Add("The field `Value of press FARREL` is required!");
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

            var checkMaterialPriceList = dbContext.AverageOutturnOverTimes.Where(w => w.idAverageOutturnOverTime != this.idAverageOutturnOverTime &&
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
                result.Add("Average outturn over time with overlapping date from/to interval persist in the data base!");
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


        public string ValueOfPressSMS1_RoundFormatted
        {
            get
            {
                if (this.ValueOfPressSMS1.HasValue)
                {
                    return Math.Round(this.ValueOfPressSMS1.Value, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
                }
                else
                {
                    return "";
                }
            }
        }

       public string ValueOfPressSMS2_RoundFormatted
        {
            get
            {
                if (this.ValueOfPressSMS1.HasValue)
                {
                    return Math.Round(this.ValueOfPressSMS2.Value, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
                }
                else
                {
                    return "";
                }
            }
        }

        public string ValueOfPressBREDA_RoundFormatted
        {
            get
            {
                if (this.ValueOfPressSMS1.HasValue)
                {
                    return Math.Round(this.ValueOfPressBREDA.Value, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
                }
                else
                {
                    return "";
                }
            }
        }

        public string ValueOfPressFARREL_RoundFormatted
        {
            get
            {
                if (this.ValueOfPressSMS1.HasValue)
                {
                    return Math.Round(this.ValueOfPressFARREL.Value, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
                }
                else
                {
                    return "";
                }
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