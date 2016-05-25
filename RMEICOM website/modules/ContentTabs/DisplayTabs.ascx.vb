
Partial Class modules_ContentTabs_DisplayTabs
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
        Dim Width As Integer = 0
        Dim Height As Integer = 0

        Dim SQL As String = "SELECT * FROM qryPageModuleProperties WHERE PageModuleID = " & PageModuleID & " ORDER BY SortOrder"
        Dim Rs As Data.SqlClient.SqlDataReader = Emagine.GetDataReader(SQL)
        Do While Rs.Read
            Select Case Rs("PropertyName")
                Case "Width"
                    Width = Emagine.GetNumber(Rs("PropertyValue").ToString)

                Case "Height"
                    Height = Emagine.GetNumber(Rs("PropertyValue").ToString)
            End Select
        Loop
        Rs.Close()

        Dim TabData As DataTable = Emagine.GetDataTable("SELECT * FROM [qryContentTabs] WHERE PageModuleID = " & PageModuleID & " AND CategoryID = " & CategoryID & " AND (DisplayStartDate <= GetDate() OR DisplayStartDate = '1/1/1900 12:00:00 AM') AND (DisplayEndDate >= GetDate() OR DisplayEndDate = '1/1/1900 12:00:00 AM') AND IsEnabled = 'True' ORDER BY SortOrder")
        Dim TabStrip1 As New Telerik.WebControls.RadTabStrip
        TabStrip1.Orientation = Telerik.WebControls.RadTabStripOrientation.HorizontalBottomToTop
        TabStrip1.Skin = "Default"
        Dim MultiPage1 As New Telerik.WebControls.RadMultiPage
        MultiPage1.ID = "MultiPage1"
        If Width > 0 Then MultiPage1.Width = Width
        If Height > 0 Then MultiPage1.Height = Height

        TabStrip1.MultiPageID = "MultiPage1"

        For i As Integer = 0 To (TabData.Rows.Count - 1)
            TabStrip1.Tabs.Add(New Telerik.WebControls.Tab(TabData.Rows(i).Item("ResourceName")))
            Dim PageView As New Telerik.WebControls.PageView
            PageView.CssClass = "tabsContent"
            If Width > 0 Then PageView.Width = Width
            If Height > 0 Then PageView.Height = Height
            Dim lblContent As New Label
            lblContent.Text = TabData.Rows(i).Item("Content")
            PageView.Controls.Add(lblContent)
            MultiPage1.PageViews.Add(PageView)
        Next

        TabStrip1.SelectedIndex = 0
        MultiPage1.SelectedIndex = 0

        plcTabs.Controls.Add(TabStrip1)
        plcTabs.Controls.Add(MultiPage1)

    End Sub

End Class
