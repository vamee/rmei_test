<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="Ezedit_Admin_AppVariables_Default" Title="Application Variables" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <p class="pagetitle">
        Admin > Application Variables</p>
    <asp:Label ID="lblAlert" runat="server" CssClass="alert" />
    <VAM:ValidationSummary ID="ValidationSummary1" runat="server" HeaderCssClass="alert"
        HeaderText="The following fields are required:" CssClass="alert" AutoUpdate="true" />
    <hr />
    <asp:Panel ID="pnlList" runat="server" Visible="true">

        <script type="text/javascript" language="javascript">
        function confirmDeleteItem()
        {
            if (confirm("Are you sure you want to delete this variable?")==true)
                return true;
            else
                return false;
        } 
        </script>

        <table cellpadding="0" cellspacing="0" border="0" id="table1">
            <tr>
                <td>
                    <asp:ImageButton ID="ibtnAddNew" SkinID="IconAddNew" runat="server" ToolTip="Add New Variable"
                        ImageAlign="Left" />
                </td>
                <td>
                    <asp:LinkButton ID="lbtnAddNew" runat="server" Text="Add New Variable" CssClass="main" />
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="gdvList" runat="server" AutoGenerateColumns="False" Width="100%"
            CellPadding="3" HeaderStyle-CssClass="table-header" HeaderStyle-HorizontalAlign="Left" AlternatingRowStyle-CssClass="table-altrow"
            RowStyle-CssClass="table-row" DataSourceID="dataAppVariables">
            <RowStyle CssClass="table-row" />
            <Columns>
            <asp:BoundField DataField="VariableName" HeaderText="Name" />
            <asp:BoundField DataField="VariableValue" HeaderText="Value" />
            <asp:BoundField DataField="Description" HeaderText="Description" />
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50" ItemStyle-VerticalAlign="Top">
                    <ItemTemplate>
                        <table>
                            <tr>
                                <td width="20">
                                    <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/edit.png"
                                        OnClick="btnEdit_Click" ToolTip="Edit Variable" CommandArgument='<%#Eval("VariableID") %>' />
                                </td>
                                <td width="20">
                                    <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/delete.png"
                                        OnClientClick="return confirmDeleteItem();" OnClick="btnDelete_Click" ToolTip="Delete Variable" CommandArgument='<%#Eval("VariableID") %>' />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dataAppVariables" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
            SelectCommand="SELECT * FROM [ApplicationVariables] ORDER BY VariableName" />
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
        <table cellpadding="3" cellspacing="0">
            <tr>
                <td>
                    <asp:Label runat="server" CssClass="form-label" Text="Variable Name:" />
                </td>
                <td>
                    <asp:TextBox ID="txtVariableName" runat="server" Width="300" CssClass="form-textbox" />
                    <VAM:RequiredTextValidator runat="server" ControlIDToEvaluate="txtVariableName" SummaryErrorMessage="Variable Name" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" CssClass="form-label" Text="Variable Value:" />
                </td>
                <td>
                    <asp:TextBox ID="txtVariableValue" runat="server" Width="300" CssClass="form-textbox" />
                    <VAM:RequiredTextValidator ID="RequiredTextValidator1" runat="server" ControlIDToEvaluate="txtVariableValue" SummaryErrorMessage="Variable Value" />
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="Label1" runat="server" CssClass="form-label" Text="Description:" />
                </td>
                <td>
                    <asp:TextBox ID="txtDescription" runat="server" Width="300" CssClass="form-textbox"
                        TextMode="MultiLine" Height="50" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <VAM:Button ID="btnSave" runat="server" CssClass="form-button" Text="Save" />
                    <asp:Button ID="btnCancel" runat="server" CssClass="form-button" Text="Cancel" />
                    <asp:HiddenField ID="hdnItemID" runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
