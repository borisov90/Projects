using System;
using System.Linq;
using ETEMModel.Helpers.Extentions;


namespace ETEMModel.Models.DataView.CostCalculation
{
    public class OfferDataView : Offer
    {
        public string IdEntity
        {
            get { return this.idOffer.ToString(); }
            set { this.IdEntity = value; }
        }

        public string ProfileStr { get ; set; }
        public string SalesManager { get; set; }
        public string FinalPriceSrt { get; set; }

        

        public string TotalSalesPriceStr 
        {
            get {

                    if (TotalSalesPrice.HasValue)
                    {
                        return TotalSalesPrice.ToStringFormatted();
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            set
            {
                TotalSalesPriceStr = value;
            }
        }

        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }

         public string FullName
        {
            get
            {
                return
                        (string.IsNullOrEmpty(this.FirstName) ? "" : this.FirstName.Trim()) +
                        (string.IsNullOrEmpty(this.SecondName) ? "" : " " + this.SecondName.Trim()) +
                        (string.IsNullOrEmpty(this.LastName) ? "" : " " + this.LastName.Trim());
            }
        }

    }
}