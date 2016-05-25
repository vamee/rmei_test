<%@ Control Language="c#" Inherits="Telerik.WebControls.EditorDialogControls.MediaManager" AutoEventWireUp="false" CodeBehind="MediaManager.ascx.cs" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="telerik" NameSpace="Telerik.WebControls.EditorControls" Assembly="RadEditor.Net2" %>
<%@ Register TagPrefix="telerik" TagName="TabControl" Src="../Controls/TabControl.ascx" %>
<%@ Register TagPrefix="telerik" TagName="MediaPreviewer" Src="../Controls/MediaPreviewer.ascx" %>
<%@ Register TagPrefix="telerik" TagName="FileBrowser" Src="../Controls/FileBrowser.ascx" %>
<%@ Register TagPrefix="telerik" TagName="FileUploader" Src="../Controls/FileUploader.ascx" %>
<asp:literal ID="messageHolder" Runat="server"></asp:literal>
<asp:panel id="actionControlsHolder" Runat="server">
<table width="550px" id="MainTable" class="MainTable" cellpadding="0" cellspacing="0">
	<tr>
		<th align="left" valign="bottom">
			<telerik:tabcontrol id="TabHolder" runat="server" ResizeControlId="MainTable">
				<telerik:tab image="Dialogs/TabIcons/MediaTab1.gif" text="<script>localization.showText('Tab1HeaderText');</script>" selected="True" elementid="MediaViewer"/>
				<telerik:tab image="Dialogs/TabIcons/MediaTab2.gif" text="<script>localization.showText('Tab2HeaderText');</script>" elementid="MediaUploader" onclientclick="ConfigureUploadPanel();" enabled="false"/>
			</telerik:tabcontrol>
		</th>
	</tr>
	<tr>
		<td class="MainTableContentCell">
			<div class="ErrorMessage" id="divErrorMessage" runat="server" visible="false"></div>
			<div id="MediaViewer" style="OVERFLOW:hidden;HEIGHT:300px">
				<table cellspacing="0" cellpadding="0" border="0">
					<tr>
						<td colspan="3" >
							<table cellpadding=0 cellspacing=0>
							<tr>
								<td nowrap class="Label">&nbsp;<script>localization.showText('Directory');</script></td>
								<td width="100%"><input type="text" style="WIDTH:100%" class="RadETextBox" id="FolderPathBox"></td>
							</tr>
							</table>
						</td>
						<td rowspan="2" width="40" valign="top">
							<button class="Button" onclick="return OkClicked()"
								type=button><script>localization.showText('Insert');</script></button>
								<button class="Button" onclick="CloseDlg()"
								type=button><script>localization.showText('Close');</script></button>
						</td>
					</tr>
					<tr>
						<td valign="top">
							<telerik:filebrowser
								id="fileBrowser"
								runat="server"></telerik:filebrowser>
						</td>
						<td class="VerticalSeparator" nowrap></td>
						<td valign="top">
							<telerik:mediapreviewer
							id="previewer"
							runat="server"></telerik:mediapreviewer>
						</td>
					</tr>
				</table>
			</div>
			<div id="MediaUploader" style="OVERFLOW:hidden;HEIGHT:300px">
				<telerik:FileUploader id="fileUploader" runat="server"/>
			</div>
		</td>
	</tr>
</table>
<asp:literal id="javascriptInitialize" Runat="server"></asp:literal>
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
	document.getElementById("FolderPathBox").value = path;
}

/* OK button clicked */
function OkClicked()
{
	if (fileBrowser.SelectedItem.Type == "D")
	{
		alert(localization["NoMediaSelectedToInsert"]);
		return;
	}

	if ((trim(document.getElementById("mediaWidth").value) == "") ||
		(trim(document.getElementById("mediaHeight").value) == ""))
	{
		alert(localization.AlertWidthHeight);
	}
	else
	{
		var mediaObject = previewer.GetHtml();
		CloseDlg(mediaObject);
	}
}

function SwitchPreviewMode()
{
	previewer.SwitchPreviewMode(fileBrowser.SelectedItem, FileFullName);
}

fileBrowser.OnFolderChange = function(browserItem)
{
	previewer.Preview(browserItem, FileFullName);
	ShowPath(browserItem.GetPath());
	TabHolder.SetTabEnabled(1, browserItem.Permissions & fileBrowser.UploadPermission);
};

fileBrowser.OnClientClick = function(browserItem)
{
	previewer.Preview(browserItem, FileFullName);
	ShowPath(browserItem.GetPath());
};

var FileFullName = null;

function OnLoad()
{
	dialogArgs = GetDialogArguments();
	FileFullName = dialogArgs.MediaPath;
	if (FileFullName)
	{
		ShowPath(fileBrowser.SelectedItem.GetPath());
		previewer.Preview(fileBrowser.SelectedItem, FileFullName);
	}

	var keys = GetProperties().keys;
	for (var i=0; i<keys.length; i++)
	{
		addOption(document.getElementById("property"), keys[i], keys[i]);
	}
	TabHolder.SetTabEnabled(1, fileBrowser.CurrentItem.Permissions & fileBrowser.UploadPermission);
	TabHolder.SelectCurrentTab();
	var itemToShow = fileBrowser.SelectedItem != null?fileBrowser.SelectedItem:fileBrowser.CurrentItem;
	ShowPath(itemToShow.GetPath());
}

AttachEvent(window, "load", OnLoad);
</script>
</asp:panel>