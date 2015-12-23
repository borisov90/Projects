using ETEM.Freamwork;
using ETEMModel.Helpers;
using System;
using System.Linq;
using ETEMModel.Helpers.Extentions;
using ETEMModel.Models;


namespace ETEM.Controls.CostCalculation
{
    
    public partial class BilletScrapData : BaseUserControl
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

        private void SetEmptyValues()
        {
            ClearResultContext();

            this.tbxLME.Text = string.Empty;





           

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
            if (this.ownerPage == null)
            {
                throw new UMSException("Current Page is null or is not inheritor of BasicPage.");
            }

            InitLoadControls();

            SetEmptyValues();

            if (!string.IsNullOrEmpty(this.hdnRowMasterKey.Value) && this.hdnRowMasterKey.Value != Constants.INVALID_ID_STRING)
            {
                this.CurrentEntityMasterID = this.hdnRowMasterKey.Value;
            }

            this.currentEntity = this.ownerPage.CostCalculationRef.GetOfferByID(this.CurrentEntityMasterID);

            if (this.currentEntity != null)
            {
                this.tbxLME.Text = this.currentEntity.LME.ToStringNotFormatted();
                this.tbxPREMIUM.Text = this.currentEntity.PREMIUM.ToStringNotFormatted();
                this.tbxMaterial.Text = this.currentEntity.MaterialComputable.ToStringNotFormatted();
                this.tbxConsumptionRatio.Text = this.currentEntity.ConsumptionRatio.ToStringNotFormatted();
                this.tbxScrapValuePercent.Text = this.currentEntity.ScrapValuePercent.ToStringNotFormatted();
                this.tbxConsumption.Text = this.currentEntity.Consumption_EUR_TON_Computable.ToStringNotFormatted();
                this.tbxScrapValue.Text = this.currentEntity.ScrapValue_EUR_TON_Computable.ToStringNotFormatted();
                this.tbxNetConsumption.Text = this.currentEntity.NetConsumptionComputable.ToStringNotFormatted();
                this.tbxCostOfScrap.Text = this.currentEntity.CostOfScrap.ToStringNotFormatted();

                


                this.hdnRowMasterKey.Value = this.currentEntity.idOffer.ToString();

                ClearResultContext();
            }
            else
            {
                SetEmptyValues();
            }
        }

        public override Tuple<CallContext, string> UserControlSave()
        {
            this.currentEntity =  this.ownerPage.CostCalculationRef.GetOfferByID(this.CurrentEntityMasterID);

            if (this.currentEntity == null)
            {
                string falseResult = string.Format(BaseHelper.GetCaptionString("Entity_is_not_update_in_tab"), BaseHelper.GetCaptionString("UserMain_Data"));

                this.ownerPage.CallContext.ResultCode = ETEMEnums.ResultEnum.Error;
                this.ownerPage.CallContext.Message = falseResult;

                return new Tuple<CallContext, string>(this.ownerPage.CallContext, BaseHelper.GetCaptionString("BilletScrapData_Data"));
            }


           

            
            this.ownerPage.CallContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;

            this.ownerPage.CallContext = this.ownerPage.CostCalculationRef.OfferSave(this.currentEntity, this.ownerPage.CallContext);

            this.lbResultContext.Text = this.ownerPage.CallContext.Message;
            if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {
                this.hdnRowMasterKey.Value = this.ownerPage.CallContext.EntityID;
            }

            CheckIfResultIsSuccess();



            return new Tuple<CallContext, string>(this.ownerPage.CallContext, BaseHelper.GetCaptionString("OfferMain_Data"));
        }

        protected void tbxLifespan_TextChanged(object sender, EventArgs e)
        {

        }

    }
}