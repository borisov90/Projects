using ETEMModel.Helpers.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models.DataView.CostCalculation
{
    public class ProductCostDataView
    {
        public string ProductCostType { get; set; }
        public string RowType { get; set; }

        public string Name { get; set; }
        public string RowKeyIntCode { get; set; }

        public decimal? Value_EUR_ton { get; set; }
        public string Value_EUR_ton_Formatted
        {
            get
            {
                return (this.Value_EUR_ton.HasValue ? Math.Round(this.Value_EUR_ton.Value, 2, MidpointRounding.AwayFromZero).ToStringFormatted() : string.Empty);
            }
        }
        public decimal? Value_EUR_kg { get; set; }
        public string Value_EUR_kg_Formatted
        {
            get
            {
                return (this.Value_EUR_kg.HasValue ? Math.Round(this.Value_EUR_kg.Value, 2, MidpointRounding.AwayFromZero).ToStringFormatted() : string.Empty);
            }
        }
        public decimal? Value_EUR_PC { get; set; }
        public string Value_EUR_PC_Formatted
        {
            get
            {
                return (this.Value_EUR_PC.HasValue ? Math.Round(this.Value_EUR_PC.Value, 2, MidpointRounding.AwayFromZero).ToStringFormatted() : string.Empty);
            }
        }
    }
}