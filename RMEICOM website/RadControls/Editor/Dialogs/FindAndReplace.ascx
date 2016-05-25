<%@ Control Language="c#" Inherits="Telerik.WebControls.EditorDialogControls.FindAndReplace" AutoEventWireUp="false" CodeBehind="FindAndReplace.ascx.cs" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="telerik" TagName="FindAndReplaceControl" Src="../Controls/FindAndReplaceControl.ascx" %>
<%@ Register TagPrefix="telerik" TagName="TabControl" Src="../Controls/TabControl.ascx" %>
<%@ Register TagPrefix="telerik" NameSpace="Telerik.WebControls.EditorControls" Assembly="RadEditor.Net2" %>
<%@ OutputCache Duration="600" VaryByParam="Language;SkinPath" %>
<table id="mainTable" width="100%" cellpadding="0" cellspacing="0" class="MainTable">
	<tr>
		<th height="39" valign="bottom">
			<telerik:tabcontrol id="TabHolder" runat="server" resizecontrolid="mainTable">
				<telerik:tab image="Dialogs/TabIcons/FindAndReplaceTab1.gif" text="<script>localization.showText('Tab1HeaderText');</script>" selected="True" elementid="TabbedEmptySpan1" onclientclick="SwitchTab(false);" />
				<telerik:tab image="Dialogs/TabIcons/FindAndReplaceTab2.gif" text="<script>localization.showText('Tab2HeaderText');</script>" elementid="TabbedEmptySpan2" onclientclick="SwitchTab(true);" />
			</telerik:tabcontrol>
		</th>
	</tr>
	<tr>
		<td class="MainTableContentCell">
			<span id="TabbedEmptySpan1"></span><span id="TabbedEmptySpan2"></span>
			<telerik:FindAndReplaceControl
				id="theFindAndReplaceControl"
				runat="server"/>
		</td>
	</tr>
</table>
<script language="javascript">
	function SwitchTab(ShowReplace)
	{
		var theControl = <%=theFindAndReplaceControl.ClientID%>;
		theControl.SetFieldsVisibility(ShowReplace);
		theControl.SetFocusToSearchBox();
	}

	function CloseWindow()
	{
		//ERJO:RE5-2835
		/*<%=theFindAndReplaceControl.ClientID%>.SelectOriginalSelection();*/
		CloseDlg(null, null, false);
	}

	function CancelChanges()
	{
		//ERJO:RE5-2835
		window.radWindow.CallbackFunc = null;
		<%=theFindAndReplaceControl.ClientID%>.RestoreOriginalText();
		CloseDlg(null, null, false);
	}

	function InitControl()
	{
		var args = GetDialogArguments();
		var control = <%=theFindAndReplaceControl.ClientID%> = new FindAndReplaceControl('<%=theFindAndReplaceControl.ClientID%>', args.area);
		control.OnCancelChanges = CancelChanges;
		control.OnCloseWindow = CloseWindow;
	}

	AttachEvent(window, "load", InitControl);
</script>