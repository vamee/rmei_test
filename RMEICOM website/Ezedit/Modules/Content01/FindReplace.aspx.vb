Imports System.Data
Imports System.Data.SqlClient

Partial Class Ezedit_Modules_Content01_FindReplace
    Inherits System.Web.UI.Page

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim ErrorCount As Integer = 0
        Dim ContentData As DataTable = Emagine.GetDataTable("SELECT ContentID, Content FROM Content WHERE Content LIKE '%" & txtFind.Text & "%'")
        For i As Integer = 0 To (ContentData.Rows.Count - 1)
            If Not Me.UpdateContent(ContentData.Rows(i).Item("ContentID"), ContentData.Rows(i).Item("Content")) Then
                ErrorCount += 1
            End If
        Next

        lblalert.Text = ContentData.Rows.Count & " record(s) were found matching '" & txtFind.Text & "'<br />"
        lblalert.Text = lblalert.Text & ErrorCount & " error(s) occured while updating these records."

    End Sub

    Function UpdateContent(ByVal intContentID As Integer, ByVal strContent As String) As Boolean
        Dim Result As Boolean = False
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim Sql As String = "UPDATE Content SET Content = @Content WHERE ContentID = @ContentID"

        Dim Command As New SqlCommand(Sql, Conn)
        Command.CommandType = CommandType.Text
        Command.Parameters.AddWithValue("@Content", strContent.Replace(txtFind.Text, txtReplace.Text))
        Command.Parameters.AddWithValue("@ContentID", intContentID)

        Try
            Conn.Open()
            Result = Command.ExecuteNonQuery()

        Catch ex As Exception
            ex.HelpLink = "Calling Page: " & HttpContext.Current.Request.ServerVariables("SCRIPT_NAME") & vbCrLf & "SQL: " & Sql
            Emagine.LogError(ex)

        Finally
            Conn.Close()
        End Try

        Return Result
    End Function
End Class
