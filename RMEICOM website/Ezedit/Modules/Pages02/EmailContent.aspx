<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EmailContent.aspx.vb" Inherits="Ezedit_Modules_Pages01_EmailContent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Email Link to a Colleague</title>
</head>
<body>
    <form id="form1" runat="server">
    <span class="page-header">Email Link to a Colleague</span>
    <hr />
        
         <asp:validationsummary id="ValidationSummary1" runat="server" cssclass="message" headertext="Please correct the following errors: " ></asp:validationsummary>

        <table border="0" width="100%" cellpadding="5" cellspacing="0" id="table1">
		    <tr>
			    <td class="form_label" nowrap>
				    Email To:<span class="message">*</span>
                  </td>
                <td width="100%">
                    <asp:TextBox ID="txtEmailTo" runat="server" Width="250"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmailTo" ErrorMessage="Email To is Required."  Display="None"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmailTo" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="Email To is not a valid email address."  Display="None"></asp:RegularExpressionValidator>
			    </td>
		    </tr>
		    <tr>
			    <td class="form_label" nowrap>
				    Email CC:
			    </td>
			    <td width="100%">
			         <asp:TextBox ID="txtEmailCC" runat="server" Width="250"></asp:TextBox>
			         <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEmailCC" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="Email CC is not a valid email address."  Display="None"></asp:RegularExpressionValidator>
		        </td>
		    </tr>
		    <tr>
			    <td class="form_label" nowrap>
				    Email Subject:<span class="message">*</span></td>
			    <td width="100%">
                    <asp:TextBox ID="txtEmailSubject" runat="server" Width="250"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEmailSubject" ErrorMessage="Email Subject is Required."  Display="None"></asp:RequiredFieldValidator>
			    </td>
		    </tr>
		    <tr>
			    <td class="form_label" nowrap valign="top">
				    Message:
			    </td>
			    <td width="100%">
                    <asp:TextBox ID="txtEmailMessage" TextMode="MultiLine" Rows="7" columns="52"  runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtEmailMessage" ErrorMessage="Email Message is Required."  Display="None"></asp:RequiredFieldValidator>
			    </td>
		    </tr>
		    <tr>
			    <td colspan="2" align="center">
                    <asp:Button ID="btnSubmit" runat="server" Text=" Send " />
                    <asp:Button ID="btnCancel" runat="server" Text=" Cancel " CausesValidation="false" />
		        </td>
		    </tr>
        </table>
    </form>
</body>
</html>
