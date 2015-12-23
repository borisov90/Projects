<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiesData.ascx.cs" Inherits="ETEM.Controls.CostCalculation.DiesData" %>

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
                    <asp:Label ID="lbCostOfDie" runat="server" Text="Die price (EUR)"></asp:Label>
                </p>
                <asp:TextBox ID="tbxCostOfDie" runat="server" CssClass="span4" Enabled="False"></asp:TextBox>
            </div>

        </div>
        <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="lbLifespan" runat="server" Text="Lifespan (ton)"></asp:Label>
                </p>

                <asp:TextBox ID="tbxLifespan" runat="server" CssClass="span4" Enabled="False" ></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="lbCostOfDieTon" runat="server" Text="Cost of die (EUR/ton)"></asp:Label>
                </p>

                <asp:TextBox ID="tbxCostOfDieTon" runat="server" CssClass="span4" Enabled="False"></asp:TextBox>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnParentControlID" runat="server" />
    <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
</asp:Panel>
