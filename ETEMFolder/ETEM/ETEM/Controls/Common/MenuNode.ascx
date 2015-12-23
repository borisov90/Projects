<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuNode.ascx.cs" Inherits="ETEM.Controls.Common.MenuNode" %>
<%@ Register Src="~/Controls/Common/SMCTextArea.ascx" TagName="SMCTextArea" TagPrefix="uc1" %>
<asp:Label ID="lbRole" runat="server" Text="Роля"></asp:Label>
<div class="row">
    <div class="span3">
        <asp:DropDownList ID="ddlRole" runat="server" Width="200px" DataTextField="Name"
            DataValueField="EntityID" AutoPostBack="true" OnSelectedIndexChanged="ddlRole_OnSelectedIndexChanged">
        </asp:DropDownList>
    </div>
    <div class="span2">
        <asp:HiddenField ID="hdnDllValue" runat="server" />
        <asp:Button ID="btnSave" runat="server" Text="Запис" OnClick="btnSave_Click" CssClass="btn" />
    </div>
    <div class="span2">
        <asp:Button ID="btnShowPlNewItem" runat="server" CssClass="btn" Text="Add new item"
            OnClick="btnShowPlNewItem_Click" />
    </div>
</div>
<asp:GridView ID="gvMainManu" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvMainManu_RowDataBound"
    BorderWidth="1px" Width="40%" ShowHeader="False" CssClass="MenuGrid MainMenuGrid">
    <Columns>
        <asp:TemplateField HeaderText="Подменю" ItemStyle-Width="300px" HeaderStyle-Width="99%">
            <ItemTemplate>
                <span class="btn-hide-show" rel='tooltip' data-placement="right" data-toggle="tooltip"
                    title="Отвори / Cancel"><i class=" icon-minus-sign"></i></span>
                <%-- <asp:LinkButton ID="btnHideNestedGrids" runat="server" Text="" CssClass="btn-hide-show"></asp:LinkButton>--%>
                <asp:CheckBox ID="cbxRoot" runat="server" />
                <asp:TextBox ID="txtMainMenuName" CssClass="span4" runat="server" Text='<%# Bind("name") %>'></asp:TextBox>
                <asp:HiddenField ID="hdnRootNoteID" runat="server" Value='<%# Bind("idNode") %>' />
                <asp:LinkButton ID="LnkManageTitle" runat="server" Text="Edit" OnClick="lnkbtnEdit_OnClick"
                    CssClass="blue" CommandArgument='<%# Bind("idNode") %>'></asp:LinkButton>
                <asp:LinkButton ID="LnkDeleteNode" runat="server" Text="| Delete" OnClick="lnkbtnDelete_OnClick"
                    CssClass="blue" CommandArgument='<%# Bind("idNode") %>'></asp:LinkButton>
                <asp:GridView ID="gvSubMenu" runat="server" AutoGenerateColumns="False" Width="100%"
                    OnRowDataBound="gvSubMenu_RowDataBound" ShowHeader="False" BorderWidth="0px"
                    CssClass="SubMenuGrid">
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="99%">
                            <ItemTemplate>
                                <span class="btn-hide-show" rel='tooltip' data-placement="right" data-toggle="tooltip"
                                    title="Отвори / Cancel"><i class=" icon-minus-sign"></i></span>
                                <asp:CheckBox ID="cbxSubMenu" runat="server" />
                                <asp:TextBox ID="txbSubMenu" runat="server" CssClass="span4" Text='<%# Bind("name") %>'></asp:TextBox>
                                <asp:HiddenField ID="hdnNoteSubMenuID" runat="server" Value='<%# Bind("idNode") %>' />
                                <asp:LinkButton ID="LnkManageTitle" CommandArgument='<%# Bind("idNode") %>' runat="server"
                                    CssClass="blue" Text="Edit" OnClick="lnkbtnEdit_OnClick"></asp:LinkButton>
                                <asp:LinkButton ID="LnkDeleteNode" runat="server" CssClass="blue" Text="| Delete"
                                    OnClick="lnkbtnDelete_OnClick" CommandArgument='<%# Bind("idNode") %>'></asp:LinkButton>
                                <asp:GridView ID="gvSubMenuLink" runat="server" AutoGenerateColumns="False" Width="100%"
                                    BorderWidth="0px" CssClass="LinkMenuGrid" ShowHeader="False">
                                    <Columns>
                                        <asp:TemplateField HeaderText="name" HeaderStyle-Width="99%">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnNoteDoubleSubMenuID" runat="server" Value='<%# Bind("idNode") %>' />
                                                <asp:CheckBox ID="cbxDoubleSubMenu" runat="server" />
                                                <asp:TextBox ID="txbLastMenuNode" CssClass="span4" runat="server" Text='<%# Bind("name") %>'></asp:TextBox>
                                                <asp:LinkButton ID="LnkManageTitle" CssClass="blue" runat="server" Text="Edit" CommandArgument='<%# Bind("idNode") %>'
                                                    OnClick="lnkbtnEdit_OnClick"></asp:LinkButton>
                                                <asp:LinkButton ID="LnkDeleteNode" CssClass="blue" runat="server" Text="| Delete"
                                                    OnClick="lnkbtnDelete_OnClick" CommandArgument='<%# Bind("idNode") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="99%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ItemTemplate>
                            <HeaderStyle Width="99%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ItemTemplate>
            <HeaderStyle Width="99%"></HeaderStyle>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:Panel ID="pnlDeleteComfirmation" runat="server" Visible="false" CssClass="modalPopup container-fluid">
    <div class="row-fluid newItemPopUp">
        <div class="span8">
            <div class="offset01">
                <h4>
                    Потвърждаване на изтриването</h4>
            </div>
        </div>
    </div>
    <div class="row span12Separator">
        <div class="span12">
        </div>
    </div>
    <div class="ResultContext">
        <asp:Label ID="lbDelComfirmationResult" runat="server" Text=""></asp:Label>
    </div>
    <div class="row">
        <div class="span6 offset4">
            <asp:Label ID="lbAreYouSure" runat="server" Text="Сигурен ли си,че искаш да изтриеш елемeнта от менютo?"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="span4 offset5">
            <br />
            <asp:Button ID="btnComfirmDelete" runat="server" CssClass="btn" Text="Да" OnClick="btnComfirmDelete_Click" />
            <asp:Button ID="btnCancelDelete" runat="server" CssClass="btn" Text="Отказ" OnClick="btnCancelDelete_Click" />
        </div>
    </div>
</asp:Panel>
<asp:Panel ID="pnlAddNewMenuItem" runat="server" Visible="false" CssClass="modalPopup">
    <div class="newItemPopUp">
        <div class="offset01">
            <h4 runat="server" id="PopUpHeadline">
                Добавяне на нов елемент в менюто</h4>
        </div>
        <div class="pnl-size-icons">
            <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/close3.png"
                CssClass="closeModalWindow" OnClick="btnCancelAddMenuItem_Click" />
        </div>
    </div>
    <div class="row span12Separator">
        <div class="span12">
        </div>
    </div>
    <div class="ResultContext">
        <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
    </div>
    <div class="row">
            <div class="leftBtn span2">
                <asp:Button ID="btnAddMenuItem" runat="server" CssClass="btn" Text="Добави" OnClick="btnAddMenuItem_Click" />
            </div>
            <div class="span2">
                <asp:Button ID="btnCancelAddMenuItem" runat="server" CssClass="btn" Text="Cancel"
                    OnClick="btnCancelAddMenuItem_Click" Visible="false" />
            </div>
        </div>
    <div class="container-fluid">
        <div class="row">
            <div class="span6">
                <p>
                    <asp:Label ID="lbNodePlace" runat="server" Text="Елемента в момента е в главното меню"
                        Visible="false"></asp:Label></p>
                <asp:DropDownList ID="ddlCurrentNodePlace" runat="server" CssClass="span6" DataTextField="name"
                    DataValueField="EntityID" Visible="false">
                </asp:DropDownList>
            </div>
            <div class="span6">
            </div>
        </div>
        <div class="row">
            <div class="span6">
                <p>
                    <asp:Label ID="lbMainNodesDll" runat="server" Text="Главни елементи на менюто"></asp:Label></p>
                <asp:DropDownList ID="ddlMainMenuItems" runat="server" CssClass="span6" DataTextField="name"
                    DataValueField="EntityID" AutoPostBack="true" OnSelectedIndexChanged="ddlMainMenuItems_OnSelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <div class="span6">
                <p>
                    <asp:Label ID="lbSubMenuItems" runat="server" Text="Вложени елементи на менюто"></asp:Label></p>
                <asp:DropDownList ID="ddlSubMenuItems" runat="server" CssClass="span6" DataTextField="name"
                    DataValueField="EntityID" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlSubMenuItems_OnSelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="span6">
                <p>
                    <asp:Label ID="lbNewMenuItemName" runat="server" Text="Добавяне на нов елемент в 'Основното меню' с име:"></asp:Label></p>
                <asp:TextBox ID="tbxNewMenuItemName" CssClass="span6" runat="server"></asp:TextBox>
            </div>
            <div class="span6">
                <p>
                    <asp:Label ID="lbNavUrl" runat="server" Text="URL"></asp:Label></p>
                <asp:TextBox ID="txvNavUrl" CssClass="span6" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span12">
                <p>
                    <asp:Label ID="lbQueryParams" runat="server" Text="Параметри към стринг заявката:"></asp:Label></p>
                <uc1:SMCTextArea ID="tbxQueryParams" runat="server" MaxLength="500" CssClass="TextBoxDescription span11" />
            </div>
        </div>
        
    </div>
    <asp:HiddenField ID="hdnEditingIndexValue" runat="server" />
    <asp:HiddenField ID="hdnEditingNodeType" runat="server" />
    <asp:HiddenField ID="hdnEditingNodeParent" runat="server" />
    <asp:HiddenField ID="hdnEditingNodeUrlIndex" runat="server" />
</asp:Panel>
