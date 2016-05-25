<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="EditPage.aspx.vb" Inherits="Ezedit_Modules_Pages01_EditPage" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">

    <script language="javascript" type="text/javascript">
    function deleteHeader() {
        if (confirm("Are you sure you want to delete this header?")) {
            return true;
        }else{
            return false;
        }
    }
    </script>

    <p>
        <asp:Label ID="lblPageTitle" runat="server" CssClass="pageTitle" />
    </p>
    <p>
        <asp:Label ID="lblAlert" runat="server" CssClass="message"></asp:Label>
    </p>
    <p>
        <VAM:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert" HeaderText="Please correct the following errors:"
            AutoUpdate="true" ScrollIntoView="Top" HyperLinkToFields="true" />
    </p>
    <table width="100%" cellpadding="0" cellspacing="0" border="0" bordercolor="#999999">
        <tr>
            <td>
                <table border="0" width="100%" cellpadding="5" cellspacing="0">
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td class="form_label" nowrap>
                            Type:<span class="message">*</span>
                        </td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlPageTypeId" runat="server" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="form_label" nowrap>
                            Parent Page:<span class="message">*</span>
                        </td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlParentPageId" runat="server">
                                <asp:ListItem Value="0">&lt;Root&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trPageTemplate" runat="server">
                        <td class="form_label" nowrap>
                            Page Template:<span class="message">*</span>
                        </td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlTemplateId" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="form_label" nowrap>
                            Name:<span class="message">*</span>
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtPageName" runat="server" Width="250" MaxLength="100"></asp:TextBox>
                            <VAM:RequiredTextValidator ID="rfvPageName" runat="server" SummaryErrorMessage="Name is Required."
                                ControlIDToEvaluate="txtPageName" />
                        </td>
                    </tr>
                    <tr id="trLink" runat="server" visible="false">
                        <td class="form_label" nowrap>
                            Link URL:<span class="message">*</span>
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtPageAction" runat="server" Width="350" MaxLength="255"></asp:TextBox>
                            <VAM:RequiredTextValidator ID="rfvPageAction" runat="server" SummaryErrorMessage="Link URL is Required."
                                ControlIDToEvaluate="txtPageAction" />
                        </td>
                    </tr>
                    <tr runat="server" id="trPageKey">
                        <td class="form_label" nowrap>
                            Key:<span class="message">*</span>
                        </td>
                        <td width="100%">
                            <VAM:FilteredTextBox ID="txtPageKey" runat="server" Width="250" MaxLength="100" Space="false"
                                LettersLowercase="true" Digits="true" OtherCharacters="-_" />
                            <VAM:RequiredTextValidator ID="rfvPageKey" runat="server" SummaryErrorMessage="Key is Required."
                                ControlIDToEvaluate="txtPageKey" />
                            <VAM:CustomValidator ID="csvPageKey" runat="server" EventsThatValidate="OnSubmit"
                                SummaryErrorMessage="Page Key already in use." ControlIDToEvaluate="txtPageKey" />
                        </td>
                    </tr>
                    <tr>
                        <td class="form_label" nowrap>
                            Menu Name:<span class="message">*</span>
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtMenuName" runat="server" Width="250" MaxLength="100"></asp:TextBox>
                            <VAM:RequiredTextValidator ID="rfvMenuName" runat="server" SummaryErrorMessage="Menu Name is Required."
                                ControlIDToEvaluate="txtMenuName" />
                        </td>
                    </tr>
                    <tr runat="server" id="trTitleTag">
                        <td class="form_label" nowrap>
                            Title Tag:
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtTitleTag" runat="server" Width="350" MaxLength="255"></asp:TextBox>
                        </td>
                    </tr>
                    <tr runat="server" id="trMetaDescription">
                        <td class="form_label" nowrap>
                            Meta Description:
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtMetaDescription" TextMode="MultiLine" runat="server" Width="350"
                                Height="50" MaxLength="500"></asp:TextBox>
                        </td>
                    </tr>
                    <tr runat="server" id="trMetaKeywords">
                        <td class="form_label" nowrap>
                            Meta Keywords:
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtMetaKeywords" TextMode="MultiLine" runat="server" Width="350"
                                Height="75" MaxLength="500"></asp:TextBox>
                        </td>
                    </tr>
                    <tr runat="server" id="trSEOScript">
                        <td class="form_label" nowrap>
                            SEO Script:
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtSEOScript" TextMode="MultiLine" runat="server" Width="350" Height="75"
                                MaxLength="1000"></asp:TextBox>
                        </td>
                    </tr>
                    <asp:Repeater ID="rptTemplateHeaders" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td class="form_label" nowrap>
                                    <asp:Label ID="lblFriendlyName" runat="server" />
                                    <asp:HiddenField ID="hdnHeaderID" runat="server" />
                                </td>
                                <td width="100%">
                                    <asp:Panel ID="pnlUploadHeader" runat="server">
                                        <asp:FileUpload ID="uplImageUrl" runat="server" Width="300" />
                                        <asp:HyperLink ID="hypHelpText" runat="server" ImageUrl="~/App_Themes/EzEdit/images/help.png"
                                            NavigateUrl="#A" />
                                    </asp:Panel>
                                    <asp:Panel ID="pnlViewHeader" runat="server" Visible="false">
                                        <asp:HyperLink ID="hypImageUrl" runat="server" Target="_blank" CssClass="main" />
                                        <asp:ImageButton ID="btnDeleteHeader" runat="server" OnClientClick="return deleteHeader()"
                                            OnClick="DeleteHeader" ImageUrl="~/App_Themes/EzEdit/images/delete.png" ImageAlign="AbsBottom" />
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td class="form_label" valign="top" nowrap>
                                    <asp:Label ID="lblHeaderText" runat="server" />
                                </td>
                                <td width="100%">
                                    <VAM:TextBox ID="txtHeaderText" runat="server" Width="400" Height="100" TextMode="MultiLine"  />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr runat="server" id="trPageContent">
                        <td class="form_label" nowrap>
                            Page Content:
                        </td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlContentPageId" runat="server">
                                <asp:ListItem Value="0"> --- </asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trIsDefault" runat="server" visible="false">
                        <td class="form_label" nowrap>
                            Default Page:
                        </td>
                        <td width="100%" class="form">
                            <asp:RadioButtonList ID="rblDefaultPage" runat="server" RepeatDirection="Horizontal" />
                        </td>
                    </tr>
                    <tr>
                        <td class="form_label" nowrap>
                            Has Children:
                        </td>
                        <td width="100%" class="form">
                            <asp:RadioButtonList ID="rblHasChildren" runat="server" RepeatDirection="Horizontal" />
                        </td>
                    </tr>
                    <tr runat="server" visible="false">
                        <td class="form_label" nowrap>
                            Is Permanent:
                        </td>
                        <td width="100%" class="form">
                            <asp:RadioButtonList ID="rblIsPermanent" runat="server" RepeatDirection="Horizontal" />
                        </td>
                    </tr>
                    <tr>
                        <td class="form_label" nowrap>
                            Is Hidden:
                        </td>
                        <td width="100%" class="form">
                            <asp:RadioButtonList ID="rblIsHidden" runat="server" RepeatDirection="Horizontal" />
                        </td>
                    </tr>
                    <tr>
                        <td class="form_label" nowrap>
                            Is Secure:
                        </td>
                        <td width="100%" class="form">
                            <asp:RadioButtonList ID="rblIsSecure" runat="server" RepeatDirection="Horizontal" />
                        </td>
                    </tr>
                    <tr>
                        <td class="form_label" nowrap>
                            Is Searchable:
                        </td>
                        <td width="100%" class="form">
                            <asp:RadioButtonList ID="rblIsSearchable" runat="server" RepeatDirection="Horizontal" />
                        </td>
                    </tr>
                    <tr>
                        <td class="form_label" nowrap>
                            Display Sub-Menu:
                        </td>
                        <td width="100%" class="form">
                            <asp:RadioButtonList ID="rblDisplaySubMenu" runat="server" RepeatDirection="Horizontal" />
                        </td>
                    </tr>
                    <%--<tr>
                        <td class="form_label" nowrap>
                            Log Impressions:
                        </td>
                        <td width="100%" class="form">
                            <asp:RadioButtonList ID="rblLogImpressions" runat="server" RepeatDirection="Horizontal" />
                        </td>
                    </tr>--%>
                    <tr runat="server" visible="false">
                        <td class="form_label" nowrap>
                            Status:
                        </td>
                        <td width="100%" class="form">
                            <asp:DropDownList ID="ddlPageStatus" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trMemberPermissions" runat="server">
                        <td class="form_label" valign="top">
                            Member Permissions:
                        </td>
                        <td width="100%" class="form">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="2">
                                        <asp:RadioButtonList ID="rblMemberPermissions" runat="server" AutoPostBack="true">
                                            <asp:ListItem Text="All users can access this page." Value="0" />
                                            <asp:ListItem Text="Only the following member groups can access this page." Value="1" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="10">
                                    </td>
                                    <td>
                                        <asp:CheckBoxList ID="cblMemberCategories" runat="server" CssClass="main" Font-Italic="true" Enabled="false" />
                                        <VAM:RequiredListValidator ID="RequiredTextValidator1" runat="server" ControlIDToEvaluate="cblMemberCategories"
                                            SummaryErrorMessage="Choose at least one membership role from the list.">
                                            <EnablerContainer>
                                                <VAM:MultiCondition Name="MultiCondition" Operator="AND">
                                                    <Conditions>
                                                        <VAM:RequiredListCondition ControlIDToEvaluate="rblMemberPermissions" UnassignedIndex="0" />
                                                    </Conditions>
                                                </VAM:MultiCondition>
                                            </EnablerContainer>
                                        </VAM:RequiredListValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr runat="server" id="trMembershipFormID" visible="false">
                        <td class="form_label" nowrap>
                            Member Login Form:
                        </td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlMembershipFormID" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Value="0"><- Please Choose -></asp:ListItem>
                            </asp:DropDownList>
                            <VAM:RequiredListValidator runat="server" ControlIDToEvaluate="ddlMembershipFormID"
                                SummaryErrorMessage="Choose a membership form from the list." UnassignedIndex="0" />
                        </td>
                    </tr>
                    <tr>
                        <td class="form_label" valign="top">
                            EzEdit User Permissions:<span class="message">*</span>
                        </td>
                        <td width="100%" class="form">
                            <asp:ListBox ID="lstUserPermissions" runat="server" SelectionMode="Multiple"></asp:ListBox>
                            <VAM:RequiredListValidator ID="rfvUserPermissions" runat="server" SummaryErrorMessage="Choose at least one EzEdit user from the list."
                                ControlIDToEvaluate="lstUserPermissions" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <VAM:Button ID="btnSave" runat="server" Text="Save" CssClass="form-button" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" CssClass="form-button" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
