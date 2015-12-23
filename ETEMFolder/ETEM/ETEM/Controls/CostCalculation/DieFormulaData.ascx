<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DieFormulaData.ascx.cs" Inherits="ETEM.Controls.CostCalculation.DieFormulaData" %>

<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx" TagName="SMCDropDownList" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Common/SMCFileUploder.ascx" TagName="FileUploder" TagPrefix="uc2" %>

<asp:Panel ID="pnlDieFormulaData" runat="server" CssClass="modalPopup" Visible="false">
    <div class="newItemPopUp">
        <div class="offset01">
            <h4 id="H1" runat="server">Die Formula</h4>
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

        <div class="container-fluid">
            <div class="row">
                <div class="leftBtn span2">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn" />
                </div>

            </div>
            <div class="ResultContext">
                <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
            </div>
            <div class="row">
                <div class="span4">
                    <p>
                        <asp:Label ID="lbNumberCavities" runat="server" Text="Number of cavities"></asp:Label>
                    </p>
                    <uc1:SMCDropDownList ID="ddlNumberCavities" runat="server" KeyTypeIntCode="NumberOfCavities"
                        OrderBy="V_Order" ShowButton="false" />
                </div>
                <div class="span4">
                    <p>
                        <asp:Label ID="lbProfileType" runat="server" Text="Profile type"></asp:Label>
                    </p>
                    <uc1:SMCDropDownList ID="ddlProfileType" runat="server" KeyTypeIntCode="ProfileType"
                        OrderBy="V_Order" ShowButton="false" />
                </div>
                <div class="span4">
                    <p>
                        <asp:Label ID="lbProfileCategory" runat="server" Text="Profile category"></asp:Label>
                    </p>
                    <uc1:SMCDropDownList ID="ddlProfileCategory" runat="server" KeyTypeIntCode="ProfileCategory"
                        OrderBy="V_Order" ShowButton="false" />
                </div>

            </div>
            <div class="row">
                <div class="span8">
                    <p>
                        <asp:Label ID="lbDieFormula" runat="server" Text="Ødie formula ( +, -, *, ^(power), /(Division),sqrt() ):" CssClass="labelSmall" />
                    </p>
                    <asp:TextBox ID="tbxDieFormulaText" runat="server" TextMode="MultiLine" CssClass="TextBoxDescription span8"></asp:TextBox>
                </div>
            </div>

            <div class="span3">
                <asp:ImageButton ID="imgBtnFormula" runat="server" OnClick="imgBtnFormulaImage_Click"
                    ToolTip="Image formula" AlternateText="Image formula" Width="100px" />
            </div>

            <div class="row span12Separator">
                <div class="span12">
                </div>
            </div>


            <asp:UpdatePanel ID="chidlup" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlAddFormulaImage" runat="server" Visible="false" CssClass="modalPopup">
                    <div class="newItemPopUp">
                        <div class="offset01">
                            <h4 id="H2" runat="server">
                                Upload formula image </h4>
                        </div>
                        <div class="pnl-size-icons">
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/close3.png"
                                CssClass="closeModalWindow" OnClick="btnCancelParentPanel_OnClick" />
                        </div>
                    </div>
                    <div class="container-fluid">
                        <div class="row">
                            <div class="span3">
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="span2">
                                <asp:Button ID="btnUpload" runat="server" CssClass="btn" Text="Upload" OnClick="btnUpload_Click" />
                            </div>
                            <div class="span2">
                                <asp:Button ID="btnClose" runat="server" CssClass="btn" Text="Close" OnClick="btnClose_Click"
                                    Visible="false" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnUpload" />
            </Triggers>
        </asp:UpdatePanel>



        </div>
        <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
    </div>

</asp:Panel>
