using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;
using System.Threading;


namespace ETEMModel.Helpers.Admin
{
    public class CronProcessHistoryBL : BaseClassBL<CronProcessHistory>
    {

        public CronProcessHistoryBL()
        {
            this.EntitySetName = "CronProcessHistories";
        }

        internal override CronProcessHistory GetEntityById(int idEntity)
        {
            return this.dbContext.CronProcessHistories.Where(e => e.idCronProcessHistory == idEntity).FirstOrDefault();
        }



        internal override void EntityToEntity(CronProcessHistory entity, CronProcessHistory saveEntity)
        {

            saveEntity.ExecuteDate = entity.ExecuteDate;
            saveEntity.RunTime = entity.RunTime;
            saveEntity.Successful = entity.Successful;
            saveEntity.Exception = entity.Exception;
            saveEntity.EndTime = entity.EndTime;
        }


        internal List<CronProcessHistory> GetAll(string sortExpression, string sortDirection)
        {
            List<CronProcessHistory> list = GetAllEntities<CronProcessHistory>();

            return list;

        }

        internal CronProcessHistory GetLastCronProcessHistory()
        {
            return this.dbContext.CronProcessHistories.OrderByDescending(e => e.idCronProcessHistory).FirstOrDefault();
        }




    }
}