<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false" CodeFile="EditItem.aspx.vb" Inherits="Ezedit_Modules_Events01_EditItem" ValidateRequest="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="javascript">
function deleteFile(strURL) {
    if(confirm("Are you sure you want to delete this file?")) {
        window.location.href=strURL;
    }
}
</script>
    <p>
        <asp:Literal ID="litPageTitle" runat="server" />
    </p>
    <p>
        <asp:label id="lblAlert" runat="server" cssclass="message"></asp:label>
    </p>
    <p>
        <vam:validationsummary id="ValidationSummary1" runat="server" cssclass="message" headertext="Please correct the following errors: "></vam:validationsummary>
    </p>
    <table border="0" width="100%" cellpadding="5" cellspacing="0" id="table1">
	    <tr>
		    <td colspan="2">
			    <hr />
		    </td>
	    </tr>
	    <tr>
		    <td class="form_label" nowrap>
			    Category:<span class="message">*</span></td>
		    <td width="100%">
		        <asp:DropDownList ID="ddlCategoryId" runat="server" TabIndex="1"></asp:DropDownList>
			</td>
	    </tr>
	    <tr>
		    <td class="form_label" nowrap>
			    Title:<span class="message">*</span></td>
		    <td width="100%">
		        <vam:textbox id="txtResourceName" Width="400" runat="server" tabindex="3"></vam:textbox>
		        <vam:RequiredTextValidator ControlIDToEvaluate="txtResourceName" ErrorMessage="Title is Required." ID="RequiredFieldValidator2" runat="server">
		            <ErrorFormatterContainer><vam:TextErrorFormatter Display="None" /></ErrorFormatterContainer>
		        </vam:RequiredTextValidator>
			</td>
	    </tr>
	    <tr runat="server" visible="false">
		    <td class="form_label" nowrap>
			    Sub Category:</td>
		    <td width="100%">
		         <asp:DropDownList ID="ddlResourceCategory" runat="server" TabIndex="1"></asp:DropDownList>
		         <vam:textbox id="txtResourceCategory" runat="server" tabindex="3"></vam:textbox> 
		    </td>
	    </tr>
	    
	    <tr>
		    <td class="form_label" valign="top" nowrap>Summary:</td>
		    <td width="100%">
		       <asp:textbox id="txtSummary" runat="server" width="400" MaxLength="255" tabindex="8" Rows="5" TextMode="MultiLine"></asp:textbox>
		    </td>
		</tr>
		  <tr>
		    <td class="form_label" valign="top" nowrap>Keywords:</td>
		    <td width="100%">
		       <asp:textbox id="txtKeywords" runat="server" width="400" MaxLength="255" tabindex="8" Rows="5" TextMode="MultiLine"></asp:textbox>
		    </td>
		</tr>
		<tr>
		    <td class="form_label" valign="top" nowrap>URL:</td>
		    <td width="100%">
		       <asp:TextBox ID="txtArchiveUrl" runat="server" Width="400"></asp:TextBox>
		    </td>
		</tr>
		<tr>
		    <td class="form_label" valign="top" nowrap>Image:</td>
		    <td width="100%">
		       <asp:FileUpload id="uplImageUrl" runat="server" width="400"></asp:FileUpload>
		       <asp:Label ID="lblImageUrl" runat="server" Text="" CssClass="form"></asp:Label>
		    </td>
		</tr>

		<tr>
            <td class="form_label" nowrap valign="top">
                Event Detail:<span class="message">*</span></td>
            <td width="100%">
                 <EzEdit:ContentEditor EditorWidth="600" EditorHeight="600" ID="ContentEditor" runat="server" />
            </td>
        </tr>
		<tr>
		    <td colspan="2">
			    <hr />
		    </td>
	    </tr>
	    <tr>
		    <td colspan="2" align="center">
                <vam:Button ID="btnSave" CssClass="button" runat="server" Text=" Save " />
                <vam:Button ID="btnCancel" CssClass="button"  runat="server" Text=" Cancel " CausesValidation="false" />
            </td>
	    </tr>
    </table>
</asp:Content>

