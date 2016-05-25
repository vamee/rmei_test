<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InternalLink.aspx.vb" Inherits="RadControls_Editor_Custom_InternalLink" ValidateRequest="false" %>

<%@ Register Assembly="RadAjax.Net2, Culture=neutral, PublicKeyToken=3f7b438d1c762d0b"
    Namespace="Telerik.WebControls" TagPrefix="radA" %>
<%@ Register Assembly="RadTreeView.Net2, Culture=neutral, PublicKeyToken=dbd98e0feff9f06b"
    Namespace="Telerik.WebControls" TagPrefix="radT" %>

<%--<%@ Register TagPrefix="radT" Namespace="Telerik.WebControls" %>--%>
<html>
<head>
<link href="/App_Themes/Ezedit/EzEdit.css" rel="stylesheet" type="text/css" />
</head>
<body bgcolor="#ECE9D8" onload="ConfigureDialog();">
<script type="text/javascript" src="/RadControls/Editor/Scripts/7_0_2/RadWindow.js"></script>
<script type="text/javascript">
	InitializeRadWindow();
</script>
<form id="form1" runat="server">
    <radA:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <radA:AjaxSetting AjaxControlID="tvwSiteMap">
                <UpdatedControls>
                    <radA:AjaxUpdatedControl ControlID="tvwPageModules" />
                    <radA:AjaxUpdatedControl ControlID="lblPageInfo" />
                    <radA:AjaxUpdatedControl ControlID="btnInsert" />
                </UpdatedControls>
            </radA:AjaxSetting>
            <radA:AjaxSetting AjaxControlID="tvwPageModules">
                <UpdatedControls>
                    <radA:AjaxUpdatedControl ControlID="lblPageInfo" />
                    <radA:AjaxUpdatedControl ControlID="btnInsert" />
                </UpdatedControls>
            </radA:AjaxSetting>
        </AjaxSettings>
    </radA:RadAjaxManager>
<table>
  <tr>
    <td valign="top">
        <asp:Panel ID="pnlSiteMap" BackColor="White" BorderStyle="Solid" BorderColor="Black" BorderWidth="1" ScrollBars="Auto" Width="280" Height="240" runat="server">
            <radT:RadTreeView ID="tvwSiteMap" runat="server" AutoPostBack="true"></radT:RadTreeView>
        </asp:Panel>
    </td>
    <td width="20"></td>
    <td valign="top">
        <asp:Panel ID="pnlPageInfo" BackColor="White" BorderStyle="Solid" BorderColor="Black" BorderWidth="1" ScrollBars="Auto" Width="280" Height="240" runat="server">
            <radT:RadTreeView ID="tvwPageModules" runat="server" AutoPostBack="True">
                
            </radT:RadTreeView>
        </asp:Panel>
    </td>
  </tr>
  <tr>
    <td colspan="2"><asp:Label ID="lblPageInfo" runat="server"></asp:Label></td>
    <td>
        <table border="0" cellspacing="0" cellpadding="1" width="100%">
          <tr>
            <td width="50%" valign="bottom"><br></td>
            <td align="right" width="50%">
	        <table border="0" cellpadding="0" cellspacing="0" id="table2">
		        <tr>
			        <td align="left"><span class="form_label">Link name:</span><br>
			        <asp:TextBox ID="txtLinkName" CssClass="form" Width="200" runat="server"></asp:TextBox>
			        </td>
		        </tr>
		        <tr>
			        <td align="left"><span class="form_label">Form Required:</span><br>
                        <asp:DropDownList ID="ddlFormPages" runat="server" Width="200" CssClass="form">
                        </asp:DropDownList>
                    </td>
		        </tr>
	        </table>
	        </td>
          </tr>
            
          <tr>
            <td width="50%">
	        </td>
            <td align="right" width="50%"><asp:Button ID="btnInsert" Text="Insert" runat="server" CssClass="button" enabled="false"/> <asp:Button ID="lblCancel" CssClass="button" OnClientClick="CloseDlg(null);" Text="Cancel" runat="server" /></td>
          </tr>
        </table>
    </td>
  </tr>
</table>

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
<asp:Literal ID="litAlert" runat="server"></asp:Literal>
</form>
</body>
</html>
