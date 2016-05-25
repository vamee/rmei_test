
Partial Class Events01_CurrentWebcasts_1
    Inherits System.Web.UI.UserControl

    Dim _PageModuleID As Integer = 0

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

        If EventDateID > 0 Then
            If Emagine.GetNumber(Emagine.GetDbValue("SELECT COUNT(*) FROM EventDates WHERE EventDateID = " & EventDateID)) = 0 Then
                EventDateID = 0
            End If
        End If

        If EventDateID > 0 Then
            RegisterForEvent(EventDateID, ResourceID)
        ElseIf EventID > 0 Then
            'DeliveryPage Display
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

    Sub RegisterForEvent(ByVal intEventDateID As Integer, ByVal strResourceID As String)
        Dim UserID As String = Emagine.Users.User.GetUserID()
        Dim FormPageKey As String = ""
        Dim SQL As String = "SELECT * FROM qryEventDates WHERE EventDateID = " & intEventDateID
        Dim Rs As System.Data.SqlClient.SqlDataReader = Emagine.GetDataReader(SQL)

        If Rs.Read Then
            FormPageKey = Rs("FormPageKey").ToString()

            Dim EventDate As String
            If Rs("StartDate").ToString = Rs("EndDate").ToString Then
                EventDate = String.Format("{0:d}", Rs("StartDate").ToString)
            Else
                EventDate = String.Format("{0:d}", Rs("StartDate").ToString) & " - " & String.Format("{0:d}", Rs("EndDate").ToString)
            End If

            Dim EventTime As String = ""
            If Rs("StartTime").ToString.Length > 0 Then EventTime = Rs("StartTime").ToString
            If Rs("EndTime").ToString.Length > 0 Then
                If EventTime.Length > 0 Then EventTime = EventTime & " - "
                EventTime = EventTime & Rs("EndTime").ToString
            End If

            If FormPageKey.Length > 0 Then
                Emagine.Users.User.SetResource(UserID, strResourceID, "Webcast Name:", Rs("ResourceName").ToString())
                Emagine.Users.User.SetResource(UserID, strResourceID, "Webcast Date:", EventDate)
                Emagine.Users.User.SetResource(UserID, strResourceID, "Webcast Time:", EventTime)
            End If
        End If

        If FormPageKey.Length > 0 Then
            Response.Redirect("/" & strResourceID & "/" & FormPageKey & ".htm")
        Else
            lblAlert.Text = "We're sorry. An error has occurred while processing your request.<br>Please try again later."
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
            hypRegister.NavigateUrl = "/" & ResourceID & "/" & DisplayPageKey & ".htm?DateID=" & EventDateID.ToString()

        End If
    End Sub

End Class
