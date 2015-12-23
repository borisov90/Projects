using ETEMModel.Helpers;
using ETEMModel.Helpers.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models.DataView.CostCalculation
{
    public class SAPDataExpenseDataView : SAPDataExpense, DataViewInterface
    {
        #region DataViewInterface Members
        public string IdEntity
        {
            get { return this.idSAPDataExpense.ToString(); }
            set { this.IdEntity = value; }
        }
        #endregion

        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
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

        public string CostCenterName { get; set; }
        public string ExpenseTypeName { get; set; }
    }
}