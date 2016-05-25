Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports PeterBlum.VAM
Imports PeterBlum.PetersDatePackage

Public Class FormFields01
    Inherits Forms01

    Public FieldID As Integer = 0
    Public Shadows FormID As Integer = 0
    Public FieldTypeID As Integer = 0
    Public ColumnID As Integer = 0
    Public DataTypeID As Integer = 0
    Public ListTypeID As Integer = 0
    Public FieldName As String = ""
    Public SalesForceFieldName As String = ""
    Public SalesForceObjectType As String = ""
    Public Label As String = ""
    Public LabelAlign As String = ""
    Public LabelCSS As String = ""
    Public MinRequired As Integer = 0
    Public MaxRequired As Integer = 0
    Public MinValue As String = ""
    Public MaxValue As String = ""
    Public AllowedChars As String = ""
    Public DisallowedChars As String = ""
    Public DefaultValue As String = ""
    Public ListOptions As String = ""
    Public HelpText As String = ""
    Public ValidationText As String = ""
    Public ValidationCSS As String = ""
    Public Width As Integer = 0
    Public Height As Integer = 0
    Public ColCount As Integer = 0
    Public FieldSize As Integer = 0
    Public FieldCSS As String = ""
    Public SortOrder As Integer = 0

    Public Function GetFormFieldInfo(ByVal intFieldId As Integer) As FormFields01
        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim strSQL As String = "sp_Forms01_GetFieldInfo"
        Dim objCommand As New SqlCommand(strSQL, objConn)

        objCommand.CommandType = CommandType.StoredProcedure
        objCommand.Parameters.AddWithValue("@FieldID", intFieldId)

        Dim dtrFormData As SqlDataReader
        Dim Field As New FormFields01
        Try
            objConn.Open()
            dtrFormData = objCommand.ExecuteReader(CommandBehavior.SingleRow)
            If dtrFormData.Read Then
                Field.FieldID = dtrFormData("FieldID")
                Field.FormID = dtrFormData("FormID")
                Field.FieldTypeID = dtrFormData("FieldTypeID")
                Field.ColumnID = dtrFormData("ColumnID")
                Field.DataTypeID = dtrFormData("DataTypeID")
                Field.ListTypeID = dtrFormData("ListTypeID")
                Field.FieldName = dtrFormData("FieldName").ToString
                Field.SalesForceFieldName = dtrFormData("SalesForceFieldName").ToString
                Field.SalesForceObjectType = dtrFormData("SalesForceObjectType").ToString
                Field.Label = dtrFormData("Label").ToString
                Field.LabelAlign = dtrFormData("LabelAlign").ToString
                Field.LabelCSS = dtrFormData("LabelCSS").ToString
                Field.MinRequired = dtrFormData("MinRequired")
                Field.MaxRequired = dtrFormData("MaxRequired")
                Field.MinValue = dtrFormData("MinValue").ToString
                Field.MaxValue = dtrFormData("MaxValue").ToString
                Field.AllowedChars = dtrFormData("AllowedChars").ToString
                Field.DisallowedChars = dtrFormData("DisallowedChars").ToString
                Field.DefaultValue = dtrFormData("DefaultValue").ToString
                Field.HelpText = dtrFormData("HelpText").ToString
                Field.ValidationText = dtrFormData("ValidationText").ToString
                Field.ValidationCSS = dtrFormData("ValidationCSS").ToString
                Field.Width = dtrFormData("Width")
                Field.Height = dtrFormData("Height")
                Field.ColCount = dtrFormData("ColCount")
                Field.FieldCSS = dtrFormData("FieldCSS").ToString
                Field.SortOrder = dtrFormData("SortOrder")
            Else
                Field = Nothing
            End If
            dtrFormData.Close()
        Finally
            objConn.Close()
        End Try
        Return Field
    End Function

    Public Shared Function AddFormField(ByVal Field As FormFields01) As Boolean

        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim blnResult As Boolean = False
        Dim strSQL As String = "sp_Forms01_AddFormField"

        Dim objCommand As New SqlCommand(strSQL, objConn)
        objCommand.CommandType = CommandType.StoredProcedure
        objCommand.Parameters.AddWithValue("@FormID", Field.FormID)
        objCommand.Parameters.AddWithValue("@FieldName", Field.FieldName)
        objCommand.Parameters.AddWithValue("@SalesForceFieldName", Field.SalesForceFieldName)
        objCommand.Parameters.AddWithValue("@SalesForceObjectType", Field.SalesForceObjectType)
        objCommand.Parameters.AddWithValue("@FieldTypeID", Field.FieldTypeID)
        objCommand.Parameters.AddWithValue("@DataTypeID", Field.DataTypeID)
        objCommand.Parameters.AddWithValue("@DefaultValue", Field.DefaultValue)
        objCommand.Parameters.AddWithValue("@Label", Field.Label)
        objCommand.Parameters.AddWithValue("@LabelAlign", Field.LabelAlign)
        objCommand.Parameters.AddWithValue("@LabelCSS", Field.LabelCSS)
        objCommand.Parameters.AddWithValue("@ListTypeID", Field.ListTypeID)
        objCommand.Parameters.AddWithValue("@Width", Field.Width)
        objCommand.Parameters.AddWithValue("@Height", Field.Height)
        objCommand.Parameters.AddWithValue("@FieldCss", Field.FieldCSS)
        objCommand.Parameters.AddWithValue("@ColCount", Field.ColCount)
        objCommand.Parameters.AddWithValue("@MinRequired", Field.MinRequired)
        objCommand.Parameters.AddWithValue("@MaxRequired", Field.MaxRequired)
        objCommand.Parameters.AddWithValue("@MinValue", Field.MinValue)
        objCommand.Parameters.AddWithValue("@MaxValue", Field.MaxValue)
        objCommand.Parameters.AddWithValue("@AllowedChars", Field.AllowedChars)
        objCommand.Parameters.AddWithValue("@DisallowedChars", Field.DisallowedChars)
        objCommand.Parameters.AddWithValue("@ValidationText", Field.ValidationText)
        objCommand.Parameters.AddWithValue("@ValidationCss", Field.ValidationCSS)
        objCommand.Parameters.AddWithValue("@HelpText", Field.HelpText)
        objCommand.Parameters.AddWithValue("@SortOrder", Field.SortOrder)

        Try
            objConn.Open()

            objCommand.ExecuteNonQuery()

            blnResult = True
        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            If objConn.State = ConnectionState.Open Then objConn.Dispose()
        End Try

        Return blnResult
    End Function

    Public Shared Function UpdateFormField(ByVal Field As FormFields01) As Boolean

        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim blnResult As Boolean = False
        Dim strSQL As String = "sp_Forms01_UpdateFormField"

        Dim objCommand As New SqlCommand(strSQL, objConn)
        objCommand.CommandType = CommandType.StoredProcedure
        objCommand.Parameters.AddWithValue("@FieldID", Field.FieldID)
        objCommand.Parameters.AddWithValue("@FieldName", Field.FieldName)
        objCommand.Parameters.AddWithValue("@SalesForceFieldName", Field.SalesForceFieldName)
        objCommand.Parameters.AddWithValue("@SalesForceObjectType", Field.SalesForceObjectType)
        objCommand.Parameters.AddWithValue("@FieldTypeID", Field.FieldTypeID)
        objCommand.Parameters.AddWithValue("@DataTypeID", Field.DataTypeID)
        objCommand.Parameters.AddWithValue("@DefaultValue", Field.DefaultValue)
        objCommand.Parameters.AddWithValue("@Label", Field.Label)
        objCommand.Parameters.AddWithValue("@LabelAlign", Field.LabelAlign)
        objCommand.Parameters.AddWithValue("@LabelCSS", Field.LabelCSS)
        objCommand.Parameters.AddWithValue("@ListTypeID", Field.ListTypeID)
        objCommand.Parameters.AddWithValue("@Width", Field.Width)
        objCommand.Parameters.AddWithValue("@Height", Field.Height)
        objCommand.Parameters.AddWithValue("@FieldCss", Field.FieldCSS)
        objCommand.Parameters.AddWithValue("@ColCount", Field.ColCount)
        objCommand.Parameters.AddWithValue("@MinRequired", Field.MinRequired)
        objCommand.Parameters.AddWithValue("@MaxRequired", Field.MaxRequired)
        objCommand.Parameters.AddWithValue("@MinValue", Field.MinValue)
        objCommand.Parameters.AddWithValue("@MaxValue", Field.MaxValue)
        objCommand.Parameters.AddWithValue("@AllowedChars", Field.AllowedChars)
        objCommand.Parameters.AddWithValue("@DisallowedChars", Field.DisallowedChars)
        objCommand.Parameters.AddWithValue("@ValidationText", Field.ValidationText)
        objCommand.Parameters.AddWithValue("@ValidationCss", Field.ValidationCSS)
        objCommand.Parameters.AddWithValue("@HelpText", Field.HelpText)
        objCommand.Parameters.AddWithValue("@SortOrder", Field.SortOrder)

        Try
            objConn.Open()

            objCommand.ExecuteNonQuery()

            blnResult = True
        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            If objConn.State = ConnectionState.Open Then objConn.Dispose()
        End Try

        Return blnResult
    End Function

    Public Shared Function GetFieldTypes() As SqlDataReader
        Dim strSQL As String = "EXECUTE sp_Forms01_GetAllFieldTypes"

        Dim dtrFieldTypes As SqlDataReader = GetDataReader(strSQL)

        Return dtrFieldTypes
    End Function

    Public Shared Function GetLookups() As SqlDataReader
        Dim strSQL As String = "EXECUTE sp_Forms01_GetAllLookups"

        Dim dtrLookups As SqlDataReader = GetDataReader(strSQL)

        Return dtrLookups
    End Function

    Public Shared Function GetDataTypes() As SqlDataReader
        Dim strSQL As String = "EXECUTE sp_Forms01_GetAllDataTypes"

        Dim dtrDataTypes As SqlDataReader = GetDataReader(strSQL)

        Return dtrDataTypes
    End Function

    Public Shared Sub DisplayFormField(ByVal intFieldID As Integer, ByVal strFieldValue As String, ByVal plcFormField As PlaceHolder, ByVal blnVisible As Boolean)
        Dim Field As New FormFields01
        Dim FormField As New Control
        Field = Field.GetFormFieldInfo(intFieldID)

        If Len(strFieldValue) = 0 Then strFieldValue = Field.DefaultValue


        Select Case Field.FieldTypeID
            Case 1
                FormField = GetContentBlock(Field.FieldID, Field.DefaultValue)

            Case 2
                Select Case Field.DataTypeID
                    Case 1 'Integer
                        FormField = GetIntegerTextBox(Field.FieldID, strFieldValue, Field.Width, Field.Height, Field.FieldCSS, Field.MaxRequired)

                    Case 2 'Currency
                        FormField = GetCurrencyTextBox(Field.FieldID, strFieldValue, Field.Width, Field.Height, Field.FieldCSS, Field.MaxRequired)

                    Case 3 'Decimal
                        FormField = GetDecimalTextBox(Field.FieldID, strFieldValue, Field.Width, Field.Height, Field.FieldCSS, Field.MaxRequired)

                    Case 10 'Date
                        FormField = GetDateTextBox(Field.FieldID, strFieldValue, Field.Width, Field.Height, Field.FieldCSS, Field.MaxRequired, Field.MinValue, Field.MaxValue)

                    Case 11 'US Phone
                        FormField = GetPhoneTextBox(Field.FieldID, strFieldValue, Field.Width, Field.Height, Field.FieldCSS, Field.MaxRequired)

                    Case 12 'International Phone
                        FormField = GetPhoneTextBox(Field.FieldID, strFieldValue, Field.Width, Field.Height, Field.FieldCSS, Field.MaxRequired)

                    Case 13 'ZIP Code
                        FormField = GetFilteredTextBox(Field.FieldID, strFieldValue, Field.Width, Field.Height, Field.FieldCSS, Field.MaxRequired, "1234567890 -")

                    Case 20 'Email
                        FormField = GetTextBox(Field.FieldID, strFieldValue, Field.Width, Field.Height, Field.FieldCSS, Field.MaxRequired)

                    Case 21 'URL
                        FormField = GetTextBox(Field.FieldID, strFieldValue, Field.Width, Field.Height, Field.FieldCSS, Field.MaxRequired)

                    Case 30 'Credit Card Number
                        FormField = GetFilteredTextBox(Field.FieldID, strFieldValue, Field.Width, Field.Height, Field.FieldCSS, Field.MaxRequired, "1234567890")

                    Case 31 'Social Security Number
                        FormField = GetSocialSecurityNumberTextBox(Field.FieldID, strFieldValue, Field.Width, Field.Height, Field.FieldCSS, Field.MaxRequired)

                    Case Else
                        FormField = GetFilteredTextBox(Field.FieldID, strFieldValue, Field.Width, Field.Height, Field.FieldCSS, Field.MaxRequired, Field.AllowedChars)

                End Select

            Case 3
                FormField = GetFilteredTextArea(Field.FieldID, strFieldValue, Field.Width, Field.Height, Field.FieldCSS, Field.AllowedChars)

            Case 4
                If Field.MinRequired > 1 Then
                    FormField = GetListBox(Field.FieldID, strFieldValue, Field.ListTypeID, Field.ListOptions, Field.Width, Field.Height, Field.FieldCSS, Field.MinRequired, Field.MaxRequired)
                Else
                    FormField = GetDropDownList(Field.FieldID, strFieldValue, Field.ListTypeID, Field.ListOptions, Field.Width, Field.Height, Field.FieldCSS, Field.MinRequired, Field.MaxRequired)
                End If

            Case 5
                FormField = GetCheckBoxList(Field.FieldID, strFieldValue, Field.ListTypeID, Field.ListOptions, Field.FieldCSS, Field.MinRequired, Field.MaxRequired, Field.ColCount)

            Case 6
                FormField = GetRadioButtonList(Field.FieldID, strFieldValue, Field.ListTypeID, Field.ListOptions, Field.FieldCSS, Field.MinRequired, Field.MaxRequired, Field.ColCount)

            Case 7
                FormField = GetFileUpload(Field.FieldID, Field.Width, Field.Height, Field.FieldCSS)

            Case 8
                FormField = GetHiddenField(Field.FieldID, Field.DefaultValue)
                If blnVisible Then
                    plcFormField.Controls.Add(GetHiddenFieldInfo(Field.FieldID, Field.FieldName, Field.DefaultValue))
                End If
        End Select

        plcFormField.Controls.Add(FormField)

        'If Field.SortOrder = 1 Then PeterBlum.VAM.Globals.Page.InitialFocusControl = FormField

        DisplayValidators(Field, plcFormField)

    End Sub

    Public Shared Function GetContentBlock(ByVal intFieldID As Integer, ByVal strContent As String) As Literal
        Dim ltrFormField As New Literal()

        ltrFormField.ID = intFieldID
        ltrFormField.Text = strContent

        Return ltrFormField
    End Function

    Public Shared Function GetTextBox(ByVal intFieldID As Integer, ByVal strFieldValue As String, ByVal intWidth As Integer, ByVal intHeight As Integer, ByVal strCssClass As String, ByVal intMaxLength As Integer) As PeterBlum.VAM.TextBox
        Dim FormField As New PeterBlum.VAM.TextBox()
        FormField.ID = intFieldID
        FormField.Text = strFieldValue
        FormField.Width = intWidth
        FormField.Height = intHeight
        FormField.MaxLength = intMaxLength
        FormField.CssClass = strCssClass

        Return FormField
    End Function

    Private Shared Function GetFilteredTextBox(ByVal intFieldID As Integer, ByVal strFieldValue As String, ByVal intWidth As Integer, ByVal intHeight As Integer, ByVal strCssClass As String, ByVal intMaxLength As Integer, ByVal strAllowedChars As String) As FilteredTextBox
        Dim FormField As New FilteredTextBox()
        FormField.ID = intFieldID
        FormField.Text = strFieldValue
        FormField.Width = intWidth
        FormField.Height = intHeight
        FormField.MaxLength = intMaxLength
        FormField.CssClass = strCssClass
        If strAllowedChars.Length > 0 Then FormField.OtherCharacters = strAllowedChars
        Return FormField
    End Function

    Private Shared Function GetIntegerTextBox(ByVal intFieldID As Integer, ByVal strFieldValue As String, ByVal intWidth As Integer, ByVal intHeight As Integer, ByVal strCssClass As String, ByVal intMaxLength As Integer) As IntegerTextBox
        Dim FormField As New IntegerTextBox
        FormField.ID = intFieldID
        FormField.Text = strFieldValue
        FormField.Width = intWidth
        FormField.Height = intHeight
        FormField.MaxLength = intMaxLength
        FormField.CssClass = strCssClass
        'FormField.ShowSpinner = True

        Return FormField
    End Function

    Private Shared Function GetCurrencyTextBox(ByVal intFieldID As Integer, ByVal strFieldValue As String, ByVal intWidth As Integer, ByVal intHeight As Integer, ByVal strCssClass As String, ByVal intMaxLength As Integer) As CurrencyTextBox
        Dim FormField As New CurrencyTextBox
        FormField.ID = intFieldID
        FormField.Width = intWidth
        FormField.Height = intHeight
        FormField.MaxLength = intMaxLength
        FormField.CssClass = strCssClass
        FormField.UseCurrencySymbol = True
        FormField.ShowThousandsSeparator = True
        FormField.AllowNegatives = False
        If IsNumeric(strFieldValue) Then FormField.DoubleValue = strFieldValue

        Return FormField
    End Function

    Private Shared Function GetDecimalTextBox(ByVal intFieldID As Integer, ByVal strFieldValue As String, ByVal intWidth As Integer, ByVal intHeight As Integer, ByVal strCssClass As String, ByVal intMaxLength As Integer) As DecimalTextBox
        Dim FormField As New DecimalTextBox
        FormField.ID = intFieldID
        FormField.Text = strFieldValue
        FormField.Width = intWidth
        FormField.Height = intHeight
        FormField.MaxLength = intMaxLength
        FormField.CssClass = strCssClass

        Return FormField
    End Function

    Private Shared Function GetDateTextBox(ByVal intFieldID As Integer, ByVal strFieldValue As String, ByVal intWidth As Integer, ByVal intHeight As Integer, ByVal strCssClass As String, ByVal intMaxLength As Integer, ByVal strMinValue As String, ByVal strMaxValue As String) As DateTextBox

        Dim FormField As New DateTextBox
        FormField.ID = intFieldID
        FormField.Width = intWidth
        FormField.Height = intHeight
        FormField.MaxLength = intMaxLength
        FormField.CssClass = strCssClass
        If IsDate(strFieldValue) Then FormField.xDate = CDate(strFieldValue)
        If IsDate(strMinValue) Then FormField.xMinDate = CDate(strMinValue)
        If IsDate(strMaxValue) Then FormField.xMaxDate = CDate(strMaxValue)

        Return FormField
    End Function

    Private Shared Function GetPhoneTextBox(ByVal intFieldID As Integer, ByVal strFieldValue As String, ByVal intWidth As Integer, ByVal intHeight As Integer, ByVal strCssClass As String, ByVal intMaxLength As Integer) As MultiSegmentDataEntry

        Dim FormField As New MultiSegmentDataEntry()
        FormField.ID = intFieldID
        FormField.Width = intWidth
        FormField.Height = intHeight
        'FormField.CssClass = "VAMMultiSegContainer"

        Dim Segment1 As New TextSegment()
        Segment1.CssClass = strCssClass
        Segment1.TabOnTheseKeys = ") -"
        Segment1.MaxLength = "3"
        Segment1.MinLength = "3"
        Segment1.Width = "30px"
        Segment1.AutoWidth = "False"
        'Segment1.DisplayTextBefore = "("
        'Segment1.DisplayTextAfter = ")"
        Segment1.FormattingTextAfter = " "
        'Segment1.FormattingTextBefore = "("
        Segment1.IgnoreTheseCharsAfter = " "
        Segment1.IgnoreTheseCharsBefore = " "
        FormField.Segments.Add(Segment1)

        Dim Segment2 As New TextSegment()
        Segment2.CssClass = strCssClass
        Segment2.TabOnTheseKeys = "- "
        Segment2.MaxLength = "3"
        Segment2.MinLength = "3"
        Segment2.Width = "30px"
        Segment2.AutoWidth = "False"
        Segment2.FormattingTextAfter = "-"
        Segment2.IgnoreTheseCharsAfter = " "
        FormField.Segments.Add(Segment2)

        Dim Segment3 As New TextSegment()
        Segment3.CssClass = strCssClass
        Segment3.TabOnTheseKeys = "ext:"
        Segment3.MaxLength = "4"
        Segment3.MinLength = "4"
        Segment3.Width = "40px"
        Segment3.AutoWidth = "False"
        Segment3.IgnoreTheseCharsAfter = " "
        FormField.Segments.Add(Segment3)

        Dim Segment4 As New TextSegment()
        Segment4.CssClass = strCssClass
        Segment4.MaxLength = "5"
        Segment4.Required = "False"
        Segment4.FormattingTextBefore = " x"
        Segment4.NoTextBeforeWhenBlank = "True"
        Segment4.DisplayTextBefore = "&nbsp;ext:"
        FormField.Segments.Add(Segment4)

        FormField.Text = strFieldValue

        Return FormField
    End Function

    Private Shared Function GetTextArea(ByVal intFieldID As Integer, ByVal strFieldValue As String, ByVal intWidth As Integer, ByVal intHeight As Integer, ByVal strCssClass As String) As TextBox
        Dim txtFormField As New TextBox()
        txtFormField.ID = intFieldID
        txtFormField.TextMode = TextBoxMode.MultiLine
        txtFormField.Text = strFieldValue
        txtFormField.Width = intWidth
        txtFormField.Height = intHeight
        txtFormField.CssClass = strCssClass

        Return txtFormField
    End Function

    Private Shared Function GetFilteredTextArea(ByVal intFieldID As Integer, ByVal strFieldValue As String, ByVal intWidth As Integer, ByVal intHeight As Integer, ByVal strCssClass As String, ByVal strAllowedChars As String) As FilteredTextBox
        Dim FormField As New FilteredTextBox()
        FormField.ID = intFieldID
        FormField.TextMode = TextBoxMode.MultiLine
        FormField.Text = strFieldValue
        FormField.Width = intWidth
        FormField.Height = intHeight
        FormField.CssClass = strCssClass
        If strAllowedChars.Length > 0 Then FormField.OtherCharacters = strAllowedChars

        Return FormField
    End Function

    Private Shared Function GetDropDownList(ByVal intFieldID As Integer, ByVal strFieldValue As String, ByVal intListTypeID As Integer, ByVal strListOptions As String, ByVal intWidth As Integer, ByVal intHeight As Integer, ByVal strCssClass As String, ByVal intMinRequired As Integer, ByVal intMaxRequired As Integer) As DropDownList
        Dim ddlFormField As New DropDownList()
        Dim intCounter As Integer = 0

        ddlFormField.AppendDataBoundItems = True
        ddlFormField.ID = intFieldID
        If intWidth > 0 And intHeight > 0 Then
            ddlFormField.Width = intWidth
            ddlFormField.Height = intHeight
        End If

        ddlFormField.CssClass = strCssClass

        If intMinRequired > 0 Then
            ddlFormField.Items.Add(New ListItem("<- Please Choose ->", ""))
            intCounter = 1
        End If

        Select Case intListTypeID
            Case 1
                Dim objRs As SqlDataReader

                Try
                    objRs = GetDataReader(strListOptions)

                    ddlFormField.DataSource = objRs
                    ddlFormField.DataTextField = objRs.GetName(0)
                    ddlFormField.DataValueField = objRs.GetName(1)
                    ddlFormField.SelectedValue = strFieldValue
                    ddlFormField.DataBind()

                Catch ex As Exception
                    ddlFormField.Items.Add(New ListItem("ERROR", "ERROR"))
                End Try

            Case 2
                Try
                    Dim SalesForceFieldName As String = Emagine.GetDbValue("SELECT SalesForceFieldName FROM FormFields WHERE FieldID = " & intFieldID)
                    Dim SalesForceObjectType As String = Emagine.GetDbValue("SELECT SalesForceObjectType FROM FormFields WHERE FieldID = " & intFieldID)
                    Dim SalesForceService As SalesForceApi.SforceService = SalesForceConnector.GetSalesForceService()
                    Dim SalesForceObject As SalesForceApi.DescribeSObjectResult = SalesForceService.describeSObject(SalesForceObjectType)

                    Dim Fields As SalesForceApi.Field() = SalesForceObject.fields

                    For i As Integer = 0 To Fields.Length - 1
                        Dim Field As SalesForceApi.Field = Fields(i)
                        If Field.name = SalesForceFieldName Then
                            If Field.type = 1 Or Field.type = 2 Then
                                Dim OptionValues As SalesForceApi.PicklistEntry() = Field.picklistValues
                                For j As Integer = 0 To OptionValues.Length - 1
                                    ddlFormField.Items.Add(New ListItem(OptionValues(j).label, OptionValues(j).value))
                                Next
                            Else
                                ddlFormField.Items.Add(New ListItem("ERROR", "ERROR"))
                            End If
                            Exit For
                        End If
                    Next
                Catch ex As Exception
                    Emagine.LogError(ex)
                    ddlFormField.Items.Add(New ListItem("ERROR", "ERROR"))
                End Try
                

            Case Is > 100
                Dim objRs As SqlDataReader
                Dim strSQL As String = "SELECT OptionText, OptionValue FROM LookupOptions WHERE LookupID = " & intListTypeID & " ORDER BY SortOrder"

                Try
                    objRs = GetDataReader(strSQL)
                    ddlFormField.DataSource = objRs
                    ddlFormField.DataTextField = objRs.GetName(0)
                    ddlFormField.DataValueField = objRs.GetName(1)
                    ddlFormField.DataBind()
                Catch ex As Exception
                    Emagine.LogError(ex)
                    ddlFormField.Items.Add(New ListItem("ERROR", "ERROR"))
                End Try

                Try
                    ddlFormField.SelectedValue = strFieldValue
                Catch ex As Exception
                    ddlFormField.SelectedIndex = 0
                End Try

        End Select

        Return ddlFormField
    End Function

    Private Shared Function GetListBox(ByVal intFieldID As Integer, ByVal strFieldValue As String, ByVal intListTypeID As Integer, ByVal strListOptions As String, ByVal intWidth As Integer, ByVal intHeight As Integer, ByVal strCssClass As String, ByVal intMinRequired As Integer, ByVal intMaxRequired As Integer) As ListBox
        Dim lstFormField As New ListBox()
        Dim intCounter As Integer = 0

        lstFormField.AppendDataBoundItems = True
        lstFormField.ID = intFieldID
        lstFormField.SelectionMode = ListSelectionMode.Multiple
        If intWidth > 0 And intHeight > 0 Then
            lstFormField.Width = intWidth
            lstFormField.Height = intHeight
        End If

        lstFormField.CssClass = strCssClass

        Select Case intListTypeID
            Case 1
                Dim objRs As SqlDataReader

                Try
                    objRs = GetDataReader(strListOptions)

                    lstFormField.DataSource = objRs
                    lstFormField.DataTextField = objRs.GetName(0)
                    lstFormField.DataValueField = objRs.GetName(1)
                    lstFormField.DataBind()

                Catch ex As Exception
                    Emagine.LogError(ex)
                    lstFormField.Items.Add(New ListItem("ERROR", "ERROR"))
                End Try

                Try
                    lstFormField.SelectedValue = strFieldValue
                Catch ex As Exception
                    lstFormField.SelectedIndex = 0
                End Try

            Case 2
                Try
                    Dim SalesForceFieldName As String = Emagine.GetDbValue("SELECT SalesForceFieldName FROM FormFields WHERE FieldID = " & intFieldID)
                    Dim SalesForceObjectType As String = Emagine.GetDbValue("SELECT SalesForceObjectType FROM FormFields WHERE FieldID = " & intFieldID)
                    Dim SalesForceService As SalesForceApi.SforceService = SalesForceConnector.GetSalesForceService()
                    Dim SalesForceObject As SalesForceApi.DescribeSObjectResult = SalesForceService.describeSObject(SalesForceObjectType)

                    Dim Fields As SalesForceApi.Field() = SalesForceObject.fields

                    For i As Integer = 0 To Fields.Length - 1
                        Dim Field As SalesForceApi.Field = Fields(i)
                        If Field.name = SalesForceFieldName Then
                            If Field.type = 1 Or Field.type = 2 Then
                                Dim OptionValues As SalesForceApi.PicklistEntry() = Field.picklistValues
                                For j As Integer = 0 To OptionValues.Length - 1
                                    lstFormField.Items.Add(New ListItem(OptionValues(j).label, OptionValues(j).value))
                                Next
                            Else
                                lstFormField.Items.Add(New ListItem("ERROR", "ERROR"))
                            End If
                            Exit For
                        End If
                    Next
                Catch ex As Exception
                    Emagine.LogError(ex)
                    lstFormField.Items.Add(New ListItem("ERROR", "ERROR"))
                End Try
                

            Case Is > 100
                Dim objRs As SqlDataReader
                Dim strSQL As String = "SELECT OptionText, OptionValue FROM LookupOptions WHERE LookupID = " & intListTypeID & " ORDER BY SortOrder"

                Try
                    objRs = GetDataReader(strSQL)
                    lstFormField.DataSource = objRs
                    lstFormField.DataTextField = objRs.GetName(0)
                    lstFormField.DataValueField = objRs.GetName(1)
                    lstFormField.DataBind()
                Catch ex As Exception
                    Emagine.LogError(ex)
                    lstFormField.Items.Add(New ListItem("ERROR", "ERROR"))
                End Try

                Try
                    lstFormField.SelectedValue = strFieldValue
                Catch ex As Exception
                    lstFormField.SelectedIndex = 0
                End Try

        End Select

        Return lstFormField
    End Function

    Private Shared Function GetCheckBoxList(ByVal intFieldID As Integer, ByVal strFieldValue As String, ByVal intListTypeID As Integer, ByVal strListOptions As String, ByVal strCssClass As String, ByVal intMinRequired As Integer, ByVal intMaxRequired As Integer, ByVal intColCount As Integer) As CheckBoxList
        Dim cbxFormField As New CheckBoxList()
        Dim intCounter As Integer = 0

        cbxFormField.AppendDataBoundItems = True
        cbxFormField.ID = intFieldID
        cbxFormField.RepeatDirection = RepeatDirection.Horizontal
        cbxFormField.RepeatColumns = intColCount

        cbxFormField.CssClass = strCssClass

        Select Case intListTypeID
            Case 1
                Dim objRs As SqlDataReader

                Try
                    objRs = GetDataReader(strListOptions)

                    cbxFormField.DataSource = objRs
                    cbxFormField.DataTextField = objRs.GetName(0)
                    cbxFormField.DataValueField = objRs.GetName(1)
                    cbxFormField.DataBind()

                Catch ex As Exception
                    Emagine.LogError(ex)
                    cbxFormField.Items.Add(New ListItem("ERROR", "ERROR"))
                End Try

                Try
                    cbxFormField.SelectedValue = strFieldValue
                Catch ex As Exception
                    cbxFormField.SelectedIndex = 0
                End Try

            Case 2
                Try
                    Dim SalesForceFieldName As String = Emagine.GetDbValue("SELECT SalesForceFieldName FROM FormFields WHERE FieldID = " & intFieldID)
                    Dim SalesForceObjectType As String = Emagine.GetDbValue("SELECT SalesForceObjectType FROM FormFields WHERE FieldID = " & intFieldID)
                    Dim SalesForceService As SalesForceApi.SforceService = SalesForceConnector.GetSalesForceService()
                    Dim SalesForceObject As SalesForceApi.DescribeSObjectResult = SalesForceService.describeSObject(SalesForceObjectType)

                    Dim Fields As SalesForceApi.Field() = SalesForceObject.fields

                    For i As Integer = 0 To Fields.Length - 1
                        Dim Field As SalesForceApi.Field = Fields(i)
                        If Field.name = SalesForceFieldName Then
                            If Field.type = 1 Or Field.type = 2 Then
                                Dim OptionValues As SalesForceApi.PicklistEntry() = Field.picklistValues
                                For j As Integer = 0 To OptionValues.Length - 1
                                    cbxFormField.Items.Add(New ListItem(OptionValues(j).label, OptionValues(j).value))
                                Next
                            Else
                                cbxFormField.Items.Add(New ListItem("ERROR", "ERROR"))
                            End If
                            Exit For
                        End If
                    Next
                Catch ex As Exception
                    Emagine.LogError(ex)
                    cbxFormField.Items.Add(New ListItem("ERROR", "ERROR"))
                End Try


            Case Is > 100
                Dim objRs As SqlDataReader
                Dim strSQL As String = "SELECT OptionText, OptionValue FROM LookupOptions WHERE LookupID = " & intListTypeID & " ORDER BY SortOrder"

                Try
                    objRs = GetDataReader(strSQL)
                    cbxFormField.DataSource = objRs
                    cbxFormField.DataTextField = objRs.GetName(0)
                    cbxFormField.DataValueField = objRs.GetName(1)
                    cbxFormField.DataBind()
                Catch ex As Exception
                    Emagine.LogError(ex)
                    cbxFormField.Items.Add(New ListItem("ERROR", "ERROR"))
                End Try

                Try
                    cbxFormField.SelectedValue = strFieldValue
                Catch ex As Exception
                    cbxFormField.SelectedIndex = 0
                End Try

        End Select

        For Each Item As ListItem In cbxFormField.Items
            Dim aryValues As Array = strFieldValue.Split("||")

            If Item.Value = "OtherWithText" Then
                Dim FieldValue As String = ""

                If strFieldValue.Length > 0 Then
                    For i As Integer = 0 To aryValues.GetUpperBound(0)
                        If aryValues(i).ToString.IndexOf(Item.Text) > -1 Then
                            Item.Selected = True
                            FieldValue = aryValues(i).ToString.Replace(Item.Text, "").Trim
                            Exit For
                        End If
                    Next
                End If
                Item.Text = Item.Text & " <input type='textbox' name='txtOther" & cbxFormField.ID & "' width='200' class='form-textbox' value='" & FieldValue & "' />"
            Else
                For i As Integer = 0 To aryValues.GetUpperBound(0)
                    If aryValues(i).ToString = Item.Value Then
                        Item.Selected = True
                        Exit For
                    End If
                Next
            End If
        Next

        Return cbxFormField
    End Function

    Private Shared Function GetFileUpload(ByVal intFieldID As Integer, ByVal intWidth As Integer, ByVal intHeight As Integer, ByVal strCssClass As String) As FileUpload
        Dim uplFormField As New FileUpload()
        uplFormField.ID = intFieldID
        uplFormField.Width = intWidth
        uplFormField.Height = intHeight
        uplFormField.CssClass = strCssClass

        Return uplFormField
    End Function

    Public Shared Function GetRadioButtonList(ByVal intFieldID As Integer, ByVal strFieldValue As String, ByVal intListTypeID As Integer, ByVal strListOptions As String, ByVal strCssClass As String, ByVal intMinRequired As Integer, ByVal intMaxRequired As Integer, ByVal intColCount As Integer) As RadioButtonList
        Dim rblFormField As New RadioButtonList()
        Dim intCounter As Integer = 0

        rblFormField.AppendDataBoundItems = True
        rblFormField.ID = intFieldID
        rblFormField.CssClass = strCssClass
        rblFormField.RepeatDirection = RepeatDirection.Horizontal
        rblFormField.RepeatColumns = intColCount

        Select Case intListTypeID
            Case 1
                Dim objRs As SqlDataReader

                Try
                    objRs = GetDataReader(strListOptions)

                    rblFormField.DataSource = objRs
                    rblFormField.DataTextField = objRs.GetName(0)
                    rblFormField.DataValueField = objRs.GetName(1)
                    rblFormField.DataBind()

                Catch ex As Exception
                    Emagine.LogError(ex)
                    rblFormField.Items.Add(New ListItem("ERROR", "ERROR"))
                End Try

                Try
                    rblFormField.SelectedValue = strFieldValue
                Catch ex As Exception
                    rblFormField.SelectedIndex = 0
                End Try

            Case 2

                Try
                    Dim SalesForceFieldName As String = Emagine.GetDbValue("SELECT SalesForceFieldName FROM FormFields WHERE FieldID = " & intFieldID)
                    Dim SalesForceObjectType As String = Emagine.GetDbValue("SELECT SalesForceObjectType FROM FormFields WHERE FieldID = " & intFieldID)
                    Dim SalesForceService As SalesForceApi.SforceService = SalesForceConnector.GetSalesForceService()
                    Dim SalesForceObject As SalesForceApi.DescribeSObjectResult = SalesForceService.describeSObject(SalesForceObjectType)

                    Dim Fields As SalesForceApi.Field() = SalesForceObject.fields

                    For i As Integer = 0 To Fields.Length - 1
                        Dim Field As SalesForceApi.Field = Fields(i)
                        If Field.name = SalesForceFieldName Then
                            If Field.type = 1 Or Field.type = 2 Then
                                Dim OptionValues As SalesForceApi.PicklistEntry() = Field.picklistValues
                                For j As Integer = 0 To OptionValues.Length - 1
                                    rblFormField.Items.Add(New ListItem(OptionValues(j).label, OptionValues(j).value))
                                Next
                            Else
                                rblFormField.Items.Add(New ListItem("ERROR", "ERROR"))
                            End If
                            Exit For
                        End If
                    Next
                Catch ex As Exception
                    Emagine.LogError(ex)
                    rblFormField.Items.Add(New ListItem("ERROR", "ERROR"))
                End Try
                

            Case Is > 100
                Dim objRs As SqlDataReader
                Dim strSQL As String = "SELECT OptionText, OptionValue FROM LookupOptions WHERE LookupID = " & intListTypeID & " ORDER BY SortOrder"

                Try
                    objRs = GetDataReader(strSQL)
                    rblFormField.DataSource = objRs
                    rblFormField.DataTextField = objRs.GetName(0)
                    rblFormField.DataValueField = objRs.GetName(1)
                    rblFormField.DataBind()
                Catch ex As Exception
                    Emagine.LogError(ex)
                    rblFormField.Items.Add(New ListItem("ERROR", "ERROR"))
                End Try

                Try
                    rblFormField.SelectedValue = strFieldValue
                Catch ex As Exception
                    rblFormField.SelectedIndex = 0
                End Try
        End Select




        For Each Item As ListItem In rblFormField.Items
            Dim aryValues As Array = strFieldValue.Split("||")

            If Item.Value = "OtherWithText" Then
                Dim FieldValue As String = ""

                If strFieldValue.Length > 0 Then
                    For i As Integer = 0 To aryValues.GetUpperBound(0)
                        If aryValues(i).ToString.IndexOf(Item.Text) > -1 Then
                            Item.Selected = True
                            FieldValue = aryValues(i).ToString.Replace(Item.Text, "").Trim
                            Exit For
                        End If
                    Next
                End If
                Item.Text = Item.Text & " <input type='textbox' name='txtOther" & rblFormField.ID & "' width='200' class='form-textbox' value='" & FieldValue & "' />"
            Else
                For i As Integer = 0 To aryValues.GetUpperBound(0)
                    If aryValues(i).ToString = Item.Value Then
                        Item.Selected = True
                        Exit For
                    End If
                Next
            End If
        Next

        Return rblFormField
    End Function

    Private Shared Function GetSocialSecurityNumberTextBox(ByVal intFieldID As Integer, ByVal strFieldValue As String, ByVal intWidth As Integer, ByVal intHeight As Integer, ByVal strCssClass As String, ByVal intMaxLength As Integer) As MultiSegmentDataEntry

        Dim FormField As New MultiSegmentDataEntry()
        FormField.ID = intFieldID
        FormField.Width = intWidth
        FormField.Height = intHeight
        FormField.CssClass = "VAMMultiSegContainer"

        Dim Segment1 As New TextSegment()
        Segment1.CssClass = "VAMMultiSegTextBox"
        'Segment1.TabOnTheseKeys = "-,. "
        Segment1.MaxLength = "3"
        Segment1.MinLength = "3"
        'Segment1.Width = "30px"
        Segment1.AutoWidth = "True"
        'Segment1.DisplayTextBefore = "("
        Segment1.DisplayTextAfter = " - "
        Segment1.FormattingTextAfter = "-"
        'Segment1.FormattingTextBefore = "("
        Segment1.IgnoreTheseCharsAfter = " "
        'Segment1.IgnoreTheseCharsBefore = " "
        FormField.Segments.Add(Segment1)

        Dim Segment2 As New TextSegment()
        Segment2.CssClass = "VAMMultiSegTextBox"
        Segment2.TabOnTheseKeys = "-,. "
        Segment2.MaxLength = "2"
        Segment2.MinLength = "2"
        'Segment2.Width = "30px"
        Segment2.AutoWidth = "True"
        Segment2.FormattingTextAfter = "-"
        Segment2.IgnoreTheseCharsAfter = " "
        FormField.Segments.Add(Segment2)

        Dim Segment3 As New TextSegment()
        Segment3.CssClass = "VAMMultiSegTextBox"
        Segment3.TabOnTheseKeys = "-,. "
        Segment3.MaxLength = "4"
        Segment3.MinLength = "4"
        'Segment3.Width = "40px"
        Segment3.AutoWidth = "True"
        Segment3.IgnoreTheseCharsAfter = " "
        FormField.Segments.Add(Segment3)

        FormField.Text = strFieldValue

        Return FormField
    End Function

    Private Shared Function GetHiddenField(ByVal intFieldID As Integer, ByVal strFieldValue As String) As HiddenField
        Dim hidFormField As New HiddenField()
        hidFormField.ID = intFieldID
        hidFormField.Value = strFieldValue

        Return hidFormField
    End Function

    Private Shared Function GetHiddenFieldInfo(ByVal intFieldID As Integer, ByVal strName As String, ByVal strValue As String) As Label
        Dim lblFormField As New Label()
        Dim strFormFieldInfo As String = ""

        strFormFieldInfo = "{ HIDDEN: Name=" & strName & "; Value=" & strValue & "; }"

        lblFormField.CssClass = "form_label"
        lblFormField.Text = strFormFieldInfo

        Return lblFormField
    End Function

    Public Shared Function UpdateSortOrder(ByVal intFieldID As Integer, ByVal intSortOrder As Integer) As Boolean
        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim objCommand As New SqlCommand
        Dim objField As New FormFields01
        Dim blnResult As Boolean = False
        Dim intOldSortOrder As Integer
        Dim intFormID As Integer
        Dim strSQL As String = ""

        objCommand.Connection = objConn
        objField = objField.GetFormFieldInfo(intFieldID)
        intFormID = objField.FormID
        intOldSortOrder = objField.SortOrder

        If intOldSortOrder > intSortOrder Then
            strSQL = "UPDATE FormFields SET SortOrder = SortOrder + 1 WHERE FormID = " & intFormID & " AND SortOrder >= " & intSortOrder & " AND SortOrder < " & intOldSortOrder & " AND FieldID <> " & intFieldID
        ElseIf intOldSortOrder < intSortOrder Then
            strSQL = "UPDATE FormFields SET SortOrder = SortOrder - 1 WHERE FormID = " & intFormID & " AND SortOrder <= " & intSortOrder & " AND SortOrder > " & intOldSortOrder & " AND FieldID <> " & intFieldID
        End If

        Try
            objConn.Open()

            If strSQL.Length > 0 Then
                objCommand.CommandText = strSQL
                objCommand.ExecuteNonQuery()
            End If

            If intFieldID > 0 Then
                strSQL = "UPDATE FormFields SET SortOrder = " & intSortOrder & " WHERE FieldID = " & intFieldID
                objCommand.CommandText = strSQL
                objCommand.ExecuteNonQuery()
            End If

            ResetSortOrder(intFormID)

            blnResult = True
        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            objConn.Close()
        End Try

        Return blnResult
    End Function

    Public Shared Function ResetSortOrder(ByVal intFormID As Integer) As Boolean
        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim strSQL = "SELECT FieldID, SortOrder FROM FormFields WHERE FormID = " & intFormID & " ORDER BY SortOrder"
        Dim objCommand As New SqlCommand(strSQL, objConn)
        Dim intCounter As Integer = 0
        Dim blnResult As Boolean = False

        objConn.Open()
        Dim objRs As SqlDataReader = Emagine.GetDataReader(strSQL)

        While objRs.Read
            intCounter = intCounter + 1
            objCommand.CommandText = "UPDATE FormFields SET SortOrder = " & intCounter & " WHERE FieldID = " & objRs("FieldID")
            objCommand.ExecuteNonQuery()
        End While

        objRs.Close()
        objConn.Close()

        blnResult = True

        Return blnResult
    End Function

    Public Shared Function DeleteField(ByVal intFieldID As Integer) As Boolean
        Dim _blnResult As Boolean = False
        Dim _strSQL As String = "sp_Forms01_DeleteField"
        Dim _objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim _intFormID As Integer
        Dim _objField As New FormFields01

        _objField = _objField.GetFormFieldInfo(intFieldID)
        _intFormID = _objField.FormID

        Dim _objCommand As New SqlCommand()
        _objCommand.Connection = _objConn
        _objCommand.CommandText = _strSQL
        _objCommand.CommandType = CommandType.StoredProcedure
        _objCommand.Parameters.AddWithValue("@FieldID", intFieldID)

        Try
            _objConn.Open()
            _objCommand.ExecuteNonQuery()

            ResetSortOrder(_intFormID)

            _blnResult = True
        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            If _objConn.State = ConnectionState.Open Then _objConn.Close()
        End Try

        Return _blnResult
    End Function

    Public Shared Sub GetStyles(ByVal strFilePath As String, ByVal ddlList As DropDownList)
        Dim strLine As String

        Try
            Dim sr As StreamReader = New StreamReader(strFilePath)
            Do
                strLine = sr.ReadLine()

                If Left(strLine, 1) = "." Then
                    If Len(strLine) > 0 Then
                        If InStr(strLine, "{") > 0 Then strLine = Trim(Left(strLine, InStr(1, strLine, "{") - 1))
                        strLine = Trim(Right(strLine, Len(strLine) - 1))
                        If InStr(strLine, ":") = 0 Then ddlList.Items.Add(New ListItem(strLine, strLine))
                    End If
                End If

            Loop Until strLine Is Nothing
            sr.Close()
        Catch ex As Exception
            Emagine.LogError(ex)
        End Try

    End Sub

    Public Shared Function AddLookup(ByVal strLookupName As String) As Integer
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_Forms01_AddLookup"
        Dim objCommand As New SqlCommand(SQL, Conn)
        Dim Result As Integer = 0

        objCommand.CommandType = CommandType.StoredProcedure
        objCommand.Parameters.AddWithValue("@LookupName", strLookupName)
        objCommand.Parameters.Add("@LookupID", SqlDbType.Int)
        objCommand.Parameters("@LookupID").Direction = ParameterDirection.Output
        Try
            Conn.Open()

            objCommand.ExecuteNonQuery()
            Result = objCommand.Parameters("@LookupID").Value

        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            If Conn.State = ConnectionState.Open Then Conn.Dispose()
        End Try

        Return Result
    End Function

    Public Shared Function UpdateLookup(ByVal intLookupID As Integer, ByVal strLookupName As String) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_Forms01_UpdateLookup"
        Dim objCommand As New SqlCommand(SQL, Conn)
        Dim Result As Boolean = False

        objCommand.CommandType = CommandType.StoredProcedure
        objCommand.Parameters.AddWithValue("@LookupID", intLookupID)
        objCommand.Parameters.AddWithValue("@LookupName", strLookupName)

        Try
            Conn.Open()

            Result = objCommand.ExecuteNonQuery()

        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            If Conn.State = ConnectionState.Open Then Conn.Dispose()
        End Try

        Return Result
    End Function

    Public Shared Function DeleteLookup(ByVal intLookupID As Integer) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_Forms01_DeleteLookup"
        Dim objCommand As New SqlCommand(SQL, Conn)
        Dim Result As Boolean = False

        objCommand.CommandType = CommandType.StoredProcedure
        objCommand.Parameters.AddWithValue("@LookupID", intLookupID)

        Try
            Conn.Open()

            Result = objCommand.ExecuteNonQuery()

        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            If Conn.State = ConnectionState.Open Then Conn.Dispose()
        End Try

        Return Result
    End Function

    Public Shared Function AddLookupValues(ByVal intLookupID As Integer, ByVal strLookupValues As String) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim aryOptions() As String = Split(strLookupValues, vbCrLf)
        Dim i As Integer
        Dim OptionDelimiter As String = "^"
        Dim OptionText As String = ""
        Dim OptionValue As String = ""
        Dim Result As Boolean = False
        Dim SQL As String = "sp_Forms01_AddLookupOption"
        Dim Command As New SqlCommand(SQL, Conn)

        Command.CommandType = CommandType.StoredProcedure
        Command.Parameters.AddWithValue("@LookupID", intLookupID)
        Command.Parameters.Add("@OptionText", SqlDbType.VarChar)
        Command.Parameters.Add("@OptionValue", SqlDbType.VarChar)

        Try
            Conn.Open()
        Catch ex As Exception
            Emagine.LogError(ex)
        End Try

        For i = 0 To UBound(aryOptions)
            If Len(Trim(aryOptions(i))) > 0 Then
                If InStr(aryOptions(i), OptionDelimiter) Then
                    OptionText = Trim(Left(aryOptions(i), InStr(aryOptions(i), OptionDelimiter) - 1))
                    OptionValue = Trim(Right(aryOptions(i), (Len(aryOptions(i)) - InStr(aryOptions(i), OptionDelimiter))))
                Else
                    OptionText = Trim(aryOptions(i))
                    OptionValue = Trim(aryOptions(i))
                End If

                Command.Parameters("@OptionText").Value = OptionText
                Command.Parameters("@OptionValue").Value = OptionValue

                Try
                    Command.ExecuteNonQuery()
                    Result = True
                Catch ex As Exception
                    Emagine.LogError(ex)
                End Try
            End If
        Next

        If Conn.State = ConnectionState.Open Then Conn.Dispose()

        Return Result
    End Function

    Public Shared Function UpdateLookupOption(ByVal intOptionID As Integer, ByVal strOptionText As String, ByVal strOptionValue As String) As Boolean
        Dim Result As Boolean = False
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_Forms01_UpdateLookupOption"
        Dim Command As New SqlCommand(SQL, Conn)
        Command.CommandType = CommandType.StoredProcedure
        Command.Parameters.AddWithValue("@OptionID", intOptionID)
        Command.Parameters.AddWithValue("@OptionText", strOptionText)
        Command.Parameters.AddWithValue("@OptionValue", strOptionValue)

        Try
            Conn.Open()
            Result = Command.ExecuteNonQuery()
        Catch ex As Exception
            Emagine.LogError(ex)
        End Try


        Return Result
    End Function

    'Public Shared Function GetLookupOptions(ByVal intLookupID As Integer) As SqlDataReader
    ' Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
    'Dim SQL As String = "sp_Forms01_GetLookupOptions"
    'Dim Command As New SqlCommand(Sql, Conn)
    'Dim Rs As SqlDataReader

    '   Command.CommandType = CommandType.StoredProcedure
    '   Command.Parameters.AddWithValue("@LookupID", intLookupID)

    '  Conn.Open()
    ' Return Command.ExecuteReader()

    'End Function

    Public Shared Function DeleteLookupOptions(ByVal intLookupID As Integer) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_Forms01_DeleteLookupOptions"
        Dim objCommand As New SqlCommand(SQL, Conn)
        Dim Result As Boolean = False

        objCommand.CommandType = CommandType.StoredProcedure
        objCommand.Parameters.AddWithValue("@LookupID", intLookupID)

        Try
            Conn.Open()

            Result = objCommand.ExecuteNonQuery()

        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            If Conn.State = ConnectionState.Open Then Conn.Dispose()
        End Try

        Return Result
    End Function


    'VALIDATION

    Public Shared Sub DisplayValidators(ByVal Field As FormFields01, ByVal PlaceHolder As PlaceHolder)
        Dim FieldID As Integer = Field.FieldID
        Dim FieldTypeID As Integer = Field.FieldTypeID
        Dim DataTypeID As Integer = Field.DataTypeID
        Dim Label As String = Field.Label
        Dim MinRequired As Integer = Field.MinRequired
        Dim MaxRequired As Integer = Field.MaxRequired
        Dim MinValue As String = Field.MinValue
        Dim MaxValue As String = Field.MaxValue
        Dim AllowedChars As String = Field.AllowedChars
        Dim DisallowedChars As String = Field.DisallowedChars
        Dim ValidationText As String = Field.ValidationText
        Dim ValidationCss As String = Field.ValidationCSS

        Select Case FieldTypeID
            Case 2 'Text Box
                Select Case DataTypeID
                    Case 1 'Integer
                        PlaceHolder.Controls.Add(GetDataTypeValidator(FieldID, "Positive Integer", Label))
                        If MinValue.Length > 0 Or MaxValue.Length > 0 Then
                            PlaceHolder.Controls.Add(GetRangeValidator(FieldID, "Positive Integer", MinValue, MaxValue, Label))
                        End If

                    Case 2 'Currency
                        PlaceHolder.Controls.Add(GetDataTypeValidator(FieldID, "Currency", Label))
                        If MinValue.Length > 0 Or MaxValue.Length > 0 Then
                            PlaceHolder.Controls.Add(GetRangeValidator(FieldID, "Currency", MinValue, MaxValue, Label))
                        End If

                    Case 3 'Decimal
                        PlaceHolder.Controls.Add(GetDataTypeValidator(FieldID, "Decimal", Label))
                        If MinValue.Length > 0 Or MaxValue.Length > 0 Then
                            PlaceHolder.Controls.Add(GetRangeValidator(FieldID, "Decimal", MinValue, MaxValue, Label))
                        End If

                    Case 10 'Date
                        PlaceHolder.Controls.Add(GetDataTypeValidator(FieldID, "Date", Label))
                        If MinValue.Length > 0 Or MaxValue.Length > 0 Then
                            PlaceHolder.Controls.Add(GetRangeValidator(FieldID, "Date", MinValue, MaxValue, Label))
                        End If

                    Case 11 'US Phone
                        PlaceHolder.Controls.Add(GetPhoneValidator(FieldID, Label))

                    Case 12 'International Phone
                        PlaceHolder.Controls.Add(GetPhoneValidator(FieldID, Label))

                    Case 13 'ZIP Code
                        PlaceHolder.Controls.Add(GetZipCodeValidator(FieldID, Label))

                    Case 20 'Email
                        PlaceHolder.Controls.Add(GetEmailAddressValidator(FieldID, Label))

                    Case 21 'URL
                        PlaceHolder.Controls.Add(GetURLValidator(FieldID, Label))

                    Case 30 'Credit Card Number

                    Case 31 'Social Security Number


                    Case Else
                        If Trim(AllowedChars.Length) > 0 And AllowedChars Is DBNull.Value = False Then
                            PlaceHolder.Controls.Add(GetCharacterValidator(FieldID, Label))
                        End If

                End Select

                If MinRequired > 0 And MaxRequired > 0 Then
                    PlaceHolder.Controls.Add(GetRequiredTextValidator(FieldID, Label))
                    PlaceHolder.Controls.Add(GetLengthValidator(FieldID, MinRequired, MaxRequired, Label))
                End If

            Case 3 'Text Area
                If MinRequired > 0 And MaxRequired > 0 Then
                    PlaceHolder.Controls.Add(GetRequiredTextValidator(FieldID, Label))
                    PlaceHolder.Controls.Add(GetLengthValidator(FieldID, MinRequired, MaxRequired, Label))
                End If
                If Trim(AllowedChars.Length) > 0 And AllowedChars Is DBNull.Value = False Then
                    PlaceHolder.Controls.Add(GetCharacterValidator(FieldID, Label))
                End If

            Case 4 'Drop-Down List
                If MinRequired > 0 Then
                    PlaceHolder.Controls.Add(GetRequiredListValidator(FieldID, Label))
                ElseIf MinRequired > 1 Then
                    PlaceHolder.Controls.Add(GetCountSelectionsValidator(FieldID, MinRequired, MaxRequired, Label))
                End If

            Case 5  'Check Box List
                If MinRequired > 0 Then
                    PlaceHolder.Controls.Add(GetCountSelectionsValidator(FieldID, MinRequired, MaxRequired, Label))
                End If

            Case 6 'Radio Button List
                If MinRequired > 0 Then
                    PlaceHolder.Controls.Add(GetRadioValidator(FieldID, Label))
                End If

            Case 7 'File Upload
                If MinRequired > 0 Then
                    PlaceHolder.Controls.Add(GetUploadValidator(FieldID, Label))
                End If
                If Len(AllowedChars) > 0 Then
                    PlaceHolder.Controls.Add(GetFileExtensionValidator(FieldID, Label, AllowedChars))
                End If

        End Select

    End Sub

    Private Shared Function GetLengthValidator(ByVal intFieldID As Integer, ByVal intMinRequired As Integer, ByVal intMaxRequired As Integer, ByVal strLabel As String) As TextLengthValidator
        Dim Validator As New TextLengthValidator()

        Validator.ID = "LengthValidator_" & intFieldID
        Validator.ControlIDToEvaluate = intFieldID.ToString
        Validator.ErrorMessage = "*"
        Validator.SummaryErrorMessage = strLabel & " Please enter between {MINIMUM} and {MAXIMUM} characters."
        Validator.Trim = True
        Validator.EventsThatValidate = ValidationEvents.All
        Validator.Minimum = intMinRequired
        Validator.Maximum = intMaxRequired
        Validator.ShowRequiredFieldMarker = False

        Return Validator
    End Function

    Private Shared Function GetPhoneValidator(ByVal intFieldID As Integer, ByVal strErrorMessage As String) As MultiSegmentDataEntryValidator
        Dim Validator As New MultiSegmentDataEntryValidator

        Validator.ID = "PhoneValidator_" & intFieldID
        Validator.ControlIDToEvaluate = intFieldID.ToString
        Validator.ErrorFormatter.Display = ValidatorDisplay.Dynamic
        Validator.ErrorMessage = strErrorMessage

        Return Validator
    End Function

    Private Shared Function GetRequiredTextValidator(ByVal intFieldID As Integer, ByVal strLabel As String) As RequiredTextValidator
        Dim Validator As New RequiredTextValidator

        Validator.ID = "RequiredTextValidator_" & intFieldID
        Validator.ControlIDToEvaluate = intFieldID.ToString
        Validator.ErrorMessage = "*"
        Validator.SummaryErrorMessage = strLabel & " Please enter a valid phone."
        Validator.Trim = True
        Validator.ShowRequiredFieldMarker = False
        Validator.EventsThatValidate = ValidationEvents.OnChange

        Return Validator
    End Function

    Private Shared Function GetDataTypeValidator(ByVal intFieldID As Integer, ByVal strDataType As String, ByVal strLabel As String) As DataTypeCheckValidator
        Dim Validator As New DataTypeCheckValidator()

        Validator.ID = "DataTypeValidator_" & intFieldID
        Validator.ControlIDToEvaluate = intFieldID.ToString
        Validator.ErrorMessage = "*"
        Validator.SummaryErrorMessage = strLabel & " Not a valid " & strDataType & "."
        Validator.DataType = strDataType

        Return Validator
    End Function

    Private Shared Function GetCharacterValidator(ByVal intFieldID As Integer, ByVal strLabel As String) As CharacterValidator
        Dim Validator As New CharacterValidator()

        Validator.ID = "CharacterValidator_" & intFieldID
        Validator.ControlIDToEvaluate = intFieldID.ToString
        Validator.ErrorMessage = "*"
        Validator.SummaryErrorMessage = strLabel & " Invalid characters."

        Return Validator
    End Function

    Private Shared Function GetRangeValidator(ByVal intFieldID As Integer, ByVal strDataType As String, ByVal strMinValue As String, ByVal strMaxValue As String, ByVal strLabel As String) As RangeValidator
        Dim Validator As New RangeValidator()

        Validator.ID = "RangeValidator_" & intFieldID
        Validator.ControlIDToEvaluate = intFieldID.ToString
        Validator.ErrorMessage = "*"
        Validator.SummaryErrorMessage = strLabel & " Please enter a value between " & strMinValue & " and " & strMaxValue & "."
        Validator.DataType = strDataType
        Validator.Minimum = strMinValue
        Validator.Maximum = strMaxValue

        Return Validator
    End Function

    Private Shared Function GetEmailAddressValidator(ByVal intFieldID As Integer, ByVal strLabel As String) As EmailAddressValidator
        Dim Validator As New EmailAddressValidator()

        Validator.ID = "EmailAddressValidator_" & intFieldID
        Validator.ControlIDToEvaluate = intFieldID.ToString
        Validator.ErrorMessage = "*"
        Validator.SummaryErrorMessage = strLabel & " Please enter a valid email address."
        Validator.Trim = True

        Return Validator
    End Function

    Private Shared Function GetZipCodeValidator(ByVal intFieldID As Integer, ByVal strLabel As String) As RegexValidator
        Dim Validator As New RegexValidator()

        Validator.ID = "ZipCodeValidator_" & intFieldID
        Validator.ControlIDToEvaluate = intFieldID.ToString
        Validator.ErrorMessage = "*"
        Validator.SummaryErrorMessage = strLabel & " Please enter a valid zip code."
        Validator.Trim = True
        Validator.Expression = "^(\d{5}-\d{4}|\d{5})$"

        Return Validator
    End Function

    Private Shared Function GetURLValidator(ByVal intFieldID As Integer, ByVal strLabel As String) As RegexValidator
        Dim Validator As New RegexValidator()

        Validator.ID = "UrlValidator_" & intFieldID
        Validator.ControlIDToEvaluate = intFieldID.ToString
        Validator.ErrorMessage = "*"
        Validator.SummaryErrorMessage = strLabel & " Please enter a valid URL."
        Validator.Trim = True
        Validator.Expression = "^(?:ftp\://|http\://|mailto\://|https\://|file\://)?(\w+\@)?(www\.)?\w+(\.\w+)+(\:\d+)?"

        Return Validator
    End Function

    Private Shared Function GetRequiredListValidator(ByVal intFieldID As Integer, ByVal strLabel As String) As RequiredListValidator
        Dim Validator As New RequiredListValidator()

        Validator.ID = "RequiredListValidator_" & intFieldID
        Validator.ControlIDToEvaluate = intFieldID.ToString
        Validator.UnassignedIndex = 0
        Validator.ErrorMessage = "*"
        Validator.SummaryErrorMessage = strLabel & " Please make a selection."
        Validator.Trim = True

        Return Validator
    End Function

    Private Shared Function GetRadioValidator(ByVal intFieldID As Integer, ByVal strLabel As String) As RequiredListValidator
        Dim Validator As New RequiredListValidator()

        Validator.ID = "RequiredListValidator_" & intFieldID
        Validator.ControlIDToEvaluate = intFieldID.ToString
        Validator.UnassignedIndex = -1
        'Validator.ErrorMessage = "*"
        Validator.SummaryErrorMessage = strLabel & " Please make a selection."
        Validator.Trim = True

        Return Validator
    End Function

    Private Shared Function GetCountSelectionsValidator(ByVal intFieldID As Integer, ByVal intMinRequired As Integer, ByVal intMaxRequired As Integer, ByVal strLabel As String) As CountSelectionsValidator
        Dim Validator As New CountSelectionsValidator()

        Validator.ID = "CountSelectionsValidator_" & intFieldID
        Validator.ControlIDToEvaluate = intFieldID.ToString
        Validator.Trim = True
        Validator.Minimum = intMinRequired
        If intMaxRequired < intMinRequired Then intMaxRequired = intMinRequired
        Validator.Maximum = intMaxRequired
        Validator.ErrorMessage = "*"

        If intMaxRequired > intMinRequired Then
            Validator.SummaryErrorMessage = strLabel & " Please make between " & intMinRequired & " and " & intMaxRequired & " selections."
        Else
            Validator.SummaryErrorMessage = strLabel & " Please make at least " & intMinRequired & " selections."
        End If

        Return Validator
    End Function

    Private Shared Function GetUploadValidator(ByVal intFieldID As Integer, ByVal strLabel As String) As RequiredTextValidator
        Dim Validator As New RequiredTextValidator()

        Validator.ID = "UploadValidator_" & intFieldID
        Validator.ControlIDToEvaluate = intFieldID.ToString
        Validator.ErrorMessage = "*"
        Validator.SummaryErrorMessage = strLabel & " Choose a file to upload."
        Validator.Trim = True

        Return Validator
    End Function

    Private Shared Function GetFileExtensionValidator(ByVal intFieldID As Integer, ByVal strLabel As String, ByVal strAllowedExtensions As String) As CompareToStringsValidator
        Dim Validator As New CompareToStringsValidator()
        Dim aryFileExtensions As Array = Split(strAllowedExtensions, ",")
        Dim i As Integer

        Validator.ID = "FileExtensionValidator_" & intFieldID
        Validator.ControlIDToEvaluate = intFieldID.ToString
        Validator.MatchTextRule = MatchTextRule.EndsWith
        Validator.CaseInsensitive = True
        Validator.ErrorMessage = "*"
        Validator.SummaryErrorMessage = strLabel & " Only files with (" & strAllowedExtensions & ") are allowed."
        Validator.Trim = True
        For i = 0 To UBound(aryFileExtensions)
            Dim Item As New CompareToStringsItem()
            Item.Value = Trim(aryFileExtensions(i).ToString())
            Validator.Items.Add(Item)
        Next

        Return Validator
    End Function

    Private Shared Function GetCreditCardValidator(ByVal intFieldID As Integer, ByVal strLabel As String) As CreditCardNumberValidator
        Dim Validator As New CreditCardNumberValidator()

        Validator.ID = "CredidtCardValidator_" & intFieldID
        Validator.ControlIDToEvaluate = intFieldID.ToString
        Validator.ErrorMessage = "*"
        Validator.SummaryErrorMessage = strLabel & " Please enter a valid credit card number."
        Validator.Trim = True

        Return Validator
    End Function

End Class