using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Models;
using ETEMModel.Models.DataView.CostCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers.CostCalculation
{
    public class ProductivityAndScrapDetailBL : BaseClassBL<ProductivityAndScrapDetail>
    {
        public ProductivityAndScrapDetailBL()
        {
            this.EntitySetName = "ProductivityAndScrapDetails";
        }

        internal override void EntityToEntity(ProductivityAndScrapDetail sourceEntity, ProductivityAndScrapDetail targetEntity)
        {
            targetEntity.idProductivityAndScrap = sourceEntity.idProductivityAndScrap;
            targetEntity.idCostCenter = sourceEntity.idCostCenter;
            targetEntity.idProfileSetting = sourceEntity.idProfileSetting;
            targetEntity.SumOfHours = sourceEntity.SumOfHours;
            targetEntity.SumOfConsumption = sourceEntity.SumOfConsumption;
            targetEntity.SumOfProduction = sourceEntity.SumOfProduction;
            targetEntity.ProductivityKGh = sourceEntity.ProductivityKGh;
            targetEntity.ScrapRate = sourceEntity.ScrapRate;
        }

        internal override ProductivityAndScrapDetail GetEntityById(int idEntity)
        {
            return this.dbContext.ProductivityAndScrapDetails.Where(w => w.idProductivityAndScrapDetail == idEntity).FirstOrDefault();
        }

        internal List<ProductivityAndScrapDetailDataView> GetAllProductivityAndScrapDetailList(ICollection<AbstractSearch> searchCriteria,
                                                                                               DateTime? dateActiveTo,
                                                                                               string sortExpression, string sortDirection)
        {
            List<ProductivityAndScrapDetailDataView> listView = new List<ProductivityAndScrapDetailDataView>();

            listView = (from psd in this.dbContext.ProductivityAndScrapDetails 
                        join ps in this.dbContext.ProductivityAndScraps on psd.idProductivityAndScrap equals ps.idProductivityAndScrap
                        join kvCc in this.dbContext.KeyValues on psd.idCostCenter equals kvCc.idKeyValue
                        join pfs in this.dbContext.ProfileSettings on psd.idProfileSetting equals pfs.idProfileSetting into grPfs
                        from subPfs in grPfs.DefaultIfEmpty()
                        orderby ps.DateFrom ascending
                        where (dateActiveTo.HasValue ?
                               ((!ps.DateTo.HasValue && ps.DateFrom <= dateActiveTo.Value) ||
                                (ps.DateTo.HasValue && ps.DateFrom <= dateActiveTo.Value && ps.DateTo >= dateActiveTo.Value)) :
                              1 == 1)
                        select new ProductivityAndScrapDetailDataView
                        {
                            idProductivityAndScrapDetail = psd.idProductivityAndScrapDetail,
                            idProductivityAndScrap = psd.idProductivityAndScrap,
                            DateFrom = ps.DateFrom,
                            DateTo = ps.DateTo,
                            idCostCenter = psd.idCostCenter,
                            CostCenterName = kvCc.DefaultValue4,
                            idProfileSetting = psd.idProfileSetting,
                            ProfileSettingName = (subPfs != null ? subPfs.ProfileName : string.Empty),
                            SumOfHours = psd.SumOfHours,
                            SumOfConsumption = psd.SumOfConsumption,
                            SumOfProduction = psd.SumOfProduction,
                            ProductivityKGh = psd.ProductivityKGh,
                            ScrapRate = psd.ScrapRate
                        }).ApplySearchCriterias(searchCriteria).ToList();

            if (string.IsNullOrEmpty(sortExpression) || sortExpression == Constants.INVALID_ID_STRING)
            {
                sortDirection = string.Empty;

                sortExpression = "DateFrom";
            }

            listView = OrderByHelper.OrderBy<ProductivityAndScrapDetailDataView>(listView, sortExpression, sortDirection).ToList<ProductivityAndScrapDetailDataView>();

            return listView;
        }

        public ProductivityAndScrapDetail GetEntityByDateActiveToAndPressAndProfile(DateTime? dateActiveTo, int idCostCenterPress, int idProfile)
        {
            var entity = (from psd in this.dbContext.ProductivityAndScrapDetails
                          join ps in this.dbContext.ProductivityAndScraps on psd.idProductivityAndScrap equals ps.idProductivityAndScrap                        
                          where (dateActiveTo.HasValue ?
                                 ((!ps.DateTo.HasValue && ps.DateFrom <= dateActiveTo.Value) ||
                                   (ps.DateTo.HasValue && ps.DateFrom <= dateActiveTo.Value && ps.DateTo >= dateActiveTo.Value)) :
                                 1 == 1) &&
                                psd.idCostCenter == idCostCenterPress && psd.idProfileSetting == idProfile
                          select psd).FirstOrDefault();

            return entity;
        }

        public CallContext ProductivityAndScrapDetailDelete(List<int> listSelectedIDs, CallContext resultContext)
        {
            try
            {
                resultContext.ResultCode = ETEMEnums.ResultEnum.Error;

                List<ProductivityAndScrapDetail> listProductivityAndScrapDetails = new List<ProductivityAndScrapDetail>();

                listProductivityAndScrapDetails = (from psd in this.dbContext.ProductivityAndScrapDetails
                                                   where listSelectedIDs.Contains(psd.idProductivityAndScrapDetail)
                                                   select psd).ToList();

                CallContext deleteContext = new CallContext();
                deleteContext = resultContext;

                deleteContext = base.EntityDelete<ProductivityAndScrapDetail>(listProductivityAndScrapDetails, deleteContext);

                if (deleteContext.ResultCode == ETEMEnums.ResultEnum.Success)
                {
                    resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
                    resultContext.Message = "Selected rows `Productivity & Scrap details` have been deleted successfully!";
                }
                else
                {
                    resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                    resultContext.Message = "Error delete selected rows `Productivity & Scrap details`!";
                }
            }
            catch (Exception ex)
            {
                resultContext.Message = "Error delete selected rows `Productivity & Scrap details`!";

                BaseHelper.Log("Error delete entities `ProductivityAndScrapDetail`, IDs - (" + string.Join(",", listSelectedIDs.ToArray()) + ")!");
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

            return resultContext;
        }
    }
}