<%@ Page ValidateRequest="false" language="c#" Codebehind="XhtmlValidator.aspx.cs" AutoEventWireup="false" Inherits="Telerik.WebControls.RadEditorUtils.XhtmlValidator" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head runat=server>
		<title>XhtmlValidator</title>								
	</head>
	<body style="font-family:Tahoma,Arial;font-size:12px;">
		<form id="RadEditorXhtmlForm" method="post" runat="server">
			<input id="EditorContent" type="hidden" value="" name="EditorContent" runat="server">
			<input id="EditorDoctype" type="hidden" value="" name="EditorDoctype" runat="server">
			<asp:Button ID="Submit" Runat="server" Text="Click" Visible="false"></asp:Button>
			Note: This module uses  <a href="http://validator.w3.org/check">http://validator.w3.org</a> to validate content for XHTML compliance.
		</form>
	</body>
</html>
