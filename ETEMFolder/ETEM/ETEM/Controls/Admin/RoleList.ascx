<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RoleList.ascx.cs" Inherits="ETEM.Controls.Admin.RoleList" %>
<asp:Panel ID="pnlList" runat="server" Visible="true">
    <div class="row">
        <div class="span4">
            <asp:Button ID="btnShowRoles" runat="server" Text="Add role" CssClass="btn modalWindow"
                OnClick="btnShowRoles_Click" />
            <asp:Button ID="btnRemove" runat="server" Text="Remove role" CssClass="btn" OnClick="btnRemove_Click" />
        </div>
    </div>
    <asp:GridView ID="gvAddedRoles" runat="server" CssClass="MainGrid" AutoGenerateColumns="false"
        AllowSorting="true" OnSorting="gvAddedRoles_OnSorting">
        <Columns>
            <asp:TemplateField HeaderText="№">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HiddenField ID="hdnRowMasterKey" runat="server" Value='<%# Bind("EntityID") %>' />
                    <asp:CheckBox ID="chbxIdRole" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField SortExpression="Name"           DataField="Name"            HeaderText="Role" />
            <asp:BoundField SortExpression="Description"    DataField="Description"     HeaderText="Description" />
        </Columns>
        <PagerStyle CssClass="cssPager" />
    </asp:GridView>
    <asp:HiddenField ID="hdnCurrentCallerUniqueID"          runat="server" />
    <asp:HiddenField ID="hdnCurrentCallerValueUniqueID"     runat="server" />
    <asp:HiddenField ID="hdnCurrentUserID"                  runat="server" />
</asp:Panel>
<asp:Panel ID="pnlRoles" runat="server" CssClass="modalPopup" Visible="false">
    <div class="newItemPopUp">
        <div class="offset01">
            <h4 id="H3" runat="server">
                Adding a role</h4>
        </div>
        <div class="pnl-size-icons">
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/close3.png"
                CssClass="closeModalWindow" OnClick="btnCancelParentPanel_OnClick" />
        </div>
    </div>
    <div class="container-fluid">
         <div class="row span12Separator">
            <div class="span12">
            </div>
        </div>
        <div class="row">
            <div class="span3">
                <p>
                    <asp:Label      ID="lbFiltername"   runat="server" Text="Name"></asp:Label></p>
                    <asp:TextBox    ID="tbxFilterName"  runat="server" CssClass="span3"></asp:TextBox>
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="Label1" runat="server" Text="Description"></asp:Label></p>
                    <asp:TextBox ID="tbxFilterDescription" runat="server" CssClass="span3"></asp:TextBox>
            </div>
            <div class="span2">
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn" OnClick="btnSearch_Click" />
            </div>
        </div>
        <div class="row">
            <div class="span4">
                <asp:Button ID="btnAdd" runat="server" Text="Add selected roles" CssClass="btn"
                    OnClick="btnAdd_Click" />
            </div>
        </div>
        <asp:GridView ID="gvRoles" CssClass="MainGrid" runat="server" AutoGenerateColumns="false"
            AllowPaging="True"  OnPageIndexChanging="gvRoles_PageIndexChanging"
            AllowSorting="true" OnSorting="gvRoles_OnSorting">
            <Columns>
                <asp:TemplateField HeaderText="№">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HiddenField ID="hdnRowMasterKey" runat="server" Value='<%# Bind("EntityID") %>' />
                        <asp:CheckBox ID="chbxIdRole" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField SortExpression="Name"           DataField="Name"        HeaderText="Role" />
                <asp:BoundField SortExpression="Description"    DataField="Description" HeaderText="Description" />
            </Columns>
            <PagerStyle CssClass="cssPager" />
        </asp:GridView>
    </div>
</asp:Panel>
