﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models.DataView.Admin
{
    public class AllowIPDataView : AllowIP, DataViewInterface
    {
        public string IdEntity
        {
            get { return this.idAllowIP.ToString(); }
            set { this.IdEntity = value; }
        }
    }
}