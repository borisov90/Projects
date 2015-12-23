<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DieFormulaList.aspx.cs" Inherits="ETEM.CostCalculation.DieFormulaList" %>

<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx" TagName="SMCDropDownList" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/CostCalculation/DieFormulaData.ascx" TagName="DieFormulaData" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div id="btns-groups-holder">
        <div class="row">
            <div class="span4">
                <asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" CssClass="btn modalWindow" />
                <asp:Button ID="btnNew" runat="server" Text="New formula" OnClick="btnNew_Click" CssClass="btn modalWindow" />
            </div>
            <div class="span4">
                <uc1:SMCDropDownList ID="ddlPagingRowsCount" runat="server" KeyTypeIntCode="PagingRowsCount"
                    ShowButton="false" OrderBy="V_Order" CssClassDropDown="width6-important"
                    AddingDefaultValue="false" KeyValueDataValueField="DefaultValue1" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlPagingRowsCount_SelectedIndexChanged" />
            </div>
        </div>
    </div>
    <div class="row-fluid">
        <div class="span12">
            <asp:GridView ID="gvDieFormulaList" runat="server" CssClass="MainGrid" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                OnSorting="gvDieFormulaList_Sorting" AllowSorting="true" AllowPaging="True" OnPageIndexChanging="gvDieFormulaList_PageIndexChanging"
                PageSize="10">

                <Columns>
                    <asp:TemplateField HeaderText="№" ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdnRowMasterKey" runat="server" Value='<%# Bind("idDieFormula") %>' />
                            <asp:LinkButton ID="lnkBtnServerEdit" runat="server" CausesValidation="False" Text=""
                                CssClass="modalWindow" ToolTip='<%# GetCaption("GridView_Edit") %>' CommandArgument='<%# "idRowMasterKey=" +  Eval("idDieFormula") %>'
                                OnClick="lnkBtnServerEdit_Click"><i class="icon-pencil"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="NumberOfCavitiesName" HeaderText="Number of cavities" SortExpression="NumberOfCavitiesName" ItemStyle-Width="300px" />
                    <asp:BoundField DataField="ProfileTypeName" HeaderText="Profile type" SortExpression="ProfileTypeName" ItemStyle-Width="300px" />
                    <asp:BoundField DataField="ProfileCategoryName" HeaderText="Profile category" SortExpression="ProfileCategoryName" ItemStyle-Width="300px" />
                    <asp:BoundField DataField="DieFormulaText" HeaderText="Die Formula" SortExpression="DieFormulaText" ItemStyle-Width="400px" Visible="false" />
                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="lbEmptyDataText" runat="server" Text='<%# GetCaption("GridView_EmptyDataRow") %>' />
                </EmptyDataTemplate>
                <PagerStyle CssClass="cssPager" />
            </asp:GridView>
        </div>
    </div>

    <uc2:DieFormulaData ID="ucDieFormula" runat="server" />

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
                        <asp:Label ID="lbProfileComplexity" runat="server" Text="Number of cavities:"></asp:Label>
                    </p>
                    <uc1:SMCDropDownList ID="ddlNumberCavitiesFilter" runat="server" KeyTypeIntCode="NumberOfCavities" OrderBy="V_Order" ShowButton="false" />
                </div>
                <div class="span4">
                    <p>
                        <asp:Label ID="lbProfileType" runat="server" Text="Type:"></asp:Label>
                    </p>
                    <uc1:SMCDropDownList ID="ddlProfileTypeFilter" runat="server" KeyTypeIntCode="ProfileType" OrderBy="V_Order" ShowButton="false" />
                </div>
                <div class="span4">
                    <p>
                        <asp:Label ID="lbProfileCategory" runat="server" Text="Category:"></asp:Label>
                    </p>
                    <uc1:SMCDropDownList ID="ddlProfileCategoryFilter" runat="server" KeyTypeIntCode="ProfileCategory" OrderBy="V_Order" ShowButton="false" />
                </div>
            </div>
            <div class="row">
                <div class="span2">
                    <asp:Button ID="btnSearch" CssClass="btn closeModalWindow" runat="server" Text="Search"
                        OnClick="btnSearch_Click" />
                </div>
                <div class="span2">
                    <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Clear" OnClick="btnClear_Click" />
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
