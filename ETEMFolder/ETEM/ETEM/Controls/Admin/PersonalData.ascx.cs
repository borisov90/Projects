using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Models;
using ETEMModel.Helpers;
using System.IO;
using ETEM.Controls.Common;
using ETEMModel.Models.DataView;

namespace ETEM.Controls.Admin
{
    public partial class PersonalData : BaseUserControl
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

            this.tbxIdentityNumber.Text = string.Empty;

            this.tbxFirstName.Text = string.Empty;
            this.tbxSecondName.Text = string.Empty;
            this.tbxLastName.Text = string.Empty;

            this.tbxFirstNameEN.Text = string.Empty;
            this.tbxSecondNameEN.Text = string.Empty;
            this.tbxLastNameEN.Text = string.Empty;


            this.tbxPermanentAddressAddition.Text = string.Empty;
            this.tbxBirthDate.Text = string.Empty;
            this.tbxEMail.Text = string.Empty;
            this.tbxMobilePhone.Text = string.Empty;
            this.tbxPhone.Text = string.Empty;

            this.dllSex.SelectedValue = Constants.INVALID_ID_STRING;
            this.ddlCitizenship.SelectedValue = Constants.INVALID_ID_STRING;
            this.ddlSecondCitizenship.SelectedValue = Constants.INVALID_ID_STRING;

            this.ucAutoCompleteLocationPermanentAdress.SelectedValue = Constants.INVALID_ID_STRING;
            this.ucAutoCompleteLocationPermanentAdress.Text = string.Empty;

            this.ucAutoCompleteLocationCoreposndation.SelectedValue = Constants.INVALID_ID_STRING;
            this.ucAutoCompleteLocationCoreposndation.Text = string.Empty;

            this.ucAutoCompleteLocationPlaceOfBirth.SelectedValue = Constants.INVALID_ID_STRING;
            this.ucAutoCompleteLocationPlaceOfBirth.Text = string.Empty;



            ddlCitizenshipPlaceOfBirth.UserControlLoad();
            ddlCitizenshipPermanentAdress.UserControlLoad();
            ddlCitizenshipCorespondention.UserControlLoad();

            this.ddlMunicipalityCorespondation.SelectedValue = Constants.INVALID_ID_STRING;
            this.ddlMunicipalityPermanentAdress.SelectedValue = Constants.INVALID_ID_STRING;
            this.ddlMunicipalityPlaceOfBirth.SelectedValue = Constants.INVALID_ID_STRING;

            //this.ddlCitizenshipCorespondention.SelectedValue = Constants.INVALID_ID_STRING;
            //this.ddlCitizenshipPermanentAdress.SelectedValue = Constants.INVALID_ID_STRING;
            //this.ddlCitizenshipPlaceOfBirth.SelectedValue = Constants.INVALID_ID_STRING;



            this.ddlMunicipalityPlaceOfBirth.AdditionalParam = "idCountry=" + this.ddlCitizenshipPlaceOfBirth.SelectedValue;
            this.ddlMunicipalityPlaceOfBirth.UserControlLoad();

            this.ddlMunicipalityPermanentAdress.AdditionalParam = "idCountry=" + this.ddlCitizenshipPermanentAdress.SelectedValue;
            this.ddlMunicipalityPermanentAdress.UserControlLoad();

            this.ddlMunicipalityCorespondation.AdditionalParam = "idCountry=" + this.ddlCitizenshipCorespondention.SelectedValue;
            this.ddlMunicipalityCorespondation.UserControlLoad();


            this.tbxEGN.Text = string.Empty;

            this.tbxIDN.Text = string.Empty;

            this.tbxIdentityNumber.Text = string.Empty;

            this.tbxIdentityNumberDate.Text = string.Empty;

            this.tbxIdentityNumberIssueBy.Text = string.Empty;

            this.tbxForeignerIdentityNumber.Text = string.Empty;

            this.tbxTitle.Text = string.Empty;

            this.tbxIDCard.Text = string.Empty;

            this.tbxPermanentPostCode.Text = string.Empty;
            this.tbxCorespondationPostCode.Text = string.Empty;
            this.ddlMaritalStatus.SelectedValue = Constants.INVALID_ID_STRING;
            this.tbxCorespondationAddress.Text = string.Empty;
            this.hdnRowMasterKey.Value = string.Empty;
            this.imgBtnPerson.ImageUrl = string.Empty;


            this.tbxAccessCard1.Text = string.Empty;
            this.tbxAccessCard2.Text = string.Empty;
            this.ddlPosition.SelectedValue = Constants.INVALID_ID_STRING;

            if (this.PersonUsedFor == ETEMEnums.PersonTypeEnum.Student.ToString() ||
                    this.PersonUsedFor == ETEMEnums.PersonTypeEnum.PhD.ToString())
            {

                if (string.IsNullOrEmpty(this.tbxIDN.Text))
                {
                    this.divIDN.Visible = true;
                }
                else
                {
                    this.divIDN.Visible = false;
                }

            }
            else
            {
                this.divIDN.Visible = false;
            }
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
                this.pnlDivSearchStudent.Visible = false;

                this.tbxIdentityNumber.Text = currentEntity.IdentityNumber;

                this.tbxFirstName.Text = currentEntity.FirstName;
                this.tbxSecondName.Text = currentEntity.SecondName;
                this.tbxLastName.Text = currentEntity.LastName;

                this.tbxFirstNameEN.Text = currentEntity.FirstNameEN;
                this.tbxSecondNameEN.Text = currentEntity.SecondNameEN;
                this.tbxLastNameEN.Text = currentEntity.LastNameEN;


                this.dllSex.SelectedValue = currentEntity.idSex.ToString();
                this.ddlCitizenship.SelectedValue = currentEntity.idCitizenship.ToString();
                this.ddlSecondCitizenship.SelectedValue = currentEntity.idSecondCitizenship.ToString();
                this.tbxPermanentAddressAddition.Text = currentEntity.Address;
                this.tbxBirthDate.SetTxbDateTimeValue(currentEntity.BirthDate);
                //this.tbxBirthDate.Text = currentEntity.BirthDateStr;
                this.tbxEMail.Text = currentEntity.EMail;
                this.tbxMobilePhone.Text = currentEntity.MobilePhone;
                this.tbxPhone.Text = currentEntity.Phone;

                this.tbxEGN.Text = currentEntity.EGN;
                this.tbxForeignerIdentityNumber.Text = currentEntity.ForeignerIdentityNumber;
                this.tbxIDN.Text = currentEntity.IDN;

                if (this.PersonUsedFor == ETEMEnums.PersonTypeEnum.Student.ToString() ||
                    this.PersonUsedFor == ETEMEnums.PersonTypeEnum.PhD.ToString())
                {

                    if (string.IsNullOrEmpty(this.tbxIDN.Text))
                    {
                        this.divIDN.Visible = true;
                    }
                    else
                    {
                        this.divIDN.Visible = false;
                    }

                }
                else
                {
                    this.divIDN.Visible = false;
                }

                this.tbxIdentityNumber.Text = currentEntity.IdentityNumber;
                this.tbxIdentityNumberDate.SetTxbDateTimeValue(currentEntity.IdentityNumberDate);
                // this.tbxIdentityNumberDate.Text = currentEntity.IdentityNumberDateStr;
                this.tbxIdentityNumberIssueBy.Text = currentEntity.IdentityNumberIssueBy;
                this.tbxTitle.Text = currentEntity.Title;
                this.tbxIDCard.Text = currentEntity.IDCard;

                this.tbxPermanentPostCode.Text = currentEntity.PermanentPostCode;
                this.tbxCorespondationPostCode.Text = currentEntity.CorespondationPostCode;
                this.ddlMaritalStatus.SelectedValue = currentEntity.idMaritalstatus.ToString();
                this.tbxCorespondationAddress.Text = currentEntity.CorespondationAddress;

                this.tbxAccessCard1.Text = currentEntity.AccessCard1;
                this.tbxAccessCard2.Text = currentEntity.AccessCard2;
                this.ddlPosition.SelectedValue = currentEntity.idPosition.ToString();

                this.hdnRowMasterKey.Value = currentEntity.idPerson.ToString();
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

                if (this.currentEntity.idLocation.HasValue)
                {
                    var location = this.ownerPage.AdminClientRef.GetLocationById(this.currentEntity.idLocation.Value.ToString());

                    this.ucAutoCompleteLocationPermanentAdress.Text = (location != null ? location.LocationName : string.Empty);
                }

                if (this.currentEntity.idPlaceOfBirthCountry.HasValue)
                {
                    SelectEKATEsData(this.ddlCitizenshipPlaceOfBirth,
                         this.ddlMunicipalityPlaceOfBirth,
                        this.currentEntity.idPlaceOfBirthCountry.ToString(),
                        this.currentEntity.idPlaceOfBirthMunicipality,
                        this.ucAutoCompleteLocationPlaceOfBirth,
                        this.currentEntity.idPlaceOfBirthCity);
                }
                else
                {
                    SelectEKATEsData(this.ddlCitizenshipPlaceOfBirth,
                        this.ddlMunicipalityPlaceOfBirth,
                       Constants.INVALID_ID_STRING,
                       Constants.INVALID_ID_NULLABLE,
                       this.ucAutoCompleteLocationPlaceOfBirth,
                       Constants.INVALID_ID_NULLABLE);
                }

                if (this.currentEntity.idPermanentAdressCountry.HasValue)
                {
                    SelectEKATEsData(this.ddlCitizenshipPermanentAdress,
                         this.ddlMunicipalityPermanentAdress,
                        this.currentEntity.idPermanentAdressCountry.ToString(),
                        this.currentEntity.idPermanentAdressMunicipality,
                        this.ucAutoCompleteLocationPermanentAdress,
                        this.currentEntity.idPermanentAdressCity);
                }
                else
                {
                    SelectEKATEsData(this.ddlCitizenshipPermanentAdress,
                         this.ddlMunicipalityPermanentAdress,
                         Constants.INVALID_ID_STRING,
                         Constants.INVALID_ID_NULLABLE,
                         this.ucAutoCompleteLocationPermanentAdress,
                         Constants.INVALID_ID_NULLABLE);
                }

                if (this.currentEntity.idCorespondationCountry.HasValue)
                {
                    SelectEKATEsData(this.ddlCitizenshipCorespondention,
                         this.ddlMunicipalityCorespondation,
                        this.currentEntity.idCorespondationCountry.ToString(),
                        this.currentEntity.idCorespondationMunicipality,
                        this.ucAutoCompleteLocationCoreposndation,
                        this.currentEntity.idCorespondationCity);
                }
                else
                {
                    SelectEKATEsData(this.ddlCitizenshipCorespondention,
                         this.ddlMunicipalityCorespondation,
                        Constants.INVALID_ID_STRING,
                         Constants.INVALID_ID_NULLABLE,
                        this.ucAutoCompleteLocationCoreposndation,
                        Constants.INVALID_ID_NULLABLE);
                }

                this.PersonInfo = this.currentEntity.FullName;

                LoadPersonHistory();


            }
            else
            {
                LoadDdlLoadStudentInfoBy();

                //this.ucTextForSearchStudent.CustomCase = (this.PersonUsedFor == UMSEnums.PersonTypeEnum.StudentCandidate.ToString() ?
                //                                          "PersonAndCandidateByName" : "StudentAndCandidateByName");

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
                else
                {
                    this.ucTextForSearchStudent.CustomCase = "PersonALLByName";
                }

                this.ucTextForSearchStudent.Text = string.Empty;
                this.ucTextForSearchStudent.UserControlLoad();

                //this.pnlDivSearchStudent.Visible = true;

                SetEmptyValues();

                this.gvPersonHistory.DataSource = null;
                this.gvPersonHistory.DataBind();
            }
        }

        private void LoadPersonHistory()
        {
            List<PersonHistoryDataView> listPersonHistory = this.ownerPage.AdminClientRef.GetPersonHistoryDataViewByPersonID(Int32.Parse(this.MasterKeyID));
            this.gvPersonHistory.DataSource = listPersonHistory;
            this.gvPersonHistory.DataBind();
        }

        private void SelectEKATEsData(SMCDropDownList ddlCountry, SMCDropDownList ddlMun, string countryId, int? municipalityId, SMCAutoCompleteTextBox acCity, int? cityId)
        {
            ddlCountry.SelectedValue = countryId;
            ddlMun.AdditionalParam = "idCountry=" + countryId;
            ddlMun.UserControlLoad();
            if (municipalityId.HasValue && municipalityId != Constants.INVALID_ID_NULLABLE)
            {
                ddlMun.SelectedValue = municipalityId.Value.ToString();
                acCity.AdditionalWhereParam = "idMunicipality=" + municipalityId.Value.ToString();
                acCity.UserControlLoad();

                if (cityId.HasValue && cityId != Constants.INVALID_ID_NULLABLE)
                {
                    var location = this.ownerPage.AdminClientRef.GetLocationById(cityId.Value.ToString());
                    acCity.SelectedValue = cityId.ToString();
                    acCity.Text = (location != null ? location.LocationName : string.Empty);
                }
            }

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
                    this.lbResultContext.Text = String.Format(BaseHelper.GetCaptionString("Entity_Person_Not_Found_By_ID"), this.hdnRowMasterKey.Value);

                    string falseResult = String.Format(BaseHelper.GetCaptionString("Entity_is_not_update_in_tab"),
                                                       BaseHelper.GetCaptionString("Personal_Data"));

                    this.ownerPage.CallContext.ResultCode = ETEMEnums.ResultEnum.Error;
                    this.ownerPage.CallContext.Message = falseResult;

                    return new Tuple<CallContext, string>(this.ownerPage.CallContext, BaseHelper.GetCaptionString("Personal_Data"));
                }

                isNewEntity = false;
            }

            currentEntity.IdentityNumber = this.tbxIdentityNumber.Text;

            currentEntity.FirstName = this.tbxFirstName.Text;
            currentEntity.SecondName = this.tbxSecondName.Text;
            currentEntity.LastName = this.tbxLastName.Text;

            currentEntity.FirstNameEN = this.tbxFirstNameEN.Text;
            currentEntity.SecondNameEN = this.tbxSecondNameEN.Text;
            currentEntity.LastNameEN = this.tbxLastNameEN.Text;


            currentEntity.idSex = this.dllSex.SelectedValueINT;
            currentEntity.idCitizenship = this.ddlCitizenship.SelectedValueINT;
            currentEntity.idSecondCitizenship = this.ddlSecondCitizenship.SelectedValueINT;

            currentEntity.idPermanentAdressCountry = this.ddlCitizenshipPermanentAdress.SelectedValueINT;
            currentEntity.idPermanentAdressMunicipality = this.ddlMunicipalityPermanentAdress.SelectedValueINT;
            currentEntity.idPermanentAdressCity = this.ucAutoCompleteLocationPermanentAdress.SelectedValueINT;
            //currentEntity.idLocation = ;

            currentEntity.idPlaceOfBirthCountry = this.ddlCitizenshipPlaceOfBirth.SelectedValueINT;
            currentEntity.idPlaceOfBirthMunicipality = this.ddlMunicipalityPlaceOfBirth.SelectedValueINT;
            currentEntity.idPlaceOfBirthCity = this.ucAutoCompleteLocationPlaceOfBirth.SelectedValueINT;

            currentEntity.idCorespondationCountry = (this.ddlCitizenshipCorespondention.SelectedValueINT != Constants.INVALID_ID) ? this.ddlCitizenshipCorespondention.SelectedValueINT : this.ddlCitizenshipPermanentAdress.SelectedValueINT;
            currentEntity.idCorespondationMunicipality = (this.ddlMunicipalityCorespondation.SelectedValueINT != Constants.INVALID_ID) ? this.ddlMunicipalityCorespondation.SelectedValueINT : this.ddlMunicipalityPermanentAdress.SelectedValueINT;
            currentEntity.idCorespondationCity = (this.ucAutoCompleteLocationCoreposndation.SelectedValueINT != null) ? this.ucAutoCompleteLocationCoreposndation.SelectedValueINT : this.ucAutoCompleteLocationPermanentAdress.SelectedValueINT;

            if (this.currentEntity.idCorespondationCountry.HasValue)
            {
                SelectEKATEsData(this.ddlCitizenshipCorespondention,
                     this.ddlMunicipalityCorespondation,
                    this.currentEntity.idCorespondationCountry.ToString(),
                    this.currentEntity.idCorespondationMunicipality,
                    this.ucAutoCompleteLocationCoreposndation,
                    this.currentEntity.idCorespondationCity);
            }

            currentEntity.Address = this.tbxPermanentAddressAddition.Text;
            currentEntity.CorespondationAddress = (!string.IsNullOrEmpty(this.tbxCorespondationAddress.Text)) ? this.tbxCorespondationAddress.Text : this.tbxPermanentAddressAddition.Text;

            this.tbxCorespondationAddress.Text = currentEntity.CorespondationAddress;

            DateTime? tmpBirthDate = null;

            if (this.tbxBirthDate.TextAsDateParseExact.HasValue)
            {
                tmpBirthDate = this.tbxBirthDate.TextAsDateParseExact;
            }
            else if (!string.IsNullOrEmpty(this.tbxEGN.Text))
            {
                try
                {
                    string firstPartEGN = this.tbxEGN.Text.Substring(0, 6);
                    int year = int.Parse(firstPartEGN.Substring(0, 2));
                    int month = int.Parse(firstPartEGN.Substring(2, 2));
                    int day = int.Parse(firstPartEGN.Substring(4, 2));

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

            currentEntity.BirthDate = tmpBirthDate;
            currentEntity.EMail = this.tbxEMail.Text;
            currentEntity.MobilePhone = this.tbxMobilePhone.Text;
            currentEntity.Phone = this.tbxPhone.Text;
            currentEntity.EGN = this.tbxEGN.Text;
            currentEntity.ForeignerIdentityNumber = this.tbxForeignerIdentityNumber.Text;
            currentEntity.IDN = this.tbxIDN.Text;
            currentEntity.IdentityNumber = this.tbxIdentityNumber.Text;
            currentEntity.IdentityNumberDate = this.tbxIdentityNumberDate.TextAsDateParseExact;
            currentEntity.IdentityNumberIssueBy = this.tbxIdentityNumberIssueBy.Text;
            currentEntity.Title = this.tbxTitle.Text;
            currentEntity.IDCard = this.tbxIDCard.Text;


            currentEntity.AccessCard1 = this.tbxAccessCard1.Text;
            currentEntity.AccessCard2 = this.tbxAccessCard2.Text;
            currentEntity.idPosition = this.ddlPosition.SelectedValueINT;

            currentEntity.PermanentPostCode = this.tbxPermanentPostCode.Text;
            currentEntity.CorespondationPostCode = (!string.IsNullOrEmpty(this.tbxCorespondationPostCode.Text)) ? this.tbxCorespondationPostCode.Text : this.tbxPermanentPostCode.Text;

            this.tbxCorespondationPostCode.Text = currentEntity.CorespondationPostCode;

            currentEntity.idMaritalstatus = this.ddlMaritalStatus.SelectedValueINT;


            

            this.ownerPage.CallContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;

            this.ownerPage.CallContext = this.ownerPage.AdminClientRef.PersonSave(currentEntity, this.ownerPage.CallContext);

            if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {
                this.lbResultContext.Text = this.ownerPage.CallContext.Message;

                this.hdnRowMasterKey.Value = this.ownerPage.CallContext.EntityID;

                CheckIfResultIsSuccess(lbResultContext);

                this.PersonInfo = this.currentEntity.FullName;
                if (!string.IsNullOrEmpty(PersonUsedFor))
                {
                    
                    if (PersonUsedFor == ETEMEnums.PersonTypeEnum.Employe.ToString())
                    {
                        if (isNewEntity)
                        {
                            Employe newEmploye = new Employe();
                            newEmploye.idPerson = Convert.ToInt32(this.ownerPage.CallContext.EntityID);
                            CallContext callContext = new CallContext();
                            callContext = this.ownerPage.EmployeClientRef.EmployeSave(newEmploye, callContext);

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

            if (this.PersonUsedFor == ETEMEnums.PersonTypeEnum.Student.ToString() ||
                    this.PersonUsedFor == ETEMEnums.PersonTypeEnum.PhD.ToString())
            {

                if (string.IsNullOrEmpty(this.tbxIDN.Text))
                {
                    this.divIDN.Visible = true;
                }
                else
                {
                    this.divIDN.Visible = false;
                }

            }
            else
            {
                this.divIDN.Visible = false;
            }

            

            return new Tuple<CallContext, string>(this.ownerPage.CallContext, BaseHelper.GetCaptionString("Personal_Data"));
        }

        private void loadInitControls()
        {
            this.dllSex.UserControlLoad();
            this.ddlCitizenship.UserControlLoad();
            this.ddlSecondCitizenship.UserControlLoad();
            this.ddlCitizenshipPermanentAdress.UserControlLoad();
            this.ddlCitizenshipPlaceOfBirth.UserControlLoad();
            this.ddlCitizenshipCorespondention.UserControlLoad();
            this.ddlMaritalStatus.UserControlLoad();
            this.ddlPosition.UserControlLoad();

            this.idDIVAdministrativeInformation.Visible = false;


            if (!string.IsNullOrEmpty(this.PersonUsedFor) && (PersonUsedFor == ETEMEnums.PersonTypeEnum.Employe.ToString() || PersonUsedFor == ETEMEnums.PersonTypeEnum.Lecturer.ToString()))
            {
                this.idDIVAdministrativeInformation.Visible = true;
            }

            //показва полето 'Титла' само за  преподаватели
            if (!string.IsNullOrEmpty(this.PersonUsedFor) && PersonUsedFor == Constants.PERSON_LECTURER)
            {
                this.divTitle.Visible = true;
                this.divBR.Visible = false;
            }
            else
            {
                this.divTitle.Visible = false;
                this.divBR.Visible = true;
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
            //if (!string.IsNullOrEmpty(this.hdnRowMasterKey.Value) && this.hdnRowMasterKey.Value != Constants.INVALID_ID_STRING)
            //{
            //    this.currentEntity = this.ownerPage.AdminClientRef.GetPersonByPersonID(this.hdnRowMasterKey.Value);

            //    //създава и отваря ресурсна папка с име - PersonName_idPerson
            //    string folderName = BaseHelper.ConvertCyrToLatin(this.currentEntity.FirstName) + "_" + this.hdnRowMasterKey.Value;//this.ownerPage.UserProps.IdUser;

            //    DirectoryInfo folder = new DirectoryInfo(@"C:\Resources\Person\" + folderName);


            //    if (!folder.Exists)
            //    {
            //        folder = Directory.CreateDirectory(@"C:\Resources\Person\" + folderName);
            //    }

            //    string path = "UploadPath=" + folder.FullName + "&ResourceName=" + this.currentEntity.FirstName + "&idResource=" + this.hdnRowMasterKey.Value;

            //    this.OpenPageInNewWindow(ETEM.Share.MultiFilesUpload.formResource, path);
            //}
            this.pnlAddPersonImage.Visible = true;
        }

        protected void ddlCitizenshipPlaceOfBirth_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlMunicipalityPlaceOfBirth.AdditionalParam = "idCountry=" + this.ddlCitizenshipPlaceOfBirth.SelectedValue;
            this.ddlMunicipalityPlaceOfBirth.UserControlLoad();
        }

        protected void ddlCitizenshipPermanentAdress_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlMunicipalityPermanentAdress.AdditionalParam = "idCountry=" + this.ddlCitizenshipPermanentAdress.SelectedValue;
            this.ddlMunicipalityPermanentAdress.UserControlLoad();
        }

        protected void ddlCitizenshipCorespondention_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlMunicipalityCorespondation.AdditionalParam = "idCountry=" + this.ddlCitizenshipCorespondention.SelectedValue;
            this.ddlMunicipalityCorespondation.UserControlLoad();
        }

        protected void ddlMunicipalityPlaceOfBirth_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ucAutoCompleteLocationPlaceOfBirth.AdditionalWhereParam = "idMunicipality=" + this.ddlMunicipalityPlaceOfBirth.SelectedValue;
            this.ucAutoCompleteLocationPlaceOfBirth.UserControlLoad();
        }

        protected void ddlMunicipalityPermanentAdress_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ucAutoCompleteLocationPermanentAdress.AdditionalWhereParam = "idMunicipality=" + this.ddlMunicipalityPermanentAdress.SelectedValue;
            this.ucAutoCompleteLocationPermanentAdress.UserControlLoad();
        }

        protected void ddlMunicipalityCorespondation_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ucAutoCompleteLocationCoreposndation.AdditionalWhereParam = "idMunicipality=" + this.ddlMunicipalityCorespondation.SelectedValue;
            this.ucAutoCompleteLocationCoreposndation.UserControlLoad();
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

            LoadMainControl();

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

        protected void LoadMainControl()
        {
            

            
        }

        protected void btnМакеHistory_Click(object sender, EventArgs e)
        {
            if (!this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.PersonSave, false))
            {
                return;
            }

            PersonHistory newPersonHistory = new PersonHistory();
            this.currentEntity = this.ownerPage.AdminClientRef.GetPersonByPersonID(this.MasterKeyID);

            newPersonHistory.idPerson = currentEntity.idPerson;
            newPersonHistory.FirstName = currentEntity.FirstName;
            newPersonHistory.SecondName = currentEntity.SecondName;
            newPersonHistory.LastName = currentEntity.LastName;
            newPersonHistory.IdentityNumber = currentEntity.IDCard;
            newPersonHistory.IdentityNumberDate = currentEntity.IdentityNumberDate;
            newPersonHistory.IdentityNumberIssueBy = currentEntity.IdentityNumberIssueBy;

            newPersonHistory.idCreateUser = Int32.Parse(this.ownerPage.UserProps.IdUser);
            newPersonHistory.idModifyUser = Int32.Parse(this.ownerPage.UserProps.IdUser);
            newPersonHistory.CreationDate = DateTime.Now;
            newPersonHistory.ModifyDate = DateTime.Now;


            this.ownerPage.CallContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;
            this.ownerPage.AdminClientRef.PersonHistorySave(newPersonHistory, this.ownerPage.CallContext);

            LoadPersonHistory();
        }

      

    }
}