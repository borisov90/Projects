using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Models.DataView.CostCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETEM.Controls.CostCalculation
{
    public partial class ProfilesListCtrl : BaseUserControl
    {

        public static FormResources formResource = new FormResources
        {
            Module          = Constants.MODULE_COST_CALCULATION,
            PageFullName    = Constants.ETEM_COSTCALCULATION_DIEFORMULALIST,
            PagePath        = "../CostCalculation/ProfilesListCtrl.aspx"
        };

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string OfferID
        {
            get { return this.hdnOfferID.Value; }
            set { this.hdnOfferID.Value = value; }
        }
       
        public override void UserControlLoad()
        {
            if (this.ownerPage == null)
            {
                throw new UMSException("Current Page is null or is not inheritor of BasicPage.");
            }

            try
            {

                loadInitControls();
                LoadProfilesList();
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Erron in Method FormLoad " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        private void loadInitControls()
        {
            try
            {
                this.ddlProfileCategoryFilter.UserControlLoad();
                this.ddlProfileComplexityFilter.UserControlLoad();
                this.ddlProfileTypeFilter.UserControlLoad();

                string pageSize = this.ownerPage.AdminClientRef.GetSettingByCode(ETEMModel.Helpers.ETEMEnums.AppSettings.PageSize.ToString()).SettingValue;

                this.ddlPagingRowsCount.UserControlLoad();
                this.ddlPagingRowsCount.SetDefaultValue(pageSize);

                this.gvProfilesList.PageSize = Int32.Parse(pageSize);
            }
            catch{}
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            this.pnlFilterData.Visible = true;
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            this.ucProfile.CurrentEntityMasterID = Constants.INVALID_ID_STRING; ;
            this.ucProfile.UserControlLoad();
            this.ucProfile.Visible = true;
        }

        protected void lnkBtnServerEdit_Click(object sender, EventArgs e)
        {


            LinkButton lnkBtnServerEdit = sender as LinkButton;

            if (lnkBtnServerEdit == null)
            {
                this.ownerPage.ShowMSG("lnkBtnServerEdit is null");
                return;
            }

            string idRowMasterKey = BaseHelper.ParseStringByAmpersand(lnkBtnServerEdit.CommandArgument)["idRowMasterKey"].ToString();            

            this.ucProfile.CurrentEntityMasterID = idRowMasterKey;
            this.ucProfile.SetHdnField(idRowMasterKey);
            this.ucProfile.UserControlLoad();
            this.ucProfile.Visible = true;
        }

        public void LoadProfilesList()
        {
            this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.ProfilesShowList, true);

            LoadFilteredList();
        }

        public void LoadFilteredList()
        {
            try
            {
                ICollection<AbstractSearch> searchCriteria          = new List<AbstractSearch>();
                AddCustomSearchCriterias(searchCriteria);

                List<ProfileSettingDataView> listProfileSetting     = new List<ProfileSettingDataView>();
                listProfileSetting                                  = base.CostCalculationRef.GetProfilesList(searchCriteria, base.GridViewSortExpression, base.GridViewSortDirection);

                LoadSearchResult(listProfileSetting);
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Erron in Method LoadFilteredList " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        public void LoadSearchResult(List<ProfileSettingDataView> listProfileSetting)
        {
            try
            {

                this.gvProfilesList.DataSource = listProfileSetting;

                BindDataWithPaging();  
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Error in LoadSearchResult " + formResource.PagePath);
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
                    this.gvProfilesList.PageIndex = NewPageIndex.Value;
                }

                this.gvProfilesList.DataBind();

                

            }
            catch (Exception ex)
            {
                BaseHelper.Log("Error in BindDataWithPaging " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        private void AddCustomSearchCriterias(ICollection<AbstractSearch> searchCriteria)
        {
            //ProfileCategory
            if (this.ddlProfileCategoryFilter.SelectedValue != Constants.INVALID_ID_STRING)
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator  = NumericComparators.Equal,
                        Property    = "idProfileCategory",
                        SearchTerm  = this.ddlProfileCategoryFilter.SelectedValueINT
                    });
            }
            //Type
            if (this.ddlProfileTypeFilter.SelectedValue != Constants.INVALID_ID_STRING)
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator  = NumericComparators.Equal,
                        Property    = "idProfileType",
                        SearchTerm  = this.ddlProfileTypeFilter.SelectedValueINT
                    });
            }
            //Complexity
            if (this.ddlProfileComplexityFilter.SelectedValue != Constants.INVALID_ID_STRING)
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator  = NumericComparators.Equal,
                        Property    = "idProfileComplexity",
                        SearchTerm  = this.ddlProfileComplexityFilter.SelectedValueINT
                    });
            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFilterForm();
        }

        protected void btnCancelFilter_OnClick(object sender, EventArgs e)
        {
            this.pnlFilterData.Visible = false;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadFilteredList();

            this.pnlFilterData.Visible = false;
        }

        protected void ddlPagingRowsCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.ddlPagingRowsCount.SelectedValue == ETEMEnums.PagingRowsCountEnum.AllRows.ToString())
                {
                    this.gvProfilesList.AllowPaging = false;
                }
                else
                {
                    this.gvProfilesList.AllowPaging = true;
                    this.gvProfilesList.PageIndex   = 0;
                    this.gvProfilesList.PageSize    = Convert.ToInt32(this.ddlPagingRowsCount.SelectedValue);
                }

                LoadFilteredList();
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Erron in Method ddlPagingRowsCount_SelectedIndexChanged " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        protected void gvProfilesList_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.GridViewSortExpression = e.SortExpression;
            base.SetSortDirection();
            LoadProfilesList();
        }

        protected void gvProfilesList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.NewPageIndex = e.NewPageIndex;
            LoadProfilesList();
        }

        public void AlphabetClick(string alpha)
        {
            try
            {
                if (alpha == Constants.ALL_CHARACTERS)
                {
                    List<AbstractSearch> searchCriteria     = new List<AbstractSearch>();
                    List<ProfileSettingDataView> list       = CostCalculationRef.GetProfilesList(searchCriteria, GridViewSortExpression, GridViewSortDirection);

                    ClearFilterForm();
                    LoadSearchResult(list);

                }
                else
                {
                    ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();
                    AddCustomSearchCriterias(searchCriteria);

                    searchCriteria.Add(new TextSearch
                    {
                        Comparator = TextComparators.StartsWith,
                        Property = "Customer",
                        SearchTerm = alpha
                    });

                    List<ProfileSettingDataView> list = CostCalculationRef.GetProfilesList(searchCriteria, GridViewSortExpression, GridViewSortDirection);

                    LoadSearchResult(list);
                }
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Error in AlphabetClick " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        private void ClearFilterForm()
        {
            
            this.ddlProfileCategoryFilter.SelectedValue     = Constants.INVALID_ID_STRING;
            this.ddlProfileComplexityFilter.SelectedValue   = Constants.INVALID_ID_STRING;
            this.ddlProfileTypeFilter.SelectedValue         = Constants.INVALID_ID_STRING;
        }


    }
}