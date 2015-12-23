using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Models;
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
    public class ProductivityAndScrapBL : BaseClassBL<ProductivityAndScrap>
    {
        public ProductivityAndScrapBL()
        {
            this.EntitySetName = "ProductivityAndScraps";
        }

        internal override void EntityToEntity(ProductivityAndScrap sourceEntity, ProductivityAndScrap targetEntity)
        {            
            targetEntity.DateFrom = sourceEntity.DateFrom;
            targetEntity.DateTo = sourceEntity.DateTo;
        }

        internal override ProductivityAndScrap GetEntityById(int idEntity)
        {
            return this.dbContext.ProductivityAndScraps.Where(w => w.idProductivityAndScrap == idEntity).FirstOrDefault();
        }

        public ProductivityAndScrap GetProductivityAndScrapByDateActiveTo(DateTime dateActiveTo)
        {
            ProductivityAndScrap result = new ProductivityAndScrap();

            result = this.dbContext.ProductivityAndScraps.Where(w => (w.DateFrom <= dateActiveTo && !w.DateTo.HasValue) ||
                                                                (w.DateTo.HasValue &&
                                                                 w.DateFrom <= dateActiveTo &&
                                                                 w.DateTo >= dateActiveTo)).FirstOrDefault();

            return result;
        }

        public ProductivityAndScrapDataView GetProductivityAndScrapByDateActiveToWithAvgData(DateTime dateActiveTo)
        {
            ProductivityAndScrapDataView result = null;

            var list = (from ps in this.dbContext.ProductivityAndScraps
                        join psd in this.dbContext.ProductivityAndScrapDetails on ps.idProductivityAndScrap equals psd.idProductivityAndScrap
                        join kvPress in this.dbContext.KeyValues on psd.idCostCenter equals kvPress.idKeyValue
                        where (ps.DateFrom <= dateActiveTo && !ps.DateTo.HasValue) ||
                              (ps.DateTo.HasValue && ps.DateFrom <= dateActiveTo && ps.DateTo >= dateActiveTo)
                        select new ProductivityAndScrapDataView
                        {
                            idProductivityAndScrap = ps.idProductivityAndScrap,
                            DateFrom = ps.DateFrom,
                            DateTo = ps.DateTo,
                            ProductivityKGh = psd.ProductivityKGh,
                            ScrapRate = psd.ScrapRate,
                            KeyValueIntCodeForPress = kvPress.KeyValueIntCode
                        }).ToList();

            if (list.Count > 0)
            {
                result = (from ps in list
                          group ps by new { ps.idProductivityAndScrap, ps.DateFrom, ps.DateTo } into grPs
                          select new ProductivityAndScrapDataView
                          {
                              idProductivityAndScrap = grPs.Key.idProductivityAndScrap,
                              DateFrom = grPs.Key.DateFrom,
                              DateTo = grPs.Key.DateTo,
                              AvgProductivityKGhForPressSMS1 = grPs.Where(w => w.ProductivityKGh.HasValue && w.KeyValueIntCodeForPress == ETEMEnums.CostCenterEnum.SMS1.ToString()).Count() > 0 ?
                                                               grPs.Where(w => w.ProductivityKGh.HasValue && w.KeyValueIntCodeForPress == ETEMEnums.CostCenterEnum.SMS1.ToString()).Average(a => a.ProductivityKGh.Value) : decimal.Zero,
                              AvgScrapRateForPressSMS1 = grPs.Where(w => w.ScrapRate.HasValue && w.KeyValueIntCodeForPress == ETEMEnums.CostCenterEnum.SMS1.ToString()).Count() > 0 ?
                                                         grPs.Where(w => w.ScrapRate.HasValue && w.KeyValueIntCodeForPress == ETEMEnums.CostCenterEnum.SMS1.ToString()).Average(a => a.ScrapRate.Value) * 100 : decimal.Zero,
                              AvgProductivityKGhForPressSMS2 = grPs.Where(w => w.ProductivityKGh.HasValue && w.KeyValueIntCodeForPress == ETEMEnums.CostCenterEnum.SMS2.ToString()).Count() > 0 ?
                                                               grPs.Where(w => w.ProductivityKGh.HasValue && w.KeyValueIntCodeForPress == ETEMEnums.CostCenterEnum.SMS2.ToString()).Average(a => a.ProductivityKGh.Value) : decimal.Zero,
                              AvgScrapRateForPressSMS2 = grPs.Where(w => w.ScrapRate.HasValue && w.KeyValueIntCodeForPress == ETEMEnums.CostCenterEnum.SMS2.ToString()).Count() > 0 ?
                                                         grPs.Where(w => w.ScrapRate.HasValue && w.KeyValueIntCodeForPress == ETEMEnums.CostCenterEnum.SMS2.ToString()).Average(a => a.ScrapRate.Value) * 100 : decimal.Zero,
                              AvgProductivityKGhForPressBREDA = grPs.Where(w => w.ProductivityKGh.HasValue && w.KeyValueIntCodeForPress == ETEMEnums.CostCenterEnum.Breda.ToString()).Count() > 0 ?
                                                                grPs.Where(w => w.ProductivityKGh.HasValue && w.KeyValueIntCodeForPress == ETEMEnums.CostCenterEnum.Breda.ToString()).Average(a => a.ProductivityKGh.Value) : decimal.Zero,
                              AvgScrapRateForPressBREDA = grPs.Where(w => w.ScrapRate.HasValue && w.KeyValueIntCodeForPress == ETEMEnums.CostCenterEnum.Breda.ToString()).Count() > 0 ?
                                                          grPs.Where(w => w.ScrapRate.HasValue && w.KeyValueIntCodeForPress == ETEMEnums.CostCenterEnum.Breda.ToString()).Average(a => a.ScrapRate.Value) * 100 : decimal.Zero,
                              AvgProductivityKGhForPressFARREL = grPs.Where(w => w.ProductivityKGh.HasValue && w.KeyValueIntCodeForPress == ETEMEnums.CostCenterEnum.Farrel.ToString()).Count() > 0 ?
                                                                 grPs.Where(w => w.ProductivityKGh.HasValue && w.KeyValueIntCodeForPress == ETEMEnums.CostCenterEnum.Farrel.ToString()).Average(a => a.ProductivityKGh.Value) : decimal.Zero,
                              AvgScrapRateForPressFARREL = grPs.Where(w => w.ScrapRate.HasValue && w.KeyValueIntCodeForPress == ETEMEnums.CostCenterEnum.Farrel.ToString()).Count() > 0 ?
                                                           grPs.Where(w => w.ScrapRate.HasValue && w.KeyValueIntCodeForPress == ETEMEnums.CostCenterEnum.Farrel.ToString()).Average(a => a.ScrapRate.Value) * 100 : decimal.Zero,
                          }).FirstOrDefault();
            }

            return result;
        }

        internal List<ProductivityAndScrapDataView> GetAllProductivityAndScrapList(ICollection<AbstractSearch> searchCriteria,
                                                                                   DateTime? dateActiveTo,
                                                                                   string sortExpression, string sortDirection)
        {
            List<ProductivityAndScrapDataView> listView = new List<ProductivityAndScrapDataView>();

            listView = (from ps in this.dbContext.ProductivityAndScraps
                        orderby ps.DateFrom ascending
                        where (dateActiveTo.HasValue ?
                               ((!ps.DateTo.HasValue && ps.DateFrom <= dateActiveTo.Value) ||
                                (ps.DateTo.HasValue && ps.DateFrom <= dateActiveTo.Value && ps.DateTo >= dateActiveTo.Value)) :
                              1 == 1)
                        select new ProductivityAndScrapDataView
                        {
                            idProductivityAndScrap = ps.idProductivityAndScrap,                            
                            DateFrom = ps.DateFrom,
                            DateTo = ps.DateTo
                        }).ApplySearchCriterias(searchCriteria).ToList();

            if (string.IsNullOrEmpty(sortExpression) || sortExpression == Constants.INVALID_ID_STRING)
            {
                sortDirection = string.Empty;

                sortExpression = "DateFrom";
            }

            listView = OrderByHelper.OrderBy<ProductivityAndScrapDataView>(listView, sortExpression, sortDirection).ToList<ProductivityAndScrapDataView>();

            return listView;
        }

        internal CallContext ProductivityAndScrapSave(List<ProductivityAndScrap> entities, CallContext resultContext)
        {
            if (entities.Count > 0)
            {
                List<ProductivityAndScrap> listProductivityAndScrapsActiveNotClosed = new List<ProductivityAndScrap>();

                var listNewProductivityAndScraps = entities.Where(w => w.idProductivityAndScrap == Constants.INVALID_ID ||
                                                                  w.idProductivityAndScrap == Constants.INVALID_ID_ZERO).ToList<ProductivityAndScrap>();

                if (listNewProductivityAndScraps.Count > 0)
                {
                    listProductivityAndScrapsActiveNotClosed = (from ps in this.dbContext.ProductivityAndScraps
                                                                where !ps.DateTo.HasValue
                                                                select ps).ToList<ProductivityAndScrap>();
                }

                if (listProductivityAndScrapsActiveNotClosed.Count > 0)
                {
                    var minDateFrom = listNewProductivityAndScraps.Min(m => m.DateFrom);

                    if (minDateFrom != DateTime.MinValue)
                    {
                        foreach (ProductivityAndScrap productivityAndScrap in listProductivityAndScrapsActiveNotClosed)
                        {
                            productivityAndScrap.DateTo = minDateFrom.AddDays(-1);
                        }

                        resultContext = base.EntitySave<ProductivityAndScrap>(listProductivityAndScrapsActiveNotClosed, resultContext);
                    }                    
                }

                if (resultContext.ResultCode == ETEMEnums.ResultEnum.Success)
                {
                    resultContext = base.EntitySave<ProductivityAndScrap>(entities, resultContext);
                }
            }

            return resultContext;
        }

        public CallContext ProductivityAndScrapDelete(List<int> listSelectedIDs, CallContext resultContext)
        {
            try
            {
                resultContext.ResultCode = ETEMEnums.ResultEnum.Error;

                List<ProductivityAndScrap> listProductivityAndScraps = new List<ProductivityAndScrap>();
                List<ProductivityAndScrapDetail> listProductivityAndScrapDetails = new List<ProductivityAndScrapDetail>();

                listProductivityAndScraps = (from ps in this.dbContext.ProductivityAndScraps
                                             where listSelectedIDs.Contains(ps.idProductivityAndScrap)
                                             select ps).ToList<ProductivityAndScrap>();

                listProductivityAndScrapDetails = (from psd in this.dbContext.ProductivityAndScrapDetails
                                                   where listSelectedIDs.Contains(psd.idProductivityAndScrap)
                                                   select psd).ToList<ProductivityAndScrapDetail>();

                CallContext deleteContext = new CallContext();
                deleteContext = resultContext;

                deleteContext = new ProductivityAndScrapDetailBL().EntityDelete<ProductivityAndScrapDetail>(listProductivityAndScrapDetails, deleteContext);                

                if (deleteContext.ResultCode == ETEMEnums.ResultEnum.Success)
                {
                    deleteContext = base.EntityDelete<ProductivityAndScrap>(listProductivityAndScraps, deleteContext);

                    if (deleteContext.ResultCode == ETEMEnums.ResultEnum.Success)
                    {
                        resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
                        resultContext.Message = "Selected rows `Productivity & Scrap` and their details have been deleted successfully!";
                    }
                    else
                    {
                        resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                        resultContext.Message = "Error delete selected rows `Productivity & Scrap` and their details!";
                    }
                }
                else
                {
                    resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                    resultContext.Message = "Error delete selected rows `Productivity & Scrap`!";
                }
            }
            catch (Exception ex)
            {
                resultContext.Message = "Error delete selected rows `Productivity & Scrap`!";

                BaseHelper.Log("Error delete entities `ProductivityAndScrap`, IDs - (" + string.Join(",", listSelectedIDs.ToArray()) + ")!");
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

            return resultContext;
        }

        public CallContext ImportProductivityAndScrapCostData(string fileFullName, int idEntity, CallContext resultContext)
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

                    ExcelWorksheet workSheet = package.Workbook.Worksheets[1];                    

                    if (workSheet == null)
                    {
                        resultContext.Message = "Error! No Excel work sheet with `Productivity and Scrap` data!";
                        return resultContext;
                    }

                    ProductivityAndScrap productivityAndScrap = this.GetEntityById(idEntity);

                    if (productivityAndScrap == null)
                    {
                        resultContext.Message = "Entity `ProductivityAndScrap` not found by ID (`" + idEntity + "`)!";
                        return resultContext;
                    }

                    List<KeyValue> listKeyValuesToCostCenterPresses = new List<KeyValue>();
                    List<ProfileSetting> listProfileSettings = new List<ProfileSetting>();

                    List<string> listKeyTypeIntCodes = new List<string>()
                    {
                        ETEMEnums.KeyTypeEnum.CostCenter.ToString()
                    };

                    listKeyValuesToCostCenterPresses = (from kv in this.dbContext.KeyValues
                                                        join kt in this.dbContext.KeyTypes on kv.idKeyType equals kt.idKeyType
                                                        where listKeyTypeIntCodes.Contains(kt.KeyTypeIntCode)
                                                        select kv).ToList<KeyValue>();

                    listProfileSettings = (from ps in this.dbContext.ProfileSettings
                                           select ps).ToList();

                    List<ProductivityAndScrapDetail> listProductivityAndScrapDetailOld = new List<ProductivityAndScrapDetail>();
                    List<ProductivityAndScrapDetail> listProductivityAndScrapDetailNew = new List<ProductivityAndScrapDetail>();

                    listProductivityAndScrapDetailOld = (from psd in this.dbContext.ProductivityAndScrapDetails
                                                         where psd.idProductivityAndScrap == productivityAndScrap.idProductivityAndScrap
                                                         select psd).ToList<ProductivityAndScrapDetail>();

                    Dictionary<string, string> dictErrorsProductivityAndScrapDetails = new Dictionary<string, string>();

                    ProductivityAndScrapDetail newProductivityAndScrapDetail = new ProductivityAndScrapDetail();
                    
                    bool hasNotErrorInRow = true;

                    string rangeValueStr = string.Empty;

                    ExcelRange range;

                    for (; ;)
                    {
                        currRow++;

                        hasNotErrorInRow = true;
                        
                        if (string.IsNullOrWhiteSpace(workSheet.Cells[currRow, 1].Text) &&
                            string.IsNullOrWhiteSpace(workSheet.Cells[currRow, 2].Text) &&
                            string.IsNullOrWhiteSpace(workSheet.Cells[currRow, 3].Text) &&
                            string.IsNullOrWhiteSpace(workSheet.Cells[currRow, 4].Text) &&
                            string.IsNullOrWhiteSpace(workSheet.Cells[currRow, 5].Text) &&
                            string.IsNullOrWhiteSpace(workSheet.Cells[currRow, 6].Text))
                        {
                            break;
                        }

                        newProductivityAndScrapDetail = new ProductivityAndScrapDetail();

                        newProductivityAndScrapDetail.idProductivityAndScrap = idEntity;

                        currCol = 1;
                        range = workSheet.Cells[currRow, currCol];
                        rangeValueStr = (range.Value != null ? range.Value.ToString().Replace(" ", "") : string.Empty);

                        var kvPress = listKeyValuesToCostCenterPresses.Where(w => w.DefaultValue4 != null && w.DefaultValue4.Replace(" ", "").Trim().ToUpper() == rangeValueStr.Trim().ToUpper()).FirstOrDefault();

                        if (kvPress != null)
                        {
                            newProductivityAndScrapDetail.idCostCenter = kvPress.idKeyValue;
                        }
                        else
                        {
                            hasNotErrorInRow = false;
                            //if (dictErrorsProductivityAndScrapDetails.ContainsKey("CostCenterPresses"))
                            //{
                            //    dictErrorsProductivityAndScrapDetails["CostCenterPresses"] += "," + currRow;
                            //}
                            //else
                            //{
                            //    dictErrorsProductivityAndScrapDetails.Add("CostCenterPresses", currRow.ToString());
                            //}
                        }

                        currCol = 2;
                        range = workSheet.Cells[currRow, currCol];
                        rangeValueStr = (range.Value != null ? range.Value.ToString().Replace(" ", "") : string.Empty);

                        var profileSetting = listProfileSettings.Where(w => w.ProfileNameSAP != null && w.ProfileNameSAP.Replace(" ", "").Trim().ToUpper() == rangeValueStr.Trim().ToUpper()).FirstOrDefault();

                        if (profileSetting != null)
                        {
                            newProductivityAndScrapDetail.idProfileSetting = profileSetting.idProfileSetting;
                        }
                        else
                        {
                            hasNotErrorInRow = false;
                            //if (dictErrorsProductivityAndScrapDetails.ContainsKey("ProfileSetting"))
                            //{
                            //    dictErrorsProductivityAndScrapDetails["ProfileSetting"] += "," + currRow;
                            //}
                            //else
                            //{
                            //    dictErrorsProductivityAndScrapDetails.Add("ProfileSetting", currRow.ToString());
                            //}
                        }

                        currCol = 3;
                        range = workSheet.Cells[currRow, currCol];
                        rangeValueStr = (range.Value != null ? range.Value.ToString() : string.Empty);

                        res = Decimal.TryParse(rangeValueStr, NumberStyles.Any, BaseHelper.GetNumberFormatInfo("", ".", 9), out resultParseDecimal);
                        if (res)
                        {
                            newProductivityAndScrapDetail.SumOfHours = resultParseDecimal;
                        }
                        else
                        {
                            newProductivityAndScrapDetail.SumOfHours = decimal.Zero;
                        }

                        currCol = 4;
                        range = workSheet.Cells[currRow, currCol];
                        rangeValueStr = (range.Value != null ? range.Value.ToString() : string.Empty);

                        res = Decimal.TryParse(rangeValueStr, NumberStyles.Any, BaseHelper.GetNumberFormatInfo("", ".", 9), out resultParseDecimal);
                        if (res)
                        {
                            newProductivityAndScrapDetail.SumOfConsumption = resultParseDecimal;
                        }
                        else
                        {
                            newProductivityAndScrapDetail.SumOfConsumption = decimal.Zero;
                        }

                        currCol = 5;
                        range = workSheet.Cells[currRow, currCol];
                        rangeValueStr = (range.Value != null ? range.Value.ToString() : string.Empty);

                        res = Decimal.TryParse(rangeValueStr, NumberStyles.Any, BaseHelper.GetNumberFormatInfo("", ".", 9), out resultParseDecimal);
                        if (res)
                        {
                            newProductivityAndScrapDetail.SumOfProduction = resultParseDecimal;
                        }
                        else
                        {
                            newProductivityAndScrapDetail.SumOfProduction = decimal.Zero;
                        }

                        currCol = 7;
                        range = workSheet.Cells[currRow, currCol];
                        rangeValueStr = (range.Value != null ? range.Value.ToString() : string.Empty);

                        res = Decimal.TryParse(rangeValueStr, NumberStyles.Any, BaseHelper.GetNumberFormatInfo("", ".", 9), out resultParseDecimal);
                        if (res)
                        {
                            newProductivityAndScrapDetail.ProductivityKGh = resultParseDecimal;
                        }
                        else
                        {
                            newProductivityAndScrapDetail.ProductivityKGh = decimal.Zero;
                        }

                        currCol = 8;
                        range = workSheet.Cells[currRow, currCol];
                        rangeValueStr = (range.Value != null ? range.Value.ToString() : string.Empty);

                        res = Decimal.TryParse(rangeValueStr, NumberStyles.Any, BaseHelper.GetNumberFormatInfo("", ".", 9), out resultParseDecimal);
                        if (res)
                        {
                            newProductivityAndScrapDetail.ScrapRate = resultParseDecimal;
                        }
                        else
                        {
                            newProductivityAndScrapDetail.ScrapRate = decimal.Zero;
                        }

                        var checkProductivityAndScrapDetail = listProductivityAndScrapDetailOld.Where(w => w.idCostCenter == newProductivityAndScrapDetail.idCostCenter &&
                                                                                                      w.idProfileSetting == newProductivityAndScrapDetail.idProfileSetting).ToList();

                        if (checkProductivityAndScrapDetail.Count > 0)
                        {
                            hasNotErrorInRow = false;
                            if (dictErrorsProductivityAndScrapDetails.ContainsKey("DuplicateProductivityAndScrapDetail"))
                            {
                                dictErrorsProductivityAndScrapDetails["DuplicateProductivityAndScrapDetail"] += "," + currRow;
                            }
                            else
                            {
                                dictErrorsProductivityAndScrapDetails.Add("DuplicateProductivityAndScrapDetail", currRow.ToString());
                            }
                        }

                        if (hasNotErrorInRow)
                        {
                            listProductivityAndScrapDetailNew.Add(newProductivityAndScrapDetail);                            
                        }
                    }

                    if (dictErrorsProductivityAndScrapDetails.Count == 0)
                    {
                        resultContext = new ProductivityAndScrapDetailBL().EntitySave<ProductivityAndScrapDetail>(listProductivityAndScrapDetailNew, resultContext);
                        if (resultContext.ResultCode == ETEMEnums.ResultEnum.Success)
                        {                            
                            resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
                            resultContext.Message = "The productivity and scrap cost data have been imported successfully!";
                        }
                        else
                        {
                            resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                            resultContext.Message = "Error import the productivity and scrap cost data!";
                        }
                    }
                    else
                    {
                        List<string> listErrors = new List<string>();

                        if (dictErrorsProductivityAndScrapDetails.ContainsKey("CostCenterPresses"))
                        {
                            listErrors.Add("Error! The field `PRESS - Short Text` is missing or in wrong format, Rows (" + dictErrorsProductivityAndScrapDetails["CostCenterPresses"] + ")!");
                        }
                        if (dictErrorsProductivityAndScrapDetails.ContainsKey("ProfileSetting"))
                        {
                            listErrors.Add("Error! The field `SHAPE` is missing or in wrong format, Rows (" + dictErrorsProductivityAndScrapDetails["ProfileSetting"] + ")!");
                        }
                        if (dictErrorsProductivityAndScrapDetails.ContainsKey("DuplicateProductivityAndScrapDetail"))
                        {
                            listErrors.Add("Error! The selected file includes productivity and scrap cost data with duplicate data in the database, Rows (" + dictErrorsProductivityAndScrapDetails["DuplicateProductivityAndScrapDetail"] + ")!");
                        }                        

                        resultContext.Message = string.Join(Constants.ERROR_MESSAGES_SEPARATOR, listErrors);
                    }
                }
            }
            catch (Exception ex)
            {
                resultContext.Message = "Error import the productivity and scrap cost data!";

                BaseHelper.Log("Error import entities `ProductivityAndScrapDetail`!");
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

            return resultContext;
        }
    }
}