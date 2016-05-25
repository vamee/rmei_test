
Partial Class modules_Navigation_Breadcrumbs
    Inherits System.Web.UI.UserControl

    Function GetHeaderGraphic(ByVal intPageID As Integer)
        Dim SQL As String = "SELECT HeaderGraphic FROM Pages WHERE PageID = " & intPageID
        Dim HeaderGraphic As String = Emagine.GetDbValue(SQL)

        If Len(HeaderGraphic) = 0 Then
            SQL = "SELECT HeaderGraphic FROM Pages WHERE PageID = " & Pages01.GetSectionPageId(intPageID)
            HeaderGraphic = Emagine.GetDbValue(SQL)
        End If

        Return HeaderGraphic
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim PageID As Integer = CInt(Session("PageID"))
        If Not IsPostBack Then

        End If
        imgPageHeader.ImageUrl = GetHeaderGraphic(PageID)
    End Sub
End Class
