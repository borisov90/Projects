using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Models;
using ETEMModel.Models.DataView.CostCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers.CostCalculation
{
    public class ProfileSettingBL : BaseClassBL<ProfileSetting>
    {
        public ProfileSettingBL()
        {
            this.EntitySetName = "ProfileSettings";
        }

        internal override void EntityToEntity(ProfileSetting sourceEntity, ProfileSetting targetEntity)
        {
            targetEntity.ProfileName            = sourceEntity.ProfileName;
            targetEntity.idProfileType          = sourceEntity.idProfileType;
            targetEntity.idProfileCategory      = sourceEntity.idProfileCategory;
            targetEntity.idProfileComplexity    = sourceEntity.idProfileComplexity;
            targetEntity.hasA                   = sourceEntity.hasA;
            targetEntity.hasB                   = sourceEntity.hasB;
            targetEntity.hasC                   = sourceEntity.hasC;
            targetEntity.hasD                   = sourceEntity.hasD;
            targetEntity.hasS                   = sourceEntity.hasS;
            targetEntity.DiameterFormula        = sourceEntity.DiameterFormula;
            targetEntity.idCreateUser           = sourceEntity.idCreateUser;
            targetEntity.dCreate                = sourceEntity.dCreate;
            targetEntity.idModifyUser           = sourceEntity.idModifyUser;
            targetEntity.dModify                = sourceEntity.dModify;
            targetEntity.ImagePath              = sourceEntity.ImagePath;
            targetEntity.ProfileNameSAP         = sourceEntity.ProfileNameSAP;
        }

        internal override ProfileSetting GetEntityById(int idEntity)
        {
            return this.dbContext.ProfileSettings.Where(w => w.idProfileSetting == idEntity).FirstOrDefault();
        }

        internal CallContext ProfileSettingSave(ProfileSetting profileSetting, CallContext callContext)
        {
            callContext = this.EntitySave<ProfileSetting>(profileSetting, callContext);
            return callContext;
        }

        public CallContext RemoveProfileSetting(List<ProfileSetting> list, CallContext resultContext)
        {

            foreach (ProfileSetting entity in list)
            {
                ProfileSetting removeEntity = this.dbContext.ProfileSettings.Where(a => a.idProfileSetting == entity.idProfileSetting).FirstOrDefault();

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

        internal List<ProfileSettingDataView> GetProfilesList(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection)
        {
            List<ProfileSettingDataView> listView = new List<ProfileSettingDataView>();

            listView = (from ps in this.dbContext.ProfileSettings
                        //Profile Complexity
                        join kvPrCom in this.dbContext.KeyValues on ps.idProfileComplexity equals kvPrCom.idKeyValue into grPrCom
                        from subPrCom in grPrCom.DefaultIfEmpty()
                        //Profile type
                        join kvPT in this.dbContext.KeyValues on ps.idProfileType equals kvPT.idKeyValue into grPT
                        from subPT in grPT.DefaultIfEmpty()
                        //Profile category
                        join kvPC in this.dbContext.KeyValues on ps.idProfileCategory equals kvPC.idKeyValue into grPC
                        from subPC in grPC.DefaultIfEmpty()

                        select new ProfileSettingDataView
                        {

                            idProfileSetting        = ps.idProfileSetting,
                            ProfileName             = ps.ProfileName ,
                            idProfileType           = ps.idProfileType,
                            idProfileCategory       = ps.idProfileCategory,
                            idProfileComplexity     = ps.idProfileComplexity,
                            hasA                    = ps.hasA,
                            hasB                    = ps.hasB,
                            hasC                    = ps.hasC,
                            hasD                    = ps.hasD,
                            hasS                    = ps.hasS,
                            DiameterFormula         = ps.DiameterFormula,
                            idCreateUser            = ps.idCreateUser,
                            dCreate                 = ps.dCreate,
                            idModifyUser            = ps.idModifyUser,
                            dModify                 = ps.dModify,
                            ImagePath               = ps.ImagePath,

                            //NumberOfCavitiesName    = (subNC != null ? subNC.Name : string.Empty),
                            ProfileCategoryName     = (subPC != null ? subPC.Name : string.Empty),
                            ProfileTypeName         = (subPT != null ? subPT.Name : string.Empty),
                            ProfileComplexityName   = (subPrCom != null ? subPrCom.Name : string.Empty),
                        }
                        ).ApplySearchCriterias(searchCriteria).OrderBy(z => z.ProfileTypeName).ThenBy(z => z.ProfileCategoryName).ThenBy(z => z.ProfileComplexityName).ToList<ProfileSettingDataView>();

            listView = OrderByHelper.OrderBy<ProfileSettingDataView>(listView, sortExpression, sortDirection).ToList<ProfileSettingDataView>();

            return listView;
        }

    }
}