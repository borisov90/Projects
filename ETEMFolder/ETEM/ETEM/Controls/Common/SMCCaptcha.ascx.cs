using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;

namespace ETEM.Controls.Common
{
    public partial class SMCCaptcha : BaseUserControl
    {
        public string TextBoxCode { get { return this.tbxCode.Text; } }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void UserControlLoad()
        {


        }
    }
}