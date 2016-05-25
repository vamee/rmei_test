<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DisplayEvents_2.ascx.vb" Inherits="modules_Events01_DisplayEvents_2" %>
<asp:GridView ID="gdvEvents" runat="server" AutoGenerateColumns="false" Width="100%"
            CellPadding="3" Border="0" ShowHeader="false"  AlternatingRowStyle-CssClass="table-altrow"
            RowStyle-CssClass="table-row">
    <Columns>
        <asp:TemplateField ItemStyle-Width="200">
            <ItemTemplate>
                <asp:Hyperlink ID="hypImageUrl" runat="server" Target="_blank"><asp:Image ID="imgImageUrl" runat="server" Width="200" /></asp:Hyperlink>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField ItemStyle-VerticalAlign="Top">
            <ItemTemplate>
                <h2><asp:Label ID="lblEventName" runat="server"></asp:Label></h2>
                <asp:Label ID="lblEventDescription" runat="server"></asp:Label><br /><br />
                <strong><asp:HyperLink ID="hypArchiveUrl" runat="server" Target="_blank"></asp:HyperLink></strong>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>        
            
</asp:GridView>