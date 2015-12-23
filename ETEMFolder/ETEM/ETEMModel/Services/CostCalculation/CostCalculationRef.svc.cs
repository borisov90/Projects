using ETEMModel.Helpers;
using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Helpers.CostCalculation;
using ETEMModel.Models;
using ETEMModel.Models.DataView.CostCalculation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.UI.WebControls;

namespace ETEMModel.Services.CostCalculation
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CostCalculation" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CostCalculation.svc or CostCalculation.svc.cs at the Solution Explorer and start debugging.
    public class CostCalculationRef : ICostCalculation
    {
        #region DiePriceList
        public DiePriceList GetDiePriceListById(string entityID)
        {
            return new DiePriceListBL().GetEntityById(Int32.Parse(entityID));
        }

        public List<DiePriceListDataView> GetAllDiePriceList(ICollection<AbstractSearch> searchCriteria,
                                                             DateTime? dateActiveTo,
                                                             string sortExpression, string sortDirection)
        {
            return new DiePriceListBL().GetAllDiePriceList(searchCriteria, dateActiveTo, sortExpression, sortDirection);
        }

        public CallContext DiePriceListSave(List<DiePriceList> entities, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.DiePriceListSave;
            CallContext resContext = new DiePriceListBL().DiePriceListSave(entities, resultContext);

            return resContext;
        }

        public CallContext DiePriceListDelete(List<int> listSelectedIDs, CallContext resultContext)
        {
            return new DiePriceListBL().DiePriceListDelete(listSelectedIDs, resultContext);
        }

        public DiePriceList GetDiePriceListByVendorAndDateActiveTo(string vendorID, DateTime dateActiveTo)
        {
            return new DiePriceListBL().GetDiePriceListByVendorAndDateActiveTo(Int32.Parse(vendorID), dateActiveTo);
        }

        public CallContext ImportDiePriceListDetails(string fileFullName, string entityID, CallContext resultContext)
        {
            return new DiePriceListBL().ImportDiePriceListDetails(fileFullName, Int32.Parse(entityID), resultContext);
        }

        #endregion

        #region DiePriceListDetail
        public DiePriceListDetail GetDiePriceListDetailById(string entityID)
        {
            return new DiePriceListDetailBL().GetEntityById(Int32.Parse(entityID));
        }

        public List<DiePriceListDetailDataView> GetAllDiePriceListDetails(ICollection<AbstractSearch> searchCriteria,
                                                                          DateTime? dateActiveTo,
                                                                          string sortExpression, string sortDirection)
        {
            return new DiePriceListDetailBL().GetAllDiePriceListDetails(searchCriteria, dateActiveTo, sortExpression, sortDirection);
        }

        public CallContext DiePriceListDetailsSave(List<DiePriceListDetail> entities, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.DiePriceListDetailsSave;
            CallContext resContext = new DiePriceListDetailBL().EntitySave<DiePriceListDetail>(entities, resultContext);

            return resContext;
        }

        public CallContext DiePriceListDetailDelete(List<int> listSelectedIDs, CallContext resultContext)
        {
            return new DiePriceListDetailBL().DiePriceListDetailDelete(listSelectedIDs, resultContext);
        }
        #endregion

        #region MaterialPriceList
        public MaterialPriceList GetMaterialPriceListById(string entityID)
        {
            return new MaterialPriceListBL().GetEntityById(Int32.Parse(entityID));
        }

        public List<MaterialPriceListDataView> GetAllMaterialPriceList(ICollection<AbstractSearch> searchCriteria,
                                                                       DateTime? dateActiveTo,
                                                                       string sortExpression, string sortDirection)
        {
            return new MaterialPriceListBL().GetAllMaterialPriceList(searchCriteria, dateActiveTo, sortExpression, sortDirection);
        }

        public CallContext MaterialPriceListSave(List<MaterialPriceList> entities, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.MaterialPriceListSave;
            CallContext resContext = new MaterialPriceListBL().MaterialPriceListSave(entities, resultContext);

            return resContext;
        }

        public CallContext MaterialPriceListDelete(List<int> listSelectedIDs, CallContext resultContext)
        {
            return new MaterialPriceListBL().MaterialPriceListDelete(listSelectedIDs, resultContext);
        }
        public MaterialPriceListDataView GetActiveMaterialPriceList(DateTime? dateTime)
        {
            return  new MaterialPriceListBL().GetActiveMaterialPriceList(dateTime);
        }
        #endregion

        #region SAPData
        public SAPData GetSAPDataById(string entityID)
        {
            return new SAPDataBL().GetEntityById(Int32.Parse(entityID));
        }

        public List<SAPDataDataView> GetAllSAPData(ICollection<AbstractSearch> searchCriteria,
                                                   DateTime? dateActiveTo,
                                                   string sortExpression, string sortDirection)
        {
            return new SAPDataBL().GetAllSAPData(searchCriteria, dateActiveTo, sortExpression, sortDirection);
        }

        public CallContext SAPDataSave(List<SAPData> entities, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.SAPDataSave;
            CallContext resContext = new SAPDataBL().SAPDataSave(entities, resultContext);

            return resContext;
        }

        public CallContext SAPDataDelete(List<int> listSelectedIDs, CallContext resultContext)
        {
            return new SAPDataBL().SAPDataDelete(listSelectedIDs, resultContext);
        }

        public SAPData GetSAPDataByDateActiveTo(DateTime dateActiveTo)
        {
            return new SAPDataBL().GetSAPDataByDateActiveTo(dateActiveTo);
        }

        public Dictionary<string, TableRow[]> LoadSAPDataExpensesAndQuantities(string entityID, CallContext resultContext)
        {
            return new SAPDataBL().LoadSAPDataExpensesAndQuantities(Int32.Parse(entityID), resultContext);
        }

        public CallContext ImportSAPDataExpensesAndQuantities(string fileFullName, string entityID, CallContext resultContext)
        {
            return new SAPDataBL().ImportSAPDataExpensesAndQuantities(fileFullName, Int32.Parse(entityID), resultContext);
        }

        public CallContext CalculateCostCentersTotal(string entityID, CallContext resultContext)
        {
            return new ExpenseCalculationBL().CalculateCostCentersTotal(Int32.Parse(entityID), resultContext);
        }
        #endregion

        #region SAPDataExpense
        public SAPDataExpense GetSAPDataExpenseById(string entityID)
        {
            return new SAPDataExpensesBL().GetEntityById(Int32.Parse(entityID));
        }

        public List<SAPDataExpenseDataView> GetAllSAPDataExpense(ICollection<AbstractSearch> searchCriteria,
                                                                 DateTime? dateActiveTo,
                                                                 string sortExpression, string sortDirection)
        {
            return new SAPDataExpensesBL().GetAllSAPDataExpense(searchCriteria, dateActiveTo, sortExpression, sortDirection);
        }

        public CallContext SAPDataExpenseSave(List<SAPDataExpense> entities, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.SAPDataExpenseSave;
            CallContext resContext = new SAPDataExpensesBL().EntitySave<SAPDataExpense>(entities, resultContext);

            return resContext;
        }

        public CallContext SAPDataExpenseDelete(List<int> listSelectedIDs, CallContext resultContext)
        {
            return new SAPDataExpensesBL().SAPDataExpenseDelete(listSelectedIDs, resultContext);
        }
        #endregion

        #region SAPDataQuantity
        public SAPDataQuantity GetSAPDataQuantityById(string entityID)
        {
            return new SAPDataQuantityBL().GetEntityById(Int32.Parse(entityID));
        }

        public List<SAPDataQuantityDataView> GetAllSAPDataQuantity(ICollection<AbstractSearch> searchCriteria,
                                                                   DateTime? dateActiveTo,
                                                                   string sortExpression, string sortDirection)
        {
            return new SAPDataQuantityBL().GetAllSAPDataQuantity(searchCriteria, dateActiveTo, sortExpression, sortDirection);
        }

        public CallContext SAPDataQuantitySave(List<SAPDataQuantity> entities, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.SAPDataQuantitySave;
            CallContext resContext = new SAPDataQuantityBL().EntitySave<SAPDataQuantity>(entities, resultContext);

            return resContext;
        }

        public CallContext SAPDataQuantityDelete(List<int> listSelectedIDs, CallContext resultContext)
        {
            return new SAPDataQuantityBL().SAPDataQuantityDelete(listSelectedIDs, resultContext);
        }
        #endregion

        #region DieFormula

        public DieFormula GetDieFormulaById(string entityID)
        {
            return new DieFormulaBL().GetEntityById(Int32.Parse(entityID));
        }

        public List<DieFormulaDataView> GetAllDieFormula(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection)
        {
            return new DieFormulaBL().GetAllDieFormulaList(searchCriteria, sortExpression, sortDirection);
        }

        public CallContext DieFormulaSave(DieFormula entity, CallContext resultContext)
        {
            resultContext.securitySettings  = ETEMEnums.SecuritySettings.DieFormulaSave;
            CallContext resContext          = new DieFormulaBL().EntitySave<DieFormula>(entity, resultContext);

            return resContext;
        }

        public CallContext RemoveDieFormula(List<DieFormula> list, CallContext callContext)
        {
            callContext.securitySettings    = ETEMEnums.SecuritySettings.DieFormulaSave;
            callContext                     = new DieFormulaBL().RemoveDieFormula(list, callContext);
            return callContext;
        }


        public DieFormula GetDieFormulaParams(int idProfileCategory, int idProfileType, int idNumberOfCavities, CallContext callContext)
        {
            return new DieFormulaBL().GetDieFormulaParams(idProfileCategory, idProfileType, idNumberOfCavities, callContext);
        }

        #endregion

        #region ProfileSetting

        public ProfileSetting GetProfileSettingById(string entityID)
        {
            return new ProfileSettingBL().GetEntityById(Int32.Parse(entityID));
        }

        public List<ProfileSettingDataView> GetProfilesList(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection)
        {
            return new ProfileSettingBL().GetProfilesList(searchCriteria, sortExpression, sortDirection);
        }

        public CallContext ProfileSettingSave(ProfileSetting entity, CallContext resultContext)
        {
            resultContext.securitySettings  = ETEMEnums.SecuritySettings.ProfilesSave;
            CallContext resContext          = new ProfileSettingBL().EntitySave<ProfileSetting>(entity, resultContext);

            return resContext;
        }

        public CallContext RemoveProfileSetting(List<ProfileSetting> list, CallContext callContext)
        {
            callContext.securitySettings = ETEMEnums.SecuritySettings.ProfilesSave;
            callContext = new ProfileSettingBL().RemoveProfileSetting(list, callContext);
            return callContext;
        }

        #endregion

        #region ProfileSettingValidation

        public ProfileSettingValidation GetProfileSettingValidationById(string entityID)
        {
            return new ProfileSettingValidationBL().GetEntityById(Int32.Parse(entityID));
        }

        public List<ProfileSettingValidation> GetProfileSettingValidationByIDProfile(int idProfileSetting)
        {
            return new ProfileSettingValidationBL().GetProfileSettingValidationByIDProfile(idProfileSetting);
        }

        public CallContext ProfileSettingValidationSave(ProfileSettingValidation entity, CallContext resultContext)
        {
            resultContext.securitySettings  = ETEMEnums.SecuritySettings.ProfileSettingValidationSave;
            CallContext resContext          = new ProfileSettingValidationBL().EntitySave<ProfileSettingValidation>(entity, resultContext);

            return resContext;
        }

        public CallContext ProfileSettingValidationRemove(List<ProfileSettingValidation> list, CallContext callContext)
        {
            callContext.securitySettings    = ETEMEnums.SecuritySettings.ProfileSettingValidationSave;
            callContext                     = new ProfileSettingValidationBL().ProfileSettingValidationRemove(list, callContext);
            return callContext;
        }

        #endregion

        #region CommissionsByAgent
        public CommissionsByAgent GetCommissionsByAgentById(string entityID)
        {
            return new CommissionsByAgentBL().GetEntityById(Int32.Parse(entityID));
        }

        public List<CommissionsByAgentDataView> GetAllCommissionsByAgentsList(ICollection<AbstractSearch> searchCriteria,
                                                                              DateTime? dateActiveTo,
                                                                              string sortExpression, string sortDirection)
        {
            return new CommissionsByAgentBL().GetAllCommissionsByAgentsList(searchCriteria, dateActiveTo, sortExpression, sortDirection);
        }

        public CallContext CommissionsByAgentSave(List<CommissionsByAgent> entities, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.CommissionsByAgentSave;
            CallContext resContext = new CommissionsByAgentBL().CommissionsByAgentSave(entities, resultContext);

            return resContext;
        }

        public CallContext CommissionsByAgentDelete(List<int> listSelectedIDs, CallContext resultContext)
        {
            return new CommissionsByAgentBL().CommissionsByAgentDelete(listSelectedIDs, resultContext);
        }
        #endregion

        #region OfferDataExpenseGroupBL

        public List<OfferDataExpenseGroupView> GetAllOfferDataExpenseGroupByOffer(int idOffer)
        {
            return new OfferDataExpenseGroupBL().GetAllOfferDataExpenseGroupByOffer(idOffer);
        }

        public Dictionary<string, TableRow[]> LoadOfferDataCostCenterAndExpensesType(string entityID, ETEMEnums.CalculationType calculationType, CallContext resultContext)
        {
            return new OfferDataExpenseGroupBL().LoadOfferDataCostCenterAndExpensesType(Int32.Parse(entityID), calculationType, resultContext);
        }
        //public DataTable LoadOfferDataCostCenterAndExpensesType(string entityID, ETEMEnums.CalculationType calculationType, CallContext resultContext)
        //{
        //    return new OfferDataExpenseGroupBL().LoadOfferDataCostCenterAndExpensesType(Int32.Parse(entityID), calculationType, resultContext);
        //}

        #endregion

        #region ProductCost
        public ProductCostResult LoadProductCostsByOfferId(string offerID, CallContext resultContext)
        {

            return new ProductCostBL().LoadProductCostsByOfferId(int.Parse(offerID), resultContext);
        }

        public ProductCostResult LoadProductCostsInTableRowsByOfferId(string offerID, CallContext resultContext)
        {
            return new ProductCostBL().LoadProductCostsInTableRowsByOfferId(int.Parse(offerID), resultContext);
        }
        #endregion

        #region ProductivityAndScrap
        public ProductivityAndScrap GetProductivityAndScrapById(string entityID)
        {
            return new ProductivityAndScrapBL().GetEntityById(Int32.Parse(entityID));
        }

        public ProductivityAndScrap GetProductivityAndScrapByDateActiveTo(DateTime dateActiveTo)
        {
            return new ProductivityAndScrapBL().GetProductivityAndScrapByDateActiveTo(dateActiveTo);
        }

        public ProductivityAndScrapDataView GetProductivityAndScrapByDateActiveToWithAvgData(DateTime dateActiveTo)
        {
            return new ProductivityAndScrapBL().GetProductivityAndScrapByDateActiveToWithAvgData(dateActiveTo);
        }

        public List<ProductivityAndScrapDataView> GetAllProductivityAndScrapList(ICollection<AbstractSearch> searchCriteria,
                                                                                 DateTime? dateActiveTo,
                                                                                 string sortExpression, string sortDirection)
        {
            return new ProductivityAndScrapBL().GetAllProductivityAndScrapList(searchCriteria, dateActiveTo, sortExpression, sortDirection);
        }

        public CallContext ProductivityAndScrapSave(List<ProductivityAndScrap> entities, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.ProductivityAndScrapSave;
            CallContext resContext = new ProductivityAndScrapBL().ProductivityAndScrapSave(entities, resultContext);

            return resContext;
        }

        public CallContext ProductivityAndScrapDelete(List<int> listSelectedIDs, CallContext resultContext)
        {
            return new ProductivityAndScrapBL().ProductivityAndScrapDelete(listSelectedIDs, resultContext);
        }

        public CallContext ImportProductivityAndScrapCostData(string fileFullName, string entityID, CallContext resultContext)
        {
            return new ProductivityAndScrapBL().ImportProductivityAndScrapCostData(fileFullName, Int32.Parse(entityID), resultContext);
        }
        #endregion

        #region ProductivityAndScrapDetail
        public ProductivityAndScrapDetail GetProductivityAndScrapDetailById(string entityID)
        {
            return new ProductivityAndScrapDetailBL().GetEntityById(Int32.Parse(entityID));
        }

        public ProductivityAndScrapDetail GetProductivityAndScrapDetailByDateActiveToAndPressAndProfile(DateTime? dateActiveTo, int idCostCenterPress, int idProfile)
        {
            return new ProductivityAndScrapDetailBL().GetEntityByDateActiveToAndPressAndProfile(dateActiveTo, idCostCenterPress, idProfile);
        }

        public List<ProductivityAndScrapDetailDataView> GetAllProductivityAndScrapDetailList(ICollection<AbstractSearch> searchCriteria,
                                                                                             DateTime? dateActiveTo,
                                                                                             string sortExpression, string sortDirection)
        {
            return new ProductivityAndScrapDetailBL().GetAllProductivityAndScrapDetailList(searchCriteria, dateActiveTo, sortExpression, sortDirection);
        }

        public CallContext ProductivityAndScrapDetailSave(List<ProductivityAndScrapDetail> entities, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.ProductivityAndScrapSave;
            CallContext resContext = new ProductivityAndScrapDetailBL().EntitySave<ProductivityAndScrapDetail>(entities, resultContext);

            return resContext;
        }

        public CallContext ProductivityAndScrapDetailDelete(List<int> listSelectedIDs, CallContext resultContext)
        {
            return new ProductivityAndScrapDetailBL().ProductivityAndScrapDetailDelete(listSelectedIDs, resultContext);
        }
        #endregion

        public List<OfferDataView> GetAllOfferDataView(ICollection<AbstractSearch> searchCriteria, string GridViewSortExpression, string GridViewSortDirection)
        {
            return new OfferBL().GetAllOfferDataView(searchCriteria, GridViewSortExpression, GridViewSortDirection);
        }

        public Offer GetOfferByID(string idEntity)
        {

            if (string.IsNullOrEmpty(idEntity))
            {
                return null;
            }
            else
            {
                return new OfferBL().GetEntityById(int.Parse(idEntity));
            }
        }
       

        public CallContext OfferSave(Offer entity, CallContext callContext)
        {
            callContext.securitySettings = ETEMEnums.SecuritySettings.OfferSave;

            //CallContext resContext = new OfferBL().EntitySave<Offer>(entity, callContext);
            
            CallContext resContext = new OfferBL().OfferSave(entity, callContext);

            return resContext;
        }

        public OfferProducitivity GetOfferProducitivityByID(string idEntity)
        {
            if (string.IsNullOrEmpty(idEntity))
            {
                return null;
            }
            else
            {
               return new OfferProducitivityBL().GetEntityById(int.Parse(idEntity));
            }
            
        }

        public OfferProducitivity GetOfferProducitivityByOfferID(string idEntity)
        {

              if (string.IsNullOrEmpty(idEntity))
            {
                return null;
            }
            else
            {
                return new OfferProducitivityBL().GetOfferProducitivityByOfferID(int.Parse(idEntity));
            }

           
        }

        public void CreateExpenseGroupForOffer(int idOffer)
        {
            new ExpenseCalculationBL().CreateExpenseGroupForOffer(idOffer);
        }

         public List<Expense> GetListGroupExpense(Offer offer, CallContext callContext)
        {
            return  new ExpenseCalculationBL().GetListGroupExpense(offer, callContext);
        }



         public CallContext OfferDataExpenseGroupSave(List<OfferDataExpenseGroup> listOfferDataExpenseGroup, CallContext callContext)
         {
             return new OfferDataExpenseGroupBL().EntitySave<OfferDataExpenseGroup>(listOfferDataExpenseGroup, callContext);
         }

         public bool hasOfferDataExpenseByOffer(int idOffer)
         {
             return new OfferDataExpenseGroupBL().HasOfferDataExpenseByOffer(idOffer);
        
         }
         public CallContext OfferDataExpenseGroupDelete(int idOffer, CallContext resultContext)
         {
             return new OfferDataExpenseGroupBL().OfferDataExpenseGroupDelete(idOffer, resultContext);
         }


         public CallContext OfferProducitivitySave(OfferProducitivity offerProducitivity, CallContext callContext)
         {
            
            CallContext resContext = new OfferProducitivityBL().EntitySave<OfferProducitivity>(offerProducitivity, callContext);
            return resContext;
         }

         public CommissionsByAgent GetCommissionsByAgentByDateActiveTo(int idAgent, DateTime dateActiveTo)
         {
             return new CommissionsByAgentBL().GetCommissionsByAgentByDateActiveTo(idAgent, dateActiveTo);
         }



         public OfferOverviewResult LoadOfferOverviewResultInTableRowsByOfferId(string offerID, CallContext callContext)
         {
             return new OfferOverviewBL().LoadOfferOverviewResultInTableRowsByOfferId(int.Parse(offerID), callContext);
         }


        public List<AverageOutturnOverTimeDataView> GetAllAverageOutturnOverTimeDataView(ICollection<AbstractSearch> searchCriteria,
                                                                       DateTime? dateActiveTo,
                                                                       string sortExpression, string sortDirection)
        {
            return new AverageOutturnOverTimeBL().GetAllAverageOutturnOverTimeDataView(searchCriteria, dateActiveTo, sortExpression, sortDirection);
        }

        public AverageOutturnOverTimeDataView GetActiveAverageOutturnOverTime(DateTime dateTime)
        {
            return new AverageOutturnOverTimeBL().GetActiveAverageOutturnOverTime(dateTime);
        }

        public AverageOutturnOverTime GetAverageOutturnOverTimeById(string idAverageOutturnOverTime)
        {
            return new AverageOutturnOverTimeBL().GetEntityById(Int32.Parse(idAverageOutturnOverTime));
        }

        public CallContext AverageOutturnOverTimeSave(List<AverageOutturnOverTime> entities, CallContext resultContext)
        {
            resultContext.securitySettings  = ETEMEnums.SecuritySettings.AverageOutturnOverTimeListSave;
            CallContext resContext          = new AverageOutturnOverTimeBL().AverageOutturnOverTimeSave(entities, resultContext);

            return resContext;
        }

        public CallContext AverageOutturnOverTimeDelete(List<int> listSelectedIDsToDelete, CallContext callContext)
        {
             return new AverageOutturnOverTimeBL().AverageOutturnOverTimeDelete(listSelectedIDsToDelete, callContext);

           
        }

        public List<string> GetPressIntCodeListByDimension(Offer offer)
        {
            return new ExpenseCalculationBL().GetPressIntCodeListByDimension(offer);
        }
    }
}
