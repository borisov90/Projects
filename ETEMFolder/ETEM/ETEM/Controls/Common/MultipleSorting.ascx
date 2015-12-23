<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultipleSorting.ascx.cs"
    Inherits="ETEM.Controls.Common.MultipleSorting" %>
<asp:Panel ID="pnlSorting" runat="server" Visible="false" CssClass="modalPopup-middle">

  <div class="newItemPopUp">
            <div class="offset01"><h4 id="H1" runat="server">
                       Сортиране на данните</h4></div>
            <div class="pnl-size-icons">
                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/close3.png" CssClass="closeModalWindow" OnClick="btnCancelParentPanel_OnClick" />
            </div>
        </div>

    <div class="row span12Separator">
        <div class="span12">
        </div>
    </div>
    <div class="container-fluid">
        <div class="row-fluid">
            <div class="span9">
                <asp:GridView ID="gvGridColumns" runat="server" AutoGenerateColumns="false" CssClass="MainGrid">
                    <Columns>
                        <asp:TemplateField HeaderText="№" ItemStyle-Width="20px">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle Width="20px" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnRowIndex" runat="server" Value='<%# Container.DataItemIndex %>' />
                                <asp:CheckBox ID="chbxSelect" runat="server" AutoPostBack="true" 
                                    oncheckedchanged="chbxSelect_CheckedChanged" />
                            </ItemTemplate>
                            <ItemStyle CssClass="MainGrid_td_item_center" Width="24px" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="56%" HeaderText="Колона">
                            <ItemTemplate>                                
                                <asp:Label ID="lbColumnName" runat="server" Text='<%# Bind("ColumnName") %>' />
                                <asp:HiddenField ID="hdnColumnCode" runat="server" Value='<%# Bind("ColumnCode") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="60%" />
                        </asp:TemplateField>                        
                        <asp:TemplateField ItemStyle-Width="38%" ItemStyle-CssClass="MainGrid_td_item_center" HeaderText="Посока">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlSortDirection" runat="server" 
                                    DataValueField="SortDirectionCode" DataTextField="SortDirectionName" 
                                    DataSource='<%# Bind("ListSortDirections") %>' CssClass="width6">
                                </asp:DropDownList>                                
                            </ItemTemplate>
                            <ItemStyle CssClass="MainGrid_td_item_center" Width="38%" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="32px" ItemStyle-CssClass="MainGrid_td_item_center" HeaderText="Пореден №">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlSequenceNumber" runat="server" DataValueField="Value" DataTextField="Text"
                                    DataSource='<%# Bind("ListItems") %>' CssClass="width3" 
                                    AutoPostBack="True" 
                                    onselectedindexchanged="ddlSequenceNumber_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <ItemStyle CssClass="MainGrid_td_item_center" Width="32px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView> 
            </div>
        </div>
        <div class="row">
            <div class="span6">
                <asp:Button ID="btnSort" CssClass="btn closeModalWindow" runat="server" Text="Сортирай" OnClick="btnSort_Click" />
                <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Clear" OnClick="btnClear_Click" />
             <%--   <asp:Button ID="btnCancel" CssClass="btn" runat="server" Text="Cancel" OnClick="btnCancel_Click" />--%>
            </div>
        </div>
    </div>
</asp:Panel>
