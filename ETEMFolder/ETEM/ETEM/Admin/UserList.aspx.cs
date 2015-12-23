using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Models;
using ETEMModel.Models.DataView;
using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Helpers.Common;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace ETEM.Admin
{
    public partial class UserList : BasicPage
    {
        public static FormResources formResource = new FormResources
        {
            Module = Constants.MODULE_PERMISSION,
            PageFullName = Constants.UMS_ADMIN_USERLIST,
            PagePath = "../Admin/UserList.aspx"

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
            if (this.gvUsers.Rows.Count > 0)
            {
                ((LinkButton)this.gvUsers.HeaderRow.Cells[3].Controls[0]).Text = GetCaption("GridView_Users_UserName");
                ((LinkButton)this.gvUsers.HeaderRow.Cells[4].Controls[0]).Text = GetCaption("GridView_Users_Person");
                ((LinkButton)this.gvUsers.HeaderRow.Cells[5].Controls[0]).Text = GetCaption("GridView_Users_Status");
                ((LinkButton)this.gvUsers.HeaderRow.Cells[6].Controls[0]).Text = GetCaption("GridView_Users_Description");
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
                CheckUserActionPermission(ETEMEnums.SecuritySettings.UserListShow, true);

                LoadFilterControls();
                this.ddlPagingRowsCount.UserControlLoad();

                this.gvUsers.PageSize = Int32.Parse(GetSettingByCode(ETEMModel.Helpers.ETEMEnums.AppSettings.PageSize).SettingValue);

                LoadFilteredList();


                //this.btnUpdateStudentInELearning.Text = "Акуализация на студенти в ДО за " + GeneralPage.GetSettingByCode(UMSEnums.AppSettings.CurrentYear).SettingValue;
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в FormLoad " + formResource.PagePath);
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
                    this.gvUsers.AllowPaging = false;
                }
                else
                {
                    this.gvUsers.AllowPaging = true;
                    this.gvUsers.PageIndex = 0;
                    this.gvUsers.PageSize = Convert.ToInt32(this.ddlPagingRowsCount.SelectedValue);
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

        private void BindDataWithPaging()
        {
            try
            {
                if (NewPageIndex.HasValue)
                {
                    this.gvUsers.PageIndex = NewPageIndex.Value;
                }
                this.gvUsers.DataBind();
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в BindDataWithPaging " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        public void LoadSearchResult(List<UserDataView> listUsers)
        {
            try
            {


                if (this.ddlRole.SelectedValueINT != Constants.INVALID_ID)
                {
                    listUsers = listUsers.Where(u => u.UserRoleLinkList.Where(r => r.idRole == this.ddlRole.SelectedValueINT).Count() > 0).ToList();
                }

                this.gvUsers.DataSource = listUsers;
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

                List<UserDataView> listUsers = base.AdminClientRef.GetAllUsers(searchCriteria,
                                                                               base.GridViewSortExpression,
                                                                               base.GridViewSortDirection);

                LoadSearchResult(listUsers);
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


                this.AlphaBetCtrl.UserControlLoad();

                this.ddlStatus.UserControlLoad();


                this.ddlFaculty.UserControlLoad();
                this.ddlForeigner.UserControlLoad();



                this.ddlStudentStatus.UserControlLoad();
                this.tbxAutoCompleteSpecialityFilter.UserControlLoad();
                this.ddlCourse.UserControlLoad();
                this.ddlStudentDegree.UserControlLoad();
                this.ddlStatus.UserControlLoad();
                this.ddlRole.UserControlLoad();
            }
            catch { }



        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {


                if (!CheckUserActionPermission(ETEMEnums.SecuritySettings.UserSave, false))
                {
                    return;
                }

                this.ucUserData.TabContainerActiveTabIndex = 0;

                this.ucUserData.CurrentEntityMasterID = Constants.INVALID_ID_STRING;
                this.ucUserData.SetHdnField(Constants.INVALID_ID_STRING);
                this.ucUserData.UserControlLoad();
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


                if (!CheckUserActionPermission(ETEMEnums.SecuritySettings.UserSave, false))
                {
                    return;
                }

                LinkButton lnkBtnServerEdit = sender as LinkButton;
                if (lnkBtnServerEdit == null)
                {
                    ShowMSG("lnkBtnServerEdit is null");
                }

                this.ucUserData.TabContainerActiveTabIndex = 0;

                string idRowMasterKey = BaseHelper.ParseStringByAmpersand(lnkBtnServerEdit.CommandArgument)["idRowMasterKey"].ToString();

                this.ucUserData.CurrentEntityMasterID = idRowMasterKey;
                this.ucUserData.SetHdnField(idRowMasterKey);
                this.ucUserData.UserControlLoad();
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в lnkBtnServerEdit_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {

        }

        protected void gvUsers_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.GridViewSortExpression = e.SortExpression;
            base.SetSortDirection();
            LoadFilteredList();
        }

        protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.NewPageIndex = e.NewPageIndex;
            LoadFilteredList();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            this.pnlFilterData.Visible = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
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
            this.tbxUserName.Text = string.Empty;
            this.ddlStatus.SelectedValue = Constants.INVALID_ID_STRING;
            this.tbxFirstName.Text = string.Empty;
            this.tbxSecondName.Text = string.Empty;
            this.tbxLastName.Text = string.Empty;
            this.tbxEGN.Text = string.Empty;
            this.tbxIdentityNumber.Text = string.Empty;

            this.tbxFacultyNo.Text = string.Empty;
            this.tbxAutoCompleteSpecialityFilter.Text = string.Empty;
            this.tbxAutoCompleteSpecialityFilter.SelectedValue = Constants.INVALID_ID_STRING;

            this.ddlStudentStatus.SelectedValue = Constants.INVALID_ID_STRING;
            this.ddlStudentDegree.SelectedValue = Constants.INVALID_ID_STRING;
            this.ddlCourse.SelectedValue = Constants.INVALID_ID_STRING;
            this.ddlForeigner.SelectedValue = Constants.INVALID_ID_STRING;
            this.ddlFaculty.SelectedValue = Constants.INVALID_ID_STRING;
            this.ddlRole.SelectedValue = Constants.INVALID_ID_STRING;
        }

        private void AddCustomSearchCriterias(ICollection<AbstractSearch> searchCriteria)
        {
            if (!string.IsNullOrEmpty(this.AlphaBetCtrl.SelectedLetter) &&
                this.AlphaBetCtrl.SelectedLetter != Constants.ALL_CHARACTERS)
            {
                searchCriteria.Add(new TextSearch
                {
                    Comparator = TextComparators.StartsWith,
                    Property = "FirstName",
                    SearchTerm = this.AlphaBetCtrl.SelectedLetter
                });
            }

            if (!string.IsNullOrEmpty(this.tbxUserName.Text))
            {
                searchCriteria.Add(
                    new TextSearch
                    {
                        Comparator = TextComparators.Contains,
                        Property = "UserName",
                        SearchTerm = this.tbxUserName.Text
                    });
            }

            if (this.ddlStatus.SelectedValue != Constants.INVALID_ID_STRING)
            {
                searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.Equal,
                        Property = "idStatus",
                        SearchTerm = this.ddlStatus.SelectedValueINT
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

            if (!string.IsNullOrEmpty(this.tbxEGN.Text))
            {
                searchCriteria.Add(
                    new TextSearch
                    {
                        Comparator = TextComparators.Contains,
                        Property = "EGN",
                        SearchTerm = this.tbxEGN.Text
                    });
            }

            if (!string.IsNullOrEmpty(this.tbxIdentityNumber.Text))
            {
                searchCriteria.Add(
                    new TextSearch
                    {
                        Comparator = TextComparators.Contains,
                        Property = "IdentityNumber",
                        SearchTerm = this.tbxIdentityNumber.Text
                    });
            }


            if (!string.IsNullOrEmpty(this.tbxFacultyNo.Text))
            {
                searchCriteria.Add(
                    new TextSearch
                    {
                        Comparator = TextComparators.Contains,
                        Property = "FacultyNo",
                        SearchTerm = this.tbxFacultyNo.Text
                    });
            }
            if (this.ddlFaculty.SelectedValueINT != Constants.INVALID_ID)
            {
                searchCriteria.Add(
                   new NumericSearch
                   {
                       Comparator = NumericComparators.Equal,
                       Property = "idFaculty",
                       SearchTerm = ddlFaculty.SelectedValueINT
                   });
            }

            if (this.tbxAutoCompleteSpecialityFilter.SelectedValueIntOrInvalidID != Constants.INVALID_ID)
            {
                searchCriteria.Add(
                   new NumericSearch
                   {
                       Comparator = NumericComparators.Equal,
                       Property = "idSpecialty",
                       SearchTerm = this.tbxAutoCompleteSpecialityFilter.SelectedValueIntOrInvalidID
                   });
            }
            if (this.ddlCourse.SelectedValueINT != Constants.INVALID_ID)
            {
                searchCriteria.Add(
                   new NumericSearch
                   {
                       Comparator = NumericComparators.Equal,
                       Property = "idCourse",
                       SearchTerm = this.ddlCourse.SelectedValueINT
                   });
            }
            if (this.ddlStudentDegree.SelectedValueINT != Constants.INVALID_ID)
            {
                searchCriteria.Add(
                   new NumericSearch
                   {
                       Comparator = NumericComparators.Equal,
                       Property = "idOKC",
                       SearchTerm = this.ddlStudentDegree.SelectedValueINT
                   });
            }
            if (this.ddlStudentStatus.SelectedValueINT != Constants.INVALID_ID)
            {
                searchCriteria.Add(
                   new NumericSearch
                   {
                       Comparator = NumericComparators.Equal,
                       Property = "idStudentStatus",
                       SearchTerm = this.ddlStudentStatus.SelectedValueINT
                   });
            }

            if (this.ddlForeigner.SelectedValueINT != Constants.INVALID_ID)
            {
                KeyValue kvForeignerYes = this.GetKeyValueByIntCode("YES_NO", "Yes");

                searchCriteria.Add(
                   new BooleanSearch
                   {
                       Comparator = BooleanComparators.Equal,
                       Property = "IsForeigner",
                       SearchTerm = (ddlForeigner.SelectedValueINT == kvForeignerYes.idKeyValue) ? true : false
                   });
            }
        }

        protected void chbxSelectOrDeselectAll_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox chbx = sender as CheckBox;

            BaseHelper.SelectOrDeselectAllEnabledGridCheckBox(this.gvUsers, chbx.Checked, "chbxCheckForSend");
        }

        protected void btnSendEmailsPopUp_Click(object sender, EventArgs e)
        {
            try
            {


                List<int> listSelectedIDs = new List<int>();
                CheckBox chbxCheckForSend = new CheckBox();
                HiddenField hdnIdEntity = new HiddenField();
                foreach (GridViewRow row in this.gvUsers.Rows)
                {
                    chbxCheckForSend = row.FindControl("chbxCheckForSend") as CheckBox;

                    if (chbxCheckForSend != null && chbxCheckForSend.Checked)
                    {
                        hdnIdEntity = row.FindControl("hdnIdEntity") as HiddenField;

                        if (hdnIdEntity != null && !listSelectedIDs.Contains(Int32.Parse(hdnIdEntity.Value)))
                        {
                            listSelectedIDs.Add(Int32.Parse(hdnIdEntity.Value));
                        }
                    }
                }

                if (listSelectedIDs.Count == 0)
                {
                    base.ShowMSG(BaseHelper.GetCaptionString("Form_Email_Please_Select_Recipient_User"));
                    return;
                }

                this.tbxSubject.Text = GetCaption("Entity_User_Send_Password_Subject");
                this.tbxBody.Text = GetCaption("Entity_User_Send_Password_Body");

                this.pnlSendEmail.Visible = true;
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в btnSendEmailsPopUp_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        protected void btnCancelSendEmail_Click(object sender, EventArgs e)
        {
            this.pnlSendEmail.Visible = false;
        }

        protected void btnSendEmails_Click(object sender, EventArgs e)
        {
            try
            {


                string errorMsg = string.Empty;
                if (string.IsNullOrEmpty(this.tbxSubject.Text.Trim()))
                {
                    errorMsg = "`" + this.lbSubject.Text + "`";
                }
                if (string.IsNullOrEmpty(this.tbxBody.Text.Trim()))
                {
                    errorMsg += (string.IsNullOrEmpty(errorMsg) ? string.Empty : ", ") + "`" + this.lbBody.Text + "`";
                }

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    base.ShowJavaScriptMSG(string.Format(BaseHelper.GetCaptionString("Form_Email_Subject_And_Body_Required"), errorMsg));
                    return;
                }

                List<int> listSelectedIDs = new List<int>();
                CheckBox chbxCheckForSend = new CheckBox();
                HiddenField hdnIdEntity = new HiddenField();
                foreach (GridViewRow row in this.gvUsers.Rows)
                {
                    chbxCheckForSend = row.FindControl("chbxCheckForSend") as CheckBox;

                    if (chbxCheckForSend != null && chbxCheckForSend.Checked)
                    {
                        hdnIdEntity = row.FindControl("hdnIdEntity") as HiddenField;

                        if (hdnIdEntity != null && !listSelectedIDs.Contains(Int32.Parse(hdnIdEntity.Value)))
                        {
                            listSelectedIDs.Add(Int32.Parse(hdnIdEntity.Value));
                        }
                    }
                }

                this.CallContext.CurrentYear = Convert.ToInt32(GeneralPage.GetSettingByCode(ETEMEnums.AppSettings.CurrentYear).SettingValue);

                if (this.CallContext.ListKvParams.Where(w => w.Key == ETEMEnums.AppSettings.SendExternalMail.ToString()).Count() == 0)
                {
                    this.CallContext.ListKvParams.Add(new KeyValuePair<string, object>(ETEMEnums.AppSettings.SendExternalMail.ToString(),
                                                      GetSettingByCode(ETEMEnums.AppSettings.SendExternalMail).SettingValue));
                }
                if (this.CallContext.ListKvParams.Where(w => w.Key == ETEMEnums.AppSettings.DefaultEmail.ToString()).Count() == 0)
                {
                    this.CallContext.ListKvParams.Add(new KeyValuePair<string, object>(ETEMEnums.AppSettings.DefaultEmail.ToString(),
                                                      GetSettingByCode(ETEMEnums.AppSettings.DefaultEmail).SettingValue));
                }
                if (this.CallContext.ListKvParams.Where(w => w.Key == ETEMEnums.AppSettings.MailServer.ToString()).Count() == 0)
                {
                    this.CallContext.ListKvParams.Add(new KeyValuePair<string, object>(ETEMEnums.AppSettings.MailServer.ToString(),
                                                      GetSettingByCode(ETEMEnums.AppSettings.MailServer).SettingValue));
                }
                if (this.CallContext.ListKvParams.Where(w => w.Key == ETEMEnums.AppSettings.MailServerPort.ToString()).Count() == 0)
                {
                    this.CallContext.ListKvParams.Add(new KeyValuePair<string, object>(ETEMEnums.AppSettings.MailServerPort.ToString(),
                                                      GetSettingByCode(ETEMEnums.AppSettings.MailServerPort).SettingValue));
                }
                if (this.CallContext.ListKvParams.Where(w => w.Key == ETEMEnums.AppSettings.MailFromPassword.ToString()).Count() == 0)
                {
                    this.CallContext.ListKvParams.Add(new KeyValuePair<string, object>(ETEMEnums.AppSettings.MailFromPassword.ToString(),
                                                      GetSettingByCode(ETEMEnums.AppSettings.MailFromPassword).SettingValue));
                }
                if (this.CallContext.ListKvParams.Where(w => w.Key == ETEMEnums.AppSettings.MailServerPop3.ToString()).Count() == 0)
                {
                    this.CallContext.ListKvParams.Add(new KeyValuePair<string, object>(ETEMEnums.AppSettings.MailServerPop3.ToString(),
                                                      GetSettingByCode(ETEMEnums.AppSettings.MailServerPop3).SettingValue));
                }
                if (this.CallContext.ListKvParams.Where(w => w.Key == ETEMEnums.AppSettings.MailServerPop3Port.ToString()).Count() == 0)
                {
                    this.CallContext.ListKvParams.Add(new KeyValuePair<string, object>(ETEMEnums.AppSettings.MailServerPop3Port.ToString(),
                                                      GetSettingByCode(ETEMEnums.AppSettings.MailServerPop3Port).SettingValue));
                }
                if (this.CallContext.ListKvParams.Where(w => w.Key == ETEMEnums.AppSettings.WaitCheckMailDeliveryInMinutes.ToString()).Count() == 0)
                {
                    this.CallContext.ListKvParams.Add(new KeyValuePair<string, object>(ETEMEnums.AppSettings.WaitCheckMailDeliveryInMinutes.ToString(),
                                                      GetSettingByCode(ETEMEnums.AppSettings.WaitCheckMailDeliveryInMinutes).SettingValue));
                }

                SendMailHelper sendMailData = new SendMailHelper()
                {
                    SubjectBG = this.tbxSubject.Text.Trim(),
                    BodyBG = this.tbxBody.Text.Trim()
                };

                ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();
                AddCustomSearchCriterias(searchCriteria);

                this.CallContext = this.AdminClientRef.UserSendingEmails(searchCriteria,
                                                                               listSelectedIDs,
                                                                               sendMailData,
                                                                               this.CallContext);

                if (this.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
                {
                    ShowMSG(this.CallContext.Message);

                    RemoveModalWindow();

                    this.pnlSendEmail.Visible = false;
                }
                else
                {
                    ShowMSG(this.CallContext.Message);
                }
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в btnSendEmails_Click " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

        public override void AlphabetClick(string alpha)
        {
            if (alpha == Constants.ALL_CHARACTERS)
            {
                List<UserDataView> listUsers = base.AdminClientRef.GetAllUsers(new List<AbstractSearch>(),
                                                                               base.GridViewSortExpression,
                                                                               base.GridViewSortDirection);
                ClearFilterForm();
                LoadSearchResult(listUsers);
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

                List<UserDataView> listUsers = base.AdminClientRef.GetAllUsers(searchCriteria,
                                                                               base.GridViewSortExpression,
                                                                               base.GridViewSortDirection);

                LoadSearchResult(listUsers);
            }
        }
        private static bool ValidateRemoteCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors policyErrors)
        {
            bool result = false;
            if (cert.Subject.ToUpper().Contains(""))
            {
                result = true;
            }

            return result;
        }
        

        

        protected void btnUpdateLectureInELearning_Click(object sender, EventArgs e)
        {

        }
    }
}