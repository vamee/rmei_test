<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="Ezedit_Modules_Pages02_Default" Title="EzEdit - Site Content - Pages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">

    <script language="javascript" type="text/javascript">
        function confirmDeletePage() {
            if (confirm("Are you sure you want to delete this page?")) {
                return true;
            }else{
                return false;
            }
        }
        
        function confirmDeleteSection() {
            if (confirm("Are you sure you want to delete this page?\r\rWARNING: All child pages will be deleted as well.")) {
                return true;
            }else{
                return false;
            }
        }
    </script>

    <asp:Label ID="lblBreadcrumbs" runat="server" />
    <asp:Label ID="lblAlert" runat="server" CssClass="alert" />
    <table border="0" cellpadding="3" cellspacing="0" width="100%">
        <tr>
            <td>
                <asp:LinkButton ID="lbtnAddNew" runat="server" class="main"><img src="/App_Themes/EzEdit/Images/page_add.png" alt="Add New Page" border="0" /> Add New Page</asp:LinkButton>
            </td>
        </tr>
    </table>
    <asp:GridView ID="gdvList" runat="server" AutoGenerateColumns="False" Width="100%"
        CellPadding="3" HeaderStyle-CssClass="table-header" AlternatingRowStyle-CssClass="table-altrow"
        RowStyle-CssClass="table-row" DataSourceID="dataPages" AllowSorting="true">
        <RowStyle CssClass="table-row"></RowStyle>
        <Columns>
            <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="left"
                ItemStyle-HorizontalAlign="left" SortExpression="PageName">
                <ItemTemplate>
                    <asp:Label ID="lblPageName" runat="server" /> <asp:Label ID="lblIsHidden" runat="server" Text="(hidden)" Font-Italic="true" Visible="false" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="PageName" HeaderText="Name" HeaderStyle-HorizontalAlign="left"
                ItemStyle-HorizontalAlign="left" SortExpression="PageName" />
            <asp:BoundField DataField="PageKey" HeaderText="Key" HeaderStyle-HorizontalAlign="left" SortExpression="PageKey" />
            <asp:TemplateField HeaderText="Sort" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50" SortExpression="PageSortOrder">
                <ItemTemplate>
                    <asp:DropDownList ID="ddlSortOrder" runat="server" OnSelectedIndexChanged="ddlSortOrder_IndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:HiddenField ID="hdnSortOrder" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="130" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <table border="0" cellspacing="2">
                        <tr>
                            <td width="16">
                                <asp:ImageButton ID="btnExpand" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/sitemap.png"
                                    OnClick="btnExpand_Click" ToolTip="View Children" />
                            </td>
                            <td width="16">
                                <asp:ImageButton ID="btnEditPage" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/page_white_gear.png"
                                    OnClick="btnEditPage_Click" ToolTip="Edit Page Properties" />
                            </td>
                            <td width="16">
                                <asp:ImageButton ID="btnEditContent" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/page_edit.png"
                                    OnClick="btnEditContent_Click" ToolTip="Edit Content" />
                            </td>
                            <td width="16">
                                <asp:ImageButton ID="btnEditModules" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/application_side_boxes.png"
                                    OnClick="btnEditModules_Click" ToolTip="Edit Page Modules" Visible="false" />
                            </td>
                            <td width="16">
                                <asp:ImageButton ID="btnEditLibraryItems" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/plugin.png"
                                    OnClick="btnEditLibraryItems_Click" ToolTip="Edit Page Library Items" />
                            </td>
                            <td width="16">
                                <asp:ImageButton ID="btnDeletePage" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/delete.png"
                                    OnClientClick="return confirmDeletePage();" OnClick="btnDeletePage_Click" ToolTip="Delete Page" />
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hdnPageID" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle CssClass="table-header"></HeaderStyle>
        <AlternatingRowStyle CssClass="table-altrow"></AlternatingRowStyle>
    </asp:GridView>
    <asp:SqlDataSource ID="dataPages" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
        SelectCommand="SELECT * FROM [qryPages] WHERE (StatusID = 20) AND ([ParentPageID] = @ParentPageID) AND ([LanguageID] = @LanguageID) ORDER BY [PageSortOrder]">
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="0" Name="ParentPageID" QueryStringField="ParentPageID"
                Type="Int32" />
            <asp:SessionParameter DefaultValue="1" Name="LanguageID" SessionField="EzEditLanguageID"
                Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
