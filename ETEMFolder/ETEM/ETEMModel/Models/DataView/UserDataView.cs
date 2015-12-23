using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using ETEMModel.Helpers;
using System.Collections.Generic;

namespace ETEMModel.Models.DataView
{
    public class UserDataView : User, DataViewInterface
    {
        #region DataViewInterface Members

        public string IdEntity
        {
            get { return this.IdUser.ToString(); }
        }

        #endregion

        #region DataViewInterface Members

        string DataViewInterface.IdEntity
        {
            get
            {
                return this.IdUser.ToString();
            }
            set
            {
                this.IdUser = Int32.Parse(value);
            }
        }

        #endregion

        public string Title { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string PersonName { get; set; }
        private string egn;

        public string EGN
        {
            get
            {

                if (!string.IsNullOrEmpty(egn))
                {
                    return egn;

                }
                else if (!string.IsNullOrEmpty(IdentityNumber))
                {
                    return IdentityNumber;
                }
                else if (!string.IsNullOrEmpty(IDN))
                {
                    return IDN;
                }
                else
                {
                    return Constants.INVALID_ID_STRING;

                }

            }
            set
            {
                egn = value;
            }
        }
        public string IdentityNumber { get; set; }
        public string IDN { get; set; }
        public string Status { get; set; }
        public string Roles { get; set; }
        public int IdUser { get; set; }

        public string SexIntCode { get; set; }
        public string EMail { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string AllPhone
        {
            get
            {

                if (!string.IsNullOrEmpty(Phone) && !string.IsNullOrEmpty(Mobile))
                {
                    return Phone + ", " + Mobile;
                }

                if (string.IsNullOrEmpty(Phone) && !string.IsNullOrEmpty(Mobile))
                {
                    return Mobile;
                }

                if (!string.IsNullOrEmpty(Phone) && string.IsNullOrEmpty(Mobile))
                {
                    return Phone;
                }

                return string.Empty;

            }
        }
        public int? idFaculty { get; set; }
        public int? idSpecialty { get; set; }
        public bool? IsMasterLikeBachelor { get; set; }
        public string SpecialtyStr { get; set; }
        public string OKCName { get; set; }
        public string PositionName { get; set; }
        public int? idOKC { get; set; }
        public int? idStudentStatus { get; set; }
        public string FacultyNo { get; set; }
        public string FacultyCode { get; set; }

        public string BirthPlace { get; set; }
        public string CourseName { get; set; }
        public string StudentStatusName { get; set; }
        public string StudentStatusIntCode { get; set; }

        public int? idCourse { get; set; }
        public int? CourseOrder { get; set; }
        public string SchoolFromNameSE { get; set; }
        public string ProfessionNameSE { get; set; }
        public string DiplomaNo { get; set; }
        public string AddInfo
        {
            get
            {
                string result = string.Empty;

                if (IsStudent)
                {
                    result += "Студент ";
                }

                if (IsLecturer)
                {
                    result += "Преподавател ";
                }

                if (IsPhD)
                {
                    result += "Докторант ";
                }

                if (IsEmployee)
                {
                    result += "Служител на НХА ";
                }


                if (!string.IsNullOrEmpty(FacultyCode))
                {
                    result += FacultyCode;
                }

                if (!string.IsNullOrEmpty(SpecialtyStr))
                {
                    result += ", " + SpecialtyStr;
                }

                if (!string.IsNullOrEmpty(OKCName))
                {
                    result += ", " + OKCName;
                }

                if (!string.IsNullOrEmpty(CourseName))
                {
                    result += ", " + CourseName;
                }

                if (!string.IsNullOrEmpty(FacultyNo))
                {
                    result += ", " + FacultyNo;
                }

                if (!string.IsNullOrEmpty(StudentStatusName))
                {
                    result += ", " + StudentStatusName;
                }

                if (IsForeigner)
                {
                    result += ", Чужденец ";
                }


                if (!string.IsNullOrEmpty(PositionName))
                {
                    result += ",  " + PositionName;
                }


                return result;


            }
            set
            {
                AddInfo = value;
            }
        }


        public bool IsForeigner { get; set; }

        public String IdUserStr
        {
            get
            {
                return IdUser.ToString();
            }
        }

        public string PersonFullNameTwo
        {
            get
            {
                return (string.IsNullOrEmpty(this.FirstName) ? string.Empty : this.FirstName) +
                       (string.IsNullOrEmpty(this.LastName) ? string.Empty : " " + this.LastName);
            }
        }

        public string TwoNamesPlusTitle
        {
            get
            {
                return (string.IsNullOrEmpty(this.Title) ? "" : this.Title + " ") +
                       (string.IsNullOrEmpty(this.FirstName) ? "" : this.FirstName) +
                       (string.IsNullOrEmpty(this.LastName) ? "" : " " + this.LastName);
            }
        }

        public string PersonFullName
        {
            get
            {
                return (string.IsNullOrEmpty(this.FirstName) ? string.Empty : this.FirstName) +
                       (string.IsNullOrEmpty(this.SecondName) ? string.Empty : " " + this.SecondName) +
                       (string.IsNullOrEmpty(this.LastName) ? string.Empty : " " + this.LastName);
            }
        }

        public int idStudent { get; set; }

        public bool IsLecturer { get; set; }
        public bool IsStudent { get; set; }
        public bool IsPhD { get; set; }
        public bool IsEmployee { get; set; }

        public List<UserRoleLink> UserRoleLinkList { get; set; }
    }
}
