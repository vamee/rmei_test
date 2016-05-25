Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class Events01
    Inherits Resources.Resource

    Public EventID As Integer
    Public CategoryID As Integer
    Public CategoryName As String
    Public EventSummary As String
    Public EventDescription As String
    Public ArchiveURL As String
    Public ImageUrl As String
    Public AllowRegistration As Boolean
    Public SortOrder As Integer

    Public Shared Function GetEvents(ByVal intCategoryId As String) As SqlDataReader
        Dim strSQL As String
        strSQL = "SELECT * FROM qryEvents WHERE CategoryID = " & intCategoryId & " ORDER BY SortOrder"
        Dim dtr As SqlDataReader = Emagine.GetDataReader(strSQL)
        Return dtr
    End Function

    Public Shared Function GetEvent(ByVal intItemId As Integer) As Events01
        Dim Event01 As New Events01
        Dim dtr As SqlDataReader = Emagine.GetDataReader("SELECT * FROM qryEvents WHERE EventID = " & intItemId)
        If dtr.Read Then
            Event01.EventID = dtr("EventID")
            Event01.CategoryID = dtr("CategoryID")
            Event01.ResourceId = dtr("ResourceId")
            Event01.CategoryName = dtr("CategoryName").ToString()
            Event01.SortOrder = dtr("SortOrder")
            Event01.ResourceName = dtr("ResourceName").ToString()
            Event01.ResourceCategory = dtr("ResourceCategory")
            Event01.ResourcePageKey = dtr("ResourcePageKey").ToString()
            Event01.EventSummary = dtr("EventSummary").ToString()
            Event01.ResourceKeywords = dtr("ResourceKeywords").ToString()
            Event01.EventDescription = dtr("EventDescription").ToString()
            Event01.ArchiveURL = dtr("ArchiveURL").ToString()
            Event01.ImageUrl = dtr("ImageUrl").ToString()
            Event01.AllowRegistration = dtr("AllowRegistration")
        Else
            Event01 = Nothing
        End If

        dtr.Close()

        Return Event01
    End Function

    Public Shared Function AddEvent(ByVal objEvent As Events01) As Integer
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_Events01_AddEvent"
        Dim EventID As Integer = 0

        Dim Cmd As New SqlCommand(SQL, Conn)
        Dim Param As New SqlParameter

        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.AddWithValue("@CategoryID", objEvent.CategoryID)
        Cmd.Parameters.AddWithValue("@ResourceID", objEvent.ResourceId)
        Cmd.Parameters.AddWithValue("@EventSummary", objEvent.EventSummary)
        Cmd.Parameters.AddWithValue("@EventDescription", objEvent.EventDescription)
        Cmd.Parameters.AddWithValue("@ArchiveURL", objEvent.ArchiveURL)
        Cmd.Parameters.AddWithValue("@ImageUrl", objEvent.ImageUrl)
        Cmd.Parameters.AddWithValue("@AllowRegistration", objEvent.AllowRegistration)
        Cmd.Parameters.AddWithValue("@SortOrder", objEvent.SortOrder)

        Try
            Conn.Open()
            Dim DataReader As SqlDataReader = Cmd.ExecuteReader()
            If DataReader.Read() Then
                EventID = DataReader(0)
            End If
            DataReader.Close()

        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            Conn.Close()
        End Try

        Return EventID
    End Function

    Public Shared Function UpdateEvent(ByVal objEvent As Events01) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_Events01_UpdateEvent"
        Dim Result As Boolean = False

        Dim Cmd As New SqlCommand(SQL, Conn)
        Dim Param As New SqlParameter

        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.AddWithValue("@EventID", objEvent.EventID)
        Cmd.Parameters.AddWithValue("@CategoryID", objEvent.CategoryID)
        Cmd.Parameters.AddWithValue("@EventSummary", objEvent.EventSummary)
        Cmd.Parameters.AddWithValue("@EventDescription", objEvent.EventDescription)
        Cmd.Parameters.AddWithValue("@ArchiveURL", objEvent.ArchiveURL)
        Cmd.Parameters.AddWithValue("@ImageUrl", objEvent.ImageUrl)
        Cmd.Parameters.AddWithValue("@AllowRegistration", objEvent.AllowRegistration)
        Cmd.Parameters.AddWithValue("@SortOrder", objEvent.SortOrder)

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

    Public Shared Function DeleteEvent(ByVal intEventID As Integer) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_Events01_DeleteEvent"
        Dim Result As Boolean = False

        Dim Cmd As New SqlCommand(SQL, Conn)
        Dim Param As New SqlParameter

        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.AddWithValue("@EventID", intEventID)

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

    Public Shared Function GetEventCount(ByVal intCategoryId As String) As Integer
        Dim strSQL As String
        strSQL = "SELECT COUNT(*) AS RecordCount FROM Events WHERE CategoryID = " & intCategoryId
        Return Emagine.GetNumber(Emagine.GetDbValue(strSQL))
    End Function

    Public Shared Function GetMaxSortOrder(ByVal intCategoryID As Integer) As Integer
        Return Emagine.GetNumber(Emagine.GetDbValue("SELECT MAX(SortOrder) As MaxSortOrder FROM Events WHERE CategoryID = " & intCategoryID))
    End Function

    Public Shared Function IsUniquePageKey(ByVal strPageKey As String, ByVal intEventID As Integer, ByVal intCategoryId As Integer) As Boolean
        Dim strSQL As String = "SELECT Count(*) FROM qryEvents WHERE EventID <> " & intEventID & " AND ResourcePageKey = '" & strPageKey & "' AND CategoryId = " & intCategoryId
        If Emagine.GetDbValue(strSQL) = 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function UpdateSortOrder(ByVal intEventID As Integer, ByVal intSortOrder As Integer) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim Command As New SqlCommand
        Dim MyEvent As Events01 = Events01.GetEvent(intEventID)
        Dim Result As Boolean = False
        Dim SQL As String = ""

        If MyEvent.SortOrder > intSortOrder Then
            SQL = "UPDATE Events SET SortOrder = SortOrder + 1 WHERE CategoryID = " & MyEvent.CategoryID & " AND SortOrder >= " & intSortOrder & " AND SortOrder < " & MyEvent.SortOrder & " AND EventID <> " & intEventID
        ElseIf MyEvent.SortOrder < intSortOrder Then
            SQL = "UPDATE Events SET SortOrder = SortOrder - 1 WHERE CategoryID = " & MyEvent.CategoryID & " AND SortOrder <= " & intSortOrder & " AND SortOrder > " & MyEvent.SortOrder & " AND EventID <> " & intEventID
        End If

        Try
            Command.Connection = Conn
            Conn.Open()

            If SQL.Length > 0 Then
                Command.CommandText = SQL
                Command.ExecuteNonQuery()
            End If

            If intEventID > 0 Then
                SQL = "UPDATE Events SET SortOrder = " & intSortOrder & " WHERE EventID = " & intEventID
                Command.CommandText = SQL
                Command.ExecuteNonQuery()
            End If

            ResetSortOrder(MyEvent.CategoryID)

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
        Dim SQL = "SELECT EventID, SortOrder FROM Events WHERE CategoryID = " & intCategoryID & " ORDER BY SortOrder"
        Dim Command As New SqlCommand(SQL, Conn)
        Dim Counter As Integer = 0
        Dim Result As Boolean = False
        Dim Rs As SqlDataReader = Emagine.GetDataReader(SQL)

        Try
            Conn.Open()

            Do While Rs.Read
                Counter += 1
                Command.CommandText = "UPDATE Events SET SortOrder = " & Counter & " WHERE EventID = " & Rs("EventID")

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

Public Class EventDates
    Public Shared Function GetEventDates(ByVal intEventID As String, Optional ByRef strErrorMessage As String = "") As SqlDataReader
        Dim MyCommand As New SqlCommand
        MyCommand.Parameters.AddWithValue("@EventID", intEventID)

        Dim Sql As String = "SELECT * FROM qryEventDates WHERE EventID = @EventID ORDER BY EventDate"

        Return Emagine.GetDataReader(Sql, MyCommand, strErrorMessage)
    End Function

    Public Shared Function GetEventDatesByCategory(ByVal intCategoryID As String, Optional ByRef strErrorMessage As String = "") As SqlDataReader
        Dim MyCommand As New SqlCommand
        MyCommand.Parameters.AddWithValue("@CategoryID", intCategoryID)

        Dim Sql As String = "SELECT * FROM qryEventDates WHERE CategoryID = @CategoryID ORDER BY EventDate"

        Return Emagine.GetDataReader(Sql, MyCommand, strErrorMessage)
    End Function

End Class

Public Class EventDate
    Public EventDateID As Integer = -1
    Public EventID As Integer = -1
    Public EventDate As String = ""
    Public Duration As String = ""
    Public Location As String = ""
    Public CreatedDate As Date = "1/1/1900"
    Public CreatedBy As String = ""
    Public UpdatedDate As Date = "1/1/1900"
    Public UpdatedBy As String = ""

    Public Shared Function GetEventDate(ByVal intEventDateID As Integer, Optional ByRef strErrorMessage As String = "") As EventDate
        Dim MyCommand As New SqlCommand
        MyCommand.Parameters.AddWithValue("@EventDateID", intEventDateID)

        Dim EventDate As New EventDate
        Dim MyDataReader As SqlDataReader = Emagine.GetDataReader("SELECT * FROM EventDates WHERE EventDateID = @EventDateID", MyCommand)
        If MyDataReader.Read Then
            EventDate.EventDateID = MyDataReader("EventDateID")
            EventDate.EventID = MyDataReader("EventID")
            EventDate.EventDate = MyDataReader("EventDate").ToString
            EventDate.Duration = MyDataReader("Duration").ToString
            EventDate.Location = MyDataReader("Location").ToString
            EventDate.CreatedDate = MyDataReader("CreatedDate")
            EventDate.CreatedBy = MyDataReader("CreatedBy").ToString
            EventDate.UpdatedDate = MyDataReader("UpdatedDate")
            EventDate.UpdatedBy = MyDataReader("UpdatedBy").ToString
        End If

        MyDataReader.Close()

        Return EventDate
    End Function

    Public Shared Function Add(ByVal objEventDate As EventDate, Optional ByRef strErrorMessage As String = "") As Boolean
        Dim SqlBuilder As New StringBuilder

        Dim MyCommand As New SqlCommand
        MyCommand.Parameters.AddWithValue("@EventID", objEventDate.EventID)
        MyCommand.Parameters.AddWithValue("@EventDate", objEventDate.EventDate)
        MyCommand.Parameters.AddWithValue("@Duration", objEventDate.Duration)
        MyCommand.Parameters.AddWithValue("@Location", objEventDate.Location)
        MyCommand.Parameters.AddWithValue("@CreatedBy", objEventDate.CreatedBy)
        MyCommand.Parameters.AddWithValue("@UpdatedBy", objEventDate.UpdatedBy)

        SqlBuilder.Append("INSERT INTO EventDates ")
        SqlBuilder.Append("(EventID,EventDate,Duration,Location,CreatedBy,UpdatedBy) ")
        SqlBuilder.Append("VALUES ")
        SqlBuilder.Append("(@EventID,@EventDate,@Duration,@Location,@CreatedBy,@UpdatedBy)")

        Return Emagine.ExecuteSQL(SqlBuilder.ToString, MyCommand, strErrorMessage)
    End Function

    Public Shared Function Update(ByVal objEventDate As EventDate, Optional ByRef strErrorMessage As String = "") As Boolean
        Dim SqlBuilder As New StringBuilder

        Dim MyCommand As New SqlCommand
        MyCommand.Parameters.AddWithValue("@EventDateID", objEventDate.EventDateID)
        MyCommand.Parameters.AddWithValue("@EventDate", objEventDate.EventDate)
        MyCommand.Parameters.AddWithValue("@Duration", objEventDate.Duration)
        MyCommand.Parameters.AddWithValue("@Location", objEventDate.Location)
        MyCommand.Parameters.AddWithValue("@UpdatedDate", objEventDate.UpdatedDate)
        MyCommand.Parameters.AddWithValue("@UpdatedBy", objEventDate.UpdatedBy)

        SqlBuilder.Append("UPDATE EventDates SET ")
        SqlBuilder.Append("EventDate=@EventDate,")
        SqlBuilder.Append("Duration=@Duration,")
        SqlBuilder.Append("Location=@Location,")
        SqlBuilder.Append("UpdatedDate=@UpdatedDate,")
        SqlBuilder.Append("UpdatedBy=@UpdatedBy ")
        SqlBuilder.Append("WHERE EventDateID = @EventDateID")

        Return Emagine.ExecuteSQL(SqlBuilder.ToString, MyCommand, strErrorMessage)
    End Function

    Public Shared Function Delete(ByVal intEventDateID As Integer, Optional ByRef strErrorMessage As String = "") As Boolean
        Dim SqlBuilder As New StringBuilder

        Dim MyCommand As New SqlCommand
        MyCommand.Parameters.AddWithValue("@EventDateID", intEventDateID)

        Return Emagine.ExecuteSQL(SqlBuilder.ToString, MyCommand, strErrorMessage)
    End Function

End Class


