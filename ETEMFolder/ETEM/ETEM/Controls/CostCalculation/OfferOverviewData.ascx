<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OfferOverviewData.ascx.cs" Inherits="ETEM.Controls.CostCalculation.OfferOverviewData" %>

<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx" TagName="SMCDropDownList" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Common/SMCAutoCompleteTextBox.ascx" TagName="SMCAutoComplete" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Common/SMCTextArea.ascx" TagName="SMCTextArea" TagPrefix="uc3" %>
<%@ Register Src="../Common/SMCCalendar.ascx" TagName="SMCCalendar" TagPrefix="uc4" %>
<asp:Panel ID="pnlUserMainData" runat="server">
    <div class="ResultContext">
        <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
    </div>
    <div class="container-fluid">
        <div class="row tab-content">
            <div class="span6">
                <asp:Table ID="tblOverviewCostTable" runat="server" CssClass="GridExpenses width100pc-important"></asp:Table>
            </div>
            <div class="span6">
                <asp:Table ID="tblOverviewSalesPriceTable" runat="server" CssClass="GridExpenses width100pc-important"></asp:Table>
            </div>
        </div>
        <div class="row">
            <div class="span12">
                &nbsp;
            </div>
        </div>
        <div class="row tab-content">
            <div class="span6">
                <asp:Table ID="tblOverviewEtemCalculations" runat="server" CssClass="GridExpenses width100pc-important"></asp:Table>
            </div>
            <div class="span6">
                <asp:Table ID="tblOverviewSummaryExpenses" runat="server" CssClass="GridExpenses width100pc-important"></asp:Table>
            </div>

        </div>

    </div>

    <asp:HiddenField ID="hdnParentControlID" runat="server" />
    <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
</asp:Panel>
