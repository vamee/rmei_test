<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="Ezedit_Modules_LB01_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <p class="pagetitle">
        Library Items</p>
    <asp:Label ID="lblAlert" runat="server" CssClass="alert" />
    <VAM:ValidationSummary runat="server" CssClass="alert" HeaderText="The following fields are required:"
        AutoUpdate="true" ScrollIntoView="Top" />
    <hr />
    <asp:Panel ID="pnlList" runat="server">
        <script type="text/javascript" language="javascript">
        function confirmDeleteItem()
        {
            if (confirm("Are you sure you want to delete this Library Item?")) {
                return true;
            }else{
                return false;
            }
        } 
        </script>
        <table cellpadding="3" cellspacing="0" border="0">
            <tr>
                <td>
                    <asp:LinkButton ID="lbtnAddNew" runat="server" ToolTip="Add New Library Item" CssClass="main"><img src="/App_Themes/EzEdit/Images/plugin_add.png" alt="" border="0" />&nbsp;Add New Library Item</asp:LinkButton>
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="gdvList" runat="server" AutoGenerateColumns="False" Width="100%"
            CellPadding="3" HeaderStyle-CssClass="table-header" AlternatingRowStyle-CssClass="table-altrow"
            RowStyle-CssClass="table-row" DataSourceID="dataLibraryItems" AllowSorting="true" AllowPaging="true" PageSize="25" PagerSettings-Mode="NextPreviousFirstLast">
            <Columns>
                <asp:BoundField DataField="ResourceName" HeaderText="Name" HeaderStyle-HorizontalAlign="Left" SortExpression="ResourceName" />
                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60">
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" Width="55" /><br />
                        <asp:Label ID="lblDisplayDates" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Enabled" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40" SortExpression="IsEnabled">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbxIsEnabled" runat="server" Checked='<%# Eval("IsEnabled") %>'
                            OnCheckedChanged="cbxIsEnabled_CheckChanged" AutoPostBack="true" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <table>
                            <tr>
                                <td width="20">
                                    <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="False" OnClick="btnEdit_Click"
                                        ImageUrl="~/App_Themes/EzEdit/Images/plugin_edit.png" CommandArgument='<%# Eval("ItemID") %>'
                                        ToolTip="Edit" />
                                </td>
                                <td width="20">
                                    <asp:ImageButton ID="btnAssign" runat="server" CausesValidation="False" OnClick="btnAssign_Click"
                                        ImageUrl="~/App_Themes/EzEdit/Images/plugin_link.png" CommandArgument='<%# Eval("ItemID") %>'
                                        ToolTip="Edit Page Assignments" />
                                </td>
                                <td width="20">
                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" OnClick="btnDelete_Click"
                                        ImageUrl="~/App_Themes/EzEdit/Images/delete.png" OnClientClick="return confirmDeleteItem()"
                                        CommandArgument='<%# Eval("ItemID") %>' ToolTip="Delete" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dataLibraryItems" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
            SelectCommand="SELECT * FROM qryLibraryItems"></asp:SqlDataSource>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
        <table border="0" width="100%" cellpadding="5" cellspacing="0">
            <tr>
                <td class="form-label" nowrap>
                    Name:
                    <VAM:RequiredTextValidator runat="server" ControlIDToEvaluate="txtResourceName" SummaryErrorMessage="Name"
                        ErrorMessage="*" />
                </td>
                <td width="100%">
                    <asp:TextBox ID="txtResourceName" TabIndex="2" runat="server" CssClass="form" Width="400px" />
                </td>
            </tr>
            <tr>
                <td class="form-label" nowrap>
                    Enabled:
                </td>
                <td width="100%">
                    <asp:RadioButtonList ID="rblIsEnabled" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Yes" Value="True" />
                        <asp:ListItem Text="No" Value="False" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="form-label" nowrap>
                    Start Date:
                </td>
                <td width="100%">
                    <Date:DateTextBox ID="txtDisplayStartDate" runat="server" CssClass="form-text" Width="125" />
                </td>
            </tr>
            <tr>
                <td class="form-label" nowrap>
                    End Date:
                </td>
                <td width="100%">
                    <Date:DateTextBox ID="txtDisplayEndDate" runat="server" CssClass="form-text" Width="125" />
                </td>
            </tr>            
            <tr>
                <td class="form-label" valign="top">
                    Content:
                </td>
                <td>
                    <EzEdit:ContentEditor EditorWidth="600" EditorHeight="600" ID="txtContent" runat="server" />
                </td>
            </tr>
            
        </table>
        <hr />
        <div align="center">
            <asp:HiddenField ID="hdnItemID" runat="server" />
            <VAM:Button ID="btnSave" runat="server" CssClass="form-button" Text="Save" />&nbsp;
            <asp:Button ID="btnCancel" runat="server" CssClass="form-button" Text="Cancel" />
        </div>
    </asp:Panel>
</asp:Content>
