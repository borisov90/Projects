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
    public partial class ModuleMainData : BaseUserControl
    {
        private Module currentEntity;

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public override void UserControlLoad()
        {
            if (this.ownerPage == null)
            {
                throw new UMSException("Current Page is null or is not inheritor of BasicPage.");
            }


            this.pnlFormData.Visible = true;
            this.currentEntity = this.ownerPage.AdminClientRef.GetModuleByID(this.CurrentEntityMasterID);

            if (currentEntity != null)
            {

                this.tbxModuleName.Text = currentEntity.ModuleName;
                this.tbxModuleSysName.Text = currentEntity.ModuleSysName;
                this.tbxComment.Text = currentEntity.Comment;
                this.chbxNeedCheck.Checked = currentEntity.NeedCheck;

                this.hdnRowMasterKey.Value = currentEntity.EntityID.ToString();

            }
            else
            {
                this.lbResultContext.Text = "";

                this.tbxModuleName.Text = string.Empty;
                this.tbxModuleSysName.Text = string.Empty;
                this.tbxComment.Text = string.Empty;
                this.chbxNeedCheck.Checked = false;


                this.hdnRowMasterKey.Value = string.Empty;
            }


        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.hdnRowMasterKey.Value))
            {
                this.currentEntity = new Module();
            }
            else
            {
                this.currentEntity = this.ownerPage.AdminClientRef.GetModuleByID(this.hdnRowMasterKey.Value);

                if (this.currentEntity == null)
                {
                    this.lbResultContext.Text = String.Format(BaseHelper.GetCaptionString("Entity_Setting_Not_Found_By_ID"), this.hdnRowMasterKey.Value);
                    this.ownerPage.FormLoad();
                    return;
                }

            }


            currentEntity.ModuleName = this.tbxModuleName.Text;
            currentEntity.ModuleSysName = this.tbxModuleSysName.Text;
            currentEntity.Comment = this.tbxComment.Text;
            currentEntity.NeedCheck = this.chbxNeedCheck.Checked;

            CallContext resultContext = new CallContext();

            resultContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;

            resultContext = this.ownerPage.AdminClientRef.ModuleSave(currentEntity, resultContext);

            this.lbResultContext.Text = resultContext.Message;
            this.hdnRowMasterKey.Value = resultContext.EntityID;
            this.CurrentEntityMasterID = resultContext.EntityID;


            this.ownerPage.ReloadModuleDataViewApplication();

            this.ownerPage.FormLoad();

        }
    }
}