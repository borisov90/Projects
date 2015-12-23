using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models.Partial
{
    public class AttachmentDataView : Attachment
    {
        public string AttachmentTypeName { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string TwoNamesPlusTitle
        {
            get
            {
                return (string.IsNullOrEmpty(this.Title) ? "" : this.Title + " ") +
                       (string.IsNullOrEmpty(this.FirstName) ? "" : this.FirstName) +
                       (string.IsNullOrEmpty(this.LastName) ? "" : " " + this.LastName);
            }
        }

        public int idAttachmentTypeKeyType { get; set; }
        public string AttachmentTypeKeyTypeIntCode { get; set; }
    }
}