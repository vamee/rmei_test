<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="Ezedit_Modules_PR01_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <asp:Label ID="lblPageTitle" runat="server" CssClass="pageTitle" Text="Membership" /><br />
    <VAM:ValidationSummary ID="vdlSummary" runat="server" CssClass="alert" />
    <asp:Label ID="lblAlert" runat="server" CssClass="alert" />
    <hr />
    <asp:Panel ID="pnlList" runat="server">

        <script type="text/javascript" language="javascript">
        function confirmDelete()
        {
            if (confirm("Are you sure you want to delete this role?"))
                return true;
            else
                return false;
        } 
        </script>

        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" border="0" id="table1">
                        <tr>
                            <td>
                                <asp:ImageButton ID="btnAddNew1" runat="server" ToolTip="Add New Membership Role"
                                    ImageUrl="~/App_Themes/EzEdit/Images/group_add.png" ImageAlign="Left" />
                            </td>
                            <td>
                                <asp:LinkButton ID="btnAddNew2" runat="server" CssClass="main" ToolTip="Add New Membership Role"
                                    Text="Add New Role" />
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
        <asp:GridView ID="gdvList" runat="server" AutoGenerateColumns="False" DataKeyNames="CategoryID"
            DataSourceID="ObjectDataSource1" Width="100%" SkinID="GridView" RowStyle-VerticalAlign="Top">
            <Columns>
                <asp:BoundField HeaderText="Name" DataField="CategoryName" />
                <asp:TemplateField ShowHeader="False" ItemStyle-Width="100">
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:ImageButton ID="btnViewChildren" runat="server" CausesValidation="false" CommandName="ViewChildren"
                            ImageUrl="~/App_Themes/EzEdit/Images/sitemap.png" CommandArgument='<%# Eval("CategoryID") %>'
                            ToolTip="View" />
                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="False" CommandName="EditCategory"
                            ImageUrl="~/App_Themes/EzEdit/Images/edit.png" CommandArgument='<%# Eval("CategoryID") %>'
                            ToolTip="Edit" />
                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="DeleteCategory"
                            ImageUrl="~/App_Themes/EzEdit/Images/delete.png" OnClientClick="return confirmDelete()"
                            CommandArgument='<%# Eval("CategoryID") %>' ToolTip="Delete" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <RowStyle VerticalAlign="Top" />
        </asp:GridView>
        &nbsp;
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetModuleCategories"
            TypeName="ModuleCategory">
            <SelectParameters>
                <asp:QueryStringParameter Name="strModuleKey" QueryStringField="ModuleKey" Type="String"
                    DefaultValue="Membership" />
                <asp:Parameter DefaultValue="20" Name="intStatusID" Type="Int32" />
                <asp:SessionParameter DefaultValue="" Name="intLanguageID" SessionField="EzEditLanguageID"
                    Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
        <table width="100%" cellpadding="0" cellspacing="0" border="0" bordercolor="#999999">
            <tr>
                <td>
                    <table border="0" width="100%" cellpadding="5" cellspacing="0">
                        <tr>
                            <td class="form-label" nowrap>
                                Role Name:
                                <VAM:RequiredTextValidator ID="RequiredTextValidator4" runat="server" ControlIDToEvaluate="txtCategoryName"
                                    SummaryErrorMessage="Role name is required" ErrorMessage="*" />
                            </td>
                            <td width="100%">
                                <asp:TextBox ID="txtCategoryName" Width="400px" CssClass="form-textbox" TabIndex="1"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="form-label" valign="top" nowrap>
                                Home Page:
                            </td>
                            <td width="100%">
                                <asp:DropDownList ID="ddlHomePageID" runat="server" AppendDataBoundItems="true">
                                    <asp:ListItem Text="<- Please Choose ->" Value="" />
                                </asp:DropDownList>
                                <VAM:RequiredListValidator runat="server" ControlIDToEvaluate="ddlHomePageID" SummaryErrorMessage="Please choose a home page from the list." ErrorMessage="*" UnassignedIndex="0" />
                            </td>
                        </tr>
                        <tr>
                            <td class="form-label" valign="top" nowrap>
                                Description:
                            </td>
                            <td width="100%">
                                <asp:TextBox ID="txtDescription" Width="400px" Height="50px" TextMode="MultiLine"
                                    CssClass="form-textbox" TabIndex="2" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:HiddenField ID="hdnCategoryID" runat="server" />
                                <VAM:Button ID="btnSave" runat="server" Text="Save" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
