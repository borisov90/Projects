using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Models.DataView;

namespace ETEM.Admin
{
    public partial class PersonList : BasicPage
    {
        public static FormResources formResource = new FormResources
        {
            Module = Constants.MODULE_NOMENCLATURES,
            PageFullName = Constants.UMS_ADMIN_PERSONLIST,
            PagePath = "../Admin/PersonList.aspx"

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

        protected void btnFilter_Click(object sender, EventArgs e)
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
                    this.GridViewSortExpression = "FirstName";

                }

                this.gvPerson.DataSource = AdminClientRef.GetAllPersons(searchCriteria, GridViewSortExpression, GridViewSortDirection);
                if (NewPageIndex.HasValue)
                {
                    this.gvPerson.PageIndex = NewPageIndex.Value;
                }
                this.gvPerson.DataBind();
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в FormLoad " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        public void LoadSearchResult(List<PersonDataView> listStudents)
        {
            this.gvPerson.DataSource = listStudents;
            BindDataWithPaging();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {


                ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();
                AddCustomSearchCriterias(searchCriteria);
                List<PersonDataView> list = AdminClientRef.GetAllPersons(searchCriteria, GridViewSortExpression, GridViewSortDirection);


                LoadSearchResult(list);
                this.pnlFilterData.Visible = false;
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в btnSearch_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        private void BindDataWithPaging()
        {
            if (NewPageIndex.HasValue)
            {
                this.gvPerson.PageIndex = NewPageIndex.Value;
            }
            this.gvPerson.DataBind();
        }

        public override void AlphabetClick(string alpha)
        {
            try
            {


                if (alpha == Constants.ALL_CHARACTERS)
                {
                    List<AbstractSearch> searchCriteria = new List<AbstractSearch>();

                    List<PersonDataView> list = AdminClientRef.GetAllPersons(searchCriteria, GridViewSortExpression, GridViewSortDirection);

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
                        Property = "FirstName",
                        SearchTerm = alpha
                    });
                    List<PersonDataView> list = AdminClientRef.GetAllPersons(searchCriteria, GridViewSortExpression, GridViewSortDirection);


                    LoadSearchResult(list);
                }
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в AlphabetClick " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        public void LoadPersonsList()
        {
            try
            {
                ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();
                AddCustomSearchCriterias(searchCriteria);
                List<PersonDataView> list = AdminClientRef.GetAllPersons(searchCriteria, GridViewSortExpression, GridViewSortDirection);
                LoadSearchResult(list);
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в LoadPersonsList " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }







        }

        private void ClearFilterForm()
        {
            this.AlphaBetCtrl.SelectedLetter = string.Empty;
            this.tbxFirstName.Text = "";
            this.tbxSecondName.Text = "";
            this.tbxLastName.Text = "";
        }

        private void AddCustomSearchCriterias(ICollection<AbstractSearch> searchCriteria)
        {
            if (!string.IsNullOrEmpty(this.AlphaBetCtrl.SelectedLetter))
            {
                searchCriteria.Add(new TextSearch
                {
                    Comparator = TextComparators.StartsWith,
                    Property = "FirstName",
                    SearchTerm = this.AlphaBetCtrl.SelectedLetter
                });
            }

            if (!string.IsNullOrEmpty(this.tbxFirstName.Text))
            {
                searchCriteria.Add(
                    new TextSearch
                    {
                        Comparator = TextComparators.Contains,
                        Property = "FirstName",
                        SearchTerm = this.tbxFirstName.Text
                    });
            }

            if (!string.IsNullOrEmpty(this.tbxSecondName.Text))
            {
                searchCriteria.Add(
                    new TextSearch
                    {
                        Comparator = TextComparators.Contains,
                        Property = "SecondName",
                        SearchTerm = this.tbxSecondName.Text
                    });
            }

            if (!string.IsNullOrEmpty(this.tbxLastName.Text))
            {
                searchCriteria.Add(
                    new TextSearch
                    {
                        Comparator = TextComparators.Contains,
                        Property = "LastName",
                        SearchTerm = this.tbxLastName.Text
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

                this.PersonMainData.CurrentEntityMasterID = idRowMasterKey;
                this.PersonMainData.SetHdnField(idRowMasterKey);
                this.PersonMainData.UserControlLoad();

                this.PersonMainData.Visible = true;

            }
            catch (Exception ex)
            {
                BaseHelper.Log("Грешка в lnkBtnServerEdit_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        protected void bntNew_Click(object sender, EventArgs e)
        {
            try
            {


                this.PersonMainData.CurrentEntityMasterID = Constants.INVALID_ID_STRING;
                this.PersonMainData.SetHdnField(Constants.INVALID_ID_STRING);

                //this.PersonMainData.ClearForm();
                this.PersonMainData.UserControlLoad();
                this.PersonMainData.Visible = true;
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в bntNew_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        protected void gvPerson_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                this.GridViewSortExpression = e.SortExpression;
                base.SetSortDirection();
                FormLoad();
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в gvPerson_Sorting " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

        }

        protected void gvPerson_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.NewPageIndex = e.NewPageIndex;
                FormLoad();
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в gvPerson_Sorting " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

        }


        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFilterForm();

            LoadPersonsList();
        }



    }
}