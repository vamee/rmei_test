<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="EditPageItems.aspx.vb" Inherits="Ezedit_Modules_LB01_EditPageItems" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <asp:Label ID="lblBreadcrumbs" runat="server" />
    <asp:Label ID="lblAlert" runat="server" CssClass="alert" />
    <hr />
    <asp:Panel ID="pnlList" runat="server">

        <script type="text/javascript" language="javascript">
        function confirmDeleteItem() {
            if (confirm("Are you sure you want to delete this library item from the page?")) {
                return true;
            }else{
                return false;
            }
        } 
        </script>

        <table cellpadding="2">
            <tr>
                <td>
                    <asp:LinkButton ID="lbtnAddNew" runat="server" CssClass="main"><img src="/App_Themes/EzEdit/Images/plugin_add.png" border="0" />&nbsp;Add Item to Page</asp:LinkButton>
                </td>
            </tr>
        </table>
        <asp:GridView ID="gdvList" runat="server" AutoGenerateColumns="False" Width="100%"
            CellPadding="3" HeaderStyle-CssClass="table-header" AlternatingRowStyle-CssClass="table-altrow"
            RowStyle-CssClass="table-row" DataSourceID="dataLibraryItems" AllowSorting="true"
            AllowPaging="true" PageSize="25" PagerSettings-Mode="NextPreviousFirstLast">
            <EmptyDataTemplate>
                No records found.
            </EmptyDataTemplate>
            <Columns>
                <asp:BoundField DataField="ResourceName" HeaderText="Name" SortExpression="ResourceName"
                    HeaderStyle-HorizontalAlign="Left" />
                <asp:TemplateField HeaderText="Sort" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40"
                    HeaderStyle-HorizontalAlign="Center" SortExpression="SortOrder">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlSortOrder" runat="server" OnSelectedIndexChanged="ddlSortOrder_SelectedIndexChanged"
                            AutoPostBack="true" />
                        <asp:HiddenField ID="hdnSortOrder" runat="server" Value='<%#Eval("SortOrder")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <table>
                            <tr>
                                <td width="20">
                                    <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="False" OnClick="btnEdit_Click"
                                        ImageUrl="~/App_Themes/EzEdit/Images/edit.png" CommandArgument='<%# Eval("PageItemID") %>'
                                        ToolTip="Edit" />
                                </td>
                                <td width="20">
                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" OnClick="btnDelete_Click"
                                        ImageUrl="~/App_Themes/EzEdit/Images/delete.png" OnClientClick="return confirmDeleteItem()"
                                        CommandArgument='<%# Eval("PageItemID") %>' ToolTip="Delete" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dataLibraryItems" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
            SelectCommand="SELECT * FROM qryPageLibraryItems WHERE PageID=@PageID ORDER BY SortOrder">
            <SelectParameters>
                <asp:QueryStringParameter Name="PageID" DefaultValue="0" QueryStringField="PageID" />
            </SelectParameters>
        </asp:SqlDataSource>
    </asp:Panel>
    
    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
        <VAM:ValidationSummary runat="server" CssClass="alert" HeaderText="" AutoUpdate="true" ScrollIntoView="Top" />
        <table cellpadding="3" cellspacing="0">
            <tr>
                <td>
                    <asp:DropDownList ID="ddlItemID" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="<- Please Choose ->" Value="-1" />
                    </asp:DropDownList>
                    <VAM:RequiredListValidator runat="server" ControlIDToEvaluate="ddlItemID" UnassignedIndex="0" SummaryErrorMessage="Please choose a library item from the list." />
                </td>
            </tr>
            <tr>
                <td>
                    <VAM:Button ID="btnSave" runat="server" CssClass="form-button" Text="Save" />
                    <asp:Button ID="btnCancel" runat="server" CssClass="form-button" Text="Cancel" />
                    <asp:HiddenField ID="hdnPageItemID" runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
