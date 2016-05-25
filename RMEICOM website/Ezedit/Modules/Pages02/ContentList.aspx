<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false" CodeFile="ContentList.aspx.vb" Inherits="Ezedit_Modules_Pages01_ContentList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <script language="javascript" type="text/javascript">
        function confirmDelete() {
            if (confirm("Are you sure you want to delete this content?")) {
                return true;
            }else{
                return false;
            }
        }
    </script>
    <asp:Label id="lblBreadcrumbs" runat="server" />
	<asp:Label id="lblAlert" runat="server" cssclass="alert" />
	<hr />
	
	<%--<asp:Panel ID="pnlList" runat="server">
        <script language="javascript" type="text/javascript">
            function confirmDelete() {
                if (confirm("Are you sure you want to delete this content?")) {
                    return true;
                }else{
                    retrun false;
                }
            }
        </script>
    
        <asp:Repeater ID="rptContent" runat="server" DataSourceID="dataContent">
            <HeaderTemplate>
                <table width="100%" cellpadding="2" cellspacing="0">
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <span class="subTitle">
                            <%#Eval("Status")%>
                            Content</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gdvContent" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            HeaderStyle-CssClass="table-header" AlternatingRowStyle-CssClass="table-altrow"
                            RowStyle-CssClass="table-row" DataSourceID="dataContent">
                            <Columns>
                                <asp:BoundField DataField="Version" HeaderText="Version" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="UpdatedDate" HeaderText="Last Updated" ItemStyle-Width="150"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" ItemStyle-Width="150"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:TemplateField ItemStyle-Width="120" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <table border="0" cellspacing="0">
                                            <tr>
                                                <td width="20">
                                                    <asp:ImageButton ID="btnPublish" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/arrow_up.png"
                                                        OnClick="btnPublish_Click" ToolTip="Publish this Content" />
                                                </td>
                                                <td width="20">
                                                    <asp:HyperLink ID="hypPreview" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/magnifier_zoom_in.png"
                                                        ToolTip="Preview this Page" Target="_blank" />
                                                </td>
                                                <td width="20">
                                                    <asp:ImageButton ID="btnEmail" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/email_add.png"
                                                        OnClick="btnEmail_Click" ToolTip="Email link to a Colleague" />
                                                </td>
                                                <td width="20">
                                                    <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/page_edit.png"
                                                        OnClick="btnEdit_Click" ToolTip="Edit Content" />
                                                </td>
                                                <td width="20">
                                                    <VAM:ImageButton ID="btnDelete" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/delete.png"
                                                         OnClientClick="return confirmDelete();" OnClick="btnDelete_Click"
                                                        ToolTip="Delete Content" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:HiddenField ID="hdnPageID" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="dataContent" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
                            SelectCommand=""></asp:SqlDataSource>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <asp:SqlDataSource ID="dataContent" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
            SelectCommand="SELECT * FROM ItemStatus WHERE StatusID IN (SELECT StatusID FROM Content WHERE ModuleKey=@ModuleKey AND ForeignKey=@ForeignKey) ORDER BY StatusID DESC">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="0" Name="ForeignKey" QueryStringField="PageID"
                    Type="Int32" />
                <asp:Parameter Name="ModuleKey" DefaultValue="Pages01" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
    </asp:Panel>
    <asp:Panel ID="pnlEmail" runat="server" Visible="false">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr class="ButtonBar">
                <td id="Td1" colspan="2" class="ButtonBar">
                    <table class="TwoCellButtonBar">
                        <tr>
                            <td class="ButtonBarLeft">
                                <div class="ButtonBarImageButton">
                                    <VAM:LinkButton ID="btnSend" runat="server" CssClass="ButtonBarLinkButton"><img src="/App_Themes/WebAdmin/Images/email_add.png"  class="ButtonBarLinkButton" />&nbsp;Send</VAM:LinkButton>
                                </div>
                            </td>
                            <td class="ButtonBarRight" align="center">
                                <div class="ButtonBarRight">
                                    <asp:LinkButton ID="btnCancel" runat="server" CssClass="ButtonBarImageButton"><img src="/App_Themes/WebAdmin/Images/cross.png"  class="ButtonBarLinkButton" />&nbsp;Cancel</asp:LinkButton>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <table border="0" width="100%" cellpadding="5" cellspacing="0" id="table1">
            <tr>
                <td class="form-label" nowrap>
                    Email To:
                    <VAM:EmailAddressValidator ID="EmailAddressValidator1" runat="server" ControlIDToEvaluate="txtEmailTo" SummaryErrorMessage="Invalid 'To:' address." ErrorMessage="*" IgnoreBlankText="false" />
                </td>
                <td width="100%">
                    <asp:TextBox ID="txtEmailTo" runat="server" Width="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="form-label" nowrap>
                    Email CC:
                    <VAM:EmailAddressValidator ID="EmailAddressValidator2" runat="server" ControlIDToEvaluate="txtEmailCc" SummaryErrorMessage="Invalid 'CC:' address." ErrorMessage="*" IgnoreBlankText="true" />
                </td>
                <td width="100%">
                    <asp:TextBox ID="txtEmailCC" runat="server" Width="250"></asp:TextBox>
                    
                </td>
            </tr>
            <tr>
                <td class="form-label" nowrap>
                    Email Subject:
                    <VAM:RequiredTextValidator ID="RequiredTextValidator1" runat="server" ControlIDToEvaluate="txtEmailSubject" SummaryErrorMessage="Email subject cannot be blank." ErrorMessage="*" />
                </td>
                <td width="100%">
                    <asp:TextBox ID="txtEmailSubject" runat="server" Width="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="form-label" nowrap valign="top">
                    Message:
                    <VAM:RequiredTextValidator ID="RequiredTextValidator2" runat="server" ControlIDToEvaluate="txtEmailMessage" SummaryErrorMessage="Email message cannot be blank." ErrorMessage="*" />
                </td>
                <td width="100%">
                    <asp:TextBox ID="txtEmailMessage" TextMode="MultiLine" Rows="7" Columns="52" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:Panel>--%>
	
	
	<table width="530" cellpadding="3" cellspacing="1" border="0">
        <asp:Repeater
            Id="rpProductionContent"
            Runat="Server"
            EnableViewState="False">
            <HeaderTemplate>
                <tr>
	                <td colspan="4">
	                     <span class="subTitle">Live Content</span>
	                </td>
	            </tr>
                <tr bgcolor="#7288AD">
				    <td class="table-header">Version</td>
				    <td class="table-header">Last Updated</td>
				    <td class="table-header">Updated By</td>
				    <td class="table-header">&nbsp;</td>
			    </tr> 
            </HeaderTemplate>
            <ItemTemplate>
                <tr id="trPages" runat="server">
                    <td class="table_text" align="center">
                        <%#Eval("Version")%>
                    </td>
                    <td class="table_text" align="center">
                        <%#Eval("UpdatedDate")%>
                    </td>
                    <td class="table_text" align="center">
                        <%#Eval("UpdatedBy")%>
                    </td>
                    <td>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Image Width="22" Height="15" runat="server" />
                                </td>
                                <td align="center">
                                    <asp:HyperLink SkinID="IconPreviewContent" ID="hypPreviewContent" runat="server" Target="_blank" />
                                </td>
                                <td align="center">
                                    <asp:HyperLink SkinID="IconEmailContent" ID="hypEmailContent" runat="server" />
                                </td>
                                <td align="center">
                                    <a href="EditContent.aspx?ContentID=<%# Eval("ContentID") %>&PageID=<%= Request("PageId") %>">
                                        <asp:Image SkinID="IconEditBody" runat="server" />
                                    </a>
                                </td>
                                <td>
                                    <asp:Image Width="15" Height="15" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                <tr bgcolor="#7288AD">
                    <td colspan="4">
                        &nbsp;
                    </td>
                </tr>
	        </FooterTemplate>
        </asp:Repeater>
        <tr>
           <td colspan="4">&nbsp;</td>
        </tr>
        <asp:Repeater
            Id="rpStagingContent"
            Runat="Server"
            EnableViewState="False"
            OnItemCommand="OnItemCommand">
            <HeaderTemplate>
                <tr>
                    <td colspan="4">
                        <span class="subTitle">Staged Content</span>
                    </td>
                </tr>
                <tr bgcolor="#7288AD">
                    <td class="table_header">Version</td>
                    <td class="table_header">Last Updated</td>
                    <td class="table_header">Updated By</td>
                    <td class="table_header">&nbsp;</td>
                </tr> 
            </HeaderTemplate>
            <ItemTemplate>
                <asp:HiddenField ID="HiddenField1" Value='<%#Eval("ContentId")%>' runat="server" />
                <tr id="trPages" runat="server">
                    <td class="table_text" align="center">
                        <%#Eval("Version")%>
                    </td>
                    <td class="table_text" align="center">
                        <%#Eval("UpdatedDate")%>
                    </td>
                    <td class="table_text" align="center">
                        <%#Eval("UpdatedBy")%>
                    </td>
                    <td>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="center">
                                    <asp:ImageButton SkinID="IconPromoteContent" CommandName="Promote" ID="btnPromote" runat="server" CommandArgument='<%# Eval("ContentID") %>' />
                                </td>
                                <td align="center">
                                    <asp:Hyperlink SkinID="IconPreviewContent" ID="hypPreviewContent" runat="server" Target="_blank" />
                                </td>
                                <td align="center">
                                    <a href="#A" onClick="emailContent(<%# Eval("ContentID") %>, <%=_PageID%>);">
                                        <asp:Image SkinID="IconEmailContent" ID="hypEmailContent" runat="server" />
                                    </a>
                                    
                                </td>
                                <td align="center">
                                   <a href="EditContent.aspx?ContentID=<%# Eval("ContentID") %>&PageID=<%= Request("PageId") %>">
                                        <asp:Image ID="Image1" SkinID="IconEditBody" runat="server" />
                                    </a>
                                </td>
                                <td align="center">
                                    <asp:ImageButton SkinID="IconDelete" CommandName="Delete" ID="btnDelete" runat="server" ToolTip="Delete this Content" OnClientClick="return confirmDelete();" CommandArgument='<%# Eval("ContentID") %>' />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                <tr bgcolor="#7288AD">
                    <td colspan="4">
                        &nbsp;
                    </td>
                </tr>
	         </FooterTemplate>
        </asp:Repeater>
    </table>
    <script language="javascript">
                
        function emailContent(contentID, pageID)
        {
	        window.open("EmailContent.aspx?ContentID=" + contentID + "&PageID=" + pageID, "popup", "width=450,height=300,scrollbars=no,history=no");
        }
    </script>
</asp:Content>