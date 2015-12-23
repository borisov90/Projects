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
    public partial class AllowIPList : BasicPage
    {
        public static FormResources formResource = new FormResources
        {
            Module = Constants.MODULE_SETTINGS,
            PageFullName = Constants.UMS_ADMIN_ALLOWIPLIST,
            PagePath = "../Admin/AllowIPList.aspx"

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

                CheckUserActionPermission(ETEMEnums.SecuritySettings.ShowAllowIPList, true);


                ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();


                this.gvAllowIP.DataSource = AdminClientRef.GetAllAllowIP(searchCriteria, GridViewSortExpression, GridViewSortDirection);
                if (NewPageIndex.HasValue)
                {
                    this.gvAllowIP.PageIndex = NewPageIndex.Value;
                }
                this.gvAllowIP.DataBind();
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

                this.AllowIPMainData.CurrentEntityMasterID = idRowMasterKey;
                this.AllowIPMainData.UserControlLoad();
                this.AllowIPMainData.Visible = true;
                this.AllowIPMainData.Focus();
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в lnkBtnServerEdit_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

        }


        protected void gvAllowIP_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.NewPageIndex = e.NewPageIndex;
            FormLoad();
        }

        protected void gvAllowIP_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.GridViewSortExpression = e.SortExpression;
            base.SetSortDirection();
            FormLoad();
        }

        protected void bntNew_Click(object sender, EventArgs e)
        {
            try
            {
                this.AllowIPMainData.CurrentEntityMasterID = Constants.INVALID_ID_STRING; ;
                this.AllowIPMainData.UserControlLoad();
                this.AllowIPMainData.Visible = true;
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в bntNew_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

        }
    }
}