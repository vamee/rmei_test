
Partial Class Ezedit_Modules_Content01_Default
    Inherits System.Web.UI.Page

    Dim _ModuleKey As String = "Content01"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.DisplayContentList()
        End If
    End Sub

    Sub DisplayContentList()
        pnlContent.Visible = True
        pnlEditContent.Visible = False

        gdvContent.DataSource = Emagine.GetDataTable("SELECT * FROM Content WHERE ModuleKey = '" & _ModuleKey & "'")
        gdvContent.DataBind()
    End Sub


    Sub AddContent()
        pnlEditContent.Visible = True
        pnlContent.Visible = False

        txtContentName.Text = ""
        txtContent.EditorContent = ""
        hdnContentID.Value = 0

        txtContentName.Focus()
    End Sub

    Sub EditContent(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim btnEdit As ImageButton = sender
        Dim ContentID As Integer = CInt(btnEdit.CommandArgument)

        pnlEditContent.Visible = True
        pnlContent.Visible = False

        Dim DataTable As DataTable = Emagine.GetDataTable("SELECT * FROM Content WHERE ContentID = " & ContentID)

        If DataTable.Rows.Count > 0 Then
            txtContentName.Text = DataTable.Rows(0).Item("VersionNotes")
            txtContent.EditorContent = DataTable.Rows(0).Item("Content")
        End If

        hdnContentID.Value = ContentID
    End Sub

    Sub DeleteContent(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim btnDelete As ImageButton = sender
        Dim ContentID As Integer = CInt(btnDelete.CommandArgument)

        If Emagine.ExecuteSQL("DELETE FROM Content WHERE ContentID = " & ContentID) Then
            lblAlert.Text = "The content has been removed successfully."
            Me.DisplayContentList()
        Else
            lblAlert.Text = "An error occurred while deleteing the content."
        End If
    End Sub

    Protected Sub gdvContent_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvContent.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ContentID As Integer = CInt(DataBinder.Eval(e.Row.DataItem, "ContentID"))
            Dim strPageLinks As String = ""
            Dim ltrPageModules As Literal = e.Row.FindControl("ltrPageModules")
            Dim btnEdit As ImageButton = e.Row.FindControl("btnEdit")
            Dim btnDelete As ImageButton = e.Row.FindControl("btnDelete")

            Dim dtrModulePages As Data.SqlClient.SqlDataReader = Modules.GetModulePages(_ModuleKey, ContentID.ToString)

            While dtrModulePages.Read
                strPageLinks += "<li class='main'><a href='EditModuleProperties.aspx?PageModuleID=" & dtrModulePages("PageModuleID") & "&ContentID=" & ContentID & "' class='main'>"
                strPageLinks += dtrModulePages("PageName").ToString
                strPageLinks += "</a></li><br>"
            End While
            dtrModulePages.Close()

            strPageLinks += "<li class='main'><a href='EditModuleProperties.aspx?PageModuleID=0&ContentID=" & ContentID & "' class='main'>"
            strPageLinks += "Add to Page"
            strPageLinks += "</a></li><br>"

            ltrPageModules.Text = strPageLinks

            If btnEdit IsNot Nothing Then btnEdit.CommandArgument = ContentID
            If btnDelete IsNot Nothing Then btnDelete.CommandArgument = ContentID

        End If
    End Sub

    
    Protected Sub btnCancelContent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelContent.Click
        Me.DisplayContentList()
    End Sub

    Protected Sub btnAddContent1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddContent1.Click
        Me.AddContent()
    End Sub


    Protected Sub btnAddcontent2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddcontent2.Click
        Me.AddContent()
    End Sub

    Protected Sub btnSaveContent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveContent.Click
        If PeterBlum.VAM.Globals.Page.IsValid Then
            Dim ContentID As Integer = Emagine.GetNumber(hdnContentID.Value)
            Dim Content As String = txtContent.EditorContent

            Dim Result As Boolean = False
            Dim Conn As New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
            Dim SqlBuilder As New StringBuilder
            Dim Command As New System.Data.SqlClient.SqlCommand
            Command.Connection = Conn

            If ContentID > 0 Then
                SqlBuilder.Append("UPDATE Content SET ")
                SqlBuilder.Append("VersionNotes=@ContentName,")
                SqlBuilder.Append("Content=@Content ")
                SqlBuilder.Append("WHERE ContentID=@ContentID")

                Command.Parameters.AddWithValue("@ContentID", ContentID)
                Command.Parameters.AddWithValue("@ContentName", txtContentName.Text)
                Command.Parameters.AddWithValue("@Content", Content)


            Else
                SqlBuilder.Append("INSERT INTO Content ")
                SqlBuilder.Append("(StatusID,ModuleKey,ForeignKey,Version,VersionNotes,Content)")
                SqlBuilder.Append(" VALUES ")
                SqlBuilder.Append("(@StatusID,@ModuleKey,@ForeignKey,@Version,@ContentName,@Content)")


                Command.Parameters.AddWithValue("@StatusID", 20)
                Command.Parameters.AddWithValue("@ModuleKey", "Content01")
                Command.Parameters.AddWithValue("@ForeignKey", "0")
                Command.Parameters.AddWithValue("@Version", "0")
                Command.Parameters.AddWithValue("@ContentName", txtContentName.Text)
                Command.Parameters.AddWithValue("@Content", Content)
            End If

            Command.CommandText = SqlBuilder.ToString

            Try
                Conn.Open()
                Result = Command.ExecuteNonQuery()

            Catch ex As Exception
                ex.HelpLink = "Error while inserting or updating misc content."
                Emagine.LogError(ex)
            End Try

            If Result = True Then
                If ContentID > 0 Then
                    lblAlert.Text = "The content has been updated successfully."
                Else
                    lblAlert.Text = "The content has been updated successfully."
                End If

                Me.DisplayContentList()

            Else
                lblAlert.Text = "An error occurred while saving this record."
            End If

        End If
    End Sub
End Class
