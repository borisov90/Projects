<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AppSetting.aspx.cs" Inherits="ETEM.Admin.AppSetting" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register src="../Controls/Admin/SettingMainData.ascx" tagname="SettingMainData" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style>
    @media print{@page {size: landscape}}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <asp:Button ID="btnNew"             runat="server" Text="New"       CssClass="btn" onclick="btnNew_Click" />
    <asp:Button ID="btnShowFilterPnl"   runat="server" Text="Filter"    CssClass="btn" onclick="btnShowFilterPnl_OnClick" />
    <asp:Button ID="btnPrint"           runat="server" Text="Print"     CssClass="btn" onclick="btnPrint_Click" />

    <asp:GridView ID="gvSetting" runat="server" CssClass="MainGrid" AllowSorting="true" OnSorting="gvSettings_Sorting"
    AllowPaging="true" OnPageIndexChanging="gvSettings_OnPageIndexChanging" PagerSettings-PageButtonCount="20"
        AutoGenerateColumns="false">
        <Columns>
            
            <asp:TemplateField HeaderText="№">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HiddenField ID="hdnRowMasterKey" runat="server" Value='<%# Bind("EntityID") %>' />
                    <asp:LinkButton ID="lnkBtnServerEdit" runat="server" CausesValidation="False" Text="" Width="8px"
                        CommandArgument='<%# "idRowMasterKey=" +  Eval("EntityID") %>' OnClick="lnkBtnServerEdit_Click"><i class="icon-pencil"></i></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>            
            <asp:BoundField DataField="SettingName"             HeaderText="Name"           SortExpression="SettingName" />
            <asp:BoundField DataField="SettingIntCode"          HeaderText="Code"           SortExpression="SettingIntCode" />
            <asp:BoundField DataField="SettingValue"            HeaderText="Value"          SortExpression="SettingValue" />
            <asp:BoundField DataField="SettingDefaultValue"     HeaderText="Default value"  SortExpression="SettingDefaultValue" />
        </Columns>
        <PagerStyle CssClass="cssPager" />
    </asp:GridView>
    <uc1:SettingMainData ID="SettingMainData" runat="server" />

        <asp:Panel ID="pnlFilter" runat="server" CssClass="modalPopup" Visible="false">
        <div class="newItemPopUp">
            <div class="offset01">
                <h4>
                    Filter data</h4>
            </div>
            <div class="pnl-size-icons">
                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/close3.png"
                    CssClass="closeModalWindow" OnClick="btnCancelParentPanel_OnClick" />
            </div>
        </div>
        <div class="container-fluid">
          
             <div class="row">
                <div class="span6">
                    <p>
                        <asp:Label ID="lbFilterDiscipline" runat="server" Text="Name"></asp:Label></p>
                    <asp:TextBox CssClass="span6" ID="tbxFilterName" runat="server"></asp:TextBox>
                </div>
          
                 <div class="span6">
                    <p>
                        <asp:Label ID="Label1" runat="server" Text="Code"></asp:Label></p>
                    <asp:TextBox CssClass="span6" ID="tbxFilterCode" runat="server"></asp:TextBox>
                </div>
            </div>

               <div class="row">
                <div class="span6">
                    <p>
                        <asp:Label ID="Label2" runat="server" Text="Value"></asp:Label></p>
                    <asp:TextBox CssClass="span6" ID="tbxFilterValue" runat="server"></asp:TextBox>
                </div>
          
                 <div class="span6">
                    <p>
                        <asp:Label ID="Label3" runat="server" Text="Default Value"></asp:Label></p>
                    <asp:TextBox CssClass="span6" ID="tbxFilterDefaultValue" runat="server"></asp:TextBox>
                </div>
            </div>

            <div class="row">
                <div class="span6">
                    <%--<asp:Button ID="btnFilterClose" runat="server" OnClick="btnFilterClose_OnClick" Text="Cancel" />--%>
                    <asp:Button ID="btnFilterSearch" runat="server" OnClick="btnFilterSearch_OnClick"
                        CssClass="btn closeModalWindow" Text="Search" />
                    <asp:Button ID="btnClearFilter" runat="server" OnClick="btnClearFilter_OnClick" Text="Clear"
                        CssClass="btn" />
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>


