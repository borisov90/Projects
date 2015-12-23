<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserData.ascx.cs" Inherits="ETEM.Controls.Admin.UserData" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/Controls/Admin/UserMainData.ascx" TagName="UserMainData" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Admin/RoleList.ascx" TagName="RoleList" TagPrefix="uc2" %>
<asp:Panel ID="pnlFormData" runat="server" Visible="false" CssClass="modalPopup resizeableModal">
    <div class="newItemPopUp">
        <div class="offset01">
            <h4 id="H3" runat="server">
                User's info</h4>
        </div>
        <div class="pnl-size-icons">
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/close3.png"
                CssClass="closeModalWindow" OnClick="btnCancelParentPanel_OnClick" />
        </div>
    </div>
    <asp:Panel ID="pnlErrors" runat="server" Visible="false" CssClass="modalPopup pnlErrorsPopUp">
        <div class="newItemPopUp">
            <div class="offset01">
                <h4 id="H2" runat="server">
                    <asp:Label ID="lbErrorsTitle" runat="server" Text="Errors" /></h4>
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
        <div class="row-fluid">
            <div class="span12">
                <asp:BulletedList ID="blEroorsSave" BulletStyle="Disc" DisplayMode="Text" runat="server">
                </asp:BulletedList>
            </div>
        </div>
        <%--   <div class="row">
            <div class="span2">
                <asp:Button ID="btnCancelErorrs" runat="server" CssClass="btn" Text="Cancel" OnClick="btnCancelErorrs_Click" />
            </div>
        </div>--%>
    </asp:Panel>
    <div class="container-fluid">
        <div class="buttonsTopPanel">
            <span class="leftBtn span2">
                <asp:LinkButton ID="btnSaveTabs" runat="server" CssClass="btn" OnClick="btnSaveTabs_Click"> Save</asp:LinkButton>
            </span>
            <span class="leftBtn span2">
                <asp:LinkButton ID="btnSendPassword" runat="server" CssClass="btn" 
                onclick="btnSendPassword_Click" >Send password</asp:LinkButton>
            </span>
            <span class="leftBtn span4">
                <asp:LinkButton ID="btnLoginAS" runat="server" CssClass="btn" Visible=false
                onclick="btnLoginAS_Click" >Login as that user</asp:LinkButton>
            </span>
            <span class="leftBtn span8"></span>
            <span id="Span1" class="exitBtn span2"
                runat="server" visible="false">
                <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn" OnClick="btnCancel_Click"><i class="fi-x-circle size-12"></i>Cancel</asp:LinkButton>
            </span>
        </div>
        <div class="ResultContext">
            <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
        </div>
        <ajax:TabContainer ID="tabContainer" runat="server" ActiveTabIndex="0" CssClass="ajax__tab_darkblue-theme">
            <ajax:TabPanel ID="tabMainData" runat="server" HeaderText="Main data">
                <ContentTemplate>
                    <uc1:UserMainData ID="ucUserMainData" runat="server" />
                </ContentTemplate>
            </ajax:TabPanel>
            <ajax:TabPanel ID="tabRoleList" runat="server" HeaderText="Roles">
                <ContentTemplate>
                    <uc2:RoleList ID="ucRoleList" runat="server" />
                </ContentTemplate>
            </ajax:TabPanel>
        </ajax:TabContainer>
        <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
    </div>
</asp:Panel>
