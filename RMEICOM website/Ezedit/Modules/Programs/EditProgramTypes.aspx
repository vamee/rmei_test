<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false" CodeFile="EditProgramTypes.aspx.vb" Inherits="Ezedit_Modules_Programs_ProgramTypes" title="Untitled Page" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">

    <script language="javascript" type="text/jscript">
         function confirmDeleteItem() {
            if (confirm("Are you sure you want to delete this item?")==true)
                return true;
            else
                return false;
        } 
    </script>
    <table width="100%" cellpadding="2" cellspacing="0">
        <tr>
            <td>
                <a href="Default.aspx" class="pageTitle">Programs</a><span class="pageTitle"> > Edit Program Types</span>
    <asp:Label ID="lblPageTitle" CssClass="pagetitle" runat="server" Text="" />
            </td>
            <td align="right">
                <a href="Default.aspx" class="main"><< Back to Programs</a>
            </td>
        </tr>
    </table>
    <asp:Label ID="lblAlert" runat="server" CssClass="alert" />
    <VAM:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert" HeaderText="The following fields are required:"
        ScrollIntoView="Top" AutoUpdate="true" />
    <hr />
    <asp:Panel ID="pnlList" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" border="0" id="table1">
                    <tr>
                        <td>
                            <asp:LinkButton ID="btnAddNew" runat="server" CssClass="main"><img src="/App_Themes/EzEdit/Images/table_add.png" border="0" />&nbsp;Add New Program Type</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right">
                &nbsp;
            </td>
        </tr>
    </table>
    <br />
        <asp:GridView ID="gdvList" runat="server" AutoGenerateColumns="False" Width="100%"
            CellPadding="3" HeaderStyle-CssClass="table-header" AlternatingRowStyle-CssClass="table-altrow"
            RowStyle-CssClass="table-row" DataSourceID="dataProgramTypes" AllowSorting="true"
            AllowPaging="true" PageSize="25" PagerSettings-Mode="NumericFirstLast">
            <EmptyDataTemplate>
                No records found.
            </EmptyDataTemplate>
            <Columns>
                <asp:BoundField DataField="ResourceName" HeaderText="Title" HeaderStyle-HorizontalAlign="left"
                    ItemStyle-HorizontalAlign="left" SortExpression="ResourceName" />
                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60">
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" Width="55" /><br />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Enabled" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50"
                    SortExpression="IsEnabled">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbxIsEnabled" runat="server" AutoPostBack="true" OnCheckedChanged="cbxIsEnabled_CheckChanged" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sort" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50"
                    SortExpression="SortOrder">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlSortOrder" runat="server" OnSelectedIndexChanged="ddlSortOrder_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:HiddenField ID="hdnSortOrder" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <table border="0" cellspacing="2">
                            <tr>
                                <td width="16">
                                    <asp:ImageButton ID="btnEditItem" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/edit.png"
                                        OnClick="btnEditItem_Click" ToolTip="Edit Item" />
                                </td>
                                <td width="16">
                                    <asp:ImageButton ID="btnDeleteItem" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/delete.png"
                                        OnClientClick="return confirmDeleteItem();" OnClick="btnDeleteItem_Click" ToolTip="Delete Item" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dataProgramTypes" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
            SelectCommand="SELECT * FROM qryCustom_ProgramTypes ORDER BY SortOrder" />
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
        <table>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" CssClass="form-label" Text="Program Type:" />
                    <VAM:RequiredTextValidator ID="RequiredTextValidator1" runat="server" ControlIDToEvaluate="txtResourceName" SummaryErrorMessage="Program Type" />
                </td>
                <td>
                    <asp:TextBox ID="txtResourceName" runat="server" CssClass="form-textbox" Width="400" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" CssClass="form-label" Text="Icon:" />
                </td>
                <td>
                    <EzEdit:FileBrowser ID="fbIconUrl" runat="server" CssClass="form-textbox" DefaultPath="/Collateral/Images/" IsRequired="true" SummaryErrorMessage="Icon" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <VAM:Button ID="btnSave" runat="server" CssClass="form-button" Text="Save" />
                    <asp:Button ID="btnCancel" runat="server" CssClass="form-button" Text="Cancel" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnItemID" runat="server" />
    </asp:Panel>
</asp:Content>

