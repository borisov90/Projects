using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using System.Web.Services;

namespace ETEM.Admin
{
    public partial class SessionClear : BasicPage
    {

        public static FormResources formResource = new FormResources
        {
            Module = Constants.MODULE_ADMIN,
            PageFullName = Constants.SessionClear,
            PagePath = "../Admin/SessionClear.aspx"

        };

        public override string CurrentPagePath()
        {
            return formResource.PagePath;
        }

        public override string CurrentPageFullName()
        {
            return formResource.PageFullName;
        }
        public override FormResources CurrentFormResources()
        {
            return formResource;
        }
        public override string CurrentModule()
        {
            return formResource.Module;
        }


        public override void SetPageCaptions()
        {

        }

        public override void FormLoad()
        {
            base.FormLoad();
        }



        [WebMethod]
        public void ClearSession(string controlName)
        {
            Session.RemoveAll();
            Session.Abandon();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Session.Abandon();
        }
    }
}