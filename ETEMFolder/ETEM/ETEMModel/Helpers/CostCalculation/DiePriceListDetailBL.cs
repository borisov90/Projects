using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Models;
using ETEMModel.Models.DataView.CostCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers.CostCalculation
{
    public class DiePriceListDetailBL : BaseClassBL<DiePriceListDetail>
    {
        public DiePriceListDetailBL()
        {
            this.EntitySetName = "DiePriceListDetails";
        }

        internal override void EntityToEntity(DiePriceListDetail sourceEntity, DiePriceListDetail targetEntity)
        {
            targetEntity.idDiePriceList = sourceEntity.idDiePriceList;
            targetEntity.idNumberOfCavities = sourceEntity.idNumberOfCavities;
            targetEntity.idProfileCategory = sourceEntity.idProfileCategory;
            targetEntity.idProfileComplexity = sourceEntity.idProfileComplexity;
            targetEntity.DimensionA = sourceEntity.DimensionA;
            targetEntity.DimensionB = sourceEntity.DimensionB;
            targetEntity.Price = sourceEntity.Price;
            targetEntity.Lifespan = sourceEntity.Lifespan;
        }

        internal override DiePriceListDetail GetEntityById(int idEntity)
        {
            return this.dbContext.DiePriceListDetails.Where(w => w.idDiePriceListDetail == idEntity).FirstOrDefault();
        }

        internal List<DiePriceListDetailDataView> GetAllDiePriceListDetails(ICollection<AbstractSearch> searchCriteria,
                                                                            DateTime? dateActiveTo,
                                                                            string sortExpression, string sortDirection)
        {
            List<DiePriceListDetailDataView> listView = new List<DiePriceListDetailDataView>();

            listView = (from dpld in this.dbContext.DiePriceListDetails 
                        join dpl in this.dbContext.DiePriceLists on dpld.idDiePriceList equals dpl.idDiePriceList
                        join kvV in this.dbContext.KeyValues on dpl.idVendor equals kvV.idKeyValue into grV
                        from subV in grV.DefaultIfEmpty()
                        join kvNmCav in this.dbContext.KeyValues on dpld.idNumberOfCavities equals kvNmCav.idKeyValue into grNmCav
                        from subNmCav in grNmCav.DefaultIfEmpty()
                        join kvPrCat in this.dbContext.KeyValues on dpld.idProfileCategory equals kvPrCat.idKeyValue into grPrCat
                        from subPrCat in grPrCat.DefaultIfEmpty()
                        join kvPrCompl in this.dbContext.KeyValues on dpld.idProfileComplexity equals kvPrCompl.idKeyValue into grPrCompl
                        from subPrCompl in grPrCompl.DefaultIfEmpty()
                        where (dateActiveTo.HasValue ?
                               ((!dpl.DateTo.HasValue && dpl.DateFrom <= dateActiveTo.Value) ||
                                (dpl.DateTo.HasValue && dpl.DateFrom <= dateActiveTo.Value && dpl.DateTo >= dateActiveTo.Value)) :
                              1 == 1)
                        orderby dpl.DateFrom ascending
                        select new DiePriceListDetailDataView
                        {
                            idDiePriceListDetail = dpld.idDiePriceListDetail,
                            idDiePriceList = dpl.idDiePriceList,
                            VendorName = (subV != null ? subV.Name : string.Empty),
                            idVendor = dpl.idVendor,
                            DateFrom = dpl.DateFrom,
                            DateTo = dpl.DateTo,
                            idNumberOfCavities = dpld.idNumberOfCavities,
                            NumberOfCavitiesName = (subNmCav != null ? subNmCav.Name : string.Empty),
                            idProfileCategory = dpld.idProfileCategory,
                            ProfileCategoryName = (subPrCat != null ? subPrCat.Name : string.Empty),
                            idProfileComplexity = dpld.idProfileComplexity,
                            ProfileComplexityName = (subPrCompl != null ? subPrCompl.Name : string.Empty),
                            DimensionA = dpld.DimensionA,
                            DimensionB = dpld.DimensionB,
                            Price = dpld.Price,
                            Lifespan = dpld.Lifespan
                        }).ApplySearchCriterias(searchCriteria).ToList<DiePriceListDetailDataView>();

         



            if (string.IsNullOrEmpty(sortExpression) || sortExpression == Constants.INVALID_ID_STRING)
            {
                sortDirection = string.Empty;

                sortExpression = "VendorName, DimensionA, DimensionB, ProfileComplexityName, ProfileCategoryName, NumberOfCavitiesName";
            }

            listView = OrderByHelper.OrderBy<DiePriceListDetailDataView>(listView, sortExpression, sortDirection).ToList<DiePriceListDetailDataView>();

            return listView;
        }

        public CallContext DiePriceListDetailDelete(List<int> listSelectedIDs, CallContext resultContext)
        {
            try
            {
                resultContext.ResultCode = ETEMEnums.ResultEnum.Error;

                List<DiePriceListDetail> listDiePriceListDetails = new List<DiePriceListDetail>();

                listDiePriceListDetails = (from dpld in this.dbContext.DiePriceListDetails
                                           where listSelectedIDs.Contains(dpld.idDiePriceListDetail)
                                           select dpld).ToList();

                CallContext deleteContext = new CallContext();
                deleteContext = resultContext;

                deleteContext = base.EntityDelete<DiePriceListDetail>(listDiePriceListDetails, deleteContext);

                if (deleteContext.ResultCode == ETEMEnums.ResultEnum.Success)
                {
                    resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
                    resultContext.Message = "Selected rows `Die Price Lists by Dimensions` have been deleted successfully!";
                }
                else
                {
                    resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                    resultContext.Message = "Error delete selected rows `Die Price Lists by Dimensions`!";
                }
            }
            catch (Exception ex)
            {
                resultContext.Message = "Error delete selected rows `Die Price Lists by Dimensions`!";

                BaseHelper.Log("Error delete entities `DiePriceListDetail`, IDs - (" + string.Join(",", listSelectedIDs.ToArray()) + ")!");
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

            return resultContext;
        }
    }
}