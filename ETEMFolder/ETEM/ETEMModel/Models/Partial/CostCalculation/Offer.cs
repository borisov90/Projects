using ETEMModel.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using ETEMModel.Helpers.Extentions;
using ETEMModel.Helpers.CostCalculation;
using ETEMModel.Helpers.Admin;
using ETEMModel.Models.DataView.CostCalculation;

namespace ETEMModel.Models
{
    public partial class Offer : Identifiable
    {
        #region Identifiable Members

        public int EntityID
        {
            get { return this.idOffer; }
        }

        public string ValidationErrorsAsString { get; set; }


        public override bool Equals(object obj)
        {
            bool result = true;
            Offer item = obj as Offer;

            if (item == null)
            {
                return false;
            }

            result = result && item.idOffer == this.idOffer;
            result = result && item.OfferDate   == this.OfferDate;
            result = result && item.idNumberOfCavities == this.idNumberOfCavities;
            result = result && item.ValueForA   == this.ValueForA;
            result = result && item.ValueForB   == this.ValueForB;
            result = result && item.ValueForC   == this.ValueForC;
            result = result && item.ValueForD   == this.ValueForD;
            result = result && item.ValueForS   == this.ValueForS;
            result = result && item.idAging     == this.idAging;
            result = result && item.idPress     == this.idPress;
            result = result && item.WeightPerMeter == this.WeightPerMeter;

            return result;
            

        }

        public override int GetHashCode()
        {
            return this.idOffer.GetHashCode();
        }


        public List<string> ValidateEntity(CallContext outputContext)
        {
            List<string> result = new List<string>();

            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;

            ValidationErrorsAsString = string.Empty;

          
            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;

            //if (string.IsNullOrEmpty(this.InquiryNumber))
            //{
            //    result.Add("The field `InquiryNumber` is required!");
            //}

            if (this.OfferDate == DateTime.MinValue)
            {
                result.Add("The field `Date` is required!");
            }


            if (string.IsNullOrEmpty(this.Customer))
            {
                result.Add("The field `Customer` is required!");
            }

            if (!this.idProfileSetting.HasValue)
            {
                result.Add("The field `Profile` is required!");
            }


            if (!this.idNumberOfCavities.HasValue)
            {
                result.Add("The field `Number of cavities` is required!");
            }

            if (!this.LME.HasValue)
            {
                result.Add("The field `LME` is required!");
            }

            if (!this.PREMIUM.HasValue)
            {
                result.Add("The field `PREMIUM` is required!");
            }


            if (!this.idProfileSetting.HasValue)
            {
                result.Add("The field `Profile` is required!");
            }


            if (this.idProfileSetting.HasValue)
            {
                ProfileSetting profileSetting = new ProfileSettingBL().GetEntityById(this.idProfileSetting.Value);

                if (profileSetting.hasA && !this.ValueForA.HasValue)
                {
                    result.Add("The field `Value for A (mm)` is required!");
                }

                if (profileSetting.hasB && !this.ValueForB.HasValue)
                {
                    result.Add("The field `Value for B (mm)` is required!");
                }

                if (profileSetting.hasC && !this.ValueForC.HasValue)
                {
                    result.Add("The field `Value for C (mm)` is required!");
                }

                if (profileSetting.hasD && !this.ValueForD.HasValue)
                {
                    result.Add("The field `Value for D (mm)` is required!");
                }

                if (profileSetting.hasS && !this.ValueForS.HasValue)
                {
                    result.Add("The field `Value for S (mm)` is required!");
                }


            }


            if (!this.LME.HasValue)
            {
                result.Add("The field `LME` is required!");
            }


            if (this.LME.HasValue && this.LME.Value == decimal.MinValue)
            {
                result.Add("The field `LME` is required and should be equal or bigger than zero!");
            }



            if (!this.PREMIUM.HasValue)
            {
                result.Add("The field `PREMIUM` is required!");
            }


            if (this.PREMIUM.HasValue && this.PREMIUM.Value == decimal.MinValue)
            {
                result.Add("The field `PREMIUM` is required and should be equal or bigger than zero!");
            }


            this.ValidationErrorsAsString = string.Join(Constants.ERROR_MESSAGES_SEPARATOR, result.ToArray());

            if (!string.IsNullOrEmpty(this.ValidationErrorsAsString))
            {
                outputContext.ResultCode = ETEMEnums.ResultEnum.Error;
            }

            outputContext.Message = this.ValidationErrorsAsString;
            if (result.Count > 0)
            {
                outputContext.EntityID = "";
            }
            else
            {
                outputContext.EntityID = this.EntityID.ToString();
            }


            return result;
        }

        #endregion

        private ProductCostResult productCostResult;
        private OfferOverviewResult offerOverviewResult;

        public ProductCostResult LoadProductCostResult()
        {
            productCostResult = new ProductCostBL().LoadProductCostsByOfferId(idOffer, new CallContext());

            return productCostResult;
        }

        public OfferOverviewResult LoadOfferOverviewResult()
        {
            this.offerOverviewResult = new OfferOverviewBL().LoadOfferOverviewByOfferId(idOffer, new CallContext());

            return offerOverviewResult;
        }
        private decimal? _CostOfDieEUR_Per_TON_Computable;
        public decimal? CostOfDieEUR_Per_TON_Computable
        {
            get {

                if (_CostOfDieEUR_Per_TON_Computable.HasValue)
                {
                    return _CostOfDieEUR_Per_TON_Computable;
                }
                else
                {
                    try
                    {
                        if (this.CostOfDie.HasValue && this.Lifespan.HasValue && this.Lifespan.Value != 0)
                        {
                            return (this.CostOfDie.Value / this.Lifespan.Value);
                        }
                        else
                        {
                            return Decimal.Zero;
                        }
                    }
                    catch
                    {
                        return Decimal.Zero;
                    }
                }
            }
            set {

                _CostOfDieEUR_Per_TON_Computable = value;
            }
        }
        
        private decimal? _MaterialComputable;
        public decimal? MaterialComputable
        {

          get {

                if (_MaterialComputable.HasValue)
                {
                    return _MaterialComputable;
                }
                else
                {
                    decimal result = Decimal.Zero;

                    try
                    {
                       

                        if (this.LME.HasValue)
                        {
                            result += this.LME.Value;
                        }

                        if (this.PREMIUM.HasValue)
                        {
                            result += this.PREMIUM.Value;
                        }

                        return result;
                        
                    }
                    catch
                    {
                        return result;
                    }
                }
            }
            set {

                _MaterialComputable = value;
            }
        }
        
        private decimal? _Consumption_EUR_TON_Computable;
        public decimal? Consumption_EUR_TON_Computable
        {

          get {

              if (_Consumption_EUR_TON_Computable.HasValue)
                {
                    return _Consumption_EUR_TON_Computable.Value;
                }
                else
                {
                    try
                    {
                        decimal material = Decimal.Zero;

                        if (this.LME.HasValue)
                        {
                            material += this.LME.Value;
                        }

                        if (this.PREMIUM.HasValue)
                        {
                            material += this.PREMIUM.Value;
                        }


                        if (ConsumptionRatio.HasValue)
                        {
                            return (ConsumptionRatio.Value * material);
                        }
                        else
                        {
                            return null;
                        }

                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            set {

                _Consumption_EUR_TON_Computable = value;
            }
        }
        
        private decimal? _ScrapValue_EUR_TON_Computable;
        public decimal? ScrapValue_EUR_TON_Computable
        {

          get {

              if (_ScrapValue_EUR_TON_Computable.HasValue)
                {
                    return _ScrapValue_EUR_TON_Computable.Value;
                }
                else
                {
                    try
                    {
                        decimal scrapValuePercent = Decimal.Zero;
                        decimal lme = Decimal.Zero;
                        decimal consumptionRatio = Decimal.Zero;



                        if (this.ScrapValuePercent.HasValue)
                        {
                            scrapValuePercent = this.ScrapValuePercent.Value;
                        }

                        if (this.LME.HasValue)
                        {
                            lme = this.LME.Value;
                        }

                        if (ConsumptionRatio.HasValue)
                        {
                            consumptionRatio = this.ConsumptionRatio.Value;
                        }

                        if (consumptionRatio  < 0)
                        {
                            throw new ETEMModelException("Consumption ratio should be bigger than one. Please check it.");
                        }

                        return scrapValuePercent * lme * (consumptionRatio - 1);

                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            set {

                _ScrapValue_EUR_TON_Computable = value;
            }
        }
        
        private decimal? _NetConsumptionComputable;
        public decimal? NetConsumptionComputable
        {

          get {

              if (_NetConsumptionComputable.HasValue)
                {
                    return _NetConsumptionComputable.Value;
                }
                else
                {
                    try
                    {
                      

                        return Consumption_EUR_TON_Computable  - ScrapValue_EUR_TON_Computable;

                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            set {

                _NetConsumptionComputable = value;
            }
        }
        
        private decimal? _CostOfScrapComputable;
        public decimal? CostOfScrapComputable
        {

            get
            {

                if (_CostOfScrapComputable.HasValue)
                {
                    return _CostOfScrapComputable.Value;
                }
                else
                {
                    try
                    {
                        decimal lme = LME.HasValue?LME.Value:Decimal.Zero;
                        decimal premium = PREMIUM.HasValue?PREMIUM.Value:Decimal.Zero;

                        return NetConsumptionComputable - lme - premium;

                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            set
            {

                _CostOfScrapComputable = value;
            }
        }


        private decimal? _Financial_variable_Computable;
        public decimal? Financial_variable_Computable
        {

            get
            {

                if (_Financial_variable_Computable.HasValue)
                {
                    return _Financial_variable_Computable.Value;
                }
                else
                {
                    try
                    {




                        ProductCostDataView totalConversionCost = this.productCostResult.ListProductCosts.FirstOrDefault(p=>p.RowKeyIntCode == "TotalConversionCost");

                        if (totalConversionCost != null)
                        {
                            return (totalConversionCost.Value_EUR_ton + Commission_EUR_Computable / Tonnage + TransportationCost) * DaysOfCredit / 360 * Interest / 100;
                        }
                        else
                        {
                            return null;
                        }


                        

                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            set
            {

                _Financial_variable_Computable = value;
            }
        }


         private decimal? _TonnageComputable_KG;
        public decimal? TonnageComputable_KG
        {

            get
            {

                if (_TonnageComputable_KG.HasValue)
                {
                    return _TonnageComputable_KG.Value;
                }
                else
                {
                    try
                    {
                        decimal tempPcsForTheWholeProject = PcsForTheWholeProject.HasValue ? PcsForTheWholeProject.Value : decimal.Zero;
                        decimal tempWeightPerPC = WeightPerPC.HasValue ? WeightPerPC.Value : decimal.Zero;

                        return tempPcsForTheWholeProject * tempWeightPerPC;

                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            set
            {

                _TonnageComputable_KG = value;
            }
        }

        private decimal? _TonnageComputable;
        public decimal? TonnageComputable
        {

            get
            {

                if (_TonnageComputable.HasValue)
                {
                    return _TonnageComputable.Value;
                }
                else
                {
                    try
                    {
                        decimal tempPcsForTheWholeProject = PcsForTheWholeProject.HasValue ? PcsForTheWholeProject.Value : decimal.Zero;
                        decimal tempWeightPerPC = WeightPerPC.HasValue ? WeightPerPC.Value : decimal.Zero;

                        return tempPcsForTheWholeProject * tempWeightPerPC / 1000;

                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            set
            {

                _TonnageComputable = value;
            }
        }

        private decimal? _WeightPerPCComputable;
        public decimal? WeightPerPCComputable
        {

            get
            {

                if (_WeightPerPCComputable.HasValue)
                {
                    return _WeightPerPCComputable.Value;
                }
                else
                {
                    try
                    {
                        decimal tempLenghthOfFinalPC = LengthOfFinalPC.HasValue ? LengthOfFinalPC.Value : decimal.Zero;
                        decimal tempWeightPerMeter = WeightPerMeter.HasValue ? WeightPerMeter.Value : decimal.Zero;

                        return tempLenghthOfFinalPC / 1000 * tempWeightPerMeter / 1000;

                    }
                    catch
                    {
                        return Decimal.Zero;
                    }
                }
            }
            set
            {

                _WeightPerPCComputable = value;
            }
        }


        private decimal? _Material_EUR_Computable;
        public decimal? Material_EUR_Computable
        {

            get
            {

                if (_Material_EUR_Computable.HasValue)
                {
                    return _Material_EUR_Computable.Value;
                }
                else
                {
                    try
                    {
                        decimal tmpTonnage = Tonnage.HasValue ? Tonnage.Value : decimal.Zero;

                        return MaterialComputable * tmpTonnage;

                    }
                    catch
                    {
                        return Decimal.Zero;
                    }
                }
            }
            set
            {

                _Material_EUR_Computable = value;
            }
        }

        
        private decimal? _Transportation_EUR_Computable;
        public decimal? Transportation_EUR_Computable
        {

            get
            {

                if (_Transportation_EUR_Computable.HasValue)
                {
                    return _Transportation_EUR_Computable.Value;
                }
                else
                {
                    try
                    {
                        decimal tmpTonnage = Tonnage.HasValue ? Tonnage.Value : decimal.Zero;
                        decimal tmpTransportationCost = TransportationCost.HasValue ? TransportationCost.Value : decimal.Zero;


                        return tmpTransportationCost  * tmpTonnage;

                    }
                    catch
                    {
                        return Decimal.Zero;
                    }
                }
            }
            set
            {

                _Transportation_EUR_Computable = value;
            }
        }

        private decimal? _Commission_EUR_Computable;
        public decimal? Commission_EUR_Computable
        {

            get
            {

                if (_Commission_EUR_Computable.HasValue)
                {
                    return _Commission_EUR_Computable.Value;
                }
                else
                {
                    try
                    {
                        if (!this.idAgent.HasValue)
                        {
                            return Decimal.Zero;
                        }

                        KeyValue kvExport = new  KeyValueBL().GetKeyValueByIntCode("Commision","Export");

                        if(!idCommision.HasValue || idCommision != kvExport.idKeyValue )
                        {
                            return Decimal.Zero;
                        }
                        CommissionsByAgent commissionsByAgent = new CommissionsByAgentBL().GetCommissionsByAgentByDateActiveTo(this.idAgent.Value, this.OfferDate);

                        KeyValue kvFixedCommissionPerTon = new  KeyValueBL().GetKeyValueByIntCode("CalculationCommission","FixedCommissionPerTon");
                        KeyValue kvPercentFinalInvoiceValue = new  KeyValueBL().GetKeyValueByIntCode("CalculationCommission","PercentFinalInvoiceValue");


                        if(commissionsByAgent != null)
                        {
                            if(idCalculationCommission == kvFixedCommissionPerTon.idKeyValue)
                            {
                               return BaseHelper.ConvertToDecimalOrZero(this.Tonnage.Value) * BaseHelper.ConvertToDecimalOrZero(commissionsByAgent.FixedCommission);
                            }
                            else if (idCalculationCommission == kvPercentFinalInvoiceValue.idKeyValue)
                            {

                                
                                //return Decimal.Zero;

                                Decimal? res = decimal.Zero;


                                Decimal? man = this.Extrusion_EUR_Computable + Packaging_EUR_Computable;
                                


                                //res = commissionsByAgent.CommissionPercent / 100 * (
                                //    (man + Transportation_EUR_Computable ) * (1 + DaysOfCredit / 360 * Interest /100) + this.AdministrationExpenses * this.Tonnage + this.SalesExpenses * this.Tonnage + this.FinancialFixedExpenses * this.Tonnage
                                //    )
                                //    /
                                //    ( 1 - commissionsByAgent.CommissionPercent / 100 - DaysOfCredit / 360 * Interest / 100);

                                res = commissionsByAgent.CommissionPercent / 100 * (man + Transportation_EUR_Computable + this.AdministrationExpenses * this.Tonnage + this.SalesExpenses * this.Tonnage + this.FinancialFixedExpenses * this.Tonnage + DaysOfCredit / 360 * Interest / 100 * this.Tonnage * (man + Transportation_EUR_Computable)) 
                                    / (
                                    1 - DaysOfCredit / 360 * Interest /100
                                    );

                                return res;
                                //ProductCostResult pcr =  new ProductCostBL().LoadProductCostsByOfferId(idOffer, new CallContext());

                                //List<string> listRowKeyIntCode = new List<string>()
                                //{
                                //    "TotalExtrusionCost", "TotalPackagingCost", "TotalTransportationCostAndCommission", "TotalSGAsAndFinancialCost"
                                //};

                                //List<ProductCostDataView> listPCDV = pcr.ListProductCosts.Where(p => listRowKeyIntCode.Contains(p.RowKeyIntCode) && p.RowType == "Total").ToList();

                                //Decimal total = listPCDV.Where(s => s.Value_EUR_ton.HasValue).Sum(s => s.Value_EUR_ton.Value);

                    
                                //return commissionsByAgent.CommissionPercent / 100 * total  / 
                                //        (1 - commissionsByAgent.CommissionPercent /100 ) * 
                                //        BaseHelper.ConvertToDecimalOrZero(this.Tonnage.Value);

                            }
                            else
                            {
                                return Decimal.Zero;
                            }               
                        }
                        else
                        {
                            return Decimal.Zero;
                        }
                    }
                    catch
                    {
                        return Decimal.Zero;
                    }
                }
            }
            set
            {

                _Commission_EUR_Computable = value;
            }
        }
        
        
        private decimal? _SGAsAndFin_EUR_TON_Computable;
        public decimal? SGAsAndFin_EUR_TON_Computable
        {

            get
            {

                if (_SGAsAndFin_EUR_TON_Computable.HasValue)
                {
                    return _SGAsAndFin_EUR_TON_Computable.Value;
                }
                else
                {
                    try
                    {
                        decimal tmpAdministrationExpenses = AdministrationExpenses.HasValue ? AdministrationExpenses.Value : decimal.Zero;
                        decimal tmpSalesExpenses = SalesExpenses.HasValue ? SalesExpenses.Value : decimal.Zero;
                        decimal tmpFinancialFixedExpenses = FinancialFixedExpenses.HasValue ? FinancialFixedExpenses.Value : decimal.Zero;
                        decimal tmpFinancialVariableExpenses = FinancialVariableExpenses.HasValue ? FinancialVariableExpenses.Value : decimal.Zero;

                        


                        return tmpAdministrationExpenses  + tmpSalesExpenses+ tmpFinancialFixedExpenses+tmpFinancialVariableExpenses;

                    }
                    catch
                    {
                        return Decimal.Zero;
                    }
                }
            }
            set
            {

                _SGAsAndFin_EUR_TON_Computable = value;
            }
        }


        private decimal? _SGAsAndFin_EUR_Computable;
        public decimal? SGAsAndFin_EUR_Computable
        {

            get
            {

                if (_SGAsAndFin_EUR_Computable.HasValue)
                {
                    return _SGAsAndFin_EUR_Computable.Value;
                }
                else
                {
                    try
                    {
                       
                        return SGAsAndFin_EUR_TON_Computable  *  (Tonnage.HasValue ? Tonnage.Value : Decimal.Zero);

                    }
                    catch
                    {
                        return Decimal.Zero;
                    }
                }
            }
            set
            {

                _SGAsAndFin_EUR_Computable = value;
            }
        }

        private decimal? _TotalSalesPrice_EUR_TON_Computable ;
        public decimal?  TotalSalesPrice_EUR_TON_Computable 
        {

            get
            {

                if (_TotalSalesPrice_EUR_TON_Computable.HasValue)
                {
                    return _TotalSalesPrice_EUR_TON_Computable.Value;
                }
                else
                {
                    try
                    {
                        
                        OfferOverviewDataView totalSalesPrice = this.offerOverviewResult.ListOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == "TotalSalesPrice_EUR_TON");

                       if (totalSalesPrice != null && totalSalesPrice.Indicator_Value.HasValue)
                       {
                           return totalSalesPrice.Indicator_Value.Value;
                       }
                       else
                       {
                           return Decimal.Zero;
                       }

                    }
                    catch
                    {
                        return Decimal.Zero;
                    }
                }
            }
            set
            {

                _TotalSalesPrice_EUR_TON_Computable = value;
            }
        }
        

        private decimal? _TotalSalesPrice_EUR_Computable ;
        public decimal?  TotalSalesPrice_EUR_Computable 
        {

            get
            {

                if (_TotalSalesPrice_EUR_Computable.HasValue)
                {
                    return _TotalSalesPrice_EUR_Computable.Value;
                }
                else
                {
                    try
                    {
                        
                        OfferOverviewDataView totalSalesPrice = this.offerOverviewResult.ListOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == "TotalSalesPrice_EUR_TON");

                       if (totalSalesPrice != null && totalSalesPrice.Indicator_Value.HasValue)
                       {
                           return totalSalesPrice.Indicator_Value.Value * (Tonnage.HasValue ? Tonnage.Value : Decimal.Zero);
                       }
                       else
                       {
                           return Decimal.Zero;
                       }

                    }
                    catch
                    {
                        return Decimal.Zero;
                    }
                }
            }
            set
            {

                _TotalSalesPrice_EUR_Computable = value;
            }
        }


       
        public string OfferedSalesPriceToCustomer_EUR_Str
        {
            get
            {
                try
                {
                    if (ActualTotalSalesPriceToCustomer.HasValue)
                    {
                        return (ActualTotalSalesPriceToCustomer.Value * this.Tonnage).ToStringFormatted();
                    }
                    else
                    {
                        return Decimal.Zero.ToString("N", BaseHelper.GetNumberFormatInfo(".", ",", 4));
                    }
                }
                catch
                {
                    return Decimal.Zero.ToString("N", BaseHelper.GetNumberFormatInfo(".", ",", 4));
                }
            }
        }

        private decimal? _Extrusion_EUR_TON_Computable ;
        public decimal?  Extrusion_EUR_TON_Computable 
        {

            get
            {

                if (_Extrusion_EUR_TON_Computable.HasValue)
                {
                    return _Extrusion_EUR_TON_Computable.Value;
                }
                else
                {
                    try
                    {
                      

                       ProductCostDataView totalConversionCost = this.productCostResult.ListProductCosts.FirstOrDefault(p => p.ProductCostType == "Extrusion" && p.RowKeyIntCode == "TotalExtrusionConversionCost");

                       if (totalConversionCost != null)
                       {
                           return totalConversionCost.Value_EUR_ton;
                       }
                       else
                       {
                           return Decimal.Zero;
                       }

                    }
                    catch
                    {
                        return Decimal.Zero;
                    }
                }
            }
            set
            {

                _Extrusion_EUR_TON_Computable = value;
            }
        }

        
        private decimal? _Extrusion_EUR_Computable ;
        public decimal?  Extrusion_EUR_Computable 
        {

            get
            {

                if (_Extrusion_EUR_Computable.HasValue)
                {
                    return _Extrusion_EUR_Computable.Value;
                }
                else
                {
                    try
                    {
                       

                       ProductCostDataView totalConversionCost = this.productCostResult.ListProductCosts.FirstOrDefault(p => p.ProductCostType == "Extrusion" && p.RowKeyIntCode == "TotalExtrusionConversionCost");

                       if (totalConversionCost != null)
                       {
                           return totalConversionCost.Value_EUR_ton * this.Tonnage;
                       }
                       else
                       {
                           return Decimal.Zero;
                       }

                    }
                    catch
                    {
                        return Decimal.Zero;
                    }
                }
            }
            set
            {

                _Extrusion_EUR_Computable = value;
            }
        }

        

        private decimal? _Packaging_EUR_TON_Computable ;
        public decimal?  Packaging_EUR_TON_Computable 
        {

            get
            {

                if (_Packaging_EUR_TON_Computable.HasValue)
                {
                    return _Packaging_EUR_TON_Computable.Value;
                }
                else
                {
                    try
                    {
                       

                       ProductCostDataView totalPackagingCost = this.productCostResult.ListProductCosts.FirstOrDefault(p => p.ProductCostType == "Packaging" && p.RowKeyIntCode == "TotalPackagingCost");

                       if (totalPackagingCost != null)
                       {
                           return totalPackagingCost.Value_EUR_ton;
                       }
                       else
                       {
                           return Decimal.Zero;
                       }

                    }
                    catch
                    {
                        return Decimal.Zero;
                    }
                }
            }
            set
            {

                _Packaging_EUR_TON_Computable = value;
            }
        }

        public decimal? Lifespan_Computable
        {
            get
            {
                Decimal tmpLifespan = Decimal.Zero;
                
                Decimal tmpSDI = 0, tmpValueForPout = 1, tmpValueForPin = 0;
                
                if(!ValueForS.HasValue || !this.DieDimensions.HasValue )
                {
                    return tmpLifespan;
                }

                KeyValueBL kvBL = new KeyValueBL();

                if (ValueForPout.HasValue && ValueForPout.Value != decimal.Zero)
                {
                    tmpValueForPout = ValueForPout.Value;
                }


                if (ValueForPin.HasValue && ValueForPin.Value != decimal.Zero)
                {
                    tmpValueForPin = ValueForPin.Value;
                }

                if (SDI.HasValue)
                {
                    tmpSDI = SDI.Value;
                }

                Decimal numberOfCavitie = decimal.Zero;
                KeyValue kvNumberOfCavitie = kvBL.GetEntityById(this.idNumberOfCavities.Value);
                numberOfCavitie = Decimal.Parse(kvNumberOfCavitie.DefaultValue1);

                decimal tmpDieDifficultyIndex = (this.DieDimensions.Value / this.ValueForS.Value) / (1 - tmpValueForPin/ (tmpValueForPin + tmpValueForPout)) * numberOfCavitie;

                this.DieDifficultyIndex = tmpDieDifficultyIndex;

                foreach (KeyValue kvSPBN in kvBL.GetAllKeyValueByKeyTypeIntCode("SPBN"))
                {
                    tmpLifespan += Math.Truncate( Decimal.Parse(kvSPBN.DefaultValue1) * Convert.ToDecimal( Math.Sqrt( Convert.ToDouble( tmpSDI ) / Convert.ToDouble( tmpDieDifficultyIndex)) ));
                }

                return tmpLifespan * (this.AverageOutturnOverTime.HasValue ? this.AverageOutturnOverTime : Decimal.Zero) / 1000;
               
            }
            
        }
        

        private decimal? _Packaging_EUR_Computable ;
        public decimal?  Packaging_EUR_Computable 
        {

            get
            {

                if (_Packaging_EUR_Computable.HasValue)
                {
                    return _Packaging_EUR_Computable.Value;
                }
                else
                {
                    try
                    {
                     

                       ProductCostDataView totalPackagingCost = this.productCostResult.ListProductCosts.FirstOrDefault(p => p.ProductCostType == "Packaging" && p.RowKeyIntCode == "TotalPackagingCost");

                       if (totalPackagingCost != null)
                       {
                           return totalPackagingCost.Value_EUR_ton * this.Tonnage;
                       }
                       else
                       {
                           return Decimal.Zero;
                       }

                    }
                    catch
                    {
                        return Decimal.Zero;
                    }
                }
            }
            set
            {

                _Packaging_EUR_Computable = value;
            }
        }

        
    }
}