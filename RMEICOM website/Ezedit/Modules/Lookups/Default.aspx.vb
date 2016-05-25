
Partial Class Ezedit_Modules_Lookups_Default
    Inherits System.Web.UI.Page

    Dim OptionCount As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            pnlList.Visible = True
        End If

        lblAlert.Text = ""
        cvLookupName.ServerCondition = New PeterBlum.VAM.ServerConditionEventHandler(AddressOf cvLookupName_Validate)
    End Sub

    Protected Sub btnEditItem_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Button As ImageButton = sender
        Me.DisplayEditPanel(Button.CommandArgument)
    End Sub

    Protected Sub btnDeleteItem_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Button As ImageButton = sender
        Dim LookupID As Integer = Button.CommandArgument

        If Lookup.Delete(LookupID) Then
            lblAlert.Text = "The lookup has been removed successfully."
            Me.DisplayListPanel()
        Else
            lblAlert.Text = "An error occurred while processing your request."
        End If
    End Sub

    Protected Sub ibtnAddNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnAddNew.Click
        Me.DisplayEditPanel(0)
    End Sub

    Protected Sub lbtnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnAddNew.Click
        Me.DisplayEditPanel(0)
    End Sub

    Protected Sub btnAddOptions_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        hdnAction.Value = "AddOptions"

        trBulkView.Visible = True
        trItemView.Visible = False
        trLookupName.Visible = False
        trValueFieldType.Visible = False
    End Sub

    Protected Sub btnDeleteOption_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Button As ImageButton = sender
        Dim OptionID As Integer = Button.CommandArgument

        If LookupOption.Delete(OptionID) Then
            LookupOption.ResetSortOrder(hdnItem.Value)
            Me.DisplayEditPanel(hdnItem.Value)
            lblAlert.Text = "The option has been removed successfully."
        Else
            lblAlert.Text = "An error occurred while processing your request."
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DisplayListPanel()
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim LookupID As Integer = Emagine.GetNumber(hdnItem.Value.ToString)

        Dim Result As Boolean = False
        If LookupID > 0 And hdnAction.Value = "AddOptions" Then
            Dim objLookup As Lookup = Lookup.GetLookup(LookupID)
            Dim SortOrder As Integer = LookupOption.GetMaxSortOrder(LookupID)

            Dim aryOptions() As String = Split(txtBulkValues.Text, vbCrLf)
            For i As Integer = 0 To aryOptions.GetUpperBound(0)
                SortOrder += 1
                Dim aryOptionValue As Array = aryOptions(i).Split("^")
                Dim OptionText As String = aryOptionValue(0)
                Dim OptionValue As String = OptionText
                If aryOptionValue.GetUpperBound(0) > 0 Then
                    OptionValue = aryOptionValue(1)
                End If

                Dim NewOption As New LookupOption
                NewOption.LookupID = LookupID
                NewOption.OptionText = OptionText
                If objLookup.ValueFieldType.IndexOf("AutoNumber") > -1 Then
                    NewOption.OptionValue = SortOrder
                Else
                    NewOption.OptionValue = OptionValue
                End If
                NewOption.SortOrder = SortOrder
                Result = LookupOption.Add(NewOption)

                If Result = False Then Exit For
            Next

            If Result = True Then
                gdvItems.DataSource = LookupOptions.GetOptions(LookupID)
                gdvItems.DataBind()

                Me.DisplayEditPanel(LookupID)
                hdnAction.Value = ""
            Else
                cvLookupName.SummaryErrorMessage = "An error occurred while updating the lookup."
            End If
        End If
    End Sub

    Protected Sub cvLookupName_Validate(ByVal sourceCondition As PeterBlum.VAM.BaseCondition, ByVal args As PeterBlum.VAM.ConditionEventArgs)
        Dim IsValid As Boolean = False
        Dim LookupID As Integer = Emagine.GetNumber(hdnItem.Value.ToString)

        If LookupID = 0 Then
            Dim RecordCount As Integer = Emagine.GetNumber(Emagine.GetDbValue("SELECT COUNT(*) As RecordCount FROM Lookups WHERE LookupName = '" & txtLookupName.Text.Replace("'", "''") & "'"))
            If RecordCount = 0 Then IsValid = True
        Else
            IsValid = True
        End If

        If IsValid Then
            Dim Result As Boolean = False
            If LookupID > 0 Then
                Dim objLookup As Lookup = Lookup.GetLookup(LookupID)
                objLookup.LookupName = txtLookupName.Text
                objLookup.ValueFieldType = ddlValueFieldType.SelectedValue
                Result = Lookup.Update(objLookup)

                If Result Then
                    For i As Integer = 0 To (gdvItems.Rows.Count - 1)
                        If gdvItems.Rows(i).RowType = DataControlRowType.DataRow Then
                            Dim hdnOptionID As HiddenField = gdvItems.Rows(i).FindControl("hdnOptionID")
                            Dim txtOptionText As TextBox = gdvItems.Rows(i).FindControl("txtOptionText")
                            Dim txtOptionValue As TextBox = gdvItems.Rows(i).FindControl("txtOptionValue")

                            Dim objOption As New LookupOption
                            objOption.OptionID = hdnOptionID.Value
                            objOption.OptionText = txtOptionText.Text
                            objOption.OptionValue = txtOptionValue.Text
                            objOption.SortOrder = i + 1
                            Result = LookupOption.Update(objOption)

                            If Not Result Then Exit For
                        End If
                    Next
                End If

                If Result = True Then
                    args.IsMatch = True
                    Me.DisplayListPanel()
                Else
                    cvLookupName.SummaryErrorMessage = "An error occurred while updating the lookup."
                    args.IsMatch = False
                End If

            Else
                Dim NewLookup As New Lookup
                NewLookup.LookupName = txtLookupName.Text
                NewLookup = Lookup.Add(NewLookup)
                LookupID = NewLookup.LookupID

                If LookupID > -1 Then
                    Dim aryOptions() As String = Split(txtBulkValues.Text, vbCrLf)
                    For i As Integer = 0 To aryOptions.GetUpperBound(0)
                        Dim aryOptionValue As Array = aryOptions(i).Split("^")
                        Dim OptionText As String = aryOptionValue(0)
                        Dim OptionValue As String = OptionText
                        If aryOptionValue.GetUpperBound(0) > 0 Then
                            OptionValue = aryOptionValue(1)
                        End If

                        Dim NewOption As New LookupOption
                        NewOption.LookupID = NewLookup.LookupID
                        NewOption.OptionText = OptionText
                        NewOption.OptionValue = OptionValue
                        NewOption.SortOrder = (i + 1)
                        Result = LookupOption.Add(NewOption)

                        If Result = False Then Exit For
                    Next
                End If

                If Result = True Then
                    gdvItems.DataSource = LookupOptions.GetOptions(LookupID)
                    gdvItems.DataBind()

                    Me.DisplayEditPanel(LookupID)
                    args.IsMatch = True
                Else
                    cvLookupName.SummaryErrorMessage = "An error occurred while updating the lookup."
                    args.IsMatch = False
                End If
            End If



        Else
            args.IsMatch = False
        End If
    End Sub

    Sub DisplayEditPanel(ByVal intItemID As Integer)
        pnlList.Visible = False
        pnlEdit.Visible = True
        hdnItem.Value = intItemID

        If intItemID > 0 Then
            trValueFieldType.Visible = True
            Dim Lookup As Lookup = Lookup.GetLookup(intItemID)
            txtLookupName.Text = Lookup.LookupName

            For Each Item As ListItem In ddlValueFieldType.Items
                If Item.Value = Lookup.ValueFieldType Then
                    Item.Selected = True
                Else
                    Item.Selected = False
                End If
            Next

            txtLookupName.Enabled = Lookup.CanEditLookup
            ddlValueFieldType.Enabled = Lookup.CanEditLookup

            OptionCount = Emagine.GetNumber(Emagine.GetDbValue("SELECT COUNT(OptionID) AS OptionCount FROM LookupOptions WHERE LookupID = " & intItemID))

            gdvItems.DataSource = LookupOptions.GetOptions(intItemID)
            gdvItems.DataBind()

            trLookupName.Visible = True
            trValueFieldType.Visible = True
            trItemView.Visible = True
            trBulkView.Visible = False
        Else
            txtLookupName.Text = ""
            trLookupName.Visible = True
            trItemView.Visible = False
            trBulkView.Visible = True
            trValueFieldType.Visible = False
        End If
    End Sub

    Sub DisplayListPanel()
        pnlList.Visible = True
        pnlEdit.Visible = False
        trItemView.Visible = False
        trBulkView.Visible = False
        trValueFieldType.Visible = False
        hdnItem.Value = 0

        txtLookupName.Text = ""
        ddlValueFieldType.SelectedIndex = -1
        txtBulkValues.Text = ""
        txtLookupName.Enabled = True
        ddlValueFieldType.Enabled = True

        gdvList.DataBind()
    End Sub

    Protected Sub gdvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim LookupID As Integer = e.Row.DataItem("LookupID")
            Dim objLookup As Lookup = Lookup.GetLookup(LookupID)

            Dim btnEditItem As ImageButton = e.Row.FindControl("btnEditItem")
            Dim btnDeleteItem As ImageButton = e.Row.FindControl("btnDeleteItem")

            btnEditItem.Visible = objLookup.CanEditLookup
            btnDeleteItem.Visible = objLookup.CanDeleteLookup

            btnEditItem.CommandArgument = LookupID
            btnDeleteItem.CommandArgument = LookupID
        End If
    End Sub

    Protected Sub gdvItems_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvItems.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim OptionID As Integer = e.Row.DataItem("OptionID")
            Dim OptionText As String = e.Row.DataItem("OptionText").ToString
            Dim OptionValue As String = e.Row.DataItem("OptionValue").ToString
            Dim SortOrder As Integer = e.Row.DataItem("SortOrder")
            Dim ValueFieldType As String = e.Row.DataItem("ValueFieldType").ToString

            Dim txtOptionText As TextBox = e.Row.FindControl("txtOptionText")
            Dim txtOptionValue As TextBox = e.Row.FindControl("txtOptionValue")
            Dim ddlSortOrder As DropDownList = e.Row.FindControl("ddlSortOrder")
            Dim hdnSortOrder As HiddenField = e.Row.FindControl("hdnSortOrder")
            Dim hdnOptionID As HiddenField = e.Row.FindControl("hdnOptionID")
            Dim btnDeleteOption As ImageButton = e.Row.FindControl("btnDeleteOption")

            txtOptionText.Text = OptionText
            txtOptionValue.Text = OptionValue

            If ValueFieldType.IndexOf("AutoNumber") > -1 Then
                txtOptionValue.Width = 30
                txtOptionValue.Enabled = False
            ElseIf ValueFieldType.IndexOf("Sync") > -1 Then
                txtOptionValue.Enabled = False
            End If

            ddlSortOrder = Me.PopulateSortOrder(ddlSortOrder, SortOrder)

            hdnSortOrder.Value = SortOrder
            hdnOptionID.Value = OptionID
            btnDeleteOption.CommandArgument = OptionID
        End If
    End Sub

    Function PopulateSortOrder(ByVal ddlSortOrder As DropDownList, ByVal intSortOrder As Integer) As DropDownList
        For i As Integer = 1 To OptionCount
            Dim Item As New ListItem(i, i)
            If i = intSortOrder Then Item.Selected = True

            ddlSortOrder.Items.Add(Item)
        Next

        Return ddlSortOrder
    End Function

    Protected Sub ddlSortOrder_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ddlSortOrder As DropDownList = sender
        Dim hdnOptionID As HiddenField = ddlSortOrder.Parent.Parent.FindControl("hdnOptionID")
        Dim OptionID As Integer = Emagine.GetNumber(hdnOptionID.Value)
        Dim SortOrder As Integer = ddlSortOrder.SelectedValue

        LookupOption.UpdateSortOrder(OptionID, SortOrder)

        Me.DisplayEditPanel(hdnItem.Value)
    End Sub

    Protected Sub ddlValueFieldType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlValueFieldType.SelectedIndexChanged
        Dim objLookup As Lookup = Lookup.GetLookup(CInt(hdnItem.Value))

        For i As Integer = 0 To (gdvItems.Rows.Count - 1)
            If gdvItems.Rows(i).RowType = DataControlRowType.DataRow Then
                Dim hdnOptionID As HiddenField = gdvItems.Rows(i).FindControl("hdnOptionID")
                Dim txtOptionText As TextBox = gdvItems.Rows(i).FindControl("txtOptionText")
                Dim txtOptionValue As TextBox = gdvItems.Rows(i).FindControl("txtOptionValue")
                Dim ddlSortOrder As DropDownList = gdvItems.Rows(i).FindControl("ddlSortOrder")

                If ddlValueFieldType.SelectedValue.IndexOf("AutoNumber") > -1 Then
                    If objLookup.ValueFieldType.IndexOf("AutoNumber") > -1 Then
                        txtOptionValue.Text = Emagine.GetDbValue("SELECT OptionValue FORM LookupOptions WHERE OptionID = " & hdnOptionID.Value)
                    Else
                        txtOptionValue.Text = ddlSortOrder.SelectedValue
                    End If

                    txtOptionValue.Enabled = False
                    txtOptionValue.Width = 30

                ElseIf ddlValueFieldType.SelectedValue.IndexOf("Sync") > -1 Then
                    txtOptionValue.Text = txtOptionText.Text
                    txtOptionValue.Enabled = False
                    txtOptionValue.Width = 250
                Else
                    txtOptionValue.Text = Emagine.GetDbValue("SELECT OptionValue FROM LookupOptions WHERE OptionID = " & hdnOptionID.Value)
                    txtOptionValue.Enabled = True
                    txtOptionValue.Width = 250
                End If
            End If
        Next

    End Sub


End Class
