using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ETEMModel.Helpers;
using ETEMModel.Models;
using ETEMModel.Models.DataView;
using ETEMModel.Helpers.AbstractSearchBLHolder;
using System.Web.Script.Services;
using System.ServiceModel.Web;
using ETEMModel.Models.DataView.Admin;

namespace ETEMModel.Services.Admin
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAdministration" in both code and config file together.
    [ServiceContract]
    public interface IAdministration
    {
        /*
        List<UserDataView> GetAllStudentUsers();

       
        #region ПОТРЕБИТЕЛИ
        
        [OperationContract]
        List<UserDataView> GetAllUsers(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection);

        [OperationContract]
        User GetUserByUserID(string entityID);

        [OperationContract]
        CallContext UserSave(User entity, CallContext resultContext);

        [OperationContract]
        CallContext Login(string userName, string Password); 

       

        
        #endregion

        #region РОЛИ
        [OperationContract]
        CallContext RolesMenuSave(List<KeyValuePair<string, string>> listNewNodeNames, List<KeyValuePair<string, string>> listRootMenuChecked, CallContext resultContext);

        [OperationContract]
        List<Role> GetAllRoles(string sortExpression, string sortDirection);

        [OperationContract]
        List<Role> GetAllRolesByUser(string userID,string sortExpression,string sortDirection);

        [OperationContract]
        List<Role> GetAllRoleByUserNotAdded(string userID, string sortExpression, string sortDirection);        

        [OperationContract]
        Role GetRoleByID(string entityID);

        [OperationContract]
        CallContext RoleSave(Role entity, CallContext resultContext);

        [OperationContract]
        CallContext AddUserRole(string userID, string roleID, CallContext resultContext);        
                
        [OperationContract]
        CallContext RemoveUserRole(string userID, string roleID, CallContext resultContext);
        #endregion

        #region ПОЗВОЛЕНИ ДЕЙСТВИЯ
        [OperationContract]
        CallContext AddPermittedActionToRole(List<RolePermittedActionLink> listEntity, CallContext resultContext);

        [OperationContract]
        CallContext RemovePermittedActionToRole(List<RolePermittedActionLink> listEntity, CallContext resultContext);

        [OperationContract]
        List<PermittedAction> GetAllPermittedActions(string sortExpression, string sortDirection);

        [OperationContract]
        List<PermittedActionDataView> GetAllPermittedActionsByRole(string entityID,string sortExpression,string sortDirection);

        [OperationContract]
        List<PermittedActionDataView> GetAllPermittedActionsByRoleNotAdded(string entityID, string sortExpression, string sortDirection);

        [OperationContract]
        PermittedAction GetPermittedActionByID(string entityID);

        [OperationContract]
        CallContext PermittedActionSave(PermittedAction entity, CallContext resultContext);

        [OperationContract]
        CallContext PermittedActionMerge(CallContext resultContext);
        #endregion

        #region ТИПОВЕ НОМЕНКЛАТУРИ
        [OperationContract]
        List<KeyType> GetAllKeyTypes(string sortExpression, string sortDirection);

        [OperationContract]
        List<KeyTypeDataView> GetKeyTypeDataView(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection);

        [OperationContract]
        KeyType GetKeyTypeByKeyTypeID(string entityID);

        [OperationContract]
        KeyType GetKeyTypeByIntCode(string keyTypeIntCode);

        [OperationContract]
        CallContext KeyTypeSave(KeyType entity, CallContext resultContext);
        #endregion

        #region СТОЙНОСТИ ЗА НОМЕНКЛАТУРИ
        [OperationContract]
        List<KeyValue> GetAllKeyValue();

        [OperationContract]
        List<KeyValue> GetAllKeyValueByKeyTypeID(string entityID,string sortExpression,string sortDirection);

        [OperationContract]
        List<KeyValue> GetAllKeyValueByKeyTypeIntCode(string keyTypeIntCode);

        [OperationContract]
        KeyValue GetKeyValueByKeyValueID(string entityID);

        [OperationContract]
        int GetKeyValueIdByIntCode(string _IntCodeType, string _IntCodeValue);

        [OperationContract]
        KeyValue GetKeyValueByIntCode(string _IntCodeType, string _IntCodeValue);

        [OperationContract]
        CallContext KeyValueSave(KeyValue entity);
        #endregion

        #region НАСТРОЙКИ
        [OperationContract]
        List<Setting> GetAllSettings(string sortExpression, string sortDirection);

        [OperationContract]
        Setting GetSettingByCode(string settingIntCode);

        [OperationContract]
        Setting GetSettingBySettingID(string entityID);

        [OperationContract]
        CallContext SettingSave(Setting entity, CallContext resultContext);
        #endregion

        #region ЛИЦА
        [OperationContract]
        List<PersonDataView> GetAllPersons(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection);

        [OperationContract]
        Person GetPersonByPersonID(string entityID);

        [OperationContract]
        Person GetPersonByPersonIdentityNumber(string identityNumber);

        [OperationContract]
        Person GetPersonByNames(string firstName, string secondName, string lastName, CallContext resultContext);

        [OperationContract]
        Person GetPersonByEGN(string _EGN, CallContext resultContext);

        [OperationContract]
        CallContext PersonSave(Person entity, CallContext resultContext);
        #endregion

        [OperationContract]
        UNI GetUNIByUNIID(string entityID);

        [OperationContract]
        UNI GetCurrentUNI(CallContext resultContext);

        [OperationContract]
        CallContext UNISave(UNI entity, CallContext resultContext);

        [OperationContract]
        Affiliate GetAffiliateByAffiliateID(string entityID);

        [OperationContract]
        List<AffiliateDataView> GetAllCurrentAffiliateByUNIID(int UNIID, string sortExpression, string sortDirection);

        [OperationContract]
        CallContext AffiliateSave(Affiliate entity, CallContext resultContext);

        [OperationContract]
        Faculty GetFacultyByFacultyID(string entityID);
                
        [OperationContract]
        CallContext FacultySave(Faculty entity, CallContext resultContext);

        [OperationContract]
        Speciality GetSpecialityBySpecialityID(string entityID);

        
        //[OperationContract]
        //List<SpecialityDataView> GetAllCurrentSpecialityDataViewByUNIID(string UNIID, CallContext resultContext,string sortExpression,string sortDirection);

        [OperationContract]
        CallContext SpecialitySave(Speciality entity, CallContext resultContext);

        #region CronProcessHistory
        [OperationContract]
        CallContext CronProcessHistorySave(CronProcessHistory entity, CallContext resultContext);

        [OperationContract]
        List<CronProcessHistory> GetAllCronProcessHistory(string sortExpression, string sortDirection);

        [OperationContract]
        CronProcessHistory GetLastCronProcessHistory();

        [OperationContract]
        CronProcessHistory GetCronProcessHistoryByID(string entityID);
        #endregion

        #region SpecialityCode
        [OperationContract]
        List<SpecialityCode> GetAllSpecialityCode(string sortExpression, string sortDirection);

        [OperationContract]
        CallContext SpecialityCodeSave(SpecialityCode entity, CallContext resultContext);

        [OperationContract]
        SpecialityCode GetSpecialityCodeBySpecialityCodeID(int entityID);
        #endregion

        #region MenuNodes
        [OperationContract]
        List<MenuNodeDataView> GetAllMenuNode(CallContext resultContext);

        [OperationContract]
        string CreateManuNodeXML(CallContext resultContext);

        [OperationContract]
        List<int> GetAllRoleMenuNodeByRoleId(CallContext callContext, int roleID);
        #endregion

        #region ДЪРЖАВИ
        [OperationContract]
        List<Country> GetAllCountries();

        [OperationContract]
        Country GetCountryByCode(string countryCode);

        [OperationContract]
        Country GetCountryById(string entityID);
        #endregion

        #region ОБЛАСТИ
        [OperationContract]
        List<District> GetAllDistricts();

        [OperationContract]
        List<District> GetDistrictsByCountryId(string countryID);

        [OperationContract]
        District GetDistrictById(string entityID);

        [OperationContract]
        District GetDistrictByCode(string districtCode);
        #endregion

        #region ОБЩИНИ
        [OperationContract]
        List<Municipality> GetAllMunicipalities();

        [OperationContract]
        List<Municipality> GetMunicipalitiesByDistrictId(string districtID);

        [OperationContract]
        List<Municipality> GetMunicipalitiesByCountryId(string countryID);

        [OperationContract]
        Municipality GetMunicipalityById(string entityID);
        #endregion

        #region EKATTEs
        [OperationContract]
        List<EKATTE> GetAllEKATTEs();

        [OperationContract]
        List<EKATTE> GetEKATTEsByMunicipalityId(string municipalityID);

        [OperationContract]
        EKATTE GetEKATTEById(string entityID);
        #endregion

        #region НАСЕЛЕНИ МЕСТА
        [OperationContract]
        List<Location> GetAllLocations();
        
        [OperationContract]
        List<LocationView> GetAllLocationViews();

        [OperationContract]
        List<Location> GetLocationsByMunicipalityId(string municipalityID);

        [OperationContract]
        List<LocationView> GetLocationViewsByMunicipalityId(string municipalityID);

        [OperationContract]
        Location GetLocationById(string entityID);

        [OperationContract]
        LocationView GetLocationViewById(string entityID);

        [OperationContract]
        CallContext LocationSave(Location entity, CallContext resultContext);
        #endregion

        #region SEQUENCE
        int GetSequenceNextValue(string resource, int? idResource, int? year, CallContext resultContext);
        #endregion
         * 
         */
    }
}
