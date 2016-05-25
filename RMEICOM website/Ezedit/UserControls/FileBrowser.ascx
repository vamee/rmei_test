<%@ Control Language="VB" AutoEventWireup="false" CodeFile="FileBrowser.ascx.vb"
    Inherits="Ezedit_UserControls_FileBrowser" %>

<script type="text/javascript">
    var txtFileName;

    function ShowFileDialog(controlID, fileUrl, defaultPath, fileExtensions) {
        txtFileName = controlID;
        window.radopen("/EzEdit/Tools/FileBrowser.aspx?FilePath=" + fileUrl + "&DefaultPath=" + defaultPath + "&ControlID=" + controlID + "&FileExtensions=" + fileExtensions, "FileDialog");
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
        while ((match = myregexp.exec(output)) != null && match.length > 1 && match[1] != '') {
            binVal = parseInt(match[1].substr(1),16);
            thisString = String.fromCharCode(binVal);
            output = output.replace(match[1], thisString);
        }
        return output;
    }
</script>

<asp:Panel ID="pnlUpload" runat="server" Visible="false">
    <table>
        <tr>
            <td>
                <asp:TextBox ID="txtFileName" runat="server" CssClass="form-textbox" Width="280" />
                <asp:Button ID="btnFileName" runat="server" Text="..." />
                <VAM:RequiredTextValidator ID="rtvFileName" runat="server" ControlIDToEvaluate="txtFileName"
                    Enabled="false" />
            </td>
            <td>
                <asp:LinkButton ID="btnCancel" runat="server" CssClass="main" Text="Cancel" Visible="false" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="pnlLink" runat="server" Visible="false">

    <script language="javascript" type="text/javascript">
        function confirmFileDelete() {
            if (confirm("Are you sure you want to remove this file?")) {
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
                    ToolTip="Remove File" OnClientClick="return confirmFileDelete();" />
            </td>
        </tr>
    </table>
</asp:Panel>
<RadControls:RadWindowManager ID="RadWindowManager1" runat="server" VisibleStatusbar="false"
    ReloadOnShow="true" Width="400" Height="400" Skin="Default" BackColor="#EEEEEE">
    <Windows>
        <RadControls:RadWindow ID="FileDialog" runat="server" Title="Choose a File" Left="150px"
            ReloadOnShow="true" Modal="true" VisibleStatusbar="false" />
    </Windows>
</RadControls:RadWindowManager>
