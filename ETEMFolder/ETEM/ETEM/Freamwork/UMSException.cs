using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEM.Freamwork
{
    public class UMSException : Exception
    {

        public string CustomMessage { get; set; }

        public UMSException(string message)
            : base(message)
        {
            CustomMessage = message;
        }

        public UMSException(string message, Exception innerException)
            : base(message, innerException)
        {
            CustomMessage = message;
        }
    }
}