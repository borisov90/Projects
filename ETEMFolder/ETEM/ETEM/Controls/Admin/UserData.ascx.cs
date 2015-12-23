using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Models;
using ETEMModel.Helpers.Common;
using ETEM.Admin;
using ETEM.Share;

namespace ETEM.Controls.Admin
{
    public partial class UserData : BaseUserControl
    {
        public void SetHdnField(string value)
        {
            this.hdnRowMasterKey.Value = value;
        }

        public Panel PnlErrors
        {
            get { return this.pnlErrors; }
        }

        public BulletedList BlEroorsSave
        {
            get { return this.blEroorsSave; }
        }

        public AjaxControlToolkit.TabContainer TabContainerCtrl
        {
            get { return this.tabContainer; }
        }

        public int TabContainerActiveTabIndex
        {
            get { return this.tabContainer.ActiveTabIndex; }
            set { this.tabContainer.ActiveTabIndex = value; }
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

            if (this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.LoginAS, false))
            {
                this.btnLoginAS.Visible = true;
            }


            this.ucUserMainData.CurrentEntityMasterID = this.hdnRowMasterKey.Value;
            this.ucUserMainData.SetHdnField(this.hdnRowMasterKey.Value);
            this.ucUserMainData.UserControlLoad();

            this.ucRoleList.CurrentUserID = this.hdnRowMasterKey.Value;
            this.ucRoleList.SetHdnField(this.hdnRowMasterKey.Value);
            this.ucRoleList.UserControlLoad();

            if (this.hdnRowMasterKey.Value == string.Empty || this.hdnRowMasterKey.Value == Constants.INVALID_ID_STRING)
            {
                this.tabRoleList.Visible = false;
                //this.tabHistory.Visible = false;
            }
            else
            {
                this.tabRoleList.Visible = true;
                //this.tabHistory.Visible = true;
            }

            this.pnlFormData.Visible = true;
            this.pnlFormData.Focus();
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
        }

        private void ClearAllLabels()
        {
            this.ucUserMainData.ClearResultContext();
        }

        protected void btnCancelErorrs_Click(object sender, EventArgs e)
        {
            this.blEroorsSave.Items.Clear();
            this.pnlErrors.Visible = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearAllLabels();
            this.pnlFormData.Visible = false;
        }

        protected void btnSaveTabs_Click(object sender, EventArgs e)
        {

            if (!this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.UserSave, false))
            {
                return;
            }

            List<Tuple<CallContext, string>> listCallContext = new List<Tuple<CallContext, string>>();
            List<string> listErrors = new List<string>();

            listCallContext.Add(this.ucUserMainData.UserControlSave());

            if (listCallContext.First().Item1.ResultCode == ETEMEnums.ResultEnum.Success)
            {
                this.CurrentEntityMasterID = listCallContext.First().Item1.EntityID;
                this.SetHdnField(listCallContext.First().Item1.EntityID);
                this.ucUserMainData.CurrentEntityMasterID = listCallContext.First().Item1.EntityID;
                this.ucUserMainData.SetHdnField(listCallContext.First().Item1.EntityID);
                this.ucRoleList.SetHdnField(listCallContext.First().Item1.EntityID);

                this.tabRoleList.Visible = true;
                //this.tabHistory.Visible = true;
            }

            foreach (var item in listCallContext)
            {
                if (item.Item1.ResultCode == ETEMEnums.ResultEnum.Error)
                {
                    string[] currentItemErrors = item.Item1.Message.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < currentItemErrors.Length; i++)
                    {
                        currentItemErrors[i] = currentItemErrors[i] + "('" + item.Item2 + "')";
                    }
                    listErrors.AddRange(currentItemErrors);
                }
            }

            if (listErrors.Count > 0)
            {
                foreach (var error in listErrors)
                {
                    var listItem = new ListItem(error);
                    listItem.Attributes.Add("class", "lbResultSaveError");
                    this.blEroorsSave.Items.Add(listItem);
                }

                this.pnlErrors.Visible = true;
            }

            if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {

                if (Page != null && (Page as UserList) != null)
                {
                    (Page as UserList).LoadFilteredList();
                }

            }

        }

        protected void btnSendPassword_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.hdnRowMasterKey.Value) && this.hdnRowMasterKey.Value != Constants.INVALID_ID_STRING)
            {
                this.CurrentEntityMasterID = this.hdnRowMasterKey.Value;
            }

            User currentUser = this.ownerPage.AdminClientRef.GetUserByUserID(this.CurrentEntityMasterID);

            KeyValue kvActiveStatus = this.ownerPage.GetKeyValueByIntCode("UserStatus", "Active");
            KeyValue kvTemporarilyInactiveStatus = this.ownerPage.GetKeyValueByIntCode("UserStatus", "TemporarilyInactive");

            if (currentUser != null)
            {

                Person person = null;

                if (currentUser.idStatus == kvActiveStatus.idKeyValue)
                {
                    person = this.ownerPage.AdminClientRef.GetPersonByPersonID(currentUser.idPerson.ToString());
                }
                else if (currentUser.idStatus == kvTemporarilyInactiveStatus.idKeyValue)
                {
                    person = this.ownerPage.AdminClientRef.GetPersonByPersonID(currentUser.idAltPerson.ToString());
                }
                else
                {
                    this.ownerPage.ShowJavaScriptMSG("Потребителя е неактивен.");
                    return;
                }


                if (string.IsNullOrEmpty(person.EMail))
                {
                    this.ownerPage.ShowJavaScriptMSG("Няма въведен е-маил адрес.");
                    return;
                }

                this.ownerPage.AdminClientRef.SendMailPassword(currentUser);
                this.ownerPage.ShowJavaScriptMSG("Паролата беше изпратена.");
            }


        }

        protected void btnLoginAS_Click(object sender, EventArgs e)
        {
            BasicPage currentPage = this.Page as BasicPage;
            currentPage.MakeLoginByUserID(this.hdnRowMasterKey.Value);
            UserProps userProps = new UserProps();

            BasicPage.LogDebug("Потребител " + userProps.UserName + " влезе в системата");

            Response.Redirect(Welcome.formResource.PagePath);
        }
    }
}