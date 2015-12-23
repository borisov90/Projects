using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;
using ETEMModel.Helpers.Admin;

namespace ETEMModel.Helpers.Settings
{
    public class BaseSettingHelper
    {
        protected ETEMDataModelEntities dbContext;

        public BaseSettingHelper()
        {
            dbContext = new ETEMDataModelEntities();
        }
        public void MergeSettingsAll()
        {
            try
            {

                CallContext resultContext = new CallContext();

                PermittedActionBL permittedActionBL = new PermittedActionBL();
                resultContext.securitySettings = ETEMEnums.SecuritySettings.PermittedActionMergeSettings;
                permittedActionBL.MergeSettings(resultContext);

                SettingBL SettingBL = new SettingBL();
                resultContext.securitySettings = ETEMEnums.SecuritySettings.SettingMergeSettings;
                SettingBL.MergeSettings(resultContext);
            }

            catch (ETEMModelException e)
            {
                BaseHelper.Log(e.Message);
            }

        }
    }
}