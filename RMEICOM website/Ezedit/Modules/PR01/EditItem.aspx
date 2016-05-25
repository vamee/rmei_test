<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="EditItem.aspx.vb" Inherits="Ezedit_Modules_PR01_EditArticle" ValidateRequest="false" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <p>
        <asp:Literal ID="litPageTitle" runat="server" />
    </p>
    <p>
        <asp:Label ID="lblAlert" runat="server" CssClass="message"></asp:Label>
    </p>
    <p>
        <VAM:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="message"
            HeaderText="Please correct the following errors: " AutoUpdate="true" />
    </p>
    <table border="0" width="100%" cellpadding="5" cellspacing="0" id="table1">
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>
        <tr>
            <td class="form_label" nowrap>
                Enabled:
            </td>
            <td width="100%">
                <asp:CheckBox ID="cbxIsEnabled" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="form_label" nowrap>
                Category:
            </td>
            <td width="100%">
                <asp:DropDownList ID="ddlCategoryId" runat="server" TabIndex="1">
                </asp:DropDownList>
            </td>
        </tr>
        <tr id="Tr1" runat="server" visible="false">
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
                <VAM:RequiredListValidator ID="RequiredListValidator1" runat="server" ControlIDToEvaluate="ddlModuleTypeID" SummaryErrorMessage="Type is required." ErrorMessage="*" UnassignedIndex="0" />
            </td>
            <td width="100%">
                <asp:DropDownList ID="ddlModuleTypeID" runat="server" TabIndex="1" AutoPostBack="true">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="form_label" nowrap>
                Date:
                <VAM:RequiredTextValidator ID="RequiredTextValidator1" ControlIDToEvaluate="pdpDate"
                    SummaryErrorMessage="Date is required" runat="server" ErrorMessage="*" />
            </td>
            <td width="100%">
                <Date:DateTextBox ID="pdpDate" runat="server" CssClass="main" xMinDate="1/1/1940"
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
                <VAM:TextBox ID="txtResourceName" Width="600" runat="server" TabIndex="3"></VAM:TextBox>
            </td>
        </tr>
        <tr>
            <td class="form_label" valign="top" nowrap>
                Summary:
            </td>
            <td width="100%">
                <asp:TextBox ID="txtSummary" runat="server" Width="600" MaxLength="255" TabIndex="8"
                    Rows="5" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="form_label" valign="top" nowrap>
                Keywords:
            </td>
            <td width="100%">
                <asp:TextBox ID="txtKeywords" runat="server" Width="600" MaxLength="255" TabIndex="8"
                    Rows="5" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <asp:Panel ID="pnlExternalURL" runat="server" Visible="false" EnableViewState="false">
            <tr>
                <td class="form_label" valign="top" nowrap>
                    URL:
                </td>
                <td width="100%">
                    <asp:TextBox ID="txtURL" runat="server" Width="350" MaxLength="255" TabIndex="8"
                        Rows="5"></asp:TextBox>
                </td>
            </tr>
        </asp:Panel>
        <asp:Panel ID="pnlContent" runat="server" Visible="false" EnableViewState="false">
            <tr>
                <td class="form_label" nowrap valign="top">
                    Text:<span class="message"></span>
                </td>
                <td width="100%">
                    <EzEdit:ContentEditor EditorWidth="600" EditorHeight="600" ID="ContentEditor" runat="server" />
                </td>
            </tr>
        </asp:Panel>
        <asp:Panel ID="pnlFile" runat="server" Visible="false" EnableViewState="false">
            <tr>
                <td class="form_label" nowrap valign="top">
                    File:<span class="message"></span>
                </td>
                <td width="100%">
                    <asp:FileUpload ID="uplFile" runat="server" CssClass="form" Width="350" />
                    <asp:Label ID="lblFile" runat="server" Text="" CssClass="form"></asp:Label>
                </td>
            </tr>
        </asp:Panel>
        <tr>
            <td class="form_label" valign="top" nowrap>
                Image:
            </td>
            <td width="100%">
                <asp:FileUpload ID="uplImageURL" runat="server" Width="250" />
                <asp:Label ID="lblImageURL" runat="server" Text="" CssClass="form"></asp:Label>
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
                    <VAM:RequiredTextValidator ID="RequiredTextValidator2" runat="server" ControlIDToEvaluate="txtRssAuthor" SummaryErrorMessage="Rss Item Author is required" ErrorMessage="*" />
                </td>
                <td width="100%">
                    <asp:TextBox ID="txtRssAuthor" runat="server" Width="600" CssClass="form" />
                </td>
            </tr>
            <tr>
                <td class="form_label" valign="top" nowrap>
                    Rss Item Description:
                    <VAM:RequiredTextValidator ID="RequiredTextValidator3" runat="server" ControlIDToEvaluate="txtRssDescription" SummaryErrorMessage="Rss Item Description is required" ErrorMessage="*" />
                </td>
                <td width="100%">
                    <asp:TextBox ID="txtRssDescription" runat="server" Width="600" Height="100" TextMode="MultiLine" CssClass="form" />
                </td>
            </tr>
        </asp:Panel>
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <VAM:Button ID="btnSave" runat="server" Text=" Save " CausesValidation="true" />
                <VAM:Button ID="btnCancel" runat="server" Text=" Cancel " CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Content>
