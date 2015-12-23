using ETEMModel.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models.DataView.CostCalculation
{
    public class OfferOverviewDataView
    {
        public string OfferOverviewType { get; set; }
        public ETEMEnums.DataRowType RowType { get; set; }

        public string Name { get; set; }
        public string RowKeyIntCode { get; set; }

        public decimal? Indicator_Value { get; set; }
    }
    
}