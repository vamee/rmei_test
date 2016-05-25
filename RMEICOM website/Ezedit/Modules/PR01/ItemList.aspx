<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false" CodeFile="ItemList.aspx.vb" Inherits="Ezedit_Modules_PR01_ArticleList" ValidateRequest="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <script language="javascript" type="text/jscript">
         function confirmDeleteItem() {
            if (confirm("Are you sure you want to delete this article?")==true)
                return true;
            else
                return false;
        } 
    </script>
    <a href="../Default.aspx?ModuleKey=PR01" class="pageTitle">News</a> <asp:Label ID="lblPageTitle" CssClass="pagetitle" runat="server" Text="" />
    <br />
	<asp:label id="lblAlert" runat="server" cssclass="alert" />
	<VAM:ValidationSummary runat="server" CssClass="alert" HeaderText="The following fields are required:" ScrollIntoView="Top" AutoUpdate="true" />
	<hr />
	<asp:Panel ID="pnlList" runat="server">
	<table border="0" cellpadding="0" cellspacing="0" width="100%">
	    <tr>
		    <td>
                <table cellpadding="0" cellspacing="0" border="0" id="table1">
	                <tr>
		                <td>
		                    <asp:LinkButton ID="btnAddNew" runat="server" CssClass="main"><img src="/App_Themes/EzEdit/Images/table_add.png" border="0" />&nbsp;Add New Article</asp:LinkButton>
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
     <asp:GridView ID="gdvList" runat="server" AutoGenerateColumns="False" Width="100%"
            CellPadding="3" HeaderStyle-CssClass="table-header" AlternatingRowStyle-CssClass="table-altrow"
            RowStyle-CssClass="table-row" DataSourceID="dataArticles" AllowSorting="true" AllowPaging="true" PageSize="25" PagerSettings-Mode="NumericFirstLast">
            <EmptyDataTemplate>
                The are no articles to display.
            </EmptyDataTemplate>
            <Columns>
                <asp:BoundField DataField="ResourceName" HeaderText="Title" HeaderStyle-HorizontalAlign="left"
                    ItemStyle-HorizontalAlign="left" SortExpression="ResourceName" />
               <asp:TemplateField HeaderText="Type" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50" SortExpression="ModuleType">
                    <ItemTemplate>
                        <asp:Hyperlink ID="hypModuleType" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60">
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" Width="55" /><br />
                        <asp:Label ID="lblDisplayDates" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Enabled" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50" SortExpression="IsEnabled">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbxIsEnabled" runat="server" AutoPostBack="true" OnCheckedChanged="cbxIsEnabled_CheckChanged" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sort" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50" SortExpression="SortOrder">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlSortOrder" runat="server" OnSelectedIndexChanged="ddlSortOrder_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:HiddenField ID="hdnSortOrder" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <table border="0" cellspacing="2">
                            <tr>
                                <td width="16">
                                    <asp:ImageButton ID="btnEditItem" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/edit.png"
                                        OnClick="btnEditItem_Click" ToolTip="Edit Page Properties" />
                                </td>
                                <td width="16">
                                    <asp:ImageButton ID="btnDeleteItem" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/delete.png"
                                        OnClientClick="return confirmDeleteItem();" OnClick="btnDeleteItem_Click" ToolTip="Delete Item" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dataArticles" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
            SelectCommand="SELECT DISTINCT ArticleID,CategoryID,ModuleType,DisplayDate,ResourceName,ArticleSummary,ArticleUrl,FileName,SortOrder,DisplayStartDate,DisplayEndDate,IsEnabled FROM qryArticles WHERE CategoryID = @CategoryID ORDER BY SortOrder">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="0" Name="CategoryID" QueryStringField="CategoryID"
                    Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
      </asp:Panel>
      <asp:Panel ID="pnlEdit" runat="server" Visible="false">
        
        <table border="0" width="100%" cellpadding="5" cellspacing="0" id="table2">
            <tr>
                <td class="form_label" nowrap>
                    Category:
                </td>
                <td width="100%">
                    <asp:DropDownList ID="ddlModuleCategoryID" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="<- Please Choose ->" Value="-1" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trSubCategory" runat="server" visible="false">
                <td class="form_label" nowrap>
                    Sub Category:
                </td>
                <td width="100%">
                    <asp:DropDownList ID="ddlResourceCategory" runat="server">
                    </asp:DropDownList>
                    <VAM:TextBox ID="txtResourceCategory" runat="server"></VAM:TextBox>
                </td>
            </tr>
            <tr>
                <td class="form_label" nowrap>
                    Type:
                    <VAM:RequiredListValidator ID="RequiredListValidator1" runat="server" ControlIDToEvaluate="ddlModuleTypeID"
                        SummaryErrorMessage="Type is required." ErrorMessage="*" UnassignedIndex="0" />
                </td>
                <td width="100%">
                    <asp:DropDownList ID="ddlModuleTypeID" runat="server" AutoPostBack="true" AppendDataBoundItems="true">
                        <asp:ListItem Text="<- Please Choose ->" Value="-1" />
                    </asp:DropDownList>
                </td>
            </tr>
            <asp:Panel ID="pnlEditFields" runat="server" Visible="false">
                <tr>
                    <td class="form_label" nowrap>
                        Release Date:
                        <VAM:RequiredTextValidator ID="RequiredTextValidator1" ControlIDToEvaluate="pdpDate"
                            SummaryErrorMessage="Date is required" runat="server" ErrorMessage="*" />
                    </td>
                    <td width="100%">
                        <Date:DateTextBox ID="pdpDate" runat="server" CssClass="form-textbox" xMinDate="1/1/1940"
                            xMaxDate="1/1/2025"></Date:DateTextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_label" nowrap>
                        Title:
                        <VAM:RequiredTextValidator ControlIDToEvaluate="txtResourceName" SummaryErrorMessage="Title is required"
                            ErrorMessage="*" ID="RequiredFieldValidator2" runat="server" />
                    </td>
                    <td width="100%">
                        <VAM:TextBox ID="txtResourceName" Width="600" runat="server" CssClass="form-textbox"></VAM:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_label" valign="top" nowrap>
                        Summary:
                    </td>
                    <td width="100%">
                        <asp:TextBox ID="txtSummary" runat="server" Width="600" MaxLength="255" Rows="5" TextMode="MultiLine" CssClass="form-textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_label" valign="top" nowrap>
                        Keywords:
                    </td>
                    <td width="100%">
                        <asp:TextBox ID="txtKeywords" runat="server" Width="600" MaxLength="255" CssClass="form-textbox"
                            Rows="5" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <asp:Panel ID="pnlExternalURL" runat="server" Visible="false">
                    <tr>
                        <td class="form_label" valign="top" nowrap>
                            URL:
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtURL" runat="server" Width="350" MaxLength="255" CssClass="form-textbox"
                                Rows="5"></asp:TextBox>
                        </td>
                    </tr>
                </asp:Panel>
                <asp:Panel ID="pnlContent" runat="server" Visible="false">
                    <tr>
                        <td class="form_label" nowrap valign="top">
                            Text:<span class="message"></span>
                        </td>
                        <td width="100%">
                            <EzEdit:ContentEditor EditorWidth="600" EditorHeight="600" ID="ContentEditor" runat="server" />
                        </td>
                    </tr>
                </asp:Panel>
                <asp:Panel ID="pnlFile" runat="server" Visible="false">
                    <tr>
                        <td class="form_label" nowrap valign="top">
                            File:<span class="message"></span>
                        </td>
                        <td width="100%">
                            <asp:FileUpload ID="uplFile" runat="server" Width="350" CssClass="form-textbox" /><br />
                            <asp:Label ID="lblFile" runat="server" Text="" CssClass="form"></asp:Label>
                        </td>
                    </tr>
                </asp:Panel>
                <tr id="Tr1" runat="server" visible="false">
                    <td class="form_label" valign="top" nowrap>
                        Image:
                    </td>
                    <td width="100%">
                        <asp:FileUpload ID="uplImageURL" runat="server" Width="250" CssClass="form-textbox" />
                        <asp:Label ID="lblImageURL" runat="server" Text="" CssClass="form"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" CssClass="form-label" Text="Start Date:" />
                    </td>
                    <td>
                        <Date:DateTextBox ID="txtDisplayStartDate" runat="server" Width="100" CssClass="form-textbox" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" CssClass="form-label" Text="End Date:" />
                    </td>
                    <td>
                        <Date:DateTextBox ID="txtDisplayEndDate" runat="server" Width="100" CssClass="form-textbox" />
                    </td>
                </tr>
                <tr id="trRssFeed" runat="server">
                    <td class="form_label" valign="top" nowrap>
                        Publish to RSS:
                    </td>
                    <td width="100%">
                        <asp:CheckBox ID="cbxPublishToRss" runat="server" AutoPostBack="true" />
                    </td>
                </tr>
                <asp:Panel ID="pnlRssFeed" runat="server" Visible="false">
                    <tr>
                        <td class="form_label" valign="top" nowrap>
                            Rss Item Author:
                            <VAM:RequiredTextValidator ID="RequiredTextValidator2" runat="server" ControlIDToEvaluate="txtRssAuthor"
                                SummaryErrorMessage="Rss Item Author is required" ErrorMessage="*" />
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtRssAuthor" runat="server" Width="600" CssClass="form" />
                        </td>
                    </tr>
                    <tr>
                        <td class="form_label" valign="top" nowrap>
                            Rss Item Description:
                            <VAM:RequiredTextValidator ID="RequiredTextValidator3" runat="server" ControlIDToEvaluate="txtRssDescription"
                                SummaryErrorMessage="Rss Item Description is required" ErrorMessage="*" />
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtRssDescription" runat="server" Width="600" Height="100" TextMode="MultiLine"
                                CssClass="form" />
                        </td>
                    </tr>
                </asp:Panel>
            </asp:Panel>
            <tr>
                <td></td>
                <td>
                    <VAM:Button ID="btnSave" runat="server" CssClass="form-button" Text="Save" />
                    <asp:Button ID="btnCancel" runat="server" CssClass="form-button" Text="Cancel" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnItemID" runat="server" />
    </asp:Panel>
    
   
</asp:Content>

