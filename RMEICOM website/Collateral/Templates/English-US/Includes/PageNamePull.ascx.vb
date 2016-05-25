Partial Class modules_Pages01_PageNamePull
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblPageName.Text = Emagine.GetDbValue("SELECT MenuName FROM Pages WHERE PageID = " & Session("PageID"))

    End Sub
End Class