using ETEMModel.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models
{
    public partial class DiePriceList : Identifiable, IModifiable
    {
        #region Identifiable Members
        public int EntityID
        {
            get { return this.idDiePriceList; }
        }

        public string ValidationErrorsAsString { get; set; }
        public List<string> ValidateEntity(CallContext outputContext)
        {
            List<string> result = new List<string>();
            ValidationErrorsAsString = string.Empty;

            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;

            if (this.idVendor == Constants.INVALID_ID || this.idVendor == Constants.INVALID_ID_ZERO)
            {
                result.Add("The field `Vendor` is required!");
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

            var checkDiePriceList = dbContext.DiePriceLists.Where(w => w.idDiePriceList != this.idDiePriceList &&
                                                                  w.idVendor == this.idVendor &&
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
                result.Add("Die price list with the same vendor and period already exists in the database!");
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
    }
}