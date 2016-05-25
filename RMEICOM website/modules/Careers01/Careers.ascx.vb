
Partial Class Display_Careers01
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

    Sub DisplayCareerList(ByVal intCategoryID As Integer, ByVal strDisplayPageKey As String)
        Dim Sql As String = "SELECT DISTINCT DisplayDate, ResourceName, CareerSummary, CareerID, SortOrder, CategoryID, ResourcePageKey, DeliveryPageKey, ResourceID, ResourceCategory, IsEnabled FROM qryCareers WHERE DisplayPageID = " & Session("PageID") & " AND CategoryID = " & intCategoryID & " AND IsEnabled = 1 ORDER BY ResourceCategory, SortOrder"

        Dim Rs As DataTable = Emagine.GetDataTable(Sql) 'Careers01.GetCareers(intCategoryID, True)
        Dim RowStyle As String = ""
        Dim Careers As String = ""
        Dim LastResourceCategory As String = "XXXXXXXXXXXXXXXX"
        Dim FirstPass As Boolean = True

        Careers = Careers & "<!-- START CAREERS -->"
        If Rs.Rows.Count > 0 Then
            For i As Integer = 0 To (Rs.Rows.Count - 1)
                If RowStyle = "table-row" Then RowStyle = "table-altrow" Else RowStyle = "table-row"

                If Rs.Rows(i).Item("ResourceCategory") <> LastResourceCategory Then
                    If Not FirstPass Then Careers = Careers & "</table><br>"
                    Careers = Careers & "<h2>" & Rs.Rows(i).Item("ResourceCategory") & "</h2>"
                    Careers = Careers & "<table id='careerTable' cellpadding='0' cellspacing='0' width='100%' border='0'>"
                End If

                Careers = Careers & "<tr>"
                Careers = Careers & "<td valign='top'>"
                Careers = Careers & "<table class='" & RowStyle & "' cellpadding='0' cellspacing='0' width='100%' border='0'>"
                Careers = Careers & "<tr>"
                Careers = Careers & "<td style='padding-left: 10px; padding-top: 5px;'>"
                Careers = Careers & "<b><a href='/" & Rs.Rows(i).Item("ResourceID") & "/" & Rs.Rows(i).Item("DeliveryPageKey") & ".htm'>" & Rs.Rows(i).Item("ResourceName") & "</a></b>"
                Careers = Careers & "<br>"
                Careers = Careers & "<div>" & Rs.Rows(i).Item("CareerSummary") & "</div>"
                Careers = Careers & "</td>"
                Careers = Careers & "</tr>"
                Careers = Careers & "</table>"
                Careers = Careers & "</td>"
                Careers = Careers & "</tr>"
                LastResourceCategory = Rs.Rows(i).Item("ResourceCategory")
                FirstPass = False
            Next
            Careers = Careers & "</table>"
            lblCareerList.Text = Careers
        Else
            lblCareerList.Text = "We're sorry. There are currently no open positions. Please check back soon as this page is updated regularly."
        End If
        Careers = Careers & "<!-- END CAREERS -->"


    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim CategoryID As Integer = Emagine.GetDbValue("SELECT ForeignValue FROM PageModules WHERE PageModuleID = " & PageModuleID)
        Dim ResourceID As String = Resources.GetResourceID()
        Dim CareerID As Integer = 0
        Dim DisplayPageKey As String = ""
        Dim Apply As Integer = CInt(Request("ApplyOnline"))

        If Len(ResourceID) > 0 Then
            Dim SQL As String = "SELECT * FROM qryCareers WHERE ResourceID = '" & ResourceID & "'"
            Dim Rs As Data.SqlClient.SqlDataReader = Emagine.GetDataReader(SQL)
            If Rs.Read Then
                CareerID = Rs("CareerID")
                DisplayPageKey = Rs("DisplayPageKey")
            End If
            Rs.Close()
            Rs = Nothing
        End If

        If Apply = 1 And CareerID > 0 Then
            ApplyOnline(CareerID, ResourceID)
        ElseIf CareerID > 0 Then
            DisplayCareer(CareerID)
        Else
            DisplayCareerList(CategoryID, DisplayPageKey)
        End If
    End Sub

    Sub DisplayCareer(ByVal intCareerID As Integer)

        Dim FormRedirect As String = ""
        Dim ResourceID As String = ""
        Dim SQL As String = "SELECT ResourceID, FormPageID, FormPageKey, ResourceID FROM qryCareers WHERE CareerID = " & intCareerID
        Dim Rs As Data.SqlClient.SqlDataReader = Emagine.GetDataReader(SQL)

        If Rs.Read Then
            Resources.UpdateClickCount(Rs("ResourceID"))
            ResourceID = Rs("ResourceID")
            If Emagine.GetNumber(Rs("FormPageID").ToString) > 0 Then
                FormRedirect = "/" & Rs("ResourceID") & "/" & Rs("FormPageKey") & ".htm"
            End If
        End If
        Rs.Close()
        Rs = Nothing

        SQL = "SELECT ResourceName, CareerText FROM qryCareers WHERE CareerID = " & intCareerID
        Rs = Emagine.GetDataReader(SQL)
        If Rs.Read Then
            lblJobTitle.Text = Rs("ResourceName")
            lblJobDescription.Text = Rs("CareerText")
            JobDetail.Visible = True
            If Len(FormRedirect) > 0 Then
                btnApplyOnline.Visible = True
                btnApplyOnline.PostBackUrl = Request.RawUrl.ToString
            End If
        End If
        Rs.Close()
        Rs = Nothing
        
    End Sub

    Sub ApplyOnline(ByVal intCareerID As Integer, ByVal strResourceID As String)
        Dim ResourceID As String = Resources.GetResourceID()
        Dim UserID As String = Emagine.Users.User.GetUserID()
        Dim FormPageKey As String = ""
        Dim SQL As String = "SELECT * FROM qryCareers WHERE CareerID = " & intCareerID
        Dim Rs As System.Data.SqlClient.SqlDataReader = Emagine.GetDataReader(SQL)

        If Rs.Read Then
            FormPageKey = Rs("FormPageKey")
            If FormPageKey.Length > 0 Then
                Emagine.Users.User.SetResource(UserID, strResourceID, "Career Name:", Rs("ResourceName").ToString())
            End If
        End If

        If FormPageKey.Length > 0 Then
            Response.Redirect("/" & strResourceID & "/" & FormPageKey & ".htm")
        Else
            lblAlert.Text = "We're sorry. An error has occurred while processing your request.<br>Please try again later."
        End If
    End Sub


    Protected Sub btnApplyOnline_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApplyOnline.Click

    End Sub

End Class
