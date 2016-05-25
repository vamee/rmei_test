
Partial Class modules_Pages01_GetPage
    Inherits System.Web.UI.Page

    Dim PageKey As String = ""
    Public PageID As Integer = 0
    Dim LanguageID As Integer = 0
    Dim PageInfo As New Pages01
    Dim MemberID As String = ""
    Dim ServerName As String = HttpContext.Current.Request.ServerVariables("SERVER_NAME")

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Page.IsPostBack Then
            'Page.EnableViewState = False
        End If

        Dim SQL As String = ""
        Dim PageID As Integer = CInt(Request("PageID"))
        LanguageID = Emagine.GetNumber(Context.Session("LanguageID"))
        If LanguageID = 0 Then LanguageID = Emagine.GetNumber(Emagine.GetDbValue("SELECT LanguageID FROM Domains WHERE DomainName = '" & Request.ServerVariables("SERVER_NAME") & "'"))
        If LanguageID = 0 Then LanguageID = GetDefaultLanguageID(Request.ServerVariables("SERVER_NAME"))
        Session("LanguageID") = LanguageID

        If PageID > 0 Then
            SQL = "SELECT PageKey FROM Pages WHERE PageID = " & PageID
            PageKey = Emagine.GetDbValue(SQL)
        Else
            If Len(Trim(Request("PageKey"))) > 0 Then
                PageKey = Trim(Request("PageKey"))
                PageKey = Right(PageKey, Len(PageKey) - InStrRev(PageKey, "/"))
                PageKey = Emagine.GetDbValue("SELECT PageKey FROM Pages WHERE PageKey = '" & PageKey & "' AND LanguageID = " & LanguageID)
            Else
                PageKey = Emagine.GetPageKey()
            End If
        End If

        
        PageKey = Emagine.GetDbValue("SELECT PageKey FROM Pages WHERE PageKey = '" & PageKey.Replace("'", "''") & "' AND LanguageID = " & LanguageID & " AND StatusID = 20")
        If PageKey.Length = 0 Then
            PageKey = Emagine.GetDbValue("SELECT PageKey FROM Pages WHERE DefaultPage = 1 AND StatusID = 20 AND LanguageID = " & LanguageID)
            If PageKey.Length > 0 Then
                Response.Clear()
                Response.Status = "301 Moved Permanently"
                Response.AddHeader("Location", "/" & PageKey & ".htm")
                Response.End()
            End If

        ElseIf PageKey.Length > 0 And LanguageID > 0 Then
            Me.SetMasterPage(PageKey, LanguageID)
        End If

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        PageID = Emagine.GetNumber(Emagine.GetDbValue("SELECT PageID FROM Pages WHERE PageKey = '" & PageKey & "' AND StatusID = 20 AND LanguageID = " & LanguageID))

        If PageID = 0 Then Emagine.GetNumber(Emagine.GetDbValue("SELECT PageID FROM Pages WHERE DefaultPage= 1 AND StatusID = 20"))
        If PageID > 0 Then
            Me.CheckPermission(PageID)

            'Me.PopulateTemplate(PageID)
            Session("PageID") = PageID
            PageInfo = Pages01.GetPageInfo(PageID)
            If Request.QueryString.HasKeys Then SaveCampaignInfo()

            
            Me.ProcessPage()

        Else
            'Response.Redirect("/")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Page.Master IsNot Nothing Then
            DisplayPageInfo(PageID)
        End If
    End Sub

    Sub CheckPermission(ByVal intPageID As Integer)
        Dim MemberData As DataTable = Emagine.GetDataTable("SELECT * FROM Pages_MemberCategories WHERE PageID = " & intPageID)
        If MemberData.Rows.Count > 0 Then
            Dim MemberCategoryID As Integer = 0
            If Session("MemberCategoryID") IsNot Nothing Then
                MemberCategoryID = Emagine.GetNumber(Session("MemberCategoryID"))
            End If

            Dim IsLoginAllowed As Boolean = False
            For i As Integer = 0 To (MemberData.Rows.Count - 1)
                If MemberData.Rows(i).Item("MemberCategoryID") = MemberCategoryID Then
                    IsLoginAllowed = True
                    Exit For
                End If
            Next

            If Not IsLoginAllowed Then
                Dim LoginPageKey As String = Emagine.GetDbValue("SELECT PageKey FROM Pages WHERE PageID IN (SELECT MembershipFormPageID FROM Pages WHERE PageID = " & intPageID & ")")
                Session("RequestedPageUrl") = Request.RawUrl
                Session("RequestedPageID") = PageID

                If LoginPageKey.Length > 0 Then
                    Response.Redirect("/" & LoginPageKey & ".htm")
                Else
                    'Response.Redirect(GlobalVariables.GetValue("MemberLoginUrl"))
                End If
            End If
        End If
    End Sub

    Sub ProcessPage()
        Session("PageID") = PageID

        If PageInfo.IsSecure And Emagine.GetNumber(Request.ServerVariables("SERVER_PORT_SECURE")) = 0 Then
            If Application("SslUrl") IsNot Nothing Then
                Response.Redirect(Application("SslUrl") & Request.RawUrl.ToString)
            Else
                Response.Redirect("https://" & Request.ServerVariables("SERVER_NAME") & Request.RawUrl.ToString)
            End If
        ElseIf PageInfo.IsSecure = False And Emagine.GetNumber(Request.ServerVariables("SERVER_PORT_SECURE")) = 1 Then
            If Application("BaseUrl") IsNot Nothing Then
                Response.Redirect(Application("BaseUrl") & Request.RawUrl.ToString)
            Else
                Response.Redirect("http://" & Request.ServerVariables("SERVER_NAME") & Request.RawUrl.ToString)
            End If
        End If
    End Sub

    

    'Sub SetMasterPage(ByVal strPageKey As String, ByVal intLanguageID As Integer)
    '    Dim SQL As String = "SELECT FileName FROM qryPageTemplates WHERE PageKey = '" & strPageKey & "' AND LanguageID = " & intLanguageID
    '    Dim Filename As String = ""

    '    Filename = Emagine.GetDbValue(SQL)

    '    If Filename.Length > 0 Then
    '        Page.MasterPageFile = Filename
    '    Else
    '        Response.Write("Template Error")
    '    End If

    'End Sub

    Sub SetMasterPage(ByVal strPageKey As String, ByVal intLanguageID As Integer)
        'Dim SQL As String = "SELECT FileName FROM qryPageTemplates WHERE PageKey = '" & strPageKey & "' AND LanguageID = " & intLanguageID
        Dim ScriptName As String = Request.RawUrl

        Dim Filename As String = ""

        If Filename.Length = 0 Then
            If ScriptName.StartsWith("/print/", StringComparison.OrdinalIgnoreCase) Then
                Dim LanguageName As String = Emagine.GetDbValue("SELECT LanguageName FROM Languages WHERE LanguageID = " & LanguageID)
                Filename = "/Collateral/Templates/" & LanguageName & "/" & GlobalVariables.GetValue("PrinterFriendlyTemplate")
            Else
                Dim SQL As String = "SELECT FileName FROM qryPageTemplates WHERE PageKey = '" & strPageKey & "' AND LanguageID = " & intLanguageID

                Filename = Emagine.GetDbValue(SQL)
            End If
        End If

        If Filename.Length > 0 Then
            Page.MasterPageFile = Filename
        Else
            Response.Write("Template Error")
        End If

    End Sub

    Function GetDefaultLanguageID(ByVal strDomainName As String) As Integer
        Dim SQL As String = "SELECT LanguageID FROM qryLanguages WHERE LOWER(DomainName) = '" & LCase(strDomainName) & "'"

        Dim LanguageID As Integer = Emagine.GetNumber(Emagine.GetDbValue(SQL))

        If LanguageID = 0 Then
            SQL = "SELECT LanguageID FROM qryLanguages WHERE DefaultLanguage = 1"
            LanguageID = Emagine.GetNumber(Emagine.GetDbValue(SQL))
        End If

        Return LanguageID
    End Function

    Sub DisplayPageInfo(ByVal intPageId As Integer)
        Dim ResourceKeywords As String = Resources.Resource.GetResource(Resources.GetResourceID()).ResourceKeywords
        Dim Keywords As String = ""
        If PageInfo.MetaKeywords.Length > 0 Then Keywords = PageInfo.MetaKeywords
        If ResourceKeywords.Length > 0 Then Keywords = Keywords & ResourceKeywords

        Page.Title = PageInfo.TitleTag

        Dim MetaDesc As New HtmlMeta
        MetaDesc.Name = "description"
        MetaDesc.Content = PageInfo.MetaDescription
        Page.Header.Controls.Add(MetaDesc)

        Dim MetaKeywords As New HtmlMeta
        MetaKeywords.Name = "keywords"
        MetaKeywords.Content = Keywords
        Page.Header.Controls.Add(MetaKeywords)

        Dim MetaPageInfo As New HtmlMeta
        MetaPageInfo.Name = "timestamp"
        MetaPageInfo.Content = DateTime.Now
        Page.Header.Controls.Add(MetaPageInfo)

        If Not PageInfo.IsSearchable Then
            Dim MetaRobots As New HtmlMeta
            MetaRobots.Name = "ROBOTS"
            MetaRobots.Content = "NOINDEX, NOFOLLOW"
            Page.Header.Controls.Add(MetaRobots)
        End If

        Me.SetContentType()
    End Sub

    Sub PopulateTemplate(ByVal intPageID As Integer)

        Dim Form As Control = Page.Form

        For Each Control As Control In Form.Controls
            If Control.GetType().ToString = "System.Web.UI.WebControls.PlaceHolder" Then
                Response.Write(Control.GetType().ToString & " - " & Control.ID.ToString & "<br />")
            End If
        Next
        Response.End()
    End Sub

    Sub SaveCampaignInfo()
        Dim CampaignInfo As String = ""
        Dim CampaignData As DataTable = Emagine.GetDataTable("SELECT * FROM CampaignVariables")
        For i As Integer = 0 To (CampaignData.Rows.Count - 1)
            Dim CampaignKey As String = CampaignData.Rows(i).Item("VariableKey")
            If HttpContext.Current.Request.QueryString(CampaignKey) IsNot Nothing Then
                Dim CampaignValue As String = Request.QueryString(CampaignKey)
                If CampaignValue.Length > 0 Then
                    Me.SetCampaignCookie(CampaignKey, CampaignValue)
                End If
            End If
        Next
    End Sub

    Sub SetCampaignCookie(ByVal strCampaignKey As String, ByVal strCampaignValue As String)
        Dim ReadCookie As HttpCookie = HttpContext.Current.Request.Cookies(strCampaignKey)

        If ReadCookie Is Nothing Then
            Dim WriteCookie As New HttpCookie(strCampaignKey)
            WriteCookie.Value = strCampaignValue
            WriteCookie.Expires = DateAdd(DateInterval.Year, 1, Now())
            WriteCookie.Domain = HttpContext.Current.Request.ServerVariables("SERVER_NAME")
            Response.Cookies.Add(WriteCookie)
        End If
    End Sub

    Sub SetContentType()
        Dim FileExtension As String = Emagine.GetFileExtension(Request.RawUrl).Trim
        Dim MimeType As String = Emagine.GetDbValue("SELECT MimeType FROM MimeTypes WHERE FileExtension = '" & FileExtension & "'")
        If MimeType.Length > 0 Then
            'Response.Write(MimeType)
            'Response.Clear()
            Response.ContentType = MimeType
        End If
    End Sub

End Class
