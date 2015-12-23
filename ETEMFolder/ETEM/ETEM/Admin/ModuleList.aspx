<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ModuleList.aspx.cs" Inherits="ETEM.Admin.ModuleList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<%@ Register src="../Controls/Admin/ModuleMainData.ascx" tagname="ModuleMainData" tagprefix="uc1" %>


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
    
    
    
    <asp:GridView ID="gvModule" runat="server" CssClass="MainGrid" AllowSorting="true"
        AllowPaging="true" AutoGenerateColumns="false" OnSorting="gvModule_Sorting"
        OnPageIndexChanging="gvModule_OnPageIndexChanging">
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
            <asp:BoundField DataField="ModuleName" HeaderText="Име на модул" SortExpression="ModuleName" />
            <asp:BoundField DataField="Comment" HeaderText="Коментар" SortExpression="Comment" />
            <asp:TemplateField ItemStyle-Width="24px" HeaderText="Необходима проверка" ItemStyle-CssClass="MainGrid_td_item_center">
                <ItemTemplate>
                    <asp:CheckBox ID="chbxNeedCheck" runat="server" Checked='<%# Bind("NeedCheck") %>' />
                </ItemTemplate>
                <ItemStyle CssClass="MainGrid_td_item_center" Width="24px" />
            </asp:TemplateField>
        </Columns>
        <PagerStyle CssClass="cssPager" />
    </asp:GridView>
    <uc1:ModuleMainData ID="ModuleMainData" runat="server" />
</asp:Content>
