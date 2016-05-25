
Partial Class modules_Links01_GetResource
    Inherits System.Web.UI.Page



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim UserID As String = Emagine.Users.User.GetUserID()
        Dim ResourceID As String = Trim(Request("ResourceID"))
        Dim LinkType As String = ""
        Dim LinkURL As String = ""
        Dim ResourcePageKey As String = ""
        Dim FormRedirect As String = ""
        Dim SQL As String = "SELECT LinkType, LinkURL, ResourcePageKey, FormPageID, FormPageKey FROM qryContentLinks WHERE ResourceID = '" & ResourceID & "'"

        Dim Rs As Data.SqlClient.SqlDataReader = Emagine.GetDataReader(SQL)

        If Rs.Read Then
            LinkType = Rs("LinkType")
            LinkURL = Rs("LinkURL")
            ResourcePageKey = Rs("ResourcePageKey")

            If Emagine.GetNumber(Rs("FormPageID").ToString) > 0 Then
                FormRedirect = "/" & Rs("ResourcePageKey").ToString & "/" & Rs("FormPageKey").ToString & ".htm"
            End If
        End If

        Rs.Close()

        If FormRedirect.Length > 0 And UserHasRegistered(ResourcePageKey) = False Then
            Emagine.Users.User.DeleteResources(UserID)
            Emagine.Users.User.SetResource(UserID, ResourceID, LinkURL, LinkURL)
            UpdateClickCount(ResourceID)
            Response.Redirect(FormRedirect)

        ElseIf LinkURL.Length > 0 Then
            UpdateClickCount(ResourceID)
            UpdateDownloadCount(ResourceID)

            If LinkType = "FILE" Then
                DownloadFile(LinkURL)
            Else
                Response.Redirect(LinkURL)
            End If
        Else
            Response.Redirect("/")
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
        Dim File As New IO.FileInfo(AbsolutePath)
        Dim FileName As String = File.Name
        Dim FileSize As Long = File.Length

        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" & FileName)
        HttpContext.Current.Response.AppendHeader("Content-Length", FileSize.ToString())
        HttpContext.Current.Response.ContentType = "application/octet-stream"
        HttpContext.Current.Response.WriteFile(AbsolutePath)
        HttpContext.Current.Response.Flush()
        HttpContext.Current.Response.End()

    End Sub

    Function UserHasRegistered(ByVal strPageKey As String) As Boolean
        Dim Result As Boolean = False
        Dim Cookie As HttpCookie = Request.Cookies("Resources")

        If Not Cookie Is Nothing Then
            If IsNumeric(Cookie(strPageKey)) Then Result = True
        End If
        Return Result
    End Function
    
End Class
