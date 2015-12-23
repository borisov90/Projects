using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Models;
using ETEMModel.Models.DataView.CostCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers.CostCalculation
{
    public class SAPDataQuantityBL : BaseClassBL<SAPDataQuantity>
    {
        public SAPDataQuantityBL()
        {
            this.EntitySetName = "SAPDataQuantities";
        }

        internal override void EntityToEntity(SAPDataQuantity sourceEntity, SAPDataQuantity targetEntity)
        {
            targetEntity.idSAPData       = sourceEntity.idSAPData;
            targetEntity.idCostCenter = sourceEntity.idCostCenter;
            targetEntity.idQuantityType = sourceEntity.idQuantityType;
            targetEntity.ValueData = sourceEntity.ValueData;

        }

        internal override SAPDataQuantity GetEntityById(int idEntity)
        {
            return this.dbContext.SAPDataQuantities.Where(w => w.idSAPDataQuantity == idEntity).FirstOrDefault();
        }

        internal List<SAPDataQuantityDataView> GetAllSAPDataQuantity(ICollection<AbstractSearch> searchCriteria,
                                                                     DateTime? dateActiveTo,
                                                                     string sortExpression, string sortDirection)
        {
            List<SAPDataQuantityDataView> listView = new List<SAPDataQuantityDataView>();

            listView = (from sdq in this.dbContext.SAPDataQuantities
                        join sd in this.dbContext.SAPDatas on sdq.idSAPData equals sd.idSAPData
                        join kvCc in this.dbContext.KeyValues on sdq.idCostCenter equals kvCc.idKeyValue
                        join kvQt in this.dbContext.KeyValues on sdq.idQuantityType equals kvQt.idKeyValue
                        where (dateActiveTo.HasValue ?
                               ((!sd.DateTo.HasValue && sd.DateFrom <= dateActiveTo.Value) ||
                                (sd.DateTo.HasValue && sd.DateFrom <= dateActiveTo.Value && sd.DateTo >= dateActiveTo.Value)) :
                              1 == 1)
                        orderby sd.DateFrom ascending
                        select new SAPDataQuantityDataView
                        {
                            idSAPDataQuantity = sdq.idSAPDataQuantity,
                            idSAPData = sdq.idSAPData,
                            idCostCenter = sdq.idCostCenter,
                            CostCenterName = kvCc.Name,
                            idQuantityType = sdq.idQuantityType,
                            QuantityTypeName = kvQt.Name,
                            ValueData = sdq.ValueData,
                            DateFrom = sd.DateFrom,
                            DateTo = sd.DateTo
                        }).ApplySearchCriterias(searchCriteria).ToList<SAPDataQuantityDataView>();

            if (string.IsNullOrEmpty(sortExpression) || sortExpression == Constants.INVALID_ID_STRING)
            {
                sortDirection = string.Empty;

                sortExpression = "DateFrom";
            }

            listView = OrderByHelper.OrderBy<SAPDataQuantityDataView>(listView, sortExpression, sortDirection).ToList<SAPDataQuantityDataView>();

            return listView;
        }

        public CallContext SAPDataQuantityDelete(List<int> listSelectedIDs, CallContext resultContext)
        {
            try
            {
                resultContext.ResultCode = ETEMEnums.ResultEnum.Error;

                List<SAPDataQuantity> listSAPDataQuantities = new List<SAPDataQuantity>();

                listSAPDataQuantities = (from sdq in this.dbContext.SAPDataQuantities
                                         where listSelectedIDs.Contains(sdq.idSAPDataQuantity)
                                         select sdq).ToList();

                CallContext deleteContext = new CallContext();
                deleteContext = resultContext;

                deleteContext = base.EntityDelete<SAPDataQuantity>(listSAPDataQuantities, deleteContext);

                if (deleteContext.ResultCode == ETEMEnums.ResultEnum.Success)
                {
                    resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
                    resultContext.Message = "Selected rows `Quantities by Cost Centers` have been deleted successfully!";
                }
                else
                {
                    resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                    resultContext.Message = "Error delete selected rows `Quantities by Cost Centers`!";
                }
            }
            catch (Exception ex)
            {
                resultContext.Message = "Error delete selected rows `Quantities by Cost Centers`!";

                BaseHelper.Log("Error delete entities `SAPDataQuantity`, IDs - (" + string.Join(",", listSelectedIDs.ToArray()) + ")!");
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

            return resultContext;
        }
    }
}