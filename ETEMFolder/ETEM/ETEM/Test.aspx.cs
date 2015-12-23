using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEMModel.Helpers;
using ETEMModel.Helpers.CostCalculation;

namespace ETEM
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            EvaluateExpressionHelper eval = new EvaluateExpressionHelper();

            Dictionary<string, string> vals = new Dictionary<string, string>();


           

            if (!string.IsNullOrEmpty( this.tbxKey1.Text) )
                vals.Add(this.tbxKey1.Text, tbxValue1.Text);

            if (!string.IsNullOrEmpty(this.tbxKey2.Text))
                vals.Add(this.tbxKey2.Text, tbxValue2.Text);
            if (!string.IsNullOrEmpty(this.tbxKey3.Text))
                vals.Add(this.tbxKey3.Text, tbxValue3.Text);
            if (!string.IsNullOrEmpty(this.tbxKey4.Text))
                vals.Add(this.tbxKey4.Text, tbxValue4.Text);
            try
            {
                this.tbxResult.Text = eval.EvalExpression(this.tbxExpr.Text, vals).ToString();
                this.lbError.Text = string.Empty;
            }
            catch(Exception ex)
            {
                this.lbError.Text = ex.Message;
            }
        }

        protected void btnLoadSAPData_Click(object sender, EventArgs e)
        {
            ExpenseCalculationBL expenseCalculationBL = new ExpenseCalculationBL();

            List<Expense> list = expenseCalculationBL.LoadExpenseGroupByIdSAPData(Int32.Parse(this.tbxIDSAPData.Text));
        }
    }
}
