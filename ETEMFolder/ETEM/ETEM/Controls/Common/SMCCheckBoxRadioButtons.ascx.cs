using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers;

namespace ETEM.Controls.Common
{
    public partial class SMCCheckBoxRadioButtons : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public string FirstCheckBoxText
        {
            get { return this.HdnFirstCheckBoxText.Value; }
            set { this.CheckBoxOne.Text = value; this.HdnFirstCheckBoxText.Value = value; }
        }

        public string SecondCheckBoxText
        {
            get { return this.HdnSecondCheckBoxText.Value; }
            set { this.CheckBoxTwo.Text = value; this.HdnSecondCheckBoxText.Value = value; }
        }

        public string FirstCheckBoxId
        {
            get { return this.HdnFirstCheckBox.Value; }
            set { this.HdnFirstCheckBox.Value = value; }
        }


        public string SecondCheckBoxId
        {
            get { return this.HdnSecondCheckBox.Value; }
            set { this.HdnSecondCheckBox.Value = value; }
        }
        public override void UserControlLoad()
        {

        }

        public int CheckedId
        {
            get
            {
                if (this.CheckBoxOne.Checked == true)
                {
                    return int.Parse(this.HdnFirstCheckBox.Value);
                }
                else if (this.CheckBoxTwo.Checked == true)
                {
                    return int.Parse(this.HdnSecondCheckBox.Value);
                }
                else
                {
                    return Constants.INVALID_ID_ZERO;
                }

            }
        }

        public void SetCheckedByText(string text)
        {
            if (this.HdnFirstCheckBoxText.Value == text)
            {
                this.CheckBoxOne.Checked = true;
            }
            else if (this.HdnSecondCheckBoxText.Value == text)
            {
                this.CheckBoxTwo.Checked = true;
            }
        }

        public void SetCheckedById(int id)
        {
            if (int.Parse(this.HdnFirstCheckBox.Value) == id)
            {
                this.CheckBoxOne.Checked = true;
            }
            else if (int.Parse(this.HdnSecondCheckBox.Value) == id)
            {
                this.CheckBoxTwo.Checked = true;
            }
        }

        public bool EnabledCheckBoxes
        {
            get
            {
                this.checkBoxControl.Attributes.Remove("class");
                return (this.CheckBoxOne.Enabled && this.CheckBoxTwo.Enabled);
            }
            set
            {
                this.CheckBoxOne.Enabled = value;
                this.CheckBoxTwo.Enabled = value;
                this.checkBoxControl.Attributes.Add("class", "gray-backgroupnd");
            }
        }

    }
}