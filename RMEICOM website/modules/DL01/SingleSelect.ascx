<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SingleSelect.ascx.vb"
    Inherits="modules_DL01_SingleSelect" %>
<asp:Label ID="lblAlert" CssClass="alert" runat="server" />
<asp:Panel ID="pnlDisplayPage" runat="server" Visible="false">
    <h2><asp:Label ID="lblCategoryName" runat="Server" /></h2>
    <asp:GridView ID="gdvDownloads" runat="server" RowStyle-CssClass="table-row" AlternatingRowStyle-CssClass="table-altrow"
        AutoGenerateColumns="false" HeaderStyle-CssClass="table-header" ShowHeader="false"
        Width="100%" CellPadding="3" CellSpacing="0" CssClass="tableBorder">
        <Columns>
            <asp:TemplateField Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblDate" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="btnFileName" runat="server" Font-Bold="true" OnClick="btnFileName_Click"
                        Visible="false" />
                    <asp:HyperLink ID="hypFileName" runat="server" Font-Bold="true" />
                    <asp:Label ID="lblFileSize" runat="server" /><br />
                    <asp:Label ID="lblDescription" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <br />
</asp:Panel>
<asp:Panel ID="pnlDeliveryPage" runat="server" Visible="false">
    <asp:GridView ID="gdvLinks" runat="server" RowStyle-CssClass="table-row" AlternatingRowStyle-CssClass="table-altrow"
        AutoGenerateColumns="false" HeaderStyle-CssClass="table-header" ShowHeader="true"
        Width="100%" CellPadding="3" CellSpacing="0" BorderWidth="1">
        <Columns>
            <asp:BoundField DataField="ResourceName" HeaderText="Name" />
            <asp:TemplateField HeaderText="Size" ItemStyle-Width="70" ItemStyle-HorizontalAlign="left"
                HeaderStyle-HorizontalAlign="left">
                <ItemTemplate>
                    <asp:Label ID="lblFileSize" runat="Server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="75">
                <ItemTemplate>
                    <asp:HyperLink ID="hypDownload" runat="Server" Text="Download" Target="_blank"></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Panel>
