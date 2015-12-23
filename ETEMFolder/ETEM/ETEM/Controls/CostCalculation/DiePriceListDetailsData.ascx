<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiePriceListDetailsData.ascx.cs" Inherits="ETEM.Controls.CostCalculation.DiePriceListDetailsData" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspAjax" %>
<%@ Register Src="~/Controls/Common/SMCCalendar.ascx" TagName="SMCCalendar" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx" TagName="SMCDropDownList" TagPrefix="uc2" %>
<asp:Panel ID="pnlFormData" runat="server" Visible="false" CssClass="modalPopup80pc">
    <div class="newItemPopUp">
        <div class="offset01">
            <h4 id="H1" runat="server">
                <asp:Label ID="lbHeaderText" runat="server" Text="Die Price List by Dimensions"></asp:Label></h4>
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
            <div class="span2">
                <asp:Button ID="btnSave" runat="server" CssClass="btn" Text="Save" OnClick="btnSave_Click" />
            </div>
        </div>
        <div class="ResultContext">
            <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
        </div>
        <div class="row-fluid">
            <div class="span10">
                <p>
                    <asp:Label ID="lbVendor" runat="server" Text="Vendor die price list"></asp:Label></p>
                <uc2:SMCDropDownList ID="ddlVendor" runat="server" DataSourceType="VendorDiePriceList"
                    ShowButton="false" OrderBy="V_Order" CssClassDropDown="span10" DropDownEnabled="false" />
            </div>
        </div>
        <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="lbNumberOfCavities" runat="server" Text="Number of cavities"></asp:Label></p>
                <uc2:SMCDropDownList ID="ddlNumberOfCavities" runat="server" KeyTypeIntCode="NumberOfCavities"
                    ShowButton="false" OrderBy="V_Order" CssClassDropDown="span4" />
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbProfileCategory" runat="server" Text="Profile category"></asp:Label></p>
                <uc2:SMCDropDownList ID="ddlProfileCategory" runat="server" KeyTypeIntCode="ProfileCategory"
                    ShowButton="false" OrderBy="V_Order" CssClassDropDown="span4" />
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbProfileComplexity" runat="server" Text="Profile complexity"></asp:Label></p>
                <uc2:SMCDropDownList ID="ddlProfileComplexity" runat="server" KeyTypeIntCode="ProfileComplexity"
                    ShowButton="false" OrderBy="V_Order" CssClassDropDown="span4" />
            </div>
        </div>
        <div class="row">
            <div class="span3">
                <p>
                    <asp:Label ID="lbDimensionA" runat="server" Text="Dimension A (mm)"></asp:Label></p>
                <asp:TextBox ID="tbxDimensionA" runat="server" MaxLength="19" onkeypress="return isNumeric(event, false);"></asp:TextBox>
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="lbDimensionB" runat="server" Text="Dimension B (mm)"></asp:Label></p>
                <asp:TextBox ID="tbxDimensionB" runat="server" MaxLength="19" onkeypress="return isNumeric(event, false);"></asp:TextBox>
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="lbDiePrice" runat="server" Text="Die price (EUR)"></asp:Label></p>
                <asp:TextBox ID="tbxDiePrice" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="lbLifespan" runat="server" Text="Lifespan (ton)"></asp:Label></p>
                <asp:TextBox ID="tbxLifespan" runat="server" MaxLength="19" onkeypress="return isNumeric(event, true);"></asp:TextBox>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnDiePriceListID" runat="server" Value="-1" />
    <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
</asp:Panel>