
Partial Class PR01_DisplayNews_GroupByYear
    Inherits System.Web.UI.UserControl

    Dim _PageModuleID As Integer = 0

    Public Property PageModuleID() As Integer
        Get
            Return _PageModuleID
        End Get
        Set(ByVal value As Integer)
            _PageModuleID = value
        End Set
    End Property

    Sub DisplayArticleList(ByVal intCategoryId As Integer, ByVal strDisplayPageKey As String)
        Dim Rs As Data.SqlClient.SqlDataReader = PR01.GetArticles(intCategoryId, 1, "DisplayDate", "DESC")
        If Rs.HasRows Then
            Dim Result As New StringBuilder
            Dim CurrentYear As Integer = 0
            Dim RowStyle As String = ""

            'Result.Append(Rs("CategoryName"))
            Result.Append("<table cellpadding='0' cellspacing='0' width='100%' border='0'>")

            Do While Rs.Read
                If Year(Rs("DisplayDate")) <> CurrentYear Then
                    Result.Append("<tr class='table-row'>")
                    Result.Append("<td colspan='3'>" & Year(Rs("DisplayDate")))
                    Result.Append("</tr>")
                    RowStyle = "table-row"
                End If

                If RowStyle = "table-row" Then RowStyle = "table-altrow" Else RowStyle = "table-row"

                Result.Append("<tr class='" & RowStyle & "'>")

                Result.Append("<td width='25'></td>")

                Result.Append("<td valign='top' width='15%' style='padding-right:6px;'>")
                Result.Append("<b>" & FormatDateTime(Rs("DisplayDate"), DateFormat.ShortDate) & "</b>")
                Result.Append("</td>")

                Result.Append("<td valign='top'>")
                Result.Append("<b><a href='/" & Rs("ResourceID") & "/" & Rs("DeliveryPageKey") & ".htm'>" & Rs("ResourceName") & "</a></b><br />")
                Result.Append(Rs("ArticleSummary"))
                Result.Append("</td>")

                Result.Append("</tr>")
                CurrentYear = Year(Rs("DisplayDate"))
            Loop
            Result.Append("</table>")

            ltrArticles.Text = Result.ToString
        Else
            pnlArticles.Visible = False
        End If
        Rs.Close()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim CategoryID As Integer = Emagine.GetDbValue("SELECT ForeignValue FROM PageModules WHERE PageModuleID = " & PageModuleID)
        Dim ResourceID As String = Resources.GetResourceID()
        Dim ArticleID As Integer = 0
        Dim DisplayPageKey As String = ""

        If Len(ResourceID) > 0 Then
            Dim SQL As String = "SELECT * FROM qryArticles WHERE ResourceID = '" & ResourceID & "'"
            Dim Rs As Data.SqlClient.SqlDataReader = Emagine.GetDataReader(SQL)
            If Rs.Read Then
                ArticleID = Rs("ArticleID")
                DisplayPageKey = Rs("DisplayPageKey")
            End If
            Rs.Close()
            Rs = Nothing
        End If

        If ArticleID > 0 Then
            DisplayArticle(ArticleID)
        Else
            DisplayArticleList(CategoryID, DisplayPageKey)
        End If
    End Sub

    Sub DisplayArticle(ByVal intArticleID As Integer)

        Dim FormRedirect As String = ""
        Dim ResourcePageKey As String = ""
        Dim ResourceID As String = ""
        Dim UserID As String = Emagine.Users.User.GetUserID()
        Dim SQL As String = "SELECT FormPageID, FormPageKey, ResourceID FROM qryArticles WHERE ArticleID = " & intArticleID
        Dim Rs As Data.SqlClient.SqlDataReader = Emagine.GetDataReader(SQL)

        If Rs.Read Then
            Resources.UpdateClickCount(Rs("ResourceID"))
            ResourceID = Rs("ResourceID")
            'ResourcePageKey = Rs("ResourcePageKey")
            If Emagine.GetNumber(Rs("FormPageID").ToString) > 0 Then
                FormRedirect = "/" & Rs("ResourceID") & "/" & Rs("FormPageKey") & ".htm"
            End If
        End If
        Rs.Close()
        Rs = Nothing

        If Len(FormRedirect) = 0 Or Resources.UserHasRegistered(UserID, ResourceID) Then
            SQL = "SELECT ResourceName, ArticleText FROM qryArticles WHERE ArticleID = " & intArticleID
            Rs = Emagine.GetDataReader(SQL)
            If Rs.Read Then
                lblArticleTitle.Text = Rs("ResourceName")
                lblArticleText.Text = Rs("ArticleText")
                ArticleDetail.Visible = True
            End If
            Rs.Close()
            Rs = Nothing
        Else
            Response.Redirect(FormRedirect)
        End If
    End Sub
End Class
