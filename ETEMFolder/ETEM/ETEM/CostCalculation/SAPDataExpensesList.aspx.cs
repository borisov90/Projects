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
    public partial class SAPDataExpensesList : BasicPage
    {
        public static FormResources formResource = new FormResources
        {
            Module = Constants.MODULE_COST_CALCULATION,
            PageFullName = Constants.ETEM_COSTCALCULATION_SAPDATAEXPENSESLIST,
            PagePath = "../CostCalculation/SAPDataExpensesList.aspx"
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FormLoad();
            }
        }

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
            //if (this..Rows.Count > 0)
            //{
            //    //((LinkButton)this.gvUsers.HeaderRow.Cells[3].Controls[0]).Text = GetCaption("GridView_Users_UserName");
            //    //((LinkButton)this.gvUsers.HeaderRow.Cells[4].Controls[0]).Text = GetCaption("GridView_Users_Person");
            //    //((LinkButton)this.gvUsers.HeaderRow.Cells[5].Controls[0]).Text = GetCaption("GridView_Users_Status");
            //    //((LinkButton)this.gvUsers.HeaderRow.Cells[6].Controls[0]).Text = GetCaption("GridView_Users_Description");
            //}
        }
        #endregion

        public override void FormLoad()
        {
            try
            {
                CheckUserActionPermission(ETEMEnums.SecuritySettings.SAPDataExpenseListView, true);

                string pageSize = GetSettingByCode(ETEMModel.Helpers.ETEMEnums.AppSettings.PageSize).SettingValue;

                this.ddlPagingRowsCount.UserControlLoad();
                this.ddlPagingRowsCount.SetDefaultValue(pageSize);

                LoadFilterControls();

                this.gvSAPDataExpenses.PageSize = Int32.Parse(pageSize);

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
                    this.gvSAPDataExpenses.PageIndex = NewPageIndex.Value;
                }
                this.gvSAPDataExpenses.DataBind();
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Грешка в BindDataWithPaging " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        public void LoadSearchResult(List<SAPDataExpenseDataView> listSAPDataExpenses)
        {
            try
            {
                //                if (this.ddlRole.SelectedValueINT != Constants.INVALID_ID)
                //                {
                ////                    listUsers = listUsers.Where(u => u.UserRoleLinkList.Where(r => r.idRole == this.ddlRole.SelectedValueINT).Count() > 0).ToList();
                //                }

                this.gvSAPDataExpenses.DataSource = listSAPDataExpenses;

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

                List<SAPDataExpenseDataView> listSAPDataExpenses = new List<SAPDataExpenseDataView>();

                DateTime? dateActiveTo = null;
                if (!string.IsNullOrWhiteSpace(this.tbxDateFromTo.Text))
                {
                    dateActiveTo = this.tbxDateFromTo.TextAsDateParseExact;
                }

                listSAPDataExpenses = base.CostCalculationRef.GetAllSAPDataExpense(searchCriteria,
                                                                                   dateActiveTo,
                                                                                   base.GridViewSortExpression,
                                                                                   base.GridViewSortDirection);

                LoadSearchResult(listSAPDataExpenses);
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
                this.ddlExpenseType.UserControlLoad();
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
                    this.gvSAPDataExpenses.AllowPaging = false;
                }
                else
                {
                    this.gvSAPDataExpenses.AllowPaging = true;
                    this.gvSAPDataExpenses.PageIndex = 0;
                    this.gvSAPDataExpenses.PageSize = Convert.ToInt32(this.ddlPagingRowsCount.SelectedValue);
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
                if (!CheckUserActionPermission(ETEMEnums.SecuritySettings.SAPDataExpenseSave, false))
                {
                    return;
                }

                //this.ucSAPDataExpenseData.CurrentEntityMasterID = Constants.INVALID_ID_STRING;
                //this.ucSAPDataExpenseData.SetHdnField(Constants.INVALID_ID_STRING);
                //this.ucSAPDataExpenseData.UserControlLoad();
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
                if (!CheckUserActionPermission(ETEMEnums.SecuritySettings.SAPDataExpensePreview, false))
                {
                    return;
                }

                LinkButton lnkBtnServerEdit = sender as LinkButton;
                if (lnkBtnServerEdit == null)
                {
                    ShowMSG("lnkBtnServerEdit is null");
                }

                string idRowMasterKey = BaseHelper.ParseStringByAmpersand(lnkBtnServerEdit.CommandArgument)["idRowMasterKey"].ToString();

                this.ucSAPDataExpenseData.CurrentEntityMasterID = idRowMasterKey;
                this.ucSAPDataExpenseData.SetHdnField(idRowMasterKey);
                this.ucSAPDataExpenseData.UserControlLoad();
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
                if (!CheckUserActionPermission(ETEMEnums.SecuritySettings.SAPDataExpenseDelete, false))
                {
                    return;
                }

                List<int> listSelectedIDsToDelete = new List<int>();

                CheckBox chbxCheckForDeletion = new CheckBox();
                HiddenField hdnIdEntity = new HiddenField();
                foreach (GridViewRow row in this.gvSAPDataExpenses.Rows)
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

                this.CallContext = this.CostCalculationRef.SAPDataExpenseDelete(listSelectedIDsToDelete, this.CallContext);

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

        protected void gvSAPDataExpenses_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.GridViewSortExpression = e.SortExpression;
            base.SetSortDirection();
            LoadFilteredList();
        }

        protected void gvSAPDataExpenses_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.NewPageIndex = e.NewPageIndex;
            LoadFilteredList();
        }

        protected void chbxCheckOrUncheckAll_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox chbx = sender as CheckBox;

            BaseHelper.SelectOrDeselectAllEnabledGridCheckBox(this.gvSAPDataExpenses, chbx.Checked, "chbxCheckForDeletion");
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
            this.ddlExpenseType.SelectedValue = Constants.INVALID_ID_STRING;

            this.tbxValueDataFrom.Text = string.Empty;
            this.tbxValueDataTo.Text = string.Empty;
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
            if (this.ddlExpenseType.SelectedValue != Constants.INVALID_ID_STRING)
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.Equal,
                        Property = "idExpensesType",
                        SearchTerm = this.ddlExpenseType.SelectedValueINT
                    });
            }
            if (!string.IsNullOrWhiteSpace(this.tbxValueDataFrom.Text))
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.GreaterOrEqual,
                        Property = "ValueData",
                        SearchTerm = Convert.ToDecimal(this.tbxValueDataFrom.Text)
                    });
            }
            if (!string.IsNullOrWhiteSpace(this.tbxValueDataTo.Text))
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.LessOrEqual,
                        Property = "ValueData",
                        SearchTerm = Convert.ToDecimal(this.tbxValueDataTo.Text)
                    });
            }
        }
    }
}