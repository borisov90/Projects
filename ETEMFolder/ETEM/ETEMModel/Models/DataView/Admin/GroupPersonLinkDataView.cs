using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models.DataView.Admin
{
    public class GroupPersonLinkDataView : GroupPersonLink
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }

        public string PersonTitle
        {
            get
            {
                return (string.IsNullOrEmpty(this.Title) ? "" : this.Title + " ") +
                        (string.IsNullOrEmpty(this.FirstName) ? string.Empty : this.FirstName) +
                        (string.IsNullOrEmpty(this.LastName) ? string.Empty : " " + this.LastName);
            }
        }

    }
}