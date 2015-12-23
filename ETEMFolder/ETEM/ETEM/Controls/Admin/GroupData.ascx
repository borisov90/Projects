<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupData.ascx.cs" Inherits="ETEM.Controls.Admin.GroupData" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx"           TagName="SMCDropDownList"   TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Common/SMCAutoCompleteTextBox.ascx"    TagName="SMCAutoComplete"   TagPrefix="uc3" %>

<asp:Panel ID="pnlGroupData" runat="server" Visible="false" CssClass="modalPopup-middle">
    <div class="newItemPopUp">
        <div class="offset01"><h4 id="H1" runat="server">
                <asp:Label ID="lbHeaderText" runat="server" Text="Преглед на група"></asp:Label></h4></div>
        <div class="pnl-size-icons">
            <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/close3.png" CssClass="closeModalWindow" OnClick="btnCancelParentPanel_OnClick" />
        </div>
    </div>
    <div class="container-fluid">
        
        <div class="row">
            <div class="span10">
            </div>
        </div>

        <div class="row">
            <div class="span2">
                <asp:Button ID="btnSave" runat="server" CssClass="btn" Text="Save" OnClick="btnSave_Click" />
            </div>
        </div>

        <div class="ResultContext">
            <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
        </div>

        <div class="row">
            <div class="span10">
                <p>
                    <asp:Label ID="lbGroupName" runat="server" Text="Име на група:" CssClass="labelSmall" /></p>
                    <asp:TextBox ID="tbxGroupName" runat="server" TextMode="MultiLine" CssClass="TextBoxDescription"></asp:TextBox>
            </div>
            
        </div>

        <div class="row">
            <div class="span9">
                <p>
                    <asp:Label ID="lbPersonForGroup" runat="server" Text="Служител"></asp:Label></p>
                    <uc3:SMCAutoComplete ID="acAddPersonForGroup" runat="server"  CustomCase="EmployeePerson"
                    CssClassTextBox="span9" />
            </div>
                 
           <%-- <div class="span4">
                <p>
                    <asp:Label ID="lbComboBox" runat="server" Text="Падащ списък"></asp:Label></p>
                     <uc1:SMCDropDownList ID="ddlAbout" runat="server" KeyTypeIntCode="OrderAbout" OrderBy="V_Order" ShowButton="false" />
            </div>--%>
        </div>        

        <div class="row">
            <div class="span12">
            </div>
        </div>

        <div class="row">
            <div class="span2">
                <asp:Button ID="btnAddPerson" runat="server" Text="Добави" OnClick="btnAddPerson_Click" CssClass="btn" />
            </div>
            <div class="span2">
                <asp:Button ID="btnDeletePerson" runat="server" Text="Премахни" OnClick="btnDeletePerson_Click" CssClass="btn" />
            </div>
            <div class="span2">
                <p>
                    <asp:Label ID="lbSharedAccess" runat="server" Text="Групата е видима за всички"></asp:Label></p>                    
            </div>
            <div class="span2">
                    <asp:CheckBox ID="chbxSharedAccess" runat="server"  />
            </div>
        </div>



        <div class="row-fluid">
            <div class="span12">
                <asp:GridView ID="gvGroupPerson" runat="server" AutoGenerateColumns="false" Width="80%">
                    <Columns>
                        <asp:TemplateField HeaderText="№" ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnRowDetailKey" runat="server" Value='<%# Bind("idGroupPersonLink") %>' />
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chbxDeleteGroupPersonLink" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PersonTitle" HeaderText="Име" SortExpression="PersonTitle" />
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="lbEmptyDataText" runat="server" Text='<%# GetCaption("GridView_EmptyDataRow") %>' />
                    </EmptyDataTemplate>
                    <PagerStyle CssClass="cssPager" />
                </asp:GridView>
            </div>
        </div>

    </div>
    <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
</asp:Panel>
