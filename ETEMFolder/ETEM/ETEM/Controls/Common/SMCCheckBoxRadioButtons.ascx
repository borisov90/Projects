<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SMCCheckBoxRadioButtons.ascx.cs" Inherits="ETEM.Controls.Common.SMCCheckBoxRadioButtons" %>
<div id="checkBoxControl" runat="server">

<p><asp:CheckBox id="CheckBoxOne" CssClass="depenableCheckBox"  runat="server"  Text="OptionOne"/></p>

<p><asp:CheckBox id="CheckBoxTwo" CssClass="depenableCheckBox"  runat="server" Text="OptionTwo"/></p>
<asp:HiddenField id="HdnFirstCheckBoxText" runat="server"/>
<asp:HiddenField id="HdnSecondCheckBoxText" runat="server"/>
<asp:HiddenField id="HdnFirstCheckBox" runat="server"/>
<asp:HiddenField id="HdnSecondCheckBox" runat="server"/>
</div>

