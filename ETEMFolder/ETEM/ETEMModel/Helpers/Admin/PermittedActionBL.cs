using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;
using ETEMModel.Helpers.Settings;
using ETEMModel.Models.DataView;
using ETEMModel.Helpers.AbstractSearchBLHolder;

namespace ETEMModel.Helpers.Admin
{
    public class PermittedActionBL : BaseClassBL<PermittedAction>
    {
        public List<PermittedActionSetting> ListPermittedActionSetting;

        public PermittedActionBL()
        {
            this.EntitySetName = "PermittedActions";
        }

        internal override PermittedAction GetEntityById(int idEntity)
        {
            return this.dbContext.PermittedActions.Where(e => e.idPermittedAction == idEntity).FirstOrDefault();
        }

        internal override void EntityToEntity(PermittedAction entity, PermittedAction saveEntity)
        {

            saveEntity.GroupSecuritySetting = entity.GroupSecuritySetting;
            saveEntity.SecuritySetting = entity.SecuritySetting;
            saveEntity.Description = entity.Description;
            saveEntity.FrendlyName = entity.FrendlyName;
            saveEntity.idModule = entity.idModule;

        }

        public List<PermittedActionDataView> GetAllPermittedActionDataView(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection)
        {
            List<PermittedActionDataView> list = new List<PermittedActionDataView>();

            list = (from p in this.dbContext.PermittedActions
                    join m in this.dbContext.Modules on p.idModule equals m.idModule into grModule
                    from module in grModule.DefaultIfEmpty()
                    select new PermittedActionDataView
                    {
                        idPermittedAction = p.idPermittedAction,
                        idModule = p.idModule,
                        ModuleName = (module != null) ? module.ModuleName : string.Empty,
                        GroupSecuritySetting = p.GroupSecuritySetting,
                        SecuritySetting = p.SecuritySetting,
                        FrendlyName = p.FrendlyName,
                        Description = p.Description

                    }).ApplySearchCriterias(searchCriteria).ToList();

            list = BaseClassBL<PermittedActionDataView>.Sort(list, sortExpression, sortDirection).ToList();
            return list;


        }


        public List<PermittedAction> GetAll(string sortExpression, string sortDirection)
        {
            List<PermittedAction> list = GetAllEntities<PermittedAction>();
            list = BaseClassBL<PermittedAction>.Sort(list, sortExpression, sortDirection).ToList();
            return list;
        }


        private void LoadPermittedAction()
        {
            this.ListPermittedActionSetting = new List<PermittedActionSetting>();

            #region glabal actions


            #endregion
           
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.PersonSave,
                FrendlyName = "Запис на лице",
                Description = "Разрешава запис на лице"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.SettingSave,
                FrendlyName = "Запис на настройки",
                Description = "Разрешава запис на настройки"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.UserSave,
                FrendlyName = "Запис на потребител",
                Description = "Разрешава запис на потребител"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.LoginAS,
                FrendlyName = "Вход като друг потребител",
                Description = "Вход като друг потребител"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.KeyTypeSave,
                FrendlyName = "Запис на типова номенклатура",
                Description = "Разрешава запис на типова номенклатура"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.KeyValueSave,
                FrendlyName = "Запис на стойност на номенклатура",
                Description = "Разрешава запис на стойност на номенклатура"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.PermittedActionMergeSettings,
                FrendlyName = "Прехвърляне на позволените действия в базата данни",
                Description = "Стартира прехвърляне на позволените действия заложени в кода в базата данни"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.PermittedActionSave,
                FrendlyName = "Запис на позволените действия",
                Description = "Запис на позволените действия"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.RoleSave,
                FrendlyName = "Запис на роля",
                Description = "Разрешава запис на роля"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.RemoveUserRole,
                FrendlyName = "Премахва роля от потребител",
                Description = "Премахва роля от потребител"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.AddUserRole,
                FrendlyName = "Добавяне на роля на потребител",
                Description = "Добавяне на роля на потребител"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.UserListShow,
                FrendlyName = "Преглед на данните за потребители в системата",
                Description = "Преглед на данните за потребители в системата"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.RoleShowList,
                FrendlyName = "Преглед на данните за ролите",
                Description = "Преглед на данните за ролите"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.RoleEditView,
                FrendlyName = "Преглед на данните за дадена роля",
                Description = "Преглед на данните за дадена роля"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.SettingMergeSettings,
                FrendlyName = "Прехвърляне на настройките в базата данни",
                Description = "Стартира прехвърляне на настройките заложени в кода в базата данни"
            });
            
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.RolesMenuSave,
                FrendlyName = "Запис на елемент от менюто отнасящ се за дадена роля",
                Description = "Запис на елемент от менюто отнасящ се за дадена роля"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.NavUrlSave,
                FrendlyName = "Запис на URL към даден елемент от менюто",
                Description = "Запис на URL към даден елемент от менюто"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.MenuNodeSave,
                FrendlyName = "Запис на нов елемент към менюто",
                Description = "Запис на нов елемент към менюто"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.RemoveMenuNode,
                FrendlyName = "Премахване на елемент от менюто",
                Description = "Премахване на елемент от менюто"
            });

            #region Groups setings

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.GroupShowList,
                FrendlyName = "Зареждане на списък групи",
                Description = "Зареждане на списък групи"
            }
            );

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.GroupPreview,
                FrendlyName = "Преглед група",
                Description = "Преглед група"
            }
            );

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.GroupSave,
                FrendlyName = "Запис група",
                Description = "Запис група"
            }
            );

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.GroupPersonAddDelete,
                FrendlyName = "Добавяне и изтриване на лице от група",
                Description = "Добавяне и изтриване на лице от група"
            });
            #endregion

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.ChangeCurrentPeriodInSession,
                FrendlyName = "Промяна на текущия период в сесията на протребителя",
                Description = "Промяна на текущия период в сесията на протребителя"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.OnlineUsersListDelete,
                FrendlyName = "Прекратяване на сесия на потребител",
                Description = "Прекратяване на сесия на потребител"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.ShowDownloadLogFile,
                FrendlyName = "Преглед на действията в системата",
                Description = "Преглед на действията в системата"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.ShowOnlineUsersList,
                FrendlyName = "Преглед на активни потребители",
                Description = "Преглед на активни потребители"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.ShowModuleList,
                FrendlyName = "Преглед на списък модули",
                Description = "Преглед на списък модули"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Admin,
                SecuritySetting = ETEMEnums.SecuritySettings.ShowAllowIPList,
                FrendlyName = "Преглед на списък ИП адреси",
                Description = "Преглед на списък ИП адреси"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Share,
                SecuritySetting = ETEMEnums.SecuritySettings.AttachmentShowList,
                FrendlyName = "Преглед на списък документи/справки",
                Description = "Преглед на списък документи/справки"

            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Share,
                SecuritySetting = ETEMEnums.SecuritySettings.AttachmentSave,
                FrendlyName = "Добавяне на документ / справки",
                Description = "Добавяне на документ / справки"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.Share,
                SecuritySetting = ETEMEnums.SecuritySettings.AttachmentPreview,
                FrendlyName = "Преглед на документ/справкa",
                Description = "Преглед на документ/справкa"
            });

            #region CostCalculation

            #region DiePriceList
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.DiePricesListView,
                FrendlyName = "View a list of price lists",
                Description = "View a list of price lists"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.DiePriceListPreview,
                FrendlyName = "Preview a price list",
                Description = "Preview a price list"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.DiePriceListSave,
                FrendlyName = "Save a price list",
                Description = "Save a price list"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.DiePriceListImportDetails,
                FrendlyName = "Import details for price list",
                Description = "Import details for price list"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.DiePriceListDelete,
                FrendlyName = "Delete price lists with all details",
                Description = "Delete price lists with all details"
            });
            #endregion
            #region DiePriceListDetail
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.DiePriceListDetailsListView,
                FrendlyName = "View a list of price lists details",
                Description = "View a list of price lists details"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.DiePriceListDetailsPreview,
                FrendlyName = "Preview a price list details",
                Description = "Preview a price list details"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.DiePriceListDetailsSave,
                FrendlyName = "Save a price list details",
                Description = "Save a price list details"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.DiePriceListDetailsDelete,
                FrendlyName = "Delete price list details",
                Description = "Delete price list details"
            });
            #endregion
            #region MaterialPriceList
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.MaterialPricesListView,
                FrendlyName = "View a list of material price list",
                Description = "View a list of material price list"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.MaterialPriceListPreview,
                FrendlyName = "Preview a material price list",
                Description = "Preview a material price list"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.MaterialPriceListSave,
                FrendlyName = "Save a material price list",
                Description = "Save a material price list"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.MaterialPriceListDelete,
                FrendlyName = "Delete material price list",
                Description = "Delete material price list"
            });
            #endregion
            #region SAPData
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.SAPDataListView,
                FrendlyName = "View a list of SAP data by cost centers",
                Description = "View a list of SAP data by cost centers"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.SAPDataPreview,
                FrendlyName = "Preview a SAP data by cost centers",
                Description = "Preview a SAP data by cost centers"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.SAPDataSave,
                FrendlyName = "Save a SAP data by cost centers",
                Description = "Save a SAP data by cost centers"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.SAPDataImport,
                FrendlyName = "Import expenses by cost centers",
                Description = "Import expenses by cost centers"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.SAPDataDelete,
                FrendlyName = "Delete SAP data with all expenses by cost centers",
                Description = "Delete SAP data with all expenses by cost centers"
            });            
            #endregion
            #region SAPDataExpense
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.SAPDataExpenseListView,
                FrendlyName = "View a list of SAP data expenses by cost centers",
                Description = "View a list of SAP data expenses by cost centers"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.SAPDataExpensePreview,
                FrendlyName = "Preview a SAP data expenses by cost centers",
                Description = "Preview a SAP data expenses by cost centers"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.SAPDataExpenseSave,
                FrendlyName = "Save a SAP data expenses by cost centers",
                Description = "Save a SAP data expenses by cost centers"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.SAPDataExpenseDelete,
                FrendlyName = "Delete SAP data expenses by cost centers",
                Description = "Delete SAP data expenses by cost centers"
            });
            #endregion
            #region SAPDataQuantity
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.SAPDataQuantityListView,
                FrendlyName = "View a list of SAP data quantities by cost centers",
                Description = "View a list of SAP data quantities by cost centers"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.SAPDataQuantityPreview,
                FrendlyName = "Preview a SAP data quantities by cost centers",
                Description = "Preview a SAP data quantities by cost centers"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.SAPDataQuantitySave,
                FrendlyName = "Save a SAP data quantities by cost centers",
                Description = "Save a SAP data quantities by cost centers"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.SAPDataQuantityDelete,
                FrendlyName = "Delete SAP data quantities by cost centers",
                Description = "Delete SAP data quantities by cost centers"
            });
            #endregion
            #region DieFormula
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting    = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting         = ETEMEnums.SecuritySettings.DieFormulaListView,
                FrendlyName             = "Review of list of formulas",
                Description             = "Review of list of formulas"

            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting    = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting         = ETEMEnums.SecuritySettings.DieFormulaPreview,
                FrendlyName             = "Review of formula",
                Description             = "Review of formula"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting    = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting         = ETEMEnums.SecuritySettings.DieFormulaSave,
                FrendlyName             = "Save formula",
                Description             = "Save formula"

            });
            #endregion
            #region CommissionsByAgent
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.CommissionsByAgentListView,
                FrendlyName = "View a list of Commissions by Agents",
                Description = "View a list of Commissions by Agents"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.CommissionsByAgentPreview,
                FrendlyName = "Preview a data for Commissions by Agent",
                Description = "Preview a data for Commissions by Agent"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.CommissionsByAgentSave,
                FrendlyName = "Save a data for Commissions by Agent",
                Description = "Save a data for Commissions by Agent"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.CommissionsByAgentDelete,
                FrendlyName = "Delete Commissions by Agents",
                Description = "Delete Commissions by Agents"
            });
            #endregion
            #region ProfilesList

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting    = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting         = ETEMEnums.SecuritySettings.ProfilesShowList,
                FrendlyName             = "Review of list of profiles",
                Description             = "Review of list of profiles"

            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting    = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting         = ETEMEnums.SecuritySettings.ProfilesPreview,
                FrendlyName             = "Review of profile",
                Description             = "Review of profile"
            });

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting    = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting         = ETEMEnums.SecuritySettings.ProfilesSave,
                FrendlyName             = "Save profile",
                Description             = "Save profile"

            });

            #endregion
            #region ProfileSettingValidation
            
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting    = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting         = ETEMEnums.SecuritySettings.ProfileSettingValidationSave,
                FrendlyName             = "Save Profile Setting Validation",
                Description             = "Save Profile Setting Validation"

            });

            #endregion
            #region Offer

            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting    = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting         = ETEMEnums.SecuritySettings.ShowOfferFullList,
                FrendlyName             = "Show offer full list",
                Description             = "User can see all offers not only its"

            });

            #endregion
            #region AverageOutturnOverTimeList
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.AverageOutturnOverTimeListView,
                FrendlyName = "View a list of average outturn over time",
                Description = "View a list of average outturn over time"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.AverageOutturnOverTimeListPreview,
                FrendlyName = "Preview an average outturn over time",
                Description = "Preview an average outturn over time"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.AverageOutturnOverTimeListSave,
                FrendlyName = "Save an average outturn over time",
                Description = "Save an average outturn over time"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.AverageOutturnOverTimeListDelete,
                FrendlyName = "Delete an average outturn over time",
                Description = "Delete an average outturn over time"
            });
            #endregion
            #region ProductivityAndScrap
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.ProductivityAndScrapListView,
                FrendlyName = "View a list of productivity and scrap",
                Description = "View a list of productivity and scrap"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.ProductivityAndScrapPreview,
                FrendlyName = "Preview productivity and scrap data",
                Description = "Preview productivity and scrap data"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.ProductivityAndScrapSave,
                FrendlyName = "Save productivity and scrap data",
                Description = "Save productivity and scrap data"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.ProductivityAndScrapImport,
                FrendlyName = "Import productivity and scrap data",
                Description = "Import productivity and scrap data"
            });
            this.ListPermittedActionSetting.Add(new PermittedActionSetting()
            {
                GroupSecuritySetting = ETEMEnums.GroupSecuritySettings.CostCalculation,
                SecuritySetting = ETEMEnums.SecuritySettings.ProductivityAndScrapDelete,
                FrendlyName = "Delete productivity and scrap data",
                Description = "Delete productivity and scrap data"
            });
            #endregion
            #endregion

        }

        public CallContext MergeSettings(CallContext inputContext)
        {
            CallContext outputContext = new CallContext();
            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;

            if (!HasUserActionPermission(null, outputContext, inputContext))
            {
                return outputContext;
            }

            LoadPermittedAction();

            List<PermittedAction> listActionDB = this.dbContext.PermittedActions.ToList();

            List<PermittedAction> listActionToBeSave = new List<PermittedAction>();

            foreach (PermittedActionSetting apToBeSave in ListPermittedActionSetting)
            {
                if (listActionDB.Where(
                    apDB => apDB.GroupSecuritySetting == apToBeSave.GroupSecuritySetting.ToString() && apDB.SecuritySetting == apToBeSave.SecuritySetting.ToString()).Count() == 0)
                {
                    listActionToBeSave.Add(new PermittedAction()
                        {
                            GroupSecuritySetting = apToBeSave.GroupSecuritySetting.ToString(),
                            SecuritySetting = apToBeSave.SecuritySetting.ToString(),
                            FrendlyName = apToBeSave.FrendlyName,
                            Description = apToBeSave.Description
                        }
                    );
                }
            }

            CallContext resultContext = new CallContext();
            resultContext.securitySettings = ETEMEnums.SecuritySettings.PermittedActionSave;
            resultContext.CurrentConsumerID = new SettingBL().GetSettingByCode(ETEMEnums.AppSettings.UserIDBindWithSystem.ToString()).SettingValue;

            foreach (PermittedAction entity in listActionToBeSave)
            {
                resultContext = new PermittedActionBL().EntitySave<PermittedAction>(entity, resultContext);
            }

            return outputContext;
        }

        internal List<PermittedActionDataView> GetAllPermittedActionsByRole(string entityID, string sortExpression, string sortDirection)
        {
            int idRole = Int32.Parse(entityID);

            List<PermittedActionDataView> list = (from p in this.dbContext.PermittedActions
                                                  join rp in this.dbContext.RolePermittedActionLinks on p.idPermittedAction equals rp.idPermittedAction
                                                  where rp.idRole == idRole
                                                  orderby p.GroupSecuritySetting, p.SecuritySetting
                                                  select new PermittedActionDataView()
                                                      {
                                                          idPermittedAction = p.idPermittedAction,
                                                          RolePermittedActionID = rp.idRolePermittedAction,
                                                          FrendlyName = p.FrendlyName,
                                                          Description = p.Description
                                                      }
                                                  ).OrderBy(s => s.FrendlyName).ToList();

            list = BaseClassBL<PermittedActionDataView>.Sort(list, sortExpression, sortDirection).ToList();
            return list;
        }

        internal List<PermittedActionDataView> GetAllPermittedActionsByRoleNotAdded(string entityID, string sortExpression, string sortDirection)
        {
            int idRole = Int32.Parse(entityID);

            List<int> listPermittedActionID = (from p in this.dbContext.RolePermittedActionLinks
                                               where p.idRole == idRole && p.idPermittedAction.HasValue
                                               select p.idPermittedAction.Value).ToList();

            List<PermittedActionDataView> list = (from p in this.dbContext.PermittedActions
                                                  join m in this.dbContext.Modules on p.idModule equals m.idModule into grModule
                                                  from subModule in grModule.DefaultIfEmpty()
                                                  where !listPermittedActionID.Contains(p.idPermittedAction)
                                                  select new PermittedActionDataView()
                                                    {
                                                        idPermittedAction = p.idPermittedAction,
                                                        FrendlyName = p.FrendlyName,
                                                        Description = p.Description,
                                                        ModuleName = subModule.ModuleName,
                                                        idModule = subModule.idModule
                                                    }).ToList();

            list = BaseClassBL<PermittedActionDataView>.Sort(list, sortExpression, sortDirection).ToList();
            return list;
        }
    }

    public class PermittedActionSetting : BaseSettings
    {

    }
}