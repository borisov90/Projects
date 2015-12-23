﻿using ETEMModel.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models.DataView.CostCalculation
{
    public class MaterialPriceListDataView : MaterialPriceList, DataViewInterface
    {
        #region DataViewInterface Members
        public string IdEntity
        {
            get { return this.idMaterialPriceList.ToString(); }
            set { this.IdEntity = value; }
        }
        #endregion

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