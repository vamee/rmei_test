Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class Pages01

    Public PageID As Integer = -1
    Public ParentPageID As Integer = -1
    Public PageTypeID As Integer = -1
    Public StatusID As Integer = 20
    Public TemplateID As Integer = -1
    Public ContentPageID As Integer = -1
    Public MembershipFormPageID As Integer = -1
    Public PageKey As String = ""
    Public PageAction As String = ""
    Public PageName As String = ""
    Public MenuName As String = ""
    Public TitleTag As String = ""
    Public MetaDescription As String = ""
    Public MetaKeywords As String = ""
    Public SEOScript As String = ""
    Public HeaderGraphic As String = ""
    Public LogImpressions As Boolean = False
    Public HasChildren As Boolean = False
    Public IsPermanent As Boolean = False
    Public IsHidden As Boolean = False
    Public IsSecure As Boolean = False
    Public IsSearchable As Boolean = False
    Public DisplaySubMenu As Boolean = True
    Public SortOrder As Integer = -1
    Public StartDate As String = ""
    Public EndDate As String = ""
    Public DateCreated As DateTime
    Public CreatedBy As String = ""
    Public LastUpdated As DateTime
    Public UpdatedBy As String = ""
    Public DefaultPage As Boolean = 0
    Public LanguageID As Integer = -1

    Public Shared Function GetPages(ByVal parentPageId As Integer, ByVal statusId As Integer) As SqlDataReader
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim selectString As String
        selectString = "SELECT * FROM qryPages "
        selectString += "WHERE ParentPageId = @ParentPageId And StatusId = @StatusId "
        selectString += "ORDER BY PageSortOrder"
        Dim cmd As New SqlCommand(selectString, con)
        cmd.Parameters.AddWithValue("@ParentPageId", parentPageId)
        cmd.Parameters.AddWithValue("@StatusId", statusId)
        con.Open()
        Dim dtr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
        Return dtr
        If con.State = ConnectionState.Open Then con.Close()
    End Function

    Public Shared Function GetPages(ByVal intParentPageID As Integer, ByVal intStatusID As Integer, ByVal intLanguageID As Integer) As SqlDataReader

        Dim SqlBuilder As New StringBuilder
        SqlBuilder.Append("SELECT * FROM qryPages ")
        SqlBuilder.Append("WHERE ParentPageID = @ParentPageID AND StatusID = @StatusID AND LanguageID = @LanguageID ")
        SqlBuilder.Append("ORDER BY PageSortOrder")

        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim Rs As SqlDataReader = Nothing
        Dim Command As New SqlCommand(SqlBuilder.ToString, Conn)
        Command.Parameters.AddWithValue("@ParentPageID", intParentPageID)
        Command.Parameters.AddWithValue("@StatusID", intStatusID)
        Command.Parameters.AddWithValue("@LanguageID", intLanguageID)

        Try
            Conn.Open()
            Rs = Command.ExecuteReader(CommandBehavior.CloseConnection)
        Catch ex As Exception
            Emagine.LogError(ex)
        End Try

        Return Rs
        If Conn.State = ConnectionState.Open Then Conn.Close()
    End Function

    Public Shared Function GetChildPages(ByVal intPageID As Integer) As SqlDataReader
        Dim SQLBuilder As New StringBuilder
        SQLBuilder.Append("SELECT PageId, PageKey, ParentPageId, PageTypeId, MenuName, PageAction, IsSecure ")
        SQLBuilder.Append("FROM Pages ")
        SQLBuilder.Append("WHERE ParentPageId = " & intPageID & " AND StatusId = 20 AND IsHidden = 0 ")
        SQLBuilder.Append("ORDER BY SortOrder")

        Return Emagine.GetDataReader(SQLBuilder.ToString)
    End Function

    Public Shared Function GetPageKey() As String
        Dim RequestUrl As String = HttpContext.Current.Request.RawUrl.ToString.Replace("/print", "")
        Dim QueryString As String = HttpContext.Current.Request.QueryString.ToString

        Dim PageKey As String = RequestUrl.Replace("?" & QueryString, "")
        Dim FileExtension As String = Emagine.GetFileExtension(PageKey)
        PageKey.Replace(FileExtension, "")

        Return PageKey
    End Function

    Public Shared Function GetFormPages(ByVal intFormPageTypeID As Integer, ByVal intStatusID As Integer, ByVal intLanguageID As Integer) As SqlDataReader
        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim strSQL As String = "sp_Pages01_GetFormPages"

        Dim objCommand As New SqlCommand(strSQL, objConn)
        objCommand.CommandType = CommandType.StoredProcedure
        objCommand.Parameters.AddWithValue("@FormPageTypeID", intFormPageTypeID)
        objCommand.Parameters.AddWithValue("@StatusID", intStatusID)
        objCommand.Parameters.AddWithValue("@LanguageID", intLanguageID)

        objConn.Open()
        Dim objDataReader As SqlDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection)

        Return objDataReader
        If objConn.State = ConnectionState.Open Then objConn.Close()
    End Function

    Public Shared Function GetPageInfo(ByVal PageID As Integer) As Pages01
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim sSelect As String
        sSelect = "SELECT * FROM Pages WHERE PageId = @PageID"
        Dim cmd As New SqlCommand(sSelect, con)
        cmd.Parameters.AddWithValue("@PageID", PageID)
        Dim dtr As SqlDataReader
        Dim Page As New Pages01
        Try
            con.Open()
            dtr = cmd.ExecuteReader(CommandBehavior.SingleRow)
            If dtr.Read Then
                Page.PageID = dtr("PageId")
                Page.ParentPageID = dtr("ParentPageId")
                Page.PageTypeID = dtr("PageTypeId")
                Page.StatusID = dtr("StatusId")
                Page.TemplateID = dtr("TemplateId")
                Page.ContentPageID = dtr("ContentPageID")
                Page.MembershipFormPageID = dtr("MembershipFormPageID")
                Page.PageKey = dtr("PageKey").ToString
                Page.PageAction = dtr("PageAction").ToString
                Page.PageName = dtr("PageName").ToString
                Page.MenuName = dtr("MenuName").ToString
                Page.TitleTag = dtr("TitleTag").ToString
                Page.MetaDescription = dtr("MetaDescription").ToString
                Page.MetaKeywords = dtr("MetaKeywords").ToString
                Page.SEOScript = dtr("SEOScript").ToString
                Page.HeaderGraphic = dtr("HeaderGraphic").ToString
                Page.DefaultPage = dtr("DefaultPage")
                Page.HasChildren = dtr("HasChildren")
                Page.IsPermanent = dtr("IsPermanent")
                Page.IsHidden = dtr("IsHidden")
                Page.IsSecure = dtr("IsSecure")
                Page.IsSearchable = dtr("IsSearchable")
                Page.DisplaySubMenu = dtr("DisplaySubMenu")
                Page.LogImpressions = dtr("LogImpressions")
                Page.SortOrder = dtr("SortOrder")
                ' Page.StartDate = dtr("StartDate")
                'Page.EndDate = dtr("EndDate")
                Page.DateCreated = dtr("DateCreated")
                Page.CreatedBy = dtr("CreatedBy").ToString
                Page.LastUpdated = dtr("LastUpdated")
                Page.UpdatedBy = dtr("UpdatedBy").ToString
            End If
            dtr.Close()
        Finally
            con.Close()
        End Try
        Return Page
    End Function

    Public Shared Function AddPage(ByVal Page As Pages01) As Integer
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_Pages01_AddPage"
        Dim PageID As Integer = 0

        Dim Cmd As New SqlCommand(SQL, Conn)
        Dim Param As New SqlParameter

        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.AddWithValue("@PageId", Page.PageId)
        Cmd.Parameters.AddWithValue("@ParentPageID", Page.ParentPageId)
        Cmd.Parameters.AddWithValue("@LanguageID", Page.LanguageID)
        Cmd.Parameters.AddWithValue("@PageTypeId", Page.PageTypeId)
        Cmd.Parameters.AddWithValue("@StatusID", Page.StatusId)
        Cmd.Parameters.AddWithValue("@PageTemplateID", Page.TemplateId)
        Cmd.Parameters.AddWithValue("@ContentPageID", Page.ContentPageID)
        Cmd.Parameters.AddWithValue("@MembershipFormPageID", Page.MembershipFormPageID)
        Cmd.Parameters.AddWithValue("@DefaultPage", Page.DefaultPage)
        Cmd.Parameters.AddWithValue("@PageKey", Page.PageKey)
        Cmd.Parameters.AddWithValue("@PageAction", Page.PageAction)
        Cmd.Parameters.AddWithValue("@PageName", Page.PageName)
        Cmd.Parameters.AddWithValue("@MenuName", Page.MenuName)
        Cmd.Parameters.AddWithValue("@TitleTag", Page.TitleTag)
        Cmd.Parameters.AddWithValue("@MetaDescription", Page.MetaDescription)
        Cmd.Parameters.AddWithValue("@MetaKeywords", Page.MetaKeywords)
        Cmd.Parameters.AddWithValue("@SEOScript", Page.SEOScript)
        Cmd.Parameters.AddWithValue("@HasChildren", Page.HasChildren)
        Cmd.Parameters.AddWithValue("@IsHidden", Page.IsHidden)
        Cmd.Parameters.AddWithValue("@IsSecure", Page.IsSecure)
        Cmd.Parameters.AddWithValue("@IsSearchable", Page.IsSearchable)
        Cmd.Parameters.AddWithValue("@LogImpressions", Page.LogImpressions)
        Cmd.Parameters.AddWithValue("@IsPermanent", Page.IsPermanent)
        Cmd.Parameters.AddWithValue("@DisplaySubMenu", Page.DisplaySubMenu)
        Cmd.Parameters.AddWithValue("@SortOrder", GetMaxSortOrder(Page.ParentPageId) + 1)
        Cmd.Parameters.AddWithValue("@StartDate", Page.StartDate)
        Cmd.Parameters.AddWithValue("@EndDate", Page.EndDate)
        Cmd.Parameters.AddWithValue("@CreatedBy", "")
        Cmd.Parameters.AddWithValue("@UpdatedBy", "")

        Try
            Conn.Open()
            Dim DataReader As SqlDataReader = Cmd.ExecuteReader()
            If DataReader.Read() Then
                PageID = DataReader(0)
            End If
            DataReader.Close()

        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            Conn.Close()
        End Try

        Return PageID
    End Function

    Public Shared Function UpdatePage(ByVal Page As Pages01) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_Pages01_UpdatePage"
        Dim Result As Boolean = False

        Dim Cmd As New SqlCommand(Sql, Conn)
        Dim Param As New SqlParameter

        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.AddWithValue("@PageId", Page.PageId)
        Cmd.Parameters.AddWithValue("@ParentPageID", Page.ParentPageId)
        Cmd.Parameters.AddWithValue("@PageTypeId", Page.PageTypeId)
        Cmd.Parameters.AddWithValue("@StatusID", Page.StatusId)
        Cmd.Parameters.AddWithValue("@PageTemplateID", Page.TemplateId)
        Cmd.Parameters.AddWithValue("@ContentPageID", Page.ContentPageID)
        Cmd.Parameters.AddWithValue("@MembershipFormPageID", Page.MembershipFormPageID)
        Cmd.Parameters.AddWithValue("@DefaultPage", Page.DefaultPage)
        Cmd.Parameters.AddWithValue("@PageKey", Page.PageKey)
        Cmd.Parameters.AddWithValue("@PageAction", Page.PageAction)
        Cmd.Parameters.AddWithValue("@PageName", Page.PageName)
        Cmd.Parameters.AddWithValue("@MenuName", Page.MenuName)
        Cmd.Parameters.AddWithValue("@TitleTag", Page.TitleTag)
        Cmd.Parameters.AddWithValue("@MetaDescription", Page.MetaDescription)
        Cmd.Parameters.AddWithValue("@MetaKeywords", Page.MetaKeywords)
        Cmd.Parameters.AddWithValue("@SEOScript", Page.SEOScript)
        Cmd.Parameters.AddWithValue("@HasChildren", Page.HasChildren)
        Cmd.Parameters.AddWithValue("@IsHidden", Page.IsHidden)
        Cmd.Parameters.AddWithValue("@IsSecure", Page.IsSecure)
        Cmd.Parameters.AddWithValue("@IsSearchable", Page.IsSearchable)
        Cmd.Parameters.AddWithValue("@LogImpressions", Page.LogImpressions)
        Cmd.Parameters.AddWithValue("@IsPermanent", Page.IsPermanent)
        Cmd.Parameters.AddWithValue("@DisplaySubMenu", Page.DisplaySubMenu)
        'Cmd.Parameters.AddWithValue("@SortOrder", GetMaxSortOrder(Page.ParentPageId) + 1)
        Cmd.Parameters.AddWithValue("@StartDate", Page.StartDate)
        Cmd.Parameters.AddWithValue("@EndDate", Page.EndDate)
        Cmd.Parameters.AddWithValue("@UpdatedBy", Page.UpdatedBy)

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

    Public Shared Function DeletePage(ByVal intPageID As Integer) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_Pages01_DeletePage"
        Dim Result As Boolean = False

        Dim Cmd As New SqlCommand(SQL, Conn)
        Dim Param As New SqlParameter
        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.AddWithValue("@PageId", intPageID)

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

    Public Shared Function UpdateSortOrder(ByVal intPageID As Integer, ByVal intSortOrder As Integer) As Boolean
        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim objCommand As New SqlCommand
        Dim objPage As New Pages01
        Dim blnResult As Boolean = False
        Dim intParentPageID As Integer
        Dim intOldSortOrder As Integer
        Dim intStatusID As Integer
        Dim strSQL As String = ""

        objCommand.Connection = objConn
        objPage = Pages01.GetPageInfo(intPageID)
        intParentPageID = objPage.ParentPageID
        intOldSortOrder = objPage.SortOrder
        intStatusID = objPage.StatusID

        If intOldSortOrder > intSortOrder Then
            strSQL = "UPDATE Pages SET SortOrder = SortOrder + 1 WHERE ParentPageID = " & intParentPageID & " AND SortOrder >= " & intSortOrder & " AND SortOrder < " & intOldSortOrder & " AND PageID <> " & intPageID & " AND StatusID = " & intStatusID
        ElseIf intOldSortOrder < intSortOrder Then
            strSQL = "UPDATE Pages SET SortOrder = SortOrder - 1 WHERE ParentPageID = " & intParentPageID & " AND SortOrder <= " & intSortOrder & " AND SortOrder > " & intOldSortOrder & " AND PageID <> " & intPageID & " AND StatusID = " & intStatusID
        End If

        Try
            objConn.Open()

            If strSQL.Length > 0 Then
                objCommand.CommandText = strSQL
                objCommand.ExecuteNonQuery()
            End If

            If intPageID > 0 Then
                strSQL = "UPDATE Pages SET SortOrder = " & intSortOrder & " WHERE PageID = " & intPageID
                objCommand.CommandText = strSQL
                objCommand.ExecuteNonQuery()
            End If

            ResetSortOrder(intParentPageID, intStatusID)

            blnResult = True
        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            objConn.Close()
        End Try

        Return blnResult
    End Function

    Public Shared Function ResetSortOrder(ByVal intParentPageID As Integer, ByVal intStatusID As Integer) As Boolean
        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim strSQL = "SELECT PageID, SortOrder FROM Pages WHERE ParentPageID = " & intParentPageID & " AND StatusID = " & intStatusID & " ORDER BY SortOrder"
        Dim objCommand As New SqlCommand(strSQL, objConn)
        Dim intCounter As Integer = 0
        Dim blnResult As Boolean = False

        objConn.Open()
        Dim objRs As SqlDataReader = Emagine.GetDataReader(strSQL)

        Do While objRs.Read
            intCounter = intCounter + 1
            objCommand.CommandText = "UPDATE Pages SET SortOrder = " & intCounter & " WHERE PageID = " & objRs("PageID")
            objCommand.ExecuteNonQuery()
        Loop
        objRs.Close()
        objConn.Close()

        blnResult = True

        Return blnResult
    End Function

    Public Shared Function GetMaxSortOrder(ByVal parentPageId As Integer) As Integer
        Dim intMaxSortOrder As Integer
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim selectString As String = "SELECT COUNT(*) As RecordCount FROM Pages WHERE ParentPageId = @ParentPageID AND StatusID = 20"
        Dim cmd As New SqlCommand(selectString, con)
        cmd.Parameters.AddWithValue("@ParentPageId", parentPageId)
        Try
            con.Open()
            Dim dtr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleResult)
            If dtr.Read And Not IsDBNull(dtr(0)) Then
                intMaxSortOrder = dtr(0)
            Else
                intMaxSortOrder = 0
            End If
            dtr.Close()
        Finally
            con.Close()
        End Try
        Return intMaxSortOrder
    End Function

    Public Shared Function GetStatusName(ByVal statusId As Integer) As String
        Dim strStatusName As String
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim selectString As String = _
            "SELECT Status FROM ItemStatus WHERE StatusID = @StatusId"
        Dim cmd As New SqlCommand(selectString, con)
        cmd.Parameters.AddWithValue("@StatusId", statusId)
        Try
            con.Open()
            Dim dtr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleResult)
            If dtr.Read And Not IsDBNull(dtr("Status")) Then
                strStatusName = dtr("Status")
            Else
                strStatusName = ""
            End If
            dtr.Close()
        Finally
            con.Close()
        End Try
        Return strStatusName
    End Function

    Public Shared Function GetContentID(ByVal intPageID As Integer, Optional ByRef strErrorMessage As String = "") As Integer
        Dim ContentID As Integer = 0
        Dim MyPage As Pages01 = Pages01.GetPageInfo(intPageID)

        If MyPage.ContentPageID > 0 Then
            ContentID = Content01.GetContentID("Pages01", MyPage.ContentPageID, 20, strErrorMessage)
        Else
            ContentID = Content01.GetContentID("Pages01", intPageID, 20, strErrorMessage)
        End If

        Return ContentID
    End Function

    Public Shared Function DeleteContent(ByVal intPageID As Integer) As Boolean
        'Dim Sql As String = 
    End Function



    Public Shared Function GetParentPageID(ByVal intPageId As Integer) As Integer
        Dim intParentPageid As Integer
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim selectString As String = "SELECT ParentPageId FROM PAges WHERE PageID = @PageId"
        Dim cmd As New SqlCommand(selectString, con)
        cmd.Parameters.AddWithValue("@PageId", intPageId)
        Try
            con.Open()
            Dim dtr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleResult)
            If dtr.Read Then
                intParentPageid = dtr(0)
            Else
                intParentPageid = 0
            End If
            dtr.Close()
        Finally
            con.Close()
        End Try
        Return intParentPageid
    End Function

    Public Shared Function GetPageTypes() As SqlDataReader
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim selectString As String
        selectString = "SELECT * FROM PageTypes ORDER BY PageTypeID"
        Dim cmd As New SqlCommand(selectString, con)
        con.Open()
        Dim dtr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
        Return dtr
        con.Close()
    End Function

    Public Shared Function PopulatePageOptionsDDL(ByVal ddlPageOptions As DropDownList, ByVal intPageID As Integer, ByVal intParentPageID As Integer, ByVal intLevel As Integer, ByVal intStatusID As Integer, ByVal intLanguageID As Integer) As DropDownList
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim selectString As String
        selectString = "SELECT PageID, ParentPageID, PageName, PageKey FROM Pages WHERE PageID <> @PageId AND ParentPageID = @ParentPageId  AND StatusID = @StatusID AND LanguageID = @LanguageID ORDER BY SortOrder"

        Dim cmd As New SqlCommand(selectString, con)
        cmd.Parameters.AddWithValue("@PageID", intPageID)
        cmd.Parameters.AddWithValue("@ParentPageID", intParentPageID)
        cmd.Parameters.AddWithValue("@StatusID", intStatusID)
        cmd.Parameters.AddWithValue("@LanguageID", intLanguageID)
        Try
            con.Open()
            Dim dtr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.Default)
            Dim i As Integer
            Dim strlevelDots As String = ""
            For i = 1 To intLevel
                strlevelDots = strlevelDots & " + "
            Next
            While dtr.Read
                ddlPageOptions.Items.Add(New ListItem(strlevelDots & dtr("PageName"), dtr("PageId")))
                If dtr("PageKey") <> "index" Then
                    PopulatePageOptionsDDL(ddlPageOptions, intPageID, dtr("PageId"), intLevel + 1, intStatusID, intLanguageID)
                End If
            End While
            dtr.Close()
        Finally
            con.Close()
        End Try
        Return ddlPageOptions
    End Function

    Public Shared Function GetTemplates(ByVal intLanguageID As Integer) As SqlDataReader
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)

        Dim SQL As String = "SELECT * FROM Templates WHERE LanguageID = " & intLanguageID
        Dim Command As New SqlCommand(SQL, Conn)
        Conn.Open()

        Return Command.ExecuteReader(CommandBehavior.CloseConnection)
        Conn.Close()
    End Function

    Public Shared Function GetPageStatuses() As SqlDataReader
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim selectString As String
        selectString = "SELECT * FROM ItemStatus WHERE StatusID = 20 OR StatusID = -10 ORDER BY StatusID"
        Dim cmd As New SqlCommand(selectString, con)
        con.Open()
        Dim dtr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
        Return dtr
        con.Close()
    End Function

    Public Shared Function IsUniquePageKey(ByVal strPageKey As String, ByVal intPageId As Integer, ByVal intLanguageID As Integer) As Boolean
        Dim Result As Boolean = True
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "SELECT PageID FROM Pages WHERE PageKey = @PageKey AND PageID <> @PageID AND LanguageID = @LanguageID AND StatusID = 20"
        Dim Cmd As New SqlCommand(SQL, Conn)
        Cmd.Parameters.AddWithValue("@PageKey", strPageKey)
        Cmd.Parameters.AddWithValue("@PageID", intPageId)
        Cmd.Parameters.AddWithValue("@LanguageID", intLanguageID)

        Try
            Conn.Open()
            Dim dtr As SqlDataReader = Cmd.ExecuteReader(CommandBehavior.Default)
            If dtr.Read Then Result = False
            dtr.Close()
        Finally
            Conn.Close()
        End Try
        Return Result
    End Function

    Public Shared Function GetBreadcrumbs(ByVal intPageId As Integer) As String
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim id As Integer
        Dim cmd As New SqlCommand
        Dim dr As SqlDataReader
        Dim temp As String = ""
        Dim aryTemp As Array
        Dim i As Integer
        Dim breadcrumb As String
        id = intPageId
        Try
            con.Open()
            cmd.Connection = con
            Do While id > 0
                cmd.CommandText = "SELECT PageId, ParentPageID, PageName FROM pages WHERE PageID = " & id
                dr = cmd.ExecuteReader(CommandBehavior.SingleRow)
                If dr.Read Then
                    temp += "<a href='/ezedit/modules/pages01/Default.aspx?PageId=" & dr("PageId") & "' class='breadcrumb'>" & dr("PageName") & "</a>,"
                    id = dr("ParentPageId")
                End If
                dr.Close()
            Loop

            If intPageId > 0 Then
                aryTemp = Split(temp, ",")
                temp = ""
                For i = (UBound(aryTemp) - 1) To (LBound(aryTemp)) Step -1
                    temp += aryTemp(i)
                    If i <> LBound(aryTemp) Then
                        temp += " > "
                    End If
                Next
            End If
            breadcrumb = "<p class='breadcrumb'><a href='/ezedit/modules/pages01/Default.aspx' class='breadcrumb'>Site Content</a> "
            If intPageId > 0 Then
                breadcrumb += "> "
            End If
            breadcrumb += temp & "</p>"
        Finally
            con.Close()
        End Try
        Return breadcrumb
    End Function

    Public Shared Function GetBreadcrumbs(ByVal intPageID As Integer, ByVal strModuleKey As String, Optional ByVal blnLinkCurrentPage As Boolean = False) As String
        Dim ResultBuilder As New StringBuilder
        Dim TempID As Integer = intPageID
        Dim RootLink As String = "<p class='breadcrumb'><a href='/ezedit/modules/" & strModuleKey & "/Default.aspx' class='breadcrumb'>Site Content</a> "

        Do While TempID > 0
            Dim PageData As DataTable = Emagine.GetDataTable("SELECT PageId, ParentPageID, PageName FROM pages WHERE PageID = " & TempID)

            If PageData.Rows.Count > 0 Then
                If TempID = intPageID And blnLinkCurrentPage = False Then
                    ResultBuilder.Insert(0, " > <span class='breadcrumb'>" & PageData.Rows(0).Item("PageName") & "</span>")
                Else
                    ResultBuilder.Insert(0, " > <a href='/ezedit/modules/" & strModuleKey & "/Default.aspx?ParentPageID=" & PageData.Rows(0).Item("PageID") & "' class='breadcrumb'>" & PageData.Rows(0).Item("PageName") & "</a>")
                End If

                TempID = PageData.Rows(0).Item("ParentPageID")
            End If
        Loop

        ResultBuilder.Insert(0, RootLink)
        ResultBuilder.Append("</p>")

        Return ResultBuilder.ToString
    End Function

    Public Shared Function DeleteVersion(ByVal intContentID As Integer) As Boolean
        Dim blnResult As Boolean = False
        Dim strSQL As String
        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim objPage As New Pages01

        strSQL = "UPDATE Content SET StatusID = -20 WHERE ContentID = @ContentID"
        Dim objCommand As New SqlCommand(strSQL, objConn)
        objCommand.Parameters.AddWithValue("@ContentID", intContentID)
        Try
            objConn.Open()
            objCommand.ExecuteNonQuery()
            blnResult = True
        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            If objConn.State = ConnectionState.Open Then objConn.Close()
        End Try

        Return blnResult
    End Function

    Public Shared Function PromoteContent(ByVal intPageId As Integer, ByVal intContentID As Integer) As Boolean
        Dim blnResult As Boolean = False
        Dim strSQL As String
        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim objPage As New Pages01

        strSQL = "UPDATE Content SET StatusID = 10 WHERE ModuleKey = 'Pages01' AND ForeignKey = @PageId AND StatusID = 20"
        Dim objCommand As New SqlCommand(strSQL, objConn)
        objCommand.Parameters.AddWithValue("@ContentID", intContentID)
        objCommand.Parameters.AddWithValue("@PageId", intPageId)
        Try
            objConn.Open()
            objCommand.ExecuteNonQuery()
            objCommand.CommandText = "UPDATE Content SET StatusID = 20 WHERE ModuleKey = 'Pages01' AND ForeignKey = @PageId AND ContentID = @ContentId"
            objCommand.ExecuteNonQuery()
            blnResult = True
        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            If objConn.State = ConnectionState.Open Then objConn.Close()
        End Try

        Return blnResult
    End Function

    Public Shared Function GetPreviewPageKey(ByVal intContentId As Integer) As String
        Dim strPreviewPageKey As String
        strPreviewPageKey = Emagine.GetDbValue("SELECT PageKey FROM Pages WHERE PageID IN (SELECT ForeignKey FROM Content WHERE ContentID = " & intContentId & ")")
        Return strPreviewPageKey
    End Function

    Public Shared Function DisplaySiteContent(ByVal iPageId As Integer, ByVal iPadding As Integer, ByRef sHTML As String, ByVal intUserId As Integer, ByVal strLevel As String, ByVal intLanguageID As Integer) As String
        Dim strSQL As String = "SELECT PageId, ParentPageId, PageName, PageKey FROM Pages WHERE LanguageID = " & intLanguageID & " AND ParentPageId = " & iPageId & " ORDER BY SortOrder"
        Dim dtr As SqlDataReader = Emagine.GetDataReader(strSQL)
        Do While dtr.Read()
            sHTML += _
                    "<tr bgcolor=""#" & IIf(dtr("ParentPageId") = 0, "F3F2F7", "FFFFFF") & """>" & vbCrLf _
                  & "   <td style=""padding-left:" & iPadding & """ class=""main"">" & vbCrLf _
                  & "       <input type=""checkbox"" name=""pageId" & strLevel & "_" & dtr("PageId") & """ value=""" & dtr("PageId") & """ onclick='selectPermissions(this)' " & CheckSelectedPageId(intUserId, dtr("PageId")) & ">&nbsp;&nbsp;" & vbCrLf
            If dtr("ParentPageId") = 0 Then
                sHTML += "<b>" & dtr("PageName") & "</b>" & vbCrLf
            Else
                sHTML += dtr("PageName") & vbCrLf
            End If
            sHTML += _
                     "   </td>" & vbCrLf _
                  & "</tr>" & vbCrLf
            DisplaySiteContent(dtr("PageId"), iPadding + 25, sHTML, intUserId, strLevel & "_" & dtr("PageId").ToString, intLanguageID)
        Loop
        dtr.Close()
        Return sHTML
    End Function

    Public Shared Function CheckSelectedPageId(ByVal intUserId, ByVal intPageId) As String
        If Emagine.GetDbValue("SELECT Count(*) FROM PagePermissions WHERE EzUserId = " & intUserId & " AND PageId = " & intPageId) > 0 Then
            Return "CHECKED"
        Else
            Return ""
        End If
    End Function

    Public Shared Function GetDefaultPageID(ByVal intLanguageID As Integer) As Integer
        Dim DefaultPageID As Integer = Emagine.GetNumber(Emagine.GetDbValue("SELECT PageID FROM Pages WHERE DefaultPage = 1 AND LanguageID = " & intLanguageID))

        Return DefaultPageID
    End Function

    Public Shared Function GetSectionPageId(ByVal intPageId As Integer) As Integer
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim cmd As New SqlCommand
        Dim dr As SqlDataReader
        Dim intSectionId As Integer
        Try
            con.Open()
            cmd.Connection = con
            Do While intPageId > 0
                cmd.CommandText = "SELECT PageId, ParentPageID, PageName FROM pages WHERE PageID = " & intPageId
                dr = cmd.ExecuteReader(CommandBehavior.SingleRow)
                If dr.Read Then
                    intPageId = CInt(dr("ParentPageId"))
                    intSectionId = dr("PageId")
                End If
                dr.Close()
            Loop
        Finally
            con.Close()
        End Try
        Return intSectionId
    End Function

    Public Shared Function GetSectionID(ByVal intPageID As Integer) As Integer
        Dim ParentPageID As Integer = intPageID
        Dim PageID As Integer = 0
        Dim PageTypeID As Integer = -1

        For i As Integer = 0 To 10
            Dim SQL As String = "SELECT PageID, ParentPageID, PageTypeID FROM Pages WHERE PageID = " & ParentPageID
            Dim Rs As DataTable = Emagine.GetDataTable(SQL)
            If Rs.Rows.Count > 0 Then
                PageID = Emagine.GetNumber(Rs.Rows(0).Item("PageID").ToString)
                ParentPageID = Emagine.GetNumber(Rs.Rows(0).Item("ParentPageID").ToString)
                PageTypeID = Emagine.GetNumber(Rs.Rows(0).Item("PageTypeID").ToString)
                If PageTypeID = 0 And ParentPageID = 0 Then Exit For
            Else
                Exit For
            End If
        Next

        Return PageID
    End Function

    Public Shared Function GetSectionName(ByVal intPageID As Integer) As String
        Dim ParentPageID As Integer = intPageID
        Dim PageID As Integer = 0
        Dim PageTypeID As Integer = -1
        Dim MenuName As String = ""

        Do While PageTypeID <> 0
            Dim SQL As String = "SELECT MenuName, PageID, ParentPageID, PageTypeID FROM Pages WHERE PageID = " & ParentPageID
            Dim Rs As DataTable = Emagine.GetDataTable(SQL)
            If Rs.Rows.Count > 0 Then
                PageID = Emagine.GetNumber(Rs.Rows(0).Item("PageID").ToString)
                ParentPageID = Emagine.GetNumber(Rs.Rows(0).Item("ParentPageID").ToString)
                PageTypeID = Emagine.GetNumber(Rs.Rows(0).Item("PageTypeID").ToString)
                MenuName = Rs.Rows(0).Item("MenuName").ToString
                'HttpContext.Current.Response.Write(MenuName & "<br>")
            Else
                Exit Do
            End If
        Loop

        Return MenuName
    End Function

    Public Shared Function IsRelated(ByVal intPageId As Integer, ByVal intSectionId As Integer) As Boolean
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim cmd As New SqlCommand
        Dim dr As SqlDataReader
        If intPageId = intSectionId Then
            con.Dispose()
            Return True
        Else
            Try
                con.Open()
                cmd.Connection = con
                Do While intPageId > 0
                    cmd.CommandText = "SELECT PageId, ParentPageID, PageName FROM pages WHERE PageID = " & intPageId
                    dr = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If dr.Read Then
                        If dr("ParentPageId") = intSectionId Then
                            Return True
                        End If
                        intPageId = CInt(dr("ParentPageId"))
                    End If
                    dr.Close()
                Loop
            Finally
                con.Close()
            End Try
        End If
    End Function

    Public Shared Function HasChildPages(ByVal intPageID As Integer) As Boolean
        Dim strSQL As String
        strSQL = "SELECT Count(*) FROM Pages WHERE ParentPageId = " & intPageID & " AND StatusId = 20 AND IsHidden = 0 "
        'HttpContext.Current.Response.Write(strSQL)
        'HttpContext.Current.Response.End()
        If Emagine.GetDbValue(strSQL) > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function IsPageRelated(ByVal intPageID As Integer, ByVal intSectionID As Integer) As Boolean
        Dim SQL As String
        Dim PageID As Integer = intPageID
        Dim ParentPageID As Integer = intPageID
        Dim Result As Boolean = False

        If intPageID = intSectionID Then
            Result = True
        Else
            Do While ParentPageID > 0
                SQL = "SELECT ParentPageID FROM Pages WHERE PageID = " & ParentPageID
                ParentPageID = CInt(Emagine.GetDbValue(SQL))
                If intSectionID = ParentPageID Then Result = True
            Loop
        End If

        Return Result
    End Function

    Public Shared Function InsertPageModule(ByVal intPageID As Integer, ByVal strModuleKey As String, ByVal strForeignKey As String, ByVal intCategoryID As Integer, ByVal intSortOrder As Integer) As Integer

        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_Pages01_AddModule"
        Dim PageModuleID As Integer = 0

        Dim Cmd As New SqlCommand(SQL, Conn)
        Dim Param As New SqlParameter

        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.AddWithValue("@PageID", intPageID)
        Cmd.Parameters.AddWithValue("@ModuleKey", strModuleKey)
        Cmd.Parameters.AddWithValue("@ForeignKey", strForeignKey)
        Cmd.Parameters.AddWithValue("@ForeignValue", intCategoryID)
        Cmd.Parameters.AddWithValue("@SortOrder", intSortOrder)

        Try
            Conn.Open()
            Dim DataReader As SqlDataReader = Cmd.ExecuteReader()
            DataReader.Read()
            PageModuleID = DataReader(0)
            DataReader.Close()

        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            Conn.Close()
        End Try

        Return PageModuleID
    End Function

    Public Shared Function InsertPageModuleProperty(ByVal intPageModuleID As Integer, ByVal intPropertyID As Integer, ByVal strPropertyValue As String) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_Pages01_AddModuleProperty"
        Dim Result As Boolean = False

        Dim Cmd As New SqlCommand(SQL, Conn)
        Dim Param As New SqlParameter

        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.AddWithValue("@PageModuleID", intPageModuleID)
        Cmd.Parameters.AddWithValue("@PropertyID", intPropertyID)
        Cmd.Parameters.AddWithValue("@PropertyValue", strPropertyValue)

        Try
            Conn.Open()
            Cmd.ExecuteNonQuery()
            Result = True

        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            Conn.Close()
        End Try

        Return Result
    End Function

    Public Shared Function GetPageLink(ByVal intPageTypeID As Integer, ByVal strPageKey As String, ByVal strPageAction As String, ByVal blnIsSecure As Boolean) As String
        Dim PageLink As String = ""

        If intPageTypeID = 2 Then
            PageLink = strPageAction
        Else
            If blnIsSecure Then
                If HttpContext.Current.Application("SslUrl") IsNot Nothing Then
                    PageLink = HttpContext.Current.Application("SslUrl") & "/" & strPageKey & ".htm"
                Else
                    PageLink = "https://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & "/" & strPageKey & ".htm"
                End If
            Else
                If HttpContext.Current.Application("BaseUrl") IsNot Nothing Then
                    PageLink = HttpContext.Current.Application("BaseUrl") & "/" & strPageKey & ".htm"
                Else
                    PageLink = "http://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & "/" & strPageKey & ".htm"
                End If
            End If
        End If

        Return PageLink
    End Function

    Public Shared Function GetPageLink(ByVal intPageTypeID As Integer, ByVal strPageKey As String, ByVal strPageAction As String, ByVal blnIsSecure As Boolean, ByVal blnChildPageLink As Boolean) As String
        Dim PageLink As String = ""

        Select Case intPageTypeID
            Case 0
                HttpContext.Current.Response.Write("SELECT PageKey + '.htm' AS NavigateURL FROM Pages WHERE ParentPageID IN (SELECT PageID FROM Pages WHERE PageKey = '" & strPageKey & "') ORDER BY SortOrder<br>")
                PageLink = "/" & Emagine.GetDbValue("SELECT PageKey + '.htm' AS NavigateURL FROM Pages WHERE ParentPageID IN (SELECT PageID FROM Pages WHERE PageKey = '" & strPageKey & "') ORDER BY SortOrder")

            Case 2
                PageLink = strPageAction

            Case Else
                If blnIsSecure Then
                    If HttpContext.Current.Application("SslUrl") IsNot Nothing Then
                        PageLink = HttpContext.Current.Application("SslUrl") & "/" & strPageKey & ".htm"
                    Else
                        PageLink = "https://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & "/" & strPageKey & ".htm"
                    End If
                Else
                    If HttpContext.Current.Application("BaseUrl") IsNot Nothing Then
                        PageLink = HttpContext.Current.Application("BaseUrl") & "/" & strPageKey & ".htm"
                    Else
                        PageLink = "http://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & "/" & strPageKey & ".htm"
                    End If
                End If

        End Select

        Return PageLink
    End Function

    Public Shared Function GetPageLink(ByVal intPageTypeID As Integer, ByVal intPageID As Integer, ByVal strPageAction As String, ByVal blnIsSecure As Boolean, ByVal blnChildPageLink As Boolean) As String
        Dim PageLink As String = ""

        Select Case intPageTypeID
            Case 0
                'HttpContext.Current.Response.Write("SELECT PageKey + '.htm' AS NavigateURL FROM Pages WHERE ParentPageID = " & intPageID & " ORDER BY SortOrder<br>")
                PageLink = "/" & Emagine.GetDbValue("SELECT PageKey + '.htm' AS NavigateURL FROM Pages WHERE ParentPageID = " & intPageID & " ORDER BY SortOrder")

        End Select

        Return PageLink
    End Function

    Public Shared Function DeletePagePermissions(ByVal intPageID As Integer) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_Pages01_DeletePermissions"
        Dim Result As Boolean = False

        Dim Cmd As New SqlCommand(SQL, Conn)
        Dim Param As New SqlParameter
        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.AddWithValue("@PageID", intPageID)

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

    Public Shared Function AddPagePermissions(ByVal intPageID As Integer, ByVal intUserID As Integer) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_Pages01_AddPermissions"
        Dim Result As Boolean = False

        Dim Cmd As New SqlCommand(SQL, Conn)
        Dim Param As New SqlParameter
        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.AddWithValue("@PageID", intPageID)
        Cmd.Parameters.AddWithValue("@UserID", intUserID)

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

End Class
