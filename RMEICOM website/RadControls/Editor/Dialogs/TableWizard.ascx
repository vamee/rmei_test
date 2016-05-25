<%@ Control Language="c#" AutoEventWireup="false" Codebehind="TableWizard.ascx.cs" Inherits="Telerik.WebControls.EditorDialogControls.TableWizard" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="telerik" NameSpace="Telerik.WebControls.EditorControls" Assembly="RadEditor.Net2"%>
<%@ Register TagPrefix="telerik" TagName="TabControl" Src="../Controls/TabControl.ascx"%>
<%@ Register TagPrefix="telerik" TagName="TablePropertiesControl" Src="../Controls/TablePropertiesControl.ascx"%>
<%@ Register TagPrefix="telerik" TagName="CellPropertiesControl" Src="../Controls/CellPropertiesControl.ascx"%>
<%@ Register TagPrefix="telerik" TagName="AccessibleTable" Src="../Controls/AccessibleTable.ascx"%>
<%@ Register TagPrefix="telerik" TagName="TableDesignControl" Src="../Controls/TableDesignControl.ascx"%>
<table cellpadding="0" cellspacing="0" class="MainTable" id="holderTable">
	<tr>
		<th height="39" valign="bottom">
			<telerik:tabcontrol id="TabHolder" runat="server" ResizeControlId="holderTable">
				<telerik:tab elementid="TabbedDesigner" onclientclick="SwitchTableDesigner();" selected="True" text="<script>localization.showText('Tab1HeaderText');</script>" image="Dialogs/TabIcons/TableTab1.gif"/>
				<telerik:tab elementid="TabbedTableProperties" onclientclick="SwitchTableProperties();" text="<script>localization.showText('Tab2HeaderText');</script>" image="Dialogs/TabIcons/TableTab2.gif"/>
				<telerik:tab elementid="TabbedCellProperties" onclientclick="SwitchCellProperties();" text="<script>localization.showText('Tab3HeaderText');</script>" image="Dialogs/TabIcons/TableTab3.gif"/>
				<telerik:tab elementid="Tabbed508" onclientclick="SwitchAccessibleTable();" text="<script>localization.showText('Tab4HeaderText');</script>" image="Dialogs/TabIcons/TableTab4.gif"/>
			</telerik:tabcontrol>
		</th>
	</tr>
	<tr>
		<td class="MainTableContentCell">
			<table width="100%" height="100%" cellpadding="6" cellspacing="0">
				<tr>
					<td valign="top">
						<table id="TabbedDesigner" width="346" cellpadding="0" cellspacing="0">
							<tr>
								<td>
									<telerik:TableDesignControl
										id="theTableDesignControl"
										runat="server"/>
								</td>
							</tr>
						</table>
						<table id="TabbedTableProperties" border="0" cellpadding="0" cellspacing="0">
							<tr>
								<td valign="top">
									<telerik:TablePropertiesControl
										id="theTablePropertiesControl"
										runat="server"/>
								</td>
							</tr>
						</table>
						<table id="TabbedCellProperties" border="0"  cellpadding="0" cellspacing="0">
							<tr>
								<td>
									<span id="<%=this.ClientID%>_CellPropertiesPreviewTableHolder"></span>
									<telerik:CellPropertiesControl
										id="theCellPropertiesControl"
										runat="server"/>
								</td>
							</tr>
						</table>
						<table id="Tabbed508" height="100%"> 
							<tr>
								<td>
									<telerik:accessibletable
										id="theAccessibleTable"
										runat="server"/>
								</td>
							</tr>
						</table>
					</td>
					<td width="90" valign="top" >
						<table cellpadding="0" cellspacing="0" style="DISPLAY:block">
							<tr>
								<td>
									<button class="Button" onclick="<%=this.ClientID%>.InsertTable();" type=button>
										<script>localization.showText('OK');</script>
									</button>
								</td>
							</tr>
							<tr>
								<td height="4"></td>
							</tr>
							<tr>
								<td>
									<button class="Button" onclick="javascript:CancelChanges();" type=button>
										<script>localization.showText('Cancel');</script>
									</button>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
<script language="javascript">
	function SwitchCellProperties()
	{
		<%=this.ClientID%>.InitCellProperties();
	}

	function SwitchTableProperties()
	{
		<%=this.ClientID%>.InitTableProperties();
	}
	
	function SwitchAccessibleTable()
	{
		<%=this.ClientID%>.InitAccessibleTable();
	}
	
	function SwitchTableDesigner()
	{
		if (!<%=this.ClientID%>)
		{
			InitControl();
		}
		<%=this.ClientID%>.InitDesigner();
	}
	
	function CancelChanges()
	{
		<%=this.ClientID%>.RestoreOriginalTable();
		CloseDlg();
	}

	var <%=this.ClientID%>;
	function InitControl()
	{
		if (!<%=this.ClientID%>)
		{
			<%=this.ClientID%> = new TableWizard('<%=this.ClientID%>', <%=this.theTableDesignControl.ClientID%>, <%=this.theTablePropertiesControl.ClientID%>, <%=this.theCellPropertiesControl.ClientID%>, <%=this.theAccessibleTable.ClientID%>);
			var args = GetDialogArguments();
			<%=this.ClientID%>.Initialize(args.tableToModify, args.CssClasses, args.CellCssClasses, args.EditorObj, args.ColorsArray, args.CanAddCustomColors);
		}
	}

	AttachEvent(window, "load", InitControl);
</script>