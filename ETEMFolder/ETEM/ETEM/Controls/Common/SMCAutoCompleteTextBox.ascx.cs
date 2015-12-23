using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using System.Text;
using ETEMModel.Helpers;

namespace ETEM.Controls.Common
{
    public partial class SMCAutoCompleteTextBox : BaseUserControl
    {
        public event EventHandler ClientItemSelected;

        private static string JAVASCRIPT_ITEMSELECTED_FUNCTION_NAME = "_ItemSelected";

        private static string JAVASCRIPT_AUTOCOMPLETE_ITEMSELECTED(string funcName, string hdnID, string btnID, bool isAutoPostBack)
        {
            string result = "<script type=\"text/javascript\" language=\"javascript\">" +
                            "function " + funcName + "(source, eventArgs) { " +
                            "var hdnKey = eventArgs.get_value(); " +
                // "alert(hdnKey);" +
                // "alert('" + hdnID + "');" +
                            "document.getElementById('" + hdnID + "').value = hdnKey; " +
                            (isAutoPostBack ? "document.getElementById('" + btnID + "').click(); " : string.Empty) +
                            "} " +
                            "</script>";

            return result;
        }

        public void ClearSelection()
        {
            this.SelectedValue = string.Empty;
            this.Text = string.Empty;
        }

        public string SelectedValueAddParam
        {
            get { return this.hdnSelectedItemID.Value; }
            set { this.hdnSelectedItemID.Value = value; }
        }
        public string OnlyAddParam
        {
            get
            {
                string[] arrSelectedValue = this.SelectedValueAddParam.Split('|').Where(w => w.Trim() != string.Empty).ToArray();

                if (arrSelectedValue.Length > 0)
                {
                    return arrSelectedValue[arrSelectedValue.Length - 1];
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string SelectedValue
        {
            get
            {
                string[] arrSelectedValue = this.SelectedValueAddParam.Split('|').Where(w => w.Trim() != string.Empty).ToArray();
                if (arrSelectedValue.Length > 0)
                {
                    return arrSelectedValue[0];
                }
                else
                {
                    return string.Empty;
                }
            }
            set { this.hdnSelectedItemID.Value = value; }
        }
        public int? SelectedValueINT
        {
            get
            {
                int tmpValue = Constants.INVALID_ID;
                if (int.TryParse(this.SelectedValue, out tmpValue) && tmpValue > 0 &&
                    !string.IsNullOrEmpty(this.Text.Trim()))
                {
                    return tmpValue;
                }
                else
                {
                    return null;
                }
            }
        }
        public int SelectedValueIntOrInvalidID
        {
            get
            {
                int tmpValue = Constants.INVALID_ID;
                if (int.TryParse(this.SelectedValue, out tmpValue) && tmpValue > 0 &&
                    !string.IsNullOrEmpty(this.Text.Trim()))
                {
                    return tmpValue;
                }
                else
                {
                    return Constants.INVALID_ID;
                }
            }
        }

        public bool ShowTextAsToolTip
        {
            get;
            set;
        }

        public string Text
        {
            get { return this.tbxTextToComplete.Text; }
            set
            {
                this.tbxTextToComplete.Text = value;
                if (ShowTextAsToolTip)
                {
                    this.tbxTextToComplete.ToolTip = value;
                }
            }
        }
        public string CustomSuffixForControlID
        {
            get { return this.hdnCustomSuffixForControlID.Value; }
            set { this.hdnCustomSuffixForControlID.Value = value; }
        }
        public string CustomCase
        {
            get { return this.hdnCustomCase.Value; }
            set { this.hdnCustomCase.Value = value; }
        }
        public string TableForSelection
        {
            get { return this.hdnTableForSelection.Value; }
            set { this.hdnTableForSelection.Value = value; }
        }
        public string ColumnIdForSelection
        {
            get { return this.hdnColumnIdForSelection.Value; }
            set { this.hdnColumnIdForSelection.Value = value; }
        }
        public string ColumnNameForSelection
        {
            get { return this.hdnColumnNameForSelection.Value; }
            set { this.hdnColumnNameForSelection.Value = value; }
        }
        public string ColumnNameForOrderBy
        {
            get { return this.hdnColumnNameForOrderBy.Value; }
            set { this.hdnColumnNameForOrderBy.Value = value; }
        }
        public string AdditionalWhereParam
        {
            get { return this.hdnAdditionalWhereParam.Value; }
            set { this.hdnAdditionalWhereParam.Value = value; }
        }

        public TextBox TextBoxCtrl
        {
            get { return this.tbxTextToComplete; }
        }

        public TextBoxMode TextBoxModeValue
        {
            get { return this.tbxTextToComplete.TextMode; }
            set { this.tbxTextToComplete.TextMode = value; }
        }

        public string CssClassTextBox
        {
            get { return this.tbxTextToComplete.CssClass; }
            set { this.tbxTextToComplete.CssClass = value; }
        }

        public bool ReadOnly
        {
            get { return this.tbxTextToComplete.ReadOnly; }
            set { this.tbxTextToComplete.ReadOnly = value; }
        }

        public bool AutoCompleteEnabled
        {
            get { return this.tbxTextToComplete_AutoCompleteExtender.Enabled; }
            set { this.tbxTextToComplete_AutoCompleteExtender.Enabled = value; }
        }

        public bool Enabled
        {
            get { return !this.tbxTextToComplete.ReadOnly; }
            set
            {
                this.tbxTextToComplete.ReadOnly = !value;
                this.tbxTextToComplete_AutoCompleteExtender.Enabled = value;
            }
        }

        public bool IsAutoPostBack { get; set; }

        public int MinimumPrefixLength
        {
            get { return this.tbxTextToComplete_AutoCompleteExtender.MinimumPrefixLength; }
            set { this.tbxTextToComplete_AutoCompleteExtender.MinimumPrefixLength = value; }
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

            InitData();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            string scriptItemSelectedName = this.ID + this.CustomSuffixForControlID + JAVASCRIPT_ITEMSELECTED_FUNCTION_NAME;

            AjaxControlToolkit.ToolkitScriptManager.RegisterClientScriptBlock(this,
                                                                              this.GetType(),
                                                                              scriptItemSelectedName,
                                                                              JAVASCRIPT_AUTOCOMPLETE_ITEMSELECTED(scriptItemSelectedName,
                                                                                                                   this.hdnSelectedItemID.ClientID,
                                                                                                                   this.btnValueChanged.ClientID,
                                                                                                                   this.IsAutoPostBack),
                                                                              false);

            this.tbxTextToComplete_AutoCompleteExtender.OnClientItemSelected = scriptItemSelectedName;

            if (this.ClientItemSelected != null)
            {
                this.btnValueChanged.Click += new EventHandler(this.ClientItemSelected);
            }

            InitData();
        }

        private void InitData()
        {
            string scriptItemSelectedName = this.ID + this.CustomSuffixForControlID + JAVASCRIPT_ITEMSELECTED_FUNCTION_NAME;

            AjaxControlToolkit.ToolkitScriptManager.RegisterClientScriptBlock(this,
                                                                              this.GetType(),
                                                                              scriptItemSelectedName,
                                                                              JAVASCRIPT_AUTOCOMPLETE_ITEMSELECTED(scriptItemSelectedName,
                                                                                                                   this.hdnSelectedItemID.ClientID,
                                                                                                                   this.btnValueChanged.ClientID,
                                                                                                                   this.IsAutoPostBack
                                                                                                                   ),
                                                                              false);

            this.tbxTextToComplete_AutoCompleteExtender.OnClientItemSelected = scriptItemSelectedName;

            string contextKey = string.Empty;
            if (!string.IsNullOrEmpty(this.CustomCase))
            {
                contextKey = "case=" + this.CustomCase;
            }
            if (!string.IsNullOrEmpty(this.TableForSelection))
            {
                contextKey += (string.IsNullOrEmpty(contextKey) ? this.TableForSelection : "|" + this.TableForSelection);
            }
            if (!string.IsNullOrEmpty(this.ColumnNameForSelection))
            {
                contextKey += (string.IsNullOrEmpty(contextKey) ? this.ColumnNameForSelection : "|" + this.ColumnNameForSelection);
            }
            if (!string.IsNullOrEmpty(this.ColumnIdForSelection))
            {
                contextKey += (string.IsNullOrEmpty(contextKey) ? this.ColumnIdForSelection : "|" + this.ColumnIdForSelection);
            }
            if (!string.IsNullOrEmpty(this.ColumnNameForOrderBy))
            {
                contextKey += (string.IsNullOrEmpty(contextKey) ? this.ColumnNameForOrderBy : "|" + this.ColumnNameForOrderBy);
            }
            if (!string.IsNullOrEmpty(this.AdditionalWhereParam))
            {
                contextKey += (string.IsNullOrEmpty(contextKey) ? this.AdditionalWhereParam : "|" + this.AdditionalWhereParam);
            }
            this.tbxTextToComplete_AutoCompleteExtender.ContextKey = contextKey;
        }

    }
}