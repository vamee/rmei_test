<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="ItemList.aspx.vb" Inherits="Ezedit_Modules_Events01_ItemList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">

    <script type="text/javascript" language="javascript">
        function updateSortOrder(fieldID, sortOrder) {
            window.document.forms[0].Action.value = "Sort";
            window.document.forms[0].FieldID.value = fieldID;
            window.document.forms[0].SortOrder.value = sortOrder;
            window.document.forms[0].submit();
        }
    </script>

    <input type="hidden" name="FieldID" value="" />
    <input type="hidden" name="Action" value="" />
    <input type="hidden" name="SortOrder" value="" />
    <p>
        <a href="../Default.aspx?ModuleKey=Events01" class="pageTitle">Events</a>
        <asp:Label ID="lblPageTitle" CssClass="pagetitle" runat="server" Text=""></asp:Label>
    </p>
    <asp:Label ID="lblAlert" runat="server" CssClass="message"></asp:Label>
    <hr />
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" border="0" id="table1">
                    <tr>
                        <td>
                            <a href="EditItem.aspx?CategoryID=<%= Request("CategoryId") %>&ItemId=0" class="main">
                                <asp:Image ID="Image1" SkinID="IconAddNew" runat="server" ToolTip="Add New Event"
                                    ImageAlign="Left" /></a>
                        </td>
                        <td>
                            <a href="EditItem.aspx?CategoryID=<%= Request("CategoryId") %>&ItemId=0" class="main">
                                Add New Event</a>
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right">
                &nbsp;
            </td>
        </tr>
    </table>
    <br />
       
    <asp:GridView ID="gdvList" runat="server" AutoGenerateColumns="False"
            DataSourceID="dataEvents" Width="100%" SkinID="GridView" RowStyle-VerticalAlign="Top" AllowSorting="true" AllowPaging="true" PageSize="25" PagerSettings-Mode="NumericFirstLast">
            <Columns>
                <asp:BoundField DataField="ResourceName" HeaderText="Name" />
                <asp:TemplateField HeaderText="Dates">
                    <ItemTemplate>
                        <asp:Repeater ID="rpEventDates" runat="server" OnItemDataBound="rpEventDates_ItemDataBound">
                        <ItemTemplate>
                            <asp:Label ID="lblEventDates" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:Repeater>
                    <li><a href="EditDate.aspx?EventID=<%#Eval("EventID")%>" class="main">Add New Date</a>
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
                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Enabled" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40" SortExpression="IsEnabled">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbxIsEnabled" runat="server" Checked='<%# Eval("IsEnabled") %>'
                            OnCheckedChanged="cbxIsEnabled_CheckChanged" AutoPostBack="true" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <table>
                            <tr>
                                <td width="20">
                                    <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="False" OnClick="btnEdit_Click"
                                        ImageUrl="~/App_Themes/EzEdit/Images/edit.png" CommandArgument='<%# Eval("EventID") %>'
                                        ToolTip="Edit" />
                                </td>
                                <td width="20">
                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" OnClick="btnDelete_Click"
                                        ImageUrl="~/App_Themes/EzEdit/Images/delete.png" OnClientClick="return confirmDeleteItem()"
                                        CommandArgument='<%# Eval("EventID") %>' ToolTip="Delete" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="dataEvents" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
            SelectCommand="SELECT * FROM qryEvents WHERE CategoryID = @CategoryID ORDER BY SortOrder">
            <SelectParameters>
                <asp:QueryStringParameter Name="CategoryID" QueryStringField="CategoryID" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
    <script language="javascript" type="text/jscript">
         function confirmDeleteItem() {
            if (confirm("Are you sure you want to delete this event and all its associated dates?"))
                return true;
            else
                return false;
        } 
    </script>

</asp:Content>
