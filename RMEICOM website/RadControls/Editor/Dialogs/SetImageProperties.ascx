<%@ Control Language="c#" Inherits="Telerik.WebControls.EditorDialogControls.SetImageProperties" AutoEventWireUp="false" CodeBehind="SetImageProperties.ascx.cs" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="telerik" NameSpace="Telerik.WebControls.EditorControls" Assembly="RadEditor.Net2" %>
<%@ Register TagPrefix="telerik" TagName="ImageInfoControl" Src="../Controls/ImageInfoControl.ascx" %>
<%@ Register TagPrefix="telerik" TagName="ImagePropertiesControl" Src="../Controls/ImagePropertiesControl.ascx" %>
<%@ Register TagPrefix="telerik" TagName="ThumbnailCreator" Src="../Controls/ThumbnailCreator.ascx" %>
<%@ Register TagPrefix="telerik" TagName="TabControl" Src="../Controls/TabControl.ascx" %>
<table id="MainTable" cellpadding="0" cellspacing="0" class="MainTable">
	<tr>
		<th height="39" valign="bottom" colspan=2>
			<telerik:tabcontrol id="TabHolder" runat="server" ResizeControlId="MainTable">
				<telerik:tab elementid="TabbedImageProperties" onclientclick="SetButtonsVisibility(true);" selected="True" text="<script>localization.showText('Tab1HeaderText')</script>" image="Dialogs/TabIcons/ImagePropertiesTab1.gif"/>				
				<telerik:tab elementid="TabbedThumbnailCreator" onclientclick="SetButtonsVisibility(false);" text="<script>localization.showText('Tab3HeaderText')</script>" image="Dialogs/TabIcons/ImagePropertiesTab3.gif"/>
			</telerik:tabcontrol>
		</th>
	</tr>
	<tr>
		<td class="MainTableContentCell" valign="top">
			<table id="TabbedImageProperties" cellpadding="0" cellspacing="0" border="0" width="100%">
				<tr>
					<td>
						<telerik:ImagePropertiesControl
							id="theImagePropertiesControl"
							runat="server"/>
					</td>
				</tr>
			</table>	
			<table id="TabbedThumbnailCreator" cellpadding="0" cellspacing="0" border="0" width="100%">
				<tr>
					<td>
						<telerik:ThumbnailCreator
							id="theThumbnailCreator"
							runat="server"/>
					</td>
				</tr>
			</table>
		</td>
		<td id="<%=this.ClientID%>_buttonsHolder" valign=top rowspan=8 width="60">
			<button class="Button" onclick="javascript:SubmitChanges();"><script>localization.showText('OK');</script></button>
			<button class="Button" onclick="javascript:CloseDlg();"><script>localization.showText('Cancel');</script></button>
		</td>
	</tr>
</table>
<script language="javascript">
	function InitControls()
	{
		var args = GetDialogArguments();
		var imgPropertiesControl = <%=theImagePropertiesControl.ClientID%>;
		imgPropertiesControl.Initialize(args.imageToModify, args.EditorObj, args.ColorsArray, args.CanAddCustomColors);
		var theThumbnailCreator;
		if (<%=theThumbnailCreator.Visible.ToString().ToLower()%>)
		{
			theThumbnailCreator = <%=theThumbnailCreator.ClientID%>;
			theThumbnailCreator.Initialize(imgPropertiesControl.GetOriginalImage(), args.ThumbnailSuffix);
			if (thumbnailCreatorErrorToken != "")
			{
				theThumbnailCreator.SetMessage(thumbnailCreatorErrorToken);
			}
		}
		else
		{
			theThumbnailCreator = null;
		}
	}

	function SubmitChanges()
	{
		CloseDlg(<%=theImagePropertiesControl.ClientID%>.GetUpdatedImage());
	}
	
	function SetButtonsVisibility(Visibility)
	{
	}
	AttachEvent(window, "load", InitControls);
</script>