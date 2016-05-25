
Partial Class Ezedit_Modules_Forms01_EditForm
    Inherits System.Web.UI.Page

    Dim FieldID As Integer = Emagine.GetNumber(HttpContext.Current.Request("FieldID"))
    Dim FormID As Integer = Emagine.GetNumber(HttpContext.Current.Request("FormID"))
    Dim FormName As String = Emagine.GetDbValue("SELECT FormName FROM Forms WHERE FormID = " & FormID)
    Dim SalesForceService As SalesForceApi.SforceService

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If GlobalVariables.GetValue("SalesForceIntegrationType") = "API" Then
            SalesForceService = SalesForceConnector.GetSalesForceService()
        End If

        If Not IsPostBack Then
            BindControls(FieldID)
        End If

        DisplayFieldProperties(FieldID)
    End Sub

    'Sub EditField(ByVal intFieldID As Integer)
    '   lblPageTitle.Text = "Edit Form Field"

    '  DisplayFieldProperties(intFieldID)

    'End Sub

    Sub BindControls(ByVal intFieldId As Integer)
        Dim Field As New FormFields01
        Dim FieldTypeId As Integer = 0
        Dim FieldName As String = ""
        Dim SalesForceFieldName As String = ""
        Dim SalesForceObjectType As String = "lead"
        Dim DataTypeID As Integer = 0
        Dim DefaultValue As String = ""
        Dim Label As String = ""
        Dim LabelAlign As String = ""
        Dim LabelCss As String = ""
        Dim ListTypeID As Integer = 0
        Dim Width As Integer = 0
        Dim Height As Integer = 0
        Dim FieldCss As String = ""
        Dim ColCount As Integer = 0
        Dim MinRequired As Integer = 0
        Dim MaxRequired As Integer = 0
        Dim MinValue As String = ""
        Dim MaxValue As String = ""
        Dim AllowedChars As String = ""
        Dim DisallowedChars As String = ""
        Dim ValidationText As String = ""
        Dim ValidationCss As String = ""
        Dim HelpText As String = ""

        If intFieldId > 0 Then
            Field = Field.GetFormFieldInfo(intFieldId)
            FieldName = Field.FieldName
            SalesForceFieldName = Field.SalesForceFieldName
            SalesForceObjectType = Field.SalesForceObjectType
            FieldTypeId = Field.FieldTypeID
            DataTypeID = Field.DataTypeID
            DefaultValue = Field.DefaultValue
            Label = Field.Label
            LabelAlign = Field.LabelAlign
            LabelCss = Field.LabelCSS
            ListTypeID = Field.ListTypeID
            Width = Field.Width
            Height = Field.Height
            FieldCss = Field.FieldCSS
            ColCount = Field.ColCount
            MinRequired = Field.MinRequired
            MaxRequired = Field.MaxRequired
            MinValue = Field.MinValue
            MaxValue = Field.MaxValue
            AllowedChars = Field.AllowedChars
            DisallowedChars = Field.DisallowedChars
            ValidationText = Field.ValidationText
            ValidationCss = Field.ValidationCSS
            HelpText = Field.HelpText
        End If

        Me.BindSalesForceFieldsDdl(SalesForceFieldName, SalesForceObjectType)

        Me.BindFieldTypeIdDDL(FieldTypeId)
        txtFieldName.Text = FieldName
        Me.BindDataTypeIdDDL(DataTypeID)
        txtDefaultValue.Text = DefaultValue
        txtLabel.Text = Label
        txtContentEditor.EditorContent = DefaultValue
        Me.BindLabelAlignmentsDDL(LabelAlign)
        Me.BindLabelStylesDDL(LabelCss)
        Me.BindListTypesDDL(ListTypeID)
        txtWidth.Text = Width
        txtHeight.Text = Height
        Me.BindFieldStylesDDL(FieldCss)
        Me.BindColCountDDL(ColCount)
        If FieldTypeId = 6 Then
            If MinRequired > 0 Then
                rdoRequired.SelectedIndex = 0
            Else
                rdoRequired.SelectedIndex = 1
            End If
        Else
            txtMinRequired.Text = MinRequired
        End If
        txtMaxRequired.Text = MaxRequired
        txtMinValue.Text = MinValue
        txtMaxValue.Text = MaxValue
        txtAllowedChars.Text = AllowedChars
        txtDisallowedChars.Text = DisallowedChars
        txtValidationText.Text = ValidationText
        Me.BindValidationStylesDDL(ValidationCss)
        txtHelpText.Text = HelpText
    End Sub

    Sub BindSalesForceFieldsDdl(ByVal strSalesForceFieldName As String, ByVal strSalesForceObjectType As String)

        If Not Page.IsPostBack And GlobalVariables.GetValue("SalesForceIntegrationType") = "API" Then
            Dim FieldData As DataTable = Emagine.GetDataTable("SELECT SalesForceFieldName FROM FormFields WHERE FormID = " & FormID & " AND SalesForceObjectType = '" & strSalesForceObjectType & "'")

            Dim SalesForceObject As SalesForceApi.DescribeSObjectResult = SalesForceService.describeSObject(strSalesForceObjectType)
            Dim Fields As SalesForceApi.Field() = SalesForceObject.fields

            For i As Integer = 0 To Fields.Length - 1
                Dim Field As SalesForceApi.Field = Fields(i)
                If Field.createable And Not Field.defaultedOnCreate Then
                    Dim FoundMatch As Boolean = False
                    For j As Integer = 0 To (FieldData.Rows.Count - 1)
                        If (Field.name = FieldData.Rows(j).Item(0).ToString) And (Field.name <> strSalesForceFieldName) Then
                            FoundMatch = True
                            Exit For
                        End If
                    Next
                    If Not FoundMatch Then
                        Dim ItemText As String = strSalesForceObjectType & ": " & Field.label
                        If (Not Field.nillable) Or (Field.name = "Email") Then ItemText = ItemText & " *"
                        Dim ListItem As ListItem = New ListItem(ItemText, Field.name)
                        If Field.name = strSalesForceFieldName Then ListItem.Selected = True
                        ddlSalesForceFieldName.Items.Add(ListItem)
                    End If
                End If
            Next
        End If
    End Sub

    Sub BindFieldTypeIdDDL(ByVal intFieldTypeID As Integer)
        ddlFieldTypeId.Items.Add(New ListItem("<- Please Choose ->", "", True))
        ddlFieldTypeId.DataSource = FormFields01.GetFieldTypes()
        ddlFieldTypeId.DataTextField = "FieldType"
        ddlFieldTypeId.DataValueField = "FieldTypeId"
        ddlFieldTypeId.DataBind()
        Try
            ddlFieldTypeId.SelectedValue = intFieldTypeID.ToString
        Catch ex As Exception
            ddlFieldTypeId.SelectedIndex = 0
        End Try
    End Sub

    Sub BindDataTypeIdDDL(ByVal intDataTypeID As Integer)
        ddlDataTypeId.DataSource = FormFields01.GetDataTypes()
        ddlDataTypeId.DataTextField = "DataType"
        ddlDataTypeId.DataValueField = "DataTypeId"
        ddlDataTypeId.DataBind()
        Try
            ddlDataTypeId.SelectedValue = intDataTypeID.ToString
        Catch ex As Exception
            ddlDataTypeId.SelectedIndex = 0
        End Try
    End Sub

    Sub BindLabelAlignmentsDDL(ByVal strSelectedValue As String)
        Dim aryAlignments() As String = {"Left", "Right", "Top", "Bottom"}
        ddlLabelAlign.DataSource = aryAlignments
        ddlLabelAlign.DataBind()
        Try
            ddlLabelAlign.SelectedValue = strSelectedValue
        Catch ex As Exception
            ddlLabelAlign.SelectedIndex = 0
        End Try
    End Sub

    Sub BindLabelStylesDDL(ByVal strSelectedValue As String)
        FormFields01.GetStyles(Server.MapPath("/Collateral/Templates/English-US/Styles.css"), ddlLabelCSS)
        Try
            ddlLabelCSS.SelectedValue = strSelectedValue
        Catch ex As Exception
            ddlLabelCSS.SelectedIndex = 0
        End Try
    End Sub

    Sub BindListTypesDDL(ByVal intListTypeID As Integer)
        ddlListTypeID.Items.Add(New ListItem("<- Please Choose ->", 0, True))
        ddlListTypeID.Items.Add(New ListItem("Custom - SQL Statement", 1, True))
        If GlobalVariables.GetValue("SalesForceIntegrationType") = "API" Then ddlListTypeID.Items.Add(New ListItem("Custom - SalesForce Lookup", 2, True))
        ddlListTypeID.DataSource = FormFields01.GetLookups()
        ddlListTypeID.DataTextField = "LookupName"
        ddlListTypeID.DataValueField = "LookupID"
        ddlListTypeID.DataBind()
        ddlListTypeID.Items.Add(New ListItem("<New Lookup...>", -1, True))
        Try
            ddlListTypeID.SelectedValue = intListTypeID.ToString
        Catch ex As Exception
            ddlListTypeID.SelectedIndex = 0
        End Try
    End Sub

    Sub BindFieldStylesDDL(ByVal strSelectedValue As String)
        FormFields01.GetStyles(Server.MapPath("/Collateral/Templates/English-US/Styles.css"), ddlFieldCss)
        Try
            ddlFieldCss.SelectedValue = strSelectedValue
        Catch ex As Exception
            ddlFieldCss.SelectedIndex = 0
        End Try
    End Sub

    Sub BindColCountDDL(ByVal strSelectedValue As String)
        Dim i As Integer

        For i = 1 To 10
            ddlColCount.Items.Add(New ListItem(i, i))
        Next

        Try
            ddlColCount.SelectedValue = strSelectedValue
        Catch ex As Exception
            ddlColCount.SelectedIndex = 0
        End Try
    End Sub

    Sub BindValidationStylesDDL(ByVal strSelectedValue As String)
        FormFields01.GetStyles(Server.MapPath("/Collateral/Templates/English-US/Styles.css"), ddlValidationCSS)
        Try
            ddlValidationCSS.SelectedValue = strSelectedValue
        Catch ex As Exception
            ddlValidationCSS.SelectedIndex = 0
        End Try
    End Sub

    Sub DisplayFieldProperties(ByVal intFieldId As Integer)
        Dim FieldTypeId As Integer = Emagine.GetNumber(ddlFieldTypeId.SelectedValue)
        Dim DataTypeId As Integer = Emagine.GetNumber(ddlDataTypeId.SelectedValue)

        trDataTypeId.Visible = False
        trFieldName.Visible = False
        If FieldTypeId > 0 And GlobalVariables.GetValue("SalesForceIntegrationType") = "API" Then trSalesForceAPI.Visible = True
        trContentBlock.Visible = False
        trDataTypeId.Visible = False
        trDefaultValue.Visible = False
        trLabel.Visible = False
        trLabelAlign.Visible = False
        trLabelCSS.Visible = False
        trListTypeID.Visible = False
        trLookupName.Visible = False
        trListOptions.Visible = False
        trWidth.Visible = False
        trHeight.Visible = False
        trFieldCss.Visible = False
        trColCount.Visible = False
        trMinRequired.Visible = False
        trMaxRequired.Visible = False
        trRequired.Visible = False
        trMinValue.Visible = False
        trMaxValue.Visible = False
        trAllowedChars.Visible = False
        trDisallowedChars.Visible = False
        trValidationText.Visible = False
        trValidationCSS.Visible = False
        trHelpText.Visible = False

        Select Case FieldTypeId
            Case 1 'Content Block
                'trFieldName.Visible = True
                trContentBlock.Visible = True

            Case 2 'Text Box
                trDataTypeId.Visible = True
                trFieldName.Visible = True
                trDataTypeId.Visible = True
                trDefaultValue.Visible = True
                trLabel.Visible = True
                'trLabelAlign.Visible = True
                trLabelCSS.Visible = True
                trWidth.Visible = True
                trHeight.Visible = True
                trFieldCss.Visible = True
                trMinRequired.Visible = True
                trMaxRequired.Visible = True
                trMinValue.Visible = True
                trMaxValue.Visible = True
                trAllowedChars.Visible = True
                'trDisallowedChars.Visible = True
                'trValidationText.Visible = True
                'trValidationCSS.Visible = True
                trHelpText.Visible = True
                lblMinRequired.Text = "Validation - Min Length:"
                lblMaxRequired.Text = "Validation - Max Length:"

            Case 3 'Text Area
                trDataTypeId.Visible = True
                trFieldName.Visible = True
                trDefaultValue.Visible = True
                trLabel.Visible = True
                'trLabelAlign.Visible = True
                trLabelCSS.Visible = True
                trWidth.Visible = True
                trHeight.Visible = True
                trFieldCss.Visible = True
                trMinRequired.Visible = True
                trMaxRequired.Visible = True
                trMinValue.Visible = True
                trMaxValue.Visible = True
                trAllowedChars.Visible = True
                'trDisallowedChars.Visible = True
                'trValidationText.Visible = True
                'trValidationCSS.Visible = True
                trHelpText.Visible = True
                lblMinRequired.Text = "Validation - Min Length:"
                lblMaxRequired.Text = "Validation - Max Length:"

            Case 4 'Drop-Down List
                trFieldName.Visible = True
                trDefaultValue.Visible = True
                trLabel.Visible = True
                'trLabelAlign.Visible = True
                trLabelCSS.Visible = True
                trListTypeID.Visible = True
                trWidth.Visible = True
                trHeight.Visible = True
                trFieldCss.Visible = True
                trMinRequired.Visible = True
                trMaxRequired.Visible = True
                'trValidationText.Visible = True
                'trValidationCSS.Visible = True
                trHelpText.Visible = True
                lblMinRequired.Text = "Validation - Min Selections:"
                lblMaxRequired.Text = "Validation - Max Selections:"


            Case 5 'Check Box List
                trFieldName.Visible = True
                trDefaultValue.Visible = True
                trLabel.Visible = True
                trLabelAlign.Visible = True
                trLabelCSS.Visible = True
                trListTypeID.Visible = True
                trFieldCss.Visible = True
                trColCount.Visible = True
                trMinRequired.Visible = True
                trMaxRequired.Visible = True
                'trValidationText.Visible = True
                'trValidationCSS.Visible = True
                trHelpText.Visible = True
                lblMinRequired.Text = "Validation - Min Selections:"
                lblMaxRequired.Text = "Validation - Max Selections:"

            Case 6 'Radio Button List
                trFieldName.Visible = True
                trDefaultValue.Visible = True
                trLabel.Visible = True
                trLabelAlign.Visible = True
                trLabelCSS.Visible = True
                trListTypeID.Visible = True
                trFieldCss.Visible = True
                trColCount.Visible = True
                'trMinRequired.Visible = True
                'trMaxRequired.Visible = True
                'trValidationText.Visible = True
                'trValidationCSS.Visible = True
                trHelpText.Visible = True
                trRequired.Visible = True

            Case 7 'File Upload
                trFieldName.Visible = True
                trDefaultValue.Visible = True
                trLabel.Visible = True
                'trLabelAlign.Visible = True
                trLabelCSS.Visible = True
                trWidth.Visible = True
                trHeight.Visible = True
                trFieldCss.Visible = True
                trMinRequired.Visible = True
                trMaxRequired.Visible = True
                trAllowedChars.Visible = True
                'trValidationText.Visible = True
                'trValidationCSS.Visible = True
                trHelpText.Visible = True

                lblDefaultValue.Text = "Virtual Upload Path:"
                lblMinRequired.Text = "Validation - Min File Size Required:"
                lblMaxRequired.Text = "Validation - Max File Size Required:"
                lblAllowedChars.Text = "Validation - Allowed File Extensions:"

            Case 8 'Hidden Field
                trFieldName.Visible = True
                trDefaultValue.Visible = True

                lblDefaultValue.Text = "Value:"

        End Select


    End Sub

    Protected Sub ddlFieldTypeId_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFieldTypeId.SelectedIndexChanged
        Dim FieldID As Integer = Emagine.GetNumber(Request("FieldID"))

        ddlFieldTypeId.Items(0).Enabled = False

        DisplayFieldProperties(FieldID)
        DisplayLookupFields()
    End Sub

    Protected Sub ddlListTypeID_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlListTypeID.SelectedIndexChanged
        ddlListTypeID.Items(0).Enabled = False
        DisplayLookupFields()
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim FormID As Integer = Emagine.GetNumber(Context.Request("FormID"))
        Response.Redirect("BuildForm.aspx?FormID=" & FormID)
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim FormID As Integer = Emagine.GetNumber(Context.Request("FormID"))
        Dim FieldID As Integer = Emagine.GetNumber(Context.Request("FieldID"))
        Dim FieldTypeID As Integer = ddlFieldTypeId.SelectedValue

        If Page.IsValid Then
            Dim Field As New FormFields01
            Field.FieldID = FieldID
            Field.FormID = FormID
            Field.FieldName = txtFieldName.Text
            Field.SalesForceFieldName = ddlSalesForceFieldName.SelectedValue
            Field.SalesForceObjectType = "lead"
            Field.FieldTypeID = FieldTypeID
            Field.DataTypeID = ddlDataTypeId.SelectedValue
            If FieldTypeID = 1 Then
                Field.DefaultValue = txtContentEditor.EditorContent
            Else
                Field.DefaultValue = txtDefaultValue.Text
            End If
            Field.Label = txtLabel.Text
            Field.LabelAlign = ddlLabelAlign.SelectedValue
            Field.LabelCSS = ddlLabelCSS.SelectedValue
            Field.ListTypeID = ddlListTypeID.SelectedValue
            Field.Width = txtWidth.Text
            Field.Height = txtHeight.Text
            Field.FieldCSS = ddlFieldCss.SelectedValue
            Field.ColCount = ddlColCount.SelectedValue
            If FieldTypeID = 6 Then
                Field.MinRequired = rdoRequired.SelectedValue
            Else
                Field.MinRequired = txtMinRequired.Text
            End If
            Field.MaxRequired = txtMaxRequired.Text
            Field.MinValue = txtMinValue.Text
            Field.MaxValue = txtMaxValue.Text
            Field.AllowedChars = txtAllowedChars.Text
            Field.DisallowedChars = txtDisallowedChars.Text
            Field.ValidationText = txtValidationText.Text
            Field.ValidationCSS = ddlValidationCSS.Text
            Field.HelpText = txtHelpText.Text
            Field.SortOrder = Emagine.GetNumber(Emagine.GetDbValue("EXECUTE sp_Forms01_GetMaxFieldSortOrder " & FormID)) + 1

            If ddlListTypeID.SelectedValue = -1 Then
                Dim LookupID As Integer = FormFields01.AddLookup(txtLookupName.Text)
                If LookupID > 0 Then
                    If FormFields01.AddLookupValues(LookupID, txtListOptions.Text) Then
                        Field.ListTypeID = LookupID
                    End If
                End If

            End If

            If FieldID > 0 Then
                If FormFields01.UpdateFormField(Field) Then
                    Session("Alert") = "The field has been updated successfully."
                    Response.Redirect("BuildForm.aspx?FormID=" & FormID)
                Else
                    lblAlert.Text = "An error has occurred."
                End If
            Else
                
                If FormFields01.AddFormField(Field) Then
                    Session("Alert") = "The field has been added successfully."
                    Response.Redirect("BuildForm.aspx?FormID=" & FormID)
                Else
                    lblAlert.Text = "An error has occurred."
                End If
            End If
        End If
    End Sub

    Sub DisplayLookupFields()
        Dim ListTypeID As Integer = ddlListTypeID.SelectedValue
        Dim FieldTypeID As Integer = ddlFieldTypeId.SelectedValue

        Select Case FieldTypeID
            Case 4, 5, 6
                Select Case ListTypeID
                    Case -1
                        trLookupName.Visible = True
                        trListOptions.Visible = True
                        rfvListOptions.ErrorMessage = "Lookup values are required."
                        lblListOptions.Text = "Lookup Values:"
                        txtLookupName.Focus()

                    Case 1
                        trListOptions.Visible = True
                        rfvListOptions.ErrorMessage = "SQL Statement is required."
                        lblListOptions.Text = "SQL Statement:"
                        txtListOptions.Text = ""

                    Case 2
                        trListOptions.Visible = True
                        txtListOptions.Enabled = False
                        txtListOptions.Text = ""

                        Dim SalesForceObject As SalesForceApi.DescribeSObjectResult = SalesForceService.describeSObject("lead")

                        Dim Fields As SalesForceApi.Field() = SalesForceObject.fields

                        For i As Integer = 0 To Fields.Length - 1
                            Dim Field As SalesForceApi.Field = Fields(i)
                            If Field.name = ddlSalesForceFieldName.SelectedValue Then
                                If Field.type = 1 Or Field.type = 2 Then
                                    Dim OptionValues As SalesForceApi.PicklistEntry() = Field.picklistValues
                                    For j As Integer = 0 To OptionValues.Length - 1
                                        txtListOptions.Text = txtListOptions.Text & OptionValues(j).label & "^" & OptionValues(j).value & vbCrLf
                                        'Response.Write("-" & OptionValues(j).label & "<br />")
                                    Next
                                Else
                                    txtListOptions.Text = "SalesForce field """ & Field.name & """ is not of type 'PickList'"
                                End If
                                Exit For
                            End If
                        Next
                End Select
        End Select
    End Sub


    Protected Sub lblPageTitle_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblPageTitle.Load
        If FieldID > 0 Then
            lblPageTitle.Text = "<a href='Default.aspx'>Forms</a> > <a href='BuildForm.aspx?FormID=" & FormID & "'>" & FormName & "</a> > Edit Form Field"
        Else
            lblPageTitle.Text = "<a href='Default.aspx'>Forms</a> > <a href='BuildForm.aspx?FormID=" & FormID & "'>" & FormName & "</a> > Add New Form Field"
        End If
    End Sub

    Protected Sub ddlSalesForceFieldName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSalesForceFieldName.SelectedIndexChanged
        'Dim SalesForceObject As SalesForceApi.DescribeSObjectResult = SalesForceService.describeSObject(strSalesForceObjectType)
        'Dim Fields As SalesForceApi.Field() = SalesForceObject.fields

        Me.DisplayLookupFields()
    End Sub
End Class