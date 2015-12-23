using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models.DataView.CostCalculation
{
    public class CommissionsByAgentDataView : CommissionsByAgent, DataViewInterface
    {
        #region DataViewInterface Members
        public string IdEntity
        {
            get { return this.idCommissionsByAgent.ToString(); }
            set { this.IdEntity = value; }
        }
        #endregion

        public string AgentName { get; set; }
    }
}