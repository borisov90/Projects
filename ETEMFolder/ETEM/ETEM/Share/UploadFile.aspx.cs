using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers;

namespace ETEM.Share
{
    public partial class UploadFile : BasicPage
    {
        public static FormResources formResource = new FormResources
        {
            Module = Constants.MODULE_SHARE,
            PageFullName = Constants.UMS_SHARE_UPLOADFILE,
            PagePath = "../Share/UploadFile.aspx"

        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FormLoad();
            }
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