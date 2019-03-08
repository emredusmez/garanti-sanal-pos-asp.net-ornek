<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="odeme.aspx.cs" Inherits="GarantiSanalPos.odeme" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
      
 
    <form id="form1" runat="server">
        <div>
            <table style="width: 100%">
              
                <tr>
                    <td colspan="2">Kart No:<asp:TextBox ID="txtKartNo" runat="server"></asp:TextBox> </td>

                </tr>
                <tr>
                    <td>Ay:<asp:TextBox ID="txtAy" runat="server"></asp:TextBox>Yıl:<asp:TextBox ID="txtYil" runat="server"></asp:TextBox></td>
                    <td>Cvv:<asp:TextBox ID="txtCvv" runat="server"></asp:TextBox></td>
                  
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnOdemeYap" runat="server" Text="Ödeme Yap" OnClick="btnOdemeYap_Click" /></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
