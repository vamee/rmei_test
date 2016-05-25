Imports System.Data.SqlClient

Partial Class Ezedit_Admin_Sql_Default
    Inherits System.Web.UI.Page

    Dim PageSize As Integer = 10

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.Form.ToString.IndexOf("btnSave") > -1 Then
            Dim RowID As Integer = hdnRowID.Value
            Me.PopulateEditForm(RowID, False)
        End If

        lblAlert.Text = ""
    End Sub

    Protected Sub ddlTableNames_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTableNames.Load
        gdvData.PageSize = PageSize

        If Not Page.IsPostBack Then
            ddlTableNames.DataSource = Emagine.GetDataTable("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME")
            ddlTableNames.DataTextField = "TABLE_NAME"
            ddlTableNames.DataValueField = "TABLE_NAME"
            ddlTableNames.DataBind()
        End If
    End Sub

    Protected Sub ddlTableNames_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTableNames.SelectedIndexChanged
        If ddlTableNames.SelectedIndex > 0 Then
            ddlTableNames.Items(0).Enabled = False
            btnAddNew.Visible = True
            Me.BindTableData()
        End If
    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim btnEdit As LinkButton = sender
        Dim RowID As Integer = btnEdit.CommandArgument
        hdnRowID.Value = RowID
        Me.PopulateEditForm(RowID, True)
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim btnDelete As LinkButton = sender
        Dim RowID As Integer = btnDelete.CommandArgument
        Dim SqlBuilder As New StringBuilder
        Dim Command As New SqlCommand
        Dim ErrorMessage As String = ""

        Dim DataTable As DataTable = Emagine.GetDataTable("SELECT * FROM [" & ddlTableNames.SelectedValue & "]")
        Dim DataRow As DataRow = DataTable.Rows(RowID)

        Dim TableData As SqlDataReader = Emagine.GetDataReader("SELECT * FROM [" & ddlTableNames.SelectedValue & "]")
        Dim SchemaTable As DataTable = TableData.GetSchemaTable()

        SqlBuilder.Append("DELETE FROM [" & ddlTableNames.SelectedValue & "] WHERE 1=1 ")

        For Each Row In SchemaTable.Rows
            Dim ColumnName As String = Row("ColumnName").ToString
            Dim ColumnSize As Integer = Row("ColumnSize")
            Dim IsIdentity As Boolean = Row("IsIdentity")

            If ColumnSize < 255 Then
                SqlBuilder.Append("AND " & ColumnName & "=@" & ColumnName & " ")
                Command.Parameters.AddWithValue("@" & ColumnName, DataRow(ColumnName))
            End If
        Next

        If Emagine.ExecuteSQL(SqlBuilder.ToString, Command, ErrorMessage) Then
            lblAlert.Text = "The record has been removed successfully."
            Me.BindTableData()
        Else
            lblAlert.Text = "The following error occurred while attempting to delete this record:<br/>" & ErrorMessage
        End If


        'Response.Write(SqlBuilder.ToString & "<br><br>")
        'For Each Param As SqlParameter In Command.Parameters
        '    Response.Write(Param.ParameterName & ": " & Param.Value & "<br>")
        'Next


    End Sub

    Sub PopulateEditForm(ByVal intRowID As Integer, ByVal blnPopulateForm As Boolean)
        Dim DataTable As DataTable = Emagine.GetDataTable("SELECT * FROM [" & ddlTableNames.SelectedValue & "]")

        Dim TableData As SqlDataReader = Emagine.GetDataReader("SELECT * FROM [" & ddlTableNames.SelectedValue & "]")
        Dim SchemaTable As DataTable = TableData.GetSchemaTable()

        For Each Row As DataRow In SchemaTable.Rows
            Dim FieldLabel As New Label
            FieldLabel.CssClass = "form-label"
            FieldLabel.Text = Row(0).ToString & ":"
            Dim LabelCell As New TableCell
            LabelCell.VerticalAlign = VerticalAlign.Top
            LabelCell.Controls.Add(FieldLabel)

            Dim FieldValue As String = ""
            If blnPopulateForm And intRowID >= 0 Then
                Dim DataRow As DataRow = DataTable.Rows(intRowID)
                FieldValue = DataRow(Row("ColumnName").ToString).ToString
            Else
                FieldValue = ""
            End If

            Dim Field As Control = Me.GetFormField(Row("ColumnName"), Row("DataType").ToString, Row("ColumnSize"), FieldValue, Row("IsIdentity"))

            Dim FieldCell As New TableCell
            FieldCell.Controls.Add(Field)

            Dim TableRow As New TableRow
            TableRow.Cells.Add(LabelCell)
            TableRow.Cells.Add(FieldCell)

            tblEdit.Rows.Add(TableRow)
        Next

        Dim Cell1 As New TableCell
        Dim Cell2 As New TableCell


        pnlList.Visible = False
        pnlEdit.Visible = True
    End Sub

    Function GetFormField(ByVal strFieldName As String, ByVal strDataType As String, ByVal intFieldSize As Integer, ByVal strFieldValue As String, ByVal blnIsIdentity As Boolean) As Control
        Dim Result As New Control

        Select Case strDataType
            Case "System.String"
                If intFieldSize < 4000 Then
                    Dim Field As New PeterBlum.VAM.TextBox
                    If blnIsIdentity Then Field.ReadOnly = True
                    Field.ID = "txt" & strFieldName
                    Field.CssClass = "form-textbox"
                    Field.Text = strFieldValue
                    Field.Width = 600
                    If intFieldSize >= 255 Then
                        Field.Height = 100
                        Field.TextMode = TextBoxMode.MultiLine
                    End If
                    Field.MaxLength = intFieldSize
                    Result = Field
                Else
                    Dim Field As Control = LoadControl("/Ezedit/UserControls/ContentEditor.ascx")
                    Field.ID = "txt" & strFieldName

                    Dim EditorIDProperty As System.Reflection.PropertyInfo
                    EditorIDProperty = Field.GetType().GetProperty("EditorID")
                    If Not EditorIDProperty Is Nothing Then EditorIDProperty.SetValue(Field, "txt" & strFieldName, Nothing)

                    Dim EditorWidthProperty As System.Reflection.PropertyInfo
                    EditorWidthProperty = Field.GetType().GetProperty("EditorWidth")
                    If Not EditorWidthProperty Is Nothing Then EditorWidthProperty.SetValue(Field, 600, Nothing)

                    Dim EditorHeightProperty As System.Reflection.PropertyInfo
                    EditorHeightProperty = Field.GetType().GetProperty("EditorHeight")
                    If Not EditorHeightProperty Is Nothing Then EditorHeightProperty.SetValue(Field, 400, Nothing)

                    Dim EditorContentProperty As System.Reflection.PropertyInfo
                    EditorContentProperty = Field.GetType().GetProperty("EditorContent")
                    If Not EditorContentProperty Is Nothing Then EditorContentProperty.SetValue(Field, strFieldValue, Nothing)

                    Result = Field
                End If

            Case "System.Boolean"
                Dim Field As New CheckBox
                'If blnIsIdentity Then Field.ReadOnly = True
                Field.ID = "cbx" & strFieldName
                If strFieldValue.Length > 0 Then Field.Checked = CBool(strFieldValue)
                Result = Field

            Case "System.Int16", "System.Int32", "System.Int64", "System.Double"
                Dim Field As New PeterBlum.VAM.IntegerTextBox
                If blnIsIdentity Then Field.ReadOnly = True
                Field.ID = "txt" & strFieldName
                Field.Width = 75
                Field.CssClass = "form-textbox"
                Field.Text = strFieldValue
                Field.MaxLength = 20
                Result = Field

            Case "System.DateTime"
                Dim Field As New PeterBlum.PetersDatePackage.DateTextBox
                If blnIsIdentity Then Field.ReadOnly = True
                Field.ID = "txt" & strFieldName
                Field.Width = 150
                Field.CssClass = "form-textbox"
                Field.Text = strFieldValue
                Field.MaxLength = 10
                Result = Field
        End Select

        Return Result
    End Function

    Protected Sub gdvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gdvData.PageIndexChanging
        Me.BindTableData()
    End Sub

    Protected Sub gdvData_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvData.PreRender
        For Each Row As GridViewRow In gdvData.Rows
            For Each Cell As TableCell In Row.Cells
                'Response.Write(Cell.Text & "<br />")
                If Cell.Text.Length > 100 Then Cell.Text = Cell.Text.Substring(0, 100) & "... <b>more</b>"
            Next
            'For i As Integer = 0 To (gdvData.Columns.Count - 1)

            '    For Each Control As Control In Row.Cells(i).Controls
            '        Response.Write(Control.ToString)
            '        If Control.ID IsNot Nothing Then Response.Write(" : " & Control.ID)

            '        Response.Write("<br>")
            '    Next
            'Next


            'For i As Integer = 0 To gdvData.Colum
        Next
    End Sub

    Protected Sub gdvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvData.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim btnEdit As LinkButton = e.Row.FindControl("btnEdit")
            Dim btnDelete As LinkButton = e.Row.FindControl("btnDelete")

            btnEdit.CommandArgument = e.Row.DataItemIndex
            btnDelete.CommandArgument = e.Row.DataItemIndex
        End If
    End Sub

    Sub BindTableData()
        Dim TableData As SqlDataReader = Emagine.GetDataReader("SELECT * FROM [" & ddlTableNames.SelectedValue & "]")
        Dim SchemaTable As DataTable = TableData.GetSchemaTable()

        dsTableData.SelectCommand = "SELECT * FROM [" & ddlTableNames.SelectedValue & "]"
        dsTableData.DataBind()
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        pnlList.Visible = True
        pnlEdit.Visible = False
        hdnRowID.Value = -1
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If PeterBlum.VAM.Globals.Page.IsValid Then
            Dim RowID As Integer = hdnRowID.Value
            Dim Command As New SqlCommand
            Dim SqlBuilder As New StringBuilder
            Dim ErrorMessage As String = ""

            If RowID > -1 Then
                Dim WhereClauseBuilder As New StringBuilder
                SqlBuilder.Append("UPDATE [" & ddlTableNames.SelectedValue & "] SET ")
                WhereClauseBuilder.Append(" WHERE 1=1 ")

                Dim DataTable As DataTable = Emagine.GetDataTable("SELECT * FROM [" & ddlTableNames.SelectedValue & "]")
                Dim DataRow As DataRow = DataTable.Rows(RowID)

                Dim TableData As SqlDataReader = Emagine.GetDataReader("SELECT * FROM [" & ddlTableNames.SelectedValue & "]")
                Dim SchemaTable As DataTable = TableData.GetSchemaTable()

                For Each Row In SchemaTable.Rows
                    Dim ColumnName As String = Row("ColumnName").ToString
                    Dim ColumnSize As Integer = Row("ColumnSize")
                    Dim IsIdentity As Boolean = Row("IsIdentity")

                    If Not IsIdentity Then
                        SqlBuilder.Append(ColumnName & "=@New" & ColumnName & ", ")
                        Command.Parameters.AddWithValue("@New" & ColumnName, Me.GetFieldValue(Me.GetFormField(ColumnName)))

                    End If

                    If ColumnSize < 255 Then WhereClauseBuilder.Append("AND " & ColumnName & "=@Old" & ColumnName & " ")
                    Command.Parameters.AddWithValue("@Old" & ColumnName, DataRow(ColumnName))

                Next

                SqlBuilder.Remove(SqlBuilder.Length - 2, 2)

                If Emagine.ExecuteSQL(SqlBuilder.ToString & WhereClauseBuilder.ToString, Command, ErrorMessage) Then
                    Me.BindTableData()

                    pnlList.Visible = True
                    pnlEdit.Visible = False
                    hdnRowID.Value = -1

                    lblAlert.Text = "The record has been updated successfully."
                Else
                    lblAlert.Text = "The following error occurred while attempting to update this record:<br />" & ErrorMessage
                End If

            Else
                Dim ValueBuilder As New StringBuilder
                SqlBuilder.Append("INSERT INTO [" & ddlTableNames.SelectedValue & "] (")

                Dim TableData As SqlDataReader = Emagine.GetDataReader("SELECT * FROM [" & ddlTableNames.SelectedValue & "]")
                Dim SchemaTable As DataTable = TableData.GetSchemaTable()

                For Each Row In SchemaTable.Rows
                    Dim ColumnName As String = Row("ColumnName").ToString
                    Dim ColumnSize As Integer = Row("ColumnSize")
                    Dim IsIdentity As Boolean = Row("IsIdentity")

                    If Not IsIdentity Then
                        SqlBuilder.Append(ColumnName & ", ")
                        ValueBuilder.Append("@" & ColumnName & ", ")
                        Command.Parameters.AddWithValue("@" & ColumnName, Me.GetFieldValue(Me.GetFormField(ColumnName)))
                    End If
                Next

                SqlBuilder.Remove(SqlBuilder.Length - 2, 2)
                ValueBuilder.Remove(ValueBuilder.Length - 2, 2)

                SqlBuilder.Append(") VALUES (")
                ValueBuilder.Append(")")

                If Emagine.ExecuteSQL(SqlBuilder.ToString & ValueBuilder.ToString, Command, ErrorMessage) Then
                    Me.BindTableData()

                    pnlList.Visible = True
                    pnlEdit.Visible = False
                    hdnRowID.Value = -1

                    lblAlert.Text = "The record has been added successfully."
                Else
                    lblAlert.Text = "The following error occurred while attempting to add a new record:<br />" & ErrorMessage
                End If
            End If
        End If
    End Sub

    Function GetFormField(ByVal strColumnName As String) As Control
        Dim Result As New Control

        For Each Row As TableRow In tblEdit.Rows
            Dim Cell As TableCell = Row.Cells(1)

            For Each MyControl As Control In Cell.Controls
                If MyControl.ID.Length > 0 Then
                    If MyControl.ID.ToString.IndexOf(strColumnName) > -1 Then
                        Result = MyControl
                        Exit For
                        Exit For

                    End If
                End If
            Next
        Next

        Return Result
    End Function

    Function GetFieldValue(ByVal objFormField As Control) As String
        Dim Result As String = ""

        If TypeOf objFormField Is PeterBlum.VAM.TextBox Then
            Result = DirectCast(objFormField, PeterBlum.VAM.TextBox).Text

        ElseIf TypeOf objFormField Is System.Web.UI.WebControls.CheckBox Then
            Result = DirectCast(objFormField, System.Web.UI.WebControls.CheckBox).Checked.ToString

        ElseIf TypeOf objFormField Is PeterBlum.VAM.IntegerTextBox Then
            Result = DirectCast(objFormField, PeterBlum.VAM.IntegerTextBox).Text

        ElseIf TypeOf objFormField Is PeterBlum.PetersDatePackage.DateTextBox Then
            Result = DirectCast(objFormField, PeterBlum.PetersDatePackage.DateTextBox).Text

        ElseIf TypeOf objFormField Is UserControl Then
            If objFormField.GetType.ToString = "ASP.ezedit_usercontrols_contenteditor_ascx" Then
                Dim Field As UserControl = objFormField

                Dim EditorContentProperty As System.Reflection.PropertyInfo
                EditorContentProperty = Field.GetType().GetProperty("EditorContent")
                If Not EditorContentProperty Is Nothing Then Result = EditorContentProperty.GetValue(Field, Nothing)
            End If

        End If

        Return Result
    End Function


    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        Me.PopulateEditForm(-1, False)
    End Sub
End Class
