using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using System.Web.UI.HtmlControls;

namespace ETEM.Controls.Common
{
    public partial class AlphaBetCtrl : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            string[] character = Constants.alphabetLatin;

            for (int i = 0; i < character.Length; i++)
            {

                LinkButton lnkChar = new LinkButton();
                lnkChar.CssClass = "alphabet";
                lnkChar.Click += new EventHandler(lnkChar_Click);

                lnkChar.Text = character[i];
                lnkChar.CommandArgument = character[i];

                lnkChar.ID = "lnkChar" + i;
                this.alphabetCyrillic.Controls.Add(lnkChar);
            }
        }

        public string SelectedLetter
        {
            get { return this.hdnSelectedLetter.Value; }
            set { this.hdnSelectedLetter.Value = value; }
        }

        public override void UserControlLoad()
        {

            //
        }

        void lnkChar_Click(object sender, EventArgs e)
        {
            LinkButton lnkChar = sender as LinkButton;
            var allButtnsCount = this.alphabetCyrillic.Controls.Count;
            string idStart = "lnkChar";
            for (int i = 0; i < allButtnsCount; i++)
            {
                var control = this.alphabetCyrillic.FindControl(idStart + i) as LinkButton;
                if (control != null)
                {
                    control.Attributes.Remove("class");
                    control.CssClass = "alphabet";
                    if (control.Text == lnkChar.CommandArgument)
                    {
                        if (lnkChar.CommandArgument != Constants.ALL_CHARACTERS)
                        {
                            this.SelectedLetter = lnkChar.CommandArgument;
                        }
                        control.CssClass = "alphabet activeLatter";
                    }
                }
            }

            if (lnkChar != null)
            {
                this.ownerPage.AlphabetClick(lnkChar.CommandArgument);
            }
        }
    }
}