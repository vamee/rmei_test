
Partial Class Ezedit_Modules_LB01_Default
    Inherits System.Web.UI.Page

    Dim _ModuleKey As String = "LB01"

    Sub DisplayListPanel()
        txtResourceName.Text = ""
        rblIsEnabled.SelectedIndex = 0
        txtDisplayStartDate.Text = ""
        txtDisplayEndDate.Text = ""
        txtContent.EditorContent = ""
        hdnItemID.Value = -1

        gdvList.DataBind()

        pnlList.Visible = True
        pnlEdit.Visible = False
    End Sub

    Sub DisplayEditPanel(ByVal intItemID As Integer)
        Dim MyLibraryItem As LB01.LibraryItems.LibraryItem = LB01.LibraryItems.LibraryItem.GetLibraryItem(intItemID)
        If MyLibraryItem.ItemID > 0 Then
            txtResourceName.Text = MyLibraryItem.ResourceName
            rblIsEnabled.SelectedValue = MyLibraryItem.IsEnabled.ToString
            If MyLibraryItem.DisplayStartDate <> "1/1/1900" Then txtDisplayStartDate.Text = MyLibraryItem.DisplayStartDate
            If MyLibraryItem.DisplayEndDate <> "1/1/1900" Then txtDisplayEndDate.Text = MyLibraryItem.DisplayEndDate
            txtContent.EditorContent = MyLibraryItem.Content
            hdnItemID.Value = intItemID
        Else
            txtResourceName.Text = ""
            rblIsEnabled.SelectedValue = "True"
            txtDisplayStartDate.Text = ""
            txtDisplayEndDate.Text = ""
            txtContent.EditorContent = ""
            hdnItemID.Value = 0
        End If

        pnlList.Visible = False
        pnlEdit.Visible = True
        txtResourceName.Focus()
    End Sub

    Protected Sub lbtnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnAddNew.Click
        Me.DisplayEditPanel(0)
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DisplayListPanel()
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If PeterBlum.VAM.Globals.Page.IsValid Then
            Dim ItemID As Integer = Emagine.GetNumber(hdnItemID.Value)
            Dim MyLibraryItem As New LB01.LibraryItems.LibraryItem

            If ItemID > 0 Then
                MyLibraryItem = LB01.LibraryItems.LibraryItem.GetLibraryItem(ItemID)
                MyLibraryItem.ResourceName = txtResourceName.Text
                MyLibraryItem.IsEnabled = rblIsEnabled.SelectedValue
                If txtDisplayStartDate.Text.Length > 0 Then MyLibraryItem.DisplayStartDate = txtDisplayStartDate.Text
                If txtDisplayEndDate.Text.Length > 0 Then MyLibraryItem.DisplayEndDate = txtDisplayEndDate.Text
                MyLibraryItem.Content = txtContent.EditorContent
                MyLibraryItem.UpdatedBy = Session("EzEditUsername")

                If LB01.LibraryItems.LibraryItem.Update(MyLibraryItem) Then
                    lblAlert.Text = "The item has been updated successfully."
                    Me.DisplayListPanel()
                Else
                    lblAlert.Text = "An error has occurred while attempting to save this item."
                End If
            Else
                MyLibraryItem.ResourceName = txtResourceName.Text
                MyLibraryItem.IsEnabled = rblIsEnabled.SelectedValue
                If txtDisplayStartDate.Text.Length > 0 Then MyLibraryItem.DisplayStartDate = txtDisplayStartDate.Text
                If txtDisplayEndDate.Text.Length > 0 Then MyLibraryItem.DisplayEndDate = txtDisplayEndDate.Text
                MyLibraryItem.Content = txtContent.EditorContent
                MyLibraryItem.CreatedBy = Session("EzEditUsername")
                MyLibraryItem.UpdatedBy = Session("EzEditUsername")

                ItemID = LB01.LibraryItems.LibraryItem.Add(MyLibraryItem).ItemID

                If ItemID > 0 Then
                    lblAlert.Text = "The item has been added successfully."
                    Me.DisplayListPanel()
                Else
                    lblAlert.Text = "An error has occurred while attempting to save this item."
                End If

            End If

        End If
    End Sub

    Protected Sub cbxIsEnabled_CheckChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cbxIsEnabled As CheckBox = sender
        Dim btnEdit As ImageButton = cbxIsEnabled.Parent.Parent.FindControl("btnEdit")
        Dim ItemID As Integer = Emagine.GetNumber(btnEdit.CommandArgument)
        Dim EnabledText As String = "enabled"

        If Not cbxIsEnabled.Checked Then EnabledText = "disabled"

        If ItemID > 0 Then
            Dim MyLibraryItem As LB01.LibraryItems.LibraryItem = LB01.LibraryItems.LibraryItem.GetLibraryItem(ItemID)
            MyLibraryItem.IsEnabled = cbxIsEnabled.Checked
            MyLibraryItem.UpdatedBy = Session("EzEditUsername")
            If LB01.LibraryItems.LibraryItem.Update(MyLibraryItem) Then
                lblAlert.Text = "The item has been " & EnabledText & " successfully."
                Me.DisplayListPanel()
            Else
                lblAlert.Text = "An error occurred."
            End If
        End If
    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim ItemID As Integer = Button.CommandArgument

        Me.DisplayEditPanel(ItemID)
    End Sub

    Protected Sub btnAssign_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim ItemID As Integer = Button.CommandArgument

        Response.Redirect("PageAssignments.aspx?ItemID=" & ItemID)
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim ItemID As Integer = Button.CommandArgument

        If LB01.LibraryItems.LibraryItem.Delete(ItemID) Then

            Me.DisplayListPanel()
            lblAlert.Text = "The item has been removed successfully."
        Else
            lblAlert.Text = "An error occurred while attempting to delete this item."
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblAlert.Text = ""

        If Session("Alert") IsNot Nothing Then
            lblAlert.Text = Session("Alert")
            Session("Alert") = ""
        End If
    End Sub

    Protected Sub gdvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim DisplayStartDate As Date = e.Row.DataItem("DisplayStartDate").ToString
            Dim DisplayEndDate As Date = e.Row.DataItem("DisplayEndDate").ToString
            Dim IsEnabled As Boolean = e.Row.DataItem("IsEnabled")

            Dim lblStatus As Label = e.Row.FindControl("lblStatus")
            Dim lblDisplayDates As Label = e.Row.FindControl("lblDisplayDates")

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
        End If
    End Sub
End Class
