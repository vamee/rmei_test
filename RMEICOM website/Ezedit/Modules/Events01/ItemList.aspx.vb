Imports System.Data.SqlClient

Partial Class Ezedit_Modules_Events01_ItemList
    Inherits System.Web.UI.Page

    Dim _CategoryID As Integer = 0

#Region "On Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _CategoryID = Emagine.GetNumber(Request("CategoryID"))

        If Not IsPostBack Then
            lblPageTitle.Text = "> " & ModuleCategory.GetModuleCategoryName(_CategoryID)
        End If

        lblAlert.Text = Session("Alert")
        Session("Alert") = ""
    End Sub
#End Region

#Region "Events"
    Sub rpEventDates_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim EventID As Integer = e.Item.DataItem("EventID")
            Dim EventDateID As Integer = e.Item.DataItem("EventDateID")
            Dim Location As String = e.Item.DataItem("Location").ToString
            Dim EventDate As String = e.Item.DataItem("EventDate").ToString

            Dim str As New StringBuilder
            str.Append("<li>")
            str.Append("<a href='EditDate.aspx?EventID=" & EventID & "&EventDateID=" & EventDateID & "' class='main'>")
            If Location.Length > 0 Then
                str.Append(Location & " [")
            End If
            str.Append(EventDate)

            If Location.Length > 0 Then
                str.Append("]")
            End If
            str.Append("</a>")
            str.Append("</li>")

            Dim lbl As Label = CType(e.Item.FindControl("lblEventDates"), Label)
            lbl.Text = str.ToString
        End If
    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim btnEdit As ImageButton = sender
        Dim ItemID As Integer = btnEdit.CommandArgument
        Response.Redirect("EditItem.aspx?CategoryID=" & _CategoryID & "&ItemID=" & ItemID)
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim btnDelete As ImageButton = sender
        Dim ItemID As Integer = btnDelete.CommandArgument

        Dim MyEvent As Events01 = Events01.GetEvent(ItemID)

        If Events01.DeleteEvent(ItemID) Then
            Events01.ResetSortOrder(_CategoryID)
            lblAlert.Text = "The event has been removed successfully."
            gdvList.DataBind()
        Else
            lblAlert.Text = "An error occurred."
        End If
    End Sub

    Protected Sub gdvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ItemID As Integer = e.Row.DataItem("EventID")
            Dim CategoryID As Integer = e.Row.DataItem("CategoryID")
            Dim SortOrder As Integer = e.Row.DataItem("SortOrder")
            Dim IsEnabled As Boolean = e.Row.DataItem("IsEnabled")

            Dim hdnSortOrder As HiddenField = e.Row.FindControl("hdnSortOrder")
            Dim ddlSortOrder As DropDownList = e.Row.FindControl("ddlSortOrder")
            Dim btnEdit As ImageButton = e.Row.FindControl("btnEdit")
            Dim btnDelete As ImageButton = e.Row.FindControl("btnDelete")
            Dim lblStatus As Label = e.Row.FindControl("lblStatus")

            If IsEnabled Then
                lblStatus.Text = "CURRENT"
                lblStatus.BackColor = Drawing.Color.Green
                lblStatus.ForeColor = Drawing.Color.White
                lblStatus.ToolTip = "No Expiration"
            Else
                lblStatus.Text = "DISABLED"
                lblStatus.BackColor = Drawing.Color.Red
                lblStatus.ForeColor = Drawing.Color.White
                lblStatus.ToolTip = "Check the 'Enabled' box to enable this item."
            End If

            Dim MyRepeater As System.Web.UI.WebControls.Repeater
            MyRepeater = e.Row.FindControl("rpEventDates")
            MyRepeater.DataSource = EventDates.GetEventDates(ItemID)
            MyRepeater.DataBind()

            If hdnSortOrder IsNot Nothing Then hdnSortOrder.Value = SortOrder
            If ddlSortOrder IsNot Nothing Then BindSortOrder(ddlSortOrder, SortOrder)
        End If
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
            Sql = "UPDATE Events SET SortOrder = SortOrder + 1 WHERE CategoryID = " & _CategoryID & " AND EventID <> " & ItemID & " AND SortOrder >= " & NewSortOrder & " AND SortOrder < " & OldSortOrder
        Else
            Sql = "UPDATE Events SET SortOrder = SortOrder - 1 WHERE CategoryID = " & _CategoryID & " AND EventID <> " & ItemID & " AND SortOrder <= " & NewSortOrder & " AND SortOrder > " & OldSortOrder
        End If

        Emagine.ExecuteSQL(Sql)

        Emagine.ExecuteSQL("UPDATE Events SET SortOrder = " & NewSortOrder & " WHERE EventID = " & ItemID)

        Me.ResetSortOrder()

        gdvList.DataBind()
    End Sub

    Protected Sub cbxIsEnabled_CheckChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cbxIsEnabled As CheckBox = sender
        Dim btnEdit As ImageButton = cbxIsEnabled.Parent.Parent.FindControl("btnEdit")
        Dim ItemID As Integer = Emagine.GetNumber(btnEdit.CommandArgument)
        Dim EnabledText As String = "enabled"

        If Not cbxIsEnabled.Checked Then EnabledText = "disabled"

        If ItemID > 0 Then
            Dim MyCommand As New SqlCommand
            MyCommand.Parameters.AddWithValue("@IsEnabled", cbxIsEnabled.Checked)
            MyCommand.Parameters.AddWithValue("@EventID", ItemID)

            Dim Sql As String = "UPDATE Resources SET IsEnabled = @IsEnabled WHERE ResourceID IN (SELECT ResourceID FROM Events WHERE EventID = @EventID)"
            Dim ErrorMessage As String = ""

            If Emagine.ExecuteSQL(Sql, MyCommand, ErrorMessage) Then
                lblAlert.Text = "The event has been " & EnabledText & " successfully."
                gdvList.DataBind()
            Else
                lblAlert.Text = "An error occurred: " & ErrorMessage
            End If
        End If
    End Sub
#End Region

#Region "Custom Procedures"
    Sub BindSortOrder(ByVal ddlSortOrder As DropDownList, ByVal intSortOrder As Integer)
        Dim MaxSortOrder As Integer = Emagine.GetDbValue("SELECT COUNT(*) AS RecordCount FROM Events WHERE CategoryID = " & _CategoryID)

        For i As Integer = 1 To MaxSortOrder
            Dim ListItem As New ListItem(i.ToString, i.ToString)
            If i = intSortOrder Then ListItem.Selected = True
            ddlSortOrder.Items.Add(ListItem)
        Next
    End Sub

    Sub ResetSortOrder()
        Dim MyEventData As DataTable = Emagine.GetDataTable("SELECT * FROM Events WHERE CategoryID = " & _CategoryID & " ORDER BY SortOrder")
        For i As Integer = 0 To MyEventData.Rows.Count - 1
            Dim Sql As String = "UPDATE Events SET SortOrder = " & (i + 1) & " WHERE EventID = " & MyEventData.Rows(i).Item("EventID")
            Emagine.ExecuteSQL(Sql)
        Next
    End Sub
#End Region

End Class
