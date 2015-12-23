using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using MoreLinq;
namespace ETEM.Controls.Common
{
    public partial class SMCErrorPnl : BaseUserControl
    {
        public List<string> ListErrors { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void UserControlLoad()
        {
            List<ListItem> resultErrors = new List<ListItem>();
            ListErrors = ListErrors.DistinctBy(s => s).ToList();

            for (int i = 0; i < ListErrors.Count; i++)
            {
                ListItem item = new ListItem(ListErrors[i]);
                resultErrors.Add(item);

            }
            this.blPersonalEroorsSave.DataSource = resultErrors;
            this.blPersonalEroorsSave.DataBind();
            this.pnlPersonalErrors.Visible = true;
        }
    }
}