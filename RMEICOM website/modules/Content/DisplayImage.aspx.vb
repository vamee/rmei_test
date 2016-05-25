Imports System.Drawing.Imaging

Partial Class modules_ContentTabs_DisplayImage
    Inherits System.Web.UI.Page

    'Function ThumbnailCallback() As Boolean
    '    Return False
    'End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ImageUrl As String = ""
        Dim ImageHeight As Integer = 0
        Dim ImageWidth As Integer = 0

        If Request.QueryString("url") IsNot Nothing Then ImageUrl = Server.UrlDecode(Request.QueryString("url").ToString.Trim)
        If Request.QueryString("h") IsNot Nothing Then ImageHeight = Emagine.GetNumber(Request.QueryString("h").ToString.Trim)
        If Request.QueryString("w") IsNot Nothing Then ImageWidth = Emagine.GetNumber(Request.QueryString("w").ToString.Trim)

        'If ImageUrl.Length > 0 Then
        If Not System.IO.File.Exists(Server.MapPath(ImageUrl)) Then
            ImageUrl = GlobalVariables.GetValue("NoPhotoImageUrl")
        End If

        If System.IO.File.Exists(Server.MapPath(ImageUrl)) Then
            Dim FileExtension As String = Emagine.GetFileExtension(ImageUrl)
            Dim MyContentType As String = ""
            Dim MyImageFormat As ImageFormat

            Select Case FileExtension
                Case ".jpg", ".jpeg"
                    MyContentType = "image/jpeg"
                    MyImageFormat = ImageFormat.Jpeg
                Case ".png"
                    MyContentType = "image/png"
                    MyImageFormat = ImageFormat.Png
                Case Else
                    MyContentType = "image/gif"
                    MyImageFormat = ImageFormat.Gif
            End Select

            Dim FullSizeImage As System.Drawing.Image
            FullSizeImage = System.Drawing.Image.FromFile(Server.MapPath(ImageUrl))
            FullSizeImage.RotateFlip(Drawing.RotateFlipType.Rotate180FlipNone)
            FullSizeImage.RotateFlip(Drawing.RotateFlipType.Rotate180FlipNone)

            If ImageWidth = 0 And (ImageHeight < FullSizeImage.Height) Then
                ImageWidth = Math.Round((ImageHeight / FullSizeImage.Height) * FullSizeImage.Width)

            ElseIf ImageHeight = 0 And (ImageWidth < FullSizeImage.Width) Then
                ImageHeight = Math.Round((ImageWidth / FullSizeImage.Width) * FullSizeImage.Height)

            ElseIf (ImageWidth > 0 And ImageWidth <= FullSizeImage.Width) And (ImageHeight > 0 And ImageHeight <= FullSizeImage.Height) Then
                'Don't do nuthin
            Else
                'Set the dimensions to the same size as the original image
                ImageWidth = FullSizeImage.Width
                ImageHeight = FullSizeImage.Height
            End If

            'Dim DummyCallBack As System.Drawing.Image.GetThumbnailImageAbort
            'DummyCallBack = New System.Drawing.Image.GetThumbnailImageAbort(AddressOf ThumbnailCallback)
            Response.Clear()

            If ImageWidth = FullSizeImage.Width And ImageHeight = ImageHeight Then
                'If dimensions are same as full sized image, output the same format as the requested image
                Response.ContentType = MyContentType
                FullSizeImage.Save(Response.OutputStream, MyImageFormat)
            Else
                'If image is to be resized, output as JPEG. It looks much better than other formats
                Try
                    Response.ContentType = "image/jpeg"
                    FullSizeImage.GetThumbnailImage(ImageWidth, ImageHeight, Nothing, IntPtr.Zero).Save(Response.OutputStream, ImageFormat.Jpeg) ' MyImageFormat)
                Catch ex As Exception
                    Emagine.LogError(ex)
                End Try

            End If

            FullSizeImage.Dispose()

        End If

        'End If

        Response.End()
    End Sub
End Class
