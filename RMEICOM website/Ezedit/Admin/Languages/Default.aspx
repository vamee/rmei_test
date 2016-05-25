<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="Ezedit_Admin_Languages_Default" Title="EzEdit - Languages" %>

<%@ Register TagPrefix="vs" Namespace="Vladsm.Web.UI.WebControls" Assembly="GroupRadioButton" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <p class="pagetitle">
        Admin > Languages</p>
    <asp:Label ID="lblAlert" runat="server" CssClass="alert" />
    <VAM:ValidationSummary ID="ValidationSummary1" runat="server" HeaderCssClass="alert"
        HeaderText="The following fields are required:" CssClass="alert" AutoUpdate="true" />
    <hr />
    <asp:Panel ID="pnlList" runat="server" Visible="true">

        <script type="text/javascript" language="javascript">
        function confirmDeleteItem()
        {
            if (confirm("Are you sure you want to delete this language?")==true)
                return true;
            else
                return false;
        } 
        </script>

        <table cellpadding="0" cellspacing="0" border="0" id="table1">
            <tr>
                <td>
                    <asp:ImageButton ID="ibtnAddNew" SkinID="IconAddNew" runat="server" ToolTip="Add New Language"
                        ImageAlign="Left" />
                </td>
                <td>
                    <asp:LinkButton ID="lbtnAddNew" runat="server" Text="Add New Language" CssClass="main" />
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="gdvList" runat="server" AutoGenerateColumns="False" Width="100%"
            CellPadding="3" HeaderStyle-CssClass="table-header" AlternatingRowStyle-CssClass="table-altrow"
            RowStyle-CssClass="table-row" DataSourceID="dataLanguages">
            <RowStyle CssClass="table-row" />
            <Columns>
                <asp:BoundField DataField="LanguageName" HeaderText="Language Name" HeaderStyle-HorizontalAlign="left"
                    ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Top" />
                <asp:TemplateField HeaderText="Domain(s)" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Top">
                    <ItemTemplate>
                        <asp:BulletedList ID="lstDomains" runat="server">
                        </asp:BulletedList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Default" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"
                    ItemStyle-Width="50" ItemStyle-VerticalAlign="Top">
                    <ItemTemplate>
                        <vs:GroupRadioButton ID="rdoIsDefault" GroupName="IsDefault" runat="server" autopostback="True"
                            OnCheckedChanged="rdoIsDefault_CheckChanged" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Enabled" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"
                    ItemStyle-Width="50" ItemStyle-VerticalAlign="Top">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbxIsEnabled" runat="server" AutoPostBack="true" OnCheckedChanged="cbxIsEnabled_CheckChanged" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100" ItemStyle-VerticalAlign="Top">
                    <ItemTemplate>
                        <table>
                            <tr>
                                <td width="20">
                                    <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/edit.png"
                                        OnClick="btnEdit_Click" ToolTip="Edit Language" />
                                </td>
                                <td width="20">
                                    <asp:ImageButton ID="btnCopy" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/icon_copy.gif"
                                        OnClick="btnCopy_Click" ToolTip="Copy Language" />
                                </td>
                                <td width="20">
                                    <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/delete.png"
                                        OnClientClick="return confirmDeleteItem();" OnClick="btnDelete_Click" ToolTip="Delete Language" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="table-header" />
            <AlternatingRowStyle CssClass="table-altrow" />
        </asp:GridView>
        <asp:SqlDataSource ID="dataLanguages" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
            SelectCommand="SELECT * FROM [Languages]"></asp:SqlDataSource>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
        <table cellpadding="3" cellspacing="0" border="0">
            <tr>
                <td>
                    <asp:Label ID="lblLanguageName" runat="server" CssClass="form-label" Text="Language Name:" />
                </td>
                <td>
                    <asp:TextBox ID="txtLanguageName" runat="server" Width="300" />
                    <VAM:RequiredTextValidator runat="server" ControlIDToEvaluate="txtLanguageName" SummaryErrorMessage="Language Name" />
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="Label1" runat="server" CssClass="form-label" Text="Domains:" />
                </td>
                <td>
                    <asp:Table ID="tblDomains" runat="server" CellPadding="3" CellSpacing="0" BorderWidth="1">
                        <asp:TableHeaderRow CssClass="table-header">
                            <asp:TableHeaderCell HorizontalAlign="Left">Domain Name</asp:TableHeaderCell>
                            <asp:TableHeaderCell HorizontalAlign="Center">Enabled</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                </td>
                <td>
                    <VAM:Button ID="btnSave" runat="server" CssClass="form-button" Text="Save" />
                    <asp:Button ID="btnCancel" runat="server" CssClass="form-button" Text="Cancel" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnLanguageID" runat="server" />
        <asp:HiddenField ID="hdnCopyLanguageID" runat="server" />
    </asp:Panel>
</asp:Content>
