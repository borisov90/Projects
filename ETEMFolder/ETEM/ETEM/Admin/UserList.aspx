<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="UserList.aspx.cs" Inherits="ETEM.Admin.UserList" %>

<%@ Register Src="../Controls/Common/AlphaBetCtrl.ascx" TagName="AlphaBetCtrl" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx" TagName="SMCDropDownList"
    TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Admin/UserData.ascx" TagName="UserData" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/Common/SMCTextArea.ascx" TagName="SMCTextArea" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/Common/SMCAutoCompleteTextBox.ascx" TagName="SMCAutoComplete"
    TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div id="btns-students-holder">
        <div class="row">
            <div class="span2">
                <asp:Button ID="btnFilter" runat="server" CssClass="alphabet alphabet-additional modalWindow"
                    Text="Filter" OnClick="btnFilter_Click" />
            </div>
            <div class="span2">
                <asp:Button ID="bntNew" runat="server" CssClass="alphabet alphabet-additional newItemAdd modalWindow"
                    Text="New" OnClick="btnNew_Click" />
                <asp:Button ID="btnSendEmailsPopUp" runat="server" CssClass="btn modalWindow" Text="Изпрати E-Mail"
                    OnClick="btnSendEmailsPopUp_Click" />&nbsp;
            </div>
            <div class="span4">
                <uc2:SMCDropDownList ID="ddlPagingRowsCount" runat="server" KeyTypeIntCode="PagingRowsCount"
                    ShowButton="false" OrderBy="V_Order" KeyValueDefault="TwentyRowsPerPage" CssClassDropDown="width6-important"
                    AddingDefaultValue="false" KeyValueDataValueField="DefaultValue1" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlPagingRowsCount_SelectedIndexChanged" />
            </div>
            
            
        </div>
    </div>
    <div class="row-fluid">
        <div class="span1">
            <uc1:AlphaBetCtrl ID="AlphaBetCtrl" runat="server" />
        </div>
        <div class="span11">
            <asp:GridView ID="gvUsers" runat="server" CssClass="MainGrid" AutoGenerateColumns="False"
                OnSorting="gvUsers_Sorting" AllowSorting="true" AllowPaging="True" OnPageIndexChanging="gvUsers_PageIndexChanging"
                PageSize="20">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="20px" ItemStyle-CssClass="MainGrid_td_item_center"
                        HeaderText="№">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                        <ItemStyle CssClass="MainGrid_td_item_center" Width="20px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnServerEdit" runat="server" CausesValidation="False" OnClick="lnkBtnServerEdit_Click"
                                Text="" ToolTip='<%# GetCaption("GridView_Edit") %>' CommandArgument='<%# "idRowMasterKey=" + Eval("IdEntity") %>'>
                                <i class="icon-pencil"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="24px" />
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="24px" ItemStyle-CssClass="MainGrid_td_item_center">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chbxSelectOrDeselectAll" runat="server" OnCheckedChanged="chbxSelectOrDeselectAll_OnCheckedChanged"
                                CssClass="disableable" AutoPostBack="true" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chbxCheckForSend" runat="server" />
                            <asp:HiddenField ID="hdnIdEntity" runat="server" Value='<%# Bind("IdEntity") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="UserName" HeaderText="User name" SortExpression="UserName" />
                    <asp:BoundField DataField="PersonFullName" HeaderText="Person full name" SortExpression="PersonFullName" />
                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                    <asp:BoundField DataField="Roles" HeaderText="Roles" SortExpression="Roles" />
                    <asp:BoundField DataField="AddInfo" HeaderText="More information" SortExpression="AddInfo" />
                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="lbEmptyDataText" runat="server" Text='<%# GetCaption("GridView_EmptyDataRow") %>' />
                </EmptyDataTemplate>
                <PagerSettings PageButtonCount="20" />
                <PagerStyle CssClass="cssPager" />
            </asp:GridView>
        </div>
    </div>
    <uc3:UserData ID="ucUserData" runat="server" />
    <asp:Panel ID="pnlFilterData" runat="server" Visible="false" CssClass="modalPopup-middle">
        <div class="newItemPopUp">
            <div class="offset01">
                <h4>
                    Filter data</h4>
            </div>
            <div class="pnl-size-icons">
                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/close3.png"
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
                <div class="span3">
                    <p>
                        <asp:Label ID="lbUserName" runat="server" Text="Потребителско име"></asp:Label></p>
                    <asp:TextBox ID="tbxUserName" runat="server"></asp:TextBox>
                </div>
                <div class="span3">
                    <p>
                        <asp:Label ID="lbStatus" runat="server" Text="Статус"></asp:Label></p>
                    <uc2:SMCDropDownList ID="ddlStatus" runat="server" KeyTypeIntCode="UserStatus" ShowButton="false" />
                </div>
            </div>
            <div class="row">
                <div class="span3">
                    <p>
                        <asp:Label ID="lbFirstName" runat="server" Text="Име"></asp:Label></p>
                    <asp:TextBox ID="tbxFirstName" runat="server"></asp:TextBox>
                </div>
                <div class="span3">
                    <p>
                        <asp:Label ID="lbSecondName" runat="server" Text="Презиме"></asp:Label></p>
                    <asp:TextBox ID="tbxSecondName" runat="server"></asp:TextBox>
                </div>
                <div class="span3">
                    <p>
                        <asp:Label ID="lbLastName" runat="server" Text="Фамилия"></asp:Label></p>
                    <asp:TextBox ID="tbxLastName" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="span3">
                    <p>
                        <asp:Label ID="lbEGN" runat="server" Text="ЕГН"></asp:Label></p>
                    <asp:TextBox ID="tbxEGN" runat="server"></asp:TextBox>
                </div>
                <div class="span3">
                    <p>
                        <asp:Label ID="lbIdentityNumber" runat="server" Text="ЛНЧ"></asp:Label></p>
                    <asp:TextBox ID="tbxIdentityNumber" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="span3">
                    <p>
                        <asp:Label ID="Label3" runat="server" Text="Роля"></asp:Label></p>
                    <uc2:SMCDropDownList ID="ddlRole" runat="server" DataSourceType="Role" ShowButton="false" />
                </div>
            </div>
            <div class="row">
                <div class="span9 size-16">
                    <hr />
                    <p>
                        Filter - студенти
                    </p>
                </div>
            </div>
            <div class="row">
                <div class="span3">
                    <p>
                        <asp:Label ID="lbFacultyNo" runat="server" Text="Фак. №"></asp:Label></p>
                    <asp:TextBox ID="tbxFacultyNo" runat="server"></asp:TextBox>
                </div>
                <div class="span3">
                    <p>
                        <asp:Label ID="lbFaculty" runat="server" Text="Факултет"></asp:Label></p>
                    <uc2:SMCDropDownList ID="ddlFaculty" runat="server" DataSourceType="Faculty" ShowButton="false"
                        KeyValueDataTextField="DefaultValue1" />
                </div>
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <p>
                        <asp:Label ID="lbSpeciality" runat="server" Text="Специалност"></asp:Label></p>
                    <uc5:SMCAutoComplete ID="tbxAutoCompleteSpecialityFilter" runat="server" CustomCase="SpecialtyAllData"
                        CssClassTextBox="span10" />
                </div>
            </div>
            <div class="row">
                <div class="span3">
                    <p>
                        <asp:Label ID="lbCourse" runat="server" Text="Курс"></asp:Label></p>
                    <uc2:SMCDropDownList ID="ddlCourse" runat="server" ShowButton="false" KeyTypeIntCode="Course"
                        OrderBy="V_Order" />
                </div>
                <div class="span3">
                    <p>
                        <asp:Label ID="lbStudentDegree" runat="server" Text="ОКС"></asp:Label></p>
                    <uc2:SMCDropDownList ID="ddlStudentDegree" runat="server" ShowButton="false" KeyTypeIntCode="StudentDegree" />
                </div>
            </div>
            <div class="row">
                <div class="span3">
                    <p>
                        <asp:Label ID="Label1" runat="server" Text="Статус"></asp:Label></p>
                    <uc2:SMCDropDownList ID="ddlStudentStatus" runat="server" ShowButton="false" KeyTypeIntCode="StatusStudent"
                        CssClassDropDown="span4" />
                </div>
                <div class="span3">
                    <p>
                        <asp:Label ID="Label2" runat="server" Text="Чужденец"></asp:Label></p>
                    <uc2:SMCDropDownList ID="ddlForeigner" runat="server" KeyTypeIntCode="YES_NO" ShowButton="false"
                        CssClassDropDown="span4" />
                </div>
            </div>
            <div class="row">
                <div class="span2">
                    <asp:Button ID="btnSearch" CssClass="btn closeModalWindow" runat="server" Text="Search"
                        OnClick="btnSearch_Click" />
                </div>
                <div class="span2">
                    <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Clear" OnClick="btnClear_Click" />
                </div>
                <%-- <div class="span2">
                    <asp:Button ID="btnCancel" CssClass="btn" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                </div>--%>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlSendEmail" runat="server" Visible="false" CssClass="modalPopup-middle">
        <div class="newItemPopUp">
            <div class="offset01">
                <h4>
                    Изпращане на E-mail</h4>
            </div>
            <div class="pnl-size-icons">
                <asp:ImageButton ID="imgBtnCancelSendEmail" runat="server" ImageUrl="~/Images/close3.png"
                    CssClass="closeModalWindow" OnClick="btnCancelSendEmail_Click" />
            </div>
        </div>
        <div class="row">
            <div class="span8">
            </div>
        </div>
        <div class="container-fluid">
            <div class="row">
                <div class="span8">
                    <p>
                        <asp:Label ID="lbSubject" runat="server" Text="Заглавие"></asp:Label></p>
                    <asp:TextBox ID="tbxSubject" runat="server" CssClass="span8"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="span8">
                    <p>
                        <asp:Label ID="lbBody" runat="server" Text="Съдържание"></asp:Label></p>
                    <uc4:SMCTextArea ID="tbxBody" runat="server" CssClass="TextBoxDescription height6 span8 maxLengthable1000"  />
                </div>
            </div>
            <div class="row">
                <div class="span8">
                    <span class="span2">
                        <asp:Button ID="btnSendEmails" CssClass="btn" runat="server" Text="Изпрати E-mail"
                            OnClick="btnSendEmails_Click" />
                    </span>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
