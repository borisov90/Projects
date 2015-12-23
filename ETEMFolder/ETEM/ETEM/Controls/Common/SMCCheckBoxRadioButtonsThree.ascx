<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SMCCheckBoxRadioButtonsThree.ascx.cs" Inherits="ETEM.Controls.Common.SMCCheckBoxRadioButtonsThree" %>

<div id="checkBoxControl" runat="server">

<p><asp:CheckBox id="CheckBox1"   runat="server"  AutoPostBack="true" OnCheckedChanged="CheckBox_OnCheckedChanged" Text="OptionOne"/></p>

<p><asp:CheckBox id="CheckBox2"  runat="server" AutoPostBack="true" Text="OptionTwo"/></p>

<p><asp:CheckBox id="CheckBox3" Visible="false"  Checked="true" runat="server" Text="OptionTwo"/></p>

<asp:HiddenField id="HdnFirstCheckBoxText" runat="server"/>
<asp:HiddenField id="HdnSecondCheckBoxText" runat="server"/>
<asp:HiddenField id="HdnThirdCheckBoxText" runat="server"/>

<asp:HiddenField id="HdnFirstCheckBox" runat="server"/>
<asp:HiddenField id="HdnSecondCheckBox" runat="server"/>
<asp:HiddenField id="HndThirdCheckBox" runat="server"/>

</div>


