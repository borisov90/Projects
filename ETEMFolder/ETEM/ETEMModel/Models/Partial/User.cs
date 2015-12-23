using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Helpers;
using ETEMModel.Helpers.Admin;

namespace ETEMModel.Models
{
    public partial class User : Identifiable
    {
        private int passwordMinLength = 3;

        public int EntityID
        {
            get { return this.idUser; }
        }

        public string ValidationErrorsAsString { get; set; }

        public List<string> ValidateEntity(CallContext outputContext)
        {
            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;

            ETEMDataModelEntities dbContext = new ETEMDataModelEntities();

            this.passwordMinLength = Int32.Parse(new SettingBL().GetSettingByCode(ETEMEnums.AppSettings.PasswordMinLength.ToString()).SettingValue);

            ValidationErrorsAsString = string.Empty;
            List<string> result = new List<string>();

            if (string.IsNullOrEmpty(this.UserName))
            {
                result.Add(string.Format(BaseHelper.GetCaptionString("Entity_Common_Field_Mandatory"), BaseHelper.GetCaptionString("Entity_User_UserName")));
            }
            if (string.IsNullOrEmpty(this.Password))
            {
                result.Add(string.Format(BaseHelper.GetCaptionString("Entity_Common_Field_Mandatory"), BaseHelper.GetCaptionString("Entity_User_Password")));
            }
            if (this.Password.Length < passwordMinLength)
            {
                result.Add(BaseHelper.GetCaptionString("Entity_User_PasswordMinLength") + " " + passwordMinLength);
            }
            if (this.idPerson == Constants.INVALID_ID || this.idPerson == Constants.INVALID_ID_ZERO)
            {
                result.Add(string.Format(BaseHelper.GetCaptionString("Entity_Common_Field_Mandatory"), BaseHelper.GetCaptionString("Entity_User_Person")));
            }
            if (this.idStatus == Constants.INVALID_ID || this.idStatus == Constants.INVALID_ID_ZERO)
            {
                result.Add(string.Format(BaseHelper.GetCaptionString("Entity_Common_Field_Mandatory"), BaseHelper.GetCaptionString("Entity_User_Status")));
            }

            var existUser = dbContext.Users.Where(u => u.UserName == this.UserName && u.idUser != this.idUser).FirstOrDefault();

            if (existUser != null)
            {
                result.Add(BaseHelper.GetCaptionString("Entity_User_UserName_Exist"));
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


    }
}