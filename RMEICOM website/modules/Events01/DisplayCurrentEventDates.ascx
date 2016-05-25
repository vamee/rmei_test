<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DisplayCurrentEventDates.ascx.vb" Inherits="modules_Events01_DisplayCurrentEventDates" %>
<asp:GridView ID="gdvList" runat="server" RowStyle-CssClass="table-row" AlternatingRowStyle-CssClass="table-altrow"
    ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table-header"
    Width="100%" CellPadding="3" CellSpacing="0" BorderWidth="1">
    <Columns>
        <asp:BoundField DataField="ResourceName" HeaderText="Course Name" />
        <asp:BoundField DataField="EventDate" HeaderText="Date" />
        <asp:BoundField DataField="Location" HeaderText="Location" />
        <asp:TemplateField ItemStyle-HorizontalAlign="Right">
            <ItemTemplate>
                <table>
                    <tr>
                        <td width="100" align="center">
                            <asp:HyperLink ID="hypDetail" runat="server" Text="Detail" />        
                        </td>
                        <td width="100" align="center">
                            <asp:HyperLink ID="hypRegister" runat="server" Text="Register" />
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>