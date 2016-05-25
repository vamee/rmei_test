
Partial Class Ezedit_Modules_Programs_Default
    Inherits System.Web.UI.Page

    Dim _ModuleKey As String = "Programs"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If EzeditUser.HasModulePermissions(Emagine.GetNumber(Session("EzEditUserID")), _ModuleKey) Then
            lblAlert.Text = Session("Alert")
            Session("Alert") = ""
        Else
            pnlList.Visible = False
            lblAlert.Text = "Sorry, you do not have access to this module."
        End If


        lblPageTitle.Text = Modules.GetModuleName(_ModuleKey)
    End Sub

    Sub FormatModulePagesRow(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim PageModuleId As String = DataBinder.Eval(e.Item.DataItem, "PageModuleID")
            Dim PageName As String = DataBinder.Eval(e.Item.DataItem, "PageName").ToString
            Dim ForeignKey As String = DataBinder.Eval(e.Item.DataItem, "ForeignKey")
            Dim ForeignValue As String = DataBinder.Eval(e.Item.DataItem, "ForeignValue")

            Dim str As New StringBuilder
            str.Append("<li>")
            str.Append("<a href='EditModuleProperties.aspx?ModuleKey=" & _ModuleKey & "&PageModuleID=" & PageModuleId & "&ForeignKey=" & ForeignKey & "&ForeignValue=" & ForeignValue & "' class='main'>")
            str.Append(PageName)
            str.Append("</a>")
            str.Append("</li>")

            Dim lbl As Label = CType(e.Item.FindControl("lblDisplayPages"), Label)
            lbl.Text = str.ToString
        End If
    End Sub

    Protected Sub gdvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gdvList.RowCommand
        Select Case e.CommandName
            Case "DeleteCategory"
                Me.DeleteCategory(e.CommandArgument)

            Case "EditCategory"
                Me.DisplayEditPanel(e.CommandArgument)

            Case "ViewChildren"
                Me.EditModuleProperties(e.CommandArgument)
        End Select
    End Sub

    Protected Sub gdvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvList.RowDataBound
        Dim GridView As GridView = sender

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim CategoryID As String = DataBinder.Eval(e.Row.DataItem, "CategoryID")

            Dim rptDisplayPages As Repeater = e.Row.FindControl("rptDisplayPages")
            rptDisplayPages.DataSource = PageModule.GetModulePages(_ModuleKey, "CategoryID", CategoryID)
            rptDisplayPages.DataBind()
        End If
    End Sub

    Sub DeleteCategory(ByVal intCategoryID As Integer)
        ModuleCategory.DeleteModuleCategory(intCategoryID, _ModuleKey)
        gdvList.DataBind()
        lblAlert.Text = "The category has been removed successfully."
    End Sub

    Sub DisplayListPanel()
        pnlList.Visible = True
        pnlEdit.Visible = False

        gdvList.DataBind()
    End Sub

    Sub DisplayEditPanel(ByVal intCategoryID As Integer)
        pnlEdit.Visible = True
        pnlList.Visible = False
        lblAlert.Text = ""

        txtCategoryName.Text = ""
        txtDescription.Text = ""
        hdnCategoryID.Value = 0
        cbxPublishToRss.Checked = False
        txtRssTitle.Text = ""
        txtRssDescription.Text = ""
        txtRssManagingEditor.Text = ""
        hypRssImageUrl.Text = ""
        pnlRssInfo.Visible = False
        txtCategoryName.Focus()
        If _ModuleKey <> "PR01" Then trRssFeed.Visible = False

        If intCategoryID > 0 Then
            Dim Category As ModuleCategory = ModuleCategory.GetModuleCategory(intCategoryID)
            txtCategoryName.Text = Category.CategoryName
            txtDescription.Text = Category.Description
            hdnCategoryID.Value = Category.CategoryID
            cbxPublishToRss.Checked = Category.PublishToRss
            txtRssTitle.Text = Category.RssTitle
            txtRssDescription.Text = Category.RssDescription
            txtRssManagingEditor.Text = Category.RssManagingEditor
            hypRssImageUrl.Text = Category.RssImageUrl
            hypRssImageUrl.NavigateUrl = Category.RssImageUrl
            If Category.PublishToRss Then pnlRssInfo.Visible = True
        End If
    End Sub


    Protected Sub btnAddNew1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddNew1.Click
        Me.DisplayEditPanel(0)
    End Sub

    Protected Sub btnAddNew2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew2.Click
        Me.DisplayEditPanel(0)
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DisplayListPanel()
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Page.IsValid Then
            Dim RssImageUrl As String = ""
            Dim CategoryID As Integer = Emagine.GetNumber(hdnCategoryID.Value)
            Dim Category As New ModuleCategory
            If CategoryID > 0 Then Category = ModuleCategory.GetModuleCategory(CategoryID)
            Category.CategoryID = CategoryID
            Category.ModuleKey = _ModuleKey
            Category.StatusID = Session("EzEditStatusID")
            Category.LanguageID = Session("EzEditLanguageID")
            Category.CategoryName = txtCategoryName.Text
            Category.Description = txtDescription.Text
            Category.PublishToRss = cbxPublishToRss.Checked
            Category.RssTitle = txtRssTitle.Text
            Category.RssDescription = txtRssDescription.Text
            Category.RssManagingEditor = txtRssManagingEditor.Text
            If uplRssImageUrl.HasFile Then
                RssImageUrl = GlobalVariables.GetValue("VirtualImageUploadPath") & Session("EzEditLanguageName") & "/" & uplRssImageUrl.FileName
                uplRssImageUrl.SaveAs(Server.MapPath(RssImageUrl))
                Category.RssImageUrl = RssImageUrl
            End If

            If Category.CategoryID > 0 Then
                If Category.UpdateModuleCategory(Category) Then
                    lblAlert.Text = "The category has been updated successfully."
                    Me.DisplayListPanel()
                Else
                    lblAlert.Text = "An Error has occurred."
                End If
            Else
                If Category.InsertModuleCategory(Category) Then
                    lblAlert.Text = "The category has been added successfully."
                    Me.DisplayListPanel()
                Else
                    lblAlert.Text = "An Error has occurred."
                End If
            End If
        End If
    End Sub

    Sub EditModuleProperties(ByVal intCategoryID As Integer)
        Response.Redirect("ItemList.aspx?CategoryID=" & intCategoryID)
    End Sub

    Protected Sub cbxPublishToRss_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxPublishToRss.CheckedChanged
        If cbxPublishToRss.Checked Then
            pnlRssInfo.Visible = True
        Else
            pnlRssInfo.Visible = False
        End If
    End Sub
End Class
