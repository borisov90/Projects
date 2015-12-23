using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Models;
using System;
using System.Linq;
using ETEMModel.Helpers.Extentions;
using System.Collections.Generic;
using ETEMModel.Models.DataView.CostCalculation;
using ETEMModel.Helpers.AbstractSearchBLHolder;
using System.Web.UI.HtmlControls;
using ETEMModel.Helpers.CostCalculation;
using System.Web.UI.WebControls;
using MoreLinq;

namespace ETEM.Controls.CostCalculation
{
    
    public partial class InquiryData : BaseUserControl
    {
        private bool NeedReCalculation;
        private Offer currentEntity;
        private Offer currentEntityBeforeSave;
        private DiePriceListDetail diePriceListDetail;
        private DiePriceList diePriceList;
        private ProfileSetting profileSetting;
        private EvaluateExpressionHelper evalHelper = new EvaluateExpressionHelper();
        private Dictionary<string, string> primitives = new Dictionary<string, string>();
        private double tmpDiameter;
        private double tmpDieDiameter;

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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.tbxWeightPerPC.Attributes.Add("onchange", "var param = ['"+this.tbxWeightPerMeter.ClientID+"','"+tbxLengthOfFinalPC.ClientID+"'];  clearTextBoxValue('0',param)");

            this.tbxWeightPerMeter.Attributes.Add("onchange", "var param = ['"+this.tbxWeightPerPC.ClientID+"'];  clearTextBoxValue('0',param)");
            this.tbxLengthOfFinalPC.Attributes.Add("onchange", "var param = ['"+this.tbxWeightPerPC.ClientID+"'];  clearTextBoxValue('0',param)");
        }
        

        private void SetEmptyValues()
        {
            ClearResultContext();

            this.btnChoose.Enabled = true;

            this.tbxInquiryNumber.Text = string.Empty;
            this.tbxCustomer.Text = string.Empty;
            this.tbxOfferDate.SetTxbDateTimeValue(DateTime.Now);
            this.tbxProfileSettingName.Text = string.Empty;
            this.hdnIdProfileSetting.Value = string.Empty;
            



            this.tbxValueForA.Text = string.Empty;
            this.tbxValueForB.Text = string.Empty;
            this.tbxValueForC.Text = string.Empty;
            this.tbxValueForD.Text = string.Empty;
            this.tbxValueForS.Text = string.Empty;


            this.tbxPin.Text = string.Empty;
            this.tbxPout.Text = string.Empty;


            this.tbxValueForA.Enabled = false;
            this.tbxValueForB.Enabled = false;
            this.tbxValueForC.Enabled = false;
            this.tbxValueForD.Enabled = false;
            this.tbxValueForS.Enabled = false;

            this.tbxDiameter.Text = string.Empty;
            this.tbxDieDiameter.Text = string.Empty;
            this.tbxDieDimensions.Text = string.Empty;
            this.tbxPress.Text = string.Empty;
            this.tbxDiePrice.Text = string.Empty;
            this.tbxVendor.Text = string.Empty;
            this.tbxPriceListStr.Text = string.Empty;


            MaterialPriceListDataView material = this.ownerPage.CostCalculationRef.GetActiveMaterialPriceList(DateTime.Now);


            this.tbxLME.Text = material.LME.ToStringNotFormatted();
            this.tbxPREMIUM.Text = material.Premium.ToStringNotFormatted();
            this.tbxMaterial.Text = (material.LME + material.Premium).ToStringFormatted();
            this.tbxMaterialPriceList.Text = material.DateFromString + " - " + material.DateToString;
            this.hdnIdMaterialPriceList.Value = material.idMaterialPriceList.ToString();

            this.tbxLengthOfFinalPC.Text = Constants.INVALID_ID_ZERO_STRING;
            this.tbxWeightPerMeter.Text = Constants.INVALID_ID_ZERO_STRING;
            this.tbxWeightPerPC.Text = Constants.INVALID_ID_ZERO_STRING;
            this.tbxPcsForTheWholeProject.Text = Constants.INVALID_ID_ZERO_STRING;
            this.tbxTonnage.Text = Constants.INVALID_ID_ZERO_STRING;
            this.tbxTonnage_KG.Text = Constants.INVALID_ID_ZERO_STRING;
            this.tbxDaysOfCredit.Text = Constants.INVALID_ID_ZERO_STRING;
            this.tbxGrossMargin.Text = Constants.INVALID_ID_ZERO_STRING;
            this.tbxTransportationCost.Text = Constants.INVALID_ID_ZERO_STRING;
            this.tbxCommission.Text = Constants.INVALID_ID_ZERO_STRING;


            this.tbxAdministrationExpenses.Text = BasicPage.GetSettingByCode(ETEMEnums.AppSettings.Administration_expenses).SettingValue;
            this.tbxSalesExpenses.Text = BasicPage.GetSettingByCode(ETEMEnums.AppSettings.Sales_expenses).SettingValue;
            this.tbxFinancialFixedExpenses.Text = BasicPage.GetSettingByCode(ETEMEnums.AppSettings.Financial_fixed_expenses).SettingValue;
            this.tbxInterest.Text = BasicPage.GetSettingByCode(ETEMEnums.AppSettings.Interest).SettingValue;

            this.tbxSavingsRate.Text = BasicPage.GetSettingByCode(ETEMEnums.AppSettings.SavingsRate).SettingValue;
            


            this.tbxFinancialVariableExpenses.Text = string.Empty;
            this.tbxSGAsAndFin_EUR_TON.Text = string.Empty;


            this.tbxMaterial_EUR.Text = string.Empty;
            this.tbxTransportation_EUR.Text = string.Empty;
            this.tbxCommission_EUR.Text = string.Empty;
            this.tbxSGAsAndFin_EUR.Text = string.Empty;
            
            this.tbxTotalSalesPrice_EUR.Text = string.Empty;
            
            this.tbxActualTotalSalesPriceToCustomer.Text = string.Empty;
            this.tbxTargetPrice.Text = string.Empty;
            this.tbxTotalSalesPrice_EUR_TON.Text = string.Empty;

            this.tbxActualTotalSalesPriceToCustomer.Text = string.Empty;
            this.tbxActualTotalSalesPriceToCustomerAll.Text = string.Empty;

            this.tbxExtrusion_EUR_TON.Text = string.Empty;
            this.tbxExtrusion_EUR.Text = string.Empty;

            this.tbxPackaging_EUR_TON.Text = string.Empty;
            this.tbxPackaging_EUR.Text = string.Empty;

            this.ddlNumberOfCavities.SelectedValue = Constants.INVALID_ID_STRING;
            //this.ddlNumberOfCavities.DropDownEnabled = true;
            this.ddlAging.SelectedValue= this.ownerPage.GetKeyValueByIntCode("YES_NO","Yes").idKeyValue.ToString();
            this.ddlAging.DropDownEnabled = false;

            this.ddlStandardPackaging.SelectedValue= this.ownerPage.GetKeyValueByIntCode("YES_NO","Yes").idKeyValue.ToString();

            this.ddlCommision.SelectedValue = Constants.INVALID_ID_STRING;
            this.ddlAgent.SelectedValue = Constants.INVALID_ID_STRING;
            this.ddlCalculationCommission.SelectedValue = Constants.INVALID_ID_STRING;

            this.ddlHardness.SelectedValue = Constants.INVALID_ID_STRING;
            this.ddlPress.SelectedValue = Constants.INVALID_ID_STRING;


            this.hdnRowMasterKey.Value = string.Empty;


        }



        private void InitLoadControls()
        {
            this.ddlAging.UserControlLoad();
            this.ddlHardness.UserControlLoad();
            this.ddlStandardPackaging.UserControlLoad();

            this.ddlCommision.UserControlLoad();
            this.ddlAgent.UserControlLoad();
            this.ddlNumberOfCavities.UserControlLoad();
            this.ddlCalculationCommission.UserControlLoad();

            this.ddlPress.UserControlLoad();

            this.divGridValidation.Visible = false;
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

                //this.btnChoose.Enabled = false;

                this.currentEntity.LoadProductCostResult();
                this.currentEntity.LoadOfferOverviewResult();


                if (this.currentEntity.idDiePriceListDetail.HasValue)
                {
                    this.diePriceListDetail = this.ownerPage.CostCalculationRef.GetDiePriceListDetailById(this.currentEntity.idDiePriceListDetail.Value.ToString());
                    this.diePriceList = this.ownerPage.CostCalculationRef.GetDiePriceListById(this.diePriceListDetail.idDiePriceList.ToString());
                    this.tbxVendor.Text = this.ownerPage.GetKeyValueByID(this.diePriceList.idVendor).Name;
                    this.tbxPriceListStr.Text = diePriceList.DateFromString + "-" + diePriceList.DateToString;
                }


                
                this.tbxInquiryNumber.Text = this.currentEntity.InquiryNumber;
                this.tbxOfferDate.SetTxbDateTimeValue(this.currentEntity.OfferDate);
                //this.tbxOfferDate.ReadOnly = true;
                this.tbxCustomer.Text = this.currentEntity.Customer;
              

                this.tbxValueForA.Text = this.currentEntity.ValueForA.ToStringFormatted();
                this.tbxValueForB.Text = this.currentEntity.ValueForB.ToStringFormatted();
                this.tbxValueForC.Text = this.currentEntity.ValueForC.ToStringFormatted();
                this.tbxValueForD.Text = this.currentEntity.ValueForD.ToStringFormatted();
                this.tbxValueForS.Text = this.currentEntity.ValueForS.ToStringFormatted();

                this.tbxPin.Text = this.currentEntity.ValueForPin.ToStringNotFormatted();
                this.tbxPout.Text = this.currentEntity.ValueForPout.ToStringNotFormatted();

                this.tbxLME.Text        = this.currentEntity.LME.ToStringNotFormatted();
                this.tbxPREMIUM.Text    = this.currentEntity.PREMIUM.ToStringNotFormatted();
                this.tbxMaterial.Text   = this.currentEntity.MaterialComputable.ToStringFormatted();
                this.tbxMaterial_EUR.Text = this.currentEntity.Material_EUR_Computable.ToStringFormatted();
                

                if (this.currentEntity.idMaterialPriceList.HasValue)
                {
                    MaterialPriceList material = this.ownerPage.CostCalculationRef.GetMaterialPriceListById(this.currentEntity.idMaterialPriceList.ToString());
                    this.tbxMaterialPriceList.Text = material.DateFromString + " - " + material.DateToString;
                    this.hdnIdMaterialPriceList.Value = material.idMaterialPriceList.ToString();
                }

                ReloadDimensionCTRLByProfileSettingID(this.currentEntity.idProfileSetting.ToString());
                  



                this.tbxDiameter.Text = this.currentEntity.Diameter.ToStringNotFormatted();
                this.tbxDieDiameter.Text = this.currentEntity.DieDiameter.ToStringNotFormatted();
                this.tbxDieDimensions.Text = this.currentEntity.DieDimensions.ToStringNotFormatted();


                
                //OfferProducitivity  offerProducitivity = this.ownerPage.CostCalculationRef.GetOfferProducitivityByOfferID(this.CurrentEntityMasterID);


                //if(offerProducitivity != null)
                //{
                //    KeyValue kvPress = this.ownerPage.GetKeyValueByID(offerProducitivity.idPress);

                //    if(kvPress != null)
                //    {
                //        this.tbxPress.Text = kvPress.Name;
                //        this.ddlPress.SelectedValue = kvPress.idKeyValue.ToString();
                //    }
                //}
                
                this.tbxDiePrice.Text = this.currentEntity.DiePrice.ToStringNotFormatted();
                this.ddlPress.SelectedValue = this.currentEntity.idPress.ToString();

                this.tbxLengthOfFinalPC.Text = this.currentEntity.LengthOfFinalPC.ToStringNotFormatted();
                this.tbxWeightPerMeter.Text = this.currentEntity.WeightPerMeter.ToStringNotFormatted();
                this.tbxWeightPerPC.Text = this.currentEntity.WeightPerPC.ToStringNotFormatted();
                this.tbxDaysOfCredit.Text = this.currentEntity.DaysOfCredit.ToStringNotFormatted();
                this.tbxInterest.Text = this.currentEntity.Interest.ToStringNotFormatted();
                this.tbxGrossMargin.Text = this.currentEntity.GrossMargin.ToStringNotFormatted();
                this.tbxSavingsRate.Text = this.currentEntity.SavingsRate.ToStringNotFormatted();

                
                
                this.tbxPcsForTheWholeProject.Text = this.currentEntity.PcsForTheWholeProject.ToStringNotFormatted();


                this.ddlNumberOfCavities.SelectedValue = currentEntity.idNumberOfCavities.ToString();
                //this.ddlNumberOfCavities.DropDownEnabled = false;

                this.ddlAgent.SelectedValue = currentEntity.idAgent.ToString();
                this.ddlAging.SelectedValue = currentEntity.idAging.ToString();

                this.ddlHardness.SelectedValue = currentEntity.idHardness.ToString();
                this.ddlStandardPackaging.SelectedValue = currentEntity.idStandardPackaging.ToString();
                //this.ddlAging.DropDownEnabled = false;

                this.ddlCommision.SelectedValue = currentEntity.idCommision.ToString();
                this.ddlCalculationCommission.SelectedValue = currentEntity.idCalculationCommission.ToString();
                


                this.tbxExtrusion_EUR_TON.Text = currentEntity.Extrusion_EUR_TON_Computable.ToStringFormatted();
                this.tbxExtrusion_EUR.Text = currentEntity.Extrusion_EUR_Computable.ToStringFormatted();

                this.tbxPackaging_EUR_TON.Text = currentEntity.Packaging_EUR_TON_Computable.ToStringFormatted();
                this.tbxPackaging_EUR.Text = currentEntity.Packaging_EUR_Computable.ToStringFormatted();



                this.tbxTransportationCost.Text = this.currentEntity.TransportationCost.ToStringNotFormatted();
                this.tbxAdministrationExpenses.Text = this.currentEntity.AdministrationExpenses.ToStringNotFormatted();
                this.tbxSalesExpenses.Text = this.currentEntity.SalesExpenses.ToStringNotFormatted();
                this.tbxFinancialFixedExpenses.Text = this.currentEntity.FinancialFixedExpenses.ToStringNotFormatted();
                this.tbxFinancialVariableExpenses.Text = this.currentEntity.Financial_variable_Computable.ToStringNotFormatted();
                

                this.tbxSGAsAndFin_EUR_TON.Text = this.currentEntity.SGAsAndFin_EUR_TON_Computable.ToStringNotFormatted();
                this.tbxSGAsAndFin_EUR.Text = this.currentEntity.SGAsAndFin_EUR_Computable.ToStringFormatted();

                //

                this.tbxActualTotalSalesPriceToCustomer.Text = this.currentEntity.ActualTotalSalesPriceToCustomer.ToStringNotFormatted();

                this.tbxActualTotalSalesPriceToCustomerAll.Text = (this.currentEntity.ActualTotalSalesPriceToCustomer * this.currentEntity.Tonnage).ToStringFormatted();


                this.tbxTargetPrice.Text = this.currentEntity.TargetPrice_EUR_TON.ToStringNotFormatted();

                this.tbxTotalSalesPrice_EUR_TON.Text = this.currentEntity.TotalSalesPrice_EUR_TON_Computable.ToStringFormatted();

                this.tbxTotalSalesPrice_EUR.Text = this.currentEntity.TotalSalesPrice.ToStringFormatted();

                PresetData();

                ddlCommision_SelectedIndexChanged(null, null);
                ddlCalculationCommission_SelectedIndexChanged(null, null);

                this.hdnRowMasterKey.Value = this.currentEntity.idOffer.ToString();

                ClearResultContext();
            }
            else
            {
                SetEmptyValues();
            }
        }
 
        public void ReloadDimensionCTRLByProfileSettingID(string idProfileSetting)
        {
            ProfileSetting profileSetting = this.ownerPage.CostCalculationRef.GetProfileSettingById(idProfileSetting);

            this.gvSettingValidation.DataSource = this.ownerPage.CostCalculationRef.GetProfileSettingValidationByIDProfile(profileSetting.EntityID); ;
            this.gvSettingValidation.DataBind();

            //foreach(ProfileSettingValidation profSettVal in profileSettingValidation)
            //{

            //}
                
            if (profileSetting != null)
            {

                this.tbxProfileSettingName.Text = profileSetting.ProfileName;
                this.hdnIdProfileSetting.Value = profileSetting.idProfileSetting.ToString();

                if (profileSetting.hasA)
                {
                    this.tbxValueForA.Enabled   = true;
                    this.tbxValueForA.CssClass  += " mandatory";
                }
                else
                {
                    this.tbxValueForA.Enabled   = false;
                    this.tbxValueForA.Text      = string.Empty;
                }

                if (profileSetting.hasB)
                {
                    this.tbxValueForB.Enabled   = true;
                    this.tbxValueForB.CssClass  += " mandatory";
                }
                else
                {
                    this.tbxValueForB.Enabled   = false;
                    this.tbxValueForB.Text      = string.Empty;
                }

                if (profileSetting.hasC)
                {
                    this.tbxValueForC.Enabled   = true;
                    this.tbxValueForC.CssClass  += " mandatory";
                }
                else
                {
                    this.tbxValueForC.Enabled   = false;
                    this.tbxValueForC.Text      = string.Empty;
                }

                if (profileSetting.hasD)
                {
                    this.tbxValueForD.Enabled   = true;
                    this.tbxValueForD.CssClass  += " mandatory";
                }
                else
                {
                    this.tbxValueForD.Enabled   = false;
                    this.tbxValueForD.Text      = string.Empty;
                }

                if (profileSetting.hasS)
                {
                    this.tbxValueForS.Enabled   = true;
                    this.tbxValueForS.CssClass  += " mandatory";
                }
                else
                {
                    this.tbxValueForS.Enabled   = false;
                    this.tbxValueForS.Text      = string.Empty;
                }
            }

        }

        public override Tuple<CallContext, string> UserControlSave()
        {


            bool isNewOffer = false;

            if (string.IsNullOrEmpty(this.hdnRowMasterKey.Value) || this.hdnRowMasterKey.Value == Constants.INVALID_ID_STRING)
            {
                isNewOffer = true;
                this.currentEntity = new Offer();
                this.currentEntityBeforeSave = new Offer();
                this.currentEntity.idCreateUser = Int32.Parse(this.ownerPage.UserProps.IdUser);
                this.currentEntity.dCreate = DateTime.Now;
                
                this.NeedReCalculation = false;

                SetDefaultValues();
            }
            else
            {
                this.CurrentEntityMasterID = this.hdnRowMasterKey.Value;
                this.currentEntity =  this.ownerPage.CostCalculationRef.GetOfferByID(this.CurrentEntityMasterID);
                this.currentEntityBeforeSave = this.ownerPage.CostCalculationRef.GetOfferByID(this.CurrentEntityMasterID);
                
                if (this.currentEntity == null)
                {
                    string falseResult = string.Format(BaseHelper.GetCaptionString("Entity_is_not_update_in_tab"), BaseHelper.GetCaptionString("InquiryData_Data"));

                    this.ownerPage.CallContext.ResultCode = ETEMEnums.ResultEnum.Error;
                    this.ownerPage.CallContext.Message = falseResult;

                    return new Tuple<CallContext, string>(this.ownerPage.CallContext, BaseHelper.GetCaptionString("InquiryData_Data"));
                }
             
            }

            
            this.currentEntity.idModifyUser = Int32.Parse(this.ownerPage.UserProps.IdUser);
            this.currentEntity.dModify = DateTime.Now;

            this.currentEntity.Customer = this.tbxCustomer.Text.Trim();
            this.currentEntity.InquiryNumber = this.tbxInquiryNumber.Text.Trim();

            if (this.tbxOfferDate.TextAsDateParseExact.HasValue)
            {
                this.currentEntity.OfferDate = this.tbxOfferDate.TextAsDateParseExact.Value;
            }
            else
            {
                this.currentEntity.OfferDate = DateTime.MinValue;
            }

          
            this.currentEntity.WeightPerMeter = BaseHelper.ConvertToDecimalOrZero(this.tbxWeightPerMeter.Text);

            SetOfferDataDependOnWeightPerMeter(this.currentEntity);
            

            if (!ValidateRequiredField())
            {   
                return new Tuple<CallContext, string>(this.ownerPage.CallContext, string.Empty);
            }


            


            GeneralPage.LogDebug("this.currentEntity.OfferDate = " + this.currentEntity.OfferDate);


            #region ProfileSetting

            if (this.hdnIdProfileSetting.Value.IsNotNullOrEmpty())
            {
                this.currentEntity.idProfileSetting = Int32.Parse(this.hdnIdProfileSetting.Value);
            }
            else
            {
                string falseResult = "The field `Profile` is required!";

                this.ownerPage.CallContext.ResultCode = ETEMEnums.ResultEnum.Error;
                this.ownerPage.CallContext.Message = falseResult;

                return new Tuple<CallContext, string>(this.ownerPage.CallContext, BaseHelper.GetCaptionString("InquiryData_Data"));
            }

           



            if (this.currentEntity.idNumberOfCavities.HasValue)
            {
                //this.currentEntity.idNumberOfCavities = this.ddlNumberOfCavities.SelectedValueNullINT;
                this.ddlNumberOfCavities.SelectedValue = this.currentEntity.idNumberOfCavities.ToString();
            }
         
            

            if (this.tbxValueForA.Enabled && this.tbxValueForA.Text.IsNotNullOrEmpty())
            {
                this.currentEntity.ValueForA =  BaseHelper.ConvertToIntOrZero(this.tbxValueForA.Text.Trim());
            }

            if (this.tbxValueForB.Enabled && this.tbxValueForB.Text.IsNotNullOrEmpty())
            {
                this.currentEntity.ValueForB =  BaseHelper.ConvertToIntOrZero(this.tbxValueForB.Text.Trim());
            }

            if (this.tbxValueForC.Enabled && this.tbxValueForC.Text.IsNotNullOrEmpty())
            {
                this.currentEntity.ValueForC =  BaseHelper.ConvertToIntOrZero(this.tbxValueForC.Text.Trim());
            }

            if (this.tbxValueForD.Enabled && this.tbxValueForD.Text.IsNotNullOrEmpty())
            {
                this.currentEntity.ValueForD =  BaseHelper.ConvertToIntOrZero(this.tbxValueForD.Text.Trim());
            }

            if (this.tbxValueForS.Enabled && this.tbxValueForS.Text.IsNotNullOrEmpty())
            {
                this.currentEntity.ValueForS =  BaseHelper.ConvertToDecimalOrZero(this.tbxValueForS.Text.Trim());
            }

            this.currentEntity.Diameter = BaseHelper.ConvertToIntOrMinValue(this.tbxDiameter.Text.Trim());
            this.currentEntity.DieDiameter = BaseHelper.ConvertToIntOrMinValue(this.tbxDieDiameter.Text.Trim());

            

            this.profileSetting = this.ownerPage.CostCalculationRef.GetProfileSettingById(this.currentEntity.idProfileSetting.ToString());

            try
            {
                if (profileSetting != null)
                {

                     AddPrimitives();
                     this.tmpDiameter = Convert.ToDouble( evalHelper.EvalExpression(profileSetting.DiameterFormula, primitives) );

                     this.tbxDiameter.Text = this.tmpDiameter.ToStringNotFormatted();
                     this.currentEntity.Diameter = BaseHelper.ConvertToDecimalOrZero(this.tbxDiameter.Text);
                    
                }
            }
            catch (Exception ex)
            {
                string falseResult = ex.Message;

                this.ownerPage.CallContext.ResultCode = ETEMEnums.ResultEnum.Error;
                this.ownerPage.CallContext.Message = falseResult;

                return new Tuple<CallContext, string>(this.ownerPage.CallContext, BaseHelper.GetCaptionString("InquiryData_Data"));
            } 
            #endregion

            #region DieFormula Die Dieamter

            if (this.currentEntity.idNumberOfCavities.HasValue)
            {
                DieFormula dieFormula = this.ownerPage.CostCalculationRef.GetDieFormulaParams(
                                    profileSetting.idProfileCategory,
                                    profileSetting.idProfileType,
                                    this.currentEntity.idNumberOfCavities.Value,
                                    this.ownerPage.CallContext);


                if (dieFormula != null)
                {
                    AddPrimitives();

                    this.tmpDieDiameter = Convert.ToDouble(evalHelper.EvalExpression(dieFormula.DieFormulaText, primitives));
                    this.tbxDieDiameter.Text = tmpDieDiameter.ToStringNotFormatted();                     
                    this.currentEntity.DieDiameter= BaseHelper.ConvertToDecimalOrZero(this.tbxDieDiameter.Text);
               
                    string result = ValidateFormulas();


                    if (result.IsNotNullOrEmpty())
                    {
                        string falseResult = "Is not met either of the following conditions: " + result;

                        this.ownerPage.CallContext.ResultCode = ETEMEnums.ResultEnum.Error;
                        this.ownerPage.CallContext.Message = falseResult;

                        return new Tuple<CallContext, string>(this.ownerPage.CallContext, BaseHelper.GetCaptionString("InquiryData_Data"));
                    }

                    

                }
                else
                {

                    string falseResult = BaseHelper.GetCaptionString("InquiryData_DieFormula_not_found");

                    this.ownerPage.CallContext.ResultCode = ETEMEnums.ResultEnum.Error;
                    this.ownerPage.CallContext.Message = falseResult;

                    return new Tuple<CallContext, string>(this.ownerPage.CallContext, BaseHelper.GetCaptionString("InquiryData_Data"));
                }
            }
            else
            {
                    string falseResult = "The field `Number of cavities` is required!";

                    this.ownerPage.CallContext.ResultCode = ETEMEnums.ResultEnum.Error;
                    this.ownerPage.CallContext.Message = falseResult;

                    return new Tuple<CallContext, string>(this.ownerPage.CallContext, BaseHelper.GetCaptionString("InquiryData_Data"));
            }
            
            #endregion

            #region DiePrice
            ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();
            AddCustomSearchCriterias(searchCriteria);

            GeneralPage.LogDebug("this.currentEntity.OfferDate = " + this.currentEntity.OfferDate.ToString());
            List<DiePriceListDetailDataView> listDiePrice = this.ownerPage
                                                                .CostCalculationRef
                                                                .GetAllDiePriceListDetails(
                                                                        searchCriteria,
                                                                        this.currentEntity.OfferDate, 
                                                                        "Price", 
                                                                        Constants.SORTING_ASC
                                                                        );


             GeneralPage.LogDebug("listDiePrice.count = " + listDiePrice.Count);

            listDiePrice = listDiePrice.OrderBy(s => s.DimensionA).ThenByDescending(s => s.Price).ToList();

            DiePriceListDetailDataView tmpDiePrice = listDiePrice.FirstOrDefault();



            if (tmpDiePrice != null)
            {
                this.tbxDieDimensions.Text = tmpDiePrice.DimensionA_String;
                this.tbxDiePrice.Text = tmpDiePrice.Price.ToStringNotFormatted();
                this.tbxVendor.Text = tmpDiePrice.VendorName;
                this.tbxPriceListStr.Text = tmpDiePrice.DateFromString + "-" + tmpDiePrice.DateToString;


                this.currentEntity.DiePrice = tmpDiePrice.Price;
                this.currentEntity.CostOfDie = tmpDiePrice.Price;
                
                this.currentEntity.DieDimensions = tmpDiePrice.DimensionA;
                this.currentEntity.idDiePriceListDetail = tmpDiePrice.idDiePriceListDetail;

            }
            else
            {
                string falseResult = "No die price is found. Please change your offer data..";

                this.ownerPage.CallContext.ResultCode = ETEMEnums.ResultEnum.Error;
                this.ownerPage.CallContext.Message = falseResult;

                return new Tuple<CallContext, string>(this.ownerPage.CallContext, BaseHelper.GetCaptionString("InquiryData_Data"));
            } 
            #endregion

          
            
           
            
            this.currentEntity.ValueForPin = BaseHelper.ConvertToIntOrZero(this.tbxPin.Text.Trim());
            this.currentEntity.ValueForPout = BaseHelper.ConvertToIntOrZero(this.tbxPout.Text.Trim());
            this.currentEntity.SDI = 75;
            
            this.currentEntity.Lifespan = this.currentEntity.Lifespan_Computable; 

            this.currentEntity.LME = BaseHelper.ConvertToDecimalOrZero(this.tbxLME.Text);
            this.currentEntity.PREMIUM = BaseHelper.ConvertToDecimalOrZero(this.tbxPREMIUM.Text);

            this.currentEntity.LengthOfFinalPC = BaseHelper.ConvertToDecimalOrZero(this.tbxLengthOfFinalPC.Text);
            

            this.currentEntity.WeightPerPC = BaseHelper.ConvertToDecimalOrZero(this.tbxWeightPerPC.Text);

            if (this.currentEntity.WeightPerPC == Decimal.Zero)
            {
                this.currentEntity.WeightPerPC = this.currentEntity.WeightPerPCComputable;
                this.tbxWeightPerPC.Text = this.currentEntity.WeightPerPC.ToStringNotFormatted();
            }
            
            
            this.currentEntity.PcsForTheWholeProject = BaseHelper.ConvertToDecimalOrZero(this.tbxPcsForTheWholeProject.Text);
            
            this.currentEntity.Tonnage = this.currentEntity.TonnageComputable;          

            
            this.currentEntity.DaysOfCredit = BaseHelper.ConvertToIntOrMinValue(this.tbxDaysOfCredit.Text);
            this.currentEntity.GrossMargin = BaseHelper.ConvertToDecimalOrZero(this.tbxGrossMargin.Text);
            this.currentEntity.SavingsRate = BaseHelper.ConvertToDecimalOrZero(this.tbxSavingsRate.Text);

            this.currentEntity.idCommision = this.ddlCommision.SelectedValueNullINT;
            this.currentEntity.idAgent= this.ddlAgent.SelectedValueNullINT;
            this.currentEntity.idAging= this.ddlAging.SelectedValueINT;

            this.currentEntity.idHardness= this.ddlHardness.SelectedValueINT;
            this.currentEntity.idStandardPackaging = this.ddlStandardPackaging.SelectedValueINT;

            this.currentEntity.idCalculationCommission= this.ddlCalculationCommission.SelectedValueNullINT;

            
            this.currentEntity.CostOfScrap = this.currentEntity.NetConsumptionComputable - this.currentEntity.MaterialComputable;
            this.currentEntity.AdministrationExpenses = BaseHelper.ConvertToDecimalOrZero(this.tbxAdministrationExpenses.Text);
            this.currentEntity.SalesExpenses = BaseHelper.ConvertToDecimalOrZero(this.tbxSalesExpenses.Text);
            this.currentEntity.FinancialFixedExpenses = BaseHelper.ConvertToDecimalOrZero(this.tbxFinancialFixedExpenses.Text);

            this.currentEntity.FinancialVariableExpenses= BaseHelper.ConvertToDecimalOrZero(this.tbxFinancialVariableExpenses.Text);

            this.currentEntity.TransportationCost = BaseHelper.ConvertToDecimalOrZero(this.tbxTransportationCost.Text);

            this.currentEntity.ActualTotalSalesPriceToCustomer = BaseHelper.ConvertToDecimalOrZero(this.tbxActualTotalSalesPriceToCustomer.Text);

            this.currentEntity.TargetPrice_EUR_TON = BaseHelper.ConvertToDecimalOrZero(this.tbxTargetPrice.Text);

            

            this.ownerPage.CallContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;


            PresetData();


           

            

            if (!ValidateBeforeSave())
            {   
                return new Tuple<CallContext, string>(this.ownerPage.CallContext, string.Empty);
            }
        
            ProductivityAndScrap ps = this.ownerPage.CostCalculationRef.GetProductivityAndScrapByDateActiveTo(this.currentEntity.OfferDate);

            ProductivityAndScrapDataView psDataView = this.ownerPage.CostCalculationRef.GetProductivityAndScrapByDateActiveToWithAvgData(this.currentEntity.OfferDate);

            ProductivityAndScrapDetail productivityAndScrapDetail = this.ownerPage.CostCalculationRef.GetProductivityAndScrapDetailByDateActiveToAndPressAndProfile(this.currentEntity.OfferDate, this.currentEntity.idPress.Value, this.currentEntity.idProfileSetting.Value);

            if (productivityAndScrapDetail != null)
            {
                this.currentEntity.ConsumptionRatio = 1 +  productivityAndScrapDetail.ScrapRate;
            }
            

            this.ownerPage.CallContext = this.ownerPage.CostCalculationRef.OfferSave(this.currentEntity, this.ownerPage.CallContext);


            this.lbResultContext.Text = this.ownerPage.CallContext.Message;

            if (isNewOffer)
            {
                this.hdnRowMasterKey.Value = this.ownerPage.CallContext.EntityID;
                this.CurrentEntityMasterID = this.ownerPage.CallContext.EntityID;
            }
           
            if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {

                 if (this.currentEntity.idPress.HasValue)
                {
                    this.tbxPress.Text = this.ownerPage.GetKeyValueByID(this.currentEntity.idPress.Value).Name;
                    this.ddlPress.SelectedValue = this.currentEntity.idPress.ToString();
                }
               
                this.NeedReCalculation = !this.currentEntity.Equals(this.currentEntityBeforeSave);
                
                this.hdnRowMasterKey.Value = this.ownerPage.CallContext.EntityID;
                this.CurrentEntityMasterID = this.ownerPage.CallContext.EntityID;


                #region OfferDataExpenseGroup
                if (NeedReCalculation)
                {
                    List<Expense> listGroupExpense = this.ownerPage.CostCalculationRef.GetListGroupExpense(this.currentEntity, this.ownerPage.CallContext);

                    if (listGroupExpense == null || listGroupExpense.Count == 0)
                    {
                      

                        this.ownerPage.CallContext.ResultCode = ETEMEnums.ResultEnum.Error;
                        this.ownerPage.CallContext.Message = "No press is found. Please change your offer data.";

                        return new Tuple<CallContext, string>(this.ownerPage.CallContext, BaseHelper.GetCaptionString("InquiryData_Data"));
                    }
                    List<OfferDataExpenseGroup> listOfferDataExpenseGroup = new List<OfferDataExpenseGroup>();

                    foreach (Expense expense in listGroupExpense)
                    {
                        OfferDataExpenseGroup odeg = new OfferDataExpenseGroup();

                        odeg.idOffer = Int32.Parse(this.ownerPage.CallContext.EntityID);
                        odeg.idSAPData = expense.IdSAPData;
                        odeg.idCostCenter = expense.CostCenter.idKeyValue;
                        odeg.idExpensesType = expense.ExpenseGroup.idKeyValue;
                        odeg.ValueData = expense.ExpenseValue_MH;
                        listOfferDataExpenseGroup.Add(odeg);
                    }


                    this.ownerPage.CallContext = this.ownerPage.CostCalculationRef.OfferDataExpenseGroupDelete(this.currentEntity.idOffer, this.ownerPage.CallContext);
                    this.ownerPage.CallContext = this.ownerPage.CostCalculationRef.OfferDataExpenseGroupSave(listOfferDataExpenseGroup, this.ownerPage.CallContext);
                } 
                #endregion

                #region OfferProducitivity
                OfferProducitivity offerProducitivity = this.ownerPage.CostCalculationRef.GetOfferProducitivityByOfferID(this.CurrentEntityMasterID);

                if (offerProducitivity == null)
                {
                    offerProducitivity = new OfferProducitivity();
                    offerProducitivity.idOffer = Int32.Parse(this.CurrentEntityMasterID);
                }



                KeyValue kvSelectPress, kvPackaging;
                List<KeyValue> listPress = this.ownerPage.GetAllKeyValueByKeyTypeIntCode("CostCenter").Where(p => p.DefaultValue1 == "Press").ToList();
                List<KeyValue> listCOMETAL = this.ownerPage.GetAllKeyValueByKeyTypeIntCode("CostCenter").Where(p => p.DefaultValue1 == "COMETAL").ToList();

                List<OfferDataExpenseGroupView> listOfferDataExpenseGroupView = this.ownerPage.CostCalculationRef.GetAllOfferDataExpenseGroupByOffer(offerProducitivity.idOffer);

                 kvPackaging = this.ownerPage.GetKeyValueByIntCode("CostCenter", "Packaging");

                if (listOfferDataExpenseGroupView.Count > 0)
                {

                    OfferDataExpenseGroupView odegvPress = (from e in listOfferDataExpenseGroupView
                                                            join p in listPress on e.idCostCenter equals p.idKeyValue
                                                            select e).FirstOrDefault();






                    offerProducitivity.idPress = odegvPress.idCostCenter;

                   
                    kvSelectPress = listPress.Where(p => p.idKeyValue == offerProducitivity.idPress).FirstOrDefault();


                    OfferDataExpenseGroupView odegvCOMETAL = (from e in listOfferDataExpenseGroupView
                                                              join p in listCOMETAL on e.idCostCenter equals p.idKeyValue
                                                              select e).FirstOrDefault();

                    if (odegvCOMETAL != null)
                    {
                        offerProducitivity.idCOMetal = odegvCOMETAL.idCostCenter;
                    }
                    else
                    {
                        offerProducitivity.idCOMetal = null;
                    }

                    if (productivityAndScrapDetail != null && productivityAndScrapDetail.ProductivityKGh.HasValue)
                    {
                        offerProducitivity.PressProducitivity_KG_MH = productivityAndScrapDetail.ProductivityKGh.Value;
                    }
                    else
                    {
                        offerProducitivity.PressProducitivity_KG_MH = Decimal.Zero;
                    }

                    



                    if (offerProducitivity.PressProducitivity_KG_MH > BaseHelper.ConvertToDecimalOrZero(kvSelectPress.DefaultValue3))
                    {
                        offerProducitivity.PressProducitivity_KG_MH = BaseHelper.ConvertToDecimalOrZero(kvSelectPress.DefaultValue3);
                    }


                    offerProducitivity.PressProducitivity_TON_MH = offerProducitivity.PressProducitivity_TON_MH_Computable;

                    offerProducitivity.COMetalProducitivity_KG_MH = offerProducitivity.PressProducitivity_KG_MH;//Productivity for QC and COMETAL is equal to productivity for press
                    offerProducitivity.COMetalProducitivity_TON_MH = offerProducitivity.COMetalProducitivity_TON_MH_Computable;

                    offerProducitivity.QCProducitivity_KG_MH = offerProducitivity.PressProducitivity_KG_MH;//Productivity for QC and COMETAL is equal to productivity for press
                    offerProducitivity.QCProducitivity_TON_MH = offerProducitivity.QCProducitivity_TON_MH_Computable;


                }


                if (NeedReCalculation)
                {


                    List<SAPDataQuantityDataView> listSAPDataQuantities = new List<SAPDataQuantityDataView>();


                    ICollection<AbstractSearch> searchCriteriaSAPDataQuantity = new List<AbstractSearch>();

                    searchCriteriaSAPDataQuantity.Add(
                      new NumericSearch
                      {
                          Comparator = NumericComparators.Equal,
                          Property = "idCostCenter",
                          SearchTerm = this.ownerPage.GetKeyValueByIntCode("CostCenter", "Packaging").idKeyValue
                      });


                    listSAPDataQuantities = base.CostCalculationRef.GetAllSAPDataQuantity(searchCriteriaSAPDataQuantity,
                                                                                          this.currentEntity.OfferDate,
                                                                                          base.GridViewSortExpression,
                                                                                          base.GridViewSortDirection);

                    KeyValue kvProductionQuantity = this.ownerPage.GetKeyValueByIntCode("QuantityType", "ProductionQuantity");
                    KeyValue kvMachineHours = this.ownerPage.GetKeyValueByIntCode("QuantityType", "MachineHours");

                    Decimal productionQuantity = listSAPDataQuantities.FirstOrDefault(q => q.idQuantityType == kvProductionQuantity.idKeyValue).ValueData;
                    Decimal machineHours = listSAPDataQuantities.FirstOrDefault(q => q.idQuantityType == kvMachineHours.idKeyValue).ValueData;



                    if (productionQuantity != Decimal.Zero && machineHours != Decimal.Zero)
                    {
                        offerProducitivity.PackagingProducitivity_KG_MH = productionQuantity / machineHours;

                        offerProducitivity.PackagingMachineHours = machineHours;
                        offerProducitivity.PackagingProductionQuantity = productionQuantity;
                    }
                    else
                    {
                        offerProducitivity.PackagingProducitivity_KG_MH = BaseHelper.ConvertToDecimalOrZero((BasicPage.GetSettingByCode(ETEMEnums.AppSettings.PackagingProducitivity_KG_MH).SettingValue));
                    }



                    if (offerProducitivity.PackagingProducitivity_KG_MH > BaseHelper.ConvertToDecimalOrZero(kvPackaging.DefaultValue3))
                    {
                        offerProducitivity.PackagingProducitivity_KG_MH = BaseHelper.ConvertToDecimalOrZero(kvPackaging.DefaultValue3);
                    }


                    offerProducitivity.PackagingProducitivity_TON_MH = offerProducitivity.PackagingProducitivity_TON_MH_Computable;


                    


                    this.ownerPage.CallContext = this.ownerPage.CostCalculationRef.OfferProducitivitySave(offerProducitivity, this.ownerPage.CallContext);

                    ProducitivityData producitivityData = FindControlById(this.Page, "ProducitivityData") as ProducitivityData;

                    if (producitivityData != null)
                    {
                        producitivityData.CurrentEntityMasterID = this.CurrentEntityMasterID;
                        producitivityData.SetHdnField(this.CurrentEntityMasterID);
                        producitivityData.UserControlLoad();
                    }
                }

               
                
                #endregion

                ddlCommision_SelectedIndexChanged(null, null);
                ddlCalculationCommission_SelectedIndexChanged(null, null);

                this.currentEntity =  this.ownerPage.CostCalculationRef.GetOfferByID(this.CurrentEntityMasterID);

               

                this.ownerPage.CallContext = this.ownerPage.CostCalculationRef.OfferSave(this.currentEntity, this.ownerPage.CallContext);
                
                this.currentEntity =  this.ownerPage.CostCalculationRef.GetOfferByID(this.CurrentEntityMasterID);

                
                this.currentEntity.LoadProductCostResult();
                this.currentEntity.LoadOfferOverviewResult();


                this.tbxSGAsAndFin_EUR_TON.Text = this.currentEntity.SGAsAndFin_EUR_TON_Computable.ToStringNotFormatted();
                this.tbxSGAsAndFin_EUR.Text = this.currentEntity.SGAsAndFin_EUR_Computable.ToStringNotFormatted();

                 this.tbxFinancialVariableExpenses.Text = this.currentEntity.Financial_variable_Computable.ToStringNotFormatted();
                this.currentEntity.FinancialVariableExpenses = this.currentEntity.Financial_variable_Computable;
                this.currentEntity.TotalSalesPrice = this.currentEntity.TotalSalesPrice_EUR_Computable;

                this.tbxExtrusion_EUR_TON.Text = currentEntity.Extrusion_EUR_TON_Computable.ToStringFormatted();
                this.tbxExtrusion_EUR.Text = currentEntity.Extrusion_EUR_Computable.ToStringFormatted();

                this.tbxPackaging_EUR_TON.Text = currentEntity.Packaging_EUR_TON_Computable.ToStringFormatted();
                this.tbxPackaging_EUR.Text = currentEntity.Packaging_EUR_Computable.ToStringFormatted();
                
                this.tbxTotalSalesPrice_EUR_TON.Text = this.currentEntity.TotalSalesPrice_EUR_TON_Computable.ToStringFormatted();
                this.tbxTotalSalesPrice_EUR.Text = this.currentEntity.TotalSalesPrice_EUR_Computable.ToStringFormatted();

                this.currentEntity.TotalSalesPrice = this.currentEntity.TotalSalesPrice_EUR_Computable;

                this.ownerPage.CallContext = this.ownerPage.CostCalculationRef.OfferSave(this.currentEntity, this.ownerPage.CallContext);
                
                this.currentEntity =  this.ownerPage.CostCalculationRef.GetOfferByID(this.CurrentEntityMasterID);
                
                PresetData();


            }

           

            CheckIfResultIsSuccess();

            return new Tuple<CallContext, string>(this.ownerPage.CallContext, BaseHelper.GetCaptionString("InquiryData_Data"));
        }


        
       /// <summary>
       /// 	Number of cavities and choice of press:
       /// •	W>750gr/m -1 cavity;
       ///       -750gr/m≤W≤2085gr/m- Breda press;
       ///       -2085gr/m≤W≤2639gr/m- SMS2 press;
       ///       -W>2639gr/m-SMS 1;
       ///•    375gr/m≤ W≤750gr/m - 2cavities -only Breda press;
       ///•	   W<375gr/m  - 4cavities- only Breda press.
        /// </summary>
        /// <param name="offer"></param>
        private void SetOfferDataDependOnWeightPerMeter(Offer offer)
        {
            if (offer.WeightPerMeter.HasValue && offer.OfferDate != DateTime.MinValue)
            {

                AverageOutturnOverTimeDataView aoot = this.ownerPage.CostCalculationRef.GetActiveAverageOutturnOverTime(offer.OfferDate);



                if (offer.WeightPerMeter < 375)
                {
                    offer.idNumberOfCavities = this.ownerPage.GetKeyValueByIntCode("NumberOfCavities","FourCavities").idKeyValue;
                    offer.idPress = this.ownerPage.GetKeyValueByIntCode("CostCenter","Breda").idKeyValue;
                    offer.AverageOutturnOverTime = aoot.ValueOfPressBREDA;
                }
                else if(offer.WeightPerMeter >= 375 && offer.WeightPerMeter <= 750 )
                {
                    offer.idNumberOfCavities = this.ownerPage.GetKeyValueByIntCode("NumberOfCavities","TwoCavities").idKeyValue;
                    offer.idPress = this.ownerPage.GetKeyValueByIntCode("CostCenter","Breda").idKeyValue;
                    offer.AverageOutturnOverTime = aoot.ValueOfPressBREDA;
                }
                else if(offer.WeightPerMeter >= 750 && offer.WeightPerMeter <= 2085)
                {
                    offer.idNumberOfCavities = this.ownerPage.GetKeyValueByIntCode("NumberOfCavities","OneCavity").idKeyValue;
                    offer.idPress = this.ownerPage.GetKeyValueByIntCode("CostCenter","Breda").idKeyValue;
                    offer.AverageOutturnOverTime = aoot.ValueOfPressBREDA;
                }
                else if(offer.WeightPerMeter >= 2085 && offer.WeightPerMeter <= 2639)
                {
                    offer.idNumberOfCavities = this.ownerPage.GetKeyValueByIntCode("NumberOfCavities","OneCavity").idKeyValue;
                    offer.idPress = this.ownerPage.GetKeyValueByIntCode("CostCenter","SMS2").idKeyValue;
                    offer.AverageOutturnOverTime = aoot.ValueOfPressSMS2;
                }
                else if(offer.WeightPerMeter > 2639 )
                {
                    offer.idNumberOfCavities = this.ownerPage.GetKeyValueByIntCode("NumberOfCavities","OneCavity").idKeyValue;
                    offer.idPress = this.ownerPage.GetKeyValueByIntCode("CostCenter","SMS1").idKeyValue;
                    offer.AverageOutturnOverTime = aoot.ValueOfPressSMS1;
                }

                if (offer.idNumberOfCavities.HasValue)
                {
                    this.ddlNumberOfCavities.SelectedValue = offer.idNumberOfCavities.ToString();
                }

               
            }
            else
            {
                offer.idPress = null;
                offer.idNumberOfCavities = null;


            }
        }
        
        private bool ValidateBeforeSave()
        {
            bool result = true;
            string errorMsg = string.Empty;
            
            this.ownerPage.CallContext.ResultCode = ETEMEnums.ResultEnum.Error;
            this.ownerPage.CallContext.Message = errorMsg;


            if (this.currentEntity.idPress.HasValue)
            {
                KeyValue kvSelectePress = this.ownerPage.GetKeyValueByID(this.currentEntity.idPress.Value);

                if (string.IsNullOrEmpty(kvSelectePress.DefaultValue5) || BaseHelper.ConvertToDecimalOrZero(kvSelectePress.DefaultValue5) == decimal.Zero)
                {
                    this.ownerPage.CallContext.Message += "Is not set value for minimum tonage for press `" + kvSelectePress.Name + "`. The value shulde be entred in default value 5 ";
                }
                else
                {

                    if (BaseHelper.ConvertToDecimalOrZero(kvSelectePress.DefaultValue5) > this.currentEntity.TonnageComputable_KG)
                    {
                        //Weight per meter (gr/m)` should be greater than or equal to 185 (mm)
                         this.ownerPage.CallContext.ResultCode = ETEMEnums.ResultEnum.Error;
                         this.ownerPage.CallContext.Message += " Tonnage  should be greater than or equal to " + kvSelectePress.DefaultValue5 + " (kg).,";
                    }
                }
            }
            
            List<string> pressIntCodeListByDimension = this.ownerPage.CostCalculationRef.GetPressIntCodeListByDimension(this.currentEntity);

            if (this.currentEntity.idPress.HasValue)
            {
                KeyValue kvSelectedPress = this.ownerPage.GetKeyValueByID(this.currentEntity.idPress.Value);

                if (!pressIntCodeListByDimension.Contains(kvSelectedPress.KeyValueIntCode))
                {
                     this.ownerPage.CallContext.ResultCode = ETEMEnums.ResultEnum.Error;
                     this.ownerPage.CallContext.Message += "No press is found. Please change your offer data.,";
                }

                 
            }
            else
            {
                this.ownerPage.CallContext.ResultCode = ETEMEnums.ResultEnum.Error;
                this.ownerPage.CallContext.Message += "No press is found. Please change your offer data.,";
                
            }


            if (this.ownerPage.CallContext.Message == errorMsg)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        private bool ValidateRequiredField()
        {
            bool result = true;
            string errorMsg = string.Empty;
            
            this.ownerPage.CallContext.ResultCode = ETEMEnums.ResultEnum.Error;
            this.ownerPage.CallContext.Message = errorMsg;


            if (this.ddlStandardPackaging.SelectedValueINT != this.ownerPage.GetKeyValueByIntCode("YES_NO", "Yes").idKeyValue)
            {
                this.ownerPage.CallContext.Message += "Please confirm that the offer is with standard packaging.";
            }



            #region Length of final PC (mm)
            if (string.IsNullOrEmpty(this.tbxLengthOfFinalPC.Text))
            {
                this.ownerPage.CallContext.Message += "`Length of final PC (mm)` is required,";
            }
            else
            {
                int lengthOfFinalPC = BaseHelper.ConvertToIntOrMinValue(this.tbxLengthOfFinalPC.Text);

                int lengthOfFinalPC_MIN = BaseHelper.ConvertToIntOrMinValue(GeneralPage.GetSettingByCode(ETEMEnums.AppSettings.LengthOfFinalPC_MIN).SettingValue);
                int lengthOfFinalPC_MAX = BaseHelper.ConvertToIntOrMinValue(GeneralPage.GetSettingByCode(ETEMEnums.AppSettings.LengthOfFinalPC_MAX).SettingValue);



                if (lengthOfFinalPC > lengthOfFinalPC_MAX)
                {
                    this.ownerPage.CallContext.Message += "`Length of final PC (mm)` should be less than or equal to " + lengthOfFinalPC_MAX + " (mm),";
                    
                }

                if (lengthOfFinalPC < lengthOfFinalPC_MIN)
                {
                    this.ownerPage.CallContext.Message += "`Length of final PC (mm)` should be greater than  or equal to " + lengthOfFinalPC_MIN + " (mm). If you have a smaller length please multiply the value per meter.,";
                    
                }


            } 
            #endregion

            #region s (mm) - Tickness

            if (this.tbxValueForS.Enabled)
            {
                if (string.IsNullOrEmpty(this.tbxValueForS.Text))
                {
                    this.ownerPage.CallContext.Message += "`s (mm) - Tickness` is required,";
                }
                else
                {
                    Decimal valueS = BaseHelper.ConvertToDecimalOrZero(this.tbxValueForS.Text);

                    Decimal value_MIN = BaseHelper.ConvertToDecimalOrZero(GeneralPage.GetSettingByCode(ETEMEnums.AppSettings.Tickness_MIN).SettingValue);
                    Decimal value_MAX = BaseHelper.ConvertToDecimalOrZero(GeneralPage.GetSettingByCode(ETEMEnums.AppSettings.Tickness_MAX).SettingValue);



                    if (valueS > value_MAX)
                    {
                        this.ownerPage.CallContext.Message += "`s (mm) - Tickness` should be less than or equal to " + value_MAX + " (mm),";
                    }

                    if (valueS < value_MIN)
                    {
                        this.ownerPage.CallContext.Message += "`s (mm) - Tickness` should be greater than or equal to " + value_MIN + " (mm),";
                    }
                } 
            }
            
            #endregion

            #region Weight per meter (gr/m)
            if (string.IsNullOrEmpty(this.tbxWeightPerMeter.Text))
            {
                this.ownerPage.CallContext.Message += "`Weight per meter (gr/m)` is required,";
            }
            else
            {
                int value = BaseHelper.ConvertToIntOrMinValue(this.tbxWeightPerMeter.Text);

                int value_MIN = BaseHelper.ConvertToIntOrMinValue(GeneralPage.GetSettingByCode(ETEMEnums.AppSettings.Weight_Per_Meter_MIN).SettingValue);
                int value_MAX = BaseHelper.ConvertToIntOrMinValue(GeneralPage.GetSettingByCode(ETEMEnums.AppSettings.Weight_Per_Meter_MAX).SettingValue);



                if (value > value_MAX)
                {
                    this.ownerPage.CallContext.Message += "`Weight per meter (gr/m)` should be less than or equal to " + value_MAX + " (mm),";
                }

                if (value < value_MIN)
                {
                    this.ownerPage.CallContext.Message += "`Weight per meter (gr/m)` should be greater than or equal to  " + value_MIN + " (mm),";
                }


            }  
            #endregion

            #region Pin / Pout
            if (string.IsNullOrEmpty(this.tbxPin.Text))
            {
                this.ownerPage.CallContext.Message += "`Pin` is required,";
            }

            if (string.IsNullOrEmpty(this.tbxPout.Text))
            {
                this.ownerPage.CallContext.Message += "`Pout` is required,";
            }
            
            #endregion

            #region Date, Customer, Profile, Hardness
            if (string.IsNullOrEmpty(this.tbxOfferDate.Text))
            {
                this.ownerPage.CallContext.Message += "`Date` is required,";
            }
            if (string.IsNullOrEmpty(this.tbxCustomer.Text))
            {
                this.ownerPage.CallContext.Message += "`Customer` is required,";
            }

            if (string.IsNullOrEmpty(this.tbxProfileSettingName.Text))
            {
                this.ownerPage.CallContext.Message += "`Profile` is required,";
            }
            if (string.IsNullOrEmpty(this.tbxProfileSettingName.Text))
            {
                this.ownerPage.CallContext.Message += "`Profile` is required,";
            }

            if (this.ddlHardness.SelectedValueINT == Constants.INVALID_ID)
            {
                this.ownerPage.CallContext.Message += "`Hardness` is required,";
            }
            #endregion

            

            if (this.ownerPage.CallContext.Message == errorMsg)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;

        }

        private void SetDefaultValues()
        {
            this.currentEntity.ConsumptionRatio = BaseHelper.ConvertToDecimalOrZero((BasicPage.GetSettingByCode(ETEMEnums.AppSettings.ConsumptionRatio).SettingValue));
            this.currentEntity.ScrapValuePercent = BaseHelper.ConvertToDecimalOrZero((BasicPage.GetSettingByCode(ETEMEnums.AppSettings.ScrapValuePercent).SettingValue));
            this.currentEntity.Interest = BaseHelper.ConvertToDecimalOrZero((BasicPage.GetSettingByCode(ETEMEnums.AppSettings.Interest).SettingValue));


            

            this.currentEntity.MaterialCostForPackaging = BaseHelper.ConvertToDecimalOrZero((BasicPage.GetSettingByCode(ETEMEnums.AppSettings.Material_cost_for_packaging).SettingValue));
            this.currentEntity.RatioConsumptionPackaging = BaseHelper.ConvertToDecimalOrZero((BasicPage.GetSettingByCode(ETEMEnums.AppSettings.Ratio_consumption_packaging).SettingValue));
        }

        private void PresetData()
        {
            this.tbxInquiryNumber.Text = this.currentEntity.InquiryNumber;
            this.tbxTonnage.Text = this.currentEntity.TonnageComputable.ToStringNotFormatted();
            this.tbxTonnage_KG.Text = this.currentEntity.TonnageComputable_KG.ToStringFormatted();
            
            this.tbxMaterial_EUR.Text = this.currentEntity.Material_EUR_Computable.ToStringFormatted();
            this.tbxTransportation_EUR.Text = this.currentEntity.Transportation_EUR_Computable.ToStringFormatted();
            //this.tbxFinancialVariableExpenses.Text = this.currentEntity.Financial_variable_Computable.ToStringNotFormatted();
            this.tbxActualTotalSalesPriceToCustomerAll.Text = (this.currentEntity.ActualTotalSalesPriceToCustomer * this.currentEntity.Tonnage).ToStringFormatted();
            OfferProducitivity  offerProducitivity = this.ownerPage.CostCalculationRef.GetOfferProducitivityByOfferID(this.CurrentEntityMasterID);


            if(offerProducitivity != null)
            {
                KeyValue kvPress = this.ownerPage.GetKeyValueByID(offerProducitivity.idPress);

                if(kvPress != null)
                {
                    this.tbxPress.Text = kvPress.Name;
                    this.ddlPress.SelectedValue = kvPress.idKeyValue.ToString();
                }
            }
                
        }
 
        private string ValidateFormulas()
        {
            bool validData = false;
            string strValidation = string.Empty;

            List<ProfileSettingValidation> listValidation = this.ownerPage.CostCalculationRef.GetProfileSettingValidationByIDProfile(this.profileSetting.idProfileSetting);

            AddPrimitives();

            foreach (ProfileSettingValidation validation in listValidation)
            {
                strValidation += "," + validation.ValidationRequirement;
                string[] expression = validation.ValidationRequirement.RemoveSpaces().SplitByCharArr(new char[] { ';' });
                        
                validData = false;

                foreach (string exp in expression)
                {
                    string[] operand = new string[] { "" };
                    string[] expPars = new string[] { "" };

                    if (exp.IndexOf(">=") > -1) 
                    {
                        operand = new string[] { ">=" };                         
                        expPars = exp.Split(operand, StringSplitOptions.RemoveEmptyEntries);
                        validData = evalHelper.EvalExpression(expPars[0], primitives) >= evalHelper.EvalExpression(expPars[1], primitives);
                    }
                    else if (exp.IndexOf("<=") > -1) 
                    {
                        operand = new string[] { "<=" };                            
                        expPars = exp.Split(operand, StringSplitOptions.RemoveEmptyEntries);
                        validData = evalHelper.EvalExpression(expPars[0], primitives) <= evalHelper.EvalExpression(expPars[1], primitives);
                    }
                    else if (exp.IndexOf(">") > -1) 
                    {
                        operand = new string[] { ">" };                            
                        expPars = exp.Split(operand, StringSplitOptions.RemoveEmptyEntries);
                         validData = evalHelper.EvalExpression(expPars[0], primitives) > evalHelper.EvalExpression(expPars[1], primitives);
                    }
                    else if (exp.IndexOf("<") > -1) 
                    {
                        operand = new string[] { "<" };                            
                        expPars = exp.Split(operand, StringSplitOptions.RemoveEmptyEntries);
                        validData = evalHelper.EvalExpression(expPars[0], primitives) < evalHelper.EvalExpression(expPars[1], primitives);
                    }
                    else if (exp.IndexOf("=") > -1) 
                    {
                        operand = new string[] { "=" };                            
                        expPars = exp.Split(operand, StringSplitOptions.RemoveEmptyEntries);
                         validData = evalHelper.EvalExpression(expPars[0], primitives) == evalHelper.EvalExpression(expPars[1], primitives);
                    }

                    if (!validData)
                    {
                        break;
                    }
                }

                if (validData)
                {
                    break;
                }
            }

            if (!validData)
            {
                return strValidation;
            }
            else
            {
                return string.Empty;
            }

            
        }
 
        private void AddPrimitives()
        {
            this.primitives.Clear();

            if (profileSetting.hasA)
            {
                primitives.Add("A", this.tbxValueForA.Text.Replace(",", "."));
            }
            else
            {
                 primitives.Add("A", "0");   
            }
            
            if (profileSetting.hasB)
            {
                primitives.Add("B", this.tbxValueForB.Text.Replace(",", "."));
            }
            else
            {
                 primitives.Add("B", "0");   
            }


            if (profileSetting.hasC)
            {
                primitives.Add("C", this.tbxValueForC.Text.Replace(",", "."));
            }
            else
            {
                 primitives.Add("C", "0");   
            }

            if (profileSetting.hasD)
            {
                primitives.Add("D", this.tbxValueForD.Text.Replace(",", "."));
            }
            else
            {
                 primitives.Add("D", "0");   
            }


            if (profileSetting.hasS)
            {
                primitives.Add("s", this.tbxValueForS.Text.Replace(",", "."));
            }
            else
            {
                 primitives.Add("s", "0");   
            }

            primitives.Add("Ø", this.tbxDiameter.Text.Replace(",", "."));

            primitives.Add("Ødie", this.tbxDieDiameter.Text.Replace(",", "."));
            
        }

        private void AddCustomSearchCriterias(ICollection<AbstractSearch> searchCriteria)
        {
            searchCriteria.Add(
                      new NumericSearch
                      {
                          Comparator = NumericComparators.Equal,
                          Property = "idNumberOfCavities",
                          SearchTerm = this.currentEntity.idNumberOfCavities
                      });

            searchCriteria.Add(
                  new NumericSearch
                  {
                      Comparator = NumericComparators.Equal,
                      Property = "idProfileCategory",
                      SearchTerm = this.profileSetting.idProfileCategory
                  });

            searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.Equal,
                        Property = "idProfileComplexity",
                        SearchTerm = this.profileSetting.idProfileComplexity
                    });

           
            GeneralPage.LogDebug("tbxDieDiameter = " + Decimal.ToInt32( Math.Ceiling(BaseHelper.ConvertToDecimalOrZero(this.tbxDieDiameter.Text))));
            searchCriteria.Add( 
                    new NumericSearch
                    {
                        Comparator = NumericComparators.GreaterOrEqual,
                        Property = "DimensionA",
                        SearchTerm = Decimal.ToInt32( Math.Ceiling(BaseHelper.ConvertToDecimalOrZero(this.tbxDieDiameter.Text)))
                    });
        }

        protected void btnChoose_Click(object sender, EventArgs e)
        {
            //HtmlControl control         = FindControl("ucProfilesListCtrl") as HtmlControl;
            //control.Attributes["class"] = "modalPopupInnerWidthLarge";


            this.ucProfilesListCtrlChooseOffer.OfferID = this.hdnRowMasterKey.Value;
            this.ucProfilesListCtrlChooseOffer.UserControlLoad();
            this.ucProfilesListCtrlChooseOffer.Visible = true;
        }

        protected void ddlCommision_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeyValue kvDomestic = this.ownerPage.GetKeyValueByIntCode("Commision","Domestic");
            KeyValue kvExport = this.ownerPage.GetKeyValueByIntCode("Commision","Export");
            
            if (ddlCommision.SelectedValueINT == kvDomestic.idKeyValue)
            {
                ddlAgent.DropDownEnabled = false;
                ddlCalculationCommission.DropDownEnabled  = false;
                ddlAgent.SelectedValue = Constants.INVALID_ID_STRING;
                ddlCalculationCommission.SelectedValue = Constants.INVALID_ID_STRING;
            }
            else if (ddlCommision.SelectedValueINT == kvExport.idKeyValue)
            {
                ddlAgent.DropDownEnabled = true;                
                ddlCalculationCommission.DropDownEnabled  = true;
                

            }
            else
            {
                ddlAgent.DropDownEnabled = false;
                ddlCalculationCommission.DropDownEnabled  = false;
                ddlAgent.SelectedValue = Constants.INVALID_ID_STRING;
                ddlCalculationCommission.SelectedValue = Constants.INVALID_ID_STRING;
                this.tbxCommission.Text = string.Empty;
                this.lbCommission.Text = "Commission (%)";
            }

            ReLoadAllOffer();

        }

        private void ReLoadAllOffer()
        {
             string idOffer = this.CurrentEntityMasterID == Constants.INVALID_ID_STRING ? this.hdnRowMasterKey.Value : this.CurrentEntityMasterID;

            DiesData diesData = FindControlById(this.Page, "DiesData") as DiesData;

            if(diesData != null){
                diesData.CurrentEntityMasterID = idOffer;
                diesData.SetHdnField(idOffer);
                diesData.UserControlLoad();
            }


            BilletScrapData billetScrapData = FindControlById(this.Page, "BilletScrap") as BilletScrapData;

            if (billetScrapData != null)
            {
                billetScrapData.CurrentEntityMasterID = idOffer;
                billetScrapData.SetHdnField(idOffer);
                billetScrapData.UserControlLoad();
            }

            //ProducitivityData producitivityData = FindControlById(this.Page, "ProducitivityData") as ProducitivityData;

            //if (producitivityData != null)
            //{
            //    producitivityData.CurrentEntityMasterID = idOffer;
            //    producitivityData.SetHdnField(idOffer);
            //    producitivityData.UserControlLoad();
            //}


            ExpensesByCostCentersData expensesByCostCentersData = FindControlById(this.Page, "ExpensesByCostCentersData") as ExpensesByCostCentersData;

            if (expensesByCostCentersData != null)
            {
                expensesByCostCentersData.CurrentEntityMasterID = idOffer;
                expensesByCostCentersData.SetHdnField(idOffer);
                expensesByCostCentersData.UserControlLoad();
            }

            ExpensesByCostCentersDataTon expensesByCostCentersDataTon = FindControlById(this.Page, "ExpensesByCostCentersDataTon") as ExpensesByCostCentersDataTon;

            if (expensesByCostCentersDataTon != null)
            {
                expensesByCostCentersDataTon.CurrentEntityMasterID =idOffer;
                expensesByCostCentersDataTon.SetHdnField(idOffer);
                expensesByCostCentersDataTon.UserControlLoad();
            }

            ProductCostData productCostData = FindControlById(this.Page, "ucProductCostData") as ProductCostData;

            productCostData.CurrentEntityMasterID = idOffer;
            productCostData.SetHdnField(idOffer);
            productCostData.UserControlLoad();

            OfferOverviewData offerOverviewData = FindControlById(this.Page, "OfferOverviewData") as OfferOverviewData;


            offerOverviewData.CurrentEntityMasterID = this.CurrentEntityMasterID;
            offerOverviewData.SetHdnField(this.CurrentEntityMasterID);
            offerOverviewData.UserControlLoad();


        }

        protected void ddlCalculationCommission_SelectedIndexChanged(object sender, EventArgs e)
        {


            CommissionsByAgent commissionsByAgent = null;

            try
            {
                commissionsByAgent = this.ownerPage.CostCalculationRef.GetCommissionsByAgentByDateActiveTo(this.ddlAgent.SelectedValueINT, this.tbxOfferDate.TextAsDateParseExact.Value);
            }
            catch { }


            if (commissionsByAgent == null)
            {
                return;
            }
            KeyValue kvFixedCommissionPerTon = this.ownerPage.GetKeyValueByIntCode("CalculationCommission","FixedCommissionPerTon");
            KeyValue kvPercentFinalInvoiceValue = this.ownerPage.GetKeyValueByIntCode("CalculationCommission","PercentFinalInvoiceValue");

            
            



            if(commissionsByAgent != null)
            {
                if(this.ddlCalculationCommission.SelectedValueINT == kvFixedCommissionPerTon.idKeyValue)
                {

                    this.tbxCommission.Text = commissionsByAgent.FixedCommission.ToStringNotFormatted();
                    this.lbCommission.Text = kvFixedCommissionPerTon.DefaultValue1;

                    this.tbxCommission_EUR.Text = (BaseHelper.ConvertToDecimalOrZero(this.tbxTonnage.Text) * commissionsByAgent.FixedCommission).ToStringFormatted();
                }
                else if (this.ddlCalculationCommission.SelectedValueINT == kvPercentFinalInvoiceValue.idKeyValue)
                {

                    string idOffer = this.CurrentEntityMasterID == Constants.INVALID_ID_STRING ? this.hdnRowMasterKey.Value : this.CurrentEntityMasterID;

                    if (!string.IsNullOrEmpty(idOffer))
                    {

                        ProductCostResult productCost = this.CostCalculationRef.LoadProductCostsByOfferId(idOffer, this.ownerPage.CallContext);

                        ProductCostDataView productCostCommission = productCost.ListProductCosts.Where(p => p.RowKeyIntCode == "Commission").FirstOrDefault();



                        this.tbxCommission_EUR.Text = (productCostCommission.Value_EUR_ton * BaseHelper.ConvertToDecimalOrZero(this.tbxTonnage.Text)).ToStringFormatted();
                    }

                    this.tbxCommission.Text = commissionsByAgent.CommissionPercent.ToStringNotFormatted();
                    this.lbCommission.Text = kvPercentFinalInvoiceValue.DefaultValue1;

                }
                else
                {
                    this.tbxCommission_EUR.Text = string.Empty;
                    this.tbxCommission.Text = string.Empty;
                    this.lbCommission.Text = "Commission (%)";
                }



               
            }
            
             ReLoadAllOffer();
        }

        protected void btnShowHideGridView_Click(object sender, EventArgs e)
        {
            if (this.divGridValidation.Visible)
            {
                this.divGridValidation.Visible = false;
            }
            else
            {
                this.divGridValidation.Visible = true;
            }

        }

    }
}