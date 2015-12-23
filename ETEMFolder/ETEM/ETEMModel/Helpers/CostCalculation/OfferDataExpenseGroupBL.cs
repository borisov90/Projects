using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Models;
using ETEMModel.Models.DataView;
using ETEMModel.Models.DataView.CostCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using MoreLinq;
using System.Data;

namespace ETEMModel.Helpers.CostCalculation
{
    
    public class OfferDataExpenseGroupBL : BaseClassBL<OfferDataExpenseGroup>
    {

        public OfferDataExpenseGroupBL()
        {
            this.EntitySetName = "OfferDataExpenseGroups";
        }

        internal override void EntityToEntity(OfferDataExpenseGroup sourceEntity, OfferDataExpenseGroup targetEntity)
        {
            targetEntity.idOffer            = sourceEntity.idOffer;
            targetEntity.idSAPData          = sourceEntity.idSAPData;
            targetEntity.idCostCenter       = sourceEntity.idCostCenter;
            targetEntity.idExpensesType     = sourceEntity.idExpensesType;
            targetEntity.ValueData          = sourceEntity.ValueData;
            targetEntity.idCreateUser       = sourceEntity.idCreateUser;
            targetEntity.dCreate            = sourceEntity.dCreate;
            targetEntity.idModifyUser       = sourceEntity.idModifyUser;
            targetEntity.dModify            = sourceEntity.dModify;
        }

        internal override OfferDataExpenseGroup GetEntityById(int idEntity)
        {
            return this.dbContext.OfferDataExpenseGroups.Where(w => w.idOfferDataExpenseGroup == idEntity).FirstOrDefault();
        }

        internal CallContext OfferDataExpenseGroupSave(OfferDataExpenseGroup offerDataExpenseGroup, CallContext callContext)
        {
            callContext = this.EntitySave<OfferDataExpenseGroup>(offerDataExpenseGroup, callContext);
            return callContext;
        }

        internal bool HasOfferDataExpenseByOffer(int idOffer)
        {
            return this.dbContext.OfferDataExpenseGroups.Count(o => o.idOffer == idOffer) > 0;
        }

        internal List<OfferDataExpenseGroupView> GetAllOfferDataExpenseGroupByOffer(int idOffer)
        {

            List<OfferDataExpenseGroupView> listView = (from odeg in dbContext.OfferDataExpenseGroups

                                                        //CostCenterName                                                           
                                                        join kvCC in dbContext.KeyValues on odeg.idCostCenter equals kvCC.idKeyValue into grCC
                                                        from subCC in grCC.DefaultIfEmpty()
                                                        //ExpensesTypeName 
                                                        join kvET in dbContext.KeyValues on odeg.idExpensesType equals kvET.idKeyValue into grET
                                                        from subET in grET.DefaultIfEmpty()

                                                        where odeg.idOffer == idOffer

                                                        select new OfferDataExpenseGroupView
                                                        {
                                                            idOfferDataExpenseGroup     = odeg.idOfferDataExpenseGroup,
                                                            idOffer                     = odeg.idOffer,
                                                            idSAPData                   = odeg.idSAPData,
                                                            idCostCenter                = odeg.idCostCenter,
                                                            idExpensesType              = odeg.idExpensesType,
                                                            ValueData                   = odeg.ValueData,
                                                            idCreateUser                = odeg.idCreateUser,
                                                            dCreate                     = odeg.dCreate,
                                                            idModifyUser                = odeg.idModifyUser,
                                                            dModify                     = odeg.dModify,                                                            

                                                            CostCenterName              = (subCC != null ? subCC.Name : string.Empty),
                                                            ExpensesTypeName            = (subET != null ? subET.Name : string.Empty),
                                                        }
                                                        ).ToList();

                            return listView;
        }

        public Dictionary<string, TableRow[]> LoadOfferDataCostCenterAndExpensesType(int idOffer, ETEMEnums.CalculationType calculationType, CallContext resultContext)
        {
            Dictionary<string, TableRow[]> result = new Dictionary<string, TableRow[]>();

            try
            {
                List<string> listKeyTypeIntCodes = new List<string>()
                                                                    {
                                                                        ETEMEnums.KeyTypeEnum.CostCenter.ToString(),
                                                                        ETEMEnums.KeyTypeEnum.ExpensesTypeGroup.ToString(),
                                                                    };

                List<KeyValueDataView> listKeyValuesToSAPDataCostCenterAndExpensesTypeGroup = new List<KeyValueDataView>();
                List<KeyValueDataView> listKeyValueCostCenter                       = new List<KeyValueDataView>();
                List<KeyValueDataView> listKeyValueExpensesTypeGroup                = new List<KeyValueDataView>();

                listKeyValuesToSAPDataCostCenterAndExpensesTypeGroup = (from kv in this.dbContext.KeyValues
                                                             join kt in this.dbContext.KeyTypes on kv.idKeyType equals kt.idKeyType
                                                             where listKeyTypeIntCodes.Contains(kt.KeyTypeIntCode)
                                                             orderby kv.V_Order ascending
                                                            

                                                             select new KeyValueDataView
                                                             {
                                                                 idKeyValue         = kv.idKeyValue,
                                                                 Name               = kv.Name,
                                                                 NameEN             = kv.NameEN,
                                                                 DefaultValue1      = kv.DefaultValue1,
                                                                 KeyValueIntCode    = kv.KeyValueIntCode,
                                                                 KeyTypeIntCode     = kt.KeyTypeIntCode
                                                             }
                                                             ).ToList<KeyValueDataView>();

                List<OfferDataExpenseGroupView> listGroupExpense    = GetAllOfferDataExpenseGroupByOffer(idOffer);
                Offer offer                                         = new OfferBL().GetEntityById(idOffer);
                OfferProducitivity offerProducitivity               = new OfferProducitivityBL().GetOfferProducitivityByOfferID(idOffer);

                List<int> listIdCostCenter      = listGroupExpense.DistinctBy(k => k.idCostCenter).Select(k => k.idCostCenter).ToList();

                listKeyValueCostCenter          = listKeyValuesToSAPDataCostCenterAndExpensesTypeGroup.Where(w => listIdCostCenter.Contains(w.idKeyValue)).ToList();

                listKeyValueExpensesTypeGroup   = listKeyValuesToSAPDataCostCenterAndExpensesTypeGroup.Where(w => w.KeyTypeIntCode == ETEMEnums.KeyTypeEnum.ExpensesTypeGroup.ToString()).ToList();

                int countRows                   = listKeyValueExpensesTypeGroup.Count + 3;

                if (calculationType == ETEMEnums.CalculationType.EUR_TON)
                {
                    countRows = listKeyValueExpensesTypeGroup.Count + 4;
                }
                

                TableRow[] arrExpensesTypeGroup = new TableRow[countRows];

                TableRow tableRow                   = new TableRow();
                TableCell tableCell                 = new TableCell();
                TableHeaderRow tableHeaderRow       = new TableHeaderRow();
                TableHeaderCell tableHeaderCell     = new TableHeaderCell();
                int rowIndex                        = 0;

                #region Expenses by Cost Centers (EUR/MH)

                string eurTonMHbr   = string.Empty;
                string eurTonMH     = string.Empty;
                int columnSpan      = 2;

                if (calculationType == ETEMEnums.CalculationType.EUR_MH)
                {
                    eurTonMHbr  = "<br/>(EUR/MH)";
                    eurTonMH    = "(EUR/MH)";
                    columnSpan  --;
                }
                else
                {
                    eurTonMHbr  = "<br/>(EUR/ton)";
                    eurTonMH    = "(EUR/ton)";
                }
              
                tableHeaderRow              = new TableHeaderRow();
                tableHeaderCell             = new TableHeaderCell();

                tableHeaderCell.Text        = "Expenses by Cost Centers " + eurTonMH;
                tableHeaderCell.ColumnSpan  = listIdCostCenter.Count + columnSpan;
                tableHeaderCell.CssClass    = "GridExpenses_tr_th_Main";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrExpensesTypeGroup[rowIndex++] = tableHeaderRow;

                tableHeaderRow              = new TableHeaderRow();
                tableHeaderCell             = new TableHeaderCell();

                if (calculationType == ETEMEnums.CalculationType.EUR_TON)
                {

                    tableHeaderCell.Text = "Expenses " + eurTonMHbr;
                }
                else
                {
                    tableHeaderCell.Text = "Expenses";
                }

                tableHeaderCell.Width       = Unit.Pixel(320);
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrExpensesTypeGroup[rowIndex] = tableHeaderRow;


                #region Add header and footer columns and rows

                //Calculate tableHeaderCell width
                int headerWidth;

                if (calculationType == ETEMEnums.CalculationType.EUR_MH)
                {
                    headerWidth = (int)(100 / (listKeyValueCostCenter.Count + 1));
                }
                else
                {
                    headerWidth = (int)(100 / (listKeyValueCostCenter.Count + 2));
                }


                // Add header columns
                foreach (KeyValueDataView kvCostCenter in listKeyValueCostCenter)
                {
                    if (kvCostCenter.DefaultValue1 == "Packaging" && calculationType == ETEMEnums.CalculationType.EUR_TON)
                    {
                        //add total extrusion column
                        if (calculationType == ETEMEnums.CalculationType.EUR_TON)
                        {
                            //TotalColumn
                            tableHeaderCell         = new TableHeaderCell();
                            tableHeaderCell.Text    = "Total Extrusion " + eurTonMHbr;
                            //В офертата да направим колоните в таблиците с разходите с еднакъв размер
                            tableHeaderCell.Width   = new Unit(headerWidth.ToString() + "%");

                            arrExpensesTypeGroup[rowIndex].Cells.Add(tableHeaderCell);
                        }
                    }

                    tableHeaderCell         = new TableHeaderCell();
                    tableHeaderCell.Text    = kvCostCenter.Name;

                    //В офертата да направим колоните в таблиците с разходите с еднакъв размер
                    tableHeaderCell.Width = new Unit(headerWidth.ToString() + "%");


                    if(kvCostCenter.DefaultValue1 == "DIES" && calculationType == ETEMEnums.CalculationType.EUR_MH)
                    {
                        tableHeaderCell.Text += "<br/>" + "(EUR/kg)";
                    }
                     
                    arrExpensesTypeGroup[rowIndex].Cells.Add(tableHeaderCell);
                }
                
                //add total extrusion column
                //if (calculationType == ETEMEnums.CalculationType.EUR_TON)
                //{
                //    //TotalColumn
                //    tableHeaderCell = new TableHeaderCell();
                //    tableHeaderCell.Text = "Total Extrusion " + eurTonMH;
                //    //В офертата да направим колоните в таблиците с разходите с еднакъв размер
                //    tableHeaderCell.Width = new Unit(headerWidth.ToString() + "%");

                //    arrExpensesTypeGroup[rowIndex].Cells.Add(tableHeaderCell);
                //}

                rowIndex++;
                // Add header rows
                foreach (KeyValueDataView kvExpensesTypeGroup in listKeyValueExpensesTypeGroup)
                {
                    tableRow    = new TableRow();
                    tableCell   = new TableCell();

                    tableCell.Text = kvExpensesTypeGroup.Name.Replace("group", "");

                    tableRow.Cells.Add(tableCell);                    

                    arrExpensesTypeGroup[rowIndex++] = tableRow;
                }

                int i = 0;
                if (calculationType == ETEMEnums.CalculationType.EUR_TON)
                {

                    TableRow tableFooterRow         = new TableRow();
                    tableCell                       = new TableCell();
                    tableCell.Text                  = "Die depreciation";
                    tableFooterRow.Cells.Add(tableCell);
                    arrExpensesTypeGroup[rowIndex]  = tableFooterRow;

                    rowIndex++;
                    i++;
                }

                TableRow tableFooterTotalRow    = new TableRow();
                tableCell                       = new TableCell();
                tableCell.Text                  = "Total " + eurTonMH;
                tableCell.Font.Bold             = true;
                tableCell.CssClass              = "MainGrid_td_item_right GridExpenses_td_background_yellow";
                tableFooterTotalRow.Cells.Add(tableCell);

                arrExpensesTypeGroup[rowIndex] = tableFooterTotalRow;

                #endregion

                Dictionary<string, decimal> dictTotalByColl         = new Dictionary<string, decimal>();
                
                decimal totalByRowTon   = decimal.Zero;
                decimal currValue       = decimal.Zero;
                decimal totalByTotalRow = decimal.Zero;

                rowIndex = rowIndex - (listKeyValueExpensesTypeGroup.Count + i);

                foreach (KeyValueDataView kvExpensesTypeGroup in listKeyValueExpensesTypeGroup)
                {
                    totalByRowTon   = decimal.Zero;

                    foreach (KeyValueDataView kvCostCenter in listKeyValueCostCenter)
                    {
                        tableCell                   = new TableCell();
                        tableCell.HorizontalAlign   = HorizontalAlign.Right;
                        tableCell.CssClass          = "MainGrid_td_item_right";

                        var currGroupExpense = listGroupExpense.Where(w => w.idExpensesType == kvExpensesTypeGroup.idKeyValue &&
                                                                      w.idCostCenter == kvCostCenter.idKeyValue).FirstOrDefault();

                        if (currGroupExpense != null)
                        {
                            currValue = Convert.ToDecimal(currGroupExpense.ValueData);

                            if (dictTotalByColl.ContainsKey(kvCostCenter.KeyValueIntCode))
                            {
                                if (calculationType == ETEMEnums.CalculationType.EUR_MH)
                                {
                                    dictTotalByColl[kvCostCenter.KeyValueIntCode] += currValue;
                                }
                                else if (calculationType == ETEMEnums.CalculationType.EUR_TON)
                                {
                                    #region EUR_TON
                                    if (kvCostCenter.DefaultValue1 == "Press")
                                    {
                                        if (offerProducitivity != null && offerProducitivity.PressProducitivity_TON_MH != decimal.Zero)
                                        {
                                            dictTotalByColl[kvCostCenter.KeyValueIntCode] += currValue / offerProducitivity.PressProducitivity_TON_MH;
                                            currValue = currValue / offerProducitivity.PressProducitivity_TON_MH;                                            
                                        }
                                        else
                                        {
                                            dictTotalByColl[kvCostCenter.KeyValueIntCode] += currValue;
                                        }

                                        totalByRowTon += currValue;
                                    }
                                    else if (kvCostCenter.DefaultValue1 == "COMETAL")
                                    {
                                        if (offerProducitivity != null && offerProducitivity.COMetalProducitivity_TON_MH != decimal.Zero)
                                        {
                                            dictTotalByColl[kvCostCenter.KeyValueIntCode] += currValue / offerProducitivity.COMetalProducitivity_TON_MH;

                                            currValue = currValue / offerProducitivity.COMetalProducitivity_TON_MH;
                                        }
                                        else
                                        {
                                            dictTotalByColl[kvCostCenter.KeyValueIntCode] += currValue;
                                        }

                                        totalByRowTon += currValue;
                                    }
                                    else if (kvCostCenter.DefaultValue1 == "QualityControl")
                                    {
                                        if (offerProducitivity != null && offerProducitivity.QCProducitivity_TON_MH != decimal.Zero)
                                        {
                                            dictTotalByColl[kvCostCenter.KeyValueIntCode] += currValue / offerProducitivity.QCProducitivity_TON_MH;
                                            currValue = currValue / offerProducitivity.QCProducitivity_TON_MH;
                                        }
                                        else
                                        {
                                            dictTotalByColl[kvCostCenter.KeyValueIntCode] += currValue;
                                        }

                                        totalByRowTon += currValue;
                                    }
                                    else if (kvCostCenter.DefaultValue1 == "Packaging")
                                    {

                                        ///
                                        //fill total in each row

                                        TableCell tableCellTotal    = new TableCell();

                                        tableCellTotal.Text         = totalByRowTon.ToString("N", BaseHelper.GetNumberFormatInfo(".", ",", 4));
                                        totalByTotalRow             += totalByRowTon;

                                        tableCellTotal.Font.Bold    = true;
                                        tableCellTotal.CssClass     = "MainGrid_td_item_right GridExpenses_td_background_yellow";
                                        arrExpensesTypeGroup[rowIndex].Cells.Add(tableCellTotal);


                                        ///

                                        if (offerProducitivity != null && offerProducitivity.PackagingProducitivity_TON_MH != decimal.Zero)
                                        {
                                            dictTotalByColl[kvCostCenter.KeyValueIntCode] += currValue / offerProducitivity.PackagingProducitivity_TON_MH;
                                            currValue = currValue / offerProducitivity.PackagingProducitivity_TON_MH;
                                        }
                                        else
                                        {
                                            dictTotalByColl[kvCostCenter.KeyValueIntCode] += currValue;
                                        }                                        
                                    }
                                    else if (kvCostCenter.DefaultValue1 == "DIES")
                                    {
                                        //for Direct depreciation Expenses
                                        //offer.CostOfDie

                                        if (kvExpensesTypeGroup.KeyValueIntCode != "DirectDepreciationGroup")
                                        {
                                            currValue = currValue * 1000;
                                            dictTotalByColl[kvCostCenter.KeyValueIntCode]   += currValue; //For DIES Department formula is different: '=Value (EUR/kg)*1000                                            
                                        }
                                        else
                                        {
                                            dictTotalByColl[kvCostCenter.KeyValueIntCode] += decimal.Zero;
                                            currValue = decimal.Zero;
                                        }

                                        totalByRowTon += currValue;
                                        
                                    } 
                                    #endregion
                                }
                                else
                                {
                                    dictTotalByColl[kvCostCenter.KeyValueIntCode] += decimal.Zero;
                                }
                            }
                            else
                            {
                                if (calculationType == ETEMEnums.CalculationType.EUR_MH)
                                {
                                    dictTotalByColl.Add(kvCostCenter.KeyValueIntCode, currValue);
                                }
                                else if (calculationType == ETEMEnums.CalculationType.EUR_TON)
                                {
                                    #region EUR_TON
                                    if (kvCostCenter.DefaultValue1 == "Press")
                                    {
                                        if (offerProducitivity != null && offerProducitivity.PressProducitivity_TON_MH != decimal.Zero)
                                        {
                                            dictTotalByColl.Add(kvCostCenter.KeyValueIntCode, currValue / offerProducitivity.PressProducitivity_TON_MH);
                                            currValue = currValue / offerProducitivity.PressProducitivity_TON_MH;
                                        }
                                        else
                                        {
                                            dictTotalByColl.Add(kvCostCenter.KeyValueIntCode, currValue);
                                        }

                                        totalByRowTon += currValue;
                                    }
                                    else if (kvCostCenter.DefaultValue1 == "COMETAL")
                                    {
                                        if (offerProducitivity != null && offerProducitivity.COMetalProducitivity_TON_MH != decimal.Zero)
                                        {
                                            dictTotalByColl.Add(kvCostCenter.KeyValueIntCode, currValue / offerProducitivity.COMetalProducitivity_TON_MH);
                                            currValue = currValue / offerProducitivity.COMetalProducitivity_TON_MH;
                                        }
                                        else
                                        {
                                            dictTotalByColl.Add(kvCostCenter.KeyValueIntCode, currValue);
                                        }

                                        totalByRowTon += currValue;
                                    }
                                    else if (kvCostCenter.DefaultValue1 == "QualityControl")
                                    {
                                        if (offerProducitivity != null && offerProducitivity.QCProducitivity_TON_MH != decimal.Zero)
                                        {
                                            dictTotalByColl.Add(kvCostCenter.KeyValueIntCode, currValue / offerProducitivity.QCProducitivity_TON_MH);
                                            currValue = currValue / offerProducitivity.QCProducitivity_TON_MH;
                                        }
                                        else
                                        {
                                            dictTotalByColl.Add(kvCostCenter.KeyValueIntCode, currValue);
                                        }

                                        totalByRowTon += currValue;
                                    }
                                    else if (kvCostCenter.DefaultValue1 == "Packaging")
                                    {

                                        ///
                                        //fill total in each row

                                        TableCell tableCellTotal    = new TableCell();

                                        tableCellTotal.Text         = totalByRowTon.ToString("N", BaseHelper.GetNumberFormatInfo(".", ",", 4));
                                        totalByTotalRow             += totalByRowTon;

                                        tableCellTotal.Font.Bold    = true;
                                        tableCellTotal.CssClass     = "MainGrid_td_item_right GridExpenses_td_background_yellow";
                                        arrExpensesTypeGroup[rowIndex].Cells.Add(tableCellTotal);

                                        ///

                                        if (offerProducitivity != null && offerProducitivity.PackagingProducitivity_TON_MH != decimal.Zero)
                                        {
                                            dictTotalByColl.Add(kvCostCenter.KeyValueIntCode, currValue / offerProducitivity.PackagingProducitivity_TON_MH);
                                            currValue = currValue / offerProducitivity.PackagingProducitivity_TON_MH;
                                        }
                                        else
                                        {
                                            dictTotalByColl.Add(kvCostCenter.KeyValueIntCode, currValue);
                                        }                                      
                                    }
                                    else if (kvCostCenter.DefaultValue1 == "DIES")
                                    {
                                        //for Direct depreciation Expenses
                                        //offer.CostOfDie
                                        //dictTotalByColl[kvCostCenter.KeyValueIntCode] = currValue * 1000; //For DIES Department formula is different: '=Value (EUR/kg)*1000
                                        if (kvExpensesTypeGroup.KeyValueIntCode != "DirectDepreciationGroup")
                                        {
                                            dictTotalByColl.Add(kvCostCenter.KeyValueIntCode, currValue * 1000); //For DIES Department formula is different: '=Value (EUR/kg)*1000
                                            currValue       = currValue * 1000;                                            
                                        }
                                        else
                                        {
                                            currValue = decimal.Zero;
                                            dictTotalByColl.Add(kvCostCenter.KeyValueIntCode, currValue);
                                           
                                        }

                                        totalByRowTon += currValue;

                                    }
                                    #endregion
                                }
                                else
                                {
                                    dictTotalByColl.Add(kvCostCenter.KeyValueIntCode, decimal.Zero);
                                }
                                
                            }

                            tableCell.Text = currValue.ToString("N", BaseHelper.GetNumberFormatInfo(".", ",", 4));
                            arrExpensesTypeGroup[rowIndex].Cells.Add(tableCell);
                        }
                    }

                    ////fill total in each row
                    //if (calculationType == ETEMEnums.CalculationType.EUR_TON)
                    //{
                    //    tableCell = new TableCell();


                    //    tableCell.Text  = totalByRowTon.ToString("N", BaseHelper.GetNumberFormatInfo(".", ",", 4));
                    //    totalByTotalRow += totalByRowTon;


                    //    tableCell.Font.Bold = true;
                    //    tableCell.CssClass  = "MainGrid_td_item_right";
                    //    arrExpensesTypeGroup[rowIndex].Cells.Add(tableCell);
                    //}

                    rowIndex++;                    
                }

                string dieDepreciation = decimal.Zero.ToString("N", BaseHelper.GetNumberFormatInfo(".", ",", 4));

                //Fill cell in additional row 'Die depreciation'
                if (calculationType == ETEMEnums.CalculationType.EUR_TON)
                {
                    foreach (KeyValueDataView kvCostCenter in listKeyValueCostCenter)
                    {
                        tableCell                   = new TableCell();
                        tableCell.HorizontalAlign   = HorizontalAlign.Right;
                        tableCell.CssClass          = "MainGrid_td_item_right";

                        if (kvCostCenter.DefaultValue1 == "DIES")
                        {
                            if (offer.CostOfDieEUR_Per_TON_Computable.HasValue)
                            {
                                dictTotalByColl[kvCostCenter.KeyValueIntCode]   += offer.CostOfDieEUR_Per_TON_Computable.Value;
                                tableCell.Text                                  = offer.CostOfDieEUR_Per_TON_Computable.Value.ToString("N", BaseHelper.GetNumberFormatInfo(".", ",", 4)); //dictTotalByColl[kvCostCenter.KeyValueIntCode].ToString("N", BaseHelper.GetNumberFormatInfo(".", ",", 4));
                                totalByTotalRow                                 += offer.CostOfDieEUR_Per_TON_Computable.Value;
                            }
                            else
                            {
                                dictTotalByColl[kvCostCenter.KeyValueIntCode] += decimal.Zero;
                                tableCell.Text = decimal.Zero.ToString("N", BaseHelper.GetNumberFormatInfo(".", ",", 4));
                            }

                            dieDepreciation = tableCell.Text;
                        }
                        else if (kvCostCenter.DefaultValue1 == "Packaging")
                        {
                            //fill total in Die depreciation row
                            TableCell tableCellTotal        = new TableCell();
                            tableCellTotal.Font.Bold        = true;
                            tableCellTotal.CssClass         = "MainGrid_td_item_right GridExpenses_td_background_yellow";
                            tableCellTotal.Text             = dieDepreciation;
                            arrExpensesTypeGroup[rowIndex].Cells.Add(tableCellTotal);

                            tableCell.Text = decimal.Zero.ToString("N", BaseHelper.GetNumberFormatInfo(".", ",", 4));
                        }
                        else
                        {
                            tableCell.Text = decimal.Zero.ToString("N", BaseHelper.GetNumberFormatInfo(".", ",", 4));
                        }

                        arrExpensesTypeGroup[rowIndex].Cells.Add(tableCell);
                    }

                   
                    //fill total in Die depreciation row
                    //tableCell               = new TableCell();
                    //tableCell.Font.Bold     = true;
                    //tableCell.CssClass      = "MainGrid_td_item_right";
                    //tableCell.Text          = dieDepreciation;
                    //arrExpensesTypeGroup[rowIndex].Cells.Add(tableCell);

                    rowIndex++;                    
                }                

                foreach (KeyValueDataView kvCostCenter in listKeyValueCostCenter)
                {
                    if (kvCostCenter.DefaultValue1 == "Packaging" && calculationType == ETEMEnums.CalculationType.EUR_TON)
                    {
                        TableCell tableCellTotal    = new TableCell();
                        tableCellTotal.Font.Bold    = true;
                        tableCellTotal.CssClass     = "MainGrid_td_item_right GridExpenses_td_background_yellow";
                        tableCellTotal.Text         = totalByTotalRow.ToString("N", BaseHelper.GetNumberFormatInfo(".", ",", 4));
                        arrExpensesTypeGroup[rowIndex].Cells.Add(tableCellTotal);
                    }

                    tableCell                       = new TableCell();
                    tableCell.HorizontalAlign       = HorizontalAlign.Right;
                    tableCell.CssClass              = "MainGrid_td_item_right GridExpenses_td_background_yellow";
                    tableCell.Font.Bold             = true;
                    tableCell.Text                  = dictTotalByColl[kvCostCenter.KeyValueIntCode].ToString("N", BaseHelper.GetNumberFormatInfo(".", ",", 4));
                    arrExpensesTypeGroup[rowIndex].Cells.Add(tableCell);
                }

                //fill cell total on Total Extrusion
                //if (calculationType == ETEMEnums.CalculationType.EUR_TON)
                //{
                //    tableCell               = new TableCell();
                //    tableCell.Font.Bold     = true;
                //    tableCell.CssClass      = "MainGrid_td_item_right";
                //    tableCell.Text          = totalByTotalRow.ToString("N", BaseHelper.GetNumberFormatInfo(".", ",", 4));
                //    arrExpensesTypeGroup[rowIndex].Cells.Add(tableCell);
                //}                                

                #endregion

                result.Add("Expenses", arrExpensesTypeGroup);

            }
            catch (Exception ex)
            {
                resultContext.Message = "Error load SAP data expenses and quantities!";

                BaseHelper.Log("Error load SAP data expenses and quantities!");
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

            return result;
        }


        internal CallContext OfferDataExpenseGroupDelete(int idOffer, CallContext resultContext)
        {
            List<OfferDataExpenseGroup> list = this.dbContext.OfferDataExpenseGroups.Where(o => o.idOffer == idOffer).ToList();
            resultContext =  EntityDelete<OfferDataExpenseGroup>(list, resultContext);

            return resultContext;


        }
    }
}