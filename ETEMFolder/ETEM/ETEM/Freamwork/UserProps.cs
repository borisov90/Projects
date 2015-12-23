using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;
using ETEMModel.Helpers;

namespace ETEM.Freamwork
{
    public class UserProps : Identifiable
    {
        public string Page { get; set; }
        public string SecuritySetting { get; set; }
        public string SecuritySettingBG { get; set; }

        public string UserName { get; set; }
        public string IdUser { get; set; }

        public string Password { get; set; }
        public string PersonNamePlusTitle { get; set; }
        public string PersonTwoNamePlusTitle { get; set; }
        public string PersonNameNoTitle { get; set; }
        public string PersonNameAndFamily { get; set; }
        public string PersonID { get; set; }
        public string Email { get; set; }
        public string StructureName { get; set; }
        public string StructureFullName { get; set; }
        public string CurrentLanguage { get; set; }
        public string SessionID { get; set; }
        public string IPAddress { get; set; }
        public DateTime LoginDateTime { get; set; }
        public DateTime LastRequestDateTime { get; set; }
        public string LastPageName { get; set; }
        public string LastModuleName { get; set; }

        public bool IsPersonAdministrator { get; set; }
        public bool IsPersonAdminActivity { get; set; }
        public bool IsPersonCampusExpert { get; set; }
        public bool IsPersonGeneral { get; set; }
        public bool IsPersonLecturer { get; set; }
        public bool IsPersonStudent { get; set; }

        public bool IsOnlyPersonAdministrator { get; set; }
        public bool IsOnlyPersonAdminActivity { get; set; }
        public bool IsOnlyCampusExpert { get; set; }
        public bool IsOnlyGeneral { get; set; }
        public bool IsOnlyPersonLecturer { get; set; }
        public bool IsOnlyPersonStudent { get; set; }

        public bool IsUserAdministrator { get; set; }
        public bool IsUserAdminActivity { get; set; }
        public bool IsUserCampusExpert { get; set; }
        public bool IsUserCampusHost { get; set; }
        public bool IsUserScholarshipExpert { get; set; }
        public bool IsUserGeneral { get; set; }
        public bool IsUserLecturer { get; set; }
        public bool IsUserStudent { get; set; }
        public bool IsUserSUPPORT { get; set; }
        public bool IsCheckDomain { get; set; }
        public bool IsKilled { get; set; }
        public string IsKilledStr { get { return IsKilled ? "Неактивна" : "Активна"; } }

        public List<Role> Roles { get; set; }
        public List<int> ListUserPermittedActionsID { get; set; }

        public UserProps()
        {
            this.IdUser = ETEMModel.Helpers.Constants.INVALID_ID_STRING;

            this.Roles = new List<Role>();
            this.ListUserPermittedActionsID = new List<int>();
        }

        public int EntityID
        {
            get { return int.Parse(this.PersonID); }
        }

        public List<string> ValidateEntity(CallContext outputContext)
        {
            throw new NotImplementedException();
        }

        public string ValidationErrorsAsString { get; set; }

        public int idStudent { get; set; }
    }
}