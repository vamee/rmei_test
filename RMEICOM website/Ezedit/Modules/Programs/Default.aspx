<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="Ezedit_Modules_Programs_Default" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table width="100%" cellpadding="2" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="lblPageTitle" runat="server" CssClass="pageTitle" /><br />
            </td>
            <td align="right">
                <a href="EditDiseases.aspx" class="main">Edit Diseases</a> | <a href="EditProgramTypes.aspx" class="main">Edit Program Types</a>
            </td>
        </tr>
    </table>
    
    <VAM:ValidationSummary ID="vdlSummary" runat="server" CssClass="alert" />
    <asp:Label ID="lblAlert" runat="server" CssClass="alert" />
    <hr />
    <asp:Panel ID="pnlList" runat="server">

        <script type="text/javascript" language="javascript">
        function confirmDelete()
        {
            if (confirm("Are you sure you want to delete this category?"))
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
                                <asp:ImageButton ID="btnAddNew1" runat="server" SkinID="IconAddNew" ToolTip="Add New Category"
                                    ImageAlign="Left" />
                            </td>
                            <td>
                                <asp:LinkButton ID="btnAddNew2" runat="server" CssClass="main" ToolTip="Add New Category"
                                    Text="Add New Category" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td align="right">
                    
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="gdvList" runat="server" AutoGenerateColumns="False" DataKeyNames="CategoryID"
            DataSourceID="dataModuleCategories" Width="100%" SkinID="GridView" RowStyle-VerticalAlign="Top"
            AllowSorting="true">
            <Columns>
                <asp:BoundField HeaderText="Name" DataField="CategoryName" ItemStyle-Width="200"
                    SortExpression="CategoryName" />
                <asp:TemplateField HeaderText="Display Pages">
                    <ItemTemplate>
                        <asp:Repeater ID="rptDisplayPages" runat="server" OnItemDataBound="FormatModulePagesRow">
                            <ItemTemplate>
                                <asp:Label ID="lblDisplayPages" runat="server" />
                            </ItemTemplate>
                        </asp:Repeater>
                        <li class="main"><a href="EditModuleProperties.aspx?ModuleKey=<%#Eval("ModuleKey")%>&ForeignKey=CategoryId&ForeignValue=<%#Eval("CategoryID")%>"
                            class="main">Add to Page </a></li>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <table>
                            <tr>
                                <td width="20" align="center">
                                    <asp:ImageButton ID="btnViewChildren" runat="server" CausesValidation="false" CommandName="ViewChildren"
                                        ImageUrl="~/App_Themes/EzEdit/Images/sitemap.png" CommandArgument='<%# Eval("CategoryID") %>'
                                        ToolTip="View" />
                                </td>
                                <td width="20" align="center">
                                    <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="False" CommandName="EditCategory"
                                        ImageUrl="~/App_Themes/EzEdit/Images/edit.png" CommandArgument='<%# Eval("CategoryID") %>'
                                        ToolTip="Edit" />
                                </td>
                                <td width="20" align="center">
                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="DeleteCategory"
                                        ImageUrl="~/App_Themes/EzEdit/Images/delete.png" OnClientClick="return confirmDelete()"
                                        CommandArgument='<%# Eval("CategoryID") %>' ToolTip="Delete" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <RowStyle VerticalAlign="Top" />
        </asp:GridView>
        <asp:SqlDataSource ID="dataModuleCategories" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
            SelectCommand="SELECT * FROM ModuleCategories WHERE ModuleKey = @ModuleKey And StatusID = @StatusID And LanguageID = @LanguageID ORDER BY CategoryName">
            <SelectParameters>
                <asp:Parameter Name="ModuleKey" Type="String" DefaultValue="Programs" />
                <asp:Parameter DefaultValue="20" Name="StatusID" Type="Int32" />
                <asp:SessionParameter Name="LanguageID" SessionField="EzEditLanguageID" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
        <%--<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetModuleCategories"
            TypeName="ModuleCategory">
            <SelectParameters>
                <asp:QueryStringParameter Name="strModuleKey" QueryStringField="ModuleKey" Type="String" />
                <asp:Parameter DefaultValue="20" Name="intStatusID" Type="Int32" />
                <asp:SessionParameter DefaultValue="" Name="intLanguageID" SessionField="EzEditLanguageID"
                    Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>--%>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
        <table width="100%" cellpadding="0" cellspacing="0" border="0" bordercolor="#999999">
            <tr>
                <td>
                    <table border="0" width="100%" cellpadding="5" cellspacing="0">
                        <tr>
                            <td class="form-label" nowrap>
                                Category Name:
                                <VAM:RequiredTextValidator ID="RequiredTextValidator4" runat="server" ControlIDToEvaluate="txtCategoryName"
                                    SummaryErrorMessage="Category name is required" ErrorMessage="*" />
                            </td>
                            <td width="100%">
                                <asp:TextBox ID="txtCategoryName" Width="400px" CssClass="form-textbox" TabIndex="1"
                                    runat="server" />
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
                        <tr id="trRssFeed" runat="server">
                            <td class="form-label" valign="top" nowrap>
                                Publish as RSS Feed:
                            </td>
                            <td width="100%">
                                <asp:CheckBox ID="cbxPublishToRss" runat="server" AutoPostBack="true" />
                            </td>
                        </tr>
                        <asp:Panel ID="pnlRssInfo" runat="server" Visible="false">
                            <tr>
                                <td class="form-label" valign="top" nowrap>
                                    Rss Feed Title:
                                    <VAM:RequiredTextValidator ID="RequiredTextValidator1" runat="server" ControlIDToEvaluate="txtRssTitle"
                                        SummaryErrorMessage="RSS feed title is required" ErrorMessage="*" />
                                </td>
                                <td width="100%">
                                    <asp:TextBox ID="txtRssTitle" Width="400px" CssClass="form-textbox" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="form-label" valign="top" nowrap>
                                    Rss Feed Description:
                                    <VAM:RequiredTextValidator ID="RequiredTextValidator2" runat="server" ControlIDToEvaluate="txtRssDescription"
                                        SummaryErrorMessage="RSS feed description is required" ErrorMessage="*" />
                                </td>
                                <td width="100%">
                                    <asp:TextBox ID="txtRssDescription" Width="400px" CssClass="form-textbox" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="form-label" valign="top" nowrap>
                                    Rss Feed Manager Email:
                                    <VAM:EmailAddressValidator ID="RequiredTextValidator3" runat="server" ControlIDToEvaluate="txtRssManagingEditor"
                                        SummaryErrorMessage="Valid RSS feed manager email is required" ErrorMessage="*" />
                                </td>
                                <td width="100%">
                                    <asp:TextBox ID="txtRssManagingEditor" Width="400px" CssClass="form-textbox" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="form-label" valign="top" nowrap>
                                    Rss Feed Image:
                                    <VAM:CompareToStringsValidator ID="CompareToStringsValidator1" runat="server" ControlIDToEvaluate="uplRssImageUrl"
                                        MatchTextRule="EndsWith" SummaryErrorMessage="Only valid image formats are allowed (.jpg, .gif, .png)"
                                        ErrorMessage="*">
                                        <Items>
                                            <VAM:CompareToStringsItem Value=".gif" />
                                            <VAM:CompareToStringsItem Value=".jpg" />
                                            <VAM:CompareToStringsItem Value=".jpeg" />
                                            <VAM:CompareToStringsItem Value=".png" />
                                        </Items>
                                    </VAM:CompareToStringsValidator>
                                </td>
                                <td width="100%">
                                    <asp:FileUpload ID="uplRssImageUrl" runat="server" Width="400" CssClass="form-textbox" /><br />
                                    <asp:HyperLink ID="hypRssImageUrl" runat="server" CssClass="main" />
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:HiddenField ID="hdnCategoryID" runat="server" />
                                <VAM:Button ID="btnSave" runat="server" Text="Save" CssClass="form-button" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False"
                                    CssClass="form-button" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
