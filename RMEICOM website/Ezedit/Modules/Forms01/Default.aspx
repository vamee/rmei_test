<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="Ezedit_Modules_Forms01_Default" %>
<%@ Register src="SystemForm.ascx" tagname="SystemForm" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">

    <script type="text/javascript" language="javascript">
        function confirmDelete() {
            if (confirm("Are you sure you want to delete this form?")) {
                return true;
            }else{
                return false;
            }
        }
    </script>

    <asp:Label ID="lblPageTitle" runat="server" CssClass="pageTitle" Text="Forms" /><br />
    <asp:Label ID="lblAlert" runat="server" CssClass="message" />
    <hr />
    <asp:Panel ID="pnlList" runat="server">
        <table cellpadding="0" cellspacing="0" border="0" id="table1">
            <tr>
                <td>
                    <asp:LinkButton ID="lbtnAddNew" runat="server" CssClass="main"><img src="/App_Themes/EzEdit/Images/table_add.png"border="0" />&nbsp;Add New Form</asp:LinkButton>
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="gdvList" runat="server" AutoGenerateColumns="False" DataSourceID="dataForms"
            Width="100%" SkinID="GridView" RowStyle-VerticalAlign="Top" AllowSorting="true"
            AllowPaging="true" PageSize="25" PagerSettings-Mode="NumericFirstLast">
            <Columns>
                <asp:BoundField DataField="FormName" HeaderText="Name" SortExpression="FormName" />
                <asp:BoundField DataField="FormType" HeaderText="Type" SortExpression="FormType" />
                <asp:TemplateField HeaderText="Display Pages">
                    <ItemTemplate>
                        <asp:Repeater ID="rptDisplayPages" runat="server" OnItemDataBound="rptDisplayPages_ItemDataBound">
                            <ItemTemplate>
                                <asp:Label ID="lblDisplayPages" runat="server" />
                            </ItemTemplate>
                        </asp:Repeater>
                        <li class="main"><a href='EditModuleProperties.aspx??PageModuleID=0&FormID=<%#Eval("FormID")%>'
                            class="main">Add to Page </a></li>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False" ItemStyle-Width="110" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <table width="100" border="0" cellspacing="0">
                            <tr>
                                <td width="20" align="center">
                                    <asp:ImageButton ID="btnEditProperties" runat="server" OnClick="btnEditProperties_Click"
                                        ImageUrl="~/App_Themes/EzEdit/Images/table_gear.png" CommandArgument='<%# Eval("FormID") %>'
                                        ToolTip="Edit Form Properties" />
                                </td>
                                <td width="20" align="center">
                                    <asp:ImageButton ID="btnEditForm" runat="server" OnClick="btnEditForm_Click" ImageUrl="~/App_Themes/EzEdit/Images/pencil.png"
                                        CommandArgument='<%# Eval("FormID") %>' ToolTip="Edit Form" />
                                </td>
                                <td width="20" align="center">
                                    <asp:ImageButton ID="btnPreviewForm" runat="server" OnClick="btnPreviewForm_Click"
                                        ImageUrl="~/App_Themes/EzEdit/Images/magnifier_zoom_in.png" CommandArgument='<%# Eval("FormID") %>'
                                        ToolTip="Preview Form" />
                                </td>
                                <td width="20" align="center">
                                    <asp:ImageButton ID="btnCopyForm" runat="server" OnClick="btnCopyForm_Click" ImageUrl="~/App_Themes/EzEdit/Images/table_multiple.png"
                                        CommandArgument='<%# Eval("FormID") %>' ToolTip="Copy Form" />
                                </td>
                                <td width="20" align="center">
                                    <asp:ImageButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" ImageUrl="~/App_Themes/EzEdit/Images/delete.png"
                                        OnClientClick="return confirmDelete()" CommandArgument='<%# Eval("FormID") %>'
                                        ToolTip="Delete Form" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dataForms" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
            SelectCommand="SELECT * FROM qryForms WHERE LanguageID = @LanguageID ORDER BY FormName">
            <SelectParameters>
                <asp:SessionParameter Name="LanguageID" SessionField="EzEditLanguageID" Type="Int32"
                    DefaultValue="0" />
            </SelectParameters>
        </asp:SqlDataSource>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
        <VAM:ValidationSummary runat="server" CssClass="alert" HeaderText="The following fields are required:" AutoUpdate="true" ScrollIntoView="Top" />
        <table border="0" cellpadding="5" cellspacing="0">
            <tr>
                <td class="form-label">
                    Form Type:
                    <VAM:RequiredListValidator runat="server" ControlIDToEvaluate="ddlFormTypeID" SummaryErrorMessage="Form Type"
                        ErrorMessage="*" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlFormTypeID" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="<- Please Choose ->" Value="-1" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="form-label">
                    Form Name:
                    <VAM:RequiredTextValidator runat="server" ControlIDToEvaluate="txtFormName" SummaryErrorMessage="Form Name"
                        ErrorMessage="*" />
                </td>
                <td>
                    <asp:TextBox ID="txtFormName" runat="server" Width="250" MaxLength="100" CssClass="form-textbox" />
                </td>
            </tr>
            <tr>
                <td class="form-label" valign="top">
                    Description:
                </td>
                <td>
                    <asp:TextBox ID="txtDescription" runat="server" Width="250" Height="50" TextMode="MultiLine" MaxLength="255" CssClass="form-textbox" />
                </td>
            </tr>
            <tr>
                <td class="form-label">
                    Label Width:
                    <VAM:RequiredTextValidator runat="server" ControlIDToEvaluate="txtLabelWidth" SummaryErrorMessage="Label Width"
                        ErrorMessage="*" />
                </td>
                <td>
                    <VAM:IntegerTextBox ID="txtLabelWidth" runat="server" MaxLength="3" CssClass="form-textbox"
                        Width="50" ShowSpinner="true" />
                </td>
            </tr>
            <tr>
                <td class="form-label">
                    Display CAPTCHA?:
                </td>
                <td>
                    <asp:RadioButtonList ID="rblDisplayCaptcha" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Yes" Value="True" />
                        <asp:ListItem Text="No" Value="False" Selected="True" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <VAM:Button ID="btnSave" runat="server" Text="Save" CssClass="form-button" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="form-button" />
                    <asp:HiddenField ID="hdnItemID" runat="server" />
                    <asp:HiddenField ID="hdnAction" runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    <asp:Panel ID="pnlPreview" runat="server" Visible="false">
        <uc1:SystemForm ID="ucPreviewForm" runat="server" />
    </asp:Panel>
    
    
    
</asp:Content>
