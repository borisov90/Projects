using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;
using ETEMModel.Helpers.AbstractSearchBLHolder;

namespace ETEMModel.Helpers.Admin
{
    public class SettingBL : BaseClassBL<Setting>
    {
        public List<Setting> ListAppSetting;


        public SettingBL()
        {
            this.EntitySetName = "Settings";
        }

        public SettingBL(bool withListAppSetting)
        {
            this.EntitySetName = "Settings";
            if (withListAppSetting)
            {
                LoadAppSetting();
            }
        }

        internal override Setting GetEntityById(int idEntity)
        {
            return this.dbContext.Settings.Where(e => e.idSetting == idEntity).FirstOrDefault();
        }

        internal override void EntityToEntity(Setting sourceEntity, Setting targetEntity)
        {

            targetEntity.SettingName = sourceEntity.SettingName;
            targetEntity.SettingDescription = sourceEntity.SettingDescription;
            targetEntity.SettingIntCode = sourceEntity.SettingIntCode;
            targetEntity.SettingValue = sourceEntity.SettingValue;
            targetEntity.SettingDefaultValue = sourceEntity.SettingDefaultValue;
            targetEntity.SettingClass = sourceEntity.SettingClass;

        }


        public CallContext MergeSettings(CallContext inputContext)
        {
            CallContext outputContext = new CallContext();
            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;

            if (!HasUserActionPermission(null, outputContext, inputContext))
            {
                return outputContext;
            }

            LoadAppSetting();

            List<Setting> listActionDB = this.dbContext.Settings.ToList();

            List<Setting> listSettingToBeSave = new List<Setting>();

            foreach (Setting apToBeSave in this.ListAppSetting)
            {

                if (listActionDB.Where(
                    apDB => apDB.SettingIntCode == apToBeSave.SettingIntCode.ToString()).Count() == 0)
                {

                    listSettingToBeSave.Add(new Setting()
                    {
                        SettingName = apToBeSave.SettingName,
                        SettingDescription = apToBeSave.SettingDescription,
                        SettingIntCode = apToBeSave.SettingIntCode,
                        SettingValue = apToBeSave.SettingValue,
                        SettingDefaultValue = apToBeSave.SettingDefaultValue,
                        SettingClass = apToBeSave.SettingClass
                    }
                    );
                }

            }

            CallContext resultContext = new CallContext();
            resultContext.securitySettings = ETEMEnums.SecuritySettings.SettingSave;
            resultContext.CurrentConsumerID = new SettingBL().GetSettingByCode(ETEMEnums.AppSettings.UserIDBindWithSystem.ToString()).SettingValue;

            foreach (Setting entity in listSettingToBeSave)
            {
                resultContext = new SettingBL().EntitySave<Setting>(entity, resultContext);
            }

            return outputContext;

        }

        private void LoadAppSetting()
        {
            this.ListAppSetting = new List<Setting>();


            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Автоматичен вход в системата при DEBUG",
                SettingDescription = "Автоматичен вход в системата при DEBUG",
                SettingIntCode = ETEMEnums.AppSettings.AutomaticDebugLogin.ToString(),
                SettingValue = "true",
                SettingDefaultValue = "true",
                SettingClass = ETEMEnums.AppSettingsClass.Boolean.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Сесия в системата",
                SettingDescription = "Сесия в системата",
                SettingIntCode = ETEMEnums.AppSettings.WebSessionTimeOut.ToString(),
                SettingValue = "60",
                SettingDefaultValue = "60",
                SettingClass = ETEMEnums.AppSettingsClass.Integer.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Наименование на системата",
                SettingDescription = "Наименование на системата",
                SettingIntCode = ETEMEnums.AppSettings.WebApplicationName.ToString(),
                SettingValue = "ETEM",
                SettingDefaultValue = "ETEM",
                SettingClass = ETEMEnums.AppSettingsClass.String.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Стартиране на фонов процес",
                SettingDescription = "При стартиране на системата автоматично да се старират всички фонови процеси",
                SettingIntCode = ETEMEnums.AppSettings.CronProcessStart.ToString(),
                SettingValue = "no",
                SettingDefaultValue = "yes|no",
                SettingClass = ETEMEnums.AppSettingsClass.String.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Период на циклично стартиране на процесите",
                SettingDescription = "Период на циклично стартиране на процесите в минути. Процесите ще се старират отново след този период.",
                SettingIntCode = ETEMEnums.AppSettings.CronProcessStartPeriod.ToString(),
                SettingValue = "60",
                SettingDefaultValue = "60",
                SettingClass = ETEMEnums.AppSettingsClass.Integer.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "ID на потребител асоциран със системата",
                SettingDescription = "ID на потребител асоциран със системата. Висчки автоматични действия ще се изпълняват под това ID",
                SettingIntCode = ETEMEnums.AppSettings.UserIDBindWithSystem.ToString(),
                SettingValue = "1",
                SettingDefaultValue = "1",
                SettingClass = ETEMEnums.AppSettingsClass.Integer.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                //this one is changed in the app
                SettingName = "E-mail адрес, на който се получават грешки насъпили в системата",
                SettingDescription = "E-mail адрес, на който се получават грешки насъпили в системата",
                SettingIntCode = ETEMEnums.AppSettings.EmailForReciveError.ToString(),
                SettingValue = "ums@nha.bg",
                SettingDefaultValue = "ums@nha.bg",
                SettingClass = ETEMEnums.AppSettingsClass.EMail.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                //this one is changed in the app
                SettingName = "E-mail адрес, от който се изпращат ел. съобщеня",
                SettingDescription = "E-mail адрес, от който се изпращат ел. съобщеня",
                SettingIntCode = ETEMEnums.AppSettings.EmailForSending.ToString(),
                SettingValue = "support@nha.bg",
                SettingDefaultValue = "support@nha.bg",
                SettingClass = ETEMEnums.AppSettingsClass.EMail.ToString()
            });

            //------ TODO
            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Парола за новия e-mail за изпращане от e-mail сървър",
                SettingDescription = "Парола за новия mail за изпращане от e-mail сървър",
                SettingIntCode = ETEMEnums.AppSettings.MailFromPasswordNew.ToString(),
                SettingValue = "ams150105",
                SettingDefaultValue = "ams150105",
                SettingClass = ETEMEnums.AppSettingsClass.EMail.ToString()
            });
            //------

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Минимална дължина на паролата",
                SettingDescription = "Минимален брой символи, които трябва да съдържа паролата",
                SettingIntCode = ETEMEnums.AppSettings.PasswordMinLength.ToString(),
                SettingValue = "3",
                SettingDefaultValue = "5",
                SettingClass = ETEMEnums.AppSettingsClass.Integer.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Брой на редовете в списък",
                SettingDescription = "Определя броя на редовете в списъците",
                SettingIntCode = ETEMEnums.AppSettings.PageSize.ToString(),
                SettingValue = "10",
                SettingDefaultValue = "10",
                SettingClass = ETEMEnums.AppSettingsClass.Integer.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Наименование на папката за ресурси",
                SettingDescription = "Наименование на папката за ресурси",
                SettingIntCode = ETEMEnums.AppSettings.ResourcesFolderName.ToString(),
                SettingValue = "C:\\Resources_ETEM",
                SettingDefaultValue = "C:\\Resources_ETEM",
                SettingClass = ETEMEnums.AppSettingsClass.String.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Наименование на Web Virtual Folder за ресурси",
                SettingDescription = "Наименование на Web Virtual Folder за ресурси",
                SettingIntCode = ETEMEnums.AppSettings.WebResourcesFolderName.ToString(),
                SettingValue = "/Resources_ETEM",
                SettingDefaultValue = "/Resources_ETEM",
                SettingClass = ETEMEnums.AppSettingsClass.String.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Папка с шаблони за документи",
                SettingDescription = "Папка с шаблони за документи",
                SettingIntCode = ETEMEnums.AppSettings.FolderTemplates.ToString(),
                SettingValue = "Templates",
                SettingDefaultValue = "Templates",
                SettingClass = ETEMEnums.AppSettingsClass.String.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Наименование на e-mail сървър",
                SettingDescription = "Наименование на e-mail сървър",
                SettingIntCode = ETEMEnums.AppSettings.MailServer.ToString(),
                SettingValue = "smtp.gmail.com",
                SettingDefaultValue = "smtp.gmail.com",
                SettingClass = ETEMEnums.AppSettingsClass.String.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Позволено изпращане на e-mail",
                SettingDescription = "Позволено изпращане на e-mail",
                SettingIntCode = ETEMEnums.AppSettings.SendExternalMail.ToString(),
                SettingValue = "true",
                SettingDefaultValue = "true",
                SettingClass = ETEMEnums.AppSettingsClass.Boolean.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Порт на e-mail сървър",
                SettingDescription = "Порт на e-mail сървър",
                SettingIntCode = ETEMEnums.AppSettings.MailServerPort.ToString(),
                SettingValue = "587",
                SettingDefaultValue = "587",
                SettingClass = ETEMEnums.AppSettingsClass.Integer.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Парола за изпращане от e-mail сървър",
                SettingDescription = "Парола за изпращане от e-mail сървър",
                SettingIntCode = ETEMEnums.AppSettings.MailFromPassword.ToString(),
                SettingValue = "ums100714",
                SettingDefaultValue = "ums100714",
                SettingClass = ETEMEnums.AppSettingsClass.String.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "E-mail на ETEM по подразбиране",
                SettingDescription = "E-mail на ETEM по подразбиране",
                SettingIntCode = ETEMEnums.AppSettings.DefaultEmail.ToString(),
                SettingValue = "ums@nha.bg",
                SettingDefaultValue = "ums@nha.bg",
                SettingClass = ETEMEnums.AppSettingsClass.String.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Наименование на Pop3 e-mail сървър",
                SettingDescription = "Наименование Pop3 на e-mail сървър",
                SettingIntCode = ETEMEnums.AppSettings.MailServerPop3.ToString(),
                SettingValue = "pop.gmail.com",
                SettingDefaultValue = "pop.gmail.com",
                SettingClass = ETEMEnums.AppSettingsClass.String.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Порт на Pop3 e-mail сървър",
                SettingDescription = "Порт на Pop3 e-mail сървър",
                SettingIntCode = ETEMEnums.AppSettings.MailServerPop3Port.ToString(),
                SettingValue = "995",
                SettingDefaultValue = "995",
                SettingClass = ETEMEnums.AppSettingsClass.Integer.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Наименование на e-mail за грешки при изпращане поща от e-mail сървър",
                SettingDescription = "Наименование на e-mail за грешки при изпращане поща от e-mail сървър",
                SettingIntCode = ETEMEnums.AppSettings.MailDeliverySubsystemEmail.ToString(),
                SettingValue = "mailer-daemon@googlemail.com",
                SettingDefaultValue = "mailer-daemon@googlemail.com",
                SettingClass = ETEMEnums.AppSettingsClass.String.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Време за изчакване на проверка за неуспешно изпратени email-и от e-mail сървър",
                SettingDescription = "Време за изчакване на проверка за неуспешно изпратени email-и от e-mail сървър",
                SettingIntCode = ETEMEnums.AppSettings.WaitCheckMailDeliveryInMinutes.ToString(),
                SettingValue = "2",
                SettingDefaultValue = "2",
                SettingClass = ETEMEnums.AppSettingsClass.Integer.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Име на домайн",
                SettingDescription = "Име на домайн",
                SettingIntCode = ETEMEnums.AppSettings.DomainName.ToString(),
                SettingValue = "nhabg",
                SettingDefaultValue = "nhabg",
                SettingClass = ETEMEnums.AppSettingsClass.String.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Създаване на подробен лог в базата данни(YES|NO)",
                SettingDescription = "Създаване на подробен лог в базата данни(YES|NO)",
                SettingIntCode = ETEMEnums.AppSettings.MakeLogInDB.ToString(),
                SettingValue = "YES",
                SettingDefaultValue = "YES",
                SettingClass = ETEMEnums.AppSettingsClass.String.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Template to import die price list details",
                SettingDescription = "Template to import die price list details",
                SettingIntCode = ETEMEnums.AppSettings.TemplateDiePriceListDetails.ToString(),
                SettingValue = "Template_DiePriceListDetails.xlsx",
                SettingDefaultValue = "Template_DiePriceListDetails.xlsx",
                SettingClass = ETEMEnums.AppSettingsClass.String.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Template to import SAP data expenses and quantities",
                SettingDescription = "Template to import SAP data expenses and quantities",
                SettingIntCode = ETEMEnums.AppSettings.TemplateSAPExpensesAndQuantities.ToString(),
                SettingValue = "Template_SAP_ExpensesAndQuantities.xlsx",
                SettingDefaultValue = "Template_SAP_ExpensesAndQuantities.xlsx",
                SettingClass = ETEMEnums.AppSettingsClass.String.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Template to import SAP data productivity and scrap",
                SettingDescription = "Template to import SAP data productivity and scrap",
                SettingIntCode = ETEMEnums.AppSettings.Template_SAP_ProductivityAndScrap.ToString(),
                SettingValue = "Template_SAP_ProductivityAndScrap.xlsx",
                SettingDefaultValue = "Template_SAP_ProductivityAndScrap.xlsx",
                SettingClass = ETEMEnums.AppSettingsClass.String.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Consumption ratio. Depends on the selected profile",
                SettingDescription = "Consumption ratio. Depends on the selected profile. Don't use this setting when there is formula for consumption ratio. ",
                SettingIntCode = ETEMEnums.AppSettings.ConsumptionRatio.ToString(),
                SettingValue = "1,2",
                SettingDefaultValue = "1,2",
                SettingClass = ETEMEnums.AppSettingsClass.Double.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Scrap value (%LME)",
                SettingDescription = "Scrap value (%LME)",
                SettingIntCode = ETEMEnums.AppSettings.ScrapValuePercent.ToString(),
                SettingValue = "0,9",
                SettingDefaultValue = "0,9",
                SettingClass = ETEMEnums.AppSettingsClass.Double.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "PackagingProducitivity (KG/MH)",
                SettingDescription = "PackagingProducitivity (KG/MH)",
                SettingIntCode = ETEMEnums.AppSettings.PackagingProducitivity_KG_MH.ToString(),
                SettingValue = "1397",
                SettingDefaultValue = "1397",
                SettingClass = ETEMEnums.AppSettingsClass.Double.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "PressProducitivity (KG/MH)",
                SettingDescription = "PressProducitivity (KG/MH)",
                SettingIntCode = ETEMEnums.AppSettings.PressProducitivity_KG_MH.ToString(),
                SettingValue = "1800",
                SettingDefaultValue = "1800",
                SettingClass = ETEMEnums.AppSettingsClass.Double.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Administration expenses",
                SettingDescription = "Administration expenses",
                SettingIntCode = ETEMEnums.AppSettings.Administration_expenses.ToString(),
                SettingValue = "2",
                SettingDefaultValue = "2",
                SettingClass = ETEMEnums.AppSettingsClass.Double.ToString()
            });
            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Sales expenses",
                SettingDescription = "Sales expenses",
                SettingIntCode = ETEMEnums.AppSettings.Sales_expenses.ToString(),
                SettingValue = "15",
                SettingDefaultValue = "15",
                SettingClass = ETEMEnums.AppSettingsClass.Double.ToString()
            });
            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Financial fixed expenses",
                SettingDescription = "Financial fixed expenses",
                SettingIntCode = ETEMEnums.AppSettings.Financial_fixed_expenses.ToString(),
                SettingValue = "18",
                SettingDefaultValue = "18",
                SettingClass = ETEMEnums.AppSettingsClass.Double.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Interest (%)",
                SettingDescription = "Interest (%)",
                SettingIntCode = ETEMEnums.AppSettings.Interest.ToString(),
                SettingValue = "8",
                SettingDefaultValue = "8",
                SettingClass = ETEMEnums.AppSettingsClass.Double.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Material cost for packaging",
                SettingDescription = "Material cost for packaging",
                SettingIntCode = ETEMEnums.AppSettings.Material_cost_for_packaging.ToString(),
                SettingValue = "12",
                SettingDefaultValue = "12",
                SettingClass = ETEMEnums.AppSettingsClass.Double.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Ratio consumption packaging",
                SettingDescription = "Ratio consumption packaging",
                SettingIntCode = ETEMEnums.AppSettings.Ratio_consumption_packaging.ToString(),
                SettingValue = "1,02",
                SettingDefaultValue = "1,02",
                SettingClass = ETEMEnums.AppSettingsClass.Double.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "SavingsRate",
                SettingDescription = "SavingsRate",
                SettingIntCode = ETEMEnums.AppSettings.SavingsRate.ToString(),
                SettingValue = "0",
                SettingDefaultValue = "0",
                SettingClass = ETEMEnums.AppSettingsClass.Double.ToString()
            });

            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Length of final PC (mm) - MIN",
                SettingDescription = "Length of final PC (mm) - MIN",
                SettingIntCode = ETEMEnums.AppSettings.LengthOfFinalPC_MIN.ToString(),
                SettingValue = "3000",
                SettingDefaultValue = "3000",
                SettingClass = ETEMEnums.AppSettingsClass.Integer.ToString()
            });
            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Length of final PC (mm) - MAX",
                SettingDescription = "Length of final PC (mm) - MAX",
                SettingIntCode = ETEMEnums.AppSettings.LengthOfFinalPC_MAX.ToString(),
                SettingValue = "7000",
                SettingDefaultValue = "7000",
                SettingClass = ETEMEnums.AppSettingsClass.Integer.ToString()
            });


            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "s (mm) - Tickness - MIN",
                SettingDescription = "s (mm) - Tickness - MIN",
                SettingIntCode = ETEMEnums.AppSettings.Tickness_MIN.ToString(),
                SettingValue = "1,3",
                SettingDefaultValue = "1,3",
                SettingClass = ETEMEnums.AppSettingsClass.Double.ToString()
            });
            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "s (mm) - Tickness - MAX",
                SettingDescription = "s (mm) - Tickness - MAX",
                SettingIntCode = ETEMEnums.AppSettings.Tickness_MAX.ToString(),
                SettingValue = "6",
                SettingDefaultValue = "6",
                SettingClass = ETEMEnums.AppSettingsClass.Double.ToString()
            });


            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Weight per meter (gr/m) - MIN",
                SettingDescription = "Weight per meter (gr/m) - MIN",
                SettingIntCode = ETEMEnums.AppSettings.Weight_Per_Meter_MIN.ToString(),
                SettingValue = "185",
                SettingDefaultValue = "185",
                SettingClass = ETEMEnums.AppSettingsClass.Integer.ToString()
            });
            this.ListAppSetting.Add(new Setting()
            {
                SettingName = "Weight per meter (gr/m) - MAX",
                SettingDescription = "Weight per meter (gr/m) - MAX",
                SettingIntCode = ETEMEnums.AppSettings.Weight_Per_Meter_MAX.ToString(),
                SettingValue = "5800",
                SettingDefaultValue = "5800",
                SettingClass = ETEMEnums.AppSettingsClass.Integer.ToString()
            });


        }

        internal List<Setting> GetAllSettings(string sortExpression, string sortDirection, List<AbstractSearch> searchCriterias)
        {
            List<Setting> list = dbContext.Settings.ApplySearchCriterias<Setting>(searchCriterias).ToList();

            list = BaseClassBL<Setting>.Sort(list, sortExpression, sortDirection).ToList();

            return list;
        }

        public Setting GetSettingByCode(string settingIntCode)
        {
            return this.dbContext.Settings.Where(e => e.SettingIntCode == settingIntCode).FirstOrDefault();
        }

        internal Setting GetSettingBySettingID(string _entityID)
        {
            int entityID;
            if (Int32.TryParse(_entityID, out entityID))
            {
                return this.dbContext.Settings.Where(e => e.idSetting == entityID).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
    }
}