Imports System.Data

Partial Class Ezedit_Admin_ErrorLog_Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblAlert.Text = ""
    End Sub

    Protected Sub cbxSelectAll_CheckChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim CheckBox As CheckBox = sender

        For Each Row As GridViewRow In gdvList.Rows
            Dim cbxErrorID As CheckBox = Row.FindControl("cbxErrorID")
            cbxErrorID.Checked = CheckBox.Checked
        Next
    End Sub

    Protected Sub btnDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Button As LinkButton = sender
        Dim ErrorID As Integer = Button.CommandArgument

        Dim ErrorData As DataTable = Emagine.GetDataTable("SELECT * FROM ErrorLog WHERE ErrorID = " & ErrorID)
        If ErrorData.Rows.Count > 0 Then
            pnlList.Visible = False
            pnlDetail.Visible = True

            lblErrorDate.Text = ErrorData.Rows(0).Item("ErrorDate").ToString
            lblErrorMessage.Text = ErrorData.Rows(0).Item("Message").ToString
            lblSource.Text = ErrorData.Rows(0).Item("Source").ToString
            lblStackTrace.Text = ErrorData.Rows(0).Item("StackTrace").ToString
            lblData.Text = ErrorData.Rows(0).Item("Data").ToString.Replace(vbCrLf, "<br />")
        End If
    End Sub

    Protected Sub gdvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ErrorID As Integer = e.Row.DataItem("ErrorID")

            'Dim cbxErrorID As CheckBox = e.Row.FindControl("cbxErrorID")
            Dim btnDetail As LinkButton = e.Row.FindControl("btnDetail")

            btnDetail.CommandArgument = ErrorID
        End If
    End Sub

    Protected Sub btnDeleteAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteAll.Click
        If Emagine.ExecuteSQL("DELETE FROM ErrorLog") Then
            gdvList.DataBind()
            lblAlert.Text = "The event log has been cleared successfully."

        Else
            gdvList.DataBind()
            lblAlert.Text = "An error occurred."
        End If

    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        pnlList.Visible = True
        pnlDetail.Visible = False
    End Sub

    Protected Sub btnDeleteSelected_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteSelected.Click
        Dim ErrorIDs As New StringBuilder

        For Each Row As GridViewRow In gdvList.Rows
            Dim cbxErrorID As CheckBox = Row.FindControl("cbxErrorID")
            If cbxErrorID.Checked Then
                Dim btnDetail As LinkButton = Row.FindControl("btnDetail")
                Dim ErrorID As Integer = btnDetail.CommandArgument

                ErrorIDs.Append(ErrorID & ",")
            End If
        Next

        If ErrorIDs.Length > 0 Then
            ErrorIDs.Remove(ErrorIDs.Length - 1, 1)
            If Emagine.ExecuteSQL("DELETE FROM ErrorLog WHERE ErrorID IN (" & ErrorIDs.ToString & ")") Then
                gdvList.DataBind()
                lblAlert.Text = "The events have been removed successfully."
            Else
                lblAlert.Text = "An error occurred while attepting to delete the selected events."
            End If
        End If
    End Sub
End Class
