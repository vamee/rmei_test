
Partial Class Ezedit_Modules_Pages01_EditContent
    Inherits System.Web.UI.Page

    Dim _ContentID As Integer = 0
    Dim _PageID As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request("ContentID") IsNot Nothing Then _ContentID = Request("ContentID")
        If Request("PageID") IsNot Nothing Then _PageID = Request("PageID")

        If Not IsPostBack Then
            Dim Redirect As String = ViewState("Redirect")
            If Len(Redirect) = 0 Then Redirect = Request.ServerVariables("HTTP_REFERER")
            If Len(Redirect) = 0 Then Redirect = "Default.aspx"

            If _PageID = 0 Then Response.Redirect(Redirect)
            If _ContentID = 0 Then
                Dim ResourceID As String = Emagine.GetUniqueID()
                Dim MyContent01 As New Content01
                MyContent01.ResourceID = ResourceID
                MyContent01.ModuleKey = "Pages01"
                MyContent01.ForeignKey = _PageID
                MyContent01.Version = "1"
                MyContent01.CreatedBy = Session("EzEditUsername")
                MyContent01.UpdatedBy = Session("EzEditUsername")
                MyContent01 = Content01.AddContent(MyContent01)
                _ContentID = MyContent01.ContentID

            Else
                Dim MyContent01 As Content01 = Content01.GetContent("Pages01", _PageID)
                If MyContent01.ResourceID.Length = 0 Then
                    Dim ResourceID As String = Emagine.GetUniqueID()
                    MyContent01.ResourceID = ResourceID
                    Content01.UpdateContent(MyContent01)
                End If
            End If

            If _ContentID = 0 Then
                Session("Alert") = "An error occurred while attempting to edit content for this page."
                Response.Redirect(Redirect)
            End If

            ViewState("Redirect") = Redirect

            Dim MyContent As Content01 = Content01.GetContent(_ContentID)
            ContentEditor.EditorContent = MyContent.Content
        End If

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect(ViewState("Redirect"))
    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        ViewState("Editor1") = ContentEditor.EditorContent
        Session("Content") = ""
        Session("Content") = ContentEditor.EditorContent

        pnlEdit.Visible = False
        pnlPreview.Visible = True

        litContent.Text = "<iframe width='800' height='600' src='/Modules/Pages01/GetPage.aspx?PageID=" & Request("PageID") & "' scrolling='auto' frameborder='no'></iframe>"
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Request.Form("preview") = "true" Then
            MasterPageFile = "~/Ezedit/PreviewContent.master"
        Else
            MasterPageFile = "~/Ezedit/MasterPage.master"
        End If
    End Sub

    Protected Sub btnSaveChanges_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveChanges.Click
        Dim MyContent As Content01 = Content01.GetContent(_ContentID)
        MyContent.Content = ViewState("Editor1")
        MyContent.UpdatedBy = Session("EzEditUsername")
        MyContent.UpdatedDate = Date.Now()

        'If MyContent.ResourceID.Length = 0 Then
        '    Dim ResourceID As String = Emagine.GetUniqueID()
        '    MyContent = ResourceID

        '    Dim MyResource As New Resources.Resource
        '    MyResource.ResourceID = ResourceID
        '    MyResource.ResourceType = "Pages01"
        '    MyResource.SortOrder = 1
        '    MyResource.CreatedBy = Session("EzEditUsername")
        '    MyResource.UpdatedBy = Session("EzEditUsername")
        '    Resources.Resource.AddResource(MyResource)
        'End If

        If Content01.UpdateContent(MyContent) Then
            Session("Alert") = "The content has been updated successfully."
            Response.Redirect("ContentList.aspx?PageID=" & _PageID & "&ParentPageID=" & Pages01.GetParentPageID(_PageID))
        Else
            Session("Alert") = "An error was encountered while trying to update the page content."
            Response.Redirect("Default.aspx?ParentPageID=" & Pages01.GetParentPageID(_PageID))
        End If

    End Sub

    Protected Sub btnSaveVersion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveVersion.Click

        Dim MyContent As New Content01
        MyContent.ModuleKey = "Pages01"
        MyContent.ForeignKey = _PageID.ToString
        MyContent.StatusID = 10
        MyContent.Version = Content01.GetMaxVersion("Pages01", _PageID.ToString) + 1
        MyContent.Content = ViewState("Editor1")
        MyContent.CreatedBy = Session("EzEditUsername")
        MyContent.UpdatedBy = Session("EzEditUsername")
        MyContent = Content01.AddContent(MyContent)

        If MyContent.ContentID > 0 Then
            Session("Alert") = "The new version has been saved successfully."
            Response.Redirect("ContentList.aspx?PageID=" & _PageID & "&ParentPageID=" & Pages01.GetParentPageID(_PageID))
        Else
            Session("Alert") = "An error was encountered while trying to Update the Page Content."
            Response.Redirect("Default.aspx?PageId=" & Pages01.GetParentPageID(_PageID))
        End If
    End Sub

    'Protected Sub Editor1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Editor1.Load
    'litEditorInstance.Text = Emagine.SetEditorInstance("Editor1", "pages")
    'End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        ContentEditor.EditorContent = ViewState("Editor1")
    End Sub
End Class
