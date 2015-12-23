using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Linq.Expressions;
using ETEMModel.Helpers;

namespace ETEMModel.Models.DataView
{
    public class PersonDataView : Person, DataViewInterface
    {
        public PersonDataView()
        {

        }

        public string IdentityNumberTypeStr { get; set; }
        public string NationalityStr { get; set; }

        public int idStreamGroupDetail { get; set; }
        public int idExamProtocol { get; set; }
        public int idPreparationCourse { get; set; }
        public int idLecturer { get; set; }
        public int idArtModel { get; set; }
        public int idStudentCandidateSpeciality { get; set; }

        public string FullNamePlusEgn
        {
            get
            {
                return (string.IsNullOrEmpty(this.FirstName) ? string.Empty : " " + this.FirstName.Trim()) +
                       (string.IsNullOrEmpty(this.SecondName) ? string.Empty : " " + this.SecondName.Trim()) +
                       (string.IsNullOrEmpty(this.LastName) ? string.Empty : " " + this.LastName.Trim()) +
                        (string.IsNullOrEmpty(this.EGN) ? string.Empty : " " + this.EGN.Trim()) +
                       (string.IsNullOrEmpty(this.IDN) ? string.Empty : " " + this.IDN.Trim()) +
                       (string.IsNullOrEmpty(this.IdentityNumber) ? string.Empty : " " + this.IdentityNumber.Trim());
            }
        }

        public string TitlePlusFullName
        {
            get
            {
                return (string.IsNullOrEmpty(this.Title) ? string.Empty : this.Title.Trim()) +
                       (string.IsNullOrEmpty(this.FirstName) ? string.Empty : " " + this.FirstName.Trim()) +
                       (string.IsNullOrEmpty(this.SecondName) ? string.Empty : " " + this.SecondName.Trim()) +
                       (string.IsNullOrEmpty(this.LastName) ? string.Empty : " " + this.LastName.Trim());
            }
        }

        public string FullNamePlusTitle
        {
            get
            {
                return (string.IsNullOrEmpty(this.FirstName) ? string.Empty : this.FirstName.Trim()) +
                       (string.IsNullOrEmpty(this.SecondName) ? string.Empty : " " + this.SecondName.Trim()) +
                       (string.IsNullOrEmpty(this.LastName) ? string.Empty : " " + this.LastName.Trim()) +
                       (string.IsNullOrEmpty(this.Title) ? string.Empty : " " + this.Title.Trim());
            }
        }

        public string TitlePlusTwoNames
        {
            get
            {
                return (string.IsNullOrEmpty(this.Title) ? string.Empty : this.Title.Trim()) +
                       (string.IsNullOrEmpty(this.FirstName) ? string.Empty : " " + this.FirstName.Trim()) +
                       (string.IsNullOrEmpty(this.LastName) ? string.Empty : " " + this.LastName.Trim());
            }
        }

        public string AdditionalInfo { get; set; }
        public string Source { get; set; }
        public string FacultyNo { get; set; }

        public string AutoCompleteParam { get; set; }


        /// <summary>
        /// Данни необходими при генериренто на акаунти
        /// </summary>
        public bool IsStudent { get; set; }
        public bool IsLecturer { get; set; }
        public bool IsPhD { get; set; }
        public bool IsEmployee { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }



        public string MasterProgram { get; set; }


        #region DataViewInterface Members

        public string IdEntity
        {
            get { return this.idPerson.ToString(); }
        }


        string DataViewInterface.IdEntity
        {
            get { return this.idPerson.ToString(); }
            set
            {
                this.idPerson = Int32.Parse(value);
            }
        }

        #endregion

        public int idExamCalendar { get; set; }




        #region Export props
        /// array 1
        public string SexCode { get; set; }
        public string IdentifyFlag { get; set; }
        public string CitizienShipCountryCode { get; set; }
        public string PlaceOfBirthLocationCode { get; set; }

        //array 2
        public string ScDegree { get; set; }
        public string AcadRank { get; set; }
        public string ScArea { get; set; }
        public string DropInOrder { get; set; }







        public string PermanentAdressCountryCode { get; set; }

        public string PermanentLocationCountryCode { get; set; }

        public int? idStudent { get; set; }



        public int? idSpeciality { get; set; }

        public int? idSpecialitySecondSpec { get; set; }

        public int? idCourse { get; set; }

        public int? idCourseSecondSpec { get; set; }

        public bool IsForeigner { get; set; }

        public bool DuplicatePerson { get; set; }

        public int? FacultyCode { get; set; }

        public int? AffiliateCode { get; set; }

        public string NumberSpec { get; set; }

        public string SpecCode { get; set; }

        public string EdFormCode { get; set; }

        public string EdDurationFristSpecCode { get; set; }

        public string StudyTypeCode { get; set; }

        public string MasterTypeCode { get; set; }

        public int IncommingYear { get; set; }

        public int? IncommingYearSecondSpec { get; set; }

        public string ReasonForAccCode { get; set; }

        public bool GetScolarShip { get; set; }

        public DateTime? ModifyStudentStatusDate { get; set; }

        public string DropInOrderDayStr
        {
            get
            {
                if (DropInOrderDay != null)
                {
                    return DropInOrderDay.Value.ToString(Constants.DATE_FORMAT_MMDDYYY);
                }
                else
                {
                    return null;
                }
            }
        }

        public string ActualModifyDate
        {
            get
            {
                //string shordDateTimeModify = ModifyStudentStatusDate.ToString(Constants.SHORT_AMERICAN_DATE_PATTERN_SLASHES);
                //string shordDateTimeCreate = CreationStudentStatusDate.ToString(Constants.SHORT_AMERICAN_DATE_PATTERN_SLASHES);

                //if (shordDateTimeModify != shordDateTimeCreate)
                //{
                //    return shordDateTimeModify;
                //}
                //else
                //{
                //    return "0";
                //}

                if (ModifyStudentStatusDate != null)
                {
                    return ModifyStudentStatusDate.Value.ToString(Constants.SHORT_AMERICAN_DATE_PATTERN_SLASHES);
                }

                else
                {
                    return Constants.INVALID_ID_ZERO_STRING;
                }

            }
        }



        public string StudentStatusCode { get; set; }

        public int MoveToAnotherUniCode { get; set; }

        public string StudentStatusChangeComment { get; set; }

        public bool IsInHostel { get; set; }

        public bool UseCampus { get; set; }

        public bool IsInStdsProgrammes { get; set; }

        public DateTime? PhdSignInDate { get; set; }
        public DateTime? PhdFinishDate { get; set; }

        public string PhdSignInDateText
        {
            get
            {
                if (PhdSignInDate != null)
                {
                    string phdSignInDateTxt = PhdSignInDate.Value.ToString(Constants.SHORT_AMERICAN_DATE_PATTERN_SLASHES);
                    return phdSignInDateTxt;
                }
                else
                {
                    return "0";
                }
            }
        }
        public string PhdFinishDateText
        {
            get
            {
                if (PhdFinishDate != null)
                {
                    string phdFinishDateTxt = PhdFinishDate.Value.ToString(Constants.SHORT_AMERICAN_DATE_PATTERN_SLASHES);
                    return phdFinishDateTxt;
                }
                else
                {
                    return "0";
                }
            }
        }



        public string PhdDurationNotes { get; set; }

        public string PrevEducationOKCCode { get; set; }

        public int? YearSE { get; set; }

        public string SchoolFromNameSE { get; set; }

        public string SchoolLocationSECode { get; set; }

        public string SchoolCountrySECode { get; set; }

        public string ProfessionNameSE { get; set; }

        public string SEUnicode { get; set; }

        public string SEProfGroup { get; set; }

        public int NewSpecSameOKS { get; set; }

        public DateTime? SEDiplomaDate { get; set; }

        public string SEDiplomaDateText
        {
            get
            {
                if (SEDiplomaDate != null)
                {
                    string seDiplomaDateText = SEDiplomaDate.Value.ToString(Constants.SHORT_AMERICAN_DATE_PATTERN_SLASHES);
                    return seDiplomaDateText;
                }
                else
                {
                    return "0";
                }
            }
        }



        public string SEDiplomaNumber { get; set; }



        public PersonDataView ShallowCopy()
        {
            return (PersonDataView)this.MemberwiseClone();
        }


        public string CourseCode { get; set; }

        public bool IsDuplicate { get; set; }

        public string EdDurationSecondSpecCode { get; set; }

        public string GetScolarShipCode { get; set; }

        public string SecondCitizienShipCountryCode { get; set; }

        public string FacultyName { get; set; }

        public string SpecialityName { get; set; }

        public string OKCCode { get; set; }

        public int? ProfGroupId { get; set; }





        public string NSICODE { get; set; }

        public string SecondSpecCode { get; set; }

        public string SecondSpecName { get; set; }

        public string ProfessionalQualification { get; set; }

        public string DiplomaProtocolNumber { get; set; }

        public DateTime? DiplomaProtocolDate { get; set; }

        public string DiplomaProtocolDateStr
        {
            get
            {
                return this.DiplomaProtocolDate != null ? this.DiplomaProtocolDate.Value.ToString(Constants.SHORT_BG_DATE_PATTERN_SLASHES) : Constants.EXPORT_MISSING_MANDATORY_FIELD;
            }
        }

        public string StikerYear { get; set; }

        public string SEDiplomaProtocolNumber { get; set; }

        public DateTime? SEDiplomaProtocolDate { get; set; }

        public string SEDiplomaProtocolDateStr
        {
            get
            {
                return this.SEDiplomaProtocolDate != null ? this.SEDiplomaProtocolDate.Value.ToString(Constants.SHORT_BG_DATE_PATTERN_SLASHES) : "";
            }
        }

        public string OriginalIdentifyNumber { get; set; }

        public string OriginalRegistryNumber { get; set; }

        public DateTime? OriginalReleaseDate { get; set; }

        public int? idOKC { get; set; }

        public string OriginalReleaseDateStr
        {
            get
            {
                return this.OriginalReleaseDate != null ? this.OriginalReleaseDate.Value.ToString(Constants.SHORT_BG_DATE_PATTERN_SLASHES) : "";
            }
        }

        public bool? NeedAdditionalChange { get; set; }

        public int idPhd { get; set; }

        public string PlaceOfBirthCountry { get; set; }

        public int idStatus { get; set; }

        public int idDiplomaInfo { get; set; }


        public string DocumentCode { get; set; }

        public string DiplomaSeriaNumber { get; set; }

        public string DiplomaRegistryNumber { get; set; }

        public DateTime? DiplomaRegistryDate { get; set; }

        public string DiplomaRegistryDateStr
        {
            get
            {
                return this.DiplomaRegistryDate != null ? this.DiplomaRegistryDate.Value.ToString(Constants.SHORT_BG_DATE_PATTERN_SLASHES) : "";
            }
        }

        public int CountDiplomaImages { get; set; }


        #endregion





        public string NameAfterDuplicatesCheck { get; set; }

        public int ImageNumber { get; set; }

        public string DiplimaName { get; set; }

        public string CourseText { get; set; }

        public string OKC { get; set; }

        public int idAcademicDegree { get; set; }

        public int? idAcadRank { get; set; }

        public int idMainContract { get; set; }

        public DateTime? DropInOrderDay { get; set; }

        public int idExamProtocolPhd { get; set; }

        public bool IsItPaid { get; set; }
    }
}
