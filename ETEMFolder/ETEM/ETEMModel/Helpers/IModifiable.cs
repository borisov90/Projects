using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETEMModel.Helpers
{
    public interface IModifiable
    {
        void SetCreationData(CallContext outputContext);
        void SetModificationData(CallContext outputContext);
    }
}
