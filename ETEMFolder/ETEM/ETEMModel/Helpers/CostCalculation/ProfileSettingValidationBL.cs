using ETEMModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers.CostCalculation
{
    public class ProfileSettingValidationBL : BaseClassBL<ProfileSettingValidation>
    {
        public ProfileSettingValidationBL()
        {
            this.EntitySetName = "ProfileSettingValidations";
        }

        internal override void EntityToEntity(ProfileSettingValidation sourceEntity, ProfileSettingValidation targetEntity)
        {
            targetEntity.idProfileSetting       = sourceEntity.idProfileSetting;
            targetEntity.ValidationRequirement  = sourceEntity.ValidationRequirement;            
        }

        internal override ProfileSettingValidation GetEntityById(int idEntity)
        {
            return this.dbContext.ProfileSettingValidations.Where(w => w.idProfileSettingValidation == idEntity).FirstOrDefault();
        }

        internal CallContext ProfileSettingValidationSave(ProfileSettingValidation profileSettingValidation, CallContext callContext)
        {
            callContext = this.EntitySave<ProfileSettingValidation>(profileSettingValidation, callContext);
            return callContext;
        }

        public CallContext ProfileSettingValidationRemove(List<ProfileSettingValidation> list, CallContext resultContext)
        {

            foreach (ProfileSettingValidation entity in list)
            {
                ProfileSettingValidation removeEntity = this.dbContext.ProfileSettingValidations.Where(z => z.idProfileSettingValidation == entity.idProfileSettingValidation).FirstOrDefault();
                
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

        public List<ProfileSettingValidation> GetProfileSettingValidationByIDProfile(int idProfileSetting)
        {
            List<ProfileSettingValidation> list = (from psv in this.dbContext.ProfileSettingValidations
                                                   where psv.idProfileSetting == idProfileSetting
                                                   select psv).ToList();

            return list;
        }

    }
}