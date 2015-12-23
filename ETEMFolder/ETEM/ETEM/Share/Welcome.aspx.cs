using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Models;
using ETEMModel.Helpers.Compator;
using ETEMModel.Models.DataView;
using System.Web.UI.HtmlControls;

namespace ETEM.Share
{
    public partial class Welcome : BasicPage
    {
        public static FormResources formResource = new FormResources
        {
            Module = Constants.MODULE_SHARE,
            PageFullName = Constants.UMS_SHARE_WELCOME,
            PagePath = "../Share/Welcome.aspx"


        };

        protected void Page_Load(object sender, EventArgs e)
        {
            //За тестване
            //if (this.Server.MachineName.ToLower() == "emo".ToLower())
            //{
            //    CallContext resultContext = AdminClientRef.Login("emo", "emo");
            //    ETEMModel.Models.User currentUser = AdminClientRef.GetUserByUserID(resultContext.EntityID);
            //    this.userProps = MakeLoginByUserID(resultContext.EntityID);
            //    Response.Redirect(AdministrativeActivities.ExamProtocols.formResource.PagePath);
            //}

            if (!IsPostBack)
            {
                FormLoad();
            }
        }

        public override void FormLoad()
        {

            this.lbUserName.Text = this.UserProps.PersonNamePlusTitle;
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