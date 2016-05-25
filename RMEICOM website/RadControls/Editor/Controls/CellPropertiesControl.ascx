<%@ Control Language="c#" AutoEventWireup="false" Codebehind="CellPropertiesControl.ascx.cs" Inherits="Telerik.WebControls.EditorControls.CellPropertiesControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="telerik" TagName="AlignmentSelector" Src="./AlignmentSelector.ascx" %>
<%@ Register TagPrefix="telerik" TagName="ColorPicker" Src="./ColorPicker.ascx" %>
<%@ Register TagPrefix="telerik" TagName="ImageDialogCaller" Src="./ImageDialogCaller.ascx" %>
<%@ Register TagPrefix="telerik" TagName="CssClassSelector" Src="./CssClassSelector.ascx" %>
<%@ Register TagPrefix="telerik" TagName="StyleBuilderCaller" Src="./StyleBuilderCaller.ascx"%>
<table width="350" cellpadding="0" cellspacing="0">
	<tr>
		<td class="Label">
			<script>localization.showText('ContentAlignment');</script>:&nbsp;
			<telerik:alignmentselector id="CellAlignmentSelector"
									runat="server"
									mode="TD"></telerik:alignmentselector>
		</td>
		<td class="Label">
			&nbsp;
		</td>
		<td class="Label">
			<script>localization.showText('Background');</script>:&nbsp;
			<telerik:colorpicker id="CellBackgroundColorPicker"
								runat="server"></telerik:colorpicker>
		</td>
	</tr>
	<tr>
		<td width="44%">
			<fieldset>
				<legend>
					<script>localization.showText('Dimensions');</script>
				</legend>
				<table width="100%" cellspacing="0">
					<tr>
						<td class="Label">
							<script>localization.showText('Height');</script>
						</td>
						<td>
							<input tabindex=1 class="Text" type="text" id="<%=this.ClientID%>_columnHeight" maxlength="6" style="width:50px" />
						</td>
					</tr>
					<tr>
						<td class="Label">
							<script>localization.showText('Width');</script>
						</td>
						<td>
							<input tabindex=2 class="Text" type="text" id="<%=this.ClientID%>_columnWidth" maxlength="6" style="width:50px" />
						</td>
					</tr>
				</table>
			</fieldset>
		</td>
		<td width="2%"></td>
		<td width="54%">
			<fieldset>
				<legend>
					<script>localization.showText('Additional');</script>
				</legend>
				<table width="100%" cellspacing="0">
					<tr>
						<td class="Label">
							<script>localization.showText('Id');</script>
						</td>
						<td>
							<input tabindex=3 class="Text" type="text" id="<%=this.ClientID%>_idHolder"/>
						</td>
					</tr>
					<tr>
						<td class="Label" colspan="2">
							<input tabindex=4  type="checkbox" id="<%=this.ClientID%>_columnWrap" />
							<script>localization.showText('NoWrapping');</script>
						</td>
					</tr>
				</table>
			</fieldset>
		</td>
	</tr>
	<tr>
		<td colspan="3">
			<fieldset>
				<legend>
					<script>localization.showText('BackgroundImage');</script>
				</legend>
				<telerik:imagedialogcaller id="bgImageDialogCaller" tabindex=5 runat="server" />
			</fieldset>
		</td>
	</tr>
	<tr>
		<td colspan="3">
			<fieldset>
				<legend>
					<script>localization.showText('CellCss');</script>
				</legend>
				<table width="100%">
					<tr>
						<td class="Label" nowrap>
							<script>localization.showText('CssClass');</script>
						</td>
						<td width="100%">
							<telerik:CssClassSelector id="CssClassSelector1"
								cssfilter="ALL, TD"
								width="150px"
								popupwidth="200px"
								popupheight="160px"
								runat="server">
							</telerik:CssClassSelector>
						</td>
						<td class="Label" style="display:none;">
							<telerik:StyleBuilderCaller
								id="theStyleBuilderCaller"
								runat="server"
							/>
						</td>
					</tr>
				</table>
			</fieldset>
		</td>
	</tr>
</table>
<script language="javascript">
	<%=this.ClientID%> = new CellPropertiesControl('<%=this.ClientID%>', <%=CssClassSelector1.ClientID%>, <%=CellAlignmentSelector.ClientID%>, <%=CellBackgroundColorPicker.ClientID%>, <%=bgImageDialogCaller.ClientID%>, <%=this.theStyleBuilderCaller.ClientID%>);
</script>