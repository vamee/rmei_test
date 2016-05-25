Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Namespace LB01

    Public Class LibraryItems

        Public Shared Function GetLibraryItemsByPageID(ByVal intPageID As Integer) As SqlDataReader
            Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
            Dim SQL As String = "sp_Pages01_GetLibraryItems"
            Dim Rs As SqlDataReader = Nothing
            Dim Command As New SqlCommand(SQL, Conn)

            Command.CommandType = CommandType.StoredProcedure
            Command.Parameters.AddWithValue("@PageID", intPageID)

            Try
                Conn.Open()
                Rs = Command.ExecuteReader(CommandBehavior.CloseConnection)
            Catch ex As Exception
                Emagine.LogError(ex)
            End Try

            Return Rs
        End Function

        Public Shared Function UpdateSortOrder(ByVal strValues As String) As Boolean
            Dim Result As Boolean = True
            Dim arySortOrder As Array = Split(strValues, ",")

            Try
                For i As Integer = 0 To UBound(arySortOrder)
                    If arySortOrder(i).ToString.Length > 0 Then
                        Dim aryValues As Array = Split(arySortOrder(i), "_")
                        Dim PageID As Integer = Emagine.GetNumber(aryValues(0))
                        Dim SortOrder As Integer = Emagine.GetNumber(aryValues(1))

                        Dim SQL As String = "UPDATE PageLibraryItems SET SortOrder = " & SortOrder & " WHERE PageItemID = " & PageID
                        Emagine.ExecuteSQL(SQL)
                    End If
                Next
            Catch e As SqlException
                Result = False
            End Try

            Return Result
        End Function

        Public Shared Function DisplaySiteContent(ByVal intPageID As Integer, ByVal intPadding As Integer, ByRef strHTML As String, ByVal intItemID As Integer, ByVal intLanguageID As Integer) As String
            Dim SQL As String = "SELECT PageID, ParentPageID, PageName, PageKey, SupportsLibraryItems FROM qryPages WHERE LanguageID = " & intLanguageID & " AND StatusID > 0 AND ParentPageID = " & intPageID & " ORDER BY PageSortOrder"
            Dim Rs As SqlDataReader = Emagine.GetDataReader(SQL)
            Do While Rs.Read()
                strHTML += _
                        "<tr bgcolor=""#" & IIf(Rs("ParentPageId") = 0, "F3F2F7", "FFFFFF") & """>" & vbCrLf _
                      & "   <td style=""padding-left:" & intPadding & """ class=""main"">" & vbCrLf _
                      & "       <input type=""checkbox"" name=""PageID"" value=""" & Rs("PageID") & """" & CheckSelectedPageID(intItemID, Rs("PageID")) & IIf(Rs("SupportsLibraryItems") = 0, "disabled", "") & ">&nbsp;&nbsp;" & vbCrLf
                If Rs("ParentPageID") = 0 Then
                    strHTML += "<b>" & Rs("PageName") & "</b>" & vbCrLf
                Else
                    strHTML += Rs("PageName") & vbCrLf
                End If
                strHTML += _
                         "   </td>" & vbCrLf _
                      & "</tr>" & vbCrLf
                DisplaySiteContent(Rs("PageID"), intPadding + 25, strHTML, intItemID, intLanguageID)
            Loop

            Rs.Close()

            Return strHTML
        End Function

        Public Shared Function CheckSelectedPageID(ByVal intItemID As Integer, ByVal intPageID As Integer) As String
            If Emagine.GetDbValue("SELECT Count(*) FROM PageLibraryItems WHERE ItemID = " & intItemID & " AND PageID = " & intPageID) > 0 Then
                Return "CHECKED"
            Else
                Return ""
            End If
        End Function


        Public Class LibraryItem
            Inherits Content01

            Public ItemID As Integer = -1


            Public Shared Function GetLibraryItem(ByVal intItemID As Integer) As LB01.LibraryItems.LibraryItem
                Dim LibraryItem As New LB01.LibraryItems.LibraryItem
                Dim LibraryItemData As DataTable = Emagine.GetDataTable("SELECT * FROM qryLibraryItems WHERE ItemID = " & intItemID)

                If LibraryItemData.Rows.Count > 0 Then
                    LibraryItem.ItemID = LibraryItemData.Rows(0).Item("ItemID")
                    LibraryItem.ResourceID = LibraryItemData.Rows(0).Item("ResourceID")
                    LibraryItem.ResourceName = LibraryItemData.Rows(0).Item("ResourceName")
                    LibraryItem.Content = LibraryItemData.Rows(0).Item("Content")
                    LibraryItem.DisplayStartDate = LibraryItemData.Rows(0).Item("DisplayStartDate")
                    LibraryItem.DisplayEndDate = LibraryItemData.Rows(0).Item("DisplayEndDate")
                    LibraryItem.IsEnabled = LibraryItemData.Rows(0).Item("IsEnabled")
                    LibraryItem.CreatedDate = LibraryItemData.Rows(0).Item("CreatedDate")
                    LibraryItem.CreatedBy = LibraryItemData.Rows(0).Item("CreatedBy")
                    LibraryItem.UpdatedDate = LibraryItemData.Rows(0).Item("UpdatedDate")
                    LibraryItem.UpdatedBy = LibraryItemData.Rows(0).Item("UpdatedBy")
                End If

                Return LibraryItem
            End Function

            Public Shared Function Add(ByVal objLibraryItem As LB01.LibraryItems.LibraryItem) As LB01.LibraryItems.LibraryItem
                Dim Result As Boolean = False
                Dim ResourceID As String = Emagine.GetUniqueID()
                Dim SQL As String = "INSERT INTO LibraryItems (ResourceID) VALUES (@ResourceID)"

                Dim Command As New SqlCommand
                Command.Parameters.AddWithValue("@ResourceID", ResourceID)

                Result = Emagine.ExecuteSQL(SQL, Command)
                Command.Dispose()

                If Result Then
                    objLibraryItem.ItemID = Emagine.GetNumber(Emagine.GetDbValue("SELECT MAX(ItemID) AS MaxItemID FROM LibraryItems"))

                    Dim MyContent As New Content01
                    MyContent.ResourceID = Emagine.GetUniqueID()
                    MyContent.ModuleKey = "LB01"
                    MyContent.ForeignKey = objLibraryItem.ItemID
                    MyContent.Content = objLibraryItem.Content
                    MyContent.CreatedBy = objLibraryItem.CreatedBy
                    MyContent.UpdatedBy = objLibraryItem.UpdatedBy
                    MyContent = Content01.AddContent(MyContent)

                    Dim MyResource As New Resources.Resource
                    MyResource.ResourceID = ResourceID
                    MyResource.ResourceName = objLibraryItem.ResourceName
                    MyResource.ResourceType = "LB01"
                    MyResource.DisplayStartDate = objLibraryItem.DisplayStartDate
                    MyResource.DisplayEndDate = objLibraryItem.DisplayEndDate
                    MyResource.IsEnabled = objLibraryItem.IsEnabled
                    MyResource.CreatedBy = objLibraryItem.CreatedBy
                    MyResource.UpdatedBy = objLibraryItem.UpdatedBy

                    Result = Resources.Resource.AddResource(MyResource)

                    If Not Result Then Emagine.ExecuteSQL("DELETE FROM LibraryItems WHERE ResourceID = '" & objLibraryItem.ResourceID & "'")
                End If

                Return objLibraryItem
            End Function


            Public Shared Function Update(ByVal objLibraryItem As LB01.LibraryItems.LibraryItem) As Boolean
                Dim Result As Boolean = False
                Dim Sql As String = "UPDATE Content SET Content=@Content WHERE ModuleKey=@ModuleKey AND ForeignKey=@ForeignKey"
                Dim Command As New SqlCommand

                Dim MyContent As Content01 = Content01.GetContent("LB01", objLibraryItem.ItemID)
                MyContent.Content = objLibraryItem.Content
                MyContent.UpdatedDate = Date.Now()
                MyContent.UpdatedBy = objLibraryItem.UpdatedBy

                If Content01.UpdateContent(MyContent) Then
                    Dim MyResource As Resources.Resource = Resources.Resource.GetResource(objLibraryItem.ResourceID)

                    If MyResource.ResourceID.Length > 0 Then
                        MyResource.ResourceID = objLibraryItem.ResourceID
                        MyResource.ResourceName = objLibraryItem.ResourceName
                        MyResource.DisplayStartDate = objLibraryItem.DisplayStartDate
                        MyResource.DisplayEndDate = objLibraryItem.DisplayEndDate
                        MyResource.SortOrder = objLibraryItem.SortOrder
                        MyResource.IsEnabled = objLibraryItem.IsEnabled
                        MyResource.UpdatedBy = objLibraryItem.UpdatedBy
                        MyResource.UpdatedDate = Date.Now()

                        Result = Resources.Resource.UpdateResource(MyResource)
                    Else
                        MyResource.ResourceID = objLibraryItem.ResourceID
                        MyResource.ResourceName = objLibraryItem.ResourceName
                        MyResource.ResourceType = "LB01"
                        MyResource.DisplayStartDate = objLibraryItem.DisplayStartDate
                        MyResource.DisplayEndDate = objLibraryItem.DisplayEndDate
                        MyResource.IsEnabled = objLibraryItem.IsEnabled
                        MyResource.CreatedBy = objLibraryItem.CreatedBy
                        MyResource.UpdatedBy = objLibraryItem.UpdatedBy

                        Result = Resources.Resource.AddResource(MyResource)
                    End If
                End If

                Return Result
            End Function

            Public Shared Function Delete(ByVal intItemID As Integer) As Boolean
                Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
                Dim Result As Boolean = False

                Dim Command As New SqlCommand("sp_LB01_DeleteLibraryItem", Conn)
                Command.CommandType = CommandType.StoredProcedure

                Command.Parameters.AddWithValue("@ItemId", intItemID)

                Try
                    Conn.Open()
                    Command.ExecuteNonQuery()
                    Result = True

                Catch ex As Exception
                    Emagine.LogError(ex)

                Finally
                    Conn.Close()

                End Try

                Return Result
            End Function
        End Class


    End Class

    Public Class PageLibraryItems

        Public Shared Sub ResetPageSortOrder(ByVal intPageID As Integer)
            Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
            Dim SQL As String = "sp_LB01_GetPageItems"

            Dim Command As New SqlCommand(SQL, Conn)
            Command.CommandType = CommandType.StoredProcedure
            Command.Parameters.AddWithValue("@PageID", intPageID)

            Try
                Conn.Open()
                Dim Rs As SqlDataReader = Command.ExecuteReader(CommandBehavior.CloseConnection)
                Dim SortOrder As Integer = 0
                Do While Rs.Read
                    SortOrder = SortOrder + 1
                    Emagine.ExecuteSQL("UPDATE PageLibraryItems SET SortOrder = " & SortOrder & " WHERE PageItemID = " & Rs("PageItemID"))
                Loop
                Rs.Close()
                Rs = Nothing
            Catch ex As Exception
                Emagine.LogError(ex)

            Finally
                Conn.Close()

            End Try
        End Sub

        Public Shared Sub ResetSortOrderAll()
            Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
            Dim Rs As SqlDataReader = Emagine.GetDataReader("SELECT DISTINCT PageID FROM PageLibraryItems")
            Do While Rs.Read
                Dim SQL As String = "sp_LB01_GetPageItems"

                Dim Command As New SqlCommand(SQL, Conn)
                Command.CommandType = CommandType.StoredProcedure
                Command.Parameters.AddWithValue("@PageID", Rs("PageID"))

                Try
                    Conn.Open()
                    Dim Rs2 As SqlDataReader = Command.ExecuteReader(CommandBehavior.CloseConnection)
                    Dim SortOrder As Integer = 0
                    Do While Rs2.Read
                        SortOrder = SortOrder + 1
                        Emagine.ExecuteSQL("UPDATE PageLibraryItems SET SortOrder = " & SortOrder & " WHERE PageItemID = " & Rs2("PageItemID"))
                    Loop
                    Rs2.Close()
                    Rs2 = Nothing
                Catch ex As Exception
                    Emagine.LogError(ex)

                End Try

            Loop

            Rs.Close()
            Rs = Nothing
            Conn.Close()

        End Sub

        Public Class PageLibraryItem

            Public Shared Function Insert(ByVal intPageID As Integer, ByVal intItemID As Integer) As Integer
                Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
                Dim Result As Integer = 0
                Dim Rs As SqlDataReader
                Dim SortOrder As Integer = Emagine.GetDbValue("SELECT COUNT(PageItemID) + 1 AS RecordCount FROM PageLibraryItems WHERE PageID  = " & intPageID)

                Dim Command As New SqlCommand("sp_LB01_AddPageLibraryItem", Conn)
                Command.CommandType = CommandType.StoredProcedure
                Command.Parameters.AddWithValue("@PageID", intPageID)
                Command.Parameters.AddWithValue("@ItemID", intItemID)
                Command.Parameters.AddWithValue("@SortOrder", SortOrder)

                Try
                    Conn.Open()
                    Rs = Command.ExecuteReader()
                    If Rs.Read Then Result = Rs(0)
                    Rs.Close()

                Catch ex As Exception
                    Emagine.LogError(ex)

                Finally
                    Conn.Close()
                End Try

                Return Result
            End Function

            Public Shared Function Update(ByVal intPageItemID As Integer, ByVal intItemID As Integer, Optional ByRef strErrorMessage As String = "") As Boolean
                Dim Result As Boolean = False

                Dim Sql As String = "UPDATE PageLibraryItems SET ItemID = @ItemID WHERE PageItemID = @PageItemID"

                Dim Command As New SqlCommand
                Command.Parameters.AddWithValue("@PageItemID", intPageItemID)
                Command.Parameters.AddWithValue("@ItemID", intItemID)

                Result = Emagine.ExecuteSQL(Sql, Command, strErrorMessage)

                Return Result
            End Function

            Public Shared Function Delete(ByVal intPageItemID As Integer) As Boolean
                Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
                Dim Result As Boolean = False

                Dim Command As New SqlCommand("sp_LB01_DeletePageLibraryItem", Conn)
                Command.CommandType = CommandType.StoredProcedure
                Command.Parameters.AddWithValue("@PageItemID", intPageItemID)

                Try
                    Conn.Open()
                    Command.ExecuteNonQuery()
                    Result = True

                Catch ex As Exception
                    Emagine.LogError(ex)

                Finally
                    Conn.Close()

                End Try

                Return Result
            End Function

        End Class
        
    End Class

End Namespace
