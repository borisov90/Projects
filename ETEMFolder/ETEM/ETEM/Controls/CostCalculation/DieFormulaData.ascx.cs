using ETEM.CostCalculation;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETEM.Controls.CostCalculation
{
    public partial class DieFormulaData : BaseUserControl
    {
        private DieFormula currentEntity;

        public void SetHdnField(string value)
        {
            this.hdnRowMasterKey.Value = value;
        }

        private void SetEmptyValues()
        {
            this.lbResultContext.Attributes.Remove("class");

            this.lbResultContext.Text               = string.Empty;
            this.hdnRowMasterKey.Value              = string.Empty;

            this.ddlNumberCavities.SelectedValue    = Constants.INVALID_ID_STRING;
            this.ddlProfileCategory.SelectedValue   = Constants.INVALID_ID_STRING;
            this.ddlProfileType.SelectedValue       = Constants.INVALID_ID_STRING;
            this.tbxDieFormulaText.Text             = string.Empty;
            this.imgBtnFormula.ImageUrl             = string.Empty;
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
                this.currentEntity                      = this.ownerPage.CostCalculationRef.GetDieFormulaById(this.CurrentEntityMasterID);

                this.ddlNumberCavities.SelectedValue    = this.currentEntity.idNumberOfCavities.ToString();
                this.ddlProfileCategory.SelectedValue   = this.currentEntity.idProfileCategory.ToString();
                this.ddlProfileType.SelectedValue       = this.currentEntity.idProfileType.ToString();
                this.tbxDieFormulaText.Text             = this.currentEntity.DieFormulaText;

                if (!string.IsNullOrEmpty(currentEntity.ImagePath))
                {
                    this.imgBtnFormula.ImageUrl = currentEntity.ImagePath;
                }
                else
                {
                    //this.imgBtnFormula.ImageUrl = @"~/Images/imageFormula.png";
                }

            }
            else
            {
                SetEmptyValues();
            }

            this.pnlDieFormulaData.Visible = true;
            this.pnlDieFormulaData.Focus();

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.DieFormulaSave, false))
            {
                return;
            }

            if (ddlNumberCavities.SelectedValue == Constants.INVALID_ID_STRING)
            {
                AddErrorMessage(this.lbResultContext, BaseHelper.GetCaptionString("Please_add_NumberCavities"));
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

            this.CurrentEntityMasterID = this.hdnRowMasterKey.Value;

            DieFormula dieFormula = new DieFormula();

            //редакция
            if (this.CurrentEntityMasterID != Constants.INVALID_ID_STRING && !string.IsNullOrEmpty(this.CurrentEntityMasterID))
            {
                dieFormula = this.ownerPage.CostCalculationRef.GetDieFormulaById(this.CurrentEntityMasterID);

                dieFormula.idModifyUser     = Convert.ToInt32(this.ownerPage.UserProps.IdUser);
                dieFormula.dModify          = DateTime.Now;
            }
            //нов документ
            else
            {
                dieFormula.idCreateUser     = Convert.ToInt32(this.ownerPage.UserProps.IdUser);
                dieFormula.dCreate          = DateTime.Now;
            }

            dieFormula.idNumberOfCavities   = this.ddlNumberCavities.SelectedValueINT;
            dieFormula.idProfileType        = this.ddlProfileType.SelectedValueINT;
            dieFormula.idProfileCategory    = this.ddlProfileCategory.SelectedValueINT;
            dieFormula.DieFormulaText       = this.tbxDieFormulaText.Text;
            
            this.ownerPage.CallContext = this.ownerPage.CostCalculationRef.DieFormulaSave(dieFormula, this.ownerPage.CallContext);

            if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {
                this.CurrentEntityMasterID = dieFormula.EntityID.ToString();

                UserControlLoad();

                RefreshParent();

                EvaluateExpressionHelper eval   = new EvaluateExpressionHelper();
                Dictionary<string, string> vals = new Dictionary<string, string>();

                vals.Add("A", "100");
                vals.Add("B", "100");
                vals.Add("C", "100");
                vals.Add("D", "100");
                vals.Add("s", "4");
                vals.Add("Ø", "4");

                try
                {
                    eval.EvalExpression(this.tbxDieFormulaText.Text, vals).ToString();

                }
                catch (Exception ex)
                {
                    this.ownerPage.ShowMSG(ex.Message.Replace("'", "\""));
                }
            }

            CheckIfResultIsSuccess(this.lbResultContext);
            lbResultContext.Text = this.ownerPage.CallContext.Message;
        }

        private void RefreshParent()
        {
            if (Parent != null)
            {
                if (Parent.Page != null && (Parent.Page as DieFormulaList) != null)
                {
                    (Parent.Page as DieFormulaList).LoadDieFormulaList();
                }
            }
        }

        private void loadInitControls()
        {
            ClearResultContext(lbResultContext);
            
            this.ddlNumberCavities.UserControlLoad();
            this.ddlProfileCategory.UserControlLoad();
            this.ddlProfileType.UserControlLoad();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //update на снимката на формулата
            if (!string.IsNullOrEmpty(this.hdnRowMasterKey.Value) && this.hdnRowMasterKey.Value != Constants.INVALID_ID_STRING)
            {
                string idPerson                 = this.hdnRowMasterKey.Value;
                this.currentEntity              = this.ownerPage.CostCalculationRef.GetDieFormulaById(this.hdnRowMasterKey.Value);

                //създава и отваря ресурсна папка с име - idFormula
                string folderName               = this.hdnRowMasterKey.Value;
                string resourcesFolderName      = GeneralPage.GetSettingByCode(ETEMEnums.AppSettings.ResourcesFolderName).SettingValue + @"\DieFormula\";

                //ID с което започва папката за импорт. Пример C:\Resources_ETEM\DieFormula\198
                string idStartFolder            = folderName.Split('_')[0].ToString();

                DirectoryInfo folder            = new DirectoryInfo(resourcesFolderName);

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
                        this.currentEntity.ImagePath                = GeneralPage.GetSettingByCode(ETEMEnums.AppSettings.WebResourcesFolderName).SettingValue + "/DieFormula/" + folderName + "/" + FileUpload1.FileName;
                        CallContext resultPersontContext            = new CallContext();
                        resultPersontContext.CurrentConsumerID      = idPerson;
                        resultPersontContext                        = this.ownerPage.CostCalculationRef.DieFormulaSave(this.currentEntity, resultPersontContext);
                    }
                }

                this.CurrentEntityMasterID = idPerson;
            }

            this.pnlAddFormulaImage.Visible = false;            

        }

        protected void imgBtnFormulaImage_Click(object sender, ImageClickEventArgs e)
        {
            this.pnlAddFormulaImage.Visible = true;
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.pnlAddFormulaImage.Visible = false;
        }



    }
}