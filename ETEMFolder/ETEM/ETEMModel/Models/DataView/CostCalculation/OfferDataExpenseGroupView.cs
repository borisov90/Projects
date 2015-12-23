using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models.DataView.CostCalculation
{
    public class OfferDataExpenseGroupView : OfferDataExpenseGroup
    {
        public string CostCenterName    { get; set; }
        public string ExpensesTypeName  { get; set; }
    }
}