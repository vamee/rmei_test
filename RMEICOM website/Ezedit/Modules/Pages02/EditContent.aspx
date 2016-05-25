<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditContent.aspx.vb" Inherits="Ezedit_Modules_Pages01_EditContent" validateRequest="false" MasterPageFile="~/Ezedit/MasterPage.master"%>

<asp:Content id="EditContent" ContentPlaceHolderID="Main" visible="false" Runat="Server">
   
    <asp:panel runat="server" id="pnlEdit">
        <table width="700">
            <tr>
                <td>
                    <EzEdit:ContentEditor EditorWidth="600" EditorHeight="600" ID="ContentEditor" runat="server" />
                        
                        </td>
            </tr>
            
             <tr>
                <td align="center">
                    <asp:button id="btnPreview" runat="server" Text=" Preview " CssClass="button"></asp:button>&nbsp;
			        <asp:button id="btnCancel" runat="server" Text=" Cancel " CssClass="button"></asp:button>
		        </td>
            </tr>
        </table>
        <input type="hidden" name="preview" value="true" />
	</asp:panel>
	<asp:panel runat="server" id="pnlPreview" visible="false" align="center">
        <table cellpadding="5" width="100%" border="1" style="border-collapse: collapse" bordercolor="#000000" bgcolor="#7288AD">
            <tr>
                <td align="center">
                    <asp:button id="btnEdit" runat="server" Text=" Go Back & Edit " CssClass="form-button"></asp:button>
                    &nbsp;&nbsp;
                    <asp:button id="btnSaveChanges" runat="server" Text=" Save Changes " CssClass="form-button"></asp:button>
                    &nbsp;&nbsp;
                    <asp:button id="btnSaveVersion" runat="server" Text=" Save As New Version " CssClass="form-button"></asp:button>
                    &nbsp;&nbsp;
                    <asp:button id="btnCancel1" runat="server" Text=" Cancel " CssClass="form-button" onClick="btnCancel_Click"></asp:button>       
                </td>
            </tr>
        </table>
        
        <asp:literal id="litContent" runat="server"></asp:literal>
	</asp:panel>



</asp:Content>

