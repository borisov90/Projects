<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SAPDataData.ascx.cs" Inherits="ETEM.Controls.CostCalculation.SAPDataData" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspAjax" %>
<%@ Register Src="~/Controls/Common/SMCCalendar.ascx" TagName="SMCCalendar" TagPrefix="uc1" %>
<asp:Panel ID="pnlFormData" runat="server" Visible="false" CssClass="modalPopupLarge">
    <div class="newItemPopUp">
        <div class="offset01">
            <h4 id="H1" runat="server">
                <asp:Label ID="lbHeaderText" runat="server" Text="SAP Data by Cost Centers"></asp:Label></h4>
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
                <asp:Button ID="btnSave" runat="server" CssClass="btn" Text="Save" OnClick="btnSave_Click" />&nbsp;
                <asp:Button ID="btnDownloadImportTemplate" runat="server" CssClass="btn" Text="Download import template" OnClick="btnDownloadImportTemplate_Click" />
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
                <uc1:SMCCalendar ID="tbxDateFrom" runat="server" />
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbDateTo" runat="server" Text="Valid to"></asp:Label>
                </p>
                <uc1:SMCCalendar ID="tbxDateTo" runat="server" />
            </div>
            <div class="span4">
                &nbsp;
            </div>
        </div>
        <div class="row">
            <div class="span8">
                <p>
                    <asp:Label ID="lbImport" runat="server" Text="Please select a file for import"></asp:Label>
                </p>
                <asp:UpdatePanel ID="updPnlImport" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:FileUpload ID="fuImport" runat="server" CssClass="span8" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnImport" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="span4">
                <br />
                <asp:Button ID="btnImport" runat="server" CssClass="btn" Text="Import" OnClick="btnImport_Click" />
                <aspAjax:ConfirmButtonExtender ID="cbeBtnImport" runat="server" TargetControlID="btnImport"
                    ConfirmText="Are you sure you want to import expenses and quantities by cost centers?">
                </aspAjax:ConfirmButtonExtender>
            </div>
        </div>
    </div>
    <div class="container-fluid" id="divSAPDataExpensesAndQuantities" runat="server">
        <br />
        <div class="row tab-content">
            <div>
<%--                <div class="headerLine">
                    <asp:Label ID="lbSAPDataExpenses" runat="server" Text="Detailed Expenses by Cost Centers (EUR)" CssClass="headline offset01 font-bold fontColorWhite"></asp:Label>
                </div>--%>
                <asp:Table ID="tblSAPDataExpenses" runat="server" CssClass="GridExpenses width100pc-important"></asp:Table>
            </div>            
        </div>
<%--        <div class="row tab-content">
            <div>
                <div class="headerLine">
                    <asp:Label ID="lbSAPDataExpensesGroups" runat="server" Text="Expenses by Cost Centers (EUR)" CssClass="headline offset01 font-bold fontColorWhite"></asp:Label>
                </div>
                <asp:Table ID="tblSAPDataExpensesGroups" runat="server" CssClass="GridExpenses width100pc-important"></asp:Table>
            </div>
            <br />
        </div>
        <div class="row tab-content">
            <div>
                <div class="headerLine">
                    <asp:Label ID="lbSAPDataQuantities" runat="server" Text="Quantity" CssClass="headline offset01 font-bold fontColorWhite"></asp:Label>
                </div>
                <asp:Table ID="tblSAPDataQuantities" runat="server" CssClass="GridExpenses width100pc-important"></asp:Table>
            </div>
            <br />
        </div>
        <div class="row tab-content">
            <div>
                <div class="headerLine">
                    <asp:Label ID="lbSAPDataExpensesGroupsByMH" runat="server" Text="Expenses by Cost Centers (EUR/MH)" CssClass="headline offset01 font-bold fontColorWhite"></asp:Label>
                </div>
                <asp:Table ID="tblSAPDataExpensesGroupsByMH" runat="server" CssClass="GridExpenses width100pc-important"></asp:Table>
            </div>
        </div>--%>
    </div>
    <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
</asp:Panel>
