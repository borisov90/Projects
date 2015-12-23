using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers
{
    public class ETEMModelException : Exception
    {
        public ETEMModelException(string message)
            : base(message)
        {

        }

        public ETEMModelException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}