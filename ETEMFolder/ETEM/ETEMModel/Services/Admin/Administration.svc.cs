using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ETEMModel.Models;
using ETEMModel.Helpers;
using System.Linq.Expressions;
using System.Data.SqlClient;
using ETEMModel.Models.DataView;
using ETEMModel.Helpers.Admin;

using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Helpers.Common;
using System.ServiceModel.Activation;



using ETEMModel.Models.DataView.Admin;

using ETEMModel.Helpers.Employees;

using System.ServiceModel.Channels;
using ETEMModel.Services.Base;



namespace ETEMModel.Services.Admin
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Administration" in code, svc and config file together.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Administration : BaseClassService, IAdministration
    {
        #region IAdministration Members




      

      

        public User GetUserByUserID(string entityID)
        {
            return new UserBL().GetEntityById(Int32.Parse(entityID));
        }

        public User GetUserByPersonID(int personID)
        {
            return new UserBL().GetUserByPersonID(personID);
        }

        public List<User> GetUsersByPersonIDs(List<int> personIDs)
        {
            return new UserBL().GetUsersByPersonIDs(personIDs);
        }

        public CallContext UserSave(User entity, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.UserSave;
            CallContext resContext = new UserBL().EntitySave<User>(entity, resultContext);

            return resContext;
        }

        public void DecryptAllUserPassword()
        {
            new UserBL().DecryptAllUserPassword();


        }

        public void EncryptAllUserPassword()
        {
            new UserBL().EncryptAllUserPassword();


        }



        private void MakeEventLog(string EventAction, string EntityName, CallContext resContext)
        {
            ///TODO: Да се направи EventLog
        }

        private bool HasUserActionPermission(string ActionPermission, string currentUserID)
        {
            return true;

        }

        public CallContext Login(string userName, string Password)
        {
            RequestMeasure requestMeasure = new RequestMeasure("Administration.svc.Login");
            CallContext inputContext = new CallContext();
            CallContext outputContext = new UserBL().Login(userName, Password, inputContext);
            BaseHelper.Log(requestMeasure.ToString());
            return outputContext;
        }

        #region KeyType

        public List<KeyType> GetAllKeyTypes(string sortExpression, string sortDirection)
        {
            List<KeyType> list = new KeyTypeBL().GetAll(sortExpression, sortDirection);
            return list;
        }

        public KeyType GetKeyTypeByKeyTypeID(string _entityID)
        {
            return new KeyTypeBL().GetEntityById(Int32.Parse(_entityID));
        }

        public KeyType GetKeyTypeByIntCode(string keyTypeIntCode)
        {
            return new KeyTypeBL().GetKeyTypeByIntCode(keyTypeIntCode);
        }

        public List<KeyValue> GetKeyValuesByKeyTypeId(int idKeyType)
        {
            List<KeyValue> listKeyValues = new KeyValueBL().GetKeyValuesByKeyTypeId(idKeyType);
            return listKeyValues;
        }



        public CallContext KeyTypeSave(KeyType entity, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.KeyTypeSave;
            CallContext resContext = new KeyTypeBL().EntitySave<KeyType>(entity, resultContext);

            return resContext;
        }
        #endregion

        #region KeyValue
        public List<KeyValue> GetAllKeyValue()
        {
            return new KeyValueBL().GetAllEntities<KeyValue>();
        }

        public List<KeyValue> GetAllKeyValueByKeyTypeID(string _entityID, string sortExpression, string sortDirection)
        {
            return new KeyValueBL().GetAllKeyValueByKeyTypeID(_entityID, sortExpression, sortDirection);
        }

        public KeyValue GetKeyValueByKeyValueID(string _entityID)
        {
            return new KeyValueBL().GetKeyValueByKeyValueID(_entityID);
        }

        public List<KeyValue> GetAllKeyValueByKeyTypeIntCode(string keyTypeIntCode)
        {
            return new KeyValueBL().GetAllKeyValueByKeyTypeIntCode(keyTypeIntCode);
        }

        public int GetKeyValueIdByIntCode(string _IntCodeType, string _IntCodeValue)
        {
            return new KeyValueBL().GetKeyValueIdByIntCode(_IntCodeType, _IntCodeValue);
        }

        public KeyValue GetKeyValueByIntCode(string _IntCodeType, string _IntCodeValue)
        {
            if (DictionaryKeyType != null && DictionaryKeyValue != null)
            {
                return GetKeyValueByIntCodeByApplication(_IntCodeType, _IntCodeValue);
            }
            else
            {
                return new KeyValueBL().GetKeyValueByIntCode(_IntCodeType, _IntCodeValue);
            }

        }

        public CallContext KeyValueSave(KeyValue entity)
        {
            CallContext resultContext = new CallContext();
            resultContext.securitySettings = ETEMEnums.SecuritySettings.KeyTypeSave;
            CallContext resContext = new KeyValueBL().EntitySave<KeyValue>(entity, resultContext);
            return resContext;
        }
        #endregion

        #region Person

        

        


        public List<PersonDataView> GetAllPersons(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection)
        {
            List<PersonDataView> listView = new PersonBL().GetPersonDataView(searchCriteria, sortExpression, sortDirection);

            return listView;
        }

        public Person GetPersonByPersonID(string _entityID)
        {
            return new PersonBL().GetEntityById(Int32.Parse(_entityID));
        }

        public NavURL GetUrlNavById(int? id)
        {
            ETEMDataModelEntities dbContext = new ETEMDataModelEntities();
            NavURL url = dbContext.NavURLs.FirstOrDefault(n => n.idNavURL == id);
            return url;
        }

        public Person GetPersonByPersonIdentityNumber(string identityNumber)
        {
            return new PersonBL().GetPersonByIdentityNumber(identityNumber);
        }

        public Person GetPersonByNames(string firstName, string secondName, string lastName, CallContext resultContext)
        {
            return new PersonBL().GetPersonByNames(firstName, secondName, lastName, resultContext);
        }

        public Person GetPersonByEGN(string _EGN, CallContext resultContext)
        {
            return new PersonBL().GetPersonByEGN(_EGN, resultContext);
        }

        public CallContext PersonSave(Person entity, CallContext resultContext)
        {

            resultContext.securitySettings = ETEMEnums.SecuritySettings.PersonSave;
            CallContext resContext = new PersonBL().EntitySave<Person>(entity, resultContext);

            return resContext;
        }
        #endregion

        

        #region Setting
        public List<Setting> GetAllSettings(string sortExpression, string sortDirection, List<AbstractSearch> searchCriterias)
        {
            List<Setting> list = new SettingBL().GetAllSettings(sortExpression, sortDirection, searchCriterias);
            return list;
        }

        public Setting GetSettingByCode(string settingIntCode)
        {
            return new SettingBL().GetSettingByCode(settingIntCode);
        }
        public Setting GetSettingBySettingID(string _entityID)
        {
            return new SettingBL().GetSettingBySettingID(_entityID);
        }

        public CallContext SettingSave(Setting entity, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.SettingSave;
            CallContext resContext = new SettingBL().EntitySave<Setting>(entity, resultContext);

            return resContext;
        }
        #endregion



       

        





        #region CronProcessHistory
        public CallContext CronProcessHistorySave(CronProcessHistory entity, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.KeyTypeSave;
            CallContext resContext = new CronProcessHistoryBL().EntitySave<CronProcessHistory>(entity, resultContext);

            return resContext;
        }

        public List<CronProcessHistory> GetAllCronProcessHistory(string sortExpression, string sortDirection)
        {
            List<CronProcessHistory> list = new CronProcessHistoryBL().GetAll(sortExpression, sortDirection);
            return list;
        }

        public CronProcessHistory GetLastCronProcessHistory()
        {

            return new CronProcessHistoryBL().GetLastCronProcessHistory();
        }

        public CronProcessHistory GetCronProcessHistoryByID(string entityID)
        {
            return new CronProcessHistoryBL().GetEntityById(Int32.Parse(entityID));
        }

        #endregion

        #region Roles

        public CallContext RolesMenuSave(List<KeyValuePair<string, string>> listNewNodeNames, List<KeyValuePair<string, string>> listRootMenuChecked, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.RolesMenuSave;
            List<MenuNode> newNodes = new List<MenuNode>();
            foreach (var node in listNewNodeNames)
            {
                newNodes.Add(new MenuNode
                {
                    name = node.Value,
                    idNode = int.Parse(node.Key)
                });
            }

            CallContext resContext = new MenuNodeBL().EntitySave<MenuNode>(newNodes, resultContext);
            if (resContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {
                resContext = new RoleMenuNodeBL().SaveRoleMenuNodes(listRootMenuChecked, resultContext);
            }


            return resContext;
        }

        public bool GetAllMenuNode(int nodeId, int roleId)
        {
            bool isCheked = new RoleMenuNodeBL().IsCheckBoxChecked(nodeId, roleId);
            return isCheked;
        }

        public CallContext MenuNodeSave(NavURL navUrl, MenuNode entity, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.NavUrlSave;
            CallContext resContext = new NavUrlBL().EntitySave<NavURL>(navUrl, resultContext);
            if (resContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {
                resultContext.securitySettings = ETEMEnums.SecuritySettings.MenuNodeSave;
                entity.idNavURL = int.Parse(resContext.EntityID);
                resContext = new MenuNodeBL().EntitySave<MenuNode>(entity, resultContext);
            }

            return resContext;
        }

        public CallContext RemoveMenuNode(int nodeID, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.RemoveMenuNode;
            CallContext resContext = new MenuNodeBL().RemoveMenuNode(nodeID, resultContext);

            return resContext;
        }


        public List<Role> GetAllRoles(string sortExpression, string sortDirection)
        {
            return new RoleBL().GetAll(sortExpression, sortDirection);
        }

        public List<Role> GetAllRolesByUser(string userID, string sortExpression, string sortDirection)
        {
            return new RoleBL().GetAllRolesByUser(userID, sortExpression, sortDirection);
        }

        public List<Role> GetAllRoleByUserNotAdded(string userID, string sortExpression, string sortDirection, List<AbstractSearch> searchCriterias)
        {
            return new RoleBL().GetAllRoleByUserNotAdded(userID, sortExpression, sortDirection, searchCriterias);
        }

        public Role GetRoleByID(string entityID)
        {
            return new RoleBL().GetEntityById(Int32.Parse(entityID));
        }

        public CallContext RoleSave(Role entity, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.KeyTypeSave;
            CallContext resContext = new RoleBL().EntitySave<Role>(entity, resultContext);

            return resContext;
        }

        public CallContext AddUserRole(string userID, string roleID, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.AddUserRole;
            CallContext resContext = new RoleBL().AddUserRole(userID, roleID, resultContext);

            return resContext;
        }

        public CallContext RemoveUserRole(string userID, string roleID, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.RemoveUserRole;
            CallContext resContext = new RoleBL().RemoveUserRole(userID, roleID, resultContext);

            return resContext;
        }


        public CallContext AddPermittedActionToRole(List<RolePermittedActionLink> listEntity, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.RoleSave;
            CallContext resContext = new RoleBL().AddPermittedAction(listEntity, resultContext);

            return resContext;
        }

        public CallContext RemovePermittedActionToRole(List<RolePermittedActionLink> listEntity, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.RoleSave;
            CallContext resContext = new RoleBL().RemovePermittedAction(listEntity, resultContext);

            return resContext;
        }





        #endregion

        #region PermittedAction
        public List<PermittedAction> GetAllPermittedActions(string sortExpression, string sortDirection)
        {
            return new PermittedActionBL().GetAll(sortExpression, sortDirection);
        }

        public List<PermittedActionDataView> GetAllPermittedActions(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection)
        {
            return new PermittedActionBL().GetAllPermittedActionDataView(searchCriteria, sortExpression, sortDirection);
        }

        public PermittedAction GetPermittedActionByID(string entityID)
        {
            return new PermittedActionBL().GetEntityById(Int32.Parse(entityID));
        }

        public CallContext PermittedActionSave(PermittedAction entity, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.PermittedActionSave;
            CallContext resContext = new PermittedActionBL().EntitySave<PermittedAction>(entity, resultContext);

            return resContext;
        }

        public CallContext PermittedActionMerge(CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.PermittedActionMergeSettings;

            new PermittedActionBL().MergeSettings(resultContext);

            return resultContext;
        }


        public List<PermittedActionDataView> GetAllPermittedActionsByRole(string entityID, string sortExpression, string sortDirection)
        {
            return new PermittedActionBL().GetAllPermittedActionsByRole(entityID, sortExpression, sortDirection);
        }

        public List<PermittedActionDataView> GetAllPermittedActionsByRoleNotAdded(string entityID, string sortExpression, string sortDirection)
        {
            return new PermittedActionBL().GetAllPermittedActionsByRoleNotAdded(entityID, sortExpression, sortDirection);
        }


        #endregion

      

        #region MenuNodes
        public List<MenuNodeDataView> GetAllMenuNode(CallContext resultContext)
        {
            return new MenuNodeBL().GetAll(resultContext);
        }

        public string CreateManuNodeXML(CallContext resultContext)
        {
            return new MenuNodeBL().CreateManuNodeXML(resultContext);
        }

        public List<int> GetAllRoleMenuNodeByRoleId(CallContext callContext, int roleID)
        {
            return new MenuNodeBL().GetAllRoleMenuNodeByRoleId(callContext, roleID);
        }


        public List<MenuNodeDataView> GetAllRoleMenuNodeByAllRoles(List<Role> roles)
        {
            return new MenuNodeBL().GetAllRoleMenuNodeByAllRoles(roles);
        }

        public List<MenuNodeDataView> GetAllRoleMenuNodeByRoleId(int roleID)
        {
            return new MenuNodeBL().GetAllRoleMenuNodeByRoleId(roleID);
        }

        public string GetMenuNodeFullPath(int idChildNode, string nodeName)
        {
            return new MenuNodeBL().GetMenuNodeFullPath(idChildNode, nodeName);
        }

        #endregion

        #endregion



        public Dictionary<int, KeyType> DictionaryKeyType { get; set; }
        public Dictionary<int, KeyValue> DictionaryKeyValue { get; set; }

        public KeyValue GetKeyValueByIntCodeByApplication(string keyTypeIntCode, string keyValueIntCode)
        {
            KeyValue keyValue = new KeyValue();

            keyValue = (from kv in DictionaryKeyValue.Values
                        join kt in DictionaryKeyType.Values on kv.idKeyType equals kt.idKeyType
                        where kt.KeyTypeIntCode == keyTypeIntCode && kv.KeyValueIntCode == keyValueIntCode
                        select kv).FirstOrDefault();

            return keyValue;
        }



        #region ДЪРЖАВИ
        public List<Country> GetAllCountries()
        {
            List<Country> list = new LocationBL().GetAllCountries();
            return list;
        }

        public Country GetCountryByCode(string countryCode)
        {
            Country country = new LocationBL().GetCountryByCode(countryCode);
            return country;
        }

        public Country GetCountryById(string entityID)
        {
            Country country = new LocationBL().GetCountryById(Int32.Parse(entityID));
            return country;
        }
        #endregion

        #region ОБЛАСТИ
        public List<District> GetAllDistricts()
        {
            List<District> list = new LocationBL().GetAllDistricts();
            return list;
        }

        public List<District> GetDistrictsByCountryId(string countryID)
        {
            List<District> list = new LocationBL().GetDistrictsByCountryID(countryID);
            return list;
        }

        public District GetDistrictByCode(string districtCode)
        {
            District district = new LocationBL().GetDistrictByCode(districtCode);
            return district;
        }

        public District GetDistrictById(string entityID)
        {
            District district = new LocationBL().GetDistrictById(Int32.Parse(entityID));
            return district;
        }
        #endregion

        #region ОБЩИНИ
        public List<Municipality> GetAllMunicipalities()
        {
            List<Municipality> list = new LocationBL().GetAllMunicipalities();
            return list;
        }

        public List<Municipality> GetMunicipalitiesByDistrictId(string districtID)
        {
            List<Municipality> list = new LocationBL().GetMunicipalitiesByDistrictId(Int32.Parse(districtID));
            return list;
        }

        public List<Municipality> GetMunicipalitiesByCountryId(string countryID)
        {
            List<Municipality> list = new LocationBL().GetMunicipalitiesByCountryId(Int32.Parse(countryID));
            return list;
        }

        public Municipality GetMunicipalityById(string entityID)
        {
            Municipality municipality = new LocationBL().GetMunicipalityById(Int32.Parse(entityID));
            return municipality;
        }
        #endregion

        #region EKATTEs
        public List<EKATTE> GetAllEKATTEs()
        {
            List<EKATTE> list = new LocationBL().GetAllEKATTEs();
            return list;
        }

        public List<EKATTE> GetEKATTEsByMunicipalityId(string municipalityID)
        {
            List<EKATTE> list = new LocationBL().GetEKATTEsByMunicipalityId(Int32.Parse(municipalityID));
            return list;
        }

        public EKATTE GetEKATTEById(string entityID)
        {
            EKATTE eKATTE = new LocationBL().GetEKATTEById(Int32.Parse(entityID));
            return eKATTE;
        }
        #endregion

        #region НАСЕЛЕНИ МЕСТА
        public List<Location> GetAllLocations()
        {
            List<Location> list = new LocationBL().GetAllLocations();
            return list;
        }

        public List<Location> GetLocationsByMunicipalityId(string municipalityID)
        {
            List<Location> list = new LocationBL().GetLocationsByMunicipalityId(Int32.Parse(municipalityID));
            return list;
        }

        public Location GetLocationById(string entityID)
        {
            Location location = new LocationBL().GetLocationById(Int32.Parse(entityID));
            return location;
        }

        public List<LocationView> GetAllLocationViews()
        {
            List<LocationView> list = new LocationBL().GetAllLocationViews();
            return list;
        }

        public List<LocationView> GetLocationViewsByMunicipalityId(string municipalityID)
        {
            List<LocationView> list = new LocationBL().GetLocationViewsByMunicipalityId(Int32.Parse(municipalityID));
            return list;
        }

        public LocationView GetLocationViewById(string entityID)
        {
            LocationView locationView = new LocationBL().GetLocationViewById(Int32.Parse(entityID));
            return locationView;
        }

        public CallContext LocationSave(Location entity, CallContext resultContext)
        {
            CallContext resContext = new LocationBL().EntitySave<Location>(entity, resultContext);

            return resContext;
        }
        #endregion



        public List<KeyTypeDataView> GetKeyTypeDataView(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection)
        {
            return new KeyTypeBL().GetKeyTypeDataView(searchCriteria, sortExpression, sortDirection);
        }

        #region SEQUENCE
        public int GetSequenceNextValue(string resource, int? idResource, int? year, CallContext resultContext)
        {
            return new SequenceBL().GetSequenceNextValue(resource, idResource, year, resultContext);
        }

        public string GetFileVersion(string fileName)
        {
            return new SequenceBL().GetFileVersion(fileName);
        }

        #endregion



        public bool DoesEmployeExistByPersonId(int employeId)
        {
            return new EmployeBL().DoesEmployeExistByPersonId(employeId);
        }

       

        #region StreamsAndGroups

       
       
        

        #endregion

        



        

        







        












        
        public Person GetPersonByUserID(int userId)
        {
            return new PersonBL().GetPersonByUserID(userId);
        }

       

        

        #region Изпращане на E-mails
        public CallContext SendMail(List<SendMailHelper> listSendMailHelper, ETEMEnums.EmailTypeEnum emailType, CallContext resultContext)
        {
            return new CommonBL().SendMail(listSendMailHelper, emailType, resultContext);
        }

        public CallContext SendMailToAdministrator(SendMailHelper sendMailHelper)
        {
            return new CommonBL().SendMailToAdministrator(sendMailHelper);
        }


        public void SendMailPassword(User currentUser)
        {
            new UserBL().SendMailPassword(currentUser);

        }


        #endregion

        public List<AllowIPDataView> GetAllAllowIP(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection)
        {
            List<AllowIPDataView> listView = new AllowIPBL().GetAllAllowIP(searchCriteria, sortExpression, sortDirection);

            return listView;
        }

        public AllowIP GetAllowIPByID(string idEntity)
        {
            return new AllowIPBL().GetEntityById(Int32.Parse(idEntity));
        }

        public CallContext AllowIPSave(AllowIP entity, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.AllowIPSave;
            CallContext resContext = new AllowIPBL().EntitySave<Setting>(entity, resultContext);

            return resContext;
        }

        public List<ModuleDataView> GetAllModule(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection)
        {
            List<ModuleDataView> listView = new ModuleBL().GetAllModules(searchCriteria, sortExpression, sortDirection);

            return listView;
        }

        public Module GetModuleByID(string idEntity)
        {
            return new ModuleBL().GetEntityById(Int32.Parse(idEntity));
        }

        public CallContext ModuleSave(Module entity, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.ModuleSave;
            CallContext resContext = new ModuleBL().EntitySave<Setting>(entity, resultContext);

            return resContext;
        }

        public Module GetModuleBySysName(string moduleSysName)
        {
            return new ModuleBL().GetEntityByIPModuleSysName(moduleSysName);
        }

        public AllowIP GetEntityByIPAddress(string ipAddress)
        {
            return new AllowIPBL().GetEntityByIPAddress(ipAddress);
        }

        #region Групи

        public List<GroupDataView> GetGroupDataView(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection)
        {
            return new GroupBL().GetGroupDataView(searchCriteria, sortExpression, sortDirection);
        }

        public Group GetGroupByID(int entityID)
        {
            return new GroupBL().GetEntityById(entityID);
        }

        public CallContext GroupSave(Group group, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.GroupSave;
            resultContext = new GroupBL().EntitySave<Setting>(group, resultContext);

            return resultContext;
        }

        public bool IsUniqueRecordGroupPersonLink(int idGroup, int idPerson)
        {
            return new GroupPersonLinkBL().IsUniqueRecordGroupPersonLink(idGroup, idPerson);
        }

        public CallContext GroupPersonLinkDelete(List<GroupPersonLink> list, CallContext callContext)
        {
            callContext = new GroupPersonLinkBL().GroupPersonLinkDelete(list, callContext);
            return callContext;
        }

        public CallContext GroupPersonLinkSave(GroupPersonLink groupPersonLink, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.GroupSave;
            resultContext = new GroupPersonLinkBL().EntitySave<Setting>(groupPersonLink, resultContext);

            return resultContext;
        }

        public GroupPersonLink GetGroupPersonLinkByID(int entityID)
        {
            return new GroupPersonLinkBL().GetEntityById(entityID);
        }

        public List<GroupPersonLinkDataView> GetGroupPersonLinkDataViewByGroupID(int groupID)
        {
            return new GroupPersonLinkBL().GetGroupPersonLinkDataViewByGroupID(groupID);
        }

        #endregion



        



        

        
        public CallContext UserSendingEmails(ICollection<AbstractSearch> searchCriteria, List<int> listSelectedIDs, SendMailHelper sendMailData, CallContext callContext)
        {
            return new UserBL().UserSendingEmails(searchCriteria,
                                                         listSelectedIDs,
                                                         sendMailData,
                                                         callContext);
        }

        

       
       

        public List<PersonHistoryDataView> GetPersonHistoryDataViewByPersonID(int idPerson)
        {
            return new PersonHistoryBL().GetPersonHistoryDataViewByPersonID(idPerson);
        }

        public CallContext PersonHistorySave(PersonHistory entity, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.PersonSave;
            CallContext resContext = new PersonHistoryBL().EntitySave<PersonHistory>(entity, resultContext);

            return resContext;
        }

       

      

        public void DisableUserAccountByPersonID(int idPerson)
        {
            new UserBL().DisableUserAccountByPersonID(idPerson);
        }

        public User GetUserByUsername(string username)
        {
            var user = new UserBL().GetUserByUsername(username);
            return user;
        }

        public CallContext ChangeUserForgottenPasswordPassword(User user, Person person, CallContext callcontext)
        {
            callcontext = new UserBL().ChangeUserForgottenPasswordPassword(user, person, callcontext);
            return callcontext;
        }

        public List<UserDataView> GetAllUsers(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection)
        {
            return new UserBL().GetAllUsers(searchCriteria, sortExpression, sortDirection);
        }
    }
}
