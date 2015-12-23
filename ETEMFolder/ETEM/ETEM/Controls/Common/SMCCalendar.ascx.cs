using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using System.Globalization;
using AjaxControlToolkit;

namespace ETEM.Controls.Common
{
    public partial class SMCCalendar : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void UserControlLoad()
        {

        }

        public TextBox TextBoxDate
        {
            get { return this.tbxDate; }
        }

        public CalendarExtender CalendarCtrl
        {
            get { return this.tbxDate_CalendarExtender; }
        }

        public string Text
        {
            get { return this.tbxDate.Text; }
            set { this.tbxDate.Text = value; }

        }

        /// <summary>
        /// if the format comming from the db is always the the same it will work, otherwise it wont
        /// </summary>
        /// <param name="dateTime"></param>
        public void SetTxbDateTimeValue(DateTime? dateTime)
        {
            //DateTime tmpDate;
            //if (DateTime.TryParseExact(dateTime.ToString(),
            //  "MM/dd/yyyy hh:mm:ss tt",
            //  CultureInfo.InvariantCulture,
            //   DateTimeStyles.None
            //   , out tmpDate))
            if (dateTime != null)
            {
                //string dateWrongFormat = dateTime.ToString().Split(' ').First();
                //string[] dateParams = dateWrongFormat.Split(Constants.CHAR_SEPARATORS, StringSplitOptions.RemoveEmptyEntries);
                //string month = dateParams[0];
                //dateParams[0] = dateParams[1];
                //dateParams[1] = month;
                //dateParams[0] = dateParams[0].Length == 1 ? "0" + dateParams[0] : dateParams[0];
                //dateParams[1] = dateParams[1].Length == 1 ? "0" + dateParams[1] : dateParams[1];
                //this.tbxDate.Text = string.Join(".", dateParams);

                this.tbxDate.Text = dateTime.Value.ToString(Constants.SHORT_DATE_PATTERN);

            }
            else
            {
                this.tbxDate.Text = string.Empty;
            }
        }

        public DateTime? TextAsDateParseExact
        {
            get
            {
                DateTime tmpDate;
                if (DateTime.TryParseExact(this.tbxDate.Text,
                    Constants.SHORT_DATE_PATTERN,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out tmpDate))
                {
                    return tmpDate;
                }
                else
                {
                    return null;
                }
            }
        }

        public DateTime? TextAsDate
        {
            get
            {
                DateTime tmpDate;

                if (DateTime.TryParse(this.tbxDate.Text, out tmpDate))
                {
                    return tmpDate;
                }
                else
                {
                    return null;
                }
            }
        }

        public DateTime TextAsDateParseExactOrMinValue
        {
            get
            {
                DateTime tmpDate;
                if (DateTime.TryParseExact(this.tbxDate.Text,
                    Constants.SHORT_DATE_PATTERN,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out tmpDate))
                {
                    return tmpDate;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
        }

        public string CssClassTextBox
        {
            get { return this.tbxDate.CssClass; }
            set { this.tbxDate.CssClass = value; }
        }

        public string CssClassCalendar
        {
            get { return this.tbxDate_CalendarExtender.CssClass; }
            set { this.tbxDate_CalendarExtender.CssClass = value; }
        }

        public bool ReadOnly
        {
            get { return this.tbxDate.ReadOnly; }
            set
            {
                this.tbxDate.ReadOnly = value;
                this.CalendarCtrl.Enabled = !value;
            }
        }
    }
}