<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingMainData.ascx.cs"
    Inherits="ETEM.Controls.Admin.SettingMainData" %>
<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx" TagName="SMCDropDownList" TagPrefix="uc1" %>
<asp:Panel ID="pnlFormData" runat="server" Visible="false" CssClass="modalPopup">

<div class="newItemPopUp">
        <div class="offset01">
            <h4 id="H2" runat="server">
                Add a new setting</h4>
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
            <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
        </div>
        <div class="row">
            <div class="span4">
               <p><asp:Label ID="lbSettingName" runat="server" Text="Name"></asp:Label></p>
                <asp:TextBox ID="tbxSettingName" runat="server" ></asp:TextBox>     
            </div>
            <div class="span4">
               <p><asp:Label ID="lbSettingIntCode" runat="server" Text="Code"></asp:Label></p>
               <asp:TextBox ID="tbxSettingIntCode" runat="server" ReadOnly=true ></asp:TextBox>     
            </div>
        </div>
    
        <div class="row">
            <div class="span4">
              <p><asp:Label ID="lbSettingDescription" runat="server" Text="Description"></asp:Label></p>
              <asp:TextBox ID="tbxSettingDescription" runat="server"  CssClass="TextBoxDescription"></asp:TextBox>      
            </div>
        </div>
        <div class="row">
            <div class="span4">
                <p><asp:Label ID="lbSettingValue" runat="server" Text="Value"></asp:Label></p>
                <asp:TextBox ID="tbxSettingValue" runat="server"></asp:TextBox>    
            </div>
            <div class="span4">
             <p><asp:Label ID="lbSettingDefaultValue" runat="server" Text="Default value"></asp:Label></p> 
             <asp:TextBox ID="tbxSettingDefaultValue" runat="server"></asp:TextBox>  
             </div>
        </div>
        
    </div>
    <asp:Panel ID="pnlCronProcessStart" runat="server" Visible="false">
        <asp:Label ID="lbCronProcessStart" runat="server" Text=""></asp:Label>
    </asp:Panel>
</asp:Panel>
