using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Models;
using ETEMModel.Helpers;
using ETEMModel.Models.DataView;
using ETEMModel.Helpers.AbstractSearchBLHolder;

namespace ETEM.Controls.Admin
{
    public partial class RoleMainData : BaseUserControl
    {
        private Role currentEntity;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string CurrentId
        {
            get { return this.hdnRowMasterKey.Value; }
            set { this.hdnRowMasterKey.Value = value; }
        }

        public override void UserControlLoad()
        {
            if (this.ownerPage == null)
            {
                throw new UMSException("Current Page is null or is not inheritor of BasicPage.");
            }

            loadInitControls();

            this.pnlFormData.Visible = true;
            if (!string.IsNullOrEmpty(this.CurrentEntityMasterID) && this.CurrentEntityMasterID != Constants.INVALID_ID_STRING)
            {
                this.currentEntity = this.ownerPage.AdminClientRef.GetRoleByID(this.CurrentEntityMasterID);

            }
            else
            {
                this.currentEntity = this.ownerPage.AdminClientRef.GetRoleByID(this.hdnRowMasterKey.Value);
            }

            if (currentEntity != null)
            {
                this.tbxName.Text = currentEntity.Name;
                this.tbxDescription.Text = currentEntity.Description;
                this.tbxIntCode.Text = currentEntity.RoleIntCode;

                this.CurrentId = currentEntity.EntityID.ToString();

                List<PermittedActionDataView> listAction = this.AdminClientRef.
                    GetAllPermittedActionsByRole(this.CurrentId, this.GridViewSortExpression, this.GridViewSortDirection).ToList();

                this.gvPermittedAction.DataSource = listAction;
                if (NewPageIndex.HasValue)
                {
                    this.gvPermittedAction.PageIndex = NewPageIndex.Value;
                }
                this.gvPermittedAction.DataBind();
            }
            else
            {
                this.lbResultContext.Text = string.Empty;
                this.tbxName.Text = string.Empty;
                this.tbxDescription.Text = string.Empty;
                this.tbxIntCode.Text = string.Empty;

                List<PermittedAction> listAction = new List<PermittedAction>();

                this.gvPermittedAction.DataSource = listAction;
                if (NewPageIndex.HasValue)
                {
                    this.gvPermittedAction.PageIndex = NewPageIndex.Value;
                }
                this.gvPermittedAction.DataBind();
                this.CurrentId = string.Empty;
            }
        }

        private void loadInitControls()
        {
            this.ddlModule.UserControlLoad();
            ClearResultContext(lbResultContext);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.RoleSave, false))
            {
                return;
            }

            if (string.IsNullOrEmpty(this.CurrentId))
            {
                this.currentEntity = new Role();
            }
            else
            {
                this.currentEntity = this.ownerPage.AdminClientRef.GetRoleByID(this.CurrentId);

                if (this.currentEntity == null)
                {
                    this.lbResultContext.Text = String.Format(BaseHelper.GetCaptionString("Entity_Role_Not_Found_By_ID"), this.CurrentId);
                    this.ownerPage.FormLoad();
                    return;
                }
            }

            currentEntity.Name = this.tbxName.Text;
            currentEntity.Description = this.tbxDescription.Text;
            currentEntity.RoleIntCode = this.tbxIntCode.Text;

            CallContext resultContext = new CallContext();

            resultContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;

            resultContext = this.ownerPage.AdminClientRef.RoleSave(currentEntity, resultContext);

            CheckIfResultIsSuccess(lbResultContext);
            this.lbResultContext.Text = resultContext.Message;
            this.hdnRowMasterKey.Value = resultContext.EntityID;

            this.ownerPage.ReloadSettingApplication();

            this.ownerPage.FormLoad();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.pnlFormData.Visible = false;
            this.CurrentId = string.Empty;

            this.pnlPermittedActionToBeAdded.Visible = false;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.PermittedActionSave, false))
            {
                return;
            }

            this.GridViewSortExpression                 = "FrendlyName";
            this.GridViewSortDirection                  = "ASC";
            List<PermittedActionDataView> listAction    = this.AdminClientRef.GetAllPermittedActionsByRoleNotAdded(this.hdnRowMasterKey.Value,
                                                                                this.GridViewSortExpression, this.GridViewSortDirection).ToList();

            this.gvPermittedActionToBeAdded.DataSource = listAction;

            if (NewPageIndex.HasValue)
            {
                this.gvPermittedActionToBeAdded.PageIndex = NewPageIndex.Value;
            }

            this.gvPermittedActionToBeAdded.DataBind();

            this.tbxSearchPermittedActions.Text         = string.Empty;
            this.pnlPermittedActionToBeAdded.Visible    = true;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            List<PermittedActionDataView> listAction    = this.AdminClientRef.GetAllPermittedActionsByRoleNotAdded(this.hdnRowMasterKey.Value,
                                                           this.GridViewSortExpression, this.GridViewSortDirection).ToList();

            ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();
            AddCustomSearchCriterias(searchCriteria);

            listAction = listAction.Select(x => { x.FrendlyName = x.FrendlyName.ToLower(); return x; }).ToList();
            listAction = (listAction.AsQueryable<PermittedActionDataView>()).ApplySearchCriterias(searchCriteria).ToList();
            listAction = listAction.Select(x => { x.FrendlyName = (x.FrendlyName[0].ToString().ToUpper() + x.FrendlyName.Substring(1, x.FrendlyName.Length - 1)); return x; }).ToList();

            this.gvPermittedActionToBeAdded.DataSource = listAction;
            if (NewPageIndex.HasValue)
            {
                this.gvPermittedActionToBeAdded.PageIndex = NewPageIndex.Value;
            }
            this.gvPermittedActionToBeAdded.DataBind();
        }

        private void AddCustomSearchCriterias(ICollection<AbstractSearch> searchCriteria)
        {
            if (this.ddlModule.SelectedValue != Constants.INVALID_ID_STRING)
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator  = NumericComparators.Equal,
                        Property    = "idModule",
                        SearchTerm  = this.ddlModule.SelectedValueINT
                    });
            }
            if (!string.IsNullOrEmpty(this.tbxSearchPermittedActions.Text.Trim()))
            {
                searchCriteria.Add(
                    new TextSearch
                    {
                        Comparator  = TextComparators.Contains,
                        Property    = "FrendlyName",
                        SearchTerm  = this.tbxSearchPermittedActions.Text.Trim().ToLower()
                    });
            }
        }

        protected void btnClosePermittedActionPnl_Click(object sender, EventArgs e)
        {
            this.pnlPermittedActionToBeAdded.Visible = false;
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            if (!this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.RemoveUserRole, false))
            {
                return;
            }

            List<RolePermittedActionLink> list = new List<RolePermittedActionLink>();

            foreach (GridViewRow row in this.gvPermittedAction.Rows)
            {
                HiddenField idPermittedAction = row.FindControl("hdnPermittedActionID") as HiddenField;
                HiddenField idRolePermittedAction = row.FindControl("hdnRolePermittedActionID") as HiddenField;
                CheckBox chbxPermittedAction = row.FindControl("chbxPermittedAction") as CheckBox;//cbxGridCheckbox

                if (chbxPermittedAction.Checked)
                {
                    list.Add(new RolePermittedActionLink()
                        {
                            idRolePermittedAction = Int32.Parse(idRolePermittedAction.Value),
                            idRole = Int32.Parse(this.hdnRowMasterKey.Value),
                            idPermittedAction = Int32.Parse(idPermittedAction.Value)
                        }
                    );
                }
            }

            if (list.Count > 0)
            {
                CallContext resultContext = new CallContext();
                resultContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;

                resultContext = this.ownerPage.AdminClientRef.RemovePermittedActionToRole(list, resultContext);
                this.CurrentEntityMasterID = this.hdnRowMasterKey.Value;
                this.pnlPermittedActionToBeAdded.Visible = false;
                UserControlLoad();
            }
        }

        protected void btnAddPermittedAction_Click(object sender, EventArgs e)
        {
            List<RolePermittedActionLink> list = new List<RolePermittedActionLink>();

            foreach (GridViewRow row in this.gvPermittedActionToBeAdded.Rows)
            {
                HiddenField idPermittedAction = row.FindControl("hdnPermittedActionID") as HiddenField;
                HiddenField idRolePermittedAction = row.FindControl("hdnRolePermittedActionID") as HiddenField;
                CheckBox chbxPermittedAction = row.FindControl("cbxGridCheckbox") as CheckBox;

                if (chbxPermittedAction.Checked)
                {
                    list.Add(new RolePermittedActionLink()
                        {
                            idRole = Int32.Parse(this.hdnRowMasterKey.Value),
                            idPermittedAction = Int32.Parse(idPermittedAction.Value)
                        }
                    );
                }
            }

            if (list.Count > 0)
            {
                CallContext resultContext = new CallContext();
                resultContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;

                resultContext = this.ownerPage.AdminClientRef.AddPermittedActionToRole(list, resultContext);
                this.CurrentEntityMasterID = this.hdnRowMasterKey.Value;
                this.pnlPermittedActionToBeAdded.Visible = false;
                UserControlLoad();
            }
        }

        protected void gvPermittedAction_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.NewPageIndex = e.NewPageIndex;
            UserControlLoad();
        }

        protected void gvPermittedAction_OnSorting(object sender, GridViewSortEventArgs e)
        {
            this.GridViewSortExpression = e.SortExpression;
            base.SetSortDirection();
            UserControlLoad();
        }

        protected void gvPermittedActionToBeAdded_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.NewPageIndex = e.NewPageIndex;
            btnSearch_Click(null, null);
        }

        protected void gvPermittedActionToBeAdded_OnSorting(object sender, GridViewSortEventArgs e)
        {
            this.GridViewSortExpression = e.SortExpression;
            base.SetSortDirection();

            btnSearch_Click(null, null);
        }
    }
}