using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Models;
using ETEMModel.Helpers;

namespace ETEM.Controls.Admin
{
    public partial class PermittedActionMainData : BaseUserControl
    {

        private PermittedAction currentEntity;

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public override void UserControlLoad()
        {
            if (this.ownerPage == null)
            {
                throw new UMSException("Current Page is null or is not inheritor of BasicPage.");
            }

            loadInitControls();

            this.pnlFormData.Visible = true;
            this.currentEntity = this.ownerPage.AdminClientRef.GetPermittedActionByID(this.CurrentEntityMasterID);

            if (currentEntity != null)
            {

                this.tbxFrendlyName.Text = currentEntity.FrendlyName;
                this.tbxDescription.Text = currentEntity.Description;
                this.ddlModule.SelectedValue = (currentEntity.idModule.HasValue) ? currentEntity.idModule.ToString() : Constants.INVALID_ID_STRING;

                this.hdnRowMasterKey.Value = currentEntity.EntityID.ToString();
                ClearResultContext(this.lbResultContext);

            }
            else
            {
                this.lbResultContext.Text = "";
                this.tbxFrendlyName.Text = string.Empty;
                this.tbxDescription.Text = string.Empty;

                this.hdnRowMasterKey.Value = string.Empty;
            }


        }

        private void loadInitControls()
        {
            this.ddlModule.UserControlLoad();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.hdnRowMasterKey.Value))
            {
                this.currentEntity = new PermittedAction();
            }
            else
            {
                this.currentEntity = this.ownerPage.AdminClientRef.GetPermittedActionByID(this.hdnRowMasterKey.Value);

                if (this.currentEntity == null)
                {
                    this.lbResultContext.Text = String.Format(BaseHelper.GetCaptionString("Entity_Role_Not_Found_By_ID"), this.hdnRowMasterKey.Value);
                    this.ownerPage.FormLoad();
                    return;
                }

            }


            currentEntity.FrendlyName = this.tbxFrendlyName.Text;
            currentEntity.Description = this.tbxDescription.Text;
            currentEntity.idModule = this.ddlModule.SelectedValueINT;



            CallContext resultContext = new CallContext();

            resultContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;

            resultContext = this.ownerPage.AdminClientRef.PermittedActionSave(currentEntity, resultContext);

            this.lbResultContext.Text = resultContext.Message;
            this.hdnRowMasterKey.Value = resultContext.EntityID;

            this.ownerPage.ReloadSettingApplication();

            this.ownerPage.FormLoad();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.pnlFormData.Visible = false;
        }

        public override void ClearResultContext()
        {
            this.lbResultContext.Text = string.Empty;
        }
    }
}