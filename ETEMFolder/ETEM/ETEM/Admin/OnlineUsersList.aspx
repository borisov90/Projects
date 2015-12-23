<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="OnlineUsersList.aspx.cs" Inherits="ETEM.Admin.OnlineUsersList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="OnlineUsersHolder" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <asp:GridView ID="gvOnlineUsers" runat="server" CssClass="MainGrid" AutoGenerateColumns="false"
    AllowSorting="true" OnSorting="gvOnlineUsers_Sorting" 
    AllowPaging="true" OnPageIndexChanging="gvOnlineUsers_OnPageIndexChanging">
        <Columns>
            <asp:TemplateField HeaderText="№">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnServerDelete" runat="server" CausesValidation="False" Text=""
                                    ToolTip='<%# GetCaption("GridView_Delete") %>' CommandArgument='<%# "SessionID=" + Eval("SessionID") %>'
                                    OnClick="lnkBtnServerDelete_Click" OnClientClick='<%# GetConfirmJavaScript("Message_Confirm_Delete", true) %>'>
                                <i class="fi-trash size-21"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
            <asp:BoundField DataField="UserName"                HeaderText="User name"              SortExpression="UserName" />
            <asp:BoundField DataField="PersonNamePlusTitle"     HeaderText="Person name and title"  SortExpression="PersonNamePlusTitle" />
            <asp:BoundField DataField="SessionID"               HeaderText="Session ID"             SortExpression="SessionID" />
            <asp:BoundField DataField="LoginDateTime"           HeaderText="Last login"             SortExpression="LoginDateTime"          DataFormatString="{0:dd.MM.yyyy HH:mm:ss}"  />
            <asp:BoundField DataField="LastRequestDateTime"     HeaderText="Last ativity"           SortExpression="LastRequestDateTime"    DataFormatString="{0:dd.MM.yyyy HH:mm:ss}"/>
            <asp:BoundField DataField="LastPageName"            HeaderText="Page name"              SortExpression="LastPageName" />
            <asp:BoundField DataField="LastModuleName"          HeaderText="Module name"            SortExpression="LastModuleName" />
            <asp:BoundField DataField="IPAddress"               HeaderText="IP Address"             SortExpression="IPAddress" />
            <asp:BoundField DataField="IsKilledStr"             HeaderText="Status of session"      SortExpression="IsKilledStr" />
            
        </Columns>
        <PagerStyle CssClass="cssPager" />
    </asp:GridView>
</asp:Content>
