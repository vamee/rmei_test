
Partial Class Ezedit_UserControls_ContentEditor
    Inherits System.Web.UI.UserControl

    Dim _ContentID As Integer = 0
    Dim _EditorID As String = ""
    Dim _EditorWidth As Integer = 0
    Dim _EditorHeight As Integer = 0
    Dim _EditorContent As String = ""
    Dim _EnableSaveButton As Boolean = False
    Dim _EditorEditable As Boolean = True
    Dim _EditorHasPermission As Boolean = True
    Dim _SuccessMessage As String = ""
    Dim _ErrorMessage As String = ""

    Public Property ContentID() As Integer
        Get
            Return _ContentID
        End Get
        Set(ByVal value As Integer)
            _ContentID = value
        End Set
    End Property

    Public Property EditorID() As String
        Get
            Return _EditorID
        End Get
        Set(ByVal value As String)
            _EditorID = value
        End Set
    End Property

    Public Property EditorWidth() As Integer
        Get
            Return _EditorWidth
        End Get
        Set(ByVal value As Integer)
            _EditorWidth = value
        End Set
    End Property

    Public Property EditorHeight() As Integer
        Get
            Return _EditorHeight
        End Get
        Set(ByVal value As Integer)
            _EditorHeight = value
        End Set
    End Property

    Public Property EditorContent() As String
        Get
            Return RadEditor1.Html
        End Get
        Set(ByVal value As String)
            RadEditor1.Html = value
        End Set
    End Property

    Public WriteOnly Property EnableSaveButton() As Boolean
        Set(ByVal value As Boolean)
            _EnableSaveButton = value
        End Set
    End Property

    Public Property EditorEditable() As Boolean
        Get
            Return _EditorEditable
        End Get
        Set(ByVal value As Boolean)
            _EditorEditable = value
        End Set
    End Property

    Public Property EditorHasPermission() As Boolean
        Get
            Return _EditorHasPermission
        End Get
        Set(ByVal value As Boolean)
            _EditorHasPermission = value
        End Set
    End Property

    Public Property SuccessMessage() As String
        Get
            Return _SuccessMessage
        End Get
        Set(ByVal value As String)
            _SuccessMessage = value
        End Set
    End Property

    Public Property ErrorMessage() As String
        Get
            Return _ErrorMessage
        End Get
        Set(ByVal value As String)
            _ErrorMessage = value
        End Set
    End Property


    Public Delegate Sub SaveButtonClickedEventHandler(ByVal sender As Object, ByVal e As SaveButtonEventArgs)

    Public Event SaveButtonClicked As SaveButtonClickedEventHandler


    Protected Sub RadEditor1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadEditor1.Load
        RadEditor1.Editable = _EditorEditable
        RadEditor1.HasPermission = _EditorHasPermission
        'RadEditor1.Toolbars(0).Tools("SaveContent").IsEnabled = _EnableSaveButton
        RadEditor1.CssFiles = New String(1) {"~/Collateral/Templates/" & Session("EzEditLanguageName") & "/styles.css", "~/Templates/Common/styles.css"}
        'PopulateStyles()
        RadEditor1.Width = EditorWidth
        RadEditor1.Height = EditorHeight

        RadEditor1.MaxImageSize = GlobalVariables.GetValue("MaxImageSize")
        RadEditor1.ImagesPaths = New String(1) {GlobalVariables.GetValue("VirtualImageUploadPath") & Session("EzEditLanguageName"), GlobalVariables.GetValue("VirtualImageUploadPath") & "Common"}
        RadEditor1.DeleteImagesPaths = New String(1) {GlobalVariables.GetValue("VirtualImageUploadPath") & Session("EzEditLanguageName"), GlobalVariables.GetValue("VirtualImageUploadPath") & "Common"}
        RadEditor1.UploadImagesPaths = New String(1) {GlobalVariables.GetValue("VirtualImageUploadPath") & Session("EzEditLanguageName"), GlobalVariables.GetValue("VirtualImageUploadPath") & "Common"}

        RadEditor1.MaxDocumentSize = GlobalVariables.GetValue("MaxDocumentSize")
        RadEditor1.DocumentsPaths = New String(1) {GlobalVariables.GetValue("VirtualDocumentUploadPath") & Session("EzEditLanguageName"), GlobalVariables.GetValue("VirtualDocumentUploadPath") & "Common"}
        RadEditor1.DeleteDocumentsPaths = New String(0) {GlobalVariables.GetValue("VirtualDocumentUploadPath") & Session("EzEditLanguageName")}
        RadEditor1.UploadDocumentsPaths = New String(1) {GlobalVariables.GetValue("VirtualDocumentUploadPath") & Session("EzEditLanguageName"), GlobalVariables.GetValue("VirtualDocumentUploadPath") & "Common"}

        RadEditor1.MediaPaths = New String(1) {GlobalVariables.GetValue("VirtualMediaUploadPath") & Session("EzEditLanguageName"), GlobalVariables.GetValue("VirtualMediaUploadPath") & "Common"}
        RadEditor1.DeleteMediaPaths = New String(0) {GlobalVariables.GetValue("VirtualMediaUploadPath") & Session("EzEditLanguageName")}
        RadEditor1.UploadMediaPaths = New String(1) {GlobalVariables.GetValue("VirtualMediaUploadPath") & Session("EzEditLanguageName"), GlobalVariables.GetValue("VirtualMediaUploadPath") & "Common"}

        RadEditor1.FlashPaths = New String(1) {GlobalVariables.GetValue("VirtualFlashUploadPath") & Session("EzEditLanguageName"), GlobalVariables.GetValue("VirtualFlashUploadPath") & "Common"}
        RadEditor1.DeleteFlashPaths = New String(0) {GlobalVariables.GetValue("VirtualFlashUploadPath") & Session("EzEditLanguageName")}
        RadEditor1.UploadFlashPaths = New String(1) {GlobalVariables.GetValue("VirtualFlashUploadPath") & Session("EzEditLanguageName"), GlobalVariables.GetValue("VirtualFlashUploadPath") & "Common"}
        'Response.Write(EditorContent)
        'RadEditor1.Html = EditorContent
    End Sub

    Protected Sub RadEditor1_SubmitClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadEditor1.SubmitClicked
        Dim EventArgs As New SaveButtonEventArgs
        EventArgs.ContentID = _ContentID
        EventArgs.EditorID = _EditorID
        EventArgs.Content = RadEditor1.Html
        EventArgs.SuccessMessage = _SuccessMessage
        EventArgs.ErrorMessage = _ErrorMessage
        RaiseEvent SaveButtonClicked(Me, EventArgs)
    End Sub

    Sub PopulateStyles()
        Dim File As New System.IO.StreamReader(System.IO.File.Open(Server.MapPath("/Collateral/Templates/Common/styles.css"), IO.FileMode.Open))
        Dim CanRead As Boolean = False

        Do While Not File.EndOfStream
            Dim FileLine As String = File.ReadLine()

            If CanRead = False Then
                If InStr(FileLine, "/* start editor styles */") > 0 Then CanRead = True

            Else
                If FileLine.Length > 0 Then
                    If InStr(FileLine, "/* end editor styles */") > 0 Then
                        Exit Do

                    ElseIf InStr(FileLine, "{", CompareMethod.Text) > 0 Then

                        Dim StyleName As String = FileLine.Substring(0, InStr(FileLine, "{") - 1).Trim
                        'Response.Write(StyleName & "<br>")
                        RadEditor1.CssClasses.Add(StyleName, StyleName)
                    End If
                End If
            End If
        Loop

        File.Close()

    End Sub

End Class

Public Class SaveButtonEventArgs
    Inherits System.EventArgs

    Public ContentID As Integer
    Public EditorID As String
    Public Content As String
    Public SuccessMessage As String
    Public ErrorMessage As String

End Class




