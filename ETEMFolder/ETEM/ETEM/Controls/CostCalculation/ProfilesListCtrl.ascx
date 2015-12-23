<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProfilesListCtrl.ascx.cs" Inherits="ETEM.Controls.CostCalculation.ProfilesListCtrl" %>

<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx"                       TagName="SMCDropDownList"   TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Common/SMCAutoCompleteTextBox.ascx"                TagName="SMCAutoComplete"   TagPrefix="uc2" %>
<%@ Register Src="~/Controls/CostCalculation/Profile.ascx" TagName="Profile"    TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit"       TagPrefix="asp" %>

<%--<asp:Panel ID="pnlProfileListCtrl" runat="server"  CssClass="modalPopupInnerWidthLarge">--%>
   <%-- <div runat="server" id="dvMain" class="newItemPopUp">--%>

<div id="btns-groups-holder">

    <div class="row">
        <div class="span2">
            
            <asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" CssClass="btn modalWindow" />            
            <asp:Button ID="btnNew" runat="server" Text="New profile" OnClick="btnNew_Click" CssClass="btn modalWindow" />
        </div>

        <div class="span2">
            <uc1:SMCDropDownList ID="ddlPagingRowsCount" runat="server" KeyTypeIntCode="PagingRowsCount"
                ShowButton="false" OrderBy="V_Order" CssClassDropDown="width6-important"
                AddingDefaultValue="false" KeyValueDataValueField="DefaultValue1" AutoPostBack="true"
                OnSelectedIndexChanged="ddlPagingRowsCount_SelectedIndexChanged" />
        </div>
    </div>
</div>

<div class="row-fluid">
   
    <div class="span11">
        <asp:GridView ID="gvProfilesList" runat="server" CssClass="MainGrid" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" AllowSorting="True" AllowPaging="True" 
            OnSorting="gvProfilesList_Sorting" OnPageIndexChanging="gvProfilesList_PageIndexChanging">

            <Columns>
                <asp:TemplateField HeaderText="№" ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                    <ItemStyle CssClass="MainGrid_td_item_center" Width="24px"></ItemStyle>
                </asp:TemplateField>



                <asp:TemplateField ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdnRowMasterKey" runat="server" Value='<%# Bind("idProfileSetting") %>' />
                        <asp:LinkButton ID="lnkBtnServerEdit" runat="server" CausesValidation="False" Text=""
                            CssClass="modalWindow" ToolTip='<%# GetCaption("GridView_Edit") %>' CommandArgument='<%# "idRowMasterKey=" +  Eval("idProfileSetting") %>'
                            OnClick="lnkBtnServerEdit_Click"><i class="icon-pencil"></i></asp:LinkButton>
                    </ItemTemplate>

                    <ItemStyle CssClass="MainGrid_td_item_center" Width="24px"></ItemStyle>
                </asp:TemplateField>

                <asp:ImageField DataImageUrlField="ImagePath" HeaderText="Image" ItemStyle-Width="100px" ControlStyle-Width="100" ControlStyle-Height = "100">
                </asp:ImageField>

                <asp:BoundField DataField="ProfileName"             HeaderText="Profile name"       SortExpression="ProfileName"            ItemStyle-Width="300px">
                    
                </asp:BoundField>
                <asp:BoundField DataField="ProfileTypeName"         HeaderText="Profile type"       SortExpression="ProfileTypeName"        ItemStyle-Width="300px">
                </asp:BoundField>
                <asp:BoundField DataField="ProfileCategoryName"     HeaderText="Profile category"   SortExpression="ProfileCategoryName"    ItemStyle-Width="300px">
                </asp:BoundField>
                <asp:BoundField DataField="ProfileComplexityName"   HeaderText="Profile complexity" SortExpression="ProfileComplexityName"  ItemStyle-Width="300px">
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

<%--    </div>--%>
<%--</asp:Panel>--%>

<asp:HiddenField    ID="hdnOfferID"  runat="server" />