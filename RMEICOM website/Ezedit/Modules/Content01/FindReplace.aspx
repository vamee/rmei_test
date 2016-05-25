<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="FindReplace.aspx.vb" Inherits="Ezedit_Modules_Content01_FindReplace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <asp:Label ID="lblalert" runat="server" CssClass="alert" />
    <br /><br />
    <table cellpadding="3" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="lblFind" runat="server" Text="Find:" CssClass="form-label" />
            </td>
            <td>
                <asp:TextBox ID="txtFind" runat="server" CssClass="form-textbox" Width="250" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Replace:" CssClass="form-label" />
            </td>
            <td>
                <asp:TextBox ID="txtReplace" runat="server" CssClass="form-textbox" Width="250" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" CssClass="form-button" Text="Go!" />
            </td>
        </tr>
    </table>
</asp:Content>
