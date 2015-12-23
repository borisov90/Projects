using ETEM.CostCalculation;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Models;
using ETEMModel.Helpers.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETEM.Controls.CostCalculation
{
    public partial class ProductivityAndScrapDetailData : BaseUserControl
    {
        private ProductivityAndScrapDetail currentEntity;

        public void SetHdnField(string value)
        {
            this.hdnRowMasterKey.Value = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void SetEmptyValues()
        {
            this.lbResultContext.Attributes.Remove("class");
            this.lbResultContext.Text = string.Empty;

            this.tbxDateFrom.Text = string.Empty;            
            this.tbxDateTo.Text = string.Empty;
            this.tbxStatus.Text = string.Empty;

            this.ddlCostCenter.SelectedValue = Constants.INVALID_ID_STRING;
            this.ddlProfileSetting.SelectedValue = Constants.INVALID_ID_STRING;

            this.tbxSumOfHours.Text = string.Empty;
            this.tbxSumOfConsumption.Text = string.Empty;
            this.tbxSumOfProduction.Text = string.Empty;

            this.tbxProductivityKGh.Text = string.Empty;
            this.tbxScrapRate.Text = string.Empty;

            this.hdnRowMasterKey.Value = string.Empty;
        }

        public override void UserControlLoad()
        {
            SetEmptyValues();
            base.ClearResultContext(this.lbResultContext);

            if (this.ownerPage == null)
            {
                throw new UMSException("Current Page is null or is not inheritor of BasicPage.");
            }

            if (!string.IsNullOrEmpty(this.hdnRowMasterKey.Value) && this.hdnRowMasterKey.Value != Constants.INVALID_ID_ZERO_STRING)
            {
                this.CurrentEntityMasterID = this.hdnRowMasterKey.Value;
            }

            InitLoadControls();

            this.currentEntity = this.ownerPage.CostCalculationRef.GetProductivityAndScrapDetailById(this.CurrentEntityMasterID);

            if (this.currentEntity != null)
            {
                this.SetHdnField(this.currentEntity.idProductivityAndScrapDetail.ToString());

                this.tbxDateFrom.SetTxbDateTimeValue(this.currentEntity.ProductivityAndScrap.DateFrom);
                this.tbxDateTo.SetTxbDateTimeValue(this.currentEntity.ProductivityAndScrap.DateTo);
                this.tbxStatus.Text = this.currentEntity.ProductivityAndScrap.Status;

                BaseHelper.CheckAndSetSelectedValue(this.ddlCostCenter.DropDownListCTRL, this.currentEntity.idCostCenter.ToString(), false);
                BaseHelper.CheckAndSetSelectedValue(this.ddlProfileSetting.DropDownListCTRL, this.currentEntity.idProfileSetting.ToString(), false);

                this.tbxSumOfHours.Text = this.currentEntity.SumOfHours_RoundString;
                this.tbxSumOfConsumption.Text = this.currentEntity.SumOfConsumption_RoundString;
                this.tbxSumOfProduction.Text = this.currentEntity.SumOfProduction_RoundString;

                this.tbxProductivityKGh.Text = this.currentEntity.ProductivityKGh_RoundString;
                this.tbxScrapRate.Text = this.currentEntity.ScrapRatePercent_RoundString;
                
                base.ClearResultContext(this.lbResultContext);
            }
            else
            {
                SetEmptyValues();
            }

            this.pnlFormData.Visible = true;
            this.pnlFormData.Focus();
        }

        private void InitLoadControls()
        {
            this.ddlCostCenter.UserControlLoad();
            this.ddlProfileSetting.UserControlLoad();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.ProductivityAndScrapSave, false))
            {
                return;
            }

            bool isNew = true;
            if (string.IsNullOrEmpty(this.hdnRowMasterKey.Value) || this.hdnRowMasterKey.Value == Constants.INVALID_ID_STRING)
            {
                this.currentEntity = new ProductivityAndScrapDetail();
            }
            else
            {
                this.currentEntity = this.ownerPage.CostCalculationRef.GetProductivityAndScrapDetailById(this.hdnRowMasterKey.Value);

                if (this.currentEntity == null)
                {
                    this.ownerPage.CallContext.ResultCode = ETEMEnums.ResultEnum.Error;

                    base.AddMessage(this.lbResultContext, string.Format("Entity `ProductivityAndScrapDetail` not found by ID ({0})!", this.hdnRowMasterKey.Value));

                    return;
                }

                isNew = false;
            }

            this.currentEntity.idCostCenter = this.ddlCostCenter.SelectedValueINT;
            this.currentEntity.idProfileSetting = this.ddlProfileSetting.SelectedValueINT;
            this.currentEntity.SumOfHours = BaseHelper.ConvertToDecimalOrMinValue(this.tbxSumOfHours.Text.Trim());
            this.currentEntity.SumOfConsumption = BaseHelper.ConvertToDecimalOrMinValue(this.tbxSumOfConsumption.Text.Trim());
            this.currentEntity.SumOfProduction = BaseHelper.ConvertToDecimalOrMinValue(this.tbxSumOfProduction.Text.Trim());

            decimal productivityKGh = decimal.Zero;
            decimal scrapRate = decimal.Zero;

            if (this.currentEntity.SumOfHours.HasValue && this.currentEntity.SumOfHours.Value != 0 &&
                this.currentEntity.SumOfProduction.HasValue)
            {
                productivityKGh = Math.Round((this.currentEntity.SumOfProduction.Value / this.currentEntity.SumOfHours.Value), 9, MidpointRounding.AwayFromZero);

                this.currentEntity.ProductivityKGh = productivityKGh;

                this.tbxProductivityKGh.Text = this.currentEntity.ProductivityKGh_RoundString;
            }
            if (this.currentEntity.SumOfConsumption.HasValue && this.currentEntity.SumOfConsumption.Value != 0 &&
                this.currentEntity.SumOfProduction.HasValue)
            {
                scrapRate = Math.Round(((this.currentEntity.SumOfConsumption.Value - this.currentEntity.SumOfProduction.Value) / this.currentEntity.SumOfConsumption.Value), 9, MidpointRounding.AwayFromZero);

                this.currentEntity.ScrapRate = scrapRate;

                this.tbxScrapRate.Text = this.currentEntity.ScrapRate_RoundString;
            }

            this.ownerPage.CallContext = this.ownerPage.CostCalculationRef.ProductivityAndScrapDetailSave(new List<ProductivityAndScrapDetail>() { this.currentEntity }, this.ownerPage.CallContext);

            if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {
                this.hdnRowMasterKey.Value = this.ownerPage.CallContext.EntityID;

                base.AddMessage(this.lbResultContext, this.ownerPage.CallContext.Message);
            }
            else
            {
                if (!ShowErrors(new List<CallContext>() { this.ownerPage.CallContext }))
                {
                    return;
                }
            }

            if (this.ownerPage is ProductivityAndScrapDetailList)
            {
                ((ProductivityAndScrapDetailList)this.ownerPage).LoadFilteredList();
            }
        }

        protected void btnCancelErorrs_Click(object sender, EventArgs e)
        {
            this.blEroorsSave.Items.Clear();
            this.pnlErrors.Visible = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            base.ClearResultContext(this.lbResultContext);
            this.pnlFormData.Visible = false;
        }

        private bool ShowErrors(List<CallContext> listCallContext)
        {
            bool result = true;

            List<string> listErrors = new List<string>();

            foreach (var item in listCallContext)
            {
                if (item.ResultCode == ETEMEnums.ResultEnum.Error)
                {
                    string[] currentItemErrors = item.Message.Split(new string[] { Constants.ERROR_MESSAGES_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < currentItemErrors.Length; i++)
                    {
                        currentItemErrors[i] = currentItemErrors[i];
                    }
                    listErrors.AddRange(currentItemErrors);
                }
                else if (item.ResultCode == ETEMEnums.ResultEnum.Warning)
                {
                    listErrors.Add(item.Message);
                }
            }
            if (listErrors.Count > 0)
            {
                this.blEroorsSave.Items.Clear();

                foreach (var error in listErrors)
                {
                    var listItem = new ListItem(error);
                    listItem.Attributes.Add("class", "lbResultSaveError");
                    this.blEroorsSave.Items.Add(listItem);
                }

                this.pnlErrors.Visible = true;

                result = false;
            }

            return result;
        }
    }
}