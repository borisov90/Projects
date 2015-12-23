using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Helpers.CostCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETEM.Controls.CostCalculation
{
    public partial class OfferOverviewData : BaseUserControl
    {
        private ETEMModel.Models.Offer currentOffer;

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

        private void SetEmptyValues()
        {
            this.ClearResultContext();

            
                      

            this.hdnRowMasterKey.Value = string.Empty;
        }

        private void InitLoadControls()
        {
            //this.ddlAging.UserControlLoad();
            
            
        }

        private void CheckIfResultIsSuccess()
        {
            if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {
                this.lbResultContext.Attributes.Add("class", "alert alert-success");
            }
            else
            {
                this.lbResultContext.Attributes.Add("class", "alert alert-error");
            }
        }

        public override void ClearResultContext()
        {
            this.lbResultContext.Text = string.Empty;
            this.lbResultContext.Attributes.Clear();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void UserControlLoad()
        {
            base.ClearResultContext(this.lbResultContext);

            this.tblOverviewCostTable.Rows.Clear();
            this.tblOverviewSalesPriceTable.Rows.Clear();
            this.tblOverviewEtemCalculations.Rows.Clear();
            this.tblOverviewSummaryExpenses.Rows.Clear();
            

            if (!string.IsNullOrEmpty(this.hdnRowMasterKey.Value) && this.hdnRowMasterKey.Value != Constants.INVALID_ID_ZERO_STRING)
            {
                this.CurrentEntityMasterID = this.hdnRowMasterKey.Value;
            }

            this.currentOffer = this.ownerPage.CostCalculationRef.GetOfferByID(this.hdnRowMasterKey.Value);

            if (this.currentOffer == null)
            {
                this.ownerPage.CallContext.ResultCode = ETEMEnums.ResultEnum.Error;

                base.AddMessage(this.lbResultContext, string.Format("Entity `Offer` not found by ID ({0})!", this.hdnRowMasterKey.Value));

                return;
            }

            OfferOverviewResult  offerOverviewResult = this.ownerPage.CostCalculationRef.LoadOfferOverviewResultInTableRowsByOfferId(this.hdnRowMasterKey.Value, this.ownerPage.CallContext);

            if (offerOverviewResult.ResultContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {
                

                this.tblOverviewCostTable.Rows.AddRange(offerOverviewResult.OfferOverviewCostTableRows);
                this.tblOverviewSalesPriceTable.Rows.AddRange(offerOverviewResult.OfferOverviewSalesPriceTableRows);
                this.tblOverviewEtemCalculations.Rows.AddRange(offerOverviewResult.OfferOverviewEtemCalculationsTableRows);
                this.tblOverviewSummaryExpenses.Rows.AddRange(offerOverviewResult.OfferOverviewSummaryExpensesTableRows);
                
            }
            else
            {
                base.AddErrorMessage(this.lbResultContext, offerOverviewResult.ResultContext.Message);
            }
        }
    }
}