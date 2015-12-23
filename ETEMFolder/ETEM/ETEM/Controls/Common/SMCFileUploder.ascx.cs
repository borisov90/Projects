using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEMModel.Helpers;
using System.IO;
using ETEM.Freamwork;

using ETEMModel.Models;

namespace ETEM.Controls.Common
{
    public partial class SMCFileUploder : BaseUserControl
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
        public new string  UserControlName { get; set; }
        public string CurrentObjectName { get; set; }

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

        public string AllowedFileExtentions
        {
            get { return this.hdnAllowedFileExtentions.Value; }
            set { this.hdnAllowedFileExtentions.Value = value; }
        }

        public bool? IsForDiplomaImages
        {
            get { return !string.IsNullOrEmpty(this.hdnIsForDiplomaImages.Value) ? bool.Parse(this.hdnIsForDiplomaImages.Value) : false; }
            set { this.hdnIsForDiplomaImages.Value = value != null && value.ToString() != string.Empty ? value.ToString() : (false).ToString(); }
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

        /*public Button BtnShowUploadedFiles
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
         */
        public string CustomFolder { get; set; }

        public Button BtnUploadFile
        {
            get { return this.btnUploadfile; }
        }

        public bool BtnUploadFileVisible
        {
            get { return this.btnUploadfile.Visible; }
            set { this.btnUploadfile.Visible = value; }
        }

        public bool BtnAddFileVisible
        {
            get { return this.btnAdd.Visible; }
            set { this.btnAdd.Visible = value; }
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

        public override void UserControlLoad()
        {
            if (string.IsNullOrEmpty(this.HdnRootDirectoryValue) || !string.IsNullOrEmpty(this.CustomFolder))
            {
                this.hdnRoorDirectory.Value = CreateResourcesPath();
            }



            FormLoad(this.HdnRootDirectoryValue);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    if (!string.IsNullOrEmpty(this.HdnRootDirectoryValue))
            //    {

            //        FormLoad(this.HdnRootDirectoryValue);
            //    }
            //}
        }

        public void FormLoad(string directoryPath)
        {
            this.hdnInitFilePath.Value = directoryPath;

            this.fexUploadFiles.HdnRootDirectoryValue = this.hdnInitFilePath.Value;
            this.fexUploadFiles.FormLoad(this.fexUploadFiles.HdnRootDirectoryValue);


            //            CurrentDirectoryPath = directoryPath;
            IList<Directories> res = new List<Directories>();


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

                SelectDataFromDirectory(res, directory);

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
                GeneralPage.LogDebug("Грешка при зареждане на списък с прикачени файлове - метод 'FormLoad', форма 'FileExplorer.ascx'!" + ex.ToString());
                GeneralPage.LogError("Грешка при зареждане на списък с прикачени файлове - метод 'FormLoad', форма 'FileExplorer.ascx'!" + ex.ToString());

            }
        }

        private void SelectDataFromDirectory(IList<Directories> res, DirectoryInfo directory)
        {
            Directories dirs;
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
                //dirs.LastWriteTime = item.LastWriteTime.ToString();
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
                //dirs.LastWriteTime = item.LastWriteTime.ToString();
                dirs.LastWriteTime = Convert.ToDateTime(item.LastWriteTime);
                dirs.FileLength = (item.Length / 1024).ToString();
                dirs.IsFile = "true";
                dirs.ImageSRC = "../Images/FileXPtest.png";
                dirs.DeleteVisible = (directory.Name.Equals("Deleted") ? false : CanDeleteFiles);
                dirs.CbxVisible = CanViewCbxlink;
                dirs.ChbxToArchiveVisible = this.CanViewToArchive;
                res.Add(dirs);
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
                fi.MoveTo(dirInfoToDeletedFiles.FullName + "\\" + fi.Name);

                //                fi.Delete();
                FormLoad(this.HdnRootDirectoryValue);

                UserControlLoad();
                /*ScriptManager.RegisterStartupScript(
                                          this.Page,
                                          this.Page.GetType(),
                                          "ReloadForm",
                                          "<script type=\"text/javascript\" language=\"javascript\">window.opener.reloadForm();</script>",
                                          false
                                          );
                 */
            }
            catch (Exception ex)
            {
                GeneralPage.LogDebug("Грешка при изтриване на файл от списък с прикачени файлове - метод 'imgBtnDelete_Click', форма 'FileExplorer.ascx'!" + ex.ToString());
                GeneralPage.LogError("Грешка при изтриване на файл от списък с прикачени файлове - метод 'imgBtnDelete_Click', форма 'FileExplorer.ascx'!" + ex.ToString());
            }

            ReloadCallingControl();
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
                //string repfilename = info[0];
                //FileStream readRepFile = File.OpenRead(repfilename);
                //if (readRepFile.Length != 0)
                //{
                //    byte[] repData = new byte[readRepFile.Length];
                //    readRepFile.Read(repData, 0, (int)readRepFile.Length);
                //    readRepFile.Flush();
                //    readRepFile.Close();

                //    Response.Clear();
                //    Response.AddHeader("Content-Disposition", "attachment;filename=" + Path.GetFileName(repfilename));
                //    //Response.ContentType = "application/excel";
                //    Response.BinaryWrite(repData);
                //    Response.End();
                //}
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
            // public string LastWriteTime { get; set; }
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

            /*
            RadioButton rbtn = (RadioButton)e.Row.FindControl("rbtnIsFileLink");
            if (rbtn != null)
                rbtn.GroupName = "Link1";
             * */
        }

        protected void btnRefreshFileExplorer_Click(object sender, EventArgs e)
        {
            FormLoad(this.HdnRootDirectoryValue);
        }

        protected void hdnReloadFiles_ValueChanged(object sender, EventArgs e)
        {
            FormLoad(this.HdnRootDirectoryValue);
        }

        /*protected void btnShowUploadedFiles_Click(object sender, EventArgs e)
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
        }*/

        protected void lnkBtnUpLavel_Click(object sender, EventArgs e)
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

        protected void btnUploadfile_Click(object sender, EventArgs e)
        {
            //string path = BaseHelper.Encrypt(@"UploadPath=C:\Resources");
            //Response.Redirect("~/Share/MultiFilesUpload.aspx?" + path);

            if (!pnlUploadFile.Visible)
            {
                pnlUploadFile.Focus();
                pnlUploadFile.Visible = true;

                //FormLoad(this.hdnRoorDirectory.Value);
            }

        }

        #region New Upload File

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.fuAddFileToArchive.HasFile)
            {

                try
                {
                    string trimmedFileName = this.fuAddFileToArchive.FileName.Split('.')[0].Trim() + "." + this.fuAddFileToArchive.FileName.Split('.')[1].Trim();

                    string uploadPath = this.hdnInitFilePath.Value;
                    CheckBox chbxFileToArchive = new CheckBox();
                    LinkButton lbtnOpenDirectory = new LinkButton();

                    foreach (GridViewRow row in this.fexUploadFiles.GvExplorer.Rows)
                    {
                        chbxFileToArchive = row.FindControl("chbxFileToArchive") as CheckBox;

                        if (chbxFileToArchive.Checked)
                        {
                            lbtnOpenDirectory = row.FindControl("lbtnOpenDirectory") as LinkButton;
                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(lbtnOpenDirectory.Text))
                    {
                        DirectoryInfo dirInfoArchive = new DirectoryInfo(uploadPath + "\\" + "Archive");

                        if (!dirInfoArchive.Exists)
                        {
                            dirInfoArchive.Create();
                        }

                        FileInfo fileInfoArchive = new FileInfo(uploadPath + "\\" + lbtnOpenDirectory.Text);

                        string fileNameToArchive = this.AdminClientRef.GetFileVersion(lbtnOpenDirectory.Text);

                        fileInfoArchive.MoveTo(dirInfoArchive.FullName + "\\" + fileNameToArchive);
                    }

                    string uploadFileFullName = uploadPath + "\\" + trimmedFileName;

                    FileInfo fileInfoNew = new FileInfo(uploadFileFullName);




                    if (fileInfoNew.Exists)
                    {
                        ScriptManager.RegisterStartupScript(
                                                            this.Page,
                                                            this.GetType(),
                                                           "Error_Has_Same_File",
                                                           "<script type=\"text/javascript\" language=\"javascript\">alert('" + "Съществува прикачен файл със същото име!" + "');</script>",
                                                           false
                                                           );
                    }

                    //filter ententions by control proporty
                    bool allowFileExtentiton = true;
                    List<string> listAllowedExtentions = new List<string>();
                    string extention = fileInfoNew.Extension.ToLower();

                    //check if there is set filter for the file Extentions
                    if (!string.IsNullOrEmpty(AllowedFileExtentions))
                    {

                        listAllowedExtentions = AllowedFileExtentions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                        if (!listAllowedExtentions.Any(s => s == extention))
                        {
                            allowFileExtentiton = false;
                        }

                    }


                    //if the control is user for Diploma images upload.The images have to have specific requierments
                    bool meetDimentionRequierments = true;
                    if (IsForDiplomaImages.HasValue && IsForDiplomaImages.Value)
                    {
                        //if the files are jpg i can check there dimention
                        if (extention == Constants.FILE_JPG_EXTENSION)
                        {
                            //create temp directory to save the file in it, to get it as an image after that 
                            string tempDirectoryPath = uploadPath + Constants.DIRECTORY_SLASH + "temp" + Constants.DIRECTORY_SLASH;
                            DirectoryInfo tempDirInfo = Directory.CreateDirectory(tempDirectoryPath);
                            string tempFileFullPath = tempDirInfo.FullName + trimmedFileName;

                            this.fuAddFileToArchive.SaveAs(tempFileFullPath);

                            using (var img = System.Drawing.Image.FromFile(tempFileFullPath))
                            {
                                float horisontalResolution = img.HorizontalResolution;//gives pixels per inch
                                float verticalResolution = img.VerticalResolution;
                                var height = img.Height;
                                var width = img.Width;

                                if (height < Constants.EXPORT_MIN_DIPLOMA_IMAGE_HEIGHT || width < Constants.EXPORT_MIN_DIPLOMA_IMAGE_WIDTH
                                    || horisontalResolution < Constants.EXPORT_MIN_DIPLOMA_IMAGE_PIXEL_PER_INCH
                                    || verticalResolution < Constants.EXPORT_MIN_DIPLOMA_IMAGE_PIXEL_PER_INCH)
                                {
                                    meetDimentionRequierments = false;
                                }
                            }

                            //delete the temp directory
                            Directory.Delete(tempDirectoryPath, true);
                        }
                        //if the file is pdf i can
                    }

                    if (allowFileExtentiton && meetDimentionRequierments)
                    {

                        //here is actual save of the file on the server
                        DirectoryInfo dirInfoUploadPath = Directory.CreateDirectory(uploadPath);

                        this.fuAddFileToArchive.SaveAs(uploadFileFullName);

                        this.fexUploadFiles.FormLoad(uploadPath);
                    }
                    else if (allowFileExtentiton == false)
                    {

                        ScriptManager.RegisterStartupScript(
                                                           this.Page,
                                                           this.GetType(),
                                                          "Error_Not_Extentions_File",
                                                          "<script type=\"text/javascript\" language=\"javascript\">alert(' \"Позволени са само файлове с разширение: " + AllowedFileExtentions + "');</script>",
                                                          false
                                                          );
                    }
                    else if (meetDimentionRequierments == false)
                    {
                        ScriptManager.RegisterStartupScript(
                                                           this.Page,
                                                           this.GetType(),
                                                          "Error_Not_Extentions_File",
                                                          "<script type=\"text/javascript\" language=\"javascript\">alert('Минимален размер: 2 мегапиксела (1600х1200).Документите се сканират с резолюция 72 dpi.');</script>",
                                                          false
                                                          );
                    }


                    UserControlLoad();






                    //Добавя качения файл към SearchEngine Index
                    //SearchEngine.AddFileToIndex(uploadPath + "\\" + fuAddFileToArchive.FileName);
                    //SearchInstance.AddFile(uploadFileFullName);


                    /*ScriptManager.RegisterStartupScript(
                                          this.Page,
                                          this.Page.GetType(),
                                          "ReloadForm",
                                          "<script type=\"text/javascript\" language=\"javascript\">window.opener.reloadForm();</script>",
                                          false
                                          );
                     */
                }
                catch (Exception ex)
                {
                    BasicPage.LogDebug("Грешка при прикачване на файл към обект или документ - метод 'btnAdd_Click', форма 'UploadFile.aspx'!" + ex.ToString());
                    BaseHelper.LogToMail("Грешка при прикачване на файл към обект или документ - метод 'btnAdd_Click', форма 'UploadFile.aspx'!" + ex.ToString());
                }
            }

            ReloadCallingControl();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.pnlUploadFile.Visible = false;
            //BtnUploadFileEnabled        = true;
            //Response.Write("<script type=\"text/javascript\" language=\"javascript\">window.close();</script>");
        }

        protected void btnAddToArchive_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.hdnInitFilePath.Value))
            {
                try
                {
                    string uploadPath = this.hdnInitFilePath.Value;

                    DirectoryInfo dirInfoArchive = null;
                    FileInfo fileInfoArchive = null;
                    CheckBox chbxFileToArchive = new CheckBox();
                    LinkButton lbtnOpenDirectory = new LinkButton();

                    foreach (GridViewRow row in this.fexUploadFiles.GvExplorer.Rows)
                    {
                        chbxFileToArchive = row.FindControl("chbxFileToArchive") as CheckBox;

                        if (chbxFileToArchive.Checked)
                        {
                            lbtnOpenDirectory = row.FindControl("lbtnOpenDirectory") as LinkButton;

                            if (!string.IsNullOrEmpty(lbtnOpenDirectory.Text))
                            {
                                dirInfoArchive = new DirectoryInfo(uploadPath + "\\" + "Archive");

                                if (!dirInfoArchive.Exists)
                                {
                                    dirInfoArchive.Create();
                                }

                                fileInfoArchive = new FileInfo(uploadPath + "\\" + lbtnOpenDirectory.Text);
                                string fileNameToArchive = this.AdminClientRef.GetFileVersion(lbtnOpenDirectory.Text);

                                fileInfoArchive.MoveTo(dirInfoArchive.FullName + "\\" + fileNameToArchive);
                            }
                        }
                    }

                    this.fexUploadFiles.FormLoad(uploadPath);

                    UserControlLoad();
                    /*ScriptManager.RegisterStartupScript(
                                                          this.Page,
                                                          this.Page.GetType(),
                                                          "ReloadForm",
                                                          "<script type=\"text/javascript\" language=\"javascript\">window.opener.reloadForm();</script>",
                                                          false
                                                          );
                     */
                }
                catch (Exception ex)
                {
                    BasicPage.LogDebug("Грешка при преместване на избрани файлове към архив (папка 'Archive') - метод 'btnAddToArchive_Click', форма 'UploadFile.aspx'!" + ex.ToString());
                    BaseHelper.LogToMail("Грешка при преместване на избрани файлове към архив (папка 'Archive') - метод 'btnAddToArchive_Click', форма 'UploadFile.aspx'!" + ex.ToString());
                }
            }
        }

        protected string CreateResourcesPath()
        {
            //създава и отваря ресурсна папка с име - PersonName_idPerson
            //string userNameFolder = BaseHelper.ConvertCyrToLatin(this.ownerPage.UserProps.UserName) + "_" + this.ownerPage.UserProps.IdUser;

            if (string.IsNullOrEmpty(CustomFolder))
            {
                throw new Exception("Моля, задайте CustomFolder за контролата " + this.ID);
            }

            string tmpCustumFolder = BaseHelper.ConvertCyrToLatin(CustomFolder).Trim().Replace(' ', '_').Replace('.', '_').Replace('\r', '_');

            //start
            string resourcesFolderName = GeneralPage.GetSettingByCode(ETEMEnums.AppSettings.ResourcesFolderName).SettingValue + @"\" + UserControlName + @"\";

            //ID с което започва папката за импорт. Пример C:\Resources_UMS\IncomingDocumentData\35_RegNumber_1
            string idStartFolder = tmpCustumFolder.Split('_')[0].ToString();

            DirectoryInfo directory = new DirectoryInfo(resourcesFolderName);
            if (directory.Exists)
            {
                DirectoryInfo[] directories = directory.GetDirectories();

                foreach (var item in directories)
                {
                    if (item.Name.StartsWith(idStartFolder + "_"))
                    {
                        tmpCustumFolder = item.Name;
                    }
                }
            }
            //end

            resourcesFolderName += tmpCustumFolder + @"\" + this.ID;

            return resourcesFolderName;
        }

        #endregion

        internal void ClearGrid()
        {
            this.gvExplorer.DataSource = null;
            this.gvExplorer.DataBind();
        }

        protected void ReloadCallingControl()
        {
            
        }


    }
}