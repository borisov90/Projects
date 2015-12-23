using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Models;
using ETEMModel.Models.DataView.CostCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers.CostCalculation
{
    public class CommissionsByAgentBL : BaseClassBL<CommissionsByAgent>
    {
        public CommissionsByAgentBL()
        {
            this.EntitySetName = "CommissionsByAgents";
        }

        internal override void EntityToEntity(CommissionsByAgent sourceEntity, CommissionsByAgent targetEntity)
        {
            targetEntity.idAgent = sourceEntity.idAgent;
            targetEntity.FixedCommission = sourceEntity.FixedCommission;
            targetEntity.CommissionPercent = sourceEntity.CommissionPercent;
            targetEntity.DateFrom = sourceEntity.DateFrom;
            targetEntity.DateTo = sourceEntity.DateTo;
        }

        internal override CommissionsByAgent GetEntityById(int idEntity)
        {
            return this.dbContext.CommissionsByAgents.Where(w => w.idCommissionsByAgent == idEntity).FirstOrDefault();
        }

        internal List<CommissionsByAgentDataView> GetAllCommissionsByAgentsList(ICollection<AbstractSearch> searchCriteria,
                                                                                DateTime? dateActiveTo,
                                                                                string sortExpression, string sortDirection)
        {
            List<CommissionsByAgentDataView> listView = new List<CommissionsByAgentDataView>();

            listView = (from ca in this.dbContext.CommissionsByAgents
                        join kvA in this.dbContext.KeyValues on ca.idAgent equals kvA.idKeyValue
                        where (dateActiveTo.HasValue ?
                               ((!ca.DateTo.HasValue && ca.DateFrom <= dateActiveTo.Value) ||
                                (ca.DateTo.HasValue && ca.DateFrom <= dateActiveTo.Value && ca.DateTo >= dateActiveTo.Value)) :
                              1 == 1)
                        orderby kvA.Name ascending, ca.DateFrom ascending
                        select new CommissionsByAgentDataView
                        {
                            idCommissionsByAgent = ca.idCommissionsByAgent,
                            idAgent = ca.idAgent,
                            AgentName = kvA.Name,
                            FixedCommission = ca.FixedCommission,
                            CommissionPercent = ca.CommissionPercent,
                            DateFrom = ca.DateFrom,
                            DateTo = ca.DateTo
                        }).ApplySearchCriterias(searchCriteria).ToList<CommissionsByAgentDataView>();

            if (string.IsNullOrEmpty(sortExpression) || sortExpression == Constants.INVALID_ID_STRING)
            {
                sortDirection = string.Empty;

                sortExpression = "AgentName, DateFrom";
            }

            listView = OrderByHelper.OrderBy<CommissionsByAgentDataView>(listView, sortExpression, sortDirection).ToList<CommissionsByAgentDataView>();

            return listView;
        }

        public CommissionsByAgent GetCommissionsByAgentByDateActiveTo(int idAgent, DateTime dateActiveTo)
        {
            CommissionsByAgent result = new CommissionsByAgent();

            result = this.dbContext.CommissionsByAgents.Where(w => (w.DateFrom <= dateActiveTo && !w.DateTo.HasValue && w.idAgent == idAgent) ||
                                                              (w.DateTo.HasValue &&
                                                               w.DateFrom <= dateActiveTo &&
                                                               w.DateTo >= dateActiveTo &&
                                                               w.idAgent == idAgent)).FirstOrDefault();

            return result;
        }

        internal CallContext CommissionsByAgentSave(List<CommissionsByAgent> entities, CallContext resultContext)
        {
            if (entities.Count > 0)
            {
                List<CommissionsByAgent> listCommissionsByAgentsActiveNotClosed = new List<CommissionsByAgent>();

                var listNewCommissionsByAgents = entities.Where(w => w.idCommissionsByAgent == Constants.INVALID_ID ||
                                                                w.idCommissionsByAgent == Constants.INVALID_ID_ZERO).ToList();

                if (listNewCommissionsByAgents.Count > 0)
                {
                    var listAgentIDs = listNewCommissionsByAgents.Select(s => s.idAgent).ToList();

                    listCommissionsByAgentsActiveNotClosed = (from ca in this.dbContext.CommissionsByAgents
                                                              where listAgentIDs.Contains(ca.idAgent) &&
                                                                    !ca.DateTo.HasValue
                                                              select ca).ToList<CommissionsByAgent>();
                }

                if (listCommissionsByAgentsActiveNotClosed.Count > 0)
                {
                    var minDateFrom = listNewCommissionsByAgents.Min(m => m.DateFrom);

                    if (minDateFrom != DateTime.MinValue)
                    {
                        foreach (CommissionsByAgent commissionsByAgent in listCommissionsByAgentsActiveNotClosed)
                        {
                            commissionsByAgent.DateTo = minDateFrom.AddDays(-1);
                        }

                        resultContext = base.EntitySave<CommissionsByAgent>(listCommissionsByAgentsActiveNotClosed, resultContext);
                    }                    
                }

                if (resultContext.ResultCode == ETEMEnums.ResultEnum.Success)
                {
                    resultContext = base.EntitySave<CommissionsByAgent>(entities, resultContext);
                }
            }

            return resultContext;
        }

        public CallContext CommissionsByAgentDelete(List<int> listSelectedIDs, CallContext resultContext)
        {
            try
            {
                resultContext.ResultCode = ETEMEnums.ResultEnum.Error;

                List<CommissionsByAgent> listCommissionsByAgents = new List<CommissionsByAgent>();

                listCommissionsByAgents = (from ca in this.dbContext.CommissionsByAgents
                                           where listSelectedIDs.Contains(ca.idCommissionsByAgent)
                                           select ca).ToList();

                CallContext deleteContext = new CallContext();
                deleteContext = resultContext;

                deleteContext = base.EntityDelete<CommissionsByAgent>(listCommissionsByAgents, deleteContext);

                if (deleteContext.ResultCode == ETEMEnums.ResultEnum.Success)
                {
                    resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
                    resultContext.Message = "Selected rows `Commissions by Agent` have been deleted successfully!";
                }
                else
                {
                    resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                    resultContext.Message = "Error delete selected rows `Commissions by Agent`!";
                }
            }
            catch (Exception ex)
            {
                resultContext.Message = "Error delete selected rows `Commissions by Agent`!";

                BaseHelper.Log("Error delete entities `CommissionsByAgent`, IDs - (" + string.Join(",", listSelectedIDs.ToArray()) + ")!");
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

            return resultContext;
        }
    }
}