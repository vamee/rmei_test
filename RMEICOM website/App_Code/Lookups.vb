Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class Lookups
    Public Shared Function GetLookups(Optional ByVal strSortBy As String = "", Optional ByVal strSortDirection As String = "") As DataTable
        Dim Sql As String = "SELECT * FROM Lookups"
        If strSortBy.Length > 0 Then
            Sql = Sql & " ORDER BY " & strSortBy
            If strSortDirection.Length > 0 Then Sql = Sql & " " & strSortDirection
        End If

        Return Emagine.GetDataTable(Sql)
    End Function
End Class


Public Class Lookup
    Public LookupID As Integer = 0
    Public LookupName As String = ""
    Public ValueFieldType As String = "Editable"
    Public CanEditLookup As Boolean = True
    Public CanDeleteLookup As Boolean = True
    Public CanAddOptions As Boolean = True
    Public CanUpdateOptions As Boolean = True
    Public CanDeleteOptions As Boolean = True

    Public Shared Function GetLookup(ByVal intLookupID As Integer) As Lookup
        Return PopulateObject(intLookupID)
    End Function

    Public Shared Function GetLookup(ByVal strLookupName As String) As Lookup
        Dim LookupID As Integer = Emagine.GetNumber(Emagine.GetDbValue("SELECT LookupID FROM Lookups WHERE LookupName = '" & strLookupName.Replace("'", "''") & "'"))
        Return PopulateObject(LookupID)
    End Function

    Public Shared Function Add(ByVal objLookup As Lookup) As Lookup
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)

        Dim Command As New SqlCommand("sp_Lookups_AddLookup", Conn)
        Command.CommandType = CommandType.StoredProcedure
        Command.Parameters.AddWithValue("@LookupName", objLookup.LookupName)
        Command.Parameters.AddWithValue("@ValueFieldType", objLookup.ValueFieldType)

        Dim OutputParam As New SqlParameter
        OutputParam.ParameterName = "@LookupID"
        OutputParam.SqlDbType = SqlDbType.Int
        OutputParam.Direction = ParameterDirection.Output
        Command.Parameters.Add(OutputParam)

        Try
            Conn.Open()

            Command.ExecuteNonQuery()
            objLookup.LookupID = Command.Parameters("@LookupID").Value

        Catch ex As Exception
            Emagine.LogError(ex)

        Finally
            Conn.Close()
        End Try

        Return objLookup
    End Function

    Public Shared Function Update(ByVal objLookup As Lookup) As Boolean
        Dim Result As Boolean = False
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)

        Dim Command As New SqlCommand("sp_Lookups_UpdateLookup", Conn)
        Command.CommandType = CommandType.StoredProcedure
        Command.Parameters.AddWithValue("@LookupID", objLookup.LookupID)
        Command.Parameters.AddWithValue("@LookupName", objLookup.LookupName)
        Command.Parameters.AddWithValue("@ValueFieldType", objLookup.ValueFieldType)

        Try
            Conn.Open()
            Result = Command.ExecuteNonQuery()

        Catch ex As Exception
            Emagine.LogError(ex)

        Finally
            Conn.Close()
        End Try

        Return Result
    End Function

    Public Shared Function Delete(ByVal intLookupID As Integer) As Boolean
        Dim Result As Boolean = False

        Result = Emagine.ExecuteSQL("DELETE FROM LookupOptions WHERE LookupID = " & intLookupID)
        If Result Then Result = Emagine.ExecuteSQL("DELETE FROM Lookups WHERE LookupID = " & intLookupID)

        Return Result
    End Function

    Public Shared Function Delete(ByVal objLookup As Lookup) As Boolean
        Dim Result As Boolean = False

        Result = Emagine.ExecuteSQL("DELETE FROM LookupOptions WHERE LookupID = " & objLookup.LookupID)
        If Result Then Result = Emagine.ExecuteSQL("DELETE FROM Lookups WHERE LookupID = " & objLookup.LookupID)

        Return Result
    End Function

    Private Shared Function PopulateObject(ByVal intLookupID As Integer) As Lookup
        Dim Lookup As New Lookup
        Dim ObjectData As DataTable = Emagine.GetDataTable("SELECT * FROM Lookups WHERE LookupID = " & intLookupID)

        If ObjectData.Rows.Count > 0 Then
            Lookup.LookupID = ObjectData.Rows(0).Item("LookupID")
            Lookup.LookupName = ObjectData.Rows(0).Item("LookupName").ToString
            Lookup.ValueFieldType = ObjectData.Rows(0).Item("ValueFieldType").ToString
            Lookup.CanEditLookup = ObjectData.Rows(0).Item("CanEditLookup")
            Lookup.CanDeleteLookup = ObjectData.Rows(0).Item("CanDeleteLookup")
            Lookup.CanAddOptions = ObjectData.Rows(0).Item("CanAddOptions")
            Lookup.CanUpdateOptions = ObjectData.Rows(0).Item("CanUpdateOptions")
            Lookup.CanDeleteOptions = ObjectData.Rows(0).Item("CanDeleteOptions")
        End If
        Return Lookup
    End Function
End Class

Public Class LookupOptions
    Public Shared Function GetOptions(ByVal intLookupID As Integer) As DataTable
        Return Emagine.GetDataTable("SELECT * FROM qryLookupOptions WHERE LookupID = " & intLookupID & " ORDER BY SortOrder")
    End Function

    Public Shared Function GetOptions(ByVal strLookupName As String) As DataTable
        Return Emagine.GetDataTable("SELECT * FROM qryLookupOptions WHERE LookupID IN (SELECT LookupID FROM Lookups WHERE LookupName = '" & strLookupName.Replace("'", "''") & "') ORDER BY SortOrder")
    End Function
End Class

Public Class LookupOption
    Public OptionID As Integer = -1
    Public LookupID As Integer = -1
    Public OptionText As String = ""
    Public OptionValue As String = ""
    Public SortOrder As Integer = 0

    Public Shared Function GetLookupOption(ByVal intOptionID As Integer) As LookupOption
        Dim LookupOption As New LookupOption
        LookupOption.OptionID = intOptionID

        Return PopulateObject(LookupOption)
    End Function

    Private Shared Function PopulateObject(ByVal objOption As LookupOption) As LookupOption
        Dim ObjectData As DataTable = Emagine.GetDataTable("SELECT * FROM LookupOptions WHERE OptionID = " & objOption.OptionID)
        If ObjectData.Rows.Count > 0 Then
            objOption.LookupID = ObjectData.Rows(0).Item("LookupID")
            objOption.OptionText = ObjectData.Rows(0).Item("OptionText")
            objOption.OptionValue = ObjectData.Rows(0).Item("OptionValue")
            objOption.SortOrder = ObjectData.Rows(0).Item("SortOrder")
        End If

        Return objOption
    End Function

    Public Shared Function Add(ByVal objLookupOption As LookupOption) As Boolean
        Dim Result As Boolean = False
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)

        Dim Command As New SqlCommand("sp_Lookups_AddLookupOption", Conn)
        Command.CommandType = CommandType.StoredProcedure
        Command.Parameters.AddWithValue("@LookupID", objLookupOption.LookupID)
        Command.Parameters.AddWithValue("@OptionText", objLookupOption.OptionText)
        Command.Parameters.AddWithValue("@OptionValue", objLookupOption.OptionValue)
        Command.Parameters.AddWithValue("@SortOrder", objLookupOption.SortOrder)

        Try
            Conn.Open()
            Result = Command.ExecuteNonQuery()

        Catch ex As Exception
            Emagine.LogError(ex)

        Finally
            Conn.Close()
        End Try

        Return Result
    End Function

    Public Shared Function Update(ByVal objLookupOption As LookupOption) As Boolean
        Dim Result As Boolean = False
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)

        Dim Command As New SqlCommand("sp_Lookups_UpdateLookupOption", Conn)
        Command.CommandType = CommandType.StoredProcedure
        Command.Parameters.AddWithValue("@OptionID", objLookupOption.OptionID)
        Command.Parameters.AddWithValue("@OptionText", objLookupOption.OptionText)
        Command.Parameters.AddWithValue("@OptionValue", objLookupOption.OptionValue)
        Command.Parameters.AddWithValue("@SortOrder", objLookupOption.SortOrder)

        Try
            Conn.Open()
            Result = Command.ExecuteNonQuery()

        Catch ex As Exception
            Emagine.LogError(ex)

        Finally
            Conn.Close()
        End Try

        Return Result
    End Function

    Public Shared Function Delete(ByVal intOptionID As Integer) As Boolean
        Return Emagine.ExecuteSQL("DELETE FROM LookupOptions WHERE OptionID = " & intOptionID)
    End Function

    Public Shared Function GetMaxSortOrder(ByVal intLookupID As Integer) As Integer
        Return Emagine.GetNumber(Emagine.GetDbValue("SELECT MAX(SortOrder) As MaxSortOrder FROM LookupOptions WHERE LookupID = " & intLookupID))
    End Function

    Public Shared Function UpdateSortOrder(ByVal intOptionID As Integer, ByVal intSortOrder As Integer) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim Command As New SqlCommand
        Dim LookupOption As LookupOption = LookupOption.GetLookupOption(intOptionID)
        Dim Result As Boolean = False
        Dim Sql As String = ""

        If LookupOption.SortOrder > intSortOrder Then
            Sql = "UPDATE LookupOptions SET SortOrder = SortOrder + 1 WHERE LookupID = " & LookupOption.LookupID & " AND SortOrder >= " & intSortOrder & " AND SortOrder < " & LookupOption.SortOrder & " AND OptionID <> " & intOptionID
        ElseIf LookupOption.SortOrder < intSortOrder Then
            Sql = "UPDATE LookupOptions SET SortOrder = SortOrder - 1 WHERE LookupID = " & LookupOption.LookupID & " AND SortOrder <= " & intSortOrder & " AND SortOrder > " & LookupOption.SortOrder & " AND OptionID <> " & intOptionID
        End If

        Try
            Command.Connection = Conn
            Conn.Open()

            If Sql.Length > 0 Then
                Command.CommandText = Sql
                Command.ExecuteNonQuery()
            End If

            If intOptionID > 0 Then
                Sql = "UPDATE LookupOptions SET SortOrder = " & intSortOrder & " WHERE OptionID = " & intOptionID
                Command.CommandText = Sql
                Command.ExecuteNonQuery()
            End If

            ResetSortOrder(LookupOption.LookupID)

            Result = True
        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            Conn.Close()
        End Try

        Return Result
    End Function

    Public Shared Function ResetSortOrder(ByVal intLookupID As Integer) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL = "SELECT OptionID, SortOrder FROM LookupOptions WHERE LookupID = " & intLookupID & " ORDER BY SortOrder"
        Dim Command As New SqlCommand(SQL, Conn)
        Dim Counter As Integer = 0
        Dim Result As Boolean = False
        Dim OptionData As DataTable = Emagine.GetDataTable(SQL)

        Try
            Conn.Open()

            For i As Integer = 0 To (OptionData.Rows.Count - 1)
                Counter += 1
                Command.CommandText = "UPDATE LookupOptions SET SortOrder = " & Counter & " WHERE OptionID = " & OptionData.Rows(i).Item("OptionID")

                Result = Command.ExecuteNonQuery()
                If Not Result Then Exit For
            Next

        Catch ex As Exception
            Emagine.LogError(ex)

        Finally
            Conn.Close()
        End Try

        Return Result
    End Function

End Class

