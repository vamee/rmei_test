Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class DL01
    Inherits Resources.Resource

    Public DownloadID As Integer = 0
    Public Shadows ModuleTypeID As Integer = 0
    Public CategoryID As Integer = 0
    Public Description As String = ""
    Public DisplayDate As String = ""
    Public Filename As String = ""
    Public FileSize As Integer = 0
    Public ExternalUrl As String = ""
    Public SortOrder As Integer = 0
    Public RegistrationRequired As Boolean = False

    Public Shared Function GetDownloads(ByVal intCategoryID As String, Optional ByVal blnIsEnabled As Boolean = False) As SqlDataReader
        Dim strSQL As String = ""
        If blnIsEnabled Then
            strSQL = "SELECT DISTINCT DisplayDate, ResourceName, FileName, ExternalUrl, Description, ResourcePageKey, DeliveryPageKey, FileSize, DownloadID, SortOrder, CategoryID, ResourceID, RegistrationRequired, IsEnabled, ResourceCategory, ModuleType FROM qryDownloads WHERE IsEnabled = 'True' AND CategoryID = " & intCategoryID & " ORDER BY SortOrder"
        Else
            strSQL = "SELECT DISTINCT DisplayDate, ResourceName, FileName, ExternalUrl, Description, ResourcePageKey, DeliveryPageKey, FileSize, DownloadID, SortOrder, CategoryID, ResourceID, RegistrationRequired, IsEnabled, ResourceCategory, ModuleType FROM qryDownloads WHERE CategoryID = " & intCategoryID & " ORDER BY SortOrder"
        End If
        Dim dtr As SqlDataReader = Emagine.GetDataReader(strSQL)
        Return dtr
    End Function

    Public Shared Function GetDownloads(ByVal intCategoryID As String, ByVal intDisplayPageID As Integer, Optional ByVal blnIsEnabled As Boolean = False) As SqlDataReader
        Dim strSQL As String = ""
        If blnIsEnabled Then
            strSQL = "SELECT DISTINCT DisplayDate, ResourceName, FileName, ExternalUrl, Description, ResourcePageKey, DeliveryPageKey, FileSize, DownloadID, SortOrder, CategoryID, ResourceID, RegistrationRequired, IsEnabled, ResourceCategory, ModuleType FROM qryDownloads WHERE IsEnabled = 'True' AND CategoryID = " & intCategoryID & " AND DisplayPageID = " & intDisplayPageID & " ORDER BY SortOrder"
        Else
            strSQL = "SELECT DISTINCT DisplayDate, ResourceName, FileName, ExternalUrl, Description, ResourcePageKey, DeliveryPageKey, FileSize, DownloadID, SortOrder, CategoryID, ResourceID, RegistrationRequired, IsEnabled, ResourceCategory, ModuleType FROM qryDownloads WHERE CategoryID = " & intCategoryID & " AND DisplayPageID = " & intDisplayPageID & " ORDER BY SortOrder"
        End If

        Dim dtr As SqlDataReader = Emagine.GetDataReader(strSQL)
        Return dtr
    End Function

    Public Shared Function GetDownload(ByVal intItemId As Integer) As DL01
        Dim Download As New DL01
        Dim dtr As SqlDataReader = Emagine.GetDataReader("SELECT * FROM qryDownloads WHERE DownloadID = " & intItemId)
        If dtr.Read Then
            With Download
                .DownloadID = dtr("DownloadID")
                .ModuleTypeID = dtr("ModuleTypeID")
                .CategoryId = dtr("CategoryId")
                .DisplayDate = dtr("DisplayDate")
                .ResourceId = dtr("ResourceID")
                .ResourceName = dtr("ResourceName").ToString
                .ResourceCategory = dtr("ResourceCategory").ToString
                .ResourcePageKey = dtr("ResourcePageKey").ToString()
                .ResourceKeywords = dtr("ResourceKeywords").ToString()
                .Description = dtr("Description").ToString
                .Filename = dtr("Filename").ToString
                .Filesize = dtr("Filesize")
                .ExternalUrl = dtr("ExternalUrl").ToString
                .SortOrder = dtr("SortOrder")
                .RegistrationRequired = dtr("RegistrationRequired")
                .IsEnabled = dtr("IsEnabled")
                .DisplayStartDate = dtr("DisplayStartDate").ToString
                .DisplayEndDate = dtr("DisplayEndDate").ToString
                .CampaignInfo = dtr("CampaignInfo")
            End With
        Else
            Download = Nothing
        End If

        dtr.Close()

        Return Download
    End Function

    Public Shared Function GetDownloadCount(ByVal intCategoryId As String) As Integer
        Dim strSQL As String
        strSQL = "SELECT COUNT(DownloadID) AS RecordCount FROM Downloads WHERE CategoryID = " & intCategoryId
        Return Emagine.GetNumber(Emagine.GetDbValue(strSQL))
    End Function

    Public Function AddDownload(ByVal Download As DL01) As Integer
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_DL01_AddDownload"
        Dim DownloadID As Integer = 0

        Dim Cmd As New SqlCommand(SQL, Conn)
        Dim Param As New SqlParameter

        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.AddWithValue("@CategoryID", Download.CategoryId)
        Cmd.Parameters.AddWithValue("@ResourceID", Download.ResourceId)
        Cmd.Parameters.AddWithValue("@Description", Download.Description)
        Cmd.Parameters.AddWithValue("@DisplayDate", Download.DisplayDate)
        Cmd.Parameters.AddWithValue("@FileName", Download.Filename)
        Cmd.Parameters.AddWithValue("@FileSize", Download.Filesize)
        Cmd.Parameters.AddWithValue("@ExternalUrl", Download.ExternalUrl)
        Cmd.Parameters.AddWithValue("@SortOrder", Download.SortOrder)
        Cmd.Parameters.AddWithValue("@RegistrationRequired", Download.RegistrationRequired)

        Try
            Conn.Open()
            Dim DataReader As SqlDataReader = Cmd.ExecuteReader()
            If DataReader.Read() Then
                DownloadID = DataReader(0)
            End If
            DataReader.Close()

        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            Conn.Close()
        End Try

        Return DownloadID
    End Function

    Public Shared Function UpdateDownload(ByVal Download As DL01) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_DL01_UpdateDownload"
        Dim Result As Boolean = False

        Dim Cmd As New SqlCommand(SQL, Conn)
        Dim Param As New SqlParameter

        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.AddWithValue("@DownloadID", Download.DownloadID)
        Cmd.Parameters.AddWithValue("@CategoryID", Download.CategoryID)
        Cmd.Parameters.AddWithValue("@Description", Download.Description)
        Cmd.Parameters.AddWithValue("@DisplayDate", Download.DisplayDate)
        Cmd.Parameters.AddWithValue("@FileName", Download.Filename)
        Cmd.Parameters.AddWithValue("@FileSize", Download.FileSize)
        Cmd.Parameters.AddWithValue("@ExternalUrl", Download.ExternalUrl)
        Cmd.Parameters.AddWithValue("@SortOrder", Download.SortOrder)
        Cmd.Parameters.AddWithValue("@RegistrationRequired", Download.RegistrationRequired)

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

    Public Shared Function DeleteDownload(ByVal intDownloadID As Integer) As Boolean
        Dim FileName As String = Emagine.GetDbValue("SELECT FileName FROM Downloads WHERE DownloadID = " & intDownloadID)

        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_DL01_DeleteDownload"
        Dim Result As Boolean = False

        Dim Cmd As New SqlCommand(SQL, Conn)
        Dim Param As New SqlParameter

        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.AddWithValue("@DownloadID", intDownloadID)

        Try
            Conn.Open()
            Result = Cmd.ExecuteNonQuery()

            Emagine.DeleteFile(HttpContext.Current.Server.MapPath(FileName))

        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            Conn.Close()
        End Try

        Return Result
    End Function

    Public Shared Function IsUniquePageKey(ByVal strPageKey As String, ByVal intDownloadID As Integer, ByVal intCategoryId As Integer) As Boolean
        Dim strSQL As String = "SELECT Count(*) FROM qryDownloads WHERE DownloadID <> " & intDownloadID & " AND ResourcePageKey = '" & strPageKey & "' AND CategoryId = " & intCategoryId
        If Emagine.GetDbValue(strSQL) = 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function GetMaxSortOrder(ByVal intCategoryID As Integer) As Integer
        Return Emagine.GetNumber(Emagine.GetDbValue("SELECT MAX(SortOrder) As MaxSortOrder FROM Downloads WHERE CategoryID = " & intCategoryID))
    End Function

    Public Shared Function UpdateSortOrder(ByVal intDownloadID As Integer, ByVal intSortOrder As Integer) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim Command As New SqlCommand
        Dim Download As DL01 = DL01.GetDownload(intDownloadID)
        Dim Result As Boolean = False
        Dim SQL As String = ""

        If Download.SortOrder > intSortOrder Then
            SQL = "UPDATE Downloads SET SortOrder = SortOrder + 1 WHERE CategoryID = " & Download.CategoryId & " AND SortOrder >= " & intSortOrder & " AND SortOrder < " & Download.SortOrder & " AND DownloadID <> " & intDownloadID
        ElseIf Download.SortOrder < intSortOrder Then
            SQL = "UPDATE Downloads SET SortOrder = SortOrder - 1 WHERE CategoryID = " & Download.CategoryId & " AND SortOrder <= " & intSortOrder & " AND SortOrder > " & Download.SortOrder & " AND DownloadID <> " & intDownloadID
        End If

        Try
            Command.Connection = Conn
            Conn.Open()

            If SQL.Length > 0 Then
                Command.CommandText = SQL
                Command.ExecuteNonQuery()
            End If

            If intDownloadID > 0 Then
                SQL = "UPDATE Downloads SET SortOrder = " & intSortOrder & " WHERE DownloadID = " & intDownloadID
                Command.CommandText = SQL
                Command.ExecuteNonQuery()
            End If

            ResetSortOrder(Download.CategoryId)

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
        Dim SQL = "SELECT DownloadID, SortOrder FROM Downloads WHERE CategoryID = " & intCategoryID & " ORDER BY SortOrder"
        Dim Command As New SqlCommand(SQL, Conn)
        Dim Counter As Integer = 0
        Dim Result As Boolean = False
        Dim Rs As SqlDataReader = Emagine.GetDataReader(SQL)

        Try
            Conn.Open()

            Do While Rs.Read
                Counter += 1
                Command.CommandText = "UPDATE Downloads SET SortOrder = " & Counter & " WHERE DownloadID = " & Rs("DownloadID")

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
