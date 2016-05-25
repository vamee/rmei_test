
Partial Class modules_Content_DisplaySeoScript
    Inherits System.Web.UI.UserControl

    Protected Sub ltrlSeoScript_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ltrlSeoScript.Load
        ltrlSeoScript.Text = Emagine.GetDbValue("SELECT SeoScript FROM Pages WHERE PageID = " & Session("PageID"))
    End Sub
End Class
