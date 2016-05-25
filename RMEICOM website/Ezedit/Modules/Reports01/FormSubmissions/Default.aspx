<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="Ezedit_Modules_Reports01_FormSubmissions_Default" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<p>
        <asp:Label ID="lblPageTitle" runat="server" CssClass="pageTitle" />
    </p>
    <asp:label id="lblAlert" runat="server" cssclass="message"></asp:label>
	<hr />
	<br />
    <asp:GridView ID="gdvFormSubmissions" runat="server" DataSourceID="FormListDataSource" AlternatingRowStyle-CssClass="table-altrow" RowStyle-CssClass="table-row" AutoGenerateColumns="False" HeaderStyle-CssClass="table-header" CellPadding="3" Width="100%" >
    <Columns>
        <asp:BoundField HeaderText="Name" DataField="FormName" >
            <HeaderStyle HorizontalAlign="Left" />
        </asp:BoundField>
        <asp:BoundField HeaderText="Description" DataField="Description" >
            <HeaderStyle HorizontalAlign="Left" />
        </asp:BoundField>
        <asp:TemplateField HeaderText="# Submissions" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100">
            <ItemTemplate>
                <asp:Label ID="lblSubmissionCount" runat="server"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:ImageButton ID="btnQuery" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/icon_products.gif" OnClick="btnQuery_Click" />
                <asp:HiddenField ID="hdnFormID" runat="server" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
    </Columns>
        <RowStyle CssClass="table-row" />
        <HeaderStyle CssClass="table-header" />
        <AlternatingRowStyle CssClass="table-altrow" />
    </asp:GridView>
    
    <asp:Panel ID="pnlQueryForm" runat="server" Visible="false">
        <table cellpadding="5">
            <tr>
                <td valign="top"><asp:Label ID="lblStartDate" runat="server" CssClass="form-label" Text="Start Date:"></asp:Label></td>
                <td><Date:DateTextBox ID="txtStartDate" runat="server" CssClass="form" Width="100"></Date:DateTextBox></td>
            </tr>
            <tr>
                <td valign="top"><asp:Label ID="lblEndDate" runat="server" CssClass="form-label" Text="End Date:"></asp:Label></td>
                <td><Date:DateTextBox ID="txtEndDate" runat="server" CssClass="form" Width="100"></Date:DateTextBox></td>
            </tr>
            <tr>
                <td valign="top"><asp:Label ID="lblFields" runat="server" CssClass="form-label" Text="Fields:"></asp:Label></td>
                <td><asp:CheckBoxList ID="cblFields" runat="server" CssClass="main"></asp:CheckBoxList></td>
            </tr>
            <tr>
                <td valign="top"></td>
                <td>
                    <asp:Button ID="btnPreview" runat="server" CssClass="form" Text="Preview" /> 
                    <asp:Button ID="btnDownload" runat="server" CssClass="form" Text="Download" /> 
                    <asp:Button ID="btnCancel" runat="server" CssClass="form" Text="Cancel" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    <asp:Panel ID="pnlPreview" runat="server" Visible="false">
        <script language="javascript" type="text/javascript">
            function confirmDelete() {
                if(confirm("Are you sure you want to delete these records?")) {
                    return true;
                }else{
                    return false;
                }
            }
        </script>
        <table cellpadding="5">
            <tr>
                <td>
                    <asp:Table ID="tblPreviewData" runat="server" CellPadding="3" BorderWidth="1" CellSpacing="0" BorderStyle="Solid" BorderColor="Black" GridLines="Both"></asp:Table>            
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnDownload2" runat="server" CssClass="button" Text="Download" />
                    <asp:Button ID="btnDelete" runat="server" CssClass="button" Text="Delete Selected" OnClientClick="return confirmDelete();" />
                    <asp:Button ID="btnQuery" runat="server" CssClass="button" Text="Run Another Query" /> 
                    <asp:Button ID="btnCancel2" runat="server" CssClass="button" Text="Cancel" />
                    
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    <asp:HiddenField ID="hdnFormID" runat="server" />
    <asp:SqlDataSource ID="FormListDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>" SelectCommand="SELECT * FROM [Forms]"></asp:SqlDataSource>
</asp:Content>

