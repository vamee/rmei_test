<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DisplayEvents.ascx.vb" Inherits="modules_Events01_DisplayEvents" %>
<asp:Panel ID="pnlDisplay" runat="server" Visible="false">
    <h2><asp:Label ID="lblCategoryName" runat="Server" /></h2>
    <asp:GridView ID="gdvList" runat="server" RowStyle-CssClass="table-row" AlternatingRowStyle-CssClass="table-altrow" ShowHeader="false"
        AutoGenerateColumns="false" HeaderStyle-CssClass="table-header" Width="100%" CellPadding="3" CellSpacing="0" BorderWidth="1">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink ID="hypResourceName" runat="server" Text='<%#Eval("ResourceName")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            
        </Columns>
    </asp:GridView>
    <br />
</asp:Panel>

<asp:Panel ID="pnlDetail" runat="server" Visible="false">
    <asp:Literal ID="ltrEventDescription" runat="server" />
    <asp:Button ID="btnRegister" runat="server" CssClass="form-button" Text="Click Here to Register" Visible="false" />
</asp:Panel>

<asp:Panel ID="pnlRegistration" runat="server" Visible="false">

</asp:Panel>

<asp:Panel ID="pnlThankYou" runat="server" Visible="false">

</asp:Panel>