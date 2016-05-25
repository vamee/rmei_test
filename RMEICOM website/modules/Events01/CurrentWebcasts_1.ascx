<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CurrentWebcasts_1.ascx.vb" Inherits="Events01_CurrentWebcasts_1" %>
<asp:Label ID="lblAlert" CssClass="message" runat="server"></asp:Label>
<asp:Repeater ID="rptEvents" runat="server">
<HeaderTemplate>
  <table cellpadding="3" cellspacing="0" border="0" width="100%">
    
</HeaderTemplate>

<ItemTemplate>
    <tr class='table-row'>
      <td><asp:Label ID="lblEventDate" runat="server" CssClass="main"></asp:Label> - <asp:Label ID="lblEventName" runat="server" CssClass="page-subheader"></asp:Label></td>
      <td align="right"><asp:HyperLink ID="hypRegister" runat="server" CssClass="main" Text="Register"></asp:HyperLink></td>
    </tr>
    <tr class='table-row'>
      <td colspan="2"><asp:Label ID="lblEventDescription" runat="server" CssClass="main"></asp:Label></td>
    </tr>
</ItemTemplate>
<AlternatingItemTemplate>
    <tr class='table-altrow'>
      <td><asp:Label ID="lblEventDate" runat="server" CssClass="main"></asp:Label> - <asp:Label ID="lblEventName" runat="server" CssClass="page-subheader"></asp:Label></td>
      <td align="right"><asp:HyperLink ID="hypRegister" runat="server" CssClass="main" Text="Register"></asp:HyperLink></td>
    </tr>
    <tr class='table-altrow'>
      <td colspan="2"><asp:Label ID="lblEventDescription" runat="server" CssClass="main"></asp:Label></td>
    </tr>
</AlternatingItemTemplate>

<FooterTemplate>
      
  </table>
</FooterTemplate>
</asp:Repeater>