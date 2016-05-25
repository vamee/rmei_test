<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false" CodeFile="ItemList.aspx.vb" Inherits="Ezedit_Modules_Members_Default" title="Membership" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    
    
<asp:Label ID="lblPageTitle" runat="server" CssClass="pageTitle" />
    <br />
    <asp:Label ID="lblAlert" runat="server" CssClass="message" EnableViewState="false"></asp:Label>
    <VAM:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="The following fields are required:"
        CssClass="alert" AutoUpdate="true" />
    <hr />
    <br />
    <asp:Panel ID="pnlMembers" runat="server" Visible="false">
        <script type="text/javascript" language="javascript">
        function deleteRecord()
        {
            if (confirm("Are you sure you want to delete this member?")==true)
                return true;
            else
                return false;
        } 
        </script>

        <table cellpadding="2">
            <tr>
                <td>
                    <asp:ImageButton ID="btnAddMember2" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/user_add.png" /></td>
                <td>
                    <asp:LinkButton ID="btnAddmember" runat="server" Text="Add New Member" CssClass="main"></asp:LinkButton></td>
            </tr>
        </table>
        <asp:GridView ID="gdvMembers" runat="server" AutoGenerateColumns="False" Width="100%"
            CellPadding="3" HeaderStyle-CssClass="table-header" AlternatingRowStyle-CssClass="table-altrow"
            RowStyle-CssClass="table-row">
            <Columns>
                <asp:BoundField DataField="MemberName" HeaderText="Name" >
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Description" HeaderText="Description" >
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                
                <asp:TemplateField ItemStyle-Width="50">
                    <ItemTemplate>
                        <table>
                            <tr>
                                <td width="20">
                                    <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/user_edit.png"
                                        OnClick="EditMember" ToolTip="Edit Member" />
                                </td>
                                <td width="20">
                                    <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/delete.png"
                                        OnClientClick="return deleteRecord();" OnClick="DeleteMember" ToolTip="Delete Member" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
            <RowStyle CssClass="table-row" />
            <HeaderStyle CssClass="table-header" />
            <AlternatingRowStyle CssClass="table-altrow" />
        </asp:GridView>
    </asp:Panel>
    
    <asp:Panel ID="pnlEditMember" runat="server" Visible="false">
        <table cellpadding="5" cellspacing="0" border="0">
            <tr>
                <td align="right">
                    <asp:Label ID="Label2" runat="server" CssClass="form-label" Text="Member Type:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlCategoryID" runat="server" AppendDataBoundItems="true">
                        
                    </asp:DropDownList>
                    
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblFirstName" runat="server" CssClass="form-label" Text="First Name:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-textbox" Width="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label5" runat="server" CssClass="form-label" Text="Last Name:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtLastName" runat="server" CssClass="form-textbox" Width="250"></asp:TextBox>
                </td>
            </tr>
             <tr>
                <td align="right">
                    <asp:Label ID="Label1" runat="server" CssClass="form-label" Text="Email:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-textbox" Width="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblUsername" runat="server" CssClass="form-label" Text="Username:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-textbox" Width="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblPassword" runat="server" CssClass="form-label" Text="Password:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-textbox" Width="250"></asp:TextBox>
                </td>
            </tr>
            <%--<tr>
                <td align="right">
                    <asp:Label ID="lblPasswordConfirm" runat="server" CssClass="form-label" Text="Confirm Password:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPasswordConfirm" runat="server" CssClass="form-textbox" Width="250"></asp:TextBox>
                </td>
            </tr>--%>
            <tr>
                <td align="right" valign="top">
                    <asp:Label ID="lblDescription" runat="server" CssClass="form-label" Text="Description:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-textbox" Width="250"
                        Height="50" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            
            <tr>
                <td align="right">
                </td>
                <td>
                    <VAM:Button ID="btnSave" runat="server" CssClass="button" Text="Save" />
                    <asp:Button ID="btnCancel" runat="server" CssClass="button" Text="Cancel"
                        CausesValidation="False" />
                </td>
            </tr>
        </table>
        <VAM:RequiredTextValidator ID="Validator1" runat="server" ControlIDToEvaluate="txtFirstName"
            SummaryErrorMessage="First Name" />
        <VAM:RequiredTextValidator ID="Validator2" runat="server" ControlIDToEvaluate="txtLastName"
            SummaryErrorMessage="Last Name" />
        <VAM:EmailAddressValidator ID="EmailAddressValidator1" runat="server" ControlIDToEvaluate="txtEmail" SummaryErrorMessage="Email" IgnoreBlankText="false" />
        <VAM:RequiredTextValidator ID="Validator3" runat="server" ControlIDToEvaluate="txtUsername"
            SummaryErrorMessage="Username" />
        <VAM:RequiredTextValidator ID="Validator4" runat="server" ControlIDToEvaluate="txtPassword"
            SummaryErrorMessage="Password" />
        </asp:Panel>
    
    <asp:HiddenField ID="hdnMemberID" runat="server" />
    
</asp:Content>

