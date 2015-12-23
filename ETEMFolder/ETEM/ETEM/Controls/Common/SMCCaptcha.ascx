<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SMCCaptcha.ascx.cs" Inherits="ETEM.Controls.Common.SMCCaptcha" %>

<asp:Image ID="Image1" runat="server" ImageUrl="~/Controls/Common/CImage.aspx" 
    Height="100px" Width="300px"/>
<br />
<asp:Label ID="lblmsg" runat="server" Font-Bold="True" ForeColor="Red" >
</asp:Label>
<br />
<asp:TextBox ID="tbxCode" runat="server" CssClass="mandatory"></asp:TextBox>
