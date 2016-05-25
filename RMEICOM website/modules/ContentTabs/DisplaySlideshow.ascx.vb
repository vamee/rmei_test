
Partial Class modules_ContentTabs_DisplaySlideshow
    Inherits System.Web.UI.UserControl

    Dim _PageID As Integer
    Dim _SlideshowName As String = ""
    Dim _SlideshowSpeed As Integer = 0
    Dim _SlideShowTransition As String = ""
    Dim _SlideshowWidth As Integer = 0
    Dim _SlideshowHeight As Integer = 0
    Dim _PageModuleID As Integer = 0

    Public Property PageModuleID() As Integer
        Get
            Return _PageModuleID
        End Get
        Set(ByVal value As Integer)
            _PageModuleID = value
        End Set
    End Property

    Public Property PageID() As Integer
        Get
            Return _PageID
        End Get
        Set(ByVal value As Integer)
            _PageID = value
        End Set
    End Property

    Public Property SlideshowName() As String
        Get
            Return _SlideshowName
        End Get
        Set(ByVal value As String)
            _SlideshowName = value
        End Set
    End Property

    Public Property SlideshowSpeed() As Integer
        Get
            Return _SlideshowSpeed
        End Get
        Set(ByVal value As Integer)
            _SlideshowSpeed = value
        End Set
    End Property

    Public Property SlideshowTransition() As String
        Get
            Return _SlideShowTransition
        End Get
        Set(ByVal value As String)
            _SlideShowTransition = value
        End Set
    End Property

    Public Property SlideshowWidth() As Integer
        Get
            Return _SlideshowWidth
        End Get
        Set(ByVal value As Integer)
            _SlideshowWidth = value
        End Set
    End Property

    Public Property SlideshowHeight() As Integer
        Get
            Return _SlideshowHeight
        End Get
        Set(ByVal value As Integer)
            _SlideshowHeight = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Emagine.GetNumber(_PageModuleID) > 0 Then
            Dim CategoryID As Integer = Emagine.GetDbValue("SELECT ForeignValue FROM PageModules WHERE PageModuleID = " & PageModuleID)

            Dim SQL As String = "SELECT * FROM qryPageModuleProperties WHERE PageModuleID = " & PageModuleID & " ORDER BY SortOrder"
            Dim Rs As Data.SqlClient.SqlDataReader = Emagine.GetDataReader(SQL)
            Do While Rs.Read
                Select Case Rs("PropertyName")
                    Case "Slideshow Speed"
                        objSlideshow.FrameTimeout = Emagine.GetNumber(Rs("PropertyValue").ToString)

                    Case "Slideshow Transition Effects"
                        objSlideshow.TransitionEffect = Emagine.GetNumber(Rs("PropertyValue").ToString)

                    Case "Width"
                        objSlideshow.Width = Emagine.GetNumber(Rs("PropertyValue").ToString)

                    Case "Height"
                        objSlideshow.Height = Emagine.GetNumber(Rs("PropertyValue").ToString)
                End Select
            Loop
            Rs.Close()

            objSlideshow.DataSource = Emagine.GetDataTable("SELECT * FROM [qryContentTabs] WHERE PageModuleID = " & PageModuleID & " AND CategoryID = " & CategoryID & " AND (DisplayStartDate <= GetDate() OR DisplayStartDate = '1/1/1900 12:00:00 AM') AND (DisplayEndDate >= GetDate() OR DisplayEndDate = '1/1/1900 12:00:00 AM') AND IsEnabled = 'True' ORDER BY SortOrder")
            objSlideshow.DataBind()


        ElseIf _SlideshowName IsNot Nothing Then
            Dim CategoryID As Integer = Emagine.GetNumber(Emagine.GetDbValue("SELECT * FROM ModuleCategories WHERE CategoryName = '" & _SlideshowName & "'"))

            objSlideshow.FrameTimeout = _SlideshowSpeed
            objSlideshow.TransitionEffect = 1 'CInt(DataTable.Rows(0).Item("TransitionEffect").ToString)
            objSlideshow.Width = _SlideshowWidth
            objSlideshow.Height = _SlideshowHeight

            objSlideshow.DataSource = Emagine.GetDataTable("SELECT * FROM [qryContentTabs] WHERE CategoryID = " & CategoryID & " ORDER BY SortOrder")
            objSlideshow.DataBind()
        End If
    End Sub
End Class
