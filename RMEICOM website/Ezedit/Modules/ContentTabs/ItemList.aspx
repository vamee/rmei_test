<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="ItemList.aspx.vb" Inherits="Ezedit_Modules_ContentTabs_ItemList" Title="Content Tabs"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <asp:Label ID="lblPageTitle" runat="server" CssClass="pageTitle" />
    <br />
    <asp:Label ID="lblAlert" runat="server" CssClass="message" EnableViewState="false"></asp:Label>
    <VAM:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="The following fields are required:"
        CssClass="message" />
    <hr />
    <asp:Panel ID="pnlItemList" runat="server" Visible="false">

        <script type="text/javascript" language="javascript">
        function deleteItem()
        {
            if (confirm("Are you sure you want to delete this tab?")==true)
                return true;
            else
                return false;
        } 
        </script>

        <table cellpadding="2" width="100%">
            <tr>
                <td>
                    <asp:LinkButton ID="btnAddItem" runat="server" Text="Add New Tab" CssClass="main"><img src="/App_Themes/EzEdit/Images/tab_add.png" border="0" />&nbsp;Add New Tab</asp:LinkButton>
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="gdvItems" runat="server" AutoGenerateColumns="false" Width="100%"
            CellPadding="3" HeaderStyle-CssClass="table-header" AlternatingRowStyle-CssClass="table-altrow"
            RowStyle-CssClass="table-row" DataSourceID="dataContent" AllowSorting="true"
            AllowPaging="true" PageSize="25" PagerSettings-Mode="NumericFirstLast">
            <EmptyDataTemplate>
                There are no tabs in this category.
            </EmptyDataTemplate>
            <Columns>
                <asp:BoundField DataField="ResourceName" HeaderText="Tab Name" HeaderStyle-HorizontalAlign="left"
                    ItemStyle-HorizontalAlign="left" SortExpression="ResourceName" />
                <asp:TemplateField HeaderText="Sort" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50"
                    SortExpression="SortOrder">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlSortOrder" runat="server" OnSelectedIndexChanged="UpdateSortOrder"
                            AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:HiddenField ID="hdnSortOrder" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60"
                    SortExpression="IsEnabled">
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Enabled" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center"
                    ItemStyle-Width="50" SortExpression="IsEnabled">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbxIsEnabled" runat="server" OnCheckedChanged="ToggleIsEnabled"
                            AutoPostBack="true" />
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:CheckBoxField DataField="IsEnabled" HeaderText="Enabled" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center" ItemStyle-Width="50" />--%>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60">
                    <ItemTemplate>
                        <table>
                            <tr>
                                <td width="20">
                                    <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/edit.png"
                                        OnClick="EditItem" ToolTip="Edit Tab" />
                                </td>
                                <td width="20">
                                    <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/delete.png"
                                        OnClientClick="return deleteItem();" OnClick="DeleteRecord" ToolTip="Delete Tab" />
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField ID="hdnItemID" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dataContent" runat="server" SelectCommand="SELECT DISTINCT TabID, ResourceName, SortOrder, DisplayStartDate, DisplayEndDate, IsEnabled FROM qryContentTabs WHERE CategoryID = @CategoryID ORDER BY SortOrder"
            ConnectionString="<%$ ConnectionStrings:WebsiteDB %>">
            <SelectParameters>
                <asp:QueryStringParameter Name="CategoryID" QueryStringField="CategoryID" DefaultValue="0"
                    Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
    </asp:Panel>
    <asp:Panel ID="pnlEditItem" runat="server" Visible="false">
        <table cellpadding="5" cellspacing="0" border="0">
            <tr>
                <td align="right">
                    <asp:Label ID="lblCategory" runat="server" CssClass="form-label" Text="Tab Group:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCategoryID" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="<- Please Choose ->" Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblResourceName" runat="server" CssClass="form-label" Text="Title:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtResourceName" runat="server" CssClass="form-textbox" Width="400"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top">
                    <asp:Label ID="lblIsEnabled" runat="server" CssClass="form-label" Text="Enabled:"></asp:Label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblIsEnabled" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top">
                    <asp:Label ID="lblStartDate" runat="server" CssClass="form-label" Text="Start Date:"></asp:Label>
                </td>
                <td>
                    <Date:DateTextBox ID="txtStartDate" runat="server" Width="100" CssClass="form-textbox" />
                </td>
            </tr>
            <tr>
                <td align="right" valign="top">
                    <asp:Label ID="lblEndDate" runat="server" CssClass="form-label" Text="End Date:"></asp:Label>
                </td>
                <td>
                    <Date:DateTextBox ID="txtEndDate" runat="server" Width="100" CssClass="form-textbox" />
                </td>
            </tr>
            <tr>
                <td align="right" valign="top">
                    <asp:Label ID="lblContent" runat="server" CssClass="form-label" Text="Content:"></asp:Label>
                </td>
                <td>
                    <EzEdit:ContentEditor EditorWidth="600" EditorHeight="600" ID="ucContentEditor" runat="server" />
                </td>
            </tr>
            <tr>
                <td align="right">
                </td>
                <td>
                    <VAM:Button ID="btnSaveItem" runat="server" CssClass="button" Text="Save" />
                    <asp:Button ID="btnCancelItem" runat="server" CssClass="button" Text="Cancel" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnItemID" runat="server" />
        <asp:HiddenField ID="hdnCategoryID" runat="server" />
        <VAM:RequiredListValidator runat="server" ControlIDToEvaluate="ddlCategoryID" SummaryErrorMessage="Category"
            UnassignedIndex="0" />
        <VAM:RequiredTextValidator runat="server" ControlIDToEvaluate="txtResourceName" SummaryErrorMessage="Tab Name" />
    </asp:Panel>
</asp:Content>
