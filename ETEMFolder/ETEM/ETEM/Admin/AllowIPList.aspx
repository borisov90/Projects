<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="AllowIPList.aspx.cs" Inherits="ETEM.Admin.AllowIPList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../Controls/Admin/PersonMainData.ascx" TagName="PersonMainData"
    TagPrefix="uc1" %>
<%@ Register src="../Controls/Admin/AllowIPMainData.ascx" tagname="AllowIPMainData" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div id="btns-lecturers-holder">
        <div class="row">
            <div class="span2">
                <asp:Button ID="bntNew" runat="server" Text="New" OnClick="bntNew_Click" CssClass="btn" />
            </div>
        </div>
    </div>
    
    <asp:GridView ID="gvAllowIP" runat="server" CssClass="MainGrid" AllowSorting="true"
        AllowPaging="true" AutoGenerateColumns="false" OnSorting="gvAllowIP_Sorting"
        OnPageIndexChanging="gvAllowIP_OnPageIndexChanging">
        <Columns>
            <asp:TemplateField ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center"
                HeaderText="№">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                <ItemTemplate>
                    <asp:HiddenField ID="hdnRowMasterKey" runat="server" Value='<%# Bind("IdEntity") %>' />
                    <asp:LinkButton ID="lnkBtnServerEdit" runat="server" CausesValidation="False" Text=""
                        CssClass="modalWindow" CommandArgument='<%# "idRowMasterKey=" +  Eval("IdEntity") %>'
                        OnClick="lnkBtnServerEdit_Click"><i class="icon-pencil"></i></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="IP" HeaderText="IP Адрес" SortExpression="IP" />
            <asp:BoundField DataField="Commnet" HeaderText="Коментар" SortExpression="Commnet" />
            <asp:TemplateField ItemStyle-Width="24px" HeaderText="Разрешен" ItemStyle-CssClass="MainGrid_td_item_center">
                <ItemTemplate>
                    <asp:CheckBox ID="chbxAllow" runat="server" Checked='<%# Bind("Allow") %>' />
                </ItemTemplate>
                <ItemStyle CssClass="MainGrid_td_item_center" Width="24px" />
            </asp:TemplateField>
        </Columns>
        <PagerStyle CssClass="cssPager" />
    </asp:GridView>
    <uc2:AllowIPMainData ID="AllowIPMainData" runat="server" />
</asp:Content>
