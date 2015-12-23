using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETEM.Controls.Common
{
    public partial class SMCTextArea : System.Web.UI.UserControl
    {
        private int alowedLength = 490;
        public int AllowedLength
        {
            get { return this.alowedLength; }
            set { this.alowedLength = value; }
        }
        public string CssClass
        {
            get { return this.tbxTextArea.CssClass; }
            set
            {

                if (!value.Contains("maxLengthable"))
                {
                    this.tbxTextArea.CssClass += " maxLengthable480 " + value;
                }
                else
                {
                    this.tbxTextArea.CssClass += " " + value;
                }
            }
        }
        public int MaxLength
        {
            get { return this.tbxTextArea.MaxLength; }
            set { this.tbxTextArea.MaxLength = value; }
        }
        public bool ReadOnly
        {
            get { return this.tbxTextArea.ReadOnly; }
            set { this.tbxTextArea.ReadOnly = value; }
        }
        public string Text
        {
            get { return this.tbxTextArea.Text; }
            set { this.tbxTextArea.Text = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //this.meeTbxTextArea.Mask = "?{" + this.MaxLength + "}";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void tbxTextArea_OnTextChanged(object sender, EventArgs e)
        {
            if (this.tbxTextArea.Text.Length >= this.MaxLength)
            {
                this.tbxTextArea.Text = tbxTextArea.Text.Substring(0, this.MaxLength);
            }
        }
    }
}