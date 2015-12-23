<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="PermittedActionList.aspx.cs" Inherits="ETEM.Admin.PermittedActionList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/Admin/PermittedActionMainData.ascx" TagName="PermittedActionMainData"
    TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx" TagName="SMCDropDownList"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="row">
        <div class="span2">
            <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn modalWindow"
                OnClick="btnFilter_OnClick" />
        </div>
        <div class="span2">
            <asp:Button ID="btnMarge" runat="server" Text="Update" OnClick="btnMarge_Click"
                CssClass="btn" />
        </div>
    </div>
    <asp:GridView ID="gvPermittedAction" runat="server" CssClass="MainGrid" AllowSorting="true"
        OnSorting="gvPermittedAction_OnSorting" AutoGenerateColumns="false" AllowPaging="true"
        OnPageIndexChanging="gvPermittedAction_OnPageIndexChanging">
        <Columns>
            <asp:TemplateField HeaderText="№" ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                <ItemTemplate>
                    <asp:HiddenField ID="hdnRowMasterKey" runat="server" Value='<%# Bind("EntityID") %>' />
                    <asp:LinkButton ID="lnkBtnServerEdit" runat="server" CausesValidation="False" Text=""
                        ToolTip='<%# GetCaption("GridView_Edit") %>' CommandArgument='<%# "idRowMasterKey=" +  Eval("EntityID") %>'
                        OnClick="lnkBtnServerEdit_Click"><i class="icon-pencil"></i></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ModuleName"      HeaderText="Module name"                     SortExpression="ModuleName" />
            <asp:BoundField DataField="SecuritySetting" HeaderText="Security setting code"          SortExpression="SecuritySetting" />
            <asp:BoundField DataField="FrendlyName"     HeaderText="Action"                         SortExpression="FrendlyName" />
            <asp:BoundField DataField="Description"     HeaderText="Description"                    SortExpression="Description" />
        </Columns>
        <PagerStyle CssClass="cssPager" />
    </asp:GridView>
    <uc1:PermittedActionMainData ID="PermittedActionMainData" runat="server" />
    <asp:Panel runat="server" ID="pnlFilter" Visible="false" CssClass="modalPopup">
        <div class="newItemPopUp">
            <div class="offset01">
                <h4>
                    Filter data</h4>
            </div>
            <div class="pnl-size-icons">
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/close3.png"
                    CssClass="closeModalWindow" OnClick="btnCancelParentPanel_OnClick" />
            </div>
        </div>
        <div class="container-fluid">
            <div class="row span12Separator">
                <div class="span12">
                </div>
            </div>
            <div class="ResultContext">
                <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
            </div>
            <div class="row">
                <div class="span4">
                    <p>
                        <asp:Label ID="lbModule" runat="server" Text="Module"></asp:Label></p>
                    <uc1:SMCDropDownList ID="ddlModule" runat="server" DataSourceType="Modules" ShowButton="false" />
                </div>
            </div>
            <div class="row">
                <div class="span10">
                    <p>
                        <asp:Label ID="lbName" runat="server" Text="Name"></asp:Label></p>
                    <asp:TextBox ID="tbxFrendlyName" runat="server" CssClass="TextBoxDescription span10"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="span10">
                    <p>
                        <asp:Label ID="lbDescription" runat="server" Text="Description"></asp:Label></p>
                    <asp:TextBox ID="tbxDescription" runat="server" TextMode="MultiLine" CssClass="TextBoxDescription span10"></asp:TextBox>
                </div>
            </div>  
              <div class="row">
                <div class="span10">
                    <p>
                        <asp:Label ID="Label1" runat="server" Text="Security setting code"></asp:Label></p>
                    <asp:TextBox ID="tbxFilterSecuritySetting" runat="server" TextMode="MultiLine" CssClass="TextBoxDescription span10"></asp:TextBox>
                </div>
            </div>  
                  
            <div class="row">
                <div class="span2">
                    <asp:Button ID="btnFilterData" runat="server" Text="Search" OnClick="btnFilterData_OnClick"
                        CssClass="btn closeModalWindow" />
                </div>
                <div class="span2">
                    <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_OnClick"
                        CssClass="btn closeModalWindow" />
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
