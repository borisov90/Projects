using ETEM.Controls.Common;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Models;
using ETEMModel.Models.DataView;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETEM.Controls.Admin
{
    public partial class PersonalDataReduced : BaseUserControl
    {
        private ETEMModel.Models.Person currentEntity;

        public void SetHdnField(string value)
        {
            this.hdnRowMasterKey.Value = value;
        }

        public string NoLoadUserPanel
        {
            get { return this.hdnNoLoadUserPanel.Value; }
            set { this.hdnNoLoadUserPanel.Value = value; }
        }

        public string MasterKeyID
        {
            get { return this.hdnRowMasterKey.Value; }
            set { this.hdnRowMasterKey.Value = value; }
        }

        public string IdStudentCandidate
        {
            get { return this.hdnIdStudentCandidate.Value; }
            set { this.hdnIdStudentCandidate.Value = value; }
        }

        public string DetailsKeyID
        {
            get { return this.hdnDetailsKey.Value; }
            set { this.hdnDetailsKey.Value = value; }
        }

        public string ParentControlID
        {
            get { return this.hdnParentControlID.Value; }
            set { this.hdnParentControlID.Value = value; }
        }

        public Panel PnlDivSearchStudent
        {
            get { return this.pnlDivSearchStudent; }
        }

        public string PersonUsedFor { get; set; }

        public string PersonInfo { get; set; }

        public override void ClearResultContext()
        {
            this.lbResultContext.Text = string.Empty;
        }

        public int IdAcademicPeriod
        {
            get { return (string.IsNullOrWhiteSpace(this.hdnIdAcademicPeriod.Value) ? Constants.INVALID_ID : Convert.ToInt32(this.hdnIdAcademicPeriod.Value)); }
            set { this.hdnIdAcademicPeriod.Value = value.ToString(); }
        }

        private void SetEmptyValues()
        {
            this.lbResultContext.Attributes.Remove("class");
            ClearResultContext(this.lbResultContext);

            this.tbxIdentityNumber.Text             = string.Empty;
            this.tbxFirstName.Text                  = string.Empty;
            this.tbxSecondName.Text                 = string.Empty;
            this.tbxLastName.Text                   = string.Empty;
            this.tbxBirthDate.Text                  = string.Empty;
            this.tbxEMail.Text                      = string.Empty;
            this.tbxMobilePhone.Text                = string.Empty;
            this.tbxPhone.Text                      = string.Empty;
            this.dllSex.SelectedValue               = Constants.INVALID_ID_STRING;
            this.tbxEGN.Text                        = string.Empty;
            this.tbxIDN.Text                        = string.Empty;
            this.tbxIdentityNumber.Text             = string.Empty;
            this.tbxTitle.Text                      = string.Empty;            
            this.hdnRowMasterKey.Value              = string.Empty;
            this.imgBtnPerson.ImageUrl              = string.Empty;
            
        }

        private void LoadDdlLoadStudentInfoBy()
        {
            this.ddlLoadStudentInfoBy.Items.Clear();

            ListItem item = new ListItem();
            item.Value = "Name";
            item.Text = BaseHelper.GetCaptionString("Entity_CampusApplication_FirstName");
            item.Selected = true;
            this.ddlLoadStudentInfoBy.Items.Add(item);

            if (this.PersonUsedFor != ETEMEnums.PersonTypeEnum.StudentCandidate.ToString())
            {
                item = new ListItem();
                item.Value = "EGN";
                item.Text = BaseHelper.GetCaptionString("Entity_CampusApplication_EGN");
                this.ddlLoadStudentInfoBy.Items.Add(item);

                item = new ListItem();
                item.Value = "FacultyNo";
                item.Text = BaseHelper.GetCaptionString("Entity_CampusApplication_FacultyNo");
                this.ddlLoadStudentInfoBy.Items.Add(item);
            }
        }

        public override void UserControlLoad()
        {
            SetEmptyValues();

            if (NoLoadUserPanel == Constants.TRUE_VALUE_TEXT)
            {
                this.pnlDivSearchStudent.Visible = false;
            }

            if (this.ownerPage == null)
            {
                throw new UMSException("Current Page is null or is not inheritor of BasicPage.");
            }

            loadInitControls();

            this.currentEntity = this.ownerPage.AdminClientRef.GetPersonByPersonID(this.CurrentEntityMasterID);

            if (currentEntity != null)
            {
                this.pnlDivSearchStudent.Visible    = false;
                this.tbxIdentityNumber.Text         = currentEntity.IdentityNumber;
                this.tbxFirstName.Text              = currentEntity.FirstName;
                this.tbxSecondName.Text             = currentEntity.SecondName;
                this.tbxLastName.Text               = currentEntity.LastName;                
                this.dllSex.SelectedValue           = currentEntity.idSex.ToString();
                this.tbxBirthDate.SetTxbDateTimeValue(currentEntity.BirthDate);
                this.tbxEMail.Text                  = currentEntity.EMail;
                this.tbxMobilePhone.Text            = currentEntity.MobilePhone;
                this.tbxPhone.Text                  = currentEntity.Phone;
                this.tbxEGN.Text                    = currentEntity.EGN;
                this.tbxIDN.Text                    = currentEntity.IDN;
                this.tbxIdentityNumber.Text         = currentEntity.IdentityNumber;
                this.tbxTitle.Text                  = currentEntity.Title;
                this.hdnRowMasterKey.Value          = currentEntity.idPerson.ToString();
                ClearResultContext(this.lbResultContext);
                this.lbResultContext.Attributes.Remove("class");

                if (!string.IsNullOrEmpty(currentEntity.ImagePath))
                {
                    this.imgBtnPerson.ImageUrl = currentEntity.ImagePath;
                }
                else
                {
                    this.imgBtnPerson.ImageUrl = @"~/Images/person-clip-art-2.png";
                }

                this.PersonInfo = this.currentEntity.FullName;

                LoadPersonHistory();
            }
            else
            {
                LoadDdlLoadStudentInfoBy();

                if (this.PersonUsedFor == ETEMEnums.PersonTypeEnum.Student.ToString())
                {
                    this.ucTextForSearchStudent.CustomCase              = "StudentAndCandidateByName";
                    this.ucTextForSearchStudent.AdditionalWhereParam    = (this.IdAcademicPeriod != Constants.INVALID_ID ?
                                                                            "idAcademicPeriod=" + this.IdAcademicPeriod : string.Empty);
                }
                else if (this.PersonUsedFor == ETEMEnums.PersonTypeEnum.StudentCandidate.ToString())
                {
                    this.ucTextForSearchStudent.CustomCase = "PreparationANDStudentAndCandidateByName";
                }
                else if (this.PersonUsedFor == ETEMEnums.PersonTypeEnum.ArtModel.ToString())
                {
                    this.ucTextForSearchStudent.CustomCase = "PersonALLByName";
                }
                else if (this.PersonUsedFor == ETEMEnums.PersonTypeEnum.PhD.ToString())
                {
                    this.ucTextForSearchStudent.CustomCase = "PersonsAllTypes";
                }
                else
                {
                    this.ucTextForSearchStudent.CustomCase = "PersonALLByName";
                }

                this.ucTextForSearchStudent.Text = string.Empty;
                this.ucTextForSearchStudent.UserControlLoad();

                SetEmptyValues();

                this.gvPersonHistory.DataSource = null;
                this.gvPersonHistory.DataBind();
            }
        }

        private void LoadPersonHistory()
        {
            List<PersonHistoryDataView> listPersonHistory   = this.ownerPage.AdminClientRef.GetPersonHistoryDataViewByPersonID(Int32.Parse(this.MasterKeyID));
            this.gvPersonHistory.DataSource                 = listPersonHistory;
            this.gvPersonHistory.DataBind();
        }

        public override Tuple<CallContext, string> UserControlSave()
        {
            bool isNewEntity = true;

            if (string.IsNullOrEmpty(this.hdnRowMasterKey.Value) || this.hdnRowMasterKey.Value == Constants.INVALID_ID_STRING)
            {
                this.currentEntity = new Person();
            }
            else
            {
                this.currentEntity = this.ownerPage.AdminClientRef.GetPersonByPersonID(this.hdnRowMasterKey.Value);

                if (this.currentEntity == null)
                {
                    this.lbResultContext.Text   = String.Format(BaseHelper.GetCaptionString("Entity_Person_Not_Found_By_ID"), this.hdnRowMasterKey.Value);

                    string falseResult          = String.Format(BaseHelper.GetCaptionString("Entity_is_not_update_in_tab"),
                                                                BaseHelper.GetCaptionString("Personal_Data"));

                    this.ownerPage.CallContext.ResultCode   = ETEMEnums.ResultEnum.Error;
                    this.ownerPage.CallContext.Message      = falseResult;

                    return new Tuple<CallContext, string>(this.ownerPage.CallContext, BaseHelper.GetCaptionString("Personal_Data"));
                }

                isNewEntity = false;
            }

            currentEntity.IdentityNumber = this.tbxIdentityNumber.Text;

            currentEntity.FirstName     = this.tbxFirstName.Text;
            currentEntity.SecondName    = this.tbxSecondName.Text;
            currentEntity.LastName      = this.tbxLastName.Text;            
            currentEntity.idSex         = this.dllSex.SelectedValueINT;
            DateTime? tmpBirthDate      = null;

            if (this.tbxBirthDate.TextAsDateParseExact.HasValue)
            {
                tmpBirthDate = this.tbxBirthDate.TextAsDateParseExact;
            }
            else if (!string.IsNullOrEmpty(this.tbxEGN.Text))
            {
                try
                {
                    string firstPartEGN     = this.tbxEGN.Text.Substring(0, 6);
                    int year                = int.Parse(firstPartEGN.Substring(0, 2));
                    int month               = int.Parse(firstPartEGN.Substring(2, 2));
                    int day                 = int.Parse(firstPartEGN.Substring(4, 2));

                    if (month > 40)
                    {
                        month = month - 40;
                        year = year + 2000;
                    }
                    else
                    {
                        year = year + 1900;
                    }



                    tmpBirthDate = new DateTime(
                          year,
                          month,
                          day
                        );
                    this.tbxBirthDate.Text = tmpBirthDate.Value.ToString("dd.MM.yyyy");
                }


                catch { }
            }

            currentEntity.BirthDate                         = tmpBirthDate;
            currentEntity.EMail                             = this.tbxEMail.Text;
            currentEntity.MobilePhone                       = this.tbxMobilePhone.Text;
            currentEntity.Phone                             = this.tbxPhone.Text;
            currentEntity.EGN                               = this.tbxEGN.Text;
            currentEntity.IDN                               = this.tbxIDN.Text;
            currentEntity.IdentityNumber                    = this.tbxIdentityNumber.Text;
            currentEntity.Title                             = this.tbxTitle.Text;
            this.ownerPage.CallContext.CurrentConsumerID    = this.ownerPage.UserProps.IdUser;
            this.ownerPage.CallContext                      = this.ownerPage.AdminClientRef.PersonSave(currentEntity, this.ownerPage.CallContext);

            if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {
                this.lbResultContext.Text   = this.ownerPage.CallContext.Message;
                this.hdnRowMasterKey.Value  = this.ownerPage.CallContext.EntityID;

                CheckIfResultIsSuccess(lbResultContext);

                this.PersonInfo = this.currentEntity.FullName;

                if (!string.IsNullOrEmpty(PersonUsedFor))
                {
                    if (PersonUsedFor == ETEMEnums.PersonTypeEnum.Employe.ToString())
                    {
                        if (isNewEntity)
                        {
                            Employe newEmploye          = new Employe();
                            newEmploye.idPerson         = Convert.ToInt32(this.ownerPage.CallContext.EntityID);
                            CallContext callContext     = new CallContext();
                            callContext                 = this.ownerPage.EmployeClientRef.EmployeSave(newEmploye, callContext);

                            if (callContext.ResultCode == ETEMEnums.ResultEnum.Success)
                            {
                                this.DetailsKeyID = callContext.EntityID;
                            }
                            else
                            {
                                this.DetailsKeyID = Constants.INVALID_ID_STRING;
                            }
                        }
                    }

                }
            }

            this.pnlDivSearchStudent.Visible = false;

            if (string.IsNullOrWhiteSpace(this.PersonUsedFor))
            {
                this.ownerPage.FormLoad();
            }


            return new Tuple<CallContext, string>(this.ownerPage.CallContext, BaseHelper.GetCaptionString("Personal_Data"));
        }

        private void loadInitControls()
        {
            this.dllSex.UserControlLoad();

            //показва полето 'Титла' само за  преподаватели
            if (!string.IsNullOrEmpty(this.PersonUsedFor) && PersonUsedFor == Constants.PERSON_LECTURER)
            {
                this.divTitle.Visible   = true;
                this.divBR.Visible      = false;
            }
            else
            {
                this.divTitle.Visible   = false;
                this.divBR.Visible      = true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            UserControlSave();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        protected void imgBtnPersonImage_Click(object sender, ImageClickEventArgs e)
        {
            this.pnlAddPersonImage.Visible = true;
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //update на снимката на потребителя
            if (!string.IsNullOrEmpty(this.hdnRowMasterKey.Value) && this.hdnRowMasterKey.Value != Constants.INVALID_ID_STRING)
            {
                string idPerson = this.hdnRowMasterKey.Value;
                this.currentEntity = this.ownerPage.AdminClientRef.GetPersonByPersonID(idPerson);

                //създава и отваря ресурсна папка с име - idPerson_PersonName
                string folderName = this.hdnRowMasterKey.Value + "_" + BaseHelper.ConvertCyrToLatin(this.currentEntity.FirstName.Trim());

                string resourcesFolderName = GeneralPage.GetSettingByCode(ETEMEnums.AppSettings.ResourcesFolderName).SettingValue + @"\Person\";

                //ID с което започва папката за импорт. Пример C:\Resources_UMS_Person\198_Kiril
                string idStartFolder = folderName.Split('_')[0].ToString();

                DirectoryInfo folder = new DirectoryInfo(resourcesFolderName);// + folderName);

                //Винаги изтриваме целевата папка за да не се пълни с всяка следваща снимка
                if (folder.Exists)
                {
                    DirectoryInfo[] directories = folder.GetDirectories();

                    foreach (var file in directories)
                    {
                        if (file.Name.StartsWith(idStartFolder + "_"))
                        {
                            FileInfo[] filesToDelete = file.GetFiles();
                            foreach (var delFile in filesToDelete)
                            {
                                File.Delete(delFile.FullName);
                            }

                            //Directory.Delete(file.FullName);

                            break;
                        }
                    }
                }

                //и отново създаваме потребителската директория
                folder = new DirectoryInfo(resourcesFolderName + folderName);

                if (!folder.Exists)
                {
                    folder = Directory.CreateDirectory(resourcesFolderName + folderName);
                }


                //ако сме избрали нещо
                if (!string.IsNullOrEmpty(FileUpload1.FileName))
                {
                    //записваме картинката в папката
                    string pathToSave = (folder.FullName.EndsWith("\\") ? folder.FullName : folder.FullName + "\\") + FileUpload1.FileName;

                    FileUpload1.SaveAs(pathToSave);

                    //update Person
                    if (this.currentEntity != null)
                    {
                        this.currentEntity.ImagePath = GeneralPage.GetSettingByCode(ETEMEnums.AppSettings.WebResourcesFolderName).SettingValue + "/Person/" + folderName + "/" + FileUpload1.FileName;
                        CallContext resultPersontContext = new CallContext();
                        resultPersontContext.CurrentConsumerID = idPerson;
                        resultPersontContext = AdminClientRef.PersonSave(this.currentEntity, resultPersontContext);
                    }
                }

                this.CurrentEntityMasterID = idPerson;
            }

            this.pnlAddPersonImage.Visible = false;
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.pnlAddPersonImage.Visible = false;
        }

        protected void ddlLoadStudentInfoBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlLoadStudentInfoBy.SelectedValue == "Name")
            {
                if (this.PersonUsedFor == ETEMEnums.PersonTypeEnum.Student.ToString())
                {
                    this.ucTextForSearchStudent.CustomCase = "StudentAndCandidateByName";
                    this.ucTextForSearchStudent.AdditionalWhereParam = (this.IdAcademicPeriod != Constants.INVALID_ID ?
                                                                        "idAcademicPeriod=" + this.IdAcademicPeriod : string.Empty);
                }
                else if (this.PersonUsedFor == ETEMEnums.PersonTypeEnum.StudentCandidate.ToString())
                {
                    this.ucTextForSearchStudent.CustomCase = "PreparationANDStudentAndCandidateByName";
                }
                else if (this.PersonUsedFor == ETEMEnums.PersonTypeEnum.ArtModel.ToString())
                {
                    this.ucTextForSearchStudent.CustomCase = "PersonALLByName";
                }
                else if (this.PersonUsedFor == ETEMEnums.PersonTypeEnum.PhD.ToString())
                {
                    this.ucTextForSearchStudent.CustomCase = "PersonsAllTypes";
                }
                else if (this.PersonUsedFor == ETEMEnums.PersonTypeEnum.Lecturer.ToString())
                {
                    this.ucTextForSearchStudent.CustomCase = "PersonALLByName";
                }
                else
                {
                    this.ucTextForSearchStudent.CustomCase = "PersonALLByName";
                }

                this.ucTextForSearchStudent.UserControlLoad();
            }
            else if (this.ddlLoadStudentInfoBy.SelectedValue == "EGN")
            {
                if (this.PersonUsedFor == ETEMEnums.PersonTypeEnum.Student.ToString())
                {
                    this.ucTextForSearchStudent.CustomCase = "StudentAndCandidateByEGN";
                    this.ucTextForSearchStudent.AdditionalWhereParam = (this.IdAcademicPeriod != Constants.INVALID_ID ?
                                                                        "idAcademicPeriod=" + this.IdAcademicPeriod : string.Empty);
                }
                else if (this.PersonUsedFor == ETEMEnums.PersonTypeEnum.StudentCandidate.ToString())
                {
                    this.ucTextForSearchStudent.CustomCase = "PreparationANDStudentAndCandidateByEGN";
                }
                else if (this.PersonUsedFor == ETEMEnums.PersonTypeEnum.ArtModel.ToString())
                {
                    this.ucTextForSearchStudent.CustomCase = "PersonALLByEGN";
                }
                else if (this.PersonUsedFor == ETEMEnums.PersonTypeEnum.PhD.ToString())
                {
                    this.ucTextForSearchStudent.CustomCase = "PersonALLByEGN";
                }
                else if (this.PersonUsedFor == ETEMEnums.PersonTypeEnum.Lecturer.ToString())
                {
                    this.ucTextForSearchStudent.CustomCase = "PersonALLByEGN";
                }
                else
                {
                    this.ucTextForSearchStudent.CustomCase = "PersonALLByName";
                }
                this.ucTextForSearchStudent.UserControlLoad();
            }
            else if (this.ddlLoadStudentInfoBy.SelectedValue == "FacultyNo")
            {
                this.ucTextForSearchStudent.CustomCase = "StudentByFacultyNoCA";
                this.ucTextForSearchStudent.UserControlLoad();
            }
            else
            {
                return;
            }
        }

        protected void btnCancelPersonalErorrs_Click(object sender, EventArgs e)
        {
            this.blPersonalEroorsSave.Items.Clear();
            this.pnlPersonalErrors.Visible = false;
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            this.pnlDivSearchStudent.Visible = false;
        }

        protected void btnМакеHistory_Click(object sender, EventArgs e)
        {
            if (!this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.PersonSave, false))
            {
                return;
            }

            PersonHistory newPersonHistory          = new PersonHistory();
            this.currentEntity                      = this.ownerPage.AdminClientRef.GetPersonByPersonID(this.MasterKeyID);

            newPersonHistory.idPerson               = currentEntity.idPerson;
            newPersonHistory.FirstName              = currentEntity.FirstName;
            newPersonHistory.SecondName             = currentEntity.SecondName;
            newPersonHistory.LastName               = currentEntity.LastName;
            newPersonHistory.IdentityNumber         = currentEntity.IDCard;
            newPersonHistory.IdentityNumberDate     = currentEntity.IdentityNumberDate;
            newPersonHistory.IdentityNumberIssueBy  = currentEntity.IdentityNumberIssueBy;

            newPersonHistory.idCreateUser           = Int32.Parse(this.ownerPage.UserProps.IdUser);
            newPersonHistory.idModifyUser           = Int32.Parse(this.ownerPage.UserProps.IdUser);
            newPersonHistory.CreationDate           = DateTime.Now;
            newPersonHistory.ModifyDate             = DateTime.Now;


            this.ownerPage.CallContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;
            this.ownerPage.AdminClientRef.PersonHistorySave(newPersonHistory, this.ownerPage.CallContext);

            LoadPersonHistory();
        }


    }
}