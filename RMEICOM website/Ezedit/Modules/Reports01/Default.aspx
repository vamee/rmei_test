<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="Ezedit_Modules_Reports01_Default" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<p>
        <asp:Label ID="lblPageTitle" runat="server" CssClass="pageTitle" Text="Reports" />
    </p>
    <asp:label id="lblAlert" runat="server" cssclass="message"></asp:label>
	<hr />
	<br />
	<asp:HyperLink ID="hypFormSubmissions" runat="server" CssClass="main-bold" Text="Form Submissions" NavigateUrl="FormSubmissions/"></asp:HyperLink><br />
	<asp:Label ID="lblFormSubmissionsDesc" runat="server" CssClass="main" Text="View or download form results."></asp:Label>
</asp:Content>

