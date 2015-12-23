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
    public partial class SettingMainData : BaseUserControl
    {

        private Setting currentEntity;

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
            this.currentEntity = this.ownerPage.AdminClientRef.GetSettingBySettingID(this.CurrentEntityMasterID);

            if (currentEntity != null)
            {

                this.tbxSettingName.Text = currentEntity.SettingName;
                this.tbxSettingDescription.Text = currentEntity.SettingDescription;
                this.tbxSettingIntCode.Text = currentEntity.SettingIntCode;
                this.tbxSettingValue.Text = currentEntity.SettingValue;
                this.tbxSettingDefaultValue.Text = currentEntity.SettingDefaultValue;

                this.hdnRowMasterKey.Value = currentEntity.EntityID.ToString();

                if (currentEntity.SettingIntCode == ETEMEnums.AppSettings.CronProcessStart.ToString())
                {
                    this.pnlCronProcessStart.Visible = true;

                    //CronProcessExecution execution = this.Application[Constants.APPLICATION_CRONPROCESSEXECUTION] as CronProcessExecution;

                    //if (execution != null)
                    //{
                    //    this.lbCronProcessStart.Text = "Процеса " + ((execution.InProcess) ? "e стартиран последно в " + execution.LastExecutionTime  + "." : " не е стартиран.");
                    //}
                }
            }
            else
            {
                this.lbResultContext.Text = "";
                this.tbxSettingName.Text = string.Empty;
                this.tbxSettingDescription.Text = string.Empty;
                this.tbxSettingIntCode.Text = string.Empty;
                this.tbxSettingValue.Text = string.Empty;
                this.tbxSettingDefaultValue.Text = string.Empty;

                this.hdnRowMasterKey.Value = string.Empty;
            }


        }

        private void loadInitControls()
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.hdnRowMasterKey.Value))
            {
                this.currentEntity = new Setting();
            }
            else
            {
                this.currentEntity = this.ownerPage.AdminClientRef.GetSettingBySettingID(this.hdnRowMasterKey.Value);

                if (this.currentEntity == null)
                {
                    this.lbResultContext.Text = String.Format(BaseHelper.GetCaptionString("Entity_Setting_Not_Found_By_ID"), this.hdnRowMasterKey.Value);
                    this.ownerPage.FormLoad();
                    return;
                }

            }


            currentEntity.SettingName = this.tbxSettingName.Text;
            currentEntity.SettingDescription = this.tbxSettingDescription.Text;
            currentEntity.SettingIntCode = this.tbxSettingIntCode.Text;
            currentEntity.SettingValue = this.tbxSettingValue.Text;
            currentEntity.SettingDefaultValue = this.tbxSettingDefaultValue.Text;


            CallContext resultContext = new CallContext();

            resultContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;

            resultContext = this.ownerPage.AdminClientRef.SettingSave(currentEntity, resultContext);

            this.lbResultContext.Text = resultContext.Message;
            this.hdnRowMasterKey.Value = resultContext.EntityID;

            this.ownerPage.ReloadSettingApplication();

            this.ownerPage.FormLoad();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.pnlFormData.Visible = false;
        }
    }
}