<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="OffersList.aspx.cs" Inherits="ETEM.CostCalculation.OffersList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../Controls/Common/AlphaBetCtrl.ascx" TagName="AlphaBetCtrl" TagPrefix="uc2" %>
<%@ Register Src="../Controls/CostCalculation/OfferMainData.ascx" TagName="OfferMainData" TagPrefix="uc1" %>
<%@ Register Src="../Controls/Common/SMCCalendar.ascx" TagName="SMCCalendar" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/CostCalculation/ProfilesListCtrlChooseOffer.ascx" TagName="ucProfilesListCtrlChooseOffer" TagPrefix="uc5" %>

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

        <div class="span12">
            <asp:GridView ID="gvOffer" runat="server" CssClass="MainGrid" AllowSorting="True"
                AllowPaging="True" AutoGenerateColumns="False" OnSorting="gvOffer_Sorting" OnPageIndexChanging="gvOffer_OnPageIndexChanging" PagerSettings-PageButtonCount="20">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center"
                        HeaderText="№">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>

                        <ItemStyle CssClass="MainGrid_td_item_center" Width="24px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdnRowMasterKey" runat="server" Value='<%# Bind("IdEntity") %>' />
                            <asp:LinkButton ID="lnkBtnServerEdit" runat="server" CausesValidation="False" Text=""
                                CssClass="modalWindow" CommandArgument='<%# "idRowMasterKey=" +  Eval("IdEntity") %>'
                                OnClick="lnkBtnServerEdit_Click"><i class="icon-pencil"></i></asp:LinkButton>
                        </ItemTemplate>

                        <ItemStyle CssClass="MainGrid_td_item_center" Width="24px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="InquiryNumber"   HeaderText="Inquiry No"     SortExpression="InquiryNumber" />
                    <asp:BoundField DataField="OfferDate"       HeaderText="Date"           SortExpression="OfferDate"          DataFormatString="{0:dd.MM.yyyy}" />
                    <asp:BoundField DataField="Customer"        HeaderText="Customer"       SortExpression="Customer" />
                    <asp:BoundField DataField="ProfileStr"      HeaderText="Profile"        SortExpression="ProfileStr" />
                    <asp:BoundField DataField="FullName"        HeaderText="Salesperson"   SortExpression="FullName" />

                    <asp:TemplateField HeaderText="Total sales price (EUR)" SortExpression="TotalSalesPriceStr">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("TotalSalesPriceStr") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" CssClass="MainGrid_td_item_right" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Offered sales price to customer (EUR)" SortExpression="OfferedSalesPriceToCustomer_EUR_Str" ItemStyle-Width="250px">
                        <ItemTemplate>
                            <asp:Label ID="lbOfferedSalesPriceToCustomer_EUR_Str" runat="server" Text='<%# Bind("OfferedSalesPriceToCustomer_EUR_Str") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" CssClass="MainGrid_td_item_right" />
                    </asp:TemplateField>

                </Columns>

                <PagerSettings PageButtonCount="20"></PagerSettings>

                <PagerStyle CssClass="cssPager" />
            </asp:GridView>
        </div>
    </div>
    <uc1:OfferMainData ID="OfferMainData1" runat="server" />
    <asp:Panel ID="pnlFilterData" runat="server" Visible="false" CssClass="modalPopup-middle">
        <div class="newItemPopUp">
            <div class="offset01">
                <h4>Filter data</h4>
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
                        <asp:Label ID="lbInquiryNumber" runat="server" Text="Inquiry No"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxInquiryNumber" runat="server"></asp:TextBox>
                </div>
                <div class="span4">
                    <p>
                        <asp:Label ID="lbCustomer" runat="server" Text="Customer"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxCustomer" runat="server"></asp:TextBox>
                </div>



            </div>
            <div class="row">
                <div class="span4">
                    <p>
                        <asp:Label ID="Label2" runat="server" Text="Inquiry date from"></asp:Label>
                    </p>
                    <uc4:SMCCalendar ID="tbxInquiryDateFrom" runat="server" />
                </div>

                <div class="span4">
                    <p>
                        <asp:Label ID="Label3" runat="server" Text="Inquiry date to"></asp:Label>
                    </p>
                    <uc4:SMCCalendar ID="tbxInquiryDateTo" runat="server" />
                </div>

            </div>
            <div class="row">
                <div class="span8">
                    <p>
                        <asp:Label ID="lbProfileSetting" runat="server" Text="Profile"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxProfileSettingName" runat="server" CssClass="span6" Enabled="false"></asp:TextBox>                    
                    <asp:Button ID="btnChoose" runat="server" Text="Choose Profile" CssClass="btn modalWindow mandatory" OnClick="btnChoose_Click" />
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
    <uc5:ucProfilesListCtrlChooseOffer ID="ucProfilesListCtrlChooseOffer" runat="server" Visible="false" />

    <asp:HiddenField ID="hdnIdProfileSettingText"   runat="server" />
    <asp:HiddenField ID="hdnProfileID"              runat="server" />
    <asp:HiddenField ID="hdnInquiryNumber"          runat="server" />
    <asp:HiddenField ID="hdnCustomer"               runat="server" />
    <asp:HiddenField ID="hdnInquiryDateFrom"        runat="server" />
    <asp:HiddenField ID="hdnInquiryDateTo"          runat="server" />    
    

</asp:Content>
