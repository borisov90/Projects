using ETEM.CostCalculation;
using ETEM.Freamwork;
using ETEMModel;
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
    public partial class DiePriceListData : BaseUserControl
    {
        private DiePriceList currentEntity;

        public void SetHdnField(string value)
        {
            this.hdnRowMasterKey.Value = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void SetEmptyValues()
        {
            this.btnImportPriceListDetails.Enabled = false;

            this.lbResultContext.Attributes.Remove("class");
            this.lbResultContext.Text = string.Empty;

            this.ddlVendor.SelectedValue = Constants.INVALID_ID_STRING;
            this.tbxDateFrom.Text = string.Empty;
            this.tbxDateTo.Text = string.Empty;

            this.ddlVendor.DropDownEnabled = true;
            this.tbxDateFrom.ReadOnly = false;

            this.hdnRowMasterKey.Value = string.Empty;
        }

        public override void UserControlLoad()
        {
            SetEmptyValues();
            base.ClearResultContext(this.lbResultContext);

            if (this.ownerPage == null)
            {
                throw new UMSException("Current Page is null or is not inheritor of BasicPage.");
            }

            if (!string.IsNullOrEmpty(this.hdnRowMasterKey.Value) && this.hdnRowMasterKey.Value != Constants.INVALID_ID_ZERO_STRING)
            {
                this.CurrentEntityMasterID = this.hdnRowMasterKey.Value;
            }

            InitLoadControls();

            this.currentEntity = this.ownerPage.CostCalculationRef.GetDiePriceListById(this.CurrentEntityMasterID);

            if (this.currentEntity != null)
            {
                this.SetHdnField(this.currentEntity.idDiePriceList.ToString());

                BaseHelper.CheckAndSetSelectedValue(this.ddlVendor.DropDownListCTRL, this.currentEntity.idVendor.ToString(), false);
                this.tbxDateFrom.SetTxbDateTimeValue(this.currentEntity.DateFrom);
                this.tbxDateTo.SetTxbDateTimeValue(this.currentEntity.DateTo);

                this.btnImportPriceListDetails.Enabled = true;

                if (this.currentEntity.DiePriceListDetails != null && this.currentEntity.DiePriceListDetails.Count > 0)
                {
                    this.ddlVendor.DropDownEnabled = false;
                    this.tbxDateFrom.ReadOnly = true;
                    this.btnImportPriceListDetails.Enabled = false;
                }

                base.ClearResultContext(this.lbResultContext);                
            }
            else
            {
                SetEmptyValues();
            }

            this.pnlFormData.Visible = true;
            this.pnlFormData.Focus();
        }

        private void InitLoadControls()
        {
            this.ddlVendor.UserControlLoad();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.DiePriceListSave, false))
            {
                return;
            }

            
            if (string.IsNullOrEmpty(this.hdnRowMasterKey.Value) || this.hdnRowMasterKey.Value == Constants.INVALID_ID_STRING)
            {
                this.currentEntity = new DiePriceList();
            }
            else
            {
                this.currentEntity = this.ownerPage.CostCalculationRef.GetDiePriceListById(this.hdnRowMasterKey.Value);

                if (this.currentEntity == null)
                {
                    this.ownerPage.CallContext.ResultCode = ETEMEnums.ResultEnum.Error;

                    base.AddMessage(this.lbResultContext, string.Format("Entity `DiePriceList` not found by ID ({0})!", this.hdnRowMasterKey.Value));
                    
                    return;
                }

               
            }

            this.currentEntity.idVendor = this.ddlVendor.SelectedValueINT;
            this.currentEntity.DateFrom = this.tbxDateFrom.TextAsDateParseExactOrMinValue;
            this.currentEntity.DateTo = this.tbxDateTo.TextAsDateParseExact;

            this.ownerPage.CallContext = this.ownerPage.CostCalculationRef.DiePriceListSave(new List<DiePriceList>() { this.currentEntity }, this.ownerPage.CallContext);

            if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {
                this.hdnRowMasterKey.Value = this.ownerPage.CallContext.EntityID;

                base.AddMessage(this.lbResultContext, this.ownerPage.CallContext.Message);

                this.btnImportPriceListDetails.Enabled = true;
            }
            else
            {
                if (!ShowErrors(new List<CallContext>() { this.ownerPage.CallContext }))
                {
                    return;
                }
            }

            if (this.ownerPage is DiePricesList)
            {
                ((DiePricesList)this.ownerPage).LoadFilteredList();
            }
        }

        protected void btnCancelErorrs_Click(object sender, EventArgs e)
        {
            this.blEroorsSave.Items.Clear();
            this.pnlErrors.Visible = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            base.ClearResultContext(this.lbResultContext);
            this.pnlFormData.Visible = false;
        }

        private bool ShowErrors(List<CallContext> listCallContext)
        {
            bool result = true;

            List<string> listErrors = new List<string>();

            foreach (var item in listCallContext)
            {
                if (item.ResultCode == ETEMEnums.ResultEnum.Error)
                {
                    string[] currentItemErrors = item.Message.Split(new string[] { Constants.ERROR_MESSAGES_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < currentItemErrors.Length; i++)
                    {
                        currentItemErrors[i] = currentItemErrors[i];
                    }
                    listErrors.AddRange(currentItemErrors);
                }
                else if (item.ResultCode == ETEMEnums.ResultEnum.Warning)
                {
                    listErrors.Add(item.Message);
                }
            }
            if (listErrors.Count > 0)
            {
                this.blEroorsSave.Items.Clear();

                foreach (var error in listErrors)
                {
                    var listItem = new ListItem(error);
                    listItem.Attributes.Add("class", "lbResultSaveError");
                    this.blEroorsSave.Items.Add(listItem);
                }

                this.pnlErrors.Visible = true;

                result = false;
            }

            return result;
        }

        protected void btnDownloadImportTemplate_Click(object sender, EventArgs e)
        {
            //string appFolder = Server.MapPath("/" + GeneralPage.GetSettingByCode(ETEMEnums.AppSettings.WebApplicationName).SettingValue);
            string resourcesFolder = GeneralPage.GetSettingByCode(ETEMEnums.AppSettings.ResourcesFolderName).SettingValue;
            string templateFolder = GeneralPage.GetSettingByCode(ETEMEnums.AppSettings.FolderTemplates).SettingValue;
            string templateName = GeneralPage.GetSettingByCode(ETEMEnums.AppSettings.TemplateDiePriceListDetails).SettingValue;            

            //FilePath ContentType

            string templatePath = resourcesFolder + "\\" + templateFolder;
            string templateFullName = templatePath + "\\" + templateName;

            DirectoryInfo dirInfoTemplates = new DirectoryInfo(templatePath + "\\Downloads");

            if (!Directory.Exists(templatePath))
            {
                Directory.CreateDirectory(templatePath);
            }

            string fileNameOnly = Path.GetFileNameWithoutExtension(templateFullName);

            string fileFullNameToDownload = dirInfoTemplates.FullName + "\\" + fileNameOnly + "_" + DateTime.Now.ToString(Constants.DATE_PATTERN_FOR_FILE_SUFFIX) + Constants.FILE_XLSX_EXTENSION;

            File.Copy(templateFullName, fileFullNameToDownload);

            FileInfo fiFileToDownload = new FileInfo(fileFullNameToDownload);
            if (fiFileToDownload.Exists)
            {
                fiFileToDownload.IsReadOnly = false;
            }

            string param = "FilePath=" + fileFullNameToDownload + "&ContentType=" + BaseHelper.GetMimeType(Constants.FILE_XLSX_EXTENSION) + "&Delete=true";

            this.ownerPage.OpenPageForDownloadFile(Constants.DOWNLOAD_PAGE_PATH, param);
        }

        protected void btnImportPriceListDetails_Click(object sender, EventArgs e)
        {
            if (!this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.DiePriceListImportDetails, false))
            {
                return;
            }

            base.RunJavaScriptModalWindow();

            if (this.fuImportPriceListDetails.HasFile)
            {
                string selectedFileMimeType = this.fuImportPriceListDetails.PostedFile.ContentType;

                string excelMimeType = BaseHelper.GetMimeType(Constants.FILE_XLSX_EXTENSION);

                if (!string.Equals(selectedFileMimeType, excelMimeType, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.ownerPage.ShowMSG("Selected file is in incorrect format, it must be Excel-2007 or newer version!", false);
                    return;
                }

                string fileFullName = string.Empty;
                string folderPath = string.Empty;

                folderPath = GeneralPage.GetSettingByCode(ETEMModel.Helpers.ETEMEnums.AppSettings.ResourcesFolderName).SettingValue;

                folderPath += "\\CostCalculation\\DiePriceList\\Import\\" + DateTime.Today.Year + "\\";

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                fileFullName = folderPath + "\\Import_DiePriceListDetails_" +
                                               DateTime.Now.ToString(Constants.DATE_PATTERN_FOR_FILE_SUFFIX) +
                                               Constants.FILE_XLSX_EXTENSION;

                this.fuImportPriceListDetails.PostedFile.SaveAs(fileFullName);

                this.ownerPage.CallContext = this.ownerPage.CostCalculationRef.ImportDiePriceListDetails(fileFullName, this.hdnRowMasterKey.Value, this.ownerPage.CallContext);

                if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
                {
                    base.AddMessage(this.lbResultContext, this.ownerPage.CallContext.Message);
                }
                else
                {
                    if (!ShowErrors(new List<CallContext>() { this.ownerPage.CallContext }))
                    {
                        return;
                    }
                }
            }
            else
            {
                this.ownerPage.ShowMSG("Please select file to import!", false);
            }
        }
    }
}