using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Models;

namespace ETEM.Controls.Admin
{
    public partial class UserMainData : BaseUserControl
    {
        private ETEMModel.Models.User currentEntity;

        public void SetHdnField(string value)
        {
            this.hdnRowMasterKey.Value = value;
        }

        public string ParentControlID
        {
            get { return this.hdnParentControlID.Value; }
            set { this.hdnParentControlID.Value = value; }
        }

        public UserData ParentControl
        {
            get { return base.FindControlById(this.Page, this.ParentControlID) as UserData; }
        }

        private void SetEmptyValues()
        {
            ClearResultContext();

            this.tbxUserName.Text = string.Empty;
            this.tbxPassword.Text = string.Empty;
            this.tbxPassword.Attributes.Add("value", string.Empty);

            this.tbxAutoCompletePersonName.SelectedValue = Constants.INVALID_ID_STRING;
            this.tbxAutoCompletePersonName.Text = string.Empty;

            BaseHelper.CheckAndSetSelectedValue(this.ddlStatus.DropDownListCTRL,
                                                this.ownerPage.GetKeyValueByIntCode(ETEMEnums.KeyTypeEnum.UserStatus.ToString(),
                                                                                    ETEMEnums.UserStatusEnum.Active.ToString()).IdEntityString,
                                                false);

            this.tbxEGN.Text = string.Empty;
            this.tbxIdentityNumber.Text = string.Empty;
            this.tbxMail.Text = string.Empty;

            this.tbxAutoCompleteAltPersonName.SelectedValue = Constants.INVALID_ID_STRING;
            this.tbxAutoCompleteAltPersonName.Text = string.Empty;
            this.tbxAltPassword.Text = string.Empty;
            this.tbxAltPassword.Attributes.Add("value", string.Empty);

            this.tbxDescription.Text = string.Empty;

            this.hdnRowMasterKey.Value = string.Empty;
            this.ddlStatus.SelectedValue = Constants.INVALID_ID_STRING;
            this.ddlCheckDomain.SelectedValue = Constants.INVALID_ID_STRING;
        }

        private void InitLoadControls()
        {
            this.ddlStatus.UserControlLoad();
            this.ddlCheckDomain.UserControlLoad();

            this.tbxAutoCompletePersonName.UserControlLoad();
            this.tbxAutoCompleteAltPersonName.UserControlLoad();
        }

        private void CheckIfResultIsSuccess()
        {
            if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {
                this.lbResultContext.Attributes.Add("class", "alert alert-success");
            }
            else
            {
                this.lbResultContext.Attributes.Add("class", "alert alert-error");
            }
        }

        public override void ClearResultContext()
        {
            this.lbResultContext.Text = string.Empty;
            this.lbResultContext.Attributes.Clear();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void UserControlLoad()
        {
            if (this.ownerPage == null)
            {
                throw new UMSException("Current Page is null or is not inheritor of BasicPage.");
            }

            InitLoadControls();

            SetEmptyValues();

            if (!string.IsNullOrEmpty(this.hdnRowMasterKey.Value) && this.hdnRowMasterKey.Value != Constants.INVALID_ID_STRING)
            {
                this.CurrentEntityMasterID = this.hdnRowMasterKey.Value;
            }

            this.currentEntity = this.ownerPage.AdminClientRef.GetUserByUserID(this.CurrentEntityMasterID);

            if (this.currentEntity != null)
            {
                this.tbxUserName.Text = this.currentEntity.UserName;

                this.tbxPassword.Attributes.Add("value", BaseHelper.Decrypt(System.Web.HttpUtility.UrlDecode(this.currentEntity.Password)));
                this.tbxPassword.Text = ETEMModel.Helpers.BaseHelper.Decrypt(System.Web.HttpUtility.UrlDecode(this.currentEntity.Password));

                Person person = this.ownerPage.AdminClientRef.GetPersonByPersonID(this.currentEntity.idPerson.ToString());
                if (person != null)
                {
                    this.tbxAutoCompletePersonName.Text = person.FullName;
                    this.tbxAutoCompletePersonName.SelectedValue = person.idPerson.ToString();

                    this.tbxEGN.Text = person.EGN;
                    this.tbxIdentityNumber.Text = person.IdentityNumber;
                    this.tbxMail.Text = person.EMail;
                }

                BaseHelper.CheckAndSetSelectedValue(this.ddlStatus.DropDownListCTRL, this.currentEntity.idStatus, false);
                BaseHelper.CheckAndSetSelectedValue(this.ddlCheckDomain.DropDownListCTRL, this.currentEntity.idCheckDomain, false);

                if (this.currentEntity.idAltPerson.HasValue)
                {
                    Person altPerson = this.ownerPage.AdminClientRef.GetPersonByPersonID(this.currentEntity.idAltPerson.Value.ToString());
                    if (altPerson != null)
                    {
                        this.tbxAutoCompleteAltPersonName.Text = altPerson.FullName;
                        this.tbxAutoCompleteAltPersonName.SelectedValue = altPerson.idPerson.ToString();
                    }
                }

                this.tbxAltPassword.Attributes.Add("value", BaseHelper.Decrypt(System.Web.HttpUtility.UrlDecode(this.currentEntity.AltPassword)));
                this.tbxAltPassword.Text = ETEMModel.Helpers.BaseHelper.Decrypt(System.Web.HttpUtility.UrlDecode(this.currentEntity.AltPassword));


                this.tbxDescription.Text = this.currentEntity.Description;

                this.hdnRowMasterKey.Value = this.currentEntity.idUser.ToString();

                ClearResultContext();
            }
            else
            {
                SetEmptyValues();
            }
        }

        public override Tuple<CallContext, string> UserControlSave()
        {
            bool isNewEntity = true;

            if (string.IsNullOrEmpty(this.hdnRowMasterKey.Value) || this.hdnRowMasterKey.Value == Constants.INVALID_ID_STRING)
            {
                this.currentEntity = new User();
            }
            else
            {
                this.currentEntity = this.ownerPage.AdminClientRef.GetUserByUserID(this.hdnRowMasterKey.Value);

                if (this.currentEntity == null)
                {
                    string falseResult = string.Format(BaseHelper.GetCaptionString("Entity_is_not_update_in_tab"), BaseHelper.GetCaptionString("UserMain_Data"));

                    this.ownerPage.CallContext.ResultCode = ETEMEnums.ResultEnum.Error;
                    this.ownerPage.CallContext.Message = falseResult;

                    return new Tuple<CallContext, string>(this.ownerPage.CallContext, BaseHelper.GetCaptionString("UserMain_Data"));
                }

                isNewEntity = false;
            }

            this.currentEntity.UserName = this.tbxUserName.Text.Trim();
            this.currentEntity.Password = ETEMModel.Helpers.BaseHelper.Encrypt(this.tbxPassword.Text.Trim());


            this.currentEntity.idPerson = this.tbxAutoCompletePersonName.SelectedValueIntOrInvalidID;
            this.currentEntity.idStatus = this.ddlStatus.SelectedValueINT;
            this.currentEntity.idCheckDomain = this.ddlCheckDomain.SelectedValueINT;
            this.currentEntity.idAltPerson = this.tbxAutoCompleteAltPersonName.SelectedValueINT;
            this.currentEntity.AltPassword = ETEMModel.Helpers.BaseHelper.Encrypt(this.tbxAltPassword.Text.Trim());
            this.currentEntity.Description = this.tbxDescription.Text;

            this.ownerPage.CallContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;

            this.ownerPage.CallContext = this.ownerPage.AdminClientRef.UserSave(this.currentEntity, this.ownerPage.CallContext);

            this.lbResultContext.Text = this.ownerPage.CallContext.Message;
            if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {
                this.hdnRowMasterKey.Value = this.ownerPage.CallContext.EntityID;

                this.tbxPassword.Attributes.Add("value", BaseHelper.Decrypt(System.Web.HttpUtility.UrlDecode(this.currentEntity.Password)));
                this.tbxPassword.Text = ETEMModel.Helpers.BaseHelper.Decrypt(System.Web.HttpUtility.UrlDecode(this.currentEntity.Password));

                this.tbxAltPassword.Attributes.Add("value", BaseHelper.Decrypt(System.Web.HttpUtility.UrlDecode(this.currentEntity.AltPassword)));
                this.tbxAltPassword.Text = ETEMModel.Helpers.BaseHelper.Decrypt(System.Web.HttpUtility.UrlDecode(this.currentEntity.AltPassword));


                Person person = this.ownerPage.AdminClientRef.GetPersonByPersonID(this.currentEntity.idPerson.ToString());
                if (isNewEntity && person != null)
                {
                    this.tbxEGN.Text = person.EGN;
                    this.tbxIdentityNumber.Text = person.IdentityNumber;
                }
            }

            CheckIfResultIsSuccess();



            return new Tuple<CallContext, string>(this.ownerPage.CallContext, BaseHelper.GetCaptionString("UserMain_Data"));
        }

    }
}