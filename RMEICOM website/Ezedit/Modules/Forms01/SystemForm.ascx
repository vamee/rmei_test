<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SystemForm.ascx.vb" Inherits="Display_Forms01_SystemForm" %>
<asp:Label ID="lblBreadcrumbs" runat="server" />
<asp:Label ID="lblAlert" runat="server" CssClass="alert" />
<p>
    <VAM:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert" HeaderText="Please correct the following errors: "
        AutoUpdate="true" />
    <VAM:IgnoreConditionValidator ID="ConditionValidator" runat="server">
    </VAM:IgnoreConditionValidator>
</p>
<h3>
    <asp:Label ID="lblRegistration" runat="server" Text="You are registering for:<br />"
        Visible="false" /></h3>
<asp:Label ID="lblResourceName" runat="server" />
<asp:Repeater ID="rptFields" runat="Server" EnableViewState="False">
    <HeaderTemplate>
        <table width="100%" border="0" cellpadding="3" cellspacing="1">
            <tr runat="server" visible="false">
                <td class="table-header" colspan="4">
                    <asp:PlaceHolder ID="plcTableHeader" runat="server"></asp:PlaceHolder>
                </td>
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
                    <emagine:captchacontrol runat="server" id="Captcha" layoutstyle="Vertical"></emagine:captchacontrol>
                </td>
            </tr>
    </FooterTemplate>
</asp:Repeater>
<tr>
    <td align="center">
        <VAM:Button ID="btnSave" runat="server" Text="Test" CssClass="form-button" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="form-button" />
    </td>
</tr>
</table>
<asp:HiddenField ID="hdnFormID" runat="server" />
