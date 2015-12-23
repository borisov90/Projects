using ETEM.CostCalculation;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETEM.Controls.CostCalculation
{
    public partial class SAPDataQuantityData : BaseUserControl
    {
        private SAPDataQuantity currentEntity;

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
            this.ddlQuantityType.SelectedValue = Constants.INVALID_ID_STRING;

            this.tbxValueData.Text = string.Empty;

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

            this.currentEntity = this.ownerPage.CostCalculationRef.GetSAPDataQuantityById(this.CurrentEntityMasterID);

            if (this.currentEntity != null)
            {
                this.SetHdnField(this.currentEntity.idSAPDataQuantity.ToString());

                this.tbxDateFrom.SetTxbDateTimeValue(this.currentEntity.SAPData.DateFrom);
                this.tbxDateTo.SetTxbDateTimeValue(this.currentEntity.SAPData.DateTo);
                this.tbxStatus.Text = this.currentEntity.SAPData.Status;

                BaseHelper.CheckAndSetSelectedValue(this.ddlCostCenter.DropDownListCTRL, this.currentEntity.idCostCenter.ToString(), false);
                BaseHelper.CheckAndSetSelectedValue(this.ddlQuantityType.DropDownListCTRL, this.currentEntity.idQuantityType.ToString(), false);

                this.tbxValueData.Text = this.currentEntity.ValueDataRoundString;

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
            this.ddlQuantityType.UserControlLoad();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.SAPDataQuantitySave, false))
            {
                return;
            }

            
            if (string.IsNullOrEmpty(this.hdnRowMasterKey.Value) || this.hdnRowMasterKey.Value == Constants.INVALID_ID_STRING)
            {
                this.currentEntity = new SAPDataQuantity();
            }
            else
            {
                this.currentEntity = this.ownerPage.CostCalculationRef.GetSAPDataQuantityById(this.hdnRowMasterKey.Value);

                if (this.currentEntity == null)
                {
                    this.ownerPage.CallContext.ResultCode = ETEMEnums.ResultEnum.Error;

                    base.AddMessage(this.lbResultContext, string.Format("Entity `SAPDataQuantity` not found by ID ({0})!", this.hdnRowMasterKey.Value));

                    return;
                }

                
            }

            this.currentEntity.idCostCenter = this.ddlCostCenter.SelectedValueINT;
            this.currentEntity.idQuantityType = this.ddlQuantityType.SelectedValueINT;
            this.currentEntity.ValueData = BaseHelper.ConvertToDecimalOrMinValue(this.tbxValueData.Text.Trim());

            this.ownerPage.CallContext = this.ownerPage.CostCalculationRef.SAPDataQuantitySave(new List<SAPDataQuantity>() { this.currentEntity }, this.ownerPage.CallContext);

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

            if (this.ownerPage is SAPDataQuantitiesList)
            {
                ((SAPDataQuantitiesList)this.ownerPage).LoadFilteredList();
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