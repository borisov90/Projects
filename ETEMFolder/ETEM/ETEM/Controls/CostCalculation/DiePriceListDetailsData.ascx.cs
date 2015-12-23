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
    public partial class DiePriceListDetailsData : BaseUserControl
    {
        private DiePriceListDetail currentEntity;

        public void SetHdnField(string value)
        {
            this.hdnRowMasterKey.Value = value;
        }

        public int DiePriceListID
        {
            get
            {
                int tmpValue = Constants.INVALID_ID;
                if (int.TryParse(this.hdnDiePriceListID.Value, out tmpValue))
                {
                    return tmpValue;
                }
                else
                {
                    return Constants.INVALID_ID;
                }
            }
            set { this.hdnDiePriceListID.Value = value.ToString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void SetEmptyValues()
        {
            this.lbResultContext.Attributes.Remove("class");
            this.lbResultContext.Text = string.Empty;

            this.ddlVendor.SelectedValue = Constants.INVALID_ID_STRING;
            this.ddlNumberOfCavities.SelectedValue = Constants.INVALID_ID_STRING;
            this.ddlProfileCategory.SelectedValue = Constants.INVALID_ID_STRING;
            this.ddlProfileComplexity.SelectedValue = Constants.INVALID_ID_STRING;
            this.tbxDimensionA.Text = string.Empty;
            this.tbxDimensionB.Text = string.Empty;
            this.tbxDiePrice.Text = string.Empty;
            this.tbxLifespan.Text = string.Empty;

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

            this.currentEntity = this.ownerPage.CostCalculationRef.GetDiePriceListDetailById(this.CurrentEntityMasterID);

            if (this.currentEntity != null)
            {
                this.SetHdnField(this.currentEntity.idDiePriceListDetail.ToString());

                BaseHelper.CheckAndSetSelectedValue(this.ddlVendor.DropDownListCTRL, this.currentEntity.idDiePriceList.ToString(), false);
                BaseHelper.CheckAndSetSelectedValue(this.ddlNumberOfCavities.DropDownListCTRL, this.currentEntity.idNumberOfCavities.ToString(), false);
                BaseHelper.CheckAndSetSelectedValue(this.ddlProfileCategory.DropDownListCTRL, this.currentEntity.idProfileCategory.ToString(), false);
                BaseHelper.CheckAndSetSelectedValue(this.ddlProfileComplexity.DropDownListCTRL, this.currentEntity.idProfileComplexity.ToString(), false);
                this.tbxDimensionA.Text = this.currentEntity.DimensionA_String;
                this.tbxDimensionB.Text = this.currentEntity.DimensionB_String;
                this.tbxDiePrice.Text = this.currentEntity.PriceRoundString;
                this.tbxLifespan.Text = this.currentEntity.LifespanRoundString;

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
            if (!string.IsNullOrWhiteSpace(this.hdnRowMasterKey.Value) && this.hdnRowMasterKey.Value != Constants.INVALID_ID_STRING &&
                this.hdnRowMasterKey.Value != Constants.INVALID_ID_ZERO_STRING)
            {
                this.ddlVendor.DropDownEnabled = false;
                this.ddlVendor.AdditionalParam = string.Empty;
                this.ddlVendor.UserControlLoad();
            }
            else
            {
                this.ddlVendor.DropDownEnabled = true;
                this.ddlVendor.AdditionalParam = DateTime.Today.ToString(Constants.SHORT_DATE_PATTERN);
                this.ddlVendor.UserControlLoad();
            }
            this.ddlNumberOfCavities.UserControlLoad();
            this.ddlProfileCategory.UserControlLoad();
            this.ddlProfileComplexity.UserControlLoad();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.DiePriceListDetailsSave, false))
            {
                return;
            }

            
            if (string.IsNullOrEmpty(this.hdnRowMasterKey.Value) || this.hdnRowMasterKey.Value == Constants.INVALID_ID_STRING)
            {
                this.currentEntity = new DiePriceListDetail();

                this.currentEntity.idDiePriceList = this.ddlVendor.SelectedValueINT;
            }
            else
            {
                this.currentEntity = this.ownerPage.CostCalculationRef.GetDiePriceListDetailById(this.hdnRowMasterKey.Value);

                if (this.currentEntity == null)
                {
                    this.ownerPage.CallContext.ResultCode = ETEMEnums.ResultEnum.Error;

                    base.AddMessage(this.lbResultContext, string.Format("Entity `DiePriceListDetail` not found by ID ({0})!", this.hdnRowMasterKey.Value));

                    return;
                }

               
            }

            this.currentEntity.idNumberOfCavities = this.ddlNumberOfCavities.SelectedValueINT;
            this.currentEntity.idProfileCategory = this.ddlProfileCategory.SelectedValueINT;
            this.currentEntity.idProfileComplexity = this.ddlProfileComplexity.SelectedValueINT;
            this.currentEntity.DimensionA = BaseHelper.ConvertToIntOrMinValue(this.tbxDimensionA.Text.Trim());
            this.currentEntity.DimensionB = BaseHelper.ConvertToIntOrMinValue(this.tbxDimensionB.Text.Trim());
            this.currentEntity.Price = BaseHelper.ConvertToDecimalOrMinValue(this.tbxDiePrice.Text.Trim());
            this.currentEntity.Lifespan = BaseHelper.ConvertToDecimalOrMinValue(this.tbxLifespan.Text.Trim());

            this.ownerPage.CallContext = this.ownerPage.CostCalculationRef.DiePriceListDetailsSave(new List<DiePriceListDetail>() { this.currentEntity }, this.ownerPage.CallContext);

            if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {
                this.hdnRowMasterKey.Value = this.ownerPage.CallContext.EntityID;

                this.lbResultContext.Text = this.ownerPage.CallContext.Message;
                
                base.AddMessage(this.lbResultContext, this.ownerPage.CallContext.Message);
            }
            else
            {
                if (!ShowErrors(new List<CallContext>() { this.ownerPage.CallContext }))
                {
                    return;
                }
            }

            if (this.ownerPage is DiePriceListDetailsList)
            {
                ((DiePriceListDetailsList)this.ownerPage).LoadFilteredList();
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