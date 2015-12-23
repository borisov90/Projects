<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProfilesListCtrlChooseOffer.ascx.cs" Inherits="ETEM.Controls.CostCalculation.ProfilesListCtrlChooseOffer" %>

<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx"                       TagName="SMCDropDownList" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Common/SMCAutoCompleteTextBox.ascx"                TagName="SMCAutoComplete" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/CostCalculation/Profile.ascx" TagName="Profile"    TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit"       TagPrefix="asp" %>

<asp:Panel ID="pnlProfileListCtrl" runat="server"  CssClass="modalPopupInnerWidthLarge">
    <div class="newItemPopUp">
        <div class="offset01">
            <h4 id="H1" runat="server">
                Profile list</h4>
        </div>

<div id="btns-groups-holder">

    <div class="pnl-size-icons" id="divClose" runat="server">
            <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/close3.png"
                CssClass="closeModalWindow" OnClick="btnCancelParentPanel_OnClick" />
    </div>

    <div class="row">
        <div class="span6">        
            <asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" CssClass="btn modalWindow" />   
            <asp:Button ID="btnSelect" runat="server" Text="Select" OnClick="btnSelect_Click" CssClass="btn modalWindow" />   
        </div>

        <div class="span4">
            <uc1:SMCDropDownList ID="ddlPagingRowsCount" runat="server" KeyTypeIntCode="PagingRowsCount"
                ShowButton="false" OrderBy="V_Order" KeyValueDefault="TwentyRowsPerPage" CssClassDropDown="width6-important"
                AddingDefaultValue="false" KeyValueDataValueField="DefaultValue1" AutoPostBack="true"
                OnSelectedIndexChanged="ddlPagingRowsCount_SelectedIndexChanged" />
        </div>
    </div>
</div>

<div class="row-fluid">
    <div class="span12">
        <asp:GridView ID="gvProfilesList" runat="server" CssClass="MainGrid" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" AllowSorting="True" AllowPaging="True" 
            OnSorting="gvProfilesList_Sorting" OnPageIndexChanging="gvProfilesList_PageIndexChanging">

            <Columns>
                <asp:TemplateField HeaderText="№" ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                        <asp:HiddenField ID="hdnProfileName" runat="server" Value='<%# Bind("ProfileName") %>' />
                        <asp:HiddenField ID="hdnRowMasterKey" runat="server" Value='<%# Bind("idProfileSetting") %>' />
                    </ItemTemplate>
                    <ItemStyle CssClass="MainGrid_td_item_center" Width="24px"></ItemStyle>
                </asp:TemplateField>

                <asp:TemplateField ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                    <ItemTemplate>
                        <asp:CheckBox ID="chbxSelectProfile" runat="server"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:ImageField DataImageUrlField="ImagePath" HeaderText="Image"   ItemStyle-Width="110px" ControlStyle-Width="110" >
                </asp:ImageField>
                <asp:BoundField DataField="ProfileName" HeaderText="Profile name" SortExpression="ProfileName" ItemStyle-Width="300px">
                    <ItemStyle Width="300px"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="ProfileTypeName" HeaderText="Profile type" SortExpression="ProfileTypeName" ItemStyle-Width="300px">
                    <ItemStyle Width="300px"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="ProfileCategoryName" HeaderText="Profile category" SortExpression="ProfileCategoryName" ItemStyle-Width="300px">
                    <ItemStyle Width="300px"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="ProfileComplexityName" HeaderText="Profile complexity" SortExpression="ProfileComplexityName" ItemStyle-Width="300px">
                    <ItemStyle Width="300px"></ItemStyle>
                </asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Label ID="lbEmptyDataText" runat="server" Text='<%# GetCaption("GridView_EmptyDataRow") %>' />
            </EmptyDataTemplate>
            <PagerStyle CssClass="cssPager" />
        </asp:GridView>
    </div>
</div>



<uc3:Profile ID="ucProfile" runat="server" />


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
    <div class="container-fluid">
        <div class="ResultContext">
            <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
        </div>

        <div class="row">
            <div class="span4" >
                <p>
                    <asp:Label ID="lbProfileType" runat="server" Text="Type:"></asp:Label></p>
                    <uc1:SMCDropDownList ID="ddlProfileTypeFilter" runat="server" KeyTypeIntCode="ProfileType" ShowButton="false" />
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbProfileCategory" runat="server" Text="Category:"></asp:Label></p>
                    <uc1:SMCDropDownList ID="ddlProfileCategoryFilter" runat="server" KeyTypeIntCode="ProfileCategory" ShowButton="false" />
            </div>        
            <div class="span4">
                <p>
                    <asp:Label ID="lbProfileComplexity" runat="server" Text="Complexity:"></asp:Label></p>
                    <uc1:SMCDropDownList ID="ddlProfileComplexityFilter" runat="server" KeyTypeIntCode="ProfileComplexity" ShowButton="false" />
            </div>
        </div>


        <div class="row">
            <div class="span2">
                <asp:Button ID="btnSearch" CssClass="btn closeModalWindow" runat="server" Text="Search" OnClick="btnSearch_Click" />
            </div>
            <div class="span2">
                <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Clear" OnClick="btnClear_Click" />
            </div>
        </div>
    </div>
</asp:Panel>

    </div>
</asp:Panel>

<asp:HiddenField    ID="hdnOfferID"  runat="server" />