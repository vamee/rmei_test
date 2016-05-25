Partial Class Ezedit_Login
    Inherits System.Web.UI.Page

    Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim userName As String = txtUserId.Text
        Dim password As String = txtPassword.Text
        'Response.Write(userName & "<br>")
        'Response.Write(Emagine.GetMd5Hash(password))
        'Response.End()

        Dim MyUser As EzeditUser = EzeditUser.GetUser(userName, password)


        If MyUser.UserID <> 0 Then
            Session("EzEditUserID") = MyUser.UserID
            Session("EzEditLevelID") = MyUser.EzEditLevelID
            Session("EzEditUsername") = MyUser.Username
            Session("EzEditName") = MyUser.FirstName & " " & MyUser.LastName
            Session("EzEditEmail") = MyUser.Email
            Session("EzEditLanguageID") = MyUser.LanguageID
            Session("EzEditLanguageName") = Emagine.GetDbValue("SELECT LanguageName FROM Languages WHERE LanguageID = " & MyUser.LanguageID)
            Session("EzEditStatusID") = 20

            Dim cookie As New HttpCookie("EzEditUserId", MyUser.Username)
            cookie.Expires = Now().AddDays(90)
            Response.Cookies.Add(cookie)

            If MyUser.FirstLogin = "" Then
                MyUser.FirstLogin = Now()
            End If

            EzeditUser.UpdateUserLogin(MyUser)

            If Not (Request.QueryString("ReturnUrl") Is Nothing) Then
                FormsAuthentication.RedirectFromLoginPage(userName, False)
            Else
                FormsAuthentication.SetAuthCookie(userName, False)
                Response.Redirect("~/Ezedit/Default.aspx")
            End If


        Else
            ' Response.Write("invalid")
            lblResults.Visible = True
            lblResults.Text = "Unsuccessful login.  Please re-enter your information and try again."
        End If
    End Sub

    Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        '  Me.DataBind()
    End Sub

End Class
