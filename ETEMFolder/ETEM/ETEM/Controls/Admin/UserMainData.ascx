<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserMainData.ascx.cs" Inherits="ETEM.Controls.Admin.UserMainData" %>
<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx" TagName="SMCDropDownList"
    TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Common/SMCAutoCompleteTextBox.ascx" TagName="SMCAutoComplete"
    TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Common/SMCTextArea.ascx" TagName="SMCTextArea" TagPrefix="uc3" %>
<asp:Panel ID="pnlUserMainData" runat="server">
    <div class="ResultContext">
        <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="lbUserName" runat="server" Text="User name"></asp:Label></p>
                <asp:TextBox ID="tbxUserName" runat="server" CssClass="span4"></asp:TextBox>
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbPassword" runat="server" Text="Password"></asp:Label></p>
                <asp:TextBox ID="tbxPassword" runat="server"  TextMode="Password"></asp:TextBox>
            </div>
             <div class="span4">
                <p>
                    <asp:Label ID="Label1" runat="server" Text="Domain check"></asp:Label></p>
                <uc1:SMCDropDownList ID="ddlCheckDomain" ShowButton="false" runat="server" KeyTypeIntCode="YES_NO" />
            </div>
        </div>
        <div class="row">
            <div class="span8">
                <p>
                    <asp:Label ID="lbPerson" runat="server" Text="Person"></asp:Label></p>
                <uc2:SMCAutoComplete ID="tbxAutoCompletePersonName" runat="server" CustomCase="PersonALLByName" CssClassTextBox="span8" />
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbStatus" runat="server" Text="User status"></asp:Label></p>
                <uc1:SMCDropDownList ID="ddlStatus" ShowButton="false" runat="server" KeyTypeIntCode="UserStatus" />
            </div>
        </div>
        <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="lbEGN" runat="server" Text="Identity number"></asp:Label></p>
                <asp:TextBox ID="tbxEGN" runat="server" ReadOnly="true"></asp:TextBox>
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbIdentityNumber" runat="server" Text="ID"></asp:Label></p>
                <asp:TextBox ID="tbxIdentityNumber" runat="server" ReadOnly="true"></asp:TextBox>
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbEmail" runat="server" Text="E-mail"></asp:Label></p>
                <asp:TextBox ID="tbxMail" runat="server" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
        <div class="row" runat="server" id="divAlternativUser" visible="false">
            <div class="span8">
                <p>
                    <asp:Label ID="lbAltPerson" runat="server" Text="Substituting"></asp:Label></p>
                <uc2:SMCAutoComplete ID="tbxAutoCompleteAltPersonName" runat="server" CustomCase="PersonALLByName" CssClassTextBox="span8" />
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbAltPassword" runat="server" Text="Alternative password" ></asp:Label></p>
                <asp:TextBox ID="tbxAltPassword" runat="server" TextMode="Password"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span12">
                <p>
                    <asp:Label ID="lbDescription" runat="server" Text="Description"></asp:Label></p>
                <uc3:SMCTextArea ID="tbxDescription" runat="server" MaxLength="2000" CssClass="TextBoxDescription span11" />
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnParentControlID" runat="server" />
    <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
</asp:Panel>