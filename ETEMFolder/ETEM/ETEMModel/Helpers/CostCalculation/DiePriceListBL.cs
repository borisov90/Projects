using ETEMModel.Helpers.AbstractSearchBLHolder;
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

namespace ETEMModel.Helpers.CostCalculation
{
    public class DiePriceListBL : BaseClassBL<DiePriceList>
    {
        public DiePriceListBL()
        {
            this.EntitySetName = "DiePriceLists";
        }

        internal override void EntityToEntity(DiePriceList sourceEntity, DiePriceList targetEntity)
        {
            targetEntity.idVendor = sourceEntity.idVendor;
            targetEntity.DateFrom = sourceEntity.DateFrom;
            targetEntity.DateTo = sourceEntity.DateTo;
        }

        internal override DiePriceList GetEntityById(int idEntity)
        {
            return this.dbContext.DiePriceLists.Where(w => w.idDiePriceList == idEntity).FirstOrDefault();
        }

        internal List<DiePriceListDataView> GetAllDiePriceList(ICollection<AbstractSearch> searchCriteria,
                                                               DateTime? dateActiveTo,
                                                               string sortExpression, string sortDirection)
        {
            List<DiePriceListDataView> listView = new List<DiePriceListDataView>();

            listView = (from dpl in this.dbContext.DiePriceLists
                        join kvV in this.dbContext.KeyValues on dpl.idVendor equals kvV.idKeyValue
                        where (dateActiveTo.HasValue ?
                               ((!dpl.DateTo.HasValue && dpl.DateFrom <= dateActiveTo.Value) ||
                                (dpl.DateTo.HasValue && dpl.DateFrom <= dateActiveTo.Value && dpl.DateTo >= dateActiveTo.Value)) :
                              1 == 1)
                        orderby kvV.Name ascending, dpl.DateFrom ascending
                        select new DiePriceListDataView
                        {
                            idDiePriceList = dpl.idDiePriceList,
                            VendorName = kvV.Name,
                            idVendor = dpl.idVendor,
                            DateFrom = dpl.DateFrom,
                            DateTo = dpl.DateTo
                        }).ApplySearchCriterias(searchCriteria).ToList<DiePriceListDataView>();

            if (string.IsNullOrEmpty(sortExpression) || sortExpression == Constants.INVALID_ID_STRING)
            {
                sortDirection = string.Empty;

                sortExpression = "VendorName, DateFrom";
            }

            listView = OrderByHelper.OrderBy<DiePriceListDataView>(listView, sortExpression, sortDirection).ToList<DiePriceListDataView>();
            
            return listView;
        }

        public DiePriceList GetDiePriceListByVendorAndDateActiveTo(int idVendor, DateTime dateActiveTo)
        {
            DiePriceList result = new DiePriceList();

            result = this.dbContext.DiePriceLists.Where(w => w.idVendor == idVendor &&
                                                        ((w.DateFrom <= dateActiveTo && !w.DateTo.HasValue) ||
                                                         (w.DateTo.HasValue &&
                                                          w.DateFrom <= dateActiveTo &&
                                                          w.DateTo >= dateActiveTo))).FirstOrDefault();

            return result;
        }

        internal CallContext DiePriceListSave(List<DiePriceList> entities, CallContext resultContext)
        {
            if (entities.Count > 0)
            {
                List<DiePriceList> listDiePriceListsActiveNotClosed = new List<DiePriceList>();

                var listNewDiePriceLists = entities.Where(w => w.idDiePriceList == Constants.INVALID_ID ||
                                                          w.idDiePriceList == Constants.INVALID_ID_ZERO).ToList();

                if (listNewDiePriceLists.Count > 0)
                {
                    var listVendorIDs = listNewDiePriceLists.Select(s => s.idVendor).ToList();

                    listDiePriceListsActiveNotClosed = (from dpl in this.dbContext.DiePriceLists
                                                        where listVendorIDs.Contains(dpl.idVendor) &&
                                                              !dpl.DateTo.HasValue
                                                        select dpl).ToList<DiePriceList>();
                }

                if (listDiePriceListsActiveNotClosed.Count > 0)
                {
                    var minDateFrom = listNewDiePriceLists.Min(m => m.DateFrom);

                    if (minDateFrom != DateTime.MinValue)
                    {
                        foreach (DiePriceList diePriceList in listDiePriceListsActiveNotClosed)
                        {
                            diePriceList.DateTo = minDateFrom.AddDays(-1);
                        }

                        resultContext = base.EntitySave<DiePriceList>(listDiePriceListsActiveNotClosed, resultContext);
                    }                    
                }

                if (resultContext.ResultCode == ETEMEnums.ResultEnum.Success)
                {
                    resultContext = base.EntitySave<DiePriceList>(entities, resultContext);
                }
            }

            return resultContext;
        }

        public CallContext DiePriceListDelete(List<int> listSelectedIDs, CallContext resultContext)
        {
            try
            {
                resultContext.ResultCode = ETEMEnums.ResultEnum.Error;

                List<DiePriceList> listDiePriceLists = new List<DiePriceList>();
                List<DiePriceListDetail> listDiePriceListDetails = new List<DiePriceListDetail>();
               
                listDiePriceLists = (from dpl in this.dbContext.DiePriceLists
                                     where listSelectedIDs.Contains(dpl.idDiePriceList)
                                     select dpl).ToList();

                listDiePriceListDetails = (from dpld in this.dbContext.DiePriceListDetails
                                           where listSelectedIDs.Contains(dpld.idDiePriceList)
                                           select dpld).ToList();

                CallContext deleteContext = new CallContext();
                deleteContext = resultContext;

                deleteContext = new DiePriceListDetailBL().EntityDelete<DiePriceListDetail>(listDiePriceListDetails, deleteContext);

                if (deleteContext.ResultCode == ETEMEnums.ResultEnum.Success)
                {
                    deleteContext = base.EntityDelete<DiePriceList>(listDiePriceLists, deleteContext);

                    if (deleteContext.ResultCode == ETEMEnums.ResultEnum.Success)
                    {
                        resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
                        resultContext.Message = "Selected rows `Die Price Lists by Vendors` and their details have been deleted successfully!";
                    }
                    else
                    {
                        resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                        resultContext.Message = "Error delete selected rows `Die Price Lists by Vendors` and their details!";
                    }
                }
                else
                {
                    resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                    resultContext.Message = "Error delete selected rows `Die Price Lists by Dimensions`!";
                }
            }
            catch (Exception ex)
            {
                resultContext.Message = "Error delete selected rows `Die Price Lists by Vendors` and their details!";

                BaseHelper.Log("Error delete entities `DiePriceList`, IDs - (" + string.Join(",", listSelectedIDs.ToArray()) + ")!");
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

            return resultContext;
        }

        public CallContext ImportDiePriceListDetails(string fileFullName, int idEntity, CallContext resultContext)
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

                    ExcelWorksheet workSheet = package.Workbook.Worksheets.FirstOrDefault();

                    if (workSheet == null)
                    {
                        resultContext.Message = "Error! No Excel work sheet!";
                        return resultContext;
                    }

                    DiePriceList diePriceList = this.GetEntityById(idEntity);

                    if (diePriceList == null)
                    {
                        resultContext.Message = "Entity `DiePriceList` not found by ID (" + idEntity + ")!";
                        return resultContext;
                    }

                    List<string> listKeyTypeIntCodes = new List<string>()
                    {
                        ETEMEnums.KeyTypeEnum.NumberOfCavities.ToString(),
                        ETEMEnums.KeyTypeEnum.ProfileCategory.ToString(),
                        ETEMEnums.KeyTypeEnum.ProfileComplexity.ToString()
                    };

                    List<KeyValueDataView> listKeyValuesToDiePriceListDetail = new List<KeyValueDataView>();
                    List<KeyValueDataView> listKeyValueNumberOfCavities = new List<KeyValueDataView>();
                    List<KeyValueDataView> listKeyValueProfileCategory = new List<KeyValueDataView>();
                    List<KeyValueDataView> listKeyValueProfileComplexity = new List<KeyValueDataView>();

                    listKeyValuesToDiePriceListDetail = (from kv in this.dbContext.KeyValues
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

                    listKeyValueNumberOfCavities = listKeyValuesToDiePriceListDetail.Where(w => w.KeyTypeIntCode == ETEMEnums.KeyTypeEnum.NumberOfCavities.ToString()).ToList();
                    listKeyValueProfileCategory = listKeyValuesToDiePriceListDetail.Where(w => w.KeyTypeIntCode == ETEMEnums.KeyTypeEnum.ProfileCategory.ToString()).ToList();
                    listKeyValueProfileComplexity = listKeyValuesToDiePriceListDetail.Where(w => w.KeyTypeIntCode == ETEMEnums.KeyTypeEnum.ProfileComplexity.ToString()).ToList();

                    List<DiePriceListDetail> listDiePriceListDetailsOld = new List<DiePriceListDetail>();
                    List<DiePriceListDetail> listDiePriceListDetailsNew = new List<DiePriceListDetail>();

                    listDiePriceListDetailsOld = (from dpld in this.dbContext.DiePriceListDetails
                                                  where dpld.idDiePriceList == diePriceList.idDiePriceList
                                                  select dpld).ToList<DiePriceListDetail>();

                    Dictionary<string, string> dictErrorsRows = new Dictionary<string, string>();

                    DiePriceListDetail newDiePriceListDetail = new DiePriceListDetail();

                    bool hasNotErrorInRow = true;

                    string rangeValueStr = string.Empty;

                    ExcelRange range;

                    for (; ;)
                    {
                        currRow++;

                        hasNotErrorInRow = true;
   
                        currCol = 1;
                        range = workSheet.Cells[currRow, currCol];
                        if (string.IsNullOrWhiteSpace(workSheet.Cells[currRow, 1].Text) &&
                            string.IsNullOrWhiteSpace(workSheet.Cells[currRow, 2].Text) &&
                            string.IsNullOrWhiteSpace(workSheet.Cells[currRow, 3].Text) &&
                            string.IsNullOrWhiteSpace(workSheet.Cells[currRow, 4].Text) &&
                            string.IsNullOrWhiteSpace(workSheet.Cells[currRow, 5].Text) &&
                            string.IsNullOrWhiteSpace(workSheet.Cells[currRow, 6].Text))
                        {
                            break;
                        }

                        newDiePriceListDetail = new DiePriceListDetail();

                        newDiePriceListDetail.idDiePriceList = idEntity;

                        rangeValueStr = (range.Value != null ? range.Value.ToString() : string.Empty);

                        var numberOfCavities = listKeyValueNumberOfCavities.Where(w => w.DefaultValue1.Trim().ToUpper() == rangeValueStr.Trim().ToUpper()).FirstOrDefault();

                        if (numberOfCavities != null)
                        {
                            newDiePriceListDetail.idNumberOfCavities = numberOfCavities.idKeyValue;
                        }
                        else
                        {
                            hasNotErrorInRow = false;
                            if (dictErrorsRows.ContainsKey("NumberOfCavities"))
                            {
                                dictErrorsRows["NumberOfCavities"] += "," + currRow;
                            }
                            else
                            {
                                dictErrorsRows.Add("NumberOfCavities", currRow.ToString());
                            }
                        }

                        currCol++;
                        range = workSheet.Cells[currRow, currCol];

                        rangeValueStr = (range.Value != null ? range.Value.ToString() : string.Empty);

                        string[] arrRangeValue = rangeValueStr.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);

                        string profileComplexityVal = (arrRangeValue.Length > 0 ? arrRangeValue[0] : string.Empty);
                        string profileCategoryVal = (arrRangeValue.Length > 1 ? arrRangeValue[1] : string.Empty);

                        var profileComplexity = listKeyValueProfileComplexity.Where(w => w.Name.Trim().ToUpper() == profileComplexityVal.Trim().ToUpper()).FirstOrDefault();

                        if (profileComplexity != null)
                        {
                            newDiePriceListDetail.idProfileComplexity = profileComplexity.idKeyValue;
                        }
                        else
                        {
                            hasNotErrorInRow = false;
                            if (dictErrorsRows.ContainsKey("ProfileComplexity"))
                            {
                                dictErrorsRows["ProfileComplexity"] += "," + currRow;
                            }
                            else
                            {
                                dictErrorsRows.Add("ProfileComplexity", currRow.ToString());
                            }
                        }

                        var profileCategory = listKeyValueProfileCategory.Where(w => w.Name.Trim().ToUpper() == profileCategoryVal.Trim().ToUpper()).FirstOrDefault();

                        if (profileCategory != null)
                        {
                            newDiePriceListDetail.idProfileCategory = profileCategory.idKeyValue;
                        }
                        else
                        {
                            hasNotErrorInRow = false;
                            if (dictErrorsRows.ContainsKey("ProfileCategory"))
                            {
                                dictErrorsRows["ProfileCategory"] += "," + currRow;
                            }
                            else
                            {
                                dictErrorsRows.Add("ProfileCategory", currRow.ToString());
                            }
                        }

                        currCol++;

                        currCol++;
                        range = workSheet.Cells[currRow, currCol];

                        rangeValueStr = (range.Value != null ? range.Value.ToString() : string.Empty);

                        res = Decimal.TryParse(rangeValueStr, NumberStyles.Any, BaseHelper.GetNumberFormatInfo("", ".", 2), out resultParseDecimal);
                        if (res)
                        {
                            newDiePriceListDetail.Price = resultParseDecimal;
                        }
                        else
                        {
                            hasNotErrorInRow = false;
                            newDiePriceListDetail.Price = decimal.MinValue;

                            if (dictErrorsRows.ContainsKey("Price"))
                            {
                                dictErrorsRows["Price"] += "," + currRow;
                            }
                            else
                            {
                                dictErrorsRows.Add("Price", currRow.ToString());
                            }                                            
                        }

                        currCol++;
                        range = workSheet.Cells[currRow, currCol];

                        rangeValueStr = (range.Value != null ? range.Value.ToString() : string.Empty);

                        res = Int32.TryParse(rangeValueStr, NumberStyles.Any, BaseHelper.GetNumberFormatInfo("", ".", 0), out resultParseInt);
                        if (res)
                        {
                            newDiePriceListDetail.DimensionA = resultParseInt;
                        }
                        else
                        {
                            hasNotErrorInRow = false;
                            newDiePriceListDetail.DimensionA = int.MinValue;
                           
                            if (dictErrorsRows.ContainsKey("DimensionA"))
                            {
                                dictErrorsRows["DimensionA"] += "," + currRow;
                            }
                            else
                            {
                                dictErrorsRows.Add("DimensionA", currRow.ToString());
                            }
                        }

                        currCol++;
                        range = workSheet.Cells[currRow, currCol];

                        rangeValueStr = (range.Value != null ? range.Value.ToString() : string.Empty);

                        res = Int32.TryParse(rangeValueStr, NumberStyles.Any, BaseHelper.GetNumberFormatInfo("", ".", 0), out resultParseInt);
                        if (res)
                        {
                            newDiePriceListDetail.DimensionB = resultParseInt;
                        }
                        else
                        {
                            hasNotErrorInRow = false;
                            newDiePriceListDetail.DimensionB = int.MinValue;
                            
                            if (dictErrorsRows.ContainsKey("DimensionB"))
                            {
                                dictErrorsRows["DimensionB"] += "," + currRow;
                            }
                            else
                            {
                                dictErrorsRows.Add("DimensionB", currRow.ToString());
                            }
                        }

                        newDiePriceListDetail.Lifespan = decimal.Zero;

                        var checkDiePriceListDetail = listDiePriceListDetailsOld.Where(w => w.idNumberOfCavities == newDiePriceListDetail.idNumberOfCavities &&
                                                                                       w.idProfileComplexity == newDiePriceListDetail.idProfileComplexity &&
                                                                                       w.idProfileCategory == newDiePriceListDetail.idProfileCategory &&
                                                                                       w.Price == newDiePriceListDetail.Price &&
                                                                                       w.DimensionA == newDiePriceListDetail.DimensionA &&
                                                                                       w.DimensionB == newDiePriceListDetail.DimensionB).ToList();

                        if (checkDiePriceListDetail.Count > 0)
                        {
                            hasNotErrorInRow = false;
                            if (dictErrorsRows.ContainsKey("DuplicateOld"))
                            {
                                dictErrorsRows["DuplicateOld"] += "," + currRow;
                            }
                            else
                            {
                                dictErrorsRows.Add("DuplicateOld", currRow.ToString());
                            }
                        }

                        checkDiePriceListDetail = listDiePriceListDetailsNew.Where(w => w.idNumberOfCavities == newDiePriceListDetail.idNumberOfCavities &&
                                                                                   w.idProfileComplexity == newDiePriceListDetail.idProfileComplexity &&
                                                                                   w.idProfileCategory == newDiePriceListDetail.idProfileCategory &&
                                                                                   w.Price == newDiePriceListDetail.Price &&
                                                                                   w.DimensionA == newDiePriceListDetail.DimensionA &&
                                                                                   w.DimensionB == newDiePriceListDetail.DimensionB).ToList();

                        if (checkDiePriceListDetail.Count > 0)
                        {
                            hasNotErrorInRow = false;
                            if (dictErrorsRows.ContainsKey("DuplicateNew"))
                            {
                                dictErrorsRows["DuplicateNew"] += "," + currRow;
                            }
                            else
                            {
                                dictErrorsRows.Add("DuplicateNew", currRow.ToString());
                            }
                        }

                        if (hasNotErrorInRow)
                        {
                            listDiePriceListDetailsNew.Add(newDiePriceListDetail);
//                            this.dbContext.DiePriceListDetails.AddObject(newDiePriceListDetail);
                        }
                    }

                    if (dictErrorsRows.Count == 0)
                    {
                        resultContext = new DiePriceListDetailBL().EntitySave<DiePriceListDetail>(listDiePriceListDetailsNew, resultContext);

                        if (resultContext.ResultCode == ETEMEnums.ResultEnum.Success)
                        {
                            resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
                            resultContext.Message = "The details for current `Die Price List by Vendor` have been imported successfully!";
                        }
                        else
                        {
                            resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                            resultContext.Message = "Error import details for current `Die Price List by Vendor`!";
                        }
                    }
                    else                    
                    {
                        List<string> listErrors = new List<string>();

                        if (dictErrorsRows.ContainsKey("NumberOfCavities"))
                        {
                            listErrors.Add("Error! The field `cavities` is missing or in wrong format, Rows (" + dictErrorsRows["NumberOfCavities"] + ")!");
                        }
                        if (dictErrorsRows.ContainsKey("ProfileComplexity"))
                        {
                            listErrors.Add("Error! The field `complexity` is missing or in wrong format, Rows (" + dictErrorsRows["ProfileComplexity"] + ")!");
                        }
                        if (dictErrorsRows.ContainsKey("ProfileCategory"))
                        {
                            listErrors.Add("Error! The field `category` is missing or in wrong format, Rows (" + dictErrorsRows["ProfileCategory"] + ")!");
                        }
                        if (dictErrorsRows.ContainsKey("Price"))
                        {
                            listErrors.Add("Error! The field `price` is missing or in wrong NUMBER format, Rows (" + dictErrorsRows["Price"] + ")!");
                        }
                        if (dictErrorsRows.ContainsKey("DimensionA"))
                        {
                            listErrors.Add("Error! The field `dimensiona` is missing or in wrong INTEGER NUMBER format, Rows (" + dictErrorsRows["DimensionA"] + ")!");
                        }
                        if (dictErrorsRows.ContainsKey("DimensionB"))
                        {
                            listErrors.Add("Error! The field `dimensionb` is missing or in wrong INTEGER NUMBER format, Rows (" + dictErrorsRows["DimensionB"] + ")!");
                        }
                        if (dictErrorsRows.ContainsKey("DuplicateNew"))
                        {
                            listErrors.Add("Error! The selected file includes die price list details with duplicate values, Rows (" + dictErrorsRows["DuplicateNew"] + ")!");
                        }
                        if (dictErrorsRows.ContainsKey("DuplicateOld"))
                        {
                            listErrors.Add("Error! The selected file includes die price list details with duplicate values in the database, Rows (" + dictErrorsRows["DuplicateOld"] + ")!");
                        }

                        resultContext.Message = string.Join(Constants.ERROR_MESSAGES_SEPARATOR, listErrors);
                    }
                }
            }
            catch (Exception ex)
            {
                resultContext.Message = "Error import details for current `Die Price List by Vendor`!";

                BaseHelper.Log("Error import entities `DiePriceListDetail`!");
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

            return resultContext;
        }
    }
}