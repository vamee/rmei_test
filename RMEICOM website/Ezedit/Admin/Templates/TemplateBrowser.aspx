<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TemplateBrowser.aspx.vb"
    Inherits="Ezedit_Admin_Templates_TemplateBrowser" EnableEventValidation="false"
    Title="Template Browser" %>

<%@ Register Assembly="RadTabStrip.Net2, Version=3.4.0.0, Culture=neutral, PublicKeyToken=a87324b017ca21ce"
    Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>

    <script language="javascript" type="text/javascript">
       
    function GetRadWindow() {   
        var oWindow = null;   
        if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog   
        else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;//IE (and Moz az well)   
               
        return oWindow;   
    }   
  
    function Close() {   
        GetRadWindow().Close();        
    }
    
    function CloseAndRebind(fileName) {
        var radWindow = GetRadWindow();
        radWindow.BrowserWindow.refreshData(fileName);
        radWindow.Close();
    }
    
    

    </script>

</head>
<body style="background-color: #D2CEC2;">
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblCurrentDirectoryLabel" runat="server" CssClass="form-label" ForeColor="#999999"
            Text="Current Directory:" />
        <asp:Label ID="lblCurrentDirectory" runat="server" CssClass="main" Width="250" BorderStyle="Solid"
            BorderColor="#999999" BorderWidth="1" BackColor="White" />
        <br />
        <br />
        <radTS:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1">
            <Tabs>
                <radTS:Tab PageViewID="pvBrowse" Text="Browse">
                </radTS:Tab>
                <radTS:Tab PageViewID="pvUpload" Text="Upload">
                </radTS:Tab>
            </Tabs>
        </radTS:RadTabStrip>
        <radTS:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0" Width="365">
            <radTS:PageView ID="pvBrowse" runat="server" Width="100%">
                <asp:GridView ID="gdvFileNames" runat="server" AutoGenerateColumns="false" Width="100%"
                    CellPadding="3" CellSpacing="0" HeaderStyle-CssClass="table-header" AlternatingRowStyle-CssClass="table-altrow"
                    RowStyle-CssClass="table-row" HeaderStyle-HorizontalAlign="Left">
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Name">
                            <ItemTemplate>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Image ID="imgItemImage" runat="server" ImageUrl='<%#Eval("ItemImage")%>' />
                                        </td>
                                        <td style="width: 5px;">
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lbtnItemName" runat="server" Text='<%#Eval("ItemName")%>' CommandName='<%#Eval("ItemType")%>'
                                                CommandArgument='<%#Eval("ItemPath")%>' OnClick="lbtnItemName_Click" />
                                            <asp:Label ID="lblItemName" runat="server" Text='<%#Eval("ItemName")%>' Visible="false"/>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </radTS:PageView>
            <radTS:PageView ID="pvUpload" runat="server" Width="100%">
                <table cellpadding="3" cellspacing="0" width="100%" style="border-width: 1px; border-color: #999999;
                    border-style: solid; background-color: #EEEEEE;">
                    <tr>
                        <td class="table-header">
                            Upload a File
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="table-altrow">
                            <table width="100%" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td>
                                        Master File:
                                    </td>
                                    <td>
                                        <asp:FileUpload ID="uplFileName" runat="server" CssClass="form-textbox" Width="250" />
                                        <VAM:RequiredTextValidator ID="RequiredTextValidator1" runat="server" ControlIDToEvaluate="uplFileName"
                                            SummaryErrorMessage="Please choose a .master file to upload." />
                                        <VAM:CompareToStringsValidator ID="CompareToStringsValidator1" runat="server" ControlIDToEvaluate="uplFileName"
                                            SummaryErrorMessage="Only files with (.master) extensions are allowed." MatchTextRule="EndsWith"
                                            CaseInsensitive="true">
                                            <Items>
                                                <VAM:CompareToStringsItem Value=".master" />
                                            </Items>
                                        </VAM:CompareToStringsValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Code Behind:
                                    </td>
                                    <td>
                                        <asp:FileUpload ID="uplCodeBehind" runat="server" CssClass="form-textbox" Width="250" />
                                        <VAM:CompareToStringsValidator ID="CompareToStringsValidator2" runat="server" ControlIDToEvaluate="uplCodeBehind"
                                            SummaryErrorMessage="Only codebehind files with (.master.vb) extensions are allowed." MatchTextRule="EndsWith"
                                            CaseInsensitive="true" IgnoreBlankText="true">
                                            <Items>
                                                <VAM:CompareToStringsItem Value=".master.vb" />
                                            </Items>
                                        </VAM:CompareToStringsValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxOverwriteFile" runat="server" /><span class='form-label'>Overwrite
                                            if file with such name exists?</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <VAM:ValidationSummary runat="server" CssClass="alert" DisplayMode="Table" />
                                        <asp:Label ID="lblAlert" runat="server" CssClass="alert" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <VAM:Button ID="btnUpload" runat="server" CssClass="form-button" Text="Upload" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </radTS:PageView>
        </radTS:RadMultiPage>
    </div>
    </form>
</body>
</html>
