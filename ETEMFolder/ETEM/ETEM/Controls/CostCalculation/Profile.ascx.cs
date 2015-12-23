using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using System.Data;
using ETEMModel.Models;
using ETEM.CostCalculation;
using System.IO;

namespace ETEM.Controls.CostCalculation
{
    public partial class Profile : BaseUserControl
    {
        private ProfileSetting currentEntity;

        public void SetHdnField(string value)
        {
            this.hdnRowMasterKey.Value = value;
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

            loadInitControls();

            this.hdnRowMasterKey.Value = this.CurrentEntityMasterID;

            if (this.CurrentEntityMasterID != Constants.INVALID_ID_STRING && !string.IsNullOrEmpty(this.CurrentEntityMasterID))
            {
                this.currentEntity                          = this.ownerPage.CostCalculationRef.GetProfileSettingById(this.CurrentEntityMasterID);

                this.tbxProfile.Text                        = this.currentEntity.ProfileName;
                this.tbxProfileSAP.Text                     = this.currentEntity.ProfileNameSAP;
                this.ddlProfileCategory.SelectedValue       = this.currentEntity.idProfileCategory.ToString();
                this.ddlProfileType.SelectedValue           = this.currentEntity.idProfileType.ToString();
                this.ddlProfileComplexity.SelectedValue     = this.currentEntity.idProfileComplexity.ToString();
                this.tbxDiameterFormula.Text                = this.currentEntity.DiameterFormula;

                foreach (ListItem listItem in chBxValue.Items)
                {
                    if (listItem.Text.StartsWith("A"))
                    {
                        listItem.Selected = this.currentEntity.hasA;
                    }
                    else if (listItem.Text.StartsWith("B"))
                    {
                        listItem.Selected = this.currentEntity.hasB;
                    }
                    else if (listItem.Text.StartsWith("C"))
                    {
                        listItem.Selected = this.currentEntity.hasC;
                    }
                    else if (listItem.Text.StartsWith("D"))
                    {
                        listItem.Selected = this.currentEntity.hasD;
                    }
                    else if (listItem.Text.StartsWith("s"))
                    {
                        listItem.Selected = this.currentEntity.hasS;
                    }
                }


                this.gvValidation.DataSource                = this.ownerPage.CostCalculationRef.GetProfileSettingValidationByIDProfile(Int32.Parse(this.CurrentEntityMasterID));
                this.gvValidation.DataBind();

                if (!string.IsNullOrEmpty(currentEntity.ImagePath))
                {
                    this.imgBtnProfileSetting.ImageUrl = currentEntity.ImagePath;
                }
                else
                {
                    this.imgBtnProfileSetting.ImageUrl = @"~/Images/imageFormula.png";
                }

                this.btnAddValidation.Enabled       = true;
                this.btnRemoveValidation.Enabled    = true;
            }
            else
            {
                SetEmptyValues();
            }

            this.pnlProfile.Visible = true;
            this.pnlProfile.Focus();
            
        }

        private void loadInitControls()
        {
            ClearResultContext(lbResultContext);

            this.ddlProfileCategory.UserControlLoad();
            this.ddlProfileType.UserControlLoad();
            this.ddlProfileComplexity.UserControlLoad();            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.ProfilesSave, false))
            {
                return;
            }
            
            if (ddlProfileCategory.SelectedValue == Constants.INVALID_ID_STRING)
            {
                AddErrorMessage(this.lbResultContext, BaseHelper.GetCaptionString("Please_add_ProfileCategory"));
                return;
            }
            if (ddlProfileType.SelectedValue == Constants.INVALID_ID_STRING)
            {
                AddErrorMessage(this.lbResultContext, BaseHelper.GetCaptionString("Please_add_ProfileType"));
                return;
            }
            if (ddlProfileComplexity.SelectedValue == Constants.INVALID_ID_STRING)
            {
                AddErrorMessage(this.lbResultContext, BaseHelper.GetCaptionString("Please_add_ProfileComplexity"));
                return;
            }

            this.CurrentEntityMasterID      = this.hdnRowMasterKey.Value;
            ProfileSetting profileSetting   = new ProfileSetting();

            //редакция
            if (this.CurrentEntityMasterID != Constants.INVALID_ID_STRING && !string.IsNullOrEmpty(this.CurrentEntityMasterID))
            {
                profileSetting                  = this.ownerPage.CostCalculationRef.GetProfileSettingById(this.CurrentEntityMasterID);
                profileSetting.idModifyUser     = Convert.ToInt32(this.ownerPage.UserProps.IdUser);
                profileSetting.dModify          = DateTime.Now;
            }
            //нов документ
            else
            {
                profileSetting.idCreateUser     = Convert.ToInt32(this.ownerPage.UserProps.IdUser);
                profileSetting.dCreate          = DateTime.Now;
            }

            profileSetting.ProfileName          = this.tbxProfile.Text;
            profileSetting.ProfileNameSAP       = this.tbxProfileSAP.Text;
            profileSetting.idProfileType        = this.ddlProfileType.SelectedValueINT;
            profileSetting.idProfileCategory    = this.ddlProfileCategory.SelectedValueINT;
            profileSetting.idProfileComplexity  = this.ddlProfileComplexity.SelectedValueINT;
            profileSetting.DiameterFormula      = this.tbxDiameterFormula.Text;

            foreach (ListItem listItem in chBxValue.Items)
            {
                if (listItem.Text.StartsWith("A"))
                { 
                    profileSetting.hasA = listItem.Selected;
                }
                else if (listItem.Text.StartsWith("B"))
                {
                    profileSetting.hasB = listItem.Selected;
                }
                else if (listItem.Text.StartsWith("C"))
                {
                    profileSetting.hasC = listItem.Selected;
                }
                else if (listItem.Text.StartsWith("D"))
                {
                    profileSetting.hasD = listItem.Selected;
                }
                else if (listItem.Text.StartsWith("s"))
                {
                    profileSetting.hasS = listItem.Selected;
                }
            }

            this.ownerPage.CallContext = this.ownerPage.CostCalculationRef.ProfileSettingSave(profileSetting, this.ownerPage.CallContext);

            if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {
                this.CurrentEntityMasterID = profileSetting.EntityID.ToString();

                UserControlLoad();

                RefreshParent();

                EvaluateExpressionHelper eval = new EvaluateExpressionHelper();

                Dictionary<string, string> vals = new Dictionary<string, string>();


                vals.Add("A", "100");
                vals.Add("B", "100");
                vals.Add("C", "100");
                vals.Add("D", "100");
                vals.Add("s", "4");               

                try
                {
                    eval.EvalExpression(this.tbxDiameterFormula.Text, vals).ToString();
                    
                }
                catch (Exception ex)
                {
                    this.ownerPage.ShowMSG(ex.Message.Replace("'", "\""));
                }
            }



            CheckIfResultIsSuccess(this.lbResultContext);
            lbResultContext.Text = this.ownerPage.CallContext.Message;
        }

        private void SetEmptyValues()
        {
            this.lbResultContext.Attributes.Remove("class");
            this.lbResultContext.Text                   = string.Empty;
            this.hdnRowMasterKey.Value                  = string.Empty;

            this.ddlProfileCategory.SelectedValue       = Constants.INVALID_ID_STRING;
            this.ddlProfileType.SelectedValue           = Constants.INVALID_ID_STRING;
            this.ddlProfileComplexity.SelectedValue     = Constants.INVALID_ID_STRING;
            this.tbxProfile.Text                        = string.Empty;
            this.tbxProfileSAP.Text                     = string.Empty;          
            this.tbxValidationRequirements.Text         = string.Empty;
            this.tbxDiameterFormula.Text                = string.Empty;

            foreach (ListItem listItem in chBxValue.Items)
            {
                listItem.Selected = false;
            }

            this.btnAddValidation.Enabled               = false;
            this.btnRemoveValidation.Enabled            = false;
        }

        private void RefreshParent()
        {
            if (Parent != null)
            {
                if (Parent.Page != null && (Parent.Page as ProfilesList) != null)
                {
                    (Parent.Page as ProfilesList).FormLoad();
                }
            }
        }
      
        protected void imgBtnProfile_Click(object sender, ImageClickEventArgs e)
        {
            this.pnlAddProfileSettingImage.Visible = true;
        }

        protected void btnAddValidation_Click(object sender, EventArgs e)
        {
            if (this.tbxValidationRequirements.Text.Trim() != string.Empty && !string.IsNullOrEmpty(this.hdnRowMasterKey.Value))
            {
                ProfileSettingValidation profileSettingValidation   = new ProfileSettingValidation();
                CallContext resultContext                           = new CallContext();

                if (this.CurrentEntityMasterID == Constants.INVALID_ID_STRING || string.IsNullOrEmpty(CurrentEntityMasterID))
                {
                    this.CurrentEntityMasterID = this.hdnRowMasterKey.Value;
                }

                profileSettingValidation.idProfileSetting       = Int32.Parse(this.CurrentEntityMasterID);
                profileSettingValidation.ValidationRequirement  = this.tbxValidationRequirements.Text;
                resultContext                                   = this.ownerPage.CostCalculationRef.ProfileSettingValidationSave(profileSettingValidation, resultContext);

                if (resultContext.ResultCode == ETEMEnums.ResultEnum.Success)
                {
                    this.tbxValidationRequirements.Text = string.Empty;

                    this.gvValidation.DataSource = this.ownerPage.CostCalculationRef.GetProfileSettingValidationByIDProfile(Int32.Parse(this.CurrentEntityMasterID));
                    this.gvValidation.DataBind();
                }
            }
        }

        protected void btnRemoveValidation_Click(object sender, EventArgs e)
        {
            List<ProfileSettingValidation> listProfileSettingValidation     = new List<ProfileSettingValidation>();
            CallContext resultContext                                       = new CallContext();

            foreach (GridViewRow row in this.gvValidation.Rows)
            {
                HiddenField hdnProfileSettingValidation     = row.FindControl("hdnProfileSettingValidation") as HiddenField;
                CheckBox chbxRemoveValidation               = row.FindControl("chbxRemoveValidation") as CheckBox;

                if (chbxRemoveValidation.Checked)
                {
                    ProfileSettingValidation profileSettingValidation = this.ownerPage.CostCalculationRef.GetProfileSettingValidationById(hdnProfileSettingValidation.Value.ToString());
                    listProfileSettingValidation.Add(profileSettingValidation);
                }
            }

            if (listProfileSettingValidation.Count > 0)
            {
                //изтриваме ProfileSettingValidation
                resultContext = new CallContext();
                resultContext = this.ownerPage.CostCalculationRef.ProfileSettingValidationRemove(listProfileSettingValidation, resultContext);
            }

            this.gvValidation.DataSource = this.ownerPage.CostCalculationRef.GetProfileSettingValidationByIDProfile(Int32.Parse(this.hdnRowMasterKey.Value));
            this.gvValidation.DataBind();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //update на снимката
            if (!string.IsNullOrEmpty(this.hdnRowMasterKey.Value) && this.hdnRowMasterKey.Value != Constants.INVALID_ID_STRING)
            {
                string idProfile    = this.hdnRowMasterKey.Value;
                this.currentEntity  = this.ownerPage.CostCalculationRef.GetProfileSettingById(this.hdnRowMasterKey.Value);

                //създава и отваря ресурсна папка с име - idProfileSetting
                string folderName           = this.hdnRowMasterKey.Value;
                string resourcesFolderName  = GeneralPage.GetSettingByCode(ETEMEnums.AppSettings.ResourcesFolderName).SettingValue + @"\ProfileSetting\";

                //ID с което започва папката за импорт. Пример C:\Resources_ETEM\DieFormula\198
                string idStartFolder = folderName.Split('_')[0].ToString();

                DirectoryInfo folder = new DirectoryInfo(resourcesFolderName);

                //Винаги изтриваме целевата папка за да не се пълни с всяка следваща снимка
                if (folder.Exists)
                {
                    DirectoryInfo[] directories = folder.GetDirectories();

                    foreach (var file in directories)
                    {
                        if (file.Name.StartsWith(idStartFolder))
                        {
                            FileInfo[] filesToDelete = file.GetFiles();

                            foreach (var delFile in filesToDelete)
                            {
                                File.Delete(delFile.FullName);
                            }

                            break;
                        }
                    }
                }

                //и отново създаваме потребителската директория
                folder = new DirectoryInfo(resourcesFolderName + folderName);

                if (!folder.Exists)
                {
                    folder = Directory.CreateDirectory(resourcesFolderName + folderName);
                }


                //ако сме избрали нещо
                if (!string.IsNullOrEmpty(FileUpload1.FileName))
                {
                    //записваме картинката в папката
                    string pathToSave = (folder.FullName.EndsWith("\\") ? folder.FullName : folder.FullName + "\\") + FileUpload1.FileName;

                    FileUpload1.SaveAs(pathToSave);

                    //update Person
                    if (this.currentEntity != null)
                    {
                        string imagePath                        = GeneralPage.GetSettingByCode(ETEMEnums.AppSettings.WebResourcesFolderName).SettingValue + "/ProfileSetting/" + folderName + "/" + FileUpload1.FileName;
                        this.imgBtnProfileSetting.ImageUrl      = imagePath;
                        this.currentEntity.ImagePath            = imagePath;
                        this.currentEntity.idModifyUser         = Convert.ToInt32(this.ownerPage.UserProps.IdUser);
                        this.currentEntity.dModify              = DateTime.Now;

                        CallContext resultPersontContext        = new CallContext();
                        resultPersontContext.CurrentConsumerID  = idProfile;
                        resultPersontContext                    = this.ownerPage.CostCalculationRef.ProfileSettingSave(this.currentEntity, resultPersontContext);
                    }
                }

                this.CurrentEntityMasterID = idProfile;
            }

            this.pnlAddProfileSettingImage.Visible = false;

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.pnlAddProfileSettingImage.Visible = false;
        }

        



    }
}