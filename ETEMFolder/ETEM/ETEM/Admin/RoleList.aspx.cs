using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers;

namespace ETEM.Admin
{
    public partial class RoleList : BasicPage
    {
        public static FormResources formResource = new FormResources
        {
            Module = Constants.MODULE_PERMISSION,
            PageFullName = Constants.UMS_ADMIN_ROLELIST,
            PagePath = "../Admin/RoleList.aspx"

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
            try
            {


                CheckUserActionPermission(ETEMEnums.SecuritySettings.RoleShowList, true);
                this.gvRole.DataSource = AdminClientRef.GetAllRoles(GridViewSortExpression, GridViewSortDirection);
                if (NewPageIndex.HasValue)
                {
                    this.gvRole.PageIndex = NewPageIndex.Value;
                }
                this.gvRole.DataBind();
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в FormLoad " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
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


                if (!CheckUserActionPermission(ETEMEnums.SecuritySettings.RoleEditView, false))
                {
                    return;
                }

                LinkButton lnkBtnServerEdit = sender as LinkButton;

                if (lnkBtnServerEdit == null)
                {
                    ShowMSG("lnkBtnServerEdit is null");
                    return;
                }
                string idRowMasterKey = BaseHelper.ParseStringByAmpersand(lnkBtnServerEdit.CommandArgument)["idRowMasterKey"].ToString();

                this.RoleMainData.CurrentEntityMasterID = idRowMasterKey;
                this.RoleMainData.UserControlLoad();
                this.RoleMainData.Visible = true;
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в lnkBtnServerEdit_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {


                if (!CheckUserActionPermission(ETEMEnums.SecuritySettings.AddUserRole, false))
                {
                    return;
                }

                this.RoleMainData.CurrentId = Constants.INVALID_ID_STRING;
                this.RoleMainData.UserControlLoad();
                this.RoleMainData.Visible = true;
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в btnNew_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }


        protected void gvRole_OnSorting(object sender, GridViewSortEventArgs e)
        {
            try
            {


                this.GridViewSortExpression = e.SortExpression;
                base.SetSortDirection();
                FormLoad();
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в gvRole_OnSorting " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        protected void gvRole_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.NewPageIndex = e.NewPageIndex;
                FormLoad();
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в gvRole_OnPageIndexChanging " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

        }
    }
}