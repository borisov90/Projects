using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Models;
using ETEMModel.Models.DataView;
using ETEMModel.Helpers.AbstractSearchBLHolder;

namespace ETEM.Controls.Admin
{
    public partial class PersonList : BaseUserControl
    {
        private List<PersonDataView> listPerson;

        public string CurrentCallerUniqueID { get; set; }
        public string CurrentCallerValueUniqueID { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();

            this.listPerson = this.ownerPage.AdminClientRef.GetAllPersons(searchCriteria,
                this.GridViewSortExpression, this.GridViewSortDirection).ToList();
        }

        public override void UserControlLoad()
        {
            CallContext resultContext = new CallContext();

            if (CurrentCallerUniqueID != null)
            {
                this.hdnCurrentCallerUniqueID.Value = CurrentCallerUniqueID;
                this.hdnCurrentCallerValueUniqueID.Value = CurrentCallerValueUniqueID;
            }

            this.gvPerson.DataSource = listPerson;
            if (NewPageIndex.HasValue)
            {
                this.gvPerson.PageIndex = NewPageIndex.Value;
            }
            this.gvPerson.DataBind();
        }

        protected void tbxFullName_TextChanged(object sender, EventArgs e)
        {
            TextBox tbxFullName = sender as TextBox;

            if (tbxFullName != null && !string.IsNullOrEmpty(tbxFullName.Text))
            {

                this.gvPerson.DataSource = listPerson.Where(p => p.FullName.ToLower().Contains(tbxFullName.Text.ToLower()));

                this.gvPerson.DataBind();
            }
        }

        protected void lnkBtnSelect_Click(object sender, EventArgs e)
        {
            LinkButton lnkBtnSelect = sender as LinkButton;
            this.CurrentEntityMasterID = BaseHelper.ParseStringByAmpersand(lnkBtnSelect.CommandArgument)["EntityID"].ToString();
            UserControlShowHide(false);

            TextBox currentCallerTextBox = this.ownerPage.FindControl(this.hdnCurrentCallerUniqueID.Value) as TextBox;

            if (currentCallerTextBox != null)
            {
                currentCallerTextBox.Text = listPerson.Where(p => p.IdEntity == this.CurrentEntityMasterID).FirstOrDefault().FullName;
            }

            HiddenField hdnCurrentCallerValueUniqueID = this.ownerPage.FindControl(this.hdnCurrentCallerValueUniqueID.Value) as HiddenField;

            if (hdnCurrentCallerValueUniqueID != null)
            {
                hdnCurrentCallerValueUniqueID.Value = this.CurrentEntityMasterID;
            }


        }
        public void UserControlShowHide(bool visible)
        {
            this.pnlPerson.Visible = visible;
        }



        protected void gvPerson_OnSorting(object sender, GridViewSortEventArgs e)
        {
            this.GridViewSortExpression = e.SortExpression;
            base.SetSortDirection();
            UserControlLoad();
        }

        protected void gvPerson_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.NewPageIndex = e.NewPageIndex;
            UserControlLoad();
        }

    }
}