<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="Ezedit_Admin_Templates_Default" Title="EzEdit > Templates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">

    <script language="javascript" type="text/javascript">
            function confirmDelete(recordCount) {
                if (recordCount > 0) {
                    alert("You cannot delete this template because " + recordCount + " pages are currently using it.\r\rRemove these pages or choose a different template for the pages before attempting to delete this template.");
                    return false;
                }else{
                    if (confirm("Are you sure you want to delete this template?")) {
                        return true;
                    }else{
                        return false;
                    }
                }
            }
    </script>

    <asp:Label ID="lblPageTitle" runat="server" CssClass="pageTitle" Text="Templates" />
    <br />
    <hr />
    <asp:Label ID="lblAlert" runat="server" CssClass="message" EnableViewState="false" />
    <VAM:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="The following fields are required:"
        CssClass="alert" />
    <br />
    <asp:Panel ID="pnlItemList" runat="server">
        <table cellpadding="2">
            <tr>
                <td>
                    <asp:ImageButton ID="btnAdd1" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/image_add.png" />
                </td>
                <td>
                    <asp:LinkButton ID="btnAdd2" runat="server" Text="Add New Template" CssClass="main" />
                </td>
            </tr>
        </table>
        <asp:GridView ID="gdvList" runat="server" AutoGenerateColumns="False" DataKeyNames="TemplateID"
            DataSourceID="dataTemplateData" Width="100%" CellPadding="3" HeaderStyle-CssClass="table-header"
            AlternatingRowStyle-CssClass="table-altrow" RowStyle-CssClass="table-row" EmptyDataRowStyle-CssClass="table-row"
            HeaderStyle-HorizontalAlign="Left">
            <EmptyDataTemplate>
                No Data
            </EmptyDataTemplate>
            <Columns>
                <asp:BoundField DataField="TemplateID" HeaderText="HeaderID" InsertVisible="False"
                    ReadOnly="True" SortExpression="HeaderID" Visible="False" />
                <asp:BoundField DataField="LanguageName" HeaderText="Language" SortExpression="LanguageName" />
                <asp:BoundField DataField="TemplateName" HeaderText="Name" SortExpression="TemplateName" />
                <asp:BoundField DataField="FileName" HeaderText="Filename" SortExpression="FileName" />
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="False" CommandArgument='<%#Eval("TemplateID")%>'
                            ImageUrl="~/App_Themes/EzEdit/Images/edit.png" OnClick="btnEdit_Click" />
                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandArgument='<%#Eval("TemplateID")%>'
                            ImageUrl="~/App_Themes/EzEdit/Images/delete.png" OnClick="btnDelete_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <RowStyle CssClass="table-row" />
            <HeaderStyle CssClass="table-header" HorizontalAlign="Left" />
            <AlternatingRowStyle CssClass="table-altrow" />
            <EmptyDataRowStyle CssClass="table-row" />
        </asp:GridView>
        <asp:SqlDataSource ID="dataTemplateData" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
            SelectCommand="SELECT * FROM [qryTemplates] ORDER BY LanguageName, TemplateName">
        </asp:SqlDataSource>
    </asp:Panel>

    <script type="text/javascript">
        var txtFileName;
    
            function ShowFileDialog(controlID, filePath)
            {
                txtFileName = controlID;
                window.radopen("TemplateBrowser.aspx?FilePath=" + filePath + "&ControlID=" + controlID, "FileDialog");
                return false; 
            }
            
            function refreshData(strFileName) {
                var FileName = eval("window.document.forms[0]." + txtFileName);
                
                FileName.value = strFileName;
            }
            
            function URLDecode (encodedString) {
  var output = encodedString;
  var binVal, thisString;
  var myregexp = /(%[^%]{2})/;
  while ((match = myregexp.exec(output)) != null
             && match.length > 1
             && match[1] != '') {
    binVal = parseInt(match[1].substr(1),16);
    thisString = String.fromCharCode(binVal);
    output = output.replace(match[1], thisString);
  }
  return output;
}
            
    </script>

    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
        <table cellpadding="3" cellspacing="0">
            <tr>
                <td valign="top" align="right">
                    <asp:Label runat="server" CssClass="form-label" Text="Language:" />
                    <VAM:RequiredListValidator ID="RequiredListValidator1" runat="server" ControlIDToEvaluate="ddlLanguageID"
                        UnassignedIndex="0" SummaryErrorMessage="Language" ErrorMessage="*" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlLanguageID" runat="server" CssClass="form-dropdown" AppendDataBoundItems="true">
                        <asp:ListItem Text="<- Please Choose ->" Value="0" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right">
                    <asp:Label ID="Label1" runat="server" CssClass="form-label" Text="Template Name:" />
                    <VAM:RequiredTextValidator runat="server" ControlIDToEvaluate="txtTemplateName" SummaryErrorMessage="Template Name"
                        ErrorMessage="*" />
                </td>
                <td>
                    <asp:TextBox ID="txtTemplateName" runat="server" CssClass="form-textbox" Width="300" />
                </td>
            </tr>
            <tr>
                <td valign="top" align="right">
                    <asp:Label ID="Label2" runat="server" CssClass="form-label" Text="Description:" />
                </td>
                <td>
                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-textbox" Width="300"
                        Height="50" TextMode="MultiLine" />
                </td>
            </tr>
            <tr>
                <td valign="top" align="right">
                    <asp:Label ID="Label3" runat="server" CssClass="form-label" Text="File Name:" />
                </td>
                <td>
                    <asp:TextBox ID="txtFileName" runat="server" CssClass="form-textbox" Width="280" />
                    <asp:Button ID="btnFileName" runat="server" Text="..." />
                </td>
            </tr>
            <tr>
                <td valign="top" align="right">
                    <asp:Label ID="Label4" runat="server" CssClass="form-label" Text="Supports Library Items?:" />
                </td>
                <td>
                    <asp:CheckBox ID="cbxSupportsLibraryItems" runat="server" />
                </td>
            </tr>
            <tr>
                <td valign="top" align="right">
                    <asp:Label ID="Label5" runat="server" CssClass="form-label" Text="Default Template for this Language?:" />
                </td>
                <td>
                    <asp:CheckBox ID="cbxIsDefault" runat="server" />
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
    <RadControls:RadWindowManager ID="RadWindowManager1" runat="server" VisibleStatusbar="false"
        ReloadOnShow="true" Width="400" Height="400" Skin="Default" BackColor="#EEEEEE">
        <Windows>
            <RadControls:RadWindow ID="FileDialog" runat="server" Title="Choose a File" Left="150px"
                ReloadOnShow="true" Modal="true" VisibleStatusbar="false" />
        </Windows>
    </RadControls:RadWindowManager>
</asp:Content>
