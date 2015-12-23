using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Models;
using ETEMModel.Models.DataView.CostCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETEM.CostCalculation
{
    public partial class DieFormulaList : BasicPage
    {
        public static FormResources formResource = new FormResources
        {
            Module          = Constants.MODULE_COST_CALCULATION,
            PageFullName    = Constants.ETEM_COSTCALCULATION_DIEFORMULALIST,
            PagePath        = "../CostCalculation/DieFormulaList.aspx"
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
            if (this.gvDieFormulaList.Rows.Count > 0)
            {
                //((LinkButton)this.gvUsers.HeaderRow.Cells[3].Controls[0]).Text = GetCaption("GridView_Users_UserName");
                //((LinkButton)this.gvUsers.HeaderRow.Cells[4].Controls[0]).Text = GetCaption("GridView_Users_Person");
                //((LinkButton)this.gvUsers.HeaderRow.Cells[5].Controls[0]).Text = GetCaption("GridView_Users_Status");
                //((LinkButton)this.gvUsers.HeaderRow.Cells[6].Controls[0]).Text = GetCaption("GridView_Users_Description");
            }
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
                loadInitControls();
                LoadDieFormulaList();
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
                this.ddlNumberCavitiesFilter.UserControlLoad();
                this.ddlProfileCategoryFilter.UserControlLoad();
                this.ddlProfileTypeFilter.UserControlLoad();
                
                string pageSize = GetSettingByCode(ETEMModel.Helpers.ETEMEnums.AppSettings.PageSize).SettingValue;

                this.ddlPagingRowsCount.UserControlLoad();
                this.ddlPagingRowsCount.SetDefaultValue(pageSize);
            }
            catch { }
        }

        private void BindDataWithPaging()
        {
            try
            {
                if (NewPageIndex.HasValue)
                {
                    this.gvDieFormulaList.PageIndex = NewPageIndex.Value;
                }
                this.gvDieFormulaList.DataBind();
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Erron in Method BindDataWithPaging " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        public void LoadSearchResult(List<DieFormulaDataView> listDieFormula)
        {
            try
            {
                

                this.gvDieFormulaList.DataSource = listDieFormula;

                BindDataWithPaging();
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Erron in Method LoadSearchResult " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }      

        protected void ddlPagingRowsCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.ddlPagingRowsCount.SelectedValue == ETEMEnums.PagingRowsCountEnum.AllRows.ToString())
                {
                    this.gvDieFormulaList.AllowPaging = false;
                }
                else
                {
                    this.gvDieFormulaList.AllowPaging   = true;
                    this.gvDieFormulaList.PageIndex     = 0;
                    this.gvDieFormulaList.PageSize      = Convert.ToInt32(this.ddlPagingRowsCount.SelectedValue);
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

        public void LoadFilteredList()
        {
            try
            {
                ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();
                AddCustomSearchCriterias(searchCriteria);

                List<DieFormulaDataView> listDieFormulaList = new List<DieFormulaDataView>();
                listDieFormulaList                          = base.CostCalculationRef.GetAllDieFormula(searchCriteria, base.GridViewSortExpression, base.GridViewSortDirection);

                LoadSearchResult(listDieFormulaList);
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Erron in Method LoadFilteredList " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckUserActionPermission(ETEMEnums.SecuritySettings.DieFormulaSave, false))
                {
                    return;
                }

                this.ucDieFormula.CurrentEntityMasterID = Constants.INVALID_ID_STRING;
                this.ucDieFormula.UserControlLoad();
                this.ucDieFormula.Visible               = true;
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Erron in Method btnNew_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        protected void gvDieFormulaList_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.GridViewSortExpression = e.SortExpression;
            base.SetSortDirection();
            LoadFilteredList();
        }

        protected void gvDieFormulaList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.NewPageIndex = e.NewPageIndex;
            LoadFilteredList();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            this.pnlFilterData.Visible = true;
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

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFilterForm();

            LoadFilteredList();
        }

        private void ClearFilterForm()
        {
            this.ddlProfileCategoryFilter.SelectedValue     = Constants.INVALID_ID_STRING;
            this.ddlNumberCavitiesFilter.SelectedValue      = Constants.INVALID_ID_STRING;
            this.ddlProfileTypeFilter.SelectedValue         = Constants.INVALID_ID_STRING;
        }

        private void AddCustomSearchCriterias(ICollection<AbstractSearch> searchCriteria)
        {
            //Number of Cavities
            if (this.ddlNumberCavitiesFilter.SelectedValue != Constants.INVALID_ID_STRING)
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator  = NumericComparators.Equal,
                        Property    = "idNumberOfCavities",
                        SearchTerm  = this.ddlNumberCavitiesFilter.SelectedValueINT
                    });
            }
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
        }

        public void LoadDieFormulaList()
        {
            CheckUserActionPermission(ETEMEnums.SecuritySettings.DieFormulaListView, true);            

            this.gvDieFormulaList.PageSize = Int32.Parse(GetSettingByCode(ETEMModel.Helpers.ETEMEnums.AppSettings.PageSize).SettingValue);

            LoadFilteredList();
        }

        protected void lnkBtnServerEdit_Click(object sender, EventArgs e)
        {
            if (!CheckUserActionPermission(ETEMEnums.SecuritySettings.DieFormulaPreview, false))
            {
                return;
            }

            LinkButton lnkBtnServerEdit = sender as LinkButton;

            if (lnkBtnServerEdit == null)
            {
                ShowMSG("lnkBtnServerEdit is null");
                return;
            }

            string idRowMasterKey                       = BaseHelper.ParseStringByAmpersand(lnkBtnServerEdit.CommandArgument)["idRowMasterKey"].ToString();
            DieFormula dieFormula                       = this.CostCalculationRef.GetDieFormulaById(idRowMasterKey);

            this.ucDieFormula.CurrentEntityMasterID     = idRowMasterKey;

            this.ucDieFormula.UserControlLoad();
            this.ucDieFormula.Visible = true;

        }        

    }
}