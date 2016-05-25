Imports System.Data
Imports System.Data.SqlClient

Partial Class modules_Custom01_SiteMapTree
    Inherits System.Web.UI.UserControl


    Public Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim SQL As String = "SELECT PageID, PageName, PageKey, ParentPageID FROM Pages WHERE ParentPageID IN (SELECT PageID FROM Pages WHERE StatusID = " & Session("EzEditStatusID") & ") OR ParentPageID = 0"

        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SqlAdapter As New SqlDataAdapter(SQL, Conn)
        Dim Rs As New DataSet()
        SqlAdapter.Fill(Rs)

        For Each Row As DataRow In Rs.Tables(0).Rows
            If Emagine.GetNumber(Row.Item("ParentPageID").ToString) = 0 Then
                Row.Item("ParentPageID") = DBNull.Value
            End If
        Next

        'tvwSiteMap.DataSource = Rs.Tables(0)
        'tvwSiteMap.DataTextField = "PageName"
        'tvwSiteMap.DataFieldID = "PageID"
        'tvwSiteMap.DataFieldParentID = "ParentPageID"
        'tvwSiteMap.DataBind()
    End Sub


End Class
