
Partial Class Display_Custom01
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
        Dim SQL As String = "SELECT FileName FROM qryCustomApplications WHERE IsEnabled = 'True' AND ModuleKey = 'Custom01' AND PageModuleID = '" & PageModuleID & "' ORDER BY SortOrder"

        Dim CodeFileName As String = Emagine.GetDbValue(SQL)
        Dim ucPageModule As Control = LoadControl(CodeFileName)

        plcCustomModule.Controls.Add(ucPageModule)

    End Sub
End Class
