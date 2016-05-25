<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MemberLogin.ascx.vb"
    Inherits="modules_Membership_MemberLogin" %>
<asp:Label ID="lblAlert" runat="server" CssClass="alert" />

<asp:Panel ID="pnlLogin" runat="server" DefaultButton="btnLogin" CssClass="homeLoginForm">
    <VAM:ValidationSummary ID="ValidationSummary2" runat="server" HeaderText="The following errors were encountered:" Group="Login" />
    <table cellspacing="0" cellpadding="5">
        <tr>
            <td align="right">
                <asp:Label ID="lblUsername" runat="server" CssClass="form-label" Text="Username:&nbsp;" />
            </td>
            <td>
                <asp:TextBox ID="txtUsername" runat="server" Width="150" CssClass="form-textbox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lblPassword" runat="server" CssClass="form-label" Text="Password:&nbsp;" />
            </td>
            <td>
                <asp:TextBox ID="txtPassword" runat="server" Width="150" CssClass="form-textbox"
                    TextMode="password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
            </td>
            <td>
                <asp:CheckBox ID="cbxIsLoggedIn" runat="server" Text="Keep me logged in." CssClass="form-label" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <VAM:Button ID="btnLogin" runat="server" Text="Log In" CssClass="form-button" Group="Login" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:LinkButton ID="btnDisplayPasswordLookup" runat="server" Text="Forgot your password?" />
            </td>
        </tr>
    </table>
    <VAM:RequiredTextValidator ID="RequiredTextValidator2" runat="server" ControlIDToEvaluate="txtUsername"
        SummaryErrorMessage="Username is required." Group="Login" />
    <VAM:RequiredTextValidator ID="RequiredTextValidator1" runat="server" ControlIDToEvaluate="txtPassword"
        SummaryErrorMessage="Password is required." Group="Login" />
</asp:Panel>
<asp:Panel ID="pnlPasswordLookup" runat="server" Visible="false" CssClass="homeLoginForm"
    Height="150">
    <VAM:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="The following errors were encountered:" Group="Lookup" />
    <table cellspacing="0" cellpadding="5">
        <tr>
            <td align="right">
                <asp:Label ID="Label1" runat="server" CssClass="form-label" Text="Email:" />
            </td>
            <td>
                <asp:TextBox ID="txtEmailLookup" runat="server" Width="175" CssClass="form-textbox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <VAM:Button ID="btnPasswordLookup" runat="server" Text="Email My Password" CssClass="form-button" Group="Lookup" />
                <asp:Button ID="btnCancelPasswordLookup" runat="server" Text="Cancel" CssClass="form-button" />
            </td>
        </tr>
    </table>
    <VAM:EmailAddressValidator ID="EmailAddressValidator1" runat="server" ControlIDToEvaluate="txtEmailLookup"
        SummaryErrorMessage="Please enter a valid email address." IgnoreBlankText="false" Group="Lookup" />
</asp:Panel>
<asp:Panel ID="pnlLoggedIn" runat="server" CssClass="homeLoginForm" Height="150"
    Visible="false">
    <table cellspacing="0" cellpadding="5">
        <tr>
            <td align="right">
                <asp:Label ID="Label2" runat="server" CssClass="form-label" Text="You are logged in as:" />
            </td>
            <td>
                <asp:Label ID="lblMemberName" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label3" runat="server" CssClass="form-label" Text="Your first login:" />
            </td>
            <td>
                <asp:Label ID="lblFirstLogin" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label4" runat="server" CssClass="form-label" Text="Your last login:" />
            </td>
            <td>
                <asp:Label ID="lblLastLogin" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label5" runat="server" CssClass="form-label" Text="You have logged in:" />
            </td>
            <td>
                <asp:Label ID="lblLoginCount" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button ID="btnLogOut" runat="server" CssClass="form-button" Text="Logout" />
            </td>
        </tr>
    </table>
</asp:Panel>
