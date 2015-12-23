using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Helpers;

namespace ETEMModel.Models
{
    public partial class Location : Identifiable
    {
        public int EntityID
        {
            get { return this.idLocation; }
        }

        public string ValidationErrorsAsString { get; set; }
        public List<string> ValidateEntity(CallContext outputContext)
        {
            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;
            ETEMDataModelEntities dbContext = new ETEMDataModelEntities();

            this.ValidationErrorsAsString = string.Empty;
            List<string> result = new List<string>();

            var existEntity = dbContext.Locations.Where(
                                                        e => e.Name == this.Name &&
                                                             e.idVillageType == this.idVillageType &&
                                                             e.idMunicipality == this.idMunicipality &&
                                                             e.idLocation != this.idLocation
                                                             ).FirstOrDefault();

            if (existEntity != null)
            {
                result.Add(BaseHelper.GetCaptionString("Entity_Location_VillageType_Municipality_Exist"));
            }

            this.ValidationErrorsAsString = string.Join(",", result.ToArray());

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