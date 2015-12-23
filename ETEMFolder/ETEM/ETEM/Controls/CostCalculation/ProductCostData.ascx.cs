using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Helpers.CostCalculation;
using ETEMModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETEM.Controls.CostCalculation
{
    public partial class ProductCostData : BaseUserControl
    {
        Offer currentOffer;

        public void SetHdnField(string value)
        {
            this.hdnRowMasterKey.Value = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public override void UserControlLoad()
        {
            base.ClearResultContext(this.lbResultContext);

            this.tblProductCosts.Rows.Clear();

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

            ProductCostResult productCostResult = this.ownerPage.CostCalculationRef.LoadProductCostsInTableRowsByOfferId(this.hdnRowMasterKey.Value, this.ownerPage.CallContext);

            if (productCostResult.ResultContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {
                TableRow[] rows = productCostResult.TableRowsProductCosts;

                this.tblProductCosts.Rows.AddRange(rows);
            }
            else
            {
                base.AddErrorMessage(this.lbResultContext, productCostResult.ResultContext.Message);
            }
        }
    }
}