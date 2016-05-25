
Partial Class modules_Pages01_DisplayPageName
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim PageID As Integer = Emagine.GetNumber(Session("PageID"))

        ltrPageName.Text = Emagine.GetDbValue("SELECT PageName FROM Pages WHERE PageID = " & PageID).ToString

    End Sub
End Class
