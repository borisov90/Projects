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
    public partial class ModuleList : BasicPage
    {
        public static FormResources formResource = new FormResources
        {
            Module = Constants.MODULE_SETTINGS,
            PageFullName = Constants.UMS_ADMIN_MODULELIST,
            PagePath = "../Admin/ModuleList.aspx"

        };

        #region Overridden Form Main Data Methods
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
        #endregion

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

                CheckUserActionPermission(ETEMEnums.SecuritySettings.ShowModuleList, true);

                base.FormLoad();

                ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();


                this.gvModule.DataSource = AdminClientRef.GetAllModule(searchCriteria, GridViewSortExpression, GridViewSortDirection);
                if (NewPageIndex.HasValue)
                {
                    this.gvModule.PageIndex = NewPageIndex.Value;
                }
                this.gvModule.DataBind();
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Грешка в FormLoad " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
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

                this.ModuleMainData.CurrentEntityMasterID = idRowMasterKey;
                this.ModuleMainData.UserControlLoad();
                this.ModuleMainData.Visible = true;
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в lnkBtnServerEdit_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }


        protected void gvModule_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.NewPageIndex = e.NewPageIndex;
            FormLoad();
        }

        protected void gvModule_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.GridViewSortExpression = e.SortExpression;
            base.SetSortDirection();
            FormLoad();
        }

        protected void bntNew_Click(object sender, EventArgs e)
        {
            try
            {


                this.ModuleMainData.CurrentEntityMasterID = Constants.INVALID_ID_STRING; ;
                this.ModuleMainData.UserControlLoad();
                this.ModuleMainData.Visible = true;
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в lnkBtnServerEdit_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }
    }
}