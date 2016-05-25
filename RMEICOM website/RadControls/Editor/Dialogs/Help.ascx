<%@ Control Language="c#" AutoEventWireUp="false" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" CodeBehind="Help.ascx.cs" Inherits="Telerik.WebControls.EditorDialogControls.Help" %>
<%@ Register TagPrefix="telerik" TagName="TabControl" Src="../Controls/TabControl.ascx" %>
<%@ Register TagPrefix="telerik" NameSpace="Telerik.WebControls.EditorControls" Assembly="RadEditor.Net2" %>
<table id="MainTable" cellpadding="0" cellspacing="0" class="MainTable" width="570px;" height="400px;">
	<tr>
		<th height="39" valign="bottom" colspan="2">
			<telerik:tabcontrol id="TabHolder" runat="server" ResizeControlId="MainTable">
				<telerik:tab elementid="HelpDialogTable" selected="True" text="<script>localization.showText('Tab1HeaderText');</script>" />
			</telerik:tabcontrol>
		</th>
	</tr>
	<tr>
		<td>
			<table id="HelpDialogTable" border="0" cellpadding="0" cellspacing="2" width="100%;" height="100%;">
				<tr>
					<td class="label" valign="top">
						<div style="width:470px;height:470px;overflow:auto;">
							<asp:placeholder
								id="LocalizedHelp"
								runat="server"/><br/>
						</div>
					</td>
				</tr>
			</table>
		</td>
		<td valign="top" width="70px">
			<button id="btnClose" onclick="javascript:CloseDlg();" class="Button"><script>localization.showText('Close');</script></button>
		</td>
	</tr>
</table>