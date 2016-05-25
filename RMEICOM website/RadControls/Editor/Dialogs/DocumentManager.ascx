<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Control Language="vb" Inherits="Telerik.WebControls.EditorDialogControls.DocumentManager" AutoEventWireUp="false" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="telerik" NameSpace="Telerik.WebControls.EditorControls" Assembly="RadEditor.Net2" %>
<%@ Register TagPrefix="telerik" TagName="TabControl" Src="../Controls/TabControl.ascx" %>
<%@ Register TagPrefix="telerik" TagName="DocumentPreviewer" Src="../Controls/DocumentPreviewer.ascx" %>
<%@ Register TagPrefix="telerik" TagName="FileBrowser" Src="../Controls/FileBrowser.ascx" %>
<%@ Register TagPrefix="telerik" TagName="FileUploader" Src="../Controls/FileUploader.ascx" %>


<script runat="server" language="vbscript">
    
    Dim AttachEvent As String = "AttachEvent(window, 'load', OnLoad);" & vbCrLf
    Dim HelpText As String = "One entry per line." & vbCrLf & vbCrLf & "Example:" & vbCrLf & "campaign_id=123456" & vbCrLf & "google_id=9876543" & vbCrLf
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        
    End Sub
    
    Sub BindFormPages(ByVal intStatusID, ByVal intLanguageID)
        Dim dtrForms As Data.SqlClient.SqlDataReader
        dtrForms = Pages01.GetFormPages(1, intStatusID, intLanguageID)
        ddlFormPageID.Items.Add(New ListItem("None Required", "0", True))

        If dtrForms.HasRows Then
            ddlFormPageID.AppendDataBoundItems = True
            ddlFormPageID.DataSource = dtrForms
            ddlFormPageID.DataTextField = "PageName"
            ddlFormPageID.DataValueField = "PageID"
            ddlFormPageID.DataBind()
        End If
    End Sub

    Protected Sub ddlFormPageID_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        If ddlFormPageID.Items.Count = 0 Then
            BindFormPages(CInt(Session("EzEditStatusID")), CInt(Session("EzEditLanguageID")))
        End If
    End Sub
    
    Protected Sub btnInsert_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim LinkName As String = ""
        If hdnLinkText.Value.Length > 0 Then LinkName = Replace(hdnLinkText.Value, vbCrLf, "")
        Dim SelectedFile As String = txtFolderPathBox.Text
        Dim ToolTip As String = txtToolTip.Text
        Dim Target As String = txtLinkTarget.Text
        Dim FormPageID As Integer = CInt(ddlFormPageID.SelectedValue)
        If LinkName.Length = 0 Then LinkName = Emagine.FormatFileName(SelectedFile)
                
        Dim Href As String = Me.CreateHref(SelectedFile, FormPageID, LinkName)
        
        'AttachEvent = "AttachEvent(window, 'load', insertLink('" & LinkName & "', '" & Href & "', '" & ToolTip & "', '" & Target & "'));" & vbCrLf
        AttachEvent = "insertLink('" & LinkName & "', '" & Href & "', '" & ToolTip & "', '" & Target & "');" & vbCrLf
    End Sub
    
    Function CreateHref(ByVal SelectedFileName As String, ByVal FormPageID As Integer, ByVal strLinkName As String) As String
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = ""
        Dim ResourceID As String = Emagine.GetUniqueID()
        Dim LinkID As Integer = 0
        Dim PageModuleID As Integer = 0
        Dim FormPagePropertyID As Integer
        Dim DeliveryPagePropertyID As Integer = 0
        Dim FormPageModuleID As Integer = 0
        Dim DeliveryPageID As Integer = 0
        'Dim LinkURL As String = Application("VirtualDocumentUploadPath") & Session("EzEditLanguageID") & "/" & SelectedFileName
        Dim ReturnURL As String = ""

        If FormPageID > 0 Then
            SQL = "sp_Links01_AddContentLink"
            Dim objCommand As New SqlCommand(SQL, Conn)
            objCommand.CommandType = CommandType.StoredProcedure
            objCommand.Parameters.AddWithValue("@ResourceID", ResourceID)
            objCommand.Parameters.AddWithValue("@LinkType", "FILE")
            objCommand.Parameters.AddWithValue("@LinkURL", SelectedFileName)

            Try
                Conn.Open()
                objCommand.ExecuteNonQuery()
                LinkID = Emagine.GetNumber(Emagine.GetDbValue("sp_Links01_GetMaxContentLinkID"))

            Catch ex As Exception
                Emagine.LogError(ex)
            Finally
                If Conn.State = ConnectionState.Open Then Conn.Dispose()
            End Try

            Dim Resource As New Resources.Resource
            Resource.ResourceId = ResourceID
            Resource.ReourcePageKey = ResourceID
            Resource.ResourceType = "Links01"
            Resource.ResourceName = strLinkName
            Resource.CampaignInfo = txtCampaignInfo.Text
            
            Resource.AddResource(Resource)
            'SQL = "INSERT INTO Resources (ResourceID,ResourcePageKey,ResourceType,CampaignInfo) VALUES ('" & ResourceID & "', '" & ResourceID & "', 'Links01')"
            'Emagine.ExecuteSQL(SQL)

            SQL = "INSERT INTO PageModules (ModuleKey,PageID,ForeignKey,ForeignValue) VALUES ("
            SQL = SQL & "'Links01', "
            SQL = SQL & "0, "
            SQL = SQL & "'LinkID', "
            SQL = SQL & LinkID & ")"
            Emagine.ExecuteSQL(SQL)
        
        
            SQL = "SELECT MAX(PageModuleID) AS MaxPageModuleID FROM PageModules"
            PageModuleID = Emagine.GetNumber(Emagine.GetDbValue(SQL))

            SQL = "SELECT PropertyID FROM ModuleProperties WHERE ModuleKey = 'Links01' AND PropertyName = 'FormPage'"
            FormPagePropertyID = Emagine.GetNumber(Emagine.GetDbValue(SQL))

            SQL = "SELECT PropertyID FROM ModuleProperties WHERE ModuleKey = 'Links01' AND PropertyName = 'DeliveryPage'"
            DeliveryPagePropertyID = Emagine.GetNumber(Emagine.GetDbValue(SQL))

            SQL = "INSERT INTO PageModuleProperties (PageModuleID,PropertyID,PropertyValue) VALUES ("
            SQL = SQL & PageModuleID & ", "
            SQL = SQL & FormPagePropertyID & ", "
            SQL = SQL & "'" & FormPageID & "')"
            Emagine.ExecuteSQL(SQL)

            SQL = "SELECT PageModuleID FROM qryFormPages WHERE PageID = " & FormPageID
            FormPageModuleID = Emagine.GetNumber(Emagine.GetDbValue(SQL))

            SQL = "SELECT PropertyValue FROM qryPageModuleProperties WHERE PageModuleID = " & FormPageModuleID & " AND PropertyName = 'DeliveryPage'"
            DeliveryPageID = Emagine.GetNumber(Emagine.GetDbValue(SQL))

            SQL = "INSERT INTO PageModuleProperties (PageModuleID,PropertyID,PropertyValue) VALUES ("
            SQL = SQL & PageModuleID & ", "
            SQL = SQL & DeliveryPagePropertyID & ", "
            SQL = SQL & "'" & DeliveryPageID & "')"
            Emagine.ExecuteSQL(SQL)
            
            ReturnURL = "/" & ResourceID & "/Link" & Emagine.GetFileExtension(SelectedFileName)
        Else
            ReturnURL = SelectedFileName
        End If
        
        

        Return ReturnURL
    End Function
</script>


<asp:literal ID="messageHolder" Runat="server"></asp:literal>
<asp:panel id="actionControlsHolder" Runat="server">
<table width="550px" id="MainTable" class="MainTable" cellpadding="0" cellspacing="0">
	<tr>
		<th align="left" valign="bottom">
			<telerik:tabcontrol id="TabHolder" runat="server" ResizeControlId="MainTable">
				<telerik:tab elementid="DocumentViewer" selected="True" text="<script>localization.showText('Tab1HeaderText');</script>" image="Dialogs/TabIcons/DocumentTab1.gif"/>
				<telerik:tab elementid="DocumentUploader" onclientclick="ConfigureUploadPanel()" text="<script>localization.showText('Tab2HeaderText');</script>" image="Dialogs/TabIcons/DocumentTab2.gif" enabled="false"/>
			</telerik:tabcontrol>
		</th>
	</tr>
	<tr>
		<td class="MainTableContentCell">
			<div class="ErrorMessage" id="divErrorMessage" runat="server" visible="false"></div>
			<div id="DocumentViewer" style="OVERFLOW:hidden;HEIGHT:300px">
				<table cellspacing="0" cellpadding="0" border="0">
					<tr>
						<td colspan="3" class="Label" nowrap>
							&nbsp;<script>localization.showText('DocumentFile');</script>
							<asp:TextBox Width="370" CssClass="RadETextBox" ID="txtFolderPathBox" runat="server"></asp:TextBox>							
							<!--<input type="text" style="width:370px" class="RadETextBox" id="FolderPathBox">-->
						</td>
						<td rowspan="2" width="40" valign="top">
						
						    <asp:Button ID="btnInsert" runat="server" Text="Insert" CssClass="button" enabled=true OnClick="btnInsert_Click" OnClientClick="return OkClicked();" />  
							<!--<button class="Button" onclick="return OkClicked()" type="button">
								<script>localization.showText('Insert');</script>
							</button>-->
							
							<button class="Button" onclick="CloseDlg()" type="button">
								<script>localization.showText('Close');</script>
							</button>
						</td>
					</tr>
					<tr>
						<td valign="top">
							<telerik:filebrowser id="fileBrowser" runat="server"></telerik:filebrowser>
						</td>
						<td class="VerticalSeparator" nowrap></td>
						<td valign="top">
							<telerik:documentpreviewer id="previewer" runat="server"></telerik:documentpreviewer>
							
							<table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td height="23px">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="PreviewAreaHolder">
                                                <table cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2" align="left" valign="top">
                                                            <span id="loader" style="display: none"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label">

                                                            <script>localization.showText('Tooltip');</script>

                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtToolTip" Width="120" CssClass="RadETextBox" runat="server"></asp:TextBox>
                                                            <!--<input type="text" id="tooltip" style="width:120px" class="RadETextBox">-->
                                                            &nbsp;
                                                            <telerik:editorschemeimage relsrc="Dialogs/Accessibility.gif" id="Editorschemeimage2"
                                                                runat="server" visible="false">
                                                            </telerik:editorschemeimage>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Label">

                                                            <script>localization.showText('Target');</script>

                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtLinkTarget" Width="120" CssClass="RadETextBox" runat="server" Text="_blank"></asp:TextBox>
                                                            
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label" nowrap>
                                                            Form Required:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlFormPageID" runat="server" CssClass="DropDown" OnLoad="ddlFormPageID_Load">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><br /></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label" colspan="2">
                                                            Campaign Info:<img src="../../../App_Themes/EzEdit/Images/help.png" alt="<%=HelpText %>" /> <br />
                                                            <asp:TextBox ID="txtCampaignInfo" runat="server" Width="225" Height="50" TextMode="MultiLine" CssClass="RadETextBox" />
                                                            
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                
                                
                                
						</td>
					</tr>
				</table>
			</div>
			<div id="DocumentUploader" style="OVERFLOW:hidden;HEIGHT:300px">
				<telerik:FileUploader id="fileUploader" runat="server"/>
			</div>
		</td>
	</tr>
</table>
<asp:literal id="javascriptInitialize" Runat="server"></asp:literal>
<asp:HiddenField ID="hdnLinkText" runat="server" />

<script language="javascript">
/*----------------Common functions------------------------*/
function ConfigureUploadPanel()
{
	if (messageHolderRowID)
	{
		if (isErrorVisible)
		{
			isErrorVisible = false;
		}
		else
		{
			var tr = document.getElementById(messageHolderRowID);
			if (tr && tr.style.display != "none") tr.style.display = "none";
		}
	}
	if (fileBrowser.CurrentItem)
	{
		document.getElementById('CurrentDirectoryBox').value = fileBrowser.CurrentItem.GetPath();
	}
}

function ShowPath(path)
{
	document.getElementById("<%=txtFolderPathBox.ClientID%>").value = path;
}

/* OK button clicked */
function OkClicked()
{
	if (fileBrowser.SelectedItem.Type == "D")
	{	    
		alert(localization["NoDocumentSelectedToInsert"]);
		return false;
	}
}

fileBrowser.OnFolderChange = function(browserItem)
{
	ShowPath(browserItem.GetPath());
	previewer.Clear();
	TabHolder.SetTabEnabled(1, browserItem.Permissions & fileBrowser.UploadPermission);
};

fileBrowser.OnClientClick = function(browserItem)
{

	if (browserItem.Type != "D")
	{
	    var insertButton = document.getElementById("<%=btnInsert.ClientID%>");
	    insertButton.disabled = false;
		previewer.LoadObjectFromPath(browserItem.GetUrl());
		if (browserItem.Attributes && browserItem.Attributes[0])
		{
			previewer.SetAltText(browserItem.Attributes[0]);
		}
	}else{
	    var insertButton = document.getElementById("<%=btnInsert.ClientID%>");
	    insertButton.disabled = true;
	    previewer.LoadObjectFromPath(null);
	}
	ShowPath(browserItem.GetPath());
};

function OnLoad()
{	
	dialogArgs = GetDialogArguments();

	var linkText = dialogArgs.text;
	
	linkText = linkText.replace("<P>", "");
	linkText = linkText.replace("</P>", "");
	linkText = linkText.replace("<BR>", "");
	
	linkText = encodeURI(linkText);
	
	
	var hdnLinkText = document.getElementById("<%=hdnLinkText.ClientID%>");
	hdnLinkText.value = linkText;  //dialogArgs.text;
	
	TabHolder.SetTabEnabled(1, fileBrowser.CurrentItem.Permissions & fileBrowser.UploadPermission);
	TabHolder.SelectCurrentTab();
	var itemToShow = fileBrowser.SelectedItem != null?fileBrowser.SelectedItem:fileBrowser.CurrentItem;
	ShowPath(itemToShow.GetPath());
}

function insertLink(text, href, title, target) {
    //dialogArgs = GetDialogArguments();
    text = URLDecode(text);
    
	var returnValue = {text:text, href:href, title:title, target:target};
	CloseDlg(returnValue);
}

function URLDecode(strValue)
{
   // Replace + with ' '
   // Replace %xx with equivalent character
   // Put [ERROR] in output if %xx is invalid.
   var HEXCHARS = "0123456789ABCDEFabcdef"; 
   var encoded = strValue
   var plaintext = "";
   var i = 0;
   while (i < encoded.length) {
       var ch = encoded.charAt(i);
	   if (ch == "+") {
	       plaintext += " ";
		   i++;
	   } else if (ch == "%") {
			if (i < (encoded.length-2) 
					&& HEXCHARS.indexOf(encoded.charAt(i+1)) != -1 
					&& HEXCHARS.indexOf(encoded.charAt(i+2)) != -1 ) {
				plaintext += unescape( encoded.substr(i,3) );
				i += 3;
			} else {
				alert( 'Bad escape combination near ...' + encoded.substr(i) );
				plaintext += "%[ERROR]";
				i++;
			}
		} else {
		   plaintext += ch;
		   i++;
		}
	} // while
   
   return plaintext;
}


<%=AttachEvent%>
</script>
</asp:panel>