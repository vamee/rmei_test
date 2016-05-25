<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="ItemList.aspx.vb" Inherits="Ezedit_Modules_DL01_Default" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <p>
        <a href="../Default.aspx?ModuleKey=DL01" class="pageTitle">Downloads</a>
        <asp:Label ID="lblPageTitle" CssClass="pagetitle" runat="server" Text=""></asp:Label>
    </p>
    <asp:Label ID="lblAlert" runat="server" CssClass="alert" />
    <hr />
    <asp:Panel ID="pnlList" runat="server">
        <table cellpadding="2" cellspacing="0" border="0" id="table1">
            <tr>
                <td>
                    <asp:ImageButton ID="ibtnAdd" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/page_add.png" />
                </td>
                <td>
                    <asp:LinkButton ID="lbtnAdd" runat="server" CssClass="main">Add New Download</asp:LinkButton>
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="gdvList" runat="server" AutoGenerateColumns="False" DataKeyNames="CategoryID"
            DataSourceID="dataDownloads" Width="100%" SkinID="GridView" RowStyle-VerticalAlign="Top" AllowSorting="true" AllowPaging="true" PageSize="25" PagerSettings-Mode="NumericFirstLast">
            <Columns>
                <asp:BoundField HeaderText="Name" DataField="ResourceName" SortExpression="ResourceName" />
                <asp:TemplateField HeaderText="Type" ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" SortExpression="ModuleType">
                    <ItemTemplate>
                        <asp:HyperLink ID="hypModuleType" runat="server" />
                        <asp:HyperLink ID="hypExternalUrl" runat="server" ToolTip='External Link' NavigateUrl='<%# Eval("ExternalUrl") %>'
                            Target="_blank" Visible="false" ImageUrl="~/App_Themes/EzEdit/Images/world_go.png" />
                        <asp:HyperLink ID="hypFileName" runat="server" ToolTip='File Download' NavigateUrl='<%# Eval("FileName") %>'
                            Target="_blank" Visible="false" ImageUrl="~/App_Themes/EzEdit/Images/disk.png" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Size" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-Width="60" SortExpression="FileSize">
                    <ItemTemplate>
                        <asp:Label ID="lblFileSize" runat="server" Text='<%#Emagine.FormatFileSize(Eval("FileSize"))%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sort" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40"
                    HeaderStyle-HorizontalAlign="Center" SortExpression="SortOrder">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlSortOrder" runat="server" OnSelectedIndexChanged="ddlSortOrder_SelectedIndexChanged"
                            AutoPostBack="true" />
                        <asp:HiddenField ID="hdnSortOrder" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Reg Req'd" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40"
                    HeaderStyle-HorizontalAlign="Center" SortExpression="RegistrationRequired">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbxRegistrationRequired" runat="server" Checked='<%# Eval("RegistrationRequired") %>'
                            OnCheckedChanged="cbxRegistrationRequired_CheckChanged" AutoPostBack="true" />
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
                                        ImageUrl="~/App_Themes/EzEdit/Images/edit.png" CommandArgument='<%# Eval("DownloadID") %>'
                                        ToolTip="Edit" />
                                </td>
                                <td width="20">
                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" OnClick="btnDelete_Click"
                                        ImageUrl="~/App_Themes/EzEdit/Images/delete.png" OnClientClick="return confirmDeleteItem()"
                                        CommandArgument='<%# Eval("DownloadID") %>' ToolTip="Delete" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <RowStyle VerticalAlign="Top" />
        </asp:GridView>
        <asp:SqlDataSource ID="dataDownloads" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
            SelectCommand="SELECT * FROM qryDownloads WHERE CategoryID = @CategoryID ORDER BY SortOrder">
            <SelectParameters>
                <asp:QueryStringParameter Name="CategoryID" QueryStringField="CategoryID" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <%--<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetDownloads"
            TypeName="DL01">
            <SelectParameters>
                <asp:QueryStringParameter Name="intCategoryID" QueryStringField="CategoryID" Type="String" />
                <asp:Parameter Name="blnIsEnabled" DefaultValue="False" Type="Boolean" />
            </SelectParameters>
        </asp:ObjectDataSource>--%>

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
</asp:Content>
