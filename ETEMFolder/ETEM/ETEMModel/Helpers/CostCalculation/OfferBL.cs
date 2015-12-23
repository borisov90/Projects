using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Helpers.Admin;
using ETEMModel.Models;
using ETEMModel.Models.DataView.CostCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers.CostCalculation
{
    public class OfferBL : BaseClassBL<Offer>
    {

        public OfferBL()
        {
            this.EntitySetName = "Offers";
        }

        internal override void EntityToEntity(Offer sourceEntity, Offer targetEntity)
        {
            
            targetEntity.InquiryNumber					    = sourceEntity.InquiryNumber;
            targetEntity.OfferDate                          = sourceEntity.OfferDate;
            targetEntity.Customer                           = sourceEntity.Customer;
            targetEntity.idProfileSetting                   = sourceEntity.idProfileSetting;
            targetEntity.ValueForA                          = sourceEntity.ValueForA;
            targetEntity.ValueForB                          = sourceEntity.ValueForB;
            targetEntity.ValueForC                          = sourceEntity.ValueForC;
            targetEntity.ValueForD                          = sourceEntity.ValueForD;
            targetEntity.ValueForS                          = sourceEntity.ValueForS;
            targetEntity.ValueForPin                        = sourceEntity.ValueForPin;
            targetEntity.ValueForPout                       = sourceEntity.ValueForPout;
            targetEntity.DieDifficultyIndex                 = sourceEntity.DieDifficultyIndex;
            targetEntity.SDI                                = sourceEntity.SDI;
            targetEntity.AverageOutturnOverTime             = sourceEntity.AverageOutturnOverTime;
            

            targetEntity.idNumberOfCavities                 = sourceEntity.idNumberOfCavities;
            targetEntity.idPress                            = sourceEntity.idPress;
            targetEntity.Diameter                           = sourceEntity.Diameter;
            targetEntity.DieDiameter                        = sourceEntity.DieDiameter;
            targetEntity.DieDimensions                      = sourceEntity.DieDimensions;
            targetEntity.DiePrice                           = sourceEntity.DiePrice;
            targetEntity.idDiePriceListDetail               = sourceEntity.idDiePriceListDetail;
            targetEntity.LengthOfFinalPC                    = sourceEntity.LengthOfFinalPC;
            targetEntity.WeightPerMeter                     = sourceEntity.WeightPerMeter;
            targetEntity.WeightPerPC                        = sourceEntity.WeightPerPC;
            targetEntity.idAging                            = sourceEntity.idAging;
            targetEntity.idHardness                         = sourceEntity.idHardness;
            targetEntity.idStandardPackaging                = sourceEntity.idStandardPackaging;

            targetEntity.idAgent                            = sourceEntity.idAgent;
            targetEntity.idCommision                        = sourceEntity.idCommision;
            targetEntity.CostOfDie                          = sourceEntity.CostOfDie;
            targetEntity.Lifespan                           = sourceEntity.Lifespan;
            targetEntity.idMaterialPriceList                = sourceEntity.idMaterialPriceList;
            targetEntity.LME                                = sourceEntity.LME;
            targetEntity.PREMIUM                            = sourceEntity.PREMIUM;
            targetEntity.ConsumptionRatio                   = sourceEntity.ConsumptionRatio;
            targetEntity.ScrapValuePercent                  = sourceEntity.ScrapValuePercent;
            targetEntity.CostOfScrap                        = sourceEntity.CostOfScrap;
            targetEntity.DaysOfCredit                       = sourceEntity.DaysOfCredit;
            targetEntity.Interest                           = sourceEntity.Interest;
            targetEntity.AdministrationExpenses             = sourceEntity.AdministrationExpenses;
            targetEntity.SalesExpenses                      = sourceEntity.SalesExpenses;
            targetEntity.FinancialFixedExpenses             = sourceEntity.FinancialFixedExpenses;
            targetEntity.FinancialVariableExpenses          = sourceEntity.FinancialVariableExpenses;
            targetEntity.GrossMargin                        = sourceEntity.GrossMargin;
            targetEntity.SavingsRate                        = sourceEntity.SavingsRate;
            
            targetEntity.TransportationCost                 = sourceEntity.TransportationCost;
            targetEntity.Commission                         = sourceEntity.Commission;
            targetEntity.PcsForTheWholeProject              = sourceEntity.PcsForTheWholeProject;
            targetEntity.Tonnage                            = sourceEntity.Tonnage;
            targetEntity.idCalculationCommission            = sourceEntity.idCalculationCommission;

            targetEntity.MaterialCostForPackaging           = sourceEntity.MaterialCostForPackaging;
            targetEntity.RatioConsumptionPackaging          = sourceEntity.RatioConsumptionPackaging;

            targetEntity.idCreateUser                       = sourceEntity.idCreateUser;
            targetEntity.dCreate                            = sourceEntity.dCreate;
            targetEntity.idModifyUser                       = sourceEntity.idModifyUser;
            targetEntity.dModify                            = sourceEntity.dModify;
            
            targetEntity.TotalExtrusionCost_EUR_TON         = sourceEntity.TotalExtrusionCost_EUR_TON;
            targetEntity.TotalPackagingCost_EUR_TON         = sourceEntity.TotalPackagingCost_EUR_TON;

            targetEntity.ActualTotalSalesPriceToCustomer    = sourceEntity.ActualTotalSalesPriceToCustomer;

            targetEntity.TargetPrice_EUR_TON                = sourceEntity.TargetPrice_EUR_TON;

            targetEntity.TotalSalesPrice                    = sourceEntity.TotalSalesPrice;
 



        }

        internal void EntityToEntityInputOnly(Offer sourceEntity, Offer targetEntity)
        {

            targetEntity.ValueForA = sourceEntity.ValueForA;
            targetEntity.ValueForB = sourceEntity.ValueForB;
            targetEntity.ValueForC = sourceEntity.ValueForC;
            targetEntity.ValueForD = sourceEntity.ValueForD;
            targetEntity.ValueForS = sourceEntity.ValueForS;
            targetEntity.LengthOfFinalPC = sourceEntity.LengthOfFinalPC;
            targetEntity.WeightPerMeter = sourceEntity.WeightPerMeter;
            targetEntity.WeightPerPC = sourceEntity.WeightPerPC;
            targetEntity.idAging = sourceEntity.idAging;
            targetEntity.idAgent = sourceEntity.idAgent;
            targetEntity.idCommision = sourceEntity.idCommision;
            
            
            targetEntity.LME = sourceEntity.LME;
            targetEntity.PREMIUM = sourceEntity.PREMIUM;
            targetEntity.ConsumptionRatio = sourceEntity.ConsumptionRatio;
            targetEntity.ScrapValuePercent = sourceEntity.ScrapValuePercent;
            targetEntity.CostOfScrap = sourceEntity.CostOfScrap;
            targetEntity.DaysOfCredit = sourceEntity.DaysOfCredit;
            targetEntity.Interest = sourceEntity.Interest;
            targetEntity.AdministrationExpenses = sourceEntity.AdministrationExpenses;
            targetEntity.SalesExpenses = sourceEntity.SalesExpenses;
            targetEntity.FinancialFixedExpenses = sourceEntity.FinancialFixedExpenses;
            targetEntity.FinancialVariableExpenses = sourceEntity.FinancialVariableExpenses;
            targetEntity.GrossMargin = sourceEntity.GrossMargin;
            targetEntity.TransportationCost = sourceEntity.TransportationCost;
            targetEntity.Commission = sourceEntity.Commission;
            targetEntity.ActualTotalSalesPriceToCustomer = sourceEntity.ActualTotalSalesPriceToCustomer;
            
            targetEntity.idModifyUser = sourceEntity.idModifyUser;
            targetEntity.dModify = sourceEntity.dModify;




        }
        internal override Offer GetEntityById(int idEntity)
        {
            Offer offer = this.dbContext.Offers.Where(w => w.idOffer == idEntity).FirstOrDefault();
            return offer;
        }
        

        internal List<OfferDataView> GetAllOfferDataView(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection)
        {
            List<OfferDataView> listView = (from o in dbContext.Offers
                                            join p in dbContext.ProfileSettings on o.idProfileSetting equals p.idProfileSetting 
                                            join u in dbContext.Users on o.idCreateUser equals u.idUser
                                            join person in dbContext.Persons on u.idPerson equals person.idPerson


                                            select new OfferDataView
                                              {
                                                  idOffer = o.idOffer,
                                                  idProfileSetting = o.idProfileSetting,
                                                  InquiryNumber = o.InquiryNumber,
                                                  Customer = o.Customer,
                                                  OfferDate = o.OfferDate,
                                                  TransportationCost = o.TransportationCost,
                                                  LME = o.LME,
                                                  PREMIUM = o.PREMIUM,                                                  
                                                  AdministrationExpenses = o.AdministrationExpenses,
                                                  SalesExpenses = o.SalesExpenses,
                                                  FinancialFixedExpenses = o.FinancialFixedExpenses,
                                                  FinancialVariableExpenses = o.FinancialVariableExpenses,
                                                  ProfileStr = p.ProfileName,
                                                  FirstName = person.FirstName,
                                                  SecondName = person.SecondName,
                                                  LastName = person.LastName,
                                                  TotalSalesPrice = o.TotalSalesPrice,
                                                  ActualTotalSalesPriceToCustomer = o.ActualTotalSalesPriceToCustomer,
                                                  TargetPrice_EUR_TON = o.TargetPrice_EUR_TON,
                                                  Tonnage = o.Tonnage,
                                              }
                                              ).ApplySearchCriterias(searchCriteria).ToList();

            listView = BaseClassBL<OfferDataView>.Sort(listView, sortExpression, sortDirection).ToList();
            return listView;
        }


        internal Offer Recalculation(Offer offer, CallContext callContext)
        {

            Offer offerFromDB = GetEntityById(offer.idOffer);           
            EntityToEntityInputOnly(offer, offerFromDB);









            callContext = new OfferBL().EntitySave<Offer>(offerFromDB, callContext);
            #region OfferProducitivity
            OfferProducitivity offerProducitivity = this.dbContext.OfferProducitivities.FirstOrDefault(o => o.idOffer == offer.idOffer);

            // TODO: Add code for recalculation of Producitivity
            offerProducitivity.PressProducitivity_TON_MH = offerProducitivity.PressProducitivity_TON_MH_Computable;
            offerProducitivity.QCProducitivity_TON_MH = offerProducitivity.QCProducitivity_TON_MH_Computable;
            offerProducitivity.COMetalProducitivity_TON_MH = offerProducitivity.COMetalProducitivity_TON_MH_Computable;
            offerProducitivity.PackagingProducitivity_TON_MH = offerProducitivity.PackagingProducitivity_TON_MH_Computable;
            callContext = new OfferProducitivityBL().EntitySave<OfferProducitivity>(offerProducitivity, callContext); 
            #endregion


            
            
            return offer;
        }


        internal CallContext OfferSave(Offer entity, CallContext callContext)
        {
            CallContext resContext = new OfferBL().EntitySave<Offer>(entity, callContext);

            if (resContext.ResultCode == ETEMEnums.ResultEnum.Success && entity.InquiryNumber.Trim() == string.Empty)
            {
                entity.InquiryNumber = new SequenceBL().GetInquiryNumber();

                new OfferBL().EntitySave<Offer>(entity, callContext, new List<string>() { "InquiryNumber" });
            }


            

            return resContext;
        }
    }
}