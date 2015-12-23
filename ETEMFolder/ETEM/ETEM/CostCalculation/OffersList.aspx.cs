using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Models.DataView;
using ETEMModel.Models.DataView.CostCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETEM.CostCalculation
{
    
    public partial class OffersList : BasicPage
    {
        public static FormResources formResource = new FormResources
        {
            Module          = Constants.MODULE_COST_CALCULATION,
            PageFullName    = Constants.ETEM_COSTCALCULATION_OFFERSLIST,
            PagePath        = "../CostCalculation/OffersList.aspx"

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

        public void btnFilter_Click(object sender, EventArgs e)
        {
            this.pnlFilterData.Visible = true;
        }

        public override void FormLoad()
        {
            try
            {

                ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();
                AddCustomSearchCriterias(searchCriteria);

                if (string.IsNullOrEmpty(this.GridViewSortExpression) || this.GridViewSortExpression == Constants.INVALID_ID_STRING)
                {
                    this.GridViewSortExpression = "OfferDate";

                }

                List<OfferDataView> listOfferDataView = CostCalculationRef.GetAllOfferDataView(searchCriteria, GridViewSortExpression, GridViewSortDirection);

                //ако има права може да вижда всичко, ако няма права вижда тези, които е редактирал или създал
                if (!CheckUserActionPermission(ETEMEnums.SecuritySettings.ShowOfferFullList, false))
                {
                    listOfferDataView = listOfferDataView.Where(z => z.idCreateUser.Value == Convert.ToInt32(this.UserProps.IdUser) ||
                                                                    z.idModifyUser == Convert.ToInt32(this.UserProps.IdUser)).ToList();
                }

                this.gvOffer.DataSource = CostCalculationRef.GetAllOfferDataView(searchCriteria, GridViewSortExpression, GridViewSortDirection);

                if (NewPageIndex.HasValue)
                {
                    this.gvOffer.PageIndex = NewPageIndex.Value;
                }
                this.gvOffer.DataBind();
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Error in FormLoad " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        public void LoadSearchResult(List<OfferDataView> listStudents)
        {
            this.gvOffer.DataSource = listStudents;
            BindDataWithPaging();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {


                ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();
                AddCustomSearchCriterias(searchCriteria);
                List<OfferDataView> list = CostCalculationRef.GetAllOfferDataView(searchCriteria, GridViewSortExpression, GridViewSortDirection);


                LoadSearchResult(list);
                this.pnlFilterData.Visible = false;
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Error in btnSearch_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        private void BindDataWithPaging()
        {
            if (NewPageIndex.HasValue)
            {
                this.gvOffer.PageIndex = NewPageIndex.Value;
            }
            this.gvOffer.DataBind();
        }

        public override void AlphabetClick(string alpha)
        {
            try
            {


                if (alpha == Constants.ALL_CHARACTERS)
                {
                    List<AbstractSearch> searchCriteria = new List<AbstractSearch>();

                    List<OfferDataView> list = CostCalculationRef.GetAllOfferDataView(searchCriteria, GridViewSortExpression, GridViewSortDirection);

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
                    List<OfferDataView> list = CostCalculationRef.GetAllOfferDataView(searchCriteria, GridViewSortExpression, GridViewSortDirection);


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

        public void LoadOfferList()
        {
            try
            {
                ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();
                AddCustomSearchCriterias(searchCriteria);
                List<OfferDataView> list = CostCalculationRef.GetAllOfferDataView(searchCriteria, GridViewSortExpression, GridViewSortDirection);
                LoadSearchResult(list);
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Error in LoadPersonsList " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }







        }

        private void ClearFilterForm()
        {
         
            this.tbxCustomer.Text           = string.Empty;
            this.tbxInquiryNumber.Text      = string.Empty;
            this.tbxInquiryDateFrom.Text    = string.Empty;
            this.tbxInquiryDateTo.Text      = string.Empty;
            this.tbxProfileSettingName.Text = string.Empty;
        }

        private void AddCustomSearchCriterias(ICollection<AbstractSearch> searchCriteria)
        {
            //ако има права може да вижда всичко, ако няма права вижда тези, които е редактирал или създал
            //if (!CheckUserActionPermission(ETEMEnums.SecuritySettings.ShowOfferFullList, false))
            //{
            //    searchCriteria.Add(
            //        new NumericSearch
            //        {
            //            Comparator  = NumericComparators.Equal,
            //            Property    = "idModifyUser",
            //            SearchTerm  = Convert.ToInt32(this.UserProps.IdUser)
            //        });
            //}


            if (!string.IsNullOrEmpty(this.tbxCustomer.Text))
            {
                searchCriteria.Add(
                    new TextSearch
                    {
                        Comparator  = TextComparators.Contains,
                        Property    = "Customer",
                        SearchTerm  = this.tbxCustomer.Text
                    });
            }

            if (!string.IsNullOrEmpty(this.tbxInquiryNumber.Text))
            {
                searchCriteria.Add(
                    new TextSearch
                    {
                        Comparator  = TextComparators.Contains,
                        Property    = "InquiryNumber",
                        SearchTerm  = this.tbxInquiryNumber.Text
                    });
            }
            //Inquiry Date From
            if (!string.IsNullOrEmpty(this.tbxInquiryDateFrom.Text))
            {
                searchCriteria.Add(
                    new DateSearch
                    {
                        Comparator  = DateComparators.GreaterOrEqual,
                        Property    = "OfferDate",
                        SearchTerm  = this.tbxInquiryDateFrom.TextAsDateParseExact
                    });
            }
            //Inquiry Date To
            if (!string.IsNullOrEmpty(this.tbxInquiryDateTo.Text))
            {
                searchCriteria.Add(
                    new DateSearch
                    {
                        Comparator  = DateComparators.LessOrEqual,
                        Property    = "OfferDate",
                        SearchTerm  = Convert.ToDateTime(this.tbxInquiryDateTo.TextAsDateParseExact)
                    });
            }
            //hdnIdProfileSettingFilter
            if (!string.IsNullOrEmpty(this.hdnProfileID.Value))
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator  = NumericComparators.Equal,
                        Property    = "idProfileSetting",
                        SearchTerm  = Convert.ToInt32(this.hdnProfileID.Value)
                    });
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

                this.OfferMainData1.CurrentEntityMasterID = idRowMasterKey;
                this.OfferMainData1.SetHdnField(idRowMasterKey);
                this.OfferMainData1.UserControlLoad();

                this.OfferMainData1.Visible = true;

            }
            catch (Exception ex)
            {
                BaseHelper.Log("Error in lnkBtnServerEdit_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        protected void bntNew_Click(object sender, EventArgs e)
        {
            try
            {


                this.OfferMainData1.CurrentEntityMasterID = Constants.INVALID_ID_STRING;
                this.OfferMainData1.SetHdnField(Constants.INVALID_ID_STRING);

                
                this.OfferMainData1.UserControlLoad();
                this.OfferMainData1.Visible = true;
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Error in bntNew_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        protected void gvOffer_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                this.GridViewSortExpression = e.SortExpression;
                base.SetSortDirection();
                FormLoad();
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Error in gvOffer_Sorting " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

        }

        protected void gvOffer_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.NewPageIndex = e.NewPageIndex;
                FormLoad();
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Error in gvOffer_OnPageIndexChanging " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFilterForm();

            LoadOfferList();
        }

        protected void btnChoose_Click(object sender, EventArgs e)
        {
        
            this.ucProfilesListCtrlChooseOffer.UserControlLoad();
            this.ucProfilesListCtrlChooseOffer.Visible = true;

            this.hdnInquiryNumber.Value     = this.tbxInquiryNumber.Text;
            this.hdnCustomer.Value          = this.tbxCustomer.Text;
            this.hdnInquiryDateFrom.Value   = this.tbxInquiryDateFrom.TextAsDateParseExact.HasValue ? this.tbxInquiryDateFrom.TextAsDateParseExact.Value.ToString("dd.MM.yyyy"): string.Empty;
            this.hdnInquiryDateTo.Value     = this.tbxInquiryDateTo.TextAsDateParseExact.HasValue ? this.tbxInquiryDateTo.TextAsDateParseExact.Value.ToString("dd.MM.yyyy"): string.Empty;

        }

        public void ReloadFilterPanel()
        {
            LoadOfferList();
            this.Visible = true;
            this.Focus();
            this.pnlFilterData.Focus();
            this.pnlFilterData.Visible = true;
            

            this.tbxInquiryNumber.Text          = this.hdnInquiryNumber.Value;
            this.tbxCustomer.Text               = this.hdnCustomer.Value;
            this.tbxProfileSettingName.Text     = this.hdnIdProfileSettingText.Value;
            this.tbxInquiryDateFrom.Text        = this.hdnInquiryDateFrom.Value;
            this.tbxInquiryDateTo.Text          = this.hdnInquiryDateTo.Value;

        }


        
    }
}