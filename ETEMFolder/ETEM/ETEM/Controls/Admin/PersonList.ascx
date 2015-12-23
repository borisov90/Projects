<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PersonList.ascx.cs"
    Inherits="ETEM.Controls.Admin.PersonList" %>
<asp:Panel ID="pnlPerson" runat="server" Visible="false" CssClass="modalPopupSearch">
    <asp:GridView ID="gvPerson" runat="server" AutoGenerateColumns="False"
     CssClass="MainGrid" AllowSorting="true" OnSorting="gvPerson_OnSorting">
        <Columns>
            <asp:TemplateField HeaderText="№">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HiddenField ID="hdnRowMasterKey" runat="server" Value='<%# Bind("EntityID") %>' />
                    <asp:LinkButton ID="lnkBtnSelect" runat="server" CausesValidation="False" Text="Избор"
                        CommandArgument='<%# "EntityID=" +  Eval("EntityID") %>' OnClick="lnkBtnSelect_Click"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Име" SortExpression="FullName">
                <HeaderStyle HorizontalAlign="Left" />
                <HeaderTemplate>
                    <asp:Label ID="Label1" runat="server" Text="Име"></asp:Label>
                    <asp:TextBox ID="tbxFullName" runat="server" OnTextChanged="tbxFullName_TextChanged"></asp:TextBox>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("FullName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="IdentityNumber" HeaderText="ЕГН/ЛНК/ИДН" SortExpression="IdentityNumber" />
        </Columns>
        <PagerStyle CssClass="cssPager" />
    </asp:GridView>
    <asp:HiddenField ID="hdnCurrentCallerUniqueID" runat="server" />
    <asp:HiddenField ID="hdnCurrentCallerValueUniqueID" runat="server" />
</asp:Panel>
