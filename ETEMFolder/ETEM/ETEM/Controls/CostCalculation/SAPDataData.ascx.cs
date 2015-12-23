﻿using ETEM.CostCalculation;
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
    public partial class SAPDataData : BaseUserControl
    {
        private SAPData currentEntity;

        public void SetHdnField(string value)
        {
            this.hdnRowMasterKey.Value = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void SetEmptyValues()
        {
            this.btnImport.Enabled = false;

            this.lbResultContext.Attributes.Remove("class");
            this.lbResultContext.Text = string.Empty;

            this.tbxDateFrom.Text = string.Empty;
            this.tbxDateTo.Text = string.Empty;

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

            this.currentEntity = this.ownerPage.CostCalculationRef.GetSAPDataById(this.CurrentEntityMasterID);

            if (this.currentEntity != null)
            {
                this.SetHdnField(this.currentEntity.idSAPData.ToString());

                this.tbxDateFrom.SetTxbDateTimeValue(this.currentEntity.DateFrom);
                this.tbxDateTo.SetTxbDateTimeValue(this.currentEntity.DateTo);

                this.btnImport.Enabled = true;

                if ((this.currentEntity.SAPDataExpenses != null && this.currentEntity.SAPDataExpenses.Count > 0) ||
                    (this.currentEntity.SAPDataQuantities != null && this.currentEntity.SAPDataQuantities.Count > 0))
                {                    
                    this.tbxDateFrom.ReadOnly = true;
                    this.btnImport.Enabled = false;
                }

                if (this.currentEntity.SAPDataExpenses != null && this.currentEntity.SAPDataExpenses.Count > 0)
                {
                    LoadSAPDataExpensesAndQuantities();

                    this.divSAPDataExpensesAndQuantities.Visible = true;
                }
                else
                {
                    this.divSAPDataExpensesAndQuantities.Visible = false;
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

        }

        private void LoadSAPDataExpensesAndQuantities()
        {
            Dictionary<string, TableRow[]> dictExpensesAndQuantities = new Dictionary<string, TableRow[]>();

            dictExpensesAndQuantities = this.ownerPage.CostCalculationRef.LoadSAPDataExpensesAndQuantities(this.hdnRowMasterKey.Value, this.ownerPage.CallContext);

            this.tblSAPDataExpenses.Rows.AddRange(dictExpensesAndQuantities["Expenses"]);
//            this.tblSAPDataExpensesGroups.Rows.AddRange(dictExpensesAndQuantities["ExpensesTypeGroup"]);
            //this.tblSAPDataQuantities.Rows.AddRange(dictExpensesAndQuantities["Quantities"]);
            //this.tblSAPDataExpensesGroupsByMH.Rows.AddRange(dictExpensesAndQuantities["ExpensesTypeGroupMH"]);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.SAPDataSave, false))
            {
                return;
            }

            
            if (string.IsNullOrEmpty(this.hdnRowMasterKey.Value) || this.hdnRowMasterKey.Value == Constants.INVALID_ID_STRING)
            {
                this.currentEntity = new SAPData();
            }
            else
            {
                this.currentEntity = this.ownerPage.CostCalculationRef.GetSAPDataById(this.hdnRowMasterKey.Value);

                if (this.currentEntity == null)
                {
                    this.ownerPage.CallContext.ResultCode = ETEMEnums.ResultEnum.Error;

                    base.AddMessage(this.lbResultContext, string.Format("Entity `SAPData` not found by ID ({0})!", this.hdnRowMasterKey.Value));

                    return;
                }

                
            }

            this.currentEntity.DateFrom = this.tbxDateFrom.TextAsDateParseExactOrMinValue;
            this.currentEntity.DateTo = this.tbxDateTo.TextAsDateParseExact;

            this.ownerPage.CallContext = this.ownerPage.CostCalculationRef.SAPDataSave(new List<SAPData>() { this.currentEntity }, this.ownerPage.CallContext);

            if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {
                this.hdnRowMasterKey.Value = this.ownerPage.CallContext.EntityID;

                base.AddMessage(this.lbResultContext, this.ownerPage.CallContext.Message);

                this.btnImport.Enabled = true;

                this.currentEntity = this.ownerPage.CostCalculationRef.GetSAPDataById(this.hdnRowMasterKey.Value);

                if (this.currentEntity != null && this.currentEntity.SAPDataExpenses != null && this.currentEntity.SAPDataExpenses.Count > 0)
                {
                    LoadSAPDataExpensesAndQuantities();

                    this.divSAPDataExpensesAndQuantities.Visible = true;
                }
                else
                {
                    this.divSAPDataExpensesAndQuantities.Visible = false;
                }
            }
            else
            {
                if (!ShowErrors(new List<CallContext>() { this.ownerPage.CallContext }))
                {
                    return;
                }
            }

            if (this.ownerPage is SAPDataList)
            {
                ((SAPDataList)this.ownerPage).LoadFilteredList();
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
            string appFolder = Server.MapPath("/" + GeneralPage.GetSettingByCode(ETEMEnums.AppSettings.WebApplicationName).SettingValue);
            string resourcesFolder = GeneralPage.GetSettingByCode(ETEMEnums.AppSettings.ResourcesFolderName).SettingValue;
            string templateFolder = GeneralPage.GetSettingByCode(ETEMEnums.AppSettings.FolderTemplates).SettingValue;
            string templateName = GeneralPage.GetSettingByCode(ETEMEnums.AppSettings.TemplateSAPExpensesAndQuantities).SettingValue;

            string templatePath = string.Empty;
            if (Directory.Exists(appFolder + "\\" + templateFolder))
            {
                templatePath = appFolder + "\\" + templateFolder;
            }
            else if (Directory.Exists(resourcesFolder + "\\" + templateFolder))
            {
                templatePath = resourcesFolder + "\\" + templateFolder;
            }
            else
            {
                this.ownerPage.ShowMSG("Template folder not found!", false);
                return;
            }

            string templateFullName = templatePath + "\\" + templateName;

            DirectoryInfo dirInfoTemplates = new DirectoryInfo(resourcesFolder + "\\" + templateFolder + "\\Downloads");

            if (!Directory.Exists(dirInfoTemplates.FullName))
            {
                Directory.CreateDirectory(dirInfoTemplates.FullName);
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

        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (!this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.SAPDataImport, false))
            {
                return;
            }

            base.RunJavaScriptModalWindow();

            if (this.fuImport.HasFile)
            {
                string selectedFileMimeType = this.fuImport.PostedFile.ContentType;

                string excelMimeType = BaseHelper.GetMimeType(Constants.FILE_XLSX_EXTENSION);

                if (!string.Equals(selectedFileMimeType, excelMimeType, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.ownerPage.ShowMSG("Selected file is in incorrect format, it must be Excel-2007 or newer version!", false);
                    return;
                }

                string fileFullName = string.Empty;
                string folderPath = string.Empty;

                folderPath = GeneralPage.GetSettingByCode(ETEMModel.Helpers.ETEMEnums.AppSettings.ResourcesFolderName).SettingValue;

                folderPath += "\\CostCalculation\\SAPData\\Import\\" + DateTime.Today.Year + "\\";

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                fileFullName = folderPath + "\\Import_SAP_ExpensesAndQuantities_" +
                                               DateTime.Now.ToString(Constants.DATE_PATTERN_FOR_FILE_SUFFIX) +
                                               Constants.FILE_XLSX_EXTENSION;

                this.fuImport.PostedFile.SaveAs(fileFullName);

                this.ownerPage.CallContext = this.ownerPage.CostCalculationRef.ImportSAPDataExpensesAndQuantities(fileFullName, this.hdnRowMasterKey.Value, this.ownerPage.CallContext);

                if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
                {
                    base.AddMessage(this.lbResultContext, this.ownerPage.CallContext.Message);

                    this.CostCalculationRef.CalculateCostCentersTotal(this.hdnRowMasterKey.Value, this.ownerPage.CallContext);

                    this.currentEntity = this.ownerPage.CostCalculationRef.GetSAPDataById(this.hdnRowMasterKey.Value);

                    if (this.currentEntity.SAPDataExpenses != null && this.currentEntity.SAPDataExpenses.Count > 0)
                    {
                        LoadSAPDataExpensesAndQuantities();

                        this.divSAPDataExpensesAndQuantities.Visible = true;
                    }
                    else
                    {
                        this.divSAPDataExpensesAndQuantities.Visible = false;
                    }
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