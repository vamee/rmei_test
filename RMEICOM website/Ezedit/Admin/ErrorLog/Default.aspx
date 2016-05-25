<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="Ezedit_Admin_ErrorLog_Default" Title="Error Log" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <asp:Label ID="lblPageTitle" runat="server" CssClass="pageTitle" Text="Error Log" /><br />
    <asp:Label ID="lblAlert" runat="server" CssClass="alert" />
    <asp:Panel ID="pnlList" runat="server">

        <script language="javascript" type="text/javascript">
            function confirmDeleteSelected() {
                if (confirm("Are you sure you want to permanently delete the selected events from the event log?")) {
                    return true;
                }else{ 
                    return false;
                }                
            }
            
            function confirmDeleteAll() {
                if (confirm("Are you sure you want to permanently delete all events from the event log?")) {
                    return true;
                }else{ 
                    return false;
                }                
            }
        </script>

        <table width="100%">
            <tr>
                <td>
                </td>
                <td align="right">
                    <asp:Button ID="btnDeleteSelected" runat="server" CssClass="form-button" Text="Delete Selected"
                        OnClientClick="return confirmDeleteSelected();" />
                    <asp:Button ID="btnDeleteAll" runat="server" Text="Clear Event Log" CssClass="form-button"
                        OnClientClick="return confirmDeleteAll();" />
                </td>
            </tr>
        </table>
        <asp:GridView ID="gdvList" runat="server" AutoGenerateColumns="False" Width="100%"
            CellPadding="3" HeaderStyle-CssClass="table-header" AlternatingRowStyle-CssClass="table-altrow"
            RowStyle-CssClass="table-row" AllowPaging="True" PageSize="30" AllowSorting="true"
            DataSourceID="dataErrorLog">
            <RowStyle CssClass="table-row"></RowStyle>
            <HeaderStyle CssClass="table-header" HorizontalAlign="Left"></HeaderStyle>
            <EmptyDataTemplate>
                There are no records to display.
            </EmptyDataTemplate>
            <AlternatingRowStyle CssClass="table-altrow"></AlternatingRowStyle>
            <Columns>
                <asp:TemplateField ItemStyle-Width="30" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:CheckBox ID="cbxSelectAll" runat="server" OnCheckedChanged="cbxSelectAll_CheckChanged"
                            AutoPostBack="true" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="cbxErrorID" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ErrorDate" HeaderText="Date" ItemStyle-Width="125" ItemStyle-VerticalAlign="Top"
                    SortExpression="ErrorDate" />
                <asp:BoundField DataField="Message" HeaderText="Error Message" ItemStyle-VerticalAlign="Top"
                    SortExpression="Message" />
                <asp:BoundField DataField="Source" HeaderText="Source" ItemStyle-VerticalAlign="Top"
                    SortExpression="Source" />
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnDetail" runat="server" Text="Detail" OnClick="btnDetail_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerSettings Visible="true" Mode="NumericFirstLast" />
        </asp:GridView>
        <asp:SqlDataSource ID="dataErrorLog" runat="server" SelectCommand="SELECT * FROM [ErrorLog] ORDER BY [ErrorDate] DESC"
            ConnectionString="<%$ ConnectionStrings:WebsiteDB %>" />
    </asp:Panel>
    <asp:Panel ID="pnlDetail" runat="server" Visible="false">
        <br />
        <table cellpadding="5" cellspacing="0" border="1">
            <tr>
                <td class="table-header" style="width: 100px;">
                    <asp:Label ID="lblErrorDate1" runat="server" Text="Error Date:" />
                </td>
                <td>
                    <asp:Label ID="lblErrorDate" runat="server" CssClass="main" />
                </td>
            </tr>
            <tr>
                <td class="table-header">
                    <asp:Label ID="Label1" runat="server" Text="Error Message:" />
                </td>
                <td>
                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="main" />
                </td>
            </tr>
            <tr>
                <td class="table-header">
                    <asp:Label ID="Label2" runat="server" Text="Source:" />
                </td>
                <td>
                    <asp:Label ID="lblSource" runat="server" CssClass="main" />
                </td>
            </tr>
            <tr>
                <td class="table-header">
                    <asp:Label ID="Label3" runat="server" Text="Stack Trace:" />
                </td>
                <td>
                    <asp:Label ID="lblStackTrace" runat="server" CssClass="main" />
                </td>
            </tr>
            <tr>
                <td class="table-header">
                    <asp:Label ID="Label4" runat="server" Text="Extra Data:" />
                </td>
                <td>
                    <asp:Label ID="lblData" runat="server" CssClass="main" />
                </td>
            </tr>
        </table>
        <br />
        <center>
            <asp:Button ID="btnBack" runat="server" CssClass="form-button" Text="<< Back" />
        </center>
    </asp:Panel>
</asp:Content>
