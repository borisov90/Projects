using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Helpers.Extentions;
using ETEMModel.Models;
using ETEMModel.Models.DataView;
using ETEMModel.Models.DataView.CostCalculation;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace ETEMModel.Helpers.CostCalculation
{
    public class SAPDataBL : BaseClassBL<SAPData>
    {
        public SAPDataBL()
        {
            this.EntitySetName = "SAPDatas";
        }

        internal override void EntityToEntity(SAPData sourceEntity, SAPData targetEntity)
        {            
            targetEntity.DateFrom = sourceEntity.DateFrom;
            targetEntity.DateTo = sourceEntity.DateTo;
        }

        internal override SAPData GetEntityById(int idEntity)
        {
            return this.dbContext.SAPDatas.Where(w => w.idSAPData == idEntity).FirstOrDefault();
        }

        internal List<SAPDataDataView> GetAllSAPData(ICollection<AbstractSearch> searchCriteria,
                                                     DateTime? dateActiveTo,
                                                     string sortExpression, string sortDirection)
        {
            List<SAPDataDataView> listView = new List<SAPDataDataView>();

            listView = (from sd in this.dbContext.SAPDatas                        
                        where (dateActiveTo.HasValue ?
                               ((!sd.DateTo.HasValue && sd.DateFrom <= dateActiveTo.Value) ||
                                (sd.DateTo.HasValue && sd.DateFrom <= dateActiveTo.Value && sd.DateTo >= dateActiveTo.Value)) :
                              1 == 1)
                        orderby sd.DateFrom ascending
                        select new SAPDataDataView
                        {
                            idSAPData = sd.idSAPData,
                            DateFrom = sd.DateFrom,
                            DateTo = sd.DateTo
                        }).ApplySearchCriterias(searchCriteria).ToList<SAPDataDataView>();

            if (string.IsNullOrEmpty(sortExpression) || sortExpression == Constants.INVALID_ID_STRING)
            {
                sortDirection = string.Empty;

                sortExpression = "DateFrom";
            }

            listView = OrderByHelper.OrderBy<SAPDataDataView>(listView, sortExpression, sortDirection).ToList<SAPDataDataView>();
            
            return listView;
        }

        public SAPData GetSAPDataByDateActiveTo(DateTime dateActiveTo)
        {
            SAPData result = new SAPData();

            result = this.dbContext.SAPDatas.Where(w => (w.DateFrom <= dateActiveTo && !w.DateTo.HasValue) ||
                                                         (w.DateTo.HasValue &&
                                                          w.DateFrom <= dateActiveTo &&
                                                          w.DateTo >= dateActiveTo)).FirstOrDefault();

            return result;
        }

        internal CallContext SAPDataSave(List<SAPData> entities, CallContext resultContext)
        {
            if (entities.Count > 0)
            {
                List<SAPData> listSAPDatasActiveNotClosed = new List<SAPData>();

                var listNewSAPDatas = entities.Where(w => w.idSAPData == Constants.INVALID_ID ||
                                                          w.idSAPData == Constants.INVALID_ID_ZERO).ToList();

                if (listNewSAPDatas.Count > 0)
                {
                    listSAPDatasActiveNotClosed = (from sd in this.dbContext.SAPDatas
                                                   where !sd.DateTo.HasValue
                                                   select sd).ToList<SAPData>();
                }

                if (listSAPDatasActiveNotClosed.Count > 0)
                {
                    var minDateFrom = listNewSAPDatas.Min(m => m.DateFrom);

                    if (minDateFrom != DateTime.MinValue)
                    {
                        foreach (SAPData _SAPData in listSAPDatasActiveNotClosed)
                        {
                            _SAPData.DateTo = minDateFrom.AddDays(-1);
                        }

                        resultContext = base.EntitySave<SAPData>(listSAPDatasActiveNotClosed, resultContext);
                    }                    
                }

                if (resultContext.ResultCode == ETEMEnums.ResultEnum.Success)
                {
                    resultContext = base.EntitySave<SAPData>(entities, resultContext);
                }
            }

            return resultContext;
        }

        public CallContext SAPDataDelete(List<int> listSelectedIDs, CallContext resultContext)
        {
            try
            {
                resultContext.ResultCode = ETEMEnums.ResultEnum.Error;

                List<SAPData> listSAPDatas = new List<SAPData>();
                List<SAPDataExpense> listSAPDataExpenses = new List<SAPDataExpense>();
                List<SAPDataQuantity> listSAPDataQuantities = new List<SAPDataQuantity>();
                List<SAPDataCostCenterTotal> listSAPDataCostCenterTotals = new List<SAPDataCostCenterTotal>();

                listSAPDatas = (from sd in this.dbContext.SAPDatas
                                where listSelectedIDs.Contains(sd.idSAPData)
                                select sd).ToList();

                listSAPDataExpenses = (from sde in this.dbContext.SAPDataExpenses
                                       where listSelectedIDs.Contains(sde.idSAPData)
                                       select sde).ToList();

                listSAPDataQuantities = (from sdq in this.dbContext.SAPDataQuantities
                                         where listSelectedIDs.Contains(sdq.idSAPData)
                                         select sdq).ToList();

                listSAPDataCostCenterTotals = (from sdcct in this.dbContext.SAPDataCostCenterTotals
                                               where listSelectedIDs.Contains(sdcct.idSAPData)
                                               select sdcct).ToList();

                CallContext deleteContext = new CallContext();
                deleteContext = resultContext;

                deleteContext = new SAPDataCostCenterTotalBL().EntityDelete<SAPDataCostCenterTotal>(listSAPDataCostCenterTotals, deleteContext);

                if (deleteContext.ResultCode == ETEMEnums.ResultEnum.Success)
                {
                    deleteContext = new SAPDataQuantityBL().EntityDelete<SAPDataQuantity>(listSAPDataQuantities, deleteContext);

                    if (deleteContext.ResultCode == ETEMEnums.ResultEnum.Success)
                    {
                        deleteContext = new SAPDataExpensesBL().EntityDelete<SAPDataExpense>(listSAPDataExpenses, deleteContext);

                        if (deleteContext.ResultCode == ETEMEnums.ResultEnum.Success)
                        {
                            deleteContext = base.EntityDelete<SAPData>(listSAPDatas, deleteContext);

                            if (deleteContext.ResultCode == ETEMEnums.ResultEnum.Success)
                            {
                                resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
                                resultContext.Message = "Selected rows `SAP Data by Cost Centers` and their expenses and quantities have been deleted successfully!";
                            }
                            else
                            {
                                resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                                resultContext.Message = "Error delete selected rows `SAP Data by Cost Centers` and their expenses and quantities!";
                            }
                        }
                        else
                        {
                            resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                            resultContext.Message = "Error delete `Expenses by Cost Centers` for selected rows `SAP Data by Cost Centers`!";
                        }
                    }
                    else
                    {
                        resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                        resultContext.Message = "Error delete `Quantities by Cost Centers` for selected rows `SAP Data by Cost Centers`!";
                    }
                }
                else
                {
                    resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                    resultContext.Message = "Error delete `Totals by Cost Centers` for selected rows `SAP Data by Cost Centers`!";
                }
            }
            catch (Exception ex)
            {
                resultContext.Message = "Error delete selected rows `SAP Data by Cost Centers` and their expenses and quantities!";

                BaseHelper.Log("Error delete entities `SAPData`, IDs - (" + string.Join(",", listSelectedIDs.ToArray()) + ")!");
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

            return resultContext;
        }

        public Dictionary<string, TableRow[]> LoadSAPDataExpensesAndQuantities(int idEntity, CallContext resultContext)
        {
            Dictionary<string, TableRow[]> result = new Dictionary<string, TableRow[]>();

            try
            {
                List<string> listKeyTypeIntCodes = new List<string>()
                    {
                        ETEMEnums.KeyTypeEnum.CostCenter.ToString(),
                        ETEMEnums.KeyTypeEnum.ExpensesType.ToString(),
                        ETEMEnums.KeyTypeEnum.ExpensesTypeGroup.ToString(),
                        ETEMEnums.KeyTypeEnum.QuantityType.ToString()
                    };

                List<KeyValueDataView> listKeyValuesToSAPDataExpensesAndQuantity = new List<KeyValueDataView>();
                List<KeyValueDataView> listKeyValueCostCenter = new List<KeyValueDataView>();
                List<KeyValueDataView> listKeyValueExpensesType = new List<KeyValueDataView>();
                List<KeyValueDataView> listKeyValueExpensesTypeGroup = new List<KeyValueDataView>();
                List<KeyValueDataView> listKeyValueQuantityType = new List<KeyValueDataView>();

                listKeyValuesToSAPDataExpensesAndQuantity = (from kv in this.dbContext.KeyValues
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

                listKeyValueCostCenter = listKeyValuesToSAPDataExpensesAndQuantity.Where(w => w.KeyTypeIntCode == ETEMEnums.KeyTypeEnum.CostCenter.ToString()).ToList();
                listKeyValueExpensesType = listKeyValuesToSAPDataExpensesAndQuantity.Where(w => w.KeyTypeIntCode == ETEMEnums.KeyTypeEnum.ExpensesType.ToString()).ToList();
                listKeyValueExpensesTypeGroup = listKeyValuesToSAPDataExpensesAndQuantity.Where(w => w.KeyTypeIntCode == ETEMEnums.KeyTypeEnum.ExpensesTypeGroup.ToString()).ToList();
                listKeyValueQuantityType = listKeyValuesToSAPDataExpensesAndQuantity.Where(w => w.KeyTypeIntCode == ETEMEnums.KeyTypeEnum.QuantityType.ToString()).ToList();

                SAPData currSAPData = GetEntityById(idEntity);

                List<SAPDataExpense> listSAPDataExpenses = currSAPData.SAPDataExpenses.ToList<SAPDataExpense>();
                List<SAPDataQuantity> listSAPDataQuantities = currSAPData.SAPDataQuantities.ToList<SAPDataQuantity>();

                int countRows = 2 + listKeyValueExpensesType.Count + 
                                3 + listKeyValueExpensesTypeGroup.Count +
                                3 + listKeyValueQuantityType.Count + 
                                3 + listKeyValueExpensesTypeGroup.Count + 1;

                TableRow[] arrExpenses = new TableRow[countRows];
                TableRow[] arrExpensesTypeGroup = new TableRow[listKeyValueExpensesTypeGroup.Count + 1];
                TableRow[] arrQuantites = new TableRow[listKeyValueQuantityType.Count + 1];
                TableRow[] arrExpensesTypeGroupMH = new TableRow[listKeyValueExpensesTypeGroup.Count + 2];

                TableRow tableRow = new TableRow();
                TableCell tableCell = new TableCell();
                TableHeaderRow tableHeaderRow = new TableHeaderRow();
                TableHeaderCell tableHeaderCell = new TableHeaderCell();
                
                int rowIndex = 0;

                #region Detailed Expenses by Cost Centers (EUR)

                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();

                tableHeaderCell.Text = "Expenses by Cost Centers (EUR)";
                tableHeaderCell.ColumnSpan = 12;
                tableHeaderCell.CssClass = "GridExpenses_tr_th_Main";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrExpenses[rowIndex++] = tableHeaderRow;

                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();

                tableHeaderCell.Text = "Detailed Expenses";
                tableHeaderCell.Width = Unit.Pixel(320);
                tableHeaderCell.CssClass = "GridExpenses_td_item_center";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrExpenses[rowIndex] = tableHeaderRow;

                // Add header columns
                foreach (KeyValueDataView kvCostCenter in listKeyValueCostCenter)
                {                    
                    tableHeaderCell = new TableHeaderCell();

                    tableHeaderCell.Text = kvCostCenter.Name;
                    tableHeaderCell.CssClass = "GridExpenses_td_item_center";
                    arrExpenses[rowIndex].Cells.Add(tableHeaderCell);
                }
                rowIndex++;
                // Add header rows
                foreach (KeyValueDataView kvExpensesType in listKeyValueExpensesType)
                {
                    tableRow = new TableRow();
                    tableCell = new TableCell();

                    tableCell.Text = kvExpensesType.Name;
                    tableRow.Cells.Add(tableCell);

                    arrExpenses[rowIndex++] = tableRow;
                }

                rowIndex = rowIndex - listKeyValueExpensesType.Count;
                foreach (KeyValueDataView kvExpensesType in listKeyValueExpensesType)
                {                    
                    foreach (KeyValueDataView kvCostCenter in listKeyValueCostCenter)
                    {
                        tableCell = new TableCell();
                        tableCell.HorizontalAlign = HorizontalAlign.Right;
                        tableCell.CssClass = "GridExpenses_td_item_right";

                        var currExpense = listSAPDataExpenses.Where(w => w.idExpensesType == kvExpensesType.idKeyValue && w.idCostCenter == kvCostCenter.idKeyValue).FirstOrDefault();

                        if (currExpense != null)
                        {
                            tableCell.Text = currExpense.ValueDataRoundFormatted;
                            arrExpenses[rowIndex].Cells.Add(tableCell);
                        }
                    }
                    rowIndex++;
                }
                #endregion

                #region Expenses by Cost Centers (EUR)
                //rowIndex = 0;
                //rowIndex++;

                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();
                tableHeaderCell.ColumnSpan = 12;
                tableHeaderCell.CssClass = "GridExpenses_tr_th_Empty";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrExpenses[rowIndex++] = tableHeaderRow;

                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();

                tableHeaderCell.Text = "Expenses by Cost Centers (EUR)";               
                tableHeaderCell.ColumnSpan = 12;
                tableHeaderCell.CssClass = "GridExpenses_tr_th_Main";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrExpenses[rowIndex++] = tableHeaderRow;
                
                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();
                                
                tableHeaderCell.Text = "Expenses";
                tableHeaderCell.Width = Unit.Pixel(320);
                tableHeaderCell.CssClass = "GridExpenses_td_item_center";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrExpenses[rowIndex] = tableHeaderRow;

                // Add header columns
                foreach (KeyValueDataView kvCostCenter in listKeyValueCostCenter)
                {
                    tableHeaderCell = new TableHeaderCell();

                    tableHeaderCell.Text = kvCostCenter.Name;
                    tableHeaderCell.CssClass = "GridExpenses_td_item_center";
                    arrExpenses[rowIndex].Cells.Add(tableHeaderCell);
                }
                rowIndex++;
                // Add header rows
                foreach (KeyValueDataView kvExpensesTypeGroup in listKeyValueExpensesTypeGroup)
                {
                    tableRow = new TableRow();
                    tableCell = new TableCell();

                    tableCell.Text = kvExpensesTypeGroup.Name.Replace(" group", "");
                    tableRow.Cells.Add(tableCell);

                    arrExpenses[rowIndex++] = tableRow;
                }

                List<Expense> listGroupExpense = new List<Expense>();

                listGroupExpense = new ExpenseCalculationBL().LoadExpenseGroupByIdSAPData(idEntity);

                rowIndex = rowIndex - listKeyValueExpensesTypeGroup.Count;
                foreach (KeyValueDataView kvExpensesTypeGroup in listKeyValueExpensesTypeGroup)
                {
                    foreach (KeyValueDataView kvCostCenter in listKeyValueCostCenter)
                    {
                        tableCell = new TableCell();
                        tableCell.HorizontalAlign = HorizontalAlign.Right;
                        tableCell.CssClass = "GridExpenses_td_item_right";

                        var currGroupExpense = listGroupExpense.Where(w => w.ExpenseGroup.idKeyValue == kvExpensesTypeGroup.idKeyValue &&
                                                                      w.CostCenter.idKeyValue == kvCostCenter.idKeyValue).FirstOrDefault();

                        if (currGroupExpense != null)
                        {
                            tableCell.Text = currGroupExpense.ExpenseValueRoundFour.ToStringFormatted4();
                            arrExpenses[rowIndex].Cells.Add(tableCell);
                        }
                    }
                    rowIndex++;
                }                
                #endregion

                #region Quantity
//                rowIndex = 0;

                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();
                tableHeaderCell.ColumnSpan = 12;
                tableHeaderCell.CssClass = "GridExpenses_tr_th_Empty";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrExpenses[rowIndex++] = tableHeaderRow;

                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();

                tableHeaderCell.Text = "Quantity";
                tableHeaderCell.ColumnSpan = 12;
                tableHeaderCell.CssClass = "GridExpenses_tr_th_Main";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrExpenses[rowIndex++] = tableHeaderRow;

                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();

                tableHeaderCell.Text = "Quantity";
                tableHeaderCell.Width = Unit.Pixel(320);
                tableHeaderCell.CssClass = "GridExpenses_td_item_center";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrExpenses[rowIndex] = tableHeaderRow;

                // Add header columns
                foreach (KeyValueDataView kvCostCenter in listKeyValueCostCenter)
                {
                    tableHeaderCell = new TableHeaderCell();

                    tableHeaderCell.Text = kvCostCenter.Name;
                    tableHeaderCell.CssClass = "GridExpenses_td_item_center";
                    arrExpenses[rowIndex].Cells.Add(tableHeaderCell);
                }
                rowIndex++;
                // Add header rows
                foreach (KeyValueDataView kvQuantityType in listKeyValueQuantityType)
                {
                    tableRow = new TableRow();
                    tableCell = new TableCell();

                    tableCell.Text = kvQuantityType.Name;
                    tableRow.Cells.Add(tableCell);

                    arrExpenses[rowIndex++] = tableRow;
                }

                rowIndex = rowIndex - listKeyValueQuantityType.Count;
                foreach (KeyValueDataView kvQuantityType in listKeyValueQuantityType)
                {
                    foreach (KeyValueDataView kvCostCenter in listKeyValueCostCenter)
                    {
                        tableCell = new TableCell();
                        tableCell.HorizontalAlign = HorizontalAlign.Right;
                        tableCell.CssClass = "GridExpenses_td_item_right";

                        var currQuantity = listSAPDataQuantities.Where(w => w.idQuantityType == kvQuantityType.idKeyValue && w.idCostCenter == kvCostCenter.idKeyValue).FirstOrDefault();

                        if (currQuantity != null)
                        {
                            tableCell.Text = currQuantity.ValueDataRoundFormatted;
                            arrExpenses[rowIndex].Cells.Add(tableCell);
                        }
                    }
                    rowIndex++;
                }
                #endregion

                #region Expenses by Cost Centers (EUR/MH)
//                rowIndex = 0;

                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();
                tableHeaderCell.ColumnSpan = 12;
                tableHeaderCell.CssClass = "GridExpenses_tr_th_Empty";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrExpenses[rowIndex++] = tableHeaderRow;

                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();

                tableHeaderCell.Text = "Expenses by Cost Centers (EUR/MH)";
                tableHeaderCell.ColumnSpan = 12;
                tableHeaderCell.CssClass = "GridExpenses_tr_th_Main";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrExpenses[rowIndex++] = tableHeaderRow;

                tableHeaderRow = new TableHeaderRow();
                tableHeaderCell = new TableHeaderCell();

                tableHeaderCell.Text = "Expenses";
                tableHeaderCell.Width = Unit.Pixel(320);
                tableHeaderCell.CssClass = "GridExpenses_td_item_center";
                tableHeaderRow.Cells.Add(tableHeaderCell);

                arrExpenses[rowIndex] = tableHeaderRow;

                // Add header columns
                foreach (KeyValueDataView kvCostCenter in listKeyValueCostCenter)
                {
                    tableHeaderCell = new TableHeaderCell();

                    if (kvCostCenter.KeyValueIntCode == ETEMEnums.CostCenterEnum.DIESDepartment.ToString())
                    {
                        tableHeaderCell.Text = kvCostCenter.Name + "<br/>" + "(EUR/kg)";
                    }
                    else
                    {
                        tableHeaderCell.Text = kvCostCenter.Name;
                    }

                    tableHeaderCell.CssClass = "GridExpenses_td_item_center";
                    arrExpenses[rowIndex].Cells.Add(tableHeaderCell);
                }
                rowIndex++;
                // Add header rows
                foreach (KeyValueDataView kvExpensesTypeGroup in listKeyValueExpensesTypeGroup)
                {
                    tableRow = new TableRow();
                    tableCell = new TableCell();

                    tableCell.Text = kvExpensesTypeGroup.Name.Replace(" group", "");
                    tableRow.Cells.Add(tableCell);

                    arrExpenses[rowIndex++] = tableRow;
                }

                TableRow tableFooterRow = new TableRow();
                tableCell = new TableCell();
                tableCell.Text = "Total (EUR/MH)";
                tableCell.CssClass = "GridExpenses_td_item_total";
                tableFooterRow.Cells.Add(tableCell);

                arrExpenses[rowIndex] = tableFooterRow;

                //tableHeaderCell = new TableHeaderCell();
                //tableHeaderCell.Text = "Total (EUR/MH)";
                //tableHeaderCell.Font.Bold = true;

                //arrExpensesTypeGroupMH[0].Cells.Add(tableHeaderCell);

                listGroupExpense = new List<Expense>();

                listGroupExpense = new ExpenseCalculationBL().LoadExpenseGroupByIdSAPData(idEntity);

                Dictionary<string, decimal> dictTotalByColl = new Dictionary<string, decimal>();
                
                decimal totalByRow = decimal.Zero;
                decimal total = decimal.Zero;

                decimal currValue = decimal.Zero;

                rowIndex = rowIndex - (listKeyValueExpensesTypeGroup.Count + 0);
                foreach (KeyValueDataView kvExpensesTypeGroup in listKeyValueExpensesTypeGroup)
                {
                    totalByRow = decimal.Zero;

                    foreach (KeyValueDataView kvCostCenter in listKeyValueCostCenter)
                    {
                        tableCell = new TableCell();
                        tableCell.HorizontalAlign = HorizontalAlign.Right;
                        tableCell.CssClass = "GridExpenses_td_item_right";

                        var currGroupExpense = listGroupExpense.Where(w => w.ExpenseGroup.idKeyValue == kvExpensesTypeGroup.idKeyValue &&
                                                                      w.CostCenter.idKeyValue == kvCostCenter.idKeyValue).FirstOrDefault();

                        if (currGroupExpense != null)
                        {
                            currValue = currGroupExpense.ExpenseValue_MH_RoundFour;
                            totalByRow += currValue;
                            total += currValue;

                            if (dictTotalByColl.ContainsKey(kvCostCenter.KeyValueIntCode))
                            {
                                dictTotalByColl[kvCostCenter.KeyValueIntCode] += currValue; 
                            }
                            else
                            {
                                dictTotalByColl.Add(kvCostCenter.KeyValueIntCode, currValue);
                            }

                            tableCell.Text = currValue.ToStringFormatted4();
                            arrExpenses[rowIndex].Cells.Add(tableCell);
                        }
                    }

                    //tableCell = new TableCell();
                    //tableCell.HorizontalAlign = HorizontalAlign.Right;
                    //tableCell.CssClass = "MainGrid_td_item_right";
                    //tableCell.Font.Bold = true;
                    //tableCell.Text = totalByRow.ToStringFormatted();
                    //arrExpensesTypeGroupMH[rowIndex].Cells.Add(tableCell);

                    rowIndex++;
                }

                foreach (KeyValueDataView kvCostCenter in listKeyValueCostCenter)
                {
                    tableCell = new TableCell();
                    tableCell.HorizontalAlign = HorizontalAlign.Right;
                    tableCell.CssClass = "GridExpenses_td_item_right GridExpenses_td_item_total";                    
                    tableCell.Text = Math.Round(dictTotalByColl[kvCostCenter.KeyValueIntCode], 4, MidpointRounding.AwayFromZero).ToStringFormatted4();

                    arrExpenses[rowIndex].Cells.Add(tableCell);
                }

                //tableCell = new TableCell();
                //tableCell.HorizontalAlign = HorizontalAlign.Right;
                //tableCell.CssClass = "MainGrid_td_item_right";
                //tableCell.Font.Bold = true;
                //tableCell.Text = total.ToStringFormatted();
                //arrExpensesTypeGroupMH[rowIndex].Cells.Add(tableCell);

                #endregion

                result.Add("Expenses", arrExpenses);
                result.Add("ExpensesTypeGroup", arrExpensesTypeGroup);
                result.Add("Quantities", arrQuantites);
                result.Add("ExpensesTypeGroupMH", arrExpensesTypeGroupMH);
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

        public CallContext ImportSAPDataExpensesAndQuantities(string fileFullName, int idEntity, CallContext resultContext)
        {
            try
            {
                resultContext.ResultCode = ETEMEnums.ResultEnum.Error;

                FileInfo excelFile = new FileInfo(fileFullName);

                using (ExcelPackage package = new ExcelPackage(excelFile))
                {
                    int currRow = 1;
                    int currCol = 0;

                    bool res;
                    decimal resultParseDecimal;
                    int resultParseInt;

                    ExcelWorksheet workSheetExpenses = package.Workbook.Worksheets[1];
                    ExcelWorksheet workSheetQuantity = package.Workbook.Worksheets[2];

                    if (workSheetExpenses == null && workSheetQuantity == null)
                    {
                        resultContext.Message = "Error! No Excel work sheet `Expenses` and `Quantity`!";
                        return resultContext;
                    }
                    else if (workSheetExpenses == null)
                    {
                        resultContext.Message = "Error! No Excel work sheet `Expenses`!";
                        return resultContext;
                    }
                    else if (workSheetQuantity == null)
                    {
                        resultContext.Message = "Error! No Excel work sheet `Quantity`!";
                        return resultContext;
                    }

                    SAPData _SAPData = this.GetEntityById(idEntity);

                    if (_SAPData == null)
                    {
                        resultContext.Message = "Entity `SAPData` not found by ID (`" + idEntity + "`)!";
                        return resultContext;
                    }

                    List<string> listKeyTypeIntCodes = new List<string>()
                    {
                        ETEMEnums.KeyTypeEnum.CostCenter.ToString(),
                        ETEMEnums.KeyTypeEnum.ExpensesType.ToString(),
                        ETEMEnums.KeyTypeEnum.QuantityType.ToString()
                    };

                    List<KeyValueDataView> listKeyValuesToSAPDataExpensesAndQuantity = new List<KeyValueDataView>();
                    List<KeyValueDataView> listKeyValueCostCenter = new List<KeyValueDataView>();
                    List<KeyValueDataView> listKeyValueExpensesType = new List<KeyValueDataView>();
                    List<KeyValueDataView> listKeyValueQuantityType = new List<KeyValueDataView>();

                    listKeyValuesToSAPDataExpensesAndQuantity = (from kv in this.dbContext.KeyValues
                                                                 join kt in this.dbContext.KeyTypes on kv.idKeyType equals kt.idKeyType
                                                                 where listKeyTypeIntCodes.Contains(kt.KeyTypeIntCode)
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

                    listKeyValueCostCenter = listKeyValuesToSAPDataExpensesAndQuantity.Where(w => w.KeyTypeIntCode == ETEMEnums.KeyTypeEnum.CostCenter.ToString()).ToList();
                    listKeyValueExpensesType = listKeyValuesToSAPDataExpensesAndQuantity.Where(w => w.KeyTypeIntCode == ETEMEnums.KeyTypeEnum.ExpensesType.ToString()).ToList();
                    listKeyValueQuantityType = listKeyValuesToSAPDataExpensesAndQuantity.Where(w => w.KeyTypeIntCode == ETEMEnums.KeyTypeEnum.QuantityType.ToString()).ToList();

                    List<SAPDataExpense> listSAPDataExpenseOld = new List<SAPDataExpense>();
                    List<SAPDataQuantity> listSAPDataQuantityOld = new List<SAPDataQuantity>();
                    List<SAPDataExpense> listSAPDataExpenseNew = new List<SAPDataExpense>();
                    List<SAPDataQuantity> listSAPDataQuantityNew = new List<SAPDataQuantity>();

                    listSAPDataExpenseOld = (from sde in this.dbContext.SAPDataExpenses
                                             where sde.idSAPData == _SAPData.idSAPData
                                             select sde).ToList<SAPDataExpense>();

                    listSAPDataQuantityOld = (from sdq in this.dbContext.SAPDataQuantities
                                              where sdq.idSAPData == _SAPData.idSAPData
                                              select sdq).ToList<SAPDataQuantity>();

                    Dictionary<string, string> dictErrorsExpenses = new Dictionary<string, string>();
                    Dictionary<string, string> dictErrorsQuantities = new Dictionary<string, string>();

                    SAPDataExpense newSAPDataExpense = new SAPDataExpense();
                    SAPDataQuantity newSAPDataQuantity = new SAPDataQuantity();

                    bool hasNotErrorInRow = true;

                    string rangeValueStr = string.Empty;

                    ExcelRange range;

                    for (int i = 4; i < listKeyValueCostCenter.Count + 4; i++)
                    {
                        for (int j = 2; j < listKeyValueExpensesType.Count + 2; j++)
                        {
                            hasNotErrorInRow = true;

                            currRow = 1;
                            range = workSheetExpenses.Cells[currRow, i];
                            rangeValueStr = (range.Value != null ? range.Value.ToString() : string.Empty);

                            var kvCostCenter = listKeyValueCostCenter.Where(w => w.Name.Trim().ToUpper() == rangeValueStr.Trim().ToUpper()).FirstOrDefault();

                            newSAPDataExpense = new SAPDataExpense();

                            newSAPDataExpense.idSAPData = idEntity;
                            if (kvCostCenter != null)
                            {
                                newSAPDataExpense.idCostCenter = kvCostCenter.idKeyValue;
                            }
                            else
                            {
                                hasNotErrorInRow = false;
                                if (dictErrorsExpenses.ContainsKey("CostCenterExpenses"))
                                {
                                    dictErrorsExpenses["CostCenterExpenses"] += "," + i;
                                }
                                else
                                {
                                    dictErrorsExpenses.Add("CostCenterExpenses", i.ToString());
                                }
                            }

                            currCol = 2;
                            range = workSheetExpenses.Cells[j, currCol];
                            rangeValueStr = (range.Value != null ? range.Value.ToString() : string.Empty);

                            var kvExpensesType = listKeyValueExpensesType.Where(w => w.KeyValueIntCode.Trim().ToUpper() == rangeValueStr.Trim().ToUpper()).FirstOrDefault();
                                                        
                            if (kvExpensesType != null)
                            {
                                newSAPDataExpense.idExpensesType = kvExpensesType.idKeyValue;
                            }
                            else
                            {
                                hasNotErrorInRow = false;
                                if (dictErrorsExpenses.ContainsKey("ExpensesType"))
                                {
                                    dictErrorsExpenses["ExpensesType"] += "," + j;
                                }
                                else
                                {
                                    dictErrorsExpenses.Add("ExpensesType", j.ToString());
                                }
                            }

                            range = workSheetExpenses.Cells[j, i];
                            rangeValueStr = (range.Value != null ? range.Value.ToString() : string.Empty);

                            res = Decimal.TryParse(rangeValueStr, NumberStyles.Any, BaseHelper.GetNumberFormatInfo("", ".", 4), out resultParseDecimal);
                            if (res)
                            {
                                newSAPDataExpense.ValueData = resultParseDecimal;
                            }
                            else
                            {
                                newSAPDataExpense.ValueData = decimal.Zero;
                            }

                            var checkSAPDataExpenses = listSAPDataExpenseOld.Where(w => w.idCostCenter == newSAPDataExpense.idCostCenter &&
                                                                                    w.idExpensesType == newSAPDataExpense.idExpensesType).ToList();

                            if (checkSAPDataExpenses.Count > 0)
                            {
                                hasNotErrorInRow = false;
                                if (dictErrorsExpenses.ContainsKey("DuplicateOldExpenses"))
                                {
                                    dictErrorsExpenses["DuplicateOldExpenses"] += "," + j + "-" + i;
                                }
                                else
                                {
                                    dictErrorsExpenses.Add("DuplicateOldExpenses", j + "-" + i);
                                }
                            }

                            if (hasNotErrorInRow)
                            {
                                listSAPDataExpenseNew.Add(newSAPDataExpense);
//                                this.dbContext.SAPDataExpenses.AddObject(newSAPDataExpense);
                            }
                        }

                        for (int j = 2; j < listKeyValueQuantityType.Count + 2; j++)
                        {
                            hasNotErrorInRow = true;

                            currRow = 1;
                            range = workSheetQuantity.Cells[currRow, i];
                            rangeValueStr = (range.Value != null ? range.Value.ToString() : string.Empty);

                            var kvCostCenter = listKeyValueCostCenter.Where(w => w.Name.Trim().ToUpper() == rangeValueStr.Trim().ToUpper()).FirstOrDefault();

                            newSAPDataQuantity = new SAPDataQuantity();

                            newSAPDataQuantity.idSAPData = idEntity;
                            if (kvCostCenter != null)
                            {
                                newSAPDataQuantity.idCostCenter = kvCostCenter.idKeyValue;
                            }
                            else
                            {
                                hasNotErrorInRow = false;
                                if (dictErrorsQuantities.ContainsKey("CostCenterQuantity"))
                                {
                                    dictErrorsQuantities["CostCenterQuantity"] += "," + i;
                                }
                                else
                                {
                                    dictErrorsQuantities.Add("CostCenterQuantity", i.ToString());
                                }
                            }

                            currCol = 2;
                            range = workSheetQuantity.Cells[j, currCol];
                            rangeValueStr = (range.Value != null ? range.Value.ToString() : string.Empty);

                            var kvQuantityType = listKeyValueQuantityType.Where(w => w.KeyValueIntCode.Trim().ToUpper() == rangeValueStr.Trim().ToUpper()).FirstOrDefault();

                            if (kvQuantityType != null)
                            {
                                newSAPDataQuantity.idQuantityType = kvQuantityType.idKeyValue;
                            }
                            else
                            {
                                hasNotErrorInRow = false;
                                if (dictErrorsQuantities.ContainsKey("QuantityType"))
                                {
                                    dictErrorsQuantities["QuantityType"] += "," + j;
                                }
                                else
                                {
                                    dictErrorsQuantities.Add("QuantityType", j.ToString());
                                }
                            }

                            range = workSheetQuantity.Cells[j, i];
                            rangeValueStr = (range.Value != null ? range.Value.ToString() : string.Empty);

                            res = Decimal.TryParse(rangeValueStr, NumberStyles.Any, BaseHelper.GetNumberFormatInfo("", ".", 4), out resultParseDecimal);
                            if (res)
                            {
                                newSAPDataQuantity.ValueData = resultParseDecimal;
                            }
                            else
                            {
                                newSAPDataQuantity.ValueData = decimal.Zero;
                            }

                            var checkSAPDataQuantity = listSAPDataQuantityOld.Where(w => w.idCostCenter == newSAPDataQuantity.idCostCenter &&
                                                                                    w.idQuantityType == newSAPDataQuantity.idQuantityType).ToList();

                            if (checkSAPDataQuantity.Count > 0)
                            {
                                hasNotErrorInRow = false;
                                if (dictErrorsQuantities.ContainsKey("DuplicateOldQuantity"))
                                {
                                    dictErrorsQuantities["DuplicateOldQuantity"] += "," + j + "-" + i;
                                }
                                else
                                {
                                    dictErrorsQuantities.Add("DuplicateOldQuantity", j + "-" + i);
                                }
                            }

                            if (hasNotErrorInRow)
                            {
                                listSAPDataQuantityNew.Add(newSAPDataQuantity);
//                                this.dbContext.SAPDataQuantities.AddObject(newSAPDataQuantity);
                            }
                        }
                    }

                    if (dictErrorsExpenses.Count == 0 && dictErrorsQuantities.Count == 0)
                    {
                        resultContext = new SAPDataExpensesBL().EntitySave<SAPDataExpense>(listSAPDataExpenseNew, resultContext);
                        if (resultContext.ResultCode == ETEMEnums.ResultEnum.Success)
                        {
                            resultContext = new SAPDataQuantityBL().EntitySave<SAPDataQuantity>(listSAPDataQuantityNew, resultContext);
                            if (resultContext.ResultCode == ETEMEnums.ResultEnum.Success)
                            {
                                resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
                                resultContext.Message = "The SAP data expenses and quantities by cost centers have been imported successfully!";
                            }
                            else
                            {
                                resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                                resultContext.Message = "Error import the SAP data expenses and quantities by cost centers!";
                            }
                        }
                        else
                        {
                            resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                            resultContext.Message = "Error import the SAP data expenses and quantities by cost centers!";
                        }
                    }
                    else                    
                    {
                        List<string> listErrors = new List<string>();

                        if (dictErrorsExpenses.ContainsKey("CostCenterExpenses"))
                        {
                            listErrors.Add("Error! The field `Cost Center` for expenses is missing or in wrong format, Cols (" + dictErrorsExpenses["CostCenterExpenses"] + ")!");
                        }
                        if (dictErrorsExpenses.ContainsKey("ExpensesType"))
                        {
                            listErrors.Add("Error! The field `Expenses Type` is missing or in wrong format, Rows (" + dictErrorsExpenses["ExpensesType"] + ")!");
                        }
                        if (dictErrorsExpenses.ContainsKey("DuplicateOldExpenses"))
                        {
                            listErrors.Add("Error! The selected file includes expenses with duplicate data in the database, Rows-Cols (" + dictErrorsExpenses["DuplicateOldExpenses"] + ")!");
                        }
                        if (dictErrorsQuantities.ContainsKey("CostCenterQuantity"))
                        {
                            listErrors.Add("Error! The field `Cost Center` for quantities is missing or in wrong format, Cols (" + dictErrorsQuantities["CostCenterQuantity"] + ")!");
                        }
                        if (dictErrorsQuantities.ContainsKey("QuantityType"))
                        {
                            listErrors.Add("Error! The field `Quantity Type` is missing or in wrong format, Rows (" + dictErrorsQuantities["QuantityType"] + ")!");
                        }
                        if (dictErrorsQuantities.ContainsKey("DuplicateOldQuantity"))
                        {
                            listErrors.Add("Error! The selected file includes quantities with duplicate data in the database, Rows-Cols (" + dictErrorsQuantities["DuplicateOldQuantity"] + ")!");
                        }

                        resultContext.Message = string.Join(Constants.ERROR_MESSAGES_SEPARATOR, listErrors);
                    }
                }
            }
            catch (Exception ex)
            {
                resultContext.Message = "Error import the SAP data expenses and quantities by cost centers!";

                BaseHelper.Log("Error import entities `SAPDataExpenses`, `SAPDataQuantity`!");
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

            return resultContext;
        }
    }
}