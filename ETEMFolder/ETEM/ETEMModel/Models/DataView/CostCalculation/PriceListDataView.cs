using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models.DataView.CostCalculation
{
    public class PriceListDataView : PriceList, DataViewInterface
    {
        public string IdEntity
        {
            get { return this.idPriceList.ToString(); }
            set { this.IdEntity = value; }
        }
    }
}