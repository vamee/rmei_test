
Partial Class Ezedit_Modules_Pages02_Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim PageID As Integer = 0

        If Request("ParentPageID") IsNot Nothing Then PageID = Emagine.GetNumber(Request("ParentPageID").ToString)

        'If PageID > 0 Then
        If Not EzeditUser.HasPagePermissions(Emagine.GetNumber(Session("EzEditUserID")), PageID) Then
            lbtnAddNew.Visible = False
        End If
        'End If

        lblBreadcrumbs.Text = Pages01.GetBreadcrumbs(PageID, "Pages02")

        If Session("Alert") IsNot Nothing Then
            lblAlert.Text = Session("Alert")
            Session("Alert") = ""
        Else
            lblAlert.Text = ""
        End If
    End Sub

    Sub ddlSortOrder_IndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ddlSortOrder As DropDownList = sender
        Dim hdnSortOrder As HiddenField = ddlSortOrder.Parent.Parent.FindControl("hdnSortOrder")
        Dim btnEditPage As ImageButton = ddlSortOrder.Parent.Parent.FindControl("btnEditPage")
        Dim NewSortOrder As Integer = ddlSortOrder.SelectedValue
        Dim OldSortOrder As Integer = hdnSortOrder.Value
        Dim PageID As Integer = btnEditPage.CommandArgument

        Pages01.UpdateSortOrder(PageID, NewSortOrder)
        Pages01.ResetSortOrder(PageID, Emagine.GetNumber(Session("EzEditStatusID")))

        gdvList.DataBind()

        lblAlert.Text = "The pages have been sorted successfully."
    End Sub

    Sub btnExpand_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim PageID As Integer = Button.CommandArgument


        Response.Redirect("Default.aspx?ParentPageID=" & PageID)
    End Sub

    Sub btnEditPage_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim PageID As Integer = Button.CommandArgument
        Dim ParentPageID As Integer = 0
        If Request("ParentPageID") IsNot Nothing Then ParentPageID = Emagine.GetNumber(Request("ParentPageID").ToString)

        Response.Redirect("EditPage.aspx?PageID=" & PageID & "&ParentPageID=" & ParentPageID)
    End Sub

    Sub btnEditContent_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Response.Redirect(Button.CommandArgument)
    End Sub

    Sub btnEditModules_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Response.Redirect(Button.CommandArgument)
    End Sub

    Sub btnEditLibraryItems_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Response.Redirect(Button.CommandArgument)
    End Sub

    Sub btnDeletePage_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim PageID As Integer = Button.CommandArgument
        Dim MyPage As New Pages01
        MyPage = Pages01.GetPageInfo(PageID)


        If Pages01.DeletePage(PageID) Then
            Content01.DeleteContent("Pages01", PageID.ToString)
            Pages01.ResetSortOrder(MyPage.ParentPageID, Emagine.GetNumber(Session("EzEditStatusID")))

            gdvList.DataBind()

            lblAlert.Text = "The page has been removed successfully."
        Else
            lblAlert.Text = "An error occurred while attempting to delete this page."
        End If
    End Sub

    Protected Sub gdvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim PageID As Integer = e.Row.DataItem("PageID")
            Dim ParentPageID As Integer = e.Row.DataItem("ParentPageID")
            Dim PageTypeID As Integer = e.Row.DataItem("PageTypeID")
            Dim ContentPageID As Integer = e.Row.DataItem("ContentPageID")
            Dim PageName As String = e.Row.DataItem("PageName")
            Dim IsHidden As Boolean = e.Row.DataItem("IsHidden")
            Dim HasChildren As Boolean = e.Row.DataItem("HasChildren")
            Dim SortOrder As Integer = e.Row.DataItem("PageSortOrder")
            Dim HasPermissions As Boolean = EzeditUser.HasPagePermissions(Emagine.GetNumber(Session("EzEditUserID")), PageID)
            Dim SupportsLibraryItems As Boolean = e.Row.DataItem("SupportsLibraryItems")

            Dim lblPageName As Label = e.Row.FindControl("lblPageName")
            Dim lblIsHidden As Label = e.Row.FindControl("lblIsHidden")
            Dim ddlSortOrder As DropDownList = e.Row.FindControl("ddlSortOrder")
            Dim hdnSortOrder As HiddenField = e.Row.FindControl("hdnSortOrder")
            Dim btnExpand As ImageButton = e.Row.FindControl("btnExpand")
            Dim btnEditPage As ImageButton = e.Row.FindControl("btnEditPage")
            Dim btnEditContent As ImageButton = e.Row.FindControl("btnEditContent")
            Dim btnEditModules As ImageButton = e.Row.FindControl("btnEditModules")
            Dim btnEditLibraryItems As ImageButton = e.Row.FindControl("btnEditLibraryItems")
            Dim btnDeletePage As ImageButton = e.Row.FindControl("btnDeletePage")

            lblPageName.Text = PageName
            lblIsHidden.Visible = IsHidden
            Me.BindSortOrder(ddlSortOrder, PageID, SortOrder, ParentPageID)
            hdnSortOrder.Value = SortOrder
            btnExpand.CommandArgument = PageID
            btnExpand.Visible = HasChildren
            btnEditPage.CommandArgument = PageID
            btnEditPage.Visible = HasPermissions
            btnDeletePage.CommandArgument = PageID
            btnDeletePage.Visible = HasPermissions
            If HasChildren Then btnDeletePage.OnClientClick = "return confirmDeleteSection();"

            If PageTypeID = 1 And ContentPageID = 0 And HasPermissions Then
                If Content01.GetContentCount("Pages01", PageID.ToString) > 1 Then
                    btnEditContent.CommandArgument = "ContentList.aspx?PageID=" & PageID & "&ParentPageID=" & ParentPageID
                Else
                    btnEditContent.CommandArgument = "EditContent.aspx?PageID=" & PageID & "&ContentID=" & Pages01.GetContentID(PageID)
                End If
            Else
                btnEditContent.Visible = False
            End If

            If PageTypeID = 1 Then
                btnEditModules.CommandArgument = "PageModules.aspx?PageID=" & PageID & "&ParentPageID=" & ParentPageID
                btnEditModules.Visible = HasPermissions
            End If

            If PageTypeID = 1 And HasPermissions And SupportsLibraryItems Then
                btnEditLibraryItems.CommandArgument = "~/ezedit/modules/LB01/EditPageItems.aspx?PageID=" & PageID & "&ParentPageID=" & ParentPageID
            Else
                btnEditLibraryItems.Visible = False
            End If

        End If
    End Sub

    Sub BindSortOrder(ByVal ddlSortOrder As DropDownList, ByVal intPageID As Integer, ByVal intSortOrder As Integer, ByVal intParentPageID As Integer)
        Dim MaxSortOrder As Integer = Emagine.GetDbValue("SELECT COUNT(*) AS RecordCount FROM Pages WHERE ParentPageID = " & intParentPageID & " AND StatusID = " & Session("EzEditStatusID"))
        For i As Integer = 0 To MaxSortOrder - 1
            ddlSortOrder.Items.Add(New ListItem(i + 1, i + 1))
            If intSortOrder = i + 1 Then
                ddlSortOrder.Items(i).Selected = True
            End If
        Next
    End Sub

    Protected Sub lbtnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnAddNew.Click
        Dim PageID As Integer = 0
        If Request("ParentPageID") IsNot Nothing Then PageID = Emagine.GetNumber(Request("ParentPageID").ToString)
        Response.Redirect("EditPage.aspx?ParentPageID=" & PageID)
    End Sub
End Class
