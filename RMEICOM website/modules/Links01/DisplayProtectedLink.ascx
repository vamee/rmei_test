<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DisplayProtectedLink.ascx.vb"
    Inherits="UserControls_ContentLinks_Protected" ClassName="ContentLinks" %>
<asp:Panel ID="pnlLinks" runat="server" Visible="false">
    <center>
        <asp:HyperLink ID="hypContinue" runat="server" Text="Continue" Target="_blank" />
        &nbsp;&nbsp;
        <asp:HyperLink ID="hypCancel" runat="server" Text="Cancel" Visible="false" />
    </center>
</asp:Panel>
