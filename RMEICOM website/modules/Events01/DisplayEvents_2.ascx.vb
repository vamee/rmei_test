
Partial Class modules_Events01_DisplayEvents_2
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
        Dim CategoryID As Integer = Emagine.GetDbValue("SELECT ForeignValue FROM PageModules WHERE PageModuleID = " & PageModuleID)
        Dim ResourceID As String = Resources.GetResourceID()
        Dim EventID As Integer = 0
        Dim EventDateID As Integer = Emagine.GetNumber(Request("DateID"))
        Dim DisplayPageKey As String = ""

        Me.DisplayEventList(CategoryID, DisplayPageKey)

    End Sub

    Sub DisplayEventList(ByVal intCategoryID As Integer, ByVal strDisplayPageKey As String)
        Dim DataTable As DataTable = Emagine.GetDataTable("SELECT Events.*, Resources.ResourceName As EventName FROM Events INNER JOIN Resources ON Events.ResourceID = Resources.ResourceID ORDER BY SortOrder")

        gdvEvents.DataSource = DataTable
        gdvEvents.DataBind()

    End Sub

    Protected Sub gdvEvents_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvEvents.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ArchiveUrl As String = DataBinder.Eval(e.Row.DataItem, "ArchiveUrl")
            Dim ImageUrl As String = DataBinder.Eval(e.Row.DataItem, "ImageUrl")
            Dim EventDescription As String = DataBinder.Eval(e.Row.DataItem, "EventDescription")
            Dim EventName As String = DataBinder.Eval(e.Row.DataItem, "EventName")

            Dim hypImageUrl As HyperLink = e.Row.FindControl("hypImageUrl")
            Dim imgImageUrl As Image = e.Row.FindControl("imgImageUrl")
            Dim lblEventName As Label = e.Row.FindControl("lblEventname")
            Dim lblEventDescription As Label = e.Row.FindControl("lblEventDescription")
            Dim hypArchiveUrl As HyperLink = e.Row.FindControl("hypArchiveUrl")

            If lblEventName IsNot Nothing Then lblEventName.Text = EventName
            If imgImageUrl IsNot Nothing Then imgImageUrl.ImageUrl = ImageUrl
            If hypImageUrl IsNot Nothing And ArchiveUrl.Length > 0 Then hypImageUrl.NavigateUrl = ArchiveUrl
            If lblEventDescription IsNot Nothing Then lblEventDescription.Text = EventDescription
            If hypArchiveUrl IsNot Nothing Then
                hypArchiveUrl.NavigateUrl = ArchiveUrl
                hypArchiveUrl.Text = ArchiveUrl
            End If


        End If
    End Sub
End Class
