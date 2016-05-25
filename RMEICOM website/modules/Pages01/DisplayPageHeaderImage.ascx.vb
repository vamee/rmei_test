
Partial Class modules_Pages01_DisplayPageHeader
    Inherits System.Web.UI.UserControl

    Dim _LocationName As String = ""

    Public Property LocationName() As String
        Get
            Return _LocationName
        End Get
        Set(ByVal value As String)
            _LocationName = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not Page.IsPostBack Then
        Dim ImageUrl As String = ""
        Dim HeaderText As String = ""
        Dim PageID As Integer = CInt(Session("PageID"))

        Dim HeaderData As DataTable = Emagine.GetDataTable("SELECT ImageUrl, HeaderText FROM qryPageHeaders WHERE PageID = " & PageID & " AND HeaderName = '" & _LocationName & "'")
        If HeaderData.Rows.Count > 0 Then
            ImageUrl = HeaderData.Rows(0).Item("ImageUrl").ToString
            HeaderText = HeaderData.Rows(0).Item("HeaderText").ToString
        End If

        If ImageUrl.Length > 0 Or HeaderText.Length > 0 Then
            PageID = 0
        Else
            PageID = Emagine.GetDbValue("SELECT ParentPageID FROM Pages WHERE PageID = " & PageID)
        End If

        Do While PageID > 0
            HeaderData = Emagine.GetDataTable("SELECT ImageUrl, HeaderText FROM qryPageHeaders WHERE PageID = " & PageID & " AND HeaderName = '" & _LocationName & "'")
            'ImageUrl = Emagine.GetDbValue("SELECT ImageUrl FROM qryPageHeaders WHERE PageID = " & PageID & " AND HeaderName = '" & _LocationName & "'")
            If HeaderData.Rows.Count > 0 Then
                ImageUrl = HeaderData.Rows(0).Item("ImageUrl").ToString
                HeaderText = HeaderData.Rows(0).Item("HeaderText").ToString
            End If
            If ImageUrl.Length > 0 Then
                If System.IO.File.Exists(Server.MapPath(ImageUrl)) Or HeaderText.Length > 0 Then Exit Do
            End If
            PageID = Emagine.GetDbValue("SELECT ParentPageID FROM Pages WHERE PageID = " & PageID)
        Loop

        If ImageUrl.Length > 0 Then
            imgImageUrl.ImageUrl = ImageUrl
            imgImageUrl.AlternateText = HeaderText
            imgImageUrl.Visible = True
        ElseIf HeaderText.Length > 0 Then
            ltrHeaderText.Text = "<div id='headerText'>" & HeaderText & "</div>"
            imgImageUrl.Visible = False
        Else
            imgImageUrl.Visible = False
        End If


        'End If
    End Sub
End Class
