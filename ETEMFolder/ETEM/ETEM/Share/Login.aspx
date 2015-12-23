<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ETEM.Share.Login" %>

<%@ Register Src="~/Controls/Common/SMCMainHeader.ascx" TagName="SMCMainHeader" TagPrefix="head" %>
<%@ Register Src="~/Controls/Common/SMCFooter.ascx" TagName="SMCFooter" TagPrefix="foot" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
   <link href="~/Styles/design.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href='~/Styles/Themes/calendar_white.css' type="text/css" rel="stylesheet" />
    <link href="~/Styles/Site-Print.css" rel="stylesheet" type="text/css" media="print" />
    <meta charset="UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link rel="stylesheet" type="text/css" href="css/default.css" />
    <link rel="stylesheet" type="text/css" href="css/component.css" />
    <link href="Styles/mediaQueries.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap/bootstrap-responsive.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap/bootstrap.css" rel="stylesheet" type="text/css" />
    <%--to test--%>
    <link href="css/ui-lightness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <%--to test--%>
    <link href="css/foundation-icons/foundation-icons.css" rel="stylesheet" type="text/css" />
    <link href="css/css3buttons/css3buttons.css" rel="stylesheet" type="text/css" />
    <script src="../js/modernizr.custom.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <%--to test--%>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <%--to test--%>
    <script src="../Scripts/bootsrap-js/bootstrap.js" type="text/javascript"></script>
</head>
<body>
    <div class="global-container-login">
        <form id="form1" runat="server">
        <head:SMCMainHeader runat="server" ID="MainHeader" />
        <div class="container-fluid" id="login-form">
            <div class="login-form offset5Perc">
                <div class="login-form-fields">
                    <p>
                        <asp:Label ID="lbUserName" runat="server" Text=""></asp:Label></p>
                    <asp:TextBox ID="tbxUserName" runat="server" CssClass="span3 bg-color-white loginName"></asp:TextBox>
                    <p>
                        <asp:Label ID="lbPassword" runat="server" Text=""></asp:Label></p>
                    <asp:TextBox ID="tbxPassword" runat="server" CssClass="span3 bg-color-white loginName" TextMode="Password"></asp:TextBox>
                    <p>
                        <asp:Button ID="btnLogin" CssClass="btn btn-danger" runat="server" Text="Login" OnClick="btnLogin_Click" />
                        <asp:Button ID="btnForgottenPass" CssClass="btn btn-danger" runat="server" Text="Забравена парола" Visible=false
                            OnClick="btnForgottenPass_Click" /></p>
                    <p>
                        <asp:Label ID="lbError" runat="server" Text=""></asp:Label></p>
                </div>
            </div>
        </div>
        <asp:Panel ID="pnlForgottenPassword" runat="server" CssClass="modalPopup"
            Visible="false">
            <div class="newItemPopUp">
                <div class="offset01">
                    <h4>
                        Забравена парола</h4>
                </div>
                <div class="pnl-size-icons">
                    <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/close3.png"
                        CssClass="closeModalWindow" OnClick="btnCancelParentPanel_OnClick" />
                </div>
            </div>
            <div class="container-fluid">
                <div class="row">
                    <div class="span12">
                    </div>
                </div>
                <div class="ResultContext">
                    <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
                </div>
                <div class="row">
                    <div class="span12">
                        <asp:Button ID="btnSendNewPasswordMail" runat="server" OnClick="btnSendNewPasswordMail_OnClick"
                            CssClass="btn closeModalWindow" Text="Изпрати" />
                    </div>
                </div>
                <div class="row">
                    <div class="span12">
                        <asp:Label ID="Label4" CssClass="bold" runat="server"
                         Text="След коректното полълване на данните ще Ви бъде изпратен Email (на пощата, която Ви е въведена в системата) с нова парола"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="span4">
                        <p>
                            <asp:Label ID="Label1" runat="server" Text="Потребителско име"></asp:Label></p>
                        <asp:TextBox ID="tbxForgottenUsername" runat="server"></asp:TextBox>
                    
                    </div>
                </div>
                 <div class="row">
                    <div class="span12">
                        <asp:Label ID="Label5" CssClass="bold" runat="server" Text="Въпроси за подсигуряване на самоличността:"></asp:Label>
                    </div>
                </div>
                <div class="row">
                        <div class="span4">
                            <p>
                                <asp:Label ID="Label2" runat="server" Text="ЕГН/ЛНЧ/ИДН"></asp:Label></p>
                            <asp:TextBox ID="tbxForgottenEGN" runat="server"></asp:TextBox>
                        </div>
                </div>
                <div class="row">
                    <div class="span10">
                        <p>
                            <asp:Label ID="Label3" runat="server" Text="Вашият Email от системата (Този, на който очаквате да получите новата парола)"></asp:Label></p>
                        <asp:TextBox ID="tbxForgottenEmail" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
        </asp:Panel>
        </form>
        <foot:SMCFooter runat="server" ID="GlobalFooter" />
        <script src="../js/ResponsiveDesingTest.js" type="text/javascript"></script>
        <script src="../js/custom-scripts.js" type="text/javascript"></script>
        
    </div>
    
</body>
</html>
