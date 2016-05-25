<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="PageModules.aspx.vb" Inherits="Ezedit_Modules_Pages02_PageModules"
    Title="Page Modules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <asp:Label ID="lblBreadcrumbs" runat="server" />
    <asp:Label ID="lblAlert" runat="server" CssClass="alert" />
    <asp:Panel ID="pnlList" runat="server">

        <script language="javascript" type="text/javascript">
        function confirmDeletePageModule() {
            if (confirm("Are you sure you want to remove this module from the page?")) {
                return true;
            }else{
                return false;
            }
        }
        
        function displayFormWarning() {
            alert("This is a registration form and cannot be deleted here.");
            return false();
        }
        </script>

        <%--<table border="0" cellpadding="3" cellspacing="0" width="100%">
            <tr>
                <td>
                    <asp:LinkButton ID="lbtnAddNew" runat="server" class="main"><img src="/App_Themes/EzEdit/Images/page_add.png" alt="Add New Page" border="0" /> Add New Page</asp:LinkButton>
                </td>
            </tr>
        </table>--%>
        <asp:GridView ID="gdvList" runat="server" AutoGenerateColumns="False" Width="100%"
            CellPadding="3" HeaderStyle-CssClass="table-header" AlternatingRowStyle-CssClass="table-altrow"
            RowStyle-CssClass="table-row">
            <EmptyDataTemplate>
                No modules have been assigned to this page.
            </EmptyDataTemplate>
            <Columns>
                <asp:BoundField DataField="ModuleName" HeaderText="Name" HeaderStyle-HorizontalAlign="left"
                    ItemStyle-HorizontalAlign="left" />
                <asp:BoundField DataField="PageModuleType" HeaderText="Module Page Type" HeaderStyle-HorizontalAlign="left" />
                <asp:TemplateField HeaderText="Sort" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlSortOrder" runat="server" OnSelectedIndexChanged="ddlSortOrder_IndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:HiddenField ID="hdnSortOrder" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Width="25" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <table border="0" cellspacing="2">
                            <tr>
                                <td width="16" runat="server" visible="false">
                                    <asp:ImageButton ID="btnEditItem" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/page_white_gear.png"
                                        OnClick="btnEditItem_Click" ToolTip="Edit Module Properties" />
                                </td>
                                <td width="16" id="tdDelete" runat="server" visible="false">
                                    <asp:ImageButton ID="btnDeleteItem" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/delete.png"
                                        OnClick="btnDeleteItem_Click" OnClientClick="return confirmDeletePageModule();" 
                                        ToolTip="Remove from Page"  />
                                </td>
                                <td width="16" id="tdHelp" runat="server" visible="false">
                                    <img src="/App_Themes/EzEdit/Images/help.png" alt="This is a registration form and cannot be deleted here." onclick="displayFormWarning();" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
</asp:Content>
