using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Helpers;

namespace ETEMModel.Models
{
    public partial class Setting : Identifiable
    {

        public int EntityID { get { return this.idSetting; } }
        public string ValidationErrorsAsString { get; set; }
        public List<string> ValidateEntity(CallContext outputContext)
        {
            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;

            ETEMDataModelEntities dbContext = new ETEMDataModelEntities();

            ValidationErrorsAsString = string.Empty;
            List<string> result = new List<string>();

            var existEntity = dbContext.Settings.Where(e => e.SettingIntCode == this.SettingIntCode && e.idSetting != this.idSetting).FirstOrDefault();

            if (existEntity != null)
            {
                result.Add(BaseHelper.GetCaptionString("Entity_Setting_SettingIntCode_Exist"));
                outputContext.ResultCode = ETEMEnums.ResultEnum.Error;
            }


            if (this.SettingClass == ETEMEnums.AppSettingsClass.Date.ToString())
            {
                DateTime tmp;
                if (!DateTime.TryParse(this.SettingValue, out tmp))
                {
                    result.Add(BaseHelper.GetCaptionString("Entity_Setting_SettingValue_Date"));
                    outputContext.ResultCode = ETEMEnums.ResultEnum.Error;
                }
            }
            else if (this.SettingClass == ETEMEnums.AppSettingsClass.Double.ToString())
            {

                Double tmp;
                if (!Double.TryParse(this.SettingValue, out tmp))
                {
                    result.Add(BaseHelper.GetCaptionString("Entity_Setting_SettingValue_Double"));
                    outputContext.ResultCode = ETEMEnums.ResultEnum.Error;
                }

            }
            else if (this.SettingClass == ETEMEnums.AppSettingsClass.EMail.ToString())
            {
                //TODO:Да се прави проверка за e-mail
            }
            else if (this.SettingClass == ETEMEnums.AppSettingsClass.Integer.ToString())
            {
                Int32 tmp;
                if (!Int32.TryParse(this.SettingValue, out tmp))
                {
                    result.Add(BaseHelper.GetCaptionString("Entity_Setting_SettingValue_Int32"));
                    outputContext.ResultCode = ETEMEnums.ResultEnum.Error;
                }
            }
            else if (this.SettingClass == ETEMEnums.AppSettingsClass.List.ToString())
            {
                //TODO:Да се прави проверка за list
            }
            else if (this.SettingClass == ETEMEnums.AppSettingsClass.String.ToString())
            {
                //TODO:Да се прави проверка за стринг
            }

            ValidationErrorsAsString = string.Join(",", result.ToArray());


            outputContext.Message = this.ValidationErrorsAsString;
            outputContext.EntityID = this.EntityID.ToString();

            return result;
        }

    }
}