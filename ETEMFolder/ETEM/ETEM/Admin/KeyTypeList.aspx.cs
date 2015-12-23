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
using ETEMModel.Models.DataView;

namespace ETEM.Admin
{
    public partial class KeyTypeList : BasicPage
    {
        
        public static FormResources formResource = new FormResources
        {
            Module = Constants.MODULE_NOMENCLATURES,
            PageFullName = Constants.UMS_ADMIN_USERLIST,
            PagePath = "../Admin/KeyTypeList.aspx"
        };

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);


        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadFilterControlsData();
                FormLoad();
            }
        }

        public override void FormLoad()
        {
            LoadFiltredData();
        }

        private void LoadFilterControlsData()
        {

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

                this.KeyType.CurrentEntityMasterID = idRowMasterKey;
                this.KeyType.UserControlLoad();
                this.KeyType.Visible = true;
                this.KeyType.Focus();
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в lnkBtnServerEdit_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

        }

        protected void gvKeyTypes_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.GridViewSortExpression = e.SortExpression;
            base.SetSortDirection();
            FormLoad();
        }



        protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            this.NewPageIndex = e.NewPageIndex;
            FormLoad();

        }

        protected void bntNewKeyType_Click(object sender, EventArgs e)
        {
            try
            {
                this.KeyType.CurrentEntityMasterID = Constants.INVALID_ID_STRING;
                this.KeyType.UserControlLoad();
                this.KeyType.Visible = true;
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в bntNewKeyType_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            this.pnlFilterData.Visible = true;
            this.KeyType.Visible = false;
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
                List<KeyTypeDataView> listKeyTypes = this.AdminClientRef.GetKeyTypeDataView(searchCriteria,
                                                                                    this.GridViewSortExpression, this.GridViewSortDirection);

                if (!string.IsNullOrEmpty(this.tbxFullTextSearch.Text))
                {
                    listKeyTypes = CreateFullTextSearch(listKeyTypes);
                }

                LoadSearchResult(listKeyTypes);
                this.pnlFilterData.Visible = false;

                if (NewPageIndex.HasValue)
                {
                    this.gvKeyTypes.PageIndex = NewPageIndex.Value;
                }

                var list = listKeyTypes.Select(e => new { e.EntityID, e.idKeyType, e.Name, e.Description, e.KeyTypeIntCode, e.IsSystemAsString }).ToList().Distinct();

                this.gvKeyTypes.DataSource = list.ToList();
                this.gvKeyTypes.DataBind();
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в bntNewKeyType_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        private List<KeyTypeDataView> CreateFullTextSearch(List<KeyTypeDataView> listKeyTypes)
        {
            //try
            //{


            HashSet<KeyTypeDataView> remainingKeyTypes = new HashSet<KeyTypeDataView>();
            string[] searchTerms = this.tbxFullTextSearch.Text.Split(Constants.CHAR_SEPARATORS, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < searchTerms.Length; i++)
            {
                var term = searchTerms[i].ToLower();
                remainingKeyTypes.UnionWith(listKeyTypes.Where(s => ((s.KeyValueNameStr != null ? s.KeyValueNameStr.ToLower().Contains(term) : false) ||
                                                                    (s.KeyValueIntCodeStr != null ? s.KeyValueIntCodeStr.ToLower().Contains(term) : false) ||
                                                                    (s.KeyValueDescriptionStr != null ? s.KeyValueDescriptionStr.ToLower().Contains(term) : false)
                                                                  )));
            }

            return remainingKeyTypes.ToList();
            //}
            //catch (Exception ex)
            //{

            //    BaseHelper.Log("Грешка в CreateFullTextSearch " + formResource.PagePath);
            //    BaseHelper.Log(ex.Message);
            //    BaseHelper.Log(ex.StackTrace);
            //}
        }

        //описание на полетата в които се Search
        private void AddCustomSearchCriterias(ICollection<AbstractSearch> searchCriteria)
        {
            //KeyType Name 
            if (!string.IsNullOrEmpty(this.tbxFilterName.Text.Trim()))
            {
                searchCriteria.Add(
                    new TextSearch
                    {
                        Comparator = TextComparators.Contains,
                        Property = "Name",
                        SearchTerm = this.tbxFilterName.Text.Trim()
                    });
            }

            //KeyTypeIntCode Name 
            if (!string.IsNullOrEmpty(this.tbxFilterKeyType.Text.Trim()))
            {
                searchCriteria.Add(
                    new TextSearch
                    {
                        Comparator = TextComparators.Contains,
                        Property = "KeyTypeIntCode",
                        SearchTerm = this.tbxFilterKeyType.Text.Trim()
                    });
            }
        }

        public void LoadSearchResult(List<KeyTypeDataView> listKeyType)
        {
            this.gvKeyTypes.DataSource = listKeyType;
            BindDataWithPaging();
        }

        private void BindDataWithPaging()
        {
            if (NewPageIndex.HasValue)
            {
                this.gvKeyTypes.PageIndex = NewPageIndex.Value;
            }

            this.gvKeyTypes.DataBind();
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
            this.tbxFilterName.Text = string.Empty;
            this.tbxFilterKeyType.Text = string.Empty;
            this.tbxFullTextSearch.Text = string.Empty;
        }

    }
}