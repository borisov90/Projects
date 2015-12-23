<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="PersonList.aspx.cs" Inherits="ETEM.Admin.PersonList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../Controls/Admin/PersonMainData.ascx" TagName="PersonMainData" TagPrefix="uc1" %>
<%@ Register Src="../Controls/Common/AlphaBetCtrl.ascx" TagName="AlphaBetCtrl" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div id="btns-lecturers-holder">
        <div class="row">
             <div class="span2">
                <asp:Button ID="btnFilter" runat="server" CssClass="btn modalWindow" Text="Filter"
                    OnClick="btnFilter_Click" />
            </div>
            <div class="span2">
                <asp:Button ID="bntNew" runat="server" Text="New" OnClick="bntNew_Click" CssClass="btn newItemAdd" />
            </div>
        </div>
    </div>
    <div class="row-fluid">
        <div class="span1">
            <uc2:alphabetctrl id="AlphaBetCtrl" runat="server" />
        </div>
        <div class="span11">
            <asp:GridView ID="gvPerson" runat="server" CssClass="MainGrid" AllowSorting="true"
                AllowPaging="true" AutoGenerateColumns="false" OnSorting="gvPerson_Sorting" OnPageIndexChanging="gvPerson_OnPageIndexChanging" PagerSettings-PageButtonCount="20">
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
                    <asp:BoundField DataField="FullName" HeaderText="Full name" SortExpression="FullName" />
                </Columns>
                <PagerStyle CssClass="cssPager" />
            </asp:GridView>
        </div>
    </div>
    <uc1:PersonMainData ID="PersonMainData" runat="server" NoLoadUserPanel="True" />
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
                <div class="span4">
                    <p>
                        <asp:Label ID="lbFirstName" runat="server" Text="First name"></asp:Label></p>
                    <asp:TextBox ID="tbxFirstName" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="span4">
                    <p>
                        <asp:Label ID="lbSecondName" runat="server" Text="Second name"></asp:Label></p>
                    <asp:TextBox ID="tbxSecondName" runat="server"></asp:TextBox>
                </div>
                <div class="span4">
                    <p>
                        <asp:Label ID="lbLastName" runat="server" Text="Last name"></asp:Label></p>
                    <asp:TextBox ID="tbxLastName" runat="server"></asp:TextBox>
                </div>
            </div>
            
            <div class="row">
                <div class="span2">
                    <asp:Button ID="btnSearch" CssClass="btn closeModalWindow" runat="server" Text="Search" OnClick="btnSearch_Click" />
                </div>
                <%--<div class="span2">
                    <asp:Button ID="btnCancel" CssClass="btn" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                </div>--%>
                <div class="span2">
                    <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Clear" OnClick="btnClear_Click" />
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
