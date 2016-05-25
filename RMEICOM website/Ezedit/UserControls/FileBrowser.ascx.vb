
Partial Class Ezedit_UserControls_FileBrowser
    Inherits System.Web.UI.UserControl

    Private _DefaultPath As String = ""
    Private _AllowedFileExtensions As String = ""
    Private _IsRequired As Boolean = False
    Private _SummaryErrorMessage As String = ""
    Dim _Mode As String = ""

    Public WriteOnly Property CssClass() As String
        Set(ByVal value As String)
            txtFileName.CssClass = value
        End Set
    End Property

    Public WriteOnly Property Width() As String
        Set(ByVal value As String)
            txtFileName.Width = value
        End Set
    End Property

    Public Property DefaultPath() As String
        Get
            Return _DefaultPath
        End Get
        Set(ByVal value As String)
            _DefaultPath = value
        End Set
    End Property

    Public Property AllowedFileExtensions() As String
        Get
            Return _AllowedFileExtensions
        End Get
        Set(ByVal value As String)
            _AllowedFileExtensions = value
        End Set
    End Property

    Public Property IsRequired() As Boolean
        Get
            Return _IsRequired
        End Get
        Set(ByVal value As Boolean)
            _IsRequired = value
        End Set
    End Property

    Public Property SummaryErrorMessage() As String
        Get
            Return _SummaryErrorMessage
        End Get
        Set(ByVal value As String)
            _SummaryErrorMessage = value
        End Set
    End Property

    Public Property VirtualFilePath() As String
        Get
            Return txtFileName.Text
        End Get
        Set(ByVal value As String)
            txtFileName.Text = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            txtFileName.Text = VirtualFilePath

            btnFileName.OnClientClick = "return ShowFileDialog(""" & txtFileName.ClientID & """, window.document.forms[0]." & txtFileName.ClientID & ".value, """ & DefaultPath & """, """ & AllowedFileExtensions & """);"
        End If

        rtvFileName.Enabled = IsRequired
        rtvFileName.SummaryErrorMessage = SummaryErrorMessage
    End Sub

    Protected Sub ibtnEdit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnEdit.Click
        pnlLink.Visible = False
        pnlUpload.Visible = True

        btnCancel.Visible = True
        _Mode = "Edit"

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        pnlLink.Visible = True
        pnlUpload.Visible = False

        btnCancel.Visible = False
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If VirtualFilePath.Length > 0 And _Mode.Length = 0 Then
            If IO.File.Exists(Server.MapPath(VirtualFilePath)) Then
                hypFileUrl.NavigateUrl = VirtualFilePath
                hypFileUrl.Text = Emagine.FormatFileName(VirtualFilePath)

                pnlLink.Visible = True
                pnlUpload.Visible = False

                If IsRequired Then
                    ibtnDelete.Visible = False
                Else
                    ibtnDelete.Visible = True
                End If

            Else
                pnlLink.Visible = False
                pnlUpload.Visible = True
            End If
        Else
            pnlLink.Visible = False
            pnlUpload.Visible = True
        End If
    End Sub

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        VirtualFilePath = ""
        txtFileName.Text = ""
        pnlLink.Visible = False
        btnCancel.Visible = False
    End Sub
End Class
