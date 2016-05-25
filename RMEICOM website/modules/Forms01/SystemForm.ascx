<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SystemForm.ascx.vb" Inherits="Display_Forms01_SystemForm" %>
<asp:label id="lblBreadcrumbs" runat="server"></asp:label>
	<asp:label id="lblAlert" runat="server" cssclass="message"></asp:label>
<p>
    <vam:validationsummary id="ValidationSummary1" runat="server" cssclass="message" headertext="Please correct the following errors: " AutoUpdate="true"></vam:validationsummary>
    <VAM:IgnoreConditionValidator ID="ConditionValidator" runat="server"></VAM:IgnoreConditionValidator>
</p>

<asp:Panel ID="pnlResourceInfo" runat="server" Visible="false">
<h3><asp:Label ID="lblRegistration" runat="server" Text="You are registering for:<br />" /></h3>

<asp:Label ID="lblResourceTitle" runat="server" /><br />
<asp:Label ID="lblResourceDescription" runat="server" />
</asp:Panel>
	    
    <asp:Repeater
        Id="rptFields"
        Runat="Server"
        EnableViewState="False">
        <HeaderTemplate>
            <br />
			            <table width="100%" border="0" cellpadding="3" cellspacing="1">
				            <tr>
					            <td colspan="4"><asp:PlaceHolder ID="plcTableHeader" runat="server"></asp:PlaceHolder></td>
				            </tr> 
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr id="trForms" runat="server">
                                    <td id="tdFieldLabel" valign="top" runat="server">
                                        <asp:Label ID="lblFieldLabel" runat="server"></asp:Label>
                                    </td>
                                   <td class="table_text" valign="top" id="tdFormField" runat="server">
                                        <asp:PlaceHolder ID="plcFormField" runat="server"></asp:PlaceHolder>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                          </table>
                          <table border="0" width="100%" cellpadding="3" cellspacing="1">
                            <tr id="trCaptcha" runat="server" visible="false">
                                <td align="left">
                                    <emagine:captchacontrol runat="server" id="Captcha" LayoutStyle="Vertical"></emagine:captchacontrol>
                                </td>
                            </tr>
                             <tr>
                              <td align="center" style="padding-top:10px;">
                               <%--<VAM:ImageButton ID="btnSave" runat="server" ImageUrl="/Collateral/Templates/English-US/images/submit_btn.png" />--%>
                               <VAM:Button ID="btnSave" runat="server" CssClass="form-button" Text="Submit" />
                              </td>
                             </tr>
                            </table>
							<br />
	        </FooterTemplate>
    </asp:Repeater>
    <asp:HiddenField ID="hdnReferrer" runat="server" />