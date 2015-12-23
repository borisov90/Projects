<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Profile.ascx.cs" Inherits="ETEM.Controls.CostCalculation.Profile" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx"           TagName="SMCDropDownList"   TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Common/SMCAutoCompleteTextBox.ascx"    TagName="SMCAutoComplete"   TagPrefix="uc3" %>

<asp:Panel ID="pnlProfile" runat="server" Visible="false" CssClass="modalPopup">
    <div class="newItemPopUp">
        <div class="offset01">
            <h4 id="H1" runat="server">
                <asp:Label ID="lbHeaderText" runat="server" Text="Profile"></asp:Label></h4>
        </div>
        <div class="pnl-size-icons">
            <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/close3.png"
                CssClass="closeModalWindow" OnClick="btnCancelParentPanel_OnClick" />
        </div>
    </div>
    <div class="container-fluid">
        <div class="row span12Separator">
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
            <div class="span3">
                <asp:ImageButton ID="imgBtnProfileSetting" runat="server" ToolTip="Profile image" AlternateText="Profile image"
                    Width="100px" OnClick="imgBtnProfile_Click" />
            </div>
            <div class="span8">
                <p>
                    <asp:Label ID="lbProfileName" runat="server" Text="Profile:" CssClass="labelSmall" /></p>
                    <asp:TextBox ID="tbxProfile" runat="server" TextMode="MultiLine" CssClass="TextBoxDescription span8"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span3">
                &nbsp;
            </div>
            <div class="span8">
                <p>
                    <asp:Label ID="lbProfileNameSAP" runat="server" Text="Profile SAP:" CssClass="labelSmall" /></p>
                    <asp:TextBox ID="tbxProfileSAP" runat="server" TextMode="MultiLine" CssClass="TextBoxDescription span8"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span4" >
                <p>
                    <asp:Label ID="lbProfileType" runat="server" Text="Type:"></asp:Label></p>
                    <uc1:SMCDropDownList ID="ddlProfileType" runat="server" KeyTypeIntCode="ProfileType" ShowButton="false" />
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbProfileCategory" runat="server" Text="Category:"></asp:Label></p>
                    <uc1:SMCDropDownList ID="ddlProfileCategory" runat="server" KeyTypeIntCode="ProfileCategory" ShowButton="false" />
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbProfileComplexity" runat="server" Text="Complexity:"></asp:Label></p>
                    <uc1:SMCDropDownList ID="ddlProfileComplexity" runat="server" KeyTypeIntCode="ProfileComplexity" ShowButton="false" />
            </div>
        </div>

        <div class="row span12Separator">
            <div class="span10">
            </div>
        </div>

        <asp:CheckBoxList ID="chBxValue" runat="server" RepeatDirection="Horizontal">
            <asp:ListItem>A </asp:ListItem>
            <asp:ListItem>B </asp:ListItem>
            <asp:ListItem>C (if applicable) </asp:ListItem>
            <asp:ListItem>D (if applicable) </asp:ListItem>
            <asp:ListItem>s (if applicable) </asp:ListItem>
        </asp:CheckBoxList>

       <div class="row span12Separator">
            <div class="span10">
            </div>
        </div>

       


        <div class="row">
            <div class="span8">
                    <asp:Label ID="lbDiameterFormula" runat="server" Text="Ø formula ( +, -, *, ^(power), /(Division),sqrt() )"></asp:Label>Computable
                    <asp:TextBox ID="tbxDiameterFormula" runat="server" CssClass="TextBoxDescription span8" Text="Ø = SQRT(A^ + B^)"></asp:TextBox>
            </div>
            
        </div>

        <div class="row">
            <div class="span6">
                <p>
                    <asp:Label ID="Label9" runat="server" Text="Validation requirements"></asp:Label></p>
                    <asp:TextBox ID="tbxValidationRequirements" runat="server" CssClass="span6"></asp:TextBox>
            </div>
            <div class="span2">
                <p>
                    &nbsp;</p>
                <asp:Button ID="btnAddValidation" runat="server" Text="Add" CssClass="btn" OnClick="btnAddValidation_Click" />  
                
                <asp:Button ID="btnRemoveValidation" runat="server" Text="Remove" CssClass="btn" OnClick="btnRemoveValidation_Click" />
            </div>
           
        </div>
        <div class="row-fluid">
            <div class="span8">
                <asp:GridView ID="gvValidation" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" CssClass="MainGrid">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnProfileSettingValidation"   runat="server"  Value='<%# Bind("idProfileSettingValidation") %>' />
                                <asp:CheckBox ID="chbxRemoveValidation" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:BoundField DataField="ValidationRequirement" HeaderText="Validation requirements" SortExpression="ValidationRequirement" ItemStyle-Width="400px" />

                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="lbEmptyDataText" runat="server" Text='<%# GetCaption("GridView_EmptyDataRow") %>' ItemStyle-Width="400px" />
                    </EmptyDataTemplate>
                    <PagerStyle CssClass="cssPager" />
                </asp:GridView>
            </div>
        </div>
        <div class="row span12Separator">
            <div class="span12">
            </div>
        </div>

        <asp:UpdatePanel ID="updPnlImage" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlAddProfileSettingImage" runat="server" Visible="false" CssClass="modalPopup">
                    <div class="newItemPopUp">
                        <div class="offset01">
                            <h4 id="H2" runat="server">
                                Upload profile image </h4>
                        </div>
                        <div class="pnl-size-icons">
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/close3.png"
                                CssClass="closeModalWindow" OnClick="btnCancelParentPanel_OnClick" />
                        </div>
                    </div>
                    <div class="container-fluid">
                        <div class="row">
                            <div class="span3">
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                            </div>                            
                        </div>
                        <div class="row">
                            <div class="span2">
                                <asp:Button ID="btnUpload" runat="server" CssClass="btn" Text="Upload" OnClick="btnUpload_Click" />
                            </div>
                            <div class="span2">
                                <asp:Button ID="btnClose" runat="server" CssClass="btn" Text="Close" OnClick="btnClose_Click"
                                    Visible="false" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnUpload" />
            </Triggers>
        </asp:UpdatePanel>


        </div>
        <div class="row span12Separator">
            <div class="span12">
            </div>
        </div>


    
    <asp:HiddenField ID="hdnRowMasterKey"               runat="server" />
   
</asp:Panel>
