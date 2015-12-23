<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KeyType.ascx.cs" Inherits="ETEM.Controls.Admin.KeyType" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Panel ID="pnlKeyType" runat="server" Visible="false" CssClass="modalPopup resizeableModal">
    <%--<div class="row ">
        <div class="span2">
            <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn" OnClick="btnCancel_Click"><i class=" icon-remove-circle"></i> Cancel</asp:LinkButton>
        </div>
    </div>--%>
    <div class="newItemPopUp">
        <div class="offset01">
            <h4 id="H2" runat="server">
                Data nomenclature</h4>
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
        <div class="row ">
            <div class="span2">
                <asp:Button ID="btnSave" runat="server" CssClass="btn" Text="Save" OnClick="btnSave_Click" />
            </div>
        </div>
        <div class="ResultContext">
            <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
        </div>
    </div>
    <div class="container-fluid">
        <asp:TabContainer ID="TabContainer" runat="server" ActiveTabIndex="0" CssClass="ajax__tab_darkblue-theme">
            <asp:TabPanel ID="tabMainData" runat="server" HeaderText="Main data">
                <ContentTemplate>
                    <div class="container-fluid">
                        <div class="row ">
                            <div class="span4">
                                <p>
                                    <asp:Label ID="lbName" runat="server" Text="Name"></asp:Label></p>
                                <asp:TextBox ID="tbxName" runat="server"></asp:TextBox>
                            </div>
                            <div class="span4">
                                <p>
                                    <asp:Label ID="lbKeyTypeIntCode" runat="server" Text="Ключ"></asp:Label></p>
                                <asp:TextBox ID="tbxKeyTypeIntCode" runat="server"></asp:TextBox>
                            </div>
                            <div class="span3">
                                <%--<p>
                                <asp:Label ID="lbIsSystem" runat="server" Text="Системна номенклатура"></asp:Label></p>--%>
                                <p>
                                    <asp:CheckBox ID="cbxIsSystem" runat="server" Text="System nomenclature" /></p>
                            </div>
                        </div>
                        <div class="row ">
                            <div class="span9">
                                <p>
                                    <asp:Label ID="lbDescription" runat="server" Text="Description"></asp:Label></p>
                                <asp:TextBox ID="tbxDescription" runat="server" TextMode="MultiLine" CssClass="TextBoxDescription"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="tabValues" runat="server" HeaderText="Values">
                <ContentTemplate>
                    <div class="row ">
                        <div class="span2">
                            <asp:Button ID="bntNewKeyValue" runat="server" CssClass="btn" Text="New" OnClick="bntNewKeyValue_Click" />
                        </div>
                    </div>
                    <asp:GridView ID="gvKeyValues" runat="server" CssClass="MainGrid" AutoGenerateColumns="false"
                        AllowPaging="true" AllowSorting="true" OnPageIndexChanging="gvKeyValues_OnPageIndexChanging"
                        OnSorting="gvKeyValues_OnSorting">
                        <Columns>
                            <asp:TemplateField HeaderText="№" ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnRowSlaveKey" runat="server" Value='<%# Bind("idKeyValue") %>' />
                                    <asp:LinkButton ID="lnkBtnServerEditSlave" runat="server" CausesValidation="False"
                                        Text="" ToolTip="Edit" CommandArgument='<%# "idRowSlaveKey=" +  Eval("idKeyValue") %>'
                                        OnClick="lnkBtnServerEditSlave_Click"><i class="icon-pencil"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField SortExpression="Name"               DataField="Name"                HeaderText="Name" />
                            <asp:BoundField SortExpression="Description"        DataField="Description"         HeaderText="Description" />
                            <asp:BoundField SortExpression="KeyValueIntCode"    DataField="KeyValueIntCode"     HeaderText="KeyValueIntCode" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="tabHistory" runat="server" HeaderText="History">
            </asp:TabPanel>
        </asp:TabContainer>
    </div>
    <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
</asp:Panel>
<asp:Panel ID="pnlKeyValue" runat="server" Visible="false" CssClass="modalPopupSlave">
    <div class="newItemPopUp">
        <div class="offset01">
            <h4 id="H1" runat="server">
                General nomenclatures - values</h4>
        </div>
        <div class="pnl-size-icons">
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/close3.png"
                CssClass="closeModalWindow" OnClick="btnCancelKeyValue_Click" />
        </div>
    </div>
    <div class="row span12Separator">
        <div class="span12">
        </div>
    </div>
    <div class="ResultContext">
        <asp:Label ID="lbResultKeyValueContext" runat="server" Text=""></asp:Label>
    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="span2">
                <asp:HiddenField ID="hdnRowSlaveKey" runat="server" />
                <asp:Button ID="btnSaveKeyValue" runat="server" CssClass="btn" Text="Save" OnClick="btnSaveKeyValue_Click" />
            </div>
            <%--<div class="span2">
                <asp:Button ID="btnCancelKeyValue" runat="server" CssClass="btn" Text="Cancel" OnClick="btnCancelKeyValue_Click" />
            </div>--%>
        </div>
        <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="lbKeyValueNameBG"    runat="server" Text="Key value name"></asp:Label></p>
                <asp:TextBox ID="tbxKeyValueNameBG"     runat="server" CssClass="span4"></asp:TextBox>
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbKeyValueNameEN"    runat="server" Text="Key value name (EN)"></asp:Label></p>
                <asp:TextBox ID="tbxKeyValueNameEN"     runat="server" CssClass="span4"></asp:TextBox>
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbKeyValueIntCode"       runat="server" Text="KeyValueIntCode"></asp:Label></p>
                    <asp:TextBox ID="tbxKeyValueIntCode"    runat="server" CssClass="span4"></asp:TextBox></div>
        </div>
        <div class="row">
            <div class="span8">
                <p>
                    <asp:Label ID="lbKeyValueDescription"   runat="server" Text="Description"></asp:Label></p>
                <asp:TextBox ID="tbxKeyValueDescription"    runat="server" TextMode="MultiLine" CssClass="TextBoxDescription span8"></asp:TextBox>
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbKeyValueOrder" runat="server" Text="Order"></asp:Label></p>
                <asp:TextBox ID="tbxKeyValueOrder" runat="server" CssClass="span2"></asp:TextBox>
            </div>
        </div>
       <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="Label1" runat="server" Text="Default value 1"></asp:Label></p>
                <asp:TextBox ID="tbxDefaultValue1" runat="server" CssClass="span4"></asp:TextBox>
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="Label2" runat="server" Text="Default value 2"></asp:Label></p>
                <asp:TextBox ID="tbxDefaultValue2" runat="server" CssClass="span4"></asp:TextBox>
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="Label3" runat="server" Text="Default value 3"></asp:Label></p>
                <asp:TextBox ID="tbxDefaultValue3" runat="server" CssClass="span4"></asp:TextBox></div>
        </div>
        <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="Label4" runat="server" Text="Default value 4"></asp:Label></p>
                <asp:TextBox ID="tbxDefaultValue4" runat="server" CssClass="span4"></asp:TextBox>
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="Label5" runat="server" Text="Default value 5"></asp:Label></p>
                <asp:TextBox ID="tbxDefaultValue5" runat="server" CssClass="span4"></asp:TextBox>
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="Label6" runat="server" Text="Default value 6"></asp:Label></p>
                <asp:TextBox ID="tbxDefaultValue6" runat="server" CssClass="span4"></asp:TextBox></div>
        </div>

        <div class="row" runat="server" visible="false"> 
            <div class="span4">
                <p>
                    <asp:Label ID="Label7" runat="server" Text="Код АДМИН УНИ"></asp:Label></p>
                <asp:TextBox ID="tbxCodeAdminUNI" runat="server" CssClass="span4"></asp:TextBox>
            </div>
        </div>

    </div>
</asp:Panel>
<asp:HiddenField ID="hdnCurrentCallerDropDownListUniqueID" runat="server" />
