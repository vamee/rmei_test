
Partial Class modules_Membership_MemberLogin
    Inherits System.Web.UI.UserControl

    Dim _ServerName As String = HttpContext.Current.Request.ServerVariables("SERVER_NAME")
    Dim _EmailFromAddress As String = "webmaster@" & _ServerName
    Dim _EmailFromName As String = "webmaster@" & _ServerName
    Dim _EmailSubject As String = _ServerName & " Login Information"
    Dim _EmailCc As String = ""
    Dim _EmailBcc As String = ""
    Dim _PageModuleID As Integer = 0
    'Dim _MemberType As String = ""

    Public Property PageModuleID() As Integer
        Get
            Return _PageModuleID
        End Get
        Set(ByVal value As Integer)
            _PageModuleID = value
        End Set
    End Property

    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        If PeterBlum.VAM.Globals.Page.IsValid Then
            Dim Member As New Membership.Member
            Member = Membership.Member.GetMembershipUser(txtUsername.Text, txtPassword.Text)
            Dim RequestedPageID As Integer = 0
            If Session("RequestedPageID") IsNot Nothing Then
                RequestedPageID = Emagine.GetNumber(Session("RequestedPageID"))
            End If

            If Member.MemberID.Length > 0 Then

                Session("MemberID") = Member.MemberID
                Session("MemberName") = Member.FirstName & " " & Member.LastName
                Session("MemberType") = Member.MemberType
                Session("MemberCategoryID") = Member.CategoryID

                If Member.LoginCount = 0 Then Member.FirstLoginDate = Date.Now
                Member.LastLoginDate = Date.Now
                Member.LoginCount = (Member.LoginCount + 1)
                If cbxIsLoggedIn.Checked Then Member.LoggedIn = True Else Member.LoggedIn = False
                Membership.Member.Update(Member)

                Dim OutCookie As HttpCookie = HttpContext.Current.Response.Cookies("MemberID")
                OutCookie.Value = Member.MemberID
                OutCookie.Expires = DateAdd(DateInterval.Year, 1, Now())
                OutCookie.Domain = HttpContext.Current.Request.ServerVariables("SERVER_NAME")

                Dim Sql As String = "SELECT PageKey FROM Pages WHERE PageID IN (SELECT PropertyValue FROM PageModuleProperties WHERE PropertyID IN (SELECT PropertyID FROM ModuleProperties WHERE ModuleKey='Membership' AND PropertyName = 'HomePage' AND PageModuleID IN (SELECT PageModuleID FROM PageModules WHERE ModuleKey='Membership' AND ForeignValue='" & Member.CategoryID & "')))"
                Dim DefaultRedirectKey As String = Emagine.GetDbValue(Sql)
                If DefaultRedirectKey.Length > 0 Then
                    DefaultRedirectKey = "/" & DefaultRedirectKey & ".htm"
                Else
                    DefaultRedirectKey = "/"
                End If

                If Emagine.GetNumber(Emagine.GetDbValue("SELECT PageID FROM Pages_MemberCategories WHERE PageID = " & RequestedPageID & " AND MemberCategoryID = " & Member.CategoryID)) > 0 Then
                    If Session("RequestedPageUrl") IsNot Nothing Then
                        Response.Redirect(Session("RequestedPageUrl"))
                    Else
                        Response.Redirect(DefaultRedirectKey)
                    End If

                Else
                    Response.Redirect(DefaultRedirectKey)
                End If

            Else
                lblAlert.Text = "Login failed."
            End If

        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Dim MemberID As String = ""

            If Session("MemberID") IsNot Nothing Then MemberID = Session("MemberID")

            If MemberID.Length > 0 Then
                Dim Member As New Membership.Member
                Member = Membership.Member.GetMembershipUser(MemberID)

                Me.DisplayPanel("LoggedIn")
                Me.DisplayMemberStats(Member)

            Else
                MemberID = Membership.Member.GetMembershipUserID()

                If MemberID.Length > 0 Then
                    Dim Member As New Membership.Member
                    Member = Membership.Member.GetMembershipUser(MemberID)

                    If Member.LoggedIn Then
                        Session("MemberID") = Member.MemberID
                        Session("MemberName") = Member.FirstName & " " & Member.LastName
                        Session("MemberType") = Member.MemberType

                        Member.LastLoginDate = Date.Now
                        Member.LoginCount = (Member.LoginCount + 1)
                        Membership.Member.Update(Member)

                        Dim OutCookie As HttpCookie = HttpContext.Current.Response.Cookies("MemberID")
                        OutCookie.Value = Member.MemberID
                        OutCookie.Expires = DateAdd(DateInterval.Year, 1, Now())
                        OutCookie.Domain = HttpContext.Current.Request.ServerVariables("SERVER_NAME")

                        Dim RedirectUrl As String = ""

                        If Session("RequestedPageUrl") IsNot Nothing Then
                            RedirectUrl = Session("RequestedPageUrl")
                        Else
                            RedirectUrl = PageModuleProperty.GetProperty(_PageModuleID, "HomePage")
                        End If

                        If RedirectUrl.Length > 0 Then
                            Response.Redirect(RedirectUrl)
                        Else
                            Response.Write("/")
                        End If

                    Else
                        If Member.Username.Length > 0 Then
                            txtUsername.Text = Member.Username
                            txtPassword.Focus()
                        Else
                            txtUsername.Focus()
                        End If
                    End If

                Else
                    txtUsername.Focus()
                End If

            End If
        End If

        btnDisplayPasswordLookup.PostBackUrl = Request.RawUrl
        btnLogin.PostBackUrl = Request.RawUrl
        btnPasswordLookup.PostBackUrl = Request.RawUrl
        btnCancelPasswordLookup.PostBackUrl = Request.RawUrl
        btnLogOut.PostBackUrl = Request.RawUrl
    End Sub

    Protected Sub btnDisplayPasswordLookup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplayPasswordLookup.Click
        Me.DisplayPanel("PasswordLookup")
    End Sub

    Sub DisplayPanel(ByVal strPanelName As String)
        lblAlert.Text = ""

        Select Case strPanelName
            Case "Login"
                pnlLogin.Visible = True
                pnlPasswordLookup.Visible = False
                pnlLoggedIn.Visible = False

            Case "PasswordLookup"
                pnlPasswordLookup.Visible = True
                pnlLogin.Visible = False
                pnlLoggedIn.Visible = False

            Case "LoggedIn"
                pnlLoggedIn.Visible = True
                pnlPasswordLookup.Visible = False
                pnlLogin.Visible = False
        End Select
    End Sub

    Protected Sub btnPasswordLookup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPasswordLookup.Click
        If PeterBlum.VAM.Globals.Page.IsValid Then
            Dim MemberData As DataTable = Emagine.GetDataTable("SELECT * FROM Members WHERE Email = '" & txtEmailLookup.Text.Replace("'", "''") & "'")

            If MemberData.Rows.Count > 0 Then

                Dim MemberName As String = MemberData.Rows(0).Item("FirstName") & " " & MemberData.Rows(0).Item("LastName")

                Dim BodyBuilder As New StringBuilder
                BodyBuilder.Append("Here is your " & _ServerName & " login information:<br><br>")
                BodyBuilder.Append("Username: " & MemberData.Rows(0).Item("Username"))
                BodyBuilder.Append("<br>")
                BodyBuilder.Append("Password: " & MemberData.Rows(0).Item("Password"))
                BodyBuilder.Append("<br><br>")

                If Emagine.SendEmail(_EmailFromAddress, _EmailFromName, txtEmailLookup.Text, MemberName, _EmailCc, _EmailBcc, _EmailSubject, BodyBuilder.ToString, "", True) Then
                    Me.DisplayPanel("Login")
                    lblAlert.Text = "An email has been sent to your email address with your login information"

                Else
                    lblAlert.Text = "An error occurred while processing your request. Please try again later."
                End If

            Else
                lblAlert.Text = "Your email address could not be found in our records."

            End If
        End If
    End Sub

    Protected Sub btnCancelPasswordLookup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelPasswordLookup.Click
        Me.DisplayPanel("Login")
    End Sub

    Sub DisplayMemberStats(ByVal objMember As Membership.Member)
        lblMemberName.Text = objMember.FirstName & " " & objMember.LastName
        lblFirstLogin.Text = objMember.FirstLoginDate
        lblLastLogin.Text = objMember.LastLoginDate
        lblLoginCount.Text = objMember.LoginCount & " times"
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
        Response.Redirect("/")
    End Sub
End Class
