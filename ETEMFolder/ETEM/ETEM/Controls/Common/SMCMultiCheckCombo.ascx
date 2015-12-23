<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SMCMultiCheckCombo.ascx.cs" Inherits="ETEM.Controls.Common.SMCMultiCheckCombo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<style>
    
.PanelPadding
{
	padding-left:20px;
}    
    
</style>

<asp:TextBox ID="txtCombo" runat="server" ReadOnly="true" CssClass="span12" ></asp:TextBox>
<ajaxToolkit:PopupControlExtender ID="PopupControlExtender111" runat="server" 
    TargetControlID="txtCombo" PopupControlID="Panel111" Position="Bottom" >
</ajaxToolkit:PopupControlExtender>

<input type="hidden" name="hidVal" id="hidVal" runat="server" />
<asp:HiddenField ID="hdnCustomSuffixForControlID" runat="server" />

<asp:Panel ID="Panel111" runat="server" ScrollBars="Vertical" Height="150" BackColor="White" BorderColor="Gray" BorderWidth="1"  CssClass="PanelPadding">
    
    <asp:CheckBoxList ID="chkList"  runat="server" Height="150" DataTextField="Name" DataValueField="idKeyValue" RepeatLayout=Flow>                                                                                                                                                                        
    </asp:CheckBoxList>
    
</asp:Panel>




