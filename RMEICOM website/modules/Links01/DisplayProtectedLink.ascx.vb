
Partial Class UserControls_ContentLinks_Protected
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
        Dim ResourceID As String = Resources.GetResourceID()

        If ResourceID.Length > 0 Then

            Dim LinkID As Integer = Emagine.GetNumber(Emagine.GetDbValue("SELECT ForeignValue FROM qryDisplayPageModules WHERE PageModuleID = " & PageModuleID))

            Dim LinkUrl As String = Emagine.GetDbValue("SELECT LinkURL FROM ContentLinks WHERE LinkID = " & LinkID & " AND ResourceID = '" & ResourceID & "'")

            If LinkUrl.Length > 0 Then
                hypContinue.NavigateUrl = LinkUrl
                pnlLinks.Visible = True
            End If

            If Request.ServerVariables("HTTP_REFERER") IsNot Nothing Then
                Dim CancelUrl As String = Request.ServerVariables("HTTP_REFERER").ToString
                hypCancel.Visible = True
                hypCancel.NavigateUrl = CancelUrl
            End If
        End If
    End Sub

    
    

    
End Class
