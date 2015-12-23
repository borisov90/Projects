using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers;

namespace ETEM.Admin
{
    public partial class MenuNodePage : BasicPage
    {
        public static FormResources formResource = new FormResources
        {
            Module = Constants.MODULE_SETTINGS,
            PageFullName = Constants.UMS_ADMIN_MENUNODEPAGE,
            PagePath = "../Admin/MenuNodePage.aspx"

        };

        protected void Page_PreRender(object sender, EventArgs e)//changed on Page_Load
        {

            this.MenuNode.UserControlLoad();


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