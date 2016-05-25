
Partial Class Ezedit_Modules_Reports01_FormSubmissions_Download
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim FormID As Integer = CInt(Request("FormID"))
        Dim FieldID As String = Request("FieldID")
        Dim StartDate As String = Request("StartDate")
        Dim EndDate As String = Request("EndDate")
        Dim FormName As String = Emagine.GetDbValue("SELECT FormName FROM Forms WHERE FormID = " & FormID)
        Dim FileName As String = FormName & " Results " & StartDate.Replace("/", "-") & " to " & EndDate.Replace("/", "-") & ".xls"

        Dim DataTable As DataTable = Emagine.GetDataTable("SELECT DISTINCT FormFieldName, SortOrder FROM qryFormSubmissionValues WHERE FormID = " & FormID & " AND (SubmissionDate BETWEEN '" & StartDate & "' AND '" & EndDate & "') AND FormFieldID IN (" & FieldID.ToString & ") ORDER BY SortOrder, FormFieldName")
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


            Dim SubmissionData As DataTable = Emagine.GetDataTable("SELECT * FROM FormSubmissions WHERE FormID = " & FormID & " AND (SubmissionDate BETWEEN '" & StartDate & "' AND '" & EndDate & "') ORDER BY SubmissionDate DESC")
            If SubmissionData.Rows.Count > 0 Then
                For i As Integer = 0 To SubmissionData.Rows.Count - 1
                    Response.Write("<tr>")

                    Dim SubmissionValueData As DataTable = Emagine.GetDataTable("SELECT * FROM qryFormSubmissionValues WHERE SubmissionID = " & SubmissionData.Rows(i).Item("SubmissionID") & " AND (SubmissionDate BETWEEN '" & StartDate & "' AND '" & EndDate & "') AND FormFieldID IN (" & FieldID.ToString & ") ORDER BY SortOrder, FormFieldName")
                    If SubmissionValueData.Rows.Count > 0 Then

                        Response.Write("<td>" & SubmissionData.Rows(i).Item("SubmissionDate") & "</td>")
                        Response.Write("<td>" & SubmissionData.Rows(i).Item("UserID") & "</td>")

                        For j As Integer = 0 To SubmissionValueData.Rows.Count - 1
                            If Emagine.GetDbValue("SELECT FormFieldValue FROM FormSubmissionValues WHERE SubmissionID = " & SubmissionData.Rows(i).Item("SubmissionID") & " AND FormFieldName = '" & SubmissionValueData.Rows(j).Item("FormFieldName") & "'").Length > 0 Then
                                Response.Write("<td>" & SubmissionValueData.Rows(j).Item("FormFieldValue") & "</td>")
                            Else
                                Response.Write("<td><br></td>")
                            End If
                        Next
                    End If
                    Response.Write("</tr>")
                Next
            End If
            Response.Write("</table>")
            Response.End()
        End If
    End Sub
End Class
