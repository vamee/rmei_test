Imports System.Data.SqlClient

Partial Class Ezedit_Modules_Programs_ItemList
    Inherits System.Web.UI.Page

    Dim _CategoryID As Integer = 0
    Dim _ModuleKey As String = "Programs"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Request("CategoryID") IsNot Nothing Then _CategoryID = Emagine.GetNumber(Request("CategoryID"))
        If _CategoryID = 0 Then Response.Redirect("Default.aspx")

        If Session("Alert") IsNot Nothing Then
            lblAlert.Text = Session("Alert")
            Session("Alert") = ""
        Else
            lblAlert.Text = ""
        End If

        If Not Page.IsPostBack Then
            Dim OrderBy As String = ""
            Dim OrderByDirection As String = "ASC"
            Dim PageIndex As Integer = 0
            Dim IsArchived As String = ""

            If Request.QueryString("OrderBy") IsNot Nothing Then OrderBy = Request.QueryString("OrderBy").ToString
            If Request.QueryString("OrderByDirection") IsNot Nothing Then
                Dim Temp As String = Request.QueryString("OrderByDirection").ToString

                Select Case Temp
                    Case "Descending"
                        OrderByDirection = "DESC"
                    Case Else
                        OrderByDirection = "ASC"
                End Select


            End If

            If Request.QueryString("PageIndex") IsNot Nothing Then PageIndex = Emagine.GetNumber(Request.QueryString("PageIndex").ToString)
            If Request.QueryString("IsArchived") IsNot Nothing Then IsArchived = Request.QueryString("IsArchived").ToString

            Dim Sql As String = "SELECT * FROM qryCustom_Programs WHERE (ModuleCategoryID = @ModuleCategoryID) "
            If IsArchived.Length > 0 Then Sql = Sql & "AND IsArchived = '" & IsArchived & "' "
            If OrderBy.Length > 0 Then
                Sql = Sql & "ORDER BY " & OrderBy & " " & OrderByDirection
            Else
                Sql = Sql & "ORDER BY ResourceName"
            End If


            dataPrograms.SelectCommand = Sql

            gdvList.PageIndex = PageIndex
            gdvList.DataBind()

            For Each Item As ListItem In ddlArchive.Items
                If Item.Value = IsArchived Then
                    Item.Selected = True
                    Exit For
                End If
            Next

        Else
            Select Case ddlArchive.SelectedIndex
                Case 0
                    dataPrograms.SelectCommand = "SELECT * FROM qryCustom_Programs WHERE (ModuleCategoryID = @ModuleCategoryID) ORDER BY ResourceName"

                Case 1
                    dataPrograms.SelectCommand = "SELECT * FROM qryCustom_Programs WHERE (ModuleCategoryID = @ModuleCategoryID) AND IsArchived = 'True' ORDER BY ResourceName"

                Case 2
                    dataPrograms.SelectCommand = "SELECT * FROM qryCustom_Programs WHERE (ModuleCategoryID = @ModuleCategoryID) AND IsArchived = 'False' ORDER BY ResourceName"
            End Select
            gdvList.DataBind()
        End If
    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim Button As ImageButton = sender
        Dim ItemID As String = Button.CommandArgument
        Dim OrderBy As String = gdvList.SortExpression
        Dim OrderByDirection As String = gdvList.SortDirection.ToString
        Dim PageIndex As Integer = gdvList.PageIndex
        Dim IsArchived As String = ddlArchive.SelectedValue

        Response.Redirect("EditItem.aspx?CategoryID=" & _CategoryID & "&ProgramID=" & ItemID & "&OrderBy=" & OrderBy & "&OrderByDirection=" & OrderByDirection & "&PageIndex=" & PageIndex & "&IsArchived=" & IsArchived)
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim ItemID As String = Button.CommandArgument
        Dim Result As Boolean = False
        Dim ErrorMessage As String = ""

        If ItemID.Length > 0 Then
            Dim MyCommand As New SqlCommand
            MyCommand.Parameters.AddWithValue("@ProgramID", ItemID)

            Dim Sql As String = "DELETE FROM Resources WHERE ResourceID IN (SELECT ResourceID FROM Custom_Programs WHERE ProgramID = @ProgramID)"
            Result = Emagine.ExecuteSQL(Sql, MyCommand, ErrorMessage)

            If Result Then
                Sql = "DELETE FROM Custom_Programs WHERE ProgramID = @ProgramID"
                Result = Emagine.ExecuteSQL(Sql, MyCommand, ErrorMessage)
            End If

            If Result Then
                lblAlert.Text = "The program has been removed successfully."
                gdvList.DataBind()
            Else
                lblAlert.Text = "An error occurred: " & ErrorMessage
            End If

        End If

    End Sub

    Protected Sub lbtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnAdd.Click
        Response.Redirect("EditItem.aspx?CategoryID=" & Request("CategoryID"))
    End Sub

    Protected Sub ibtnAdd_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnAdd.Click
        Response.Redirect("EditItem.aspx?CategoryID=" & Request("CategoryID"))
    End Sub

    Protected Sub cbxIsArchived_CheckChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cbxIsArchived As CheckBox = sender
        Dim btnEdit As ImageButton = cbxIsArchived.Parent.Parent.FindControl("btnEdit")
        Dim ItemID As String = btnEdit.CommandArgument
        Dim EnabledText As String = "archived"
        Dim ErrorMessage As String = ""

        If Not cbxIsArchived.Checked Then EnabledText = "disabled"

        If ItemID.Length > 0 Then
            Dim MyCommand As New SqlCommand
            MyCommand.Parameters.AddWithValue("@ProgramID", ItemID)
            MyCommand.Parameters.AddWithValue("@IsArchived", cbxIsArchived.Checked)

            Dim Sql = "UPDATE Custom_Programs SET IsArchived = @IsArchived WHERE ProgramID = @ProgramID"

            If Emagine.ExecuteSQL(Sql, MyCommand, ErrorMessage) Then
                If cbxIsArchived.Checked Then
                    lblAlert.Text = "The program has been archived successfully."
                Else
                    lblAlert.Text = "The program has been removed from the archive successfully."
                End If
                gdvList.DataBind()
            Else
                lblAlert.Text = "An error occurred: " & ErrorMessage
            End If
        End If
    End Sub

    Protected Sub cbxIsEnabled_CheckChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cbxIsEnabled As CheckBox = sender
        Dim btnEdit As ImageButton = cbxIsEnabled.Parent.Parent.FindControl("btnEdit")
        Dim ItemID As String = btnEdit.CommandArgument
        Dim EnabledText As String = "enabled"
        Dim ErrorMessage As String = ""

        If Not cbxIsEnabled.Checked Then EnabledText = "disabled"

        If ItemID.Length > 0 Then
            Dim MyCommand As New SqlCommand
            MyCommand.Parameters.AddWithValue("@ProgramID", ItemID)

            Dim ResourceID As String = Emagine.GetDbValue("SELECT ResourceID FROM Custom_Programs WHERE ProgramID = @ProgramID", MyCommand, ErrorMessage)
            Dim MyResource As Resources.Resource = Resources.Resource.GetResource(ResourceID)
            MyResource.IsEnabled = cbxIsEnabled.Checked
            MyResource.UpdatedDate = Date.Now
            MyResource.UpdatedBy = Session("EzEditUsername")
            If Resources.Resource.UpdateResource(MyResource) Then
                lblAlert.Text = "The item has been " & EnabledText & " successfully."
                gdvList.DataBind()
            Else
                lblAlert.Text = "An error occurred: " & ErrorMessage
            End If
        End If
    End Sub

    Protected Sub gdvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ItemID As String = e.Row.DataItem("ProgramID")
            Dim IsEnabled As Boolean = e.Row.DataItem("IsEnabled")

            Dim lblStatus As Label = e.Row.FindControl("lblStatus")

            If Not IsEnabled Then
                lblStatus.Text = "DISABLED"
                lblStatus.BackColor = Drawing.Color.Red
                lblStatus.ForeColor = Drawing.Color.White
                lblStatus.ToolTip = "Check the 'Enabled' box to enable this item."

            Else
                lblStatus.Text = "CURRENT"
                lblStatus.BackColor = Drawing.Color.Green
                lblStatus.ForeColor = Drawing.Color.White
            End If
        End If
    End Sub

    Sub BindSortOrder(ByVal ddlSortOrder As DropDownList, ByVal intSortOrder As Integer)
        Dim MaxSortOrder As Integer = Emagine.GetDbValue("SELECT COUNT(*) As MaxSortOrder FROM Downloads WHERE CategoryID = " & _CategoryID)

        For i As Integer = 1 To MaxSortOrder
            Dim ListItem As New ListItem(i.ToString, i.ToString)
            If i = intSortOrder Then ListItem.Selected = True
            ddlSortOrder.Items.Add(ListItem)
        Next
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
            Sql = "UPDATE Downloads SET SortOrder = SortOrder + 1 WHERE CategoryID = " & _CategoryID & " AND DownloadID <> " & ItemID & " AND SortOrder >= " & NewSortOrder & " AND SortOrder < " & OldSortOrder
        Else
            Sql = "UPDATE Downloads SET SortOrder = SortOrder - 1 WHERE CategoryID = " & _CategoryID & " AND  DownloadID <> " & ItemID & " AND SortOrder <= " & NewSortOrder & " AND SortOrder > " & OldSortOrder
        End If

        Emagine.ExecuteSQL(Sql)

        Emagine.ExecuteSQL("UPDATE Downloads SET SortOrder = " & NewSortOrder & " WHERE DownloadID = " & ItemID)

        Me.ResetSortOrder()

        gdvList.DataBind()
    End Sub

    Sub ResetSortOrder()
        Dim RedirectID As Integer = 0

        Dim DataTable As DataTable = Emagine.GetDataTable("SELECT * FROM Downloads WHERE CategoryID = " & _CategoryID & " ORDER BY SortOrder")
        For i As Integer = 0 To DataTable.Rows.Count - 1
            Dim Sql As String = "UPDATE Downloads SET SortOrder = " & (i + 1) & " WHERE DownloadID = " & DataTable.Rows(i).Item("DownloadID")
            Emagine.ExecuteSQL(Sql)
        Next
    End Sub

    Protected Sub lblPageTitle_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblPageTitle.Load
        If Not Page.IsPostBack Then
            Dim CategoryName As String = Emagine.GetDbValue("SELECT CategoryName FROM ModuleCategories WHERE CategoryID = " & _CategoryID)
            lblPageTitle.Text = " > " & CategoryName
        End If
    End Sub

    Protected Sub ddlArchive_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlArchive.SelectedIndexChanged

    End Sub

    Protected Sub gdvList_Sorted(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvList.Sorted
        hdnOrderBy.Value = gdvList.SortExpression
        hdnOrderByDirection.Value = gdvList.SortDirection
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Select Case ddlArchive.SelectedIndex
            Case 0
                dataPrograms.SelectCommand = "SELECT * FROM qryCustom_Programs WHERE (ModuleCategoryID = @ModuleCategoryID) ORDER BY ResourceName"

            Case 1
                dataPrograms.SelectCommand = "SELECT * FROM qryCustom_Programs WHERE (ModuleCategoryID = @ModuleCategoryID) AND IsArchived = 'True' ORDER BY ResourceName"

            Case 2
                dataPrograms.SelectCommand = "SELECT * FROM qryCustom_Programs WHERE (ModuleCategoryID = @ModuleCategoryID) AND IsArchived = 'False' ORDER BY ResourceName"
        End Select
        gdvList.DataBind()
    End Sub
End Class
