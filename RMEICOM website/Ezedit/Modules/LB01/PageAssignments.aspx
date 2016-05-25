<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false" CodeFile="PageAssignments.aspx.vb" Inherits="Ezedit_Modules_LB01_PageAssignments" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<p class="pagetitle">
		<asp:Label ID="lblBreadcrumbs" runat="server" CssClass="breadcrumbs"></asp:Label>
	</p>
	<asp:label id="lblAlert" runat="server" cssclass="message"></asp:label>
	<hr>
	<br>
	<table width="100%" cellpadding="5" cellspacing="0"  border="0">
		<tr>
			<td valign="top">
				<table width="100%" cellpadding="0" cellspacing="0"  border="1" bordercolor="#999999">
					<tr>
						<td width="100%" valign="top">
							<table border="0" cellpadding="3" cellspacing="0" width="100%">
								<tr bgcolor="#7288ad">
									<td class="table_header">Site Content</td>
								</tr>
								<asp:label id="lblSiteContent" runat="server"></asp:label></table> 
						</td>
					</tr>
				</table>
			</td>
			
		</tr>
	</table>
	<div align="center">
		<br>
		<asp:button id="btnSave" runat="server" cssclass="button" text="Save"></asp:button>&nbsp;
		<asp:button id="btnCancel" runat="server" cssclass="button" text="Cancel"></asp:button>
	</div>
</asp:Content>

