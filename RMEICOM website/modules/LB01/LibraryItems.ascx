<%@ Control Language="VB" AutoEventWireup="false" CodeFile="LibraryItems.ascx.vb" Inherits="UserControls_LibraryItems" ClassName="LibraryItems" %>
<asp:Repeater ID="VerticalItems" runat="server" Visible="false">
<HeaderTemplate>

<td valign="top" id="library">
    <table cellpadding="0" width="100%" cellspacing="0">
</HeaderTemplate>

<ItemTemplate>
    <tr>
      <td valign="top" align="left">
	  	<table class="libraryItem" cellpadding="0" cellspacing="0" width="100%" height="100%" border="0">
	  		<tr>
	  			<td valign="top" align="left"><%#Eval("Content")%></td>
	  		</tr>
		</table>
	  </td>
    </tr>

</ItemTemplate>

<FooterTemplate>
    </table>
</td>
</FooterTemplate>
</asp:Repeater>

<asp:Repeater ID="HorizontalItems" runat="server" Visible="false">

<HeaderTemplate>
<div id="libraryHoriz">
</HeaderTemplate>

<ItemTemplate>    
      <div id="horizLibItem"><%#Eval("Content")%></div>
</ItemTemplate>

<FooterTemplate>
 </div>
</FooterTemplate>
</asp:Repeater>
