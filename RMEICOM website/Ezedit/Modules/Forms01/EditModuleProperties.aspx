<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false" CodeFile="EditModuleProperties.aspx.vb" Inherits="Ezedit_Modules_PR01_EditModuleProperties"%>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    
    <p>
        <asp:Label ID="lblModuleName" runat="server"/><asp:Label ID="lblPageTitle" runat="server" CssClass="pageTitle" />
    </p>
    <p>
        <asp:label id="lblAlert" runat="server" cssclass="message"></asp:label>
    </p>
    <p>
        <vam:validationsummary id="ValidationSummary1" runat="server" cssclass="message" headertext="Please correct the following errors: "></vam:validationsummary>
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
						    Form Page Type:<span class="message">*</span>
				        </td>
					    <td width="100%">
					         <asp:DropDownList ID="ddlFormPageTypeID" runat="server" TabIndex="2" AutoPostBack="true" AppendDataBoundItems="true">
					             <asp:listitem value="0"><-- Please Choose --></asp:listitem>
					         </asp:DropDownList>
					         <VAM:RequiredListValidator ControlIDToEvaluate="ddlFormPageTypeID" runat="Server" ID="RequiredListValidator1" SummaryErrorMessage="Form page type is required." UnassignedIndex="0"></VAM:RequiredListValidator>
					    </td>
				    </tr>
					<tr>
					    <td class="form_label" nowrap>
						    Display Code File:<span class="message">*</span>
				        </td>
					    <td width="100%">
					         <asp:DropDownList ID="ddlCodeFileID" runat="server" TabIndex="2">
					             <asp:listitem value="0"><-- Please Choose --></asp:listitem>
					         </asp:DropDownList>
					         <VAM:RequiredListValidator ControlIDToEvaluate="ddlCodeFileID" runat="Server" ID="RequiredListValidator2" SummaryErrorMessage="Code file is required." UnassignedIndex="0"></VAM:RequiredListValidator>
					    </td>
				    </tr>
				    <tr style="background-color:#F3F2F7;" runat="server" visible="false">
						<td class="form_label">
							Sort Order:
						</td>
						<td width="100%">
						    <asp:TextBox Id="txtSortOrder" width="50px" CssClass="form" TabIndex="2" runat="server" />
						</td>
					</tr>
					<tr>
					    <td class="form_label" nowrap>
						    Display Page:<span class="message">*</span>
				        </td>
					    <td width="100%">
					         <asp:DropDownList ID="ddlDisplayPageId" runat="server" TabIndex="2">
					             <asp:listitem value="0"><-- Please Choose --></asp:listitem>
					         </asp:DropDownList>
					         <VAM:RequiredListValidator ControlIDToEvaluate="ddlDisplayPageId" runat="Server" ID="rlvDisplayPageID" SummaryErrorMessage="Display Page is required." UnassignedIndex="0"></VAM:RequiredListValidator>
					    </td>
				    </tr>
					
					<asp:Repeater
                        Id="rptFields"
                        Runat="Server"
                        EnableViewState="True">
                        <ItemTemplate>
                            <tr id="trForms" runat="server">
                                <td id="tdFieldLabel" valign="top" runat="server" nowrap>
                                    <asp:Label ID="lblFieldLabel" runat="server" CssClass="form_label"></asp:Label>
                                </td>
                                <td class="table_text" valign="top" id="tdFormField" runat="server">
                                    <asp:PlaceHolder ID="plcFormField" runat="server"></asp:PlaceHolder>
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
                <asp:Button ID="btnSave" runat="server" Text=" Save " CssClass="button" />
                <asp:Button ID="btnDelete" runat="server" Text=" Delete " CssClass="button" />
                <asp:Button ID="btnCancel" runat="server" Text=" Cancel " CssClass="button" CausesValidation="False" />
			</td>
		</tr>
	</table>
</asp:Content>

