using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Globalization;
using ETEMModel.Helpers;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using ETEMModel.Models;
using System.Web.UI.HtmlControls;
using ETEMModel.Models.DataView;
using System.Web.SessionState;
using AjaxControlToolkit;
using ETEM.Controls.Common;
using NLog;
using ETEM.Admin;
using ETEMModel.Models.DataView.Admin;
using System.Reflection;
using System.Collections;
using ETEM.Share;

namespace ETEM.Freamwork
{
    public class BasicPage : GeneralPage
    {
        protected ETEMModel.Models.Module currentModuleObject;

        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.CurrentPageFullName() != Share.Login.formResource.PageFullName )
            {
                if (this.UserProps != null && this.UserProps.Roles.Any(a => a.Name == "VIEW"))
                {
                    HideButtonControls(Page);

                    //can be refactor to hide only delte linkbutons in needed
                    HideLinkButtonControls(Page);
                    DisableDropDownListControls(Page);
                    DisableTextBoxControls(Page);

                }
            }

            base.OnPreRenderComplete(e);
        }

        public virtual string GridViewSortExpression
        {
            get
            {
                if (ViewState["SortExpression"] != null)
                {
                    return ViewState["SortExpression"].ToString();
                }
                else
                {
                    return Constants.INVALID_ID_STRING;
                }
            }
            set
            {
                ViewState["SortExpression"] = value;
            }
        }

        public virtual string GridViewSortDirection
        {
            get
            {
                if (ViewState["SortDirection"] != null)
                {
                    return ViewState["SortDirection"].ToString();
                }
                else
                {
                    return Constants.SORTING_ASC;
                }
            }
            set
            {
                ViewState["SortDirection"] = value;
            }
        }

        protected Control FindControlById(Control holder, string idControl)
        {
            if (holder.ID == idControl)
            {
                return holder;
            }
            if (holder.HasControls())
            {
                Control temp;
                foreach (Control subcontrol in holder.Controls)
                {
                    temp = FindControlById(subcontrol, idControl);
                    if (temp != null)
                    {
                        return temp;
                    }
                }
            }
            return null;
        }

        public virtual int? NewPageIndex
        {
            get
            {
                if (ViewState["NewPageIndex"] != null)
                {
                    return Int32.Parse(ViewState["NewPageIndex"].ToString());
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ViewState["NewPageIndex"] = value;
            }
        }

        protected void cbxSelectOrDeselectAllGridItems_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbx = sender as CheckBox;
            GridView grid = BaseHelper.FindFirstParentOfSpecificType(cbx, typeof(GridView)) as GridView;
            BaseHelper.SelectOrDeselectAllEnabledGridCheckBox(grid, cbx.Checked, "cbxGridCheckbox");
        }

        protected void btnCancelParentPanel_OnClick(object sender, EventArgs e)
        {
            Panel parentPnl = BaseHelper.FindFirstParentOfSpecificType(sender as Control, new Panel().GetType()) as Panel;
            if (parentPnl != null)
            {
                parentPnl.Visible = false;
            }
            else
            {
                throw new ArgumentException("Cound not find the parent panel");
            }
        }

        protected void SetActiveTab(int tabIndex, string tabContainerName)
        {
            TabContainer container = FindControlById(Page, tabContainerName) as TabContainer;
            container.ActiveTabIndex = tabIndex;
        }

        protected void SetSortDirection()
        {
            if (string.IsNullOrEmpty(this.GridViewSortDirection))
            {
                this.GridViewSortDirection = Constants.SORTING_ASC;
            }
            else if (this.GridViewSortDirection.Equals(Constants.SORTING_ASC))
            {
                this.GridViewSortDirection = Constants.SORTING_DESC;
            }
            else if (this.GridViewSortDirection.Equals(Constants.SORTING_DESC))
            {
                this.GridViewSortDirection = Constants.SORTING_ASC;
            }
        }

        protected void SetSortDirectionDescending()
        {
            if (string.IsNullOrEmpty(this.GridViewSortDirection))
            {
                this.GridViewSortDirection = Constants.SORTING_DESC;
            }
            else if (this.GridViewSortDirection.Equals(Constants.SORTING_ASC))
            {
                this.GridViewSortDirection = Constants.SORTING_DESC;
            }
            else if (this.GridViewSortDirection.Equals(Constants.SORTING_DESC))
            {
                this.GridViewSortDirection = Constants.SORTING_ASC;
            }
        }

        public virtual void AlphabetClick(string alpha)
        {
            throw new Exception("Айде, вземи да го имплементираш този метод!!");
        }

        public virtual void MultipleSortingClick(string sortExpression)
        {
            throw new Exception("Айде, вземи да го имплементираш този метод!!");
        }



        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);




            ///Страници, които не изискват login
            if (
                this.UserProps == null &&

                this.CurrentPageFullName() != Share.Login.formResource.PageFullName

                )
            {
                Response.Redirect(Share.Login.formResource.PagePath);
            }


            UserProps userProps = GetOnlineUsers().Where(s => s.SessionID == this.Session.SessionID).FirstOrDefault() as UserProps;

            if (userProps != null && userProps.IsKilled)
            {

                Session.RemoveAll();
                Session.Abandon();
                Response.Redirect(Share.Login.formResource.PagePath);
            }


            if (userProps != null && CurrentPagePath() != OnlineUsersList.formResource.PagePath)
            {
                userProps.LastPageName = CurrentPagePath();

                ModuleDataView module = DictionaryModules.Where(m => m.Key == CurrentModule()).FirstOrDefault().Value;

                MenuNodeDataView node = null;

                if (FormContext.QueryString["Node"] != null)
                {
                    node = DictionaryMenuNodes.Where(m => m.Key == FormContext.QueryString["Node"].ToString()).FirstOrDefault().Value;
                }

                if (module != null)
                {
                    userProps.LastModuleName = module.ModuleName;
                }

                if (node != null)
                {
                    userProps.LastPageName = node.name;
                }

            }

            #region filter by ip



            this.currentModuleObject = this.AdminClientRef.GetModuleBySysName(this.CurrentModule());

            if (this.CurrentPageFullName() != Share.Login.formResource.PageFullName &&
                this.currentModuleObject != null &&
                this.currentModuleObject.NeedCheck && !Request.UserHostAddress.Equals("::1"))
            {


                AllowIP allowIP = this.AdminClientRef.GetEntityByIPAddress(Request.UserHostAddress);

                if (allowIP == null || !allowIP.Allow)
                {

                    Response.Redirect("~/UI/InternalPageInfo.aspx");

                }
            }

            #endregion

            #region filter by modules

            List<string> listPermittedModules = new List<string>()
            {
               Constants.MODULE_NOMENCLATURES,
               Constants.MODULE_SUPPORT_HISTORY, 
               Constants.MODULE_SETTINGS,
               Constants.MODULE_PERMISSION, 
               Constants.MODULE_INOUTDOCUMENT,
               
            };

            if (this.CurrentPageFullName() != Share.Login.formResource.PageFullName &&
                
                currentModuleObject != null && this.UserProps.Roles.Any(a => a.Name == "VIEW") &&
                 listPermittedModules.Any(s => s == currentModuleObject.ModuleSysName))
            {
                Response.Redirect("~/UI/InternalPageInfo.aspx");
            }


            #endregion


            if (!IsPostBack)
            {
                InitReloadParentPageControl();
            }

            AjaxControlToolkit.ToolkitScriptManager.RegisterClientScriptBlock(this,
                                                                              this.GetType(),
                                                                              "IsNumeric",
                                                                              BaseHelper.JS_SCRIPT_IS_NUMERIC,
                                                                              false);
        }

        private void InitReloadParentPageControl()
        {
            if (!Page.ClientScript.IsClientScriptIncludeRegistered("ReloadParentPageScript"))
            {
                Page.ClientScript.RegisterClientScriptInclude("ReloadParentPageScript", ResolveClientUrl("~/js/reloadParentPage.js"));
            }

            Button btnReloadParentPage = new Button();

            btnReloadParentPage.ID = "btnReloadParentPage";
            btnReloadParentPage.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            btnReloadParentPage.Height = Unit.Pixel(1);
            btnReloadParentPage.Width = Unit.Pixel(1);

            btnReloadParentPage.Style.Add(HtmlTextWriterStyle.Visibility, "hidden");

            btnReloadParentPage.Click += new EventHandler(btnReloadParentPage_Click);

            System.Web.UI.WebControls.ContentPlaceHolder contentPlaceHolder = this.ContentPlaceHolder;
            if (contentPlaceHolder != null)
            {
                if (!contentPlaceHolder.Controls.Contains(btnReloadParentPage))
                {
                    contentPlaceHolder.Controls.Add(btnReloadParentPage);
                }
            }
        }

        void btnReloadParentPage_Click(object sender, EventArgs e)
        {
            ReloadParentPageAction();
        }

        public virtual void ReloadParentPageAction()
        {

        }

        public void ReloadParentPage()
        {
            ScriptManager.RegisterStartupScript(this.Page,
                                                this.Page.GetType(),
                                                "ReloadParentPage",
                                                "<script type=\"text/javascript\" language=\"javascript\">" +
                                                "window.opener.reloadParentPage('" + "btnReloadParentPage" + "');" +
                                                "</script>",
                                                false);
        }

        protected void AddErrorMessage(Control label, string message)
        {
            Label lb = label as Label;
            lb.CssClass = "alert alert-error";
            lb.Text = message;
        }

        protected void AddSuccessMessage(Control label, string message)
        {
            Label lb = label as Label;
            lb.CssClass = "alert alert-success";
            lb.Text = message;
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
        }

        protected List<UserProps> GetOnlineUsers()
        {
            List<UserProps> activeSessions = new List<UserProps>();


            #region OLD Version


            try
            {
                foreach (var ses in GetActiveSessions().ToList())
                {
                    UserProps up = ses["USER_PROPERTIES"] as UserProps;

                    if (up != null)
                    {
                        activeSessions.Add(up);
                    }
                }

                LogDebug("Load GetActiveSessions from CacheInternal");

            }
            catch
            {
                Dictionary<string, HttpSessionState> sessionData =
                   (Dictionary<string, HttpSessionState>)Application[Constants.APPLICATION_ALL_SESSIONS];


                foreach (var session in sessionData)
                {
                    HttpSessionState sessionState = (HttpSessionState)session.Value;

                    if (sessionState["USER_PROPERTIES"] != null)
                    {
                        UserProps userProps = sessionState["USER_PROPERTIES"] as UserProps;
                        if (userProps != null)
                        {
                            activeSessions.Add(userProps);
                        }
                    }
                }

                LogDebug("Load GetActiveSessions from APPLICATION_ALL_SESSIONS");
            }


            #endregion





            return activeSessions;
        }

        public IEnumerable<SessionStateItemCollection> GetActiveSessions()
        {
            object obj = typeof(HttpRuntime).GetProperty("CacheInternal", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, null);
            object[] obj2 = (object[])obj.GetType().GetField("_caches", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj);

            for (int i = 0; i < obj2.Length; i++)
            {
                Hashtable c2 = (Hashtable)obj2[i].GetType().GetField("_entries", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj2[i]);
                foreach (DictionaryEntry entry in c2)
                {
                    object o1 = entry.Value.GetType().GetProperty("Value", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(entry.Value, null);
                    if (o1.GetType().ToString() == "System.Web.SessionState.InProcSessionState")
                    {
                        SessionStateItemCollection sess = (SessionStateItemCollection)o1.GetType().GetField("_sessionItems", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(o1);
                        if (sess != null)
                        {
                            yield return sess;
                        }
                    }
                }
            }
        }

        public bool CheckUserActionPermission(ETEMEnums.SecuritySettings securitySetting, bool _MakeRedirect)
        {

            if (userProps != null)
            {
                this.userProps.SecuritySetting = securitySetting.ToString();
                this.userProps.SecuritySettingBG = this.FormContext.DictionaryPermittedActionSetting.FirstOrDefault(s => s.Value.SecuritySetting == securitySetting.ToString()).Value.FrendlyName;
            }

            if (this.UserProps.Roles.Any(a => a.Name == "SUPPORT"))
            {
                return true;
            }

            if (this.UserProps.Roles.Any(a => a.Name == "VIEW"))
            {
                return true;
            }

            bool result = CheckUserActionPermissionByIntCode(securitySetting);

            if (!result)
            {
                if (_MakeRedirect)
                {
                    if (this.Page.Master != null)
                    {
                        Response.Redirect("~/UI/NoPermission.aspx");
                    }
                    else
                    {
                        Response.Redirect("~/UI/NoPermissionWithoutMasterPage.aspx");
                    }
                }
                else
                {

                    ShowMSG("Нямате права за това действие!");
                }
            }

            return result;

        }

        public bool CheckUserActionPermissionByIntCode(ETEMEnums.SecuritySettings securitySetting)
        {
            if (userProps != null)
            {
                this.userProps.SecuritySetting = securitySetting.ToString();

                if (this.FormContext.DictionaryPermittedActionSetting.FirstOrDefault(s => s.Value.SecuritySetting == securitySetting.ToString()).Value != null)
                {
                    this.userProps.SecuritySettingBG = this.FormContext.DictionaryPermittedActionSetting.FirstOrDefault(s => s.Value.SecuritySetting == securitySetting.ToString()).Value.FrendlyName;
                }
                else
                {
                    this.userProps.SecuritySettingBG = "###" + securitySetting.ToString() + "###";
                }
            }

            if (this.UserProps.Roles.Any(a => a.Name == "SUPPORT"))
            {
                return true;
            }

            if (this.UserProps.Roles.Any(a => a.Name == "VIEW"))
            {
                return true;
            }

            int checkCount = (from pa in this.FormContext.DictionaryPermittedActionSetting.Values
                              where pa.SecuritySetting == securitySetting.ToString() &&
                              this.UserProps.ListUserPermittedActionsID.Contains(pa.idPermittedAction)

                              select pa.SecuritySetting).Count();

            return (checkCount > 0 ? true : false);
        }

        protected override void OnPreRender(EventArgs e)
        {
            SetPageCaptions();
            base.OnPreRender(e);
        }

        public virtual void SetPageCaptions()
        {

        }

        public virtual string CurrentPagePath()
        {
            return this.Page.ToString();
            //throw new Exception("Айде, вземи да го имплементираш този метод!!");
        }

        public virtual FormResources CurrentFormResources()
        {
            throw new Exception("Айде, вземи да го имплементираш този метод!!");
        }

        public virtual string CurrentPageFullName()
        {
            return "Айде, вземи да го имплементираш този метод!!";
            //throw new Exception("Айде, вземи да го имплементираш този метод!!");
        }

        public virtual string CurrentModule()
        {
            return "CurrentModule is not set";
            //throw new Exception("Айде, вземи да го имплементираш този метод!!");
        }

        public virtual void FormSave()
        {
            throw new Exception("Айде, вземи да го имплементираш този метод!!");
        }

        public virtual void FormNew()
        {
            throw new Exception("Айде, вземи да го имплементираш този метод!!");
        }

        public virtual void FormLoad()
        {
            //ShowMSG(this.CurrentPageFullName());
        }


        public ContentPlaceHolder ContentPlaceHolder
        {
            get
            {
                if (this.Master != null)
                {
                    return this.Master.FindControl("ContentPlaceHolder") as ContentPlaceHolder;
                }
                else
                {
                    return null;
                }
            }
        }


        public HtmlGenericControl MasterPageContainer
        {
            get
            {
                if (this.Master != null)
                {
                    return this.Master.FindControl("MasterPageContainer") as HtmlGenericControl;
                }
                else
                {
                    return null;
                }
            }
        }

        public HtmlGenericControl MainNavUl
        {
            get
            {
                if (this.Master != null)
                {
                    return this.Master.FindControl("MainNavUl") as HtmlGenericControl;
                }
                else
                {
                    return null;
                }
            }
        }


        public SMCFooter GlobalFooter
        {
            get
            {
                if (this.Master != null)
                {
                    return this.Master.FindControl("GlobalFooter") as SMCFooter;
                }
                else
                {
                    return null;
                }
            }
        }


        public Control MasterPageMainManu
        {
            get
            {
                if (this.Master != null)
                {
                    return this.Master.FindControl("MasterPageMainManu") as Control;
                }
                else
                {
                    return null;
                }
            }
        }

        public void RemoveModalWindow()
        {
            ScriptManager.RegisterStartupScript(this.Page,
                                                this.Page.GetType(),
                                                "RemoveModalWindow",
                                                "<script type=\"text/javascript\" language=\"javascript\">" +
                                                "removeModalWindow();" +
                                                "</script>",
                                                false);
        }

        public void ShowJavaScriptMSG(string _MSG)
        {
            ScriptManager.RegisterStartupScript(this.Page,
                                                this.Page.GetType(),
                                                "ShowMessage",
                                                "<script type=\"text/javascript\" language=\"javascript\">" +
                                                "alert('" + _MSG + "');" +
                                                "</script>",
                                                false);
        }

        public void ShowMSG(string _MSG)
        {  //TODO shte se zapisva li v CallContext_a ???
            // this.validationHelper.ShowSingleMSG( MSG,this.CallContext);
            // this.CallContext.ErrorList.Clear();



            //ScriptManager.RegisterStartupScript(this.Page,
            //            this.Page.GetType(), "ShowMessage",
            //            "<script type=\"text/javascript\" language=\"javascript\">alert('" + MSG + "');</script>"
            //            , false);
            ShowMSG(_MSG, true);
        }

        public void ShowMSG(string _MSG, bool _ClearModal)
        {
            if (_ClearModal)
            {
                ScriptManager.RegisterStartupScript(this.Page,
                                                    this.Page.GetType(),
                                                    "ShowMessage",
                                                    "<script type=\"text/javascript\" language=\"javascript\">" +
                                                    "$('#myModal').modal('show');" +
                                                    "$('#myModalBody').text('" + _MSG + "');" +
                                                    "</script>",
                                                    false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page,
                                                    this.Page.GetType(),
                                                    "ShowMessage",
                                                    "<script type=\"text/javascript\" language=\"javascript\">" +
                                                    "$('#myModal2').modal('show');" +
                                                    "$('#myModalBody2').text('" + _MSG + "');" +
                                                    "</script>",
                                                    false);
            }
        }

        public static string GetConfirmJavaScript(string message, bool isCaptionKey)
        {
            return BaseHelper.JS_SCRIPT_CONFIRM_MESSAGE((isCaptionKey ? GetCaption(message) : message));
        }

        public void PrintSendCurrentWindow()
        {
            ScriptManager.RegisterStartupScript(
                this.Page, this.Page.GetType(), "PrintSend", "<script type=\"text/javascript\" language=\"javascript\">window.print();</script>", false);
        }

        public static string GetValueFromWebConfig(string key)
        {
            return WebConfigurationManager.AppSettings.Get(key);
        }

        public void OpenPageInNewWindow(FormResources page)
        {
            OpenPageInNewWindow(page, string.Empty);
        }

        public void OpenPageInNewWindow(FormResources page, string parameters)
        {
            string newWindowParams = "resizable=yes,scrollbars=yes,menubar=no,status=no,width=1024,height=900,location=no,toolbar=no,fullscreen=no";

            ScriptManager.RegisterStartupScript(
                                                this.Page,
                                                this.Page.GetType(),
                                                "OpenNewWindow",
                                                "<script type=\"text/javascript\" language=\"javascript\">" +
                                                ETEMModel.Helpers.BaseHelper.JS_SCRIPT_POPUP_PAGE(page.PagePath, parameters, newWindowParams) +
                                                "</script>",
                                                false
                                                );
        }

        public void OpenPageForDownloadFile(string path, string parameters)
        {
            string newWindowParams = "resizable=no,scrollbars=no,menubar=no,status=no,width=250,height=150,location=no,toolbar=no,fullscreen=no";

            ScriptManager.RegisterStartupScript(
                                                this.Page,
                                                this.Page.GetType(),
                                                "DownloadFile",
                                                "<script type=\"text/javascript\" language=\"javascript\">" +
                                                ETEMModel.Helpers.BaseHelper.JS_SCRIPT_POPUP_PAGE(path, parameters, newWindowParams) +
                                                "</script>",
                                                false
                                                );
        }

        public void RunJavaScript(string javaScript)
        {
            AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(
                                                this.Page,
                                                this.Page.GetType(),
                                                "ModalWindow",
                                                "<script type=\"text/javascript\" language=\"javascript\">" +
                                                 javaScript +
                                                "</script>",
                                                false
                                                );
        }

        public void RunJavaScriptModalWindow()
        {            
            this.RunJavaScript(ETEMModel.Helpers.BaseHelper.JS_SCRIPT_MODAL_WINDOW);
        }

        protected void CheckIfResultIsSuccess(Control lbResultContext)
        {
            Label lb = lbResultContext as Label;
            if (CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {
                lb.CssClass = "alert alert-success";
            }
            else
            {

                lb.CssClass = "alert alert-error";
            }
        }

        protected void ClearResultContext(Control lbResultContext)
        {
            Label lb = lbResultContext as Label;
            lb.CssClass = "";
            lb.Text = "";
        }

        protected void SelectOrDeselectAllGridCheckBox(System.Web.UI.WebControls.GridView gridView, bool makeItSelect, string checkBoxName)
        {
            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                GridViewRow row = gridView.Rows[i];
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox cbxGroupedDiscplines = FindControlById(row, checkBoxName) as CheckBox;
                    cbxGroupedDiscplines.Checked = makeItSelect;
                }
            }
        }

        public List<int> FindAllSelectedGridCheckBoxesByHdnRowMasterKeyName(System.Web.UI.WebControls.GridView gridView, string HdnRowMasterKey, string checkBoxName)
        {
            List<int> listIds = new List<int>();
            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                GridViewRow row = gridView.Rows[i];
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox cbxGroupedDiscplines = FindControlById(row, checkBoxName) as CheckBox;
                    if (cbxGroupedDiscplines.Checked)
                    {
                        HiddenField hdnRowMasterKey = FindControlById(row, HdnRowMasterKey) as HiddenField;
                        listIds.Add(int.Parse(hdnRowMasterKey.Value));
                    }

                }
            }

            return listIds;
        }

        public static Control GetParentOfType(Control childControl, Type parentType)
        {
            Control parent = childControl.Parent;
            while (parent.GetType() != parentType && parent.GetType().BaseType != parentType)
            {
                parent = parent.Parent;
            }
            if (parent.GetType() == parentType || parent.GetType().BaseType == parentType)
            {
                return parent;
            }
            throw new Exception("No control of expected type was found");
        }

        protected void HideButtonControls(Control control)
        {
            List<Button> listButton = BaseHelper.GetAllControlsRecusrvive<Button>(control).ToList();
            listButton.ForEach(s => s.Visible = false);
        }

        protected void HideLinkButtonControls(Control control)
        {
            List<LinkButton> listLinkButton = BaseHelper.GetAllControlsRecusrvive<LinkButton>(control).ToList();
            //listLinkButton.ForEach(s => s.Visible = false);
            //listLinkButton.Where(s => s.ID != null && !s.ID.Contains("lbtnOpenDirectory")).ToList().ForEach(s => s.Visible = false);

            listLinkButton.Where(s => s.ID == null).ToList().ForEach(s => s.Enabled = false);
            listLinkButton.Where(s => s.ID != null).ToList().ForEach(s => s.Visible = false);
            listLinkButton.Where(s => s.ID != null && s.ID.Contains("lbtnOpenDirectory")).ToList().ForEach(s => s.Visible = true);
        }

        protected void DisableTextBoxControls(Control control)
        {
            List<TextBox> listTextBox = BaseHelper.GetAllControlsRecusrvive<TextBox>(control).ToList();
            listTextBox.ForEach(s => s.Enabled = false);
        }

        protected void DisableDropDownListControls(Control control)
        {
            List<DropDownList> list = BaseHelper.GetAllControlsRecusrvive<DropDownList>(control).ToList();
            list.ForEach(s => s.Enabled = false);
        }

        protected void ShowButtonControls(Control control)
        {
            List<Button> listButton = BaseHelper.GetAllControlsRecusrvive<Button>(control).ToList();
            listButton.ForEach(s => s.Visible = true);
        }

        protected void ShowLinkButtonControls(Control control)
        {
            List<LinkButton> listLinkButton = BaseHelper.GetAllControlsRecusrvive<LinkButton>(control).ToList();
            //listLinkButton.ForEach(s => s.Visible = false);
            //listLinkButton.Where(s => s.ID != null && !s.ID.Contains("lbtnOpenDirectory")).ToList().ForEach(s => s.Visible = false);

            listLinkButton.Where(s => s.ID == null).ToList().ForEach(s => s.Enabled = true);
            listLinkButton.Where(s => s.ID != null).ToList().ForEach(s => s.Visible = true);
            listLinkButton.Where(s => s.ID != null && s.ID.Contains("lbtnOpenDirectory")).ToList().ForEach(s => s.Visible = true);
        }

        protected void EnableTextBoxControls(Control control)
        {
            List<TextBox> listTextBox = BaseHelper.GetAllControlsRecusrvive<TextBox>(control).ToList();
            listTextBox.ForEach(s => s.Enabled = true);
        }
        
        protected void EnableDropDownListControls(Control control)
        {
            List<DropDownList> list = BaseHelper.GetAllControlsRecusrvive<DropDownList>(control).ToList();
            list.ForEach(s => s.Enabled = true);
        }

        public UserProps MakeLoginByUserID(string UserID)
        {
            UserProps userProps = new UserProps();

            ETEMModel.Models.User currentUser = AdminClientRef.GetUserByUserID(UserID);

            if (currentUser != null)
            {
                userProps.Roles = AdminClientRef.GetAllRolesByUser(currentUser.idUser.ToString(), null, null);

                for (int i = 0; i < userProps.Roles.Count; i++)
                {
                    List<int> currentRolePermmitedActionsIds = AdminClientRef.
                                                               GetAllPermittedActionsByRole(userProps.Roles[i].idRole.ToString(), null, null)
                                                               .Select(r => r.idPermittedAction).ToList();
                    for (int j = 0; j < currentRolePermmitedActionsIds.Count; j++)
                    {
                        bool isDuplicate = userProps.ListUserPermittedActionsID.Any(p => p == currentRolePermmitedActionsIds[j]);
                        if (!isDuplicate)
                        {
                            userProps.ListUserPermittedActionsID.Add(currentRolePermmitedActionsIds[j]);
                        }
                    }
                }

                //userProps.ListUserPermittedActionsID = AdminClientRef.getallp
                userProps.IdUser = currentUser.idUser.ToString();
                userProps.UserName = currentUser.UserName;

                Person person = this.AdminClientRef.GetPersonByPersonID(currentUser.idPerson.ToString());
                userProps.PersonNamePlusTitle = person.FullNamePlusTitle;
                userProps.PersonNameNoTitle = person.FullName;
                userProps.PersonNameAndFamily = person.FullNameTwo;
                userProps.PersonTwoNamePlusTitle = person.TwoNamesPlusTitle;
                userProps.PersonID = person.idPerson.ToString();
                userProps.SessionID = this.Session.SessionID;
                userProps.IPAddress = Request.UserHostAddress;
                userProps.LoginDateTime = DateTime.Now;
                this.Session.Timeout = Convert.ToInt32(BasicPage.GetValueFromWebConfig("SessionTimeOut"));
                userProps.idStudent =  Constants.INVALID_ID;

                userProps.IsCheckDomain = currentUser.idCheckDomain == GetKeyValueByIntCode("YES_NO", "Yes").idKeyValue;
                userProps.IsKilled = false;

                this.Session.Add(ETEMModel.Helpers.Constants.SESSION_USER_PROPERTIES, userProps);
                this.Session.Timeout = Int32.Parse(GetSettingByCode(ETEMEnums.AppSettings.WebSessionTimeOut).SettingValue);

                Dictionary<string, HttpSessionState> sessionData =
                    (Dictionary<string, HttpSessionState>)Application[Constants.APPLICATION_ALL_SESSIONS];

                if (sessionData.Keys.Contains(HttpContext.Current.Session.SessionID))
                {
                    sessionData.Remove(HttpContext.Current.Session.SessionID);
                    sessionData.Add(HttpContext.Current.Session.SessionID, HttpContext.Current.Session);
                }
            }

            return userProps;
        }



       
    }
}