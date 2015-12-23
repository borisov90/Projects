using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models.DataView
{
    public class NotificationDataView : Notification, DataViewInterface
    {
        #region DataViewInterface Members

        public string IdEntity
        {
            get { return this.idNotification.ToString(); }
            set
            {
                this.idNotification = Int32.Parse(value);
            }
        }

        #endregion

        public string FirstNameFrom { get; set; }
        public string LastNameFrom { get; set; }
        public string StatusName { get; set; }
        public int? idPerson { get; set; }

        public string SendDateStr
        {
            get
            {
                if (this.SendDate.HasValue)
                {
                    return this.SendDate.Value.ToString("dd.MM.yyyy");
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string FullNameFrom
        {
            get
            {

                return (string.IsNullOrEmpty(this.FirstNameFrom) ? "" : this.FirstNameFrom) +
                       (string.IsNullOrEmpty(this.LastNameFrom) ? "" : " " + this.LastNameFrom);
            }
        }

        public string FirstNameTo { get; set; }
        public string LastNameTo { get; set; }

        public string FullNameTo
        {
            get
            {

                return (string.IsNullOrEmpty(this.FirstNameTo) ? "" : this.FirstNameTo) +
                       (string.IsNullOrEmpty(this.LastNameTo) ? "" : " " + this.LastNameTo);
            }
        }

    }
}