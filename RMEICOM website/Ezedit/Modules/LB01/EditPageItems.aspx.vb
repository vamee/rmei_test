
Partial Class Ezedit_Modules_LB01_EditPageItems
    Inherits System.Web.UI.Page

    Public _PageID As Integer = 0

#Region "Common"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request("PageID") IsNot Nothing Then _PageID = Emagine.GetNumber(Request("PageID"))

        If Not Page.IsPostBack Then
            lblBreadcrumbs.Text = Pages01.GetBreadcrumbs(_PageID, "Pages02")
        End If

        lblAlert.Text = ""
    End Sub

    Sub ResetSortOrder()
        Dim DataTable As DataTable = Emagine.GetDataTable("SELECT * FROM PageLibraryItems WHERE PageID = " & _PageID & " ORDER BY SortOrder")
        For i As Integer = 0 To DataTable.Rows.Count - 1
            Dim Sql As String = "UPDATE PageLibraryItems SET SortOrder = " & (i + 1) & " WHERE PageItemID = " & DataTable.Rows(i).Item("PageItemID")
            Emagine.ExecuteSQL(Sql)
        Next
    End Sub
#End Region

#Region "ListPanel"
    Protected Sub lbtnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnAddNew.Click
        Me.DisplayEditPanel(0)
    End Sub

    Protected Sub ddlSortOrder_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ddlSortOrder As DropDownList = sender
        Dim Row As GridViewRow = ddlSortOrder.Parent.Parent
        Dim btnDelete As ImageButton = Row.FindControl("btnDelete")
        Dim hdnSortOrder As HiddenField = Row.FindControl("hdnSortOrder")
        Dim PageItemID As Integer = Emagine.GetNumber(btnDelete.CommandArgument)
        Dim OldSortOrder As Integer = Emagine.GetNumber(hdnSortOrder.Value)
        Dim NewSortOrder As Integer = Emagine.GetNumber(ddlSortOrder.SelectedValue)
        Dim Sql As String = ""

        If OldSortOrder > NewSortOrder Then
            Sql = "UPDATE PageLibraryItems SET SortOrder = SortOrder + 1 WHERE PageID = " & _PageID & " AND PageItemID <> " & PageItemID & " AND SortOrder >= " & NewSortOrder & " AND SortOrder < " & OldSortOrder
        Else
            Sql = "UPDATE PageLibraryItems SET SortOrder = SortOrder - 1 WHERE PageID = " & _PageID & " AND PageItemID <> " & PageItemID & " AND SortOrder <= " & NewSortOrder & " AND SortOrder > " & OldSortOrder
        End If

        Emagine.ExecuteSQL(Sql)

        Emagine.ExecuteSQL("UPDATE PageLibraryItems SET SortOrder = " & NewSortOrder & " WHERE PageItemID = " & PageItemID)

        Me.ResetSortOrder()
        gdvList.DataBind()
        lblAlert.Text = "The items have been sorted successfully."
    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim btnDelete As ImageButton = sender
        Dim PageItemID As Integer = btnDelete.CommandArgument

        Me.DisplayEditPanel(PageItemID)
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim btnDelete As ImageButton = sender
        Dim PageItemID As Integer = btnDelete.CommandArgument

        If Emagine.ExecuteSQL("DELETE FROM PageLibraryItems WHERE PageItemID = " & PageItemID) Then
            Me.ResetSortOrder()
            gdvList.DataBind()
            lblAlert.Text = "The item has been removed from the page successfully."

        Else
            lblAlert.Text = "An error occurred while attempting to delete this item."
        End If
    End Sub

    Protected Sub gdvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ItemID As Integer = e.Row.DataItem("ItemID")
            Dim SortOrder As Integer = e.Row.DataItem("SortOrder")

            Dim ddlSortOrder As DropDownList = e.Row.FindControl("ddlSortOrder")
            Dim hdnSortOrder As HiddenField = e.Row.FindControl("hdnSortOrder")

            Me.BindSortOrder(ddlSortOrder, SortOrder)
        End If
    End Sub

    Sub BindSortOrder(ByVal ddlSortOrder As DropDownList, ByVal intSortOrder As Integer)
        Dim MaxSortOrder As Integer = Emagine.GetDbValue("SELECT COUNT(PageItemID) As MaxSortOrder FROM qryPageLibraryItems WHERE PageID = " & _PageID)

        For i As Integer = 1 To MaxSortOrder
            Dim ListItem As New ListItem(i.ToString, i.ToString)
            If i = intSortOrder Then ListItem.Selected = True
            ddlSortOrder.Items.Add(ListItem)
        Next
    End Sub

    Sub DisplayListPanel()
        Me.ResetEditForm()
        gdvList.DataBind()

        pnlList.Visible = True
        pnlEdit.Visible = False
    End Sub
#End Region

#Region "EditPanel"
    Protected Sub ddlItemID_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlItemID.Load
        If Not Page.IsPostBack Then
            Dim LibraryItemData As DataTable = Emagine.GetDataTable("SELECT * FROM qryLibraryItems ORDER BY ResourceName")
            For Each Row As DataRow In LibraryItemData.Rows
                Dim Item As New ListItem
                Item.Value = Row("ItemID")

                If Not Row("IsEnabled") Then
                    Item.Text = Row("ResourceName") & " [DISABLED]"
                Else

                    If Row("DisplayStartDate").ToString.Length > 0 And Row("DisplayEndDate").ToString.Length > 0 Then
                        If String.Format("{0:d}", CDate(Row("DisplayStartDate"))) = "1/1/1900" And String.Format("{0:d}", CDate(Row("DisplayEndDate"))) = "1/1/1900" Then
                            Item.Text = Row("ResourceName") & " [CURRENT - No Expiration]"
                        ElseIf CDate(Row("DisplayStartDate")) > Date.Now Then
                            Item.Text = Row("ResourceName") & " [PENDING - Starts " & Row("DisplayStartDate") & "]"
                        ElseIf CDate(Row("DisplayEndDate")) < Date.Now Then
                            Item.Text = Row("ResourceName") & " [EXPIRED - Expired " & Row("DisplayEndDate") & "]"
                        ElseIf CDate(Row("DisplayStartDate")) <= Date.Now And CDate(Row("DisplayEndDate")) >= Date.Now Then
                            Item.Text = Row("ResourceName") & " [CURRENT - Expires: " & Row("DisplayEndDate") & "]"
                        End If
                    End If
                End If

                ddlItemID.Items.Add(Item)
            Next
            LibraryItemData.Dispose()
        End If
    End Sub

    Sub DisplayEditPanel(ByVal intPageItemID As Integer)
        ddlItemID.SelectedIndex = -1

        If intPageItemID > 0 Then
            hdnPageItemID.Value = intPageItemID
            Dim PageLibraryItemData As DataTable = Emagine.GetDataTable("SELECT * FROM qryPageLibraryItems WHERE PageItemID = " & intPageItemID)
            If PageLibraryItemData.Rows.Count > 0 Then
                For Each Item As ListItem In ddlItemID.Items
                    If Item.Value = PageLibraryItemData.Rows(0).Item("ItemID") Then
                        Item.Selected = True
                        Exit For
                    End If
                Next
            End If
            PageLibraryItemData.Dispose()
        Else
            hdnPageItemID.Value = 0
        End If

        pnlList.Visible = False
        pnlEdit.Visible = True
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If PeterBlum.VAM.Globals.Page.IsValid Then
            Dim ErrorMessage As String = ""
            Dim PageItemID As Integer = Emagine.GetNumber(hdnPageItemID.Value)

            If PageItemID > 0 Then
                If LB01.PageLibraryItems.PageLibraryItem.Update(PageItemID, ddlItemID.SelectedValue, ErrorMessage) Then
                    Me.DisplayListPanel()

                    lblAlert.Text = "The library item has been updated successfully."
                Else
                    lblAlert.Text = "An error occurred while attempting to add this library item to the page.<br />" & ErrorMessage
                End If

            Else
                If LB01.PageLibraryItems.PageLibraryItem.Insert(_PageID, ddlItemID.SelectedItem.Value) Then
                    Me.ResetSortOrder()
                    Me.DisplayListPanel()

                    lblAlert.Text = "Library item added to page successfully."
                Else
                    lblAlert.Text = "An error occurred while adding library item."
                End If
            End If
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DisplayListPanel()
    End Sub

    Sub ResetEditForm()
        ddlItemID.SelectedIndex = -1
        hdnPageItemID.Value = -1
    End Sub
#End Region




    
    
    
End Class
