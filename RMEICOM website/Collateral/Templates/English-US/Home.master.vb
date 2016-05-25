
Partial Class templates_1_Home
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim FoundMatch As Boolean = False
        Dim StylesheetUrls As String = ""


        If Request.ServerVariables("HTTP_USER_AGENT") IsNot Nothing Then
            Dim BrowserInfo As String = Request.ServerVariables("HTTP_USER_AGENT")
            If BrowserInfo.IndexOf("MSIE") Then
                Dim aryInfo As Array = BrowserInfo.Split(";")
                For i As Integer = 0 To aryInfo.GetUpperBound(0)
                    If aryInfo(i).ToString.IndexOf("MSIE") > -1 Then
                        Dim BrowserVersion As String = aryInfo(i).ToString.Replace("MSIE", "").Trim
                        BrowserVersion = BrowserVersion.Substring(0, BrowserVersion.IndexOf("."))
                        If BrowserVersion = "6" Then
                            FoundMatch = True
                        End If
                    End If
                Next
            End If
        End If


        If FoundMatch Then
            StylesheetUrls = "/Collateral/Templates/English-US/styles.css||/Collateral/Templates/English-US/styles-IE-fix.css"
        Else
            StylesheetUrls = "/Collateral/Templates/English-US/styles.css"
        End If

        If StylesheetUrls.Length > 0 Then
            Dim aryUrls As Array = StylesheetUrls.Split("||")
            For i As Integer = 0 To aryUrls.GetUpperBound(0)
                Dim StyleSheetLink As New HtmlLink
                StyleSheetLink.Attributes("type") = "text/css"
                StyleSheetLink.Attributes("rel") = "stylesheet"
                StyleSheetLink.Href = aryUrls(i)
                Page.Header.Controls.Add(StyleSheetLink)
            Next
        End If
    End Sub

End Class

