
Partial Class Ezedit_Modules_LB01_PageAssignments
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ItemID As Integer = Emagine.GetNumber(Request("ItemID"))
        If ItemID = 0 Then Response.Redirect("Default.aspx")

        If Not IsPostBack Then
            Dim ItemName As String = Emagine.GetDbValue("SELECT ItemName FROM LibraryItems WHERE ItemID = " & ItemID)

            lblBreadcrumbs.Text = "<a href='Default.aspx' class='breadcrumbs'>Library Items</a> > " & ItemName & " > Assign to Pages"
            lblSiteContent.Text = LB01.LibraryItems.DisplaySiteContent(0, 0, "", ItemID, Emagine.GetNumber(Session("EzEditLanguageID")))

        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Me.UpdateAssignments()

        Session("Alert") = "The library item page assignments have been updated successfully."
        Response.Redirect("Default.aspx")
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("Default.aspx")
    End Sub

    Sub UpdateAssignments()
        Dim PageIDs As String = Request("PageID")
        Dim ItemID As Integer = Emagine.GetNumber(Request("ItemID"))

        If Len(PageIDs) > 0 Then
            Emagine.ExecuteSQL("DELETE FROM PageLibraryItems WHERE ItemID = " & ItemID)
            Dim aryPages As Array = Split(PageIDs, ",")
            Dim i As Integer
            For i = 0 To UBound(aryPages)
                If Len(Trim(aryPages(i))) > 0 Then
                    Dim PageID As Integer = Emagine.GetNumber(aryPages(i))
                    Dim SortOrder As Integer = Emagine.GetNumber(Emagine.GetDbValue("SELECT MAX(SortOrder) AS MaxSortOrder FROM PageLibraryItems WHERE PageID = " & PageID)) + 1
                    Emagine.ExecuteSQL("INSERT INTO PageLibraryItems(PageID, ItemID, SortOrder) VALUES(" & PageID & ", " & ItemID & ", " & SortOrder & ")")
                End If
            Next
            LB01.PageLibraryItems.ResetSortOrderAll()
        End If
    End Sub
End Class
