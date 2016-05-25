<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="Ezedit_Admin_Sql_Default" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <asp:Label ID="lblAlert" runat="server" CssClass="alert" />
    <asp:Panel ID="pnlList" runat="server">

        <script language="javascript" type="text/javascript">
        function confirmDelete() {
            if (confirm("Are you sure you want to delete this record?")) {
                return true;
            }else{
                return false;            
            }
        }
        </script>

        <table width="100%" cellpadding="3" cellspacing="0" border="0">
            <tr>
                <td>
                    <asp:DropDownList ID="ddlTableNames" runat="server" AppendDataBoundItems="true" AutoPostBack="true">
                        <asp:ListItem Text="<- Choose a Table ->" Value="" />
                    </asp:DropDownList>
                </td>
                <td align="right">
                    <asp:LinkButton ID="btnAddNew" runat="server" CssClass="main" Text="Add New Record"
                        Visible="false" />
                </td>
            </tr>
        </table>
        <asp:GridView ID="gdvData" runat="server" AutoGenerateColumns="True" Width="100%"
            CellPadding="3" HeaderStyle-CssClass="table-header" AlternatingRowStyle-CssClass="table-altrow"
            RowStyle-CssClass="table-row" AllowPaging="True" AllowSorting="false" DataSourceID="dsTableData"
            HeaderStyle-HorizontalAlign="Left" RowStyle-VerticalAlign="Top">
            <Columns>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60">
                    <ItemTemplate>
                        <table cellpadding="2" cellspacing="0">
                            <tr>
                                <td width="25">
                                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="main" OnClick="btnEdit_Click" ToolTip="Edit"><img src="../../../App_Themes/EzEdit/Images/edit.png" border="0" /></asp:LinkButton>
                                </td>
                                <td width="25">
                                    <asp:LinkButton ID="btnDelete" runat="server" CssClass="Main" OnClick="btnDelete_Click"
                                        OnClientClick="return confirmDelete();" ToolTip="Delete"><img src="../../../App_Themes/EzEdit/Images/delete.png" border="0" /></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsTableData" runat="server" SelectCommand="" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>" />
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
        <asp:Table ID="tblEdit" runat="server" Width="100%">
        </asp:Table>
        <center>
            <VAM:Button ID="btnSave" runat="server" Text="Save" CssClass="form-button" />
            &nbsp;
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="form-button" />
        </center>
        <asp:HiddenField ID="hdnRowID" runat="server" Value="-1" />
    </asp:Panel>
</asp:Content>
