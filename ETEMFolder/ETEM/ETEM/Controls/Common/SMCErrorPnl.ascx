<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SMCErrorPnl.ascx.cs" Inherits="ETEM.Controls.Common.SMCErrorPnl" %>

 <asp:Panel runat="server" ID="pnlPersonalErrors" Visible="false" CssClass="modalPopup  pnlErrorsPopUp">
        <div class="newItemPopUp">
            <div class="offset01">
                <h4 id="H1" runat="server">
                    Възникнаха следните грешки</h4>
            </div>
            <div class="pnl-size-icons">
                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/close3.png"
                    CssClass="closeModalWindow" OnClick="btnCancelParentPanel_OnClick" />
            </div>
        </div>
        <div class="container-fluid">
            <div class="row span12Separator">
                <div class="span12">
                </div>
            </div>
            <div class="row-fluid">
                <div class="span12 errorColor">
                    <asp:BulletedList ID="blPersonalEroorsSave" BulletStyle="Disc" DisplayMode="Text"
                        runat="server">
                    </asp:BulletedList>
                </div>
            </div>

        </div>
    </asp:Panel>
