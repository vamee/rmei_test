<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false" CodeFile="Domains.aspx.vb" Inherits="Ezedit_Admin_Languages_Domains" title="Untitled Page" %>
<%@ Register TagPrefix="vs" Namespace="Vladsm.Web.UI.WebControls" Assembly="GroupRadioButton" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <asp:Panel ID="pnlList" runat="server">
        <asp:GridView ID="gdvList" runat="server" AutoGenerateColumns="False" Width="100%"
            CellPadding="3" HeaderStyle-CssClass="table-header" AlternatingRowStyle-CssClass="table-altrow"
            RowStyle-CssClass="table-row" DataSourceID="dataDomains">
            <RowStyle CssClass="table-row" />
            <Columns>
                <asp:BoundField DataField="DomainName" HeaderText="Domain Name" HeaderStyle-HorizontalAlign="left"
                    ItemStyle-HorizontalAlign="left" >
                
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                
                <asp:TemplateField HeaderText="Enabled" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"
                    ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbxIsEnabled" runat="server" AutoPostBack="true" OnCheckedChanged="cbxIsEnabled_CheckedChanged" />
                        <%--<asp:CheckBox ID="cbxIsEnabled" runat="server" AutoPostBack="true" OnCheckedChanged="cbxIsEnabled_CheckChanged" />--%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100">
                    <ItemTemplate>
                        <table>
                            <tr>
                                <td width="20">
                                     <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/edit.png" OnClick="btnEdit_Click" ToolTip="Edit Language" />
                                   <%-- <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/edit.png"
                                        OnClick="btnEdit_Click" ToolTip="Edit Language" />--%>
                                </td>
                                <td width="20">
                                    <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/delete.png" 
                                        OnClientClick="return confirmDeleteItem();" OnClick="btnDelete_Click" ToolTip="Delete Language" />
                           

                                    <%--<asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/delete.png"
                                        OnClientClick="return confirmDeleteItem();" OnClick="btnDelete_Click" ToolTip="Delete Language" />--%>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="table-header" />
            <AlternatingRowStyle CssClass="table-altrow" />
        </asp:GridView>
        <asp:SqlDataSource ID="dataDomains" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
            SelectCommand="SELECT * FROM [Domains] WHERE ([LanguageID] = @LanguageID)">
            <SelectParameters>
                <asp:QueryStringParameter Name="LanguageID" QueryStringField="LanguageID" 
                    Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
    </asp:Panel>
    
    <asp:Panel ID="pnlEdit" runat="server">
    
    </asp:Panel>
</asp:Content>

