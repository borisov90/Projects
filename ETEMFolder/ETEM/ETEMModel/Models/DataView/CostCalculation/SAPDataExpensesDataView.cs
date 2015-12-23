using ETEMModel.Helpers.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models.DataView.CostCalculation
{
    public class SAPDataExpensesDataView : SAPDataExpense, DataViewInterface
    {
        #region DataViewInterface Members
        public string IdEntity
        {
            get { return this.idSAPDataExpenses.ToString(); }
            set { this.IdEntity = value; }
        }
        #endregion

        public string ValueDataString
        {
            get
            {
                return this.ValueData.ToStringNotFormatted();
            }
        }
        public string ValueDataFormatted
        {
            get
            {
                return this.ValueData.ToStringFormatted();
            }
        }
    }
}