using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Helpers;

namespace ETEMModel.Models
{
    public partial class Role : Identifiable
    {

        public int EntityID
        {
            get { return this.idRole; }
        }


        public string ValidationErrorsAsString { get; set; }
        public List<string> ValidateEntity(CallContext outputContext)
        {
            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;

            ETEMDataModelEntities dbContext = new ETEMDataModelEntities();

            ValidationErrorsAsString = string.Empty;
            List<string> result = new List<string>();

            if (this.idRole == 0)
            {
                Role role = dbContext.Roles.FirstOrDefault(s => s.Name == this.Name);
                if (role != null)
                {
                    result.Add("Вече съществува роля с това име");
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

    }
}