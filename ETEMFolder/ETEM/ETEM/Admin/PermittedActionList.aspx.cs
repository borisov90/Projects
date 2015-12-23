using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Helpers.AbstractSearchBLHolder;

namespace ETEM.Admin
{
    public partial class PermittedActionList : BasicPage
    {
        public static FormResources formResource = new FormResources
        {
            Module = Constants.MODULE_PERMISSION,
            PageFullName = Constants.UMS_ADMIN_PERMITTEDACTIONLIST,
            PagePath = "../Admin/PermittedActionList.aspx"

        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ddlModule.UserControlLoad();
                FormLoad();
            }
        }

        public override void FormLoad()
        {
            try
            {
                ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();
                AddCustomSearchCriterias(searchCriteria);

                this.gvPermittedAction.DataSource = AdminClientRef.GetAllPermittedActions(searchCriteria, GridViewSortExpression, GridViewSortDirection);
                if (NewPageIndex.HasValue)
                {
                    this.gvPermittedAction.PageIndex = NewPageIndex.Value;
                }

                this.gvPermittedAction.DataBind();
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в FormLoad " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        private void AddCustomSearchCriterias(ICollection<AbstractSearch> searchCriteria)
        {
            if (this.ddlModule.SelectedValueINT != Constants.INVALID_ID)
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.Equal,
                        Property = "idModule",
                        SearchTerm = this.ddlModule.SelectedValueINT
                    });
            }




            if (!string.IsNullOrEmpty(this.tbxFilterSecuritySetting.Text))
            {
                searchCriteria.Add(
                    new TextSearch
                    {
                        Comparator = TextComparators.Contains,
                        Property = "SecuritySetting",
                        SearchTerm = this.tbxFilterSecuritySetting.Text
                    });
            }

            if (!string.IsNullOrEmpty(this.tbxFrendlyName.Text))
            {
                searchCriteria.Add(
                    new TextSearch
                    {
                        Comparator = TextComparators.Contains,
                        Property = "FrendlyName",
                        SearchTerm = this.tbxFrendlyName.Text
                    });
            }

            if (!string.IsNullOrEmpty(this.tbxDescription.Text))
            {
                searchCriteria.Add(
                    new TextSearch
                    {
                        Comparator = TextComparators.Contains,
                        Property = "Description",
                        SearchTerm = this.tbxDescription.Text
                    });
            }
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


        protected void lnkBtnServerEdit_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkBtnServerEdit = sender as LinkButton;

                if (lnkBtnServerEdit == null)
                {
                    ShowMSG("lnkBtnServerEdit is null");
                    return;
                }
                string idRowMasterKey = BaseHelper.ParseStringByAmpersand(lnkBtnServerEdit.CommandArgument)["idRowMasterKey"].ToString();

                this.PermittedActionMainData.CurrentEntityMasterID = idRowMasterKey;
                this.PermittedActionMainData.UserControlLoad();
                this.PermittedActionMainData.Visible = true;
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в FormLoad " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                this.PermittedActionMainData.CurrentEntityMasterID = Constants.INVALID_ID_STRING;
                this.PermittedActionMainData.UserControlLoad();
                this.PermittedActionMainData.Visible = true;
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в btnNew_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

        }

        protected void btnMarge_Click(object sender, EventArgs e)
        {
            try
            {
                CallContext cc = new CallContext();

                this.AdminClientRef.PermittedActionMerge(cc);

                FormLoad();
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в btnMarge_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

        }

        protected void gvPermittedAction_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.NewPageIndex = e.NewPageIndex;
            FormLoad();
        }
        protected void gvPermittedAction_OnSorting(object sender, GridViewSortEventArgs e)
        {
            this.GridViewSortExpression = e.SortExpression;
            base.SetSortDirection();
            FormLoad();
        }

        protected void btnFilterData_OnClick(object sender, EventArgs e)
        {
            FormLoad();
            this.pnlFilter.Visible = false;
        }

        protected void btnFilter_OnClick(object sender, EventArgs e)
        {
            this.pnlFilter.Visible = true;
        }

        protected void btnClear_OnClick(object sender, EventArgs e)
        {
            this.ddlModule.SelectedValue = Constants.INVALID_ID_STRING;
            this.tbxFrendlyName.Text = string.Empty;
            this.tbxDescription.Text = string.Empty;
            this.tbxFilterSecuritySetting.Text = string.Empty;
        }
    }
}