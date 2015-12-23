<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SMCTextArea.ascx.cs" Inherits="ETEM.Controls.Common.SMCTextArea" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>--%>
<asp:TextBox ID="tbxTextArea" runat="server" TextMode="MultiLine"  ></asp:TextBox> <%--onchange="checkForMaxLen()"--%>
<%--OnTextChanged="tbxTextArea_OnTextChanged" AutoPostBack="true"--%>
<%--<ajax:MaskedEditExtender ID="meeTbxTextArea" runat="server" TargetControlID="tbxTextArea" PromptCharacter="" />--%>
<script type="text/javascript">
    function checkForMaxLen() {
        var textArea = document.getElementById("tbxTextArea");
        if (textArea.value.lenght > 498) {
            textArea.value = textArea.value.substring(0, 499);
        }
    }
</script>