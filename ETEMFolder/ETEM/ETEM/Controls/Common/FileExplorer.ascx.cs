using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEMModel.Helpers;
using System.IO;
using ETEM.Freamwork;

namespace ETEM.Controls.Common
{
    public partial class FileExplorer : BaseUserControl
    {
        public enum UploadedFilesNumberEnum
        {
            OneFile,
            ManyFiles
        }

        public UploadedFilesNumberEnum UploadedFilesNumber
        {
            get;
            set;
        }

        private string path = "";
        //private string currentDirectoryPath;
        private bool canDeleteFiles;
        private bool canViewCbxlink;
        private bool canViewToArchive;
        private bool isChangeSessionFilePath;

        public bool CanViewFolderDeleted { get; set; }
        public bool CanViewArchiveDeleted { get; set; }

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public bool CanDeleteFiles
        {
            get
            {
                return canDeleteFiles;
            }
            set
            {
                canDeleteFiles = value;
            }
        }


        private BasicPage OwnerPage
        {
            get { return (BasicPage)this.Page; }
        }

        //Свойството показва дали при смяна на директории да се сменя и стойноста на ключа в сесията - FILE_UPLOAD_PATH;
        //Това ни помага при Upload_на файл за да можем да го Upload_ване в различни директории
        public bool IsChangeSessionFilePath
        {
            get
            {
                return isChangeSessionFilePath;
            }
            set
            {
                isChangeSessionFilePath = value;
            }
        }

        public bool CanViewCbxlink
        {
            get
            {
                return canViewCbxlink;
            }
            set
            {
                canViewCbxlink = value;
            }
        }

        public bool CanViewToArchive
        {
            get { return canViewToArchive; }
            set { canViewToArchive = value; }
        }

        public string CurrentDirectoryPath
        {
            get
            {
                return ((this.ViewState["currentDirectoryPath"] != null) ? ViewState["currentDirectoryPath"].ToString() : "");
            }
            set
            {
                ViewState.Add("currentDirectoryPath", value);
                if (IsChangeSessionFilePath)
                    this.Session.Add("FILE_UPLOAD_PATH", value);
            }
        }

        private string rootDir = "";

        public string RootDir
        {
            get { return rootDir; }
            set { rootDir = value; }
        }

        public string HdnRootDirectoryValue
        {
            get { return this.hdnRoorDirectory.Value; }
            set { this.hdnRoorDirectory.Value = value; }
        }

        public string HdnParentDirectoryValue
        {
            get { return this.hdnParentDirectory.Value; }
            set
            {
                this.hdnParentDirectory.Value = value;
                //FormLoad(hdnParentDirectory.Value);
            }
        }

        public int IdObject
        {
            get { return (!string.IsNullOrEmpty(this.hdnIdObject.Value) ? Convert.ToInt32(this.hdnIdObject.Value) : Constants.INVALID_ID); }
            set { this.hdnIdObject.Value = value.ToString(); }
        }

        public HiddenField HdnReloadFileExplorer
        {
            get
            {
                return this.hdnReloadFiles;
            }
        }

        public Panel PnlExplorer
        {
            get { return this.pnlExplorer; }
        }

        public bool PnlExplorerVisible
        {
            get { return this.pnlExplorer.Visible; }
            set { this.pnlExplorer.Visible = value; }
        }

        public GridView GvExplorer
        {
            get { return this.gvExplorer; }
        }

        public TextBox TbxCaption
        {
            get { return this.tbxCaption; }
        }

        public bool TbxCaptionVisible
        {
            get { return this.tbxCaption.Visible; }
            set { this.tbxCaption.Visible = value; }
        }

        public Label LbCaption
        {
            get { return this.lbCaption; }
        }

        public bool LbCaptionVisible
        {
            get { return this.lbCaption.Visible; }
            set { this.lbCaption.Visible = value; }
        }

        public bool ChbxIsOneFileToUploadVisible
        {
            get { return this.chbxIsOneFileToUpload.Visible; }
            set { this.chbxIsOneFileToUpload.Visible = value; }
        }

        public bool ChbxIsOneFileToUploadChecked
        {
            get { return this.chbxIsOneFileToUpload.Checked; }
            set { this.chbxIsOneFileToUpload.Checked = value; }
        }

        public Button BtnShowUploadedFiles
        {
            get { return this.btnShowUploadedFiles; }
        }

        public bool BtnShowUploadedFilesVisible
        {
            get { return this.btnShowUploadedFiles.Visible; }
            set { this.btnShowUploadedFiles.Visible = value; }
        }

        public bool BtnShowUploadedFilesEnabled
        {
            get { return this.btnShowUploadedFiles.Enabled; }
            set { this.btnShowUploadedFiles.Enabled = value; }
        }

        public string BtnShowUploadedFilesCommandArgument
        {
            get { return this.btnShowUploadedFiles.CommandArgument; }
            set { this.btnShowUploadedFiles.CommandArgument = value; }
        }

        public Button BtnUploadFile
        {
            get { return this.btnUploadfile; }
        }

        public bool BtnUploadFileVisible
        {
            get { return this.btnUploadfile.Visible; }
            set { this.btnUploadfile.Visible = value; }
        }

        public bool BtnUploadFileEnabled
        {
            get { return this.btnUploadfile.Enabled; }
            set { this.btnUploadfile.Enabled = value; }
        }

        public string BtnUploadFileCommandArgument
        {
            get { return this.btnUploadfile.CommandArgument; }
            set { this.btnUploadfile.CommandArgument = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.HdnRootDirectoryValue))
                {
                    //                    CurrentDirectoryPath = this.HdnRootDirectoryValue;
                    FormLoad(this.HdnRootDirectoryValue);
                }
            }
        }

        public void FormLoad(string directoryPath)
        {
            //            CurrentDirectoryPath = directoryPath;
            IList<Directories> res = new List<Directories>();
            Directories dirs;

            try
            {
                if (string.IsNullOrEmpty(directoryPath))
                {
                    this.gvExplorer.DataSource = null;
                    this.gvExplorer.DataBind();

                    return;
                }

                DirectoryInfo directory = new DirectoryInfo(directoryPath);

                if (!directory.Exists)
                {
                    this.gvExplorer.DataSource = null;
                    this.gvExplorer.DataBind();

                    return;
                }

                DirectoryInfo[] directories = directory.GetDirectories();

                foreach (var item in directories)
                {
                    if (!this.CanViewFolderDeleted && item.Name == "Deleted")
                    {
                        continue;
                    }

                    dirs = new Directories();

                    dirs.FullName = item.FullName;
                    dirs.Name = item.Name;
                    dirs.Type = "Папка";
                    dirs.LastWriteTime = Convert.ToDateTime(item.LastWriteTime);
                    dirs.IsFile = "false";
                    dirs.ImageSRC = "../Images/Folder.png";
                    //Директории не могат да се трият
                    dirs.DeleteVisible = false;
                    dirs.CbxVisible = false;
                    dirs.ChbxToArchiveVisible = false;
                    res.Add(dirs);
                }

                FileInfo[] files = directory.GetFiles();

                foreach (var item in files)
                {
                    dirs = new Directories();

                    dirs.FullName = item.FullName;
                    dirs.Name = item.Name;
                    dirs.Type = "Файл";
                    dirs.LastWriteTime = Convert.ToDateTime(item.LastWriteTime);
                    dirs.FileLength = (item.Length / 1024).ToString();
                    dirs.IsFile = "true";
                    dirs.ImageSRC = "../Images/FileXPtest.png";
                    dirs.DeleteVisible = (directory.Name.Equals("Deleted") ? false : CanDeleteFiles);
                    dirs.CbxVisible = CanViewCbxlink;
                    dirs.ChbxToArchiveVisible = this.CanViewToArchive;
                    res.Add(dirs);
                }

                if (res.Count != 0)
                {
                    this.gvExplorer.DataSource = res;
                    this.gvExplorer.DataBind();
                }
                else
                {
                    this.gvExplorer.DataSource = null;
                    this.gvExplorer.DataBind();
                }
            }
            catch (Exception ex)
            {
                BasicPage.LogDebug("Грешка при зареждане на списък с прикачени файлове - метод 'FormLoad', форма 'FileExplorer.ascx'!" + ex.ToString());
                BasicPage.LogError("Грешка при зареждане на списък с прикачени файлове - метод 'FormLoad', форма 'FileExplorer.ascx'!" + ex.ToString());
            }
        }

        //protected void lbtnOpenFile_Click(object sender, EventArgs e)
        //{
        //    string repfilename = ((LinkButton)sender).CommandArgument.ToString();
        //    FileStream readRepFile = File.OpenRead(repfilename);
        //    if (readRepFile.Length != 0)
        //    {
        //        byte[] repData = new byte[readRepFile.Length];
        //        readRepFile.Read(repData, 0, (int)readRepFile.Length);
        //        readRepFile.Flush();
        //        readRepFile.Close();
        //        Response.Clear();
        //        Response.AddHeader("Content-Disposition", "attachment;filename=" + Path.GetFileName(repfilename));
        //        //Response.ContentType = "application/excel";
        //        Response.BinaryWrite(repData);
        //        Response.End();
        // }

        protected void imgBtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            string[] info = ((ImageButton)sender).CommandArgument.ToString().Split('|');

            try
            {
                FileInfo fi = new FileInfo(info[0].ToString());

                DirectoryInfo dirInfoToDeletedFiles = new DirectoryInfo(this.HdnRootDirectoryValue + "\\" + "Deleted");
                if (!dirInfoToDeletedFiles.Exists)
                {
                    Directory.CreateDirectory(dirInfoToDeletedFiles.FullName);
                }
                // ****

                string fileNameToDelete = this.AdminClientRef.GetFileVersion(fi.Name);

                fi.MoveTo(dirInfoToDeletedFiles.FullName + "\\" + fileNameToDelete);

                //                fi.Delete();
                FormLoad(this.HdnRootDirectoryValue);

                /*ScriptManager.RegisterStartupScript(
                                          this.Page,
                                          this.Page.GetType(),
                                          "ReloadForm",
                                          "<script type=\"text/javascript\" language=\"javascript\">window.opener.reloadForm();</script>",
                                          false
                                          );
                 */

                if (Parent != null)
                {
                    if ((Parent as SMCFileUploder) != null)
                    {
                        (Parent as SMCFileUploder).CustomFolder = dirInfoToDeletedFiles.FullName.Split('\\')[3].ToString();
                        (Parent as SMCFileUploder).UserControlLoad();
                    }
                }

            }
            catch (Exception ex)
            {
                BasicPage.LogDebug("Грешка при изтриване на файл от списък с прикачени файлове - метод 'imgBtnDelete_Click', форма 'FileExplorer.ascx'!" + ex.ToString());
                BaseHelper.LogToMail("Грешка при изтриване на файл от списък с прикачени файлове - метод 'imgBtnDelete_Click', форма 'FileExplorer.ascx'!" + ex.ToString());
            }
        }

        protected void lbtnOpenDirectory_Click(object sender, EventArgs e)
        {
            string[] info = ((LinkButton)sender).CommandArgument.ToString().Split('|');

            if (info[1] == "false")
            {
                string newDirPath = info[0];

                FormLoad(newDirPath);

                string parentDirPath = "";
                string[] dirs = newDirPath.Split('\\');

                for (int i = 0; i < dirs.Count(); i++)
                {
                    if (i == 0)
                    {
                        parentDirPath += dirs[i];
                    }
                    else
                    {
                        parentDirPath += "\\" + dirs[i];
                    }
                }

                this.hdnParentDirectory.Value = parentDirPath;
            }
            else if (info[1] == "true")
            {

            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            string parentDirPath = "";

            string[] dirs = this.hdnParentDirectory.Value.Split('\\');

            for (int i = 0; i < dirs.Count() - 1; i++)
            {
                if (i == 0)
                {
                    parentDirPath += dirs[i];
                }
                else
                {
                    parentDirPath += "\\" + dirs[i];
                }
            }

            if (this.HdnRootDirectoryValue.Split('\\').Count() != dirs.Count())
            {
                this.hdnParentDirectory.Value = parentDirPath;

                FormLoad(this.HdnParentDirectoryValue);
            }
        }

        public class Directories
        {
            public string Name { get; set; }
            public string FullName { get; set; }
            public string Type { get; set; }
            public DateTime LastWriteTime { get; set; }
            public string FileLength { get; set; }
            public string IsFile { get; set; }
            public string ImageSRC { get; set; }
            public bool DeleteVisible { get; set; } //Дали да се вижда бутона за изтриване
            public bool CbxVisible { get; set; }   //Дали да се вижда отметката за линк
            public bool ChbxToArchiveVisible { get; set; }   //Дали да се вижда отметката за архив
        }

        protected void gvExplorer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            LinkButton btn = (LinkButton)e.Row.FindControl("lbtnOpenDirectory");
            Label lbPic = (Label)e.Row.FindControl("lbPicture");

            if (btn != null)
            {
                string[] info = btn.CommandArgument.ToString().Split('|');

                if (info[1] == "true")
                {
                    string queryParams = "";
                    queryParams = "FilePath=" + info[0].Replace("\\", "\\\\");
                    btn.Attributes.Add("onclick", "window.open('../Share/DownloadFile.aspx?" + BaseHelper.Encrypt(queryParams) + "');");
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //ако има точка в разширението - файл, ако няма - директория
                    if (info[0].Remove(0, info[0].Length - 5).Contains("."))
                    {
                        lbPic.Attributes.Add("class", "fi-page size-24");
                    }
                    else
                    {
                        lbPic.Attributes.Add("class", "fi-folder size-24");
                    }

                    lbPic.Attributes.Add("style", "color:Red");
                }

            }
        }

        protected void btnRefreshFileExplorer_Click(object sender, EventArgs e)
        {
            FormLoad(this.HdnRootDirectoryValue);
        }

        protected void hdnReloadFiles_ValueChanged(object sender, EventArgs e)
        {
            FormLoad(this.HdnRootDirectoryValue);
        }

        protected void btnShowUploadedFiles_Click(object sender, EventArgs e)
        {
            if (!this.pnlExplorer.Visible)
            {
                this.pnlExplorer.Visible = true;

                FormLoad(this.HdnRootDirectoryValue);
            }
            else
            {
                this.pnlExplorer.Visible = false;
            }
        }
    }
}