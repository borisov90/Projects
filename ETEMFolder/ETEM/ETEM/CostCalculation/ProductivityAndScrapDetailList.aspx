﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ProductivityAndScrapDetailList.aspx.cs" Inherits="ETEM.CostCalculation.ProductivityAndScrapDetailList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspAjax" %>
<%@ Register Src="~/Controls/Common/SMCCalendar.ascx" TagName="SMCCalendar" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx" TagName="SMCDropDownList" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/CostCalculation/ProductivityAndScrapData.ascx" TagName="ProductivityAndScrapData" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/CostCalculation/ProductivityAndScrapDetailData.ascx" TagName="ProductivityAndScrapDetailData" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div id="btns-groups-holder">
        <div class="row">
            <div class="span8">
                <asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" CssClass="btn modalWindow" />
                <asp:Button ID="btnNew" runat="server" Text="New productivity and scrap by period" OnClick="btnNew_Click" CssClass="btn modalWindow" />
                <asp:Button ID="btnNewDetail" runat="server" Text="New productivity and scrap by press and profile" OnClick="btnNewDetail_Click" CssClass="btn modalWindow" />
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
            <asp:GridView ID="gvProductivityAndScrapDetail" runat="server" CssClass="MainGrid" AutoGenerateColumns="False"
                OnSorting="gvProductivityAndScrapDetail_Sorting" AllowSorting="true" AllowPaging="True" OnPageIndexChanging="gvProductivityAndScrapDetail_PageIndexChanging"
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
                    <asp:BoundField DataField="CostCenterName" HeaderText="Press" SortExpression="CostCenterName" />
                    <asp:BoundField DataField="ProfileSettingName" HeaderText="Shape" SortExpression="ProfileSettingName" />
                    <asp:BoundField DataField="SumOfHours_RoundFormatted" HeaderText="Sum of hours" SortExpression="SumOfHours" ItemStyle-CssClass="MainGrid_td_item_right" />
                    <asp:BoundField DataField="SumOfConsumption_RoundFormatted" HeaderText="Sum of consumption" SortExpression="SumOfConsumption" ItemStyle-CssClass="MainGrid_td_item_right" />
                    <asp:BoundField DataField="SumOfProduction_RoundFormatted" HeaderText="Sum of production" SortExpression="SumOfProduction" ItemStyle-CssClass="MainGrid_td_item_right" />
                    <asp:BoundField DataField="ProductivityKGh_RoundFormatted" HeaderText="Productivity KG/h" SortExpression="ProductivityKGh" ItemStyle-CssClass="MainGrid_td_item_right" />
                    <asp:BoundField DataField="ScrapRatePercent_RoundFormatted" HeaderText="Scrap rate %" SortExpression="ScrapRate" ItemStyle-CssClass="MainGrid_td_item_right" />
                    <asp:BoundField DataField="DateFromString" HeaderText="Valid from" SortExpression="DateFrom" HeaderStyle-Width="100px" ItemStyle-CssClass="MainGrid_td_item_center" />
                    <asp:BoundField DataField="DateToString" HeaderText="Valid to" SortExpression="DateTo" HeaderStyle-Width="100px" ItemStyle-CssClass="MainGrid_td_item_center" />
                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" HeaderStyle-Width="70px" />
                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="lbEmptyDataText" runat="server" Text="No data to display!" />
                </EmptyDataTemplate>
                <PagerStyle CssClass="cssPager" />
            </asp:GridView>
        </div>
    </div>
    <uc3:ProductivityAndScrapData ID="ucProductivityAndScrapData" runat="server" />
    <uc4:ProductivityAndScrapDetailData ID="ucProductivityAndScrapDetailData" runat="server" />
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
                <div class="span4">
                    <p>
                        <asp:Label ID="lbCostCenter" runat="server" Text="Cost center"></asp:Label>
                    </p>
                    <uc2:SMCDropDownList ID="ddlCostCenter" runat="server" DataSourceType="PRESS" KeyTypeIntCode="CostCenter"
                        ShowButton="false" OrderBy="V_Order" CssClassDropDown="span4" KeyValueDataTextField="DefaultValue4" />
                </div>
                <div class="span4">
                    <p>
                        <asp:Label ID="lbProfileSetting" runat="server" Text="Profile setting"></asp:Label>
                    </p>
                    <uc2:SMCDropDownList ID="ddlProfileSetting" runat="server" DataSourceType="ProfileSetting"
                        ShowButton="false" CssClassDropDown="span4" />
                </div>
            </div>
            <div class="row">
                <div class="span2">
                    <p>
                        <asp:Label ID="lbSumOfHoursFrom" runat="server" Text="Sum of hours from"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxSumOfHoursFrom" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbSumOfHoursTo" runat="server" Text="Sum of hours to"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxSumOfHoursTo" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbSumOfConsumptionFrom" runat="server" Text="Sum of consump. from"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxSumOfConsumptionFrom" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbSumOfConsumptionTo" runat="server" Text="Sum of consump. to"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxSumOfConsumptionTo" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="span2">
                    <p>
                        <asp:Label ID="lbSumOfProductionFrom" runat="server" Text="Sum of production from"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxSumOfProductionFrom" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbSumOfProductionTo" runat="server" Text="Sum of production to"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxSumOfProductionTo" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="span2">
                    <p>
                        <asp:Label ID="lbProductivityKGhFrom" runat="server" Text="Productivity KG/h from"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxProductivityKGhFrom" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbProductivityKGhTo" runat="server" Text="Productivity KG/h to"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxProductivityKGhTo" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbScrapRateFrom" runat="server" Text="Scrap Rate % from"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxScrapRateFrom" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbScrapRateTo" runat="server" Text="Scrap Rate % to"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxScrapRateTo" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
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
