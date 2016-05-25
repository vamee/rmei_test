<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Search.ascx.vb" Inherits="modules_Custom01_Search" EnableViewState="False" %>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>">
</asp:SqlDataSource>
<asp:SqlDataSource ID="PR01Data" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>">
</asp:SqlDataSource>

<h2>Search Results</h2>
<asp:GridView ID="gdvSearchResults" runat="server" AllowPaging="True" DataSourceID="SqlDataSource1"
    PagerSettings-Visible="true" PageSize="10" AutoGenerateColumns="false" RowStyle-CssClass="table-row"
    AlternatingRowStyle-CssClass="table-altrow" CellPadding="5" BorderWidth="0" Width="100%"
    ShowHeader="false">
    <EmptyDataTemplate>
        No results match your query.
    </EmptyDataTemplate>
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:HyperLink ID="hypPageName" runat="server" CssClass="main-bold"></asp:HyperLink><br />
                <asp:Label ID="lblPageSummary" runat="server" CssClass="main"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <PagerSettings Mode="NextPrevious" Position="Top" FirstPageText="<< First"
        PreviousPageText="< Previous" NextPageText="Next >" LastPageText="Last >>" />
    <PagerStyle HorizontalAlign="Center" />
    <PagerTemplate>
        <table width="100%" style="border-bottom: 1px solid #CCCCCC;">
            <tr>
                <td>
                    <asp:Button ID="btnPrev" runat="server" CommandName="Page" CommandArgument="Prev"
                        Text="<< Previous" CssClass="form" />
                </td>
                <td>
                    <asp:Label ID="lblPageCount" runat="server" CssClass="main"></asp:Label>
                </td>
                <td align="right">
                    <asp:Button ID="btnNext" runat="server" CommandName="Page" CommandArgument="Next"
                        Text="Next >>" CssClass="form" />
                </td>
            </tr>
        </table>
    </PagerTemplate>
</asp:GridView>
<br />

<asp:GridView ID="gdvPR01" runat="server" AllowPaging="True" DataSourceID="PR01Data"
    PagerSettings-Visible="true" PageSize="10" AutoGenerateColumns="false" RowStyle-CssClass="table-row"
    AlternatingRowStyle-CssClass="table-altrow" CellPadding="5" BorderWidth="0" Width="100%"
    ShowHeader="false" visible="false">
    <EmptyDataTemplate>
        No results match your query.
    </EmptyDataTemplate>
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:HyperLink ID="hypPageName" runat="server" CssClass="main-bold"></asp:HyperLink><br />
                <asp:Label ID="lblPageSummary" runat="server" CssClass="main"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <PagerSettings Mode="NextPrevious" Position="Top" FirstPageText="<< First"
        PreviousPageText="< Previous" NextPageText="Next >" LastPageText="Last >>" />
    <PagerStyle HorizontalAlign="Center" />
    <PagerTemplate>
        <table width="100%">
            <tr>
                <td>
                    <asp:Button ID="btnPrev" runat="server" CommandName="Page" CommandArgument="Prev"
                        Text="<< Previous" CssClass="form" />
                </td>
                <td>
                    <asp:Label ID="lblPageCount" runat="server" CssClass="main"></asp:Label>
                </td>
                <td align="right">
                    <asp:Button ID="btnNext" runat="server" CommandName="Page" CommandArgument="Next"
                        Text="Next >>" CssClass="form" />
                </td>
            </tr>
        </table>
    </PagerTemplate>
</asp:GridView>
