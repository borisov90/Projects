<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="KeyTypeList.aspx.cs" Inherits="ETEM.Admin.KeyTypeList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../Controls/Admin/KeyType.ascx" TagName="KeyType" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div id="btns-lecturers-holder">
        <div class="row">
            <div class="span2">
                <asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" CssClass="btn modalWindow" />
            </div>
            <div class="span2">
                <asp:Button ID="bntNewKeyType" runat="server" Text="New" OnClick="bntNewKeyType_Click" CssClass="btn newItemAdd modalWindow" />
            </div>
        </div>
    </div>
    <asp:GridView ID="gvKeyTypes" runat="server" CssClass="MainGrid" AutoGenerateColumns="False"
        AllowPaging="true" AllowSorting="True" OnSorting="gvKeyTypes_Sorting" OnPageIndexChanging="OnPageIndexChanging">
        <Columns>
            <asp:TemplateField ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center"
                HeaderText="№">
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
            <asp:BoundField DataField="Name"                HeaderText="Name"                   SortExpression="Name" ItemStyle-Width="200px"/>
            <asp:BoundField DataField="Description"         HeaderText="Description"            SortExpression="Description" />
            <asp:BoundField DataField="KeyTypeIntCode"      HeaderText="KeyTypeIntCode"         SortExpression="KeyTypeIntCode" ItemStyle-Width="100px"/>
            <asp:BoundField DataField="IsSystemAsString"    HeaderText="Is system as string"    SortExpression="IsSystemAsString" ItemStyle-Width="200px"/>
            <asp:BoundField DataField="idKeyType"           HeaderText="idKeyType"              SortExpression="idKeyType" ItemStyle-Width="100px"/>
        </Columns>
        <PagerStyle CssClass="cssPager" />
    </asp:GridView>
    <uc1:KeyType ID="KeyType" runat="server" Visible="false" />
    <asp:Panel ID="pnlFilterData" runat="server" Visible="false" CssClass="modalPopup-middle">

       <div class="newItemPopUp">
            <div class="offset01">
                <h4 id="H1" runat="server">
                Filter data</h4>
            </div>
            <div class="pnl-size-icons">
               
                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/close3.png" CssClass="closeModalWindow"
                    OnClick="btnCancelParentPanel_OnClick" />
            </div>
        </div>

      
        <div class="row span12Separator">
            <div class="span12">
            </div>
        </div>
        <div class="ResultContext">
            <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
        </div>
        <div class="container-fluid">
            <div class="row">
                <div class="span4">
                    <p>
                        <asp:Label ID="lbFilterName" runat="server" Text="Name of the nomenclature" /></p>
                    <asp:TextBox ID="tbxFilterName" runat="server"></asp:TextBox>
                </div>
                <div class="span4">
                    <p>
                        <asp:Label ID="lbFilterKeyType" runat="server" Text="KeyTypeIntCode" /></p>
                    <asp:TextBox ID="tbxFilterKeyType" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="span8">
                    <p>
                        <asp:Label ID="lbFullTextSearch" runat="server" Text="Value" /></p>
                    <asp:TextBox ID="tbxFullTextSearch" runat="server" TextMode="MultiLine"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="span2">
                    <asp:Button ID="btnSearch" CssClass="btn closeModalWindow" runat="server" Text="Search" OnClick="btnSearch_Click" />
                </div>
             <%--   <div class="span2">
                    <asp:Button ID="btnCancel" CssClass="btn" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                </div>--%>
                <div class="span2">
                    <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Clear" OnClick="btnClear_Click" />
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
