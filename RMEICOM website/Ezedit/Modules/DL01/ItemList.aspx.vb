Imports System.Data.SqlClient

Partial Class Ezedit_Modules_DL01_Default
    Inherits System.Web.UI.Page

    Dim _CategoryID As Integer = 0
    Dim _ModuleKey As String = "DL01"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request("CategoryID") IsNot Nothing Then _CategoryID = Emagine.GetNumber(Request("CategoryID"))
        If _CategoryID = 0 Then Response.Redirect("/EzEdit/modules/Default.aspx?ModuleKey=" & _ModuleKey)

        If Session("Alert") IsNot Nothing Then
            lblAlert.Text = Session("Alert")
            Session("Alert") = ""
        Else
            lblAlert.Text = ""
        End If

    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim ItemID As Integer = Button.CommandArgument

        Response.Redirect("EditItem.aspx?ItemID=" & ItemID)
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim ItemID As Integer = Button.CommandArgument
        If ItemID > 0 Then
            Dim Sql As String = "sp_DL01_DeleteDownload"
            Dim Command As New SqlCommand
            Command.CommandType = CommandType.StoredProcedure
            Command.Parameters.AddWithValue("@DownloadID", ItemID)

            If Emagine.ExecuteSQL(Sql, Command) Then
                Me.ResetSortOrder()
                gdvList.DataBind()
                lblAlert.Text = "The item has been removed successfully."
            Else
                lblAlert.Text = "An error occurred while attempting to delete this item."
            End If
        End If

    End Sub

    Protected Sub lbtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnAdd.Click
        Response.Redirect("EditItem.aspx?CategoryID=" & Request("CategoryID"))
    End Sub

    Protected Sub ibtnAdd_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnAdd.Click
        Response.Redirect("EditItem.aspx?CategoryID=" & Request("CategoryID"))
    End Sub

    Protected Sub cbxRegistrationRequired_CheckChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cbxRegistrationRequired As CheckBox = sender
        Dim btnEdit As ImageButton = cbxRegistrationRequired.Parent.Parent.FindControl("btnEdit")
        Dim ItemID As Integer = btnEdit.CommandArgument

        If ItemID > 0 Then
            Dim MyDownload As DL01 = DL01.GetDownload(ItemID)
            If Emagine.ExecuteSQL("UPDATE Resources SET IsEnabled = '" & MyDownload.IsEnabled & "' WHERE ResourceID = '" & MyDownload.ResourceID & "'") Then
                lblAlert.Text = "The item has been updated successfully."
            Else
                lblAlert.Text = "An error occurred."
            End If
        End If
    End Sub

    Protected Sub cbxIsEnabled_CheckChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cbxIsEnabled As CheckBox = sender
        Dim btnEdit As ImageButton = cbxIsEnabled.Parent.Parent.FindControl("btnEdit")
        Dim ItemID As Integer = Emagine.GetNumber(btnEdit.CommandArgument)
        Dim EnabledText As String = "enabled"

        If Not cbxIsEnabled.Checked Then EnabledText = "disabled"

        If ItemID > 0 Then
            Dim MyDownload As DL01 = DL01.GetDownload(ItemID)
            'MyDownload.IsEnabled = cbxIsEnabled.Checked
            Dim MyResource As Resources.Resource = Resources.Resource.GetResource(MyDownload.ResourceID)
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

    Protected Sub gdvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ItemID As Integer = e.Row.DataItem("DownloadID")
            Dim ModuleType As String = e.Row.DataItem("ModuleType")
            Dim FileName As String = e.Row.DataItem("FileName")
            Dim FileSize As Integer = e.Row.DataItem("FileSize")
            Dim ExternalUrl As String = e.Row.DataItem("ExternalUrl")
            Dim SortOrder As Integer = e.Row.DataItem("SortOrder")
            Dim DisplayStartDate As Date = e.Row.DataItem("DisplayStartDate").ToString
            Dim DisplayEndDate As Date = e.Row.DataItem("DisplayEndDate").ToString
            Dim IsEnabled As Boolean = e.Row.DataItem("IsEnabled")

            Dim lblStatus As Label = e.Row.FindControl("lblStatus")
            Dim hypModuleType As HyperLink = e.Row.FindControl("hypModuleType")
            Dim hdnSortOrder As HiddenField = e.Row.FindControl("hdnSortOrder")
            Dim ddlSortOrder As DropDownList = e.Row.FindControl("ddlSortOrder")
            Dim hypExternalUrl As HyperLink = e.Row.FindControl("hypExternalUrl")
            Dim hypFileName As HyperLink = e.Row.FindControl("hypFileName")
            Dim lblFileSize As Label = e.Row.FindControl("lblFileSize")

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

            Select Case ModuleType
                Case "External Link"
                    hypModuleType.ImageUrl = "/App_Themes/EzEdit/images/world_go.png"
                    hypModuleType.ToolTip = ModuleType & ": " & ExternalUrl
                    hypModuleType.Target = "_blank"
                    hypModuleType.NavigateUrl = ExternalUrl
                Case "File Download"
                    hypModuleType.ImageUrl = "/App_Themes/EzEdit/images/disk.png"
                    hypModuleType.ToolTip = ModuleType & ": " & Emagine.FormatFileName(FileName)
                    hypModuleType.Target = "_blank"
                    hypModuleType.NavigateUrl = FileName.Replace("~", "")
            End Select

            If FileSize = 0 Then lblFileSize.Text = "N/A"
            If hdnSortOrder IsNot Nothing Then hdnSortOrder.Value = SortOrder
            If ddlSortOrder IsNot Nothing Then BindSortOrder(ddlSortOrder, SortOrder)
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
End Class
