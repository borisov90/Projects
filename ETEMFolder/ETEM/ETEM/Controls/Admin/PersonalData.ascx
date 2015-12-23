<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PersonalData.ascx.cs"
    Inherits="ETEM.Controls.Admin.PersonalData" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx" TagName="SMCDropDownList"
    TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Common/SMCCalendar.ascx" TagName="SMCCalendar" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Common/SMCAutoCompleteTextBox.ascx" TagName="SMCAutoComplete"
    TagPrefix="uc3" %>
<asp:Panel ID="pnlPersonalData" runat="server">
    <asp:Panel runat="server" ID="pnlPersonalErrors" Visible="false" CssClass="modalPopup pnlErrorsPopUp">
        <div class="newItemPopUp">
            <div class="offset01">
                <h4 id="H1" runat="server">
                    Възникнаха следните грешки</h4>
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
            <%--<div class="row">
            <div class="span2">
                <asp:Button ID="btnCancelPersonalErorrs" runat="server" CssClass="btn" Text="Cancel" OnClick="btnCancelPersonalErorrs_Click"/>
            </div>
        </div>--%>
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
                            Добавяне на снимка</h4>
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
                            <asp:Button ID="btnUpload" runat="server" CssClass="btn" Text="Добави" OnClick="btnUpload_Click" />
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
            <%--<asp:Button ID="btnCancel" runat="server" CssClass="btn" Text="Cancel" OnClick="btnCancel_Click"
        Visible="false" />--%>
        </div>
        <div class="row">
            <div class="span3">
                <asp:ImageButton ID="imgBtnPerson" runat="server" OnClick="imgBtnPersonImage_Click"
                    ToolTip="Снимка профил" AlternateText="Снимка профил" Width="100px" />
            </div>
            <div class="span9">
                <div class="container">
                    <div class="row">
                        <div class="span3" runat="server" id="divTitle">
                            <p>
                                <asp:Label ID="lbTitle" runat="server" Text="Титла"></asp:Label></p>
                            <asp:TextBox ID="tbxTitle" runat="server"></asp:TextBox>
                        </div>
                        <div class="span9 height1" runat="server" id="divBR">
                            <br />
                        </div>
                    </div>
                    <div class="row">
                        <div class="span3">
                            <p>
                                <asp:Label ID="lbFirstName" CssClass="required" runat="server" Text="Име"></asp:Label></p>
                            <asp:TextBox ID="tbxFirstName" CssClass="mandatory" runat="server"></asp:TextBox>
                        </div>
                        <div class="span3">
                            <p>
                                <asp:Label ID="lbSecondName" runat="server" Text="Презиме"></asp:Label></p>
                            <asp:TextBox ID="tbxSecondName" CssClass="mandatory" runat="server"></asp:TextBox>
                        </div>
                        <div class="span3">
                            <p>
                                <asp:Label ID="lbLastName" CssClass="required" runat="server" Text="Фамилия"></asp:Label></p>
                            <asp:TextBox ID="tbxLastName" CssClass="mandatory" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="span3">
                            <p>
                                <asp:Label ID="Label12" runat="server" Text="Име (EN)"></asp:Label></p>
                            <asp:TextBox ID="tbxFirstNameEN" runat="server"></asp:TextBox>
                        </div>
                        <div class="span3">
                            <p>
                                <asp:Label ID="Label13" runat="server" Text="Презиме (EN)"></asp:Label>
                                
                            </p>
                            <asp:TextBox ID="tbxSecondNameEN" runat="server"></asp:TextBox>
                        </div>
                        <div class="span3">
                            <p>
                                <asp:Label ID="Label14" runat="server" Text="Фамилия (EN)"></asp:Label></p>
                            <asp:TextBox ID="tbxLastNameEN" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="span3">
                <p>
                    <asp:Label ID="lbEGN" runat="server" Text="ЕГН"></asp:Label></p>
                <asp:TextBox ID="tbxEGN" CssClass="mandatory" runat="server"></asp:TextBox>
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="lbIdentityNumber" runat="server" Text="ЛНЧ"></asp:Label></p>
                <asp:TextBox ID="tbxIdentityNumber" CssClass="mandatory" runat="server"></asp:TextBox>
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="lbIDN" runat="server" Text="ИДН"></asp:Label></p>
                 <asp:TextBox ID="tbxIDN" CssClass="mandatory" runat="server"></asp:TextBox>
            </div>
            <div class="span3" id="divIDN" runat="server"> 
                <p>
                    <asp:Label ID="Label19" runat="server" Text="Генериране на ИДН"></asp:Label></p>
            
            </div>
        </div>
        <div class="row">
            <div class="span3">
                <p>
                    <asp:Label ID="lbCitizenship" runat="server" Text="Гражданство"></asp:Label></p>
                <uc1:SMCDropDownList ID="ddlCitizenship" runat="server" KeyTypeIntCode="Nationality"
                    KeyValueDefault="BG" ShowButton="false" DataSourceType="Country" />
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="lbSecondCitizenship" runat="server" Text="Гражданство2"></asp:Label></p>
                <uc1:SMCDropDownList ID="ddlSecondCitizenship" runat="server" KeyTypeIntCode="Nationality"
                    KeyValueDefault="BG" ShowButton="false" DataSourceType="Country" />
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="lbForeignerIdentityNumber" runat="server" Text="ИДН чужденец"></asp:Label></p>
                <asp:TextBox ID="tbxForeignerIdentityNumber" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span3">
                <p>
                    <asp:Label ID="lbIDCard" runat="server" Text="Лична карта"></asp:Label></p>
                <asp:TextBox ID="tbxIDCard" runat="server"></asp:TextBox>
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="lbIdentityNumberDate" runat="server" Text="Издадена на"></asp:Label></p>
                <uc2:SMCCalendar ID="tbxIdentityNumberDate" runat="server" />
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="lbIdentityNumberIssueBy" runat="server" Text="Издадена от"></asp:Label></p>
                <asp:TextBox ID="tbxIdentityNumberIssueBy" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span3">
                <p>
                    <asp:Label ID="lbSex" runat="server" Text="Пол"></asp:Label></p>
                <uc1:SMCDropDownList ID="dllSex" runat="server" KeyTypeIntCode="Sex" ShowButton="false" />
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="lbBirthDate" runat="server" Text="Дата на раждане"></asp:Label>
                </p>
                <uc2:SMCCalendar ID="tbxBirthDate" runat="server" />
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="Label1" runat="server" Text="Семейно положение"></asp:Label></p>
                <uc1:SMCDropDownList ID="ddlMaritalStatus" runat="server" KeyTypeIntCode="Maritalstatus"
                    ShowButton="false" />
            </div>
        </div>
        <div class="row">
            <div class="span12">
                <p>
                    Месторождение
                </p>
            </div>
        </div>
        <div class="row">
            <div class="span3">
                <p>
                    <asp:Label ID="Label2" runat="server" Text="Държава"></asp:Label></p>
                <uc1:SMCDropDownList ID="ddlCitizenshipPlaceOfBirth" runat="server" KeyTypeIntCode="Nationality"
                    DefaultValueByName="България" KeyValueDefault="BG" ShowButton="false" DataSourceType="Country"
                    AutoPostBack="true" AdditionalParam="WithoutEmptyCountry" OnSelectedIndexChanged="ddlCitizenshipPlaceOfBirth_SelectedIndexChanged" />
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="lbMunicipality" runat="server" Text="Община"></asp:Label></p>
                <uc1:SMCDropDownList ID="ddlMunicipalityPlaceOfBirth" runat="server" ShowButton="false"
                    DataSourceType="Municipality" AutoPostBack="true" OnSelectedIndexChanged="ddlMunicipalityPlaceOfBirth_SelectedIndexChanged" />
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="lbLocation" runat="server" Text="Населено място"></asp:Label></p>
                <uc3:SMCAutoComplete ID="ucAutoCompleteLocationPlaceOfBirth" runat="server" TableForSelection="LocationView"
                    ColumnNameForSelection="LocationTypeName" ColumnIdForSelection="idLocation" ColumnNameForOrderBy="Name" />
            </div>
        </div>
        <div class="row">
            <div class="span12">
                <p>
                    Постоянен адрес в България
                </p>
            </div>
        </div>
        <div class="row">
            <div class="span3">
                <p>
                    <asp:Label ID="Label3" runat="server" Text="Държава"></asp:Label></p>
                <uc1:SMCDropDownList ID="ddlCitizenshipPermanentAdress" runat="server" KeyTypeIntCode="Nationality"
                    DefaultValueByName="България" KeyValueDefault="BG" ShowButton="false" DataSourceType="Country"
                    AutoPostBack="true" AdditionalParam="WithoutEmptyCountry" OnSelectedIndexChanged="ddlCitizenshipPermanentAdress_SelectedIndexChanged" />
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="Label4" runat="server" Text="Община"></asp:Label></p>
                <uc1:SMCDropDownList ID="ddlMunicipalityPermanentAdress" runat="server" ShowButton="false"
                    DataSourceType="Municipality" AutoPostBack="true" OnSelectedIndexChanged="ddlMunicipalityPermanentAdress_SelectedIndexChanged" />
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="Label5" runat="server" Text="Населено място"></asp:Label></p>
                <uc3:SMCAutoComplete ID="ucAutoCompleteLocationPermanentAdress" runat="server" TableForSelection="LocationView"
                    ColumnNameForSelection="LocationTypeName" ColumnIdForSelection="idLocation" ColumnNameForOrderBy="Name" />
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="Label6" runat="server" Text="Пощенски код"></asp:Label></p>
                <asp:TextBox ID="tbxPermanentPostCode" Text="" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span9">
                <p>
                    <asp:Label ID="lbPermanentAddressAddition" runat="server" Text="Постоянен адрес в България (кв., ул., бл., ап.)"></asp:Label></p>
                <asp:TextBox ID="tbxPermanentAddressAddition" runat="server" TextMode="MultiLine"
                    CssClass="TextBoxDescription"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span12">
                <p>
                    Адрес за кореспонденция
                </p>
            </div>
        </div>
        <div class="row">
            <div class="span3">
                <p>
                    <asp:Label ID="Label7" runat="server" Text="Държава"></asp:Label></p>
                <uc1:SMCDropDownList ID="ddlCitizenshipCorespondention" runat="server" KeyTypeIntCode="Nationality"
                    KeyValueDefault="BG" ShowButton="false" DataSourceType="Country" AutoPostBack="true"
                    DefaultValueByName="България" AdditionalParam="WithoutEmptyCountry" OnSelectedIndexChanged="ddlCitizenshipCorespondention_SelectedIndexChanged" />
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="Label8" runat="server" Text="Община"></asp:Label></p>
                <uc1:SMCDropDownList ID="ddlMunicipalityCorespondation" runat="server" ShowButton="false"
                    DataSourceType="Municipality" AutoPostBack="true" OnSelectedIndexChanged="ddlMunicipalityCorespondation_SelectedIndexChanged" />
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="Label9" runat="server" Text="Населено място"></asp:Label></p>
                <uc3:SMCAutoComplete ID="ucAutoCompleteLocationCoreposndation" runat="server" TableForSelection="LocationView"
                    ColumnNameForSelection="LocationTypeName" ColumnIdForSelection="idLocation" ColumnNameForOrderBy="Name" />
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="Label10" runat="server" Text="Пощенски код"></asp:Label></p>
                <asp:TextBox ID="tbxCorespondationPostCode" Text="" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span9">
                <p>
                    <asp:Label ID="Label11" runat="server" Text="Настоящ адрес  (кв., ул., бл., ап.)"></asp:Label></p>
                <asp:TextBox ID="tbxCorespondationAddress" runat="server" TextMode="MultiLine" CssClass="TextBoxDescription"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span3">
                <p>
                    <asp:Label ID="lbPhone" runat="server" Text="Телефон"></asp:Label></p>
                <asp:TextBox ID="tbxPhone" runat="server"></asp:TextBox>
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="lbMobilePhone" runat="server" Text="Мобилен телефон"></asp:Label></p>
                <asp:TextBox ID="tbxMobilePhone" runat="server"></asp:TextBox>
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="lbEMail" runat="server" Text="E-mail"></asp:Label></p>
                <asp:TextBox ID="tbxEMail" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row" runat="server" id="idDIVAdministrativeInformation" visible="false">
            <div class="span3">
                <p>
                    <asp:Label ID="Label15" runat="server" Text="Карта за достъп 1"></asp:Label></p>
                <asp:TextBox ID="tbxAccessCard1" runat="server"></asp:TextBox>
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="Label16" runat="server" Text="Карта за достъп 2"></asp:Label></p>
                <asp:TextBox ID="tbxAccessCard2" runat="server"></asp:TextBox>
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="Label17" runat="server" Text="Длъжност"></asp:Label></p>
                <uc1:SMCDropDownList ID="ddlPosition" runat="server" KeyTypeIntCode="Position" ShowButton="false" />
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
                        <asp:BoundField DataField="FullName"                HeaderText="Full name" />
                        <asp:BoundField DataField="IdentityNumber"          HeaderText="Identity number" />
                        <asp:BoundField DataField="CreationDate"            HeaderText="Date of change" DataFormatString="{0:dd.MM.yyyy}"/>
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
                        <asp:Label ID="lbLoadStudentInfoBy" runat="server" Text="Load data by" Font-Bold="true"
                            Visible="true"></asp:Label></p>
                    <asp:DropDownList ID="ddlLoadStudentInfoBy" runat="server" Visible="true" CssClass="span4"
                        Font-Bold="true" AutoPostBack="True" OnSelectedIndexChanged="ddlLoadStudentInfoBy_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <p>
                        <asp:Label ID="lbTextForSearchStudent" runat="server" Text="Text for search" Font-Bold="true"
                            Visible="true"></asp:Label></p>
                    <uc3:SMCAutoComplete ID="ucTextForSearchStudent" runat="server" CssClassTextBox="span12" />
                </div>
            </div>
            
        </div>
    </asp:Panel>
    <asp:HiddenField ID="hdnParentControlID"    runat="server" />
    <asp:HiddenField ID="hdnDetailsKey"         runat="server" />
    <asp:HiddenField ID="hdnRowMasterKey"       runat="server" />
    <asp:HiddenField ID="hdnIdStudentCandidate" runat="server" />
    <asp:HiddenField ID="hdnIdAcademicPeriod"   runat="server" />
    <asp:HiddenField ID="hdnNoLoadUserPanel"    runat="server" />
</asp:Panel>
