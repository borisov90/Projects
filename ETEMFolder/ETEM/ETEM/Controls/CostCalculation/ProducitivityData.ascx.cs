using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Models;
using System;
using System.Linq;
using ETEMModel.Helpers.Extentions;
using System.Web.UI.WebControls;


namespace ETEM.Controls.CostCalculation
{
    
   
    public partial class ProducitivityData : BaseUserControl
    {
        private ETEMModel.Models.OfferProducitivity currentOfferProducitivity;
        private ETEMModel.Models.Offer              currentOffer;

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

            this.tbxPressProducitivity_KG_MH.Text       = string.Empty;
            this.tbxPressProducitivity_TON_MH.Text      = string.Empty;
            this.tbxQCProducitivity_KG_MH.Text          = string.Empty;
            this.tbxQCProducitivity_TON_MH.Text         = string.Empty;
            this.tbxCOMetalProducitivity_KG_MH.Text     = string.Empty;
            this.tbxCOMetalProducitivity_TON_MH.Text    = string.Empty;
            this.tbxPackagingProducitivity_KG_MH.Text   = string.Empty;
            this.tbxPackagingProducitivity_TON_MH.Text  = string.Empty;
       
                      

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

            this.currentOffer = this.ownerPage.CostCalculationRef.GetOfferByID(this.CurrentEntityMasterID);

            this.currentOfferProducitivity = this.ownerPage.CostCalculationRef.GetOfferProducitivityByOfferID(this.CurrentEntityMasterID);



            if (this.currentOffer != null)
            {
                this.hdnRowMasterKey.Value = this.currentOffer.idOffer.ToString();

                ClearResultContext();
            }


            if (this.currentOfferProducitivity != null)
            {
                KeyValue kvPress    = this.ownerPage.GetKeyValueByID(this.currentOfferProducitivity.idPress);

                if (kvPress != null)
                {
                    this.lbPress.Text = kvPress.Name;
                }


                if (this.currentOfferProducitivity.idCOMetal.HasValue)
                {
                    KeyValue kvCOMetal = this.ownerPage.GetKeyValueByID(this.currentOfferProducitivity.idCOMetal.Value);

                    if (kvCOMetal != null)
                    {
                        this.lbCoMetal.Text = kvCOMetal.Name;
                    }
                   

                    foreach(TableRow row in this.gvProducitivity.Rows)
                    {
                        row.Cells[3].Visible = true;
                    }
                }
                else
                {
                    foreach(TableRow row in this.gvProducitivity.Rows)
                    {
                        row.Cells[3].Visible = false;
                    }
                }



                this.tbxPressProducitivity_KG_MH.Text       = this.currentOfferProducitivity.PressProducitivity_KG_MH.ToStringNotFormatted();
                this.tbxPressProducitivity_TON_MH.Text      = this.currentOfferProducitivity.PressProducitivity_TON_MH.ToStringFormatted4();
                this.tbxQCProducitivity_KG_MH.Text          = this.currentOfferProducitivity.QCProducitivity_KG_MH.ToStringNotFormatted();
                this.tbxQCProducitivity_TON_MH.Text         = this.currentOfferProducitivity.QCProducitivity_TON_MH.ToStringFormatted4();
                this.tbxCOMetalProducitivity_KG_MH.Text     = this.currentOfferProducitivity.COMetalProducitivity_KG_MH.ToStringNotFormatted();
                this.tbxCOMetalProducitivity_TON_MH.Text    = this.currentOfferProducitivity.COMetalProducitivity_TON_MH.ToStringFormatted4();
                this.tbxPackagingProducitivity_KG_MH.Text   = this.currentOfferProducitivity.PackagingProducitivity_KG_MH.ToStringNotFormatted();
                this.tbxPackagingProducitivity_TON_MH.Text  = this.currentOfferProducitivity.PackagingProducitivity_TON_MH.ToStringFormatted4();
                
                ClearResultContext();
            }
            else
            {
                SetEmptyValues();
            }
            
        }

        public override Tuple<CallContext, string> UserControlSave()
        {
     
            


            OfferProducitivity  offerProducitivity = this.ownerPage.CostCalculationRef.GetOfferProducitivityByOfferID(this.CurrentEntityMasterID);



            this.ownerPage.CallContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;

            if (offerProducitivity != null)
            {
                KeyValue kvSelectPress, kvPackaging;

                kvSelectPress = this.ownerPage.GetKeyValueByID(offerProducitivity.idPress);
                kvPackaging = this.ownerPage.GetKeyValueByIntCode("CostCenter", "Packaging");



                offerProducitivity.PressProducitivity_KG_MH     = BaseHelper.ConvertToDecimalOrZero(this.tbxPressProducitivity_KG_MH.Text);

                 
                if (offerProducitivity.PressProducitivity_KG_MH > BaseHelper.ConvertToDecimalOrZero(kvSelectPress.DefaultValue3))
                {
                    offerProducitivity.PressProducitivity_KG_MH = BaseHelper.ConvertToDecimalOrZero(kvSelectPress.DefaultValue3);

                    this.tbxPressProducitivity_KG_MH.Text = offerProducitivity.PressProducitivity_KG_MH.ToStringNotFormatted();
                }


                offerProducitivity.PressProducitivity_TON_MH    = offerProducitivity.PressProducitivity_TON_MH_Computable;

                offerProducitivity.COMetalProducitivity_KG_MH   = offerProducitivity.PressProducitivity_KG_MH;//Productivity for QC and COMETAL is equal to productivity for press
                offerProducitivity.COMetalProducitivity_TON_MH  = offerProducitivity.COMetalProducitivity_TON_MH_Computable;

                offerProducitivity.QCProducitivity_KG_MH        = offerProducitivity.PressProducitivity_KG_MH;//Productivity for QC and COMETAL is equal to productivity for press
                offerProducitivity.QCProducitivity_TON_MH       = offerProducitivity.QCProducitivity_TON_MH_Computable;

                offerProducitivity.PackagingProducitivity_KG_MH = BaseHelper.ConvertToDecimalOrZero(this.tbxPackagingProducitivity_KG_MH.Text);

                if (offerProducitivity.PackagingProducitivity_KG_MH > BaseHelper.ConvertToDecimalOrZero(kvPackaging.DefaultValue3))
                {
                    offerProducitivity.PackagingProducitivity_KG_MH = BaseHelper.ConvertToDecimalOrZero(kvPackaging.DefaultValue3);

                    this.tbxPackagingProducitivity_KG_MH.Text = offerProducitivity.PackagingProducitivity_KG_MH.ToStringNotFormatted();;
                }


                offerProducitivity.PackagingProducitivity_TON_MH = offerProducitivity.PackagingProducitivity_TON_MH_Computable;

                

                
                    
                this.ownerPage.CallContext = this.ownerPage.CostCalculationRef.OfferProducitivitySave(offerProducitivity, this.ownerPage.CallContext);
            }
            
            

           

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