using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETEMModel.Helpers
{
    public interface Identifiable
    {
        int EntityID { get; }
        List<string> ValidateEntity(CallContext outputContext);
        string ValidationErrorsAsString { get; set; }
    }
}
