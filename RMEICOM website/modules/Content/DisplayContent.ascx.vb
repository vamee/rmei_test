
Partial Class modules_Content_DisplayContent
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
        Dim ContentID As Integer = Emagine.GetNumber(Emagine.GetDbValue("SELECT ForeignValue FROM PageModules WHERE PageModuleID = " & PageModuleID))

        ltrContent.Text = Emagine.GetDbValue("SELECT Content FROM Content WHERE ContentID = " & ContentID)
    End Sub
End Class
