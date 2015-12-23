using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Models;
using System.Web.SessionState;

namespace ETEM.Share
{
    public partial class Login : BasicPage
    {
        //private ETEM.Freamwork.UserProps userProps;
        public static FormResources formResource = new FormResources
        {
            Module = Constants.MODULE_ADMIN,
            PageFullName = Constants.UMS_SHARE_LOGIN,
            PagePath = "../Share/Login.aspx"

        };

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (this.MasterPageMainManu != null)
            {
                this.MasterPageMainManu.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {



            if (!IsPostBack)
            {
                FormLoad();
            }
        }

        protected void btnForgottenPass_Click(object sender, EventArgs e)
        {
            ClearResultContext(lbResultContext);
            this.tbxForgottenUsername.Text = "";
            this.tbxForgottenEGN.Text = "";
            this.tbxForgottenEmail.Text = "";
            this.pnlForgottenPassword.Visible = true;
        }

        private string GetEgnOrIdnOrLnch(Person person)
        {
            if (!string.IsNullOrEmpty(person.EGN))
            {
                return person.EGN;
            }
            else if (!string.IsNullOrEmpty(person.IdentityNumber))
            {
                return person.IdentityNumber;
            }
            else if (!string.IsNullOrEmpty(person.IDN))
            {
                return person.IDN;
            }
            return "";
        }

        protected void btnSendNewPasswordMail_OnClick(object sender, EventArgs e)
        {
            #region Basic checkings
            if (this.tbxForgottenUsername.Text == "")
            {
                AddErrorMessage(lbResultContext, "Полето 'Потребителско име' е задължително");
                return;
            }

            if (this.tbxForgottenEGN.Text == "")
            {
                AddErrorMessage(lbResultContext, "Полето 'ЕГН/ЛНЧ/ИДН' е задължително");
                return;
            }

            if (this.tbxForgottenEmail.Text == "")
            {
                AddErrorMessage(lbResultContext, "Полето 'Email' е задължително");
                return;
            }

            #endregion

            #region dbChecks

            ETEMModel.Models.User user = AdminClientRef.GetUserByUsername(this.tbxForgottenUsername.Text);
            if (user == null)
            {
                AddErrorMessage(lbResultContext, "Не съществува потребител с това потребителско име");
                return;
            }

            Person person = AdminClientRef.GetPersonByPersonID(user.idPerson.ToString());
            string egn = GetEgnOrIdnOrLnch(person);
            if (egn != this.tbxForgottenEGN.Text)
            {
                AddErrorMessage(lbResultContext, "Данните в полето 'ЕГН/ЛНЧ/ИДН' не съвпадат с тези от потребителя");
                return;
            }

            if (person.EMail != this.tbxForgottenEmail.Text)
            {
                AddErrorMessage(lbResultContext, "Данните в полето 'Email' не съвпадат с тези от потребителя");
                return;
            }

            #endregion

            CallContext = AdminClientRef.ChangeUserForgottenPasswordPassword(user, person, CallContext);
            CheckIfResultIsSuccess(lbResultContext);
            this.lbResultContext.Text = CallContext.Message;
        }




        public override void FormLoad()
        {
           

#if DEBUG

            if (this.Server.MachineName.ToLower() == "emo".ToLower())
            {

                if (((System.Web.Configuration.HttpCapabilitiesBase)(Request.Browser)).Browser == "Chrome")
                {

                    Setting automaticLogin = GetSettingByCode(ETEMEnums.AppSettings.AutomaticDebugLogin);
                    if (automaticLogin.SettingValue.ToLower() == Constants.TRUE_VALUE_TEXT.ToLower())
                    {
                      
                        this.tbxUserName.Text = "emo";
                        this.tbxPassword.Attributes.Add("value", "123");
                        //btnLogin_Click(null, null);
                    }
                }

            }
#endif
        }

        public override string CurrentPagePath()
        {
            return formResource.PagePath;
        }

        public override string CurrentPageFullName()
        {
            return formResource.PageFullName;
        }
        public override FormResources CurrentFormResources()
        {
            return formResource;
        }
        public override string CurrentModule()
        {
            return formResource.Module;
        }


        public override void SetPageCaptions()
        {

            this.lbUserName.Text = GetCaption("UI_Label_UserName");
            this.lbPassword.Text = GetCaption("UI_Label_Password");
            this.btnLogin.Text = GetCaption("UI_Button_Login");



        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            this.lbError.Text = string.Empty;
            RequestMeasure requestMeasure = new RequestMeasure("btnLogin_Click");

            CallContext resultContext = AdminClientRef.Login(this.tbxUserName.Text, this.tbxPassword.Text);

            LogDebug(requestMeasure.ToString());

            if (resultContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {
                this.userProps = new UserProps();

                ETEMModel.Models.User currentUser = AdminClientRef.GetUserByUserID(resultContext.EntityID);

                if (currentUser != null)
                {
                    this.userProps = MakeLoginByUserID(resultContext.EntityID);

                    LogDebug("Потребител " + this.userProps.UserName + " влезе в системата");

                    Response.Redirect(Welcome.formResource.PagePath);
                }
                else
                {
                    this.lbError.Text = ETEMModel.Helpers.BaseHelper.GetCaptionString("Съществува проблем с базата данни");
                }
            }
            else
            {
                this.lbError.Text = resultContext.Message;
            }
        }


    }
}