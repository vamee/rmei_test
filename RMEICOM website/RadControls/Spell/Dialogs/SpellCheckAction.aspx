<%@ Page language="c#" SmartNavigation="false" AutoEventWireup="false" Inherits="Telerik.WebControls.Dialogs.SpellCheckAction" %>

<html>
  <head runat="server">
</head>
<body id="actionBody" runat="server">
<form method="post" id="mainForm" name="mainForm" runat="server">
<input type="hidden" id="customWord" name="customWord" runat="server" >
<input type="hidden" id="text" name="text" runat="server" >
<span id="errorMessage" runat="server" style="display:none"></span>
</form>
</body>
</html>
