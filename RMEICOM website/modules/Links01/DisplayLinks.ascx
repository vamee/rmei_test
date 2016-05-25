<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DisplayLinks.ascx.vb" Inherits="UserControls_ContentLinks" ClassName="ContentLinks" %>
<asp:Label ID="lblAlert" runat="server" CssClass="message" EnableViewState="false"></asp:Label>
<asp:Repeater ID="rptDownloadLinks" runat="server" Visible="false">
<HeaderTemplate>
<table width="100%" cellpadding="3" cellspacing="0" border="0">
  <tr>
    <td class="table-header">Name</td>
    <td class="table-header">Size</td>
    <td class="table-header"><br /></td>
  </tr>
</HeaderTemplate>

<ItemTemplate>
  <tr class="table-row">
    <td class="table-row"><span class="main"><%#Emagine.FormatFileName(Eval("LinkURL"))%></span></td>
    <td class="table-row"><asp:label ID="lblFileSize" CssClass="main" runat="server"></asp:label></td>
    <td class="table-row" align="right"><asp:HyperLink ID="hypDownload" runat="server" CssClass="main" Text="Download"></asp:HyperLink></td>
  </tr>
</ItemTemplate>

<AlternatingItemTemplate>
  <tr class="table-altrow">
    <td class="table-altrow"><span class="main"><%#Emagine.FormatFileName(Eval("LinkURL"))%></span></td>
    <td class="table-altrow"><asp:label ID="lblFileSize" CssClass="main" runat="server"></asp:label></td>
    <td align="right" class="table-altrow"><asp:HyperLink ID="hypDownload" runat="server" CssClass="main" Text="Download"></asp:HyperLink></td>
  </tr>
</AlternatingItemTemplate>

<FooterTemplate>
</table>
</FooterTemplate>

</asp:Repeater>