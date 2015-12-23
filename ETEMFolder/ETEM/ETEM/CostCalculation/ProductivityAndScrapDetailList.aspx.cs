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

namespace ETEM.CostCalculation
{
    public partial class ProductivityAndScrapDetailList : BasicPage
    {
        public static FormResources formResource = new FormResources
        {
            Module = Constants.MODULE_COST_CALCULATION,
            PageFullName = Constants.ETEM_COSTCALCULATION_PRODUCTIVITYANDSCRAPDETAILLIST,
            PagePath = "../CostCalculation/ProductivityAndScrapDetailList.aspx"
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
            if (this.gvProductivityAndScrapDetail.Rows.Count > 0)
            {

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
                CheckUserActionPermission(ETEMEnums.SecuritySettings.ProductivityAndScrapListView, true);

                string pageSize = GetSettingByCode(ETEMModel.Helpers.ETEMEnums.AppSettings.PageSize).SettingValue;

                this.ddlPagingRowsCount.UserControlLoad();
                this.ddlPagingRowsCount.SetDefaultValue(pageSize);

                LoadFilterControls();

                this.gvProductivityAndScrapDetail.PageSize = Int32.Parse(pageSize);

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
                    this.gvProductivityAndScrapDetail.PageIndex = NewPageIndex.Value;
                }
                this.gvProductivityAndScrapDetail.DataBind();
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Грешка в BindDataWithPaging " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        public void LoadSearchResult(List<ProductivityAndScrapDetailDataView> listProductivityAndScrapDetail)
        {
            try
            {
                //                if (this.ddlRole.SelectedValueINT != Constants.INVALID_ID)
                //                {
                ////                    listUsers = listUsers.Where(u => u.UserRoleLinkList.Where(r => r.idRole == this.ddlRole.SelectedValueINT).Count() > 0).ToList();
                //                }

                this.gvProductivityAndScrapDetail.DataSource = listProductivityAndScrapDetail;

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

                List<ProductivityAndScrapDetailDataView> listProductivityAndScrapDetail = new List<ProductivityAndScrapDetailDataView>();

                DateTime? dateActiveTo = null;
                if (!string.IsNullOrWhiteSpace(this.tbxDateFromTo.Text))
                {
                    dateActiveTo = this.tbxDateFromTo.TextAsDateParseExact;
                }

                listProductivityAndScrapDetail = base.CostCalculationRef.GetAllProductivityAndScrapDetailList(searchCriteria,
                                                                                                              dateActiveTo,
                                                                                                              base.GridViewSortExpression,
                                                                                                              base.GridViewSortDirection);

                LoadSearchResult(listProductivityAndScrapDetail);
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

                this.ddlCostCenter.UserControlLoad();
                this.ddlProfileSetting.UserControlLoad();
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
                    this.gvProductivityAndScrapDetail.AllowPaging = false;
                }
                else
                {
                    this.gvProductivityAndScrapDetail.AllowPaging = true;
                    this.gvProductivityAndScrapDetail.PageIndex = 0;
                    this.gvProductivityAndScrapDetail.PageSize = Convert.ToInt32(this.ddlPagingRowsCount.SelectedValue);
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

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckUserActionPermission(ETEMEnums.SecuritySettings.ProductivityAndScrapSave, false))
                {
                    return;
                }

                this.ucProductivityAndScrapData.CurrentEntityMasterID = Constants.INVALID_ID_STRING;
                this.ucProductivityAndScrapData.SetHdnField(Constants.INVALID_ID_STRING);
                this.ucProductivityAndScrapData.UserControlLoad();
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Грешка в btnNew_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        protected void btnNewDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckUserActionPermission(ETEMEnums.SecuritySettings.ProductivityAndScrapSave, false))
                {
                    return;
                }

                this.ucProductivityAndScrapDetailData.CurrentEntityMasterID = Constants.INVALID_ID_STRING;
                this.ucProductivityAndScrapDetailData.SetHdnField(Constants.INVALID_ID_STRING);
                this.ucProductivityAndScrapDetailData.UserControlLoad();
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Грешка в btnNew_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        protected void lnkBtnServerEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckUserActionPermission(ETEMEnums.SecuritySettings.ProductivityAndScrapPreview, false))
                {
                    return;
                }

                LinkButton lnkBtnServerEdit = sender as LinkButton;
                if (lnkBtnServerEdit == null)
                {
                    ShowMSG("lnkBtnServerEdit is null");
                }

                string idRowMasterKey = BaseHelper.ParseStringByAmpersand(lnkBtnServerEdit.CommandArgument)["idRowMasterKey"].ToString();

                this.ucProductivityAndScrapDetailData.CurrentEntityMasterID = idRowMasterKey;
                this.ucProductivityAndScrapDetailData.SetHdnField(idRowMasterKey);
                this.ucProductivityAndScrapDetailData.UserControlLoad();
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
                if (!CheckUserActionPermission(ETEMEnums.SecuritySettings.ProductivityAndScrapDelete, false))
                {
                    return;
                }

                List<int> listSelectedIDsToDelete = new List<int>();

                CheckBox chbxCheckForDeletion = new CheckBox();
                HiddenField hdnIdEntity = new HiddenField();
                foreach (GridViewRow row in this.gvProductivityAndScrapDetail.Rows)
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

                this.CallContext = this.CostCalculationRef.ProductivityAndScrapDetailDelete(listSelectedIDsToDelete, this.CallContext);

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

        protected void gvProductivityAndScrapDetail_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.GridViewSortExpression = e.SortExpression;
            base.SetSortDirection();
            LoadFilteredList();
        }

        protected void gvProductivityAndScrapDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.NewPageIndex = e.NewPageIndex;
            LoadFilteredList();
        }

        protected void chbxCheckOrUncheckAll_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox chbx = sender as CheckBox;

            BaseHelper.SelectOrDeselectAllEnabledGridCheckBox(this.gvProductivityAndScrapDetail, chbx.Checked, "chbxCheckForDeletion");
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

            this.ddlCostCenter.SelectedValue = Constants.INVALID_ID_STRING;
            this.ddlProfileSetting.SelectedValue = Constants.INVALID_ID_STRING;
            this.tbxSumOfHoursFrom.Text = string.Empty;
            this.tbxSumOfHoursTo.Text = string.Empty;
            this.tbxSumOfConsumptionFrom.Text = string.Empty;
            this.tbxSumOfConsumptionTo.Text = string.Empty;
            this.tbxSumOfProductionFrom.Text = string.Empty;
            this.tbxSumOfProductionTo.Text = string.Empty;
            this.tbxProductivityKGhFrom.Text = string.Empty;
            this.tbxProductivityKGhTo.Text = string.Empty;
            this.tbxScrapRateFrom.Text = string.Empty;
            this.tbxScrapRateTo.Text = string.Empty;
        }

        private void AddCustomSearchCriterias(ICollection<AbstractSearch> searchCriteria)
        {
            if (this.ddlCostCenter.SelectedValue != Constants.INVALID_ID_STRING)
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.Equal,
                        Property = "idCostCenter",
                        SearchTerm = this.ddlCostCenter.SelectedValueINT
                    });
            }
            if (this.ddlProfileSetting.SelectedValue != Constants.INVALID_ID_STRING)
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.Equal,
                        Property = "idProfileSetting",
                        SearchTerm = this.ddlProfileSetting.SelectedValueINT
                    });
            }
            if (!string.IsNullOrWhiteSpace(this.tbxSumOfHoursFrom.Text))
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.GreaterOrEqual,
                        Property = "SumOfHours",
                        SearchTerm = Convert.ToDecimal(this.tbxSumOfHoursFrom.Text)
                    });
            }
            if (!string.IsNullOrWhiteSpace(this.tbxSumOfHoursTo.Text))
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.LessOrEqual,
                        Property = "SumOfHours",
                        SearchTerm = Convert.ToDecimal(this.tbxSumOfHoursTo.Text)
                    });
            }
            if (!string.IsNullOrWhiteSpace(this.tbxSumOfConsumptionFrom.Text))
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.GreaterOrEqual,
                        Property = "SumOfConsumption",
                        SearchTerm = Convert.ToDecimal(this.tbxSumOfConsumptionFrom.Text)
                    });
            }
            if (!string.IsNullOrWhiteSpace(this.tbxSumOfConsumptionTo.Text))
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.LessOrEqual,
                        Property = "SumOfConsumption",
                        SearchTerm = Convert.ToDecimal(this.tbxSumOfConsumptionTo.Text)
                    });
            }
            if (!string.IsNullOrWhiteSpace(this.tbxSumOfProductionFrom.Text))
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.GreaterOrEqual,
                        Property = "SumOfProduction",
                        SearchTerm = Convert.ToDecimal(this.tbxSumOfProductionFrom.Text)
                    });
            }
            if (!string.IsNullOrWhiteSpace(this.tbxSumOfProductionTo.Text))
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.LessOrEqual,
                        Property = "SumOfProduction",
                        SearchTerm = Convert.ToDecimal(this.tbxSumOfProductionTo.Text)
                    });
            }
            if (!string.IsNullOrWhiteSpace(this.tbxProductivityKGhFrom.Text))
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.GreaterOrEqual,
                        Property = "ProductivityKGh",
                        SearchTerm = Convert.ToDecimal(this.tbxProductivityKGhFrom.Text)
                    });
            }
            if (!string.IsNullOrWhiteSpace(this.tbxSumOfProductionTo.Text))
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.LessOrEqual,
                        Property = "ProductivityKGh",
                        SearchTerm = Convert.ToDecimal(this.tbxSumOfProductionTo.Text)
                    });
            }
            if (!string.IsNullOrWhiteSpace(this.tbxScrapRateFrom.Text))
            {
                decimal scrapRate = BaseHelper.ConvertToDecimalOrZero(this.tbxScrapRateFrom.Text, 9) / 100;

                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.GreaterOrEqual,
                        Property = "ScrapRate",
                        SearchTerm = scrapRate
                    });
            }
            if (!string.IsNullOrWhiteSpace(this.tbxScrapRateTo.Text))
            {
                decimal scrapRate = BaseHelper.ConvertToDecimalOrZero(this.tbxScrapRateTo.Text, 9) / 100;

                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.LessOrEqual,
                        Property = "ScrapRate",
                        SearchTerm = scrapRate
                    });
            }
        }
    }
}