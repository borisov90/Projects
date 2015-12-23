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
    public partial class DiePriceListDetailsList : BasicPage
    {
        public static FormResources formResource = new FormResources
        {
            Module = Constants.MODULE_COST_CALCULATION,
            PageFullName = Constants.ETEM_COSTCALCULATION_DIEPRICELISTDETAILSLIST,
            PagePath = "../CostCalculation/DiePriceListDetailsList.aspx"
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
            if (this.gvDiePriceListDetails.Rows.Count > 0)
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
                CheckUserActionPermission(ETEMEnums.SecuritySettings.DiePriceListDetailsListView, true);

                string pageSize = GetSettingByCode(ETEMModel.Helpers.ETEMEnums.AppSettings.PageSize).SettingValue;

                this.ddlPagingRowsCount.UserControlLoad();
                this.ddlPagingRowsCount.SetDefaultValue(pageSize);

                LoadFilterControls();

                this.gvDiePriceListDetails.PageSize = Int32.Parse(pageSize);

                LoadFilteredList();
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Грешка в FormLoad " + formResource.PagePath);
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
                    this.gvDiePriceListDetails.PageIndex = NewPageIndex.Value;
                }
                this.gvDiePriceListDetails.DataBind();
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Грешка в BindDataWithPaging " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        public void LoadSearchResult(List<DiePriceListDetailDataView> listPriceListDetails)
        {
            try
            {
                //                if (this.ddlRole.SelectedValueINT != Constants.INVALID_ID)
                //                {
                ////                    listUsers = listUsers.Where(u => u.UserRoleLinkList.Where(r => r.idRole == this.ddlRole.SelectedValueINT).Count() > 0).ToList();
                //                }

                this.gvDiePriceListDetails.DataSource = listPriceListDetails;

                BindDataWithPaging();
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Грешка в LoadSearchResult " + formResource.PagePath);
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

                List<DiePriceListDetailDataView> listPriceListDetails = new List<DiePriceListDetailDataView>();

                DateTime? dateActiveTo = null;
                if (!string.IsNullOrWhiteSpace(this.tbxDateFromTo.Text))
                {
                    dateActiveTo = this.tbxDateFromTo.TextAsDateParseExact;
                }

                listPriceListDetails = base.CostCalculationRef.GetAllDiePriceListDetails(searchCriteria,
                                                                                         dateActiveTo,
                                                                                         base.GridViewSortExpression,
                                                                                         base.GridViewSortDirection);

                LoadSearchResult(listPriceListDetails);
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Грешка в LoadFilteredList " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        private void LoadFilterControls()
        {
            try
            {
                this.tbxDateFromTo.SetTxbDateTimeValue(DateTime.Today);
                this.ddlVendor.UserControlLoad();
                this.ddlNumberOfCavities.UserControlLoad();
                this.ddlProfileCategory.UserControlLoad();
                this.ddlProfileComplexity.UserControlLoad();
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Грешка в LoadFilterControls " + formResource.PagePath);
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
                    this.gvDiePriceListDetails.AllowPaging = false;
                }
                else
                {
                    this.gvDiePriceListDetails.AllowPaging = true;
                    this.gvDiePriceListDetails.PageIndex = 0;
                    this.gvDiePriceListDetails.PageSize = Convert.ToInt32(this.ddlPagingRowsCount.SelectedValue);
                }

                LoadFilteredList();
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Грешка в ddlPagingRowsCount_SelectedIndexChanged " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        protected void btnNewDiePriceList_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckUserActionPermission(ETEMEnums.SecuritySettings.DiePriceListSave, false))
                {
                    return;
                }

                this.ucDiePriceListData.CurrentEntityMasterID = Constants.INVALID_ID_STRING;
                this.ucDiePriceListData.SetHdnField(Constants.INVALID_ID_STRING);
                this.ucDiePriceListData.UserControlLoad();
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Грешка в btnNewDiePriceList_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        protected void btnNewDiePriceListDetails_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckUserActionPermission(ETEMEnums.SecuritySettings.DiePriceListDetailsSave, false))
                {
                    return;
                }

                this.ucDiePriceListDetailsData.CurrentEntityMasterID = Constants.INVALID_ID_STRING;
                this.ucDiePriceListDetailsData.SetHdnField(Constants.INVALID_ID_STRING);                
                this.ucDiePriceListDetailsData.UserControlLoad();
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Грешка в btnNewDiePriceListDetails_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        protected void lnkBtnServerEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckUserActionPermission(ETEMEnums.SecuritySettings.DiePriceListDetailsPreview, false))
                {
                    return;
                }

                LinkButton lnkBtnServerEdit = sender as LinkButton;
                if (lnkBtnServerEdit == null)
                {
                    ShowMSG("lnkBtnServerEdit is null");
                }

                string idRowMasterKey = BaseHelper.ParseStringByAmpersand(lnkBtnServerEdit.CommandArgument)["idRowMasterKey"].ToString();

                this.ucDiePriceListDetailsData.CurrentEntityMasterID = idRowMasterKey;
                this.ucDiePriceListDetailsData.SetHdnField(idRowMasterKey);
                this.ucDiePriceListDetailsData.UserControlLoad();
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Грешка в lnkBtnServerEdit_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckUserActionPermission(ETEMEnums.SecuritySettings.DiePriceListDetailsDelete, false))
                {
                    return;
                }

                List<int> listSelectedIDsToDelete = new List<int>();

                CheckBox chbxCheckForDeletion = new CheckBox();
                HiddenField hdnIdEntity = new HiddenField();
                foreach (GridViewRow row in this.gvDiePriceListDetails.Rows)
                {
                    chbxCheckForDeletion = row.FindControl("chbxCheckForDeletion") as CheckBox;
                    if (chbxCheckForDeletion != null && chbxCheckForDeletion.Checked)
                    {
                        hdnIdEntity = row.FindControl("hdnIdEntity") as HiddenField;
                        if (hdnIdEntity != null)
                        {
                            listSelectedIDsToDelete.Add(Int32.Parse(hdnIdEntity.Value));
                        }
                    }
                }

                if (listSelectedIDsToDelete.Count == 0)
                {
                    base.ShowMSG("Please select rows for deletion!", false);
                    return;
                }

                this.CallContext = this.CostCalculationRef.DiePriceListDetailDelete(listSelectedIDsToDelete, this.CallContext);

                base.ShowMSG(this.CallContext.Message, false);

                LoadFilteredList();
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Грешка в btnDelete_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        protected void gvDiePriceListDetails_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.GridViewSortExpression = e.SortExpression;
            base.SetSortDirection();
            LoadFilteredList();
        }

        protected void gvDiePriceListDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.NewPageIndex = e.NewPageIndex;
            LoadFilteredList();
        }

        protected void chbxCheckOrUncheckAll_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox chbx = sender as CheckBox;

            BaseHelper.SelectOrDeselectAllEnabledGridCheckBox(this.gvDiePriceListDetails, chbx.Checked, "chbxCheckForDeletion");
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

        protected void btnDefault_Click(object sender, EventArgs e)
        {
            ClearFilterForm();

            this.tbxDateFromTo.SetTxbDateTimeValue(DateTime.Today);

            LoadFilteredList();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFilterForm();

            LoadFilteredList();
        }

        private void ClearFilterForm()
        {
            this.tbxDateFromTo.Text = string.Empty;
            this.ddlVendor.SelectedValue = Constants.INVALID_ID_STRING;

            this.ddlNumberOfCavities.SelectedValue = Constants.INVALID_ID_STRING;
            this.ddlProfileCategory.SelectedValue = Constants.INVALID_ID_STRING;
            this.ddlProfileComplexity.SelectedValue = Constants.INVALID_ID_STRING;
            this.tbxDimensionA_From.Text = string.Empty;
            this.tbxDimensionA_To.Text = string.Empty;
            this.tbxDimensionB_From.Text = string.Empty;
            this.tbxDimensionB_To.Text = string.Empty;
            this.tbxDiePriceFrom.Text = string.Empty;
            this.tbxDiePriceTo.Text = string.Empty;
            this.tbxLifespanFrom.Text = string.Empty;
            this.tbxLifespanTo.Text = string.Empty;
        }

        private void AddCustomSearchCriterias(ICollection<AbstractSearch> searchCriteria)
        {
            if (this.ddlVendor.SelectedValue != Constants.INVALID_ID_STRING)
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.Equal,
                        Property = "idVendor",
                        SearchTerm = this.ddlVendor.SelectedValueINT
                    });
            }
            if (this.ddlNumberOfCavities.SelectedValue != Constants.INVALID_ID_STRING)
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.Equal,
                        Property = "idNumberOfCavities",
                        SearchTerm = this.ddlNumberOfCavities.SelectedValueINT
                    });
            }
            if (this.ddlProfileCategory.SelectedValue != Constants.INVALID_ID_STRING)
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.Equal,
                        Property = "idProfileCategory",
                        SearchTerm = this.ddlProfileCategory.SelectedValueINT
                    });
            }
            if (this.ddlProfileComplexity.SelectedValue != Constants.INVALID_ID_STRING)
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.Equal,
                        Property = "idProfileComplexity",
                        SearchTerm = this.ddlProfileComplexity.SelectedValueINT
                    });
            }
            if (!string.IsNullOrWhiteSpace(this.tbxDimensionA_From.Text))
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.GreaterOrEqual,
                        Property = "DimensionA",
                        SearchTerm = Convert.ToInt32(this.tbxDimensionA_From.Text)
                    });
            }
            if (!string.IsNullOrWhiteSpace(this.tbxDimensionA_To.Text))
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.LessOrEqual,
                        Property = "DimensionA",
                        SearchTerm = Convert.ToInt32(this.tbxDimensionA_To.Text)
                    });
            }
            if (!string.IsNullOrWhiteSpace(this.tbxDimensionB_From.Text))
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.GreaterOrEqual,
                        Property = "DimensionB",
                        SearchTerm = Convert.ToInt32(this.tbxDimensionB_From.Text)
                    });
            }
            if (!string.IsNullOrWhiteSpace(this.tbxDimensionB_To.Text))
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.LessOrEqual,
                        Property = "DimensionB",
                        SearchTerm = Convert.ToInt32(this.tbxDimensionB_To.Text)
                    });
            }
            if (!string.IsNullOrWhiteSpace(this.tbxDiePriceFrom.Text))
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.GreaterOrEqual,
                        Property = "Price",
                        SearchTerm = Convert.ToDecimal(this.tbxDiePriceFrom.Text)
                    });
            }
            if (!string.IsNullOrWhiteSpace(this.tbxDiePriceTo.Text))
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.LessOrEqual,
                        Property = "Price",
                        SearchTerm = Convert.ToDecimal(this.tbxDiePriceTo.Text)
                    });
            }
            if (!string.IsNullOrWhiteSpace(this.tbxLifespanFrom.Text))
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.GreaterOrEqual,
                        Property = "Lifespan",
                        SearchTerm = Convert.ToDecimal(this.tbxLifespanFrom.Text)
                    });
            }
            if (!string.IsNullOrWhiteSpace(this.tbxLifespanTo.Text))
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.LessOrEqual,
                        Property = "Lifespan",
                        SearchTerm = Convert.ToDecimal(this.tbxLifespanTo.Text)
                    });
            }
        }
    }
}