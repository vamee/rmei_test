Imports System.Data
Imports System.Data.SqlClient

Partial Class RadControls_Editor_Custom_InternalLink
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim SQL As String = "SELECT PageID, PageName, PageKey, ParentPageID FROM Pages WHERE LanguageID = " & Session("EzEditLanguageID") & " AND StatusID = " & Session("EzEditStatusID") & " AND (ParentPageID IN (SELECT PageID FROM Pages WHERE StatusID = " & Session("EzEditStatusID") & " AND LanguageID = " & Session("EzEditLanguageID") & ")) OR (ParentPageID = 0 AND StatusID = " & Session("EzEditStatusID") & " AND LanguageID = " & Session("EzEditLanguageID") & ") ORDER BY SortOrder"

            Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
            Dim SqlAdapter As New SqlDataAdapter(SQL, Conn)
            Dim Rs As New DataSet()
            SqlAdapter.Fill(Rs)

            For Each Row As DataRow In Rs.Tables(0).Rows
                If Emagine.GetNumber(Row.Item("ParentPageID").ToString) = 0 Then
                    Row.Item("ParentPageID") = DBNull.Value
                End If
            Next

            tvwSiteMap.DataSource = Rs.Tables(0)
            tvwSiteMap.DataTextField = "PageName"
            tvwSiteMap.DataFieldID = "PageID"
            tvwSiteMap.DataFieldParentID = "ParentPageID"
            tvwSiteMap.DataValueField = "PageID"
            Try
                tvwSiteMap.DataBind()
            Catch ex As Exception
                Emagine.LogError(ex)
            End Try

            BindFormPages(CInt(Session("EzEditStatusID")), CInt(Session("EzEditLanguageID")))
        End If

    End Sub

    Protected Sub tvwSiteMap_NodeClick(ByVal o As Object, ByVal e As Telerik.WebControls.RadTreeNodeEventArgs) Handles tvwSiteMap.NodeClick
        Dim Page As New Pages01
        Page = Page.GetPageInfo(Emagine.GetNumber(tvwSiteMap.SelectedNode.Value))

        If Page.PageTypeId = 1 Then

            Dim PageInfo As New StringBuilder

            PageInfo.Append("<table cellpadding='3' cellspacing='0' border='0'>")
            'PageInfo.Append("<tr>")
            'PageInfo.Append("<td><span class='form_label'>Page Name: </span><span class='main'>" & Page.PageName & "</span></td>")
            'PageInfo.Append("</tr>")
            PageInfo.Append("<tr>")
            PageInfo.Append("<td><span class='form_label'>URL: </span><span class='main'>/" & Page.PageKey & ".htm</span></td>")
            PageInfo.Append("</tr>")
            'PageInfo.Append("<tr>")
            'PageInfo.Append("<td><span class='form_label'>Date Created: </span><span class='main'>" & Page.DateCreated & "</span></td>")
            'PageInfo.Append("</tr>")
            'PageInfo.Append("<tr>")
            'PageInfo.Append("<td><span class='form_label'>Last Updated: </span><span class='main'>" & Page.LastUpdated & "</span></td>")
            'PageInfo.Append("</tr>")
            'PageInfo.Append("<tr>")
            'PageInfo.Append("<td><span class='form_label'>Has Children: </span><span class='main'>" & Page.HasChildren & "</span></td>")
            'PageInfo.Append("</tr>")
            'PageInfo.Append("<tr>")
            'PageInfo.Append("<td><span class='form_label'>Is Permanent: </span><span class='main'>" & Page.IsPermanent & "</span></td>")
            'PageInfo.Append("</tr>")
            'PageInfo.Append("<tr>")
            'PageInfo.Append("<td><span class='form_label'>Is Hidden: </span><span class='main'>" & Page.IsHidden & "</span></td>")
            'PageInfo.Append("</tr>")
            'PageInfo.Append("<tr>")
            'PageInfo.Append("<td> </td>")
            'PageInfo.Append("</tr>")
            PageInfo.Append("<tr>")
            PageInfo.Append("<td><a href='/" & Page.PageKey & ".htm' target='_blank' class='main-bold'>Preview</a></td>")
            PageInfo.Append("</tr>")
            PageInfo.Append("</table>")

            lblPageInfo.Text = PageInfo.ToString()
            BindPageModules()
            btnInsert.Enabled = True
        Else
            lblPageInfo.Text = ""
            btnInsert.Enabled = False
        End If
    End Sub

    Sub BindFormPages(ByVal intStatusID, ByVal intLanguageID)
        Dim dtrForms As Data.SqlClient.SqlDataReader
        dtrForms = Pages01.GetFormPages(1, intStatusID, intLanguageID)
        ddlFormPages.Items.Add(New ListItem("None Required", "0", True))

        If dtrForms.HasRows Then
            ddlFormPages.AppendDataBoundItems = True
            ddlFormPages.DataSource = dtrForms
            ddlFormPages.DataTextField = "PageName"
            ddlFormPages.DataValueField = "PageID"
            ddlFormPages.DataBind()
        End If
    End Sub

    Sub BindPageModules()
        tvwPageModules.Nodes.Clear()

        Dim PageID As Integer = Emagine.GetNumber(tvwSiteMap.SelectedNode.Value)
        Dim SQL As String = "SELECT ModuleKey, ForeignValue As CategoryID, CategoryName FROM qryDisplayPageModules WHERE PageID = " & PageID
        Dim Rs As SqlDataReader = Emagine.GetDataReader(SQL)
        If Rs.HasRows Then
            tvwPageModules.Visible = True
            Do While Rs.Read
                Select Case Rs("ModuleKey").ToString
                    Case "PR01"
                        SQL = "SELECT * FROM qryArticles WHERE CategoryID = " & Rs("CategoryID") & " ORDER BY DisplayDate DESC"
                    Case "DL01"
                        SQL = "SELECT * FROM qryDownloads WHERE CategoryID = " & Rs("CategoryID") & " ORDER BY SortOrder"
                    Case "Careers01"
                        SQL = "SELECT * FROM qryCareers WHERE CategoryID = " & Rs("CategoryID") & " ORDER BY SortOrder"
                    Case "Events01"
                        SQL = "SELECT * FROM qryEvents WHERE CategoryID = " & Rs("CategoryID") & " ORDER BY SortOrder"
                    Case "Knowledgebase"
                        SQL = "SELECT * FROM qryKnowledgeBaseItems WHERE ModuleCategoryID = " & Rs("CategoryID") & " ORDER BY SortOrder"
                    Case Else
                        SQL = ""
                End Select

                If SQL.Length > 0 Then

                    Dim Rs2 As SqlDataReader = Emagine.GetDataReader(SQL)
                    If Rs2.HasRows Then
                        Dim RootNode As New Telerik.WebControls.RadTreeNode(Rs("CategoryName").ToString, "")
                        tvwPageModules.Nodes.Add(RootNode)
                        Do While Rs2.Read
                            Dim SubNode As New Telerik.WebControls.RadTreeNode(Rs2("ResourceName").ToString, "/" & Rs2("ResourceID").ToString & "/" & Rs2("DeliveryPageKey").ToString & ".htm")
                            RootNode.Nodes.Add(SubNode)
                        Loop
                    End If
                    Rs2.Close()
                End If
            Loop

        Else
            tvwPageModules.Visible = False

        End If
    End Sub

    Protected Sub btnInsert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInsert.Click
        Dim LinkName As String = txtLinkName.Text
        Dim PageID As Integer = Emagine.GetNumber(tvwSiteMap.SelectedNode.Value)
        Dim FormPageID As Integer = CInt(ddlFormPages.SelectedValue)
        Dim Link As String

        If Len(LinkName) > 0 And PageID > 0 Then
            Dim PageKey As String = Emagine.GetDbValue("SELECT PageKey FROM Pages WHERE PageID = " & PageID)
            Dim LinkURL As String = ""
            If tvwPageModules.SelectedNodes.Count > 0 Then
                LinkURL = tvwPageModules.SelectedNode.Value.ToString
            Else
                LinkURL = "/" & PageKey & ".htm"
            End If
            LinkName = Replace(LinkName, Chr(34), "'")
            Link = CreateLink(LinkName, LinkURL, FormPageID)

            Response.Write("<script type=""text/javascript"" src=""/RadControls/Editor/Scripts/7_0_2/RadWindow.js""></script>" & vbCrLf)
            Response.Write("<script language='javascript'>" & vbCrLf)
            Response.Write("InitializeRadWindow();" & vbCrLf)
            Response.Write("var url = """ & Link & """;" & vbCrLf)
            Response.Write("var returnValue = {html:url};" & vbCrLf)
            Response.Write("CloseDlg(returnValue);" & vbCrLf)
            Response.Write("</script>" & vbCrLf)
            Response.End()

        ElseIf Len(LinkName) = 0 Then
            DisplayAlert("Please enter a link name in the space provided.")
        ElseIf PageID = 0 Then
            DisplayAlert("Please choose a file from the list.")
        End If

    End Sub

    Function CreateLink(ByVal LinkName As String, ByVal LinkURL As String, ByVal FormPageID As Integer) As String
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = ""
        Dim ResourceID As String = Emagine.GetUniqueID()
        Dim LinkID As Integer
        Dim PageModuleID As Integer
        Dim FormPagePropertyID As Integer
        Dim DeliveryPagePropertyID As Integer
        Dim FormPageModuleID As Integer
        Dim DeliveryPageID As Integer
        Dim ReturnURL As String = ""

        If FormPageID > 0 Then
            SQL = "sp_Links01_AddContentLink"
            Dim objCommand As New SqlCommand(SQL, Conn)
            objCommand.CommandType = CommandType.StoredProcedure
            objCommand.Parameters.AddWithValue("@ResourceID", ResourceID)
            objCommand.Parameters.AddWithValue("@LinkType", "LINK")
            objCommand.Parameters.AddWithValue("@LinkURL", LinkURL)

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

            SQL = "INSERT INTO PageModules (ModuleKey,PageID,ForeignKey,ForeignValue) VALUES ("
            SQL = SQL & "'Links01', "
            SQL = SQL & "0, "
            SQL = SQL & "'LinkID', "
            SQL = SQL & LinkID & ")"
            Emagine.ExecuteSQL(SQL)


            SQL = "SELECT MAX(PageModuleID) AS MaxPageModuleID FROM PageModules"
            PageModuleID = Emagine.GetNumber(Emagine.GetDbValue(SQL))

            SQL = "SELECT PropertyID FROM ModuleProperties WHERE ModuleKey = 'Links01' AND PropertyName = 'FormPage'"
            FormPagePropertyID = Emagine.GetNumber(Emagine.GetDbValue(SQL))

            SQL = "SELECT PropertyID FROM ModuleProperties WHERE ModuleKey = 'Links01' AND PropertyName = 'DeliveryPage'"
            DeliveryPagePropertyID = Emagine.GetNumber(Emagine.GetDbValue(SQL))

            SQL = "INSERT INTO PageModuleProperties (PageModuleID,PropertyID,PropertyValue) VALUES ("
            SQL = SQL & PageModuleID & ", "
            SQL = SQL & FormPagePropertyID & ", "
            SQL = SQL & "'" & FormPageID & "')"
            Emagine.ExecuteSQL(SQL)

            SQL = "SELECT PageModuleID FROM qryFormPages WHERE PageID = " & FormPageID
            FormPageModuleID = Emagine.GetNumber(Emagine.GetDbValue(SQL))

            SQL = "SELECT PropertyValue FROM qryPageModuleProperties WHERE PageModuleID = " & FormPageModuleID & " AND PropertyName = 'DeliveryPage'"
            DeliveryPageID = Emagine.GetNumber(Emagine.GetDbValue(SQL))

            SQL = "INSERT INTO PageModuleProperties (PageModuleID,PropertyID,PropertyValue) VALUES ("
            SQL = SQL & PageModuleID & ", "
            SQL = SQL & DeliveryPagePropertyID & ", "
            SQL = SQL & "'" & DeliveryPageID & "')"
            Emagine.ExecuteSQL(SQL)

            ReturnURL = "/" & ResourceID & "/Link.htm"
        Else
            ReturnURL = LinkURL
        End If

        ReturnURL = "<a href='" & ReturnURL & "' target='_self'>" & LinkName & "</a>"

        Return ReturnURL
    End Function

    Sub DisplayAlert(ByVal strAlert As String)
        Dim strScript As New StringBuilder

        strScript.Append("<script language='javascript'>")
        strScript.Append("alert(""" & strAlert & """);")
        strScript.Append("</script>")

        litAlert.Text = strScript.ToString()
    End Sub

    Sub WriteGetRadWindow()
        Response.Write("<script>")
        Response.Write("function GetRadWindow() {")
        Response.Write("var oWindow = null;")
        Response.Write("if (window.radWindow) {")
        Response.Write("oWindow = window.radWindow;")
        Response.Write("}else if (window.frameElement.radWindow) {")
        Response.Write("oWindow = window.frameElement.radWindow;")
        Response.Write("}")
        Response.Write("return oWindow;")
        Response.Write("}")
        Response.Write("</script>")
    End Sub

    Protected Sub tvwPageModules_NodeClick(ByVal o As Object, ByVal e As Telerik.WebControls.RadTreeNodeEventArgs) Handles tvwPageModules.NodeClick
        If tvwPageModules.SelectedNode.Value.ToString.Length > 0 Then

            Dim PageInfo As New StringBuilder

            PageInfo.Append("<table cellpadding='3' cellspacing='0' border='0'>")
            PageInfo.Append("<tr>")
            PageInfo.Append("<td><span class='form_label'>URL: </span><span class='main'>" & tvwPageModules.SelectedNode.Value & "</span></td>")
            PageInfo.Append("</tr>")

            PageInfo.Append("<tr>")
            PageInfo.Append("<td><a href='" & tvwPageModules.SelectedNode.Value & "' target='_blank' class='main-bold'>Preview</a></td>")
            PageInfo.Append("</tr>")
            PageInfo.Append("</table>")

            lblPageInfo.Text = PageInfo.ToString()
            btnInsert.Enabled = True
        Else
            lblPageInfo.Text = ""
            btnInsert.Enabled = False
        End If
    End Sub
End Class
