
Partial Class Ezedit_Modules_Pages01_EmailContent
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
	    Dim intContentId As Integer = CInt(Request("ContentID"))
	    Dim intPageID As Integer = CInt(Request("PageID"))

            txtEmailSubject.Text = "Please review this content"
            txtEmailMessage.Text = "Please review the following content:" & vbCrLf & vbCrLf
            txtEmailMessage.Text += "http://" & Request.ServerVariables("SERVER_NAME") & "/modules/Pages01/GetPage.aspx?PageID=" & intPageID & "&ContentID=" & intContentID
        End If
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Emagine.SendEmail(Session("EzEditEmail"), Session("EzEditName"), txtEmailTo.Text, txtEmailTo.Text, txtEmailCC.Text, "", txtEmailSubject.Text, txtEmailMessage.Text, "", False)
        Emagine.CloseReload()
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Emagine.CloseWindow()
    End Sub

End Class
