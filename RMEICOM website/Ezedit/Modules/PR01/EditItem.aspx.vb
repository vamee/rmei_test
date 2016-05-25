Imports PeterBlum.VAM

Partial Class Ezedit_Modules_PR01_EditArticle
    Inherits System.Web.UI.Page

    Dim ItemID As Integer = 0
    Dim CategoryID As Integer = 0
    Dim ModuleTypeID As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ItemID = CInt(Request("ItemID"))
        CategoryID = CInt(Request("CategoryID"))


        If Not IsPostBack Then
            Me.BindControls()

            If ItemID <> 0 Then
                Me.Edit(ItemID)
                Me.DisplayModuleTypeFields()
            Else
                Me.Add(CategoryID)
            End If
            'ViewState("CategoryID") = CInt(Request("CategoryID"))
        Else
            ModuleTypeID = ddlModuleTypeID.SelectedValue
            DisplayModuleTypeFields()
        End If

        'csvPageKey.ServerCondition = New ServerConditionEventHandler(AddressOf ValidatePageKey)
    End Sub

    Sub Edit(ByVal intArticleId As Integer)
        Dim Article As New PR01
        Article = PR01.GetArticle(intArticleId)
        ModuleTypeID = Article.ModuleTypeID

        ddlCategoryId.SelectedValue = Article.CategoryId
        ddlModuleTypeID.SelectedValue = Article.ModuleTypeID
        txtSummary.Text = Article.ArticleSummary
        txtURL.Text = Article.ArticleURL
        ddlResourceCategory.SelectedValue = Article.ResourceCategory
        ContentEditor.EditorContent = Article.ArticleText
        pdpDate.Text = Article.DisplayDate
        txtResourceName.Text = Article.ResourceName
        'txtResourcePageKey.Text = Article.ResourcePageKey
        txtKeywords.Text = Article.ResourceKeywords
        cbxIsEnabled.Checked = Article.IsEnabled
        cbxPublishToRss.Checked = Article.PublishToRss
        txtRssAuthor.Text = Article.RssAuthor
        txtRssDescription.Text = Article.RssDescription
        If Article.PublishToRss Then pnlRssFeed.Visible = True

        If Len(Article.FileName) > 0 Then
            lblFile.Text = "<br>"
            lblFile.Text += "<b>Current File:</b> "
            lblFile.Text += "<a href='" & Article.FileName & "' target='_blank' class='main'>"
            lblFile.Text += Article.FileName
            lblFile.Text += "</a>"
        End If

        If Len(Article.ImageURL) > 0 Then
            lblImageURL.Text = "<br>"
            lblImageURL.Text += "<b>Current Image:</b> "
            lblImageURL.Text += "<img src='" & Article.ImageURL & "' border='0' align='top'>"
        End If

        ViewState("CategoryID") = Article.CategoryId

        litPageTitle.Text = "<span class='PageTitle'><a href='../Default.aspx?ModuleKey=PR01' class='pageTitle'>Press Releases</a> > <a href='ItemList.aspx?CategoryID=" & Article.CategoryId & "' class='pageTitle'>" & Article.CategoryName & "</a> > Edit Article</span>"
    End Sub

    Sub Add(ByVal intCategoryId As Integer)
        ddlCategoryId.SelectedValue = intCategoryId
        pnlRssFeed.Visible = False

        litPageTitle.Text = "<span class='PageTitle'><a href='../Default.aspx?ModuleKey=PR01' class='pageTitle'>Press Releases</a> > <a href='ItemList.aspx?CategoryID=" & intCategoryId & "' class='pageTitle'>" & ModuleCategory.GetModuleCategoryName(intCategoryId) & "</a> > Add New Article</span>"
    End Sub

    Sub BindControls()
        Me.BindCategoryIdDDL()
        Me.BindModuleTypes()
        Me.BindResourceCategoryDDL()
    End Sub

    Sub BindCategoryIdDDL()
        ddlCategoryId.DataSource = ModuleCategory.GetModuleCategories("PR01")
        ddlCategoryId.DataTextField = "CategoryName"
        ddlCategoryId.DataValueField = "CategoryId"
        ddlCategoryId.DataBind()
    End Sub

    Sub BindModuleTypes()
        ddlModuleTypeID.Items.Add(New ListItem("<-- Please Choose -->", 0))
        ddlModuleTypeID.AppendDataBoundItems = True
	Dim Rs As System.Data.SqlClient.SqlDataReader = Modules.GetModuleTypes("PR01")
        ddlModuleTypeID.DataSource = Rs
        ddlModuleTypeID.DataTextField = "ModuleType"
        ddlModuleTypeID.DataValueField = "ModuleTypeID"
        ddlModuleTypeID.DataBind()
    End Sub

    Sub DisplayModuleTypeFields()
        Select Case ModuleTypeID
            Case 1
                pnlContent.Visible = True
            Case 2
                pnlExternalURL.Visible = True
            Case 3
                pnlFile.Visible = True

        End Select
    End Sub


    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        PeterBlum.VAM.Globals.Page.Validate()

        If PeterBlum.VAM.Globals.Page.IsValid Then

            Dim ArticleID = ItemID
            Dim NewCategoryID = Emagine.GetNumber(ddlCategoryId.SelectedValue)
            Dim OldCategoryID As Integer = 0
            Dim Result As Boolean = False

            Dim Article As New PR01
            If ArticleID > 0 Then
                Article = PR01.GetArticle(ArticleID)
                OldCategoryID = Article.CategoryID
            Else
                Article.ImageURL = ""
            End If

            Article.CategoryId = NewCategoryID
            Article.ArticleSummary = txtSummary.Text
            If Len(ContentEditor.EditorContent) > 0 Then
                Article.ArticleText = ContentEditor.EditorContent
            Else
                Article.ArticleText = ""
            End If

            If txtURL.Text.Length > 0 Then
                Article.ArticleURL = txtURL.Text
            Else
                Article.ArticleURL = ""
            End If
            Article.IsEnabled = cbxIsEnabled.Checked

            If uplImageURL.HasFile Then
                Dim VirtualFilePath2 As String = GlobalVariables.GetValue("VirtualDocumentUploadPath") & Session("EzEditLanguageName") & "/" & uplImageURL.FileName
                Dim AbsoluteFilePath2 As String = Server.MapPath(VirtualFilePath2)
                Article.ImageURL = VirtualFilePath2
                uplImageURL.PostedFile.SaveAs(AbsoluteFilePath2)
            ElseIf ModuleTypeID <> 3 Then
                Article.ImageURL = ""
            End If

            Article.DisplayDate = pdpDate.Text
            If uplFile.HasFile Then
                Dim VirtualFilePath As String = GlobalVariables.GetValue("VirtualDocumentUploadPath") & Session("EzEditLanguageName") & "/" & uplFile.FileName
                Dim AbsoluteFilePath As String = Server.MapPath(VirtualFilePath)
                Article.FileName = VirtualFilePath
                uplFile.PostedFile.SaveAs(AbsoluteFilePath)
            ElseIf ModuleTypeID <> 3 Then
                Article.FileName = ""
            End If

            Dim Resource As New Resources.Resource
            Resource.ResourceId = Article.ResourceId
            Resource.ModuleTypeID = ddlModuleTypeID.SelectedValue
            Resource.ResourceName = txtResourceName.Text
            If txtResourceCategory.Text.Length > 0 Then
                Resource.ResourceCategory = txtResourceCategory.Text
            Else
                Resource.ResourceCategory = ddlResourceCategory.SelectedValue
            End If
            Resource.ResourceType = "PR01"
            'Resource.ResourcePageKey = txtResourcePageKey.Text
            Resource.ResourcePageKey = ""
            Resource.ResourceKeywords = txtKeywords.Text
            Resource.PublishToRss = cbxPublishToRss.Checked
            Resource.RssAuthor = txtRssAuthor.Text
            Resource.RssDescription = txtRssDescription.Text

            If NewCategoryID <> OldCategoryID Then Article.SortOrder = PR01.GetMaxSortOrder(NewCategoryID) + 1

            If Emagine.GetNumber(Article.ArticleId) > 0 Then
                'Response.Write(Article.ArticleId & "<br>")
                'Response.Write(Article.CategoryID & "<br>")
                'Response.Write(Article.ArticleSummary & "<br>")
                'Response.Write(Article.ArticleText & "<br>")
                'Response.Write(Article.IsEnabled & "<br>")
                'Response.End()
                If Article.UpdateArticle(Article) Then
                    If Resources.Resource.UpdateResource(Resource) Then
                        If NewCategoryID <> OldCategoryID Then PR01.ResetSortOrder(OldCategoryID)
                        Session("Alert") = "The article has been updated successfully."
                        Result = True
                    End If
                End If
            Else
                Article.ResourceId = Emagine.GetUniqueID
                Article.ArticleId = Article.AddArticle(Article)

                If Article.ArticleId > 0 Then
                    Resource.ResourceId = Article.ResourceId
                    Result = Resources.Resource.AddResource(Resource)
                    Session("Alert") = "The article has been added successfully."
                End If
            End If

            If Result = True Then
                Response.Redirect("ItemList.aspx?CategoryId=" & Article.CategoryID)
                'lblAlert.Text = "OK"
            Else
                lblAlert.Text = "An Error has Occurred."
            End If
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("ItemList.aspx?CategoryID=" & CategoryID)
    End Sub

    'Protected Sub txtArticleText_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtArticleText.Load
    '   litEditorInstance.Text = Emagine.SetEditorInstance("txtArticleText", "")
    'End Sub

    Sub BindResourceCategoryDDL()
        ddlResourceCategory.DataSource = Resources.GetResourceCategories("PR01")
        ddlResourceCategory.DataTextField = "ResourceCategory"
        ddlResourceCategory.DataValueField = "ResourceCategory"
        ddlResourceCategory.DataBind()
    End Sub

    Protected Sub cbxPublishToRss_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxPublishToRss.CheckedChanged
        If cbxPublishToRss.Checked Then
            pnlRssFeed.Visible = True
        Else
            pnlRssFeed.Visible = False
        End If
    End Sub
End Class
