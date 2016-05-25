
Partial Class modules_Content_Content
    Inherits System.Web.UI.UserControl

    Dim _PageID As Integer
    Dim _ContentName As String = ""

    Public Property PageID() As Integer
        Get
            Return _PageID
        End Get
        Set(ByVal Value As Integer)
            _PageID = Value
        End Set
    End Property

    Public Property ContentName() As String
        Get
            Return _ContentName
        End Get
        Set(ByVal value As String)
            _ContentName = value
        End Set
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If CInt(PageID) = 0 Then PageID = Session("PageID")
        Dim ContentID As Integer = CInt(Request("ContentID"))
        Dim Content As String = ""



        If _ContentName.Length > 0 Then
            Dim MyContent As Content01 = Content01.GetContent(_ContentName)
            Content = MyContent.Content

        ElseIf Len(Session("Content")) > 0 And (PageID = Emagine.GetNumber(Request("PageID"))) Then
            Content = Session("Content")
            Session("Content") = ""

        ElseIf ContentID > 0 And (PageID = Emagine.GetNumber(Request("PageID"))) Then
            Dim MyContent As Content01 = Content01.GetContent(ContentID)
            Content = MyContent.Content

        ElseIf ContentID > 0 And PageID = Emagine.GetNumber(Emagine.GetDbValue("SELECT ForeignKey FROM Content WHERE ContentID = " & ContentID)) Then
            Dim MyContent As Content01 = Content01.GetContent(ContentID)
            Content = MyContent.Content

        Else
            Dim MyContent As Content01 = Content01.GetContent(Pages01.GetContentID(PageID))
            Content = MyContent.Content

        End If

        ltrContent.Text = Content
    End Sub
End Class
