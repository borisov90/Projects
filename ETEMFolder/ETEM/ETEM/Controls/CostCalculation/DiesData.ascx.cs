using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Models;
using System;
using System.Linq;
using ETEMModel.Helpers.Extentions;


namespace ETEM.Controls.CostCalculation
{
    
    public partial class DiesData : BaseUserControl
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
            this.ClearResultContext();

            this.tbxCostOfDie.Text = string.Empty;
                      

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
                this.tbxCostOfDie.Text = this.currentEntity.CostOfDie.ToStringNotFormatted();
                this.tbxLifespan.Text = this.currentEntity.Lifespan.ToStringNotFormatted();
                this.tbxCostOfDieTon.Text = this.currentEntity.CostOfDieEUR_Per_TON_Computable.ToStringNotFormatted();


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
            

            if (string.IsNullOrEmpty(this.hdnRowMasterKey.Value) || this.hdnRowMasterKey.Value == Constants.INVALID_ID_STRING)
            {
                this.currentEntity = new Offer();
            }
            else
            {
                this.currentEntity =  this.ownerPage.CostCalculationRef.GetOfferByID(this.CurrentEntityMasterID);

                if (this.currentEntity == null)
                {
                    string falseResult = string.Format(BaseHelper.GetCaptionString("Entity_is_not_update_in_tab"), BaseHelper.GetCaptionString("UserMain_Data"));

                    this.ownerPage.CallContext.ResultCode = ETEMEnums.ResultEnum.Error;
                    this.ownerPage.CallContext.Message = falseResult;

                    return new Tuple<CallContext, string>(this.ownerPage.CallContext, BaseHelper.GetCaptionString("OfferMain_Data"));
                }

                
            }


            if (!string.IsNullOrEmpty(this.tbxLifespan.Text)) 
            {
                Decimal tmpLifespan;
                if(Decimal.TryParse(this.tbxLifespan.Text, out tmpLifespan))
                { 
                    this.currentEntity.Lifespan = tmpLifespan;
                    this.currentEntity.CostOfDie = currentEntity.DiePrice;

                }
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