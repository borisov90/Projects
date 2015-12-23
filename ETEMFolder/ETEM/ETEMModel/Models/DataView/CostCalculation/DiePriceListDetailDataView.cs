using ETEMModel.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models.DataView.CostCalculation
{
    public class DiePriceListDetailDataView : DiePriceListDetail, DataViewInterface
    {
        #region DataViewInterface Members
        public string IdEntity
        {
            get { return this.idDiePriceListDetail.ToString(); }
            set { this.IdEntity = value; }
        }
        #endregion

        public int idVendor { get; set; }
        public string VendorName { get; set; }
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
        public string NumberOfCavitiesName { get; set; }
        public string ProfileCategoryName { get; set; }
        public string ProfileComplexityName { get; set; }
        public string DieDiemensions
        {
            get
            {
                return Convert.ToInt32(this.DimensionA).ToString() + "x" + Convert.ToInt32(this.DimensionB).ToString();
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