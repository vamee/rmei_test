<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Careers.ascx.vb" Inherits="Display_Careers01" %>
<asp:Label ID="lblAlert" CssClass="message" runat="server"></asp:Label>
<asp:Label ID="lblCareerList" runat="server"></asp:Label>

<span id="JobDetail" runat="server" visible="False">
<h1><asp:Label ID="lblJobTitle" runat="server"></asp:Label></h1>
<asp:Label ID="lblJobDescription" runat="server" CssClass="main"></asp:Label>
<br /><br />
<input type="hidden" name="ApplyOnline" value="1" />
<asp:Button ID="btnApplyOnline" runat="server" Visible="false" Text="Apply Online" />
</span>