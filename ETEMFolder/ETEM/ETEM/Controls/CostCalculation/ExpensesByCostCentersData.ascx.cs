using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Models;
using ETEMModel.Models.DataView;
using ETEMModel.Models.DataView.CostCalculation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETEM.Controls.CostCalculation
{
    
    public partial class ExpensesByCostCentersData : BaseUserControl
    {
        private ETEMModel.Models.Offer currentEntity;

        public void SetHdnField(string value)
        {
            this.hdnRowMasterKey.Value = value;
        }

        public string ParentControlID
        {
            get { return this.hdnParentControlID.Value; }
            set { this.hdnParentControlID.Value = value; }
        }

        public OfferMainData ParentControl
        {
            get { return base.FindControlById(this.Page, this.ParentControlID) as OfferMainData; }
        }
       
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public override void UserControlLoad()
        {
            if (this.ownerPage == null)
            {
                throw new UMSException("Current Page is null or is not inheritor of BasicPage.");
            }

            if (!string.IsNullOrEmpty(this.hdnRowMasterKey.Value) && this.hdnRowMasterKey.Value != Constants.INVALID_ID_STRING)
            {
                this.CurrentEntityMasterID = this.hdnRowMasterKey.Value;
            }

            this.currentEntity = this.ownerPage.CostCalculationRef.GetOfferByID(this.CurrentEntityMasterID);

            if (this.currentEntity != null)
            {
                this.hdnRowMasterKey.Value = this.currentEntity.idOffer.ToString();

                LoadtbltblOfferDataExpenseGroup();

            }
            else
            {
                SetEmptyValues();
            }
            
        }

        private void LoadtbltblOfferDataExpenseGroup()
        {
            this.tblOfferDataExpenseGroup.Rows.Clear();
            //this.tblOfferDataExpenseGroup.DataSource = null;
            Dictionary<string, TableRow[]> dictCostCenterAndExpensesType    = new Dictionary<string, TableRow[]>();
            //DataTable dictCostCenterAndExpensesType = new DataTable();

            dictCostCenterAndExpensesType                                   = this.ownerPage.CostCalculationRef.LoadOfferDataCostCenterAndExpensesType(this.hdnRowMasterKey.Value, ETEMEnums.CalculationType.EUR_MH, this.ownerPage.CallContext);
            this.tblOfferDataExpenseGroup.Rows.AddRange(dictCostCenterAndExpensesType["Expenses"]);
            //this.tblOfferDataExpenseGroup.DataSource = dictCostCenterAndExpensesType;


            
            

        }

        private void SetEmptyValues()
        {

        }

        


    }
}