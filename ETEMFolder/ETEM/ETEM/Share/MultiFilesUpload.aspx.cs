using System;
using System.Collections.Generic;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Models;
using System.Linq;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.UI;

namespace ETEM.Share
{
    public partial class MultiFilesUpload : BasicPage
    {
        private UploadedFile currentEntity;
        private ETEMModel.Models.Person personEntity;

        public static FormResources formResource = new FormResources
        {
            Module = Constants.MODULE_SHARE,
            PageFullName = Constants.UMS_SHARE_MULTIFILESUPLOAD,
            PagePath = "../Share/MultiFilesUpload.aspx"

        };

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

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.FormContext.QueryString["ResourceName"] != null && this.FormContext.QueryString["idResource"] != null)
                {

                    string resourceName = BaseHelper.ConvertCyrToLatin(this.FormContext.QueryString["ResourceName"].ToString());
                    string idResource = this.FormContext.QueryString["idResource"].ToString();

                    List<UploadedFile> fileList = this.CommonClientRef.GetUploadedFile(idResource);

                    this.gvUploadedFiles.DataSource = fileList;
                    this.gvUploadedFiles.DataBind();
                }
                else
                {
                    this.btnSave.Visible = false;
                    this.gvUploadedFiles.Visible = false;
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.hdnPath.Value = Request.RawUrl.Substring(Request.RawUrl.IndexOf('?') + 1);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string idPerson = this.FormContext.QueryString["idResource"].ToString();

            foreach (GridViewRow row in this.gvUploadedFiles.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    //ID на прикачени файл
                    HiddenField hdnRowMasterKey = row.FindControl("hdnIdUploadedFile") as HiddenField;

                    //radio button
                    RadioButton rbtn = row.FindControl("rbtnSelectedFile") as RadioButton;

                    //описание за всеки прикачен файл
                    TextBox tbxDescription = row.Cells[0].FindControl("tbxDescription") as TextBox;
                    string descr = tbxDescription.Text;

                    //Update на всички редове като записваме само описанието
                    this.currentEntity = CommonClientRef.GetUploadFileByID(hdnRowMasterKey.Value);
                    currentEntity.Description = descr;

                    CallContext resultContext = new CallContext();
                    resultContext.CurrentConsumerID = this.UserProps.IdUser;
                    resultContext = CommonClientRef.UploadedFileSave(currentEntity, resultContext);

                    //update на снимката на потребителя
                    if (rbtn.Checked)
                    {
                        this.personEntity = this.AdminClientRef.GetPersonByPersonID(idPerson);
                        if (this.personEntity != null)
                        {
                            this.personEntity.ImagePath = this.currentEntity.FilePath.Replace("\\", "/").Replace("C:", string.Empty);

                            CallContext resultPersontContext = new CallContext();
                            resultPersontContext.CurrentConsumerID = this.UserProps.IdUser;
                            resultPersontContext = AdminClientRef.PersonSave(this.personEntity, resultPersontContext);
                        }

                        //refresh Parent form
                        string script = "this.window.opener.location=this.window.opener.location;";
                        if (!ClientScript.IsClientScriptBlockRegistered("REFRESH_PARENT"))
                        {
                            ClientScript.RegisterClientScriptBlock(typeof(string), "REFRESH_PARENT", script, true);
                        }

                    }
                }
            }


        }

        protected void btnStartUpload_Click(object sender, EventArgs e)
        {

        }

    }
}