using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models.DataView
{
    public class KeyValueDataView : KeyValue, DataViewInterface
    {
        #region DataViewInterface Members
        public string IdEntity
        {
            get { return this.idKeyValue.ToString(); }
            set { this.IdEntity = value; }
        }
        #endregion

        public string KeyTypeIntCode { get; set; }
    }
}