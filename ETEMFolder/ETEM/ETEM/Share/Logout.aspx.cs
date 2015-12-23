using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using System.Web.SessionState;

namespace ETEM.Share
{
    public partial class Logout : BasicPage
    {

        public static FormResources formResource = new FormResources
        {
            Module = Constants.MODULE_ADMIN,
            PageFullName = Constants.UMS_SHARE_LOGOUT,
            PagePath = "../Share/Logout.aspx"

        };

        protected void Page_Load(object sender, EventArgs e)
        {


            Dictionary<string, HttpSessionState> sessionData = (Dictionary<string, HttpSessionState>)Application[Constants.APPLICATION_ALL_SESSIONS];
            if (HttpContext.Current != null)
            {
                sessionData.Remove(HttpContext.Current.Session.SessionID);

            }
            Application[Constants.APPLICATION_ALL_SESSIONS] = sessionData;

            Session.RemoveAll();
            Session.Abandon();
            Response.Redirect(Share.Login.formResource.PagePath);
        }

        public override void FormLoad()
        {

        }

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
    }
}