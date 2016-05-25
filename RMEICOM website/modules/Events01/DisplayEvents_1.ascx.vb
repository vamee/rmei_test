
Partial Class Events01_DisplayEvents_1
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
        Dim Rs As Data.SqlClient.SqlDataReader = Events01.GetEvents(intCategoryID)
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
            FormPageKey = Rs("FormPageKey")
            Dim EventDate As String
            If Rs("StartDate") = Rs("EndDate") Then
                EventDate = String.Format("{0:d}", Rs("StartDate"))
            Else
                EventDate = String.Format("{0:d}", Rs("StartDate")) & " - " & String.Format("{0:d}", Rs("EndDate"))
            End If

            Dim EventTime As String = ""
            If Rs("StartTime").ToString.Length > 0 Then EventTime = Rs("StartTime")
            If Rs("EndTime").ToString.Length > 0 Then
                If EventTime.Length > 0 Then EventTime = EventTime & " - "
                EventTime = EventTime & Rs("EndTime")
            End If

            If FormPageKey.Length > 0 Then
                Emagine.Users.User.SetResource(UserID, strResourceID, "Event Name:", Rs("ResourceName").ToString())
                Emagine.Users.User.SetResource(UserID, strResourceID, "Event Date:", EventDate)
                Emagine.Users.User.SetResource(UserID, strResourceID, "Event Time:", EventTime)
                Emagine.Users.User.SetResource(UserID, strResourceID, "Location:", Rs("Description").ToString())
            End If
        End If

        If FormPageKey.Length > 0 Then
            Response.Redirect("/" & strResourceID & "/" & FormPageKey & ".htm?" & intEventDateID)
        Else
            lblAlert.Text = "We're sorry. An error has occurred while processing your request.<br>Please try again later."
        End If

    End Sub

    Protected Sub rptEvents_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptEvents.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim intEventID As Integer = CInt(DataBinder.Eval(e.Item.DataItem, "EventID"))
            Dim rptEventDates As Repeater = CType(e.Item.FindControl("rptEventDates"), Repeater)
            AddHandler rptEventDates.ItemDataBound, AddressOf rptEventDates_ItemDataBound
            Dim objRs As System.Data.SqlClient.SqlDataReader = EventDates.GetEventDates(intEventID)
            If objRs.HasRows Then
                rptEventDates.DataSource = objRs
                rptEventDates.DataBind()
            End If
        End If
    End Sub

    Sub rptEventDates_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim Description As String = DataBinder.Eval(e.Item.DataItem, "Description").ToString()
            Dim StartDate As Date = CDate(DataBinder.Eval(e.Item.DataItem, "StartDate").ToString())
            Dim EndDate As Date = CDate(DataBinder.Eval(e.Item.DataItem, "EndDate").ToString())
            Dim EventDateID As Integer = CInt(DataBinder.Eval(e.Item.DataItem, "EventDateID"))
            Dim FormPageKey As String = DataBinder.Eval(e.Item.DataItem, "FormPageKey").ToString()
            Dim ResourcePageKey As String = DataBinder.Eval(e.Item.DataItem, "ResourcePageKey").ToString()
            Dim DeliveryPageKey As String = DataBinder.Eval(e.Item.DataItem, "DeliveryPageKey").ToString()
            Dim DisplayPageKey As String = DataBinder.Eval(e.Item.DataItem, "DisplayPageKey").ToString()
            Dim ResourceID As String = DataBinder.Eval(e.Item.DataItem, "ResourceID").ToString()

            Dim lblEventDate As Label = CType(e.Item.FindControl("lblEventDate"), Label)
            Dim lblEventLocation As Label = CType(e.Item.FindControl("lblEventLocation"), Label)
            Dim hypEventRegister As HyperLink = CType(e.Item.FindControl("hypEventRegister"), HyperLink)

            If StartDate = EndDate Then
                lblEventDate.Text = MonthName(StartDate.Month) & " " & StartDate.Day & ", " & StartDate.Year
            Else
                lblEventDate.Text = MonthName(StartDate.Month) & " " & StartDate.Day & "-" & EndDate.Day & ", " & StartDate.Year
            End If

            'lblEventDate.Text = StartDate.ToShortDateString()
            lblEventLocation.Text = Description

            If StartDate > Date.Now() Then
                hypEventRegister.Text = "Register"
                hypEventRegister.NavigateUrl = "/" & ResourceID & "/" & DisplayPageKey & ".htm?DateID=" & EventDateID.ToString()
            End If

        End If
    End Sub


End Class
