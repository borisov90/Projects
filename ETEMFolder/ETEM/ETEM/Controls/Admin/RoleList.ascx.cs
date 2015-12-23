using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Models;
using ETEMModel.Helpers;
using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEM.Admin;

namespace ETEM.Controls.Admin
{
    public partial class RoleList : BaseUserControl
    {
        private List<Role> entityList;

        public string CurrentUserID { get; set; }
        public string CurrentCallerUniqueID { get; set; }
        public string CurrentCallerValueUniqueID { get; set; }

        public void SetHdnField(string value)
        {
            this.hdnCurrentUserID.Value = value;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void UserControlLoad()
        {
            if (!string.IsNullOrEmpty(this.hdnCurrentUserID.Value))
            {
                this.entityList = this.ownerPage.AdminClientRef.GetAllRolesByUser(this.hdnCurrentUserID.Value,
                                                                                  this.GridViewSortExpression,
                                                                                  this.GridViewSortDirection).ToList();
            }
            else
            {
                this.hdnCurrentUserID.Value = this.CurrentUserID;
                this.entityList = this.ownerPage.AdminClientRef.GetAllRolesByUser(this.CurrentUserID,
                                                                                  this.GridViewSortExpression,
                                                                                  this.GridViewSortDirection).ToList();
            }

            if (this.CurrentCallerUniqueID != null)
            {
                this.hdnCurrentCallerUniqueID.Value = CurrentCallerUniqueID;
                this.hdnCurrentCallerValueUniqueID.Value = CurrentCallerValueUniqueID;
            }

            this.gvAddedRoles.DataSource = entityList;

            this.gvAddedRoles.DataBind();
        }

        protected void lnkBtnSelect_Click(object sender, EventArgs e)
        {
            LinkButton lnkBtnSelect = sender as LinkButton;
            this.CurrentEntityMasterID = BaseHelper.ParseStringByAmpersand(lnkBtnSelect.CommandArgument)["EntityID"].ToString();
        }

        public void UserControlShowHide(bool visible)
        {
            this.pnlList.Visible = visible;
        }

        protected void btnShowRoles_Click(object sender, EventArgs e)
        {

            if (!this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.UserSave, false))
            {
                return;
            }
            BindGvNotAddedRoles(new List<AbstractSearch>());
        }

        private void BindGvNotAddedRoles(List<AbstractSearch> searchCriterias)
        {
            this.pnlRoles.Visible = true;
            this.GridViewSortExpression = "Name";
            this.GridViewSortDirection = "ASC";

            this.gvRoles.DataSource = this.AdminClientRef.GetAllRoleByUserNotAdded(this.hdnCurrentUserID.Value,
                                                                                   this.GridViewSortExpression,
                                                                                   this.GridViewSortDirection,
                                                                                   searchCriterias);

            if (NewPageIndex.HasValue)
            {
                this.gvRoles.PageIndex = NewPageIndex.Value;
            }
            this.gvRoles.DataBind();
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {

            if (!this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.UserSave, false))
            {
                return;
            }

            foreach (GridViewRow row in this.gvAddedRoles.Rows)
            {
                HiddenField hdnRowMasterKey = row.FindControl("hdnRowMasterKey") as HiddenField;
                CheckBox chbxIdRole = row.FindControl("chbxIdRole") as CheckBox;

                if (chbxIdRole != null && chbxIdRole.Checked)
                {
                    this.AdminClientRef.RemoveUserRole(this.hdnCurrentUserID.Value, hdnRowMasterKey.Value, this.ownerPage.CallContext);
                }
            }
            this.CurrentUserID = this.hdnCurrentUserID.Value;

            if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {

                if (Page != null && (Page as UserList) != null)
                {
                    (Page as UserList).LoadFilteredList();
                }

            }

            UserControlLoad();
        }

        protected void gvRoles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.NewPageIndex = e.NewPageIndex;
            btnShowRoles_Click(null, null);
        }



        protected void gvAddedRoles_OnSorting(object sender, GridViewSortEventArgs e)
        {
            this.GridViewSortExpression = e.SortExpression;
            base.SetSortDirection();
            btnShowRoles_Click(null, null);
        }

        protected void gvRoles_OnSorting(object sender, GridViewSortEventArgs e)
        {
            this.GridViewSortExpression = e.SortExpression;
            base.SetSortDirection();
            btnShowRoles_Click(null, null);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.UserSave, false))
            {
                return;
            }

            this.pnlRoles.Visible = false;

            foreach (GridViewRow row in this.gvRoles.Rows)
            {
                HiddenField hdnRowMasterKey = row.FindControl("hdnRowMasterKey") as HiddenField;
                CheckBox chbxIdRole = row.FindControl("chbxIdRole") as CheckBox;

                if (chbxIdRole != null && chbxIdRole.Checked)
                {
                    this.AdminClientRef.AddUserRole(this.hdnCurrentUserID.Value, hdnRowMasterKey.Value, this.ownerPage.CallContext);
                }
            }
            this.CurrentUserID = this.hdnCurrentUserID.Value;


            if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {

                if (Page != null && (Page as UserList) != null)
                {
                    (Page as UserList).LoadFilteredList();
                }

            }


            UserControlLoad();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.pnlRoles.Visible = false;
        }

        #region Filter Methods

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.pnlRoles.Visible = false;
            List<AbstractSearch> searchCriteria = new List<AbstractSearch>();
            AddCustomSearchCriterias(searchCriteria);
            BindGvNotAddedRoles(searchCriteria);
        }



        private void AddCustomSearchCriterias(ICollection<AbstractSearch> searchCriteria)
        {
            if (!string.IsNullOrEmpty(this.tbxFilterName.Text))
            {
                searchCriteria.Add(new TextSearch
                {
                    Comparator = TextComparators.Contains,
                    Property = "Name",
                    SearchTerm = this.tbxFilterName.Text
                });
            }

            if (!string.IsNullOrEmpty(this.tbxFilterDescription.Text))
            {
                searchCriteria.Add(new TextSearch
                {
                    Comparator = TextComparators.Contains,
                    Property = "Description",
                    SearchTerm = this.tbxFilterDescription.Text
                });
            }


        }

        #endregion

    }
}