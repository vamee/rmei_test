<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ImagePropertiesControl.ascx.cs" Inherits="Telerik.WebControls.EditorControls.ImagePropertiesControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="telerik" NameSpace="Telerik.WebControls.EditorControls" Assembly="RadEditor.Net2" %>
<%@ Register TagPrefix="telerik" TagName="AlignmentSelector" Src="./AlignmentSelector.ascx" %>
<%@ Register TagPrefix="telerik" TagName="ColorPicker" Src="./ColorPicker.ascx" %>
<%@ Register TagPrefix="telerik" TagName="ImageDialogCaller" Src="./ImageDialogCaller.ascx"%>
<%@ Register TagPrefix="telerik" TagName="SpinBox" Src="./SpinBox.ascx"%>
<table border="0" cellpadding="3" cellspacing="0">
	<tr>
		<td nowrap class="Label">
			<script>localization.showText('BorderWidth');</script>
		</td>
		<td colspan="2">
			<telerik:spinbox
				id="borderSizeSpinBox"
				runat="server"/>
		</td>					
	</tr>
	<tr>
		<td nowrap class="Label">
			<script>localization.showText('BorderColor');</script>
		</td>
		<td colspan="2">
			<telerik:colorpicker
				id="ImgBorderColorPicker"
				runat="server"/>
		</td>
	</tr>
	<tr>
		<td nowrap class="Label">
			<script>localization.showText('ImageAltText');</script>
		</td>
		<td colspan="2">
			<input type="text" id="<%=this.ClientID%>_alt" style="width:140px;margin-left:4px" class="Text">
		</td>
	</tr>
	<tr>
		<td nowrap class="Label">
			<script>localization.showText('ImageLongDesc');</script>:
		</td>
		<td colspan="2">
			<table cellpadding=0 cellspacing=0>
				<tr>
					<td><input type="text" id="<%=this.ClientID%>_longDescription" style="width:120px;margin-left:4px" class="Text"></td>
					<td width="20" style="padding-left:5px"><telerik:editorschemeimage relsrc="Dialogs/Accessibility.gif" id="Editorschemeimage1" runat="server"></telerik:editorschemeimage></td>
				</tr>
			</table>									
		</td>
	</tr>
	<tr>
		<td nowrap class="Label">
			<script>localization.showText('ImageAlign');</script>
		</td>
		<td colspan="2">
			<telerik:alignmentselector
				id="ImgAlignmentSelector" 
				runat="server" 
				mode="IMG"/>
		</td>
	</tr>
	<tr>
		<td nowrap class="Label">
			<script>localization.showText('ChangeImageSrc');</script>
		</td>
		<td colspan="2">
			<telerik:ImageDialogCaller
				id="changeSourceImageDialogCaller"
				runat="server"/>
		</td>
	</tr>
	<tr>
		<td nowrap class="Label">
			<script>localization.showText('HorizontalSpacing');</script>
		</td>
		<td colspan="2">
			<telerik:spinbox
				id="horizontalSpacingSpinBox"
				runat="server"/>
		</td>
	</tr>
	<tr>
		<td nowrap class="Label">
			<script>localization.showText('VerticalSpacing');</script>
		</td>
		<td colspan="2">
			<telerik:spinbox
				id="verticalSpacingSpinBox"
				runat="server"/>
		</td>
	</tr>
	<tr>
		<td nowrap class="Label">
			<script>localization.showText('Width');</script>
		</td>
		<td width="40">
			<input type="text" id="<%=this.ClientID%>_width" maxlength="4" style="width:40px;margin-left:4px" onkeydown="return <%=this.ClientID%>.ValidateNumber(event);" onkeyup="return <%=this.ClientID%>.ValidateDimension(event, true);" class="Text">
		</td>
		<td width="115" align="left" rowspan="3">
			<table width="70" height="58" cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td height="12"></td>
				</tr>
				<tr>
					<td height="8"><telerik:editorschemeimage relsrc="Dialogs/constrainTop.gif" id="constrainTop" runat="server"></telerik:editorschemeimage></td>
				</tr>
				<tr>
					<td height="18" onclick="<%=this.ClientID%>.ConstrainPropotions()" style="cursor:hand" nowrap>
						<telerik:editorschemeimage relsrc="Dialogs/constrainOff.gif" id="constrainImg" runat="server" align="absmiddle"></telerik:editorschemeimage>&nbsp;
						<span class="Label"><script>localization.showText('Constrain');</script></span>
					</td>
				</tr>
				<tr>
					<td height="8"><telerik:editorschemeimage relsrc="Dialogs/constrainBottom.gif" id="constrainBottom" runat="server"></telerik:editorschemeimage></td>
				</tr>
				<tr>
					<td height="12"></td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td nowrap class="Label">
			<script>localization.showText('Height');</script>
		</td>
		<td>
			<input type="text" id="<%=this.ClientID%>_height" maxlength="4" style="width:40px;margin-left:4px" onkeydown="return <%=this.ClientID%>.ValidateNumber(event);" onkeyup="return <%=this.ClientID%>.ValidateDimension(event, false);" class="Text">
		</td>
	</tr>
</table>
<script language="javascript">
	var <%=this.ClientID%> = new ImagePropertiesControl(
			'<%=this.ClientID%>',
			<%=ImgBorderColorPicker.ClientID%>,
			<%=ImgAlignmentSelector.ClientID%>,
			document.getElementById('<%=constrainImg.ClientID%>'),
			<%=borderSizeSpinBox.ClientID%>,
			<%=horizontalSpacingSpinBox.ClientID%>,
			<%=verticalSpacingSpinBox.ClientID%>,
			<%=changeSourceImageDialogCaller.ClientID%>);
</script>