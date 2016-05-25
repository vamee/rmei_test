
Partial Class Ezedit_Modules_PR01_ArticleList
    Inherits System.Web.UI.Page

    Dim _CategoryID As Integer = 0

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Request("CategoryID") IsNot Nothing Then _CategoryID = Emagine.GetNumber(Request("CategoryID"))
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Dim intCategoryId As Integer = Request("CategoryId")

        If Not IsPostBack Then

            lblPageTitle.Text = "> " & ModuleCategory.GetModuleCategoryName(_CategoryID)


        End If
        'Me.BindRpArticles(intCategoryId)
        lblAlert.Text = Session("Alert")
        Session("Alert") = ""

    End Sub

    Protected Sub ddlSortOrder_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ddlSortOrder As DropDownList = sender
        Dim Row As GridViewRow = ddlSortOrder.Parent.Parent
        Dim btnEdit As ImageButton = Row.FindControl("btnEditItem")
        Dim hdnSortOrder As HiddenField = Row.FindControl("hdnSortOrder")
        Dim ItemID As Integer = Emagine.GetNumber(btnEdit.CommandArgument)
        Dim OldSortOrder As Integer = Emagine.GetNumber(hdnSortOrder.Value)
        Dim NewSortOrder As Integer = Emagine.GetNumber(ddlSortOrder.SelectedValue)
        Dim Sql As String = ""

        If OldSortOrder > NewSortOrder Then
            Sql = "UPDATE Articles SET SortOrder = SortOrder + 1 WHERE CategoryID = " & _CategoryID & " AND ArticleID <> " & ItemID & " AND SortOrder >= " & NewSortOrder & " AND SortOrder < " & OldSortOrder
        Else
            Sql = "UPDATE Articles SET SortOrder = SortOrder - 1 WHERE CategoryID = " & _CategoryID & " AND  ArticleID <> " & ItemID & " AND SortOrder <= " & NewSortOrder & " AND SortOrder > " & OldSortOrder
        End If

        Emagine.ExecuteSQL(Sql)

        Emagine.ExecuteSQL("UPDATE Articles SET SortOrder = " & NewSortOrder & " WHERE ArticleID = " & ItemID)

        Me.ResetSortOrder()

        gdvList.DataBind()
    End Sub

    Sub ResetSortOrder()
        Dim DataTable As DataTable = Emagine.GetDataTable("SELECT ArticleID FROM Articles WHERE CategoryID = " & _CategoryID & " ORDER BY SortOrder")
        For i As Integer = 0 To DataTable.Rows.Count - 1
            Dim Sql As String = "UPDATE Articles SET SortOrder = " & (i + 1) & " WHERE ArticleID = " & DataTable.Rows(i).Item("ArticleID")
            Emagine.ExecuteSQL(Sql)
        Next
    End Sub

    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        Me.DisplayEditPanel(0)
    End Sub

    Protected Sub btnEditItem_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Button As ImageButton = sender
        Dim ItemID As Integer = Button.CommandArgument

        Me.DisplayEditPanel(ItemID)
    End Sub

    Protected Sub btnDeleteItem_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Button As ImageButton = sender
        Dim ItemID As Integer = Button.CommandArgument
        If ItemID > 0 Then
            Dim Sql As String = "sp_PR01_DeleteArticle"
            Dim Command As New System.Data.SqlClient.SqlCommand
            Command.CommandType = CommandType.StoredProcedure
            Command.Parameters.AddWithValue("@ArticleID", ItemID)

            If Emagine.ExecuteSQL(Sql, Command) Then
                Me.ResetSortOrder()
                gdvList.DataBind()
                lblAlert.Text = "The item has been removed successfully."
            Else
                lblAlert.Text = "An error occurred while attempting to delete this item."
            End If
        End If
    End Sub

    Protected Sub cbxIsEnabled_CheckChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cbxIsEnabled As CheckBox = sender
        Dim btnEdit As ImageButton = cbxIsEnabled.Parent.Parent.FindControl("btnEditItem")
        Dim ItemID As Integer = Emagine.GetNumber(btnEdit.CommandArgument)
        Dim EnabledText As String = "enabled"

        If Not cbxIsEnabled.Checked Then EnabledText = "disabled"

        If ItemID > 0 Then
            Dim MyArticle As PR01 = PR01.GetArticle(ItemID)

            If Emagine.ExecuteSQL("UPDATE Resources SET IsEnabled = '" & cbxIsEnabled.Checked & "' WHERE ResourceID = '" & MyArticle.ResourceID & "'") Then
                lblAlert.Text = "The item has been " & EnabledText & " successfully."
                Me.DisplayListPanel()
            Else
                lblAlert.Text = "An error occurred."
            End If
        End If
    End Sub

    Sub DisplayListPanel()
        gdvList.DataBind()
        pnlList.Visible = True
        pnlEdit.Visible = False
    End Sub

    Sub DisplayEditPanel(ByVal intArticleID As Integer)
        Dim MyArticle As New PR01

        If intArticleID > 0 Then
            MyArticle = PR01.GetArticle(intArticleID)
            ddlModuleCategoryID.SelectedIndex = -1
            For Each Item As ListItem In ddlModuleCategoryID.Items
                If Item.Value = MyArticle.CategoryID Then
                    Item.Selected = True
                    Exit For
                End If
            Next
            ddlModuleTypeID.SelectedIndex = -1
            For Each Item As ListItem In ddlModuleTypeID.Items
                If Item.Value = MyArticle.ModuleTypeID Then
                    Item.Selected = True
                    Exit For
                End If
            Next

            txtSummary.Text = MyArticle.ArticleSummary
            txtURL.Text = MyArticle.ArticleURL
            'ddlResourceCategory.SelectedValue = MyArticle.ResourceCategory
            ContentEditor.EditorContent = MyArticle.ArticleText
            pdpDate.Text = String.Format("{0:d}", CDate(MyArticle.DisplayDate.ToString))
            txtResourceName.Text = MyArticle.ResourceName
            txtKeywords.Text = MyArticle.ResourceKeywords
            cbxPublishToRss.Checked = MyArticle.PublishToRss
            txtRssAuthor.Text = MyArticle.RssAuthor
            txtRssDescription.Text = MyArticle.RssDescription

            If String.Format("{0:d}", MyArticle.DisplayStartDate) <> "1/1/1900" Then txtDisplayStartDate.Text = String.Format("{0:d}", MyArticle.DisplayStartDate)
            If String.Format("{0:d}", MyArticle.DisplayEndDate) <> "1/1/1900" Then txtDisplayEndDate.Text = String.Format("{0:d}", MyArticle.DisplayEndDate)
            If MyArticle.PublishToRss Then pnlRssFeed.Visible = True
            pnlEditFields.Visible = True
            hdnItemID.Value = MyArticle.ArticleID

            If MyArticle.ModuleTypeID = 1 Then
                pnlContent.Visible = True
            ElseIf MyArticle.ModuleTypeID = 2 Then
                pnlExternalURL.Visible = True
            ElseIf MyArticle.ModuleTypeID = 3 Then
                pnlFile.Visible = True
                lblFile.Text = "<a href='" & MyArticle.FileName.Replace("~", "") & "' target='_blank'>" & MyArticle.FileName.Replace("~", "") & "</a>"
            End If
        Else
            Me.ResetEditForm()
        End If
        pnlList.Visible = False
        pnlEdit.Visible = True
    End Sub

    Sub ResetEditForm()
        ddlModuleCategoryID.SelectedIndex = -1
        For Each Item As ListItem In ddlModuleCategoryID.Items
            If Item.Value = _CategoryID Then
                Item.Selected = True
                Exit For
            End If
        Next
        ddlModuleTypeID.SelectedIndex = -1
        txtSummary.Text = ""
        txtURL.Text = ""
        'ddlResourceCategory.SelectedValue = MyArticle.ResourceCategory
        ContentEditor.EditorContent = ""
        pdpDate.Text = ""
        txtResourceName.Text = ""
        txtKeywords.Text = ""
        lblFile.Text = ""
        cbxPublishToRss.Checked = False
        txtRssAuthor.Text = ""
        txtRssDescription.Text = ""
        txtDisplayStartDate.Text = ""
        txtDisplayEndDate.Text = ""
        pnlRssFeed.Visible = False
        pnlEditFields.Visible = False
        hdnItemID.Value = 0
    End Sub

    Protected Sub gdvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ItemID As Integer = e.Row.DataItem("ArticleID")
            Dim IsEnabled As Boolean = e.Row.DataItem("IsEnabled")
            Dim SortOrder As Integer = e.Row.DataItem("SortOrder")
            Dim ModuleType As String = e.Row.DataItem("ModuleType").ToString
            Dim ArticleUrl As String = e.Row.DataItem("ArticleUrl")
            Dim FileName As String = e.Row.DataItem("FileName")
            Dim DisplayStartDate As Date = e.Row.DataItem("DisplayStartDate").ToString
            Dim DisplayEndDate As Date = e.Row.DataItem("DisplayEndDate").ToString

            Dim lblStatus As Label = e.Row.FindControl("lblStatus")
            Dim lblDisplayDates As Label = e.Row.FindControl("lblDisplayDates")
            Dim hypModuleType As HyperLink = e.Row.FindControl("hypModuleType")
            Dim cbxIsEnabled As CheckBox = e.Row.FindControl("cbxIsEnabled")
            Dim hdnSortOrder As HiddenField = e.Row.FindControl("hdnSortOrder")
            Dim ddlSortOrder As DropDownList = e.Row.FindControl("ddlSortOrder")
            Dim btnEditItem As ImageButton = e.Row.FindControl("btnEditItem")
            Dim btnDeleteItem As ImageButton = e.Row.FindControl("btnDeleteItem")

            Select Case ModuleType
                Case "Content"
                    hypModuleType.ImageUrl = "/App_Themes/EzEdit/images/page_white.png"
                    hypModuleType.ToolTip = ModuleType
                Case "External Link"
                    hypModuleType.ImageUrl = "/App_Themes/EzEdit/images/world_go.png"
                    hypModuleType.ToolTip = ModuleType & ": " & ArticleUrl
                    hypModuleType.Target = "_blank"
                    hypModuleType.NavigateUrl = ArticleUrl
                Case "File Download"
                    hypModuleType.ImageUrl = "/App_Themes/EzEdit/images/disk.png"
                    hypModuleType.ToolTip = ModuleType & ": " & Emagine.FormatFileName(FileName)
                    hypModuleType.Target = "_blank"
                    hypModuleType.NavigateUrl = FileName.Replace("~", "")
            End Select

            If Not IsEnabled Then
                lblStatus.Text = "DISABLED"
                lblStatus.BackColor = Drawing.Color.Red
                lblStatus.ForeColor = Drawing.Color.White
                lblStatus.ToolTip = "Check the 'Enabled' box to enable this item."

            Else
                If DisplayStartDate.ToString.Length > 0 And DisplayEndDate.ToString.Length > 0 Then
                    If String.Format("{0:d}", CDate(DisplayStartDate)) = "1/1/1900" And String.Format("{0:d}", CDate(DisplayEndDate)) = "1/1/1900" Then
                        lblStatus.Text = "CURRENT"
                        lblStatus.BackColor = Drawing.Color.Green
                        lblStatus.ForeColor = Drawing.Color.White
                        lblStatus.ToolTip = "No Expiration"

                    ElseIf CDate(DisplayStartDate) > Date.Now Then
                        lblStatus.Text = "PENDING"
                        lblStatus.BackColor = Drawing.Color.Yellow
                        lblStatus.ForeColor = Drawing.Color.Black
                        lblStatus.ToolTip = String.Format("{0:d}", DisplayStartDate) & "-" & String.Format("{0:d}", DisplayEndDate)

                    ElseIf CDate(DisplayEndDate) < Date.Now Then
                        lblStatus.Text = "EXPIRED"
                        lblStatus.BackColor = Drawing.Color.Black
                        lblStatus.ForeColor = Drawing.Color.White
                        lblStatus.ToolTip = String.Format("{0:d}", DisplayStartDate) & "-" & String.Format("{0:d}", DisplayEndDate)

                    ElseIf CDate(DisplayStartDate) <= Date.Now And CDate(DisplayEndDate) >= Date.Now Then
                        lblStatus.Text = "CURRENT"
                        lblStatus.BackColor = Drawing.Color.Green
                        lblStatus.ForeColor = Drawing.Color.White
                        lblStatus.ToolTip = String.Format("{0:d}", DisplayStartDate) & "-" & String.Format("{0:d}", DisplayEndDate)
                    End If
                End If
            End If

            hdnSortOrder.Value = SortOrder
            Me.BindSortOrder(ddlSortOrder, SortOrder)
            cbxIsEnabled.Checked = IsEnabled
            btnEditItem.CommandArgument = ItemID
            btnDeleteItem.CommandArgument = ItemID
        End If
    End Sub

    Sub BindSortOrder(ByVal ddlSortOrder As DropDownList, ByVal intSortOrder As Integer)
        Dim MaxSortOrder As Integer = Emagine.GetDbValue("SELECT COUNT(*) As MaxSortOrder FROM Articles WHERE CategoryID = " & _CategoryID)

        For i As Integer = 1 To MaxSortOrder
            Dim ListItem As New ListItem(i.ToString, i.ToString)
            If i = intSortOrder Then ListItem.Selected = True
            ddlSortOrder.Items.Add(ListItem)
        Next
    End Sub

    Protected Sub ddlModuleCategoryID_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlModuleCategoryID.Load
        If Not Page.IsPostBack Then
            ddlModuleCategoryID.DataSource = ModuleCategory.GetModuleCategories("PR01")
            ddlModuleCategoryID.DataTextField = "CategoryName"
            ddlModuleCategoryID.DataValueField = "CategoryID"
            ddlModuleCategoryID.DataBind()
        End If
    End Sub

    Protected Sub ddlModuleTypeID_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlModuleTypeID.Load
        If Not Page.IsPostBack Then
            ddlModuleTypeID.DataSource = Modules.GetModuleTypes("PR01")
            ddlModuleTypeID.DataTextField = "ModuleType"
            ddlModuleTypeID.DataValueField = "ModuleTypeID"
            ddlModuleTypeID.DataBind()
        End If
    End Sub

    Protected Sub ddlModuleTypeID_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlModuleTypeID.SelectedIndexChanged
        Select Case Emagine.GetNumber(ddlModuleTypeID.SelectedValue.ToString)
            Case 1
                pnlEditFields.Visible = True
                pnlContent.Visible = True
                pnlExternalURL.Visible = False
                pnlFile.Visible = False
            Case 2
                pnlEditFields.Visible = True
                pnlContent.Visible = False
                pnlExternalURL.Visible = True
                pnlFile.Visible = False
            Case 3
                pnlEditFields.Visible = True
                pnlContent.Visible = False
                pnlExternalURL.Visible = False
                pnlFile.Visible = True
            Case Else
                pnlEditFields.Visible = False
        End Select
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DisplayListPanel()
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        PeterBlum.VAM.Globals.Page.Validate()

        If PeterBlum.VAM.Globals.Page.IsValid Then

            Dim ArticleID = hdnItemID.Value
            Dim NewCategoryID = Emagine.GetNumber(ddlModuleCategoryID.SelectedValue)
            Dim OldCategoryID As Integer = 0
            Dim Result As Boolean = False

            Dim Article As New PR01
            If ArticleID > 0 Then
                Article = PR01.GetArticle(ArticleID)
                OldCategoryID = Article.CategoryID
            Else
                Article.ImageURL = ""
            End If

            Article.CategoryID = NewCategoryID
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

            If uplImageURL.HasFile Then
                Dim VirtualFilePath2 As String = GlobalVariables.GetValue("VirtualDocumentUploadPath") & Session("EzEditLanguageName") & "/" & uplImageURL.FileName
                Dim AbsoluteFilePath2 As String = Server.MapPath(VirtualFilePath2)
                Article.ImageURL = VirtualFilePath2
                uplImageURL.PostedFile.SaveAs(AbsoluteFilePath2)
            ElseIf ddlModuleTypeID.SelectedValue <> 3 Then
                Article.ImageURL = ""
            End If

            Article.DisplayDate = pdpDate.Text
            If uplFile.HasFile Then
                Dim VirtualFilePath As String = GlobalVariables.GetValue("VirtualDocumentUploadPath") & Session("EzEditLanguageName") & "/" & uplFile.FileName
                Dim AbsoluteFilePath As String = Server.MapPath(VirtualFilePath)
                Article.FileName = VirtualFilePath
                uplFile.PostedFile.SaveAs(AbsoluteFilePath)
            ElseIf ddlModuleTypeID.SelectedValue <> 3 Then
                Article.FileName = ""
            End If

            Dim Resource As New Resources.Resource
            Resource.ResourceID = Article.ResourceID
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
            If txtDisplayStartDate.Text.Length > 0 Then Resource.DisplayStartDate = txtDisplayStartDate.Text
            If txtDisplayEndDate.Text.Length > 0 Then Resource.DisplayEndDate = txtDisplayEndDate.Text

            If NewCategoryID <> OldCategoryID Then Article.SortOrder = PR01.GetMaxSortOrder(NewCategoryID) + 1

            If Emagine.GetNumber(Article.ArticleID) > 0 Then
                If PR01.UpdateArticle(Article) Then
                    If Resources.Resource.UpdateResource(Resource) Then
                        If NewCategoryID <> OldCategoryID Then PR01.ResetSortOrder(OldCategoryID)
                        lblAlert.Text = "The article has been updated successfully."
                        Result = True
                    End If
                End If
            Else
                Article.ResourceID = Emagine.GetUniqueID
                Article.ArticleID = PR01.AddArticle(Article)

                If Article.ArticleID > 0 Then
                    Resource.ResourceID = Article.ResourceID
                    Result = Resources.Resource.AddResource(Resource)
                    lblAlert.Text = "The article has been added successfully."
                End If
            End If

            If Result = True Then
                Me.ResetEditForm()
                Me.DisplayListPanel()
            Else
                lblAlert.Text = "An Error has Occurred."
            End If
        End If
    End Sub


End Class
