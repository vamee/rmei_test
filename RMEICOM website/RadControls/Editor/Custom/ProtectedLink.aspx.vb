Imports System.Data
Imports System.Data.SqlClient

Partial Class RadControls_Editor_Custom_ProtectedLink
    Inherits System.Web.UI.Page


    Protected Sub btnInsert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInsert.Click
        If PeterBlum.VAM.Globals.Page.IsValid Then
            Dim LinkUrl As String = txtLinkUrl.Text
            Dim LinkName As String = txtLinkName.Text
            Dim DeliveryPageID As Integer = ddlDeliveryPage.SelectedValue
            
            

            LinkName = Replace(LinkName, Chr(34), "'")
            Dim Link As String = CreateLink(LinkName, LinkUrl, DeliveryPageID)

            Response.Write("<script type=""text/javascript"" src=""/RadControls/Editor/Scripts/7_0_2/RadWindow.js""></script>" & vbCrLf)
            Response.Write("<script language='javascript'>" & vbCrLf)
            Response.Write("InitializeRadWindow();" & vbCrLf)
            Response.Write("var url = """ & Link & """;" & vbCrLf)
            Response.Write("var returnValue = {html:url};" & vbCrLf)
            Response.Write("CloseDlg(returnValue);" & vbCrLf)
            Response.Write("</script>" & vbCrLf)
            Response.End()
        End If
    End Sub

    Function CreateLink(ByVal strLinkName As String, ByVal strLinkURL As String, ByVal intDeliveryPageID As Integer) As String
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = ""
        Dim ResourceID As String = Emagine.GetUniqueID()
        Dim LinkID As Integer
        Dim DeliveryPageKey As String = Emagine.GetDbValue("SELECT PageKey FROM Pages WHERE PageID = " & intDeliveryPageID)

        SQL = "sp_Links01_AddContentLink"
        Dim objCommand As New SqlCommand(SQL, Conn)
        objCommand.CommandType = CommandType.StoredProcedure
        objCommand.Parameters.AddWithValue("@ResourceID", ResourceID)
        objCommand.Parameters.AddWithValue("@LinkType", "LINK")
        objCommand.Parameters.AddWithValue("@LinkURL", strLinkURL)

        Try
            Conn.Open()
            objCommand.ExecuteNonQuery()
            LinkID = Emagine.GetNumber(Emagine.GetDbValue("sp_Links01_GetMaxContentLinkID"))

        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            If Conn.State = ConnectionState.Open Then Conn.Dispose()
        End Try

        SQL = "INSERT INTO Resources (ResourceID,ResourcePageKey,ResourceType) VALUES ('" & ResourceID & "', '" & ResourceID & "', 'Links01')"
        Emagine.ExecuteSQL(SQL)

        SQL = "INSERT INTO PageModules (ModuleKey,PageID,CodeFileID,ForeignKey,ForeignValue) VALUES ("
        SQL = SQL & "'Links01', "
        SQL = SQL & intDeliveryPageID & ", "
        SQL = SQL & "14, "
        SQL = SQL & "'LinkID', "
        SQL = SQL & LinkID & ")"
        Emagine.ExecuteSQL(SQL)

        'SQL = "SELECT MAX(PageModuleID) AS MaxPageModuleID FROM PageModules"
        'Dim PageModuleID As Integer = Emagine.GetNumber(Emagine.GetDbValue(SQL))

        'SQL = "SELECT PropertyID FROM ModuleProperties WHERE ModuleKey = 'Links01' AND PropertyName = 'DeliveryPage'"
        'Dim DeliveryPagePropertyID As Integer = Emagine.GetNumber(Emagine.GetDbValue(SQL))

        'SQL = "INSERT INTO PageModuleProperties (PageModuleID,PropertyID,PropertyValue) VALUES ("
        'SQL = SQL & PageModuleID & ", "
        'SQL = SQL & DeliveryPagePropertyID & ", "
        'SQL = SQL & "'" & intDeliveryPageID & "')"
        'Emagine.ExecuteSQL(SQL)

        Dim ReturnURL As String = "<a href='/" & ResourceID & "/" & DeliveryPageKey & ".htm" & "' target='_self'>" & strLinkName & "</a>"
        'ReturnURL = "<a href='" & ReturnURL & "' target='_self'>" & LinkName & "</a>"

        Return ReturnURL
    End Function

    Protected Sub ddlDeliveryPage_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDeliveryPage.Load
        If Not Page.IsPostBack Then
            ddlDeliveryPage = Pages01.PopulatePageOptionsDDL(ddlDeliveryPage, 0, 0, 0, Session("EzEditStatusID"), Session("EzEditLanguageID"))
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtLinkUrl.Focus()
    End Sub
End Class
