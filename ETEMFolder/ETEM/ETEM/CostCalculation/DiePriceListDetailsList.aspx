<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DiePriceListDetailsList.aspx.cs" Inherits="ETEM.CostCalculation.DiePriceListDetailsList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspAjax" %>
<%@ Register Src="~/Controls/Common/SMCCalendar.ascx" TagName="SMCCalendar" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx" TagName="SMCDropDownList" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/CostCalculation/DiePriceListData.ascx" TagName="DiePriceListData" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/CostCalculation/DiePriceListDetailsData.ascx" TagName="DiePriceListDetailsData" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div id="btns-groups-holder">
        <div class="row">
            <div class="span6">
                <asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" CssClass="btn modalWindow" />
                <asp:Button ID="btnNewDiePriceList" runat="server" Text="New die price list" OnClick="btnNewDiePriceList_Click" CssClass="btn modalWindow" />
                <asp:Button ID="btnNewDiePriceListDetails" runat="server" Text="New die price list details" OnClick="btnNewDiePriceListDetails_Click" CssClass="btn modalWindow" />
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
            <asp:GridView ID="gvDiePriceListDetails" runat="server" CssClass="MainGrid" AutoGenerateColumns="False"
                OnSorting="gvDiePriceListDetails_Sorting" AllowSorting="true" AllowPaging="True" OnPageIndexChanging="gvDiePriceListDetails_PageIndexChanging"
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
                    <asp:BoundField DataField="VendorName" HeaderText="Vendor" SortExpression="VendorName" />
                    <asp:BoundField DataField="NumberOfCavitiesName" HeaderText="Number of cavities" SortExpression="NumberOfCavitiesName" HeaderStyle-Width="150px" />
                    <asp:BoundField DataField="ProfileComplexityName" HeaderText="Profile complexity" SortExpression="ProfileComplexityName" HeaderStyle-Width="145px" />
                    <asp:BoundField DataField="ProfileCategoryName" HeaderText="Profile category" SortExpression="ProfileCategoryName" HeaderStyle-Width="130px" />                    
                    <asp:BoundField DataField="DieDiemensions" HeaderText="Die diemensions" SortExpression="DieDiemensions" HeaderStyle-Width="135px" />
                    <asp:BoundField DataField="PriceRoundFormatted" HeaderText="Die price (EUR)" SortExpression="Price" ItemStyle-CssClass="MainGrid_td_item_right" HeaderStyle-Width="130px" />
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
    <uc3:DiePriceListData ID="ucDiePriceListData" runat="server" />
    <uc4:DiePriceListDetailsData ID="ucDiePriceListDetailsData" runat="server" />
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
                <div class="span8">
                    <p>
                        <asp:Label ID="lbVendor" runat="server" Text="Vendor"></asp:Label>
                    </p>
                    <uc2:SMCDropDownList ID="ddlVendor" runat="server" KeyTypeIntCode="Vendor"
                        ShowButton="false" OrderBy="V_Order" CssClassDropDown="span8" />
                </div>
            </div>
            <div class="row">
                <div class="span3">
                    <p>
                        <asp:Label ID="lbNumberOfCavities" runat="server" Text="Number of cavities"></asp:Label>
                    </p>
                    <uc2:SMCDropDownList ID="ddlNumberOfCavities" runat="server" KeyTypeIntCode="NumberOfCavities"
                        ShowButton="false" OrderBy="V_Order" />
                </div>
                <div class="span3">
                    <p>
                        <asp:Label ID="lbProfileCategory" runat="server" Text="Profile category"></asp:Label>
                    </p>
                    <uc2:SMCDropDownList ID="ddlProfileCategory" runat="server" KeyTypeIntCode="ProfileCategory"
                        ShowButton="false" OrderBy="V_Order" />
                </div>
                <div class="span3">
                    <p>
                        <asp:Label ID="lbProfileComplexity" runat="server" Text="Profile complexity"></asp:Label>
                    </p>
                    <uc2:SMCDropDownList ID="ddlProfileComplexity" runat="server" KeyTypeIntCode="ProfileComplexity"
                        ShowButton="false" OrderBy="V_Order" />
                </div>
            </div>
            <div class="row">
                <div class="span2">
                    <p>
                        <asp:Label ID="lbDimensionA_From" runat="server" Text="Dimension A (mm) from"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxDimensionA_From" runat="server" MaxLength="19" onkeypress="return isNumeric(event, false);"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbDimensionA_To" runat="server" Text="Dimension A (mm) to"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxDimensionA_To" runat="server" MaxLength="19" onkeypress="return isNumeric(event, false);"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbDimensionB_From" runat="server" Text="Dimension B (mm) from"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxDimensionB_From" runat="server" MaxLength="19" onkeypress="return isNumeric(event, false);"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbDimensionB_To" runat="server" Text="Dimension B (mm) to"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxDimensionB_To" runat="server" MaxLength="19" onkeypress="return isNumeric(event, false);"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="span2">
                    <p>
                        <asp:Label ID="lbDiePriceFrom" runat="server" Text="Die price (EUR) from"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxDiePriceFrom" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbDiePriceTo" runat="server" Text="Die price (EUR) to"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxDiePriceTo" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbLifespanFrom" runat="server" Text="Lifespan (ton) from"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxLifespanFrom" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbLifespanTo" runat="server" Text="Lifespan (ton) to"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxLifespanTo" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="span4">
                    <asp:Button ID="btnSearch" CssClass="btn closeModalWindow" runat="server" Text="Search"
                        OnClick="btnSearch_Click" />&nbsp;
                    <asp:Button ID="btnDefault" CssClass="btn" runat="server" Text="Default" OnClick="btnDefault_Click" />&nbsp;
                    <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Clear all" OnClick="btnClear_Click" />
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
