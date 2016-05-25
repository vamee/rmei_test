<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="Ezedit_Modules_Lookups_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">

    <script language="javascript" type="text/javascript">
        function confirmDelete() {
            if (confirm("Are you sure you want to delete this lookup?")) {
                return true;
            }else{
                return false;
            }
        }
    </script>

    <span class="pageTitle">Manage Lookups</span>
    <center>
        <asp:Label ID="lblAlert" runat="server" CssClass="alert" /></center>
    <asp:Panel ID="pnlList" runat="server" Visible="false">
        <table cellpadding="3" cellspacing="0" width="100%">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnAddNew" runat="server" ImageUrl="~/App_Themes/EzEdit/images/magnifier_zoom_in.png"
                                    AlternateText="Add New Lookup" />
                            </td>
                            <td>
                                <asp:LinkButton ID="lbtnAddNew" runat="server" Text="Add New Lookup" CssClass="main" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gdvList" runat="server" AllowPaging="True" AllowSorting="True"
                        AutoGenerateColumns="False" DataKeyNames="LookupID" DataSourceID="dataList" Width="100%"
                        CellPadding="2" HeaderStyle-CssClass="table-header" RowStyle-CssClass="table-row"
                        AlternatingRowStyle-CssClass="table-altrow" PageSize="25">
                        <Columns>
                            <asp:BoundField DataField="LookupName" HeaderText="Name" SortExpression="LookupName"
                                HeaderStyle-HorizontalAlign="Left" />
                            <asp:TemplateField ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Table runat="server" Width="50" CellPadding="1" CellSpacing="0">
                                        <asp:TableRow>
                                            <asp:TableCell Width="25">
                                                <asp:ImageButton ID="btnEditItem" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/Edit.png"
                                                    OnClick="btnEditItem_Click" />
                                            </asp:TableCell>
                                            <asp:TableCell Width="25">
                                                <asp:ImageButton ID="btnDeleteItem" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/Delete.png"
                                                    OnClick="btnDeleteItem_Click" OnClientClick="return confirmDelete();" />
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="dataList" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
                        SelectCommand="SELECT * FROM [Lookups] ORDER BY LookupName"></asp:SqlDataSource>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server" Visible="false">

        <script language="javascript" type="text/javascript">
        function confirmDelete() {
            if (confirm("Are you sure you want to delete this option?")) {
                return true;
            }else{
                return false;
            }
        }        
        </script>

        <table cellpadding="3" cellspacing="0" border="0">
            <tr>
                <td>
                </td>
                <td>
                    <VAM:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="The following fields are required:"
                        CssClass="alert" AutoUpdate="true" />
                </td>
            </tr>
            <%--<tr id="trAutoNumber" runat="server">
                <td><asp:Label ID="lblAutoNumber" runat="server" CssClass="form-label" /></td>
                <td>
                    <asp:CheckBox ID="cbxAutoNumber"
                </td>
            </tr>--%>
            <tr id="trLookupName" runat="server">
                <td>
                    <asp:Label ID="lblLookupName" runat="server" CssClass="form-label" Text="Lookup Name:" />
                    <VAM:RequiredTextValidator ID="RequiredTextValidator1" runat="server" ControlIDToEvaluate="txtLookupName"
                        SummaryErrorMessage="LookupName" ErrorMessage="*" />
                    <VAM:CustomValidator ID="cvLookupName" runat="server" EventsThatValidate="OnSubmit"
                        ControlIDToEvaluate="txtLookupName" SummaryErrorMessage="Lookup name already exists." />
                </td>
                <td>
                    <asp:TextBox ID="txtLookupName" runat="server" CssClass="form-textbox" Width="250" />
                </td>
            </tr>
            <tr id="trValueFieldType" runat="server">
                <td>
                    <asp:Label ID="Label2" runat="server" CssClass="form-label" Text="Type:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlValueFieldType" runat="server" CssClass="form-dropdown"
                        AutoPostBack="true">
                        <asp:ListItem Text="Editable" Value="Editable" />
                        <asp:ListItem Text="Non-Editable - AutoNumber" />
                        <asp:ListItem Text="Non-Editable - Sync w/ Text Field" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trItemView" runat="server" visible="false">
                <td valign="top">
                    <asp:Label ID="lblValues" runat="server" CssClass="form-label" Text="Values:" />
                </td>
                <td>
                    <asp:GridView ID="gdvItems" runat="server" AutoGenerateColumns="false" Width="100%"
                        CellPadding="3" HeaderStyle-CssClass="table-header" FooterStyle-CssClass="table-header"
                        RowStyle-CssClass="table-row" AlternatingRowStyle-CssClass="table-altrow" ShowFooter="true">
                        <Columns>
                            <asp:TemplateField HeaderText="Display Text">
                                <FooterTemplate>
                                    <asp:LinkButton ID="btnAddOptions" runat="server" OnClick="btnAddOptions_Click" Text="Add Options" />
                                </FooterTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtOptionText" runat="server" Width="250" CssClass="form-textbox" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Value">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtOptionValue" runat="server" Width="250" CssClass="form-textbox" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sort" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlSortOrder" runat="server" CssClass="form-dropdown" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlSortOrder_SelectedIndexChanged" />
                                    <asp:HiddenField ID="hdnSortOrder" runat="server" />
                                    <asp:HiddenField ID="hdnOptionID" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="25" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnDeleteOption" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/delete.png"
                                        OnClick="btnDeleteOption_Click" OnClientClick="return confirmDelete();" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr id="trBulkView" runat="server" visible="false">
                <td valign="top">
                    <asp:Label ID="Label1" runat="server" CssClass="form-label" Text="Options:" />
                    <VAM:RequiredTextValidator ID="vld2" runat="server" ControlIDToEvaluate="txtBulkValues"
                        SummaryErrorMessage="Options" ErrorMessage="*" />
                </td>
                <td>
                    <asp:TextBox ID="txtBulkValues" runat="server" TextMode="MultiLine" Width="400" Height="400" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <VAM:Button ID="btnSave" runat="server" CssClass="form-button" Text="Save" />&nbsp;
                    <asp:Button ID="btnCancel" runat="server" CssClass="form-button" Text="Cancel" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:HiddenField ID="hdnItem" runat="server" />
    <asp:HiddenField ID="hdnAction" runat="server" />
</asp:Content>
