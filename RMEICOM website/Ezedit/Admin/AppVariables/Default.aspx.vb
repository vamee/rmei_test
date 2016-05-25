
Partial Class Ezedit_Admin_AppVariables_Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblAlert.Text = ""
    End Sub

    Sub DisplayListPanel()
        gdvList.DataBind()
        pnlList.Visible = True
        pnlEdit.Visible = False
    End Sub

    Sub DisplayEditPanel(ByVal intItemID As Integer)
        Me.ResetEditForm()
        Dim VariableData As DataTable = Emagine.GetDataTable("SELECT * FROM ApplicationVariables WHERE VariableID = " & intItemID)

        If VariableData.Rows.Count > 0 Then
            txtVariableName.Text = VariableData.Rows(0).Item("VariableName").ToString
            txtVariableValue.Text = VariableData.Rows(0).Item("VariableValue").ToString
            txtDescription.Text = VariableData.Rows(0).Item("Description").ToString
        End If

        hdnItemID.Value = intItemID
        pnlList.Visible = False
        pnlEdit.Visible = True
    End Sub

    Sub ResetEditForm()
        txtVariableName.Text = ""
        txtVariableValue.Text = ""
        txtDescription.Text = ""
    End Sub


    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If PeterBlum.VAM.Globals.Page.IsValid Then
            Dim ErrorMessage As String = ""
            Dim ItemID As Integer = hdnItemID.Value

            Dim SqlBuilder As New StringBuilder
            Dim Command As New System.Data.SqlClient.SqlCommand
            Command.Parameters.AddWithValue("@VariableID", ItemID)
            Command.Parameters.AddWithValue("@VariableName", txtVariableName.Text)
            Command.Parameters.AddWithValue("@VariableValue", txtVariableValue.Text)
            Command.Parameters.AddWithValue("@Description", txtDescription.Text)
            Command.Parameters.AddWithValue("@CreatedBy", Session("EzEditName"))
            Command.Parameters.AddWithValue("@UpdatedDate", Date.Now)
            Command.Parameters.AddWithValue("@UpdatedBy", Session("EzEditName"))

            If ItemID > 0 Then
                SqlBuilder.Append("UPDATE ApplicationVariables SET ")
                SqlBuilder.Append("VariableName = @VariableName, ")
                SqlBuilder.Append("VariableValue = @VariableValue, ")
                SqlBuilder.Append("Description = @Description, ")
                SqlBuilder.Append("UpdatedDate = @UpdatedDate, ")
                SqlBuilder.Append("UpdatedBy = @UpdatedBy ")
                SqlBuilder.Append("WHERE VariableID = @VariableID")

                lblAlert.Text = "The variable has been updated successfully."
            Else
                SqlBuilder.Append("INSERT INTO ApplicationVariables ")
                SqlBuilder.Append("(VariableName, VariableValue, Description, CreatedBy, UpdatedBy) ")
                SqlBuilder.Append("VALUES ")
                SqlBuilder.Append("(@VariableName, @VariableValue, @Description, @CreatedBy, @UpdatedBy)")

                lblAlert.Text = "The variable has been added successfully."
            End If

            If Emagine.ExecuteSQL(SqlBuilder.ToString, Command, ErrorMessage) Then
                Me.DisplayListPanel()
                Me.ResetEditForm()
            Else
                lblAlert.Text = "An error occurred: " & ErrorMessage
            End If
        End If
    End Sub

    Protected Sub ibtnAddNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnAddNew.Click
        Me.DisplayEditPanel(0)
    End Sub

    Protected Sub lbtnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnAddNew.Click
        Me.DisplayEditPanel(0)
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DisplayListPanel()
    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim ItemID As Integer = Button.CommandArgument

        Me.DisplayEditPanel(ItemID)
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim ErrorMessage As String = ""
        Dim Button As ImageButton = sender
        Dim ItemID As Integer = Button.CommandArgument

        If Emagine.ExecuteSQL("DELETE FROM ApplicationVariables WHERE VariableID = " & ItemID, ErrorMessage) Then
            lblAlert.Text = "The variable has been removed successfully."
            Me.DisplayListPanel()
        Else
            lblAlert.Text = "An error occurred: " & ErrorMessage
        End If
    End Sub

End Class
