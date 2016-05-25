Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class Resources

    Public Class Resource
        Public ResourceID As String = ""
        Public ModuleTypeID As Integer = 0
        Public LanguageID As Integer = -1
        Public StatusID As Integer = 20
        Public ResourceName As String = ""
        Public ResourceCategory As String = ""
        Public ResourceType As String = ""
        Public ReourcePageKey As String = ""
        Public ResourcePageKey As String = ""
        Public ResourceKeywords As String = ""
        Public CampaignInfo As String = ""
        Public PublishToRss As Boolean = False
        Public RssAuthor As String = ""
        Public RssDescription As String = ""
        Public DisplayStartDate As Date = "1/1/1900"
        Public DisplayEndDate As Date = "1/1/1900"
        Public IsEnabled As Boolean = True
        Public SortOrder As Integer = -1
        Public CreatedDate As Date = "1/1/1900"
        Public CreatedBy As String = ""
        Public UpdatedDate As Date = Date.Now
        Public UpdatedBy As String = ""

        Public Shared Function GetResource(ByVal strResourceID As String) As Resources.Resource
            Dim MyResource As New Resource
            Dim ResourceData As DataTable = Emagine.GetDataTable("SELECT * FROM Resources WHERE ResourceID = '" & strResourceID & "'")
            If ResourceData.Rows.Count > 0 Then
                MyResource.ResourceID = ResourceData.Rows(0).Item("ResourceID")
                MyResource.ModuleTypeID = ResourceData.Rows(0).Item("ModuleTypeID")
                MyResource.LanguageID = ResourceData.Rows(0).Item("LanguageID")
                MyResource.StatusID = ResourceData.Rows(0).Item("StatusID")
                MyResource.ResourceName = ResourceData.Rows(0).Item("ResourceName").ToString
                MyResource.ResourceCategory = ResourceData.Rows(0).Item("ResourceCategory").ToString
                MyResource.ResourceType = ResourceData.Rows(0).Item("ResourceType").ToString
                MyResource.ResourcePageKey = ResourceData.Rows(0).Item("ResourcePageKey").ToString
                MyResource.ResourceKeywords = ResourceData.Rows(0).Item("ResourceKeywords").ToString
                MyResource.CampaignInfo = ResourceData.Rows(0).Item("CampaignInfo").ToString
                MyResource.PublishToRss = ResourceData.Rows(0).Item("PublishToRss")
                MyResource.RssAuthor = ResourceData.Rows(0).Item("RssAuthor").ToString
                MyResource.RssDescription = ResourceData.Rows(0).Item("RssDescription").ToString
                MyResource.DisplayStartDate = ResourceData.Rows(0).Item("DisplayStartDate")
                MyResource.DisplayEndDate = ResourceData.Rows(0).Item("DisplayEndDate")
                MyResource.IsEnabled = ResourceData.Rows(0).Item("IsEnabled")
                MyResource.SortOrder = ResourceData.Rows(0).Item("SortOrder")
                MyResource.CreatedDate = ResourceData.Rows(0).Item("CreatedDate")
                MyResource.CreatedBy = ResourceData.Rows(0).Item("CreatedBy").ToString
                MyResource.UpdatedDate = ResourceData.Rows(0).Item("UpdatedDate")
                MyResource.UpdatedBy = ResourceData.Rows(0).Item("UpdatedBy").ToString
            End If

            Return MyResource
        End Function

        Public Shared Function AddResource(ByVal objResource As Resources.Resource, Optional ByRef strErrorMessage As String = "") As Boolean
            Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
            Dim SQL As String = "sp_Resources_AddResource"
            Dim Result As Boolean = False

            Emagine.LogError("Executed 'AddResource' function.", "", "")

            Dim Cmd As New SqlCommand(SQL, Conn)

            Cmd.CommandType = CommandType.StoredProcedure
            Cmd.Parameters.AddWithValue("@ResourceID", objResource.ResourceID)
            Cmd.Parameters.AddWithValue("@ModuleTypeID", objResource.ModuleTypeID)
            Cmd.Parameters.AddWithValue("@LanguageID", objResource.LanguageID)
            Cmd.Parameters.AddWithValue("@StatusID", objResource.StatusID)
            Cmd.Parameters.AddWithValue("@ResourceName", objResource.ResourceName)
            Cmd.Parameters.AddWithValue("@ResourceCategory", objResource.ResourceCategory)
            Cmd.Parameters.AddWithValue("@ResourceType", objResource.ResourceType)
            Cmd.Parameters.AddWithValue("@ResourcePageKey", objResource.ResourcePageKey)
            Cmd.Parameters.AddWithValue("@ResourceKeywords", objResource.ResourceKeywords)
            Cmd.Parameters.AddWithValue("@CampaignInfo", objResource.CampaignInfo)
            Cmd.Parameters.AddWithValue("@PublishToRss", objResource.PublishToRss)
            Cmd.Parameters.AddWithValue("@RssAuthor", objResource.RssAuthor)
            Cmd.Parameters.AddWithValue("@RssDescription", objResource.RssDescription)
            Cmd.Parameters.AddWithValue("@DisplayStartDate", objResource.DisplayStartDate)
            Cmd.Parameters.AddWithValue("@DisplayEndDate", objResource.DisplayEndDate)
            Cmd.Parameters.AddWithValue("@SortOrder", objResource.SortOrder)
            Cmd.Parameters.AddWithValue("@IsEnabled", objResource.IsEnabled)
            Cmd.Parameters.AddWithValue("@CreatedBy", objResource.CreatedBy)
            Cmd.Parameters.AddWithValue("@UpdatedBy", objResource.UpdatedBy)

            Try
                Conn.Open()
                Result = Cmd.ExecuteNonQuery()
                Emagine.LogError("Executed sp_Resources_AddResource successfully", "", "")
            Catch ex As Exception
                Emagine.LogError(ex)
                strErrorMessage = ex.Message
            Finally
                Conn.Close()
            End Try

            Return Result
        End Function

        Public Shared Function DeleteResource(ByVal objResource As Resource, Optional ByRef strErrorMessage As String = "") As Boolean
            Dim Result As Boolean = False

            Result = Emagine.ExecuteSQL("DELETE FROM Resources WHERE ResourceID = '" & objResource.ResourceID & "'", strErrorMessage)

            Return Result
        End Function

        Public Shared Function UpdateResource(ByVal objResource As Resources.Resource, Optional ByRef strErrorMessage As String = "") As Boolean
            Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
            Dim SQL As String = "sp_Resources_UpdateResource"
            Dim Result As Boolean = False

            Dim Cmd As New SqlCommand(SQL, Conn)
            Dim Param As New SqlParameter

            Cmd.CommandType = CommandType.StoredProcedure
            Cmd.Parameters.AddWithValue("@ResourceID", objResource.ResourceID)
            Cmd.Parameters.AddWithValue("@ModuleTypeID", objResource.ModuleTypeID)
            Cmd.Parameters.AddWithValue("@LanguageID", objResource.LanguageID)
            Cmd.Parameters.AddWithValue("@StatusID", objResource.StatusID)
            Cmd.Parameters.AddWithValue("@ResourceName", objResource.ResourceName)
            Cmd.Parameters.AddWithValue("@ResourceCategory", objResource.ResourceCategory)
            Cmd.Parameters.AddWithValue("@ResourcePageKey", objResource.ResourcePageKey)
            Cmd.Parameters.AddWithValue("@ResourceKeywords", objResource.ResourceKeywords)
            Cmd.Parameters.AddWithValue("@CampaignInfo", objResource.CampaignInfo)
            Cmd.Parameters.AddWithValue("@PublishToRss", objResource.PublishToRss)
            Cmd.Parameters.AddWithValue("@RssAuthor", objResource.RssAuthor)
            Cmd.Parameters.AddWithValue("@RssDescription", objResource.RssDescription)
            Cmd.Parameters.AddWithValue("@DisplayStartDate", objResource.DisplayStartDate)
            Cmd.Parameters.AddWithValue("@DisplayEndDate", objResource.DisplayEndDate)
            Cmd.Parameters.AddWithValue("@IsEnabled", objResource.IsEnabled)
            Cmd.Parameters.AddWithValue("@SortOrder", objResource.SortOrder)
            Cmd.Parameters.AddWithValue("@UpdatedDate", objResource.UpdatedDate)
            Cmd.Parameters.AddWithValue("@UpdatedBy", objResource.UpdatedBy)

            Try
                Conn.Open()
                Result = Cmd.ExecuteNonQuery()

            Catch ex As Exception
                Emagine.LogError(ex)
                strErrorMessage = ex.Message
            Finally
                Conn.Close()
            End Try

            Return Result
        End Function

        Public Shared Sub WriteCookie(ByVal strResourceID As String)
            Dim OutCookie As New HttpCookie("Resources")
            OutCookie.Values(strResourceID) = 1
            HttpContext.Current.Response.Cookies.Add(OutCookie)
        End Sub

        Public Shared Sub Register(ByVal strUserID As String, ByVal strResourceID As String, ByVal intSubmissionID As Integer)
            Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
            Dim SQL As String = "sp_Resources_AddUserRegistration"

            If strResourceID.Length > 0 Then
                Dim aryResourceIDs As Array = strResourceID.Split("^")
                Dim i As Integer

                For i = 0 To UBound(aryResourceIDs)
                    Dim Cmd As New SqlCommand(SQL, Conn)
                    Dim Param As New SqlParameter

                    Cmd.CommandType = CommandType.StoredProcedure
                    Cmd.Parameters.AddWithValue("@ResourceID", aryResourceIDs(i))
                    Cmd.Parameters.AddWithValue("@UserID", strUserID)
                    Cmd.Parameters.AddWithValue("@SubmissionID", intSubmissionID)

                    Try
                        Conn.Open()
                        Cmd.ExecuteNonQuery()

                    Catch ex As Exception
                        Emagine.LogError(ex)
                    Finally
                        Conn.Close()
                    End Try
                Next
            End If

        End Sub

    End Class


    Public Shared Function GetResourceCategories(ByVal strModuleKey As String) As SqlDataReader
        Dim dtr As SqlDataReader = Emagine.GetDataReader("SELECT DISTINCT ResourceCategory FROM Resources WHERE ResourceType = '" & strModuleKey & "' AND ResourceCategory IS NOT NULL AND ResourceCategory <> ''")
        Return dtr
    End Function

    Public Shared Function GetResourcePageKey() As String
        Dim ResourcePageKey As String = Emagine.GetPageFileName() 'HttpContext.Current.Request.ServerVariables("HTTP_X_REWRITE_URL")

        If InStrRev(ResourcePageKey, "/") > 1 Then
            ResourcePageKey = Right(ResourcePageKey, Len(ResourcePageKey) - 1)
            ResourcePageKey = Left(ResourcePageKey, InStr(ResourcePageKey, "/") - 1)
        Else
            ResourcePageKey = ""
        End If

        Return ResourcePageKey
    End Function

    Public Shared Function GetResourceID() As String
        Dim ResourceID As String = Emagine.GetPageFileName() 'HttpContext.Current.Request.ServerVariables("HTTP_X_REWRITE_URL")

        ResourceID = ResourceID.Replace("/print", "")

        If InStrRev(ResourceID, "/") > 1 Then
            ResourceID = Right(ResourceID, Len(ResourceID) - 1)
            ResourceID = Left(ResourceID, InStr(ResourceID, "/") - 1)
            ResourceID = HttpContext.Current.Server.UrlDecode(ResourceID)
        Else
            ResourceID = ""
        End If

        Return ResourceID
    End Function

    Public Shared Function GetResourceID(ByVal strUserID As String, ByVal strBatchID As String) As String
        Dim ResourceID As String = ""
        Dim ResourceData As DataTable = Emagine.GetDataTable("SELECT * FROM UserResources WHERE UserID = '" & strUserID & "' AND BatchID = '" & strBatchID & "'")
        If ResourceData.Rows.Count > 0 Then
            For i As Integer = 0 To (ResourceData.Rows.Count - 1)
                ResourceID = ResourceID & ResourceData.Rows(i).Item("ResourceID")
                If i < (ResourceData.Rows.Count - 1) Then ResourceID = ResourceID & "^"
            Next
        Else
            ResourceID = GetResourceID()
        End If
        ResourceData.Dispose()

        Return ResourceID
    End Function

    Public Shared Sub UpdateClickCount(ByVal strResourceID As String)
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_Modules_UpdateClickCount"

        Dim Cmd As New SqlCommand(SQL, Conn)
        Dim Param As New SqlParameter

        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.AddWithValue("@ResourceID", strResourceID)

        Try
            Conn.Open()
            Cmd.ExecuteNonQuery()

        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            Conn.Close()
        End Try
    End Sub

    Public Shared Function UserHasRegistered(ByVal strUserID As String, ByVal strResourceID As String) As Boolean
        Dim Result As Boolean = False
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_Resources_GetUserRegistration"

        Dim Cmd As New SqlCommand(SQL, Conn)
        Dim Param As New SqlParameter

        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.AddWithValue("@UserID", strUserID)
        Cmd.Parameters.AddWithValue("@ResourceID", strResourceID)

        Try
            Conn.Open()
            Dim Rs As SqlDataReader = Cmd.ExecuteReader()
            If Rs.HasRows Then Result = True

        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            Conn.Close()
        End Try

        Return Result
    End Function
End Class
