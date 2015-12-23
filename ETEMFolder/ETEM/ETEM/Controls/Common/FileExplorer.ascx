<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileExplorer.ascx.cs"
    Inherits="ETEM.Controls.Common.FileExplorer" %>
<asp:TextBox ID="tbxCaption" runat="server" Visible="false" CssClass="textBox"></asp:TextBox>
<asp:Label ID="lbCaption" runat="server" Visible="false" CssClass="labelSmall" />
<table>
    <tr>
        <td>
            <asp:CheckBox ID="chbxIsOneFileToUpload" runat="server" Text="Един файл за прикачване!"
                Visible="false" />
        </td>
    </tr>
</table>
<div id="fileExplorer">
    <asp:HiddenField ID="hdnIdObject" runat="server" />
    <asp:HiddenField ID="hdnReloadFiles" runat="server" Value="0" OnValueChanged="hdnReloadFiles_ValueChanged" />
    <div style="white-space: nowrap;">
        <asp:Button ID="btnShowUploadedFiles" runat="server" CssClass="button" Text="ПРИКАЧЕНИ ФАЙЛОВЕ"
            OnClick="btnShowUploadedFiles_Click" />
        <asp:Button ID="btnUploadfile" runat="server" CssClass="button" Text="Добави файл" />
    </div>
    <asp:Panel ID="pnlExplorer" runat="server">
        <div class="tab-content">
            <asp:GridView ID="gvExplorer" runat="server" AutoGenerateColumns="False" Width="80%"
                PageSize="50" OnRowDataBound="gvExplorer_RowDataBound" CssClass="tableGrid" BorderStyle="Solid"
                BorderColor="Black" BorderWidth="1px">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" OnClick="ImageButton1_Click" ImageUrl="~/Images/Level Up.png" />
                            <asp:Button ID="btnRefreshFileExplorer" UseSubmitBehavior="false" BackColor="#ecdd9a"
                                BorderWidth="0px" runat="server" OnClick="btnRefreshFileExplorer_Click" Width="0px"
                                Height="0px" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="border-style: none; border-width: 0px;">
                                <tr>
                                    <td style="border-style: none; border-width: 0px;">
                                        <asp:Label ID="lbPicture" runat="server" Text=""></asp:Label>
                                        <%--<asp:Image ID="Image1" runat="server" ImageUrl='<%# Bind("ImageSRC") %>' />--%>
                                    </td>
                                    <td style="border-style: none; border-width: 0px;">
                                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/Delete.png"
                                            Visible='<%# Bind("DeleteVisible") %>' CommandArgument='<%# Eval("FullName") + "|" + Eval("IsFile") %>'
                                            OnClick="imgBtnDelete_Click" ImageAlign="Left" />
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
                        <ItemStyle Height="25px" Wrap="False" Width="40px" />
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
                    <asp:ImageButton ID="imgBtnNoData" runat="server" ImageUrl="~/Images/Level Up.png"
                        OnClick="ImageButton1_Click" />
                    &nbsp;Няма прикачени файлове към документа
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
