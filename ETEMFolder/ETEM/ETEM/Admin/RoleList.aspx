<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RoleList.aspx.cs" Inherits="ETEM.Admin.RoleList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="~/Controls/Admin/RoleMainData.ascx" tagname="RoleMainData" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="row">
        <div class="span2">
            <asp:Button ID="btnNew" runat="server" Text="New" onclick="btnNew_Click" CssClass="btn modalWindow"/>
        </div>
    </div>
    <asp:GridView ID="gvRole" runat="server" CssClass="MainGrid" AllowSorting="true" OnSorting="gvRole_OnSorting"
    AllowPaging="true" OnPageIndexChanging="gvRole_OnPageIndexChanging" AutoGenerateColumns="false">
        <Columns>
            <asp:TemplateField ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center" HeaderText="№">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                <ItemTemplate>
                    <asp:HiddenField ID="hdnRowMasterKey" runat="server" Value='<%# Bind("EntityID") %>' />
                    
                    <asp:LinkButton ID="lnkBtnServerEdit" runat="server" CausesValidation="False" Text="" ToolTip='<%# GetCaption("GridView_Edit") %>' 
                        CommandArgument='<%# "idRowMasterKey=" +  Eval("EntityID") %>' OnClick="lnkBtnServerEdit_Click"><i class="icon-pencil"></i></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>            
            <asp:BoundField DataField="Name"        HeaderText="Name"           SortExpression="Name" />
            <asp:BoundField DataField="Description" HeaderText="Description"    SortExpression="Description" />            
        </Columns>
        <PagerStyle CssClass="cssPager" />
    </asp:GridView>
    <uc1:RoleMainData ID="RoleMainData" runat="server" />
</asp:Content>
