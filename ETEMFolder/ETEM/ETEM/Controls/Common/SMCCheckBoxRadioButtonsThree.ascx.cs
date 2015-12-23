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
    public partial class SMCCheckBoxRadioButtonsThree : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public string FirstCheckBoxText
        {
            get { return this.HdnFirstCheckBoxText.Value; }
            set { this.CheckBox1.Text = value; this.HdnFirstCheckBoxText.Value = value; }
        }

        public string SecondCheckBoxText
        {
            get { return this.HdnSecondCheckBoxText.Value; }
            set { this.CheckBox2.Text = value; this.HdnSecondCheckBoxText.Value = value; }
        }

        public string ThirdCheckBoxText
        {
            get { return this.HdnThirdCheckBoxText.Value; }
            set { this.CheckBox3.Text = value; this.HdnThirdCheckBoxText.Value = value; }
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

        public string ThirdCheckBoxId
        {
            get { return this.HdnThirdCheckBoxText.Value; }
            set { this.HndThirdCheckBox.Value = value; }
        }

        public override void UserControlLoad()
        {

        }

        public int CheckedId
        {
            get
            {
                if (this.CheckBox1.Checked == true)
                {
                    return int.Parse(this.HdnFirstCheckBox.Value);
                }
                else if (this.CheckBox2.Checked == true)
                {
                    return int.Parse(this.HdnSecondCheckBox.Value);
                }
                else
                {
                    return int.Parse(this.HndThirdCheckBox.Value);
                }

            }
        }

        public void SetCheckedByText(string text)
        {
            if (this.HdnFirstCheckBoxText.Value == text)
            {
                this.CheckBox1.Checked = true;
            }
            else if (this.HdnSecondCheckBoxText.Value == text)
            {
                this.CheckBox2.Checked = true;
            }
            else
            {
                this.CheckBox3.Checked = true;
            }
        }

        public void SetCheckedById(int id)
        {
            if (int.Parse(this.HdnFirstCheckBox.Value) == id)
            {
                this.CheckBox1.Checked = true;
            }
            else if (int.Parse(this.HdnSecondCheckBox.Value) == id)
            {
                this.CheckBox2.Checked = true;
            }
            else
            {
                this.CheckBox3.Checked = true;
            }
        }

        public bool EnabledCheckBoxes
        {
            get
            {
                this.checkBoxControl.Attributes.Remove("class");
                return (this.CheckBox1.Enabled && this.CheckBox2.Enabled && this.CheckBox3.Enabled);
            }
            set
            {
                this.CheckBox1.Enabled = value;
                this.CheckBox2.Enabled = value;
                this.CheckBox3.Enabled = value;
                if (value == false)
                {
                    this.checkBoxControl.Attributes.Add("class", "gray-backgroupnd");
                }
                else
                {
                    this.checkBoxControl.Attributes.Remove("class");
                }

            }
        }

    }
}