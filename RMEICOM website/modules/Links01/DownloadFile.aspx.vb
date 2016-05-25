Imports System.Data
Imports System.Data.SqlClient

Partial Class modules_Pages01_DownloadFile
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim ResourceID As String = Trim(Request("ResourceID"))
        If Len(ResourceID) = 0 Then Emagine.GoBack()

        Dim UserID As String = Emagine.Users.User.GetUserID()
        Dim FileName As String = ""
        Dim FormPageKey As String = ""
        Dim ResourceExists As Boolean = False
        Dim SQL As String = "SELECT LinkURL, FormPageKey FROM qryContentLinks WHERE LinkType = 'FILE' AND ResourceID = '" & ResourceID & "'"

        Dim Rs As SqlDataReader = Emagine.GetDataReader(SQL)
        If Rs.Read Then
            FileName = Rs("LinkURL").ToString()
            FormPageKey = Rs("FormPageKey").ToString()
            If FileName.Length > 0 Then ResourceExists = True
        End If

        Rs.Close()
        Rs = Nothing

        If ResourceExists Then

            Me.UpdateClickCount(ResourceID)

            If Resources.UserHasRegistered(UserID, ResourceID) Or FormPageKey.Length = 0 Or String.IsNullOrEmpty(FormPageKey) Then

                If FileName.Length > 0 Then
                    Me.UpdateDownloadCount(ResourceID)
                    Me.DownloadFile(FileName)
                Else
                    Emagine.GoBack()
                End If

            Else
                Response.Redirect("/" & ResourceID & "/" & FormPageKey & ".htm")
            End If
        Else
            Emagine.GoBack()
        End If

    End Sub

    Sub UpdateDownloadCount(ByVal strResourceID As String)
        Dim SQL As String = "UPDATE Resources SET DownloadCount = DownloadCount + 1, LastDownloadDate = '" & Now() & "' WHERE ResourceID = '" & strResourceID & "'"
        Emagine.ExecuteSQL(SQL)
    End Sub

    Sub UpdateClickCount(ByVal strResourceID As String)
        Dim SQL As String = "UPDATE Resources SET ClickCount = ClickCount + 1, LastClickDate = '" & Now() & "' WHERE ResourceID = '" & strResourceID & "'"
        Emagine.ExecuteSQL(SQL)
    End Sub

    Sub DownloadFile(ByVal strFileURL As String)
        Dim VirtualPath As String = strFileURL
        Dim AbsolutePath As String = Server.MapPath(VirtualPath)

        Try
            Dim File As New IO.FileInfo(AbsolutePath)
            Dim FileName As String = File.Name
            Dim FileSize As Long = File.Length

            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" & FileName)
            HttpContext.Current.Response.AppendHeader("Content-Length", FileSize.ToString())
            HttpContext.Current.Response.ContentType = "application/octet-stream"
            HttpContext.Current.Response.WriteFile(AbsolutePath)
            HttpContext.Current.Response.Flush()

        Catch ex As Exception
            Emagine.LogError(ex)
            Dim Referer As String = Request.ServerVariables("HTTP_REFERER")
            Dim ErrorMessage As String = "We're sorry.<br>There was a problem downloading your file.<br>Please try again later.<br>"
            If Referer.Length > 0 Then
                Session("Alert") = ErrorMessage
                Response.Redirect(Referer)
            Else
                lblAlert.Text = ErrorMessage
            End If
        End Try

    End Sub

End Class
