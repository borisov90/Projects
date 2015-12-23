using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models.DataView.Admin
{
    public class ModuleDataView : Module, DataViewInterface
    {
        public string IdEntity
        {
            get { return this.idModule.ToString(); }
            set { this.IdEntity = value; }
        }
    }
}