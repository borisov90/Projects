<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="MenuNodePage.aspx.cs" Inherits="ETEM.Admin.MenuNodePage" %>
<%@ Register src="../Controls/Common/MenuNode.ascx" tagname="MenuNode" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <uc1:MenuNode ID="MenuNode" runat="server" />
</asp:Content>
