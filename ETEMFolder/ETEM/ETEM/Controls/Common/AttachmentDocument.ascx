<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AttachmentDocument.ascx.cs"
    Inherits="ETEM.Controls.Common.AttachmentDocument" %>
<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx" TagName="SMCDropDownList"
    TagPrefix="uc1" %>
<%@ Register Src="SMCCalendar.ascx" TagName="SMCCalendar" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Common/SMCFileUploder.ascx" TagName="FileUploder" TagPrefix="uc1" %>
<asp:Panel ID="pnlFormData" runat="server" Visible="false" CssClass="modalPopup65pc">
    <div class="newItemPopUp">
        <div class="offset01">
            <h4 id="H2" runat="server">
                <asp:Label ID="AttachmentTitle" runat="server" Text="Добавяне на документ"></asp:Label></h4>
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
            <div class="span4">
                <asp:Button ID="btnSave" runat="server" CssClass="btn" Text="Save" OnClick="btnSave_Click" />
                <%-- <asp:Button ID="btnCancel" runat="server" CssClass="btn" Text="Cancel" OnClick="btnCancel_Click" />--%>
            </div>
        </div>
        <div class="row">
            <div class="span10">
                <p>
                    <asp:Label ID="lbDescription" runat="server" Text="Описание на документа"></asp:Label></p>
                <asp:TextBox ID="tbxDescription" runat="server" TextMode="MultiLine" CssClass="span10" Rows="5"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="lbAttachmentDate" runat="server" Text="Дата"></asp:Label></p>
                    <uc2:SMCCalendar ID="tbxAttachmentDate" runat="server" />                
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbAttachmentType" runat="server" Text="Вид"></asp:Label></p>
                <uc1:SMCDropDownList ID="ddlAttachmentType" runat="server" KeyTypeIntCode="AttachmentType"
                    ShowButton="false" OrderBy="V_Order" />
            </div>
        </div>
        <div class="row">
            <div class="span10">
                <p>
                    <asp:Label ID="Label7" runat="server" Text="Документи:"></asp:Label></p>
                <uc1:FileUploder ID="fuAttachment" runat="server"  />
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
    <asp:HiddenField ID="hdnAttachmentDocumentType" runat="server" />
    <asp:HiddenField ID="hdnDocKeyTypeIntCode" runat="server" />
    <asp:HiddenField ID="hdnModuleSysName" runat="server" />
</asp:Panel>
