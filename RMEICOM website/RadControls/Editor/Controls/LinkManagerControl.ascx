<%@ Register TagPrefix="telerik" NameSpace="Telerik.WebControls.EditorControls" Assembly="RadEditor.Net2" %>
<%@ Register TagPrefix="telerik" TagName="CssClassSelector" Src="./CssClassSelector.ascx" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="LinkManagerControl.ascx.cs" Inherits="Telerik.WebControls.EditorControls.LinkManagerControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<td class="MainTableContentCell">
	<table cellpadding="6" cellspacing="0">
		<tr>
			<td valign="top">
				<table id="TabbedHyperlink" cellpadding="1" cellspacing="0" border="0">
					<tr>
						<td nowrap>
							<label for="<%=this.ClientID%>_linkUrl">
								<script>localization.showText('LinkUrl')</script>
							</label>
						</td>
						<td>
							<input type="text" id="<%=this.ClientID%>_linkUrl" value="http://" style="WIDTH:180px" class="Text">
						</td>
					</tr>
					<tr>
						<td height="5" colspan="2"></td>
					</tr>
					<tr>
						<td nowrap>
							<label for="<%=this.ClientID%>_pageAnchorsHolder">
								<script>localization.showText('LinkToAnchor')</script>
							</label>
						</td>
						<td>
							<select id="<%=this.ClientID%>_pageAnchorsHolder" class="DropDown" onchange="<%=this.ClientID%>.SetLinkToAnchor(this);">								
								<script>
									document.write('<option selected value="">'
								+ localization.getText('AnchorNone') + '</option>');								
								</script>
							</select>
						</td>
					</tr>
					<tr>
						<td height="5" colspan="2"></td>
					</tr>
					<tr id="<%=this.ClientID%>_rowLinkText">
						<td nowrap>
							<label for="<%=this.ClientID%>_linkText">
								<script>localization.showText('LinkText')</script>
							</label>
						</td>
						<td>
							<input type="text" id="<%=this.ClientID%>_linkText" style="WIDTH:180px" class="Text">
						</td>
					</tr>
					<tr>
						<td height="5" colspan="2"></td>
					</tr>
					<tr>
						<td nowrap>
							<label for="<%=this.ClientID%>_linkType">
								<script>localization.showText('LinkType')</script>
							</label>
						</td>
						<td>
							<select class="Text" id="<%=this.ClientID%>_linkType" style="WIDTH:70px" onchange="<%=this.ClientID%>.ChangeLinkType(this.value);">
								<option value="">
									<script>localization.showText('Other')</script>
								</option>
								<option value="file://">file:</option>
								<option value="ftp://">ftp:</option>
								<option value="gopher://">gopher:</option>
								<option value="http://" selected>http:</option>
								<option value="https://">https:</option>
								<option value="javascript:">javascript:</option>
								<option value="news:">news:</option>
								<option value="telnet:">telnet:</option>
								<option value="wais:">wais:</option>
							</select>
						</td>
					</tr>
					<tr>
						<td height="5"></td>
					</tr>
					<tr>
						<td nowrap>
							<label for="<%=this.ClientID%>_linkTarget">
								<script>localization.showText('LinkTarget')</script>
							</label>
						</td>
						<td>
							<input type="text" id="<%=this.ClientID%>_linkTarget" style="WIDTH:90px" class="Text">&nbsp;
							<select id="<%=this.ClientID%>_linkTargetSelector" class="DropDown" onchange="<%=this.ClientID%>.ChangeLinkTarget(this);">
							<script>
							document.write(														
								'<option selected value="">' + localization.getText('Target') + '</option>' + 
								'<option value="_blank">' + localization.getText('_blank') + '</option>' + 
								'<option value="_parent">' + localization.getText('_parent') + '</option>' + 
								'<option value="_self">' + localization.getText('_self') + '</option>' + 
								'<option value="_top">' + localization.getText('_top') + '</option>' + 
								'<option value="_search">' + localization.getText('_search') + '</option>' + 
								'<option value="_media">' + localization.getText('_media') + '</option>'
							);
							</script>								
							</select>
						</td>
					</tr>
					<tr>
						<td height="5"></td>
					</tr>
					<tr>
						<td nowrap>
							<label for="<%=this.ClientID%>_titleText">
								<script>localization.showText('LinkTooltip')</script>
							</label>
						</td>
						<td>
							<table cellpadding="0" cellspacing="0">
								<tr>
									<td><input type="text" id="<%=this.ClientID%>_titleText" style="WIDTH:160px" class="Text"></td>
									<td width="20" style="PADDING-LEFT:5px"><telerik:editorschemeimage relsrc="Dialogs/Accessibility.gif" id="constrainTop" runat="server"></telerik:editorschemeimage></td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td nowrap>
							<label>
								<script>localization.showText('CssClass');</script>
							</label>
						</td>
						<td>
							<telerik:cssclassselector id="HyperlinkCssClassSelector" cssfilter="A,ALL" width="150px" popupwidth="200px"
								popupheight="160px" runat="server"></telerik:cssclassselector>
						</td>
					</tr>
				</table>
				<table id="TabbedAnchor" cellpadding="3" cellspacing="0">
					<tr>
						<td width="70">
							<label for="<%=this.ClientID%>_linkName">
								<script>localization.showText('LinkName')</script>
							</label>
						</td>
						<td>
							<input type="text" id="<%=this.ClientID%>_linkName" style="WIDTH:180px" class="Text">
						</td>
					</tr>
				</table>
				<table id="TabbedEmail" cellpadding="3" cellspacing="0">
					<tr>
						<td nowrap>
							<label for="<%=this.ClientID%>_address">
								<script>localization.showText('LinkAddress')</script>
							</label>
						</td>
						<td>
							<input type="text" id="<%=this.ClientID%>_address" style="WIDTH:180px" class="Text">
						</td>
					</tr>
					<tr id="<%=this.ClientID%>_rowEmailText">
						<td nowrap>
							<label for="<%=this.ClientID%>_emailText">
								<script>localization.showText('LinkText')</script>
							</label>
						</td>
						<td>
							<input type="text" id="<%=this.ClientID%>_emailText" style="WIDTH:180px" class="Text">
						</td>
					</tr>
					<tr>
						<td nowrap>
							<label for="<%=this.ClientID%>_subject">
								<script>localization.showText('LinkSubject')</script>
							</label>
						</td>
						<td>
							<input type="text" id="<%=this.ClientID%>_subject" style="WIDTH:180px" class="Text">
						</td>
					</tr>
					<tr>
						<td nowrap>
							<label>
								<script>localization.showText('CssClass');</script>
							</label>
						</td>
						<td>
							<telerik:cssclassselector id="EmailCssClassSelector" cssfilter="A,ALL" width="150px" popupwidth="200px" popupheight="160px"
								runat="server"></telerik:cssclassselector>
						</td>
					</tr>
				</table>
			</td>
			<td width="90" valign="top">
				<table cellpadding="0" cellspacing="0" style="DISPLAY:block">
					<tr>
						<td height="4"></td>
					</tr>
					<tr>
						<td>
							<button class="Button" id="submitButton" onclick="<%=this.ClientID%>.OkClicked();" type="button">
								<script>localization.showText('OK')</script>
							</button>
						</td>
					</tr>
					<tr>
						<td height="4"></td>
					</tr>
					<tr>
						<td>
							<button class="Button" onclick="<%=this.ClientID%>.CancelClicked();" type="button">
								<script>localization.showText('Cancel')</script>
							</button>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</td>
<script language="javascript">
	var <%=this.ClientID%> = new LinkManager('<%=this.ClientID%>', <%=this.HyperlinkCssClassSelector.ClientID%>, <%=this.EmailCssClassSelector.ClientID%>);
</script>
