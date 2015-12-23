<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BilletScrapData.ascx.cs" Inherits="ETEM.Controls.CostCalculation.BilletScrapData" %>
<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx" TagName="SMCDropDownList" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Common/SMCAutoCompleteTextBox.ascx" TagName="SMCAutoComplete" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Common/SMCTextArea.ascx" TagName="SMCTextArea" TagPrefix="uc3" %>
<%@ Register Src="../Common/SMCCalendar.ascx" TagName="SMCCalendar" TagPrefix="uc4" %>
<asp:Panel ID="pnlUserMainData" runat="server">
    <div class="ResultContext">
        <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="lbLME" runat="server" Text="LME (EUR/ton)"></asp:Label>
                </p>
                <asp:TextBox ID="tbxLME" runat="server" CssClass="span4" Enabled="False"></asp:TextBox>
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbPREMIUM" runat="server" Text="PREMIUM (EUR/ton)"></asp:Label>
                </p>

                <asp:TextBox ID="tbxPREMIUM" runat="server" CssClass="span4" Enabled="False"></asp:TextBox>
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbMaterial" runat="server" Text="Material  (EUR/ton)"></asp:Label>
                </p>

                <asp:TextBox ID="tbxMaterial" runat="server" CssClass="span4" Enabled="False"></asp:TextBox>
            </div>
        </div>

        <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="lbConsumptionRatio" runat="server" Text="Consumption ratio" ></asp:Label>
                </p>
                 
                <asp:TextBox ID="tbxConsumptionRatio" runat="server"  Enabled="False" CssClass="span4"></asp:TextBox>
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbScrapValuePercent" runat="server" Text="Scrap value (%LME)"></asp:Label>
                </p>

                <asp:TextBox ID="tbxScrapValuePercent" runat="server" Enabled="False" CssClass="span4"></asp:TextBox>
            </div>
        </div>

        <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="lbConsumption" runat="server" Text="Consumption (EUR/ton)"></asp:Label>
                </p>

                <asp:TextBox ID="tbxConsumption" runat="server" Enabled="False" CssClass="span4"></asp:TextBox>
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbScrapValue" runat="server" Enabled="False" Text="Scrap value (EUR/ton)"></asp:Label>
                </p>

                <asp:TextBox ID="tbxScrapValue" runat="server" Enabled="False" CssClass="span4"></asp:TextBox>
            </div>
        </div>

        <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="lbNetConsumption" runat="server" Text="Net consumption (EUR/ton)"></asp:Label>
                </p>

                <asp:TextBox ID="tbxNetConsumption" runat="server" Enabled="False" CssClass="span4"></asp:TextBox>
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbCostOfScrap" runat="server" Text="Cost of scrap (EUR/ton)"></asp:Label>
                </p>

                <asp:TextBox ID="tbxCostOfScrap" runat="server" Enabled="False" CssClass="span4"></asp:TextBox>
            </div>
        </div>


    </div>
    <asp:HiddenField ID="hdnParentControlID" runat="server" />
    <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
</asp:Panel>
