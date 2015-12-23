<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductCostData.ascx.cs" Inherits="ETEM.Controls.CostCalculation.ProductCostData" %>
<asp:Panel ID="pnlUserMainData" runat="server">
    <div class="ResultContext">
        <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
    </div>
    <%--<input type="button" value="select table" onclick="selectElementContents('ContentPlaceHolder_OfferMainData1_TabContainer_tabProductCost_ucProductCostData_tblProductCosts');">--%>
    <div class="container-fluid">
        <div class="row tab-content">
            <div>
                <asp:Table ID="tblProductCosts" runat="server" CssClass="GridExpenses width100pc-important"></asp:Table>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnParentControlID" runat="server" />
    <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
</asp:Panel>
<script type="text/javascript">
    function selectElementContents(tableId) {

        //var table = document.getElementById(tableId);

        //var body = document.body, range, sel;
        //if (document.createRange && window.getSelection) {
        //    range = document.createRange();
        //    sel = window.getSelection();
        //    sel.removeAllRanges();
        //    try {
        //        range.selectNodeContents(table);
        //        sel.addRange(range);
        //    } catch (e) {
        //        range.selectNode(table);
        //        sel.addRange(range);
        //    }
        //} else if (body.createTextRange) {
        //    range = body.createTextRange();
        //    range.moveToElementText(table);
        //    range.select();
        //    range.execCommand("Copy");
        //}

        var textRange = document.body.createTextRange();
        textRange.moveToElementText(document.getElementById(tableId));
        textRange.execCommand("Copy");
    }

</script>