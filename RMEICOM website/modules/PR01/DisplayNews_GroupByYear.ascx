<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DisplayNews_GroupByYear.ascx.vb" Inherits="PR01_DisplayNews_GroupByYear"%>
<asp:Panel ID="pnlArticles" runat="server">
    <asp:Label ID="lblCategoryname" runat="server"></asp:Label>
    <asp:Literal ID="ltrArticles" runat="server" />
</asp:Panel>

<span id="ArticleDetail" runat="server" visible="False">
<h1><asp:Label ID="lblArticleTitle" runat="server"></asp:Label></h1>
<asp:Label ID="lblArticleText" runat="server" CssClass="main"></asp:Label>
</span>
