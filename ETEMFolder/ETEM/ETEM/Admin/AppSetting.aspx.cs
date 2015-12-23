using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Models;

namespace ETEM.Admin
{
    public partial class AppSetting : BasicPage
    {
        public static FormResources formResource = new FormResources
        {
            Module = Constants.MODULE_SETTINGS,
            PageFullName = Constants.UMS_ADMIN_APPSETTING,
            PagePath = "../Admin/AppSetting.aspx"

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
                this.gvSetting.DataSource = GeneralPage.DictionarySetting.Values.ToList();
                if (NewPageIndex.HasValue)
                {
                    this.gvSetting.PageIndex = NewPageIndex.Value;
                }
                this.gvSetting.DataBind();

            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в FormLoad " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);

                BaseHelper.LogToMail("Грешка в FormLoad " + formResource.PagePath);
                BaseHelper.LogToMail(ex.Message);
                BaseHelper.LogToMail(ex.StackTrace);
            }


        }

        private void BindSettingsGrid(List<AbstractSearch> searchCriterias)
        {
            this.gvSetting.DataSource = AdminClientRef.GetAllSettings(this.GridViewSortExpression, this.GridViewSortDirection, searchCriterias);
            if (NewPageIndex.HasValue)
            {
                this.gvSetting.PageIndex = NewPageIndex.Value;
            }
            this.gvSetting.DataBind();
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

        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);

            if (this.FormContext.QueryString.Contains("action") && this.FormContext.QueryString["action"].ToString() == "print")
            {
                MasterPageContainer.InnerHtml = string.Empty;
                this.btnNew.Visible = false;
                this.btnPrint.Visible = false;

                PrintSendCurrentWindow();
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

                this.SettingMainData.CurrentEntityMasterID = idRowMasterKey;
                this.SettingMainData.UserControlLoad();
                this.SettingMainData.Visible = true;
                this.SettingMainData.Focus();
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
                this.SettingMainData.CurrentEntityMasterID = Constants.INVALID_ID_STRING; ;
                this.SettingMainData.UserControlLoad();
                this.SettingMainData.Visible = true;
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в btnNew_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                this.OpenPageInNewWindow(ETEM.Admin.AppSetting.formResource, "action=print");
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Грешка в btnPrint_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

        }

        protected void gvSettings_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                this.GridViewSortExpression = e.SortExpression;
                base.SetSortDirection();
                btnFilterSearch_OnClick(null, null);
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в gvSettings_Sorting " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);

                BaseHelper.LogToMail("Грешка в gvSettings_Sorting " + formResource.PagePath);
                BaseHelper.LogToMail(ex.Message);
                BaseHelper.LogToMail(ex.StackTrace);
            }

        }

        protected void gvSettings_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.NewPageIndex = e.NewPageIndex;
                btnFilterSearch_OnClick(null, null);
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Грешка в gvSettings_OnPageIndexChanging " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);

                BaseHelper.LogToMail("Грешка в gvSettings_OnPageIndexChanging " + formResource.PagePath);
                BaseHelper.LogToMail(ex.Message);
                BaseHelper.LogToMail(ex.StackTrace);
            }

        }


        #region filter methods


        private List<AbstractSearch> AddCustomSearchCriterias()
        {
            List<AbstractSearch> listSearchCriterias = new List<AbstractSearch>();
            #region text Search
            if (!string.IsNullOrEmpty(this.tbxFilterValue.Text))
            {
                listSearchCriterias.Add(
                    new TextSearch
                    {
                        Comparator = TextComparators.Contains,
                        Property = "SettingValue",
                        SearchTerm = this.tbxFilterValue.Text
                    });
            }

            if (!string.IsNullOrEmpty(this.tbxFilterDefaultValue.Text))
            {
                listSearchCriterias.Add(
                    new TextSearch
                    {
                        Comparator = TextComparators.Contains,
                        Property = "SettingDefaultValue",
                        SearchTerm = this.tbxFilterDefaultValue.Text
                    });
            }

            if (!string.IsNullOrEmpty(this.tbxFilterCode.Text))
            {
                listSearchCriterias.Add(
                    new TextSearch
                    {
                        Comparator = TextComparators.Contains,
                        Property = "SettingIntCode",
                        SearchTerm = this.tbxFilterCode.Text
                    });
            }

            if (!string.IsNullOrEmpty(this.tbxFilterName.Text))
            {
                listSearchCriterias.Add(
                    new TextSearch
                    {
                        Comparator = TextComparators.Contains,
                        Property = "SettingName",
                        SearchTerm = this.tbxFilterName.Text
                    });
            }

            return listSearchCriterias;
            #endregion
        }

        protected void btnFilterSearch_OnClick(object sender, EventArgs e)
        {
            List<AbstractSearch> searchCriterias = AddCustomSearchCriterias();
            BindSettingsGrid(searchCriterias);
            this.pnlFilter.Visible = false;
        }



        protected void btnClearFilter_OnClick(object sender, EventArgs e)
        {
            this.tbxFilterCode.Text = "";
            this.tbxFilterDefaultValue.Text = "";
            this.tbxFilterName.Text = "";
            this.tbxFilterValue.Text = "";
        }

        protected void btnShowFilterPnl_OnClick(object sender, EventArgs e)
        {
            this.pnlFilter.Visible = true;
        }

        #endregion
    }
}