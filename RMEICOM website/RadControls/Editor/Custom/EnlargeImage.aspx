<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EnlargeImage.aspx.vb" Inherits="RadControls_Editor_Custom_EnlargeImage"
    ValidateRequest="false" %>


<%@  Assembly="Version=6.0.3.0, Culture=neutral, PublicKeyToken=dbd98e0feff9f06b" Namespace="Telerik.WebControls" TagPrefix="radT" %> 

<%--<%@ Register TagPrefix="radT" Namespace="Telerik.WebControls" %>--%>

<html>
<head>
    <link href="/App_Themes/Ezedit/EzEdit.css" rel="stylesheet" type="text/css" />
</head>
<body bgcolor="#ECE9D8" onload="ConfigureDialog();">

    <script type="text/javascript" src="/RadControls/Editor/Scripts/6_6_3/RadWindow.js"></script>

    <script type="text/javascript">
	InitializeRadWindow();
    </script>

    <form id="form1" runat="server">

    <script>
//This code is used to provide a reference to the radwindow "wrapper"
function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) {
        oWindow = window.radWindow;
    }else if (window.frameElement.radWindow) {
        oWindow = window.frameElement.radWindow;
    }
    return oWindow;
}   
    </script>

    <script>
function ConfigureDialog() {
    //Get a reference to the radWindow wrapper
    var oWindow = GetRadWindow();
 
     //Obtain the argument 
     var oArg = new Object();
     var selectedText = "";
     var selectedHtml = "";
     
     if (oWindow.Argument) {
        oArg = oWindow.Argument;
        if (oArg.SelectedText) selectedText = oArg.SelectedText;
        if (oArg.SelectedHtml) selectedHtml = oArg.SelectedHtml;
     }
     
     var testImage = selectedHtml.substring(0, 4);
     if (testImage.toLowerCase() == "<img"){
        selectedText = selectedHtml;		
     }
     if (selectedText !== "") {
        var oArea = document.getElementById("<%'=txtLinkName.ClientID%>");
        oArea.value = selectedText;
     }
}

function insertLink() {
    var linkContainer = document.getElementById("linkContainer");
	var returnValue = {url:'Test1',text:'Test2'};
	CloseDlg(returnValue);
}
    </script>

    <asp:Literal ID="litAlert" runat="server"></asp:Literal>
    </form>
</body>
</html>
