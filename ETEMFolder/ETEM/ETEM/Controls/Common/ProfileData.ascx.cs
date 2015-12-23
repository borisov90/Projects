using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Models;
using ETEMModel.Helpers;

namespace ETEM.Controls.Common
{
    public partial class ProfileData : BaseUserControl
    {

        private Person currentEntity;
        protected void Page_Load(object sender, EventArgs e)
        {

        }



        public override void UserControlLoad()
        {
            
            
            
            if (!this.ownerPage.UserProps.IsCheckDomain)
            {
                this.idDivChangePassword.Visible = true;
            }
            else
            {
                this.idDivChangePassword.Visible = false;
            }

            this.tbxOldPass.Text = string.Empty;
            this.tbxNewPass1.Text = string.Empty;
            this.tbxNewPass2.Text = string.Empty;


            currentEntity = this.ownerPage.AdminClientRef.GetPersonByPersonID(this.ownerPage.UserProps.PersonID);


            this.ddlCitizenshipCorespondention.UserControlLoad();

            if (currentEntity != null)
            {

                this.tbxFirstName.Text = currentEntity.FirstName;
                this.tbxSecondName.Text = currentEntity.SecondName;
                this.tbxLastName.Text = currentEntity.LastName;

                this.tbxEMail.Text = currentEntity.EMail;
                this.tbxMobilePhone.Text = currentEntity.MobilePhone;
                this.tbxPhone.Text = currentEntity.Phone;


                this.tbxCorespondationPostCode.Text = currentEntity.CorespondationPostCode;
                this.tbxCorespondationAddress.Text = currentEntity.CorespondationAddress;

                if (this.currentEntity.idCorespondationCountry.HasValue)
                {
                    SelectEKATEsData(this.ddlCitizenshipCorespondention,
                         this.ddlMunicipalityCorespondation,
                        this.currentEntity.idCorespondationCountry.ToString(),
                        this.currentEntity.idCorespondationMunicipality,
                        this.ucAutoCompleteLocationCoreposndation,
                        this.currentEntity.idCorespondationCity);
                }
                else
                {
                    SelectEKATEsData(this.ddlCitizenshipCorespondention,
                         this.ddlMunicipalityCorespondation,
                        Constants.INVALID_ID_STRING,
                         Constants.INVALID_ID_NULLABLE,
                        this.ucAutoCompleteLocationCoreposndation,
                        Constants.INVALID_ID_NULLABLE);
                }
            }


        }
        private void SelectEKATEsData(SMCDropDownList ddlCountry, SMCDropDownList ddlMun, string countryId, int? municipalityId, SMCAutoCompleteTextBox acCity, int? cityId)
        {
            ddlCountry.SelectedValue = countryId;
            ddlMun.AdditionalParam = "idCountry=" + countryId;
            ddlMun.UserControlLoad();
            if (municipalityId.HasValue && municipalityId != Constants.INVALID_ID_NULLABLE)
            {
                ddlMun.SelectedValue = municipalityId.Value.ToString();
                acCity.AdditionalWhereParam = "idMunicipality=" + municipalityId.Value.ToString();
                acCity.UserControlLoad();

                if (cityId.HasValue && cityId != Constants.INVALID_ID_NULLABLE)
                {
                    var location = this.ownerPage.AdminClientRef.GetLocationById(cityId.Value.ToString());
                    acCity.SelectedValue = cityId.ToString();
                    acCity.Text = (location != null ? location.LocationName : string.Empty);
                }
            }

        }

        protected void btnChangeAcademicPeriod_Click(object sender, EventArgs e)
        {
            
            this.ownerPage.ShowJavaScriptMSG("Текущия период беше променен.");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }
        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            User user = this.ownerPage.AdminClientRef.GetUserByUserID(this.ownerPage.UserProps.IdUser);

            string tmpOldPass = BaseHelper.Encrypt(this.tbxOldPass.Text);

            if (user.Password == tmpOldPass)
            {
                if (this.tbxNewPass1.Text == this.tbxNewPass2.Text)
                {

                    user.Password = BaseHelper.Encrypt(this.tbxNewPass1.Text);

                    this.ownerPage.CallContext = this.ownerPage.AdminClientRef.UserSave(user, this.ownerPage.CallContext);

                    if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
                    {
                        this.ownerPage.ShowJavaScriptMSG("Вие успешно сменихте вашата парола.");
                        return;
                    }
                    else
                    {
                        this.ownerPage.ShowJavaScriptMSG(this.ownerPage.CallContext.Message);
                        return;
                    }


                }
                else
                {
                    this.ownerPage.ShowJavaScriptMSG("Полетата `Нова парола:` и `Нова парола - повторение:` трябва да бъдат еднакви, моля опитайте отново!");
                    return;
                }
            }
            else
            {
                this.ownerPage.ShowJavaScriptMSG("Грешно въведена стара парола.");
                return;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}