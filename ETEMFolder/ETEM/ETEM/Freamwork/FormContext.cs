using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using ETEMModel.Models;
using ETEMModel.Helpers;
using ETEMModel.Helpers.Admin;

namespace ETEM.Freamwork
{
    public class FormContext
    {
        private UserProps userProps;
        private Hashtable queryString;
        private string currentLanguage = null;
        

        public FormContext()
        {
            this.queryString = new Hashtable();

        }
        public string CurrentLanguage
        {
            get { return currentLanguage; }
            set { currentLanguage = value; }
        }

        public Hashtable QueryString
        {
            get { return queryString; }
            set { queryString = value; }
        }

        public UserProps UserProps
        {
            get { return userProps; }
            set { userProps = value; }
        }

        public RequestMeasure RequestMeasure { get; set; }

        private List<ETEMModel.Helpers.SystemError> systemError;

        public List<ETEMModel.Helpers.SystemError> SystemErrors
        {
            get { return systemError; }
            set { systemError = value; }
        }

        private List<ETEMModel.Helpers.SystemMessage> systemMessage;

        public List<ETEMModel.Helpers.SystemMessage> SystemMessages
        {
            get { return systemMessage; }
            set { systemMessage = value; }
        }

        public Dictionary<int, KeyValue> DictionaryKeyValue
        {
            get;
            set;
        }

        public Dictionary<int, KeyType> DictionaryKeyType
        {
            get;
            set;
        }

        public Dictionary<string, Setting> DictionarySetting
        {
            get;
            set;
        }

        public Dictionary<int, PermittedAction> DictionaryPermittedActionSetting
        {
            get;
            set;
        }
    }



}