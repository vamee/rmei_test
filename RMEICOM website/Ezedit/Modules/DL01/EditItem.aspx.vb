Imports PeterBlum.VAM


Partial Class Ezedit_Modules_DL01_EditItem
    Inherits System.Web.UI.Page

    Dim _CategoryID As Integer = 0
    Dim _ItemID As Integer = 0
    Dim _ModuleKey As String = "DL01"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request("CategoryID") IsNot Nothing Then _CategoryID = Emagine.GetNumber(Request("CategoryID"))
        If Request("ItemID") IsNot Nothing Then _ItemID = Emagine.GetNumber(Request("ItemID"))

        If Not IsPostBack Then
            'Dim intCategoryId As Integer = CInt(Request("CategoryId"))
            'Dim intItemId As Integer = CInt(Request("ItemId"))
            'ViewState.Add("ItemId", intItemId)
            'ViewState.Add("CategoryId", intCategoryId)
            Me.BindControls()
            If _ItemID <> 0 Then
                Me.Edit(_ItemID)
            Else
                Me.Add(_CategoryID)
            End If
        End If

        If _ItemID > 0 Then rtvFilename.Enabled = False
        'csvPageKey.ServerCondition = New ServerConditionEventHandler(AddressOf ValidatePageKey)
    End Sub

    Sub Edit(ByVal intItemId As Integer)
        lblPageTitle.Text = "Edit Download"
        Dim Download As New DL01
        Download = DL01.GetDownload(intItemId)
        If Download.DisplayDate > CDate("1/1/1900") Then pdpDate.Text = Download.DisplayDate
        ddlCategoryId.SelectedValue = Download.CategoryID
        ddlModuleTypeID.SelectedValue = Download.ModuleTypeID
        'ViewState.Add("CategoryId", Download.CategoryId)
        txtDescription.Text = Download.Description
        txtResourceName.Text = Download.ResourceName
        ddlResourceCategory.SelectedValue = Download.ResourceCategory
        'txtResourcePageKey.Text = Download.ResourcePageKey
        txtKeywords.Text = Download.ResourceKeywords
        If Len(Download.Filename) > 0 Then
            tblFileName.Visible = True
            hypFileName.Text = Download.Filename.Replace("~", "") & " - " & Emagine.FormatFileSize(Download.FileSize)
            hypFileName.NavigateUrl = Download.Filename
        End If
        txtExternalUrl.Text = Download.ExternalUrl
        rblRegistrationRequired.SelectedValue = Emagine.GetNumber(Download.RegistrationRequired)
        txtCampaignInfo.Text = Download.CampaignInfo
        If String.Format("{0:d}", Download.DisplayStartDate) <> "1/1/1900" Then txtDisplayStartDate.Text = String.Format("{0:d}", Download.DisplayStartDate)
        If String.Format("{0:d}", Download.DisplayEndDate) <> "1/1/1900" Then txtDisplayEndDate.Text = String.Format("{0:d}", Download.DisplayEndDate)

        Me.DisplayModuleType(Download.ModuleTypeID)

        'ViewState.Add("ResourceId", Download.ResourceId)
        lblPageTitle.Text = "<span class='PageTitle'><a href='../Default.aspx?ModuleKey=DL01' class='pageTitle'>Downloads</a> > <a href='ItemList.aspx?CategoryID=" & Download.CategoryID & "' class='pageTitle'>" & ModuleCategory.GetModuleCategoryName(Download.CategoryID) & "</a> > Edit Download</span>"
    End Sub

    Sub Add(ByVal intCategoryId As Integer)
        lblPageTitle.Text = "Add New Download"
        lblPageTitle.Text = "<span class='PageTitle'><a href='../Default.aspx?ModuleKey=DL01' class='pageTitle'>Downloads</a> > <a href='ItemList.aspx?CategoryID=" & intCategoryId & "' class='pageTitle'>" & ModuleCategory.GetModuleCategoryName(intCategoryId) & "</a> > Add New Download</span>"
        ddlCategoryId.SelectedValue = intCategoryId
        ddlModuleTypeID.SelectedIndex = 0
        rblRegistrationRequired.SelectedValue = 1
        trRegistrationRequired.Visible = True
    End Sub

    Sub BindControls()
        Me.BindCategoryIdDDL()
        Me.BindResourceCategoryDDL()
    End Sub

    Sub BindCategoryIdDDL()
        ddlCategoryId.DataSource = ModuleCategory.GetModuleCategories("DL01")
        ddlCategoryId.DataTextField = "CategoryName"
        ddlCategoryId.DataValueField = "CategoryId"
        ddlCategoryId.DataBind()
    End Sub

    Sub BindResourceCategoryDDL()
        ddlResourceCategory.DataSource = LookupOptions.GetOptions("Download Categories") 'Resources.GetResourceCategories("DL01")
        ddlResourceCategory.DataTextField = "OptionText"
        ddlResourceCategory.DataValueField = "OptionValue"
        ddlResourceCategory.DataBind()
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If PeterBlum.VAM.Globals.Page.IsValid Then

            Dim DownloadID As Integer = Emagine.GetNumber(_ItemID)
            Dim NewCategoryID = Emagine.GetNumber(ddlCategoryId.SelectedValue)
            Dim OldCategoryID As Integer = 0
            Dim Result As Boolean = False

            Dim Download As New DL01
            If DownloadID > 0 Then
                Download = DL01.GetDownload(DownloadID)
                OldCategoryID = Download.CategoryID
            Else
                Download.Filename = ""
            End If

            Download.CategoryID = NewCategoryID
            Download.Description = txtDescription.Text
            Download.DisplayDate = pdpDate.Text
            If txtFilename.HasFile Then
                Dim VirtualFilePath As String = GlobalVariables.GetValue("VirtualDocumentUploadPath") & Session("EzEditLanguageName") & "/" & txtFilename.FileName
                Dim AbsoluteFilePath As String = Server.MapPath(VirtualFilePath)
                Download.Filename = VirtualFilePath
                txtFilename.PostedFile.SaveAs(AbsoluteFilePath)
                Download.Filesize = txtFilename.PostedFile.ContentLength
            End If
            Download.ExternalUrl = txtExternalUrl.Text
            Download.RegistrationRequired = rblRegistrationRequired.SelectedValue

            Dim Resource As New Resources.Resource
            Resource.ResourceId = Download.ResourceId
            Resource.ModuleTypeID = ddlModuleTypeID.SelectedValue
            Resource.ResourceName = txtResourceName.Text
            If txtResourceCategory.Text.Length > 0 Then
                Resource.ResourceCategory = txtResourceCategory.Text
            Else
                Resource.ResourceCategory = ddlResourceCategory.SelectedValue
            End If
            Resource.ResourceType = "DL01"
            Resource.ResourcePageKey = ""
            Resource.ResourceKeywords = txtKeywords.Text
            Resource.CampaignInfo = txtCampaignInfo.Text
            If txtDisplayStartDate.Text.Length > 0 Then Resource.DisplayStartDate = txtDisplayStartDate.Text
            If txtDisplayEndDate.Text.Length > 0 Then Resource.DisplayEndDate = txtDisplayEndDate.Text

            If NewCategoryID <> OldCategoryID Then Download.SortOrder = DL01.GetMaxSortOrder(NewCategoryID) + 1

            If Emagine.GetNumber(Download.DownloadID) > 0 Then
                If Download.UpdateDownload(Download) Then
                    If Resources.Resource.UpdateResource(Resource) Then
                        If NewCategoryID <> OldCategoryID Then DL01.ResetSortOrder(OldCategoryID)
                        Session("Alert") = "The download has been updated successfully."
                        Result = True
                    End If
                End If
            Else
                Download.ResourceId = Emagine.GetUniqueID
                Download.DownloadID = Download.AddDownload(Download)
                If Download.DownloadID > 0 Then
                    Resource.ResourceId = Download.ResourceId
                    Result = Resources.Resource.AddResource(Resource)
                    Session("Alert") = "The download has been added successfully."
                End If

            End If

            If Result = True Then
                Response.Redirect("ItemList.aspx?CategoryId=" & Download.CategoryId)
            Else
                lblAlert.Text = "An Error has Occurred."
            End If
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim CategoryID As Integer = _CategoryID
        If CategoryID = 0 Then CategoryID = Emagine.GetNumber(Emagine.GetDbValue("SELECT CategoryID FROM Downloads WHERE DownloadID = " & _ItemID))

        Response.Redirect("ItemList.aspx?CategoryID=" & CategoryID)
    End Sub

    Protected Sub ValidatePageKey(ByVal sourceCondition As BaseCondition, ByVal args As ConditionEventArgs)
        Dim vArgs As ConditionTwoFieldEventArgs = CType(args, ConditionTwoFieldEventArgs)
        args.IsMatch = DL01.IsUniquePageKey(vArgs.Value, _ItemID, _CategoryID)
    End Sub

    Protected Sub ddlModuleTypeID_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlModuleTypeID.Load
        If Not Page.IsPostBack Then
            ddlModuleTypeID.DataSource = Modules.GetModuleTypes("DL01")
            ddlModuleTypeID.DataTextField = "ModuleType"
            ddlModuleTypeID.DataValueField = "ModuleTypeID"
            ddlModuleTypeID.DataBind()
        End If
    End Sub

    Protected Sub ddlModuleTypeID_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlModuleTypeID.SelectedIndexChanged
        DisplayModuleType(ddlModuleTypeID.SelectedValue)
    End Sub

    Sub DisplayModuleType(ByVal intModuleTypeID As Integer)
        Select Case intModuleTypeID
            Case 0
                pnlFile.Visible = False
                pnlUrl.Visible = False
            Case 11
                pnlFile.Visible = True
                pnlUrl.Visible = False
            Case 12
                pnlFile.Visible = False
                pnlUrl.Visible = True
        End Select
    End Sub

    
End Class
