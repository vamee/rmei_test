<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MultiSelect.ascx.vb" Inherits="DL01_MultiSelect" %>

<asp:Label ID="lblAlert" CssClass="message" runat="server" EnableViewState="false"></asp:Label>

<!--- START DOWNLOADS LIST --->
<asp:Repeater ID="rptDL01" runat="server" Visible="true">
<HeaderTemplate>
    <asp:Label ID="lblCategoryName" runat="server"></asp:Label>
    <%#Eval("CategoryName")%>
    <table cellpadding='2' cellspacing='0' width="100%" border='0'>
</HeaderTemplate>

<ItemTemplate>
    <tr id="trCategory" runat="server" visible="false">
        <td class="table-row">
            <h2><%#Eval("ResourceCategory")%></h2>
        </td>
    </tr>
   <tr>
		<td id="tdItem" runat="server">
			<table cellpadding='5' cellspacing='0' width='100%' border='0'>
				<tr>
			      	<td valign='top' width="25"><input type="checkbox" name="ResourceID" value="<%#Eval("ResourceID")%>" /></td>
			      	<td valign='top'>
			      	<b><%#Eval("ResourceName")%></b> (<%#Emagine.FormatFileSize(Eval("FileSize"))%>)<br />
				  	<%#Eval("Description")%>
				 	</td>
    			</tr>
			</table>
		</td>
    </tr>
</ItemTemplate>



<FooterTemplate>
</table>
</FooterTemplate>
</asp:Repeater>
<br />
<center><asp:Button ID="btnDownload" Text="Download" CssClass="form" runat="server" Visible="false" /></center>

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
    <td class="table-row"><span class="main"><%#Eval("ResourceName")%></span></td>
    <td class="table-row"><span class="main"><%#Emagine.FormatFileSize(Eval("FileSize"))%></span></td>
    <td class="table-row" align="right"><asp:HyperLink ID="hypDownload" runat="server" CssClass="main" Text="Download" Target="_blank"></asp:HyperLink></td>
  </tr>
</ItemTemplate>

<AlternatingItemTemplate>
  <tr class="table-altrow">
    <td class="table-altrow"><span class="main"><%#Eval("ResourceName")%></span></td>
    <td class="table-altrow"><span class="main"><%#Emagine.FormatFileSize(Eval("FileSize"))%></span></td>
    <td align="right" class="table-altrow"><asp:HyperLink ID="hypDownload" runat="server" CssClass="main" Text="Download" Target="_blank"></asp:HyperLink></td>
  </tr>
</AlternatingItemTemplate>

<FooterTemplate>
</table>
</FooterTemplate>

</asp:Repeater>

