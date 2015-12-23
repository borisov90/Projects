using ETEMModel.Models;
using ETEMModel.Models.DataView.CostCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using ETEMModel.Helpers.Extentions;

namespace ETEMModel.Helpers.CostCalculation
{
    public class OfferOverviewBL
    {
        private ETEMDataModelEntities dbContext;
        private List<OfferOverviewDataView> listOfferOverview;
        private ProductCostResult productCost;
        public OfferOverviewBL()
        {
            dbContext = new ETEMDataModelEntities();
        }

        internal OfferOverviewResult LoadOfferOverviewByOfferId(int idOffer, CallContext resultContext)
        {
            OfferOverviewResult result = new OfferOverviewResult();

            listOfferOverview = new List<OfferOverviewDataView>();

            Offer offer = new OfferBL().GetEntityById(idOffer);
            
            productCost = new ProductCostBL().LoadProductCostsByOfferId(idOffer, resultContext);
            
            #region CostTable
            //Aluminum
            ProductCostDataView ProductCostLME = productCost.ListProductCosts.Where(p => p.RowKeyIntCode == "LME").FirstOrDefault();
            ProductCostDataView ProductCostPREMIUM = productCost.ListProductCosts.Where(p => p.RowKeyIntCode == "PREMIUM").FirstOrDefault();
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Aluminum",
                RowKeyIntCode = "Aluminum",
                OfferOverviewType = "CostTable",
                Indicator_Value = (ProductCostLME.Value_EUR_ton.HasValue ? ProductCostLME.Value_EUR_ton.Value : Decimal.Zero) + (ProductCostPREMIUM.Value_EUR_ton.HasValue ? ProductCostPREMIUM.Value_EUR_ton.Value : Decimal.Zero),
                RowType = ETEMEnums.DataRowType.Data
            });

            //Commission
            ProductCostDataView ProductCostCommission = productCost.ListProductCosts.Where(p => p.RowKeyIntCode == "Commission").FirstOrDefault();
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Commission",
                RowKeyIntCode = "CostTableCommission",
                OfferOverviewType = "CostTable",
                Indicator_Value = (ProductCostCommission.Value_EUR_ton.HasValue ? ProductCostCommission.Value_EUR_ton.Value : Decimal.Zero),
                RowType = ETEMEnums.DataRowType.Data
            });

            //Transportation
            ProductCostDataView ProductCostTransportation = productCost.ListProductCosts.Where(p => p.RowKeyIntCode == "Transportation").FirstOrDefault();
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Transportation",
                RowKeyIntCode = "Transportation",
                OfferOverviewType = "CostTable",
                Indicator_Value = (ProductCostTransportation.Value_EUR_ton.HasValue ? ProductCostTransportation.Value_EUR_ton.Value : Decimal.Zero),
                RowType = ETEMEnums.DataRowType.Data
            });

            //Extrusion Conversion cost (without Aluminium) per ton
            ProductCostDataView ProductCostTotalExtrusionConversionCost = productCost.ListProductCosts.Where(p => p.RowKeyIntCode == "TotalExtrusionConversionCost").FirstOrDefault();
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Extrusion conversion cost per ton",
                RowKeyIntCode = "TotalExtrusionConversionCost",
                OfferOverviewType = "CostTable",
                Indicator_Value = (ProductCostTotalExtrusionConversionCost.Value_EUR_ton.HasValue ? ProductCostTotalExtrusionConversionCost.Value_EUR_ton.Value : Decimal.Zero),
                RowType = ETEMEnums.DataRowType.Data
            });


            //Packaging  conversion cost per ton
            ProductCostDataView ProductCostTotalPackagingCost = productCost.ListProductCosts.Where(p => p.RowKeyIntCode == "TotalPackagingCost").FirstOrDefault();
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Packaging  conversion cost per ton",
                RowKeyIntCode = "TotalPackagingCost",
                OfferOverviewType = "CostTable",
                Indicator_Value = (ProductCostTotalPackagingCost.Value_EUR_ton.HasValue ? ProductCostTotalPackagingCost.Value_EUR_ton.Value : Decimal.Zero),
                RowType = ETEMEnums.DataRowType.Data
            });

            //TOTAL COST
            //'=Aluminum+Commision+Transportation+Extrusion conversion cost per ton+Packaging  conversion cost per ton
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "TOTAL COST",
                RowKeyIntCode = "CostTableTotalCost",
                OfferOverviewType = "CostTable",
                Indicator_Value = listOfferOverview.Where(o => o.OfferOverviewType == "CostTable").Sum(s => s.Indicator_Value),
                RowType = ETEMEnums.DataRowType.Total
            });
            
            #endregion

            #region SalesPrice
            //'=Gross margin (%), tab Inquiry data
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Gross margin (%)",
                RowKeyIntCode = "SalesPriceGrossMargin",
                OfferOverviewType = "SalesPrice",
                Indicator_Value = offer.GrossMargin.HasValue ? offer.GrossMargin.Value : Decimal.Zero,
                RowType = ETEMEnums.DataRowType.Data
            });

            //'=TOTAL COST/(1-Gross margin (%))

            try
            {

                listOfferOverview.Add(new OfferOverviewDataView()
                {
                    Name = "Total sales price (EUR/ton)",
                    RowKeyIntCode = "TotalSalesPrice_EUR_TON",
                    OfferOverviewType = "SalesPrice",
                    Indicator_Value = listOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == "CostTableTotalCost").Indicator_Value /
                                        (1 - listOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == "SalesPriceGrossMargin").Indicator_Value / 100),
                    RowType = ETEMEnums.DataRowType.Data
                });
            }
            catch
            {
                
                listOfferOverview.Add(new OfferOverviewDataView()
                {
                    Name = "Total sales price (EUR/ton)",
                    RowKeyIntCode = "TotalSalesPrice_EUR_TON",
                    OfferOverviewType = "SalesPrice",
                    Indicator_Value = Decimal.Zero,
                    RowType = ETEMEnums.DataRowType.Data
                });
            }
           

            //Sales conversion (EUR/ton)
            //'=Total sales price (EUR/ton)-TOTAL COST
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Sales conversion (EUR/ton)",
                RowKeyIntCode = "SalesConversion_EUR_TON",
                OfferOverviewType = "SalesPrice",
                Indicator_Value = listOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == "TotalSalesPrice_EUR_TON").Indicator_Value -
                                  listOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == "CostTableTotalCost").Indicator_Value,
                RowType = ETEMEnums.DataRowType.Data
            });

            //CONVERSION TO CUSTOMER (EUR/ton)
            //'=Total sales price (EUR/ton)-Aluminum
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "CONVERSION TO CUSTOMER (EUR/ton)",
                RowKeyIntCode = "CONVERSION_TO_CUSTOMER_EUR_TON",
                OfferOverviewType = "SalesPrice",
                Indicator_Value = listOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == "TotalSalesPrice_EUR_TON").Indicator_Value -
                                  listOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == "Aluminum").Indicator_Value,
                RowType = ETEMEnums.DataRowType.Data
            });

            ////'=Savings rate %, tab Inquiry data
            //listOfferOverview.Add(new OfferOverviewDataView()
            //{
            //    Name = "Savings rate %",
            //    RowKeyIntCode = "SavingsRate",
            //    OfferOverviewType = "SalesPrice",
            //    Indicator_Value = offer.SavingsRate.HasValue ? offer.SavingsRate.Value : Decimal.Zero,
            //    RowType = ETEMEnums.DataRowType.Data
            //});

            ////Total sales price with savings and discounts (EUR/ton)
            //listOfferOverview.Add(new OfferOverviewDataView()
            //{
            //    Name = "Total sales price with savings and discounts (EUR/ton)",
            //    RowKeyIntCode = "TotalSalesPriceWithSavingsAndDiscounts_EUR_TON",
            //    OfferOverviewType = "SalesPrice",
            //    Indicator_Value = listOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == "TotalSalesPrice_EUR_TON").Indicator_Value *
            //                    (1 + listOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == "SavingsRate").Indicator_Value / 100),
            //    RowType = ETEMEnums.DataRowType.Data
            //});

            //Target price (EUR/ton)
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Target price (EUR/ton)",
                RowKeyIntCode = "TargetPrice_EUR_TON",
                OfferOverviewType = "SalesPrice",
                Indicator_Value = offer.TargetPrice_EUR_TON.HasValue ? offer.TargetPrice_EUR_TON.Value : Decimal.Zero,
                RowType = ETEMEnums.DataRowType.Data
            });


            //Target price (EUR/ton)
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Balance (EUR/ton)",
                RowKeyIntCode = "Balance_EUR_TON",
                OfferOverviewType = "SalesPrice",
                Indicator_Value = listOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == "TargetPrice_EUR_TON").Indicator_Value -
                                    listOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == "TotalSalesPrice_EUR_TON").Indicator_Value,
                RowType = ETEMEnums.DataRowType.Data
            });



            try
            {
                
                //Target price (EUR/ton)
                listOfferOverview.Add(new OfferOverviewDataView()
                {
                    Name = "Balance (%)",
                    RowKeyIntCode = "Balance_Persent",
                    OfferOverviewType = "SalesPrice",
                    Indicator_Value = listOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == "Balance_EUR_TON").Indicator_Value /
                                        listOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == "TotalSalesPrice_EUR_TON").Indicator_Value * 100,
                    RowType = ETEMEnums.DataRowType.Data
                });
            }
            catch
            { 

                //Target price (EUR/ton)
                listOfferOverview.Add(new OfferOverviewDataView()
                {
                    Name = "Balance (%)",
                    RowKeyIntCode = "Balance_Persent",
                    OfferOverviewType = "SalesPrice",
                    Indicator_Value = Decimal.Zero,
                    RowType = ETEMEnums.DataRowType.Data
                }); 
            }
            #endregion

            #region Summary Expenses

            //Commission
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Commission",
                RowKeyIntCode = "SummaryExpensesCommission",
                OfferOverviewType = "SummaryExpenses",
                Indicator_Value = offer.Commission_EUR_Computable,
                RowType = ETEMEnums.DataRowType.Data
            }); 



            //Other material cost
            //''=(Cost of Billet Scrap, table Extrusion, tab Product cost+Material cost, table Packaging, tab Product cost+Cost of Consumed material Scrap, table Packaging, tab Product cost)*Tonnage 
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Other material cost",
                RowKeyIntCode = "Other_Material_Cost",
                OfferOverviewType = "SummaryExpenses",
                Indicator_Value = (
                                        GetPC_EUR_TON("Extrusion", "Cost_Of_Billet_Scrap") + 
                                        GetPC_EUR_TON("Packaging", "MaterialCost") +
                                        GetPC_EUR_TON("Packaging", "CostOfConsumedMaterialScrap")
                                  ) * offer.Tonnage,
                RowType = ETEMEnums.DataRowType.Data
            });

            //Variable expenses
            //'=(Direct Labour, table Extrusion, tab Product cost+Variable Expenses,  table Extrusion, tab Product cost+Direct Labour, table Packaging, tab Product cost+Variable Expenses,table 
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Variable expenses",
                RowKeyIntCode = "Variable_Expenses",
                OfferOverviewType = "SummaryExpenses",
                Indicator_Value = (
                                        GetPC_EUR_TON("Extrusion", "DirectLaborGroup") + 
                                        GetPC_EUR_TON("Extrusion", "VariableExpenses") +
                                        GetPC_EUR_TON("Packaging", "DirectLaborGroup") + 
                                        GetPC_EUR_TON("Packaging", "PackagingVariableExpenses")
                                  ) * offer.Tonnage,
                RowType = ETEMEnums.DataRowType.Data
            });

            //Fixed expenses
            //'=(Fixed Expenses,  table Extrusion, tab Product cost+Fixed Expenses,table Packaging, tab Product cost)*Tonnage (kg)/1000
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Fixed expenses",
                RowKeyIntCode = "Fixed_Expenses",
                OfferOverviewType = "SummaryExpenses",
                Indicator_Value = (
                                      GetPC_EUR_TON("Extrusion", "FixedExpenses") + 
                                      GetPC_EUR_TON("Packaging", "PackagingFixedExpenses") 
                                  ) * offer.Tonnage,
                RowType = ETEMEnums.DataRowType.Data
            });


            //Depreciation
            //'=(Depreciation, table Extrusion, tab Product cost+Depreciation,table Packaging, tab Product cost)*Tonnage (kg)/1000
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Depreciation",
                RowKeyIntCode = "Depreciation",
                OfferOverviewType = "SummaryExpenses",
               Indicator_Value = (
                                      GetPC_EUR_TON("Extrusion", "Depreciation")  + 
                                      GetPC_EUR_TON("Packaging", "PackagingDepreciation") 
                                  ) * offer.Tonnage,
                RowType = ETEMEnums.DataRowType.Data
            });

            
            //LME
            //'=LME (EUR/ton), tab Inquiry data*Tonnage (kg)/1000
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "LME",
                RowKeyIntCode = "LME",
                OfferOverviewType = "SummaryExpenses",
                Indicator_Value = offer.LME * offer.Tonnage,
                RowType = ETEMEnums.DataRowType.Data
            });

            //PREMIUM
            //'=PREMIUM (EUR/ton), tab Inquiry data*Tonnage (kg)/1000
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "PREMIUM",
                RowKeyIntCode = "PREMIUM",
                OfferOverviewType = "SummaryExpenses",
                Indicator_Value = offer.PREMIUM * offer.Tonnage,
                RowType = ETEMEnums.DataRowType.Data
            });


            
            //Production expenses
            //'=Commission+Other material cost+Variable expenses+Fixed expenses+Depreciation+LME+PREMIUM
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Production expenses",
                RowKeyIntCode = "Production_Expenses",
                OfferOverviewType = "SummaryExpenses",
                Indicator_Value = SumIndicatorValue(null, "SummaryExpensesCommission", "Other_Material_Cost", "Variable_Expenses","Fixed_Expenses","Depreciation", "LME","PREMIUM"   ) ,
                RowType = ETEMEnums.DataRowType.Total
            });

            //Administration
            //'=Commission+Other material cost+Variable expenses+Fixed expenses+Depreciation+LME+PREMIUM
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Administration",
                RowKeyIntCode = "SummaryExpensesAdministration",
                OfferOverviewType = "SummaryExpenses",
                Indicator_Value = GetPC_EUR_TON("SGAsAndFinancials", "AdministrationExpenses") *  offer.Tonnage,
                RowType = ETEMEnums.DataRowType.Data
            });

            //Selling
            //'=Sales expenses, table SGA's and Financials, tab Product cost*Tonnage (kg)/1000
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Selling",
                RowKeyIntCode = "SummaryExpensesSelling",
                OfferOverviewType = "SummaryExpenses",
                Indicator_Value = GetPC_EUR_TON("SGAsAndFinancials", "SalesExpenses") *  offer.Tonnage,
                RowType = ETEMEnums.DataRowType.Data
            });

            //Financial fixed expenses
            //'=Financial fixed expenses, table SGA's and Financials, tab Product cost*Tonnage (kg)/1000
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Financial fixed expenses",
                RowKeyIntCode = "Financial_Fixed_Expenses",
                OfferOverviewType = "SummaryExpenses",
                Indicator_Value = GetPC_EUR_TON("SGAsAndFinancials", "FinancialFixedExpenses") *  offer.Tonnage,
                RowType = ETEMEnums.DataRowType.Data
            });

            //Financial variable expenses
            //'=Financial variable expenses, table SGA's and Financials, tab Product cost*Tonnage (kg)/1000
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Financial variable expenses",
                RowKeyIntCode = "Financial_Variable_Expenses",
                OfferOverviewType = "SummaryExpenses",
               Indicator_Value = GetPC_EUR_TON("SGAsAndFinancials", "FinancialVariableExpenses") *  offer.Tonnage,
                RowType = ETEMEnums.DataRowType.Data
            });

            //Non Production expenses
            //'=Administration+Selling+Financial fixed expenses+Financial variable expenses
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Non Production expenses",
                RowKeyIntCode = "Non_Production_expenses",
                OfferOverviewType = "SummaryExpenses",
                Indicator_Value = SumIndicatorValue("SummaryExpenses", "SummaryExpensesAdministration", "SummaryExpensesSelling", "Financial_Fixed_Expenses","Financial_Variable_Expenses"   ),
                RowType = ETEMEnums.DataRowType.Total
            });


            #endregion

            #region ETEM CALCULATIONS
            //Length of final PC (mm)
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Length of final PC (mm)",
                RowKeyIntCode = "LengthOfFinalPC",
                OfferOverviewType = "EtemCalculations",
                Indicator_Value = offer.LengthOfFinalPC.HasValue ? offer.LengthOfFinalPC.Value : Decimal.Zero,
                RowType = ETEMEnums.DataRowType.Data
            });


            //Tonnage (kg)
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Tonnage (kg)",
                RowKeyIntCode = "Tonnage_KG",
                OfferOverviewType = "EtemCalculations",
                Indicator_Value = offer.Tonnage.HasValue ? offer.Tonnage.Value * 1000: Decimal.Zero,
                RowType = ETEMEnums.DataRowType.Data
            }); 

            //'=Pcs for the whole project, tab Inquiry data
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Pcs for the whole project",
                RowKeyIntCode = "PcsForTheWholeProject",
                OfferOverviewType = "EtemCalculations",
                Indicator_Value = offer.PcsForTheWholeProject.HasValue ? offer.PcsForTheWholeProject.Value : Decimal.Zero,
                RowType = ETEMEnums.DataRowType.Data
            }); 


            
            //'Price EUR/PC
            //'=Total sales price (EUR/ton)/1000*Weight per PC (kg), tab Inquiry data
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Price EUR/PC",
                RowKeyIntCode = "Price_EUR_PC",
                OfferOverviewType = "EtemCalculations",
                Indicator_Value = listOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == "TotalSalesPrice_EUR_TON").Indicator_Value * 
                                  (offer.WeightPerPC.HasValue ? offer.WeightPerPC.Value : Decimal.Zero) / 1000 ,
                RowType = ETEMEnums.DataRowType.Data
            }); 


             
            //'Total cost EUR/PC (excl. SGA)
            //'=Total Cost (incl. Extrusion, Packaging, Transportation and Commission)/1000*Weight per PC (kg), tab Inquiry data
            ProductCostDataView ProductCostTotalCostExtrusionPackagingTransportationAndCommission = productCost.ListProductCosts.Where(p => p.RowKeyIntCode == "TotalCostExtrusionPackagingTransportationAndCommission").FirstOrDefault();
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Total cost EUR/PC (excl. SGA)",
                RowKeyIntCode = "Total_Cost_EUR_PC_Excl_SGA",
                OfferOverviewType = "EtemCalculations",
                Indicator_Value = ProductCostTotalCostExtrusionPackagingTransportationAndCommission.Value_EUR_PC,
                RowType = ETEMEnums.DataRowType.Data
            }); 


             //Gross (%)
            //=(Price EUR/PC-Total cost EUR/PC (excl. SGA))/Price EUR/PC*100            
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Gross (%)",
                RowKeyIntCode = "EtemCalculationsGross",
                OfferOverviewType = "EtemCalculations",
                Indicator_Value = CalcPercentByListOfferIndicator(listOfferOverview, "Price_EUR_PC", "Total_Cost_EUR_PC_Excl_SGA" ),
                RowType = ETEMEnums.DataRowType.Data
            });
 

            
            ProductCostDataView ProductCostTotalCost = productCost.ListProductCosts.Where(p => p.RowKeyIntCode == "TotalCost").FirstOrDefault();
            //Total cost EUR/PC (incl. SGA, Finance, etc.)
            //'=TOTAL COST (EUR/ton) (incl. Extrusion, Packaging, Transportation and Commission, SGA's and Financials), tab Product cost/1000*Weight per PC (kg), tab Inquiry data    
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Total cost EUR/PC (incl. SGA, Finance, etc.)",
                RowKeyIntCode = "TotalCost_EUR_PC_incl_SGA_Finance_etc",
                OfferOverviewType = "EtemCalculations",
                Indicator_Value = ProductCostTotalCost.Value_EUR_PC,
                RowType = ETEMEnums.DataRowType.Data
            });


            //Net Profit (%)
            //'=(Price EUR/PC-Total cost EUR/PC (+SGA, Finance, etc))/Price EUR/PC*100

           

            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Net Profit (%)",
                RowKeyIntCode = "Net_Profit_Percent_PC",
                OfferOverviewType = "EtemCalculations",
                Indicator_Value = CalcPercentByListOfferIndicator(listOfferOverview, "Price_EUR_PC","TotalCost_EUR_PC_incl_SGA_Finance_etc"),
                RowType = ETEMEnums.DataRowType.Data
            });
        

             Decimal price_EUR_PC   = listOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == "Price_EUR_PC").Indicator_Value.HasValue?listOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == "Price_EUR_PC").Indicator_Value.Value:Decimal.Zero;
            //Turnover (EUR)
            //'=Price EUR/PC*Pcs for the whole project
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Turnover (EUR)",
                RowKeyIntCode = "Turnover_EUR",
                OfferOverviewType = "EtemCalculations",
                Indicator_Value = price_EUR_PC * offer.PcsForTheWholeProject,
                RowType = ETEMEnums.DataRowType.Data
            });

             
            //Total cost (EUR) (excl. SGA)
            //Total cost EUR/PC (excl. SGA)*Pcs for the whole project
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Total cost (EUR) (excl. SGA)",
                RowKeyIntCode = "Total_Cost_EUR_excl_SGA",
                OfferOverviewType = "EtemCalculations",
                Indicator_Value =  listOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == "Total_Cost_EUR_PC_Excl_SGA").Indicator_Value * offer.PcsForTheWholeProject,
                RowType = ETEMEnums.DataRowType.Data
            });


             
            //Gross (%)
            //'=(Turnover (EUR)-Total cost (EUR) (excl. SGA))/Turnover (EUR)*100
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Gross (%)",
                RowKeyIntCode = "EtemCalculationsGrossTurnover",
                OfferOverviewType = "EtemCalculations",
                Indicator_Value = CalcPercentByListOfferIndicator(listOfferOverview, "Turnover_EUR","Total_Cost_EUR_excl_SGA" ),
                RowType = ETEMEnums.DataRowType.Data
            });

            //Total cost EUR (incl. SGA, Finance, etc.)
            //Total cost EUR/PC (incl. SGA, Finance, etc.)*Pcs for the whole project
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Total cost EUR (incl. SGA, Finance, etc.)",
                RowKeyIntCode = "TotalCost_EUR_PC_incl_SGA_Finance_etc_PcsForTheWholeProject",
                OfferOverviewType = "EtemCalculations",
                Indicator_Value = listOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == "TotalCost_EUR_PC_incl_SGA_Finance_etc").Indicator_Value * offer.PcsForTheWholeProject,
                RowType = ETEMEnums.DataRowType.Data
            });


            //Net profit (EUR)
            //'=Turnover (EUR)-Total cost EUR (incl. SGA, Finance, etc.)
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Net profit (EUR)",
                RowKeyIntCode = "Net_profit_EUR",
                OfferOverviewType = "EtemCalculations",
                Indicator_Value = listOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == "Turnover_EUR").Indicator_Value - 
                                  listOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == "TotalCost_EUR_PC_incl_SGA_Finance_etc_PcsForTheWholeProject").Indicator_Value,
                RowType = ETEMEnums.DataRowType.Data
            });


            //Net profit (%)
            //'=(Turnover (EUR)-Total cost EUR (incl. SGA, Finance, etc.))/Turnover (EUR)*100
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Net profit (%)",
                RowKeyIntCode = "Net_Profit_Percent_Turnover",
                OfferOverviewType = "EtemCalculations",
                Indicator_Value = CalcPercentByListOfferIndicator(listOfferOverview, "Turnover_EUR","TotalCost_EUR_PC_incl_SGA_Finance_etc_PcsForTheWholeProject") ,
                RowType = ETEMEnums.DataRowType.Data
            });



            //Conversion (EUR)
            //'=Sales conversion (EUR/ton)/1000*Tonnage (kg)
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Conversion (EUR)",
                RowKeyIntCode = "ConversionTonnage",
                OfferOverviewType = "EtemCalculations",
                Indicator_Value = listOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == "SalesConversion_EUR_TON").Indicator_Value / 1000 * 
                                  listOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == "Tonnage_KG").Indicator_Value,
                RowType = ETEMEnums.DataRowType.Data
            });


            //Net Conversion  (EUR)
            //=Conversion (EUR)-Non Production expenses
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Net Conversion  (EUR)",
                RowKeyIntCode = "Net_Conversion_EUR",
                OfferOverviewType = "EtemCalculations",
                Indicator_Value = SumIndicatorValue("EtemCalculations", "ConversionTonnage") - SumIndicatorValue("SummaryExpenses", "Non_Production_expenses"),
                RowType = ETEMEnums.DataRowType.Data
            });

            //Contribution margin (EUR)
            //'=Turnover (EUR)-Production expenses+Fixed expenses
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "Contribution margin (EUR)",
                RowKeyIntCode = "Contribution_Margin_EUR",
                OfferOverviewType = "EtemCalculations",
                Indicator_Value =   SumIndicatorValue("EtemCalculations", "Turnover_EUR") +                                    
                                    SumIndicatorValue("SummaryExpenses", "Fixed_Expenses") -
                                    SumIndicatorValue("SummaryExpenses", "Production_Expenses") ,
                RowType = ETEMEnums.DataRowType.Data
            });



            try
            {
                
                //Marginal Contribution (%)
                //'=Contribution margin (EUR)/Turnover (EUR)*100
                listOfferOverview.Add(new OfferOverviewDataView()
                {
                    Name = "Marginal Contribution (%)",
                    RowKeyIntCode = "Marginal_Contribution_Percent",
                    OfferOverviewType = "EtemCalculations",
                    Indicator_Value = SumIndicatorValue("EtemCalculations", "Contribution_Margin_EUR") /  SumIndicatorValue("EtemCalculations", "Turnover_EUR") * 100,
                    RowType = ETEMEnums.DataRowType.Data
                });
            }
            catch
            {
                
                //Marginal Contribution (%)
                //'=Contribution margin (EUR)/Turnover (EUR)*100
                listOfferOverview.Add(new OfferOverviewDataView()
                {
                    Name = "Marginal Contribution (%)",
                    RowKeyIntCode = "Marginal_Contribution_Percent",
                    OfferOverviewType = "EtemCalculations",
                    Indicator_Value = Decimal.Zero,
                    RowType = ETEMEnums.DataRowType.Data
                });
            }
             //EBITDA
            //'=Net profit (EUR)+Financial fixed expenses+Financial variable expenses+Depreciation
            listOfferOverview.Add(new OfferOverviewDataView()
            {
                Name = "EBITDA (EUR)",
                RowKeyIntCode = "EBITDA",
                OfferOverviewType = "EtemCalculations",
                Indicator_Value = SumIndicatorValue("EtemCalculations", "Net_profit_EUR") + 
                                  SumIndicatorValue("SummaryExpenses", "Financial_Fixed_Expenses", "Financial_Variable_Expenses", "Depreciation"),
                RowType = ETEMEnums.DataRowType.Data
            });


            #endregion

            result.ListOfferOverview.AddRange(listOfferOverview);

            return result;
           
        }

        private Decimal GetPC_EUR_TON(string productCostType, string rowKeyIntCode)
        {
            ProductCostDataView productCostData = productCost.ListProductCosts.Where(p => p.RowKeyIntCode == rowKeyIntCode && p.ProductCostType == productCostType).FirstOrDefault();

            if (productCostData != null && productCostData.Value_EUR_ton.HasValue)
            {
                return productCostData.Value_EUR_ton.Value;
            }
            else
            {
                return Decimal.Zero;
            }
        }

        private Decimal SumIndicatorValue(object offerOverviewType , params string[] rowKeyIntCodes )
        {
            Decimal res = Decimal.Zero;

            foreach(string code in rowKeyIntCodes)
            {
                OfferOverviewDataView item = null;

                if (offerOverviewType == null)
                {
                    item = this.listOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == code);
                }
                else
                {
                    item = this.listOfferOverview.FirstOrDefault(p => p.OfferOverviewType == offerOverviewType.ToString() && p.RowKeyIntCode == code);
                }

               if (item != null && item.Indicator_Value.HasValue) 
               {
                   res += item.Indicator_Value.Value;
               }
            }             
            
            return res;             
           
        }

        private decimal? CalcPercentByListOfferIndicator(List<OfferOverviewDataView> listOfferOverview, string p1, string p2)
        {



            try
            {
                OfferOverviewDataView firstItem = listOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == p1);
                OfferOverviewDataView secondItem = listOfferOverview.FirstOrDefault(p => p.RowKeyIntCode == p2);

                if (firstItem == null ) { return null; }
                else if (firstItem.Indicator_Value.HasValue && firstItem.Indicator_Value.Value == Decimal.Zero)
                {
                    return null;
                }
                else
                {
                    decimal secondItemValue = Decimal.Zero;


                    if(secondItem != null && secondItem.Indicator_Value.HasValue)
                    {
                        secondItemValue = secondItem.Indicator_Value.Value;
                    }


                    return (firstItem.Indicator_Value.Value - secondItemValue) / firstItem.Indicator_Value.Value * 100;
                }
            }
            catch
            {
                return Decimal.Zero;
            }
            
            
        }


        internal OfferOverviewResult LoadOfferOverviewResultInTableRowsByOfferId(int idOffer, CallContext callContext)
        {
            OfferOverviewResult result = LoadOfferOverviewByOfferId(idOffer, callContext);


            TableRow[] arrProductCosts = new TableRow[0];
            
            TableRow tableRow = new TableRow();
            TableCell tableCell = new TableCell();
            TableHeaderRow tableHeaderRow = new TableHeaderRow();
            TableHeaderCell tableHeaderCell = new TableHeaderCell();
            int rowIndex = 0;

            tableHeaderRow = new TableHeaderRow();
            tableHeaderCell = new TableHeaderCell();

            List<OfferOverviewDataView> listCostTable = result.ListOfferOverview.Where(o => o.OfferOverviewType == "CostTable").ToList();
            List<OfferOverviewDataView> listSalesPrice = result.ListOfferOverview.Where(o => o.OfferOverviewType == "SalesPrice").ToList();
            List<OfferOverviewDataView> listEtemCalculations = result.ListOfferOverview.Where(o => o.OfferOverviewType == "EtemCalculations").ToList();
            List<OfferOverviewDataView> listSummaryExpenses = result.ListOfferOverview.Where(o => o.OfferOverviewType == "SummaryExpenses").ToList();

            #region CostTable
            rowIndex = 0;
            arrProductCosts = new TableRow[2 + listCostTable.Count];
            tableHeaderCell.Text = "Cost";
            tableHeaderCell.ColumnSpan = 2;
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

            tableHeaderCell = new TableHeaderCell();

            tableHeaderCell.Text = "EUR/ton";
            tableHeaderCell.CssClass = "GridExpenses_td_item_center";
            arrProductCosts[rowIndex].Cells.Add(tableHeaderCell);

            rowIndex++;

            foreach (OfferOverviewDataView itemCostTable in listCostTable)
            {
                tableRow = new TableRow();
                tableCell = new TableCell();

                tableCell.Text = itemCostTable.Name;
                if (itemCostTable.RowType == ETEMEnums.DataRowType.Total)
                {
                    tableCell.CssClass = "GridExpenses_td_item_total";
                }
                tableRow.Cells.Add(tableCell);


                tableCell = new TableCell();
                tableCell.Text = itemCostTable.Indicator_Value.ToStringFormatted();
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.CssClass = "GridExpenses_td_item_right";
                if (itemCostTable.RowType == ETEMEnums.DataRowType.Total)
                {
                    tableCell.CssClass += " GridExpenses_td_item_total";
                }
                tableRow.Cells.Add(tableCell);

                arrProductCosts[rowIndex++] = tableRow;
            } 
            
            result.OfferOverviewCostTableRows= arrProductCosts;

         

            #endregion

            #region SalesPrice

            rowIndex = 0;    
            arrProductCosts = new TableRow[2 + listSalesPrice.Count];
            tableHeaderRow = new TableHeaderRow();
            tableHeaderCell = new TableHeaderCell();

            tableHeaderCell.Text = "Sales Price";
            tableHeaderCell.ColumnSpan = 2;
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

            tableHeaderCell = new TableHeaderCell();

            tableHeaderCell.Text = "&nbsp;";
            tableHeaderCell.CssClass = "GridExpenses_td_item_center";
            arrProductCosts[rowIndex].Cells.Add(tableHeaderCell);
            
            rowIndex++;

            foreach (OfferOverviewDataView item in listSalesPrice)
            {
                tableRow = new TableRow();
                tableCell = new TableCell();

                tableCell.Text = item.Name;
                if (item.RowType == ETEMEnums.DataRowType.Total)
                {
                    tableCell.CssClass = "GridExpenses_td_item_total";
                }
                tableRow.Cells.Add(tableCell);


                tableCell = new TableCell();
                tableCell.Text = item.Indicator_Value.ToStringFormatted();
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.CssClass = "GridExpenses_td_item_right";
                if (item.RowType == ETEMEnums.DataRowType.Total)
                {
                    tableCell.CssClass += " GridExpenses_td_item_total";
                }
                tableRow.Cells.Add(tableCell);

                arrProductCosts[rowIndex++] = tableRow;
            }

            result.OfferOverviewSalesPriceTableRows = arrProductCosts;
            arrProductCosts = new TableRow[0]; 
            #endregion

            #region ETEM CALCULATIONS
            rowIndex = 0;
            tableHeaderRow = new TableHeaderRow();
            tableHeaderCell = new TableHeaderCell();

            arrProductCosts = new TableRow[2 + listEtemCalculations.Count];
            tableHeaderCell.Text = "ETEM CALCULATIONS";
            tableHeaderCell.ColumnSpan = 2;
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

            tableHeaderCell = new TableHeaderCell();

            tableHeaderCell.Text = "&nbsp;";
            tableHeaderCell.CssClass = "GridExpenses_td_item_center";
            arrProductCosts[rowIndex].Cells.Add(tableHeaderCell);

            rowIndex++;

            foreach (OfferOverviewDataView itemCostTable in listEtemCalculations)
            {
                tableRow = new TableRow();
                tableCell = new TableCell();

                tableCell.Text = itemCostTable.Name;
                if (itemCostTable.RowType == ETEMEnums.DataRowType.Total)
                {
                    tableCell.CssClass = "GridExpenses_td_item_total";
                }
                tableRow.Cells.Add(tableCell);


                tableCell = new TableCell();
                tableCell.Text = itemCostTable.Indicator_Value.ToStringFormatted();
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.CssClass = "GridExpenses_td_item_right";
                if (itemCostTable.RowType == ETEMEnums.DataRowType.Total)
                {
                    tableCell.CssClass += " GridExpenses_td_item_total";
                }
                tableRow.Cells.Add(tableCell);

                arrProductCosts[rowIndex++] = tableRow;
            }

            result.OfferOverviewEtemCalculationsTableRows = arrProductCosts; 
            #endregion

            #region Summary Expenses
            rowIndex = 0;
            tableHeaderRow = new TableHeaderRow();
            tableHeaderCell = new TableHeaderCell();

            arrProductCosts = new TableRow[2 + listSummaryExpenses.Count];
            tableHeaderCell.Text = "Summary Expenses";
            tableHeaderCell.ColumnSpan = 2;
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

            tableHeaderCell = new TableHeaderCell();

            tableHeaderCell.Text = "EUR";
            tableHeaderCell.CssClass = "GridExpenses_td_item_center";
            arrProductCosts[rowIndex].Cells.Add(tableHeaderCell);

            rowIndex++;

            foreach (OfferOverviewDataView itemCostTable in listSummaryExpenses)
            {
                tableRow = new TableRow();
                tableCell = new TableCell();

                tableCell.Text = itemCostTable.Name;
                if (itemCostTable.RowType == ETEMEnums.DataRowType.Total)
                {
                    tableCell.CssClass = "GridExpenses_td_item_total";
                }
                tableRow.Cells.Add(tableCell);


                tableCell = new TableCell();
                tableCell.Text = itemCostTable.Indicator_Value.ToStringFormatted();
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.CssClass = "GridExpenses_td_item_right";
                if (itemCostTable.RowType == ETEMEnums.DataRowType.Total)
                {
                    tableCell.CssClass += " GridExpenses_td_item_total";
                }
                tableRow.Cells.Add(tableCell);

                arrProductCosts[rowIndex++] = tableRow;
            }


            result.OfferOverviewSummaryExpensesTableRows = arrProductCosts; 
            #endregion


            result.ResultContext = callContext;
            result.ResultContext.ResultCode = ETEMEnums.ResultEnum.Success;  

            return result;
           
        }
    }

    public class OfferOverviewResult
    {
        public OfferOverviewResult()
        {
            ListOfferOverview = new List<OfferOverviewDataView>();
            OfferOverviewCostTableRows = new TableRow[0];
            OfferOverviewSalesPriceTableRows = new TableRow[0];
            OfferOverviewEtemCalculationsTableRows = new TableRow[0];
            OfferOverviewSummaryExpensesTableRows = new TableRow[0];
        }
        public CallContext ResultContext { get; set; }
        public List<OfferOverviewDataView> ListOfferOverview { get; set; }
        public TableRow[] OfferOverviewCostTableRows { get; set; }
        public TableRow[] OfferOverviewSalesPriceTableRows { get; set; }
        public TableRow[] OfferOverviewEtemCalculationsTableRows { get; set; }
        public TableRow[] OfferOverviewSummaryExpensesTableRows { get; set; }
        
    }
}