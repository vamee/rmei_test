<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProtectedLink.aspx.vb" Inherits="RadControls_Editor_Custom_ProtectedLink" %>

<%@ Register Assembly="PeterBlum.VAM, Version=3.0.8.5000, Culture=neutral, PublicKeyToken=76d6c019e89ec3c9"
    Namespace="PeterBlum.VAM" TagPrefix="VAM" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="/App_Themes/Ezedit/EzEdit.css" rel="stylesheet" type="text/css" />
</head>
<body bgcolor="#ECE9D8" onload="ConfigureDialog();">

    <script type="text/javascript" src="/RadControls/Editor/Scripts/7_0_2/RadWindow.js"></script>

    <script type="text/javascript">
	InitializeRadWindow();
    </script>

    <form id="form1" runat="server">
        <div>
            <table cellpadding="3" cellspacing="0">
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblLinkName" CssClass="form-label" Text="Link Name:" />
                        <VAM:RequiredTextValidator ID="RequiredTextValidator2" runat="server" ControlIDToEvaluate="txtLinkName"
                            ErrorMessage="**" SummaryErrorMessage="Link Name is required." />
                    </td>
                    <td>
                        <asp:TextBox ID="txtLinkName" runat="server" CssClass="form" Width="225" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblUrl" runat="server" Text="URL:" CssClass="form-label" />
                        <VAM:RequiredTextValidator ID="RequiredTextValidator1" runat="server" ControlIDToEvaluate="txtLinkUrl"
                            ErrorMessage="**" SummaryErrorMessage="URL is required." />
                    </td>
                    <td>
                        <asp:TextBox ID="txtLinkUrl" runat="server" CssClass="form" Width="225" Text="http://" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDeliveryPage" runat="server" CssClass="form-label" Text="Delivery Page:" />
                        <VAM:RequiredListValidator ControlIDToEvaluate="ddlDeliveryPage" runat="Server"
                            ID="rlvDisplayPageID" UnassignedIndex="0" SummaryErrorMessage="Please choose a delivery page" ErrorMessage="**" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDeliveryPage" runat="server" CssClass="form" AppendDataBoundItems="true">
                            <asp:ListItem Value="0"><-- Please Choose --></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <vam:Button ID="btnInsert" runat="server" CssClass="button" Text="Insert" />
                        &nbsp;
                        <button onclick="CloseDlg(null);" class="button">
                            Cancel</button>
                    </td>
                </tr>
            </table>
        </div>
    </form>

    <script language="javascript" type="text/javascript">
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
        var oArea = document.getElementById("<%=txtLinkName.ClientID%>");
        oArea.value = selectedText;
     }
}

function insertLink() {
    var linkContainer = document.getElementById("linkContainer");
	var returnValue = {url:'Test1',text:'Test2'};
	CloseDlg(returnValue);
}
    </script>

</body>
</html>
