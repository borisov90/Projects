using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using System.Data;
using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Models.DataView.CostCalculation;

namespace ETEM.CostCalculation
{
    public partial class ProfilesList : BasicPage
    {
        public static FormResources formResource = new FormResources
        {
            Module          = Constants.MODULE_COST_CALCULATION,
            PageFullName    = Constants.ETEM_COSTCALCULATION_PROFILESLIST,
            PagePath        = "../CostCalculation/ProfilesList.aspx"

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
            this.ucProfilesListCtrl.UserControlLoad();
        }

        //public override void FormLoad()
        //{
        //    try
        //    {
        //        loadInitControls();
        //        LoadProfilesList();
        //    }
        //    catch (Exception ex)
        //    {
        //        BaseHelper.Log("Erron in Method FormLoad " + formResource.PagePath);
        //        BaseHelper.Log(ex.Message);
        //        BaseHelper.Log(ex.StackTrace);
        //    }
        //}

        //private void loadInitControls()
        //{
        //    try
        //    {
        //        this.ddlPagingRowsCount.UserControlLoad();
        //    }
        //    catch (Exception ex)
        //    {


        //    }
        //}


        //protected void btnFilter_Click(object sender, EventArgs e)
        //{
           
        //}

        //protected void btnNew_Click(object sender, EventArgs e)
        //{
        //    this.ucProfile.CurrentEntityMasterID = Constants.INVALID_ID_STRING; ;
        //    this.ucProfile.UserControlLoad();
        //    this.ucProfile.Visible = true;
        //}

        //protected void lnkBtnServerEdit_Click(object sender, EventArgs e)
        //{
            

        //    LinkButton lnkBtnServerEdit = sender as LinkButton;

        //    if (lnkBtnServerEdit == null)
        //    {
        //        ShowMSG("lnkBtnServerEdit is null");
        //        return;
        //    }

        //    this.ucProfile.CurrentEntityMasterID = Constants.INVALID_ID_STRING; ;
        //    this.ucProfile.UserControlLoad();
        //    this.ucProfile.Visible = true;
        //}

        //public void LoadProfilesList()
        //{
        //    CheckUserActionPermission(ETEMEnums.SecuritySettings.ProfilesShowList, true);

        //    this.gvProfilesList.PageSize = Int32.Parse(GetSettingByCode(ETEMModel.Helpers.ETEMEnums.AppSettings.PageSize).SettingValue);

        //    LoadFilteredList();
        //}

        //public void LoadFilteredList()
        //{
        //    try
        //    {
        //        ICollection<AbstractSearch> searchCriteria          = new List<AbstractSearch>();
        //        AddCustomSearchCriterias(searchCriteria);

        //        List<ProfileSettingDataView> listProfileSetting     = new List<ProfileSettingDataView>();
        //        listProfileSetting                                  = base.CostCalculationRef.GetProfilesList(searchCriteria, base.GridViewSortExpression, base.GridViewSortDirection);

        //        LoadSearchResult(listProfileSetting);
        //    }
        //    catch (Exception ex)
        //    {

        //        BaseHelper.Log("Erron in Method LoadFilteredList " + formResource.PagePath);
        //        BaseHelper.Log(ex.Message);
        //        BaseHelper.Log(ex.StackTrace);
        //    }
        //}

        //public void LoadSearchResult(List<ProfileSettingDataView> listProfileSetting)
        //{
        //    try
        //    {

        //        this.gvProfilesList.DataSource = listProfileSetting;

        //        BindDataWithPaging();
        //    }
        //    catch (Exception ex)
        //    {
        //        BaseHelper.Log("Error in LoadSearchResult " + formResource.PagePath);
        //        BaseHelper.Log(ex.Message);
        //        BaseHelper.Log(ex.StackTrace);
        //    }
        //}

        //private void BindDataWithPaging()
        //{
        //    try
        //    {
        //        if (NewPageIndex.HasValue)
        //        {
        //            this.gvProfilesList.PageIndex = NewPageIndex.Value;
        //        }
        //        this.gvProfilesList.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        BaseHelper.Log("Error in BindDataWithPaging " + formResource.PagePath);
        //        BaseHelper.Log(ex.Message);
        //        BaseHelper.Log(ex.StackTrace);
        //    }
        //}

        //private void AddCustomSearchCriterias(ICollection<AbstractSearch> searchCriteria)
        //{

        //}       

        //protected void btnClear_Click(object sender, EventArgs e)
        //{
        //    //ClearFilterForm();
        //}

        //protected void btnCancelFilter_OnClick(object sender, EventArgs e)
        //{
        //    this.pnlFilterData.Visible = false;
        //}

        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    //LoadFilteredList();

        //    this.pnlFilterData.Visible = false;
        //}

        //protected void ddlPagingRowsCount_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (this.ddlPagingRowsCount.SelectedValue == ETEMEnums.PagingRowsCountEnum.AllRows.ToString())
        //        {
        //            this.gvProfilesList.AllowPaging = false;
        //        }
        //        else
        //        {
        //            this.gvProfilesList.AllowPaging = true;
        //            this.gvProfilesList.PageIndex   = 0;
        //            this.gvProfilesList.PageSize    = Convert.ToInt32(this.ddlPagingRowsCount.SelectedValue);
        //        }

        //        LoadFilteredList();
        //    }
        //    catch (Exception ex)
        //    {
        //        BaseHelper.Log("Erron in Method ddlPagingRowsCount_SelectedIndexChanged " + formResource.PagePath);
        //        BaseHelper.Log(ex.Message);
        //        BaseHelper.Log(ex.StackTrace);
        //    }
        //}

    }
}