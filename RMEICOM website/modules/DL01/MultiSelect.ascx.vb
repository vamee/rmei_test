
Partial Class DL01_MultiSelect
    Inherits System.Web.UI.UserControl

    Dim _PageModuleID As Integer = 0
    Dim ResourceCategory As String = ""
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
        Dim CategoryID As Integer = Emagine.GetDbValue("SELECT ForeignValue FROM PageModules WHERE PageModuleID = " & PageModuleID)
        Dim DisplayPageID As Integer = 0
        Dim DeliveryPageID As Integer = 0
        Dim DownloadData As DataTable = Emagine.GetDataTable("SELECT TOP 1 DisplayPageID, DeliveryPageID FROM qryDownloads WHERE PageModuleID = " & PageModuleID)
        If DownloadData.Rows.Count > 0 Then
            DisplayPageID = Emagine.GetNumber(DownloadData.Rows(0).Item("DisplayPageID"))
            DeliveryPageID = Emagine.GetNumber(DownloadData.Rows(0).Item("DeliveryPageID"))
        End If
        DownloadData.Dispose()

        Dim FoundMatch As Boolean = False
        Dim BatchID As String = Resources.GetResourceID()
        Dim ResourceID As String = Resources.GetResourceID(UserID, BatchID)

        If DeliveryPageID = Session("PageID") Then
            If ResourceID.Length > 0 Then
                If InStr(ResourceID, " ") > 0 Then ResourceID = Replace(ResourceID, " ", "")
                If InStr(ResourceID, "^") > 0 Then ResourceID = Replace(ResourceID, "^", ",")

                Dim SQL As String = "SELECT * FROM qryDownloads WHERE ResourceID IN ('" & Replace(ResourceID, ",", "','") & "')"
                Dim Rs As Data.SqlClient.SqlDataReader = Emagine.GetDataReader(SQL)
                If Rs.Read Then FoundMatch = True
                Rs.Close()
                Rs = Nothing

                If InStr(ResourceID, ",") > 0 Then ResourceID = Replace(ResourceID, ",", "^")
            End If
        End If

        If FoundMatch = True Then
            DisplayLinks(ResourceID, BatchID)
        Else
            DisplayList(CategoryID)
        End If

        btnDownload.PostBackUrl = Request.RawUrl
        lblAlert.Text = Session("Alert")
        Session("Alert") = ""
    End Sub

    Sub DisplayList(ByVal intCategoryId As Integer)

        Dim Sql As String = "SELECT DISTINCT DisplayDate, ResourceName, FileName, ExternalUrl, Description, ResourcePageKey, DeliveryPageKey, FileSize, DownloadID, SortOrder, CategoryID, ResourceID, RegistrationRequired, IsEnabled, ResourceCategory, ModuleType FROM qryDownloads WHERE IsEnabled = 'True' AND (DisplayStartDate <= '1/1/1900' OR DisplayStartDate <= '" & Date.Now & "') AND (DisplayEndDate <= '1/1/1900' OR DisplayEndDate >= '" & Date.Now & "') AND CategoryID = " & intCategoryId & " AND PageModuleID = " & PageModuleID & " ORDER BY SortOrder"
        Dim DownloadData As DataTable = Emagine.GetDataTable(Sql)

        'Dim Rs As Data.SqlClient.SqlDataReader = DL01.GetDownloads(intCategoryId)
        'If Rs.HasRows Then
        If DownloadData.Rows.Count > 0 Then
            rptDL01.DataSource = DownloadData
            rptDL01.DataBind()
            'rptDL01.EnableViewState = False
            rptDL01.Visible = True
            btnDownload.Visible = True
        End If
        DownloadData.Dispose()
    End Sub

    Sub DisplayLinks(ByVal strResourceID As String, ByVal strBatchID As String)

        Dim aryResourceIDs As Array = Split(strResourceID, "^")
        Dim i As Integer
        Dim UserCanDownload As Boolean = True
        Dim FormRedirectURL As String = ""
        Dim SQL As String = ""
        Dim Rs As System.Data.SqlClient.SqlDataReader

        For i = 0 To UBound(aryResourceIDs)
            If aryResourceIDs(i).ToString.Length > 0 Then
                SQL = "SELECT FormPageID, FormPageKey, RegistrationRequired FROM qryDownloads WHERE DownloadID IN (SELECT DownloadID FROM qryDownloads WHERE ResourceID IN ('" & aryResourceIDs(i) & "'))"
                Rs = Emagine.GetDataReader(SQL)
                If Rs.Read Then
                    If Emagine.GetNumber(Rs("FormPageID")) > 0 Then    'DOES THIS RESOURCE REQUIRE FORM SUBMISSION?
                        If Not Resources.UserHasRegistered(UserID, aryResourceIDs(i)) Then    'HAS USER ALREADY REGISTERED FOR THIS RESOURCE?
                            If Rs("RegistrationRequired") Then
				FormRedirectURL = "/" & strBatchID & "/" & Rs("FormPageKey") & ".htm"    'IF NOT, STOP LOOPING AND REDIRECT TO FORMS PAGE.
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
        'Response.Write(UserCanDownload)
        'Response.End()

        If UserCanDownload Then
            If InStr(strResourceID, "^") > 0 Then strResourceID = Replace(strResourceID, "^", ",")

            SQL = "SELECT ResourceID, DisplayPageKey, ResourcePageKey, ResourceName, FileSize FROM qryDownloads WHERE PageModuleID = " & PageModuleID & " AND ResourceID IN ('" & Replace(strResourceID, ",", "','") & "')"
            Rs = Emagine.GetDataReader(SQL)
            rptDownloadLinks.DataSource = Rs
            rptDownloadLinks.DataBind()
            rptDownloadLinks.Visible = True

        Else
            '    Emagine.Users.User.DeleteTempResources(UserID)

            '    Dim aryResources As Array = strResourceID.Split("^")
            '    For j As Integer = 0 To aryResources.GetUpperBound(0)
            '        Dim DownloadName As String = Emagine.GetDbValue("SELECT ResourceName FROM qryDownloads WHERE ResourceID = '" & aryResources(j) & "'")
            '        If DownloadName.Length > 0 Then
            '            Emagine.Users.User.SetResource(UserID, aryResources(j), "Download Name:", DownloadName)
            '        End If
            '    Next

            'Dim DownloadNames As String = ""
            'SQL = "SELECT ResourceName FROM qryDownloads WHERE DeliveryPageID='" & Session("PageID") & "' AND ResourceID IN ('" & Replace(strResourceID, "^", "','") & "')"
            'Rs = Emagine.GetDataReader(SQL)
            'Do While Rs.Read
            '    DownloadNames = DownloadNames & Rs("ResourceName") & ", "
            'Loop
            'Rs.Close()
            'Rs = Nothing

            'If DownloadNames.Length > 0 Then DownloadNames = Left(DownloadNames, Len(DownloadNames) - 2)

            'Emagine.Users.User.SetResource(UserID, strResourceID, "Download Name(s):", DownloadNames)
            Response.Redirect(FormRedirectURL)
        End If

    End Sub

    Protected Sub btnDownload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDownload.Click
        Dim ResourceID As String = Request("ResourceID")
        Dim BatchID As String = Emagine.GetUniqueID()

        If Len(ResourceID) > 0 Then
            If InStr(ResourceID, " ") > 0 Then ResourceID = Replace(ResourceID, " ", "")
            If InStr(ResourceID, ",") > 0 Then ResourceID = Replace(ResourceID, ",", "^")

            Dim aryResourceIDs As Array = ResourceID.Split("^")
            Dim DeliveryPageURL As String = "/" & BatchID & "/" & Emagine.GetDbValue("SELECT DeliveryPageKey FROM qryDownloads WHERE ResourceID = '" & aryResourceIDs(0) & "'") & ".htm"
            Emagine.Users.User.DeleteTempResources(UserID)

            For j As Integer = 0 To aryResourceIDs.GetUpperBound(0)
                Dim DownloadName As String = Emagine.GetDbValue("SELECT ResourceName FROM qryDownloads WHERE ResourceID = '" & aryResourceIDs(j) & "'")
                If DownloadName.Length > 0 Then
                    Emagine.Users.User.SetResource(UserID, BatchID, aryResourceIDs(j), "Download Name:", DownloadName)
                End If
            Next

            Response.Redirect(DeliveryPageURL)

        Else
            lblAlert.Text = "Please choose at least one download.<br><br>"
        End If
    End Sub

    Protected Sub rptDownloadLinks_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptDownloadLinks.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim ResourceID As String = DataBinder.Eval(e.Item.DataItem, "ResourceID").ToString
            Dim Hyperlink As HyperLink = CType(e.Item.FindControl("hypDownload"), HyperLink)

            Hyperlink.NavigateUrl = "/" & ResourceID & "/download.htm"
        End If
    End Sub

    Protected Sub rptDL01_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptDL01.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim tdItem As HtmlTableCell = e.Item.FindControl("tdItem")
            Dim trCategory As HtmlTableRow = e.Item.FindControl("trCategory")

            If ResourceCategory <> DataBinder.Eval(e.Item.DataItem, "ResourceCategory") Then
                trCategory.Visible = True
            End If

            If e.Item.ItemType = ListItemType.Item Then
                tdItem.Attributes("class") = "table-row"
            Else
                tdItem.Attributes("class") = "table-altrow"
            End If

            ResourceCategory = DataBinder.Eval(e.Item.DataItem, "ResourceCategory")
        End If
    End Sub
End Class
