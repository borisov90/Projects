using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models.DataView
{
    public class MenuNodeDataView : MenuNode, DataViewInterface
    {
        public string URL { get; set; }
        public string Code { get; set; }
        public string QueryParams { get; set; }

        public int IdRole { get; set; }

        #region DataViewInterface Members

        string DataViewInterface.IdEntity
        {
            get { return this.idNode.ToString(); }
            set
            {
                this.idNode = Int32.Parse(value);
            }
        }

        #endregion
    }
}