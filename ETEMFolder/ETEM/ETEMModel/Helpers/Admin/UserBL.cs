using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;
using ETEMModel.Models.DataView;
using ETEMModel.Helpers.AbstractSearchBLHolder;
using System.DirectoryServices;
using ETEMModel.Helpers.Common;
using System.Text.RegularExpressions;
using System.Text;


namespace ETEMModel.Helpers.Admin
{
    public class UserBL : BaseClassBL<User>
    {
        public UserBL()
        {
            this.EntitySetName = "Users";
        }

        internal override User GetEntityById(int idEntity)
        {
            return this.dbContext.Users.Where(e => e.idUser == idEntity).FirstOrDefault();
        }

        public User GetUserByUserID(string idEntity)
        {
            return GetEntityById(Int32.Parse(idEntity));
        }

        public User GetUserByPersonID(int idPerson)
        {
            return this.dbContext.Users.Where(p => p.idPerson == idPerson).FirstOrDefault();
        }

        public List<User> GetUsersByPersonIDs(List<int> personIDs)
        {
            return this.dbContext.Users.Where(p => personIDs.Contains(p.idPerson)).ToList<User>();
        }

        internal override void EntityToEntity(User sourceEntity, User targetEntity)
        {
            targetEntity.UserName = sourceEntity.UserName;
            targetEntity.Password = sourceEntity.Password;
            targetEntity.idPerson = sourceEntity.idPerson;
            targetEntity.idStatus = sourceEntity.idStatus;
            targetEntity.Description = sourceEntity.Description;
            targetEntity.AltPassword = sourceEntity.AltPassword;
            targetEntity.idAltPerson = sourceEntity.idAltPerson;
            targetEntity.idCheckDomain = sourceEntity.idCheckDomain;
        }

        public CallContext Login(string userName, string Password, CallContext inputContext)
        {
            this.RequestMeasure.PageName = "Login";

            CallContext outputContext = new CallContext();

            User user = dbContext.Users.Where(u => u.UserName == userName).FirstOrDefault();

            if (user != null)
            {
                KeyValue status = dbContext.KeyValues.Where(k => k.idKeyValue == user.idStatus).FirstOrDefault();

                if (status == null)
                {
                    outputContext.EntityID = Constants.INVALID_ID_STRING;
                    outputContext.ResultCode = ETEMEnums.ResultEnum.Error;
                    outputContext.Message = BaseHelper.GetCaptionString("UI_Login_Unsuccessful");
                }
                else
                {
                    if (status.KeyValueIntCode == "Active")
                    {
                        KeyValue kvCheckDomainYes = new KeyValueBL().GetKeyValueByIntCode("YES_NO", "Yes");

                        if (kvCheckDomainYes.idKeyValue == user.idCheckDomain)
                        {

                            string domainPart = new SettingBL().GetSettingByCode(ETEMEnums.AppSettings.DomainName.ToString()).SettingValue;

                            string qualifiedUserName = domainPart + "\\" + userName;
                            string serverName = domainPart;
                            DirectoryEntry entry = new DirectoryEntry("LDAP://" + serverName,
                                qualifiedUserName, Password);

                            try
                            {
                                DirectorySearcher searcher = new DirectorySearcher(entry);

                                string qryFilterFormat = String.Format("(&(objectClass=user)(objectCategory=person)(sAMAccountName={0}))", userName);
                                SearchResult result = null;

                                List<DomainUserInfo> userInfo = new List<DomainUserInfo>();
                                DomainUserInfo objuser = new DomainUserInfo();


                                searcher.Filter = qryFilterFormat;
                                SearchResultCollection results = searcher.FindAll();
                                result = (results.Count != 0) ? results[0] : null;

                                if (result != null)
                                {
                                    objuser.ShortName = (string)result.Properties["sAMAccountName"][0];
                                    objuser.DisplayName = (string)result.Properties["displayname"][0];

                                    outputContext.EntityID = user.idUser.ToString();
                                    outputContext.ResultCode = ETEMEnums.ResultEnum.Success;
                                    outputContext.Message = BaseHelper.GetCaptionString("UI_Login_Successful");
                                }



                            }
                            catch (Exception ex)
                            {
                                outputContext.EntityID = Constants.INVALID_ID_STRING;
                                outputContext.ResultCode = ETEMEnums.ResultEnum.Warning;
                                outputContext.Message = BaseHelper.GetCaptionString("UI_Login_Unsuccessful");

                                BaseHelper.Log(ex.Message);
                            }
                        }
                        else
                        {
                            string tmpPassword = ETEMModel.Helpers.BaseHelper.Encrypt(Password);

                            if (user.Password == tmpPassword)
                            {
                                outputContext.EntityID = user.idUser.ToString();
                                outputContext.ResultCode = ETEMEnums.ResultEnum.Success;
                                outputContext.Message = BaseHelper.GetCaptionString("UI_Login_Successful");
                            }
                            else
                            {
                                outputContext.EntityID = Constants.INVALID_ID_STRING;
                                outputContext.ResultCode = ETEMEnums.ResultEnum.Warning;
                                outputContext.Message = BaseHelper.GetCaptionString("UI_Login_Unsuccessful");
                            }
                        }
                    }
                    else if (status.KeyValueIntCode == "TemporarilyInactive")
                    {
                        string tmpPassword = ETEMModel.Helpers.BaseHelper.Encrypt(Password);


                        if (user.AltPassword == tmpPassword)
                        {
                            outputContext.EntityID = user.idUser.ToString();
                            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;
                            outputContext.Message = BaseHelper.GetCaptionString("UI_Login_Successful");
                        }
                        else
                        {
                            outputContext.EntityID = Constants.INVALID_ID_STRING;
                            outputContext.ResultCode = ETEMEnums.ResultEnum.Warning;
                            outputContext.Message = BaseHelper.GetCaptionString("UI_Login_Unsuccessful");
                        }
                    }
                    else
                    {
                        outputContext.EntityID = Constants.INVALID_ID_STRING;
                        outputContext.ResultCode = ETEMEnums.ResultEnum.Warning;
                        outputContext.Message = BaseHelper.GetCaptionString("UI_Login_Unsuccessful");
                    }
                }
            }
            else
            {
                user = new User();
                user.UserName = userName;
                user.idUser = Constants.INVALID_ID;
                outputContext.EntityID = Constants.INVALID_ID_STRING;
                outputContext.ResultCode = ETEMEnums.ResultEnum.Warning;
                outputContext.Message = BaseHelper.GetCaptionString("UI_Login_Unsuccessful");
            }

            MakeEventLog(outputContext, user);

            BaseHelper.Log(this.RequestMeasure.ToString());

            return outputContext;
        }

        internal List<UserDataView> GetAllUsers(string sortExpression, string sortDirection)
        {
            List<UserDataView> listView = (from u in dbContext.Users
                                           join p in dbContext.Persons on u.idPerson equals p.idPerson
                                           join kv in dbContext.KeyValues on u.idStatus equals kv.idKeyValue
                                           select new UserDataView
                                           {
                                               IdUser = u.idUser,
                                               UserName = u.UserName,
                                               FirstName = p.FirstName,
                                               SecondName = p.SecondName,
                                               LastName = p.LastName,
                                               EGN = p.EGN,
                                               IdentityNumber = p.IdentityNumber,
                                               idStatus = u.idStatus,
                                               Status = kv.Name,
                                               Description = u.Description
                                           }).ToList<UserDataView>();

            listView = BaseClassBL<UserDataView>.Sort(listView, sortExpression, sortDirection).ToList();

            return listView;
        }


        internal List<UserDataView> GetAllUsers(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection)
        {

            List<UserDataView> listView = (from u in dbContext.Users
                                           join p in dbContext.Persons on u.idPerson equals p.idPerson

                                           join kvPosition in dbContext.KeyValues on p.idPosition equals kvPosition.idKeyValue into grPosition
                                           from position in grPosition.DefaultIfEmpty()

                                           join kv in dbContext.KeyValues on u.idStatus equals kv.idKeyValue

                                           
                                           join e in dbContext.Employees on p.idPerson equals e.idPerson into grEmployees
                                           from employee in grEmployees.DefaultIfEmpty()

                                           select new UserDataView
                                           {
                                               IdUser = u.idUser,
                                               UserName = u.UserName,
                                               FirstName = p.FirstName,
                                               SecondName = p.SecondName,
                                               LastName = p.LastName,
                                               EGN = p.EGN,
                                               IdentityNumber = p.IdentityNumber,
                                               idStatus = u.idStatus,
                                               Status = kv.Name,
                                               Description = u.Description,
                                               IsEmployee = (employee != null) ? true : false,
                                               PositionName = (position != null) ? position.Name : string.Empty


                                           }).ApplySearchCriterias(searchCriteria).ToList<UserDataView>();

            listView = BaseClassBL<UserDataView>.Sort(listView, sortExpression, sortDirection).ToList();


            List<UserRoleLink> listUserRoleLink = this.dbContext.UserRoleLinks.ToList();
            List<Role> listRole = this.dbContext.Roles.ToList();

            StringBuilder sb = new StringBuilder();

            foreach (UserDataView item in listView)
            {
                item.UserRoleLinkList = listUserRoleLink.Where(u => u.idUser == item.IdUser).ToList();

                sb.Clear();

                foreach (UserRoleLink link in item.UserRoleLinkList)
                {


                    sb.Append(listRole.FirstOrDefault(r => r.idRole == link.idRole).Name).Append(", ");
                }

                if (sb.Length > 0)
                {
                    item.Roles = sb.Remove(sb.Length - 2, 2).ToString();
                }
                else
                {
                    item.Roles = sb.ToString();
                }
            }




            return listView;
        }
        



        internal Person GetCurrentPersonByUserId(int userId)
        {
            Person person = null;

            User user = GetEntityById(userId);

            if (user != null)
            {
                KeyValue status = dbContext.KeyValues.Where(k => k.idKeyValue == user.idStatus).FirstOrDefault();

                if (status.KeyValueIntCode == "Active" || status.KeyValueIntCode == "Inactive")
                {
                    person = new PersonBL().GetEntityById(user.idPerson);
                }
                else if (status.KeyValueIntCode == "TemporarilyOffline" && user.idAltPerson.HasValue)
                {
                    person = new PersonBL().GetEntityById(user.idAltPerson.Value);
                }
            }

            return person;
        }

        public override EventLog GetEventLogInfo(CallContext outputContext, User savedEntity)
        {
            if (outputContext.securitySettings == ETEMEnums.SecuritySettings.Login)
            {
                Person currentPerson = new UserBL().GetCurrentPersonByUserId(savedEntity.EntityID);

                EventLog eventLog = new EventLog();
                eventLog.idUser = savedEntity.EntityID;
                eventLog.EventDate = DateTime.Now;
                eventLog.EventMessage = outputContext.Message;
                eventLog.EventAction = outputContext.securitySettings.ToString();
                eventLog.EntityName = savedEntity.GetType().FullName;
                eventLog.PersonName = (currentPerson != null) ? currentPerson.FullName : savedEntity.UserName;


                return eventLog;
            }
            else
            {
                return base.GetEventLogInfo(outputContext, savedEntity);
            }
        }

        internal void DecryptAllUserPassword()
        {
            List<User> list = this.dbContext.Users.ToList();

            CallContext resultContext = new CallContext();

            foreach (User item in list)
            {

                item.Password = ETEMModel.Helpers.BaseHelper.Decrypt(System.Web.HttpUtility.UrlDecode(item.Password));
                item.AltPassword = ETEMModel.Helpers.BaseHelper.Decrypt(System.Web.HttpUtility.UrlDecode(item.AltPassword));
                EntitySave<User>(item, resultContext);
            }
        }

        internal void EncryptAllUserPassword()
        {
            List<User> list = this.dbContext.Users.ToList();

            CallContext resultContext = new CallContext();

            foreach (User item in list)
            {

                item.Password = ETEMModel.Helpers.BaseHelper.Encrypt(item.Password);
                item.AltPassword = ETEMModel.Helpers.BaseHelper.Encrypt(item.AltPassword);

                EntitySave<User>(item, resultContext);
            }
        }

        internal void SendMailPassword(User currentUser)
        {
            KeyValue kvActiveStatus = new KeyValueBL().GetKeyValueByIntCode("UserStatus", "Active");
            KeyValue kvTemporarilyInactiveStatus = new KeyValueBL().GetKeyValueByIntCode("UserStatus", "TemporarilyInactive");

            Person person = null;
            string password = string.Empty;

            if (currentUser.idStatus == kvActiveStatus.idKeyValue)
            {
                person = new PersonBL().GetPersonByPersonID(currentUser.idPerson.ToString());
                password = ETEMModel.Helpers.BaseHelper.Decrypt(System.Web.HttpUtility.UrlDecode(currentUser.Password));
            }
            else if (currentUser.idStatus == kvTemporarilyInactiveStatus.idKeyValue)
            {
                person = new PersonBL().GetPersonByPersonID(currentUser.idAltPerson.ToString());
                password = ETEMModel.Helpers.BaseHelper.Decrypt(System.Web.HttpUtility.UrlDecode(currentUser.AltPassword));
            }
            else
            {
                return;
            }





            CommonBL commonBL = new CommonBL();

            CallContext callContext = new CallContext();

            if (callContext.ListKvParams.Where(w => w.Key == ETEMEnums.AppSettings.MailServer.ToString()).Count() == 0)
            {
                callContext.ListKvParams.Add(new KeyValuePair<string, object>(ETEMEnums.AppSettings.MailServer.ToString(),
                                                  commonBL.GetSettingByCode(ETEMEnums.AppSettings.MailServer).SettingValue));
            }
            if (callContext.ListKvParams.Where(w => w.Key == ETEMEnums.AppSettings.MailServerPort.ToString()).Count() == 0)
            {
                callContext.ListKvParams.Add(new KeyValuePair<string, object>(ETEMEnums.AppSettings.MailServerPort.ToString(),
                                                  commonBL.GetSettingByCode(ETEMEnums.AppSettings.MailServerPort).SettingValue));
            }
            if (callContext.ListKvParams.Where(w => w.Key == ETEMEnums.AppSettings.MailFromPassword.ToString()).Count() == 0)
            {
                callContext.ListKvParams.Add(new KeyValuePair<string, object>(ETEMEnums.AppSettings.MailFromPassword.ToString(),
                                                  commonBL.GetSettingByCode(ETEMEnums.AppSettings.MailFromPassword).SettingValue));
            }




            string body = string.Format(BaseHelper.GetCaptionString("Entity_User_Send_Password_Body"),
                                person.TwoNamesPlusTitle,
                                currentUser.UserName,
                                password
                                );

            commonBL.SendMailAction(
                commonBL.GetSettingByCode(ETEMEnums.AppSettings.DefaultEmail).SettingValue,
                person.EMail,
                BaseHelper.GetCaptionString("Entity_User_Send_Password_Subject"),
                body,
                "Системен e-mail", new List<string>(), callContext);


        }

        

        public string getUserNameByFirstLastName(string firstName, string lastName)
        {
            string _firstName = BaseHelper.ConvertCyrToLatin(firstName);
            string _lastName = BaseHelper.ConvertCyrToLatin(lastName);

            string userName = _firstName.Substring(0, 1).ToLower() + "." + _lastName.ToLower();

            userName = getUniqueUserName(userName);

            return userName;
        }

        public string getUniqueUserName(string userName)
        {
            string uniqueUserName = "";

            int count = dbContext.Users.Count(u => u.UserName == userName);

            if (count >= 1)
            {
                uniqueUserName = userName + new SequenceBL().GetSequenceNextValue("SYSTEMUSER#" + userName, null, null, new CallContext());
            }
            else
            {
                uniqueUserName = userName;
            }

            return uniqueUserName;
        }

        internal CallContext UserSendingEmails(ICollection<AbstractSearch> searchCriteria, List<int> listSelectedIDs, SendMailHelper sendMailData, CallContext resultContext)
        {
            resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
            resultContext.Message = string.Empty;
            resultContext.Result = string.Empty;

            try
            {
                List<UserDataView> listUserDataViews = new List<UserDataView>();

                List<SendMailHelper> listEmailsForSending = new List<SendMailHelper>();


                listUserDataViews = (from u in this.dbContext.Users
                                     join p in this.dbContext.Persons on u.idPerson equals p.idPerson
                                     where listSelectedIDs.Contains(u.idUser)
                                     select new UserDataView
                                     {
                                         Title = p.Title,
                                         FirstName = p.FirstName,
                                         SecondName = p.SecondName,
                                         LastName = p.LastName,
                                         EMail = p.EMail,
                                         Password = u.Password,
                                         UserName = u.UserName,
                                     }).ToList();



                var pattern = @"{(.*?)}";
                var matches = Regex.Matches(sendMailData.BodyBG, pattern);



                foreach (UserDataView userDataView in listUserDataViews)
                {
                    SendMailHelper sendMailHelper = new SendMailHelper()
                    {
                        FullName = userDataView.TwoNamesPlusTitle,
                        EmailTo = userDataView.EMail.Trim(),
                        SubjectBG = sendMailData.SubjectBG,
                        BodyBG = (matches.Count == 3) ? string.Format(
                                                        sendMailData.BodyBG,
                                                        userDataView.TwoNamesPlusTitle,
                                                        userDataView.UserName,
                                                        BaseHelper.Decrypt(System.Web.HttpUtility.UrlDecode(userDataView.Password))) : sendMailData.BodyBG
                    };

                    listEmailsForSending.Add(sendMailHelper);
                }

                resultContext = new CommonBL().SendMail(listEmailsForSending, ETEMEnums.EmailTypeEnum.Users, resultContext);
            }
            catch (Exception ex)
            {
                resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                resultContext.Message = BaseHelper.GetCaptionString("Form_SpecialityForApplication_Error_StudentCandidates_Sending_Emails");

                BaseHelper.Log("Грешка при изпращане на имейл известия!");
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);


            }

            return resultContext;
        }

        internal void DisableUserAccountByPersonID(int idPerson)
        {
            List<User> listUser = this.dbContext.Users.Where(u => u.idPerson == idPerson).ToList();
            KeyValue kvStatusDisable = new KeyValueBL().GetKeyValueByIntCode("UserStatus", "Inactive");
            foreach (User user in listUser)
            {
                user.idStatus = kvStatusDisable.idKeyValue;
                EntitySave<User>(user, new CallContext());
            }

        }

        internal User GetUserByUsername(string username)
        {
            return this.dbContext.Users.Where(e => e.UserName == username).FirstOrDefault();
        }

        internal CallContext ChangeUserForgottenPasswordPassword(User user, Person person, CallContext callcontext)
        {
            string newPassword = System.Web.Security.Membership.GeneratePassword(Constants.NEW_PASSWORD_LENGTH, Constants.NUMBER_Non_Alphanumeric_Characters);
            user.Password = ETEMModel.Helpers.BaseHelper.Encrypt(System.Web.HttpUtility.UrlDecode(newPassword));
            callcontext = new UserBL().EntitySave<User>(user, callcontext);

            List<SendMailHelper> listEmailsForSending = new List<SendMailHelper>();
            SendMailHelper sendMailHelper = new SendMailHelper()
                  {
                      FullName = person.TwoNamesPlusTitle,
                      EmailTo = person.EMail.Trim(),
                      SubjectBG = "Нова парола за административната системата на НХА",
                      BodyBG = "Новата ви парола е: '" + newPassword + "'. След като влезете в системата може да я смените от настройките на профила (иконката горе в дясно). "
                  };

            listEmailsForSending.Add(sendMailHelper);

            //becouse i am not logged and those are not set
            //callcontext.ListKvParams.Add(new KeyValuePair<string, object>(UMSEnums.AppSettings.SendExternalMail.ToString(),
            //                               new CommonBL().GetSettingByCode(UMSEnums.AppSettings.SendExternalMail).SettingValue));
            //callcontext.ListKvParams.Add(new KeyValuePair<string, object>(UMSEnums.AppSettings.DefaultEmail.ToString(),
            //                                     new CommonBL().GetSettingByCode(UMSEnums.AppSettings.DefaultEmail).SettingValue));
            new CommonBL().SetDetaultKvParams(callcontext);


            callcontext = new CommonBL().SendMail(listEmailsForSending, ETEMEnums.EmailTypeEnum.Users, callcontext);

            return callcontext;
        }
    }

    public class DomainUserInfo
    {
        public string ShortName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
    }
}