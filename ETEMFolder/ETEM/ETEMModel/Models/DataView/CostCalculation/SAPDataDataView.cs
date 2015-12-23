using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models.DataView.CostCalculation
{
    public class SAPDataDataView : SAPData, DataViewInterface
    {
        #region DataViewInterface Members
        public string IdEntity
        {
            get { return this.idSAPData.ToString(); }
            set { this.IdEntity = value; }
        }
        #endregion
    }
}