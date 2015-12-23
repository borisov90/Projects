using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers.Exceptions
{
    public class UMSCustomException : Exception
    {


        public string CustomMessage { get; set; }

        public UMSCustomException(string message)
            : base(message)
        {

        }

        public UMSCustomException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

    }
}