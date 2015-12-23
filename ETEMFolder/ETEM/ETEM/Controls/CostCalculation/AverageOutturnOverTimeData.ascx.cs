using ETEM.Freamwork;
using ETEMModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEMModel.Helpers.Extentions;
using ETEMModel.Helpers;
using ETEM.CostCalculation;

namespace ETEM.Controls.CostCalculation
{
    
    public partial class AverageOutturnOverTimeData : BaseUserControl
    {
        private AverageOutturnOverTime currentEntity;

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

            this.tbxValueOfPressSMS1.Text = string.Empty;
            this.tbxValueOfPressSMS2.Text = string.Empty;

            this.tbxValueOfPressBREDA.Text = string.Empty;
            this.tbxValueOfPressFARREL.Text = string.Empty;

            this.tbxDateFrom.Text = string.Empty;
            this.tbxDateTo.Text = string.Empty;

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

            this.currentEntity = this.ownerPage.CostCalculationRef.GetAverageOutturnOverTimeById(this.CurrentEntityMasterID);

            if (this.currentEntity != null)
            {
                this.SetHdnField(this.currentEntity.idAverageOutturnOverTime.ToString());

                this.tbxValueOfPressSMS1.Text = this.currentEntity.ValueOfPressSMS1.ToStringNotFormatted();
                this.tbxValueOfPressSMS2.Text = this.currentEntity.ValueOfPressSMS2.ToStringNotFormatted();
                this.tbxValueOfPressBREDA.Text = this.currentEntity.ValueOfPressBREDA.ToStringNotFormatted();
                this.tbxValueOfPressFARREL.Text = this.currentEntity.ValueOfPressFARREL.ToStringNotFormatted();

                this.tbxDateFrom.SetTxbDateTimeValue(this.currentEntity.DateFrom);
                this.tbxDateTo.SetTxbDateTimeValue(this.currentEntity.DateTo);

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
            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrEmpty(this.hdnRowMasterKey.Value) || this.hdnRowMasterKey.Value == Constants.INVALID_ID_STRING)
            {
                this.currentEntity = new AverageOutturnOverTime();
            }
            else
            {
                this.currentEntity = this.ownerPage.CostCalculationRef.GetAverageOutturnOverTimeById(this.hdnRowMasterKey.Value);

                if (this.currentEntity == null)
                {
                    this.ownerPage.CallContext.ResultCode = ETEMEnums.ResultEnum.Error;

                    base.AddMessage(this.lbResultContext, string.Format("Entity `AverageOutturnOverTime` not found by ID ({0})!", this.hdnRowMasterKey.Value));

                    return;
                }

                
            }
            this.currentEntity.ValueOfPressSMS1 = BaseHelper.ConvertToDecimalOrMinValue(this.tbxValueOfPressSMS1.Text.Trim());
            this.currentEntity.ValueOfPressSMS2 = BaseHelper.ConvertToDecimalOrMinValue(this.tbxValueOfPressSMS2.Text.Trim());

            this.currentEntity.ValueOfPressBREDA = BaseHelper.ConvertToDecimalOrMinValue(this.tbxValueOfPressBREDA.Text.Trim());
            this.currentEntity.ValueOfPressFARREL = BaseHelper.ConvertToDecimalOrMinValue(this.tbxValueOfPressFARREL.Text.Trim());
            

            this.currentEntity.DateFrom = this.tbxDateFrom.TextAsDateParseExactOrMinValue;
            this.currentEntity.DateTo = this.tbxDateTo.TextAsDateParseExact;

            this.ownerPage.CallContext = this.ownerPage.CostCalculationRef.AverageOutturnOverTimeSave(new List<AverageOutturnOverTime>() { this.currentEntity }, this.ownerPage.CallContext);

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

            if (this.ownerPage is AverageOutturnOverTimeList)
            {
                ((AverageOutturnOverTimeList)this.ownerPage).LoadFilteredList();
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