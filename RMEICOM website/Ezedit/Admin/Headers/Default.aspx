<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="Ezedit_Admin_Headers_Default" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">

    <script language="javascript" type="text/javascript">
function confirmDelete() {
    if (confirm("Are you sure you want to delete this header?")) {
        return true;
    }else{
        return false;
    }
}
    </script>

    <asp:Label ID="lblPageTitle" runat="server" CssClass="pageTitle" Text="Template Header Locations" />
    <br />
    <asp:Label ID="lblAlert" runat="server" CssClass="message" EnableViewState="false"></asp:Label>
    <VAM:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="The following fields are required:"
        CssClass="alert" />
    <hr />
    <br />
    <asp:Panel ID="pnlItemList" runat="server">
    <table cellpadding="2">
            <tr>
                <td>
                    <asp:ImageButton ID="btnAdd1" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/image_add.png" /></td>
                <td>
                    <asp:LinkButton ID="btnAdd2" runat="server" Text="Add New Location" CssClass="main"></asp:LinkButton></td>
            </tr>
        </table>
    
    
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="HeaderID"
        DataSourceID="SqlDataSource1" Width="100%" CellPadding="3" HeaderStyle-CssClass="table-header"
        AlternatingRowStyle-CssClass="table-altrow" RowStyle-CssClass="table-row" EmptyDataRowStyle-CssClass="table-row">
        <EmptyDataTemplate>
            No Data
        </EmptyDataTemplate>
        <Columns>
            <asp:BoundField DataField="HeaderID" HeaderText="HeaderID" InsertVisible="False"
                ReadOnly="True" SortExpression="HeaderID" Visible="False" />
            <asp:BoundField DataField="HeaderName" HeaderText="Name" SortExpression="HeaderName" />
            <asp:BoundField DataField="FriendlyName" HeaderText="Friendly Name" SortExpression="FriendlyName">
                <ControlStyle Width="150px" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="Help Text" SortExpression="Description">
                <ControlStyle Width="300px" />
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="SortOrder" HeaderText="Sort" SortExpression="SortOrder">
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:TemplateField ShowHeader="False">
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="False" CommandName="Select"
                        ImageUrl="~/App_Themes/EzEdit/Images/edit.png" />
                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                        ImageUrl="~/App_Themes/EzEdit/Images/delete.png" OnClientClick="return confirmDelete()" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <RowStyle CssClass="table-row" />
        <HeaderStyle CssClass="table-header" />
        <AlternatingRowStyle CssClass="table-altrow" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
        SelectCommand="SELECT * FROM [TemplateHeaders] ORDER BY [SortOrder]" ConflictDetection="CompareAllValues"
        DeleteCommand="DELETE FROM [TemplateHeaders] WHERE [HeaderID] = @original_HeaderID AND [HeaderName] = @original_HeaderName AND [FriendlyName] = @original_FriendlyName AND [Description] = @original_Description AND [SortOrder] = @original_SortOrder"
        OldValuesParameterFormatString="original_{0}">
        <DeleteParameters>
            <asp:Parameter Name="original_HeaderID" Type="Int32" />
            <asp:Parameter Name="original_HeaderName" Type="String" />
            <asp:Parameter Name="original_FriendlyName" Type="String" />
            <asp:Parameter Name="original_Description" Type="String" />
            <asp:Parameter Name="original_SortOrder" Type="Int32" />
        </DeleteParameters>
    </asp:SqlDataSource>
    
    </asp:Panel>
    <asp:DetailsView AutoGenerateRows="False" DataKeyNames="HeaderID" DataSourceID="SqlDataSource2"
        HeaderText=" Edit Location" ID="DetailsView1" runat="server" Width="275px" Visible="False"
        CellPadding="3" HeaderStyle-CssClass="table-header" AlternatingRowStyle-CssClass="table-altrow"
        RowStyle-CssClass="table-row" OnItemUpdating="ValidateUpdateData" OnItemInserting="ValidateInsertData">
       
        <Fields>
            <asp:BoundField DataField="HeaderID" HeaderText="ID" ReadOnly="True" SortExpression="HeaderID"
                Visible="False" />
            <asp:BoundField DataField="HeaderName" HeaderText="Location Name" SortExpression="HeaderName">
                <HeaderStyle Wrap="false" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="FriendlyName" HeaderText="Friendly Name" SortExpression="FriendlyName">
            <HeaderStyle Wrap="false" VerticalAlign="Top" />
            <ControlStyle Width="200" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="Help Text" SortExpression="Description">
                <HeaderStyle Wrap="false" VerticalAlign="Top" />
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Description") %>' TextMode="MultiLine" Width="300" Height="50"></asp:TextBox>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Description") %>' TextMode="MultiLine" Width="300" Height="50"></asp:TextBox>
                </InsertItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="SortOrder" HeaderText="Sort" SortExpression="SortOrder">
                <HeaderStyle Wrap="false" VerticalAlign="Top" />
                <ControlStyle Width="50" />
            </asp:BoundField>
            
            <asp:TemplateField ShowHeader="False" ItemStyle-HorizontalAlign="Center">
                <EditItemTemplate>
                    <asp:Button ID="btnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                        Text="Update" CssClass="form-button" />
                    <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                        Text="Cancel" CssClass="form-button" />
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:Button ID="btnInsert" runat="server" CausesValidation="True" CommandName="Insert"
                        Text="Insert" CssClass="form-button" />
                    <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                        Text="Cancel" CssClass="form-button" />
                </InsertItemTemplate>
                
            </asp:TemplateField>
        </Fields>
        <RowStyle CssClass="table-row" />
        <HeaderStyle CssClass="table-header" />
        <AlternatingRowStyle CssClass="table-altrow" />
    </asp:DetailsView>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
        SelectCommand="SELECT * FROM [TemplateHeaders] WHERE ([HeaderID] = @HeaderID)"
        ConflictDetection="CompareAllValues" InsertCommand="INSERT INTO [TemplateHeaders] ([HeaderName], [FriendlyName], [Description], [SortOrder]) VALUES (@HeaderName, @FriendlyName, @Description, @SortOrder)"
        OldValuesParameterFormatString="original_{0}" UpdateCommand="UPDATE [TemplateHeaders] SET [HeaderName] = @HeaderName, [FriendlyName] = @FriendlyName, [Description] = @Description, [SortOrder] = @SortOrder WHERE [HeaderID] = @original_HeaderID AND [HeaderName] = @original_HeaderName AND [FriendlyName] = @original_FriendlyName AND [Description] = @original_Description AND [SortOrder] = @original_SortOrder"
        DeleteCommand="DELETE FROM [TemplateHeaders] WHERE [HeaderID] = @original_HeaderID AND [HeaderName] = @original_HeaderName AND [FriendlyName] = @original_FriendlyName AND [Description] = @original_Description AND [SortOrder] = @original_SortOrder">
        <UpdateParameters>
            <asp:Parameter Name="HeaderName" Type="String" />
            <asp:Parameter Name="FriendlyName" Type="String" />
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="SortOrder" Type="Int32" />
            <asp:Parameter Name="original_HeaderID" Type="Int32" />
            <asp:Parameter Name="original_HeaderName" Type="String" />
            <asp:Parameter Name="original_FriendlyName" Type="String" />
            <asp:Parameter Name="original_Description" Type="String" />
            <asp:Parameter Name="original_SortOrder" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="HeaderName" Type="String" />
            <asp:Parameter Name="FriendlyName" Type="String" />
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="SortOrder" Type="Int32" />
        </InsertParameters>
        <DeleteParameters>
            <asp:Parameter Name="original_HeaderID" Type="Int32" />
            <asp:Parameter Name="original_HeaderName" Type="String" />
            <asp:Parameter Name="original_FriendlyName" Type="String" />
            <asp:Parameter Name="original_Description" Type="String" />
            <asp:Parameter Name="original_SortOrder" Type="Int32" />
        </DeleteParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="GridView1" DefaultValue="0" Name="HeaderID" PropertyName="SelectedValue"
                Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
