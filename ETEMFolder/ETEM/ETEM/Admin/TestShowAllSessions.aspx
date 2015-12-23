<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestShowAllSessions.aspx.cs" Inherits="ETEM.Admin.TestShowAllSessions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    
    <div>
       <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns=False>
           <Columns>
               <asp:BoundField DataField="IdUser" HeaderText="IdUser" />
               <asp:BoundField DataField="PersonNamePlusTitle" 
                   HeaderText="PersonNamePlusTitle" />
               <asp:BoundField DataField="IsKilledStr" HeaderText="IsKilledStr" />
               <asp:BoundField DataField="SessionID" HeaderText="SessionID" />
           </Columns>
          
    </asp:GridView>
    
    </div>
    </form>
</body>
</html>
