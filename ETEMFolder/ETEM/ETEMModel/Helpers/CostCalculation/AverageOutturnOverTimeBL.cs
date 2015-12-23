using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Models;
using ETEMModel.Models.DataView.CostCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers.CostCalculation
{
    public class AverageOutturnOverTimeBL : BaseClassBL<AverageOutturnOverTime>
    {
        public AverageOutturnOverTimeBL()
        {
            this.EntitySetName = "AverageOutturnOverTimes";
        }

        internal override void EntityToEntity(AverageOutturnOverTime sourceEntity, AverageOutturnOverTime targetEntity)
        {
            targetEntity.idAverageOutturnOverTime = sourceEntity.idAverageOutturnOverTime;

            targetEntity.ValueOfPressSMS1 = sourceEntity.ValueOfPressSMS1;
            targetEntity.ValueOfPressSMS2 = sourceEntity.ValueOfPressSMS2;
            targetEntity.ValueOfPressBREDA = sourceEntity.ValueOfPressBREDA;
            targetEntity.ValueOfPressFARREL = sourceEntity.ValueOfPressFARREL;

            targetEntity.DateFrom = sourceEntity.DateFrom;
            targetEntity.DateTo = sourceEntity.DateTo;
        }

        internal override AverageOutturnOverTime GetEntityById(int idEntity)
        {
            return this.dbContext.AverageOutturnOverTimes.Where(w => w.idAverageOutturnOverTime == idEntity).FirstOrDefault();
        }

         internal List<AverageOutturnOverTimeDataView> GetAllAverageOutturnOverTimeDataView(ICollection<AbstractSearch> searchCriteria,
                                                                         DateTime? dateActiveTo,
                                                                         string sortExpression, string sortDirection)
        {
            List<AverageOutturnOverTimeDataView> listView = new List<AverageOutturnOverTimeDataView>();

            listView = (from mpl in this.dbContext.AverageOutturnOverTimes
                        orderby mpl.DateFrom ascending
                        where (dateActiveTo.HasValue ?
                               ((!mpl.DateTo.HasValue && mpl.DateFrom <= dateActiveTo.Value) ||
                                (mpl.DateTo.HasValue && mpl.DateFrom <= dateActiveTo.Value && mpl.DateTo >= dateActiveTo.Value)) :
                              1 == 1)
                        select new AverageOutturnOverTimeDataView
                        {
                            idAverageOutturnOverTime = mpl.idAverageOutturnOverTime,
                            ValueOfPressSMS1 = mpl.ValueOfPressSMS1,
                            ValueOfPressSMS2 = mpl.ValueOfPressSMS2,
                            ValueOfPressBREDA = mpl.ValueOfPressBREDA,
                            ValueOfPressFARREL = mpl.ValueOfPressFARREL,
                            DateFrom = mpl.DateFrom,
                            DateTo = mpl.DateTo
                        }).ApplySearchCriterias(searchCriteria).ToList();

            if (string.IsNullOrEmpty(sortExpression) || sortExpression == Constants.INVALID_ID_STRING)
            {
                sortDirection = string.Empty;

                sortExpression = "DateFrom";
            }

            listView = OrderByHelper.OrderBy<AverageOutturnOverTimeDataView>(listView, sortExpression, sortDirection).ToList<AverageOutturnOverTimeDataView>();

            return listView;
        }

        internal AverageOutturnOverTimeDataView GetActiveAverageOutturnOverTime(DateTime? dateActiveTo)
        {    
            ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();

            return GetAllAverageOutturnOverTimeDataView(searchCriteria,dateActiveTo, "", "").FirstOrDefault();
        }

        internal CallContext AverageOutturnOverTimeSave(List<AverageOutturnOverTime> entities, CallContext resultContext)
        {
            if (entities.Count > 0)
            {
                List<AverageOutturnOverTime> listAverageOutturnOverTimeActiveNotClosed = new List<AverageOutturnOverTime>();

                var listNewAverageOutturnOverTime = entities.Where(w => w.idAverageOutturnOverTime == Constants.INVALID_ID ||
                                                                   w.idAverageOutturnOverTime == Constants.INVALID_ID_ZERO).ToList<AverageOutturnOverTime>();

                if (listNewAverageOutturnOverTime.Count > 0)
                {
                    listAverageOutturnOverTimeActiveNotClosed = (from mpl in this.dbContext.AverageOutturnOverTimes
                                                                 where !mpl.DateTo.HasValue
                                                                 select mpl).ToList<AverageOutturnOverTime>();
                }

                if (listAverageOutturnOverTimeActiveNotClosed.Count > 0)
                {
                    var minDateFrom = listNewAverageOutturnOverTime.Min(m => m.DateFrom);

                    if (minDateFrom != DateTime.MinValue)
                    {
                        foreach (AverageOutturnOverTime averageOutturnOverTime in listAverageOutturnOverTimeActiveNotClosed)
                        {
                            averageOutturnOverTime.DateTo = minDateFrom.AddDays(-1);
                        }

                        resultContext = base.EntitySave<AverageOutturnOverTime>(listAverageOutturnOverTimeActiveNotClosed, resultContext);
                    }
                }

                if (resultContext.ResultCode == ETEMEnums.ResultEnum.Success)
                {
                    resultContext = base.EntitySave<AverageOutturnOverTime>(entities, resultContext);
                }
            }

            return resultContext;
        }

        internal CallContext AverageOutturnOverTimeDelete(List<int> listSelectedIDs, CallContext resultContext)
        {
            try
            {
                resultContext.ResultCode = ETEMEnums.ResultEnum.Error;

                List<AverageOutturnOverTime> listAverageOutturnOverTime = new List<AverageOutturnOverTime>();

                listAverageOutturnOverTime = (from mpl in this.dbContext.AverageOutturnOverTimes
                                          where listSelectedIDs.Contains(mpl.idAverageOutturnOverTime)
                                          select mpl).ToList<AverageOutturnOverTime>();

                CallContext deleteContext = new CallContext();
                deleteContext = resultContext;

                deleteContext = base.EntityDelete<AverageOutturnOverTime>(listAverageOutturnOverTime, deleteContext);

                if (deleteContext.ResultCode == ETEMEnums.ResultEnum.Success)
                {
                    resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
                    resultContext.Message = "Selected rows `Average outturn over time` have been deleted successfully!";
                }
                else
                {
                    resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                    resultContext.Message = "Error delete selected rows `Average outturn over time`!";
                }
            }
            catch (Exception ex)
            {
                resultContext.Message = "Error delete selected rows `Average outturn over time`!";

                BaseHelper.Log("Error delete entities `AverageOutturnOverTime`, IDs - (" + string.Join(",", listSelectedIDs.ToArray()) + ")!");
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

            return resultContext;
        }
    }
}