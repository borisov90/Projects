<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SMCModelPictureWindow.ascx.cs" Inherits="ETEM.Controls.Common.SMCModelPictureWindow" %>
<div id="ModalShowPicture" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
  <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
    <h3 id="ModalLabel" runat="server"></h3>
  </div>
  <div class="modal-body">
    <p><img scr="" alt="snimka" id="img" class="bigSizePicture" runat="server"/></p>
  </div>
  <%--<div class="modal-footer">
    <button class="btn" data-dismiss="modal" aria-hidden="true">Close</button>
    <button class="btn btn-primary">Save changes</button>
  </div>--%>
</div>

<asp:HiddenField id="hdnHeadline" runat="server"/>
<asp:HiddenField id="hdnImagePath" runat="server"/>
