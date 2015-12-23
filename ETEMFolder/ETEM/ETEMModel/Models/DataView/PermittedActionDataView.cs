using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models.DataView
{
    public class PermittedActionDataView : PermittedAction, DataViewInterface
    {
        #region DataViewInterface Members



        public int RolePermittedActionID { get; set; }


        string DataViewInterface.IdEntity
        {
            get { return this.idPermittedAction.ToString(); }
            set
            {
                this.idPermittedAction = Int32.Parse(value);
            }
        }

        public string ModuleName { get; set; }

        #endregion
    }
}