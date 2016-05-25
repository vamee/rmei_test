Imports System.Data
Imports System.Data.SqlClient

Partial Class Ezedit_Modules_Pages01_ContentList
    Inherits System.Web.UI.Page

    Public _PageID As Integer = 0
    'Dim _ModuleKey As String = "Pages01"

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    If Request("PageID") IsNot Nothing Then _PageID = Emagine.GetNumber(Request("PageID"))
    'End Sub

    'Protected Sub btnPublish_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    'Dim Button As ImageButton = sender
    '    'Dim ContentID As Integer = Button.CommandArgument

    '    'If Pages01.PromoteContent(PageID, ContentID) Then
    '    '    lblAlert.Text = "The content has been published successfully."
    '    '    rptContent.DataBind()
    '    'Else
    '    '    lblAlert.Text = "An error occurred while attempting to publish this content."
    '    'End If
    'End Sub

    'Protected Sub btnEmail_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    Dim Button As ImageButton = sender
    '    Dim ContentID As Integer = Button.CommandArgument

    '    txtEmailSubject.Text = "Please review this content"
    '    txtEmailMessage.Text = "Please review the following content:" & vbCrLf & vbCrLf
    '    txtEmailMessage.Text += "http://" & Request.ServerVariables("SERVER_NAME") & "/" & Pages01.GetPreviewPageKey(ContentID) & ".htm?ContentID=" & ContentID

    '    pnlList.Visible = False
    '    pnlEmail.Visible = True
    '    txtEmailTo.Focus()
    'End Sub

    'Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    Dim Button As ImageButton = sender
    '    Dim ContentID As Integer = Button.CommandArgument

    '    Response.Redirect("EditContent.aspx?ContentID=" & ContentID & "&PageID=" & _PageID)
    'End Sub

    'Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    'Dim Button As ImageButton = sender
    '    'Dim ContentID As Integer = Button.CommandArgument

    '    'If Pages01.DeleteVersion(ContentID) Then
    '    '    lblAlert.Text = "The content has been deleted."
    '    '    rptContent.DataBind()
    '    'Else
    '    '    lblAlert.Text = "An error occurred while attempting to delete this content."
    '    'End If
    'End Sub

    'Protected Sub rptContent_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptContent.ItemDataBound
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        Dim StatusID As Integer = e.Item.DataItem("StatusID")

    '        Dim gdvContent As GridView = e.Item.FindControl("gdvContent")
    '        Dim dataContent As SqlDataSource = e.Item.FindControl("dataContent")

    '        dataContent.SelectCommand = "SELECT * FROM qryContent WHERE ModuleKey = '" & _ModuleKey & "' AND ForeignKey=" & _PageID & " AND StatusID = " & StatusID & " ORDER BY Version"
    '        dataContent.DataBind()
    '        AddHandler gdvContent.RowDataBound, AddressOf gdvContent_RowDataBound
    '        gdvContent.DataBind()
    '    End If
    'End Sub

    'Protected Sub gdvContent_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        Dim ContentID As Integer = e.Row.DataItem("ContentID")
    '        Dim StatusID As Integer = e.Row.DataItem("StatusID")

    '        Dim btnPublish As ImageButton = e.Row.FindControl("btnPublish")
    '        Dim hypPreview As HyperLink = e.Row.FindControl("hypPreview")
    '        Dim btnEmail As ImageButton = e.Row.FindControl("btnEmail")
    '        Dim btnEdit As ImageButton = e.Row.FindControl("btnEdit")
    '        Dim btnDelete As ImageButton = e.Row.FindControl("btnDelete")

    '        btnPublish.CommandArgument = ContentID
    '        If StatusID = 20 Then btnPublish.Visible = False
    '        hypPreview.NavigateUrl = "/modules/Pages01/GetPage.aspx?PageID=" & _PageID & "&ContentID=" & ContentID
    '        btnEmail.CommandArgument = ContentID
    '        If StatusID = 20 Then btnEmail.Visible = False
    '        btnEdit.CommandArgument = ContentID
    '        btnDelete.CommandArgument = ContentID
    '        If StatusID = 20 Or StatusID = -20 Then btnDelete.Visible = False
    '    End If
    'End Sub

    'Function GetBreadcrumbs(ByVal intPageId As Integer) As String
    '    Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
    '    Dim id As Integer
    '    Dim cmd As New SqlCommand
    '    Dim dr As SqlDataReader
    '    Dim temp As String = ""
    '    Dim aryTemp As Array
    '    Dim i As Integer
    '    Dim breadcrumb As String
    '    id = intPageId

    '    Dim strStyleName As String = "pageTitle"

    '    Try
    '        con.Open()
    '        cmd.Connection = con
    '        Do While id > 0
    '            cmd.CommandText = "SELECT PageId, ParentPageID, PageName FROM pages WHERE PageID = " & id
    '            dr = cmd.ExecuteReader(CommandBehavior.SingleRow)
    '            If dr.Read Then
    '                If dr("PageId") = intPageId Then
    '                    temp += "<span class='" & strStyleName & "'>" & dr("PageName") & "</span>,"
    '                Else
    '                    temp += "<a href='/WebAdmin/modules/pages02/Default.aspx?ParentPageId=" & dr("PageId") & "' class='" & strStyleName & "'>" & dr("PageName") & "</a>,"

    '                End If
    '                id = dr("ParentPageId")
    '            End If
    '            dr.Close()
    '        Loop

    '        If intPageId > 0 Then
    '            aryTemp = Split(temp, ",")
    '            temp = ""
    '            For i = (UBound(aryTemp) - 1) To (LBound(aryTemp)) Step -1
    '                temp += aryTemp(i)
    '                If i <> LBound(aryTemp) Then
    '                    temp += " > "
    '                End If
    '            Next
    '        End If
    '        breadcrumb = "<p class='breadcrumb'><a href='/WebAdmin/modules/pages02/Default.aspx' class='" & strStyleName & "'>Site Content</a> "
    '        If intPageId > 0 Then
    '            breadcrumb += "> "
    '        End If
    '        breadcrumb += temp & "</p>"
    '    Finally
    '        con.Close()
    '    End Try
    '    Return breadcrumb
    'End Function

    'Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    '    pnlList.Visible = True
    '    pnlEmail.Visible = False
    'End Sub

    'Protected Sub btnSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSend.Click
    '    If Emagine.SendEmail(Session("EzEditEmail"), Session("EzEditName"), txtEmailTo.Text, txtEmailTo.Text, "", "", txtEmailSubject.Text, txtEmailMessage.Text, "", False) Then
    '        lblAlert.Text = "Your email has been sent successfully."
    '        pnlList.Visible = True
    '        pnlEmail.Visible = False
    '    Else
    '        lblAlert.Text = "An error occurred while attempting to send this email. Please try again later."
    '    End If
    'End Sub




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request("PageID") IsNot Nothing Then _PageID = Emagine.GetNumber(Request("PageID"))

        If _PageID > 0 Then
            lblBreadcrumbs.Text = Pages01.GetBreadcrumbs(_PageID, "Pages02")
            Me.BindRepeaters(_PageID)

            If EzeditUser.HasPagePermissions(Emagine.GetNumber(Session("EzEditUserID")), _PageID) Then
                lblAlert.Text = Session("Alert")
                Session("Alert") = ""
            Else
                Response.Redirect("Default.aspx")
            End If
        End If

    End Sub

    Sub BindRepeaters(ByVal intPageID As Integer)
        rpProductionContent.DataSource = Content01.GetContentVersions("Pages01", intPageID, 20)
        rpProductionContent.DataBind()
        rpProductionContent.EnableViewState = False

        rpStagingContent.DataSource = Content01.GetContentVersions("Pages01", intPageID, 10)
        rpStagingContent.DataBind()
        rpStagingContent.EnableViewState = False
    End Sub

    Sub OnItemCommand(ByVal Src As Object, ByVal Args As RepeaterCommandEventArgs)
        Dim ContentID As Integer = Args.CommandArgument

        If Args.CommandName = "Delete" Then

            Dim MyContent As Content01 = Content01.GetContent(ContentID)
            If Content01.DeleteContent(MyContent) Then
                lblAlert.Text = "Version Deleted"
                Me.BindRepeaters(_PageID)
            Else
                lblAlert.Text = "Error"
            End If
        ElseIf Args.CommandName = "Promote" Then
            If Content01.PromoteContent(ContentID, "Pages01", _PageID) Then
                lblAlert.Text = "The content has been published successfully."
                Me.BindRepeaters(_PageID)
            Else
                lblAlert.Text = "Error publishing content."
            End If
        End If
    End Sub

    Protected Sub rpStagingContent_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rpStagingContent.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim PageID As Integer = CInt(Request("PageID"))
            Dim ContentID As Integer = CInt(DataBinder.Eval(e.Item.DataItem, "ContentID"))

            Dim btnDelete As ImageButton = CType(e.Item.FindControl("btnDelete"), ImageButton)
            Dim hypPreview As HyperLink = CType(e.Item.FindControl("hypPreviewContent"), HyperLink)

            btnDelete.Attributes.Add("onclick", "return confirm_delete();")
            hypPreview.NavigateUrl = "/modules/Pages01/GetPage.aspx?PageID=" & PageID & "&ContentID=" & ContentID

        End If
    End Sub

    Protected Sub rpProductionContent_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rpProductionContent.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim PageID As Integer = CInt(Request("PageID"))
            Dim ContentID As Integer = CInt(DataBinder.Eval(e.Item.DataItem, "ContentID"))

            Dim hypPreview As HyperLink = CType(e.Item.FindControl("hypPreviewContent"), HyperLink)

            hypPreview.NavigateUrl = "/modules/Pages01/GetPage.aspx?PageID=" & PageID & "&ContentID=" & ContentID
        End If
    End Sub

    
End Class
