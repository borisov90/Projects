using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Helpers;
using ETEMModel.Models.DataView.Admin;
using ETEMModel.Models;

namespace ETEM.Admin
{
    public partial class GroupList : BasicPage
    {
        ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();

        public static FormResources formResource = new FormResources
        {
            Module = Constants.MODULE_NOMENCLATURES,
            PageFullName = Constants.UMS_ADMIN_GROUPLIST,
            PagePath = "../Admin/GroupList.aspx"

        };

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

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUserActionPermission(ETEMEnums.SecuritySettings.GroupShowList, true);

            if (!IsPostBack)
            {
                FormLoad();
            }
        }

        public override void FormLoad()
        {

            LoadFiltredData();


        }

        protected void lnkBtnServerEdit_Click(object sender, EventArgs e)
        {
            if (!CheckUserActionPermission(ETEMEnums.SecuritySettings.GroupPreview, false))
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
            this.GroupData.CurrentEntityMasterID = idRowMasterKey;
            this.GroupData.UserControlLoad();
        }

        protected void gvGroups_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.NewPageIndex = e.NewPageIndex;
            FormLoad();
        }

        protected void gvGroups_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.GridViewSortExpression = e.SortExpression;
            base.SetSortDirection();
            FormLoad();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            if (!this.CheckUserActionPermission(ETEMEnums.SecuritySettings.GroupSave, false))
            {
                return;
            }

            this.GroupData.CurrentEntityMasterID = Constants.INVALID_ID_STRING; ;
            this.GroupData.UserControlLoad();
            this.GroupData.Visible = true;
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            this.pnlFilterData.Visible = true;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadFiltredData();
        }

        private void LoadFiltredData()
        {
            try
            {

                ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();
                AddCustomSearchCriterias(searchCriteria);
                List<GroupDataView> listGroupDataView = this.AdminClientRef.GetGroupDataView(searchCriteria, this.GridViewSortExpression, this.GridViewSortDirection);

                LoadSearchResult(listGroupDataView);

                this.pnlFilterData.Visible = false;
            }
            catch (Exception ex)
            {


                BaseHelper.Log("Грешка в LoadFiltredData " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        //описание на полетата в които се Search
        private void AddCustomSearchCriterias(ICollection<AbstractSearch> searchCriteria)
        {
            if (!string.IsNullOrEmpty(this.tbxFilterGroupName.Text))
            {
                searchCriteria.Add(
                    new TextSearch
                    {
                        Comparator = TextComparators.Contains,
                        Property = "GroupName",
                        SearchTerm = this.tbxFilterGroupName.Text
                    });
            }



            if (this.acFilterPerson.SelectedValue != Constants.INVALID_ID_STRING && !string.IsNullOrEmpty(this.acFilterPerson.SelectedValue)
                && !string.IsNullOrEmpty(this.acFilterPerson.Text.Trim()))
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.Equal,
                        Property = "idPerson",
                        SearchTerm = this.acFilterPerson.SelectedValueINT
                    });
            }



        }

        public void LoadSearchResult(List<GroupDataView> listGroups)
        {
            try
            {
                List<Group> listGr = new List<Group>();

                foreach (var item in listGroups)
                {
                    if (listGr.Where(z => z.idGroup == item.idGroup).Count() == 0)
                    {
                        listGr.Add(new Group
                                    {
                                        idGroup = item.idGroup,
                                        GroupName = item.GroupName,
                                        SharedAccess = item.SharedAccess
                                    });
                    }
                }

                this.gvGroups.DataSource = listGr;
                BindDataWithPaging();
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в LoadSearchResult " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

        }

        private void BindDataWithPaging()
        {
            try
            {
                if (NewPageIndex.HasValue)
                {
                    this.gvGroups.PageIndex = NewPageIndex.Value;
                }
                this.gvGroups.DataBind();

            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в BindDataWithPaging " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.pnlFilterData.Visible = false;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFilterForm();
        }

        private void ClearFilterForm()
        {
            this.acFilterPerson.SelectedValue = Constants.INVALID_ID_STRING;
            this.acFilterPerson.Text = string.Empty;
            this.tbxFilterGroupName.Text = string.Empty;

        }






    }
}