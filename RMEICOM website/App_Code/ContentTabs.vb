Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports Emagine

Public Class ContentTabs

    Public Shared Function GetTabs(ByVal intCategoryID As Integer) As DataTable
        Return Emagine.GetDataTable("SELECT * FROM qryContentTabs WHERE CategoryID = " & intCategoryID)
    End Function

    Public Shared Function GetMaxSortOrder(ByVal intCategoryID As Integer) As Integer
        Return Emagine.GetNumber(Emagine.GetDbValue("SELECT COUNT(*) As MaxSortOrder FROM qryContentTabs WHERE CategoryID = " & intCategoryID))
    End Function

    Public Shared Function ResetSortOrder(ByVal intCategoryID As Integer) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL = "SELECT TabID, SortOrder FROM ContentTabs WHERE CategoryID = " & intCategoryID & " ORDER BY SortOrder"
        Dim Command As New SqlCommand(SQL, Conn)
        Dim Counter As Integer = 0
        Dim Result As Boolean = False
        Dim Rs As SqlDataReader = Emagine.GetDataReader(SQL)

        Try
            Conn.Open()

            Do While Rs.Read
                Counter += 1
                Command.CommandText = "UPDATE ContentTabs SET SortOrder = " & Counter & " WHERE TabID = " & Rs("TabID")

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

Public Class ContentTab
    Inherits Resources.Resource

    Public TabID As Integer = 0
    Public CategoryID As Integer = 0
    Public Content As String = ""


    Public Shared Function GetTab(ByVal intTabID As Integer) As ContentTab
        Dim Tab As New ContentTab
        Dim TabData As DataTable = Emagine.GetDataTable("SELECT * FROM qryContentTabs WHERE TabID = " & intTabID)
        If TabData.Rows.Count > 0 Then
            Tab.TabID = TabData.Rows(0).Item("TabID")
            Tab.CategoryID = TabData.Rows(0).Item("CategoryID")
            Tab.ResourceID = TabData.Rows(0).Item("ResourceID")
            Tab.ResourceCategory = TabData.Rows(0).Item("ResourceCategory").ToString
            Tab.ResourcePageKey = TabData.Rows(0).Item("ResourcePageKey").ToString()
            Tab.ResourceKeywords = TabData.Rows(0).Item("ResourceKeywords").ToString
            Tab.ResourceName = TabData.Rows(0).Item("ResourceName").ToString
            Tab.Content = TabData.Rows(0).Item("Content").ToString
            Tab.IsEnabled = TabData.Rows(0).Item("IsEnabled")
            Tab.SortOrder = TabData.Rows(0).Item("SortOrder")
            If IsDate(TabData.Rows(0).Item("DisplayStartDate").ToString) Then Tab.DisplayStartDate = TabData.Rows(0).Item("DisplayStartDate")
            If IsDate(TabData.Rows(0).Item("DisplayEndDate").ToString) Then Tab.DisplayEndDate = TabData.Rows(0).Item("DisplayEndDate")
            Tab.CreatedBy = TabData.Rows(0).Item("CreatedBy").ToString
            Tab.CreatedDate = TabData.Rows(0).Item("CreatedDate")
            Tab.UpdatedBy = TabData.Rows(0).Item("UpdatedBy").ToString
            Tab.UpdatedDate = TabData.Rows(0).Item("UpdatedDate")
        End If

        Return Tab
    End Function

    Public Shared Function Add(ByVal objTab As ContentTab) As Integer
        Dim Result As Boolean = False
        Dim TabID As Integer = 0
        Dim SQL As String = "INSERT INTO ContentTabs (CategoryID,ResourceID) VALUES (@CategoryID,@ResourceID)"

        Dim Cmd As New SqlCommand
        Cmd.Parameters.AddWithValue("@CategoryID", objTab.CategoryID)
        Cmd.Parameters.AddWithValue("@ResourceID", objTab.ResourceID)

        Result = Emagine.ExecuteSQL(SQL, Cmd)

        If Result Then
            TabID = Emagine.GetNumber(Emagine.GetDbValue("SELECT MAX(TabID) AS MaxTabID FROM ContentTabs"))
        End If

        Return TabID
    End Function

End Class
