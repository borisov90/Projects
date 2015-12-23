<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SMCDropDownList.ascx.cs"
    Inherits="ETEM.Controls.Common.SMCDropDownList" %>
<%@ Register Src="~/Controls/Admin/KeyType.ascx" TagName="KeyType" TagPrefix="uc1" %>
<asp:DropDownList ID="DropDownList" runat="server" DataTextField="Name" 
    DataValueField="idKeyValue" >
</asp:DropDownList>
<asp:Button ID="btnKeyValue" runat="server" Text="..." OnClick="btnKeyValue_Click" />

<uc1:KeyType ID="KeyType" runat="server" Visible="false" />
<asp:HiddenField ID="hdnUNIID" runat="server" />
<asp:HiddenField ID="hdnCustomField" runat="server" />
<asp:HiddenField id="hdnDefaultValue" runat="server"/>