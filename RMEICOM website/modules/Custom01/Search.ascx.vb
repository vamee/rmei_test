
Partial Class modules_Custom01_Search
    Inherits System.Web.UI.UserControl

    Dim PageSize As Integer = 12

    Protected Sub SqlDataSource1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles SqlDataSource1.Load
        Dim Keywords As String = Request("Keywords")

        If Len(Keywords) > 0 Then
            Dim SqlBuilder As New StringBuilder
            SqlBuilder.Append("SELECT PageKey, PageName, MenuName, TitleTag, MetaDescription, MetaKeywords, Content FROM qryPageContent WHERE ")
            SqlBuilder.Append("PageTypeID = 1 ")
            SqlBuilder.Append("AND LanguageID = " & Session("LanguageID") & " ")
            SqlBuilder.Append("AND StatusID = 20 ")
            SqlBuilder.Append("AND IsSearchable = 'True' ")
            SqlBuilder.Append("AND (")

            Dim aryKeywords As Array = Keywords.Split(" ")

            For i As Integer = 0 To UBound(aryKeywords)
                SqlBuilder.Append("(TitleTag LIKE '%" & aryKeywords(i).ToString.Trim & "%' OR MetaKeywords LIKE '%" & aryKeywords(i).ToString.Trim & "%' OR Content LIKE '%" & aryKeywords(i).ToString.Trim & "%' OR MetaDescription LIKE '%" & aryKeywords(i).ToString.Trim & "%' OR MenuName LIKE '%" & aryKeywords(i).ToString.Trim & "%')")
                If i < UBound(aryKeywords) Then SqlBuilder.Append(" OR ")
            Next
            SqlBuilder.Append(")")

            ' Response.Write(SqlBuilder)
            ' Response.Write("<br><br>")
            SqlDataSource1.SelectCommand = SqlBuilder.ToString
            '
        End If
    End Sub

    Protected Sub gdvSearchResults_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvSearchResults.Load
        gdvSearchResults.PageSize = PageSize
    End Sub

    Protected Sub gdvSearchResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvSearchResults.RowDataBound
        If e.Row.RowType = DataControlRowType.Pager Then
            Dim btnNext As Button = e.Row.FindControl("btnNext")
            Dim btnPrev As Button = e.Row.FindControl("btnPrev")
            Dim lblPageCount As Label = e.Row.FindControl("lblPageCount")

            btnPrev.PostBackUrl = Request.RawUrl.ToString
            btnNext.PostBackUrl = Request.RawUrl.ToString

            lblPageCount.Text = "Page " & (gdvSearchResults.PageIndex + 1) & " of " & gdvSearchResults.PageCount

            If gdvSearchResults.PageIndex > 0 Then
                btnPrev.Enabled = True
            Else
                btnPrev.Enabled = False
            End If

            If (gdvSearchResults.PageIndex + 1) < gdvSearchResults.PageCount Then
                btnNext.Enabled = True
            Else
                btnNext.Enabled = False
            End If
        End If

        If e.Row.DataItem IsNot Nothing Then
            Dim hypPageName As HyperLink = e.Row.FindControl("hypPageName")
            Dim lblPageSummary As Label = e.Row.FindControl("lblPageSummary")
            Dim DataRow As System.Data.DataRowView = CType(e.Row.DataItem, System.Data.DataRowView)

            hypPageName.Text = DataRow.Item("MenuName")
            hypPageName.NavigateUrl = "/" & DataRow.Item("PageKey") & ".htm"

            Dim Summary As String = stripHTML(DataRow.Item("Content").ToString)

            If Summary.Length > 250 Then

                lblPageSummary.Text = Summary.Substring(0, 250)
            Else
                lblPageSummary.Text = Summary
            End If
        End If

    End Sub


    Function stripHTML(ByVal strHTML As String) As String
        Dim Result As String = ""

        If strHTML.Length > 0 Then
            Dim RegEx As New Regex(RegexOptions.IgnoreCase)
            Result = RegEx.Replace(strHTML, "<(.+?)>", "")
            Result.Replace("<", "&lt;")
            Result.Replace(">", "&gt;")
        End If

        Return Result.Trim
    End Function

    
    Protected Sub PR01Data_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles PR01Data.Load
        Dim Keywords As String = Request("Keywords")

        If Len(Keywords) > 0 Then
            Dim SqlBuilder As New StringBuilder
            SqlBuilder.Append("SELECT ResourceID, ResourceName, ArticleSummary, ArticleText, DeliveryPageKey FROM qryArticles WHERE ")

            Dim aryKeywords As Array = Keywords.Split(" ")

            For i As Integer = 0 To UBound(aryKeywords)
                SqlBuilder.Append("(ResourceName LIKE '%" & aryKeywords(i).ToString.Trim & "%' OR ArticleSummary LIKE '%" & aryKeywords(i).ToString.Trim & "%' OR ArticleText LIKE '%" & aryKeywords(i).ToString.Trim & "%')")
                If i < UBound(aryKeywords) Then SqlBuilder.Append(" OR ")
            Next
            ' Response.Write(SqlBuilder)

            PR01Data.SelectCommand = SqlBuilder.ToString

        End If
    End Sub

    Protected Sub gdvPR01_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvPR01.Load
        gdvPR01.PageSize = PageSize
    End Sub

    Protected Sub gdvPR01_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvPR01.RowDataBound
        If e.Row.RowType = DataControlRowType.Pager Then
            Dim btnNext As Button = e.Row.FindControl("btnNext")
            Dim btnPrev As Button = e.Row.FindControl("btnPrev")
            Dim lblPageCount As Label = e.Row.FindControl("lblPageCount")

            btnPrev.PostBackUrl = Request.RawUrl.ToString
            btnNext.PostBackUrl = Request.RawUrl.ToString

            lblPageCount.Text = "Page " & (gdvPR01.PageIndex + 1) & " of " & gdvPR01.PageCount

            If gdvPR01.PageIndex > 0 Then
                btnPrev.Enabled = True
            Else
                btnPrev.Enabled = False
            End If

            If (gdvPR01.PageIndex + 1) < gdvPR01.PageCount Then
                btnNext.Enabled = True
            Else
                btnNext.Enabled = False
            End If
        End If

        If e.Row.DataItem IsNot Nothing Then
            Dim hypPageName As HyperLink = e.Row.FindControl("hypPageName")
            Dim lblPageSummary As Label = e.Row.FindControl("lblPageSummary")
            Dim DataRow As System.Data.DataRowView = CType(e.Row.DataItem, System.Data.DataRowView)

            hypPageName.Text = DataRow.Item("ResourceName")
            hypPageName.NavigateUrl = "/" & DataRow.Item("ResourceID") & "/" & DataRow.Item("DeliveryPageKey") & ".htm"
            lblPageSummary.Text = stripHTML(DataRow.Item("ArticleSummary").ToString)
        End If
    End Sub

    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
End Class
