<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DisplayNews_ExternalLinks.ascx.vb"
    Inherits="PR01_DisplayNews_ExternalLinks" %>
<table width="100%" cellpadding="3" cellspacing="0">
    <tr>
        <td align="left" class="table-header-app">
            <asp:Label ID="lblCategoryName" runat="server" />
        </td>
        <td align="right" class="table-header-app">
            <asp:HyperLink ID="hypRss" runat="server" ImageUrl="/Collateral/Images/Common/rss.gif"
                Visible="false" />
        </td>
    </tr>
</table>
<asp:Repeater ID="rptPR01" runat="server">
    <HeaderTemplate>
        
        <table cellpadding='0' cellspacing='0' width='100%' border='0'>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td class='table-row'>
                <table cellpadding='0' cellspacing='0' width='100%' border='0'>
                    <tr>
                        <td width="160">
                            <asp:Image ID="imgImageUrl" runat="server" Visible="false" Width="150" />
                        </td>
                        <td valign='top'>
                            <b>
                                <%#FormatDateTime(Eval("DisplayDate"), DateFormat.ShortDate)%></b><br />
                            <b>
                                <asp:HyperLink ID="hypArticle" runat="server"></asp:HyperLink></b><br />
                            <%#Eval("ArticleSummary")%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
        <tr>
            <td class='table-altrow'>
                <table cellpadding='0' cellspacing='0' width='100%' border='0'>
                    <tr>
                        <td width="160">
                            <asp:Image ID="imgImageUrl" runat="server" Visible="false" Width="150" />
                        </td>
                        <td valign='top'>
                            <b>
                                <%#FormatDateTime(Eval("DisplayDate"), DateFormat.ShortDate)%></b><br />
                            <b>
                                <asp:HyperLink ID="hypArticle" runat="server"></asp:HyperLink></b><br />
                            <%#Eval("ArticleSummary")%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </AlternatingItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
<span id="ArticleDetail" runat="server" visible="False">
    <h1><asp:Label ID="lblArticleTitle" runat="server" CssClass="page-subheader"></asp:Label></h1>
    <asp:Label ID="lblArticleText" runat="server" CssClass="main"></asp:Label>
</span>