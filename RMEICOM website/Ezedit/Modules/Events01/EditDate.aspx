<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="EditDate.aspx.vb" Inherits="Ezedit_Modules_Events01_EditDate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <p>
        <a href="../Default.aspx?ModuleKey=Events01" class="pageTitle">Events</a>
        <asp:Label ID="lblPageTitle" CssClass="pagetitle" runat="server" Text=""></asp:Label>
    </p>
    <p>
        <asp:Label ID="lblAlert" runat="server" CssClass="alert" />
    </p>
    <p>
        <VAM:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="message"
            HeaderText="Please correct the following errors: " />
    </p>
    <table border="0" width="100%" cellpadding="5" cellspacing="0" id="table1">
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>
        <tr>
            <td class="form-label">
                Event Date:<span class="alert">*</span>
            </td>
            <td width="100%">
                <Date:DateTextBox ID="txtEventDate" runat="server" CssClass="form-textbox" />
                <VAM:RequiredTextValidator ID="RequiredTextValidator1" ControlIDToEvaluate="txtEventDate"
                    SummaryErrorMessage="Event date is required." runat="server" />
            </td>
        </tr>
        <tr>
            <td class="form-label">
                Location:<span class="alert">*</span>
            </td>
            <td width="100%">
                <asp:TextBox ID="txtLocation" runat="server" Width="200" MaxLength="200" />
                <VAM:RequiredTextValidator ID="RequiredTextValidator5" ControlIDToEvaluate="txtLocation"
                    SummaryErrorMessage="Location is required." runat="server" />

            </td>
        </tr>
        <tr>
            <td class="form-label" nowrap>
                Duration:
            </td>
            <td width="100%">
                <asp:TextBox ID="txtDuration" runat="server" Width="200" MaxLength="100" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <VAM:Button ID="btnSave" CssClass="form-button" runat="server" Text=" Save " />
                <asp:Button ID="btnDelete" CssClass="form-button" runat="server" Text=" Delete " ConfirmMessage="Are you sure you want to delete this date?"
                    Visible="false" />
                <asp:Button ID="btnCancel" CssClass="button" runat="server" Text=" Cancel " CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Content>
