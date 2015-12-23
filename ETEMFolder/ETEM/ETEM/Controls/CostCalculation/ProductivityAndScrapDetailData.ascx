﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductivityAndScrapDetailData.ascx.cs" Inherits="ETEM.Controls.CostCalculation.ProductivityAndScrapDetailData" %>
<%@ Register Src="~/Controls/Common/SMCCalendar.ascx" TagName="SMCCalendar" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx" TagName="SMCDropDownList" TagPrefix="uc2" %>
<asp:Panel ID="pnlFormData" runat="server" Visible="false" CssClass="modalPopup80pc">
    <div class="newItemPopUp">
        <div class="offset01">
            <h4 id="H1" runat="server">
                <asp:Label ID="lbHeaderText" runat="server" Text="Productivity and Scrap by Press and Profile"></asp:Label></h4>
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
            <div class="span4">
                <asp:Button ID="btnSave" runat="server" CssClass="btn" Text="Save" OnClick="btnSave_Click" />
            </div>
        </div>
        <div class="ResultContext">
            <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
        </div>
        <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="lbDateFrom" runat="server" Text="Valid from"></asp:Label>
                </p>
                <uc1:SMCCalendar ID="tbxDateFrom" runat="server" ReadOnly="true" />
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbDateTo" runat="server" Text="Valid to"></asp:Label>
                </p>
                <uc1:SMCCalendar ID="tbxDateTo" runat="server" ReadOnly="true" />
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbStatus" runat="server" Text="Status"></asp:Label>
                </p>
                <asp:TextBox ID="tbxStatus" runat="server" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="lbCostCenter" runat="server" Text="Cost center"></asp:Label>
                </p>
                <uc2:SMCDropDownList ID="ddlCostCenter" runat="server" DataSourceType="PRESS" KeyTypeIntCode="CostCenter"
                    ShowButton="false" OrderBy="V_Order" CssClassDropDown="span4" KeyValueDataTextField="DefaultValue4" />
            </div>
            <div class="span8">
                <p>
                    <asp:Label ID="lbProfileSetting" runat="server" Text="Profile setting"></asp:Label>
                </p>
                <uc2:SMCDropDownList ID="ddlProfileSetting" runat="server" DataSourceType="ProfileSetting"
                    ShowButton="false" CssClassDropDown="span8" />
            </div>
        </div>
        <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="lbSumOfHours" runat="server" Text="Sum of hours"></asp:Label>
                </p>
                <asp:TextBox ID="tbxSumOfHours" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbSumOfConsumption" runat="server" Text="Sum of consumption"></asp:Label>
                </p>
                <asp:TextBox ID="tbxSumOfConsumption" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbSumOfProduction" runat="server" Text="Sum of production"></asp:Label>
                </p>
                <asp:TextBox ID="tbxSumOfProduction" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="lbProductivityKGh" runat="server" Text="Productivity KG/h"></asp:Label>
                </p>
                <asp:TextBox ID="tbxProductivityKGh" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbScrapRate" runat="server" Text="Scrap Rate %"></asp:Label>
                </p>
                <asp:TextBox ID="tbxScrapRate" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
            </div>
            <div class="span4">
                &nbsp;
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
</asp:Panel>