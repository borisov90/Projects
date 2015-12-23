using ETEMModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers.CostCalculation
{
    public class OfferProducitivityBL : BaseClassBL<OfferProducitivity>
    {
        public OfferProducitivityBL()
        {
            this.EntitySetName = "OfferProducitivities";
        }

        internal override void EntityToEntity(OfferProducitivity sourceEntity, OfferProducitivity targetEntity)
        {
            targetEntity.idOffer = sourceEntity.idOffer;
            targetEntity.idPress = sourceEntity.idPress;
            targetEntity.PressProducitivity_KG_MH = sourceEntity.PressProducitivity_KG_MH;
            targetEntity.PressProducitivity_TON_MH = sourceEntity.PressProducitivity_TON_MH;
            targetEntity.QCProducitivity_KG_MH = sourceEntity.QCProducitivity_KG_MH;
            targetEntity.QCProducitivity_TON_MH = sourceEntity.QCProducitivity_TON_MH;
            targetEntity.idCOMetal = sourceEntity.idCOMetal;
            targetEntity.COMetalProducitivity_KG_MH = sourceEntity.COMetalProducitivity_KG_MH;
            targetEntity.COMetalProducitivity_TON_MH = sourceEntity.COMetalProducitivity_TON_MH;
            targetEntity.PackagingProducitivity_KG_MH = sourceEntity.PackagingProducitivity_KG_MH;
            targetEntity.PackagingProducitivity_TON_MH = sourceEntity.PackagingProducitivity_TON_MH;

            targetEntity.PackagingMachineHours= sourceEntity.PackagingMachineHours;
            targetEntity.PackagingProductionQuantity = sourceEntity.PackagingProductionQuantity;
        }

        internal override OfferProducitivity GetEntityById(int idEntity)
        {
            return this.dbContext.OfferProducitivities.Where(w => w.idOfferProducitivity == idEntity).FirstOrDefault();
        }

        internal OfferProducitivity GetOfferProducitivityByOfferID(int idOffer)
        {
                return this.dbContext.OfferProducitivities.FirstOrDefault(p => p.idOffer == idOffer);
        }

    }
}