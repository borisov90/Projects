using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models.DataView
{
    public class PersonHistoryDataView : PersonHistory
    {
        public string FullName
        {
            get
            {
                return
                        (string.IsNullOrEmpty(this.FirstName) ? "" : this.FirstName) +
                        (string.IsNullOrEmpty(this.SecondName) ? "" : " " + this.SecondName) +
                        (string.IsNullOrEmpty(this.LastName) ? "" : " " + this.LastName);
            }
        }
    }
}