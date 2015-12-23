<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SMCAutoCompleteTextBox.ascx.cs"
    Inherits="ETEM.Controls.Common.SMCAutoCompleteTextBox" ClassName="SMCAutoCompleteTextBox" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Panel ID="pnlMain" runat="server">
    <asp:TextBox ID="tbxTextToComplete" runat="server"></asp:TextBox>
    <ajaxToolkit:AutoCompleteExtender ID="tbxTextToComplete_AutoCompleteExtender" runat="server"
        DelimiterCharacters=":" MinimumPrefixLength="2" CompletionInterval="1000" Enabled="True"
        ServicePath="~/Services/Common/Common.svc" ServiceMethod="GetCompletionList"
        EnableCaching="false" CompletionSetCount="50" UseContextKey="true" TargetControlID="tbxTextToComplete"
        CompletionListCssClass="maxZ-index">
    </ajaxToolkit:AutoCompleteExtender>
    <asp:Button ID="btnValueChanged" runat="server" Height="1px" Width="1px" Style="visibility: hidden;" />
    <asp:HiddenField ID="hdnCustomSuffixForControlID" runat="server" />
    <asp:HiddenField ID="hdnCustomCase" runat="server" />
    <asp:HiddenField ID="hdnSelectedItemID" runat="server" />
    <asp:HiddenField ID="hdnTableForSelection" runat="server" />
    <asp:HiddenField ID="hdnColumnIdForSelection" runat="server" />
    <asp:HiddenField ID="hdnColumnNameForSelection" runat="server" />
    <asp:HiddenField ID="hdnColumnNameForOrderBy" runat="server" />
    <asp:HiddenField ID="hdnAdditionalWhereParam" runat="server" />
</asp:Panel>
