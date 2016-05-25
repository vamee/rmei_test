<%@ Control Language="c#" AutoEventWireup="false" Inherits="Telerik.WebControls.Dialogs.SpellCheckControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div id="expireMessage" runat="server" style="font:normal 8pt MS Sans Serif;color:red;display:none">
</div>
<table class="MainTable" id="mainTable" width="100%" cellpadding="0" cellspacing="0">
	<thead>
		<tr>
			<th valign="bottom" align="left">
				<table class="TabSelected" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<td class="TabLeftSelected" style="white-space: nowrap;">&nbsp;</td>
						<td class="TabCenterSelected" style="white-space: nowrap;"><script type="text/javascript">document.write(localization.Tab1HeaderText);</script>
						</td>
						<td class="TabRightSelected"></td>
					</tr>
				</table>
			</th>
		</tr>
	</thead>
	<tbody>
		<tr>
			<td class="MainTableContentCell">
				<table style="width: 100%; height: 100%; border: 0" cellpadding="5" cellspacing="0">
					<tr valign="top">
						<td style="width: 340px;">
							<table cellpadding="0" cellspacing="0">
								<tr>
									<td class="Label">
										<script type="text/javascript">document.write(localization.NotInDictionary);</script>
									</td>
								</tr>
								<tr>
									<td>
										<div id='textDisplay' class="TextDisplay" style="position: relative; overflow: auto;font: icon;border: 1px inset;width: 337px;height: 100px;margin: 0px"></div>
										<span id="editorHolder">
											<textarea rows="5" cols="39" id="contentTextArea" name="contentTextArea" style="border:1px inset;width:337px;height:100px;background-color:#FFFFFF;margin-top:2px;display:none"></textarea>
										</span>
										<script type="text/javascript">
										/*<![CDATA[*/
										document.write("<iframe id='actionContainer' name='actionContainer' src='<%= this.ActionUrl %>&" + dialog.queryStringParameters() +"' frameborder='0' marginheight='0' marginwidth='0' height='0' width='0'></iframe>");
										/*]]>*/
										</script>
									</td>
								</tr>
							</table>
						</td>
						<td rowspan="2" align="right">
							<table style="margin-top:12px">
								<tr>
									<td>
										<button id="ignoreButton" ondblclick="return false;" onclick="return dialog.buttonAction('this.ignoreWord()', event)"
											class="Button" disabled="disabled"><script type="text/javascript">document.write(localization.Ignore);</script></button>
									</td>
								</tr>
								<tr>
									<td>
										<button id="ignoreAllButton" ondblclick="return false;" onclick="return dialog.buttonAction('this.ignoreAll()', event)"
											class="Button" disabled="disabled"><script type="text/javascript">document.write(localization.IgnoreAll);</script></button>
									</td>
								</tr>
								<tr id="addCustomRow" runat="server">
									<td>
										<button id="addCustomButton" ondblclick="return false;" onclick="return dialog.buttonAction('this.addCustom()', event)"
											class="Button" disabled="disabled"><script type="text/javascript">document.write(localization.AddCustom);</script></button>
									</td>
								</tr>
								<tr>
									<td>
										<button id="changeButton" ondblclick="return false;" onclick="return dialog.buttonAction('this.changeWord()', event)"
											class="Button" disabled="disabled"><script type="text/javascript">document.write(localization.Change);</script></button>
									</td>
								</tr>
								<tr>
									<td>
										<button id="changeAllButton" ondblclick="return false;" onclick="return dialog.buttonAction('this.changeAll()', event)"
											class="Button" disabled="disabled"><script type="text/javascript">document.write(localization.ChangeAll);</script></button>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td>
							<table cellpadding="0" cellspacing="0">
								<tr>
									<td class="Label">
										<script type="text/javascript">document.write(localization.Suggestions);</script>
									</td>
								</tr>
								<tr>
									<td>
										<select class="plaintext" id="suggestions" ondblclick="dialog.buttonAction('this.changeWord()', event)"
											size="5" style="width:340px">
											<option>&nbsp;</option>
										</select>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr valign="top">
						<td align="right">
							<button id="undoButton" ondblclick="return false;" onclick="return dialog.buttonAction('this.undoLast()', event)"
								type="button" class="ButtonDisabled" disabled="disabled"><script type="text/javascript">document.write(localization.Undo);</script></button>&nbsp; <button id="cancelButton" ondblclick="return false;" onclick="return dialog.buttonAction('dialog.cancelCheck()', event)"
								class="Button"><script type="text/javascript">document.write(localization.Cancel);</script></button>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</tbody>
</table>
