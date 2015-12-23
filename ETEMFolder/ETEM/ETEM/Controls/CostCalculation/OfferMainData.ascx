<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OfferMainData.ascx.cs" Inherits="ETEM.Controls.CostCalculation.OfferMainData" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx" TagName="SMCDropDownList"
TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Common/SMCCalendar.ascx" TagName="SMCCalendar" TagPrefix="uc2" %>

<%@ Register Src="InquiryData.ascx" TagName="InquiryData" TagPrefix="uc3" %>

<%@ Register Src="DiesData.ascx" TagName="DiesData" TagPrefix="uc4" %>

<%@ Register Src="BilletScrapData.ascx" TagName="BilletScrap" TagPrefix="uc5" %>

<%@ Register Src="ProducitivityData.ascx" TagName="ProducitivityData" TagPrefix="uc6" %>

<%@ Register Src="ExpensesByCostCentersData.ascx" TagName="ExpensesByCostCentersData" TagPrefix="uc7" %>

<%@ Register Src="ProductCostData.ascx" TagName="ProductCostData" TagPrefix="uc8" %>

<%@ Register Src="OfferOverviewData.ascx" TagName="OfferOverviewData" TagPrefix="uc9" %>

<%@ Register Src="ExpensesByCostCentersDataTon.ascx" TagName="ExpensesByCostCentersDataTon" TagPrefix="uc10" %>

<%@ Register src="../Common/SMCFileUploder.ascx" tagname="SMCFileUploder" tagprefix="uc11" %>

<asp:Panel runat="server" ID="pnlErrors" Visible="false" CssClass="modalPopup pnlErrorsPopUp">

    <div class="newItemPopUp">
        <div class="offset01">
            <h4>Errors</h4>
        </div>
        <div class="pnl-size-icons">
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/close3.png"
                             CssClass="closeModalWindow" OnClick="btnCancelErorrs_Click" />
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
    <div class="row">
        <div class="span2">
            <asp:Button ID="btnCancelErorrs" runat="server" CssClass="btn" Text="Cancel" OnClick="btnCancelErorrs_Click" />
        </div>
    </div>
</asp:Panel>

<asp:Panel ID="pnlFormData" runat="server" Visible="false" CssClass="resizeableModal modalPopup">

    <div class="newItemPopUp">

        <div class="offset01">
            <h4 id="H2" runat="server">
                Offer data
                <asp:Label ID="lbOfferData" runat="server" Text=""/>
            </h4>
        </div>


        <div class="pnl-size-icons">
            <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/close3.png"
                             CssClass="closeModalWindow" OnClick="btnCancelParentPanel_OnClick" />
        </div>
    </div>

    <div class="buttonsTopPanel">
        <span class="leftBtn span2">
            <asp:LinkButton ID="btnSaveTabs" runat="server" CssClass="btn"
                            OnClick="btnSaveTabs_Click">Save</asp:LinkButton>
        </span><span class="leftBtn span8"></span><span id="Span1" class="exitBtn span2" runat="server"
                                                        visible="false">
            <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn"
                            OnClick="btnCancel_Click">
                <i class="fi-x-circle size-12"></i>Cancel
            </asp:LinkButton>
        </span>
    </div>
    <div class="ResultContext">
        <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
    </div>
    <ajax:TabContainer ID="TabContainer" runat="server" ActiveTabIndex="0" CssClass="ajax__tab_darkblue-theme">
        <ajax:TabPanel ID="tabInquiryData" runat="server" HeaderText="Inquiry data">

            <ContentTemplate>
                <uc3:InquiryData ID="InquiryData" runat="server" />
                
            </ContentTemplate>

        </ajax:TabPanel>
        <ajax:TabPanel ID="tabDies" runat="server" HeaderText="Dies">
            <ContentTemplate>
                <uc4:DiesData ID="DiesData" runat="server"  />
            </ContentTemplate>

        </ajax:TabPanel>
        <ajax:TabPanel ID="tabBilletScrap" runat="server" HeaderText="Billet & Scrap">
            <ContentTemplate>
                <uc5:BilletScrap ID="BilletScrap" runat="server" />
            </ContentTemplate>

        </ajax:TabPanel>
        <ajax:TabPanel ID="tabProducitivity" runat="server" HeaderText="Producitivity">
            <ContentTemplate>
                <uc6:ProducitivityData ID="ProducitivityData" runat="server" />
            </ContentTemplate>

        </ajax:TabPanel>
        <ajax:TabPanel ID="tabExpensesCostCenters" runat="server" HeaderText="Expenses (EUR/MH)">
            <ContentTemplate>
                <uc7:ExpensesByCostCentersData ID="ExpensesByCostCentersData" runat="server" />
            </ContentTemplate>
        </ajax:TabPanel>

         <ajax:TabPanel ID="tabExpensesCostCentersTon" runat="server" HeaderText="Expenses (EUR/ton)">
             <ContentTemplate>
                 <uc10:ExpensesByCostCentersDataTon ID="ExpensesByCostCentersDataTon" runat="server" />
             </ContentTemplate>
        </ajax:TabPanel>

        <ajax:TabPanel ID="tabProductCost" runat="server" HeaderText="Product cost">
            <ContentTemplate>
                <uc8:ProductCostData ID="ucProductCostData" runat="server" />
            </ContentTemplate>
        </ajax:TabPanel>

        <ajax:TabPanel ID="tabOfferOverviewData" runat="server" HeaderText="Offer Overview">
            <ContentTemplate>
                <uc9:OfferOverviewData ID="OfferOverviewData" runat="server" />
            </ContentTemplate>

        </ajax:TabPanel>
        
        <ajax:TabPanel ID="tabAttachments" runat="server" HeaderText="Attachments">
            <ContentTemplate>
                <uc11:SMCFileUploder ID="ucAttachments" runat="server" UserControlName="OfferMainData" />
            </ContentTemplate>

        </ajax:TabPanel>
    </ajax:TabContainer>

    <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
</asp:Panel>
