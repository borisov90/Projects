﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="MaterialPricesList.aspx.cs" Inherits="ETEM.CostCalculation.MaterialPricesList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspAjax" %>
<%@ Register Src="~/Controls/Common/SMCCalendar.ascx" TagName="SMCCalendar" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx" TagName="SMCDropDownList" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/CostCalculation/MaterialPriceListData.ascx" TagName="MaterialPriceListData" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div id="btns-groups-holder">
        <div class="row">
            <div class="span6">
                <asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" CssClass="btn modalWindow" />
                <asp:Button ID="btnNew" runat="server" Text="New material price list" OnClick="btnNew_Click" CssClass="btn modalWindow" />
                <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" CssClass="btn" />
                <aspAjax:ConfirmButtonExtender ID="cbeBtnDelete" runat="server" TargetControlID="btnDelete"
                    ConfirmText="Are you sure you want to delete selected rows?">
                </aspAjax:ConfirmButtonExtender>
            </div>
            <div class="span4">
                <uc2:SMCDropDownList ID="ddlPagingRowsCount" runat="server" KeyTypeIntCode="PagingRowsCount"
                    ShowButton="false" OrderBy="V_Order" CssClassDropDown="width6-important"
                    AddingDefaultValue="false" KeyValueDataValueField="DefaultValue1" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlPagingRowsCount_SelectedIndexChanged" />
            </div>
        </div>
    </div>
    <div class="row-fluid">
        <div class="span12">
            <asp:GridView ID="gvMaterialPriceList" runat="server" CssClass="MainGrid" AutoGenerateColumns="False"
                OnSorting="gvMaterialPriceList_Sorting" AllowSorting="true" AllowPaging="True" OnPageIndexChanging="gvMaterialPriceList_PageIndexChanging"
                PageSize="10">
                <Columns>
                    <asp:TemplateField HeaderText="No" ItemStyle-Width="20px">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chbxCheckOrUncheckAll" runat="server" OnCheckedChanged="chbxCheckOrUncheckAll_OnCheckedChanged"
                                CssClass="disableable" AutoPostBack="true" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chbxCheckForDeletion" runat="server" />
                            <asp:HiddenField ID="hdnIdEntity" runat="server" Value='<%# Bind("IdEntity") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdnRowMasterKey" runat="server" Value='<%# Bind("IdEntity") %>' />
                            <asp:LinkButton ID="lnkBtnServerEdit" runat="server" CausesValidation="False" Text="" CssClass="modalWindow"
                                ToolTip='<%# GetCaption("GridView_Edit") %>' CommandArgument='<%# "idRowMasterKey=" + Eval("IdEntity") %>'
                                OnClick="lnkBtnServerEdit_Click">
                                <i class="icon-pencil"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="LME_RoundFormatted" HeaderText="LME (EUR/ton)" SortExpression="LME" />
                    <asp:BoundField DataField="PremiumRoundFormatted" HeaderText="Premium (EUR/ton)" SortExpression="Premium" />
                    <asp:BoundField DataField="DateFromString" HeaderText="Valid from" SortExpression="DateFrom" HeaderStyle-Width="150px" ItemStyle-CssClass="MainGrid_td_item_center" />
                    <asp:BoundField DataField="DateToString" HeaderText="Valid to" SortExpression="DateTo" HeaderStyle-Width="150px" ItemStyle-CssClass="MainGrid_td_item_center" />
                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" HeaderStyle-Width="80px" />
                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="lbEmptyDataText" runat="server" Text="No data to display!" />
                </EmptyDataTemplate>
                <PagerStyle CssClass="cssPager" />
            </asp:GridView>
        </div>
    </div>
    <uc3:MaterialPriceListData ID="ucMaterialPriceListData" runat="server" />
    <asp:Panel ID="pnlFilterData" runat="server" Visible="false" CssClass="modalPopup-middle">
        <div class="newItemPopUp">
            <div class="offset01">
                <h4>Filter data</h4>
            </div>
            <div class="pnl-size-icons">
                <asp:ImageButton ID="imgBtnCancelFilter" runat="server" ImageUrl="~/Images/close3.png"
                    CssClass="closeModalWindow" OnClick="btnCancelFilter_OnClick" />
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
                        <asp:Label ID="lbDateFromTo" runat="server" Text="Active to"></asp:Label>
                    </p>
                    <uc1:SMCCalendar ID="tbxDateFromTo" runat="server" />
                </div>
                <div class="span4">
                    &nbsp;
                </div>
            </div>
            <div class="row">
                <div class="span2">
                    <p>
                        <asp:Label ID="lbLME_From" runat="server" Text="LME (EUR/ton) from"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxLME_From" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbLME_To" runat="server" Text="LME (EUR/ton) to"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxLME_To" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbPremiumFrom" runat="server" Text="Premium (EUR/ton) from"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxPremiumFrom" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbPremiumTo" runat="server" Text="Premium (EUR/ton) to"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxPremiumTo" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="span4">
                    <asp:Button ID="btnSearch" CssClass="btn closeModalWindow" runat="server" Text="Search"
                        OnClick="btnSearch_Click" />&nbsp;
                    <asp:Button ID="btnDefault" CssClass="btn" runat="server" Text="Default" OnClick="btnDefault_Click" />&nbsp;
                    <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Clear" OnClick="btnClear_Click" />
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>