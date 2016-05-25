
Partial Class Events01_CurrentWebcasts_1
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
        Dim CategoryID As Integer = Emagine.GetDbValue("SELECT ForeignValue FROM PageModules WHERE PageModuleID = " & PageModuleID)
        Dim ResourceID As String = Resources.GetResourceID()
        Dim EventID As Integer = 0
        Dim EventDateID As Integer = Emagine.GetNumber(Request("DateID"))
        Dim DisplayPageKey As String = ""

        If Len(ResourceID) > 0 Then
            If Len(ResourceID) > 0 Then
                Dim SQL As String = "SELECT * FROM qryEvents WHERE ResourceID = '" & ResourceID & "'"
                Dim Rs As Data.SqlClient.SqlDataReader = Emagine.GetDataReader(SQL)
                If Rs.Read Then
                    EventID = Emagine.GetNumber(Rs("EventID"))
                    DisplayPageKey = Rs("DisplayPageKey").ToString()
                End If
                Rs.Close()
                Rs = Nothing
            End If
        End If

        If EventID > 0 And Resources.UserHasRegistered(UserID, ResourceID) Then
            DisplayEventDownload(EventID)
        ElseIf EventID > 0 Then
            RegisterForEvent(ResourceID, EventID)
        Else
            DisplayEventList(CategoryID, DisplayPageKey)
        End If
    End Sub

    Sub DisplayEventList(ByVal intCategoryID As Integer, ByVal strDisplayPageKey As String)
        Dim Rs As Data.SqlClient.SqlDataReader = EventDates.GetEventDatesByCategory(intCategoryID)
        If Rs.HasRows Then
            rptEvents.DataSource = Rs
            rptEvents.DataBind()
            rptEvents.EnableViewState = False
        Else
            rptEvents.Visible = False
        End If
    End Sub

    Sub DisplayEventDownload(ByVal intEventID As Integer)
        Dim SQL As String = "SELECT ArchiveURL FROM Events WHERE EventID = " & intEventID
        Dim ArchiveURL As String = Emagine.GetDbValue(SQL)
        If ArchiveURL.Length > 0 Then
            Dim FileSize As String = Emagine.FormatFileSize(Emagine.GetFileSize(Server.MapPath(ArchiveURL)))
            lblEventInfo.Text = "<a href='" & ArchiveURL & "' class='main'>" & Emagine.FormatFileName(ArchiveURL) & "</a> <i>(" & FileSize & ")</i>"
        End If

    End Sub

    Sub RegisterForEvent(ByVal strResourceID As String, ByVal intEventID As Integer)
        Dim FormPageKey As String = ""
        Dim SQL As String = "SELECT * FROM qryEvents WHERE EventID = " & intEventID
        Dim Rs As System.Data.SqlClient.SqlDataReader = Emagine.GetDataReader(SQL)

        If Rs.Read Then
            FormPageKey = Rs("FormPageKey").ToString()

            If FormPageKey.Length > 0 Then
                Emagine.Users.User.SetResource(UserID, strResourceID, "Webcast Name:", Rs("ResourceName").ToString())
                Emagine.Users.User.SetResource(UserID, strResourceID, "Webcast File:", Emagine.FormatFileName(Rs("ArchiveURL")))
            End If
        End If

        If FormPageKey.Length > 0 Then
            Response.Redirect("/" & strResourceID & "/" & FormPageKey & ".htm")
        Else
            'lblAlert.Text = "We're sorry. An error has occurred while processing your request.<br>Please try again later."
            DisplayEventDownload(intEventID)
        End If

    End Sub

    Protected Sub rptEvents_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptEvents.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim ResourceID As String = DataBinder.Eval(e.Item.DataItem, "ResourceID").ToString()
            Dim DisplayPageKey As String = DataBinder.Eval(e.Item.DataItem, "DisplayPageKey").ToString()
            Dim EventID As Integer = CInt(DataBinder.Eval(e.Item.DataItem, "EventID"))
            Dim EventDateID As Integer = CInt(DataBinder.Eval(e.Item.DataItem, "EventDateID"))
            Dim StartDate As DateTime = CDate(DataBinder.Eval(e.Item.DataItem, "StartDate").ToString())
            Dim EndDate As DateTime = CDate(DataBinder.Eval(e.Item.DataItem, "EndDate").ToString())
            Dim EventName As String = DataBinder.Eval(e.Item.DataItem, "ResourceName").ToString()
            Dim EventDescription As String = DataBinder.Eval(e.Item.DataItem, "EventDescription").ToString()

            Dim lblEventDate As Label = CType(e.Item.FindControl("lblEventDate"), Label)
            Dim lblEventName As Label = CType(e.Item.FindControl("lblEventName"), Label)
            Dim lblEventDescription As Label = CType(e.Item.FindControl("lblEventDescription"), Label)
            Dim hypRegister As HyperLink = CType(e.Item.FindControl("hypRegister"), HyperLink)

            lblEventDate.Text = StartDate
            lblEventName.Text = EventName
            lblEventDescription.Text = EventDescription
            hypRegister.NavigateUrl = "/" & ResourceID & "/" & DisplayPageKey & ".htm"

        End If
    End Sub

End Class
