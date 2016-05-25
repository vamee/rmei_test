<%@ Control Language="c#" Inherits="Telerik.WebControls.EditorDialogControls.SetTableProperties" AutoEventWireUp="false" CodeBehind="SetTableProperties.ascx.cs" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="telerik" TagName="TablePropertiesControl" Src="../Controls/TablePropertiesControl.ascx" %>
<%@ Register TagPrefix="telerik" TagName="AccessibleTable" Src="../Controls/AccessibleTable.ascx" %>
<%@ Register TagPrefix="telerik" TagName="TabControl" Src="../Controls/TabControl.ascx" %>
<%@ Register TagPrefix="telerik" NameSpace="Telerik.WebControls.EditorControls" Assembly="RadEditor.Net2" %>
<%@ OutputCache Duration="600" VaryByParam="Language;SkinPath" %>
<table class="MainTable" cellspacing="0" cellpadding="0" id="MainTable" width="100%">
	<tr>
		<th valign="bottom" height="39" colspan="2">
			<telerik:tabcontrol id="TabHolder" runat="server" resizecontrolid="MainTable">
				<telerik:tab elementid="TabbedTableProperties" selected="True" text="<script>localization.showText('Tab1HeaderText');</script>" image="Dialogs/TabIcons/TablePropertiesTab1.gif"/>
				<telerik:tab elementid="Tabbed508" text="<script>localization.showText('Tab2HeaderText');</script>" image="Dialogs/TabIcons/TablePropertiesTab2.gif"/>
			</telerik:tabcontrol></th></tr>
	<tr>
	<tr>
		<td class="MainTableContentCell">
			<table id="Tabbed508" cellspacing="0" cellpadding="0">
				<tr>
					<td><telerik:accessibletable id="theAccessibleTable" runat="server"></telerik:accessibletable></td>
				</tr>
			</table>
			<table id="TabbedTableProperties" border="0" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td>
						<telerik:TablePropertiesControl
							id="theTablePropertiesControl"
							runat="server"/>
					</td>
				</tr>
			</table>
		</td>
		<td valign="top" width="90">
			<button class="Button" onclick="javascript:doUpdate();" type="button">
				<script>localization.showText('Update');</script>
			</button><button class="Button" onclick="javascript:CloseDlg();" type="button">
				<script>localization.showText('Cancel');</script>
			</button>
		</td>
	</tr>
</table>
<script language="javascript">
	function InitControl()
	{
		var arguments = GetDialogArguments();
		var accessibleTableControl = <%=this.theAccessibleTable.ClientID%>;
		accessibleTableControl.Initialize(arguments.tableToModify, arguments.tableDocument);
		var tablePropertiesControl = <%=theTablePropertiesControl.ClientID%>;
		tablePropertiesControl.Initialize(arguments.tableToModify, arguments.tableCssClasses, arguments.EditorObj, arguments.ColorsArray, arguments.CanAddCustomColors);

		tablePropertiesControl.OnCancel = CloseDlg;
	}
	
	function doUpdate()
	{
		<%=this.theAccessibleTable.ClientID%>.UpdateTable();
		<%=theTablePropertiesControl.ClientID%>.UpdateTable();
		CloseDlg();
	}

	AttachEvent(window, "load", InitControl);
</script>