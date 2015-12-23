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
    public partial class AttachmentDocument : BaseUserControl
    {
        private Attachment currentEntity;

        public string AttachmentDocumentType
        {
            get { return this.hdnAttachmentDocumentType.Value; }
            set { this.hdnAttachmentDocumentType.Value = value; }
        }

        public string DocKeyTypeIntCode
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.hdnDocKeyTypeIntCode.Value))
                {
                    return this.hdnDocKeyTypeIntCode.Value;
                }
                else
                {
                    return "AttachmentType";
                }
            }
            set { this.hdnDocKeyTypeIntCode.Value = value; }
        }

        public string ModuleSysName
        {
            get { return this.hdnModuleSysName.Value; }
            set { this.hdnModuleSysName.Value = value; }
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

            if (string.IsNullOrEmpty(AttachmentDocumentType))
            {
                this.ownerPage.ShowMSG("Атрибура AttachmentDocumentType е NULL. Да се въведе стойност преди да се използва контролата.");
                return;
            }

            if (string.IsNullOrEmpty(ModuleSysName))
            {
                this.ownerPage.ShowMSG("Атрибура ModuleSysName е NULL. Да се въведе стойност преди да се използва контролата.");
                return;
            }

            this.ddlAttachmentType.KeyTypeIntCode = this.DocKeyTypeIntCode;

            this.ddlAttachmentType.UserControlLoad();

            this.pnlFormData.Visible = true;
            this.currentEntity = this.ownerPage.CommonClientRef.GetAttachmentID(this.CurrentEntityMasterID);

            if (currentEntity != null)
            {
                this.tbxDescription.Text = currentEntity.Description;
                this.tbxAttachmentDate.Text = currentEntity.AttachmentDate.ToString("dd.MM.yyyy");
                this.ddlAttachmentType.SelectedValue = currentEntity.idAttachmentType.ToString();

                string tmpFolderName = BaseHelper.CheckAndReplaceStringName(this.tbxDescription.Text, "_").Replace("\n", "").Replace("\r", "");

                tmpFolderName = (tmpFolderName.Length > 50) ? tmpFolderName.Substring(0, 50) : tmpFolderName;

                this.fuAttachment.UserControlName = AttachmentDocumentType;
                this.fuAttachment.CustomFolder = currentEntity.EntityID + "_" + tmpFolderName;
                this.fuAttachment.UserControlLoad();
                this.fuAttachment.BtnUploadFileEnabled = true;

                this.hdnRowMasterKey.Value = currentEntity.EntityID.ToString();
            }
            else
            {
                this.fuAttachment.UserControlName = AttachmentDocumentType;
                this.fuAttachment.ClearGrid();
                this.fuAttachment.BtnUploadFileEnabled = false;

                this.lbResultContext.Text = "";

                this.tbxDescription.Text = string.Empty;
                this.tbxAttachmentDate.Text = DateTime.Now.ToString("dd.MM.yyyy");

                this.ddlAttachmentType.SelectedValue = Constants.INVALID_ID_STRING;

                this.hdnRowMasterKey.Value = string.Empty;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string moduleSysName = this.ownerPage.FormContext.QueryString["ModuleSysName"].ToString();
            string attachmentDocumentType = this.ownerPage.FormContext.QueryString["AttachmentDocumentType"].ToString();

            if (moduleSysName == Constants.MODULE_FINANCE &&
                attachmentDocumentType == ETEMEnums.FinanceReportTypeEnum.FinanceReport.ToString())
            {
                if (!this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.AttachmentSave, false))
                {
                    return;
                }
            }





            if (string.IsNullOrEmpty(this.tbxDescription.Text))
            {
                this.ownerPage.ShowJavaScriptMSG("Моля, описание на документа.");
                return;
            }

            if (this.tbxAttachmentDate.TextAsDateParseExact == null)
            {
                this.ownerPage.ShowJavaScriptMSG("Моля, въведете дата.");
                return;
            }

            if (this.ddlAttachmentType.SelectedValueINT == Constants.INVALID_ID)
            {
                this.ownerPage.ShowJavaScriptMSG("Моля, въведете вид документ.");
                return;
            }

            if (string.IsNullOrEmpty(this.hdnRowMasterKey.Value))
            {
                this.currentEntity = new Attachment();
                this.currentEntity.idUser = Int32.Parse(this.ownerPage.UserProps.IdUser);

                Module module = this.ownerPage.AdminClientRef.GetModuleBySysName(ModuleSysName);
                this.currentEntity.idModule = module.idModule;
            }
            else
            {
                this.currentEntity = this.ownerPage.CommonClientRef.GetAttachmentID(this.hdnRowMasterKey.Value);

                if (this.currentEntity == null)
                {
                    this.lbResultContext.Text = String.Format(BaseHelper.GetCaptionString("Entity_Setting_Not_Found_By_ID"), this.hdnRowMasterKey.Value);
                    this.ownerPage.FormLoad();
                    return;
                }
            }

            currentEntity.Description = this.tbxDescription.Text;
            currentEntity.AttachmentDate = this.tbxAttachmentDate.TextAsDateParseExact.Value;
            currentEntity.idAttachmentType = this.ddlAttachmentType.SelectedValueINT;

            this.ownerPage.CallContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;

            this.ownerPage.CallContext = this.ownerPage.CommonClientRef.AttachmentSave(currentEntity, this.ownerPage.CallContext);

            this.lbResultContext.Text = this.ownerPage.CallContext.Message;
            this.hdnRowMasterKey.Value = this.ownerPage.CallContext.EntityID;
            this.CurrentEntityMasterID = this.ownerPage.CallContext.EntityID;

            CheckIfResultIsSuccess(this.lbResultContext);

            UserControlLoad();

            this.ownerPage.FormLoad();
        }

        public void SetHdnField(string value)
        {
            this.hdnRowMasterKey.Value = value;
        }
    }
}