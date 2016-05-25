
Partial Class modules_Membership_MemberLoginStatus
    Inherits System.Web.UI.UserControl

    Dim _MemberType As String = ""

    Public Property MemberType() As String
        Get
            Return _MemberType
        End Get
        Set(ByVal value As String)
            _MemberType = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim MemberID As String = ""

        If Session("MemberID") IsNot Nothing Then
            MemberID = Session("MemberID")
        Else
            Me.RedirectToLogin()
        End If


        If MemberID.Length > 0 Then
            Dim Member As New Membership.Member
            Member = Membership.Member.GetMembershipUser(MemberID)

            If Member.MemberType = _MemberType Then
                pnlLoggedIn.Visible = True
                pnlLoggedOff.Visible = False

                lblLoginName.Text = "Welcome, " & Member.FirstName
            Else
                Me.RedirectToLogin()
            End If
        Else
            Me.RedirectToLogin()
        End If
    End Sub

    Sub RedirectToLogin()
        Session("RequestedPageUrl") = Request.RawUrl

        Dim CategoryID As Integer = Emagine.GetNumber(Emagine.GetDbValue("SELECT CategoryID FROM ModuleCategories WHERE CategoryName = '" & _MemberType.Replace("'", "''") & "'"))
        Dim RedirectPageKey As String = Emagine.GetDbValue("SELECT PageKey FROM Pages WHERE PageID IN (SELECT PageID FROM PageModules WHERE ModuleKey = 'Membership' AND ForeignKey='CategoryId' AND ForeignValue=" & CategoryID & ")")

        If RedirectPageKey.Length > 0 Then
            Response.Redirect("/" & RedirectPageKey & ".htm")
        Else

        End If
    End Sub

    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        Session("RequestedPageUrl") = Request.RawUrl
        Response.Redirect(GlobalVariables.GetValue("MembershipLoginUrl"))
    End Sub

    Protected Sub btnLogin_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Load
        btnLogin.PostBackUrl = Request.RawUrl
    End Sub

    Protected Sub btnLogOut_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogOut.Click
        If Session("MemberID") IsNot Nothing Then
            Dim MemberID As String = Session("MemberID")

            Dim Member As New Membership.Member
            Member = Membership.Member.GetMembershipUser(MemberID)
            Member.LoggedIn = False

            Membership.Member.Update(Member)
        End If

        Session.Abandon()
        Response.Redirect(Request.RawUrl)
    End Sub

    Protected Sub btnLogOut_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogOut.Load
        btnLogOut.PostBackUrl = Request.RawUrl
    End Sub
End Class
