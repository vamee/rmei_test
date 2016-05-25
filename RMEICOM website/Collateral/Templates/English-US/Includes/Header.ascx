<%@ Control Language="VB" ClassName="Header" %>

<script language="vbscript" runat="server">
    Dim Keywords As String = ""
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        If Request("Keywords") IsNot Nothing Then Keywords = Request("Keywords")
    End Sub
</script>

<a id="logo" href="/index.htm">
    <img src="/Collateral/Templates/English-US/images/top_RMEI-logo.jpg" alt="RMEI logo" title="RMEI" /></a>
<table id="utilContainer" cellspacing="0" cellpadding="0">
    <tr>
        <td valign="bottom">
            <ul id="utilities">
                <li><a href="index.htm">Home</a></li><li>|</li><li><a href="about-us-contact-us.htm">
                    Contact Us</a></li><li>|</li><li>1.866.770.RMEI (7634)</li></ul>
        </td>
        <td height="18" class="searchInput">
            <form action="/search.htm" method="post">
            <input style="float: left;display: block;" id="searchBox" type="text" name="Keywords" value="<%=Keywords%>" /><input
                type="image" src="/Collateral/Templates/English-US/images/search_btn.gif" style="display: block;" /><%--<asp:ImageButton
                ID="btnSearch" runat="server" Style="display: block;" ImageUrl="/Collateral/Templates/English-US/images/search_btn.gif"
                PostBackUrl="/search.htm" />--%><!--<img style="float:left;" src="Collateral/Templates/English-US/images/search_btn.gif" border="0" />-->
            </form>
        </td>
    </tr>
</table>
<div id="topNav">
    <template:TopNavigation ID="HorizontalNavigation1" runat="server" MenuLevels="3"
        TemplateID="1" />
</div>
