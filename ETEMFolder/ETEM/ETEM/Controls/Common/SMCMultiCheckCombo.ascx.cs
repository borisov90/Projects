using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using System.Collections;
using System.Data.Odbc;
using ETEMModel.Models;
using ETEMModel.Helpers;

namespace ETEM.Controls.Common
{

    public partial class SMCMultiCheckCombo : BaseUserControl
    {
        public string KeyTypeIntCode { get; set; }
        public string DataSourceType { get; set; }
        public bool UseShortDefaultValue { get; set; }
        public string OrderBy { get; set; }
        public string OrderDirection { get; set; }


        private bool addingDefaultValue = true;
        public bool AddingDefaultValue
        {
            get { return addingDefaultValue; }
            set { addingDefaultValue = value; }
        }

        private static string JAVASCRIPT_ITEMSELECTED_FUNCTION_NAME = "_CheckItem";

        /*
         *  function CheckItem(checkBoxList) {
        
        var options = checkBoxList.getElementsByTagName('input');
        var arrayOfCheckBoxLabels = checkBoxList.getElementsByTagName("label");
        var s = "";

        for (i = 0; i < options.length; i++) {
            var opt = options[i];
            if (opt.checked) {
                s = s + ", " + arrayOfCheckBoxLabels[i].innerText;
            }
        }
        if (s.length > 0) {
            s = s.substring(2, s.length); //sacar la primer 'coma'
        }
        var TxtBox = document.getElementById("<%=txtCombo.ClientID%>");
        TxtBox.value = s;
        document.getElementById('<%=hidVal.ClientID %>').value = s;
    }
         */

        public string CustomSuffixForControlID
        {
            get { return this.hdnCustomSuffixForControlID.Value; }
            set { this.hdnCustomSuffixForControlID.Value = value; }
        }

        private List<KeyValue> list = new List<KeyValue>();

        private static string JAVASCRIPT_CHECKITEM_ITEMSELECTED(string funcName, string hdnID, string TxtBox)
        {
            string result = "<script type=\"text/javascript\" language=\"javascript\">" +
                            "function " + funcName + "(checkBoxList) { " +
                            "var options = checkBoxList.getElementsByTagName('input'); " +
                            "var arrayOfCheckBoxLabels = checkBoxList.getElementsByTagName(\"label\"); " +
                            "var s = \"\";" +
                            "for (i = 0; i < options.length; i++) {" +
                            "    var opt = options[i];" +
                            "    if (opt.checked) {" +
                            "       s = s + \", \" + (arrayOfCheckBoxLabels[i].innerText?arrayOfCheckBoxLabels[i].innerText : arrayOfCheckBoxLabels[i].textContent);" +//firefox uses textContent
                            "    }" +
                            "}" +
                            "if (s.length > 0) {" +
                            "   s = s.substring(2, s.length);" +
                            "}" +

                            "var TxtBox = document.getElementById('" + TxtBox + "');" +
                            "TxtBox.value = s;" +
                            "document.getElementById('" + hdnID + "').value = s;" +
                //"alert(hdnKey);" +
                //"alert('" + hdnID + "');" +
                            "} " +
                            "</script>";

            return result;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (string.IsNullOrEmpty(this.DataSourceType))
            {
                this.DataSourceType = "KeyValue";
            }

            string scriptItemSelectedName = this.ID + this.CustomSuffixForControlID + JAVASCRIPT_ITEMSELECTED_FUNCTION_NAME;

            AjaxControlToolkit.ToolkitScriptManager.RegisterClientScriptBlock(this,
                                                                             this.GetType(),
                                                                             scriptItemSelectedName,
                                                                             JAVASCRIPT_CHECKITEM_ITEMSELECTED(scriptItemSelectedName,
                                                                                                                  this.hidVal.ClientID,
                                                                                                                  this.txtCombo.ClientID),
                                                                             false);


            this.chkList.Attributes.Add("onclick", scriptItemSelectedName + "(this)");
        }

        public CheckBoxList CheckBoxListCTRL
        {
            get { return this.chkList; }
        }

        public TextBox TextBoxCTRL
        {
            get { return this.txtCombo; }
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

            list = LoadList();

            this.chkList.DataSource = list;
            this.chkList.DataBind();
        }

        private List<KeyValue> LoadList()
        {
            if (DataSourceType == "KeyValue")
            {
                list = this.ownerPage.GetAllKeyValueByKeyTypeIntCode(KeyTypeIntCode).OrderBy(e => e.Name).ToList();
                if (this.AddingDefaultValue)
                {
                    SMCDropDownList.AddDefaultValue(list, this.UseShortDefaultValue);
                }
                if (OrderBy != null)
                {
                    list = BaseClassBL<KeyValue>.Sort(list, this.OrderBy, this.OrderDirection != null ? this.OrderDirection : "asc").ToList();
                }
            }

            return list;

        }

        private void AddDefaultValue(List<KeyValue> list)
        {
            SMCDropDownList.AddDefaultValue(list, false);
        }

        /// <summary>
        /// Set the Width of the CheckBoxList
        /// </summary>
        public int WidthCheckListBox
        {
            set
            {
                chkList.Width = value;
                Panel111.Width = value + 20;
            }
        }
        /// <summary>
        /// Set the Width of the Combo
        /// </summary>
        public int Width
        {
            set { txtCombo.Width = value; }
            get { return (Int32)txtCombo.Width.Value; }
        }
        public bool Enabled
        {
            set { txtCombo.Enabled = value; }
        }
        /// <summary>
        /// Set the CheckBoxList font Size
        /// </summary>
        public FontUnit fontSizeCheckBoxList
        {
            set { chkList.Font.Size = value; }
            get { return chkList.Font.Size; }
        }
        /// <summary>
        /// Set the ComboBox font Size
        /// </summary>
        public FontUnit fontSizeTextBox
        {
            set { txtCombo.Font.Size = value; }
        }



        /// <summary>
        /// Add Items to the CheckBoxList.
        /// </summary>
        /// <param name="array">ArrayList to be added to the CheckBoxList</param>
        public void AddItems(ArrayList array)
        {
            for (int i = 0; i < array.Count; i++)
            {
                chkList.Items.Add(array[i].ToString());
            }
        }


        /// <summary>
        /// Add Items to the CheckBoxList
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="nombreCampoTexto">Field Name of the OdbcDataReader to Show in the CheckBoxList</param>
        /// <param name="nombreCampoValor">Value Field of the OdbcDataReader to be added to each Field Name (it can be the same string of the textField)</param>
        public void AddItems(OdbcDataReader dr, string textField, string valueField)
        {
            ClearAll();
            int i = 0;
            while (dr.Read())
            {
                chkList.Items.Add(dr[textField].ToString());
                chkList.Items[i].Value = i.ToString();
                i++;
            }
        }


        /// <summary>
        /// Uncheck of the Items of the CheckBox
        /// </summary>
        public void unselectAllItems()
        {
            for (int i = 0; i < chkList.Items.Count; i++)
            {
                chkList.Items[i].Selected = false;
            }
        }

        /// <summary>
        /// Delete all the Items of the CheckBox;
        /// </summary>
        public void ClearAll()
        {
            txtCombo.Text = "";
            chkList.Items.Clear();
        }

        /// <summary>
        /// Get or Set the Text shown in the Combo
        /// </summary>
        public string Text
        {
            get { return hidVal.Value; }
            set { txtCombo.Text = value; }
        }
    }
}