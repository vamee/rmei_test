Imports PeterBlum.VAM

Partial Class Ezedit_Modules_Pages01_EditPage
    Inherits System.Web.UI.Page

    Dim PageID As Integer = 0
    Dim ParentPageID As Integer = 0
    Dim StatusID As Integer = 0
    Dim LanguageID As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PageID = Emagine.GetNumber(Request("PageID"))
        ParentPageID = Emagine.GetNumber(Request("ParentPageID"))
        StatusID = Emagine.GetNumber(Session("EzEditStatusID"))
        LanguageID = Emagine.GetNumber(Session("EzEditLanguageID"))

        If Session("EzEditLevelID") = 1 Then trIsDefault.Visible = True

        If PageID > 0 Then
            If EzeditUser.HasPagePermissions(Emagine.GetNumber(Session("EzEditUserID")), PageID) Then
                lblAlert.Text = Session("Alert")
                Session("Alert") = ""
            Else
                Session("Alert") = "You are not allowed to edit this page."
                Response.Redirect("Default.aspx")
            End If
        Else
            If EzeditUser.HasPagePermissions(Emagine.GetNumber(Session("EzEditUserID")), ParentPageID) Then
                lblAlert.Text = Session("Alert")
                Session("Alert") = ""
            Else
                Session("Alert") = "You are not allowed to add a page here."
                Response.Redirect("Default.aspx")
            End If
        End If

        If Not IsPostBack Then
            Me.BindControls(PageID)
            If PageID > 0 Then
                Me.EditPage(PageID)
            Else
                Me.AddPage(ParentPageID)
            End If
        End If

        csvPageKey.ServerCondition = New ServerConditionEventHandler(AddressOf DuplicateKeyCheck)

        lblAlert.Text = Session("Alert")
        Session("Alert") = ""
    End Sub

    Sub EditPage(ByVal intPageId As Integer)
        lblPageTitle.Text = "Edit Page Information"
        Dim Page As Pages01 = Pages01.GetPageInfo(intPageId)
        ddlPageTypeId.SelectedValue = Page.PageTypeId
        Me.ToggleFormFields(ddlPageTypeId.SelectedValue)
        ddlParentPageId.SelectedValue = Page.ParentPageId
        ddlTemplateId.SelectedValue = Page.TemplateId
        txtPageName.Text = Page.PageName
        txtPageAction.Text = Page.PageAction
        txtPageKey.Text = Page.PageKey
        txtMenuName.Text = Page.MenuName
        txtTitleTag.Text = Page.TitleTag
        txtMetaDescription.Text = Page.MetaDescription
        txtMetaKeywords.Text = Page.MetaKeywords
        txtSEOScript.Text = Page.SEOScript
        ddlContentPageId.SelectedValue = Page.ContentPageId
        ddlMembershipFormID.SelectedValue = Page.MembershipFormPageID
        rblDefaultPage.SelectedValue = Page.DefaultPage
        rblHasChildren.SelectedValue = Page.HasChildren
        rblIsPermanent.SelectedValue = Page.IsPermanent
        rblIsHidden.SelectedValue = Page.IsHidden
        rblIsSecure.SelectedValue = Page.IsSecure
        rblIsSearchable.SelectedValue = Page.IsSearchable
        rblDisplaySubMenu.SelectedValue = Page.DisplaySubMenu
        'rblLogImpressions.SelectedValue = Page.LogImpressions
        ddlPageStatus.SelectedValue = Page.StatusId
        Emagine.SelectListBoxValues(lstUserPermissions, "")
        rblMemberPermissions.SelectedIndex = 0

        Dim MemberData As DataTable = Emagine.GetDataTable("SELECT * FROM Pages_MemberCategories WHERE PageID = " & Page.PageID)
        If MemberData.Rows.Count > 0 Then
            cblMemberCategories.Enabled = True
            rblMemberPermissions.SelectedValue = 1
            rblMemberPermissions.Enabled = False
            For Each Item As ListItem In cblMemberCategories.Items
                For i As Integer = 0 To (MemberData.Rows.Count - 1)
                    If Item.Value = MemberData.Rows(i).Item("MemberCategoryID") Then
                        Item.Selected = True
                        Exit For
                    End If
                Next
            Next
            Dim ParentMemberData As DataTable = Emagine.GetDataTable("SELECT * FROM Pages_MemberCategories WHERE PageID = " & Page.ParentPageID)
            If ParentMemberData.Rows.Count > 0 Then
                rblMemberPermissions.SelectedValue = 1
                rblMemberPermissions.Enabled = False
                For Each Item As ListItem In cblMemberCategories.Items
                    Item.Enabled = False
                    For i As Integer = 0 To (ParentMemberData.Rows.Count - 1)
                        If Item.Value = ParentMemberData.Rows(i).Item("MemberCategoryID") Then
                            Item.Enabled = True
                        End If
                    Next
                Next
            Else
                rblMemberPermissions.Enabled = True
            End If
        End If
    End Sub

    Sub AddPage(ByVal intParentId As Integer)
        ddlPageTypeId.SelectedValue = 1
        ddlParentPageId.SelectedValue = intParentId
        lblPageTitle.Text = "Add New Page"
        rblDefaultPage.SelectedValue = False
        rblHasChildren.SelectedValue = False
        rblIsPermanent.SelectedValue = False
        rblIsHidden.SelectedValue = False
        rblIsSecure.SelectedValue = False
        rblIsSearchable.SelectedValue = True
        rblDisplaySubMenu.SelectedValue = True
        rblMemberPermissions.SelectedIndex = 0
        'rblLogImpressions.SelectedValue = False

        If intParentId > 0 Then
            Dim ParentPage As Pages01 = Pages01.GetPageInfo(intParentId)
	    If ParentPage.PageTypeID = 1 Then
	            ddlTemplateId.SelectedValue = ParentPage.TemplateID
	    Else
		    ddlTemplateId.SelectedValue = Emagine.GetDbValue("SELECT TemplateID FROM Templates WHERE IsDefault = 1 AND LanguageID = " & Session("EzEditLanguageID"))
	    End If
            txtPageKey.Text = ParentPage.PageKey
            txtMetaDescription.Text = ParentPage.MetaDescription
            txtMetaKeywords.Text = ParentPage.MetaKeywords
            txtSEOScript.Text = ParentPage.SEOScript
            ddlMembershipFormID.SelectedValue = ParentPage.MembershipFormPageID
            'rblDefaultPage.SelectedValue = ParentPage.DefaultPage
            'rblHasChildren.SelectedValue = ParentPage.HasChildren
            rblIsPermanent.SelectedValue = ParentPage.IsPermanent
            'rblIsHidden.SelectedValue = ParentPage.IsHidden
            rblIsSecure.SelectedValue = ParentPage.IsSecure
            rblIsSearchable.SelectedValue = ParentPage.IsSearchable
            'rblLogImpressions.SelectedValue = Page.LogImpressions
            ddlPageStatus.SelectedValue = ParentPage.StatusID

            Dim MemberData As DataTable = Emagine.GetDataTable("SELECT * FROM Pages_MemberCategories WHERE PageID = " & intParentId)
            If MemberData.Rows.Count > 0 Then
                cblMemberCategories.Enabled = True
                rblMemberPermissions.SelectedValue = 1
                rblMemberPermissions.Enabled = False
                For Each Item As ListItem In cblMemberCategories.Items
                    Item.Enabled = False
                    For i As Integer = 0 To (MemberData.Rows.Count - 1)
                        If Item.Value = MemberData.Rows(i).Item("MemberCategoryID") Then
                            Item.Selected = True
                            Item.Enabled = True
                        End If
                    Next
                Next
            End If
        Else

            Dim DefaultTemplateID As Integer = Emagine.GetDbValue("SELECT TemplateID FROM Templates WHERE IsDefault = 1")
            ddlTemplateId.SelectedIndex = -1
            For Each Item As ListItem In ddlTemplateId.Items
                If Item.Value = DefaultTemplateID Then
                    Item.Selected = True
                    Exit For
                End If
            Next
        End If

    End Sub

    Sub BindControls(ByVal intPageId As Integer)
        Me.BindPageTypeIdDDL()
        Me.BindTemplateIdDDL()
        Me.BindTemplateHeaders()
        Me.BindUserPermissionLST()
        Me.BindPageStatusDDL()
        Me.PopulateMemerCategories()
        ddlParentPageId = Pages01.PopulatePageOptionsDDL(ddlParentPageId, intPageId, 0, 0, Session("EzEditStatusID"), Session("EzEditLanguageID"))
        ddlContentPageId = Pages01.PopulatePageOptionsDDL(ddlContentPageId, intPageId, 0, 0, Session("EzEditStatusID"), Session("EzEditLanguageID"))
        rblDefaultPage = Emagine.PopulateBooleanRBL(rblDefaultPage)
        rblHasChildren = Emagine.PopulateBooleanRBL(rblHasChildren)
        rblIsPermanent = Emagine.PopulateBooleanRBL(rblIsPermanent)
        rblIsHidden = Emagine.PopulateBooleanRBL(rblIsHidden)
        rblIsSecure = Emagine.PopulateBooleanRBL(rblIsSecure)
        rblIsSearchable = Emagine.PopulateBooleanRBL(rblIsSearchable)
        rblDisplaySubMenu = Emagine.PopulateBooleanRBL(rblDisplaySubMenu)
        'rblLogImpressions = Emagine.PopulateBooleanRBL(rblLogImpressions)
    End Sub

    Sub BindPageTypeIdDDL()
        ddlPageTypeId.DataSource = Pages01.GetPageTypes()
        ddlPageTypeId.DataTextField = "PageType"
        ddlPageTypeId.DataValueField = "PageTypeId"
        ddlPageTypeId.DataBind()
    End Sub

    Sub BindTemplateIdDDL()
        ddlTemplateId.DataSource = Pages01.GetTemplates(Session("EzEditLanguageID"))
        ddlTemplateId.DataTextField = "TemplateName"
        ddlTemplateId.DataValueField = "TemplateID"
        ddlTemplateId.DataBind()
    End Sub

    Sub BindTemplateHeaders()
        rptTemplateHeaders.DataSource = Emagine.GetDataTable("SELECT * FROM TemplateHeaders ORDER BY SortOrder")
        rptTemplateHeaders.DataBind()
    End Sub

    Sub BindUserPermissionLST()
        lstUserPermissions.DataSource = EzeditUser.GetAllUsers(CInt(Session("EzEditLanguageID")))
        lstUserPermissions.DataTextField = "UserName"
        lstUserPermissions.DataValueField = "UserId"
        lstUserPermissions.DataBind()

        Dim SQL As String = "SELECT EzUserID FROM PagePermissions WHERE PageID = " & PageID
        Dim Rs As System.Data.SqlClient.SqlDataReader = Emagine.GetDataReader(SQL)
        If Rs.HasRows Then
            Do While Rs.Read
                For Each Item As ListItem In lstUserPermissions.Items
                    If Item.Value = Emagine.GetNumber(Rs(0).ToString) Then
                        Item.Selected = True
                        Exit For
                    End If
                Next
            Loop
        Else
            For Each Item As ListItem In lstUserPermissions.Items
                If Item.Value = Emagine.GetNumber(Session("EzEditUserID")) Then
                    Item.Selected = True
                    Exit For
                End If
            Next
        End If
        Rs.Close()
    End Sub

    Sub BindPageStatusDDL()
        ddlPageStatus.DataSource = Pages01.GetPageStatuses()
        ddlPageStatus.DataTextField = "Status"
        ddlPageStatus.DataValueField = "StatusID"
        If Not IsPostBack Then ddlPageStatus.SelectedValue = 20
        ddlPageStatus.DataBind()
    End Sub

    Protected Sub ddlPageTypeId_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPageTypeId.SelectedIndexChanged
        Me.ToggleFormFields(ddlPageTypeId.SelectedValue)
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If PeterBlum.VAM.Globals.Page.IsValid Then
            Dim ParentPage As New Pages01
            If ddlParentPageId.SelectedValue > 0 Then
                ParentPage = Pages01.GetPageInfo(ddlParentPageId.SelectedValue)
                If Not ParentPage.HasChildren Then Emagine.ExecuteSQL("UPDATE Pages SET HasChildren = 1 WHERE PageID = " & ParentPage.PageID)
            End If

            Dim Page As New Pages01
            Page.PageID = PageID
            Page.LanguageID = LanguageID
            Page.PageTypeID = ddlPageTypeId.SelectedValue
            Page.ParentPageID = ddlParentPageId.SelectedValue
            Page.TemplateID = ddlTemplateId.SelectedValue
            Page.PageName = txtPageName.Text
            Page.PageAction = txtPageAction.Text
            Page.PageKey = txtPageKey.Text
            Page.MenuName = txtMenuName.Text
            Page.TitleTag = txtTitleTag.Text
            Page.MetaDescription = txtMetaDescription.Text
            Page.MetaKeywords = txtMetaKeywords.Text
            Page.SEOScript = txtSEOScript.Text
            Page.ContentPageID = ddlContentPageId.SelectedValue
            Page.MembershipFormPageID = ddlMembershipFormID.SelectedValue
            Page.DefaultPage = rblDefaultPage.SelectedValue
            Page.HasChildren = rblHasChildren.SelectedValue
            Page.IsPermanent = rblIsPermanent.SelectedValue
            Page.IsHidden = rblIsHidden.SelectedValue
            Page.IsSecure = rblIsSecure.SelectedValue
            Page.IsSearchable = rblIsSearchable.SelectedValue
            Page.DisplaySubMenu = rblDisplaySubMenu.SelectedValue
            Page.LogImpressions = 0 'rblLogImpressions.SelectedValue
            Page.StatusID = ddlPageStatus.SelectedValue
            Page.StartDate = "1/1/2006"
            Page.EndDate = "12/31/2020"
            Page.CreatedBy = Session("EzEditName")
            Page.UpdatedBy = Session("EzEditName")
            Page.LastUpdated = Date.Now()

            If PageID = 0 Then
                PageID = Pages01.AddPage(Page)
                Dim MyContent As New Content01
                MyContent.ModuleKey = "Pages01"
                MyContent.ForeignKey = PageID
                MyContent.Version = "1"
                MyContent.CreatedBy = Session("EzEditName")
                MyContent.UpdatedBy = Session("EzEditName")
                MyContent = Content01.AddContent(MyContent)

                Session("Alert") = "Page Added Successfully"
            Else
                If Not Pages01.UpdatePage(Page) Then PageID = 0
                Session("Alert") = "Page Updated Successfully"
            End If

            Me.ProcessHeaderFiles()

            Pages01.DeletePagePermissions(PageID)
            For Each Item As ListItem In lstUserPermissions.Items
                If Item.Selected Then Pages01.AddPagePermissions(PageID, Item.Value)
            Next

            Emagine.ExecuteSQL("DELETE FROM Pages_MemberCategories WHERE PageID = " & PageID)
            If rblMemberPermissions.SelectedIndex = 1 Then
                For Each Item As ListItem In cblMemberCategories.Items
                    If Item.Selected Then
                        Emagine.ExecuteSQL("INSERT INTO Pages_MemberCategories (PageID,MemberCategoryID) VALUES (" & PageID & ", " & Item.Value & ")")
                    End If
                Next
            End If

            Response.Redirect("Default.aspx?ParentPageID=" & Page.ParentPageID)
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("Default.aspx?ParentPageID=" & ParentPageID)
    End Sub

    Sub ToggleFormFields(ByVal intPageTypeId As Integer)
        If intPageTypeId = 2 Or intPageTypeId = 0 Then
            If intPageTypeId = 2 Then ' = External Link
                trLink.Visible = True
            End If
            trPageTemplate.Visible = False
            trPageKey.Visible = False
            trPageContent.Visible = False
            trTitleTag.Visible = False
            trMetaDescription.Visible = False
            trMetaKeywords.Visible = False
            trSEOScript.Visible = False
        Else
            trLink.Visible = False
            trPageKey.Visible = True
            trPageContent.Visible = True
            trTitleTag.Visible = True
            trMetaDescription.Visible = True
            trMetaKeywords.Visible = True
            trSEOScript.Visible = True
            trPageTemplate.Visible = True
        End If
    End Sub

    Protected Sub DuplicateKeyCheck(ByVal sourceCondition As BaseCondition, ByVal args As ConditionEventArgs)
        Dim vArgs As ConditionTwoFieldEventArgs = CType(args, ConditionTwoFieldEventArgs)

        args.IsMatch = Pages01.IsUniquePageKey(vArgs.Value, PageID, Session("EzEditLanguageID"))

    End Sub


    Protected Sub ddlMembershipFormID_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMembershipFormID.Load
        If Not IsPostBack Then
            ddlMembershipFormID.DataSource = Pages01.GetFormPages(3, 20, Session("EzEditLanguageID"))
            ddlMembershipFormID.DataTextField = "PageName"
            ddlMembershipFormID.DataValueField = "PageID"
            ddlMembershipFormID.DataBind()
        End If

        If rblMemberPermissions.SelectedIndex = 1 Then
            trMembershipFormID.Visible = True
        Else
            trMembershipFormID.Visible = False
        End If
    End Sub

    Protected Sub rptTemplateHeaders_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptTemplateHeaders.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim HeaderID As Integer = DataBinder.Eval(e.Item.DataItem, "HeaderID")
            Dim FriendlyName As String = DataBinder.Eval(e.Item.DataItem, "FriendlyName").ToString
            Dim Description As String = DataBinder.Eval(e.Item.DataItem, "Description").ToString

            Dim pnlUploadHeader As Panel = e.Item.FindControl("pnlUploadHeader")
            Dim pnlViewHeader As Panel = e.Item.FindControl("pnlViewHeader")
            Dim lblFriendlyName As Label = e.Item.FindControl("lblFriendlyName")
            Dim hdnHeaderID As HiddenField = e.Item.FindControl("hdnHeaderID")
            Dim hypHelpText As HyperLink = e.Item.FindControl("hypHelpText")
            Dim lblHeaderText As Label = e.Item.FindControl("lblHeaderText")
            Dim txtHeaderText As TextBox = e.Item.FindControl("txtHeaderText")

            If lblFriendlyName IsNot Nothing Then lblFriendlyName.Text = FriendlyName & ":"
            If hdnHeaderID IsNot Nothing Then hdnHeaderID.Value = HeaderID
            If hypHelpText IsNot Nothing Then hypHelpText.ToolTip = Description
            If lblHeaderText IsNot Nothing Then lblHeaderText.Text = FriendlyName & " Text:"

            Dim DataTable As DataTable = Emagine.GetDataTable("SELECT * FROM PageHeaders WHERE HeaderID = " & HeaderID & " AND PageID = " & PageID)
            If DataTable.Rows.Count > 0 Then
                Dim hypImageUrl As HyperLink = e.Item.FindControl("hypImageUrl")
                Dim btnDeleteHeader As System.Web.UI.WebControls.ImageButton = e.Item.FindControl("btnDeleteHeader")

                If hypImageUrl IsNot Nothing And DataTable.Rows(0).Item("ImageUrl").ToString.Length > 0 Then
                    pnlUploadHeader.Visible = False
                    pnlViewHeader.Visible = True
                    hypImageUrl.Text = DataTable.Rows(0).Item("ImageUrl").ToString
                    hypImageUrl.NavigateUrl = DataTable.Rows(0).Item("ImageUrl").ToString
                    btnDeleteHeader.CommandArgument = DataTable.Rows(0).Item("PageHeaderID")
                End If
                If txtHeaderText IsNot Nothing Then txtHeaderText.Text = DataTable.Rows(0).Item("HeaderText").ToString

            Else
                pnlUploadHeader.Visible = True
                pnlViewHeader.Visible = False
            End If

        End If
    End Sub

    Sub DeleteHeader(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As System.Web.UI.WebControls.ImageButton = sender
        Dim PageHeaderID As Integer = Emagine.GetNumber(Button.CommandArgument)
        Dim ImageUrl As String = Emagine.GetDbValue("SELECT ImageUrl FROM PageHeaders WHERE PageHeaderID = " & PageHeaderID)

	'COMMENTED OUT PER CRAIG'S REQUEST ON 1/13/10	        
	'If ImageUrl.Length > 0 Then
        '    If System.IO.File.Exists(Server.MapPath(ImageUrl)) Then
        '        System.IO.File.Delete(Server.MapPath(ImageUrl))
        '    End If
        'End If

        If Emagine.ExecuteSQL("DELETE FROM PageHeaders WHERE PageHeaderID = " & PageHeaderID) Then
            lblAlert.Text = "The header has been removed successfully."
            Me.BindTemplateHeaders()
        Else
            lblAlert.Text = "An error occurred while trying to delete the header."
        End If
    End Sub

    Sub ProcessHeaderFiles()
        Dim VirtualUploadPath As String = "/Collateral/templates/" & Session("EzEditLanguageName") & "/images/"

        For i As Integer = 0 To (rptTemplateHeaders.Items.Count - 1)
            If rptTemplateHeaders.Items(i).ItemType = ListItemType.Item Or rptTemplateHeaders.Items(i).ItemType = ListItemType.AlternatingItem Then
                Dim HeaderID As Integer = 0

                Dim hdnHeaderID As HiddenField = rptTemplateHeaders.Items(i).FindControl("hdnHeaderID")
                Dim lblFriendlyName As Label = rptTemplateHeaders.Items(i).FindControl("lblFriendlyName")
                Dim uplImageUrl As FileUpload = rptTemplateHeaders.Items(i).FindControl("uplImageUrl")
                Dim txtHeaderText As TextBox = rptTemplateHeaders.Items(i).FindControl("txtHeaderText")

                If hdnHeaderID IsNot Nothing Then HeaderID = hdnHeaderID.Value
                If uplImageUrl IsNot Nothing And HeaderID > 0 Then
                    If uplImageUrl.HasFile Then
                        uplImageUrl.SaveAs(Server.MapPath(VirtualUploadPath & uplImageUrl.FileName))

                        Emagine.ExecuteSQL("DELETE FROM PageHeaders WHERE HeaderID = " & HeaderID & " AND PageID = " & PageID)

                        Dim SqlBuilder As New StringBuilder
                        SqlBuilder.Append("INSERT INTO PageHeaders ")
                        SqlBuilder.Append("(HeaderID,PageID,ImageUrl,HeaderText,CreatedBy)")
                        SqlBuilder.Append(" VALUES ")
                        SqlBuilder.Append("(" & HeaderID & ", " & PageID & ", '" & VirtualUploadPath & uplImageUrl.FileName.Replace("'", "''") & "', '" & txtHeaderText.Text.Replace("'", "''") & "', '" & Session("EzEditName").ToString.Replace("'", "''") & "')")

                        Emagine.ExecuteSQL(SqlBuilder.ToString)
                        Emagine.ExecuteSQL("UPDATE PageHeaders SET HeaderText = '" & txtHeaderText.Text.Replace("'", "''") & "' WHERE HeaderID = " & HeaderID & " AND PageID = " & PageID)
                    Else
                        'Emagine.ExecuteSQL("DELETE FROM PageHeaders WHERE HeaderID = " & HeaderID & " AND PageID = " & PageID)
                        Dim SqlBuilder As New StringBuilder
                        Dim PageHeaderData As DataTable = Emagine.GetDataTable("SELECT * FROM PageHeaders WHERE HeaderID = " & HeaderID & " AND PageID = " & PageID)

                        If PageHeaderData.Rows.Count > 0 Then
                            SqlBuilder.Append("UPDATE PageHeaders SET ")
                            SqlBuilder.Append("HeaderText = '" & txtHeaderText.Text.Replace("'", "''") & "' ")
                            SqlBuilder.Append("WHERE HeaderID = " & HeaderID & " AND PageID = " & PageID)

                        Else
                            SqlBuilder.Append("INSERT INTO PageHeaders ")
                            SqlBuilder.Append("(HeaderID,PageID,HeaderText,CreatedBy)")
                            SqlBuilder.Append(" VALUES ")
                            SqlBuilder.Append("(" & HeaderID & ", " & PageID & ", '" & txtHeaderText.Text.Replace("'", "''") & "', '" & Session("EzEditName").ToString.Replace("'", "''") & "')")
                        End If

                        Emagine.ExecuteSQL(SqlBuilder.ToString)
                    End If
                End If


            End If

        Next
    End Sub

    Sub PopulateMemerCategories()
        If Not Page.IsPostBack Then
            Dim MemberCategoryData As DataTable = Emagine.GetDataTable("SELECT CategoryID, CategoryName FROM ModuleCategories WHERE ModuleKey = 'Membership'")
            If MemberCategoryData.Rows.Count > 0 Then
                cblMemberCategories.DataSource = MemberCategoryData
                cblMemberCategories.DataTextField = "CategoryName"
                cblMemberCategories.DataValueField = "CategoryID"
                cblMemberCategories.DataBind()
            Else
                trMemberPermissions.Visible = False
            End If
        End If
    End Sub

    
    Protected Sub rblMemberPermissions_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblMemberPermissions.SelectedIndexChanged
        If rblMemberPermissions.SelectedIndex = 0 Then
            cblMemberCategories.SelectedIndex = -1
            cblMemberCategories.Enabled = False
        Else
            cblMemberCategories.Enabled = True

            Dim MemberData As DataTable = Emagine.GetDataTable("SELECT * FROM Pages_MemberCategories WHERE PageID = " & PageID)
            If MemberData.Rows.Count > 0 Then
                cblMemberCategories.Enabled = True

                For Each Item As ListItem In cblMemberCategories.Items
                    For i As Integer = 0 To (MemberData.Rows.Count - 1)
                        If Item.Value = MemberData.Rows(i).Item("MemberCategoryID") Then
                            Item.Selected = True
                            Exit For
                        End If
                    Next
                Next
                Dim ParentMemberData As DataTable = Emagine.GetDataTable("SELECT * FROM Pages_MemberCategories WHERE PageID = " & ParentPageID)
                If ParentMemberData.Rows.Count > 0 Then
                    For Each Item As ListItem In cblMemberCategories.Items
                        For i As Integer = 0 To (ParentMemberData.Rows.Count - 1)
                            If Item.Value = ParentMemberData.Rows(i).Item("MemberCategoryID") Then
                                Item.Enabled = True
                            Else
                                Item.Enabled = False
                            End If
                        Next
                    Next
                End If
            End If

        End If
    End Sub
End Class