<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PermittedActionMainData.ascx.cs"
    Inherits="ETEM.Controls.Admin.PermittedActionMainData" %>
<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx" TagName="SMCDropDownList"
    TagPrefix="uc1" %>
<asp:Panel ID="pnlFormData" runat="server" Visible="false" CssClass="modalPopup-middle">
    <div class="row-fluid newItemPopUp">
        <div class="span8">
            <div class="offset01">
                <h4>
                    Редакция на позволени действия</h4>
            </div>
            <div class="pnl-size-icons">
                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/close3.png"
                    CssClass="closeModalWindow" OnClick="btnCancel_Click" />
            </div>
        </div>
    </div>
    <div class="row span12Separator">
        <div class="span12">
        </div>
    </div>
    <div class="ResultContext">
        <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
    </div>
    <div class="row">
        <div class="leftBtn span2">
            <asp:Button ID="btnSave" CssClass="btn" runat="server" Text="Save" OnClick="btnSave_Click" />
        </div>
        <div class="span2">
            <asp:Button ID="btnCancel" CssClass="btn" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                Visible="false" />
        </div>
        <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="span8">
                <p>
                    <asp:Label ID="lbName" runat="server" Text="Наименование"></asp:Label></p>
                <asp:TextBox ID="tbxFrendlyName" runat="server" CssClass="TextBoxDescription span8"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span8">
                <p>
                    <asp:Label ID="lbDescription" runat="server" Text="Описание"></asp:Label></p>
                <asp:TextBox ID="tbxDescription" runat="server" TextMode="MultiLine" CssClass="TextBoxDescription span8"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="lbTypeSE" runat="server" Text="Модул"></asp:Label></p>
                <uc1:SMCDropDownList ID="ddlModule" runat="server" ShowButton="false" DataSourceType="Modules"/>
            </div>
        </div>
    </div>
</asp:Panel>
