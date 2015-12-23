<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PersonalDataReduced.ascx.cs" Inherits="ETEM.Controls.Admin.PersonalDataReduced" %>

<%@ Register Assembly="AjaxControlToolkit"                          Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx"           TagName="SMCDropDownList"       TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Common/SMCCalendar.ascx"               TagName="SMCCalendar"           TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Common/SMCAutoCompleteTextBox.ascx"    TagName="SMCAutoComplete"       TagPrefix="uc3" %>

<asp:Panel ID="pnlPersonalData" runat="server">
    <asp:Panel runat="server" ID="pnlPersonalErrors" Visible="false" CssClass="modalPopup pnlErrorsPopUp">
        <div class="newItemPopUp">
            <div class="offset01">
                <h4 id="H1" runat="server">
                    The following errors occurred:</h4>
            </div>
            <div class="pnl-size-icons">
                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/close3.png"
                    CssClass="closeModalWindow" OnClick="btnCancelPersonalErorrs_Click" />
            </div>
        </div>
        <div class="container-fluid">
            <div class="row span12Separator">
                <div class="span12">
                </div>
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <asp:BulletedList ID="blPersonalEroorsSave" BulletStyle="Disc" DisplayMode="Text"
                        runat="server">
                    </asp:BulletedList>
                </div>
            </div>           
        </div>
    </asp:Panel>
    <div class="ResultContext">
        <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
    </div>
    <asp:UpdatePanel ID="chidlup" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlAddPersonImage" runat="server" Visible="false" CssClass="modalPopup">
                <div class="newItemPopUp">
                    <div class="offset01">
                        <h4 id="H2" runat="server">
                            Add a picture</h4>
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
                            <asp:Button ID="btnClose" runat="server" CssClass="btn" Text="Cancel" OnClick="btnClose_Click"
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
    <div class="container-fluid" id="PersonDataDIV">
        <div class="row">
            <div class="span2">
                <asp:Button ID="btnSave" runat="server" CssClass="btn" Text="Save" OnClick="btnSave_Click"
                    Visible="false" />
            </div>
        </div>
        <div class="row">
            <div class="span3">
                <asp:ImageButton ID="imgBtnPerson" runat="server" OnClick="imgBtnPersonImage_Click"
                    ToolTip="Picture profile" AlternateText="Picture profile" Width="100px" />
            </div>
            <div class="span9">
                <div class="container">
                    <div class="row">
                        <div class="span3" runat="server" id="divTitle">
                            <p>
                                <asp:Label ID="lbTitle" runat="server" Text="Title"></asp:Label></p>
                            <asp:TextBox ID="tbxTitle" runat="server"></asp:TextBox>
                        </div>
                        <div class="span9 height1" runat="server" id="divBR">
                            <br />
                        </div>
                    </div>
                    <div class="row">
                        <div class="span3">
                            <p>
                                <asp:Label ID="lbFirstName" CssClass="required" runat="server" Text="First name"></asp:Label></p>
                            <asp:TextBox ID="tbxFirstName" CssClass="mandatory" runat="server"></asp:TextBox>
                        </div>
                        <div class="span3">
                            <p>
                                <asp:Label ID="lbSecondName" runat="server" Text="Second name"></asp:Label></p>
                            <asp:TextBox ID="tbxSecondName" CssClass="mandatory" runat="server"></asp:TextBox>
                        </div>
                        <div class="span3">
                            <p>
                                <asp:Label ID="lbLastName" CssClass="required" runat="server" Text="Last name"></asp:Label></p>
                            <asp:TextBox ID="tbxLastName" CssClass="mandatory" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="span3" runat="server" id="divEGN" visible="false">
                <p>
                    <asp:Label ID="lbEGN" runat="server" Text="EGN"></asp:Label></p>
                <asp:TextBox ID="tbxEGN" CssClass="mandatory" runat="server"></asp:TextBox>
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="lbIdentityNumber" runat="server" Text="Identity number"></asp:Label></p>
                <asp:TextBox ID="tbxIdentityNumber" CssClass="mandatory" runat="server"></asp:TextBox>
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="lbIDN" runat="server" Text="IDN"></asp:Label></p>
                 <asp:TextBox ID="tbxIDN" CssClass="mandatory" runat="server"></asp:TextBox>
            </div>            
        </div>
        
        
        <div class="row">
            <div class="span3">
                <p>
                    <asp:Label ID="lbSex" runat="server" Text="Gender"></asp:Label></p>
                <uc1:SMCDropDownList ID="dllSex" runat="server" KeyTypeIntCode="Sex" ShowButton="false" />
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="lbBirthDate" runat="server" Text="Birth date"></asp:Label>
                </p>
                <uc2:SMCCalendar ID="tbxBirthDate" runat="server" />
            </div>           
        </div>
        
        
        
       
        <div class="row">
            <div class="span3">
                <p>
                    <asp:Label ID="lbPhone" runat="server" Text="Phone"></asp:Label></p>
                <asp:TextBox ID="tbxPhone" runat="server"></asp:TextBox>
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="lbMobilePhone" runat="server" Text="Mobile phone"></asp:Label></p>
                <asp:TextBox ID="tbxMobilePhone" runat="server"></asp:TextBox>
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="lbEMail" runat="server" Text="Email"></asp:Label></p>
                <asp:TextBox ID="tbxEMail" runat="server"></asp:TextBox>
            </div>
        </div>
        
        <div class="row">
            <div class="span2">
                <asp:Button ID="btnМакеHistory" runat="server" Text="Маке history" 
                    CssClass="btn modalWindow" onclick="btnМакеHistory_Click" />
            </div>
        </div>
        <div class="row">
            <div class="span12">
                <p>
                    <asp:Label ID="Label18" runat="server" Text="History of changes"></asp:Label></p>
                <asp:GridView ID="gvPersonHistory" runat="server" AutoGenerateColumns="False" CssClass="MainGrid" Width="100%">
                    <Columns>
                        <asp:BoundField DataField="FullName"            HeaderText="Full name" />
                        <asp:BoundField DataField="IdentityNumber"      HeaderText="Identity number" />
                        <asp:BoundField DataField="CreationDate"        HeaderText="Date of change" DataFormatString="{0:dd.MM.yyyy}"/>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <asp:Panel ID="pnlDivSearchStudent" runat="server" CssClass="modalPopup65pc" Visible="true">
        <div class="newItemPopUp">
            <div class="offset01">
                <h4 id="H3" runat="server">
                    Loading data</h4>
            </div>
            <div class="pnl-size-icons">
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/close3.png"
                    CssClass="closeModalWindow" OnClick="ImageButton1_Click" />
            </div>
        </div>
        <div class="row span12Separator">
            <div class="span12">
            </div>
        </div>
        <div class="container-fluid">
            <div class="row">
                <div class="span4">
                    <p>
                        <asp:Label ID="lbLoadStudentInfoBy" runat="server" Text="Load person info by" Font-Bold="true"
                            Visible="true"></asp:Label></p>
                    <asp:DropDownList ID="ddlLoadStudentInfoBy" runat="server" Visible="true" CssClass="span4"
                        Font-Bold="true" AutoPostBack="True" OnSelectedIndexChanged="ddlLoadStudentInfoBy_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <p>
                        <asp:Label ID="lbTextForSearchStudent" runat="server" Text="Text to search" Font-Bold="true"
                            Visible="true"></asp:Label></p>
                    <uc3:SMCAutoComplete ID="ucTextForSearchStudent" runat="server" CssClassTextBox="span12" />
                </div>
            </div>
            
        </div>
    </asp:Panel>
    <asp:HiddenField ID="hdnParentControlID"        runat="server" />
    <asp:HiddenField ID="hdnDetailsKey"             runat="server" />
    <asp:HiddenField ID="hdnRowMasterKey"           runat="server" />
    <asp:HiddenField ID="hdnIdStudentCandidate"     runat="server" />
    <asp:HiddenField ID="hdnIdAcademicPeriod"       runat="server" />
    <asp:HiddenField ID="hdnNoLoadUserPanel"        runat="server" />
</asp:Panel>
