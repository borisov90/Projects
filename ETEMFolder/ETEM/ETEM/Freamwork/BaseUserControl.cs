using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Models;
using ETEMModel.Services.Admin;
using ETEMModel.Services.Common;
using ETEMModel.Services.CostCalculation;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text;
using System.Collections;

namespace ETEM.Freamwork
{
    public class BaseUserControl : System.Web.UI.UserControl
    {
        public Administration AdminClientRef;
        public Common CommonClientRef;
        public CostCalculationRef CostCalculationRef;
        public string CurrentEntityMasterID { get; set; }
        public string CustomEntityID { get; set; }
        public string UserControlName { get; set; }
        public int CurrentYear { get; set; }
        public int CurrentPeriod { get; set; }
        public int MaskCampusApplicationNumber { get; set; }
        public int MaskScholarShipApplicationNumber { get; set; }

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

        protected BasicPage ownerPage;

        public virtual void UserControlLoad()
        {
            throw new Exception("Айде, вземи да го имплементираш този метод!!");
        }
        public virtual void ClearResultContext()
        {
            throw new Exception("Айде, вземи да го имплементираш този метод!!");
        }
        public virtual Tuple<CallContext, string> UserControlSave()
        {
            throw new Exception("Айде, вземи да го имплементираш този метод!!");
        }
        public virtual void SetControlCaptions()
        {
            throw new Exception("Айде, вземи да го имплементираш този метод!!");
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

        protected void CheckIfResultIsSuccess(Control lbResultContext)
        {
            Label lb = lbResultContext as Label;
            if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {
                lb.CssClass = "alert alert-success";
            }
            else
            {
                lb.CssClass = "alert alert-error";
            }
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

        protected void AddMessage(Control label, string message)
        {
            Label lb = label as Label;            
            if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {
                lb.CssClass = "alert alert-success";
            }
            else
            {
                lb.CssClass = "alert alert-error";
            }
            lb.Text = message;
        }

        protected void ClearResultContext(Control lbResultContext)
        {
            Label lb = lbResultContext as Label;
            lb.CssClass = "";
            lb.Text = "";
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.ownerPage = this.Page as BasicPage;

            this.AdminClientRef = new Administration();
            this.CommonClientRef = new Common();
            this.CostCalculationRef = new CostCalculationRef();

            this.CurrentEntityMasterID = Constants.INVALID_ID_STRING;

            AjaxControlToolkit.ToolkitScriptManager.RegisterClientScriptBlock(this,
                                                                              this.GetType(),
                                                                              "IsNumeric",
                                                                              BaseHelper.JS_SCRIPT_IS_NUMERIC,
                                                                              false);
        }

        public void InitForDynamicLoad(BasicPage _OwnerPage)
        {
            this.ownerPage = _OwnerPage;
            this.AdminClientRef = new Administration();
            this.CommonClientRef = new Common();

            this.CurrentEntityMasterID = Constants.INVALID_ID_STRING;

            AjaxControlToolkit.ToolkitScriptManager.RegisterClientScriptBlock(this,
                                                                              this.GetType(),
                                                                              "IsNumeric",
                                                                              BaseHelper.JS_SCRIPT_IS_NUMERIC,
                                                                              false);
        }

        public void OpenPageInNewWindow(FormResources page, string parameters)
        {
            string newWindowParams = "resizable=yes,scrollbars=yes,menubar=no,status=no,width=1024,height=1000,location=no,toolbar=no,fullscreen=no";

            ScriptManager.RegisterStartupScript(this.Page,
                                                this.Page.GetType(),
                                                "OpenNewWindow",
                                                "<script type=\"text/javascript\" language=\"javascript\">" +
                                                 ETEMModel.Helpers.BaseHelper.JS_SCRIPT_POPUP_PAGE(page.PagePath, parameters, newWindowParams) +
                                                "</script>",
                                                false
                                                );
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

        public static void SetAllControlsEnabledOrDisabledWithoutExceptions<T>(Control control,
                                                                               List<string> listExceptionsNames,
                                                                               bool isEnabled) where T : Control
        {
            BaseHelper.SetAllControlsEnabledOrDisabledWithoutExceptions<T>(control, listExceptionsNames, isEnabled);
        }

        public static string GetCaption(string key)
        {
            return ETEMModel.Helpers.BaseHelper.GetCaptionString(key);
        }

        public static string GetConfirmJavaScript(string message, bool isCaptionKey)
        {
            return BaseHelper.JS_SCRIPT_CONFIRM_MESSAGE((isCaptionKey ? GetCaption(message) : message));
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

        protected string GetCurrentControlSystemInfo()
        {
            //<p>An unordered list:</p>
            //<ul>
            //  <li>Coffee</li>
            //  <li>Tea</li>
            //  <li>Milk</li>
            //</ul>

            StringBuilder res = new StringBuilder();


            res.Append("<p>").Append(this.ownerPage.CurrentPageFullName()).Append("</p>");
            res.Append("<ul>");
            res.Append("<li>").Append("CurrentEntityMasterID:").Append(this.CurrentEntityMasterID).Append("</li>");
            res.Append("<li>").Append("CustomEntityID:").Append(this.CustomEntityID).Append("</li>");
            res.Append("<li>").Append("CurrentYear:").Append(this.CurrentYear).Append("</li>");
            res.Append("<li>").Append("CurrentPeriod:").Append(this.CurrentPeriod).Append("</li>");

            

            foreach (DictionaryEntry entry in this.ownerPage.FormContext.QueryString)
            {
                res.Append("<li>").Append(entry.Key).Append(":").Append(entry.Value).Append("</li>");
            }

            res.Append("<li>").Append("this.Parent.ClientID:").Append(this.Parent.ClientID).Append("</li>");
            res.Append("<li>").Append("this.ClientID:").Append(this.ClientID).Append("</li>");
            res.Append("<li>").Append("this.ID:").Append(this.ID).Append("</li>");





            res.Append("</ul>");



            return res.ToString();
        }

        protected override void OnPreRender(EventArgs e)
        {
            //Label lbCurrentControlSystemInfo = new Label();

            //lbCurrentControlSystemInfo.ID = "lbCurrentControlSystemInfo";
            //lbCurrentControlSystemInfo.Text = GetCurrentControlSystemInfo();
            //this.Controls.Add(lbCurrentControlSystemInfo);



            base.OnPreRender(e);
        }

        protected void HideButtonControls(Control control)
        {
            List<Button> listButton = BaseHelper.GetAllControlsRecusrvive<Button>(control).ToList();
            listButton.ForEach(s => s.Visible = false);
        }

        protected void HideButtonControls(Control control, List<string> listExceptionIDs)
        {
            List<Button> listButton = BaseHelper.GetAllControlsRecusrvive<Button>(control).ToList();

            if (listExceptionIDs != null)
            {
                listButton.Where(w => w.ID == null ||
                                 (w.ID != null && !listExceptionIDs.Contains(w.ID))).ToList().ForEach(s => s.Visible = false);
            }
            else
            {
                listButton.ForEach(s => s.Visible = false);
            }
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



    }
}