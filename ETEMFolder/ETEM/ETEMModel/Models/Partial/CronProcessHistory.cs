using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Helpers;
using System.Globalization;

namespace ETEMModel.Models
{
    public partial class CronProcessHistory : Identifiable
    {



        #region Identifiable Members

        public int EntityID
        {
            get { return this.idCronProcessHistory; }
        }

        public List<string> ValidateEntity(CallContext outputContext)
        {
            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;

            ETEMDataModelEntities dbContext = new ETEMDataModelEntities();

            ValidationErrorsAsString = string.Empty;
            List<string> result = new List<string>();

            ValidationErrorsAsString = string.Join(",", result.ToArray());


            outputContext.Message = this.ValidationErrorsAsString;
            outputContext.EntityID = this.EntityID.ToString();

            return result;
        }

        public string ValidationErrorsAsString
        {
            get;
            set;
        }

        #endregion


        public DateTime? ExecuteDateFormatted
        {
            get
            {

                DateTimeFormatInfo dtf = new DateTimeFormatInfo();
                dtf.DateSeparator = Constants.DATE_SEPARATOR;
                dtf.ShortDatePattern = Constants.SHORT_DATE_PATTERN;


                if (String.IsNullOrEmpty(ExecuteDate))
                {
                    return null;
                }
                else
                {
                    ///11.4.2011 г. 16:27:43
                    DateTime dtDate;

                    string tmpDate = ExecuteDate.Split(' ').First();

                    bool res = DateTime.TryParse(tmpDate, dtf, DateTimeStyles.None, out dtDate);

                    if (res)
                    {
                        return dtDate;
                    }
                    else
                    {
                        return null;
                    }


                }

            }
        }

        #region Identifiable Members




        #endregion
    }
}