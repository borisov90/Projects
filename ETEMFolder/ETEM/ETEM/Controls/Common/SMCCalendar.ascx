<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SMCCalendar.ascx.cs" Inherits="ETEM.Controls.Common.SMCCalendar" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:TextBox ID="tbxDate" runat="server" ></asp:TextBox>
<asp:CalendarExtender ID="tbxDate_CalendarExtender" runat="server" Format="dd.MM.yyyy"
    Enabled="True" TargetControlID="tbxDate">
</asp:CalendarExtender>
