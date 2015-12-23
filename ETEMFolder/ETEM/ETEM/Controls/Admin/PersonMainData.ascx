<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PersonMainData.ascx.cs"
    Inherits="ETEM.Controls.Admin.PersonMainData" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx" TagName="SMCDropDownList"
    TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Common/SMCCalendar.ascx" TagName="SMCCalendar" TagPrefix="uc2" %>
<%--<%@ Register Src="~/Controls/Admin/PersonalData.ascx" TagName="PersonalData" TagPrefix="uc3" %>--%>
<%@ Register Src="~/Controls/Admin/PersonalDataReduced.ascx" TagName="PersonalDataReduced" TagPrefix="uc3" %>

<asp:Panel runat="server" ID="pnlErrors" Visible="false" CssClass="modalPopup container-fluid pnlErrorsPopUp">

    <div class="newItemPopUp">
            <div class="offset01">
                <h4>
                   The following errors occurred</h4>
            </div>
            <div class="pnl-size-icons">
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/close3.png"
                    CssClass="closeModalWindow" OnClick="btnCancelParentPanel_OnClick" />
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
                <h4>Person data</h4>
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
                OnClick="btnCancel_Click"><i class="fi-x-circle size-12"></i>Cancel</asp:LinkButton>
        </span>
    </div>
    <div class="ResultContext">
        <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
    </div>
    <ajax:TabContainer ID="TabContainer" runat="server" ActiveTabIndex="0" CssClass="ajax__tab_darkblue-theme">
        <ajax:TabPanel ID="tabMainData" runat="server" HeaderText="Personal data">
            <ContentTemplate>
                <uc3:PersonalDataReduced ID="ucPersonalData" runat="server" NoLoadUserPanel="True" />
            </ContentTemplate>
        </ajax:TabPanel>
        <ajax:TabPanel ID="tabHistory" runat="server" HeaderText="History" Visible="false">
        </ajax:TabPanel>
    </ajax:TabContainer>
    <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
</asp:Panel>
