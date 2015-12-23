using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;

namespace ETEMModel.Models.DataView.Employees
{
    public class EmployeDataView : Employe, DataViewInterface
    {
        #region DataViewInterface Members

        public string IdEntity
        {
            get { return this.idEmploye.ToString(); }
            set { this.IdEntity = value; }
        }

        #endregion

        public static Expression<Func<Person, EmployeDataView>> FromPerson
        {
            get
            {
                return a => new EmployeDataView
                {
                    Title = a.Title,
                    FirstName = a.FirstName,
                    SecondName = a.SecondName,
                    LastName = a.LastName,
                };
            }
        }

        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string IdentityNumber { get; set; }
        public string Address { get; set; }
        public string EMail { get; set; }
        public string MobilePhone { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
        public string EGN { get; set; }
        public string IDN { get; set; }

        public string FullName
        {
            get
            {
                return (string.IsNullOrEmpty(this.FirstName) ? string.Empty : this.FirstName) +
                       (string.IsNullOrEmpty(this.SecondName) ? string.Empty : " " + this.SecondName) +
                       (string.IsNullOrEmpty(this.LastName) ? string.Empty : " " + this.LastName);
            }
        }

    }
}