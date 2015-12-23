using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers;

namespace ETEM.UI
{
    public partial class InternalPageInfo : BasicPage
    {
        public static FormResources formResource = new FormResources
        {
            Module = Constants.MODULE_UI,
            PageFullName = Constants.UMS_UI_INTERNALPAGEINFO,
            PagePath = "../Share/InternalPageInfo.aspx"

        };

        protected void Page_Load(object sender, EventArgs e)
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