<%@ Control Language="c#" AutoEventWireup="false" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" CodeBehind="ImageDialogCaller.ascx.cs" Inherits="Telerik.WebControls.EditorControls.ImageDialogCaller" %>
<nobr><input tabindex="<%=this.TabIndex%>" type="text" class="Text" style="height:20px;" size="40" id="<%=this.ClientID%>_resultTextBox">&nbsp;
<button id='<%=this.ClientID%>_dialogOpenerButton' style="margin-bottom:1px;height:20px;border:1px solid #999999;" onclick=" return <%=this.ClientID%>.CallImageDialog();" onfocus="this.blur();">
&nbsp;...&nbsp;
</button></nobr>
<asp:literal id="ltScriptHolder" runat="server"></asp:literal>