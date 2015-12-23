using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
//using log4net;
using ETEMModel.Models;
using ETEMModel.Helpers;
using ETEMModel.Services.Admin;
using ETEMModel.Services.Common;
using ETEMModel.Services.CostCalculation;
using System.Text;
using System.Web.UI.WebControls;
using ETEMModel.Services.Employe;
using NLog;
using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Models.DataView.Admin;
using ETEMModel.Models.DataView;

namespace ETEM.Freamwork
{
    public class GeneralPage : Page
    {
        // private static ILog log = LogManager.GetLogger(typeof(GeneralPage));



        //public AdminServiceReference.AdministrationClient AdminClientRef;
        public ETEMModel.Services.Admin.Administration AdminClientRef;
        
        public ETEMModel.Services.Common.Common CommonClientRef;
        public ETEMModel.Services.Employe.EmployeRef EmployeClientRef;
        public ETEMModel.Services.CostCalculation.CostCalculationRef CostCalculationRef;
        public ETEMModel.Helpers.BaseHelper BaseHelperObj;
        protected UserProps userProps = null;
        public FormContext FormContext { get; set; }
        public CallContext CallContext { get; set; }

        public static HttpContext CurrentHttpContext { get { return HttpContext.Current; } }

        #region NLog
        private static Logger logger = LogManager.GetLogger("GeneralPage");
        public static void LogDebug(string message)
        {

            logger.Debug(DateTime.Now.ToString() + GetUserSystemInfo() + "\t" + "\tMessage: " + message);
        }

        public static void LogError(string message)
        {

            logger.Error(DateTime.Now.ToString() + GetUserSystemInfo() + "\t" + "\tMessage: " + message);
        }

        public static void LogTrace(string message)
        {

            logger.Trace(DateTime.Now.ToString() + GetUserSystemInfo() + "\t" + "\tMessage: " + message);
        }
        #endregion

        public static string GetIPAddress()
        {

            var wrapper = new HttpRequestWrapper(CurrentHttpContext.Request);

            return ClientIP.ClientIPFromRequest(wrapper, false);
        }

        protected override void OnInit(EventArgs e)
        {
            LogDebug("GeneralPage OnInit");
            this.BaseHelperObj = new ETEMModel.Helpers.BaseHelper();
            this.AdminClientRef = new Administration();
            this.CommonClientRef = new Common();
            this.EmployeClientRef = new EmployeRef();
            this.CostCalculationRef = new ETEMModel.Services.CostCalculation.CostCalculationRef();

            this.CallContext = new CallContext();

            this.FormContext = new FormContext();
            this.FormContext.RequestMeasure = new RequestMeasure(this.Page.GetType().FullName);

            this.FormContext.UserProps = this.UserProps;

            this.FormContext.DictionaryKeyValue = DictionaryKeyValue;
            this.FormContext.DictionaryKeyType = DictionaryKeyType;
            this.FormContext.DictionarySetting = DictionarySetting;

            AdminClientRef.DictionaryKeyType = DictionaryKeyType;
            AdminClientRef.DictionaryKeyValue = DictionaryKeyValue;


            this.FormContext.DictionaryPermittedActionSetting = DictionaryPermittedActionSetting;

            //Функционалност за разкрептиране на криптираните ключове в QueryString_а
            if (Request.QueryString.Count > 0)
            {
                string rawQueryString = System.Web.HttpUtility.UrlDecode(Request.RawUrl.Substring(Request.RawUrl.IndexOf('?') + 1));

                string queryString = "";

                if (rawQueryString.IndexOf("Cript=false") == -1)
                {
                    queryString = ETEMModel.Helpers.BaseHelper.Decrypt(rawQueryString);
                }
                else
                {
                    queryString = rawQueryString;
                }

                string val;
                string key;

                string[] keys_values = queryString.Split('&');

                foreach (string key_value in keys_values)
                {
                    string[] kv = key_value.Split('=');

                    key = kv[0];
                    val = kv[1];

                    this.FormContext.QueryString.Add(key, val);
                }
            }

            if (userProps != null)
            {
                userProps.LastRequestDateTime = DateTime.Now;
                userProps.Page = this.Page.GetType().FullName;
            }



            base.OnInit(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            if (this.FormContext != null && this.FormContext.RequestMeasure != null)
            {
                LogDebug(this.FormContext.RequestMeasure.ToString());
            }
            base.OnUnload(e);
        }

        /// <summary>
        /// Върща системат информация на потребителя
        /// IP, Сесия, Име на потребител и т.н
        /// </summary>
        /// <returns></returns>
        private static string GetUserSystemInfo()
        {
            StringBuilder result = new StringBuilder();

            try
            {
                result.Append("\tIP:");
                if (CurrentHttpContext != null && CurrentHttpContext.Request != null && CurrentHttpContext.Request.UserHostAddress != null)
                {

                    result.Append(CurrentHttpContext.Request.UserHostAddress);
                }
                else
                {
                    result.Append("none");
                }

                result.Append("\tInternal IP:" + GetIPAddress());


                result.Append("\tSessionID:");
                if (CurrentHttpContext.Session != null)
                {

                    result.Append(CurrentHttpContext.Session.SessionID);
                }
                else
                {
                    result.Append("none");
                }


                UserProps userProps = CurrentHttpContext.Session[ETEMModel.Helpers.Constants.SESSION_USER_PROPERTIES] as UserProps;
                if (userProps != null)
                {
                    result.Append("\tUserName:");
                    result.Append(userProps.UserName);
                    result.Append("\tPersonName:");
                    result.Append(userProps.PersonNamePlusTitle);
                }
                else
                {
                    result.Append("\tUserName:");
                    result.Append("none");
                    result.Append("\tPersonName:");
                    result.Append("none");
                }
            }
            catch { }



            return result.ToString();
        }

        public static Dictionary<int, KeyValue> DictionaryKeyValue
        {
            get
            {
                Dictionary<int, KeyValue> dictionaryKeyValue = new Dictionary<int, KeyValue>();

                if (CurrentHttpContext != null && CurrentHttpContext.Application[Constants.APPLICATION_KEYVALUE_LIST] != null)
                {
                    dictionaryKeyValue = (Dictionary<int, KeyValue>)CurrentHttpContext.Application[Constants.APPLICATION_KEYVALUE_LIST];
                    return dictionaryKeyValue;
                }
                else
                {
                    Administration adminClientRef = new Administration();
                    //AdminServiceReference.AdministrationClient adminClientRef = new AdministrationClient();

                    dictionaryKeyValue = new Dictionary<int, KeyValue>();

                    List<KeyValue> list = adminClientRef.GetAllKeyValue().ToList();

                    foreach (KeyValue key in list)
                    {
                        dictionaryKeyValue.Add(key.idKeyValue, key);
                    }

                    //if runs in another thred CurrentHttpContext is null
                    if (CurrentHttpContext != null)
                    {
                        CurrentHttpContext.Application.Add(Constants.APPLICATION_KEYVALUE_LIST, dictionaryKeyValue);
                        LogDebug("KeyValue was loaded in application");
                    }

                    return dictionaryKeyValue;
                }
            }
        }

        public static Dictionary<int, PermittedAction> DictionaryPermittedActionSetting
        {
            get
            {
                Dictionary<int, PermittedAction> dictionaryPermittedAction = new Dictionary<int, PermittedAction>();

                if (CurrentHttpContext != null && CurrentHttpContext.Application[Constants.APPLICATION_PERMITTEDACTION_LIST] != null)
                {
                    dictionaryPermittedAction = (Dictionary<int, PermittedAction>)CurrentHttpContext.Application[Constants.APPLICATION_PERMITTEDACTION_LIST];
                    return dictionaryPermittedAction;
                }
                else
                {
                    Administration adminClientRef = new Administration();
                    //AdminServiceReference.AdministrationClient adminClientRef = new AdministrationClient();

                    dictionaryPermittedAction = new Dictionary<int, PermittedAction>();

                    List<PermittedAction> list = adminClientRef.GetAllPermittedActions("", "").ToList();

                    foreach (PermittedAction key in list)
                    {
                        dictionaryPermittedAction.Add(key.idPermittedAction, key);
                    }

                    if (CurrentHttpContext != null)
                    {
                        CurrentHttpContext.Application.Add(Constants.APPLICATION_PERMITTEDACTION_LIST, dictionaryPermittedAction);
                        LogDebug("PermittedAction was loaded in application");
                    }
                    return dictionaryPermittedAction;
                }
            }
        }

        public List<KeyValue> GetAllKeyValueByKeyTypeIntCode(string keyTypeIntCode)
        {
            List<KeyValue> list = new List<KeyValue>();

            KeyType keyType = DictionaryKeyType.Where(d => d.Value.KeyTypeIntCode == keyTypeIntCode).FirstOrDefault().Value;

            foreach (var item in DictionaryKeyValue.Where(k => k.Value.idKeyType == keyType.idKeyType).OrderBy(o => o.Value.Name).ToList())
            {
                list.Add(item.Value);
            }

            return list;
        }

        public List<KeyValue> GetAllKeyValueByKeyTypeIntCode(string keyTypeIntCode, string columnToOrderBy)
        {
            List<KeyValue> list = new List<KeyValue>();

            KeyType keyType = DictionaryKeyType.Where(d => d.Value.KeyTypeIntCode == keyTypeIntCode).FirstOrDefault().Value;

            var data = DictionaryKeyValue.Where(k => k.Value.idKeyType == keyType.idKeyType).OrderBy(o => o.Value.Name).Select(s => s.Value).ToList();

            data = BaseClassBL<KeyValue>.Sort(data, columnToOrderBy, Constants.SORTING_ASC).ToList();

            foreach (var item in data)
            {
                list.Add(item);
            }

            return list;
        }


        public KeyValue GetKeyValueByIntCode(string keyTypeIntCode, string keyValueIntCode)
        {
            KeyValue keyValue = new KeyValue();

            keyValue = (from kv in DictionaryKeyValue.Values
                        join kt in DictionaryKeyType.Values on kv.idKeyType equals kt.idKeyType
                        where kt.KeyTypeIntCode == keyTypeIntCode && kv.KeyValueIntCode == keyValueIntCode
                        select kv).FirstOrDefault();

            return keyValue;
        }

        public KeyValue GetKeyValueByID(int idKeyValue)
        {
            KeyValue keyValue = new KeyValue();

            keyValue = (from kv in DictionaryKeyValue.Values
                        where kv.idKeyValue == idKeyValue
                        select kv).FirstOrDefault();

            return keyValue;
        }


        
        public static Dictionary<int, KeyType> DictionaryKeyType
        {
            get
            {
                Dictionary<int, KeyType> dictionaryKeyType;
                if (CurrentHttpContext != null && CurrentHttpContext.Application[Constants.APPLICATION_KEYTYPE_LIST] != null)
                {
                    dictionaryKeyType = (Dictionary<int, KeyType>)CurrentHttpContext.Application[Constants.APPLICATION_KEYTYPE_LIST];
                    return dictionaryKeyType;
                }
                else
                {
                    //AdminServiceReference.AdministrationClient adminClientRef = new AdministrationClient();
                    Administration adminClientRef = new Administration();
                    dictionaryKeyType = new Dictionary<int, KeyType>();

                    List<KeyType> list = adminClientRef.GetAllKeyTypes("Name", Constants.SORTING_ASC).ToList();

                    foreach (KeyType key in list)
                    {
                        dictionaryKeyType.Add(key.idKeyType, key);
                    }

                    if (CurrentHttpContext != null)
                    {
                        CurrentHttpContext.Application.Add(Constants.APPLICATION_KEYTYPE_LIST, dictionaryKeyType);
                        LogDebug("KeyType was loaded in application");
                    }

                    return dictionaryKeyType;
                }
            }
        }

        /// <summary>
        /// Презарежда всички KeyValue в Application
        /// </summary>
        public void ReloadKeyValueInApplication()
        {
            Dictionary<int, KeyValue> dictionaryKeyValue = new Dictionary<int, KeyValue>();

            List<KeyValue> list = this.AdminClientRef.GetAllKeyValue().ToList();

            foreach (KeyValue key in list)
            {
                dictionaryKeyValue.Add(key.idKeyValue, key);
            }

            this.Application.Lock();
            this.Application.Remove(Constants.APPLICATION_KEYVALUE_LIST);
            this.Application.Add(Constants.APPLICATION_KEYVALUE_LIST, dictionaryKeyValue);
            this.Application.UnLock();
            LogDebug("KeyValue was reloaded in application");
        }

        /// <summary>
        /// Презарежда всички KeyType в Application
        /// </summary>
        public void ReloadKeyTypeInApplication()
        {
            Dictionary<int, KeyType> dictionaryKeyType = new Dictionary<int, KeyType>();

            List<KeyType> list = this.AdminClientRef.GetAllKeyTypes("Name", Constants.SORTING_ASC).ToList();

            foreach (KeyType key in list)
            {
                dictionaryKeyType.Add(key.idKeyType, key);
            }

            this.Application.Lock();
            this.Application.Remove(Constants.APPLICATION_KEYTYPE_LIST);
            this.Application.Add(Constants.APPLICATION_KEYTYPE_LIST, dictionaryKeyType);
            this.Application.UnLock();
            LogDebug("KeyType was reloaded in application");
        }


        private List<FormResources> listFormResources;

        public List<FormResources> ListFormResources
        {
            get
            {
                if (Application[ETEMModel.Helpers.Constants.APPLICATION_FORM_RESOURCES] != null)
                {
                    this.listFormResources = (List<FormResources>)Application[ETEMModel.Helpers.Constants.APPLICATION_FORM_RESOURCES];
                    return this.listFormResources;
                }
                else
                {
                    this.listFormResources = new List<FormResources>();

                    //this.listFormResources.Add(ETEM.Admin.UserList.);

                    this.Application.Add(ETEMModel.Helpers.Constants.APPLICATION_FORM_RESOURCES, this.listFormResources);
                    return this.listFormResources;
                }
            }
        }

        public UserProps UserProps
        {
            get
            {
                if (Session[ETEMModel.Helpers.Constants.SESSION_USER_PROPERTIES] != null)
                {
                    this.userProps = (UserProps)Session[ETEMModel.Helpers.Constants.SESSION_USER_PROPERTIES];

                    return this.userProps;
                }
                else
                {
                    return null;
                }
            }
        }

        public bool IsSUPPORT()
        {
            return this.UserProps.Roles.Any(a => a.Name == "SUPPORT");
        }

        /// <summary>
        /// Презарежда всички `Настройки` в Application
        /// </summary>
        public void ReloadSettingApplication()
        {
            Dictionary<string, Setting> dictionarySetting = new Dictionary<string, Setting>();

            List<Setting> list = this.AdminClientRef.GetAllSettings("", "", new List<AbstractSearch>()).ToList();

            foreach (Setting key in list)
            {
                dictionarySetting.Add(key.SettingIntCode, key);
            }

            this.Application.Lock();
            this.Application.Remove(Constants.APPLICATION_SETTING_LIST);
            this.Application.Add(Constants.APPLICATION_SETTING_LIST, dictionarySetting);
            this.Application.UnLock();
            LogDebug("Setting was reloaded in application");
        }

        public static Setting GetSettingByCode(ETEMEnums.AppSettings appSettings)
        {

            Setting setting = DictionarySetting.Where(k => k.Key == appSettings.ToString()).FirstOrDefault().Value;



            return setting;
        }

        public static Dictionary<string, MenuNodeDataView> DictionaryMenuNodes
        {
            get
            {


                Dictionary<string, MenuNodeDataView> dictionaryMenuNodes;

                if (CurrentHttpContext != null && CurrentHttpContext.Application[Constants.APPLICATION_MENU_NODE_LIST] != null)
                {
                    dictionaryMenuNodes = (Dictionary<string, MenuNodeDataView>)CurrentHttpContext.Application[Constants.APPLICATION_MENU_NODE_LIST];
                    return dictionaryMenuNodes;
                }
                else
                {

                    //AdminServiceReference.AdministrationClient adminClientRef = new AdministrationClient();

                    Administration adminClientRef = new Administration();
                    dictionaryMenuNodes = new Dictionary<string, MenuNodeDataView>();
                    ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();
                    List<MenuNodeDataView> list = adminClientRef.GetAllMenuNode(new CallContext()).ToList();

                    foreach (MenuNodeDataView key in list)
                    {
                        dictionaryMenuNodes.Add(key.idNode.ToString(), key);
                    }

                    if (CurrentHttpContext != null)
                    {

                        CurrentHttpContext.Application.Add(Constants.APPLICATION_MENU_NODE_LIST, dictionaryMenuNodes);

                        LogDebug("MenuNodeDataView was loaded in application");
                    }
                    return dictionaryMenuNodes;
                }
            }
        }


        public void ReloadMenuNodeDataViewApplication()
        {
            Dictionary<string, MenuNodeDataView> dictionaryMenuNodes = new Dictionary<string, MenuNodeDataView>();

            Administration adminClientRef = new Administration();
            ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();
            List<MenuNodeDataView> list = adminClientRef.GetAllMenuNode(new CallContext()).ToList();

            foreach (MenuNodeDataView key in list)
            {
                dictionaryMenuNodes.Add(key.idNode.ToString(), key);
            }


            this.Application.Lock();
            this.Application.Remove(Constants.APPLICATION_MENU_NODE_LIST);
            this.Application.Add(Constants.APPLICATION_MENU_NODE_LIST, dictionaryMenuNodes);
            this.Application.UnLock();
            LogDebug("MenuNodeDataView was reloaded in application");
        }


        public static Dictionary<string, ModuleDataView> DictionaryModules
        {
            get
            {


                Dictionary<string, ModuleDataView> dictionaryModule;

                if (CurrentHttpContext != null && CurrentHttpContext.Application[Constants.APPLICATION_MODULES_LIST] != null)
                {
                    dictionaryModule = (Dictionary<string, ModuleDataView>)CurrentHttpContext.Application[Constants.APPLICATION_MODULES_LIST];
                    return dictionaryModule;
                }
                else
                {

                    //AdminServiceReference.AdministrationClient adminClientRef = new AdministrationClient();

                    Administration adminClientRef = new Administration();
                    dictionaryModule = new Dictionary<string, ModuleDataView>();
                    ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();
                    List<ModuleDataView> list = adminClientRef.GetAllModule(searchCriteria, "", Constants.SORTING_ASC).ToList();

                    foreach (ModuleDataView key in list)
                    {
                        dictionaryModule.Add(key.ModuleSysName, key);
                    }

                    if (CurrentHttpContext != null)
                    {
                        CurrentHttpContext.Application.Add(Constants.APPLICATION_MODULES_LIST, dictionaryModule);
                        LogDebug("Modules was loaded in application");
                    }
                    return dictionaryModule;
                }
            }
        }

        public void ReloadModuleDataViewApplication()
        {
            Dictionary<string, ModuleDataView> dictionaryModule = new Dictionary<string, ModuleDataView>();

            Administration adminClientRef = new Administration();
            ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();
            List<ModuleDataView> list = adminClientRef.GetAllModule(searchCriteria, "", Constants.SORTING_ASC).ToList();

            foreach (ModuleDataView key in list)
            {
                dictionaryModule.Add(key.ModuleSysName, key);
            }


            this.Application.Lock();
            this.Application.Remove(Constants.APPLICATION_MODULES_LIST);
            this.Application.Add(Constants.APPLICATION_MODULES_LIST, dictionaryModule);
            this.Application.UnLock();
            LogDebug("Modules was reloaded in application");
        }


        public static Dictionary<string, Setting> DictionarySetting
        {
            get
            {


                Dictionary<string, Setting> dictionarySetting;

                if (CurrentHttpContext != null && CurrentHttpContext.Application[Constants.APPLICATION_SETTING_LIST] != null)
                {
                    dictionarySetting = (Dictionary<string, Setting>)CurrentHttpContext.Application[Constants.APPLICATION_SETTING_LIST];
                    return dictionarySetting;
                }
                else
                {

                    //AdminServiceReference.AdministrationClient adminClientRef = new AdministrationClient();

                    Administration adminClientRef = new Administration();
                    dictionarySetting = new Dictionary<string, Setting>();

                    List<Setting> list = adminClientRef.GetAllSettings("", "", new List<AbstractSearch>()).ToList();

                    foreach (Setting key in list)
                    {
                        dictionarySetting.Add(key.SettingIntCode, key);
                    }

                    //becouse throws an exeption if runs in another thred
                    if (CurrentHttpContext != null)
                    {
                        CurrentHttpContext.Application.Add(Constants.APPLICATION_SETTING_LIST, dictionarySetting);
                        LogDebug("Setting was loaded in application");
                    }

                    return dictionarySetting;
                }
            }
        }
        public static string GetCaption(string key)
        {
            return ETEMModel.Helpers.BaseHelper.GetCaptionString(key);
        }






        /// <summary>
        /// Връща месеца като номер 01, 02, 03, ...12
        /// </summary>
        /// <param name="idKeyValue"></param>
        /// <returns></returns>
        public string GetMonthFromKeyValueByID(int idKeyValue)
        {
            KeyValue keyValue = new KeyValue();

            string result = string.Empty;

            keyValue = (from kv in DictionaryKeyValue.Values
                        where kv.idKeyValue == idKeyValue
                        select kv).FirstOrDefault();

            if (keyValue.V_Order.HasValue)
            {
                result = keyValue.V_Order.Value.ToString();
            }

            if (result.Count() == 1)
            {
                result = "0" + result;
            }

            return result;
        }

    }
}