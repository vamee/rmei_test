<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="Ezedit_Modules_Redirects_Default" Title="Redirects"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <asp:Label ID="lblPageTitle" runat="server" CssClass="pageTitle" />
    <br />
    <asp:Label ID="lblAlert" runat="server" CssClass="alert" EnableViewState="false" />
    <VAM:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="The following fields are required:"
        CssClass="alert" />
    <hr />
    
    <asp:Panel ID="pnlRedirects" runat="server" Visible="false">

        <script type="text/javascript" language="javascript">
        function deleteRedirect()
        {
            if (confirm("Are you sure you want to delete this redirect?")==true)
                return true;
            else
                return false;
        }
        
        function confirmEdit() {
            if (confirm("This redirect is critically important to the integrity of the website. It should not be modified unless you know exactly what you are doing.\r\rAre you sure you want to edit this redirect?")) {
                return true;
            }else{
                return false;
            }
        }
        </script>

        <table cellpadding="2" width="100%">
            <tr>
                <td width="15">
                    <asp:ImageButton ID="btnAddRedirect2" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/page_add.png" />
                </td>
                <td>
                    <asp:LinkButton ID="btnAddRedirect" runat="server" Text="Add New Redirect" CssClass="main"></asp:LinkButton>
                </td>
                <td align="right">
                    <span class="form-label">Redirect Type: </span>
                    <asp:DropDownList ID="ddlRedirectType" runat="server" AutoPostBack="true" DataSourceID="dataRedirectTypes" DataTextField="RedirectType" DataValueField="RedirectTypeID">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <asp:GridView ID="gdvRedirects" runat="server" AutoGenerateColumns="false" Width="100%"
            CellPadding="3" HeaderStyle-CssClass="table-header" AlternatingRowStyle-CssClass="table-altrow"
            RowStyle-CssClass="table-row" DataSourceID="dataRedirects" AllowPaging="true" AllowSorting="true" PageSize="20">
            <Columns>
                <asp:TemplateField ItemStyle-Width="25" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Hyperlink ID="imgIsCore" runat="server" ImageUrl="/App_Themes/EzEdit/Images/exclamation.png" Visible="false" ToolTip="This redirect is critically important to the integrity of the website. It should not be modified unless you know exactly what you are doing." />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="RedirectType" HeaderText="Type" HeaderStyle-HorizontalAlign="center"
                    ItemStyle-HorizontalAlign="center" ItemStyle-Width="75" />
                <asp:BoundField DataField="RequestedUrl" HeaderText="Request Url" HeaderStyle-HorizontalAlign="left" SortExpression="RequestedUrl" />
                <asp:BoundField DataField="RedirectUrl" HeaderText="Redirect Url" HeaderStyle-HorizontalAlign="left" SortExpression="RedirectUrl" />
                <asp:TemplateField HeaderText="Sort" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50" SortExpression="SortOrder">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlSortOrder" runat="server" OnSelectedIndexChanged="UpdateSortOrder"
                            AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:HiddenField ID="hdnSortOrder" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Enabled" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40" SortExpression="IsEnabled">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbxIsEnabled" runat="server" Checked='<%# Eval("IsEnabled") %>'
                            OnCheckedChanged="cbxIsEnabled_CheckChanged" AutoPostBack="true" />
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50">
                    <ItemTemplate>
                        <table>
                            <tr>
                                <td width="20">
                                    <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/edit.png"
                                        OnClick="EditRedirect" ToolTip="Edit Redirect" />
                                </td>
                                <td width="20">
                                    <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/delete.png"
                                        OnClientClick="return deleteRedirect();" OnClick="DeleteRecord" ToolTip="Delete Redirect" />
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField ID="hdnRedirectID" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dataRedirectTypes" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
            SelectCommand="SELECT * FROM RedirectTypes ORDER BY RedirectTypeID">
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="dataRedirects" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
            SelectCommand="SELECT * FROM qryRedirects WHERE RedirectTypeID = @RedirectTypeID ORDER BY SortOrder">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlRedirectType" Name="RedirectTypeID" PropertyName="SelectedValue" DefaultValue="0" />
            </SelectParameters>
        </asp:SqlDataSource>
    </asp:Panel>
    <asp:Panel ID="pnlEditRedirect" runat="server" Visible="false">
        <table cellpadding="5" cellspacing="0" border="0">
            <tr>
                <td align="right">
                    <asp:Label ID="lblRedirectType" runat="server" CssClass="form-label" Text="Type:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlRedirectTypeID" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="<- Please Choose ->" Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblRequestedUrl" runat="server" CssClass="form-label" Text="Requested Url:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRequestedUrl" runat="server" CssClass="form-textbox" Width="400"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblRedirectUrl" runat="server" CssClass="form-label" Text="Redirect Url:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRedirectUrl" runat="server" CssClass="form-textbox" Width="400"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top">
                    <asp:Label ID="lblComments" runat="server" CssClass="form-label" Text="Comments:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtComments" runat="server" CssClass="form-textbox" Width="400"
                        Height="50" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr runat="server" id="trIsCore">
                <td align="right" valign="top">
                    <asp:Label ID="lblIsEnabled" runat="server" CssClass="form-label" Text="Is Critical?:"></asp:Label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblIsCore" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td align="right">
                </td>
                <td>
                    <VAM:Button ID="btnSaveRedirect" runat="server" CssClass="button" Text="Save" />
                    <asp:Button ID="btnCancelRedirect" runat="server" CssClass="button" Text="Cancel"
                        CausesValidation="False" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnRedirectID" runat="server" />
        <VAM:RequiredListValidator runat="server" ControlIDToEvaluate="ddlRedirectTypeID"
            SummaryErrorMessage="Redirect Type" UnassignedIndex="0" />
        <VAM:RequiredTextValidator runat="server" ControlIDToEvaluate="txtRequestedUrl" SummaryErrorMessage="Requested Url"
            IgnoreBlankText="False" />
    </asp:Panel>
</asp:Content>
