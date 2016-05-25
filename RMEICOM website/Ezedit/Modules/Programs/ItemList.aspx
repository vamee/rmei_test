<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false" EnableEventValidation="false"
    CodeFile="ItemList.aspx.vb" Inherits="Ezedit_Modules_Programs_ItemList" Title="EzEdit > Programs" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <p>
        <a href="Default.aspx?ModuleKey=Programs" class="pageTitle">Programs</a>
        <asp:Label ID="lblPageTitle" CssClass="pagetitle" runat="server" Text="" />
    </p>
    <asp:Label ID="lblAlert" runat="server" CssClass="alert" />
    <hr />
    <asp:Panel ID="pnlList" runat="server">
        <table cellpadding="2" cellspacing="0" border="0" id="table1" width="100%">
            <tr>
                <td width="16">
                    <asp:ImageButton ID="ibtnAdd" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/page_add.png" />
                </td>
                <td>
                    <asp:LinkButton ID="lbtnAdd" runat="server" CssClass="main">Add New Program</asp:LinkButton>
                </td>
                <td align="right">
                    <asp:DropDownList ID="ddlArchive" runat="server" AutoPostBack="true">
                        <asp:ListItem Text="All" Value="" />
                        <asp:ListItem Text="Archived" Value="True" />
                        <asp:ListItem Text="Not Archived" Value="False" />
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="gdvList" runat="server" AutoGenerateColumns="False"
            DataSourceID="dataPrograms" Width="100%" SkinID="GridView" RowStyle-VerticalAlign="Top" AllowSorting="true" AllowPaging="true" PageSize="25" PagerSettings-Mode="NumericFirstLast">
            <Columns>
                <asp:BoundField HeaderText="Name" DataField="ResourceName" SortExpression="ResourceName" />
                <asp:BoundField HeaderText="Type" DataField="ProgramType" SortExpression="ProgramType" />
                <asp:BoundField HeaderText="Disease" DataField="DiseaseName" SortExpression="DiseaseName" />
                <asp:TemplateField HeaderText="Archived" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40" SortExpression="IsArchived">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbxIsArchived" runat="server" Checked='<%# Eval("IsArchived") %>'
                            OnCheckedChanged="cbxIsArchived_CheckChanged" AutoPostBack="true" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Enabled" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40" SortExpression="IsEnabled">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbxIsEnabled" runat="server" Checked='<%# Eval("IsEnabled") %>'
                            OnCheckedChanged="cbxIsEnabled_CheckChanged" AutoPostBack="true" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <table>
                            <tr>
                                <td width="20">
                                    <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="False" OnClick="btnEdit_Click"
                                        ImageUrl="~/App_Themes/EzEdit/Images/edit.png" CommandArgument='<%# Eval("ProgramID") %>'
                                        ToolTip="Edit" />
                                </td>
                                <td width="20">
                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" OnClick="btnDelete_Click"
                                        ImageUrl="~/App_Themes/EzEdit/Images/delete.png" OnClientClick="return confirmDeleteItem()"
                                        CommandArgument='<%# Eval("ProgramID") %>' ToolTip="Delete" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <RowStyle VerticalAlign="Top" />
        </asp:GridView>
        <asp:SqlDataSource ID="dataPrograms" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
            SelectCommand="SELECT * FROM qryCustom_Programs WHERE (ModuleCategoryID = @ModuleCategoryID) ORDER BY ResourceName">
            <SelectParameters>
                <asp:QueryStringParameter Name="ModuleCategoryID" QueryStringField="CategoryID" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>

        <script language="javascript" type="text/javascript">
            function confirmDeleteItem() {
                if (confirm("Are you sure you want to delete this item?")) {
                    return true;
                }else{
                    return false;
                }
            }
        </script>

    </asp:Panel>
    <asp:HiddenField ID="hdnOrderBy" runat="server" />
    <asp:HiddenField ID="hdnOrderByDirection" runat="server" />
</asp:Content>
