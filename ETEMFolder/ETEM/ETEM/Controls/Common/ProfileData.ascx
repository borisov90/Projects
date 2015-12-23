<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProfileData.ascx.cs"
    Inherits="ETEM.Controls.Common.ProfileData" %>
<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx" TagName="SMCDropDownList"
    TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Common/SMCCalendar.ascx" TagName="SMCCalendar" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Common/SMCAutoCompleteTextBox.ascx" TagName="SMCAutoComplete"
    TagPrefix="uc3" %>
<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx" TagName="SMCDropDownList"
    TagPrefix="uc3" %>
<asp:Panel ID="pnlProfileData" runat="server" CssClass="modalPopup">
    <div class="newItemPopUp">
        <div class="offset01">
            <h4 id="H1" runat="server">
                Профил</h4>
        </div>
        <div class="pnl-size-icons">
            <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/close3.png"
                CssClass="closeModalWindow" OnClick="btnCancel_Click" />
        </div>
    </div>
    <div class="ResultContext">
        <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
    </div>
    <div class="container-fluid">
        <div class="row span12Separator">
            <div class="span12">
            </div>
        </div>
       
        <div class="row span12Separator">
            <div class="span12">
            </div>
        </div>
        <div class="row" id="idDIVChangePeriod" runat="server" visible="false">
            <div class="span3">
                <p>
                    <asp:Label ID="lbAcademycYear" runat="server" Text="Учебна година"></asp:Label></p>
                <uc1:SMCDropDownList ID="ddlAcademicYear" runat="server" KeyTypeIntCode="AcademicYear"
                    ShowButton="false" />
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="lbPeriod" runat="server" Text="Семестър"></asp:Label></p>
                <uc1:SMCDropDownList ID="ddlPeriod" runat="server" KeyTypeIntCode="Semester" WhereKeyValueIntCodesEQ="FirstSemester,SecondSemester"
                    ShowButton="false" KeyValueDataTextField="DefaultValue3" />
            </div>
            <div class="span3">
                <p>
                    Промяна на текущ период</p>
                <asp:Button ID="btnChangeAcademicPeriod" runat="server" CssClass="btn" Text="Промени"
                    OnClick="btnChangeAcademicPeriod_Click" />
            </div>
        </div>
        <div class="row span12Separator">
            <div class="span12">
            </div>
        </div>
        <div class="row">
            <div class="span3">
                <p>
                    <asp:Label ID="lbFirstName" runat="server" Text="Име"></asp:Label></p>
                <asp:TextBox ID="tbxFirstName" runat="server" Enabled="false"></asp:TextBox>
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="lbSecondName" runat="server" Text="Презиме"></asp:Label></p>
                <asp:TextBox ID="tbxSecondName" runat="server" Enabled="false"></asp:TextBox>
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="lbLastName" runat="server" Text="Фамилия"></asp:Label></p>
                <asp:TextBox ID="tbxLastName" runat="server" Enabled="false"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span3">
                <p>
                    <asp:Label ID="Label7" runat="server" Text="Държава"></asp:Label></p>
                <uc1:SMCDropDownList ID="ddlCitizenshipCorespondention" runat="server" KeyTypeIntCode="Nationality"
                    KeyValueDefault="BG" ShowButton="false" DataSourceType="Country" AutoPostBack="true"
                    DefaultValueByName="България" AdditionalParam="WithoutEmptyCountry" DropDownEnabled="false" />
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="Label8" runat="server" Text="Община"></asp:Label></p>
                <uc1:SMCDropDownList ID="ddlMunicipalityCorespondation" DropDownEnabled="false" runat="server"
                    ShowButton="false" DataSourceType="Municipality" AutoPostBack="true" />
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="Label9" runat="server" Text="Населено място"></asp:Label></p>
                <uc3:SMCAutoComplete ID="ucAutoCompleteLocationCoreposndation" runat="server" TableForSelection="LocationView"
                    ColumnNameForSelection="LocationTypeName" ReadOnly="true" ColumnIdForSelection="idLocation"
                    ColumnNameForOrderBy="Name" />
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="Label10" runat="server" Text="Пощенски код"></asp:Label></p>
                <asp:TextBox ID="tbxCorespondationPostCode" Enabled="false" Text="" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span9">
                <p>
                    <asp:Label ID="Label11" runat="server" Text="Настоящ адрес  (кв., ул., бл., ап.)"></asp:Label></p>
                <asp:TextBox ID="tbxCorespondationAddress" Enabled="false" runat="server" TextMode="MultiLine"
                    CssClass="TextBoxDescription span9"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span3">
                <p>
                    <asp:Label ID="lbPhone" runat="server" Text="Телефон"></asp:Label></p>
                <asp:TextBox ID="tbxPhone" runat="server" Enabled="false"></asp:TextBox>
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="lbMobilePhone" runat="server" Text="Мобилен телефон"></asp:Label></p>
                <asp:TextBox ID="tbxMobilePhone" runat="server" Enabled="false"></asp:TextBox>
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="lbEMail" runat="server" Text="E-mail"></asp:Label></p>
                <asp:TextBox ID="tbxEMail" runat="server" Enabled="false"> </asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span3">
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Visible="false" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                    Visible="false" Style="height: 26px" />
            </div>
        </div>
        <div class="row span12Separator">
            <div class="span12">
            </div>
        </div>
        <div class="row span12Separator">
            <div class="span12">
            </div>
        </div>
         <div class="row">
            <div class="span10">
                <asp:Button ID="btnChangePassword" runat="server" Text="Промени парола" CssClass="btn" OnClick="btnChangePassword_Click"
                    Visible="true" />
            </div>
        </div>
        <div class="row" id="idDivChangePassword" runat="server" visible="false">
            <div class="span3">
                <p>
                    <asp:Label ID="Label1" runat="server" Text="Стара парола"></asp:Label></p>
                <asp:TextBox ID="tbxOldPass" runat="server" TextMode="Password"></asp:TextBox>
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="Label2" runat="server" Text="Нова парола"></asp:Label></p>
                <asp:TextBox ID="tbxNewPass1" runat="server" TextMode="Password"> </asp:TextBox>
            </div>
            <div class="span3">
                <p>
                    <asp:Label ID="Label3" runat="server" Text="Нова парола - повторение"></asp:Label></p>
                <asp:TextBox ID="tbxNewPass2" runat="server" TextMode="Password"></asp:TextBox>
            </div>
        </div>
    </div>
</asp:Panel>
