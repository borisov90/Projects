using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Admin;
using ETEM.Freamwork;
using ETEMModel.Models;
using ETEMModel.Helpers;

namespace ETEM.Controls.Admin
{
    public partial class PersonMainData : BaseUserControl
    {
        private ETEMModel.Models.Person currentEntity;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SetHdnField(string value)
        {
            this.hdnRowMasterKey.Value = value;
        }

        public override void UserControlLoad()
        {
            if (this.ownerPage == null)
            {
                throw new UMSException("Current Page is null or is not inheritor of BasicPage.");
            }

            this.currentEntity = this.ownerPage.AdminClientRef.GetPersonByPersonID(this.CurrentEntityMasterID);

            this.ucPersonalData.CurrentEntityMasterID = this.CurrentEntityMasterID;
            this.ucPersonalData.SetHdnField(this.CurrentEntityMasterID);
            this.ucPersonalData.UserControlLoad();

            this.pnlFormData.Visible = true;
        }

        protected void btnSaveTabs_Click(object sender, EventArgs e)
        {
            if (!this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.PersonSave, false))
            {
                return;
            }

            List<Tuple<CallContext, string>> listCallContext = new List<Tuple<CallContext, string>>();
            List<string> listErrors = new List<string>();

            listCallContext.Add(this.ucPersonalData.UserControlSave());

            if (listCallContext.First().Item1.ResultCode == ETEMEnums.ResultEnum.Success)
            {

            }

            foreach (var item in listCallContext)
            {
                if (item.Item1.ResultCode == ETEMEnums.ResultEnum.Error)
                {
                    string[] currentItemErrors = item.Item1.Message.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < currentItemErrors.Length; i++)
                    {
                        currentItemErrors[i] = currentItemErrors[i] + "(" + item.Item2 + ")";
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
        }

        protected void imgBtnPersonImage_Click(object sender, ImageClickEventArgs e)
        {
            this.ownerPage.OpenPageInNewWindow(ETEM.Share.UploadFile.formResource);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearAllLabels();
            this.pnlFormData.Visible = false;
        }

        private void ClearAllLabels()
        {
            this.ucPersonalData.ClearResultContext();

        }

        protected void btnCancelErorrs_Click(object sender, EventArgs e)
        {
            this.blEroorsSave.Items.Clear();
            this.pnlErrors.Visible = false;
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            this.ownerPage.ShowMSG("OPS");
        }


    }
}