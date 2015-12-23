using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Helpers;

namespace ETEMModel.Models.DataView.Common
{
    public class PersonByTypeDataView
    {
        public int idPerson { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string PersonTypeName { get; set; }
        public string FacultyNo { get; set; }
        public string Title { get; set; }

        public string FullNameTwo
        {
            get
            {
                return (string.IsNullOrEmpty(this.FirstName) ? string.Empty : this.FirstName) +
                       (string.IsNullOrEmpty(this.LastName) ? string.Empty : " " + this.LastName);
            }
        }
        public string FullNameTwoAndType
        {
            get
            {
                return (string.IsNullOrEmpty(this.FirstName) ? string.Empty : this.FirstName) +
                       (string.IsNullOrEmpty(this.LastName) ? string.Empty : " " + this.LastName) +
                       (string.IsNullOrEmpty(this.PersonTypeName) ? string.Empty : " / " + this.PersonTypeName);
            }
        }
        public string FullName
        {
            get
            {
                return (string.IsNullOrEmpty(this.FirstName) ? string.Empty : this.FirstName) +
                       (string.IsNullOrEmpty(this.SecondName) ? string.Empty : " " + this.SecondName) +
                       (string.IsNullOrEmpty(this.LastName) ? string.Empty : " " + this.LastName);
            }
        }
        public string FullNameLastTwo
        {
            get
            {
                return (string.IsNullOrEmpty(this.SecondName) ? "" : this.SecondName) +
                       (string.IsNullOrEmpty(this.LastName) ? "" : " " + this.LastName);
            }
        }
        public string FullNameAndType
        {
            get
            {
                return (string.IsNullOrEmpty(this.FirstName) ? string.Empty : this.FirstName) +
                       (string.IsNullOrEmpty(this.SecondName) ? string.Empty : " " + this.SecondName) +
                       (string.IsNullOrEmpty(this.LastName) ? string.Empty : " " + this.LastName) +
                       (string.IsNullOrEmpty(this.PersonTypeName) ? string.Empty : " / " + this.PersonTypeName);
            }
        }
        public string NamesPlusTitle
        {
            get
            {
                return (string.IsNullOrEmpty(this.Title) ? string.Empty : this.Title) +
                       (string.IsNullOrEmpty(this.FirstName) ? string.Empty : " " + this.FirstName) +
                       (string.IsNullOrEmpty(this.LastName) ? string.Empty : " " + this.LastName);
            }
        }
        public string NamesPlusTitleAndType
        {
            get
            {
                return (string.IsNullOrEmpty(this.Title) ? string.Empty : this.Title) +
                       (string.IsNullOrEmpty(this.FirstName) ? string.Empty : " " + this.FirstName) +
                       (string.IsNullOrEmpty(this.LastName) ? string.Empty : " " + this.LastName) +
                       (string.IsNullOrEmpty(this.PersonTypeName) ? string.Empty : " / " + this.PersonTypeName);
            }
        }
        public string NamesPlusFacultyNo
        {
            get
            {
                return (string.IsNullOrEmpty(this.FirstName) ? string.Empty : this.FirstName) +
                       (string.IsNullOrEmpty(this.LastName) ? string.Empty : " " + this.LastName) +
                       (string.IsNullOrEmpty(this.FacultyNo) ? string.Empty : " / " + BaseHelper.GetCaptionString("GridView_CampusApplications_FacultyNo") + ": " + this.FacultyNo);
            }
        }
        public string NamesPlusFacultyNoAndType
        {
            get
            {
                return (string.IsNullOrEmpty(this.FirstName) ? string.Empty : this.FirstName) +
                       (string.IsNullOrEmpty(this.LastName) ? string.Empty : " " + this.LastName) +
                       (string.IsNullOrEmpty(this.PersonTypeName) ? string.Empty : " / " + this.PersonTypeName) +
                       (string.IsNullOrEmpty(this.FacultyNo) ? string.Empty : " / " + BaseHelper.GetCaptionString("GridView_CampusApplications_FacultyNo") + ": " + this.FacultyNo);
            }
        }
        public string FullNamePlusAll
        {
            get
            {
                return (string.IsNullOrEmpty(this.Title) ? string.Empty : this.Title) +
                       (string.IsNullOrEmpty(this.FirstName) ? string.Empty : " " + this.FirstName) +
                       (string.IsNullOrEmpty(this.SecondName) ? string.Empty : " " + this.SecondName) +
                       (string.IsNullOrEmpty(this.LastName) ? string.Empty : " " + this.LastName) +
                       (string.IsNullOrEmpty(this.FacultyNo) ? string.Empty : " / " + BaseHelper.GetCaptionString("GridView_CampusApplications_FacultyNo") + ": " + this.FacultyNo) +
                       (string.IsNullOrEmpty(this.PersonTypeName) ? string.Empty : " / " + this.PersonTypeName);
            }
        }

        public string FullNameStudentAndPhds
        {
            get
            {
                return
                    (string.IsNullOrEmpty(this.Source) ? string.Empty : "  " + this.Source) +
                    (string.IsNullOrEmpty(this.Title) ? string.Empty : this.Title) +
                    (string.IsNullOrEmpty(this.FirstName) ? string.Empty : " " + this.FirstName) +
                      (string.IsNullOrEmpty(this.SecondName) ? string.Empty : " " + this.SecondName) +
                    (string.IsNullOrEmpty(this.LastName) ? string.Empty : " " + this.LastName) +
                    (string.IsNullOrEmpty(this.FacultyNo) ? string.Empty : " / " + BaseHelper.GetCaptionString("GridView_CampusApplications_FacultyNo") + ": " + this.FacultyNo) +
                    (string.IsNullOrEmpty(this.AdditionalInfo) ? string.Empty : " / " + this.AdditionalInfo);


            }
        }

        public string Source { get; set; }
        public string AdditionalInfo { get; set; }
    }
}