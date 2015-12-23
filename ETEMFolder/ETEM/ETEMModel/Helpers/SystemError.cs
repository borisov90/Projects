using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers
{
    public class SystemError
    {
        string errorKey;
        string message = "";

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public string ErrorKey
        {
            get { return errorKey; }
            set { errorKey = value; }
        }
        public SystemError()
        {
            this.errorKey = ETEMModel.Helpers.Constants.NOT_DEFINE;
        }
        public SystemError(string key)
        {
            this.errorKey = key;
        }
        public SystemError(string key, string message)
        {
            this.errorKey = key;
            this.message = message;
        }
    }
}