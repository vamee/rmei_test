<%@ Control Language="c#" Inherits="Telerik.WebControls.EditorDialogControls.SetCellProperties" AutoEventWireUp="false" CodeBehind="SetCellProperties.ascx.cs" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="telerik" TagName="CellPropertiesControl" Src="../Controls/CellPropertiesControl.ascx" %>
<%@ Register TagPrefix="telerik" TagName="TabControl" Src="../Controls/TabControl.ascx" %>
<%@ Register TagPrefix="telerik" NameSpace="Telerik.WebControls.EditorControls" Assembly="RadEditor.Net2" %>
<table id="mainTable" width="470px" cellpadding="0" cellspacing="0" class="MainTable" border="0">
	<tr>
		<th height="39" valign="bottom">
			<telerik:tabcontrol id="TabHolder" runat="server" resizecontrolid="mainTable">
				<telerik:tab image="Dialogs/TabIcons/CellPropertiesTab1.gif" text="<script>localization.showText('Tab1HeaderText');</script>" selected="True" elementid="mainTable"/>
			</telerik:tabcontrol>
		</th>
	</tr>
	<tr>
		<td class="MainTableContentCell">
			<table border="0" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td>
						<table width="100%" height="100%" cellpadding="6" cellspacing="0">
						<tr>
							<td valign="top">
								<telerik:CellPropertiesControl
									id="theCellPropertiesControl"
									runat="server"/>
							</td>
							<td width="90" valign="top">
								<table cellpadding="0" cellspacing="0">
									<tr>
										<td>
											<button class="Button" onclick="javascript:UpdateCell();">
												<script>localization.showText('Update');</script>
											</button>
										</td>
									</tr>
									<tr>
										<td height="4"></td>
									</tr>
									<tr>
										<td>
											<button class="Button" onclick="javascript:CloseDlg();">
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
		</td>
	</tr>
</table>
<script language="javascript">
	function InitControl()
	{	
		var args = GetDialogArguments();		
		<%=theCellPropertiesControl.ClientID%>.Initialize(args.cellToModify, args.CssClasses, args.EditorObj, args.ColorsArray, args.CanAddCustomColors);
	}
	
	function UpdateCell()
	{
		<%=theCellPropertiesControl.ClientID%>.Update();
		CloseDlg();
	}
	
	AttachEvent(window, "load", InitControl);
</script>