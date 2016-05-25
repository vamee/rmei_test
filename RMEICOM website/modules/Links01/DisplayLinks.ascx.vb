
Partial Class UserControls_ContentLinks
    Inherits System.Web.UI.UserControl

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
        Dim ResourceID As String = Resources.GetResourceID()
        Dim FoundMatch As Boolean = False

        If ResourceID.Length > 0 Then
            If InStr(ResourceID, " ") > 0 Then ResourceID = Replace(ResourceID, " ", "")
            If InStr(ResourceID, "^") > 0 Then ResourceID = Replace(ResourceID, "^", ",")

            Dim SQL As String = "SELECT * FROM qryContentLinks WHERE PageModuleID = " & _PageModuleID & " AND ResourceID IN ('" & Replace(ResourceID, ",", "','") & "')"
            Dim Rs As Data.SqlClient.SqlDataReader = Emagine.GetDataReader(SQL)
            If Rs.Read Then FoundMatch = True
            Rs.Close()
            Rs = Nothing

            If InStr(ResourceID, ",") > 0 Then ResourceID = Replace(ResourceID, ",", "^")
        End If

        If FoundMatch = True Then
            DisplayLinks(ResourceID)
        Else
            'lblAlert.Text = "The document you requested is unavailable."
        End If
    End Sub

    Sub DisplayLinks(ByVal strResourceID As String)
        Dim aryResourceIDs As Array = Split(strResourceID, "^")
        Dim i As Integer
        Dim UserCanDownload As Boolean = True
        Dim FormRedirectURL As String = ""
        Dim SQL As String = ""
        Dim Rs As System.Data.SqlClient.SqlDataReader

        For i = 0 To UBound(aryResourceIDs)
            If aryResourceIDs(i).ToString.Length > 0 Then
                SQL = "SELECT FormPageID, FormPageKey FROM qryContentLinks WHERE LinkID IN (SELECT LinkID FROM qryContentLinks WHERE ResourceID IN ('" & aryResourceIDs(i) & "'))"
                Rs = Emagine.GetDataReader(SQL)
                If Rs.Read Then
                    If Emagine.GetNumber(Rs("FormPageID")) > 0 Then    'DOES THIS RESOURCE REQUIRE FORM SUBMISSION?
                        If Not Resources.UserHasRegistered(UserID, aryResourceIDs(i)) Then    'HAS USER ALREADY REGISTERED FOR THIS RESOURCE?
                            FormRedirectURL = "/" & strResourceID & "/" & Rs("FormPageKey") & ".htm"    'IF NOT, STOP LOOPING AND REDIRECT TO FORMS PAGE.
                            UserCanDownload = False
                            Exit For
                        End If
                    Else
                        Resources.Resource.Register(UserID, aryResourceIDs(i), 0)
                    End If
                End If
                Rs.Close()
            End If
        Next

        If UserCanDownload Then

            Dim aryResources As Array = strResourceID.Split

            If UBound(aryResources) = 0 And GetLinkType(aryResources(0)) = "LINK" Then
                SQL = "SELECT LinkURL FROM qryContentLinks WHERE ResourceID = '" & aryResources(0) & "'"
                Dim LinkURL As String = Emagine.GetDbValue(SQL)
                If LinkURL.Length > 0 Then Response.Redirect(LinkURL)
            Else
                If InStr(strResourceID, "^") > 0 Then strResourceID = Replace(strResourceID, "^", ",")

                SQL = "SELECT ResourceID, DisplayPageKey, ResourcePageKey, LinkURL FROM qryContentLinks WHERE ResourceID IN ('" & Replace(strResourceID, ",", "','") & "')"
                Rs = Emagine.GetDataReader(SQL)
                rptDownloadLinks.DataSource = Rs
                rptDownloadLinks.DataBind()
                rptDownloadLinks.Visible = True
            End If

        Else
            Emagine.Users.User.DeleteTempResources(UserID)

            For j As Integer = 0 To aryResourceIDs.GetUpperBound(0)
                Dim DownloadName As String = Emagine.GetDbValue("SELECT LinkURL FROM qryContentLinks WHERE ResourceID = '" & aryResourceIDs(j) & "'")
                If DownloadName.Length > 0 Then
                    Emagine.Users.User.SetResource(UserID, strResourceID, "Download Name(s):", Emagine.FormatFileName(DownloadName))
                End If
            Next

            'Dim DownloadNames As String = ""
            'SQL = "SELECT LinkURL FROM qryContentLinks WHERE ResourceID IN ('" & Replace(strResourceID, ",", "','") & "')"
            'Rs = Emagine.GetDataReader(SQL)
            'Do While Rs.Read
            '    DownloadNames = DownloadNames & Emagine.FormatFileName(Rs("LinkURL")) & ", "
            'Loop
            'Rs.Close()
            'Rs = Nothing

            'If DownloadNames.Length > 0 Then DownloadNames = Left(DownloadNames, Len(DownloadNames) - 2)

            'Emagine.Users.User.SetResource(UserID, strResourceID, "Download Name(s):", DownloadNames)
            Response.Redirect(FormRedirectURL)
        End If
    End Sub

    Protected Sub rptDownloadLinks_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptDownloadLinks.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim ResourceID As String = DataBinder.Eval(e.Item.DataItem, "ResourceID").ToString
            Dim VirtualPath As String = DataBinder.Eval(e.Item.DataItem, "LinkURL").ToString
            Dim Hyperlink As HyperLink = CType(e.Item.FindControl("hypDownload"), HyperLink)
            Dim FileSizeLabel As Label = CType(e.Item.FindControl("lblFileSize"), Label)

            Dim AbsolutePath As String = Server.MapPath(VirtualPath)

            Try
                Dim File As New IO.FileInfo(AbsolutePath)
                FileSizeLabel.Text = Emagine.FormatFileSize(File.Length)
            Catch ex As Exception
                Emagine.LogError(ex)
            End Try

            Hyperlink.NavigateUrl = "/" & ResourceID & "/ContentDownload.htm"
        End If
    End Sub

    Function GetLinkType(ByVal strResourceID As String) As String
        Dim SQL As String = "SELECT LinkType FROM ContentLinks WHERE ResourceID = '" & strResourceID & "'"
        Return Emagine.GetDbValue(SQL)
    End Function
End Class
