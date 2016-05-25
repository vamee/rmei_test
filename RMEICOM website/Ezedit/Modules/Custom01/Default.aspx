<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="Ezedit_Modules_Custom01_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <p>
        <asp:Label ID="lblPageTitle" runat="server" CssClass="pageTitle" Text="Custom Applications" /><hr />
    </p>
    <asp:Label ID="lblAlert" runat="server" CssClass="message"></asp:Label>
    <asp:Panel ID="pnlList" runat="server">

        <script type="text/javascript" language="javascript">
        function confirmDeleteItem()
        {
            if (confirm("Are you sure you want to delete this custom application?")) {
                return true;
            }else{
                return false;
            }
        } 
        </script>

        <table cellpadding="0" cellspacing="0" border="0" id="table1">
            <tr>
                <td>
                    <asp:LinkButton ID="lbtnAddItem" runat="server" CssClass="main"><img src="/App_Themes/EzEdit/Images/application_add.png" border="0" />&nbsp;Add New Application</asp:LinkButton>
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="gdvList" runat="server" AutoGenerateColumns="False" DataSourceID="dataApplications"
            Width="100%" SkinID="GridView" RowStyle-VerticalAlign="Top" AllowSorting="true"
            AllowPaging="true" PageSize="25" PagerSettings-Mode="NumericFirstLast">
            <Columns>
                <asp:BoundField HeaderText="Name" DataField="ResourceName" SortExpression="ResourceName" />
                <asp:BoundField DataField="Description" HeaderText="Description" />
                <asp:TemplateField HeaderText="Display Pages">
                    <ItemTemplate>
                        <asp:Repeater ID="rpDisplayPages" runat="server" OnItemDataBound="rpDisplayPages_ItemDataBound">
                            <ItemTemplate>
                                <asp:Label ID="lblDisplayPages" runat="server" />
                            </ItemTemplate>
                        </asp:Repeater>
                        <li class="main"><a href="../EditModuleProperties.aspx?ModuleKey=Custom01&ForeignKey=ApplicationId&ForeignValue=<%#Eval("ApplicationId")%>"
                            class="main">Add to Page </a></li>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60" SortExpression="DisplayStartDate">
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" Width="55" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Enabled" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40"
                    SortExpression="IsEnabled">
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
                                        ImageUrl="~/App_Themes/EzEdit/Images/edit.png" CommandArgument='<%# Eval("ApplicationID") %>'
                                        ToolTip="Edit" />
                                </td>
                                <td width="20">
                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" OnClick="btnDelete_Click"
                                        ImageUrl="~/App_Themes/EzEdit/Images/delete.png" OnClientClick="return confirmDeleteItem()"
                                        CommandArgument='<%# Eval("ApplicationID") %>' ToolTip="Delete" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <RowStyle VerticalAlign="Top" />
        </asp:GridView>
        <asp:SqlDataSource ID="dataApplications" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
            SelectCommand="SELECT * FROM qryCustomApplications WHERE LanguageID = @LanguageID">
            <SelectParameters>
                <asp:SessionParameter Name="LanguageID" SessionField="EzEditLanguageID" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
        <table width="100%" cellpadding="0" cellspacing="0" border="0" bordercolor="#999999">
            <tr>
                <td>
                    <table border="0" width="100%" cellpadding="5" cellspacing="0">
                        <tr>
                            <td class="form-label" nowrap>
                                Application Name:
                                <VAM:RequiredTextValidator runat="server" ControlIDToEvaluate="txtApplicationName"
                                    SummaryErrorMessage="Application Name" ErrorMessage="*" />
                            </td>
                            <td width="100%">
                                <asp:TextBox ID="txtApplicationName" Width="350" CssClass="form-textbox" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="form-label" nowrap>
                                Enabled:
                            </td>
                            <td width="100%">
                                <asp:RadioButtonList ID="rblIsEnabled" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Yes" Value="True" />
                                    <asp:ListItem Text="No" Value="False" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="form-label" nowrap>
                                Start Date:
                            </td>
                            <td width="100%">
                                <Date:DateTextBox ID="txtDisplayStartDate" runat="server" CssClass="form-text" Width="125" />
                            </td>
                        </tr>
                        <tr>
                            <td class="form-label" nowrap>
                                End Date:
                            </td>
                            <td width="100%">
                                <Date:DateTextBox ID="txtDisplayEndDate" runat="server" CssClass="form-text" Width="125" />
                            </td>
                        </tr>
                        <tr>
                            <td class="form_label" valign="top" nowrap>
                                Description:
                            </td>
                            <td width="100%">
                                <asp:TextBox ID="txtDescription" Width="350" Height="75" TextMode="MultiLine" CssClass="form-textbox"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="form_label" valign="top" nowrap>
                                Code File Name:
                                <VAM:RequiredTextValidator runat="server" ControlIDToEvaluate="txtFileName" SummaryErrorMessage="Code File name"
                                    ErrorMessage="*" />
                            </td>
                            <td width="100%">
                                <asp:TextBox ID="txtFileName" Width="325" CssClass="form-textbox" runat="server" />
                                <asp:Button ID="btnFileName" runat="server" Text="..." />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <VAM:Button ID="btnSave" runat="server" Text="Save" CssClass="form-button" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="form-button" CausesValidation="False" />
                                <asp:HiddenField ID="hdnItemID" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <script type="text/javascript">
        var txtFileName;
    
            function ShowFileDialog(controlID, filePath)
            {
                txtFileName = controlID;
                window.radopen("/EzEdit/Modules/FileBrowser.aspx?FilePath=" + filePath + "&ControlID=" + controlID, "FileDialog");
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

    <RadControls:RadWindowManager ID="RadWindowManager1" runat="server" VisibleStatusbar="false"
        ReloadOnShow="true" Width="400" Height="400" Skin="Default" BackColor="#EEEEEE">
        <Windows>
            <RadControls:RadWindow ID="FileDialog" runat="server" Title="Choose a File" Left="150px"
                ReloadOnShow="true" Modal="true" VisibleStatusbar="false" />
        </Windows>
    </RadControls:RadWindowManager>
</asp:Content>
