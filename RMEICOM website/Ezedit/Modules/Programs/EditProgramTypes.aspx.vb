
Partial Class Ezedit_Modules_Programs_ProgramTypes
    Inherits System.Web.UI.Page

    Dim _ModuleKey As String = "Custom_ProgramTypes"

#Region "On Load"

#End Region

#Region "Events"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        Me.DisplayEditPanel(0)
    End Sub

    Protected Sub btnEditItem_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Button As ImageButton = sender
        Dim ItemID As String = Button.CommandArgument

        Me.DisplayEditPanel(ItemID)
    End Sub

    Protected Sub btnDeleteItem_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Result As Boolean = False
        Dim ErrorMessage As String = ""
        Dim Button As ImageButton = sender
        Dim ItemID As String = Button.CommandArgument

        Dim MyCommand As New SqlCommand
        MyCommand.Parameters.AddWithValue("@ProgramTypeID", ItemID)

        ItemID = Emagine.GetDbValue("SELECT ProgramTypeID FROM Custom_ProgramTypes WHERE ProgramTypeID = @ProgramTypeID", MyCommand, ErrorMessage)

        If ItemID.Length > 0 Then
            Dim Sql As String = "DELETE FROM Resources WHERE ResourceID IN (SELECT ResourceID FROM Custom_ProgramTypes WHERE ProgramTypeID = @ProgramTypeID)"
            Emagine.ExecuteSQL(Sql, MyCommand)

            Sql = "DELETE FROM Custom_ProgramTypes WHERE ProgramTypeID = @ProgramTypeID"
            Result = Emagine.ExecuteSQL(Sql, MyCommand, ErrorMessage)

            If Result Then
                Me.ResetSortOrder()
                gdvList.DataBind()
                lblAlert.Text = "The item has been removed successfully."
            Else
                lblAlert.Text = "An error occurred: " & ErrorMessage
            End If

        Else
            lblAlert.Text = "An error occurred: " & ErrorMessage
        End If
    End Sub

    Protected Sub cbxIsEnabled_CheckChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Result As Boolean = False
        Dim ErrorMessage As String = ""
        Dim cbxIsEnabled As CheckBox = sender
        Dim btnEdit As ImageButton = cbxIsEnabled.Parent.Parent.FindControl("btnEditItem")
        Dim ItemID As String = btnEdit.CommandArgument
        Dim EnabledText As String = "enabled"

        If Not cbxIsEnabled.Checked Then EnabledText = "disabled"

        Dim MyCommand As New SqlCommand
        MyCommand.Parameters.AddWithValue("@ProgramTypeID", ItemID)
        MyCommand.Parameters.AddWithValue("@IsEnabled", cbxIsEnabled.Checked)

        ItemID = Emagine.GetDbValue("SELECT ProgramTypeID FROM Custom_ProgramTypes WHERE ProgramTypeID = @ProgramTypeID", MyCommand, ErrorMessage)

        If ItemID.Length > 0 Then
            Dim Sql As String = "UPDATE Resources SET IsEnabled = @IsEnabled WHERE ResourceID IN (SELECT ResourceID FROM Custom_ProgramTypes WHERE ProgramTypeID = @ProgramTypeID)"

            If Emagine.ExecuteSQL(Sql, MyCommand, ErrorMessage) Then
                lblAlert.Text = "The item has been " & EnabledText & " successfully."
                Me.DisplayListPanel()
            Else
                lblAlert.Text = "An error occurred: " & ErrorMessage
            End If
        End If
    End Sub

    Protected Sub ddlSortOrder_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ddlSortOrder As DropDownList = sender
        Dim Row As GridViewRow = ddlSortOrder.Parent.Parent
        Dim btnEdit As ImageButton = Row.FindControl("btnEditItem")
        Dim hdnSortOrder As HiddenField = Row.FindControl("hdnSortOrder")
        Dim ItemID As String = btnEdit.CommandArgument
        Dim OldSortOrder As Integer = Emagine.GetNumber(hdnSortOrder.Value)
        Dim NewSortOrder As Integer = Emagine.GetNumber(ddlSortOrder.SelectedValue)
        Dim ErrorMessage As String = ""
        Dim Sql As String = ""

        Dim MyCommand As New SqlCommand
        MyCommand.Parameters.AddWithValue("@ProgramTypeID", ItemID)
        MyCommand.Parameters.AddWithValue("@ResourceType", _ModuleKey)
        MyCommand.Parameters.AddWithValue("@OldSortOrder", OldSortOrder)
        MyCommand.Parameters.AddWithValue("@NewSortOrder", NewSortOrder)

        ItemID = Emagine.GetDbValue("SELECT ProgramTypeID FROM Custom_ProgramTypes WHERE ProgramTypeID = @ProgramTypeID", MyCommand, ErrorMessage)

        If ItemID.Length > 0 Then
            If OldSortOrder > NewSortOrder Then
                Sql = "UPDATE Resources SET SortOrder = SortOrder + 1 WHERE ResourceType = @ResourceType AND ResourceID IN (SELECT ResourceID FROM Custom_ProgramTypes WHERE ProgramTypeID <> @ProgramTypeID) AND SortOrder >= @NewSortOrder AND SortOrder < @OldSortOrder"
            Else
                Sql = "UPDATE Resources SET SortOrder = SortOrder - 1 WHERE ResourceType = @ResourceType AND  ResourceID IN (SELECT ResourceID FROM Custom_ProgramTypes WHERE ProgramTypeID <> @ProgramTypeID) AND SortOrder <= @NewSortOrder AND SortOrder > @OldSortOrder"
            End If

            Emagine.ExecuteSQL(Sql, MyCommand)

            Emagine.ExecuteSQL("UPDATE Resources SET SortOrder = @NewSortOrder WHERE ResourceID IN (SELECT ResourceID FROM Custom_ProgramTypes WHERE ProgramTypeID = @ProgramTypeID)", MyCommand)

            Me.ResetSortOrder()

            gdvList.DataBind()

            lblAlert.Text = "The items have been sorted successfully."
        Else
            lblAlert.Text = "An error occurred: " & ErrorMessage
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If PeterBlum.VAM.Globals.Page.IsValid Then
            Dim Result As Boolean = False
            Dim ErrorMessage As String = ""
            Dim ItemID As String = hdnItemID.Value

            Dim MyCommand As New SqlCommand
            MyCommand.Parameters.AddWithValue("@ProgramTypeID", ItemID)
            MyCommand.Parameters.AddWithValue("@IconUrl", fbIconUrl.VirtualFilePath)

            Dim SqlBuilder As New StringBuilder

            ItemID = Emagine.GetDbValue("SELECT ProgramTypeID FROM Custom_ProgramTypes WHERE ProgramTypeID = @ProgramTypeID", MyCommand)

            If ItemID.Length > 0 Then
                SqlBuilder.Append("UPDATE Custom_ProgramTypes SET IconUrl = @IconUrl WHERE ProgramTypeID = @ProgramTypeID")

                If Emagine.ExecuteSQL(SqlBuilder.ToString, MyCommand, ErrorMessage) Then
                    Dim ResourceID As String = Emagine.GetDbValue("SELECT ResourceID FROM Custom_ProgramTypes WHERE ProgramTypeID = @ProgramTypeID", MyCommand)
                    Dim MyResource As Resources.Resource = Resources.Resource.GetResource(ResourceID)
                    MyResource.ResourceName = txtResourceName.Text
                    MyResource.UpdatedDate = Date.Now()
                    MyResource.UpdatedBy = Session("EzEditUsername")

                    Result = Resources.Resource.UpdateResource(MyResource, ErrorMessage)
                End If

                lblAlert.Text = "The program type has been updated successfully."

            Else
                Dim MyResource As New Resources.Resource
                MyResource.ResourceID = Emagine.GetUniqueID()
                MyResource.ResourceType = _ModuleKey
                MyResource.ResourceName = txtResourceName.Text
                MyResource.SortOrder = Me.GetMaxSortOrder() + 1
                MyResource.CreatedBy = Session("EzEditUsername")
                MyResource.UpdatedBy = Session("EzEditUsername")

                ItemID = Emagine.GetUniqueID()
                MyCommand.Parameters("@ProgramTypeID").Value = ItemID
                MyCommand.Parameters.AddWithValue("@ResourceID", MyResource.ResourceID)

                SqlBuilder.Append("INSERT INTO Custom_ProgramTypes ")
                SqlBuilder.Append("(ProgramTypeID,ResourceID,IconUrl) ")
                SqlBuilder.Append("VALUES ")
                SqlBuilder.Append("(@ProgramTypeID,@ResourceID,@IconUrl)")

                If Emagine.ExecuteSQL(SqlBuilder.ToString, MyCommand, ErrorMessage) Then
                    Result = Resources.Resource.AddResource(MyResource, ErrorMessage)
                    If Result Then Me.ResetSortOrder()
                End If

                lblAlert.Text = "The program type has been added successfully."
            End If

            If Result Then
                Me.ResetEditForm()
                Me.DisplayListPanel()
            Else
                lblAlert.Text = "An error occurred: " & ErrorMessage
            End If

        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.ResetEditForm()
        Me.DisplayListPanel()
    End Sub

    Protected Sub gdvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ItemID As String = e.Row.DataItem("ProgramTypeID")
            Dim IsEnabled As Boolean = e.Row.DataItem("IsEnabled")
            Dim SortOrder As Integer = e.Row.DataItem("SortOrder")

            Dim lblStatus As Label = e.Row.FindControl("lblStatus")
            Dim cbxIsEnabled As CheckBox = e.Row.FindControl("cbxIsEnabled")
            Dim hdnSortOrder As HiddenField = e.Row.FindControl("hdnSortOrder")
            Dim ddlSortOrder As DropDownList = e.Row.FindControl("ddlSortOrder")
            Dim btnEditItem As ImageButton = e.Row.FindControl("btnEditItem")
            Dim btnDeleteItem As ImageButton = e.Row.FindControl("btnDeleteItem")

            If Not IsEnabled Then
                lblStatus.Text = "DISABLED"
                lblStatus.BackColor = Drawing.Color.Red
                lblStatus.ForeColor = Drawing.Color.White
                lblStatus.ToolTip = "Check the 'Enabled' box to enable this item."

            Else
                lblStatus.Text = "CURRENT"
                lblStatus.BackColor = Drawing.Color.Green
                lblStatus.ForeColor = Drawing.Color.White
                lblStatus.ToolTip = "This item is current and displaying on the website."
            End If

            hdnSortOrder.Value = SortOrder
            Me.BindSortOrder(ddlSortOrder, SortOrder)
            cbxIsEnabled.Checked = IsEnabled
            btnEditItem.CommandArgument = ItemID
            btnDeleteItem.CommandArgument = ItemID
        End If
    End Sub
#End Region


#Region "Custom Procedures"
    Sub DisplayListPanel()
        gdvList.DataBind()
        pnlList.Visible = True
        pnlEdit.Visible = False
    End Sub

    Sub DisplayEditPanel(ByVal strProgramTypeID As String)
        If strProgramTypeID.Length > 0 Then
            Dim MyCommand As New SqlCommand
            MyCommand.Parameters.AddWithValue("@ProgramTypeID", strProgramTypeID)

            Dim Sql As String = "SELECT * FROM qryCustom_ProgramTypes WHERE ProgramTypeID = @ProgramTypeID"

            Dim MyProgramTypeData As DataTable = Emagine.GetDataTable(Sql, MyCommand)
            If MyProgramTypeData.Rows.Count > 0 Then
                hdnItemID.Value = MyProgramTypeData.Rows(0).Item("ProgramTypeID").ToString
                txtResourceName.Text = MyProgramTypeData.Rows(0).Item("ResourceName").ToString
                fbIconUrl.VirtualFilePath = MyProgramTypeData.Rows(0).Item("IconUrl").ToString
            Else
                Me.ResetEditForm()
            End If
            MyProgramTypeData.Dispose()

        Else
            Me.ResetEditForm()
        End If

        pnlList.Visible = False
        pnlEdit.Visible = True
        txtResourceName.Focus()
    End Sub

    Sub ResetEditForm()
        txtResourceName.Text = ""
        fbIconUrl.VirtualFilePath = ""
        hdnItemID.Value = ""
    End Sub

    Function GetMaxSortOrder() As Integer
        Dim MyCommand As New SqlCommand
        MyCommand.Parameters.AddWithValue("@ResourceType", _ModuleKey)

        Dim Sql As String = "SELECT COUNT(*) AS RecordCount FROM Resources WHERE ResourceType = @ResourceType"

        Return Emagine.GetNumber(Emagine.GetDbValue(Sql, MyCommand))
    End Function

    Sub ResetSortOrder()
        Dim MyDataTable As DataTable = Emagine.GetDataTable("SELECT * FROM qryCustom_ProgramTypes ORDER BY SortOrder")
        For i As Integer = 0 To MyDataTable.Rows.Count - 1
            Dim MyCommand As New SqlCommand
            MyCommand.Parameters.AddWithValue("@ResourceID", MyDataTable.Rows(i).Item("ResourceID").ToString)
            MyCommand.Parameters.AddWithValue("@SortOrder", (i + 1))

            Dim Sql As String = "UPDATE Resources SET SortOrder = @SortOrder WHERE ResourceID = @ResourceID"
            Emagine.ExecuteSQL(Sql, MyCommand)
        Next
        MyDataTable.Dispose()
    End Sub

    Sub BindSortOrder(ByVal ddlSortOrder As DropDownList, ByVal intSortOrder As Integer)
        Dim MyCommand As New SqlCommand
        MyCommand.Parameters.AddWithValue("@ResourceType", _ModuleKey)

        Dim Sql As String = "SELECT COUNT(*) AS RecordCount FROM Resources WHERE ResourceType = @ResourceType"

        Dim MaxSortOrder As Integer = Emagine.GetNumber(Emagine.GetDbValue(Sql, MyCommand))

        For i As Integer = 1 To MaxSortOrder
            Dim ListItem As New ListItem(i.ToString, i.ToString)
            If i = intSortOrder Then ListItem.Selected = True
            ddlSortOrder.Items.Add(ListItem)
        Next
    End Sub
#End Region

    
End Class
