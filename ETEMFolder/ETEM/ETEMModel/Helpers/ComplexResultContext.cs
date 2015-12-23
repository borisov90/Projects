using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers
{
    public class ComplexResultContext<T>
    {
        public CallContext CallContext { get; set; }
        public string ListDataParentId { get; set; }
        public List<T> ListData { get; set; }
    }
}