
Partial Class Ezedit_Modules_Reports01_FormSubmissions_Default
    Inherits System.Web.UI.Page


    Protected Sub lblPageTitle_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblPageTitle.Load
        lblPageTitle.Text = "<a href='/ezedit/modules/Reports01' class='pagetitle'>Reports</a> > <span class='pagetitle'>Form Submissions</span>"
    End Sub

    Protected Sub btnQuery_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim Row As GridViewRow = Button.Parent.Parent
        Dim hdnFormID As HiddenField = Row.FindControl("hdnFormID")
        If hdnFormID IsNot Nothing Then
            Me.DisplayQueryForm(hdnFormID.Value)
        End If
    End Sub

    Protected Sub gdvFormSubmissions_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvFormSubmissions.RowDataBound
        Dim Grid As GridView = sender
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim hdnFormID As HiddenField = e.Row.FindControl("hdnFormID")
            Dim lblSubmissionCount As Label = e.Row.FindControl("lblSubmissionCount")
            If hdnFormID IsNot Nothing Then hdnFormID.Value = DataBinder.Eval(e.Row.DataItem, "FormID")
            If lblSubmissionCount IsNot Nothing Then lblSubmissionCount.Text = Emagine.GetDbValue("SELECT COUNT(FormID) AS SubmissionCount FROM FormSubmissions WHERE FormID = " & DataBinder.Eval(e.Row.DataItem, "FormID"))
        End If
    End Sub

    Sub DisplayQueryForm(ByVal intFormID As Integer)
        gdvFormSubmissions.Visible = False
        pnlQueryForm.Visible = True
        pnlPreview.Visible = False

        Dim StartDate As String = Emagine.GetDbValue("SELECT MIN(SubmissionDate) As MinSubmissionDate FROM FormSubmissions WHERE FormID = " & intFormID)
        Dim EndDate As String = Emagine.GetDbValue("SELECT MAX(SubmissionDate) As MaxSubmissionDate FROM FormSubmissions WHERE FormID = " & intFormID)

        If IsDate(StartDate) Then StartDate = FormatDateTime(StartDate, DateFormat.ShortDate)
        If IsDate(EndDate) Then EndDate = FormatDateTime(CDate(EndDate).AddDays(1), DateFormat.ShortDate)

        txtStartDate.Text = StartDate
        txtEndDate.Text = EndDate

        cblFields.DataSource = Emagine.GetDataTable("SELECT DISTINCT FormFieldName, FormFieldID FROM FormSubmissionValues WHERE SubmissionID IN (SELECT SubmissionID FROM FormSubmissions WHERE FormID = " & intFormID & ")")
        'cblFields.DataSource = Forms01.GetFields(intFormID)
        cblFields.DataTextField = "FormFieldName"
        cblFields.DataValueField = "FormFieldID"
        cblFields.DataBind()

        hdnFormID.Value = intFormID
        lblPageTitle.Text = "<a href='/ezedit/modules/Reports01' class='pagetitle'>Reports</a> > <a href='Default.aspx' class='pagetitle'>Form Submissions</a> > <span class='pagetitle'>Query Form Data</span>"
    End Sub

    Sub DisplayFormGrid()
        gdvFormSubmissions.Visible = True
        pnlQueryForm.Visible = False
        pnlPreview.Visible = False

        lblPageTitle.Text = "<a href='/ezedit/modules/Reports01' class='pagetitle'>Reports</a> > <span class='pagetitle'>Form Submissions</span>"
    End Sub

    Sub DisplayPreviewData()
        gdvFormSubmissions.Visible = False
        pnlQueryForm.Visible = False
        pnlPreview.Visible = True
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DisplayFormGrid()
    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        Dim FieldID As New StringBuilder
        Dim FormID As Integer = hdnFormID.Value

        For Each Item As ListItem In cblFields.Items
            If Item.Selected Then FieldID.Append(Item.Value & ",")
        Next
        FieldID.Append("0")

        Dim DataTable As DataTable = Emagine.GetDataTable("SELECT DISTINCT FormFieldID, FormFieldName, SortOrder FROM qryFormSubmissionValues WHERE FormID = " & FormID & " AND (SubmissionDate BETWEEN '" & txtStartDate.Text & "' AND '" & txtEndDate.Text & "') AND FormFieldID IN (" & FieldID.ToString & ") ORDER BY SortOrder, FormFieldName")
        If DataTable.Rows.Count > 0 Then
            Dim HeaderRow As New TableHeaderRow
            HeaderRow.CssClass = "table-header"

            'Dim HeaderIdCell As New TableCell
            'HeaderIdCell.Text = "ID"
            'HeaderRow.Cells.Add(HeaderIdCell)

            Dim HeaderDateCell As New TableCell
            HeaderDateCell.Text = "Date"
            HeaderRow.Cells.Add(HeaderDateCell)

            Dim HeaderUserCell As New TableCell
            HeaderUserCell.Text = "User ID"
            HeaderRow.Cells.Add(HeaderUserCell)

            For i As Integer = 0 To DataTable.Rows.Count - 1
                Dim Cell As New TableCell
                Cell.Text = DataTable.Rows(i).Item("FormFieldName")
                HeaderRow.Cells.Add(Cell)
            Next
            tblPreviewData.Rows.Add(HeaderRow)

            Me.DisplayPreviewData()
        End If

        Dim SubmissionData As DataTable = Emagine.GetDataTable("SELECT TOP 25 * FROM FormSubmissions WHERE FormID = " & FormID & " AND (SubmissionDate BETWEEN '" & txtStartDate.Text & "' AND '" & txtEndDate.Text & "') ORDER BY SubmissionDate DESC")
        If SubmissionData.Rows.Count > 0 Then
            For i As Integer = 0 To SubmissionData.Rows.Count - 1
                Dim Row As New TableRow
                Row.CssClass = "table-row"

                'Dim IdCell As New TableCell
                'IdCell.HorizontalAlign = HorizontalAlign.Center
                'Dim cbxSubmissionID As New CheckBox
                'cbxSubmissionID.ID = SubmissionData.Rows(i).Item("SubmissionID")
                'IdCell.Controls.Add(cbxSubmissionID)
                ''IdCell.Text = SubmissionData.Rows(i).Item("SubmissionID")
                'Row.Cells.Add(IdCell)

                Dim DateCell As New TableCell
                DateCell.Text = SubmissionData.Rows(i).Item("SubmissionDate")
                Row.Cells.Add(DateCell)

                Dim UserCell As New TableCell
                UserCell.Text = SubmissionData.Rows(i).Item("UserID")
                Row.Cells.Add(UserCell)

                For j As Integer = 0 To (DataTable.Rows.Count - 1)
                    Dim Cell As New TableCell
                    Dim FieldValueData As DataTable = Emagine.GetDataTable("SELECT FormFieldValue FROM FormSubmissionValues WHERE SubmissionID = " & SubmissionData.Rows(i).Item("SubmissionID") & " AND FormFieldName = '" & DataTable.Rows(j).Item("FormFieldName") & "'")
                    If FieldValueData.Rows.Count > 0 Then
                        For k As Integer = 0 To (FieldValueData.Rows.Count - 1)
                            Cell.Text = Cell.Text & FieldValueData.Rows(k).Item(0) & "<br>"
                        Next
                    Else
                        Cell.Text = "<br>"
                    End If


                    Row.Cells.Add(Cell)
                Next

                tblPreviewData.Rows.Add(Row)
            Next
        End If

    End Sub

    Protected Sub btnCancel2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel2.Click
        Me.DisplayFormGrid()
    End Sub

    Protected Sub btnQuery_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnQuery.Click
        Me.DisplayQueryForm(hdnFormID.Value)
    End Sub

    Protected Sub btnDownload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDownload.Click
        Dim FieldID As New StringBuilder

        For Each Item As ListItem In cblFields.Items
            If Item.Selected Then FieldID.Append(Item.Value & ",")
        Next
        FieldID.Append("0")

        Me.DownloadData(hdnFormID.Value, FieldID.ToString, txtStartDate.Text, txtEndDate.Text)
        'Response.Redirect("Download.aspx?FormID=" & hdnFormID.Value & "&FieldID=" & FieldID.ToString & "&StartDate=" & txtStartDate.Text & "&EndDate=" & txtEndDate.Text)
    End Sub

    Protected Sub btnDownload2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDownload2.Click
        Dim FieldID As New StringBuilder

        For Each Item As ListItem In cblFields.Items
            If Item.Selected Then FieldID.Append(Item.Value & ",")
        Next
        FieldID.Append("0")

        Me.DownloadData(hdnFormID.Value, FieldID.ToString, txtStartDate.Text, txtEndDate.Text)
        'Response.Redirect("Download.aspx?FormID=" & hdnFormID.Value & "&FieldID=" & FieldID.ToString & "&StartDate=" & txtStartDate.Text & "&EndDate=" & txtEndDate.Text)
    End Sub

    Protected Sub cblFields_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cblFields.DataBound
        For Each Item As ListItem In cblFields.Items
            Item.Selected = True
        Next
    End Sub

    Sub DownloadData(ByVal intFormID As Integer, ByVal strFieldID As String, ByVal strStartDate As String, ByVal strEndDate As String)
        Response.Clear()

        Dim FormName As String = Emagine.GetDbValue("SELECT FormName FROM Forms WHERE FormID = " & intFormID)
        Dim FileName As String = FormName & " Results " & strStartDate.Replace("/", "-") & " to " & strEndDate.Replace("/", "-") & ".xls"

        Dim DataTable As DataTable = Emagine.GetDataTable("SELECT DISTINCT FormFieldID, FormFieldName, SortOrder FROM qryFormSubmissionValues WHERE FormID = " & intFormID & " AND (SubmissionDate BETWEEN '" & strStartDate & "' AND '" & strEndDate & "') AND FormFieldID IN (" & strFieldID & ") ORDER BY SortOrder, FormFieldName")
        If DataTable.Rows.Count > 0 Then
            Response.AddHeader("Content-Disposition", "attachment; filename=" & FileName)
            Response.AddHeader("Content-type", "application/vnd.ms-excel")
            Response.Write("<table border='1'>")
            Response.Write("<tr>")
            Response.Write("<td>Date</td>")
            Response.Write("<td>User ID</td>")

            For i As Integer = 0 To DataTable.Rows.Count - 1
                Response.Write("<td>" & DataTable.Rows(i).Item("FormFieldName") & "</td>")
            Next
            Response.Write("</tr>")

            Dim SubmissionData As DataTable = Emagine.GetDataTable("SELECT * FROM FormSubmissions WHERE FormID = " & intFormID & " AND (SubmissionDate BETWEEN '" & strStartDate & "' AND '" & strEndDate & "') ORDER BY SubmissionDate DESC")
            If SubmissionData.Rows.Count > 0 Then
                For i As Integer = 0 To SubmissionData.Rows.Count - 1
                    Response.Write("<tr>")
                    Response.Write("<td>" & SubmissionData.Rows(i).Item("SubmissionDate") & "</td>")
                    Response.Write("<td>" & SubmissionData.Rows(i).Item("UserID") & "</td>")

                    For j As Integer = 0 To (DataTable.Rows.Count - 1)
                        Response.Write("<td>")
                        Dim FieldValueData As DataTable = Emagine.GetDataTable("SELECT FormFieldValue FROM FormSubmissionValues WHERE SubmissionID = " & SubmissionData.Rows(i).Item("SubmissionID") & " AND FormFieldName = '" & DataTable.Rows(j).Item("FormFieldName") & "'")
                        If FieldValueData.Rows.Count > 0 Then

                            For k As Integer = 0 To (FieldValueData.Rows.Count - 1)
                                Response.Write(FieldValueData.Rows(k).Item(0))
                                If k < FieldValueData.Rows.Count - 1 Then Response.Write(", ")
                            Next
                        Else
                            Response.Write("<br>")
                        End If

                        Response.Write("</td>")
                    Next

                    Response.Write("</tr>")
                Next
            End If
            Response.Write("</table>")
            Response.End()
        End If

    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Me.GetSubmissionID(Page)
    End Sub

    Function GetSubmissionID(ByVal objParentControl As Control) As String
        Dim Result As String = ""

        For Each Control As Control In objParentControl.Controls
            Response.Write(Control.GetType.ToString & "<br>")

            If Control.HasControls Then
                Result = Result & GetSubmissionID(Control)
            End If
        Next

        Return Result
    End Function
End Class
