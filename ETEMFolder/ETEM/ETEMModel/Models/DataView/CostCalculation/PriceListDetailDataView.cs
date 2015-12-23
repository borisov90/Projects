using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models.DataView.CostCalculation
{
    public class PriceListDetailDataView : PriceListDetail, DataViewInterface
    {
        public string IdEntity
        {
            get { return this.idPriceListDetail.ToString(); }
            set { this.IdEntity = value; }
        }
    }
}