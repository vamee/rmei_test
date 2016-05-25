<%@ Control Language="VB" AutoEventWireup="false" CodeFile="FileUpload.ascx.vb" Inherits="Ezedit_UserControls_FileUpload" %>
<asp:Panel ID="pnlUpload" runat="server" Visible="false">
    <table cellpadding="0" cellspacing="0" class="table">
        <tr>
            <td>
                <asp:FileUpload ID="uplFileUpload" runat="server" />
                <VAM:RequiredTextValidator ID="RequiredTextValidator" runat="server" ControlIDToEvaluate="uplFileUpload"
                    ErrorMessage="*" Enabled="false" />
                <VAM:CompareToStringsValidator ID="CompareToStringsValidator" runat="server" ControlIDToEvaluate="uplFileUpload"
                    ErrorMessage="*" Enabled="false" MatchTextRule="EndsWith" CaseInsensitive="true" />
            </td>
            <td>
                <asp:LinkButton ID="btnCancel" runat="server" CssClass="main" Text="Cancel" Visible="false" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="pnlLink" runat="server" Visible="false">
    <script language="javascript" type="text/javascript">
        function confirmImageDelete() {
            if (confirm("Are you sure you want to delete this image?")) {
                return true;
            }else{
                return false;
            }
        }
    </script>
    <table cellpadding="2" cellspacing="0" class="table">
        <tr>
            <td>
                <asp:HyperLink ID="hypFileUrl" runat="server" CssClass="main" Target="_blank" />&nbsp;
                <asp:Label ID="lblFileSize" runat="server" />
            </td>
            <td>
                <asp:ImageButton ID="ibtnEdit" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/edit.png"
                    ToolTip="Replace File" />
                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/App_Themes/EzEdit/Images/delete.png"
                    ToolTip="Delete File" OnClientClick="return confirmImageDelete();" />
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdnVirtualFilePath" runat="server" />
</asp:Panel>
