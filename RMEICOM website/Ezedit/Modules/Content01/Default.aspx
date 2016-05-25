<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="Ezedit_Modules_Content01_Default" Title="Miscellaneous Content" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <asp:Label ID="lblPageTitle" runat="server" CssClass="pageTitle" Text="Misc Content" />
    <br />
    <asp:Label ID="lblAlert" runat="server" CssClass="message" EnableViewState="false"></asp:Label>
    <VAM:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="The following fields are required:"
        CssClass="alert" />
    <hr />
    <br />
    <asp:Panel ID="pnlContent" runat="server" Visible="false">

        <script type="text/javascript" language="javascript">
        function deleteContent()
        {
            if (confirm("Are you sure you want to delete this content?")==true)
                return true;
            else
                return false;
        } 
        </script>

        <table cellpadding="2">
            <tr>
                <td>
                    <asp:ImageButton ID="btnAddContent1" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/page_add.png" />
                </td>
                <td>
                    <asp:LinkButton ID="btnAddcontent2" runat="server" Text="Add New Content" CssClass="main">
                    </asp:LinkButton>
                </td>
            </tr>
        </table>
        <asp:GridView ID="gdvContent" runat="server" AutoGenerateColumns="false" Width="100%"
            CellPadding="3" HeaderStyle-CssClass="table-header" AlternatingRowStyle-CssClass="table-altrow"
            RowStyle-CssClass="table-row">
            <Columns>
                <asp:BoundField DataField="VersionNotes" HeaderText="Name" HeaderStyle-HorizontalAlign="left" />
                <asp:TemplateField HeaderText="Display Pages" ItemStyle-Width="200">
                    <ItemTemplate>
                        <asp:Literal ID="ltrPageModules" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50">
                    <ItemTemplate>
                        <table>
                            <tr>
                                <td width="20">
                                    <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/page_edit.png"
                                        OnClick="EditContent" ToolTip="Edit Content" />
                                </td>
                                <td width="20">
                                    <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/delete.png"
                                        OnClientClick="return deleteContent();" OnClick="DeleteContent" ToolTip="Delete Content" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnlEditContent" runat="server" Visible="false">
        <table cellpadding="5" cellspacing="0" border="0">
            <tr>
                <td align="right">
                    <asp:Label ID="lblContentName" runat="server" CssClass="form-label" Text="Name:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtContentName" runat="server" CssClass="form-textbox" Width="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top">
                    <asp:Label ID="Label4" runat="server" CssClass="form-label" Text="Content:"></asp:Label>
                </td>
                <td>
                    <EzEdit:ContentEditor EditorWidth="600" EditorHeight="600" ID="txtContent" runat="server" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <VAM:Button ID="btnSaveContent" runat="server" CssClass="button" Text="Save" />
                    <asp:Button ID="btnCancelContent" runat="server" CssClass="button" Text="Cancel"
                        CausesValidation="False" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnContentID" runat="server" />
        <VAM:RequiredTextValidator ID="Validator1" runat="server" ControlIDToEvaluate="txtContentName"
            SummaryErrorMessage="Name" />
    </asp:Panel>
</asp:Content>
