<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SMCFileUploder.ascx.cs"
    Inherits="ETEM.Controls.Common.SMCFileUploder" %>
<%@ Register Src="~/Controls/Common/FileExplorer.ascx" TagName="FileExplorer" TagPrefix="uc1" %>
<div id="fileExplorer">
    <asp:HiddenField ID="hdnIdObject" runat="server" />
    <asp:HiddenField ID="hdnReloadFiles" runat="server" Value="0" OnValueChanged="hdnReloadFiles_ValueChanged" />
    <div style="white-space: nowrap;">
        <%--<asp:Button ID="btnShowUploadedFiles" runat="server" CssClass="button" Text="ПРИКАЧЕНИ ФАЙЛОВЕ"
            OnClick="btnShowUploadedFiles_Click" />--%>
        <div class="row">
            <div class="span2">
                <asp:Button ID="btnUploadfile" runat="server" CssClass="btn modalWindow" Text="Добави файл"
                    OnClick="btnUploadfile_Click" />
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="chidlup" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlUploadFile" runat="server" Visible="false" CssClass="modalPopup">
                <div class="newItemPopUp">
                    <div class="offset01">
                        <h4 id="H1" runat="server">
                            Добавяне на файл</h4>
                    </div>
                    <div class="pnl-size-icons">
                        <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/close3.png"
                            OnClick="btnClose_Click" />
                    </div>
                </div>
                <div class="container-fluid">
                    <div class="row">
                        <div class="span8">
                            <br />
                        </div>
                    </div>
                    <div class="row">
                        <div class="span2">
                            <asp:FileUpload ID="fuAddFileToArchive"   runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="span2">
                            <asp:Button ID="btnAdd" runat="server" CssClass="btn" Text="Прикачи" OnClick="btnAdd_Click" />
                        </div>
                        <div class="span2">
                            <asp:Button ID="btnAddToArchive" runat="server" CssClass="btn" Text="Към Архив" OnClick="btnAddToArchive_Click" />
                        </div>
                        <%--   <div class="span2">
                        <asp:Button ID="btnClose" runat="server" CssClass="btn"         Text="Cancel"      OnClick="btnClose_Click" />
                    </div>--%>
                    </div>
                    <uc1:FileExplorer runat="server" ID="fexUploadFiles" CanDeleteFiles="True" CanViewCbxlink="false"
                        CanViewFolderDeleted="true" CanViewArchiveDeleted="true" CanViewToArchive="true"
                        IsChangeSessionFilePath="true" BtnShowUploadedFilesVisible="false" BtnUploadFileVisible="false" />
                    <asp:HiddenField ID="hdnInitFilePath" runat="server" />
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAdd" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlExplorer" runat="server" Width="100%">
        <div class="tab-content">
            <asp:GridView ID="gvExplorer" runat="server" AutoGenerateColumns="False" Width="100%"
                PageSize="50" OnRowDataBound="gvExplorer_RowDataBound" CssClass="tableGrid" BorderStyle="Solid"
                BorderColor="Black" BorderWidth="1px">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkBtnUpLavel" runat="server" OnClick="lnkBtnUpLavel_Click"> <i class="fi-arrow-up size-24"></i></asp:LinkButton>
                            <asp:Button ID="btnRefreshFileExplorer" UseSubmitBehavior="false" BackColor="#ecdd9a"
                                BorderWidth="0px" runat="server" OnClick="btnRefreshFileExplorer_Click" Width="0px"
                                Height="0px" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="border-style: none; border-width: 0px;">
                                <tr>
                                    <td style="border-style: none; border-width: 0px;">
                                        <!-- <i class="fi-shopping-bag size-24"> -->
                                        <asp:Label ID="lbPicture" runat="server" Text=""></asp:Label>
                                        <!-- <asp:Image ID="Image1" runat="server" ImageUrl='<%# Bind("ImageSRC") %>' /> -->
                                    </td>
                                    <td style="border-style: none; border-width: 0px;">
                                        <asp:ImageButton ID="ImageButton2" runat="server" Visible='<%# Bind("DeleteVisible") %>'
                                            CommandArgument='<%# Eval("FullName") + "|" + Eval("IsFile") %>' OnClick="imgBtnDelete_Click"
                                            ImageAlign="Left" />
                                    </td>
                                    <td style="border-style: none; border-width: 0px;">
                                        <asp:CheckBox ID="cbxFileLink" runat="server" Visible='<%# Bind("CbxVisible") %>' />
                                    </td>
                                    <td style="border-style: none; border-width: 0px;">
                                        <asp:CheckBox ID="chbxFileToArchive" runat="server" Visible='<%# Bind("ChbxToArchiveVisible") %>' />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <ControlStyle Height="25px" Width="23px" />
                        <HeaderStyle Height="25px" Width="25px" />
                        <ItemStyle Height="25px" Wrap="False" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Име">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnOpenDirectory" ForeColor="#993333" runat="server" CommandArgument='<%# Eval("FullName") + "|" + Eval("IsFile") %>'
                                OnClick="lbtnOpenDirectory_Click" Text='<%# Bind("Name") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Type" HeaderText="Тип" ItemStyle-HorizontalAlign="Center">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="LastWriteTime" HeaderText="Дата" DataFormatString="{0:dd.MM.yyyy г.}"
                        HtmlEncode="false" ItemStyle-HorizontalAlign="Center">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="FileLength" HeaderText="Размер (KB)" HtmlEncode="false"
                        ItemStyle-HorizontalAlign="Center">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                </Columns>
                <EmptyDataTemplate>
                    <%--<i class="fi-prohibited size-48" style="color:Red">--%>
                    <asp:ImageButton ID="imgBtnNoData" runat="server" ImageUrl="~/Images/Level Up.png"
                        OnClick="ImageButton1_Click" /></i> &nbsp;Няма прикачени файлове към документа
                    <asp:Button ID="btnRefreshFileExplorer" UseSubmitBehavior="false" BackColor="red"
                        BorderWidth="0px" runat="server" OnClick="btnRefreshFileExplorer_Click" Width="0px"
                        Height="0px" />
                </EmptyDataTemplate>
                <HeaderStyle VerticalAlign="Top" />
            </asp:GridView>
        </div>
    </asp:Panel>
</div>
<asp:HiddenField ID="hdnParentDirectory" runat="server" />
<asp:HiddenField ID="hdnRoorDirectory" runat="server" />
<asp:HiddenField ID="hdnAllowedFileExtentions" runat="server" />
<asp:HiddenField ID="hdnIsForDiplomaImages" runat="server" />


