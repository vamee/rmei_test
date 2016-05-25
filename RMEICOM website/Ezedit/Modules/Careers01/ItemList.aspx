<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="ItemList.aspx.vb" Inherits="Ezedit_Modules_Careers01_CareerList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <p>
        <a href="../Default.aspx?ModuleKey=Careers01" class="pageTitle">Careers</a>
        <asp:Label ID="lblPageTitle" CssClass="pagetitle" runat="server" Text="" /><br />
        <asp:Label ID="lblAlert" runat="server" CssClass="alert" />
        <VAM:ValidationSummary runat="server" CssClass="alert" HeaderText="The following errors were found:"
            AutoUpdate="true" ScrollIntoView="Top" />
        <hr />
    </p>
    <asp:Panel ID="pnlList" runat="server">

        <script language="javascript" type="text/jscript">
         function confirmDeleteItem() {
            if (confirm("Are you sure you want to delete this Career?")) {
                return true;
            }else{
                return false;
            }
        } 
        </script>

        <table cellpadding="0" cellspacing="0" border="0" id="table1">
            <tr>
                <td>
                    <asp:LinkButton ID="lbtnAddItem" runat="server" CssClass="main"><img src="/App_Themes/EzEdit/Images/application_add.png" border="0" />&nbsp;Add New Career</asp:LinkButton>
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="gdvList" runat="server" AutoGenerateColumns="False" DataKeyNames="CategoryID"
            DataSourceID="dataCareers" Width="100%" SkinID="GridView" RowStyle-VerticalAlign="Top"
            AllowSorting="true" AllowPaging="true" PageSize="25" PagerSettings-Mode="NumericFirstLast">
            <Columns>
                <asp:BoundField DataField="DisplayDate" HeaderText="Date" SortExpression="DisplayDate"
                    DataFormatString="{0:MMMM dd, yyyy}" ItemStyle-Width="115" />
                <asp:BoundField HeaderText="Name" DataField="ResourceName" SortExpression="ResourceName" />
                <asp:TemplateField HeaderText="Type" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50"
                    SortExpression="ModuleType" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:HyperLink ID="hypModuleType" runat="server" />
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
                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60" HeaderStyle-HorizontalAlign="Center">
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
                                        ImageUrl="~/App_Themes/EzEdit/Images/edit.png" CommandArgument='<%# Eval("CareerID") %>'
                                        ToolTip="Edit" />
                                </td>
                                <td width="20">
                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" OnClick="btnDelete_Click"
                                        ImageUrl="~/App_Themes/EzEdit/Images/delete.png" OnClientClick="return confirmDeleteItem()"
                                        CommandArgument='<%# Eval("CareerID") %>' ToolTip="Delete" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <RowStyle VerticalAlign="Top" />
        </asp:GridView>
        <asp:SqlDataSource ID="dataCareers" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
            SelectCommand="SELECT * FROM qryCareers WHERE CategoryID = @CategoryID ORDER BY SortOrder">
            <SelectParameters>
                <asp:QueryStringParameter Name="CategoryID" QueryStringField="CategoryID" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
        <table border="0" cellpadding="5" cellspacing="0">
            <tr>
                <td class="form-label" nowrap>
                    Category:
                    <VAM:RequiredListValidator runat="server" ControlIDToEvaluate="ddlCategoryID" SummaryErrorMessage="Category"
                        ErrorMessage="*" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlCategoryID" runat="server" CssClass="form-dropdown" AppendDataBoundItems="true">
                        <asp:ListItem Text="<- Please Choose ->" Value="-1" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="form_label" nowrap>
                    Display Type:
                    <VAM:RequiredListValidator runat="server" ControlIDToEvaluate="ddlModuleTypeID" SummaryErrorMessage="Choose a display type from the list."
                        UnassignedIndex="0" ErrorMessage="*" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlModuleTypeID" runat="server" AutoPostBack="true" AppendDataBoundItems="true">
                        <asp:ListItem Text="<- Please Choose ->" Value="-1" />
                    </asp:DropDownList>
                </td>
            </tr>
            <asp:Panel ID="pnlCommon" runat="server" Visible="false">
                <tr>
                    <td class="form-label" nowrap>
                        Release Date:
                        <VAM:DataTypeCheckValidator ID="DataTypeCheckValidator1" DataType="date" ControlIDToEvaluate="txtDisplayDate"
                            SummaryErrorMessage="Display date is invalid" ErrorMessage="*" runat="server" />
                    </td>
                    <td>
                        <Date:DateTextBox ID="txtDisplayDate" runat="server" CssClass="form-textbox" xMinDate="1/1/1940"
                            xMaxDate="1/1/2025" Width="100" />
                    </td>
                </tr>
                <tr>
                    <td class="form-label" nowrap>
                        Title:
                        <VAM:RequiredTextValidator ControlIDToEvaluate="txtResourceName" SummaryErrorMessage="Title is required."
                            ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" />
                    </td>
                    <td width="100%">
                        <VAM:TextBox ID="txtResourceName" Width="400" runat="server" CssClass="form-textbox" />
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
                    <td class="form-label" valign="top" nowrap>
                        Summary:
                    </td>
                    <td width="100%">
                        <asp:TextBox ID="txtSummary" runat="server" Width="400" MaxLength="255" TabIndex="8"
                            Rows="5" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form-label" valign="top" nowrap>
                        Keywords:
                    </td>
                    <td width="100%">
                        <asp:TextBox ID="txtKeywords" runat="server" Width="400" MaxLength="255" TabIndex="8"
                            Rows="5" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
            </asp:Panel>
            <asp:Panel ID="pnlContent" runat="server" Visible="false">
                <tr>
                    <td class="form-label" nowrap valign="top">
                        Text:<span class="alert">*</span>
                    </td>
                    <td width="100%">
                        <EzEdit:ContentEditor ID="txtContentEditor" EditorWidth="600" EditorHeight="500"
                            runat="server" />
                    </td>
                </tr>
            </asp:Panel>
            <asp:Panel ID="pnlExternalUrl" runat="server" Visible="false">
                <tr>
                    <td class="form-label" nowrap valign="top">
                        External URL:
                        <VAM:RequiredTextValidator runat="server" ControlIDToEvaluate="txtExternalUrl" SummaryErrorMessage="External URL is required."
                            ErrorMessage="*" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtExternalUrl" runat="server" CssClass="form-textbox" Width="400" />
                    </td>
                </tr>
            </asp:Panel>
        </table>
        <hr />
        <center>
            <VAM:Button ID="btnSave" CssClass="form-button" runat="server" Text="Save" />
            <asp:Button ID="btnCancel" CssClass="form-button" runat="server" Text="Cancel" />
            <asp:HiddenField ID="hdnItemID" runat="server" />
        </center>
    </asp:Panel>
    <%--<asp:Repeater ID="rpListings" runat="Server" EnableViewState="False" OnItemCommand="OnItemCommand">
        <HeaderTemplate>
            <table width="100%" cellpadding="0" cellspacing="0" border="1" bordercolor="#999999">
                <tr>
                    <td>
                        <table border="0" width="100%" cellpadding="3" cellspacing="1">
                            <tr bgcolor="#7288AD">
                                <td class="table_header">
                                    Date
                                </td>
                                <td class="table_header">
                                    Name
                                </td>
                                <td class="table_header">
                                    Summary
                                </td>
                                <td class="table_header">
                                    Order
                                </td>
                                <td class="table_header">
                                    Online
                                </td>
                                <td class="table_header">
                                    <table width="100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Image ID="Image2" SkinID="IconEdit" runat="server" ToolTip="Edit Article" /></a>
                                            </td>
                                            <td align="center">
                                                <asp:Image ID="Image4" SkinID="IconDelete" runat="server" ToolTip="Delete Article" /></a>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <asp:HiddenField ID="hdnItemId" Value='<%#Eval("CareerID")%>' runat="server" />
            <tr id="trArticle" runat="server">
                <td class="table_text">
                    <%#String.Format("{0:d}", Eval("DisplayDate"))%>
                </td>
                <td class="table_text">
                    <%#Eval("ResourceName")%>
                </td>
                <td class="table_text">
                    <%#Eval("CareerSummary")%>
                </td>
                <td class="table_text">
                    <asp:DropDownList ID="ddlSortOrder" runat="server">
                    </asp:DropDownList>
                </td>
                <td class="table_text" align="center">
                    <asp:Label ID="lblIsEnabled" runat="server" />
                </td>
                <td>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="center">
                                <a href="EditItem.aspx?CategoryID=<%=Request("CategoryID")%>&ItemID=<%#Eval("CareerId")%>">
                                    <asp:Image ID="Image5" SkinID="IconEdit" runat="server" ToolTip="Edit Career" />
                                </a>
                            </td>
                            <td align="center">
                                <asp:ImageButton SkinID="IconDelete" CommandName="Delete" ID="btnDelete" runat="server"
                                    ToolTip="Delete Career" />
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
</asp:Content>
