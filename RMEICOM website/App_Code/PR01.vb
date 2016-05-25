Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class PR01
    Inherits Resources.Resource

    Public ArticleID As Integer
    Public Shadows ModuleTypeID As Integer
    Public CategoryID As Integer
    Public CategoryName As String
    Public ArticleSummary As String
    Public ArticleText As String
    Public ArticleURL As String
    Public ImageURL As String
    Public FileName As String
    Public DisplayDate As String
    Public SortOrder As Integer


    Public Shared Function GetArticles(ByVal intCategoryId As String) As SqlDataReader
        Dim strSQL As String
        strSQL = "SELECT DisplayDate,ResourceName,ArticleSummary,ArticleText,ArticleURL,FileName,ArticleID,SortOrder,CategoryId,ResourcePageKey,DeliveryPageKey,ResourceID,ModuleTypeID,ModuleType,ImageUrl FROM qryArticles WHERE CategoryID = " & intCategoryId & " ORDER BY DisplayDate DESC"
        Dim dtr As SqlDataReader = Emagine.GetDataReader(strSQL)
        Return dtr
    End Function

    Public Shared Function GetArticles(ByVal intCategoryID As Integer, ByVal intIsEnabled As Integer, ByVal strOrderBy As String, ByVal strOrderDirection As String) As SqlDataReader
        Dim SqlBuilder As New StringBuilder
        SqlBuilder.Append("SELECT DisplayDate,ResourceName,ArticleSummary,ArticleText,ArticleURL,FileName,ArticleID,SortOrder,CategoryId,ResourcePageKey,DeliveryPageKey,ResourceID,ModuleTypeID,ModuleType,ImageUrl FROM qryArticles WHERE CategoryID = " & intCategoryID)
        Select Case intIsEnabled
            Case 0
                SqlBuilder.Append(" AND IsEnabled = 0")
            Case 1
                SqlBuilder.Append(" AND IsEnabled = 1")
        End Select
        If strOrderBy.Length > 0 Then
            SqlBuilder.Append(" ORDER BY " & strOrderBy)
            If strOrderDirection.Length > 0 Then
                SqlBuilder.Append(" " & strOrderDirection)
            End If
        End If

        Dim DataReader As SqlDataReader = Emagine.GetDataReader(SqlBuilder.ToString)
        Return DataReader
    End Function

    Public Shared Function GetArticle(ByVal intArticleId As Integer) As PR01
        Dim Article As New PR01
        Dim dtr As SqlDataReader = Emagine.GetDataReader("SELECT * FROM qryArticles WHERE ArticleID = " & intArticleId)
        If dtr.Read Then
            Article.ArticleId = dtr("ArticleId").ToString
            Article.CategoryId = dtr("CategoryId").ToString
            Article.ModuleTypeID = dtr("ModuleTypeID").ToString
            Article.CategoryName = dtr("CategoryName").ToString
            Article.DisplayDate = dtr("DisplayDate").ToString
            Article.ResourceId = dtr("ResourceID").ToString
            Article.ResourceName = dtr("ResourceName").ToString
            Article.ResourceCategory = dtr("ResourceCategory").ToString
            Article.ResourcePageKey = dtr("ResourcePageKey").ToString
            Article.PublishToRss = dtr("PublishToRss")
            Article.RssAuthor = dtr("RssAuthor").ToString
            Article.RssDescription = dtr("RssDescription").ToString
            Article.ArticleSummary = dtr("ArticleSummary").ToString
            Article.ArticleText = dtr("ArticleText").ToString
            Article.ArticleURL = dtr("ArticleURL").ToString
            Article.ImageURL = dtr("ImageURL").ToString
            Article.FileName = dtr("FileName").ToString
            Article.ResourceKeywords = dtr("ResourceKeywords").ToString
            Article.SortOrder = dtr("SortOrder").ToString
            Article.IsEnabled = dtr("IsEnabled")
            Article.DisplayStartDate = dtr("DisplayStartDate").ToString
            Article.DisplayEndDate = dtr("DisplayEndDate").ToString
        Else
            Article = Nothing
        End If

        dtr.Close()

        Return Article
    End Function

    Public Shared Function GetArticleCount(ByVal intCategoryId As String) As Integer
        Dim strSQL As String
        strSQL = "SELECT COUNT(ArticleID) AS RecordCount FROM Articles WHERE CategoryID = " & intCategoryId
        Return Emagine.GetNumber(Emagine.GetDbValue(strSQL))
    End Function


    Public Shared Function IsUniquePageKey_OLD(ByVal strPageKey As String, ByVal intPageId As Integer) As Boolean
        Dim blnIsUniquePageKey As Boolean
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim selectString As String = _
            "SELECT PageId FROM Pages WHERE PageKey = @PageKey"
        Dim cmd As New SqlCommand(selectString, con)
        cmd.Parameters.AddWithValue("@PageKey", strPageKey)
        Try
            con.Open()
            Dim dtr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.Default)
            If dtr.Read Then
                If dtr.FieldCount = 1 Then
                    If dtr("PageId") = intPageId Then ' editing page
                        blnIsUniquePageKey = True
                    Else
                        blnIsUniquePageKey = False
                    End If
                Else
                    blnIsUniquePageKey = False
                End If
            Else
                blnIsUniquePageKey = True
            End If
            dtr.Close()
        Finally
            con.Close()
        End Try
        Return blnIsUniquePageKey
    End Function

    Public Function UpdateArticle_OLD(ByVal Article As PR01) As Boolean

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim blnSuccess As Boolean
        Dim strSelect As String

        'HttpContext.Current.Response.Write(Article.ArticleId)
        'HttpContext.Current.Response.End()

        If Article.ArticleId = 0 Then
            strSelect = "INSERT INTO Articles (CategoryID, ResourceID, DisplayDate, ArticleSummary, ArticleText, SortOrder) "
            strSelect += "VALUES (@CategoryID, @ResourceID, @DisplayDate, @ArticleSummary, @ArticleText, @SortOrder)"
            HttpContext.Current.Session("Alert") = "Aricle Added Successfully"
        Else ' UPDATE
            strSelect = "UPDATE Articles "
            strSelect += "SET CategoryID = @CategoryID, DisplayDate = @DisplayDate, ArticleSummary = @ArticleSummary, ArticleText = @ArticleText "
            strSelect += "WHERE ArticleID = @ArticleID"
            HttpContext.Current.Session("Alert") = "Article Updated Successfully"
        End If

        Dim cmd As New SqlCommand(strSelect, con)

        cmd.Parameters.AddWithValue("@ArticleID", Article.ArticleId)
        cmd.Parameters.AddWithValue("@CategoryID", Article.CategoryId)
        cmd.Parameters.AddWithValue("@ResourceID", Article.ResourceId)
        cmd.Parameters.AddWithValue("@DisplayDate", Article.DisplayDate)
        cmd.Parameters.AddWithValue("@ArticleSummary", Article.ArticleSummary)
        cmd.Parameters.AddWithValue("@ArticleText", Article.ArticleText)
        cmd.Parameters.AddWithValue("@SortOrder", Article.SortOrder)

        Try
            con.Open()
            cmd.ExecuteNonQuery()
            blnSuccess = True
        Catch ex As Exception
            Emagine.LogError(ex)
            blnSuccess = False
        Finally
            con.Close()
        End Try

        Return blnSuccess

    End Function

    Public Shared Function AddArticle(ByVal Article As PR01) As Integer
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_PR01_AddArticle"
        Dim ArticleID As Integer = 0

        Dim Cmd As New SqlCommand(SQL, Conn)
        Dim Param As New SqlParameter

        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.AddWithValue("@CategoryID", Article.CategoryID)
        Cmd.Parameters.AddWithValue("@ResourceID", Article.ResourceID)
        Cmd.Parameters.AddWithValue("@ArticleSummary", Article.ArticleSummary)
        Cmd.Parameters.AddWithValue("@ArticleText", Article.ArticleText)
        Cmd.Parameters.AddWithValue("@ArticleURL", Article.ArticleURL)
        Cmd.Parameters.AddWithValue("@ImageURL", Article.ImageURL)
        Cmd.Parameters.AddWithValue("@FileName", Article.FileName)
        Cmd.Parameters.AddWithValue("@DisplayDate", Article.DisplayDate)
        Cmd.Parameters.AddWithValue("@SortOrder", Article.SortOrder)

        Try
            Conn.Open()
            Dim DataReader As SqlDataReader = Cmd.ExecuteReader()
            If DataReader.Read() Then
                ArticleID = DataReader(0)
            End If
            DataReader.Close()

        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            Conn.Close()
        End Try

        Return ArticleID
    End Function

    Public Shared Function UpdateArticle(ByVal Article As PR01) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_PR01_UpdateArticle"
        Dim Result As Boolean = False

        Dim Cmd As New SqlCommand(SQL, Conn)
        Dim Param As New SqlParameter

        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.AddWithValue("@ArticleID", Article.ArticleID)
        Cmd.Parameters.AddWithValue("@CategoryID", Article.CategoryID)
        Cmd.Parameters.AddWithValue("@ArticleSummary", Article.ArticleSummary)
        Cmd.Parameters.AddWithValue("@ArticleText", Article.ArticleText)
        Cmd.Parameters.AddWithValue("@ArticleURL", Article.ArticleURL)
        Cmd.Parameters.AddWithValue("@ImageURL", Article.ImageURL)
        Cmd.Parameters.AddWithValue("@FileName", Article.FileName)
        Cmd.Parameters.AddWithValue("@DisplayDate", Article.DisplayDate)
        Cmd.Parameters.AddWithValue("@SortOrder", Article.SortOrder)

        Try
            Conn.Open()
            Result = Cmd.ExecuteNonQuery()

        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            Conn.Close()
        End Try

        Return Result
    End Function

    Public Shared Function DeleteArticle(ByVal intArticleID As Integer) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_PR01_DeleteArticle"
        Dim Result As Boolean = False

        Dim Cmd As New SqlCommand(SQL, Conn)
        Dim Param As New SqlParameter

        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.AddWithValue("@ArticleID", intArticleID)

        Try
            Conn.Open()
            Result = Cmd.ExecuteNonQuery()

        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            Conn.Close()
        End Try

        Return Result
    End Function

    Public Shared Function GetMaxSortOrder(ByVal intCategoryID As Integer) As Integer
        Return Emagine.GetNumber(Emagine.GetDbValue("SELECT MAX(SortOrder) As MaxSortOrder FROM Articles WHERE CategoryID = " & intCategoryID))
    End Function

    Public Shared Function IsUniquePageKey(ByVal strPageKey As String, ByVal intArticleID As Integer, ByVal intCategoryId As Integer) As Boolean
        Dim strSQL As String = "SELECT Count(*) FROM qryArticles WHERE ArticleID <> " & intArticleID & " AND ResourcePageKey = '" & strPageKey & "' AND CategoryId = " & intCategoryId
        If Emagine.GetDbValue(strSQL) = 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function UpdateSortOrder(ByVal intArticleID As Integer, ByVal intSortOrder As Integer) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim Command As New SqlCommand
        Dim Article As PR01 = PR01.GetArticle(intArticleID)
        Dim Result As Boolean = False
        Dim SQL As String = ""

        If Article.SortOrder > intSortOrder Then
            SQL = "UPDATE Articles SET SortOrder = SortOrder + 1 WHERE CategoryID = " & Article.CategoryId & " AND SortOrder >= " & intSortOrder & " AND SortOrder < " & Article.SortOrder & " AND ArticleID <> " & intArticleID
        ElseIf Article.SortOrder < intSortOrder Then
            SQL = "UPDATE Articles SET SortOrder = SortOrder - 1 WHERE CategoryID = " & Article.CategoryId & " AND SortOrder <= " & intSortOrder & " AND SortOrder > " & Article.SortOrder & " AND ArticleID <> " & intArticleID
        End If

        Try
            Command.Connection = Conn
            Conn.Open()

            If SQL.Length > 0 Then
                Command.CommandText = SQL
                Command.ExecuteNonQuery()
            End If

            If intArticleID > 0 Then
                SQL = "UPDATE Articles SET SortOrder = " & intSortOrder & " WHERE ArticleID = " & intArticleID
                Command.CommandText = SQL
                Command.ExecuteNonQuery()
            End If

            ResetSortOrder(Article.CategoryId)

            Result = True
        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            Conn.Close()
        End Try

        Return Result
    End Function

    Public Shared Function ResetSortOrder(ByVal intCategoryID As Integer) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL = "SELECT ArticleID, SortOrder FROM Articles WHERE CategoryID = " & intCategoryID & " ORDER BY SortOrder"
        Dim Command As New SqlCommand(SQL, Conn)
        Dim Counter As Integer = 0
        Dim Result As Boolean = False
        Dim Rs As SqlDataReader = Emagine.GetDataReader(SQL)

        Try
            Conn.Open()

            Do While Rs.Read
                Counter += 1
                Command.CommandText = "UPDATE Articles SET SortOrder = " & Counter & " WHERE ArticleID = " & Rs("ArticleID")

                Command.ExecuteNonQuery()
            Loop

            Rs.Close()

            Result = True

        Catch ex As Exception
            Emagine.LogError(ex)

        Finally
            Conn.Close()
        End Try

        Return Result
    End Function

End Class
