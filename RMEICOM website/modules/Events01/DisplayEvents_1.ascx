<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DisplayEvents_1.ascx.vb" Inherits="Events01_DisplayEvents_1" %>
<asp:Label ID="lblAlert" CssClass="message" runat="server"></asp:Label>
<asp:Repeater ID="rptEvents" runat="server">
<HeaderTemplate>
  <table cellpadding="1" cellspacing="0" border="0" width="100%">
    
</HeaderTemplate>

<ItemTemplate>
    <tr>
      <td class="page-subheader"><%#Eval("ResourceName")%></td>
    </tr>
    <tr>
      <td class="main"><%#Eval("EventDescription")%></td>
    </tr>
    <tr>
      <td align="center">
  
  <asp:Repeater ID="rptEventDates" runat="server">
      <HeaderTemplate>
        <table width="100%" cellpadding="1" cellspacing="0" border="0" align="center">
          <tr class="table-header">
            <td class="table-header">Date</td>
            <td class="table-header">Location</td>
            <td class="table-header"><br /></td>
          </tr>
      </HeaderTemplate>
      
      <ItemTemplate>
        <tr class="table-row">
          <td class="table-row"><asp:Label ID="lblEventDate" runat="Server" CssClass="main"></asp:Label></td>
          <td class="table-row"><asp:Label ID="lblEventLocation" runat="Server" CssClass="main"></asp:Label></td>
          <td class="table-row" align="right"><asp:Hyperlink ID="hypEventRegister" runat="Server" CssClass="main"></asp:Hyperlink><br /></td>
        </tr>
      </ItemTemplate>
      
      <AlternatingItemTemplate>
        <tr class="table-altrow">
          <td class="table-altrow"><asp:Label ID="lblEventDate" runat="Server" CssClass="main"></asp:Label></td>
          <td class="table-row"><asp:Label ID="lblEventLocation" runat="Server" CssClass="main"></asp:Label></td>
          <td class="table-altrow" align="right"><asp:Hyperlink ID="hypEventRegister" runat="Server" CssClass="main"></asp:Hyperlink><br /></td>
        </tr>
      </AlternatingItemTemplate>
      
      <FooterTemplate>
        </table><br />
      </FooterTemplate>
  </asp:Repeater>
  
  </td>
    </tr>
  
</ItemTemplate>

<FooterTemplate>
      
  </table>
</FooterTemplate>
</asp:Repeater>