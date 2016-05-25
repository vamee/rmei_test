<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MemberLoginStatus.ascx.vb"
    Inherits="modules_Membership_MemberLoginStatus" %>
<div id="loginStatus">
<asp:Panel ID="pnlLoggedIn" runat="server">
    
        <asp:Label ID="lblLoginName" runat="server" />&nbsp;&nbsp;|&nbsp;&nbsp;<asp:LinkButton
            ID="btnLogOut" runat="server" Text="Logout" />
</asp:Panel>

<asp:Panel ID="pnlLoggedOff" runat="server" Visible="false">
    <asp:LinkButton ID="btnLogin" runat="server" Text="Login" />
</asp:Panel>
</div>