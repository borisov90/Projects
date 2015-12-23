<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="Welcome.aspx.cs" Inherits="ETEM.Share.Welcome"  %>
	
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    Welcome,
    <asp:Label ID="lbUserName" runat="server" Text=""></asp:Label>    
</asp:Content>
