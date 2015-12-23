using ETEMModel.Helpers.Extentions;
using ETEMModel.Models;
using ETEMModel.Models.DataView;
using ETEMModel.Models.DataView.CostCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MoreLinq;
using ETEMModel.Helpers.Admin;
using System.Web.UI.WebControls;

namespace ETEMModel.Helpers.CostCalculation
{
    public class ProductCostBL
    {
        private ETEMDataModelEntities dbContext;

        public ProductCostBL()
        {
            dbContext = new ETEMDataModelEntities();
        }

        public ProductCostResult LoadProductCostsByOfferId(int idOffer, CallContext resultContext)
        {
            ProductCostResult productCostResult = new ProductCostResult();

            List<ProductCostDataView> listProductCosts = new List<ProductCostDataView>();

            try
            {
                productCostResult.ResultContext = resultContext;
                productCostResult.ResultContext.ResultCode = ETEMEnums.ResultEnum.Success;

                List<string> listKeyTypeIntCodes = new List<string>()
                {
                    ETEMEnums.KeyTypeEnum.CostCenter.ToString(),
                    ETEMEnums.KeyTypeEnum.ExpensesTypeGroup.ToString()
                };

                List<KeyValueDataView> listKeyValuesToSAPDataCostCenterAndExpensesTypeGroup = new List<KeyValueDataView>();
                List<KeyValueDataView> listKeyValueCostCenter = new List<KeyValueDataView>();
                List<KeyValueDataView> listKeyValueCostCenterWithoutPackaging = new List<KeyValueDataView>();
                List<KeyValueDataView> listKeyValueCostCenterWithPackaging = new List<KeyValueDataView>();
                List<KeyValueDataView> listKeyValueExpensesTypeGroup = new List<KeyValueDataView>();

                listKeyValuesToSAPDataCostCenterAndExpensesTypeGroup = (from kv in this.dbContext.KeyValues
                                                                        join kt in this.dbContext.KeyTypes on kv.idKeyType equals kt.idKeyType
                                                                        where listKeyTypeIntCodes.Contains(kt.KeyTypeIntCode)
                                                                        orderby kv.V_Order ascending
                                                                        select new KeyValueDataView
                                                                            {
                                                                                idKeyValue = kv.idKeyValue,
                                                                                Name = kv.Name,
                                                                                NameEN = kv.NameEN,
                                                                                DefaultValue1 = kv.DefaultValue1,
                                                                                KeyValueIntCode = kv.KeyValueIntCode,
                                                                                KeyTypeIntCode = kt.KeyTypeIntCode
                                                                            }
                                                                        ).ToList<KeyValueDataView>();

                Offer offer = new OfferBL().GetEntityById(idOffer);
                List<OfferDataExpenseGroupView> listGroupExpense = new OfferDataExpenseGroupBL().GetAllOfferDataExpenseGroupByOffer(idOffer);
                OfferProducitivity offerProducitivity = new OfferProducitivityBL().GetOfferProducitivityByOfferID(idOffer);

                List<int> listCostCenterIDs = listGroupExpense.DistinctBy(k => k.idCostCenter).Select(k => k.idCostCenter).ToList();

                listKeyValueCostCenter = listKeyValuesToSAPDataCostCenterAndExpensesTypeGroup.Where(w => listCostCenterIDs.Contains(w.idKeyValue)).ToList();

                listKeyValueCostCenterWithoutPackaging = listKeyValueCostCenter.Where(w => w.KeyValueIntCode != ETEMEnums.CostCenterEnum.Packaging.ToString()).ToList();
                listKeyValueCostCenterWithPackaging = listKeyValueCostCenter.Where(w => w.KeyValueIntCode == ETEMEnums.CostCenterEnum.Packaging.ToString()).ToList();

                listKeyValueExpensesTypeGroup = listKeyValuesToSAPDataCostCenterAndExpensesTypeGroup.Where(w => w.KeyTypeIntCode == ETEMEnums.KeyTypeEnum.ExpensesTypeGroup.ToString()).ToList();

                decimal weightPerPC = offer.WeightPerPC.HasValue ? offer.WeightPerPC.Value : decimal.Zero;

                decimal currValue = decimal.Zero;

                ProductCostDataView productCostDataView = new ProductCostDataView();

                #region Extrusion

                decimal totalExtrusion = decimal.Zero;

                decimal totalExtrusionCost_ton = decimal.Zero;
                decimal totalExtrusionConversionCost_ton = decimal.Zero;
                decimal totalExtrusionConversionCostWithoutDepreciation_ton = decimal.Zero;
                decimal totalVariableExpenses_ton = decimal.Zero;
                decimal totalFixedExpenses_ton = decimal.Zero;
                decimal totalDepreciations_ton = decimal.Zero;

                decimal totalExtrusionCost_kg = decimal.Zero;
                decimal totalExtrusionConversionCost_kg = decimal.Zero;
                decimal totalExtrusionConversionCostWithoutDepreciation_kg = decimal.Zero;
                decimal totalVariableExpenses_kg = decimal.Zero;
                decimal totalFixedExpenses_kg = decimal.Zero;
                decimal totalDepreciations_kg = decimal.Zero;

                decimal totalExtrusionCost_PC = decimal.Zero;
                decimal totalExtrusionConversionCost_PC = decimal.Zero;
                decimal totalExtrusionConversionCostWithoutDepreciation_PC = decimal.Zero;
                decimal totalVariableExpenses_PC = decimal.Zero;
                decimal totalFixedExpenses_PC = decimal.Zero;
                decimal totalDepreciations_PC = decimal.Zero;

                ProductCostDataView productCostDieHandlingCostDataView = new ProductCostDataView();

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "Extrusion";
                productCostDataView.RowType = "Data";
                productCostDataView.Name = "LME";
                productCostDataView.RowKeyIntCode = "LME";
                productCostDataView.Value_EUR_ton = offer.LME;
                productCostDataView.Value_EUR_kg = (productCostDataView.Value_EUR_ton.HasValue ? productCostDataView.Value_EUR_ton.Value / 1000 : decimal.Zero);
                productCostDataView.Value_EUR_PC = (productCostDataView.Value_EUR_kg.HasValue ? productCostDataView.Value_EUR_kg.Value * weightPerPC : decimal.Zero);

                listProductCosts.Add(productCostDataView);

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "Extrusion";
                productCostDataView.RowType = "Data";
                productCostDataView.Name = "PREMIUM";
                productCostDataView.RowKeyIntCode = "PREMIUM";
                productCostDataView.Value_EUR_ton = offer.PREMIUM;
                productCostDataView.Value_EUR_kg = (productCostDataView.Value_EUR_ton.HasValue ? productCostDataView.Value_EUR_ton.Value / 1000 : decimal.Zero);
                productCostDataView.Value_EUR_PC = (productCostDataView.Value_EUR_kg.HasValue ? productCostDataView.Value_EUR_kg.Value * weightPerPC : decimal.Zero);

                listProductCosts.Add(productCostDataView);

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "Extrusion";
                productCostDataView.RowType = "Data";
                productCostDataView.Name = "Cost of Billet Scrap";
                productCostDataView.RowKeyIntCode = "Cost_Of_Billet_Scrap";
                productCostDataView.Value_EUR_ton = offer.CostOfScrap;
                productCostDataView.Value_EUR_kg = (productCostDataView.Value_EUR_ton.HasValue ? productCostDataView.Value_EUR_ton.Value / 1000 : decimal.Zero);
                productCostDataView.Value_EUR_PC = (productCostDataView.Value_EUR_kg.HasValue ? productCostDataView.Value_EUR_kg.Value * weightPerPC : decimal.Zero);

                listProductCosts.Add(productCostDataView);

                foreach (KeyValueDataView kvExpensesTypeGroup in listKeyValueExpensesTypeGroup)
                {
                    totalExtrusion = decimal.Zero;

                    foreach (KeyValueDataView kvCostCenter in listKeyValueCostCenterWithoutPackaging)
                    {
                        var currGroupExpense = listGroupExpense.Where(w => w.idExpensesType == kvExpensesTypeGroup.idKeyValue &&
                                                                      w.idCostCenter == kvCostCenter.idKeyValue).FirstOrDefault();

                        if (currGroupExpense != null)
                        {
                            currValue = Convert.ToDecimal(currGroupExpense.ValueData);

                            #region Calculate EUR per TON
                            if (kvCostCenter.DefaultValue1 == "Press")
                            {
                                if (offerProducitivity != null && offerProducitivity.PressProducitivity_TON_MH != decimal.Zero)
                                {
                                    totalExtrusion += currValue / offerProducitivity.PressProducitivity_TON_MH;
                                }
                                else
                                {
                                    totalExtrusion += currValue;
                                }
                            }
                            else if (kvCostCenter.DefaultValue1 == "COMETAL")
                            {
                                if (offerProducitivity != null && offerProducitivity.COMetalProducitivity_TON_MH != decimal.Zero)
                                {
                                    totalExtrusion += currValue / offerProducitivity.COMetalProducitivity_TON_MH;
                                }
                                else
                                {
                                    totalExtrusion += currValue;
                                }
                            }
                            else if (kvCostCenter.DefaultValue1 == "QualityControl")
                            {
                                if (offerProducitivity != null && offerProducitivity.QCProducitivity_TON_MH != decimal.Zero)
                                {
                                    totalExtrusion += currValue / offerProducitivity.QCProducitivity_TON_MH;
                                }
                                else
                                {
                                    totalExtrusion += currValue;
                                }
                            }
                            else if (kvCostCenter.DefaultValue1 == "DIES")
                            {
                                if (kvExpensesTypeGroup.KeyValueIntCode == ETEMEnums.ExpensesTypeGroupEnum.IndirectDepartmentExpensesGroup.ToString())
                                {
                                    productCostDieHandlingCostDataView = new ProductCostDataView();

                                    productCostDieHandlingCostDataView.ProductCostType = "Extrusion";
                                    productCostDieHandlingCostDataView.RowType = "Data";
                                    productCostDieHandlingCostDataView.Name = "Die handling cost";
                                    productCostDieHandlingCostDataView.RowKeyIntCode = "DieHandlingCost";
                                    productCostDieHandlingCostDataView.Value_EUR_ton = currValue * 1000;
                                    productCostDieHandlingCostDataView.Value_EUR_kg = currValue;
                                    productCostDieHandlingCostDataView.Value_EUR_PC = currValue * weightPerPC;
                                }
                                else
                                {
                                    if (kvExpensesTypeGroup.KeyValueIntCode != ETEMEnums.ExpensesTypeGroupEnum.DirectDepreciationGroup.ToString())
                                    {
                                        totalExtrusion += currValue * 1000; //For DIES Department formula is different: '=Value (EUR/kg)*1000
                                    }
                                    else
                                    {

                                    }
                                }
                            }
                            else
                            {
                                totalExtrusion += decimal.Zero;
                            }
                            #endregion
                        }
                    }

                    productCostDataView = new ProductCostDataView();

                    productCostDataView.ProductCostType = "Extrusion";
                    productCostDataView.RowType = "Data";
                    productCostDataView.Name = kvExpensesTypeGroup.Name;
                    productCostDataView.RowKeyIntCode = kvExpensesTypeGroup.KeyValueIntCode;
                    productCostDataView.Value_EUR_ton = totalExtrusion;
                    productCostDataView.Value_EUR_kg = totalExtrusion / 1000;
                    productCostDataView.Value_EUR_PC = (totalExtrusion / 1000) * weightPerPC;

                    listProductCosts.Add(productCostDataView);

                    if (kvExpensesTypeGroup.KeyValueIntCode == ETEMEnums.ExpensesTypeGroupEnum.IndirectDepartmentExpensesGroup.ToString())
                    {
                        listProductCosts.Add(productCostDieHandlingCostDataView);
                    }
                }

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "Extrusion";
                productCostDataView.RowType = "Data";
                productCostDataView.Name = "Die depreciation";
                productCostDataView.RowKeyIntCode = "DieDepreciation";
                productCostDataView.Value_EUR_ton = (offer.CostOfDieEUR_Per_TON_Computable.HasValue ? offer.CostOfDieEUR_Per_TON_Computable.Value : 0);
                productCostDataView.Value_EUR_kg = productCostDataView.Value_EUR_ton / 1000;
                productCostDataView.Value_EUR_PC = productCostDataView.Value_EUR_kg * weightPerPC;

                listProductCosts.Add(productCostDataView);

                List<string> listVariableExpenses = new List<string>();
                listVariableExpenses.Add(ETEMEnums.ExpensesTypeGroupEnum.ElectricityGroup.ToString());
                listVariableExpenses.Add(ETEMEnums.ExpensesTypeGroupEnum.NaturalGasGroup.ToString());
                listVariableExpenses.Add(ETEMEnums.ExpensesTypeGroupEnum.ProdMaterialGroup.ToString());

                List<string> listFixedExpenses = new List<string>();
                listFixedExpenses.Add(ETEMEnums.ExpensesTypeGroupEnum.MaintenSparePartGroup.ToString());
                listFixedExpenses.Add(ETEMEnums.ExpensesTypeGroupEnum.OtherFixedCostsGroup.ToString());
                listFixedExpenses.Add(ETEMEnums.ExpensesTypeGroupEnum.IndirectDepartmentExpensesGroup.ToString());
                listFixedExpenses.Add("DieHandlingCost");
                listFixedExpenses.Add(ETEMEnums.ExpensesTypeGroupEnum.FeesAndOtherMaintenanceExpensesGroup.ToString());
                listFixedExpenses.Add(ETEMEnums.ExpensesTypeGroupEnum.OtherIndirectExpensesGroup.ToString());

                List<string> listDepreciations = new List<string>();
                listDepreciations.Add(ETEMEnums.ExpensesTypeGroupEnum.DirectDepreciationGroup.ToString());
                listDepreciations.Add(ETEMEnums.ExpensesTypeGroupEnum.IndirectDepreciationGroup.ToString());
                listDepreciations.Add("DieDepreciation");

                foreach (ProductCostDataView productCost in listProductCosts.Where(w => w.ProductCostType == "Extrusion").ToList())
                {
                    totalExtrusionCost_ton += (productCost.Value_EUR_ton.HasValue ? productCost.Value_EUR_ton.Value : decimal.Zero);
                    totalExtrusionCost_kg += (productCost.Value_EUR_kg.HasValue ? productCost.Value_EUR_kg.Value : decimal.Zero);
                    totalExtrusionCost_PC += (productCost.Value_EUR_PC.HasValue ? productCost.Value_EUR_PC.Value : decimal.Zero);

                    if (productCost.RowKeyIntCode != "LME" && productCost.RowKeyIntCode != "PREMIUM")
                    {
                        totalExtrusionConversionCost_ton += (productCost.Value_EUR_ton.HasValue ? productCost.Value_EUR_ton.Value : decimal.Zero);
                        totalExtrusionConversionCost_kg += (productCost.Value_EUR_kg.HasValue ? productCost.Value_EUR_kg.Value : decimal.Zero);
                        totalExtrusionConversionCost_PC += (productCost.Value_EUR_PC.HasValue ? productCost.Value_EUR_PC.Value : decimal.Zero);
                    }

                    if (productCost.RowKeyIntCode != "LME" && productCost.RowKeyIntCode != "PREMIUM" &&
                        !listDepreciations.Contains(productCost.RowKeyIntCode))
                    {
                        totalExtrusionConversionCostWithoutDepreciation_ton += (productCost.Value_EUR_ton.HasValue ? productCost.Value_EUR_ton.Value : decimal.Zero);
                        totalExtrusionConversionCostWithoutDepreciation_kg += (productCost.Value_EUR_kg.HasValue ? productCost.Value_EUR_kg.Value : decimal.Zero);
                        totalExtrusionConversionCostWithoutDepreciation_PC += (productCost.Value_EUR_PC.HasValue ? productCost.Value_EUR_PC.Value : decimal.Zero);
                    }

                    if (listVariableExpenses.Contains(productCost.RowKeyIntCode))
                    {
                        totalVariableExpenses_ton += (productCost.Value_EUR_ton.HasValue ? productCost.Value_EUR_ton.Value : decimal.Zero);
                        totalVariableExpenses_kg += (productCost.Value_EUR_kg.HasValue ? productCost.Value_EUR_kg.Value : decimal.Zero);
                        totalVariableExpenses_PC += (productCost.Value_EUR_PC.HasValue ? productCost.Value_EUR_PC.Value : decimal.Zero);
                    }

                    if (listFixedExpenses.Contains(productCost.RowKeyIntCode))
                    {
                        totalFixedExpenses_ton += (productCost.Value_EUR_ton.HasValue ? productCost.Value_EUR_ton.Value : decimal.Zero);
                        totalFixedExpenses_kg += (productCost.Value_EUR_kg.HasValue ? productCost.Value_EUR_kg.Value : decimal.Zero);
                        totalFixedExpenses_PC += (productCost.Value_EUR_PC.HasValue ? productCost.Value_EUR_PC.Value : decimal.Zero);
                    }

                    if (listDepreciations.Contains(productCost.RowKeyIntCode))
                    {
                        totalDepreciations_ton += (productCost.Value_EUR_ton.HasValue ? productCost.Value_EUR_ton.Value : decimal.Zero);
                        totalDepreciations_kg += (productCost.Value_EUR_kg.HasValue ? productCost.Value_EUR_kg.Value : decimal.Zero);
                        totalDepreciations_PC += (productCost.Value_EUR_PC.HasValue ? productCost.Value_EUR_PC.Value : decimal.Zero);
                    }
                }

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "Extrusion";
                productCostDataView.RowType = "Total";
                productCostDataView.Name = "Total Extrusion Cost (incl. Aluminium)";
                productCostDataView.RowKeyIntCode = "TotalExtrusionCost";
                productCostDataView.Value_EUR_ton = totalExtrusionCost_ton;
                productCostDataView.Value_EUR_kg = totalExtrusionCost_kg;
                productCostDataView.Value_EUR_PC = totalExtrusionCost_PC;

                listProductCosts.Add(productCostDataView);

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "Extrusion";
                productCostDataView.RowType = "Total";
                productCostDataView.Name = "Extrusion Conversion cost (excl. Aluminium)";
                productCostDataView.RowKeyIntCode = "TotalExtrusionConversionCost";
                productCostDataView.Value_EUR_ton = totalExtrusionConversionCost_ton;
                productCostDataView.Value_EUR_kg = totalExtrusionConversionCost_kg;
                productCostDataView.Value_EUR_PC = totalExtrusionConversionCost_PC;

                listProductCosts.Add(productCostDataView);

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "Extrusion";
                productCostDataView.RowType = "Total";
                productCostDataView.Name = "Extrusion Conversion cost (excl. Aluminium and Depreciation)";
                productCostDataView.RowKeyIntCode = "TotalExtrusionConversionCostWithoutDepreciation";
                productCostDataView.Value_EUR_ton = totalExtrusionConversionCostWithoutDepreciation_ton;
                productCostDataView.Value_EUR_kg = totalExtrusionConversionCostWithoutDepreciation_kg;
                productCostDataView.Value_EUR_PC = totalExtrusionConversionCostWithoutDepreciation_PC;

                listProductCosts.Add(productCostDataView);

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "Extrusion";
                productCostDataView.RowType = "Total";
                productCostDataView.Name = "Variable Expenses";
                productCostDataView.RowKeyIntCode = "VariableExpenses";
                productCostDataView.Value_EUR_ton = totalVariableExpenses_ton;
                productCostDataView.Value_EUR_kg = totalVariableExpenses_kg;
                productCostDataView.Value_EUR_PC = totalVariableExpenses_PC;

                listProductCosts.Add(productCostDataView);

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "Extrusion";
                productCostDataView.RowType = "Total";
                productCostDataView.Name = "Fixed Expenses";
                productCostDataView.RowKeyIntCode = "FixedExpenses";
                productCostDataView.Value_EUR_ton = totalFixedExpenses_ton;
                productCostDataView.Value_EUR_kg = totalFixedExpenses_kg;
                productCostDataView.Value_EUR_PC = totalFixedExpenses_PC;

                listProductCosts.Add(productCostDataView);

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "Extrusion";
                productCostDataView.RowType = "Total";
                productCostDataView.Name = "Depreciation";
                productCostDataView.RowKeyIntCode = "Depreciation";
                productCostDataView.Value_EUR_ton = totalDepreciations_ton;
                productCostDataView.Value_EUR_kg = totalDepreciations_kg;
                productCostDataView.Value_EUR_PC = totalDepreciations_PC;

                listProductCosts.Add(productCostDataView);

                #endregion

                #region Packaging

                decimal totalPackaging = decimal.Zero;

                decimal totalPackagingCost_ton = decimal.Zero;
                decimal totalPackagingCostWithoutDepreciation_ton = decimal.Zero;
                decimal totalPackVariableExpenses_ton = decimal.Zero;
                decimal totalPackFixedExpenses_ton = decimal.Zero;
                decimal totalPackDepreciations_ton = decimal.Zero;

                decimal totalPackagingCost_kg = decimal.Zero;
                decimal totalPackagingCostWithoutDepreciation_kg = decimal.Zero;
                decimal totalPackVariableExpenses_kg = decimal.Zero;
                decimal totalPackFixedExpenses_kg = decimal.Zero;
                decimal totalPackDepreciations_kg = decimal.Zero;
                
                decimal totalPackagingCost_PC = decimal.Zero;
                decimal totalPackagingCostWithoutDepreciation_PC = decimal.Zero;
                decimal totalPackVariableExpenses_PC = decimal.Zero;
                decimal totalPackFixedExpenses_PC = decimal.Zero;
                decimal totalPackDepreciations_PC = decimal.Zero;

                decimal materialCostForPackaging = decimal.Zero;
                decimal ratioConsumptionPackaging = decimal.Zero;

                if (offer.MaterialCostForPackaging.HasValue)
                {
                    materialCostForPackaging = (offer.MaterialCostForPackaging.HasValue ? offer.MaterialCostForPackaging.Value : decimal.Zero);
                }
                else
                {
                    Setting setting = new SettingBL().GetSettingByCode(ETEMEnums.AppSettings.Material_cost_for_packaging.ToString());
                    if (setting != null)
                    {
                        materialCostForPackaging = BaseHelper.ConvertToDecimalOrZero(setting.SettingValue);
                    }
                }
                if (offer.RatioConsumptionPackaging.HasValue)
                {
                    ratioConsumptionPackaging = (offer.RatioConsumptionPackaging.HasValue ? offer.RatioConsumptionPackaging.Value : decimal.Zero);
                }
                else
                {
                    Setting setting = new SettingBL().GetSettingByCode(ETEMEnums.AppSettings.Ratio_consumption_packaging.ToString());
                    if (setting != null)
                    {
                        ratioConsumptionPackaging = BaseHelper.ConvertToDecimalOrZero(setting.SettingValue);
                    }
                }                

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "Packaging";
                productCostDataView.RowType = "Data";
                productCostDataView.Name = "Material cost";
                productCostDataView.RowKeyIntCode = "MaterialCost";
                productCostDataView.Value_EUR_ton = materialCostForPackaging;
                productCostDataView.Value_EUR_kg = materialCostForPackaging / 1000;
                productCostDataView.Value_EUR_PC = (materialCostForPackaging / 1000) * weightPerPC;

                listProductCosts.Add(productCostDataView);

                decimal costOfConsumedMaterialScrap = decimal.Zero;
                decimal totalExtrusionCost  = decimal.Zero;
                
                ProductCostDataView prodCostDataView = listProductCosts.Where(w => w.ProductCostType == "Extrusion" &&
                                                                              w.RowType == "Total" &&
                                                                              w.RowKeyIntCode == "TotalExtrusionCost").FirstOrDefault();

                totalExtrusionCost = (prodCostDataView != null && prodCostDataView.Value_EUR_ton.HasValue ? prodCostDataView.Value_EUR_ton.Value : decimal.Zero);

                costOfConsumedMaterialScrap = (ratioConsumptionPackaging - 1) *
                                               (totalExtrusionCost - ((offer.ScrapValuePercent.HasValue ? offer.ScrapValuePercent.Value : decimal.Zero) *
                                                                      (offer.LME.HasValue ? offer.LME.Value : decimal.Zero)));

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "Packaging";
                productCostDataView.RowType = "Data";
                productCostDataView.Name = "Cost of Consumed material Scrap";
                productCostDataView.RowKeyIntCode = "CostOfConsumedMaterialScrap";
                productCostDataView.Value_EUR_ton = costOfConsumedMaterialScrap;
                productCostDataView.Value_EUR_kg = costOfConsumedMaterialScrap / 1000;
                productCostDataView.Value_EUR_PC = (costOfConsumedMaterialScrap / 1000) * weightPerPC;

                listProductCosts.Add(productCostDataView);

                foreach (KeyValueDataView kvExpensesTypeGroup in listKeyValueExpensesTypeGroup)
                {
                    totalPackaging = decimal.Zero;

                    foreach (KeyValueDataView kvCostCenter in listKeyValueCostCenterWithPackaging)
                    {
                        var currGroupExpense = listGroupExpense.Where(w => w.idExpensesType == kvExpensesTypeGroup.idKeyValue &&
                                                                      w.idCostCenter == kvCostCenter.idKeyValue).FirstOrDefault();

                        if (currGroupExpense != null)
                        {
                            currValue = Convert.ToDecimal(currGroupExpense.ValueData);

                            #region Calculate EUR per TON
                            if (kvCostCenter.DefaultValue1 == "Packaging")
                            {
                                if (offerProducitivity != null && offerProducitivity.PackagingProducitivity_TON_MH != decimal.Zero)
                                {
                                    totalPackaging += currValue / offerProducitivity.PackagingProducitivity_TON_MH;
                                }
                                else
                                {
                                    totalPackaging += currValue;
                                }
                            }                            
                            else
                            {
                                totalPackaging += decimal.Zero;
                            }
                            #endregion
                        }
                    }

                    productCostDataView = new ProductCostDataView();

                    productCostDataView.ProductCostType = "Packaging";
                    productCostDataView.RowType = "Data";
                    productCostDataView.Name = kvExpensesTypeGroup.Name;
                    productCostDataView.RowKeyIntCode = kvExpensesTypeGroup.KeyValueIntCode;
                    productCostDataView.Value_EUR_ton = totalPackaging;
                    productCostDataView.Value_EUR_kg = totalPackaging / 1000;
                    productCostDataView.Value_EUR_PC = (totalPackaging / 1000) * weightPerPC;

                    listProductCosts.Add(productCostDataView);
                }

                foreach (ProductCostDataView productCost in listProductCosts.Where(w => w.ProductCostType == "Packaging").ToList())
                {
                    totalPackagingCost_ton += (productCost.Value_EUR_ton.HasValue ? productCost.Value_EUR_ton.Value : decimal.Zero);
                    totalPackagingCost_kg += (productCost.Value_EUR_kg.HasValue ? productCost.Value_EUR_kg.Value : decimal.Zero);
                    totalPackagingCost_PC += (productCost.Value_EUR_PC.HasValue ? productCost.Value_EUR_PC.Value : decimal.Zero);

                    if (productCost.RowKeyIntCode != ETEMEnums.ExpensesTypeGroupEnum.DirectDepreciationGroup.ToString() &&
                        productCost.RowKeyIntCode != ETEMEnums.ExpensesTypeGroupEnum.IndirectDepreciationGroup.ToString() &&
                        productCost.RowKeyIntCode != "DieDepreciation")
                    {
                        totalPackagingCostWithoutDepreciation_ton += (productCost.Value_EUR_ton.HasValue ? productCost.Value_EUR_ton.Value : decimal.Zero);
                        totalPackagingCostWithoutDepreciation_kg += (productCost.Value_EUR_kg.HasValue ? productCost.Value_EUR_kg.Value : decimal.Zero);
                        totalPackagingCostWithoutDepreciation_PC += (productCost.Value_EUR_PC.HasValue ? productCost.Value_EUR_PC.Value : decimal.Zero);
                    }

                    if (listVariableExpenses.Contains(productCost.RowKeyIntCode))
                    {
                        totalPackVariableExpenses_ton += (productCost.Value_EUR_ton.HasValue ? productCost.Value_EUR_ton.Value : decimal.Zero);
                        totalPackVariableExpenses_kg += (productCost.Value_EUR_kg.HasValue ? productCost.Value_EUR_kg.Value : decimal.Zero);
                        totalPackVariableExpenses_PC += (productCost.Value_EUR_PC.HasValue ? productCost.Value_EUR_PC.Value : decimal.Zero);
                    }

                    if (listFixedExpenses.Contains(productCost.RowKeyIntCode))
                    {
                        totalPackFixedExpenses_ton += (productCost.Value_EUR_ton.HasValue ? productCost.Value_EUR_ton.Value : decimal.Zero);
                        totalPackFixedExpenses_kg += (productCost.Value_EUR_kg.HasValue ? productCost.Value_EUR_kg.Value : decimal.Zero);
                        totalPackFixedExpenses_PC += (productCost.Value_EUR_PC.HasValue ? productCost.Value_EUR_PC.Value : decimal.Zero);
                    }

                    if (listDepreciations.Contains(productCost.RowKeyIntCode))
                    {
                        totalPackDepreciations_ton += (productCost.Value_EUR_ton.HasValue ? productCost.Value_EUR_ton.Value : decimal.Zero);
                        totalPackDepreciations_kg += (productCost.Value_EUR_kg.HasValue ? productCost.Value_EUR_kg.Value : decimal.Zero);
                        totalPackDepreciations_PC += (productCost.Value_EUR_PC.HasValue ? productCost.Value_EUR_PC.Value : decimal.Zero);
                    }
                }

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "Packaging";
                productCostDataView.RowType = "Total";
                productCostDataView.Name = "Total Packaging Cost";
                productCostDataView.RowKeyIntCode = "TotalPackagingCost";
                productCostDataView.Value_EUR_ton = totalPackagingCost_ton;
                productCostDataView.Value_EUR_kg = totalPackagingCost_kg;
                productCostDataView.Value_EUR_PC = totalPackagingCost_PC;

                listProductCosts.Add(productCostDataView);

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "Packaging";
                productCostDataView.RowType = "Total";
                productCostDataView.Name = "Total Packaging Cost (excl. Depreciation)";
                productCostDataView.RowKeyIntCode = "TotalPackagingCostWithoutDepreciation";
                productCostDataView.Value_EUR_ton = totalPackagingCostWithoutDepreciation_ton;
                productCostDataView.Value_EUR_kg = totalPackagingCostWithoutDepreciation_kg;
                productCostDataView.Value_EUR_PC = totalPackagingCostWithoutDepreciation_PC;

                listProductCosts.Add(productCostDataView);

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "Packaging";
                productCostDataView.RowType = "Total";
                productCostDataView.Name = "Variable Expenses";
                productCostDataView.RowKeyIntCode = "PackagingVariableExpenses";
                productCostDataView.Value_EUR_ton = totalPackVariableExpenses_ton;
                productCostDataView.Value_EUR_kg = totalPackVariableExpenses_kg;
                productCostDataView.Value_EUR_PC = totalPackVariableExpenses_PC;

                listProductCosts.Add(productCostDataView);

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "Packaging";
                productCostDataView.RowType = "Total";
                productCostDataView.Name = "Fixed Expenses";
                productCostDataView.RowKeyIntCode = "PackagingFixedExpenses";
                productCostDataView.Value_EUR_ton = totalPackFixedExpenses_ton;
                productCostDataView.Value_EUR_kg = totalPackFixedExpenses_kg;
                productCostDataView.Value_EUR_PC = totalPackFixedExpenses_PC;

                listProductCosts.Add(productCostDataView);

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "Packaging";
                productCostDataView.RowType = "Total";
                productCostDataView.Name = "Depreciation";
                productCostDataView.RowKeyIntCode = "PackagingDepreciation";
                productCostDataView.Value_EUR_ton = totalPackDepreciations_ton;
                productCostDataView.Value_EUR_kg = totalPackDepreciations_kg;
                productCostDataView.Value_EUR_PC = totalPackDepreciations_PC;

                listProductCosts.Add(productCostDataView);

                #endregion

                #region Transportation Cost & Commission

                decimal commission = decimal.Zero;

                decimal totalTransportationCostAndCommission_ton = decimal.Zero;
                decimal totalTransportationCostAndCommission_kg = decimal.Zero;
                decimal totalTransportationCostAndCommission_PC = decimal.Zero;

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "TransportationCostAndCommission";
                productCostDataView.RowType = "Data";
                productCostDataView.Name = "Transportation";
                productCostDataView.RowKeyIntCode = "Transportation";
                productCostDataView.Value_EUR_ton = (offer.TransportationCost.HasValue ? offer.TransportationCost.Value : decimal.Zero);
                productCostDataView.Value_EUR_kg = (productCostDataView.Value_EUR_ton.HasValue ? productCostDataView.Value_EUR_ton / 1000 : decimal.Zero);
                productCostDataView.Value_EUR_PC = (productCostDataView.Value_EUR_kg.HasValue ? productCostDataView.Value_EUR_kg * weightPerPC : decimal.Zero);

                listProductCosts.Add(productCostDataView);

                commission = (offer.Commission_EUR_Computable.HasValue ? offer.Commission_EUR_Computable.Value : decimal.Zero) / (offer.Tonnage.HasValue && offer.Tonnage.Value != 0 ? offer.Tonnage.Value : 1);

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "TransportationCostAndCommission";
                productCostDataView.RowType = "Data";
                productCostDataView.Name = "Commission";
                productCostDataView.RowKeyIntCode = "Commission";
                productCostDataView.Value_EUR_ton = commission;
                productCostDataView.Value_EUR_kg = (productCostDataView.Value_EUR_ton.HasValue ? productCostDataView.Value_EUR_ton / 1000 : decimal.Zero);
                productCostDataView.Value_EUR_PC = (productCostDataView.Value_EUR_kg.HasValue ? productCostDataView.Value_EUR_kg * weightPerPC : decimal.Zero);

                listProductCosts.Add(productCostDataView);

                foreach (ProductCostDataView productCost in listProductCosts.Where(w => w.ProductCostType == "TransportationCostAndCommission").ToList())
                {
                    totalTransportationCostAndCommission_ton += (productCost.Value_EUR_ton.HasValue ? productCost.Value_EUR_ton.Value : decimal.Zero);
                    totalTransportationCostAndCommission_kg += (productCost.Value_EUR_kg.HasValue ? productCost.Value_EUR_kg.Value : decimal.Zero);
                    totalTransportationCostAndCommission_PC += (productCost.Value_EUR_PC.HasValue ? productCost.Value_EUR_PC.Value : decimal.Zero);
                }

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "TransportationCostAndCommission";
                productCostDataView.RowType = "Total";
                productCostDataView.Name = "Total Transportation Cost & Commission";
                productCostDataView.RowKeyIntCode = "TotalTransportationCostAndCommission";
                productCostDataView.Value_EUR_ton = totalTransportationCostAndCommission_ton;
                productCostDataView.Value_EUR_kg = totalTransportationCostAndCommission_kg;
                productCostDataView.Value_EUR_PC = totalTransportationCostAndCommission_PC;

                listProductCosts.Add(productCostDataView);

                #endregion

                #region SGA's and Financials

                decimal totalSGAsAndFinancials_ton = decimal.Zero;
                decimal totalSGAsAndFinancials_kg = decimal.Zero;
                decimal totalSGAsAndFinancials_PC = decimal.Zero;

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "SGAsAndFinancials";
                productCostDataView.RowType = "Data";
                productCostDataView.Name = "Administration expenses";
                productCostDataView.RowKeyIntCode = "AdministrationExpenses";
                productCostDataView.Value_EUR_ton = (offer.AdministrationExpenses.HasValue ? offer.AdministrationExpenses.Value : decimal.Zero);
                productCostDataView.Value_EUR_kg = (productCostDataView.Value_EUR_ton.HasValue ? productCostDataView.Value_EUR_ton / 1000 : decimal.Zero);
                productCostDataView.Value_EUR_PC = (productCostDataView.Value_EUR_kg.HasValue ? productCostDataView.Value_EUR_kg * weightPerPC : decimal.Zero);

                listProductCosts.Add(productCostDataView);

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "SGAsAndFinancials";
                productCostDataView.RowType = "Data";
                productCostDataView.Name = "Sales expenses";
                productCostDataView.RowKeyIntCode = "SalesExpenses";
                productCostDataView.Value_EUR_ton = (offer.SalesExpenses.HasValue ? offer.SalesExpenses.Value : decimal.Zero);
                productCostDataView.Value_EUR_kg = (productCostDataView.Value_EUR_ton.HasValue ? productCostDataView.Value_EUR_ton / 1000 : decimal.Zero);
                productCostDataView.Value_EUR_PC = (productCostDataView.Value_EUR_kg.HasValue ? productCostDataView.Value_EUR_kg * weightPerPC : decimal.Zero);

                listProductCosts.Add(productCostDataView);

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "SGAsAndFinancials";
                productCostDataView.RowType = "Data";
                productCostDataView.Name = "Financial fixed expenses";
                productCostDataView.RowKeyIntCode = "FinancialFixedExpenses";
                productCostDataView.Value_EUR_ton = (offer.FinancialFixedExpenses.HasValue ? offer.FinancialFixedExpenses.Value : decimal.Zero);
                productCostDataView.Value_EUR_kg = (productCostDataView.Value_EUR_ton.HasValue ? productCostDataView.Value_EUR_ton / 1000 : decimal.Zero);
                productCostDataView.Value_EUR_PC = (productCostDataView.Value_EUR_kg.HasValue ? productCostDataView.Value_EUR_kg * weightPerPC : decimal.Zero);

                listProductCosts.Add(productCostDataView);

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "SGAsAndFinancials";
                productCostDataView.RowType = "Data";
                productCostDataView.Name = "Financial variable expenses";
                productCostDataView.RowKeyIntCode = "FinancialVariableExpenses";
                productCostDataView.Value_EUR_ton = (offer.FinancialVariableExpenses.HasValue ? offer.FinancialVariableExpenses.Value : decimal.Zero);
                productCostDataView.Value_EUR_kg = (productCostDataView.Value_EUR_ton.HasValue ? productCostDataView.Value_EUR_ton / 1000 : decimal.Zero);
                productCostDataView.Value_EUR_PC = (productCostDataView.Value_EUR_kg.HasValue ? productCostDataView.Value_EUR_kg * weightPerPC : decimal.Zero);

                listProductCosts.Add(productCostDataView);

                foreach (ProductCostDataView productCost in listProductCosts.Where(w => w.ProductCostType == "SGAsAndFinancials").ToList())
                {
                    totalSGAsAndFinancials_ton += (productCost.Value_EUR_ton.HasValue ? productCost.Value_EUR_ton.Value : decimal.Zero);
                    totalSGAsAndFinancials_kg += (productCost.Value_EUR_kg.HasValue ? productCost.Value_EUR_kg.Value : decimal.Zero);
                    totalSGAsAndFinancials_PC += (productCost.Value_EUR_PC.HasValue ? productCost.Value_EUR_PC.Value : decimal.Zero);
                }

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "SGAsAndFinancials";
                productCostDataView.RowType = "Total";
                productCostDataView.Name = "Total SGA's and Financial Cost";
                productCostDataView.RowKeyIntCode = "TotalSGAsAndFinancialCost";
                productCostDataView.Value_EUR_ton = totalSGAsAndFinancials_ton;
                productCostDataView.Value_EUR_kg = totalSGAsAndFinancials_kg;
                productCostDataView.Value_EUR_PC = totalSGAsAndFinancials_PC;

                listProductCosts.Add(productCostDataView);

                #endregion

                #region TOTAL PRODUCT COST

                decimal totalConversionCost_ton = decimal.Zero;
                decimal totalConversionCost_kg = decimal.Zero;
                decimal totalConversionCost_PC = decimal.Zero;

                decimal totalConversionCostWithoutAluminium_ton = decimal.Zero;
                decimal totalConversionCostWithoutAluminium_kg = decimal.Zero;
                decimal totalConversionCostWithoutAluminium_PC = decimal.Zero;

                decimal totalConversionCostWithoutAluminiumAndDepreciation_ton = decimal.Zero;
                decimal totalConversionCostWithoutAluminiumAndDepreciation_kg = decimal.Zero;
                decimal totalConversionCostWithoutAluminiumAndDepreciation_PC = decimal.Zero;

                decimal totalCostExtrPackTranspAndComm_ton = decimal.Zero;
                decimal totalCostExtrPackTranspAndComm_kg = decimal.Zero;
                decimal totalCostExtrPackTranspAndComm_PC = decimal.Zero;

                decimal totalCostExtrPackTranspAndCommWithoutDepr_ton = decimal.Zero;
                decimal totalCostExtrPackTranspAndCommWithoutDepr_kg = decimal.Zero;
                decimal totalCostExtrPackTranspAndCommWithoutDepr_PC = decimal.Zero;

                decimal totalCost_ton = decimal.Zero;
                decimal totalCost_kg = decimal.Zero;
                decimal totalCost_PC = decimal.Zero;

                var productCostExtrusionTotal = listProductCosts.Where(w => w.ProductCostType == "Extrusion" && w.RowType == "Total" && w.RowKeyIntCode == "TotalExtrusionCost").FirstOrDefault();
                var productCostPackagingTotal = listProductCosts.Where(w => w.ProductCostType == "Packaging" && w.RowType == "Total" && w.RowKeyIntCode == "TotalPackagingCost").FirstOrDefault();

                var productCostExtrusionLME = listProductCosts.Where(w => w.ProductCostType == "Extrusion" && w.RowType == "Data" && w.RowKeyIntCode == "LME").FirstOrDefault();
                var productCostExtrusionPREMIUM = listProductCosts.Where(w => w.ProductCostType == "Extrusion" && w.RowType == "Data" && w.RowKeyIntCode == "PREMIUM").FirstOrDefault();

                var productCostExtrusionDepreciation = listProductCosts.Where(w => w.ProductCostType == "Extrusion" && w.RowType == "Total" && w.RowKeyIntCode == "Depreciation").FirstOrDefault();
                var productCostPackagingDepreciation = listProductCosts.Where(w => w.ProductCostType == "Packaging" && w.RowType == "Total" && w.RowKeyIntCode == "PackagingDepreciation").FirstOrDefault();

                var productCostSGAsAndFinancialsTotal = listProductCosts.Where(w => w.ProductCostType == "SGAsAndFinancials" && w.RowType == "Total" && w.RowKeyIntCode == "TotalSGAsAndFinancialCost").FirstOrDefault();

                var productCostTransportationCostAndCommissionTotal = listProductCosts.Where(w => w.ProductCostType == "TransportationCostAndCommission" && w.RowType == "Total" && w.RowKeyIntCode == "TotalTransportationCostAndCommission").FirstOrDefault();

                if (productCostExtrusionTotal != null)
                {
                    totalConversionCost_ton = (productCostExtrusionTotal.Value_EUR_ton.HasValue ? productCostExtrusionTotal.Value_EUR_ton.Value : decimal.Zero);
                    totalConversionCost_kg = (productCostExtrusionTotal.Value_EUR_kg.HasValue ? productCostExtrusionTotal.Value_EUR_kg.Value : decimal.Zero);
                    totalConversionCost_PC = (productCostExtrusionTotal.Value_EUR_PC.HasValue ? productCostExtrusionTotal.Value_EUR_PC.Value : decimal.Zero);

                    totalCostExtrPackTranspAndComm_ton = (productCostExtrusionTotal.Value_EUR_ton.HasValue ? productCostExtrusionTotal.Value_EUR_ton.Value : decimal.Zero);
                    totalCostExtrPackTranspAndComm_kg = (productCostExtrusionTotal.Value_EUR_kg.HasValue ? productCostExtrusionTotal.Value_EUR_kg.Value : decimal.Zero);
                    totalCostExtrPackTranspAndComm_PC = (productCostExtrusionTotal.Value_EUR_PC.HasValue ? productCostExtrusionTotal.Value_EUR_PC.Value : decimal.Zero);
                }
                if (productCostPackagingTotal != null)
                {
                    totalConversionCost_ton += (productCostPackagingTotal.Value_EUR_ton.HasValue ? productCostPackagingTotal.Value_EUR_ton.Value : decimal.Zero);
                    totalConversionCost_kg += (productCostPackagingTotal.Value_EUR_kg.HasValue ? productCostPackagingTotal.Value_EUR_kg.Value : decimal.Zero);
                    totalConversionCost_PC += (productCostPackagingTotal.Value_EUR_PC.HasValue ? productCostPackagingTotal.Value_EUR_PC.Value : decimal.Zero);

                    totalCostExtrPackTranspAndComm_ton += (productCostPackagingTotal.Value_EUR_ton.HasValue ? productCostPackagingTotal.Value_EUR_ton.Value : decimal.Zero);
                    totalCostExtrPackTranspAndComm_kg += (productCostPackagingTotal.Value_EUR_kg.HasValue ? productCostPackagingTotal.Value_EUR_kg.Value : decimal.Zero);
                    totalCostExtrPackTranspAndComm_PC += (productCostPackagingTotal.Value_EUR_PC.HasValue ? productCostPackagingTotal.Value_EUR_PC.Value : decimal.Zero);
                }
                if (productCostSGAsAndFinancialsTotal != null)
                {
                    totalCost_ton = totalConversionCost_ton + (productCostSGAsAndFinancialsTotal.Value_EUR_ton.HasValue ? productCostSGAsAndFinancialsTotal.Value_EUR_ton.Value : decimal.Zero);
                    totalCost_kg = totalConversionCost_kg + (productCostSGAsAndFinancialsTotal.Value_EUR_kg.HasValue ? productCostSGAsAndFinancialsTotal.Value_EUR_kg.Value : decimal.Zero);
                    totalCost_PC = totalConversionCost_PC + (productCostSGAsAndFinancialsTotal.Value_EUR_PC.HasValue ? productCostSGAsAndFinancialsTotal.Value_EUR_PC.Value : decimal.Zero);
                }
                if (productCostTransportationCostAndCommissionTotal != null)
                {
                    totalCost_ton += (productCostTransportationCostAndCommissionTotal.Value_EUR_ton.HasValue ? productCostTransportationCostAndCommissionTotal.Value_EUR_ton.Value : decimal.Zero);
                    totalCost_kg += (productCostTransportationCostAndCommissionTotal.Value_EUR_kg.HasValue ? productCostTransportationCostAndCommissionTotal.Value_EUR_kg.Value : decimal.Zero);
                    totalCost_PC += (productCostTransportationCostAndCommissionTotal.Value_EUR_PC.HasValue ? productCostTransportationCostAndCommissionTotal.Value_EUR_PC.Value : decimal.Zero);

                    totalCostExtrPackTranspAndComm_ton += (productCostTransportationCostAndCommissionTotal.Value_EUR_ton.HasValue ? productCostTransportationCostAndCommissionTotal.Value_EUR_ton.Value : decimal.Zero);
                    totalCostExtrPackTranspAndComm_kg += (productCostTransportationCostAndCommissionTotal.Value_EUR_kg.HasValue ? productCostTransportationCostAndCommissionTotal.Value_EUR_kg.Value : decimal.Zero);
                    totalCostExtrPackTranspAndComm_PC += (productCostTransportationCostAndCommissionTotal.Value_EUR_PC.HasValue ? productCostTransportationCostAndCommissionTotal.Value_EUR_PC.Value : decimal.Zero);
                }
                if (productCostExtrusionLME != null)
                {
                    totalConversionCostWithoutAluminium_ton = totalConversionCost_ton - (productCostExtrusionLME.Value_EUR_ton.HasValue ? productCostExtrusionLME.Value_EUR_ton.Value : decimal.Zero);
                    totalConversionCostWithoutAluminium_kg = totalConversionCost_kg - (productCostExtrusionLME.Value_EUR_kg.HasValue ? productCostExtrusionLME.Value_EUR_kg.Value : decimal.Zero);
                    totalConversionCostWithoutAluminium_PC = totalConversionCost_PC - (productCostExtrusionLME.Value_EUR_PC.HasValue ? productCostExtrusionLME.Value_EUR_PC.Value : decimal.Zero);
                }
                if (productCostExtrusionPREMIUM != null)
                {
                    totalConversionCostWithoutAluminium_ton = totalConversionCostWithoutAluminium_ton - (productCostExtrusionPREMIUM.Value_EUR_ton.HasValue ? productCostExtrusionPREMIUM.Value_EUR_ton.Value : decimal.Zero);
                    totalConversionCostWithoutAluminium_kg = totalConversionCostWithoutAluminium_kg - (productCostExtrusionPREMIUM.Value_EUR_kg.HasValue ? productCostExtrusionPREMIUM.Value_EUR_kg.Value : decimal.Zero);
                    totalConversionCostWithoutAluminium_PC = totalConversionCostWithoutAluminium_PC - (productCostExtrusionPREMIUM.Value_EUR_PC.HasValue ? productCostExtrusionPREMIUM.Value_EUR_PC.Value : decimal.Zero);
                }
                if (productCostExtrusionDepreciation != null)
                {
                    totalConversionCostWithoutAluminiumAndDepreciation_ton = totalConversionCostWithoutAluminium_ton - (productCostExtrusionDepreciation.Value_EUR_ton.HasValue ? productCostExtrusionDepreciation.Value_EUR_ton.Value : decimal.Zero);
                    totalConversionCostWithoutAluminiumAndDepreciation_kg = totalConversionCostWithoutAluminium_kg - (productCostExtrusionDepreciation.Value_EUR_kg.HasValue ? productCostExtrusionDepreciation.Value_EUR_kg.Value : decimal.Zero);
                    totalConversionCostWithoutAluminiumAndDepreciation_PC = totalConversionCostWithoutAluminium_PC - (productCostExtrusionDepreciation.Value_EUR_PC.HasValue ? productCostExtrusionDepreciation.Value_EUR_PC.Value : decimal.Zero);

                    totalCostExtrPackTranspAndCommWithoutDepr_ton = totalCostExtrPackTranspAndComm_ton - (productCostExtrusionDepreciation.Value_EUR_ton.HasValue ? productCostExtrusionDepreciation.Value_EUR_ton.Value : decimal.Zero);
                    totalCostExtrPackTranspAndCommWithoutDepr_kg = totalCostExtrPackTranspAndComm_kg - (productCostExtrusionDepreciation.Value_EUR_kg.HasValue ? productCostExtrusionDepreciation.Value_EUR_kg.Value : decimal.Zero);
                    totalCostExtrPackTranspAndCommWithoutDepr_PC = totalCostExtrPackTranspAndComm_PC - (productCostExtrusionDepreciation.Value_EUR_PC.HasValue ? productCostExtrusionDepreciation.Value_EUR_PC.Value : decimal.Zero);
                }
                if (productCostPackagingDepreciation != null)
                {
                    totalConversionCostWithoutAluminiumAndDepreciation_ton = totalConversionCostWithoutAluminiumAndDepreciation_ton - (productCostPackagingDepreciation.Value_EUR_ton.HasValue ? productCostPackagingDepreciation.Value_EUR_ton.Value : decimal.Zero);
                    totalConversionCostWithoutAluminiumAndDepreciation_kg = totalConversionCostWithoutAluminiumAndDepreciation_kg - (productCostPackagingDepreciation.Value_EUR_kg.HasValue ? productCostPackagingDepreciation.Value_EUR_kg.Value : decimal.Zero);
                    totalConversionCostWithoutAluminiumAndDepreciation_PC = totalConversionCostWithoutAluminiumAndDepreciation_PC - (productCostPackagingDepreciation.Value_EUR_PC.HasValue ? productCostPackagingDepreciation.Value_EUR_PC.Value : decimal.Zero);

                    totalCostExtrPackTranspAndCommWithoutDepr_ton = totalCostExtrPackTranspAndCommWithoutDepr_ton - (productCostPackagingDepreciation.Value_EUR_ton.HasValue ? productCostPackagingDepreciation.Value_EUR_ton.Value : decimal.Zero);
                    totalCostExtrPackTranspAndCommWithoutDepr_kg = totalCostExtrPackTranspAndCommWithoutDepr_kg - (productCostPackagingDepreciation.Value_EUR_kg.HasValue ? productCostPackagingDepreciation.Value_EUR_kg.Value : decimal.Zero);
                    totalCostExtrPackTranspAndCommWithoutDepr_PC = totalCostExtrPackTranspAndCommWithoutDepr_PC - (productCostPackagingDepreciation.Value_EUR_PC.HasValue ? productCostPackagingDepreciation.Value_EUR_PC.Value : decimal.Zero);
                }

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "TotalProductCost";
                productCostDataView.RowType = "Total";
                productCostDataView.Name = "Total Cost (incl. Extrusion and Packaging)";
                productCostDataView.RowKeyIntCode = "TotalConversionCost";
                productCostDataView.Value_EUR_ton = totalConversionCost_ton;
                productCostDataView.Value_EUR_kg = totalConversionCost_kg;
                productCostDataView.Value_EUR_PC = totalConversionCost_PC;

                listProductCosts.Add(productCostDataView);

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "TotalProductCost";
                productCostDataView.RowType = "Total";
                productCostDataView.Name = "Total Conversion Cost (excl.  Aluminium)";
                productCostDataView.RowKeyIntCode = "TotalConversionCostWithoutLMEsPREMIUM";
                productCostDataView.Value_EUR_ton = totalConversionCostWithoutAluminium_ton;
                productCostDataView.Value_EUR_kg = totalConversionCostWithoutAluminium_kg;
                productCostDataView.Value_EUR_PC = totalConversionCostWithoutAluminium_PC;

                listProductCosts.Add(productCostDataView);

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "TotalProductCost";
                productCostDataView.RowType = "Total";
                productCostDataView.Name = "Total Conversion Cost (excl. Aluminium and Depreciation)";
                productCostDataView.RowKeyIntCode = "TotalConversionCostWithoutAluminiumAndDepreciation";
                productCostDataView.Value_EUR_ton = totalConversionCostWithoutAluminiumAndDepreciation_ton;
                productCostDataView.Value_EUR_kg = totalConversionCostWithoutAluminiumAndDepreciation_kg;
                productCostDataView.Value_EUR_PC = totalConversionCostWithoutAluminiumAndDepreciation_PC;

                listProductCosts.Add(productCostDataView);

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "TotalProductCost";
                productCostDataView.RowType = "Total";
                productCostDataView.Name = "Total Cost (incl. Extrusion, Packaging, Transportation and Commission)";
                productCostDataView.RowKeyIntCode = "TotalCostExtrusionPackagingTransportationAndCommission";
                productCostDataView.Value_EUR_ton = totalCostExtrPackTranspAndComm_ton;
                productCostDataView.Value_EUR_kg = totalCostExtrPackTranspAndComm_kg;
                productCostDataView.Value_EUR_PC = totalCostExtrPackTranspAndComm_PC;

                listProductCosts.Add(productCostDataView);

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "TotalProductCost";
                productCostDataView.RowType = "Total";
                productCostDataView.Name = "Total Cost (incl. Extrusion, Packaging, Transportation and Commission, excl. Depreciation)";
                productCostDataView.RowKeyIntCode = "TotalCostExtrusionPackagingTransportationAndCommissionWithoutDepreciation";
                productCostDataView.Value_EUR_ton = totalCostExtrPackTranspAndCommWithoutDepr_ton;
                productCostDataView.Value_EUR_kg = totalCostExtrPackTranspAndCommWithoutDepr_kg;
                productCostDataView.Value_EUR_PC = totalCostExtrPackTranspAndCommWithoutDepr_PC;

                listProductCosts.Add(productCostDataView);

                productCostDataView = new ProductCostDataView();

                productCostDataView.ProductCostType = "TotalProductCost";
                productCostDataView.RowType = "Total";
                productCostDataView.Name = "TOTAL COST (incl. Extrusion, Packaging, Transportation and Commission, SGA's and Financials)";
                productCostDataView.RowKeyIntCode = "TotalCost";
                productCostDataView.Value_EUR_ton = totalCost_ton;
                productCostDataView.Value_EUR_kg = totalCost_kg;
                productCostDataView.Value_EUR_PC = totalCost_PC;

                listProductCosts.Add(productCostDataView);

                #endregion

                productCostResult.ListProductCosts = listProductCosts;
            }
            catch (Exception ex)
            {
                productCostResult.ResultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                productCostResult.ResultContext.Message = "Error load product costs by offer!";

                BaseHelper.Log("Error load product costs by offer!");
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

            return productCostResult;
        }

        public ProductCostResult LoadProductCostsInTableRowsByOfferId(int idOffer, CallContext resultContext)
        {
            ProductCostResult productCostResult = new ProductCostResult();

            TableRow[] arrProductCosts = new TableRow[0];

            try
            {
                productCostResult.ResultContext = resultContext;
                productCostResult.ResultContext.ResultCode = ETEMEnums.ResultEnum.Success;

                List<ProductCostDataView> listProductCosts = new List<ProductCostDataView>();

                List<ProductCostDataView> listProductCostsExtrusion = new List<ProductCostDataView>();
                List<ProductCostDataView> listProductCostsPackaging = new List<ProductCostDataView>();
                List<ProductCostDataView> listProductCostsTransportationCostAndCommission = new List<ProductCostDataView>();
                List<ProductCostDataView> listProductCostsSGAsAndFinancials = new List<ProductCostDataView>();
                List<ProductCostDataView> listProductCostsTotalProductCost = new List<ProductCostDataView>();

                ProductCostResult productCost = this.LoadProductCostsByOfferId(idOffer, resultContext);

                listProductCosts = productCost.ListProductCosts;

                listProductCostsExtrusion = listProductCosts.Where(w => w.ProductCostType == "Extrusion").ToList();
                listProductCostsPackaging = listProductCosts.Where(w => w.ProductCostType == "Packaging").ToList();
                listProductCostsTransportationCostAndCommission = listProductCosts.Where(w => w.ProductCostType == "TransportationCostAndCommission").ToList();
                listProductCostsSGAsAndFinancials = listProductCosts.Where(w => w.ProductCostType == "SGAsAndFinancials").ToList();
                listProductCostsTotalProductCost = listProductCosts.Where(w => w.ProductCostType == "TotalProductCost").ToList();

                List<string> listColumns = new List<string>()
                {
                    "EUR/ton",
                    "EUR/kg",
                    "EUR/PC"
                };

                int countRows = 2 + listProductCostsExtrusion.Count +
                                3 + listProductCostsPackaging.Count +
                                3 + listProductCostsTransportationCostAndCommission.Count +
                                3 + listProductCostsSGAsAndFinancials.Count +
                                3 + listProductCostsTotalProductCost.Count;

                arrProductCosts = new TableRow[countRows];

                TableRow tableRow = new TableRow();
                TableCell tableCell = new TableCell();
                TableHeaderRow tableHeaderRow = new TableHeaderRow();
                TableHeaderCell tableHeaderCell = new TableHeaderCell();

                int rowIndex = 0;

                #region Extrusion

                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();

                tableHeaderCell.Text = "Extrusion";
                tableHeaderCell.ColumnSpan = 4;
                tableHeaderCell.CssClass = "GridExpenses_tr_th_Main";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrProductCosts[rowIndex++] = tableHeaderRow;

                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();

                tableHeaderCell.Text = "";
                tableHeaderCell.Width = Unit.Pixel(600);
                tableHeaderCell.CssClass = "GridExpenses_td_item_center";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrProductCosts[rowIndex] = tableHeaderRow;

                // Add header columns
                foreach (string columnName in listColumns)
                {
                    tableHeaderCell = new TableHeaderCell();

                    tableHeaderCell.Text = columnName;
                    tableHeaderCell.CssClass = "GridExpenses_td_item_center";
                    arrProductCosts[rowIndex].Cells.Add(tableHeaderCell);
                }
                rowIndex++;
                // Add header rows
                foreach (ProductCostDataView prodCostExtrusion in listProductCostsExtrusion)
                {
                    tableRow = new TableRow();
                    tableCell = new TableCell();

                    tableCell.Text = prodCostExtrusion.Name.Replace(" group", "");
                    if (prodCostExtrusion.RowType == "Total")
                    {
                        tableCell.CssClass = "GridExpenses_td_item_total";
                    }
                    tableRow.Cells.Add(tableCell);

                    tableCell = new TableCell();
                    tableCell.Text = prodCostExtrusion.Value_EUR_ton_Formatted;
                    tableCell.HorizontalAlign = HorizontalAlign.Right;
                    tableCell.CssClass = "GridExpenses_td_item_right";
                    if (prodCostExtrusion.RowType == "Total")
                    {
                        tableCell.CssClass += " GridExpenses_td_item_total";
                    }                    
                    tableRow.Cells.Add(tableCell);

                    tableCell = new TableCell();
                    tableCell.Text = prodCostExtrusion.Value_EUR_kg_Formatted;
                    tableCell.HorizontalAlign = HorizontalAlign.Right;
                    tableCell.CssClass = "GridExpenses_td_item_right";
                    if (prodCostExtrusion.RowType == "Total")
                    {
                        tableCell.CssClass += " GridExpenses_td_item_total";
                    }
                    tableRow.Cells.Add(tableCell);

                    tableCell = new TableCell();
                    tableCell.Text = prodCostExtrusion.Value_EUR_PC_Formatted;
                    tableCell.HorizontalAlign = HorizontalAlign.Right;
                    tableCell.CssClass = "GridExpenses_td_item_right";
                    if (prodCostExtrusion.RowType == "Total")
                    {
                        tableCell.CssClass += " GridExpenses_td_item_total";
                    }
                    tableRow.Cells.Add(tableCell);

                    arrProductCosts[rowIndex++] = tableRow;
                }

                #endregion

                #region Packaging

                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();
                tableHeaderCell.ColumnSpan = 4;
                tableHeaderCell.CssClass = "GridExpenses_tr_th_Empty";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrProductCosts[rowIndex++] = tableHeaderRow;

                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();

                tableHeaderCell.Text = "Packaging";
                tableHeaderCell.ColumnSpan = 4;
                tableHeaderCell.CssClass = "GridExpenses_tr_th_Main";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrProductCosts[rowIndex++] = tableHeaderRow;

                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();

                tableHeaderCell.Text = "";
                tableHeaderCell.Width = Unit.Pixel(600);
                tableHeaderCell.CssClass = "GridExpenses_td_item_center";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrProductCosts[rowIndex] = tableHeaderRow;

                // Add header columns
                foreach (string columnName in listColumns)
                {
                    tableHeaderCell = new TableHeaderCell();

                    tableHeaderCell.Text = columnName;
                    tableHeaderCell.CssClass = "GridExpenses_td_item_center";
                    arrProductCosts[rowIndex].Cells.Add(tableHeaderCell);
                }
                rowIndex++;
                // Add header rows
                foreach (ProductCostDataView prodCostPackaging in listProductCostsPackaging)
                {
                    tableRow = new TableRow();
                    tableCell = new TableCell();

                    tableCell.Text = prodCostPackaging.Name.Replace(" group", "");
                    if (prodCostPackaging.RowType == "Total")
                    {
                        tableCell.CssClass = "GridExpenses_td_item_total";
                    }
                    tableRow.Cells.Add(tableCell);

                    tableCell = new TableCell();
                    tableCell.Text = prodCostPackaging.Value_EUR_ton_Formatted;
                    tableCell.HorizontalAlign = HorizontalAlign.Right;
                    tableCell.CssClass = "GridExpenses_td_item_right";
                    if (prodCostPackaging.RowType == "Total")
                    {
                        tableCell.CssClass += " GridExpenses_td_item_total";
                    }
                    tableRow.Cells.Add(tableCell);

                    tableCell = new TableCell();
                    tableCell.Text = prodCostPackaging.Value_EUR_kg_Formatted;
                    tableCell.HorizontalAlign = HorizontalAlign.Right;
                    tableCell.CssClass = "GridExpenses_td_item_right";
                    if (prodCostPackaging.RowType == "Total")
                    {
                        tableCell.CssClass += " GridExpenses_td_item_total";
                    }
                    tableRow.Cells.Add(tableCell);

                    tableCell = new TableCell();
                    tableCell.Text = prodCostPackaging.Value_EUR_PC_Formatted;
                    tableCell.HorizontalAlign = HorizontalAlign.Right;
                    tableCell.CssClass = "GridExpenses_td_item_right";
                    if (prodCostPackaging.RowType == "Total")
                    {
                        tableCell.CssClass += " GridExpenses_td_item_total";
                    }
                    tableRow.Cells.Add(tableCell);

                    arrProductCosts[rowIndex++] = tableRow;
                }

                #endregion

                #region Transportation Cost & Commission

                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();
                tableHeaderCell.ColumnSpan = 4;
                tableHeaderCell.CssClass = "GridExpenses_tr_th_Empty";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrProductCosts[rowIndex++] = tableHeaderRow;

                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();

                tableHeaderCell.Text = "Transportation Cost & Commission";
                tableHeaderCell.ColumnSpan = 4;
                tableHeaderCell.CssClass = "GridExpenses_tr_th_Main";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrProductCosts[rowIndex++] = tableHeaderRow;

                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();

                tableHeaderCell.Text = "";
                tableHeaderCell.Width = Unit.Pixel(600);
                tableHeaderCell.CssClass = "GridExpenses_td_item_center";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrProductCosts[rowIndex] = tableHeaderRow;

                // Add header columns
                foreach (string columnName in listColumns)
                {
                    tableHeaderCell = new TableHeaderCell();

                    tableHeaderCell.Text = columnName;
                    tableHeaderCell.CssClass = "GridExpenses_td_item_center";
                    arrProductCosts[rowIndex].Cells.Add(tableHeaderCell);
                }
                rowIndex++;
                // Add header rows
                foreach (ProductCostDataView prodCostTransportationCostAndCommission in listProductCostsTransportationCostAndCommission)
                {
                    tableRow = new TableRow();
                    tableCell = new TableCell();

                    tableCell.Text = prodCostTransportationCostAndCommission.Name.Replace(" group", "");
                    if (prodCostTransportationCostAndCommission.RowType == "Total")
                    {
                        tableCell.CssClass = "GridExpenses_td_item_total";
                    }
                    tableRow.Cells.Add(tableCell);

                    tableCell = new TableCell();
                    tableCell.Text = prodCostTransportationCostAndCommission.Value_EUR_ton_Formatted;
                    tableCell.HorizontalAlign = HorizontalAlign.Right;
                    tableCell.CssClass = "GridExpenses_td_item_right";
                    if (prodCostTransportationCostAndCommission.RowType == "Total")
                    {
                        tableCell.CssClass += " GridExpenses_td_item_total";
                    }
                    tableRow.Cells.Add(tableCell);

                    tableCell = new TableCell();
                    tableCell.Text = prodCostTransportationCostAndCommission.Value_EUR_kg_Formatted;
                    tableCell.HorizontalAlign = HorizontalAlign.Right;
                    tableCell.CssClass = "GridExpenses_td_item_right";
                    if (prodCostTransportationCostAndCommission.RowType == "Total")
                    {
                        tableCell.CssClass += " GridExpenses_td_item_total";
                    }
                    tableRow.Cells.Add(tableCell);

                    tableCell = new TableCell();
                    tableCell.Text = prodCostTransportationCostAndCommission.Value_EUR_PC_Formatted;
                    tableCell.HorizontalAlign = HorizontalAlign.Right;
                    tableCell.CssClass = "GridExpenses_td_item_right";
                    if (prodCostTransportationCostAndCommission.RowType == "Total")
                    {
                        tableCell.CssClass += " GridExpenses_td_item_total";
                    }
                    tableRow.Cells.Add(tableCell);

                    arrProductCosts[rowIndex++] = tableRow;
                }

                #endregion

                #region SGA's and Financials

                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();
                tableHeaderCell.ColumnSpan = 4;
                tableHeaderCell.CssClass = "GridExpenses_tr_th_Empty";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrProductCosts[rowIndex++] = tableHeaderRow;

                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();

                tableHeaderCell.Text = "SGA's and Financials";
                tableHeaderCell.ColumnSpan = 4;
                tableHeaderCell.CssClass = "GridExpenses_tr_th_Main";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrProductCosts[rowIndex++] = tableHeaderRow;

                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();

                tableHeaderCell.Text = "";
                tableHeaderCell.Width = Unit.Pixel(600);
                tableHeaderCell.CssClass = "GridExpenses_td_item_center";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrProductCosts[rowIndex] = tableHeaderRow;

                // Add header columns
                foreach (string columnName in listColumns)
                {
                    tableHeaderCell = new TableHeaderCell();

                    tableHeaderCell.Text = columnName;
                    tableHeaderCell.CssClass = "GridExpenses_td_item_center";
                    arrProductCosts[rowIndex].Cells.Add(tableHeaderCell);
                }
                rowIndex++;
                // Add header rows
                foreach (ProductCostDataView prodCostSGAsAndFinancials in listProductCostsSGAsAndFinancials)
                {
                    tableRow = new TableRow();
                    tableCell = new TableCell();

                    tableCell.Text = prodCostSGAsAndFinancials.Name.Replace(" group", "");
                    if (prodCostSGAsAndFinancials.RowType == "Total")
                    {
                        tableCell.CssClass = "GridExpenses_td_item_total";
                    }
                    tableRow.Cells.Add(tableCell);

                    tableCell = new TableCell();
                    tableCell.Text = prodCostSGAsAndFinancials.Value_EUR_ton_Formatted;
                    tableCell.HorizontalAlign = HorizontalAlign.Right;
                    tableCell.CssClass = "GridExpenses_td_item_right";
                    if (prodCostSGAsAndFinancials.RowType == "Total")
                    {
                        tableCell.CssClass += " GridExpenses_td_item_total";
                    }
                    tableRow.Cells.Add(tableCell);

                    tableCell = new TableCell();
                    tableCell.Text = prodCostSGAsAndFinancials.Value_EUR_kg_Formatted;
                    tableCell.HorizontalAlign = HorizontalAlign.Right;
                    tableCell.CssClass = "GridExpenses_td_item_right";
                    if (prodCostSGAsAndFinancials.RowType == "Total")
                    {
                        tableCell.CssClass += " GridExpenses_td_item_total";
                    }
                    tableRow.Cells.Add(tableCell);

                    tableCell = new TableCell();
                    tableCell.Text = prodCostSGAsAndFinancials.Value_EUR_PC_Formatted;
                    tableCell.HorizontalAlign = HorizontalAlign.Right;
                    tableCell.CssClass = "GridExpenses_td_item_right";                    
                    if (prodCostSGAsAndFinancials.RowType == "Total")
                    {
                        tableCell.CssClass += " GridExpenses_td_item_total";
                    }
                    tableRow.Cells.Add(tableCell);

                    arrProductCosts[rowIndex++] = tableRow;
                }

                #endregion

                #region TOTAL PRODUCT COST

                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();
                tableHeaderCell.ColumnSpan = 4;
                tableHeaderCell.CssClass = "GridExpenses_tr_th_Empty";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrProductCosts[rowIndex++] = tableHeaderRow;

                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();

                tableHeaderCell.Text = "TOTAL PRODUCT COST";
                tableHeaderCell.ColumnSpan = 4;
                tableHeaderCell.CssClass = "GridExpenses_tr_th_Main";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrProductCosts[rowIndex++] = tableHeaderRow;

                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();

                tableHeaderCell.Text = "";
                tableHeaderCell.Width = Unit.Pixel(600);
                tableHeaderCell.CssClass = "GridExpenses_td_item_center";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrProductCosts[rowIndex] = tableHeaderRow;

                // Add header columns
                foreach (string columnName in listColumns)
                {
                    tableHeaderCell = new TableHeaderCell();

                    tableHeaderCell.Text = columnName;
                    tableHeaderCell.CssClass = "GridExpenses_td_item_center";
                    arrProductCosts[rowIndex].Cells.Add(tableHeaderCell);
                }
                rowIndex++;
                // Add header rows
                foreach (ProductCostDataView prodCostTotalProductCost in listProductCostsTotalProductCost)
                {
                    tableRow = new TableRow();
                    tableCell = new TableCell();

                    tableCell.Text = prodCostTotalProductCost.Name.Replace(" group", "");
                    if (prodCostTotalProductCost.RowType == "Total")
                    {
                        tableCell.CssClass = "GridExpenses_td_item_total GridExpenses_td_background_yellow";
                    }
                    tableRow.Cells.Add(tableCell);

                    tableCell = new TableCell();
                    tableCell.ToolTip = prodCostTotalProductCost.Value_EUR_ton.ToString();
                    tableCell.Text = prodCostTotalProductCost.Value_EUR_ton_Formatted;
                    tableCell.HorizontalAlign = HorizontalAlign.Right;
                    tableCell.CssClass = "GridExpenses_td_item_right";
                    if (prodCostTotalProductCost.RowType == "Total")
                    {
                        tableCell.CssClass += " GridExpenses_td_item_total GridExpenses_td_background_yellow";
                    }
                    tableRow.Cells.Add(tableCell);

                    tableCell = new TableCell();
                    tableCell.ToolTip = prodCostTotalProductCost.Value_EUR_kg.ToString();
                    tableCell.Text = prodCostTotalProductCost.Value_EUR_kg_Formatted;
                    tableCell.HorizontalAlign = HorizontalAlign.Right;
                    tableCell.CssClass = "GridExpenses_td_item_right";
                    if (prodCostTotalProductCost.RowType == "Total")
                    {
                        tableCell.CssClass += " GridExpenses_td_item_total GridExpenses_td_background_yellow";
                    }
                    tableRow.Cells.Add(tableCell);

                    tableCell = new TableCell();
                    tableCell.Text = prodCostTotalProductCost.Value_EUR_PC_Formatted;
                    tableCell.ToolTip = prodCostTotalProductCost.Value_EUR_PC.ToString();
                    tableCell.HorizontalAlign = HorizontalAlign.Right;
                    tableCell.CssClass = "GridExpenses_td_item_right";
                    if (prodCostTotalProductCost.RowType == "Total")
                    {
                        tableCell.CssClass += " GridExpenses_td_item_total GridExpenses_td_background_yellow";
                    }
                    tableRow.Cells.Add(tableCell);

                    arrProductCosts[rowIndex++] = tableRow;
                }

                #endregion

                productCostResult.TableRowsProductCosts = arrProductCosts;
            }
            catch (Exception ex)
            {
                productCostResult.ResultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                productCostResult.ResultContext.Message = "Error load product costs by offer!";

                BaseHelper.Log("Error load product costs in table rows by offer!");
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

            return productCostResult;
        }
    }

    public class ProductCostResult
    {
        public CallContext ResultContext { get; set; }
        public List<ProductCostDataView> ListProductCosts { get; set; }
        public TableRow[] TableRowsProductCosts { get; set; }
    }
}