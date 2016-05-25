Imports Microsoft.VisualBasic

Public Class UrlRewriter
    Implements System.Web.IHttpModule

    Dim WithEvents _application As HttpApplication = Nothing
    Dim PageKey As String = ""
    Public PageID As Integer = 0
    Dim PageInfo As New Pages01

    Public Overridable Sub Init(ByVal context As HttpApplication) Implements IHttpModule.Init
        _application = context
    End Sub

    Public Overridable Sub Dispose() Implements IHttpModule.Dispose

    End Sub

    Public Sub context_BeginRequest(ByVal sender As Object, ByVal e As EventArgs) Handles _application.BeginRequest
        Dim RequestUrl As String = _application.Context.Request.Path.ToString
        Dim QueryString As String = _application.Context.Request.QueryString.ToString
        Dim FileExtension As String = Emagine.GetFileExtension(RequestUrl).ToLower

        Dim DataTable As DataTable = Emagine.GetDataTable("SELECT MimeType, IsProtected FROM MimeTypes WHERE FileExtension = '" & FileExtension & "'")

        If DataTable.Rows.Count > 0 Then
            If DataTable.Rows(0).Item("IsProtected") Then
                Me.ProcessFile(RequestUrl, DataTable.Rows(0).Item("MimeType"))
            Else
                If Not System.IO.File.Exists(_application.Context.Server.MapPath(RequestUrl)) Then
                    Me.ProcessPage(RequestUrl, QueryString)
                End If
            End If
        Else
                Me.ProcessPage(RequestUrl, QueryString)
        End If
    End Sub

    Public Sub ProcessPage(ByVal strRequestUrl As String, ByVal strQueryString As String)
        Dim Redirect As EmagineRedirect = Me.GetRedirect(strRequestUrl, strQueryString)
        '_application.Context.Response.Write("<br><br>" & Redirect.RedirectUrl & "<br><br>")
        '_application.Context.Response.End()

        If Redirect.RedirectUrl.Length > 0 Then
            If Redirect.RedirectType = "URL" Then
                _application.Context.Response.Status = "301 Moved Permanently"
                _application.Context.Response.AddHeader("Location", Redirect.RedirectUrl)

            ElseIf Redirect.RedirectType = "CODE" Then
                '_application.Context.Response.Write(Redirect.RedirectUrl)
                _application.Context.RewritePath("~" & Redirect.RedirectUrl)
            End If

        ElseIf Redirect.RedirectType = "IGNORE" Then

        Else
            _application.Context.Response.Redirect("/")
            '_application.Context.Response.End()
        End If
    End Sub


    Public Sub ProcessFile(ByVal strRequestUrl As String, ByVal strMimeType As String)
        Dim VirtualPath As String = strRequestUrl
        Dim AbsolutePath As String = _application.Context.Server.MapPath(VirtualPath)

        Try
            Dim File As New IO.FileInfo(AbsolutePath)
            Dim FileName As String = File.Name
            Dim FileSize As Long = File.Length
            Dim MimeType As String = Emagine.GetMimeType(Emagine.GetFileExtension(FileName))

            _application.Context.Response.Clear()
            _application.Context.Response.AppendHeader("Content-Length", FileSize.ToString())
            If MimeType.Length > 0 Then
                _application.Context.Response.AppendHeader("Content-Disposition", "attachment; filename=" & FileName)
                _application.Context.Response.ContentType = MimeType
            Else
                _application.Context.Response.AppendHeader("Content-Disposition", "attachment; filename=" & FileName)
                _application.Context.Response.ContentType = "application/octet-stream"
            End If

            _application.Context.Response.TransmitFile(AbsolutePath)
            _application.Context.Response.Flush()

        Catch ex As Exception
            Emagine.LogError(ex)
            Dim Referer As String = _application.Context.Request.ServerVariables("HTTP_REFERER")
            Dim ErrorMessage As String = "We're sorry.<br>There was a problem downloading your file.<br>Please try again later.<br>"
            If Referer.Length > 0 Then
                _application.Context.Session("Alert") = ErrorMessage
                _application.Context.Response.Redirect(Referer)
            End If
        End Try
        _application.Context.Response.End()
    End Sub

    Public Function GetRedirect(ByVal strRequestUrl As String, ByVal strQueryString As String) As EmagineRedirect
        Dim Result As New EmagineRedirect

        Dim DataTable As DataTable = Emagine.GetDataTable("sp_Redirects_GetAllRedirects")

        If strQueryString.Length > 0 Then
            For i As Integer = 0 To DataTable.Rows.Count - 1
                Dim RegExUrl As String = DataTable.Rows(i).Item("RequestedUrl").ToString.ToLower

                Dim RegEx As Regex = New Regex(RegExUrl)
                Dim Match As Match = RegEx.Match(strRequestUrl.ToLower & "?" & strQueryString.ToLower)

                '_application.Context.Response.Write(DataTable.Rows(i).Item("RedirectId").ToString & " - " & strRequestUrl.ToLower & "?" & strQueryString.ToLower & " - " & RegExUrl & " - " & Match.Success & "<br>")

                If Match.Success Then
                    
                    Dim RedirectUrl As String = RegEx.Replace(strRequestUrl.ToLower & "?" & strQueryString.ToLower, DataTable.Rows(i).Item("RedirectUrl").ToString.ToLower)
                    '_application.Context.Response.Write(RedirectUrl)

                    If RedirectUrl.IndexOf("?") > -1 Then
                        If strQueryString.Length > 0 Then RedirectUrl = RedirectUrl & "&" & strQueryString
                    Else
                        If strQueryString.Length > 0 Then RedirectUrl = RedirectUrl & "?" & strQueryString
                    End If

                    

                    Result.RedirectType = DataTable.Rows(i).Item("RedirectType").ToString.ToUpper
                    Result.RequestedUrl = DataTable.Rows(i).Item("RequestedUrl").ToString
                    Result.RedirectUrl = RedirectUrl
                    Result.CreatedDate = DataTable.Rows(i).Item("CreatedDate").ToString
                    Result.CreatedBy = DataTable.Rows(i).Item("CreatedBy").ToString
                    Result.UpdatedDate = DataTable.Rows(i).Item("UpdatedDate").ToString
                    Result.UpdatedBy = DataTable.Rows(i).Item("UpdatedBy").ToString
                    Exit For
                End If
            Next
        End If

        If Result.RedirectUrl.Length = 0 Then
            For i As Integer = 0 To DataTable.Rows.Count - 1
                Dim RegExUrl As String = DataTable.Rows(i).Item("RequestedUrl").ToString.ToLower

                Dim RegEx As Regex = New Regex(RegExUrl)
                Dim Match As Match = RegEx.Match(strRequestUrl.ToLower)

                '_application.Context.Response.Write(DataTable.Rows(i).Item("RedirectId").ToString & " - " & strRequestUrl.ToLower & " - " & RegExUrl & " - " & Match.Success & "<br>")

                If Match.Success Then
                    '_application.Context.Response.Write(RegExUrl)
                    '_application.Context.Response.End()
                    Dim RedirectUrl As String = RegEx.Replace(strRequestUrl.ToLower, DataTable.Rows(i).Item("RedirectUrl").ToString.ToLower)
                    If RedirectUrl.IndexOf("?") Then
                        If strQueryString.Length > 0 Then RedirectUrl = RedirectUrl & "&" & strQueryString
                    Else
                        If strQueryString.Length > 0 Then RedirectUrl = RedirectUrl & "?" & strQueryString
                    End If

                    Result.RedirectType = DataTable.Rows(i).Item("RedirectType").ToString.ToUpper
                    Result.RequestedUrl = DataTable.Rows(i).Item("RequestedUrl").ToString
                    Result.RedirectUrl = RedirectUrl
                    Result.CreatedDate = DataTable.Rows(i).Item("CreatedDate").ToString
                    Result.CreatedBy = DataTable.Rows(i).Item("CreatedBy").ToString
                    Result.UpdatedDate = DataTable.Rows(i).Item("UpdatedDate").ToString
                    Result.UpdatedBy = DataTable.Rows(i).Item("UpdatedBy").ToString
                    Exit For
                End If
            Next
        End If
        '_application.Context.Response.End()
        Return Result
    End Function

End Class

Public Class EmagineRedirect
    Public RedirectType As String = ""
    Public RequestedUrl As String = ""
    Public RedirectUrl As String = ""
    Public CreatedDate As Date = "1/1/1900"
    Public CreatedBy As String = ""
    Public UpdatedDate As Date = "1/1/1900"
    Public UpdatedBy As String = ""
End Class















Public Class PageHandler
    Implements IHttpHandler


    Public ReadOnly Property IsReusable() As Boolean Implements System.Web.IHttpHandler.IsReusable
        Get
            Return True
        End Get
    End Property

    Public Sub ProcessRequest(ByVal context As System.Web.HttpContext) Implements System.Web.IHttpHandler.ProcessRequest
        'context.Response.Write("<html><body><h3>Here is your web page, Jerky...</h3></body></html>")
        context.Server.Transfer("/Modules/Pages01/GetPage.aspx")
    End Sub
End Class


Public Class DownloadHandler
    Implements IHttpHandler


    Public ReadOnly Property IsReusable() As Boolean Implements System.Web.IHttpHandler.IsReusable
        Get
            Return True
        End Get
    End Property

    Public Sub ProcessRequest(ByVal context As System.Web.HttpContext) Implements System.Web.IHttpHandler.ProcessRequest
        'context.Response.Write("<html><body><h3>I see you...</h3></body></html>")
    End Sub
End Class