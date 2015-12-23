using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Helpers;

namespace ETEMModel.Models
{
    public partial class Person : Identifiable
    {
        #region Identifiable Members

        public int EntityID
        {
            get { return this.idPerson; }
        }

        public string ValidationErrorsAsString { get; set; }
        public List<string> ValidateEntity(CallContext outputContext)
        {
            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;

            ETEMDataModelEntities dbContext = new ETEMDataModelEntities();

            ValidationErrorsAsString = string.Empty;
            List<string> result = new List<string>();

            if (string.IsNullOrEmpty(this.FirstName))
            {
                result.Add(String.Format(BaseHelper.GetCaptionString("Entity_Common_Field_Mandatory"), BaseHelper.GetCaptionString("Entity_Person_FirstName")));
            }

            if (string.IsNullOrEmpty(this.LastName))
            {
                result.Add(String.Format(BaseHelper.GetCaptionString("Entity_Common_Field_Mandatory"), BaseHelper.GetCaptionString("Entity_Person_LastName")));
            }

            //if (this.idCitizenship==Constants.INVALID_ID)
            //{
            //    result.Add(String.Format(BaseHelper.GetCaptionString("Entity_Common_Field_Mandatory"), BaseHelper.GetCaptionString("Entity_Person_Citizenship")));
            //}

            /*
            if (this.BirthDate == null)
            {
                result.Add(String.Format(BaseHelper.GetCaptionString("Entity_Common_Field_Mandatory"), BaseHelper.GetCaptionString("Entity_BirthYear")));
            }

            if (!this.idSex.HasValue || this.idSex.Value == Constants.INVALID_ID || this.idSex.Value == Constants.INVALID_ID_ZERO)
            {
                result.Add(String.Format(BaseHelper.GetCaptionString("Entity_Common_Field_Mandatory"), BaseHelper.GetCaptionString("Entity_Person_Sex")));
            }

            if (string.IsNullOrEmpty(this.EGN) && string.IsNullOrEmpty(this.IdentityNumber) && string.IsNullOrEmpty(this.IDN))
            {
                result.Add(BaseHelper.GetCaptionString("Entity_Person_Identifiers"));
            }
            */

            if (!string.IsNullOrEmpty(this.EGN))
            {
                var existUser = dbContext.Persons.Where(u => u.EGN == this.EGN &&
                                                        u.FirstName != this.FirstName &&
                                                        u.LastName != this.LastName &&
                                                        u.idPerson != this.idPerson).FirstOrDefault();

                if (existUser != null)
                {
                    result.Add(BaseHelper.GetCaptionString("Entity_Person_IdentityNumber_Exist"));
                }
            }
            else if (!string.IsNullOrEmpty(this.IdentityNumber))
            {
                var existUser = dbContext.Persons.Where(u => u.IdentityNumber == this.IdentityNumber &&
                                                        u.FirstName != this.FirstName &&
                                                        u.LastName != this.LastName &&
                                                        u.idPerson != this.idPerson).FirstOrDefault();

                if (existUser != null)
                {
                    result.Add(BaseHelper.GetCaptionString("Entity_Person_IdentityNumber_Exist"));
                }
            }

            ValidationErrorsAsString = string.Join(",", result.ToArray());

            if (!string.IsNullOrEmpty(ValidationErrorsAsString))
            {
                outputContext.ResultCode = ETEMEnums.ResultEnum.Error;
            }
            

            outputContext.Message = this.ValidationErrorsAsString;
            outputContext.EntityID = this.EntityID.ToString();

            return result;
        }

        #endregion

        public string FirstNameTrimed
        {
            get { return this.FirstName.Trim(); }
        }

        public string SecondNameTrimed
        {
            get { return this.SecondName.Trim(); }
        }

        public string LastNameTrimed
        {
            get { return this.LastName.Trim(); }
        }

        public string FullName
        {
            get
            {
                return
                        (string.IsNullOrEmpty(this.FirstName) ? "" : this.FirstName.Trim()) +
                        (string.IsNullOrEmpty(this.SecondName) ? "" : " " + this.SecondName.Trim()) +
                        (string.IsNullOrEmpty(this.LastName) ? "" : " " + this.LastName.Trim());
            }
        }

        public string FullNameEN
        {
            get
            {
                return
                        (string.IsNullOrEmpty(this.FirstNameEN) ? "" : this.FirstNameEN.Trim()) +
                        (string.IsNullOrEmpty(this.SecondNameEN) ? "" : " " + this.SecondNameEN.Trim()) +
                        (string.IsNullOrEmpty(this.LastNameEN) ? "" : " " + this.LastNameEN.Trim());
            }
        }

        public string FullNameTwo
        {
            get
            {
                return (string.IsNullOrEmpty(this.FirstName) ? "" : this.FirstName.Trim()) +
                       (string.IsNullOrEmpty(this.LastName) ? "" : " " + this.LastName.Trim());
            }
        }

        public string FullNameShortFirst
        {
            get
            {
                return (string.IsNullOrEmpty(this.FirstName) ? "" : this.FirstName.Trim().Substring(0, 1) + ".") +
                       (string.IsNullOrEmpty(this.LastName) ? "" : " " + this.LastName.Trim());
            }
        }

        public string FullNameLastTwo
        {
            get
            {
                return (string.IsNullOrEmpty(this.SecondName) ? "" : this.SecondName.Trim()) +
                       (string.IsNullOrEmpty(this.LastName) ? "" : " " + this.LastName.Trim());
            }
        }

        public string FullNameTwoEN
        {
            get
            {
                return (string.IsNullOrEmpty(this.FirstNameEN) ? "" : this.FirstNameEN.Trim()) +
                       (string.IsNullOrEmpty(this.LastNameEN) ? "" : " " + this.LastNameEN.Trim());
            }
        }

        public string FullNamePlusTitle
        {
            get
            {
                return (string.IsNullOrEmpty(this.Title) ? "" : this.Title.Trim() + " ") +
                       (string.IsNullOrEmpty(this.FirstName) ? "" : this.FirstName.Trim()) +
                       (string.IsNullOrEmpty(this.SecondName) ? "" : " " + this.SecondName.Trim()) +
                       (string.IsNullOrEmpty(this.LastName) ? "" : " " + this.LastName.Trim());
            }
        }

        public string FullNamePlusTitleEN
        {
            get
            {
                return (string.IsNullOrEmpty(this.Title) ? "" : this.Title.Trim() + " ") +
                       (string.IsNullOrEmpty(this.FirstNameEN) ? "" : this.FirstNameEN.Trim()) +
                       (string.IsNullOrEmpty(this.SecondNameEN) ? "" : " " + this.SecondNameEN.Trim()) +
                       (string.IsNullOrEmpty(this.LastNameEN) ? "" : " " + this.LastNameEN.Trim());
            }
        }

        public string TwoNamesPlusTitleEN
        {
            get
            {
                return ("prof. ") +
                       (string.IsNullOrEmpty(this.FirstNameEN) ? "" : this.FirstNameEN.Trim()) +

                       (string.IsNullOrEmpty(this.LastNameEN) ? "" : " " + this.LastNameEN.Trim());
            }
        }

        public string TwoNamesPlusTitle
        {
            get
            {
                return (string.IsNullOrEmpty(this.Title) ? "" : this.Title.Trim() + " ") +
                       (string.IsNullOrEmpty(this.FirstName) ? "" : this.FirstName.Trim()) +
                       (string.IsNullOrEmpty(this.LastName) ? "" : " " + this.LastName.Trim());
            }
        }


        public string TwoNamesPlusTitleShort
        {
            get
            {
                string firstName = "";
                if (!string.IsNullOrEmpty(this.FirstName))
                {
                    if (this.FirstName.Trim().Length >= 2)
                    {
                        //not to make Пе. but П.
                        if (Constants.Bulgarian_Vowels.Any(s => s == this.FirstName.Trim()[1]))
                        {
                            firstName = this.FirstName.Trim()[0].ToString() + ".";
                        }
                        else
                        {
                            firstName = this.FirstName.Trim().Substring(0, 2) + ".";
                        }
                    }

                }

                return (string.IsNullOrEmpty(this.Title) ? "" : this.Title.Trim() + " ") +
                       (string.IsNullOrEmpty(this.FirstName) ? "" : firstName) +
                       (string.IsNullOrEmpty(this.LastName) ? "" : " " + this.LastName.Trim());
            }
        }

        public string StrigifiedFullNamePlusTitle
        {
            get
            {
                return (string.IsNullOrEmpty(this.Title) ? "" : BaseHelper.ConvertCyrToLatin(this.Title.Trim()) + "_") +
                       (string.IsNullOrEmpty(this.FirstName) ? "" : BaseHelper.ConvertCyrToLatin(this.FirstName.Trim())) +
                       (string.IsNullOrEmpty(this.SecondName) ? "" : "_" + BaseHelper.ConvertCyrToLatin(this.SecondName.Trim())) +
                       (string.IsNullOrEmpty(this.LastName) ? "" : "_" + BaseHelper.ConvertCyrToLatin(this.LastName.Trim()));
            }
        }

        public string BirthDateStr
        {
            get
            {
                if (this.BirthDate.HasValue)
                {
                    return this.BirthDate.Value.ToString("dd.MM.yyyy");
                }
                else
                {
                    return string.Empty;
                }
            }
        }


        public string BirthDateStrEn
        {
            get
            {
                if (this.BirthDate.HasValue)
                {
                    return this.BirthDate.Value.ToString(Constants.DATE_PATTERN_MONTH_AS_WORD, Constants.CULTURE_INFO_EN);
                }
                else
                {
                    return string.Empty;
                }
            }
        }


        public string IdentityNumberDateStr
        {
            get
            {
                if (this.IdentityNumberDate.HasValue)
                {
                    return this.IdentityNumberDate.Value.ToString("dd.MM.yyyy");
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string BirthDayDiplomaFormat
        {
            get
            {
                return this.BirthDate != null ? BirthDate.Value.ToString(Constants.LOND_DATE_FORMAT_BY_SPACE, Constants.CULTURE_INFO_BG) : null;
            }

        }

        public string EGN_IdentityNumber_IDN
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.EGN))
                {
                    return this.EGN.Trim();
                }
                else if (!string.IsNullOrWhiteSpace(this.IdentityNumber))
                {
                    return this.IdentityNumber.Trim();
                }
                else if (!string.IsNullOrWhiteSpace(this.IDN))
                {
                    return this.IDN.Trim();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public ETEMEnums.PersonTypeEnum PersonType { get; set; }

        public int idLecturer { get; set; }

        //public int idAcademicPeriod { get; set; }

        private static Dictionary<int, string> listMonths = new Dictionary<int, string>()
        {
             {1,"януари"},
             {2,"февруари"},
             {3,"март"},
             {4,"април"},
             {5,"май"},
             {6,"юни"},
             {7,"юли"},
             {8,"август"},
             {9,"септември"},
             {10,"октомври"},
             {11,"ноември"},
             {12,"декември"},
         };
    }
};