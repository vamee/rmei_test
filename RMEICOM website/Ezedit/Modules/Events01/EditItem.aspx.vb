Imports PeterBlum.vam

Partial Class Ezedit_Modules_Events01_EditItem
    Inherits System.Web.UI.Page

    Dim ItemID As Integer = 0
    Dim CategoryID As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ItemID = Emagine.GetNumber(Request("ItemID"))
        CategoryID = Emagine.GetNumber(Request("CategoryID"))

        If Request("Action") = "DeleteFile" Then
            Dim MyEvent As New Events01
            MyEvent = Events01.GetEvent(ItemID)
            If MyEvent.ImageUrl.Length > 0 Then
                Emagine.DeleteFile(Server.MapPath(MyEvent.ImageUrl))
                MyEvent.ImageUrl = ""
                Events01.UpdateEvent(MyEvent)

                lblAlert.Text = "The file has been removed successfully."
                'Response.Redirect("ItemList.aspx?CategoryId=" & MyEvent.CategoryID)
            End If
        End If

        If Not IsPostBack Then
            Me.BindControls()
            If ItemID <> 0 Then
                Me.Edit(ItemID)
            Else
                Me.Add(CategoryID)
            End If
        End If
        
        'csvPageKey.ServerCondition = New ServerConditionEventHandler(AddressOf ValidatePageKey)
    End Sub

    Sub Edit(ByVal intItemId As Integer)

        Dim MyEvent As New Events01
        MyEvent = Events01.GetEvent(intItemId)
        ddlCategoryId.SelectedValue = MyEvent.CategoryID
        txtResourceName.Text = MyEvent.ResourceName
        ddlResourceCategory.SelectedValue = MyEvent.ResourceCategory
        txtArchiveUrl.Text = MyEvent.ArchiveURL
        txtSummary.Text = MyEvent.EventSummary
        txtKeywords.Text = MyEvent.ResourceKeywords
        ContentEditor.EditorContent = MyEvent.EventDescription
        If Len(MyEvent.ImageURL) > 0 Then
            lblImageUrl.Text = "<br>"
            lblImageUrl.Text += "<b>Current File:</b> "
            lblImageUrl.Text += "<a href='" & MyEvent.ImageUrl & "' target='_blank' class='main'>"
            lblImageUrl.Text += MyEvent.ImageUrl & "</a> <i>(" & Emagine.FormatFileSize(Emagine.GetFileSize(Server.MapPath(MyEvent.ImageUrl))) & ")</i>"
            lblImageUrl.Text += " - <a href='#A' onclick=""deleteFile('EditItem.aspx?ItemID=" & intItemId & "&CategoryID=" & MyEvent.CategoryID & "&Asction=DeleteFile');"">Delete File</a>"
        End If
        'CategoryID = MyEvent.CategoryID

        litPageTitle.Text = "<span class='PageTitle'><a href='../Default.aspx?ModuleKey=Events01' class='pageTitle'>Events</a> > <a href='ItemList.aspx?CategoryID=" & MyEvent.CategoryID & "' class='pageTitle'>" & MyEvent.CategoryName & "</a> > Edit Event</span>"
    End Sub

    Sub Add(ByVal intCategoryId As Integer)
        ddlCategoryId.SelectedValue = intCategoryId

        litPageTitle.Text = "<span class='PageTitle'><a href='../Default.aspx?ModuleKey=Events01' class='pageTitle'>Events</a> > <a href='ItemList.aspx?CategoryID=" & intCategoryId & "' class='pageTitle'>" & ModuleCategory.GetModuleCategoryName(intCategoryId) & "</a> > Add New Event</span>"
    End Sub

    Sub BindControls()
        Me.BindCategoryIdDDL()
        Me.BindResourceCategoryDDL()
    End Sub

    Sub BindCategoryIdDDL()
        ddlCategoryId.DataSource = ModuleCategory.GetModuleCategories("Events01")
        ddlCategoryId.DataTextField = "CategoryName"
        ddlCategoryId.DataValueField = "CategoryId"
        ddlCategoryId.DataBind()
    End Sub

    Sub BindResourceCategoryDDL()
        ddlResourceCategory.DataSource = Resources.GetResourceCategories("Events01")
        ddlResourceCategory.DataTextField = "ResourceCategory"
        ddlResourceCategory.DataValueField = "ResourceCategory"
        ddlResourceCategory.DataBind()
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        PeterBlum.VAM.Globals.Page.Validate()

        If PeterBlum.VAM.Globals.Page.IsValid Then

            Dim EventID = ItemID
            Dim NewCategoryID = Emagine.GetNumber(ddlCategoryId.SelectedValue)
            Dim OldCategoryID As Integer = 0
            Dim Result As Boolean = False

            Dim MyEvent As New Events01
            If EventID > 0 Then
                MyEvent = Events01.GetEvent(EventID)
                OldCategoryID = MyEvent.CategoryID
            End If

            MyEvent.CategoryID = ddlCategoryId.SelectedValue
            MyEvent.EventSummary = txtSummary.Text
            MyEvent.EventDescription = ContentEditor.EditorContent
            MyEvent.ImageUrl = ""
            If uplImageUrl.HasFile Then
                Dim VirtualFilePath As String = Application("VirtualDocumentUploadPath") & Session("EzEditLanguageName") & "/" & uplImageUrl.FileName
                Dim AbsoluteFilePath As String = Server.MapPath(VirtualFilePath)
                MyEvent.ImageUrl = VirtualFilePath
                uplImageUrl.PostedFile.SaveAs(AbsoluteFilePath)
            ElseIf EventID = 0 Then
                MyEvent.ArchiveURL = ""
            End If
            MyEvent.AllowRegistration = False

            MyEvent.ArchiveURL = txtArchiveUrl.Text

            Dim Resource As New Resources.Resource
            Resource.ResourceId = MyEvent.ResourceId
            Resource.ResourceName = txtResourceName.Text
            If txtResourceCategory.Text.Length > 0 Then
                Resource.ResourceCategory = txtResourceCategory.Text
            Else
                Resource.ResourceCategory = ddlResourceCategory.SelectedValue
            End If
            Resource.ResourceType = "Events01"
            Resource.ResourcePageKey = "" 'txtResourcePageKey.Text
            Resource.ResourceKeywords = txtKeywords.Text

            If NewCategoryID <> OldCategoryID Then MyEvent.SortOrder = Events01.GetMaxSortOrder(NewCategoryID) + 1

            If Emagine.GetNumber(MyEvent.EventID) > 0 Then
                If Events01.UpdateEvent(MyEvent) Then
                    If Resources.Resource.UpdateResource(Resource) Then
                        If NewCategoryID <> OldCategoryID Then Events01.ResetSortOrder(OldCategoryID)
                        Session("Alert") = "The event has been updated successfully."
                        Result = True
                    End If
                End If
            Else
                MyEvent.ResourceId = Emagine.GetUniqueID
                MyEvent.EventID = Events01.AddEvent(MyEvent)
                If MyEvent.EventID > 0 Then
                    Resource.ResourceId = MyEvent.ResourceId
                    Result = Resources.Resource.AddResource(Resource)
                    Session("Alert") = "The event has been added successfully."
                End If
            End If

            If Result = True Then
                Response.Redirect("ItemList.aspx?CategoryId=" & MyEvent.CategoryID)
            Else
                lblAlert.Text = "An Error has Occurred."
            End If

        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("ItemList.aspx?CategoryID=" & CategoryID)
    End Sub

    Protected Sub ValidatePageKey(ByVal sourceCondition As BaseCondition, ByVal args As ConditionEventArgs)
        Dim vArgs As ConditionTwoFieldEventArgs = CType(args, ConditionTwoFieldEventArgs)
        args.IsMatch = Events01.IsUniquePageKey(vArgs.Value, ItemID, CategoryID)
    End Sub

End Class
