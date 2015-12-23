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
    
    
    public partial class AverageOutturnOverTimeList : BasicPage
    {
        public static FormResources formResource = new FormResources
        {
            Module = Constants.MODULE_COST_CALCULATION,
            PageFullName = Constants.ETEM_COSTCALCULATION_AVERAGEOUTTURNOVERTIMELIST,
            PagePath = "../CostCalculation/AverageOutturnOverTimeList.aspx"
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
            if (this.gvAverageOutturnOverTimeList.Rows.Count > 0)
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
                CheckUserActionPermission(ETEMEnums.SecuritySettings.AverageOutturnOverTimeListView, true);

                string pageSize = GetSettingByCode(ETEMModel.Helpers.ETEMEnums.AppSettings.PageSize).SettingValue;

                this.ddlPagingRowsCount.UserControlLoad();
                this.ddlPagingRowsCount.SetDefaultValue(pageSize);

                LoadFilterControls();

                this.gvAverageOutturnOverTimeList.PageSize = Int32.Parse(pageSize);

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
                    this.gvAverageOutturnOverTimeList.PageIndex = NewPageIndex.Value;
                }
                this.gvAverageOutturnOverTimeList.DataBind();
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Грешка в BindDataWithPaging " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        public void LoadSearchResult(List<AverageOutturnOverTimeDataView> listAverageOutturnOverTimeDetails)
        {
            try
            {
                //                if (this.ddlRole.SelectedValueINT != Constants.INVALID_ID)
                //                {
                ////                    listUsers = listUsers.Where(u => u.UserRoleLinkList.Where(r => r.idRole == this.ddlRole.SelectedValueINT).Count() > 0).ToList();
                //                }

                this.gvAverageOutturnOverTimeList.DataSource = listAverageOutturnOverTimeDetails;

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

                List<AverageOutturnOverTimeDataView> listAverageOutturnOverTimeLists = new List<AverageOutturnOverTimeDataView>();

                DateTime? dateActiveTo = null;
                if (!string.IsNullOrWhiteSpace(this.tbxDateFromTo.Text))
                {
                    dateActiveTo = this.tbxDateFromTo.TextAsDateParseExact;
                }

                listAverageOutturnOverTimeLists = base.CostCalculationRef.GetAllAverageOutturnOverTimeDataView(searchCriteria,
                                                                                         dateActiveTo,
                                                                                         base.GridViewSortExpression,
                                                                                         base.GridViewSortDirection);

                LoadSearchResult(listAverageOutturnOverTimeLists);
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
                    this.gvAverageOutturnOverTimeList.AllowPaging = false;
                }
                else
                {
                    this.gvAverageOutturnOverTimeList.AllowPaging = true;
                    this.gvAverageOutturnOverTimeList.PageIndex = 0;
                    this.gvAverageOutturnOverTimeList.PageSize = Convert.ToInt32(this.ddlPagingRowsCount.SelectedValue);
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
                if (!CheckUserActionPermission(ETEMEnums.SecuritySettings.AverageOutturnOverTimeListSave, false))
                {
                    return;
                }

                this.ucAverageOutturnOverTimeData.CurrentEntityMasterID = Constants.INVALID_ID_STRING;
                this.ucAverageOutturnOverTimeData.SetHdnField(Constants.INVALID_ID_STRING);
                this.ucAverageOutturnOverTimeData.UserControlLoad();
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
                if (!CheckUserActionPermission(ETEMEnums.SecuritySettings.AverageOutturnOverTimeListPreview, false))
                {
                    return;
                }

                LinkButton lnkBtnServerEdit = sender as LinkButton;
                if (lnkBtnServerEdit == null)
                {
                    ShowMSG("lnkBtnServerEdit is null");
                }

                string idRowMasterKey = BaseHelper.ParseStringByAmpersand(lnkBtnServerEdit.CommandArgument)["idRowMasterKey"].ToString();

                this.ucAverageOutturnOverTimeData.CurrentEntityMasterID = idRowMasterKey;
                this.ucAverageOutturnOverTimeData.SetHdnField(idRowMasterKey);
                this.ucAverageOutturnOverTimeData.UserControlLoad();
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
                if (!CheckUserActionPermission(ETEMEnums.SecuritySettings.AverageOutturnOverTimeListDelete, false))
                {
                    return;
                }

                List<int> listSelectedIDsToDelete = new List<int>();

                CheckBox chbxCheckForDeletion = new CheckBox();
                HiddenField hdnIdEntity = new HiddenField();
                foreach (GridViewRow row in this.gvAverageOutturnOverTimeList.Rows)
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

                this.CallContext = this.CostCalculationRef.AverageOutturnOverTimeDelete(listSelectedIDsToDelete, this.CallContext);

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

        protected void gvAverageOutturnOverTimeList_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.GridViewSortExpression = e.SortExpression;
            base.SetSortDirection();
            LoadFilteredList();
        }

        protected void gvAverageOutturnOverTimeList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.NewPageIndex = e.NewPageIndex;
            LoadFilteredList();
        }

        protected void chbxCheckOrUncheckAll_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox chbx = sender as CheckBox;

            BaseHelper.SelectOrDeselectAllEnabledGridCheckBox(this.gvAverageOutturnOverTimeList, chbx.Checked, "chbxCheckForDeletion");
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

            
        }

        private void AddCustomSearchCriterias(ICollection<AbstractSearch> searchCriteria)
        {
            
        }
    }
}