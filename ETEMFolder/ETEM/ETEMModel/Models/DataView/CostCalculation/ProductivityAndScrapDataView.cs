using ETEMModel.Helpers.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models.DataView.CostCalculation
{
    public class ProductivityAndScrapDataView : ProductivityAndScrap, DataViewInterface
    {
        #region DataViewInterface Members
        public string IdEntity
        {
            get { return this.idProductivityAndScrap.ToString(); }
            set { this.IdEntity = value; }
        }
        #endregion

        public decimal? ProductivityKGh { get; set; }
        public decimal? ScrapRate { get; set; }
        public decimal? ScrapRatePercent
        {
            get
            {
                return this.ScrapRate.HasValue ? (this.ScrapRate.Value * 100) : new Nullable<decimal>();
            }
        }

        #region Properties for Average Productivity KG/h and Scrap Rate by Press
        public decimal AvgProductivityKGhForPressSMS1 { get; set; }
        public string AvgProductivityKGhForPressSMS1_RoundNotFormatted
        {
            get
            {
                return Math.Round(this.AvgProductivityKGhForPressSMS1, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted();
            }
        }
        public string AvgProductivityKGhForPressSMS1_RoundFormatted
        {
            get
            {
                return Math.Round(this.AvgProductivityKGhForPressSMS1, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
            }
        }
        public decimal AvgScrapRateForPressSMS1 { get; set; }
        public string AvgScrapRateForPressSMS1_RoundNotFormatted
        {
            get
            {
                return Math.Round(this.AvgScrapRateForPressSMS1, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted();
            }
        }
        public string AvgScrapRateForPressSMS1_RoundFormatted
        {
            get
            {
                return Math.Round(this.AvgScrapRateForPressSMS1, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
            }
        }
        public decimal AvgProductivityKGhForPressSMS2 { get; set; }
        public string AvgProductivityKGhForPressSMS2_RoundNotFormatted
        {
            get
            {
                return Math.Round(this.AvgProductivityKGhForPressSMS2, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted();
            }
        }
        public string AvgProductivityKGhForPressSMS2_RoundFormatted
        {
            get
            {
                return Math.Round(this.AvgProductivityKGhForPressSMS2, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
            }
        }
        public decimal AvgScrapRateForPressSMS2 { get; set; }
        public string AvgScrapRateForPressSMS2_RoundNotFormatted
        {
            get
            {
                return Math.Round(this.AvgScrapRateForPressSMS2, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted();
            }
        }
        public string AvgScrapRateForPressSMS2_RoundFormatted
        {
            get
            {
                return Math.Round(this.AvgScrapRateForPressSMS2, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
            }
        }
        public decimal AvgProductivityKGhForPressBREDA { get; set; }
        public string AvgProductivityKGhForPressBREDA_RoundNotFormatted
        {
            get
            {
                return Math.Round(this.AvgProductivityKGhForPressBREDA, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted();
            }
        }
        public string AvgProductivityKGhForPressBREDA_RoundFormatted
        {
            get
            {
                return Math.Round(this.AvgProductivityKGhForPressBREDA, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
            }
        }
        public decimal AvgScrapRateForPressBREDA { get; set; }
        public string AvgScrapRateForPressBREDA_RoundNotFormatted
        {
            get
            {
                return Math.Round(this.AvgScrapRateForPressBREDA, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted();
            }
        }
        public string AvgScrapRateForPressBREDA_RoundFormatted
        {
            get
            {
                return Math.Round(this.AvgScrapRateForPressBREDA, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
            }
        }
        public decimal AvgProductivityKGhForPressFARREL { get; set; }
        public string AvgProductivityKGhForPressFARREL_RoundNotFormatted
        {
            get
            {
                return Math.Round(this.AvgProductivityKGhForPressFARREL, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted();
            }
        }
        public string AvgProductivityKGhForPressFARREL_RoundFormatted
        {
            get
            {
                return Math.Round(this.AvgProductivityKGhForPressFARREL, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
            }
        }
        public decimal AvgScrapRateForPressFARREL { get; set; }
        public string AvgScrapRateForPressFARREL_RoundNotFormatted
        {
            get
            {
                return Math.Round(this.AvgScrapRateForPressFARREL, 2, MidpointRounding.AwayFromZero).ToStringNotFormatted();
            }
        }
        public string AvgScrapRateForPressFARREL_RoundFormatted
        {
            get
            {
                return Math.Round(this.AvgScrapRateForPressFARREL, 2, MidpointRounding.AwayFromZero).ToStringFormatted();
            }
        }
        #endregion

        public string KeyValueIntCodeForPress { get; set; }
    }
}