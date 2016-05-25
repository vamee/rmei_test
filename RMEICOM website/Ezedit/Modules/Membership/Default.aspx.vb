
Partial Class Ezedit_Modules_PR01_Default
    Inherits System.Web.UI.Page

    Dim _ModuleKey As String = "Membership"

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
        txtCategoryName.Focus()

        If intCategoryID > 0 Then
            Dim Category As ModuleCategory = ModuleCategory.GetModuleCategory(intCategoryID)
            Dim PageModuleID As Integer = Emagine.GetNumber(Emagine.GetDbValue("SELECT PageModuleID FROM PageModules WHERE ModuleKey = '" & _ModuleKey & "' AND ForeignKey = 'CategoryID' AND ForeignValue = '" & intCategoryID & "'"))
            Dim HomePageID As String = PageModuleProperty.GetProperty(PageModuleID, "HomePage")

            ddlHomePageID.SelectedIndex = -1
            For Each Item As ListItem In ddlHomePageID.Items
                If Item.Value = HomePageID Then
                    Item.Selected = True
                    Exit For
                End If
            Next

            txtCategoryName.Text = Category.CategoryName
            txtDescription.Text = Category.Description
            hdnCategoryID.Value = Category.CategoryID
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
            Dim CategoryID As Integer = Emagine.GetNumber(hdnCategoryID.Value)
            Dim PageModuleID As Integer = 0

            Dim Category As New ModuleCategory
            If CategoryID > 0 Then
                Category = ModuleCategory.GetModuleCategory(CategoryID)
                PageModuleID = Emagine.GetNumber(Emagine.GetDbValue("SELECT * FROM PageModules WHERE ModuleKey = 'Membership' AND ForeignKey = 'CategoryID' AND ForeignValue = '" & CategoryID & "'"))
            End If

            Category.CategoryID = CategoryID
            Category.ModuleKey = _ModuleKey
            Category.StatusID = Emagine.GetNumber(Session("EzEditStatusID"))
            Category.LanguageID = Emagine.GetNumber(Session("EzEditLanguageID"))
            Category.CategoryName = txtCategoryName.Text
            Category.Description = txtDescription.Text

            Dim PageModule As New PageModule
            PageModule.PageModuleId = PageModuleID
            PageModule.PageID = 0
            PageModule.CodeFileID = 0
            PageModule.ModuleKey = _ModuleKey
            PageModule.ForeignKey = "CategoryID"
            PageModule.ForeignValue = CategoryID
            PageModule.SortOrder = 0

            If Category.CategoryID > 0 Then
                If Category.UpdateModuleCategory(Category) Then
                    If PageModuleID > 0 Then
                        PageModule.Update(PageModule)
                        PageModuleProperty.DeleteAll(PageModuleID)
                    Else
                        PageModuleID = PageModule.Insert(PageModule)
                    End If

                    Dim PageProperty As New PageModuleProperty
                    PageProperty.PageModuleId = PageModuleID
                    PageProperty.PropertyID = Emagine.GetNumber(Emagine.GetDbValue("SELECT PropertyID FROM ModuleProperties WHERE ModuleKey = '" & _ModuleKey & "' AND PropertyName = 'HomePage'"))
                    PageProperty.PropertyValue = ddlHomePageID.SelectedValue

                    PageModuleProperty.Insert(PageProperty)

                    lblAlert.Text = "The category has been updated successfully."
                    Me.DisplayListPanel()
                Else
                    lblAlert.Text = "An Error has occurred."
                End If
            Else
                If Category.InsertModuleCategory(Category) Then
                    CategoryID = Emagine.GetDbValue("SELECT MAX(CategoryID) AS MaxCategoryID FROM ModuleCategories WHERE ModuleKey = '" & _ModuleKey & "'")
                    PageModule.ForeignValue = CategoryID
                    PageModuleID = PageModule.Insert(PageModule)

                    Dim PageProperty As New PageModuleProperty
                    PageProperty.PageModuleId = PageModuleID
                    PageProperty.PropertyID = Emagine.GetNumber(Emagine.GetDbValue("SELECT PropertyID FROM ModuleProperties WHERE ModuleKey = '" & _ModuleKey & "' AND PropertyName = 'HomePage'"))
                    PageProperty.PropertyValue = ddlHomePageID.SelectedValue

                    PageModuleProperty.Insert(PageProperty)

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
    
    Protected Sub ddlHomePageID_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlHomePageID.Load
        If Not Page.IsPostBack Then
            ddlHomePageID = Pages01.PopulatePageOptionsDDL(ddlHomePageID, 0, 0, 0, Session("EzEditStatusID"), Session("EzEditLanguageID"))
        End If
    End Sub
End Class
