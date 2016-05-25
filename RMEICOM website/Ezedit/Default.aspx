<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="Ezedit_Default" MasterPageFile="~/Ezedit/MasterPage.master" %>

<asp:content ContentPlaceHolderID="Main" runat="server">
<p class="header">
	Welcome <%= Session("EzEditName") %>!
</p>
</asp:content>

