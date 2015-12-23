<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpensesByCostCentersData.ascx.cs" Inherits="ETEM.Controls.CostCalculation.ExpensesByCostCentersData" %>

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
            <div>
                <asp:Table CssClass="MainGrid" ID="tblExpensesCostCenter" runat="server">
                    <asp:TableRow>
                        <asp:TableCell>
                            <div class="row tab-content">
                                <div>
                                    <asp:Table ID="tblOfferDataExpenseGroup" runat="server" CssClass="GridExpenses width100pc-important"></asp:Table>
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>

        </div>




    </div>
    <asp:HiddenField ID="hdnParentControlID" runat="server" />
    <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
</asp:Panel>
