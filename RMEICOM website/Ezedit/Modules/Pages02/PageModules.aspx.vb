
Partial Class Ezedit_Modules_Pages02_PageModules
    Inherits System.Web.UI.Page

    Dim _PageID As Integer = 0
    Dim _ParentPageID As Integer = 0

    Protected Sub gdvList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvList.Load
        If Request("PageID") IsNot Nothing Then _PageID = Emagine.GetNumber(Request("PageID"))
        If _PageID = 0 Then Response.Redirect("Default.aspx")

        If Not Page.IsPostBack Then
            Me.PopulateItemList()
        End If

        lblBreadcrumbs.Text = Pages01.GetBreadcrumbs(_PageID, "Pages02")
        lblAlert.Text = ""
    End Sub

    Sub PopulateItemList()
        gdvList.DataSource = Emagine.GetDataReader("SELECT * FROM qryAllPageModules WHERE PageID = '" & _PageID & "' ORDER BY SortOrder")
        gdvList.DataBind()
    End Sub

    Sub ddlSortOrder_IndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ddlSortOrder As DropDownList = sender
        Dim hdnSortOrder As HiddenField = ddlSortOrder.Parent.Parent.FindControl("hdnSortOrder")
        Dim btnEditItem As ImageButton = ddlSortOrder.Parent.Parent.FindControl("btnEditItem")
        Dim NewSortOrder As Integer = ddlSortOrder.SelectedValue
        Dim OldSortOrder As Integer = hdnSortOrder.Value
        Dim PageModuleID As Integer = btnEditItem.CommandArgument

        Dim PageModuleData As DataTable = Emagine.GetDataTable("SELECT * FROM qryAllPageModules WHERE PageID = '" & _PageID & "' ORDER BY SortOrder")
        For Each Row As DataRow In PageModuleData.Rows
            Dim Sql As String = ""
            If Row("PageModuleID") = PageModuleID Then
                Select Case Row("PageModuleType")
                    Case "Display Page"
                        Sql = "UPDATE PageModuleProperties SET PropertyValue = " & NewSortOrder & " WHERE PageModuleID = " & PageModuleID & " AND PropertyID IN (SELECT PropertyID FROM ModuleProperties WHERE ModuleKey = '" & Row("ModuleKey").ToString & "' AND PropertyName = 'DisplayPageSortOrder')"

                    Case "Delivery Page"
                        Sql = "UPDATE PageModuleProperties SET PropertyValue = " & NewSortOrder & " WHERE PageModuleID = " & PageModuleID & " AND PropertyID IN (SELECT PropertyID FROM ModuleProperties WHERE ModuleKey = '" & Row("ModuleKey").ToString & "' AND PropertyName = 'DeliveryPageSortOrder')"

                    Case "Thank-You Page"
                        Sql = "UPDATE PageModuleProperties SET PropertyValue = " & NewSortOrder & " WHERE PageModuleID = " & PageModuleID & " AND PropertyID IN (SELECT PropertyID FROM ModuleProperties WHERE ModuleKey = '" & Row("ModuleKey").ToString & "' AND PropertyName = 'ThankYouPageSortOrder')"
                End Select
            Else
                If OldSortOrder > NewSortOrder Then
                    If Emagine.GetNumber(Row("SortOrder").ToString) >= NewSortOrder Then
                        Select Case Row("PageModuleType")
                            Case "Display Page"
                                Sql = "UPDATE PageModuleProperties SET PropertyValue = " & (Emagine.GetNumber(Row("SortOrder").ToString) + 1) & " WHERE PageModuleID = " & Row("PageModuleID") & " AND PropertyID IN (SELECT PropertyID FROM ModuleProperties WHERE ModuleKey = '" & Row("ModuleKey").ToString & "' AND PropertyName = 'DisplayPageSortOrder')"
                            Case "Delivery Page"
                                Sql = "UPDATE PageModuleProperties SET PropertyValue = " & (Emagine.GetNumber(Row("SortOrder").ToString) + 1) & " WHERE PageModuleID = " & Row("PageModuleID") & " AND PropertyID IN (SELECT PropertyID FROM ModuleProperties WHERE ModuleKey = '" & Row("ModuleKey").ToString & "' AND PropertyName = 'DeliveryPageSortOrder')"
                            Case "Thank-You Page"
                                Sql = "UPDATE PageModuleProperties SET PropertyValue = " & (Emagine.GetNumber(Row("SortOrder").ToString) + 1) & " WHERE PageModuleID = " & Row("PageModuleID") & " AND PropertyID IN (SELECT PropertyID FROM ModuleProperties WHERE ModuleKey = '" & Row("ModuleKey").ToString & "' AND PropertyName = 'ThankYouPageSortOrder')"
                        End Select
                    End If
                Else
                    If Emagine.GetNumber(Row("SortOrder").ToString) <= NewSortOrder Then
                        Select Case Row("PageModuleType")
                            Case "Display Page"
                                Sql = "UPDATE PageModuleProperties SET PropertyValue = " & (Emagine.GetNumber(Row("SortOrder").ToString) - 1) & " WHERE PageModuleID = " & Row("PageModuleID") & " AND PropertyID IN (SELECT PropertyID FROM ModuleProperties WHERE ModuleKey = '" & Row("ModuleKey").ToString & "' AND PropertyName = 'DisplayPageSortOrder')"
                            Case "Delivery Page"
                                Sql = "UPDATE PageModuleProperties SET PropertyValue = " & (Emagine.GetNumber(Row("SortOrder").ToString) - 1) & " WHERE PageModuleID = " & Row("PageModuleID") & " AND PropertyID IN (SELECT PropertyID FROM ModuleProperties WHERE ModuleKey = '" & Row("ModuleKey").ToString & "' AND PropertyName = 'DeliveryPageSortOrder')"
                            Case "Thank-You Page"
                                Sql = "UPDATE PageModuleProperties SET PropertyValue = " & (Emagine.GetNumber(Row("SortOrder").ToString) - 1) & " WHERE PageModuleID = " & Row("PageModuleID") & " AND PropertyID IN (SELECT PropertyID FROM ModuleProperties WHERE ModuleKey = '" & Row("ModuleKey").ToString & "' AND PropertyName = 'ThankYouPageSortOrder')"
                        End Select
                    End If
                End If
            End If
            'Response.Write(OldSortOrder & " : " & NewSortOrder & " : " & Sql & "<br />")
            If Sql.Length > 0 Then Emagine.ExecuteSQL(Sql)
        Next

        Me.ResetSortOrder()

        Me.PopulateItemList()

        lblAlert.Text = "The modules have been sorted successfully."
    End Sub

    Sub btnEditItem_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim PageModuleID As Integer = Button.CommandArgument
        Dim ParentPageID As Integer = 0
        If Request("ParentPageID") IsNot Nothing Then ParentPageID = Emagine.GetNumber(Request("ParentPageID").ToString)

    End Sub

    Sub btnDeleteItem_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim PageModuleID As Integer = Button.CommandArgument
        'Dim ParentPageID As Integer = 0
        'If Request("ParentPageID") IsNot Nothing Then ParentPageID = Emagine.GetNumber(Request("ParentPageID").ToString)

        If Emagine.ExecuteSQL("DELETE FROM PageModules WHERE PageModuleID = " & PageModuleID) Then
            Emagine.ExecuteSQL("DELETE FROM PageModuleProperties WHERE PageModuleID = " & PageModuleID)
            'Response.Redirect("Default.aspx?ParentPageID=" & ParentPageID)

            Me.ResetSortOrder()
            Me.PopulateItemList()

            lblAlert.Text = "The module has been removed successfully."
        Else
            lblAlert.Text = "An error occurred while attempting to remove the module from this page."
        End If
    End Sub

    Protected Sub gdvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim PageModuleID As Integer = e.Row.DataItem("PageModuleID")
            Dim SortOrder As Integer = Emagine.GetNumber(e.Row.DataItem("SortOrder").ToString)
            Dim PageModuleType As String = e.Row.DataItem("PageModuleType")
            Dim ModuleKey As String = e.Row.DataItem("ModuleKey")
            Dim FormPageTypeID As Integer = e.Row.DataItem("FormPageTypeID")
            Dim tdDelete As HtmlTableCell = e.Row.FindControl("tdDelete")
            Dim tdHelp As HtmlTableCell = e.Row.FindControl("tdHelp")

            Dim ddlSortOrder As DropDownList = e.Row.FindControl("ddlSortOrder")
            Dim hdnSortOrder As HiddenField = e.Row.FindControl("hdnSortOrder")
            Dim btnEditItem As ImageButton = e.Row.FindControl("btnEditItem")
            Dim btnDeleteItem As ImageButton = e.Row.FindControl("btnDeleteItem")

            Me.BindSortOrder(ddlSortOrder, _PageID, SortOrder)
            hdnSortOrder.Value = SortOrder
            btnEditItem.CommandArgument = PageModuleID
            btnEditItem.CommandName = PageModuleType
            btnDeleteItem.CommandArgument = PageModuleID

            If ModuleKey = "Forms01" And FormPageTypeID = 2 Then
                tdDelete.Visible = False
                tdHelp.Visible = True
            Else
                tdDelete.Visible = True
            End If
        End If
    End Sub

    Sub BindSortOrder(ByVal ddlSortOrder As DropDownList, ByVal intPageID As Integer, ByVal intSortOrder As Integer)
        Dim MaxSortOrder As Integer = Emagine.GetDbValue("SELECT COUNT(*) AS RecordCount FROM qryAllPageModules WHERE PageID = '" & _PageID & "'")
        For i As Integer = 0 To MaxSortOrder - 1
            ddlSortOrder.Items.Add(New ListItem(i + 1, i + 1))
            If intSortOrder = i + 1 Then
                ddlSortOrder.Items(i).Selected = True
            End If
        Next
    End Sub

    Sub ResetSortOrder()
        Dim PageModuleData As DataTable = Emagine.GetDataTable("SELECT * FROM qryAllPageModules WHERE PageID = '" & _PageID & "' ORDER BY SortOrder")

        For i As Integer = 0 To PageModuleData.Rows.Count - 1
            Dim Sql As String = ""
            Select Case PageModuleData.Rows(i).Item("PageModuleType")
                Case "Display Page"
                    Sql = "UPDATE PageModuleProperties SET PropertyValue = " & (i + 1) & " WHERE PageModuleID = " & PageModuleData.Rows(i).Item("PageModuleID") & " AND PropertyID IN (SELECT PropertyID FROM ModuleProperties WHERE ModuleKey = '" & PageModuleData.Rows(i).Item("ModuleKey") & "' AND PropertyName = 'DisplayPageSortOrder')"

                Case "Delivery Page"
                    Sql = "UPDATE PageModuleProperties SET PropertyValue = " & (i + 1) & " WHERE PageModuleID = " & PageModuleData.Rows(i).Item("PageModuleID") & " AND PropertyID IN (SELECT PropertyID FROM ModuleProperties WHERE ModuleKey = '" & PageModuleData.Rows(i).Item("ModuleKey") & "' AND PropertyName = 'DeliveryPageSortOrder')"

                Case "Thank-You Page"
                    Sql = "UPDATE PageModuleProperties SET PropertyValue = " & (i + 1) & " WHERE PageModuleID = " & PageModuleData.Rows(i).Item("PageModuleID") & " AND PropertyID IN (SELECT PropertyID FROM ModuleProperties WHERE ModuleKey = '" & PageModuleData.Rows(i).Item("ModuleKey") & "' AND PropertyName = 'ThankYouPageSortOrder')"
            End Select

            If Sql.Length > 0 Then Emagine.ExecuteSQL(Sql)
        Next

    End Sub
End Class
