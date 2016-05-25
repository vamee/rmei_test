<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false" CodeFile="EditItem2.aspx.vb" Inherits="Ezedit_Modules_PR01_EditArticle2" ValidateRequest="false"%>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <p>
        <asp:Literal ID="litPageTitle" runat="server"/>
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
			    Type:<span class="message">*</span></td>
		    <td width="100%">
		        <asp:DropDownList ID="ddlModuleTypeID" runat="server" TabIndex="1" AutoPostBack="true"></asp:DropDownList>
			</td>
	    </tr>
	    <tr>
		    <td class="form_label" nowrap>Date:<span class="message">*</span></td>
		    <td width="100%">
		        <date:DateTextBox Id="pdpDate" runat="server" cssclass="main" xMinDate="1/1/1940" xMaxDate="1/1/2025" ></date:DateTextBox>
		        <vam:RequiredTextValidator ID="RequiredTextValidator1" ControlIDToEvaluate="pdpDate" ErrorMessage="Date is Required." runat="server">
                    <ErrorFormatterContainer><vam:TextErrorFormatter Display="None" /></ErrorFormatterContainer>                
                </vam:RequiredTextValidator>
                <vam:DataTypeCheckValidator ID="DataTypeCheckValidator1" DataType="date" ControlIDToEvaluate="pdpDate" SummaryErrorMessage="Date is Invalid." runat="server">
                    <ErrorFormatterContainer><vam:TextErrorFormatter Display="None" /></ErrorFormatterContainer>   
                </vam:DataTypeCheckValidator>
		    </td>
	    </tr>
	    <tr>
		    <td class="form_label" nowrap>
			    Title:<span class="message">*</span></td>
		    <td width="100%">
		        <vam:textbox id="txtResourceName" Width="600" runat="server" tabindex="3"></vam:textbox>
		        <vam:RequiredTextValidator ControlIDToEvaluate="txtResourceName" ErrorMessage="Title is Required." ID="RequiredFieldValidator2" runat="server">
		            <ErrorFormatterContainer><vam:TextErrorFormatter Display="None" /></ErrorFormatterContainer>
		        </vam:RequiredTextValidator>
			</td>
	    </tr>	    
	    
	    <tr>
		    <td class="form_label" valign="top" nowrap>Summary:</td>
		    <td width="100%">
		       <asp:textbox id="txtSummary" runat="server" width="600" MaxLength="255" tabindex="8" Rows="5" TextMode="MultiLine"></asp:textbox>
		    </td>
		</tr>
		  <tr>
		    <td class="form_label" valign="top" nowrap>Keywords:</td>
		    <td width="100%">
		       <asp:textbox id="txtKeywords" runat="server" width="600" MaxLength="255" tabindex="8" Rows="5" TextMode="MultiLine"></asp:textbox>
		    </td>
		</tr>
		<asp:Panel ID="pnlExternalURL" runat="server" Visible="false" EnableViewState="false">
		<tr>
		    <td class="form_label" valign="top" nowrap>URL:</td>
		    <td width="100%">
		       <asp:textbox id="txtURL" runat="server" width="350" MaxLength="255" tabindex="8" Rows="5"></asp:textbox>
		    </td>
		</tr>
		</asp:Panel>
		<asp:Panel ID="pnlContent" runat="server" Visible="false" EnableViewState="false">
		<tr>
            <td class="form_label" nowrap valign="top">
                Text:<span class="message"></span></td>
            <td width="100%">
                 <EzEdit:ContentEditor EditorWidth="600" EditorHeight="500" ID="ContentEditor" runat="server" />
            </td>
        </tr>
		</asp:panel>
		<asp:Panel ID="pnlFile" runat="server" Visible="false" EnableViewState="false">
		<tr>
            <td class="form_label" nowrap valign="top">
                File:<span class="message"></span></td>
            <td width="100%">
                 <asp:FileUpload ID="uplFile" runat="server" CssClass="form" Width="350" />
                 <asp:Label ID="lblFile" runat="server" Text="" CssClass="form"></asp:Label>
            </td>
        </tr>
		</asp:panel>
		<tr>
		    <td colspan="2">
			    <hr />
		    </td>
	    </tr>
	    <tr>
		    <td colspan="2" align="center">
                <asp:Button ID="btnSave" runat="server" Text=" Save " CausesValidation="true" />
                <asp:Button ID="btnCancel" runat="server" Text=" Cancel " CausesValidation="false" />
            </td>
	    </tr>
    </table>
</asp:Content>

