using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models.DataView
{
    public class KeyTypeDataView : KeyType, DataViewInterface
    {
        #region DataViewInterface Members

        public string IdEntity
        {
            get { return this.idKeyType.ToString(); }
            set
            {
                this.idKeyType = Int32.Parse(value);
            }
        }



        public string KeyValueNameStr { get; set; }
        public string KeyValueIntCodeStr { get; set; }
        public string KeyValueDescriptionStr { get; set; }

        #endregion
    }
}