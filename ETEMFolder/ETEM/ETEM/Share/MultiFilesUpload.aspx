<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MultiFilesUpload.aspx.cs"
    Inherits="ETEM.Share.MultiFilesUpload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Прикачване на файлове</title>
    <link href="../css/uploadify.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.min.js" type="text/javascript" />
    <script src="../Scripts/jquery.uploadify-3.1.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.uploadify-3.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=fuFiles.ClientID %>").uploadify({
                'swf': '../Images/uploadify.swf',
                'uploader': '../WebHandlers/FilesUploadHandler.ashx?' + $('#<%=hdnPath.ClientID%>').val(),
                'cancelImg': '../Images/uploadify-cancel.png',
                'debug': false,
                'auto': false,
                'multi': true,
                'buttonText': 'Избери файлове',
                'method': 'get',
                'onQueueComplete': function (file, data, response) {
                    ReloadParentForm(file);
                    
                }
            });

            $('#<%=btnStartUpload.ClientID%>').bind('click', function () {

                $('#<%=fuFiles.ClientID%>').uploadify('upload', '*'); return false;
            }
             );

            $('#<%=btnStopUpload.ClientID%>').bind('click', function () {

                $('#<%=fuFiles.ClientID%>').uploadify('cancel', '*'); return false;
            }
             );
        });

        function ReloadParentForm(file) {

            //window.opener.frames['frmResourcesList'].Refresh();
            window.location.reload();
           
        }
    </script>

    <script language="javascript" type="text/javascript">

        function Selrdbtn(id) {
            var rdBtn = document.getElementById(id);

            var grid = document.getElementById('<%= gvUploadedFiles.ClientID %>');

            var List = grid.getElementsByTagName("input");
            for (i = 0; i < List.length; i++) {
                if (List[i].type == "radio" && List[i].id != rdBtn.id) {
                    List[i].checked = false;
                }
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="row-fluid">
        <div>
            <asp:Button ID="btnStartUpload" runat="server" Text="Запис (всички)" Width="120px"
                Height="30" CssClass="uploadify-button" OnClick="btnStartUpload_Click" />
            <asp:Button ID="btnStopUpload" runat="server" Text="Отказ (всички)" Width="120px"
                Height="30" CssClass="uploadify-button" />
        </div>
        <br />
        <div>
            <asp:HiddenField ID="hdnPath" runat="server" Value="path=test" />
            <asp:FileUpload ID="fuFiles" runat="server" />
        </div>
        <div class="span12">
            <asp:GridView ID="gvUploadedFiles" runat="server" CssClass="MainGrid" AllowSorting="True"
                AllowPaging="True" AutoGenerateColumns="False" >
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:RadioButton ID="rbtnSelectedFile" runat="server" OnClick="javascript:Selrdbtn(this.id)"/>
                            <asp:HiddenField ID="hdnIdUploadedFile" runat="server" Value='<%# Bind("idUploadedFile") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- <asp:TemplateField HeaderText="Име на файла">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnFileName" Visible="false" runat="server" CommandArgument='<%# Bind("Name") %>'
                            OnClick="lnkBtnFileName_Click" Text='<%# Bind("Name") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                    <asp:BoundField DataField="FileName" HeaderText="Име" />
                    <asp:BoundField DataField="Size" HeaderText="Размер(Mb)" />
                    <asp:BoundField DataField="DateUpload" HeaderText="Дата на създаване" DataFormatString="{0:dd.MM.yyyy г.}"/>

                    <asp:TemplateField HeaderText="Описание" >
                        <ItemTemplate>
                            <asp:TextBox ID="tbxDescription" runat="server" Text='<%# Bind("Description") %>' Width="300"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>


                </Columns>
                <SortedAscendingCellStyle BackColor="#FDF5AC" />
                <SortedAscendingHeaderStyle BackColor="#4D0000" />
                <SortedDescendingCellStyle BackColor="#FCF6C0" />
                <SortedDescendingHeaderStyle BackColor="#820000" />
            </asp:GridView>
        </div>
        <div  class="span12">
        <br />    
        </div>
        <div>
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn" />
        </div>
    </div>
    </form>
</body>
</html>
