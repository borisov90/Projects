using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Helpers;

namespace ETEMModel.Models
{
    public partial class RoleMenuNode : Identifiable
    {
        public int EntityID
        {
            get { throw new NotImplementedException(); }
        }

        public List<string> ValidateEntity(CallContext outputContext)
        {
            throw new NotImplementedException();
        }

        public string ValidationErrorsAsString
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}