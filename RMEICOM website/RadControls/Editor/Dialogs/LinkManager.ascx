<%@ Control Language="vb" Inherits="Telerik.WebControls.EditorDialogControls.LinkManager" AutoEventWireUp="false" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="telerik" TagName="LinkManagerControl" Src="../Controls/LinkManagerControl.ascx" %>
<%@ Register TagPrefix="telerik" TagName="TabControl" Src="../Controls/TabControl.ascx" %>
<%@ Register TagPrefix="telerik" NameSpace="Telerik.WebControls.EditorControls" Assembly="RadEditor.Net2" %>
<%@ OutputCache Duration="600" VaryByParam="Language;SkinPath" %>
<table id="MainTable" cellpadding="0" cellspacing="0" class="MainTable">
	<tr>
		<th height="39" valign="bottom">
			<telerik:tabcontrol id="TabHolder" runat="server" ResizeControlId="MainTable">
				<telerik:tab elementid="TabbedHyperlink" onclientclick="SetLinkVariant('link');" selected="True" text="<script>localization.showText('Tab1HeaderText');</script>" image="Dialogs/TabIcons/LinkTab1.gif" />
				<telerik:tab elementid="TabbedAnchor" onclientclick="SetLinkVariant('anchor')" selected="False" text="<script>localization.showText('Tab2HeaderText');</script>" image="Dialogs/TabIcons/LinkTab2.gif" />
				<telerik:tab elementid="TabbedEmail" onclientclick="SetLinkVariant('email');" selected="False" text="<script>localization.showText('Tab3HeaderText');</script>" image="Dialogs/TabIcons/LinkTab3.gif" />
			</telerik:tabcontrol>
		</th>
	</tr>
	<tr>
		<telerik:LinkManagerControl
			id="theLinkManagerControl"
			runat="server"/>
	</tr>
</table>
<script language="javascript">

	function SetLinkVariant(linkVariant)
	{
		<%=theLinkManagerControl.ClientID%>.SetLinkVariant(linkVariant);
	}

	function InitControl()
	{
		var args = GetDialogArguments();
		var control = <%=theLinkManagerControl.ClientID%>;
		control.Initialize(args);
		control.OnOkClicked = OnLinkOkClicked;
		control.OnCancelClicked = OnLinkCancelClicked;
		
		window.focus();//Mozilla under Mac!
	}
	
	function OnLinkOkClicked()
	{
		CloseDlg(<%=theLinkManagerControl.ClientID%>.GetModifiedLinkObject());
	}

	function OnLinkCancelClicked()
	{
		CloseDlg();
	}

	AttachEvent(window, "load", InitControl);
	
</script>