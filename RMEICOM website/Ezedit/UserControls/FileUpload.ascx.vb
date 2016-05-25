Imports System.IO

Partial Class Ezedit_UserControls_FileUpload
    Inherits System.Web.UI.UserControl

    Dim _IsRequired As Boolean = False
    Dim _VirtualFilePath As String = ""
    Dim _AllowedFileExtensions As String = ""
    Dim _Mode As String = ""

    Public WriteOnly Property CssClass() As String
        Set(ByVal value As String)
            uplFileUpload.CssClass = value
        End Set
    End Property

    Public WriteOnly Property Width() As String
        Set(ByVal value As String)
            uplFileUpload.Width = value
        End Set
    End Property

    Public Property IsRequired() As Boolean
        Get
            Return _IsRequired
        End Get
        Set(ByVal value As Boolean)
            _IsRequired = value
            RequiredTextValidator.Enabled = value
        End Set
    End Property

    Public Property AllowedFileExtensions() As String
        Get
            Return _AllowedFileExtensions
        End Get
        Set(ByVal value As String)
            If value.Length > 0 Then
                Dim aryFileExtensions As Array = value.Split(",")
                For i As Integer = 0 To UBound(aryFileExtensions)
                    Dim Item As New PeterBlum.VAM.CompareToStringsItem()
                    Item.Value = Trim(aryFileExtensions(i).ToString())
                    CompareToStringsValidator.Items.Add(Item)
                Next

                CompareToStringsValidator.Enabled = True
                CompareToStringsValidator.SummaryErrorMessage = "Only files with (" & value & ") are allowed."
            End If
        End Set
    End Property

    Public WriteOnly Property RequiredErrorMessage() As String
        Set(ByVal value As String)
            RequiredTextValidator.SummaryErrorMessage = value
        End Set
    End Property

    Public ReadOnly Property PostedFile() As System.Web.HttpPostedFile
        Get
            Return uplFileUpload.PostedFile
        End Get
    End Property

    Public Property VirtualFilePath() As String
        Get
            Return hdnVirtualFilePath.Value
        End Get
        Set(ByVal value As String)
            If value.Length > 0 Then hdnVirtualFilePath.Value = value
        End Set
    End Property

    Public ReadOnly Property HasFile() As Boolean
        Get
            Return uplFileUpload.HasFile
        End Get
    End Property

    Public ReadOnly Property FileName() As String
        Get
            Return uplFileUpload.FileName
        End Get
    End Property

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
            If File.Exists(Server.MapPath(VirtualFilePath)) Then
                hypFileUrl.NavigateUrl = VirtualFilePath
                hypFileUrl.Text = VirtualFilePath

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
        hdnVirtualFilePath.Value = ""
        pnlLink.Visible = False
        btnCancel.Visible = False
    End Sub
End Class
