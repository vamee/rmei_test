<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="EditModuleProperties.aspx.vb" Inherits="Ezedit_Modules_PR01_EditModuleProperties" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <p>
        <asp:Label ID="lblModuleName" runat="server" /><asp:Label ID="lblPageTitle" runat="server"
            CssClass="pageTitle" />
    </p>
    <p>
        <asp:Label ID="lblAlert" runat="server" CssClass="alert" />
    </p>
    <p>
        <VAM:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert" HeaderText="Please correct the following errors: "
            AutoUpdate="true" ScrollIntoView="Top" />
    </p>
    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td>
                <table border="0" width="100%" cellpadding="5" cellspacing="0">
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td class="form-label" nowrap>
                            Display Code File:
                            <VAM:RequiredListValidator ControlIDToEvaluate="ddlCodeFileID" runat="Server" ID="RequiredListValidator2"
                                SummaryErrorMessage="Code file is required." ErrorMessage="*" UnassignedIndex="0" />
                        </td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlCodeFileID" runat="server" AutoPostBack="true" AppendDataBoundItems="true">
                                <asp:ListItem Value="0"><-- Please Choose --></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="form-label" nowrap>
                            Display Page:
                            <VAM:RequiredListValidator ControlIDToEvaluate="ddlDisplayPageID" runat="Server"
                                ID="rlvDisplayPageID" SummaryErrorMessage="Display page is required." ErrorMessage="*"
                                UnassignedIndex="0" />
                        </td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlDisplayPageID" runat="server">
                                <asp:ListItem Value="0"><-- Please Choose --></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr style="background-color: #F3F2F7;" runat="server" visible="false">
                        <td class="form-label">
                            Sort Order:
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtSortOrder" Width="50px" CssClass="form-textbox" runat="server" />
                        </td>
                    </tr>
                    <asp:Repeater ID="rptFields" runat="Server" EnableViewState="True">
                        <ItemTemplate>
                            <tr id="trForms" runat="server">
                                <td id="tdFieldLabel" valign="top" runat="server" nowrap>
                                    <asp:Label ID="lblFieldLabel" runat="server" CssClass="form-label"></asp:Label>
                                </td>
                                <td class="table_text" valign="top" id="tdFormField" runat="server">
                                    <asp:PlaceHolder ID="plcFormField" runat="server" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <br />
                <VAM:Button ID="btnSave" runat="server" Text=" Save " CssClass="form-button" />
                <asp:Button ID="btnDelete" runat="server" Text=" Delete " CssClass="form-button" />
                <asp:Button ID="btnCancel" runat="server" Text=" Cancel " CssClass="form-button"
                    CausesValidation="False" />
            </td>
        </tr>
    </table>
</asp:Content>
