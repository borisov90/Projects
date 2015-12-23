using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Models;
using ETEMModel.Models.DataView.CostCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers.CostCalculation
{
    public class DieFormulaBL : BaseClassBL<DieFormula>
    {

        public DieFormulaBL()
        {
            this.EntitySetName = "DieFormulas";
        }

        internal override void EntityToEntity(DieFormula sourceEntity, DieFormula targetEntity)
        {
            targetEntity.idNumberOfCavities = sourceEntity.idNumberOfCavities;
            targetEntity.idProfileType      = sourceEntity.idProfileType;
            targetEntity.idProfileCategory  = sourceEntity.idProfileCategory;
            targetEntity.DieFormulaText     = sourceEntity.DieFormulaText;
            targetEntity.idCreateUser       = sourceEntity.idCreateUser;
            targetEntity.dCreate            = sourceEntity.dCreate;
            targetEntity.idModifyUser       = sourceEntity.idModifyUser;
            targetEntity.dModify            = sourceEntity.dModify;
            targetEntity.ImagePath          = sourceEntity.ImagePath;
        }

        internal override DieFormula GetEntityById(int idEntity)
        {
            return this.dbContext.DieFormulas.Where(w => w.idDieFormula == idEntity).FirstOrDefault();
        }

        internal List<DieFormulaDataView> GetAllDieFormulaList(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection)
        {
            List<DieFormulaDataView> listView = new List<DieFormulaDataView>();

            listView = (from df in this.dbContext.DieFormulas
                        //Number of cavities
                        join kvNC in this.dbContext.KeyValues on df.idNumberOfCavities equals kvNC.idKeyValue into grNC
                        from subNC in grNC.DefaultIfEmpty()
                        //Profile type
                        join kvPT in this.dbContext.KeyValues on df.idProfileType equals kvPT.idKeyValue into grPT
                        from subPT in grPT.DefaultIfEmpty()
                        //Profile category
                        join kvPC in this.dbContext.KeyValues on df.idProfileCategory equals kvPC.idKeyValue into grPC
                        from subPC in grPC.DefaultIfEmpty()

                        select new DieFormulaDataView
                        {
                            idDieFormula            = df.idDieFormula,
                            idNumberOfCavities      = df.idNumberOfCavities,
                            idProfileType           = df.idProfileType,
                            idProfileCategory       = df.idProfileCategory,
                            DieFormulaText          = df.DieFormulaText,
                            idCreateUser            = df.idCreateUser,
                            dCreate                 = df.dCreate,
                            idModifyUser            = df.idModifyUser,
                            dModify                 = df.dModify,
                            ImagePath               = df.ImagePath,

                            NumberOfCavitiesName    = (subNC != null ? subNC.Name : string.Empty),
                            ProfileCategoryName     = (subPC != null ? subPC.Name : string.Empty),
                            ProfileTypeName         = (subPT != null ? subPT.Name : string.Empty),
                        }
                        ).ApplySearchCriterias(searchCriteria).OrderBy(z => z.NumberOfCavitiesName).ThenBy(z => z.ProfileTypeName).ThenBy(z => z.ProfileCategoryName).ToList<DieFormulaDataView>();

            listView = OrderByHelper.OrderBy<DieFormulaDataView>(listView, sortExpression, sortDirection).ToList<DieFormulaDataView>();

            return listView;
        }

        internal CallContext DieFormulaSave(DieFormula dieFormula, CallContext callContext)
        {
            callContext = this.EntitySave<DieFormula>(dieFormula, callContext);
            return callContext;
        }

        public CallContext RemoveDieFormula(List<DieFormula> list, CallContext resultContext)
        {

            foreach (DieFormula entity in list)
            {
                DieFormula removeEntity = this.dbContext.DieFormulas.Where(a => a.idDieFormula == entity.idDieFormula).FirstOrDefault();
                if (removeEntity != null)
                {
                    try
                    {
                        resultContext.ResultCode = ETEMEnums.ResultEnum.Error;

                        this.dbContext.DeleteObject(removeEntity);

                        resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
                    }
                    catch (ETEMModelException e)
                    {
                        BaseHelper.Log(e.Message);
                        BaseHelper.Log(e.StackTrace);
                    }
                }
            }

            this.dbContext.SaveChanges();
            return resultContext;

        }


        internal DieFormula GetDieFormulaParams(int idProfileCategory, int idProfileType, int idNumberOfCavities, CallContext callContext)
        {
            return this.dbContext.DieFormulas.FirstOrDefault(d =>
                                                                d.idProfileCategory     == idProfileCategory &&                
                                                                d.idProfileType         == idProfileType &&
                                                                d.idNumberOfCavities    == idNumberOfCavities);
        }
    }
}