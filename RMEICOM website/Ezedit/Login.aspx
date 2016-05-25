<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Ezedit_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
      <title>Emagine - EzEdit - Login</title>
</head>
<body onload="document.forms.form1.txtUserId.focus();" class="login">
    <form id="form1" runat="server">
        <div style="text-align:center">
	        <asp:Image SkinID="LoginLogo" runat="server" />
        </div>
        <div style="text-align:center;">
            <asp:label id="lblResults" runat="server" CssClass="message" Visible="false">Results:</asp:label>
        </div>
        <table id="tblLogin" cellspacing="3" cellpadding="3" align="center">
            <tr>
                <td class="login-label">Username:</td>
                <td>
                    <asp:textbox id="txtUserId" runat="server" Width="100px"></asp:textbox>
                </td>
            </tr>
            <tr>
                <td class="login-label">Password:</td>
                <td>
                    <asp:textbox id="txtPassword" runat="server" textmode="Password" Width="100px"></asp:textbox>
                </td>
            </tr>                    
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:button id="btnLogin" runat="server" text="Login" OnClick="btnLogin_Click" CssClass="button-login"/>
                </td>
            </tr>
        </table>
    </form>
</body>

</html>
