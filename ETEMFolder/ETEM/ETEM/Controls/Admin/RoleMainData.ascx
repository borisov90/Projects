<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RoleMainData.ascx.cs"
    Inherits="ETEM.Controls.Admin.RoleMainData" %>
<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx" TagName="SMCDropDownList"
    TagPrefix="uc1" %>
<asp:Panel ID="pnlFormData" runat="server" Visible="false" CssClass="modalPopup90pc">
    <div class="newItemPopUp">
        <div class="offset01">
            <h4 id="H1" runat="server">
                Add a new role</h4>
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
        <div class="row-fluid">
            <div class="span2">
                <asp:Button ID="btnSave" CssClass="btn disableable" runat="server" Text="Save" OnClick="btnSave_Click" />
            </div>            
            <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
        </div>
        <div class="ResultContext">
            <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
        </div>
        <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="lbName" runat="server" Text="Name"></asp:Label></p>
                <asp:TextBox ID="tbxName" runat="server" CssClass="span4"></asp:TextBox>
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbIntCode" runat="server" Text="Internal code"></asp:Label></p>
                <asp:TextBox ID="tbxIntCode" runat="server" CssClass="span4"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span8">
                <p>
                    <asp:Label ID="lbDescription"       runat="server" Text="Description"></asp:Label></p>
                    <asp:TextBox ID="tbxDescription"    runat="server" TextMode="MultiLine" CssClass="TextBoxDescription span8"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span3">
                
                    <asp:Button ID="btnAdd" runat="server" CssClass="btn" Text="Add permitted actions"
                        OnClick="btnAdd_Click" />
             
            </div>
            <div class="span3">
               
                    <asp:Button ID="btnRemove" runat="server" CssClass="btn" Text="Remove permitted actions"
                        OnClick="btnRemove_Click" />
              
            </div>   
        </div>
        <div class="row-fluid">
            <div class="divCell600">
                <asp:Label ID="lbPermittedAction" runat="server" Text="Permitted actions"></asp:Label>
            </div>
        </div>
        <div class="tab-content">
            <div class="row">
                <div class="span12">
                    <asp:GridView ID="gvPermittedAction" runat="server" AutoGenerateColumns="false" CssClass="MainGrid"
                        AllowSorting="true" AllowPaging="false" OnSorting="gvPermittedAction_OnSorting"
                        OnPageIndexChanging="gvPermittedAction_OnPageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="№">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chbxPermittedAction" runat="server" />
                                    <asp:HiddenField ID="hdnPermittedActionID" runat="server" Value='<%# Bind("idPermittedAction") %>' />
                                    <asp:HiddenField ID="hdnRolePermittedActionID" runat="server" Value='<%# Bind("RolePermittedActionID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="RolePermittedActionID"   HeaderText="RolePermittedActionID"  SortExpression="RolePermittedActionID" Visible="false"/>
                            <asp:BoundField DataField="FrendlyName"             HeaderText="Name"                   SortExpression="FrendlyName" />
                            <asp:BoundField DataField="Description"             HeaderText="Description"            SortExpression="Description" />
                        </Columns>
                        <PagerStyle CssClass="cssPager" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>














    <asp:Panel ID="pnlPermittedActionToBeAdded" runat="server" CssClass="modalPopup90pc"
        Visible="false">
        <div class="newItemPopUp">
            <div class="offset01">
                <h4 id="H2" runat="server">
                    Search</h4>
            </div>
            <div class="pnl-size-icons">
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/close3.png"
                    CssClass="closeModalWindow" OnClick="btnCancelParentPanel_OnClick" />
            </div>
        </div>
        <div class="row span12Separator">
            <div class="span12">
            </div>
        </div>
        <div class="container-fluid">
            <div class="row">
                <div class="span2">
                    <asp:Button ID="btnAddPermittedAction" runat="server" Text="Add permitted action" OnClick="btnAddPermittedAction_Click"
                        CssClass="btn" />
                </div>
                <div class="span2">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                        CssClass="btn" />
                </div>
            </div>
            <div class="row">
                <div class="span4">
                    <p>
                        <asp:Label ID="lbModule" runat="server" Text="Module name"></asp:Label></p>
                        <uc1:SMCDropDownList ID="ddlModule" runat="server" DataSourceType="Modules" ShowButton="false" />
                </div>
            </div>
            <asp:TextBox ID="tbxSearchPermittedActions" runat="server" CssClass="span10"></asp:TextBox>
            <asp:GridView ID="gvPermittedActionToBeAdded" runat="server" AutoGenerateColumns="false"
                PageSize="20" AllowPaging="true" AllowSorting="true" OnSorting="gvPermittedActionToBeAdded_OnSorting"
                OnPageIndexChanging="gvPermittedActionToBeAdded_OnPageIndexChanging" CssClass="MainGrid">
                <Columns>
                    <asp:TemplateField HeaderText="№">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="cbxGridCheckboxAll" runat="server" OnCheckedChanged="cbxSelectOrDeselectAllGridItems_OnCheckedChanged"
                                CssClass="disableable" AutoPostBack="true" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cbxGridCheckbox" runat="server" />
                            <asp:HiddenField ID="hdnPermittedActionID" runat="server" Value='<%# Bind("idPermittedAction") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="FrendlyName" HeaderText="Name"           SortExpression="FrendlyName" />
                    <asp:BoundField DataField="Description" HeaderText="Description"    SortExpression="Description" />
                    <asp:BoundField DataField="ModuleName"  HeaderText="Module name"    SortExpression="ModuleName" />
                </Columns>
                <PagerStyle CssClass="cssPager" />
            </asp:GridView>
        </div>
    </asp:Panel>
</asp:Panel>
