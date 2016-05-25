<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DisplayNews_SuccessStories.ascx.vb"
    Inherits="PR01_DisplayNews_SuccessStories" %>
<asp:Repeater ID="rptPR01" runat="server">
    <HeaderTemplate>
        <asp:Label ID="lblCategoryName" runat="server"></asp:Label>
        <%#Eval("CategoryName")%>
        <table cellpadding='3' cellspacing='0' width='100%' border='0'>
    </HeaderTemplate>
    <ItemTemplate>

                    <tr>
                        <td width="125" align="center" class='table-row'>
                            <asp:Image ID="imgImageUrl" runat="server" Visible="false" />
                        </td>
                        <td valign='top' class='table-row'>
                            <b>
                                <%#FormatDateTime(Eval("DisplayDate"), DateFormat.ShortDate)%></b><br />
                            <b>
                                <asp:HyperLink ID="hypArticle" runat="server"></asp:HyperLink></b><br />
                            <%#Eval("ArticleSummary")%>
                        </td>
                    </tr>
                
    </ItemTemplate>
    <AlternatingItemTemplate>
        
                    <tr>
                        <td width="125" align="center" class='table-altrow'>
                            <asp:Image ID="imgImageUrl" runat="server" Visible="false" />
                        </td>
                        <td valign='top' class='table-altrow'>
                            <b>
                                <%#FormatDateTime(Eval("DisplayDate"), DateFormat.ShortDate)%></b><br />
                            <b>
                                <asp:HyperLink ID="hypArticle" runat="server"></asp:HyperLink></b><br />
                            <%#Eval("ArticleSummary")%>
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