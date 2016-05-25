<%@ Control Language="C#" Inherits="Telerik.WebControls.EditorDialogControls.ImageManager" AutoEventWireUp="false" CodeBehind="ImageManager.ascx.cs" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="telerik" NameSpace="Telerik.WebControls.EditorControls" Assembly="RadEditor.Net2" %>
<%@ Register TagPrefix="telerik" TagName="TabControl" Src="../Controls/TabControl.ascx" %>
<%@ Register TagPrefix="telerik" TagName="ImagePreviewer" Src="../Controls/ImagePreviewer.ascx" %>
<%@ Register TagPrefix="telerik" TagName="FileBrowser" Src="../Controls/FileBrowser.ascx" %>
<%@ Register TagPrefix="telerik" TagName="FileUploader" Src="../Controls/FileUploader.ascx" %>
<%@ Register TagPrefix="telerik" TagName="ThumbLinkOptionSetter" Src="../Controls/ThumbLinkOptionSetter.ascx"%>
<asp:literal ID="messageHolder" Runat="server"></asp:literal>
<asp:panel id="actionControlsHolder" Runat="server">
<table id="MainTable" width="550px" class="MainTable" border="0" cellpadding="0" cellspacing="0">
	<tr>
		<th align="left" valign="bottom">
			<telerik:tabcontrol id="TabHolder" runat="server" ResizeControlId="MainTable">
				<telerik:tab image="Dialogs/TabIcons/ImageTab1.gif" text="<script>localization.showText('Tab1HeaderText');</script>" selected="True" onclientclick="" elementid="ImageViewer"/>
				<telerik:tab image="Dialogs/TabIcons/ImageTab2.gif" text="<script>localization.showText('Tab2HeaderText');</script>" onclientclick="ConfigureUploadPanel();" elementid="ImageUploader" enabled="false"/>
			</telerik:tabcontrol>
		</th>
	</tr>
	<tr>
		<td class="MainTableContentCell" valign="top">
			<div class="ErrorMessage" id="divErrorMessage" runat="server" visible="false"></div>
			<div id="ImageViewer" style="OVERFLOW:hidden;HEIGHT:300px">
				<table cellspacing="0" cellpadding="0" border="0">
					<tr>
						<td colspan="3">
							<table cellpadding="0" cellspacing="0">
								<tr>
									<td nowrap class="Label">&nbsp;<script>localization.showText('ImageURL');</script></td>
									<td width="100%"><input type="text" style="WIDTH:100%" class="RadETextBox" id="FolderPathBox"></td>
								</tr>
							</table>
						</td>
						<td rowspan="2" width="95" valign="top">
							<button class="Button" onclick="return OkClicked()" type="button">
								<script>localization.showText('Insert');</script>
							</button><button class="Button" onclick="CloseDlg()" type="button">
								<script>localization.showText('Close');</script>
							</button>
							<telerik:ThumbLinkOptionSetter
								id="mainThumbLinkOptionSetter"
								runat="server"
								/>
						</td>
					</tr>
					<tr>
						<td valign="top">
							<telerik:filebrowser id="fileBrowser" runat="server"></telerik:filebrowser>
						</td>
						<td class="VerticalSeparator" nowrap></td>
						<td valign="top">
							<telerik:imagepreviewer id="previewer" runat="server"></telerik:imagepreviewer>
						</td>
					</tr>
				</table>
			</div>
			<div id="ImageUploader" style="OVERFLOW:hidden;">
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
		alert(localization["NoImageSelectedToInsert"]);
		return;
	}

	var image = {
		imagePath: "",
		linkImagePath: "",
		imageAltText: previewer.GetAltText()
	};

//	image.imagePath = document.getElementById("FolderPathBox").value;
	image.imagePath = fileBrowser.SelectedItem.GetUrl();
	var options = mainThumbLinkOptionSetter.GetOptions();
	if (options.LinkToImage)
	{
		image.linkImagePath = image.imagePath.replace(new RegExp(thumbAppendix + "(\\.\\w+)$", "ig"), "$1");
		image.targetToNew = options.TargetToNew;
	}

	retValue = image;
	CloseDlg(retValue);
}

fileBrowser.OnFolderChange = function(browserItem)
{
	/* Show the file path in the text box*/
	ShowPath(browserItem.GetPath());
	/*Previewer - Clear it*/
	previewer.Clear();
	/*Tab - see if the new folder allows uploads and enable/disable the upload tab */
	TabHolder.SetTabEnabled(1, browserItem.Permissions & fileBrowser.UploadPermission);
};

fileBrowser.OnClientClick = function(browserItem)
{
	var imagePath = browserItem.GetUrl();
	if (browserItem.Type == "F")
	{
		previewer.LoadObjectFromPath(imagePath);
		if (browserItem.Attributes && browserItem.Attributes[0])
		{
			previewer.SetAltText(browserItem.Attributes[0]);
		}
	}
	else previewer.LoadObjectFromPath(null);
	ShowPath(browserItem.GetPath());

	var isThumbnail = false;
	if (imagePath != "/")
	{
		fileExists = browserItem.IsThumbnail(thumbAppendix);
		if (fileExists)
		{
			isThumbnail = true;
		}
	}
	mainThumbLinkOptionSetter.SetVisibility(document.all && isThumbnail);
};

function OnLoad()
{
	if (hasThumbnailCreationErrorOccurred)
	{
		previewer.ShowThumbnailCreator();
	}
	TabHolder.SetTabEnabled(1, fileBrowser.CurrentItem.Permissions & fileBrowser.UploadPermission);
	TabHolder.SelectCurrentTab();
	var itemToShow = fileBrowser.SelectedItem != null?fileBrowser.SelectedItem:fileBrowser.CurrentItem;
	ShowPath(itemToShow.GetPath());
	previewer.LoadObjectFromPath(itemToShow.Type == "F"?itemToShow.GetUrl():null);
}

AttachEvent(window, "load", OnLoad);
</script>
</asp:panel>