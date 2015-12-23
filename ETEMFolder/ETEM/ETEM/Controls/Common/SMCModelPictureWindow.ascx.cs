using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;

namespace ETEM.Controls.Common
{
    public partial class SMCModelPictureWindow : BaseUserControl
    {

        public string Headline
        {
            get { return this.ModalLabel.InnerText; }
            set { this.ModalLabel.InnerText = value; }
        }

        public string ImagePath
        {
            get { return this.img.Src; }
            set { this.img.Src = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void UserControlLoad()
        {
            //this.ModalLabel.InnerText = Headline;
            //this.img.Src = ImagePath;
        }
    }
}