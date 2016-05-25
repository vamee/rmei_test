
Partial Class modules_DL01_SingleSelect
    Inherits System.Web.UI.UserControl

    Dim CategoryID As Integer = 0
    Dim CategoryName As String = ""
    Dim _PageModuleID As Integer = 0
    Dim UserID As String = Emagine.Users.User.GetUserID()

    Public Property PageModuleID() As Integer
        Get
            Return _PageModuleID
        End Get
        Set(ByVal value As Integer)
            _PageModuleID = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CategoryID = Emagine.GetDbValue("SELECT ForeignValue FROM PageModules WHERE PageModuleID = " & PageModuleID)

        Dim DownloadID As Integer = 0
        Dim FoundMatch As Boolean = False
        Dim DisplayPageKey As String = ""
        Dim ResourceID As String = Resources.GetResourceID()

        If ResourceID.Length > 0 Then
            If InStr(ResourceID, " ") > 0 Then ResourceID = Replace(ResourceID, " ", "")
            If InStr(ResourceID, "^") > 0 Then ResourceID = Replace(ResourceID, "^", ",")

            Dim SQL As String = "SELECT * FROM qryDownloads WHERE ResourceID IN ('" & Replace(ResourceID, ",", "','") & "') AND CategoryID = " & CategoryID
            Dim Rs As Data.SqlClient.SqlDataReader = Emagine.GetDataReader(SQL)
            If Rs.Read Then FoundMatch = True
            Rs.Close()
            Rs = Nothing

            If InStr(ResourceID, ",") > 0 Then ResourceID = Replace(ResourceID, ",", "^")
        End If

        If FoundMatch = True Then
            Me.DisplayLinks(ResourceID)
        ElseIf ResourceID.Length = 0 And FoundMatch = False Then
            lblCategoryName.Text = Emagine.GetDbValue("SELECT CategoryName FROM ModuleCategories WHERE CategoryID = " & CategoryID)
            Me.DisplayList(CategoryID, DisplayPageKey)
        End If

        lblAlert.Text = Session("Alert")
        Session("Alert") = ""
    End Sub

    Sub DisplayList(ByVal intCategoryId As Integer, ByVal strDisplayPageKey As String)
        pnlDisplayPage.Visible = True
        pnlDeliveryPage.Visible = False

        Dim Sql As String = "SELECT DISTINCT DisplayDate, ResourceName, FileName, ExternalUrl, Description, ResourcePageKey, DeliveryPageKey, FileSize, DownloadID, SortOrder, CategoryID, ResourceID, RegistrationRequired, IsEnabled, ResourceCategory, ModuleType FROM qryDownloads WHERE IsEnabled = 'True' AND (DisplayStartDate <= '1/1/1900' OR DisplayStartDate <= '" & Date.Now & "') AND (DisplayEndDate <= '1/1/1900' OR DisplayEndDate >= '" & Date.Now & "') AND CategoryID = " & intCategoryId & " AND PageModuleID = " & PageModuleID & " ORDER BY SortOrder"
        gdvDownloads.DataSource = Emagine.GetDataTable(Sql)
        gdvDownloads.DataBind()
    End Sub

    Sub DisplayLinks(ByVal strResourceID As String)
        Dim aryResourceIDs As Array = Split(strResourceID, "^")
        Dim i As Integer
        Dim UserCanDownload As Boolean = True
        Dim FormRedirectURL As String = ""
        Dim SQL As String = ""
        Dim Rs As System.Data.SqlClient.SqlDataReader
        Dim UserID As String = Emagine.Users.User.GetUserID()
        Dim FormPageID As Integer = 0
        Dim RegistrationRequired As Boolean = False

        For i = 0 To UBound(aryResourceIDs)
            If aryResourceIDs(i).ToString.Length > 0 Then
                SQL = "SELECT FormPageID, FormPageKey, RegistrationRequired FROM qryDownloads WHERE PageModuleID = " & _PageModuleID & " AND DownloadID IN (SELECT DownloadID FROM qryDownloads WHERE ResourceID IN ('" & aryResourceIDs(i) & "'))"
                
                Rs = Emagine.GetDataReader(SQL)
                If Rs.Read Then
                    FormPageID = Emagine.GetNumber(Rs("FormPageID").ToString)
                    RegistrationRequired = CBool(Rs("RegistrationRequired"))

                    If FormPageID > 0 Then    'DOES THIS RESOURCE REQUIRE FORM SUBMISSION?
                        If Not Resources.UserHasRegistered(UserID, aryResourceIDs(i)) Then    'HAS USER ALREADY REGISTERED FOR THIS RESOURCE?
                            If RegistrationRequired Then
                                FormRedirectURL = "/" & strResourceID & "/" & Rs("FormPageKey") & ".htm"    'IF NOT, STOP LOOPING AND REDIRECT TO FORMS PAGE.
                                UserCanDownload = False
                                Exit For
                            End If
                        End If
                    Else
                        Resources.Resource.Register(UserID, aryResourceIDs(i), 0)
                    End If
                End If
                Rs.Close()
            End If
        Next


        If FormPageID > 0 And RegistrationRequired = True And UserCanDownload = True Then
            If InStr(strResourceID, "^") > 0 Then strResourceID = Replace(strResourceID, "^", ",")

            pnlDisplayPage.Visible = False
            pnlDeliveryPage.Visible = True

            gdvLinks.DataSource = Emagine.GetDataReader("SELECT DISTINCT DownloadID, ResourceID, ModuleTypeID, DisplayPageKey, ResourcePageKey, ResourceName, FileSize, ExternalUrl FROM qryDownloads WHERE PageModuleID = " & PageModuleID & " AND DeliveryPageID = '" & Session("PageID") & "' AND ResourceID IN ('" & Replace(strResourceID, ",", "','") & "') AND ((Filename <> '' AND FileName IS NOT NULL) OR (ExternalUrl <> '' AND ExternalUrl IS NOT NULL))")
            gdvLinks.DataBind()

        ElseIf UserCanDownload = True And aryResourceIDs.GetUpperBound(0) = 0 Then
            Dim FileName As String = Emagine.GetDbValue("SELECT FileName FROM qryDownloads WHERE ResourceID = '" & aryResourceIDs(0) & "'")
            If IO.File.Exists(Server.MapPath(FileName)) Then
                Me.UpdateClickCount(aryResourceIDs(0))
                Me.UpdateDownloadCount(aryResourceIDs(0))
                Me.DownloadFile(FileName)
            Else
                lblAlert.Text = "An error occurred while attempting to retrieve your file."
            End If

        ElseIf UserCanDownload Then
            Response.Write("3")
            'If UserCanDownload Then
            If InStr(strResourceID, "^") > 0 Then strResourceID = Replace(strResourceID, "^", ",")

            pnlDisplayPage.Visible = False
            pnlDeliveryPage.Visible = True

            gdvLinks.DataSource = Emagine.GetDataReader("SELECT DISTINCT DownloadID, ResourceID, ModuleTypeID, DisplayPageKey, ResourcePageKey, ResourceName, FileSize, ExternalUrl FROM qryDownloads WHERE PageModuleID = " & PageModuleID & " AND DeliveryPageID = '" & Session("PageID") & "' AND ResourceID IN ('" & Replace(strResourceID, ",", "','") & "') AND ((Filename <> '' AND FileName IS NOT NULL) OR (ExternalUrl <> '' AND ExternalUrl IS NOT NULL))")
            gdvLinks.DataBind()

        Else
            Dim DownloadNames As String = ""
            SQL = "SELECT ResourceName FROM qryDownloads WHERE ResourceID IN ('" & Replace(strResourceID, ",", "','") & "')"
            Rs = Emagine.GetDataReader(SQL)
            Do While Rs.Read
                DownloadNames = DownloadNames & Rs("ResourceName") & ", "
            Loop
            Rs.Close()
            Rs = Nothing

            If DownloadNames.Length > 0 Then DownloadNames = Left(DownloadNames, Len(DownloadNames) - 2)

            Emagine.Users.User.DeleteResources(UserID)
            Emagine.Users.User.SetResource(UserID, strResourceID, "Download Name(s):", DownloadNames)
            Response.Redirect(FormRedirectURL)
        End If

    End Sub

    Protected Sub gdvDownloads_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvDownloads.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblDate As Label = e.Row.FindControl("lblDate")
            Dim btnFileName As LinkButton = e.Row.FindControl("btnFileName")
            Dim hypFileName As HyperLink = e.Row.FindControl("hypFileName")
            Dim lblFileSize As Label = e.Row.FindControl("lblFileSize")
            Dim lblDescription As Label = e.Row.FindControl("lblDescription")

            Dim DownloadID As Integer = DataBinder.Eval(e.Row.DataItem, "DownloadID")
            Dim DisplayDate As Date = DataBinder.Eval(e.Row.DataItem, "DisplayDate").ToString
            Dim ResourceName As String = DataBinder.Eval(e.Row.DataItem, "ResourceName").ToString
            Dim FileName As String = e.Row.DataItem("FileName").ToString
            Dim FileSize As Integer = DataBinder.Eval(e.Row.DataItem, "FileSize")
            Dim ResourceID As String = DataBinder.Eval(e.Row.DataItem, "ResourceID").ToString
            Dim Description As String = DataBinder.Eval(e.Row.DataItem, "Description").ToString
            Dim ExternalUrl As String = DataBinder.Eval(e.Row.DataItem, "ExternalUrl").ToString
            Dim RegistrationRequired As Boolean = DataBinder.Eval(e.Row.DataItem, "RegistrationRequired")

            Dim DeliveryPageKey As String = Emagine.GetDbValue("SELECT DeliveryPageKey FROM qryDownloads WHERE DownloadID = " & DownloadID)

            If lblDate IsNot Nothing Then lblDate.Text = String.Format("{0:d}", DisplayDate)
            If lblDescription IsNot Nothing Then lblDescription.Text = Description
            If lblFileSize IsNot Nothing And FileSize > 0 Then lblFileSize.Text = "(" & Emagine.FormatFileSize(FileSize) & ")"
            If hypFileName IsNot Nothing Then
                hypFileName.Text = ResourceName
                If RegistrationRequired = False And ExternalUrl.Length > 0 Then
                    hypFileName.NavigateUrl = ExternalUrl
                    hypFileName.Target = "_blank"
                ElseIf RegistrationRequired = False Or Resources.UserHasRegistered(UserID, ResourceID) Then
                    'hypFileName.NavigateUrl = "/" & ResourceID & "/" & DeliveryPageKey & ".htm"
                    hypFileName.NavigateUrl = "/" & ResourceID & "/download.htm"
                    hypFileName.Target = "_blank"

                Else
                    hypFileName.NavigateUrl = "/" & ResourceID & "/" & DeliveryPageKey & ".htm"
                End If

            End If
            If btnFileName IsNot Nothing Then
                btnFileName.Text = ResourceName
                btnFileName.PostBackUrl = Request.RawUrl
                btnFileName.CommandArgument = ResourceID
            End If

        End If
    End Sub

    Sub btnFileName_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Button As LinkButton = sender
        Dim ResourceID As String = Button.CommandArgument



        'Me.DisplayLinks(ResourceID)
    End Sub

    Protected Sub gdvLinks_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvLinks.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblFileSize As Label = e.Row.FindControl("lblFileSize")
            Dim hypDownload As HyperLink = e.Row.FindControl("hypDownload")

            Dim DownloadID As Integer = DataBinder.Eval(e.Row.DataItem, "DownloadID")
            Dim ModuleTypeID As Integer = DataBinder.Eval(e.Row.DataItem, "ModuleTypeID")
            Dim ResourceID As String = DataBinder.Eval(e.Row.DataItem, "ResourceID").ToString
            Dim FileSize As Integer = DataBinder.Eval(e.Row.DataItem, "FileSize")
            Dim ExternalUrl As String = DataBinder.Eval(e.Row.DataItem, "ExternalUrl").ToString

            If lblFileSize IsNot Nothing Then
                If FileSize > 0 Then
                    lblFileSize.Text = Emagine.FormatFileSize(FileSize)
                Else
                    lblFileSize.Text = "N/A"
                End If
            End If

            If hypDownload IsNot Nothing Then
                Select Case ModuleTypeID
                    Case 11
                        hypDownload.NavigateUrl = "/" & ResourceID & "/download.htm"
                    Case 12
                        hypDownload.NavigateUrl = ExternalUrl
                End Select
            End If


        End If
    End Sub

    Sub btnDownload_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    'Sub DownloadFile(ByVal DownloadID)

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
            Dim MimeType As String = Emagine.GetMimeType(Emagine.GetFileExtension(FileName))

            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.AppendHeader("Content-Length", FileSize.ToString())
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" & FileName)
            If MimeType.Length > 0 Then
                HttpContext.Current.Response.ContentType = MimeType
            Else
                HttpContext.Current.Response.ContentType = "application/octet-stream"
            End If
            'HttpContext.Current.Response.WriteFile(AbsolutePath)
            HttpContext.Current.Response.TransmitFile(AbsolutePath)
            HttpContext.Current.Response.Flush()

            'Emagine.ExecuteSQL("UPDATE Resources SET CreatedBy = 'File Size: " & FileSize & "'")

        Catch ex As Exception
            Emagine.LogError(ex)
            'Dim Referer As String = Request.ServerVariables("HTTP_REFERER")
            'Dim ErrorMessage As String = "We're sorry.<br>There was a problem downloading your file.<br>Please try again later.<br>"
            'If Referer.Length > 0 Then
            '    'Session("Alert") = ErrorMessage
            '    Response.Redirect(Referer)
            'Else
            '    lblAlert.Text = ErrorMessage
            'End If
        End Try

    End Sub
End Class
