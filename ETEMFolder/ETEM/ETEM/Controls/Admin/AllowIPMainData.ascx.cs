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
    public partial class AllowIPMainData : BaseUserControl
    {
        private AllowIP currentEntity;

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
            this.currentEntity = this.ownerPage.AdminClientRef.GetAllowIPByID(this.CurrentEntityMasterID);

            if (currentEntity != null)
            {

                this.tbxIP.Text = currentEntity.IP;
                this.tbxCommnet.Text = currentEntity.Commnet;
                this.chbxAllow.Checked = currentEntity.Allow;

                this.hdnRowMasterKey.Value = currentEntity.EntityID.ToString();

            }
            else
            {
                this.lbResultContext.Text = "";

                this.tbxIP.Text = string.Empty;
                this.tbxCommnet.Text = string.Empty;
                this.chbxAllow.Checked = false;


                this.hdnRowMasterKey.Value = string.Empty;
            }


        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.hdnRowMasterKey.Value))
            {
                this.currentEntity = new AllowIP();
            }
            else
            {
                this.currentEntity = this.ownerPage.AdminClientRef.GetAllowIPByID(this.hdnRowMasterKey.Value);

                if (this.currentEntity == null)
                {
                    this.lbResultContext.Text = String.Format(BaseHelper.GetCaptionString("Entity_Setting_Not_Found_By_ID"), this.hdnRowMasterKey.Value);
                    this.ownerPage.FormLoad();
                    return;
                }

            }


            currentEntity.IP = this.tbxIP.Text;
            currentEntity.Commnet = this.tbxCommnet.Text;
            currentEntity.Allow = this.chbxAllow.Checked;

            CallContext resultContext = new CallContext();

            resultContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;

            resultContext = this.ownerPage.AdminClientRef.AllowIPSave(currentEntity, resultContext);

            this.lbResultContext.Text = resultContext.Message;
            this.hdnRowMasterKey.Value = resultContext.EntityID;
            this.CurrentEntityMasterID = resultContext.EntityID;


            this.ownerPage.FormLoad();

        }
    }
}