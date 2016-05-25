<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="BuildForm.aspx.vb" Inherits="Ezedit_Modules_Forms01_Default" EnableViewState="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">

    <script type="text/javascript" language="javascript">
        function confirmDelete() {
            if (confirm("Are you sure you want to delete this field?")) {
                return true;
            }else{
                return false;
            }
        }
        
        
    </script>

    <asp:Label ID="lblPageTitle" runat="server" CssClass="pageTitle" /><br />
    <asp:Label ID="lblAlert" runat="server" CssClass="alert" />
    <hr />
    <table cellpadding="0" cellspacing="0" border="0" id="table1">
        <tr>
            <td>
                <asp:LinkButton ID="lbtnAddNew" runat="server" CssClass="main"><img src="/App_Themes/EzEdit/Images/application_form_add.png" border="0" />&nbsp;Add New Form Field</asp:LinkButton>
            </td>
        </tr>
    </table>
    <br />
    <asp:GridView ID="gdvList" runat="server" AutoGenerateColumns="False" 
        Width="100%" SkinID="GridView" RowStyle-VerticalAlign="Top" AllowSorting="true"
        ShowHeader="false" AllowPaging="true" PageSize="25" PagerSettings-Mode="NumericFirstLast">
        <Columns>
            <asp:BoundField DataField="Label" ItemStyle-HorizontalAlign="Right" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:PlaceHolder ID="plcFormField" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Sort" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40"
                HeaderStyle-HorizontalAlign="Center" SortExpression="SortOrder">
                <ItemTemplate>
                    <asp:DropDownList ID="ddlSortOrder" runat="server" OnSelectedIndexChanged="ddlSortOrder_SelectedIndexChanged"
                        AutoPostBack="true" />
                    <asp:HiddenField ID="hdnSortOrder" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <table>
                            <tr>
                                <td width="20">
                                    <asp:ImageButton ID="btnEdit" runat="server" OnClick="btnEdit_Click"
                                        ImageUrl="~/App_Themes/EzEdit/Images/pencil.png" CommandArgument='<%# Eval("FieldID") %>'
                                        ToolTip="Edit" />
                                </td>
                                <td width="20">
                                    <asp:ImageButton ID="btnDelete" runat="server" OnClick="btnDelete_Click"
                                        ImageUrl="~/App_Themes/EzEdit/Images/delete.png" OnClientClick="return confirmDelete()"
                                        CommandArgument='<%# Eval("FieldID") %>' ToolTip="Delete" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="dataFormFields" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
        SelectCommand="SELECT * FROM qryFormFields WHERE FormID = @FormID ORDER BY SortOrder">
        <SelectParameters>
            <asp:QueryStringParameter Name="FormID" QueryStringField="FormID" Type="Int32" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>
    <%--<asp:Repeater ID="rptFields" runat="Server" EnableViewState="False">
        <HeaderTemplate>
            <table cellpadding="0" cellspacing="0" border="1" bordercolor="#999999">
                <tr>
                    <td>
                        <table border="0" width="100%" cellpadding="3" cellspacing="1">
                            <tr bgcolor="#7288AD">
                                <td class="table_header" colspan="4">
                                    <asp:PlaceHolder ID="plcTableHeader" runat="server"></asp:PlaceHolder>
                                </td>
                            </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr id="trForms" runat="server">
                <td class="table_text" valign="top" id="tdFieldLabel" runat="server">
                    <%#Eval("Label")%>
                </td>
                <td class="table_text" valign="top" id="tdFormField">
                    <asp:PlaceHolder ID="plcFormField" runat="server"></asp:PlaceHolder>
                </td>
                <td align="center" valign="top">
                    <asp:DropDownList ID="ddlSortOrder" runat="server" CssClass="form">
                    </asp:DropDownList>
                </td>
                <td valign="top">
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="center" width="50%">
                                <asp:PlaceHolder ID="plcEditField" runat="server"></asp:PlaceHolder>
                            </td>
                            <td align="center" width="50%">
                                <asp:PlaceHolder ID="plcDeleteField" runat="server"></asp:PlaceHolder>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            <tr bgcolor="#7288AD">
                <td colspan="5">
                    &nbsp;
                </td>
            </tr>
            </table> </td> </tr> </table>
        </FooterTemplate>
    </asp:Repeater>--%>
    <asp:Button ID="btnSave" runat="server" Text=" Save " Visible="false" />
</asp:Content>
