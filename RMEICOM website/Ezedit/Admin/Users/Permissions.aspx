<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false" CodeFile="Permissions.aspx.vb" Inherits="Ezedit_Admin_Permissions" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <p class="pagetitle">
		Admin &gt; Ez-Edit User Permissions
	</p>
	<asp:label id="lblMessage" runat="server" cssclass="message"></asp:label>
	<hr>
	<br>
	<table width="100%" cellpadding="5" cellspacing="0"  border="0">
		<tr>
			<td width="50%" valign="top">
				<table width="100%" cellpadding="0" cellspacing="0"  border="1" bordercolor="#999999">
					<tr>
						<td width="100%" valign="top">
							<table border="0" cellpadding="5" cellspacing="0" width="100%">
								<tr bgcolor="#7288ad">
									<td class="table_header"><input type="checkbox" name="selectAll" value="" onclick="checkAll(this);">&nbsp;Site Content</td>
								</tr>
								<asp:label id="lblSiteContent" runat="server"></asp:label></table> 
						</td>
					</tr>
				</table>
			</td>
			<td width="50%" valign="top">
				<table width="100%" cellpadding="0" cellspacing="0"  border="1" bordercolor="#999999">
					<tr>
						<td width="100%" valign="top">
							<table border="0" cellpadding="5" cellspacing="0" width="100%">
								<tr bgcolor="#7288ad">
									<td class="table_header">Applications</td>
								</tr>
								<asp:label id="lblApplications" runat="server"></asp:label></table> 
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
		<input type="hidden" name="pageId" value="NULL">

	</div>
	<script type="text/javascript" language="javascript">
	
        function selectPermissions(selection) {
	        window.document.forms[0].pageId.value="";
	        selectionName = selection.name;
	                	
	        for (var i=0; i < window.document.forms[0].elements.length; i++) {
		        itemName = window.document.forms[0].elements[i].name;
		                		
		        if (itemName.indexOf(selectionName) >= 0 && selectionName != itemName) {
			        if (itemName.length > selectionName.length) {
				        nextChar = itemName.charAt(selectionName.length);
				        if (nextChar == "_") {
					        window.document.forms[0].elements[i].checked = selection.checked;
				        }
			        }else{
				        window.document.forms[0].elements[i].checked = selection.checked;
			        }
		        }
	        }

	        for (var i=0; i < window.document.forms[0].elements.length; i++) {
		        if (window.document.forms[0].elements[i].checked) {
			        aryIds = window.document.forms[0].elements[i].name.split("_");
			        window.document.forms[0].pageId.value = window.document.forms[0].pageId.value + aryIds[aryIds.length - 1] + ",";
		        }
	        }
        }


        function checkAll(selection) {
	        window.document.forms[0].pageId.value="";
	        selectionName = selection.name;
        	
	        for (var i=0; i < window.document.forms[0].elements.length; i++) {
		        itemName = window.document.forms[0].elements[i].name;
        		
		        if (itemName.indexOf("pageId_") >= 0) {
			        window.document.forms[0].elements[i].checked = selection.checked;
			        aryIds = window.document.forms[0].elements[i].name.split("_");
			        window.document.forms[0].pageId.value = window.document.forms[0].pageId.value + aryIds[aryIds.length - 1] + ",";
		        }
        		
	        }
        }
    </script>

</asp:Content>


