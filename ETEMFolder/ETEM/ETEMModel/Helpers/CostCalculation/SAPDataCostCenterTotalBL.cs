using ETEMModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers.CostCalculation
{
    public class SAPDataCostCenterTotalBL : BaseClassBL<SAPDataCostCenterTotal>
    {
        public SAPDataCostCenterTotalBL()
        {
            this.EntitySetName = "SAPDataCostCenterTotals";
        }

        internal override void EntityToEntity(SAPDataCostCenterTotal sourceEntity, SAPDataCostCenterTotal targetEntity)
        {
            targetEntity.idSAPData = sourceEntity.idSAPData;
            targetEntity.idCostCenter = sourceEntity.idCostCenter;
            targetEntity.Total_MH = sourceEntity.Total_MH;
        }

        internal override SAPDataCostCenterTotal GetEntityById(int idEntity)
        {
            return this.dbContext.SAPDataCostCenterTotals.Where(w => w.idSAPDataCostCenterTotal == idEntity).FirstOrDefault();
        }
    }
}