Imports PeterBlum.vam

Partial Class Ezedit_Modules_Events01_EditDate
    Inherits System.Web.UI.Page

    Dim CategoryID As Integer = 0
    Dim EventID As Integer = 0
    Dim EventDateID As Integer = 0

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        CategoryID = Emagine.GetNumber(Request("CategoryID"))
        EventID = Emagine.GetNumber(Request("EventID"))
        EventDateID = Emagine.GetNumber(Request("EventDateID"))
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.SetPageTitle(EventID)

            If EventDateID > 0 Then
                Me.Edit(EventDateID)
                btnDelete.Visible = True
            End If
        End If
    End Sub

    Sub SetPageTitle(ByVal intEventId As Integer)
        Dim strPageTitle As New StringBuilder
        Dim dtr As Data.SqlClient.SqlDataReader = Emagine.GetDataReader("SELECT CategoryID, CategoryName, ResourceName FROM qryEvents WHERE EventID = " & intEventId)
        If dtr.Read() Then
            strPageTitle.Append("> <a href='ItemList.aspx?CategoryID=" & dtr("CategoryId") & "' class='breadcrumb'>" & dtr("CategoryName") & "</a>")
            strPageTitle.Append(" > " & dtr("ResourceName"))
        End If
        dtr.Close()
        lblPageTitle.Text = strPageTitle.ToString
    End Sub

    Sub Edit(ByVal intEventDateID As Integer)
        Dim MyEventDate As EventDate = EventDate.GetEventDate(intEventDateID)
        With MyEventDate
            txtEventDate.Text = MyEventDate.EventDate
            txtDuration.Text = MyEventDate.Duration
            txtLocation.Text = MyEventDate.Location
        End With
        lblPageTitle.Text += " > Edit Event Date"
    End Sub

    Sub Add(ByVal intEventId As Integer)
        lblPageTitle.Text += " > Add Event Date"
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If PeterBlum.VAM.Globals.Page.IsValid Then
            Dim ErrorMessage As String = ""
            Dim EventID As Integer = CInt(Request("EventID"))
            Dim EventDateID As Integer = CInt(Request("EventDateID"))
            Dim MyEvent As Events01 = Events01.GetEvent(EventID)
            Dim CategoryID As Integer = MyEvent.CategoryID
            Dim Result As Boolean

            Dim MyEventDate As New EventDate
            If EventDateID > 0 Then MyEventDate = EventDate.GetEventDate(EventDateID)
            MyEventDate.EventID = EventID
            MyEventDate.EventDate = txtEventDate.Text
            MyEventDate.Duration = txtDuration.Text
            MyEventDate.Location = txtLocation.Text
            MyEvent.CreatedBy = Session("EzEditName")
            MyEventDate.UpdatedDate = Date.Now()
            MyEventDate.UpdatedBy = Session("EzEditName")

            If Emagine.GetNumber(MyEventDate.EventDateID) > -1 Then
                Result = EventDate.Update(MyEventDate, ErrorMessage)
                Session("Alert") = "The event date has been updated successfully."

            Else
                Result = EventDate.Add(MyEventDate, ErrorMessage)
                Session("Alert") = "The event date has been added successfully."
            End If

            If Result Then
                Response.Redirect("ItemList.aspx?CategoryID=" & CategoryID)
            Else
                Session("Alert") = ""
                lblAlert.Text = "An error has occurred: " & ErrorMessage
            End If
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        CategoryID = Emagine.GetNumber(Emagine.GetDbValue("SELECT CategoryID FROM Events WHERE EventID = " & EventID))
        Response.Redirect("ItemList.aspx?CategoryID=" & CategoryID)
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim ErrorMessage As String = ""
        Dim EventID = CInt(Request("EventID"))
        Dim EventDateID = CInt(Request("EventDateID"))
        Dim MyEvent As Events01 = Events01.GetEvent(EventID)
        Dim CategoryID As Integer = MyEvent.CategoryID

        If EventDate.Delete(EventDateID, ErrorMessage) Then
            Session("Alert") = "The date has been removed successfully."
            Response.Redirect("ItemList.aspx?CategoryID=" & CategoryID)
        Else
            lblAlert.Text = "An error has occurred: " & ErrorMessage
        End If
    End Sub
End Class
