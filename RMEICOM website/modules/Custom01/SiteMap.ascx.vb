
Partial Class modules_Custom01_SiteMap
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblSiteMap.Text = Me.GetSiteMap(0, 3, "", 0)
    End Sub

    Function GetSiteMap(ByVal intPageId As Integer, ByVal intPadding As Integer, ByRef sHTML As String, ByRef intPass As Integer)

        Dim TdClass As String = ""
        Dim LinkClass As String = ""
        Dim SQL As String = ""

        Select Case intPass
            Case 0
                TdClass = "table-header-app"
                LinkClass = "main-link"
            Case 1
                TdClass = "main-bold"
                LinkClass = "main-link"
            Case Else
                TdClass = "main"
                LinkClass = "main-link"
        End Select

        SQL += "SELECT PageID, ParentPageID, MenuName, PageKey, PageTypeID, PageAction FROM Pages "
        SQL += "WHERE DefaultPage = 0 AND IsHidden = 0 AND ParentPageID = " & intPageId & " AND StatusID = 20 AND LanguageID =" & Session("LanguageID") & " ORDER BY SortOrder"

        Dim DataTable As DataTable = Emagine.GetDataTable(SQL)

        For i As Integer = 0 To DataTable.Rows.Count - 1
            sHTML += "<tr>"
            sHTML += "<td " & "style=""padding-left:" & intPadding & "px;"" class='" & TdClass & "'>"

            If intPass > 0 Then sHTML += "-&nbsp;" '& Replace(Space(intPass), " ", "&nbsp;")

            Select Case DataTable.Rows(i).Item("PageTypeID")
                Case 0
                    sHTML += DataTable.Rows(i).Item("MenuName")
                Case 2 'External Link
                    sHTML += "<a href=' " & DataTable.Rows(i).Item("PageAction") & "' target='_blank'>" & DataTable.Rows(i).Item("MenuName") & "</a>"
                Case Else
                    sHTML += "<a href=' " & DataTable.Rows(i).Item("PageKey") & ".htm'>" & DataTable.Rows(i).Item("MenuName") & "</a>"
            End Select

            sHTML += "</td></tr>"

            GetSiteMap(DataTable.Rows(i).Item("PageId"), intPadding + 25, sHTML, intPass + 1)

        Next

        Return sHTML

    End Function

End Class

