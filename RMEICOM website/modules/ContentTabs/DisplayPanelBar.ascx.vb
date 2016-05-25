Imports Telerik.WebControls

Partial Class modules_ContentTabs_DisplayPanelBar
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

    Public ReadOnly Property CategoryID() As Integer
        Get
            Return Emagine.GetDbValue("SELECT ForeignValue FROM PageModules WHERE PageModuleID = " & PageModuleID)
        End Get
    End Property

    Public ReadOnly Property Width() As Integer
        Get
            Return Emagine.GetNumber(Emagine.GetDbValue("SELECT PropertyValue FROM qryPageModuleProperties WHERE PageModuleID = " & _PageModuleID & " AND PropertyName = 'Width'"))
        End Get
    End Property

    Public ReadOnly Property Height() As Integer
        Get
            Return Emagine.GetNumber(Emagine.GetDbValue("SELECT PropertyValue FROM qryPageModuleProperties WHERE PageModuleID = " & _PageModuleID & " AND PropertyName = 'Height'"))
        End Get
    End Property

    Protected Sub RadPanelbar1_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.WebControls.RadPanelbarEventArgs) Handles RadPanelbar1.ItemDataBound
        Dim Item As RadPanelItem = e.Item
        Item.CssClass = "main"
        Dim DataRow As DataRowView = CType(e.Item.DataItem, DataRowView)

        Dim Table As New Table
        Dim Row As New TableRow
        Dim Cell1 As New TableCell
        Cell1.Width = 20
        Dim Cell2 As New TableCell
        Cell2.Text = DataRow.Item("Content")
        'Cell2.CssClass = "main"
        Row.Cells.Add(Cell1)
        Row.Cells.Add(Cell2)
        Table.Rows.Add(Row)

        Dim ContentPanel As New Panel
        ContentPanel.Controls.Add(Table)

        Dim ChildItem As New RadPanelItem
        ChildItem.Controls.Add(ContentPanel)
        Item.Items.Add(ChildItem)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            RadPanelbar1.Width = Width
            RadPanelbar1.Height = Height

            RadPanelbar1.DataSource = Emagine.GetDataTable("SELECT * FROM [qryContentTabs] WHERE PageModuleID = " & PageModuleID & " AND CategoryID = " & CategoryID & " AND (DisplayStartDate <= GetDate() OR DisplayStartDate = '1/1/1900 12:00:00 AM') AND (DisplayEndDate >= GetDate() OR DisplayEndDate = '1/1/1900 12:00:00 AM') AND IsEnabled = 'True' ORDER BY SortOrder")
            RadPanelbar1.DataTextField = "ResourceName"
            RadPanelbar1.DataValueField = "TabID"
            RadPanelbar1.DataBind()
        End If
    End Sub
End Class
