
Partial Class Ezedit_Admin_Headers_Default
    Inherits System.Web.UI.Page

    Protected Sub DetailsView1_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewCommandEventArgs) Handles DetailsView1.ItemCommand
        If e.CommandName = "Cancel" Then
            pnlItemList.Visible = True
            DetailsView1.Visible = False
        End If
    End Sub


    Protected Sub DetailsView1_ItemInserted1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewInsertedEventArgs) Handles DetailsView1.ItemInserted
        If (Not e.Exception Is Nothing) Then
            lblAlert.Text = "An error occured while entering this record.  Please verify you have entered data in the correct format."
            e.ExceptionHandled = True
        Else
            lblAlert.Text = "The header has been added successfully."

            pnlItemList.Visible = True
            DetailsView1.Visible = False
            GridView1.DataBind()
        End If

    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "Delete" Then
            Emagine.ExecuteSQL("DELETE FROM TemplateHeaders WHERE HeaderID = " & e.CommandArgument)
            lblAlert.Text = "The header has been removed successfully."

            GridView1.DataBind()
        End If
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim HeaderID As Integer = DataBinder.Eval(e.Row.DataItem, "HeaderID")
            Dim DeleteButton As ImageButton = e.Row.FindControl("btnDelete")

            DeleteButton.CommandArgument = HeaderID
        End If
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        DetailsView1.Visible = True
        pnlItemList.Visible = False
        DetailsView1.HeaderText = "Edit Location"
        DetailsView1.ChangeMode(DetailsViewMode.Edit)
    End Sub

    Protected Sub DetailsView1_ItemUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewUpdatedEventArgs) Handles DetailsView1.ItemUpdated
        If (e.Exception IsNot Nothing) Then
            lblAlert.Text = "An error occured while entering this record.  Please verify you have entered data in the correct format."
            e.ExceptionHandled = True

        Else
            lblAlert.Text = "The header has been updated successfully."
            pnlItemList.Visible = True
            DetailsView1.Visible = False

            GridView1.DataBind()
        End If
    End Sub

    Protected Sub btnAdd1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAdd1.Click
        DetailsView1.Visible = True
        pnlItemList.Visible = False
        DetailsView1.HeaderText = "Add New Location"
        DetailsView1.ChangeMode(DetailsViewMode.Insert)
    End Sub

    Protected Sub btnAdd2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd2.Click
        DetailsView1.Visible = True
        pnlItemList.Visible = False
        DetailsView1.HeaderText = "Add New Location"
        DetailsView1.ChangeMode(DetailsViewMode.Insert)
    End Sub

    Sub ValidateUpdateData(ByVal Src As Object, ByVal Args As DetailsViewUpdateEventArgs)
        Dim ErrorBuilder As New StringBuilder
        Dim HeaderName As String = ""
        Dim FriendlyName As String = ""

        If Args.NewValues("HeaderName") IsNot Nothing Then HeaderName = Args.NewValues("HeaderName")
        If Args.NewValues("FriendlyName") IsNot Nothing Then FriendlyName = Args.NewValues("FriendlyName")

        If HeaderName.Trim.Length = 0 Then
            ErrorBuilder.Append("<li>Location Name</li>")
            Args.Cancel = True
        End If

        If FriendlyName.Trim.Length = 0 Then
            ErrorBuilder.Append("<li>Friendly Name</li>")
            Args.Cancel = True
        End If

        If Args.Cancel Then
            lblAlert.Text = "The following fields are required:" & ErrorBuilder.ToString '& "</ul>"
        End If
    End Sub

    Sub ValidateInsertData(ByVal Src As Object, ByVal Args As DetailsViewInsertEventArgs)
        Dim ErrorBuilder As New StringBuilder
        Dim HeaderName As String = ""
        Dim FriendlyName As String = ""

        If Args.Values("HeaderName") IsNot Nothing Then HeaderName = Args.Values("HeaderName")
        If Args.Values("FriendlyName") IsNot Nothing Then FriendlyName = Args.Values("FriendlyName")

        If HeaderName.Trim.Length = 0 Then
            ErrorBuilder.Append("<li>Location Name</li>")
            Args.Cancel = True
        End If

        If FriendlyName.Trim.Length = 0 Then
            ErrorBuilder.Append("<li>Friendly Name</li>")
            Args.Cancel = True
        End If

        If Args.Cancel Then
            lblAlert.Text = "The following fields are required:" & ErrorBuilder.ToString '& "</ul>"
        End If
    End Sub

End Class
