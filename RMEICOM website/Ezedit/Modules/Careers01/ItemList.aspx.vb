
Partial Class Ezedit_Modules_Careers01_CareerList
    Inherits System.Web.UI.Page

    Dim _CategoryID As Integer = 0
    Dim _ModuleKey As String = "Careers01"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request("CategoryID") IsNot Nothing Then _CategoryID = Emagine.GetNumber(Request("CategoryID"))

        If _CategoryID = 0 Then Response.Redirect("/ezedit/modules/Default.aspx?ModuleKey=" & _ModuleKey)

        lblAlert.Text = ""
    End Sub

    Protected Sub ddlSortOrder_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ddlSortOrder As DropDownList = sender
        Dim Row As GridViewRow = ddlSortOrder.Parent.Parent
        Dim btnEdit As ImageButton = Row.FindControl("btnEdit")
        Dim hdnSortOrder As HiddenField = Row.FindControl("hdnSortOrder")
        Dim ItemID As Integer = Emagine.GetNumber(btnEdit.CommandArgument)
        Dim OldSortOrder As Integer = Emagine.GetNumber(hdnSortOrder.Value)
        Dim NewSortOrder As Integer = Emagine.GetNumber(ddlSortOrder.SelectedValue)
        Dim Sql As String = ""

        If OldSortOrder > NewSortOrder Then
            Sql = "UPDATE Resources SET SortOrder = SortOrder + 1 WHERE ResourceID IN (SELECT ResourceID FROM Careers WHERE CategoryID = " & _CategoryID & " AND CareerID <> " & ItemID & ") AND SortOrder >= " & NewSortOrder & " AND SortOrder < " & OldSortOrder
        Else
            Sql = "UPDATE Resources SET SortOrder = SortOrder - 1 WHERE ResourceID IN (SELECT ResourceID FROM Careers WHERE CategoryID = " & _CategoryID & " AND CareerID <> " & ItemID & ") AND SortOrder <= " & NewSortOrder & " AND SortOrder > " & OldSortOrder
        End If

        Emagine.ExecuteSQL(Sql)

        Emagine.ExecuteSQL("UPDATE Resources SET SortOrder = " & NewSortOrder & " WHERE ResourceID IN (SELECT ResourceID FROM Careers WHERE CareerID = " & ItemID & ")")

        Me.ResetSortOrder()

        gdvList.DataBind()

        lblAlert.Text = "The careers have been sorted successfully."
    End Sub

    Sub ResetSortOrder()
        Dim DataTable As DataTable = Emagine.GetDataTable("SELECT ResourceID FROM qryArticles WHERE CategoryID = " & _CategoryID & " ORDER BY SortOrder")
        For i As Integer = 0 To DataTable.Rows.Count - 1
            Dim Sql As String = "UPDATE Resources SET SortOrder = " & (i + 1) & " WHERE ResourceID = '" & DataTable.Rows(i).Item("ResourceID") & "'"
            Emagine.ExecuteSQL(Sql)
        Next
    End Sub

    Protected Sub cbxIsEnabled_CheckChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cbxIsEnabled As CheckBox = sender
        Dim btnEdit As ImageButton = cbxIsEnabled.Parent.Parent.FindControl("btnEdit")
        Dim ItemID As Integer = Emagine.GetNumber(btnEdit.CommandArgument)
        Dim EnabledText As String = "enabled"

        If Not cbxIsEnabled.Checked Then EnabledText = "disabled"

        If ItemID > 0 Then
            Dim MyCareer As Careers01 = Careers01.GetCareer(ItemID)
            Dim MyResource As Resources.Resource = Resources.Resource.GetResource(MyCareer.ResourceID)
            MyResource.IsEnabled = cbxIsEnabled.Checked
            MyResource.UpdatedDate = Date.Now
            MyResource.UpdatedBy = Session("EzEditUsername")
            If Resources.Resource.UpdateResource(MyResource) Then
                lblAlert.Text = "The item has been " & EnabledText & " successfully."
                gdvList.DataBind()
            Else
                lblAlert.Text = "An error occurred."
            End If
        End If
    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim ItemID As Integer = Button.CommandArgument

        Me.DisplayEditPanel(ItemID)
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim ItemID As Integer = Button.CommandArgument

        Dim MyCareer As Careers01 = Careers01.GetCareer(ItemID)
        If MyCareer.CareerID > 0 Then
            If Careers01.Delete(MyCareer) Then
                Me.ResetSortOrder()
                gdvList.DataBind()

                lblAlert.Text = "The career has been removed successfully."
            Else
                lblAlert.Text = "An error occurred while attempting to delete this career."
            End If
        Else
            lblAlert.Text = "An error occurred while attempting to delete this career."
        End If
    End Sub


    Sub DisplayListPanel()
        pnlList.Visible = True
        pnlEdit.Visible = False

        Me.ResetEditForm()
        gdvList.DataBind()
    End Sub

    Sub DisplayEditPanel(ByVal intItemID As Integer)
        ddlCategoryID.SelectedIndex = -1
        ddlModuleTypeID.SelectedIndex = -1

        If intItemID > 0 Then
            Dim MyCareer As Careers01 = Careers01.GetCareer(intItemID)
            If MyCareer.CareerID > 0 Then

                For Each Item As ListItem In ddlCategoryID.Items
                    If Item.Value = MyCareer.CategoryID Then
                        Item.Selected = True
                        Exit For
                    End If
                Next

                For Each Item As ListItem In ddlModuleTypeID.Items
                    If Item.Value = MyCareer.ModuleTypeID Then
                        Item.Selected = True
                        Exit For
                    End If
                Next

                If IsDate(MyCareer.DisplayDate) Then txtDisplayDate.Text = String.Format("{0:d}", CDate(MyCareer.DisplayDate))
                txtResourceName.Text = MyCareer.ResourceName
                rblIsEnabled.SelectedValue = MyCareer.IsEnabled
                If MyCareer.DisplayStartDate > "1/1/1900" Then txtDisplayStartDate.Text = MyCareer.DisplayStartDate
                If MyCareer.DisplayEndDate > "1/1/1900" Then txtDisplayEndDate.Text = MyCareer.DisplayEndDate
                txtSummary.Text = MyCareer.CareerSummary
                txtKeywords.Text = MyCareer.ResourceKeywords
                txtContentEditor.EditorContent = MyCareer.CareerText
                txtExternalUrl.Text = MyCareer.ExternalUrl

                Select Case Emagine.GetDbValue("SELECT ModuleType FROM ModuleTypes WHERE ModuleKey = '" & _ModuleKey & "' AND ModuleTypeID = " & MyCareer.ModuleTypeID)
                    Case "Local Career Listing"
                        pnlCommon.Visible = True
                        pnlContent.Visible = True
                        pnlExternalUrl.Visible = False

                    Case "External Link"
                        pnlCommon.Visible = True
                        pnlContent.Visible = False
                        pnlExternalUrl.Visible = True
                End Select
            End If
        Else
            For Each Item As ListItem In ddlCategoryID.Items
                If Item.Value = _CategoryID Then
                    Item.Selected = True
                    Exit For
                End If
            Next
        End If

        hdnItemID.Value = intItemID
        pnlList.Visible = False
        pnlEdit.Visible = True
    End Sub

    Sub ResetEditForm()
        ddlCategoryID.SelectedIndex = -1
        ddlModuleTypeID.SelectedIndex = -1
        txtDisplayDate.Text = ""
        txtResourceName.Text = ""
        rblIsEnabled.SelectedIndex = 0
        txtDisplayStartDate.Text = ""
        txtDisplayEndDate.Text = ""
        txtSummary.Text = ""
        txtKeywords.Text = ""
        txtContentEditor.EditorContent = ""
        txtExternalUrl.Text = ""
        hdnItemID.Value = -1

        pnlCommon.Visible = False
        pnlContent.Visible = False
        pnlExternalUrl.Visible = False
    End Sub

    Protected Sub lbtnAddItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnAddItem.Click
        Me.DisplayEditPanel(0)
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DisplayListPanel()
    End Sub

    Protected Sub ddlCategoryID_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCategoryID.Load
        If Not Page.IsPostBack Then
            ddlCategoryID.DataSource = ModuleCategory.GetModuleCategories("Careers01")
            ddlCategoryID.DataTextField = "CategoryName"
            ddlCategoryID.DataValueField = "CategoryID"
            ddlCategoryID.DataBind()
        End If
    End Sub

    Protected Sub ddlModuleTypeID_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlModuleTypeID.Load
        If Not Page.IsPostBack Then
            ddlModuleTypeID.DataSource = Modules.GetModuleTypes("Careers01")
            ddlModuleTypeID.DataTextField = "ModuleType"
            ddlModuleTypeID.DataValueField = "ModuleTypeID"
            ddlModuleTypeID.DataBind()
        End If
    End Sub

    Protected Sub ddlModuleTypeID_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlModuleTypeID.SelectedIndexChanged
        Dim ModuleType As String = ddlModuleTypeID.SelectedItem.Text
        If ddlModuleTypeID.SelectedIndex > 0 Then pnlCommon.Visible = True

        Select Case ModuleType
            Case "Local Career Listing"
                pnlContent.Visible = True
                pnlExternalUrl.Visible = False

            Case "External Link"
                pnlContent.Visible = False
                pnlExternalUrl.Visible = True
        End Select
    End Sub

    Protected Sub gdvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ItemID As Integer = e.Row.DataItem("CareerID")
            Dim IsEnabled As Boolean = e.Row.DataItem("IsEnabled")
            Dim SortOrder As Integer = e.Row.DataItem("SortOrder")
            Dim ModuleType As String = e.Row.DataItem("ModuleType").ToString
            Dim ArticleUrl As String = e.Row.DataItem("ExternalUrl").ToString
            'Dim FileName As String = e.Row.DataItem("FileName").ToString
            Dim DisplayStartDate As Date = e.Row.DataItem("DisplayStartDate").ToString
            Dim DisplayEndDate As Date = e.Row.DataItem("DisplayEndDate").ToString

            Dim lblStatus As Label = e.Row.FindControl("lblStatus")
            Dim lblDisplayDates As Label = e.Row.FindControl("lblDisplayDates")
            Dim hypModuleType As HyperLink = e.Row.FindControl("hypModuleType")
            Dim cbxIsEnabled As CheckBox = e.Row.FindControl("cbxIsEnabled")
            Dim hdnSortOrder As HiddenField = e.Row.FindControl("hdnSortOrder")
            Dim ddlSortOrder As DropDownList = e.Row.FindControl("ddlSortOrder")

            Select Case ModuleType
                Case "Local Career Listing"
                    hypModuleType.ImageUrl = "/App_Themes/EzEdit/images/page_white.png"
                    hypModuleType.ToolTip = ModuleType
                Case "External Link"
                    hypModuleType.ImageUrl = "/App_Themes/EzEdit/images/world_go.png"
                    hypModuleType.ToolTip = ModuleType & ": " & ArticleUrl
                    hypModuleType.Target = "_blank"
                    hypModuleType.NavigateUrl = ArticleUrl
            End Select

            If Not IsEnabled Then
                lblStatus.Text = "DISABLED"
                lblStatus.BackColor = Drawing.Color.Red
                lblStatus.ForeColor = Drawing.Color.White
                lblStatus.ToolTip = "Check the 'Enabled' box to enable this item."

            Else
                If DisplayStartDate.ToString.Length > 0 And DisplayEndDate.ToString.Length > 0 Then
                    If String.Format("{0:d}", CDate(DisplayStartDate)) = "1/1/1900" And String.Format("{0:d}", CDate(DisplayEndDate)) = "1/1/1900" Then
                        lblStatus.Text = "CURRENT"
                        lblStatus.BackColor = Drawing.Color.Green
                        lblStatus.ForeColor = Drawing.Color.White
                        lblStatus.ToolTip = "No Expiration"

                    ElseIf CDate(DisplayStartDate) > Date.Now Then
                        lblStatus.Text = "PENDING"
                        lblStatus.BackColor = Drawing.Color.Yellow
                        lblStatus.ForeColor = Drawing.Color.Black
                        lblStatus.ToolTip = String.Format("{0:d}", DisplayStartDate) & "-" & String.Format("{0:d}", DisplayEndDate)

                    ElseIf CDate(DisplayEndDate) < Date.Now Then
                        lblStatus.Text = "EXPIRED"
                        lblStatus.BackColor = Drawing.Color.Black
                        lblStatus.ForeColor = Drawing.Color.White
                        lblStatus.ToolTip = String.Format("{0:d}", DisplayStartDate) & "-" & String.Format("{0:d}", DisplayEndDate)

                    ElseIf CDate(DisplayStartDate) <= Date.Now And CDate(DisplayEndDate) >= Date.Now Then
                        lblStatus.Text = "CURRENT"
                        lblStatus.BackColor = Drawing.Color.Green
                        lblStatus.ForeColor = Drawing.Color.White
                        lblStatus.ToolTip = String.Format("{0:d}", DisplayStartDate) & "-" & String.Format("{0:d}", DisplayEndDate)
                    End If
                End If
            End If

            hdnSortOrder.Value = SortOrder
            Me.BindSortOrder(ddlSortOrder, SortOrder)
            cbxIsEnabled.Checked = IsEnabled
        End If
    End Sub

    Sub BindSortOrder(ByVal ddlSortOrder As DropDownList, ByVal intSortOrder As Integer)
        Dim MaxSortOrder As Integer = Emagine.GetDbValue("SELECT COUNT(*) As MaxSortOrder FROM Careers WHERE CategoryID = " & _CategoryID)

        For i As Integer = 1 To MaxSortOrder
            Dim ListItem As New ListItem(i.ToString, i.ToString)
            If i = intSortOrder Then ListItem.Selected = True
            ddlSortOrder.Items.Add(ListItem)
        Next
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If PeterBlum.VAM.Globals.Page.IsValid Then
            Dim ErrorMessage As String = ""
            Dim ItemID As Integer = Emagine.GetNumber(hdnItemID.Value)

            Dim MyCareer As New Careers01
            If ItemID > 0 Then MyCareer = Careers01.GetCareer(ItemID)
            MyCareer.LanguageID = Session("EzEditLanguageID")
            MyCareer.CategoryID = ddlCategoryID.SelectedValue
            MyCareer.ModuleTypeID = ddlModuleTypeID.SelectedValue
            MyCareer.DisplayDate = txtDisplayDate.Text
            MyCareer.ResourceName = txtResourceName.Text
            MyCareer.IsEnabled = rblIsEnabled.SelectedValue
            If IsDate(txtDisplayStartDate.Text) Then MyCareer.DisplayStartDate = txtDisplayStartDate.Text
            If IsDate(txtDisplayEndDate.Text) Then MyCareer.DisplayEndDate = txtDisplayEndDate.Text
            MyCareer.CareerSummary = txtSummary.Text
            MyCareer.ResourceKeywords = txtKeywords.Text
            MyCareer.CareerText = txtContentEditor.EditorContent
            MyCareer.ExternalUrl = txtExternalUrl.Text
            MyCareer.UpdatedBy = Session("EzEditUsername")
            MyCareer.UpdatedDate = Date.Now()

            If ItemID > 0 Then
                If Careers01.Update(MyCareer, ErrorMessage) Then
                    Me.ResetEditForm()
                    Me.DisplayListPanel()
                    lblAlert.Text = "The career has been updated successfully."
                Else
                    lblAlert.Text = "An error occurred while attempting to update this career.<br />" & ErrorMessage
                End If

            Else
                MyCareer.CreatedBy = Session("EzEditUsername")
                MyCareer = Careers01.Add(MyCareer, ErrorMessage)

                If MyCareer.CareerID > 0 Then
                    Me.ResetEditForm()
                    Me.DisplayListPanel()
                    lblAlert.Text = "The career has been added successfully."
                Else
                    lblAlert.Text = "An error occurred while attempting to add this career.<br />" & ErrorMessage
                End If
            End If
        End If
    End Sub
End Class
