<%@ Control Language="c#" Inherits="Telerik.WebControls.EditorDialogControls.AboutDialog" AutoEventWireUp="false" CodeBehind="AboutDialog.ascx.cs" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="telerik" TagName="TabControl" Src="../Controls/TabControl.ascx" %>
<%@ Register TagPrefix="telerik" NameSpace="Telerik.WebControls.EditorControls" Assembly="RadEditor.Net2" %>
<%@ OutputCache Duration="600" VaryByParam="Language;SkinPath" %>
<table height="160px;" id="MainTable" cellpadding="2" cellspacing="0" class="MainTable">
	<tr>
		<th height="39" valign="bottom" colspan="2">
			<telerik:tabcontrol id="TabHolder" runat="server" ResizeControlId="MainTable">
				<telerik:tab elementid="AboutDialogTable" selected="True" text="<script>localization.showText('Tab1HeaderText');</script>" image="Dialogs/TabIcons/AboutDialogTab1.gif" />
			</telerik:tabcontrol>
		</th>
	</tr>
	<tr>
		<td>
			<table id="AboutDialogTable" border="0" cellpadding="0" cellspacing="0" width="200px;" height="100px;">
				<tr>
					<td >
						<img src="<%=this.SkinPath%>Img/productBox.gif"">
					</td>
					<td class="label" valign="top">
						<p align="center"><b><br>
						t</b>elerik r.a.d.<b>editor</b> 
						v<%=Telerik.RadEditorUtils.RadControl.Version.ToString("$m.$n$b")%>
					<br>
						<br>
						
						Copyright &copy; 2002-2006 telerik<br>
						All rights reserved.
						<br><br>
						<a target=_blank href="http://www.telerik.com">www.telerik.com</a>
						</td>
				</tr>
			</table>
		</td>
		<td valign="top">
			<button id="btnClose" onclick="javascript:CloseDlg();" class="Button"><script>localization.showText('Close');</script></button>
		</td>
	</tr>
</table>