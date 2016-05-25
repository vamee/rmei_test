
Partial Class Ezedit_Modules_ContentTabs_ItemList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim CategoryID = Request("CategoryID")
        If Not IsPostBack Then
            pnlItemList.Visible = True
            lblPageTitle.Text = "<a href='/ezedit/modules/Default.aspx?ModuleKey=ContentTabs' class='pageTitle'>Content Tabs</a>"
            hdnCategoryID.Value = CategoryID

            'gdvItems.DataSource = Emagine.GetDataTable("SELECT DISTINCT TabID, ResourceName, SortOrder, IsEnabled FROM qryContentTabs WHERE CategoryID = " & CategoryID & " ORDER BY SortOrder")
            'gdvItems.DataBind()

        End If
    End Sub

    Sub EditItem(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim btnEdit As ImageButton = sender
        pnlEditItem.Visible = True
        pnlItemList.Visible = False

        Dim ItemID As Integer = CInt(btnEdit.CommandArgument)
        If ItemID > 0 Then
            Dim Sql As String = "SELECT * FROM qryContentTabs WHERE TabID = " & ItemID
            Dim DataTable As DataTable = Emagine.GetDataTable(Sql)
            If DataTable.Rows.Count > 0 Then
                hdnItemID.Value = ItemID
                ddlCategoryID.SelectedValue = DataTable.Rows(0).Item("CategoryID").ToString
                txtResourceName.Text = DataTable.Rows(0).Item("ResourceName").ToString
                ucContentEditor.EditorContent = DataTable.Rows(0).Item("Content").ToString
                txtStartDate.Text = ""
                txtEndDate.Text = ""
                If CDate(DataTable.Rows(0).Item("DisplayStartDate")) > "1/1/1901" Then txtStartDate.Text = String.Format("{0:d}", CDate(DataTable.Rows(0).Item("DisplayStartDate").ToString()))
                If CDate(DataTable.Rows(0).Item("DisplayEndDate")) > "1/1/1901" Then txtEndDate.Text = String.Format("{0:d}", CDate(DataTable.Rows(0).Item("DisplayEndDate").ToString))
                rblIsEnabled.SelectedValue = Emagine.GetNumber(DataTable.Rows(0).Item("IsEnabled"))
            End If
        End If
    End Sub

    Sub AddItem()
        pnlEditItem.Visible = True
        pnlItemList.Visible = False
        hdnItemID.Value = 0
        ddlCategoryID.SelectedValue = hdnCategoryID.Value
        txtResourceName.Text = ""
        txtResourceName.Focus()
        ucContentEditor.EditorContent = ""
        txtStartDate.Text = ""
        txtEndDate.Text = ""
        rblIsEnabled.SelectedValue = 1

    End Sub

    Sub DeleteRecord(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim btnDelete As ImageButton = sender
        Dim RecordID As Integer = CInt(btnDelete.CommandArgument)
        Dim ResourceID As String = Emagine.GetDbValue("SELECT ResourceID FROM ContentTabs WHERE TabID = " & RecordID)
        Dim Sql As String = ""

        Emagine.ExecuteSQL("DELETE FROM Resources WHERE ResourceID IN (SELECT ResourceID FROM ContentTabs WHERE TabID = " & RecordID & ")")
        Emagine.ExecuteSQL("DELETE FROM ContentTabs WHERE TabID = " & RecordID)
        Emagine.ExecuteSQL("DELETE FROM Content WHERE ModuleKey = 'ContentTabs' AND ForeignKey = '" & RecordID & "'")

        Dim CategoryID = Emagine.GetNumber(Request("CategoryID"))
        Me.ResetSortOrder(CategoryID)

        lblAlert.Text = "The record has been removed successfully."

        pnlItemList.Visible = True
        pnlEditItem.Visible = False

        'gdvItems.DataSource = Emagine.GetDataTable("SELECT DISTINCT TabID, ResourceName, SortOrder, IsEnabled FROM qryContentTabs WHERE CategoryID = " & hdnCategoryID.Value & " ORDER BY SortOrder")
        gdvItems.DataBind()

    End Sub

    Protected Sub gdvItems_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvItems.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ItemID As Integer = CInt(DataBinder.Eval(e.Row.DataItem, "TabID"))
            Dim SortOrder As Integer = CInt(DataBinder.Eval(e.Row.DataItem, "SortOrder"))
            Dim DisplayStartDate As Date = e.Row.DataItem("DisplayStartDate").ToString
            Dim DisplayEndDate As Date = e.Row.DataItem("DisplayEndDate").ToString
            Dim IsEnabled As Boolean = DataBinder.Eval(e.Row.DataItem, "IsEnabled")

            Dim hdnSortOrder As HiddenField = e.Row.FindControl("hdnSortOrder")
            Dim cbxIsEnabled As CheckBox = e.Row.FindControl("cbxIsEnabled")
            Dim hdnItemID As HiddenField = e.Row.FindControl("hdnItemID")
            Dim btnEdit As ImageButton = e.Row.FindControl("btnEdit")
            Dim btnDelete As ImageButton = e.Row.FindControl("btnDelete")
            Dim ddlSortOrder As DropDownList = e.Row.FindControl("ddlSortOrder")
            Dim lblStatus As Label = e.Row.FindControl("lblStatus")
            lblStatus.Width = "55"

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

            If hdnSortOrder IsNot Nothing Then hdnSortOrder.Value = SortOrder
            If hdnItemID IsNot Nothing Then hdnItemID.Value = ItemID
            If btnEdit IsNot Nothing Then btnEdit.CommandArgument = ItemID
            If btnDelete IsNot Nothing Then btnDelete.CommandArgument = ItemID
            If ddlSortOrder IsNot Nothing Then BindSortOrder(ddlSortOrder, SortOrder)
            If cbxIsEnabled IsNot Nothing Then cbxIsEnabled.Checked = IsEnabled
        End If
    End Sub

    Sub BindSortOrder(ByVal ddlSortOrder As DropDownList, ByVal intSortOrder As Integer)
        Dim MaxSortOrder As Integer = Emagine.GetDbValue("SELECT COUNT(*) As MaxSortOrder FROM ContentTabs WHERE CategoryID = " & hdnCategoryID.Value)

        For i As Integer = 1 To MaxSortOrder
            Dim ListItem As New ListItem(i.ToString, i.ToString)
            If i = intSortOrder Then ListItem.Selected = True
            ddlSortOrder.Items.Add(ListItem)
        Next

    End Sub

    Protected Sub btnCancelItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelItem.Click
        pnlItemList.Visible = True
        pnlEditItem.Visible = False
    End Sub

    Protected Sub btnSaveItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveItem.Click
        If PeterBlum.VAM.Globals.Page.IsValid Then
            Dim Result As Boolean = False
            Dim ItemID As Integer = CInt(hdnItemID.Value.ToString)
            Dim NewCategoryID As Integer = ddlCategoryID.SelectedValue
            Dim OldCategoryID As Integer = 0
            
            Dim Tab As New ContentTab
            If ItemID > 0 Then
                Tab = ContentTab.GetTab(ItemID)
                OldCategoryID = Tab.CategoryID
            End If

            Tab.CategoryID = NewCategoryID
            Tab.ResourceName = txtResourceName.Text
            If ucContentEditor.EditorContent.Length > 0 Then
                Tab.Content = ucContentEditor.EditorContent
            Else
                Tab.Content = ""
                Tab.SortOrder = ContentTabs.GetMaxSortOrder(OldCategoryID) + 1
            End If
            If NewCategoryID <> OldCategoryID Then Tab.SortOrder = ContentTabs.GetMaxSortOrder(NewCategoryID) + 1
            If txtStartDate.Text.Length > 0 Then Tab.DisplayStartDate = txtStartDate.Text
            If txtEndDate.Text.Length > 0 Then Tab.DisplayEndDate = txtEndDate.Text
            Tab.IsEnabled = rblIsEnabled.SelectedValue

            Dim Resource As New Resources.Resource
            Resource.ResourceId = Tab.ResourceId
            Resource.ModuleTypeID = 0
            Resource.ResourceName = txtResourceName.Text
            Resource.ResourceCategory = ""
            Resource.ResourceType = "ContentTabs"
            Resource.ResourcePageKey = ""
            Resource.ResourceKeywords = ""
            Resource.SortOrder = Tab.SortOrder
            Resource.IsEnabled = Tab.IsEnabled
            Resource.DisplayStartDate = Tab.DisplayStartDate
            Resource.DisplayEndDate = Tab.DisplayEndDate

            Dim Content As New Content01
            If Tab.TabID > 0 Then Content = Content01.GetContent("ContentTabs", Tab.TabID.ToString)
            Content.StatusID = 20
            Content.ModuleKey = "ContentTabs"
            Content.ForeignKey = Tab.TabID
            Content.Version = ""
            Content.Content = Tab.Content
            Content.UpdatedBy = Session("EzEditUserName")
            Content.UpdatedDate = Date.Now

            If Tab.TabID > 0 Then
                If Resources.Resource.UpdateResource(Resource) Then

                    Content01.UpdateContent(Content)
                    If NewCategoryID <> OldCategoryID Then Me.ResetSortOrder(OldCategoryID)
                    lblAlert.Text = "The tab has been updated successfully."
                    Result = True
                End If
                'End If

            Else
                Tab.ResourceID = Emagine.GetUniqueID
                Content.CreatedBy = Session("EzEditUserName")
                Resource.CreatedBy = Session("EzEditUserName")
                Tab.TabID = ContentTab.Add(Tab)

                If Tab.TabID > 0 Then
                    Content.ForeignKey = Tab.TabID
                    Content01.AddContent(Content)
                    Resource.ResourceID = Tab.ResourceID
                    If Resources.Resource.AddResource(Resource) Then
                        lblAlert.Text = "The tab has been added successfully."
                        Result = True
                    End If
                End If
            End If

            If Result Then
                'gdvItems.DataSource = Emagine.GetDataTable("SELECT DISTINCT TabID, ResourceName, SortOrder, IsEnabled FROM qryContentTabs WHERE CategoryID = " & hdnCategoryID.Value & " ORDER BY SortOrder")
                gdvItems.DataBind()

                pnlEditItem.Visible = False
                pnlItemList.Visible = True
            Else
                lblAlert.Text = "An error occurred"
            End If

        End If

    End Sub

    Protected Sub btnAddItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddItem.Click
        Me.AddItem()
    End Sub


    Sub UpdateSortOrder(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ddlSortOrder As DropDownList = sender
        Dim Row As GridViewRow = ddlSortOrder.Parent.Parent
        Dim hdnItemID As HiddenField = Row.FindControl("hdnItemID")
        Dim hdnSortOrder As HiddenField = Row.FindControl("hdnSortOrder")
        Dim ItemID As Integer = CInt(hdnItemID.Value)
        Dim OldSortOrder As Integer = CInt(hdnSortOrder.Value)
        Dim NewSortOrder As Integer = CInt(ddlSortOrder.SelectedValue)
        Dim Sql As String = ""

        If OldSortOrder > NewSortOrder Then
            'Sql = "UPDATE ContentTabs SET SortOrder = SortOrder + 1 WHERE CategoryID = " & hdnCategoryID.Value & " AND TabID <> " & ItemID & " AND SortOrder >= " & NewSortOrder & " AND SortOrder < " & OldSortOrder
            Sql = "UPDATE Resources SET SortOrder = SortOrder + 1 WHERE ResourceID IN (SELECT ResourceID FROM ContentTabs WHERE CategoryID = " & hdnCategoryID.Value & " AND TabID <> " & ItemID & ") AND SortOrder >= " & NewSortOrder & " AND SortOrder < " & OldSortOrder
        Else
            'Sql = "UPDATE ContentTabs SET SortOrder = SortOrder - 1 WHERE CategoryID = " & hdnCategoryID.Value & " AND  TabID <> " & ItemID & " AND SortOrder <= " & NewSortOrder & " AND SortOrder > " & OldSortOrder
            Sql = "UPDATE Resources SET SortOrder = SortOrder - 1 WHERE ResourceID IN (SELECT ResourceID FROM ContentTabs WHERE CategoryID = " & hdnCategoryID.Value & " AND TabID <> " & ItemID & ") AND SortOrder <= " & NewSortOrder & " AND SortOrder > " & OldSortOrder
        End If

        If Emagine.ExecuteSQL(Sql) Then

            Emagine.ExecuteSQL("UPDATE Resources SET SortOrder = " & NewSortOrder & " WHERE ResourceID IN (SELECT ResourceID FROM ContentTabs WHERE TabID = " & ItemID & ")")

            Me.ResetSortOrder(hdnCategoryID.Value)

            'gdvItems.DataSource = Emagine.GetDataTable("SELECT DISTINCT TabID, ResourceName, SortOrder, IsEnabled FROM qryContentTabs WHERE CategoryID = " & hdnCategoryID.Value & " ORDER BY SortOrder")
            gdvItems.DataBind()

            lblAlert.Text = "The items have been sorted successfully."
        Else
            lblAlert.Text = "An error occurred while attempting to sort these items."
        End If
    End Sub

    Sub ResetSortOrder(ByVal intCategoryID As Integer)
        Dim TabID As Integer = 0

        Dim DataTable As DataTable = Emagine.GetDataTable("SELECT DISTINCT ResourceID, SortOrder FROM qryContentTabs WHERE CategoryID = " & intCategoryID & " ORDER BY SortOrder")
        For i As Integer = 0 To DataTable.Rows.Count - 1
            Dim Sql As String = "UPDATE Resources SET SortOrder = " & (i + 1) & " WHERE ResourceID = '" & DataTable.Rows(i).Item("ResourceID") & "'"
            Emagine.ExecuteSQL(Sql)
        Next
    End Sub


    Protected Sub ddlCategoryID_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCategoryID.Load
        If Not IsPostBack Then
            ddlCategoryID.DataSource = Emagine.GetDataTable("SELECT CategoryID, CategoryName FROM ModuleCategories WHERE ModuleKey = 'ContentTabs'")
            ddlCategoryID.DataTextField = "CategoryName"
            ddlCategoryID.DataValueField = "CategoryID"
            ddlCategoryID.DataBind()
        End If
    End Sub

    Sub ToggleIsEnabled(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cbxIsEnabled As CheckBox = sender
        Dim Row As GridViewRow = cbxIsEnabled.Parent.Parent
        Dim hdnItemID As HiddenField = Row.FindControl("hdnItemID")
        Dim ItemID As Integer = CInt(hdnItemID.Value)

        Dim MyTab As New ContentTab
        MyTab = ContentTab.GetTab(ItemID)
        Dim MyResource As Resources.Resource = Resources.Resource.GetResource(MyTab.ResourceID)

        MyResource.IsEnabled = cbxIsEnabled.Checked
        MyResource.UpdatedBy = Session("EzEditUsername")
        MyResource.UpdatedDate = Date.Now()
        If Resources.Resource.UpdateResource(MyResource) Then
            If cbxIsEnabled.Checked Then
                lblAlert.Text = "The tab has been enabled successfully."
            Else
                lblAlert.Text = "The tab has been disabled successfully."
            End If

            'gdvItems.DataSource = Emagine.GetDataTable("SELECT DISTINCT TabID, ResourceName, SortOrder, IsEnabled FROM qryContentTabs WHERE CategoryID = " & hdnCategoryID.Value & " ORDER BY SortOrder")
            gdvItems.DataBind()
        Else
            lblAlert.Text = "An error occurred."
        End If

    End Sub
End Class
