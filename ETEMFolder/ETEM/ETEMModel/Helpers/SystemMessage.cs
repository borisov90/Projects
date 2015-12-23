using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers
{
    public class SystemMessage
    {
        string messageKey;
        string message = "";

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public string MessageKey
        {
            get { return messageKey; }
            set { messageKey = value; }
        }
        public SystemMessage()
        {
            this.messageKey = Constants.NOT_DEFINE;
        }
        public SystemMessage(string key)
        {
            this.messageKey = key;
        }

        public SystemMessage(string key, string message)
        {
            this.messageKey = key;
            this.message = message;
        }
    }
}