<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="Ezedit_Admin_Default"
    MasterPageFile="~/Ezedit/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <p class="pagetitle">Admin > EzEdit Users</p>
    <asp:Label ID="lblAlert" runat="server" CssClass="alert" />
    <VAM:ValidationSummary runat="server" CssClass="alert" HeaderText="The following fields are required:" AutoUpdate="true" ScrollIntoView="Top" />
    <hr />
    <asp:Panel ID="pnlList" runat="server">

        <script type="text/javascript" language="javascript">
        function confirmDelete()
        {
            if (confirm("Are you sure you want to delete this user?")) {
                return true;
            }else{
                return false;
            }
        } 
        </script>

        <table cellpadding="0" cellspacing="0" border="0" id="table1">
            <tr>
                <td>
                    <asp:LinkButton ID="lbtnAddNew" runat="server" CssClass="main"><img src="/App_Themes/EzEdit/Images/user_add.png" border="0" />&nbsp;Add New User</asp:LinkButton>
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="gdvList" runat="server" AutoGenerateColumns="False" Width="100%"
            CellPadding="3" HeaderStyle-CssClass="table-header" HeaderStyle-HorizontalAlign="Left"
            AlternatingRowStyle-CssClass="table-altrow" RowStyle-CssClass="table-row" DataSourceID="dataUsers"
            AllowSorting="true" AllowPaging="true" PageSize="25" PagerSettings-Mode="NextPreviousFirstLast">
            <Columns>
                <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" />
                <asp:BoundField DataField="FirstLogin" HeaderText="First Login" SortExpression="FirstLogin"
                    ItemStyle-Width="125" />
                <asp:BoundField DataField="LastLogin" HeaderText="Last Login" SortExpression="LastLogin"
                    ItemStyle-Width="125" />
                <asp:BoundField DataField="LoginCount" HeaderText="# Logins" SortExpression="LoginCount"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="50" />
                <asp:BoundField DataField="EzEditLevel" HeaderText="Type" SortExpression="EzEditLevel"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50" />
                <asp:TemplateField HeaderText="Enabled" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40"
                    SortExpression="IsEnabled">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbxIsEnabled" runat="server" Checked='<%# Eval("IsEnabled") %>'
                            OnCheckedChanged="cbxIsEnabled_CheckChanged" AutoPostBack="true" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <table>
                            <tr>
                                <td width="20">
                                    <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="False" OnClick="btnEdit_Click"
                                        ImageUrl="~/App_Themes/EzEdit/Images/edit.png" CommandArgument='<%# Eval("UserID") %>'
                                        ToolTip="Edit User" />
                                </td>
                                <td width="20">
                                    <asp:ImageButton ID="btnPermissions" runat="server" CausesValidation="False" OnClick="btnPermissions_Click"
                                        ImageUrl="~/App_Themes/EzEdit/Images/page_key.png" CommandArgument='<%# Eval("UserID") %>'
                                        ToolTip="Edit Page Permissions" />
                                </td>
                                <td width="20">
                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" OnClick="btnDelete_Click"
                                        ImageUrl="~/App_Themes/EzEdit/Images/delete.png" OnClientClick="return confirmDelete()"
                                        CommandArgument='<%# Eval("UserID") %>' ToolTip="Delete User" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dataUsers" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
            SelectCommand="SELECT * FROM [qryEzEditUsers] WHERE LanguageID = @LanguageID ORDER BY Username">
            <SelectParameters>
                <asp:SessionParameter Name="LanguageID" SessionField="EzEditLanguageID" Type="Int32"
                    DefaultValue="0" />
            </SelectParameters>
        </asp:SqlDataSource>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
        <table border="0" width="100%" cellpadding="5" cellspacing="0" id="table2">
            <tr>
                <td class="form-label" nowrap>
                    Website:
                    <VAM:RequiredListValidator runat="server" ControlIDToEvaluate="ddlLanguageID" SummaryErrorMessage="Website"
                        ErrorMessage="*" UnassignedIndex="0" />
                </td>
                <td width="100%">
                    <asp:DropDownList ID="ddlLanguageID" runat="server" AppendDataBoundItems="true" CssClass="form-dropdown">
                        <asp:ListItem Text="<- Please Choose -->" Value="-1" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="form-label" nowrap>
                    User Type:
                    <VAM:RequiredListValidator ID="RequiredListValidator1" runat="server" ControlIDToEvaluate="ddlEzEditLevelID"
                        SummaryErrorMessage="User Type" ErrorMessage="*" UnassignedIndex="0" />
                </td>
                <td width="100%">
                    <asp:DropDownList ID="ddlEzEditLevelID" runat="server" AppendDataBoundItems="true"
                        CssClass="form-dropdown">
                        <asp:ListItem Text="<- Please Choose -->" Value="-1" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="form-label" nowrap>
                    First Name:
                    <VAM:RequiredTextValidator runat="server" ControlIDToEvaluate="txtFirstName" SummaryErrorMessage="First Name"
                        ErrorMessage="*" />
                </td>
                <td width="100%">
                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-textbox" Width="250" />
                </td>
            </tr>
            <tr>
                <td class="form-label" nowrap>
                    Last Name:
                    <VAM:RequiredTextValidator ID="RequiredTextValidator1" runat="server" ControlIDToEvaluate="txtLastName"
                        SummaryErrorMessage="Last Name" ErrorMessage="*" />
                </td>
                <td width="100%">
                    <asp:TextBox ID="txtLastName" runat="server" CssClass="form-textbox" Width="250" />
                </td>
            </tr>
            <tr>
                <td class="form-label" nowrap>
                    Email:
                    <VAM:EmailAddressValidator ID="RequiredTextValidator2" runat="server" ControlIDToEvaluate="txtEmail"
                        SummaryErrorMessage="Email" ErrorMessage="*" IgnoreBlankText="false" />
                </td>
                <td width="100%">
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-textbox" Width="250" />
                </td>
            </tr>
            <tr>
                <td class="form-label" nowrap>
                    Username:
                    <VAM:RequiredTextValidator runat="server" ControlIDToEvaluate="txtUsername" SummaryErrorMessage="Username"
                        ErrorMessage="*" />
                    <VAM:CustomValidator ID="csvUsername" runat="server" EventsThatValidate="OnSubmit"
                        SummaryErrorMessage="Username already in use." ControlIDToEvaluate="txtUsername" />
                </td>
                <td width="100%">
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-textbox" Width="250" />
                </td>
            </tr>
            <tr>
                <td class="form-label" nowrap>
                    Password:
                    <VAM:RequiredTextValidator runat="server" ControlIDToEvaluate="txtPassword" SummaryErrorMessage="Password"
                        ErrorMessage="*" />
                </td>
                <td width="100%">
                    <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" CssClass="form-textbox"
                        Width="250" />
                </td>
            </tr>
            <tr>
                <td class="form-label" nowrap>
                    Password Confirm:
                    <VAM:RequiredTextValidator runat="server" ControlIDToEvaluate="txtPasswordConfirm"
                        SummaryErrorMessage="Confirm Password" ErrorMessage="*" />
                    <VAM:CompareTwoFieldsValidator runat="server" ControlIDToEvaluate="txtPassword" SecondControlIDToEvaluate="txtPasswordConfirm" SummaryErrorMessage="Passwords do not match" ErrorMessage="*" Operator="Equal" NotCondition="false" />
                </td>
                <td width="100%">
                    <asp:TextBox ID="txtPasswordConfirm" TextMode="Password" runat="server" CssClass="form-textbox"
                        Width="250" />
                </td>
            </tr>
            <tr>
                <td class="form-label" nowrap>
                    Enabled:
                </td>
                <td>
                    <asp:RadioButtonList ID="rblIsEnabled" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Yes" Value="True" />
                        <asp:ListItem Text="No" Value="False" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <VAM:Button ID="btnSave" runat="server" Text="Save" CssClass="form-button" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                        CssClass="form-button" />
                    <asp:HiddenField ID="hdnUserID" runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
