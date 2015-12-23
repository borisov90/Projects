<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SMCNotification.ascx.cs"  Inherits="ETEM.Controls.Common.SMCNotification" %>

<%@ Register Assembly="AjaxControlToolkit"                          Namespace="AjaxControlToolkit"  TagPrefix="asp" %>
<%@ Register Src="SMCDropDownList.ascx"                             TagName="SMCDropDownList"       TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Common/SMCAutoCompleteTextBox.ascx"    TagName="SMCAutoComplete"       TagPrefix="uc3" %>
<%@ Register Src="~/Controls/Common/SMCFileUploder.ascx"            TagName="FileUploder"           TagPrefix="uc4" %>

<asp:Panel ID="pnlFormData" runat="server" Visible="false" CssClass="modalPopup-middle">
    <div class="newItemPopUp">
        <div class="offset01"><h4 id="H1" runat="server">
                <asp:Label ID="lbHeaderText" runat="server" Text="Преглед на съобщение"></asp:Label></h4></div>
        <div class="pnl-size-icons">
            <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/close3.png" CssClass="closeModalWindow" OnClick="btnCancelParentPanel_OnClick" />
        </div>
    </div>
    <div class="container-fluid">
        
        <div class="row span12Separator">
            <div class="span12">
            </div>
        </div>

        <div class="row">
            <div class="span2">
                <asp:Button ID="btnSave" runat="server" CssClass="btn closeModalWindow" Text="Изпрати" OnClick="btnSave_Click" />
            </div>
        <%--    <div class="span2">
                <asp:LinkButton ID="btnCancel" CssClass="btn" runat="server" OnClick="btnCancel_Click"><i class="fi-x-circle  size-18"></i> Cancel</asp:LinkButton>
            </div>--%>
            <div class="span4">
                    <asp:LinkButton ID="lnkBtnPreviewDocument" runat="server" ToolTip='Преглед' CommandArgument='<%# "idRowMasterKey=" + Eval("EntityID") %>' 
                        CssClass="btn" OnClick="lnkBtnPreviewDocument_Click" ></asp:LinkButton>
            </div>
        </div>

        <div class="ResultContext">
            <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
        </div>
        <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="lbSendFromPerson" runat="server" Text="Изпратено от"></asp:Label></p>
                <uc3:SMCAutoComplete ID="ucSendFromPerson" runat="server"  CustomCase="EmployeeLecturerStudentPhd"
                    CssClassTextBox="span4" />
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbSendToPerson" runat="server" Text="Изпратено до:"></asp:Label></p>
                <uc3:SMCAutoComplete ID="ucSendToPerson" runat="server"  CustomCase="EmployeeLecturerStudentPhd"
                    CssClassTextBox="span4" />
            </div>             
        </div>

        <div class="row">
            <div class="span9">
                <p>
                    <asp:Label ID="lbGroup" runat="server" Text="Изпрати на:" /></p>
                    <uc2:SMCDropDownList ID="ddlGroup" runat="server" DataSourceType="Group" ShowButton="false" CssClassDropDown="span9" />
            </div>
        </div>

        <div class="row"  runat="server" id="divBtnAddDelete">
            <div class="span2">
                <asp:Button ID="btnAddNoticeTo" runat="server" Text="Добави" OnClick="btnAddNoticeTo_Click"
                    CssClass="btn" />
            </div>
            <div class="span2">
                <asp:Button ID="btnDeleteNoticeTo" runat="server" Text="Премахни" OnClick="btnDeleteNoticeTo_Click"
                    CssClass="btn" />
            </div>
        </div>
        <div class="row-fluid" runat="server" id="divGvSendNoticeTo">
            <div class="span12">
                <asp:GridView ID="gvSendNoticeTo" runat="server" AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField HeaderText="№" ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnRowDetailKey" runat="server" Value='<%# Bind("idPerson") %>' />
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chbxDeleteNotificationTo" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="FullNamePlusTitle" HeaderText="Известие до" SortExpression="FullNamePlusTitle" />
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="lbEmptyDataText" runat="server" Text='<%# GetCaption("GridView_EmptyDataRow") %>' />
                    </EmptyDataTemplate>
                    <PagerStyle CssClass="cssPager" />
                </asp:GridView>
            </div>
        </div>

        <div class="row span12Separator">
            <div class="span12">
            </div>
        </div>

        <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="lbSendDate" runat="server" Text="Дата на изпращане:" /></p>
                <asp:TextBox ID="tbxSendDate" runat="server" CssClass="textBox"></asp:TextBox>
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbStatus" runat="server" Text="Статус:" /></p>
                    <uc2:SMCDropDownList ID="ddlStatus" runat="server" KeyTypeIntCode="NotificationStatus"
                        ShowButton="false"/>
            </div>
            
        </div>
        <div class="row">
            <div class="span8">
                <p>
                    <asp:Label ID="lbAbout" runat="server" Text="Относно:" CssClass="labelSmall" /></p>
                <asp:TextBox ID="tbxAbout" runat="server" TextMode="MultiLine" CssClass="TextBoxDescription span8"></asp:TextBox>
            </div>
            
        </div>
        <div class="row">
            <div class="span8">
                <p>
                    <asp:Label ID="lbComment" runat="server" Text="Коментар:" CssClass="labelSmall" /></p>
                <asp:TextBox ID="tbxComment" runat="server" TextMode="MultiLine" CssClass="TextBoxDescription span8"></asp:TextBox>
            </div>
        </div>
        
        <div class="row">
            <div class="span9">
            </div>
        </div>

        <div class="row">
            <div class="span9">
                <p>
                    <asp:Label ID="lbfuNotificationFile" runat="server" Text="Прикачени файлове"></asp:Label></p>
                    <uc4:FileUploder ID="fuNotificationFile" runat="server" UserControlName="NotificationData" />
            </div>
        </div>

    </div>
    <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
    <asp:HiddenField ID="hdnLinkedDocument" runat="server" />
</asp:Panel>
