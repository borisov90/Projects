using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Models;
using ETEMModel.Models.DataView.CostCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers.CostCalculation
{
    public class SAPDataExpensesBL : BaseClassBL<SAPDataExpense>
    {
        public SAPDataExpensesBL()
        {
            this.EntitySetName = "SAPDataExpenses";
        }

        internal override void EntityToEntity(SAPDataExpense sourceEntity, SAPDataExpense targetEntity)
        {
            targetEntity.idSAPData       = sourceEntity.idSAPData;
            targetEntity.idCostCenter = sourceEntity.idCostCenter;
            targetEntity.idExpensesType = sourceEntity.idExpensesType;
            targetEntity.ValueData = sourceEntity.ValueData;
        }

        internal override SAPDataExpense GetEntityById(int idEntity)
        {
            return this.dbContext.SAPDataExpenses.Where(w => w.idSAPDataExpense == idEntity).FirstOrDefault();
        }

        internal List<SAPDataExpenseDataView> GetAllSAPDataExpense(ICollection<AbstractSearch> searchCriteria,
                                                                   DateTime? dateActiveTo,
                                                                   string sortExpression, string sortDirection)
        {
            List<SAPDataExpenseDataView> listView = new List<SAPDataExpenseDataView>();

            listView = (from sde in this.dbContext.SAPDataExpenses
                        join sd in this.dbContext.SAPDatas on sde.idSAPData equals sd.idSAPData
                        join kvCc in this.dbContext.KeyValues on sde.idCostCenter equals kvCc.idKeyValue
                        join kvExpT in this.dbContext.KeyValues on sde.idExpensesType equals kvExpT.idKeyValue
                        where (dateActiveTo.HasValue ?
                               ((!sd.DateTo.HasValue && sd.DateFrom <= dateActiveTo.Value) ||
                                (sd.DateTo.HasValue && sd.DateFrom <= dateActiveTo.Value && sd.DateTo >= dateActiveTo.Value)) :
                              1 == 1)
                        orderby sd.DateFrom ascending
                        select new SAPDataExpenseDataView
                        {
                            idSAPDataExpense = sde.idSAPDataExpense,
                            idSAPData = sde.idSAPData,
                            idCostCenter = sde.idCostCenter,
                            CostCenterName = kvCc.Name,
                            idExpensesType = sde.idExpensesType,
                            ExpenseTypeName = kvExpT.Name,
                            ValueData = sde.ValueData,
                            DateFrom = sd.DateFrom,
                            DateTo = sd.DateTo
                        }).ApplySearchCriterias(searchCriteria).ToList<SAPDataExpenseDataView>();

            if (string.IsNullOrEmpty(sortExpression) || sortExpression == Constants.INVALID_ID_STRING)
            {
                sortDirection = string.Empty;

                sortExpression = "DateFrom";
            }

            listView = OrderByHelper.OrderBy<SAPDataExpenseDataView>(listView, sortExpression, sortDirection).ToList<SAPDataExpenseDataView>();

            return listView;
        }

        public CallContext SAPDataExpenseDelete(List<int> listSelectedIDs, CallContext resultContext)
        {
            try
            {
                resultContext.ResultCode = ETEMEnums.ResultEnum.Error;

                List<SAPDataExpense> listSAPDataExpenses = new List<SAPDataExpense>();

                listSAPDataExpenses = (from sde in this.dbContext.SAPDataExpenses
                                       where listSelectedIDs.Contains(sde.idSAPDataExpense)
                                       select sde).ToList();

                CallContext deleteContext = new CallContext();
                deleteContext = resultContext;

                deleteContext = base.EntityDelete<SAPDataExpense>(listSAPDataExpenses, deleteContext);

                if (deleteContext.ResultCode == ETEMEnums.ResultEnum.Success)
                {
                    resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
                    resultContext.Message = "Selected rows `Expenses by Cost Centers` have been deleted successfully!";
                }
                else
                {
                    resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                    resultContext.Message = "Error delete selected rows `Expenses by Cost Centers`!";
                }
            }
            catch (Exception ex)
            {
                resultContext.Message = "Error delete selected rows `Expenses by Cost Centers`!";

                BaseHelper.Log("Error delete entities `SAPDataExpense`, IDs - (" + string.Join(",", listSelectedIDs.ToArray()) + ")!");
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

            return resultContext;
        }
    }
}