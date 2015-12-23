<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="ETEM.Test" %>

<%@ Register src="Controls/CostCalculation/Profile.ascx" tagname="Profile" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        

        <table>
            <tr>
            <td colspan=3>
            Operations and functions: 
            +, -, *, ^(power),  /(Division), %(Module), sin(), cos(), tan(), sqrt(), factorial()
            </td>

            </tr>
             <tr>
            <td colspan=3>
            Example: x^2 + 2*x + 1
            </td>

            </tr>
            <tr>
                <td>Expression:
                </td>
                <td><asp:TextBox ID="tbxExpr" runat="server"></asp:TextBox>
                </td>
                <td><asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />
                </td>
            </tr>

            <tr>
                <td>A:
                </td>
                <td>
                    <asp:TextBox ID="tbxKey1" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="tbxValue1" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>B:
                </td>
                <td>
                    <asp:TextBox ID="tbxKey2" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="tbxValue2" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>C:
                </td>
                <td>
                    <asp:TextBox ID="tbxKey3" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="tbxValue3" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>D:
                </td>
                <td>
                    <asp:TextBox ID="tbxKey4" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="tbxValue4" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
            <td colspan=3><asp:TextBox ID="tbxResult" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
            <td colspan=3>
                <asp:Label ID="lbError" runat="server" Text=""></asp:Label>
                
                </td>
            </tr>
        </table>

    </div>
        tbxIDSAPData: <asp:TextBox ID="tbxIDSAPData" runat="server"></asp:TextBox><asp:Button ID="btnLoadSAPData" runat="server" Text="btnLoadSAPData" OnClick="btnLoadSAPData_Click" />
    </form>
    
</body>
</html>
