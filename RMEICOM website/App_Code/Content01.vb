Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class Content01
    Inherits Resources.Resource

    Public ContentID As Integer = 0
    Public StatusID As Integer = 20
    Public ModuleKey As String = ""
    Public ForeignKey As String = ""
    Public Version As String = ""
    Public VersionNotes As String = ""
    Public Content As String = ""

    Public Shared Function GetAllContent(ByVal strModuleKey As String, ByVal strForeignKey As String) As DataTable
        Dim Sql As String = "SELECT * FROM Content WHERE ModuleKey=@ModuleKey AND ForeignValue=@ForeignValue"
        Dim Command As New SqlCommand
        Command.Parameters.AddWithValue("@ModuleKey", strModuleKey)
        Command.Parameters.AddWithValue("@ForeignKey", strForeignKey)

        Dim ContentData As DataTable = Emagine.GetDataTable(Sql, Command)

        Return ContentData
    End Function

    Public Shared Function GetContent(ByVal strModuleKey As String, ByVal strForeignKey As String) As Content01
        Dim objContent As New Content01

        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "SELECT * FROM qryContent WHERE ModuleKey = @ModuleKey AND ForeignKey = @ForeignKey"

        Dim Command As New SqlCommand(SQL, Conn)

        Dim Param As New SqlParameter
        Command.Parameters.AddWithValue("@ModuleKey", strModuleKey)
        Command.Parameters.AddWithValue("@ForeignKey", strForeignKey)

        objContent = objContent.Populate(objContent, SQL, Command)

        Return objContent
    End Function

    Public Shared Function GetContent(ByVal intContentID As Integer) As Content01
        Dim objContent As New Content01

        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "SELECT * FROM qryContent WHERE ContentID=@ContentID"

        Dim Command As New SqlCommand(SQL, Conn)

        Dim Param As New SqlParameter
        Command.Parameters.AddWithValue("@ContentID", intContentID)

        objContent = objContent.Populate(objContent, SQL, Command)

        Return objContent
    End Function

    Public Shared Function GetContent(ByVal strVersionNotes As String) As Content01
        Dim objContent As New Content01

        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "SELECT * FROM qryContent WHERE VersionNotes = @VersionNotes"

        Dim Command As New SqlCommand(SQL, Conn)
        Command.Parameters.AddWithValue("@VersionNotes", strVersionNotes)

        objContent = objContent.Populate(objContent, SQL, Command)

        Return objContent
    End Function

    Public Function Populate(ByVal objContent As Content01, ByVal strSql As String, ByVal objCommand As SqlCommand) As Content01
        Dim dataContent As DataTable = Emagine.GetDataTable(strSql, objCommand)

        If dataContent.Rows.Count > 0 Then
            objContent.ContentID = dataContent.Rows(0).Item("ContentID")
            objContent.ResourceID = dataContent.Rows(0).Item("ResourceID").ToString
            objContent.StatusID = dataContent.Rows(0).Item("StatusID")
            objContent.ModuleKey = dataContent.Rows(0).Item("ModuleKey").ToString
            objContent.ForeignKey = dataContent.Rows(0).Item("ForeignKey").ToString
            objContent.Version = dataContent.Rows(0).Item("Version").ToString
            objContent.Content = dataContent.Rows(0).Item("Content").ToString
            If IsDate(dataContent.Rows(0).Item("DisplayStartDate").ToString) Then objContent.DisplayStartDate = dataContent.Rows(0).Item("DisplayStartDate")
            If IsDate(dataContent.Rows(0).Item("DisplayEndDate").ToString) Then objContent.DisplayEndDate = dataContent.Rows(0).Item("DisplayEndDate")
            If IsDate(dataContent.Rows(0).Item("CreatedDate").ToString) Then objContent.CreatedDate = dataContent.Rows(0).Item("CreatedDate")
            objContent.CreatedBy = dataContent.Rows(0).Item("CreatedBy").ToString
            If IsDate(dataContent.Rows(0).Item("UpdatedDate").ToString) Then objContent.UpdatedDate = dataContent.Rows(0).Item("UpdatedDate")
            objContent.UpdatedBy = dataContent.Rows(0).Item("UpdatedBy").ToString
        End If

        Return objContent
    End Function

    'Public Shared Function GetContent(ByVal strVersionNotes As String) As String
    '    Dim Result As String = ""
    '    Dim ErrorMessage As String = ""

    '    Dim SQL As String = "SELECT Content FROM Content WHERE VersionNotes = @VersionNotes"

    '    Dim Command As New SqlCommand
    '    Command.Parameters.AddWithValue("@VersionNotes", strVersionNotes)

    '    Result = Emagine.GetDbValue(SQL, Command)

    '    Return Result
    'End Function

    'Public Shared Function GetContent(ByVal intContentID As Integer) As String
    '    Dim Result As String = ""
    '    Dim ErrorMessage As String = ""

    '    Dim SQL As String = "SELECT Content FROM Content WHERE ContentID = @ContentID"

    '    Dim Command As New SqlCommand
    '    Command.Parameters.AddWithValue("@ContentID", intContentID)

    '    Result = Emagine.GetDbValue(SQL, Command)

    '    Return Result
    'End Function

    Public Shared Function UpdateContent(ByVal objContent As Content01) As Boolean
        Dim Result As Boolean = False
        Dim SQL As String = "UPDATE Content SET ResourceID=@ResourceID, StatusID=@StatusID, ModuleKey=@ModuleKey, ForeignKey=@ForeignKey, Version=@Version, VersionNotes=@VersionNotes, Content=@Content WHERE ContentID = @ContentID"

        Dim Command As New SqlCommand
        Command.Parameters.AddWithValue("@ContentID", objContent.ContentID)
        Command.Parameters.AddWithValue("@ResourceID", objContent.ResourceID)
        Command.Parameters.AddWithValue("@StatusID", objContent.StatusID)
        Command.Parameters.AddWithValue("@ModuleKey", objContent.ModuleKey)
        Command.Parameters.AddWithValue("@ForeignKey", objContent.ForeignKey)
        Command.Parameters.AddWithValue("@Version", objContent.Version)
        Command.Parameters.AddWithValue("@VersionNotes", objContent.VersionNotes)
        Command.Parameters.AddWithValue("@Content", objContent.Content)

        Result = Emagine.ExecuteSQL(SQL, Command)

        If Result Then
            Dim MyResource As Resources.Resource = Resources.Resource.GetResource(objContent.ResourceID)

            If MyResource.ResourceID.Length > 0 Then
                MyResource.ResourceID = objContent.ResourceID
                MyResource.DisplayStartDate = objContent.DisplayStartDate
                MyResource.DisplayEndDate = objContent.DisplayEndDate
                MyResource.SortOrder = objContent.SortOrder
                MyResource.IsEnabled = objContent.IsEnabled
                MyResource.UpdatedBy = objContent.UpdatedBy
                MyResource.UpdatedDate = Date.Now()

                Result = Resources.Resource.UpdateResource(MyResource)
            Else
                MyResource.ResourceID = objContent.ResourceID
                MyResource.ResourceType = "Content01"
                MyResource.DisplayStartDate = objContent.DisplayStartDate
                MyResource.DisplayEndDate = objContent.DisplayEndDate
                MyResource.SortOrder = objContent.SortOrder
                MyResource.IsEnabled = objContent.IsEnabled
                MyResource.CreatedBy = objContent.UpdatedBy
                MyResource.UpdatedBy = objContent.UpdatedBy

                Result = Resources.Resource.AddResource(MyResource)
            End If

        End If

            Return Result
    End Function

    Public Shared Function AddContent(ByVal objContent As Content01) As Content01
        Dim Result As Boolean = False
        'Dim MyContent As New Content01
        objContent.ResourceID = Emagine.GetUniqueID()
        Dim SQL As String = "INSERT INTO Content (ResourceID,StatusID,ModuleKey,ForeignKey,Version,VersionNotes,Content) VALUES (@ResourceID,@StatusID,@ModuleKey,@ForeignKey,@Version,@VersionNotes,@Content)"

        Dim Command As New SqlCommand
        Command.Parameters.AddWithValue("@ResourceID", objContent.ResourceID)
        Command.Parameters.AddWithValue("@StatusID", objContent.StatusID)
        Command.Parameters.AddWithValue("@ModuleKey", objContent.ModuleKey)
        Command.Parameters.AddWithValue("@ForeignKey", objContent.ForeignKey)
        Command.Parameters.AddWithValue("@Version", objContent.Version)
        Command.Parameters.AddWithValue("@VersionNotes", objContent.VersionNotes)
        Command.Parameters.AddWithValue("@Content", objContent.Content)

        Result = Emagine.ExecuteSQL(SQL, Command)
        Command.Dispose()

        If Result Then
            objContent.ContentID = Emagine.GetNumber(Emagine.GetDbValue("SELECT MAX(ContentID) AS MaxContentID FROM Content"))

            Dim MyResource As New Resources.Resource
            MyResource.ResourceID = objContent.ResourceID
            MyResource.ResourceType = "Content01"
            MyResource.DisplayStartDate = objContent.DisplayStartDate
            MyResource.DisplayEndDate = objContent.DisplayEndDate
            MyResource.SortOrder = objContent.SortOrder
            MyResource.IsEnabled = objContent.IsEnabled
            MyResource.CreatedBy = objContent.CreatedBy
            MyResource.UpdatedBy = objContent.UpdatedBy

            Result = Resources.Resource.AddResource(MyResource)

            If Not Result Then Emagine.ExecuteSQL("DELETE FROM Content WHERE ResourceID = '" & objContent.ResourceID & "'")
        End If

        Return objContent
    End Function

    Public Shared Function DeleteContent(ByVal objContent As Content01, Optional ByRef strErrorMessage As String = "") As Boolean
        Dim Result As Boolean = False

        If Emagine.ExecuteSQL("DELETE FROM Content WHERE ContentID = " & objContent.ContentID, strErrorMessage) Then
            Dim MyResource As Resources.Resource = Resources.Resource.GetResource(objContent.ResourceID)
            Result = Resources.Resource.DeleteResource(MyResource, strErrorMessage)
        End If

        Return Result
    End Function

    Public Shared Function DeleteContent(ByVal strModuleKey As String, ByVal strForeignKey As String, Optional ByRef strErrorMessage As String = "") As Boolean
        Dim Result As Boolean = False
        Dim Sql As String = "SELECT * FROM Content WHERE ModuleKey=@ModuleKey AND ForeignValue=@ForeignValue"
        Dim Command As New SqlCommand
        Command.Parameters.AddWithValue("@ModuleKey", strModuleKey)
        Command.Parameters.AddWithValue("@ForeignKey", strForeignKey)

        Dim ContentData As DataTable = Emagine.GetDataTable(Sql, Command)

        For Each Row As DataRow In ContentData.Rows
            Result = Emagine.ExecuteSQL("DELETE FROM Content WHERE ContentID = " & Row("ContentID"), strErrorMessage)
            If Result = True Then
                Emagine.ExecuteSQL("DELETE FROM Resources WHERE ModuleKey = '" & strModuleKey & "' AND ResourceID = '" & Row("ResourceID") & "'", strErrorMessage)
                If Result = False Then Exit For
            Else
                Exit For
            End If
        Next

        Return Result
    End Function

    Public Shared Function GetMaxVersion(ByVal strModuleKey As String, ByVal strForeignKey As String, Optional ByRef strErrorMessage As String = "") As Integer
        Dim MaxVersion As Integer = 0
        Dim Sql As String = "SELECT MAX(Version) AS MaxVersion FROM Content WHERE ModuleKey=@ModuleKey AND ForeignKey=@ForeignKey"
        Dim Command As New SqlCommand
        Command.Parameters.AddWithValue("@ModuleKey", strModuleKey)
        Command.Parameters.AddWithValue("@ForeignKey", strForeignKey)

        MaxVersion = Emagine.GetNumber(Emagine.GetDbValue(Sql, Command, strErrorMessage))

        Return MaxVersion
    End Function

    Public Shared Function PromoteContent(ByVal intContentID As Integer, ByVal strModuleKey As String, ByVal strForeignKey As String, Optional ByRef strErrorMessage As String = "") As Boolean
        Dim Result As Boolean = False
        Dim Sql As String = "UPDATE Content SET StatusID = 10 WHERE ModuleKey=@ModuleKey AND ForeignKey=@ForeignKey AND StatusID = 20"
        Dim Command As New SqlCommand

        Command.Parameters.AddWithValue("@ModuleKey", strModuleKey)
        Command.Parameters.AddWithValue("@ForeignKey", strForeignKey)

        Result = Emagine.ExecuteSQL(Sql, Command, strErrorMessage)

        If Result = True Then
            Sql = "UPDATE Content SET StatusID = 20 WHERE ModuleKey=@ModuleKey AND ForeignKey=@ForeignKey AND ContentID=@ContentID"
            Command.Parameters.AddWithValue("@ContentID", intContentID)
            Result = Emagine.ExecuteSQL(Sql, Command, strErrorMessage)
        End If

        Return Result
    End Function

    Public Shared Function GetContentID(ByVal strModuleKey As String, ByVal strForeignKey As String, ByVal intStatusID As Integer, Optional ByRef strErrorMessage As String = "") As Integer
        Dim ContentID As Integer = 0
        Dim Sql As String = "SELECT ContentID FROM Content WHERE ModuleKey=@ModuleKey AND ForeignKey=@ForeignKey AND StatusID = @StatusID"
        Dim Command As New SqlCommand
        Command.Parameters.AddWithValue("@ModuleKey", strModuleKey)
        Command.Parameters.AddWithValue("@ForeignKey", strForeignKey)
        Command.Parameters.AddWithValue("@StatusID", intStatusID)

        ContentID = Emagine.GetNumber(Emagine.GetDbValue(Sql, Command, strErrorMessage))

        Return ContentID
    End Function

    Public Shared Function GetContentCount(ByVal strModuleKey As String, ByVal strForeignKey As String, Optional ByRef strErrorMessage As String = "") As Integer
        Dim ContentCount As Integer = 0
        Dim Sql As String = "SELECT COUNT(ContentID) AS ContentCount FROM Content WHERE ModuleKey=@ModuleKey AND ForeignKey=@ForeignKey AND StatusID > 0"
        Dim Command As New SqlCommand
        Command.Parameters.AddWithValue("@ModuleKey", strModuleKey)
        Command.Parameters.AddWithValue("@ForeignKey", strForeignKey)

        ContentCount = Emagine.GetNumber(Emagine.GetDbValue(Sql, Command, strErrorMessage))

        Return ContentCount
    End Function

    Public Shared Function GetContentVersions(ByVal strModuleKey As String, ByVal strForeignKey As String, ByVal intStatusID As Integer, Optional ByRef strErrorMessage As String = "") As DataTable

        Dim Sql As String = "SELECT * FROM qryPageContent WHERE ModuleKey=@ModuleKey AND ForeignKey=@ForeignKey AND StatusID = @StatusID"
        Dim Command As New SqlCommand
        Command.Parameters.AddWithValue("@ModuleKey", strModuleKey)
        Command.Parameters.AddWithValue("@ForeignKey", strForeignKey)
        Command.Parameters.AddWithValue("@StatusID", intStatusID)

        Return Emagine.GetDataTable(Sql, Command)
    End Function

End Class


