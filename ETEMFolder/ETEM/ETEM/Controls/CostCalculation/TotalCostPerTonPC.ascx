<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TotalCostPerTonPC.ascx.cs" Inherits="ETEM.Controls.CostCalculation.TotalCostPerTonPC" %>

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
                    <asp:Label ID="lbProductivity" runat="server" Text="Total cost per ton & PC" CssClass="headline"></asp:Label>
                </h4>
                <asp:Table CssClass="MainGrid" ID="gvProducitivity" runat="server">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell>
                    
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>
                    EUR/ton
                        </asp:TableHeaderCell>

                        <asp:TableHeaderCell>
                    EUR/PC
                        </asp:TableHeaderCell>


                    </asp:TableHeaderRow>
                    <asp:TableRow>
                        <asp:TableCell>
                    Transportation Cost
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>


                    </asp:TableRow>
                     <asp:TableRow>
                        <asp:TableCell>
                    Commission
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>


                    </asp:TableRow>
                     <asp:TableRow>
                        <asp:TableCell>
                    LME
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>


                    </asp:TableRow>
                     <asp:TableRow>
                        <asp:TableCell>
                    Billeting premium
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>


                    </asp:TableRow>
                     <asp:TableRow>
                        <asp:TableCell>
                   Extrusion
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>


                    </asp:TableRow>
                     <asp:TableRow>
                        <asp:TableCell>
                    Packaging
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>


                    </asp:TableRow>
                     <asp:TableRow>
                        <asp:TableCell>
                    TOTAL CONVERSION (without transport, commission & billeting)
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>


                    </asp:TableRow>
                     <asp:TableRow>
                        <asp:TableCell>
                    Administration expenses
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>


                    </asp:TableRow>
                     <asp:TableRow>
                        <asp:TableCell>
                    Sales expenses
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>


                    </asp:TableRow>
                     <asp:TableRow>
                        <asp:TableCell>
                   Financial fixed expenses
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>


                    </asp:TableRow>
                     <asp:TableRow>
                        <asp:TableCell>
                    Financial variable expenses
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>


                    </asp:TableRow>
                     <asp:TableRow Font-Bold="true" BackColor="#F5F5F5">
                        <asp:TableCell>
                    Total cost
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>


                    </asp:TableRow>




                </asp:Table>
            </div>

        </div>
    </div>



    <asp:HiddenField ID="hdnParentControlID" runat="server" />
    <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
</asp:Panel>
