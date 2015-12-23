﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MaterialPriceListData.ascx.cs" Inherits="ETEM.Controls.CostCalculation.MaterialPriceListData" %>
<%@ Register Src="~/Controls/Common/SMCCalendar.ascx" TagName="SMCCalendar" TagPrefix="uc1" %>
<asp:Panel ID="pnlFormData" runat="server" Visible="false" CssClass="modalPopup65pc">
    <div class="newItemPopUp">
        <div class="offset01">
            <h4 id="H1" runat="server">
                <asp:Label ID="lbHeaderText" runat="server" Text="Material price list data"></asp:Label></h4>
        </div>
        <div class="pnl-size-icons">
            <asp:ImageButton ID="imgBtnCancel" runat="server" ImageUrl="~/Images/close3.png"
                CssClass="closeModalWindow" OnClick="btnCancel_Click" />
        </div>
    </div>
    <asp:Panel ID="pnlErrors" runat="server" Visible="false" CssClass="modalPopup pnlErrorsPopUp">
        <div class="newItemPopUp">
            <div class="offset01">
                <h4 id="H2" runat="server">
                    <asp:Label ID="lbErrorsTitle" runat="server" Text="Errors" /></h4>
            </div>
            <div class="pnl-size-icons">
                <asp:ImageButton ID="imgBtnCancelErrors" runat="server" ImageUrl="~/Images/close3.png"
                    OnClick="btnCancelErorrs_Click" />
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
    </asp:Panel>
    <div class="container-fluid">
        <div class="row">
            <div class="span10">
            </div>
        </div>
        <div class="row">
            <div class="span2">
                <asp:Button ID="btnSave" runat="server" CssClass="btn" Text="Save" OnClick="btnSave_Click" />
            </div>
        </div>
        <div class="ResultContext">
            <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
        </div>
        <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="lbDateFrom" runat="server" Text="Valid from"></asp:Label></p>
                <uc1:SMCCalendar ID="tbxDateFrom" runat="server" />
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbDateTo" runat="server" Text="Valid to"></asp:Label></p>
                <uc1:SMCCalendar ID="tbxDateTo" runat="server" />
            </div>
            <div class="span4">
                &nbsp;
            </div>
        </div>
        <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="lbLME" runat="server" Text="LME (EUR/ton)"></asp:Label></p>
                <asp:TextBox ID="tbxLME" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbPremium" runat="server" Text="Premium (EUR/ton)"></asp:Label></p>
                <asp:TextBox ID="tbxPremium" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
            </div>
            <div class="span4">
                &nbsp;
            </div>
        </div>
    </div>    
    <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
</asp:Panel>
