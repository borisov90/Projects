using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace ETEMModel.Models.DataView
{
    public interface DataViewInterface
    {
        string IdEntity
        {
            get;
            set;
        }
    }
}
