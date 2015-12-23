using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using System.IO;
using ETEMModel.Helpers;

namespace ETEM.Admin
{
    public partial class DownloadLogFile : BasicPage
    {
        public static FormResources formResource = new FormResources
        {
            Module = Constants.MODULE_SUPPORT_HISTORY,
            PageFullName = Constants.UMS_ADMIN_DOWNLOADLOGFILE,
            PagePath = "../Admin/DownloadLogFile.aspx"

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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CheckUserActionPermission(ETEMEnums.SecuritySettings.ShowDownloadLogFile, true);


                string resourcesFolderName = BasicPage.GetSettingByCode(ETEMModel.Helpers.ETEMEnums.AppSettings.ResourcesFolderName).SettingValue;

                DirectoryInfo directory = new DirectoryInfo(resourcesFolderName + "\\log");
                //FileInfo logFile = directory.GetFiles().OrderByDescending(s => s.LastWriteTime).FirstOrDefault();

                string initialZipFileName = BaseHelper.ZipFolder(directory);



                this.OpenPageForDownloadFile("../Share/DownloadFile.aspx", "FilePath=" + initialZipFileName);
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в Page_Load " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }
    }
}