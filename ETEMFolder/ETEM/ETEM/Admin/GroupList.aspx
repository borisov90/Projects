<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="GroupList.aspx.cs" Inherits="ETEM.Admin.GroupList" %>

<%@ Register Src="~/Controls/Admin/GroupData.ascx"                      TagName="GroupData"         TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Common/SMCAutoCompleteTextBox.ascx"        TagName="SMCAutoComplete"   TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit"                           TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div id="btns-groups-holder">
        <div class="row">
            <div class="span2">
                <asp:Button ID="btnFilter" runat="server" Text ="Filter" OnClick="btnFilter_Click" CssClass="btn modalWindow" />
            </div>
            <div class="span2">
                <asp:Button ID="btnNew" runat="server" Text="New" OnClick="btnNew_Click" CssClass="btn newItemAdd modalWindow"/>
            </div>
        </div>
    </div>
    <asp:GridView ID="gvGroups" runat="server" CssClass="MainGrid" AllowSorting="true"
        OnSorting="gvGroups_Sorting" AllowPaging="true" OnPageIndexChanging="gvGroups_OnPageIndexChanging"
        AutoGenerateColumns="False" ShowHeaderWhenEmpty="true">
        <Columns>
            <asp:TemplateField HeaderText="№" ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                <ItemTemplate>
                    <asp:HiddenField ID="hdnRowMasterKey" runat="server" Value='<%# Bind("EntityID") %>' />
                    <asp:LinkButton ID="lnkBtnServerEdit" runat="server" CssClass="modalWindow" CausesValidation="False"
                        Text="" ToolTip='<%# GetCaption("GridView_Edit")%>' CommandArgument='<%# "idRowMasterKey=" + Eval("EntityID") %>' OnClick="lnkBtnServerEdit_Click"><i
                        class="icon-pencil"></i></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="GroupName" HeaderText="Име на група" SortExpression="GroupName" />
        </Columns>
        <EmptyDataTemplate>
            <asp:Label ID="lbEmptyDataText" runat="server" Text='<%# GetCaption("GridView_EmptyDataRow") %>' />
        </EmptyDataTemplate>
        <PagerStyle CssClass="cssPager" />
    </asp:GridView>

    <uc1:GroupData        ID="GroupData"        runat="server" />

    <asp:Panel ID="pnlFilterData" runat="server" Visible="false" CssClass="modalPopup-middle">
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
        <div class="row span12Separator">
            <div class="span12">
            </div>
        </div>
        <div class="container-fluid">
            <div class="ResultContext">
                <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
            </div>
            
            <div class="row">
                <div class="span8">
                    <p>
                        <asp:Label ID="lbPerson" runat="server" Text="Служител:" /></p>
                        <uc2:SMCAutoComplete ID="acFilterPerson" runat="server" CustomCase="PersonEmployeeAndLecturer"
                            CssClassTextBox="span8" />
                </div>
            </div>
            <div class="row">
                <div class="span8">
                    <p>
                        <asp:Label ID="lbFilterGroupName" runat="server" Text="Име на група:" /></p>
                        <asp:TextBox ID="tbxFilterGroupName" runat="server" CssClass="textBox span8" />
                </div>                
            </div>
            
            <div class="row">
                <div class="span2">
                    <asp:Button ID="btnSearch" CssClass="btn closeModalWindow" runat="server" OnClick="btnSearch_Click"
                        Text="Search" />
                </div>
                <div class="span2">
                    <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Clear" OnClick="btnClear_Click" />
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>

