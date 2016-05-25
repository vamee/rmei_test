
Partial Class Ezedit_Admin_Permissions
    Inherits System.Web.UI.Page

    Dim _UserID As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _UserID = Emagine.GetNumber(Request("UserID"))
        If Not IsPostBack Then

            lblSiteContent.Text = Pages01.DisplaySiteContent(0, 0, "", _UserID, "", Session("EzEditLanguageID"))
            lblApplications.Text = Me.DisplayApplications(_UserID)
        End If
    End Sub

    Private Function DisplayApplications(ByVal intUserID As String) As String
        Dim strBuilder As New StringBuilder
        Dim dtr As Data.SqlClient.SqlDataReader = Modules.GetApplications(False)
        Dim i As Integer = 0
        Do While dtr.Read()
            strBuilder.Append("<tr bgcolor=""#" & IIf(i Mod 2, "F3F2F7", "FFFFFF") & """>")
            strBuilder.Append("<td class=""main"">")
            strBuilder.Append("<input type='checkbox' name='moduleKey' value='" & dtr("ModuleKey").ToString & "' " & CheckSelectedModuleId(intUserID, dtr("ModuleKey").ToString) & ">&nbsp;&nbsp;" & dtr("Name").ToString)
            strBuilder.Append("</td>")
            strBuilder.Append("</tr>")
            i += +1
        Loop
        dtr.Close()
        Return strBuilder.ToString
    End Function

    Function CheckSelectedModuleId(ByVal intUserID As Integer, ByVal strKey As String) As String
        Dim Result As String = ""

        If (Emagine.GetDbValue("SELECT Count(*) FROM ModulePermissions WHERE EzUserID = " & intUserID & " AND ModuleKey = '" & strKey & "'") > 0) Then
            Result = "CHECKED"
        End If

        Return Result
    End Function

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Me.UpdateSiteContentPermissions()
        Me.UpdateSelectedModuleIds()
        Session("Alert") = "Permissions have been updated successfully."
        Response.Redirect("Default.aspx")
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("Default.aspx")
    End Sub

    Sub UpdateSiteContentPermissions()
        Dim cboPageID As String = Request("PageID")
        If Len(cboPageID) > 0 Then
            Emagine.ExecuteSQL("DELETE FROM PagePermissions WHERE EzUserID = " & _UserID)
            Dim aryPages As Array = Split(cboPageID, ",")

            For i As Integer = 0 To UBound(aryPages)
                If Len(Trim(aryPages(i))) > 0 Then
                    Emagine.ExecuteSQL("INSERT INTO PagePermissions(EzUserID, PageID) VALUES(" & _UserID & "," & aryPages(i) & ")")
                End If
            Next
        End If
    End Sub

    Sub UpdateSelectedModuleIds()
        Dim SQL As String = "DELETE FROM ModulePermissions WHERE EzUserID = " & _UserID
        Emagine.ExecuteSQL(SQL)
        If Request("ModuleKey") IsNot Nothing Then
            Dim ModuleKey As String = Request("ModuleKey").ToString
            If ModuleKey.Length > 0 Then
                Dim aryModules As Array = ModuleKey.Split(",")
                For i As Integer = 0 To aryModules.GetUpperBound(0)
                    If Len(Trim(aryModules(i))) > 0 Then
                        Emagine.ExecuteSQL("INSERT INTO ModulePermissions(EzUserID, ModuleKey) VALUES(" & _UserID & ",'" & aryModules(i) & "')")
                    End If
                Next
            End If
        End If
    End Sub

End Class
