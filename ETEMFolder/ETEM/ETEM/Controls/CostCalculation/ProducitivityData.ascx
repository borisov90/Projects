<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProducitivityData.ascx.cs" Inherits="ETEM.Controls.CostCalculation.ProducitivityData" %>

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
                <h4>
                    <asp:Label ID="lbProductivity" runat="server" Text="Productivity"></asp:Label>
                </h4>
                <asp:Table CssClass="MainGrid" ID="gvProducitivity" runat="server">
                    <asp:TableHeaderRow>
                        
                        <asp:TableHeaderCell>
                    Productivity
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>
                            <asp:Label ID="lbPress" runat="server" Text="Press"></asp:Label>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>
                    QC Extrusion
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>
                            <asp:Label ID="lbCoMetal" runat="server" Text="COMetal"></asp:Label>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>
                    Packaging
                        </asp:TableHeaderCell>

                    </asp:TableHeaderRow>
                    <asp:TableRow>
                        <asp:TableCell>
                    Productivity (kg/MH)
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="tbxPressProducitivity_KG_MH" runat="server" CssClass="span2"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="tbxQCProducitivity_KG_MH" runat="server" CssClass="span2" Enabled="false"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="tbxCOMetalProducitivity_KG_MH" runat="server" CssClass="span2" Enabled="false"></asp:TextBox>
                        </asp:TableCell>

                        <asp:TableCell>
                            <asp:TextBox ID="tbxPackagingProducitivity_KG_MH" runat="server" CssClass="span2"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                    Productivity (ton/MH)
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="tbxPressProducitivity_TON_MH" runat="server" CssClass="span2" Enabled="false"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="tbxQCProducitivity_TON_MH" runat="server" CssClass="span2" Enabled="false"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="tbxCOMetalProducitivity_TON_MH" runat="server" CssClass="span2" Enabled="false"></asp:TextBox>
                        </asp:TableCell>

                        <asp:TableCell>
                            <asp:TextBox ID="tbxPackagingProducitivity_TON_MH" runat="server" CssClass="span2" Enabled="false"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>

        </div>


    </div>
    <asp:HiddenField ID="hdnParentControlID" runat="server" />
    <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
</asp:Panel>
