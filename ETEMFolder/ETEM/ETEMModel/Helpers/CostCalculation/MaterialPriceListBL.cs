using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Models;
using ETEMModel.Models.DataView.CostCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers.CostCalculation
{
    public class MaterialPriceListBL : BaseClassBL<MaterialPriceList>
    {
        public MaterialPriceListBL()
        {
            this.EntitySetName = "MaterialPriceLists";
        }

        internal override void EntityToEntity(MaterialPriceList sourceEntity, MaterialPriceList targetEntity)
        {
            targetEntity.LME = sourceEntity.LME;
            targetEntity.Premium = sourceEntity.Premium;
            targetEntity.DateFrom = sourceEntity.DateFrom;
            targetEntity.DateTo = sourceEntity.DateTo;
        }

        internal override MaterialPriceList GetEntityById(int idEntity)
        {
            return this.dbContext.MaterialPriceLists.Where(w => w.idMaterialPriceList == idEntity).FirstOrDefault();
        }

        internal List<MaterialPriceListDataView> GetAllMaterialPriceList(ICollection<AbstractSearch> searchCriteria,
                                                                         DateTime? dateActiveTo,
                                                                         string sortExpression, string sortDirection)
        {
            List<MaterialPriceListDataView> listView = new List<MaterialPriceListDataView>();

            listView = (from mpl in this.dbContext.MaterialPriceLists
                        orderby mpl.DateFrom ascending
                        where (dateActiveTo.HasValue ?
                               ((!mpl.DateTo.HasValue && mpl.DateFrom <= dateActiveTo.Value) ||
                                (mpl.DateTo.HasValue && mpl.DateFrom <= dateActiveTo.Value && mpl.DateTo >= dateActiveTo.Value)) :
                              1 == 1)
                        select new MaterialPriceListDataView
                        {
                            idMaterialPriceList = mpl.idMaterialPriceList,
                            LME = mpl.LME,
                            Premium = mpl.Premium,
                            DateFrom = mpl.DateFrom,
                            DateTo = mpl.DateTo
                        }).ApplySearchCriterias(searchCriteria).ToList();

            if (string.IsNullOrEmpty(sortExpression) || sortExpression == Constants.INVALID_ID_STRING)
            {
                sortDirection = string.Empty;

                sortExpression = "DateFrom";
            }

            listView = OrderByHelper.OrderBy<MaterialPriceListDataView>(listView, sortExpression, sortDirection).ToList<MaterialPriceListDataView>();

            return listView;
        }


        internal MaterialPriceListDataView GetActiveMaterialPriceList(DateTime? dateActiveTo)
        {    
            ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();

            return GetAllMaterialPriceList(searchCriteria,dateActiveTo, "", "").FirstOrDefault(s=>s.Status == "Active");
        }


        internal CallContext MaterialPriceListSave(List<MaterialPriceList> entities, CallContext resultContext)
        {
            if (entities.Count > 0)
            {
                List<MaterialPriceList> listMaterialPriceListsActiveNotClosed = new List<MaterialPriceList>();

                var listNewMaterialPriceLists = entities.Where(w => w.idMaterialPriceList == Constants.INVALID_ID ||
                                                               w.idMaterialPriceList == Constants.INVALID_ID_ZERO).ToList<MaterialPriceList>();

                if (listNewMaterialPriceLists.Count > 0)
                {
                    listMaterialPriceListsActiveNotClosed = (from mpl in this.dbContext.MaterialPriceLists
                                                             where !mpl.DateTo.HasValue
                                                             select mpl).ToList<MaterialPriceList>();
                }

                if (listMaterialPriceListsActiveNotClosed.Count > 0)
                {
                    var minDateFrom = listNewMaterialPriceLists.Min(m => m.DateFrom);

                    if (minDateFrom != DateTime.MinValue)
                    {
                        foreach (MaterialPriceList materialPriceList in listMaterialPriceListsActiveNotClosed)
                        {
                            materialPriceList.DateTo = minDateFrom.AddDays(-1);
                        }

                        resultContext = base.EntitySave<MaterialPriceList>(listMaterialPriceListsActiveNotClosed, resultContext);
                    }                    
                }

                if (resultContext.ResultCode == ETEMEnums.ResultEnum.Success)
                {
                    resultContext = base.EntitySave<MaterialPriceList>(entities, resultContext);
                }
            }

            return resultContext;
        }

        public CallContext MaterialPriceListDelete(List<int> listSelectedIDs, CallContext resultContext)
        {
            try
            {
                resultContext.ResultCode = ETEMEnums.ResultEnum.Error;

                List<MaterialPriceList> listMaterialPriceLists = new List<MaterialPriceList>();

                listMaterialPriceLists = (from mpl in this.dbContext.MaterialPriceLists
                                          where listSelectedIDs.Contains(mpl.idMaterialPriceList)
                                          select mpl).ToList<MaterialPriceList>();

                CallContext deleteContext = new CallContext();
                deleteContext = resultContext;

                deleteContext = base.EntityDelete<MaterialPriceList>(listMaterialPriceLists, deleteContext);

                if (deleteContext.ResultCode == ETEMEnums.ResultEnum.Success)
                {
                    resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
                    resultContext.Message = "Selected rows `LME&PREMIUM Price Lists` have been deleted successfully!";
                }
                else
                {
                    resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                    resultContext.Message = "Error delete selected rows `LME&PREMIUM Price Lists`!";
                }
            }
            catch (Exception ex)
            {
                resultContext.Message = "Error delete selected rows `LME&PREMIUM Price Lists`!";

                BaseHelper.Log("Error delete entities `MaterialPriceList`, IDs - (" + string.Join(",", listSelectedIDs.ToArray()) + ")!");
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

            return resultContext;
        }
    }
}