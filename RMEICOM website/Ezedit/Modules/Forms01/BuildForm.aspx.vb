Imports System.Data.SqlClient

Partial Class Ezedit_Modules_Forms01_Default
    Inherits System.Web.UI.Page

    Dim _FormID As Integer = 0
    Dim _MaxSortOrder As Integer = 0
    Dim _MyForm As New Forms01

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request("FormID") IsNot Nothing Then _FormID = Emagine.GetNumber(Request("FormID"))

        If _FormID = 0 Then Response.Redirect("Default.aspx")

        _MyForm = Forms01.GetFormInfo(_FormID)
        _MaxSortOrder = Emagine.GetNumber(Emagine.GetDbValue("SELECT COUNT(FieldID) AS RecordCount FROM FormFields WHERE FormID = " & _FormID))

        gdvList.DataSourceID = "dataFormFields"
        gdvList.DataBind()

        lblPageTitle.Text = "<a href='Default.aspx'>Forms</a> > " & _MyForm.FormName & " > Edit Form Fields"
        lblAlert.Text = ""

        If Session("Alert") IsNot Nothing Then
            lblAlert.Text = Session("Alert")
            Session("Alert") = ""
        End If
    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim ItemID As Integer = Button.CommandArgument

        Response.Redirect("EditField.aspx?FormID=" & _FormID & "&FieldID=" & ItemID)
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim ItemID As Integer = Button.CommandArgument

        If FormFields01.DeleteField(ItemID) Then
            Me.ResetSortOrder()
            gdvList.DataBind()
            lblAlert.Text = "The field has been removed successfully."
        Else
            lblAlert.Text = "An error occurred while attempting to delete this field."
        End If
    End Sub

    Protected Sub ddlSortOrder_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ddlSortOrder As DropDownList = sender
        Dim Row As GridViewRow = ddlSortOrder.Parent.Parent
        Dim btnEdit As ImageButton = Row.FindControl("btnEdit")
        Dim hdnSortOrder As HiddenField = Row.FindControl("hdnSortOrder")
        Dim ItemID As Integer = Emagine.GetNumber(btnEdit.CommandArgument)
        Dim OldSortOrder As Integer = Emagine.GetNumber(hdnSortOrder.Value)
        Dim NewSortOrder As Integer = Emagine.GetNumber(ddlSortOrder.SelectedValue)
        Dim Sql As String = ""

        If OldSortOrder > NewSortOrder Then
            Sql = "UPDATE FormFields SET SortOrder = SortOrder + 1 WHERE FormID = " & _FormID & " AND FieldID <> " & ItemID & " AND SortOrder >= " & NewSortOrder & " AND SortOrder < " & OldSortOrder
        Else
            Sql = "UPDATE FormFields SET SortOrder = SortOrder - 1 WHERE FormID = " & _FormID & " AND FieldID <> " & ItemID & ") AND SortOrder <= " & NewSortOrder & " AND SortOrder > " & OldSortOrder
        End If

        Emagine.ExecuteSQL(Sql)

        Emagine.ExecuteSQL("UPDATE FormFields SET SortOrder = " & NewSortOrder & " WHERE FieldID = " & ItemID)

        Me.ResetSortOrder()

        gdvList.DataBind()

        lblAlert.Text = "The fields have been sorted successfully."
    End Sub

    Sub ResetSortOrder()
        Dim DataTable As DataTable = Emagine.GetDataTable("SELECT FieldID FROM FormFields WHERE FormID = " & _FormID & " ORDER BY SortOrder")
        For i As Integer = 0 To DataTable.Rows.Count - 1
            Dim Sql As String = "UPDATE FormFields SET SortOrder = " & (i + 1) & " WHERE FieldID = " & DataTable.Rows(i).Item("FieldID")
            Emagine.ExecuteSQL(Sql)
        Next
    End Sub

    Protected Sub gdvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim FormID As Integer = e.Row.DataItem("FormID")
            Dim FieldID As Integer = e.Row.DataItem("FieldID")
            Dim FieldTypeID As Integer = e.Row.DataItem("FieldTypeID")
            Dim Label As String = e.Row.DataItem("Label").ToString
            Dim LabelCss As String = e.Row.DataItem("LabelCss").ToString
            Dim SortOrder As Integer = e.Row.DataItem("SortOrder")

            Dim plcFormField As PlaceHolder = e.Row.FindControl("plcFormField")
            Dim ddlSortOrder As DropDownList = e.Row.FindControl("ddlSortOrder")
            Dim hdnSortOrder As HiddenField = e.Row.FindControl("hdnSortOrder")

            FormFields01.DisplayFormField(FieldID, "", plcFormField, True)
            Me.BindSortOrder(ddlSortOrder, SortOrder, _MaxSortOrder)
            hdnSortOrder.Value = SortOrder

            e.Row.Cells(0).CssClass = LabelCss
            e.Row.Cells(0).Width = _MyForm.LabelWidth
            If FieldTypeID = 1 Then
                e.Row.Cells.RemoveAt(0)
                e.Row.Cells(0).ColumnSpan = 2
            End If
        End If
    End Sub


    Sub BindSortOrder(ByVal ddlSortOrder As DropDownList, ByVal intSortOrder As Integer, ByVal intMaxSortOrder As Integer)
        For i As Integer = 1 To intMaxSortOrder
            Dim ListItem As New ListItem(i.ToString, i.ToString)
            If i = intSortOrder Then ListItem.Selected = True
            ddlSortOrder.Items.Add(ListItem)
        Next
    End Sub

    Protected Sub lbtnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnAddNew.Click
        Response.Redirect("EditField.aspx?FormID=" & _FormID & "&FieldID=0")
    End Sub
End Class
