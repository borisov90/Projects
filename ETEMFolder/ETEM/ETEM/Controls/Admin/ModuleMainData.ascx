<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleMainData.ascx.cs"
    Inherits="ETEM.Controls.Admin.ModuleMainData" %>
<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx" TagName="SMCDropDownList"
    TagPrefix="uc1" %>
<asp:Panel ID="pnlFormData" runat="server" Visible="false" CssClass="modalPopup65pc">
    <div class="newItemPopUp">
        <div class="offset01">
            <h4 id="H2" runat="server">
                Добавяне/Редакция на модул</h4>
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
    <div class="ResultContext">
        <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="span8">
                <asp:Button ID="btnSave" runat="server" CssClass="btn" Text="Save" OnClick="btnSave_Click" />
                <%-- <asp:Button ID="btnCancel" runat="server" CssClass="btn" Text="Cancel" OnClick="btnCancel_Click" />--%>
            </div>
        </div>
        <div class="row">
            <div class="span8">
                <p>
                    <asp:Label ID="lbModuleName" runat="server" Text="Име на модул"></asp:Label></p>
                <asp:TextBox ID="tbxModuleName" runat="server" class="span8"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span8">
                <p>
                    <asp:Label ID="lbModuleSysName" runat="server" Text="Име на модул(системно)"></asp:Label></p>
                <asp:TextBox ID="tbxModuleSysName" runat="server" class="span8"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="Label1" runat="server" Text="Необходима проверка"></asp:Label></p>
                <asp:CheckBox ID="chbxNeedCheck" runat="server" />
            </div>
        </div>
        <div class="row">
            <div class="span8">
                <p>
                    <asp:Label ID="lbComment" runat="server" Text="Коментар"></asp:Label></p>
                <asp:TextBox ID="tbxComment" runat="server" class="span8"></asp:TextBox>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
</asp:Panel>
