
Partial Class Ezedit_Modules_Redirects_Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            pnlRedirects.Visible = True
            lblPageTitle.Text = "Redirects"
        End If
    End Sub

    Sub EditRedirect(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim btnEdit As ImageButton = sender
        pnlEditRedirect.Visible = True
        pnlRedirects.Visible = False

        Dim RedirectID As Integer = CInt(btnEdit.CommandArgument)
        If RedirectID > 0 Then
            Dim Sql As String = "SELECT * FROM Redirects WHERE RedirectID = " & RedirectID
            Dim DataTable As DataTable = Emagine.GetDataTable(Sql)
            If DataTable.Rows.Count > 0 Then
                hdnRedirectID.Value = RedirectID
                txtRequestedUrl.Text = DataTable.Rows(0).Item("RequestedUrl").ToString
                txtRedirectUrl.Text = DataTable.Rows(0).Item("RedirectUrl").ToString
                txtComments.Text = DataTable.Rows(0).Item("Comments").ToString
                rblIsCore.SelectedValue = Emagine.GetNumber(DataTable.Rows(0).Item("IsCore").ToString)
                If Session("EzEditLevelID") = 1 Then
                    trIsCore.Visible = True
                Else
                    trIsCore.Visible = False
                End If

                For Each Item As ListItem In ddlRedirectTypeID.Items
                    If Item.Value = CInt(DataTable.Rows(0).Item("RedirectTypeID").ToString) Then
                        Item.Selected = True
                    Else
                        Item.Selected = False
                    End If
                Next
            End If
        End If
    End Sub

    Sub AddRedirect()
        pnlEditRedirect.Visible = True
        pnlRedirects.Visible = False
        If Session("EzEditLevelID") = 1 Then
            trIsCore.Visible = True
        Else
            trIsCore.Visible = False
        End If


        hdnRedirectID.Value = 0
        ddlRedirectTypeID.SelectedIndex = 0
        txtRequestedUrl.Text = ""
        txtRedirectUrl.Text = ""
        txtComments.Text = ""
        rblIsCore.SelectedValue = 0

    End Sub

    Sub DeleteRecord(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim btnDelete As ImageButton = sender
        Dim RecordID As Integer = CInt(btnDelete.CommandArgument)

        Dim Sql As String = "DELETE FROM Redirects WHERE RedirectID = " & RecordID
        Emagine.ExecuteSQL(Sql)

        Me.ResetSortOrder()

        lblAlert.Text = "The record has been removed successfully."

        pnlRedirects.Visible = True
        pnlEditRedirect.Visible = False

        gdvRedirects.DataBind()

    End Sub

    Protected Sub gdvRedirects_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvRedirects.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim RedirectID As Integer = CInt(DataBinder.Eval(e.Row.DataItem, "RedirectID"))
            Dim SortOrder As Integer = CInt(DataBinder.Eval(e.Row.DataItem, "SortOrder"))
            Dim IsCore As Boolean = e.Row.DataItem("IsCore")

            Dim imgIsCore As HyperLink = e.Row.FindControl("imgIsCore")
            Dim hdnSortOrder As HiddenField = e.Row.FindControl("hdnSortOrder")
            Dim hdnRedirectID As HiddenField = e.Row.FindControl("hdnRedirectID")
            Dim btnEdit As ImageButton = e.Row.FindControl("btnEdit")
            Dim btnDelete As ImageButton = e.Row.FindControl("btnDelete")
            Dim ddlSortOrder As DropDownList = e.Row.FindControl("ddlSortOrder")

            If imgIsCore IsNot Nothing And IsCore Then imgIsCore.Visible = True
            If hdnSortOrder IsNot Nothing Then hdnSortOrder.Value = SortOrder
            If hdnRedirectID IsNot Nothing Then hdnRedirectID.Value = RedirectID
            If btnEdit IsNot Nothing Then
                btnEdit.CommandArgument = RedirectID.ToString
                If IsCore Then btnEdit.OnClientClick = "return confirmEdit();"
            End If

            If btnDelete IsNot Nothing Then btnDelete.CommandArgument = RedirectID.ToString
            If ddlSortOrder IsNot Nothing Then BindSortOrder(ddlSortOrder, SortOrder)
        End If
    End Sub

    Sub BindSortOrder(ByVal ddlSortOrder As DropDownList, ByVal intSortOrder As Integer)
        Dim MaxSortOrder As Integer = Emagine.GetDbValue("SELECT COUNT(*) As MaxSortOrder FROM Redirects WHERE RedirectTypeID = " & ddlRedirectType.SelectedValue)

        For i As Integer = 1 To MaxSortOrder
            Dim ListItem As New ListItem(i.ToString, i.ToString)
            If i = intSortOrder Then ListItem.Selected = True
            ddlSortOrder.Items.Add(ListItem)
        Next

    End Sub

    Protected Sub btnCancelRedirect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelRedirect.Click
        pnlRedirects.Visible = True
        pnlEditRedirect.Visible = False
    End Sub

    Protected Sub btnSaveRedirect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveRedirect.Click
        If PeterBlum.VAM.Globals.Page.IsValid Then
            Dim Result As Boolean = False
            Dim RedirectID As Integer = CInt(hdnRedirectID.Value.ToString)
            Dim SqlBuilder As New StringBuilder
            Dim Command As New System.Data.SqlClient.SqlCommand
            Dim ErrorMessage As String = ""

            If RedirectID > 0 Then
                SqlBuilder.Append("UPDATE Redirects SET ")
                SqlBuilder.Append("RedirectTypeID=@RedirectTypeID,")
                SqlBuilder.Append("RequestedUrl=@RequestedUrl,")
                SqlBuilder.Append("RedirectUrl=@RedirectUrl,")
                SqlBuilder.Append("Comments=@Comments,")
                SqlBuilder.Append("IsCore=@IsCore,")
                SqlBuilder.Append("UpdatedBy=@UpdatedBy,")
                SqlBuilder.Append("UpdatedDate=@UpdatedDate ")
                SqlBuilder.Append("WHERE RedirectID=@RedirectID")

                Command.Parameters.AddWithValue("@RedirectID", RedirectID)
                Command.Parameters.AddWithValue("@RedirectTypeID", ddlRedirectTypeID.SelectedValue)
                Command.Parameters.AddWithValue("@RequestedUrl", txtRequestedUrl.Text)
                Command.Parameters.AddWithValue("@RedirectUrl", txtRedirectUrl.Text)
                Command.Parameters.AddWithValue("@Comments", txtComments.Text)
                Command.Parameters.AddWithValue("@IsCore", rblIsCore.SelectedValue)
                Command.Parameters.AddWithValue("@UpdatedBy", Session("EzEditName"))
                Command.Parameters.AddWithValue("@UpdatedDate", Date.Now)

            Else

                SqlBuilder.Append("INSERT INTO Redirects ")
                SqlBuilder.Append("(RedirectTypeID,RequestedUrl,RedirectUrl,Comments,IsCore,SortOrder,CreatedBy,UpdatedBy)")
                SqlBuilder.Append(" VALUES ")
                SqlBuilder.Append("(@RedirectTypeID,@RequestedUrl,@RedirectUrl,@Comments,@IsCore,@SortOrder,@CreatedBy,@UpdatedBy)")

                Command.Parameters.AddWithValue("@RedirectTypeID", ddlRedirectTypeID.SelectedValue)
                Command.Parameters.AddWithValue("@RequestedUrl", txtRequestedUrl.Text)
                Command.Parameters.AddWithValue("@RedirectUrl", txtRedirectUrl.Text)
                Command.Parameters.AddWithValue("@Comments", txtComments.Text)
                Command.Parameters.AddWithValue("@IsCore", rblIsCore.SelectedValue)
                Command.Parameters.AddWithValue("@RedirectID", hdnRedirectID.Value)
                Command.Parameters.AddWithValue("@SortOrder", CInt(Emagine.GetDbValue("SELECT COUNT(*) As MaxSortOrder FROM Redirects WHERE RedirectTypeID = " & ddlRedirectType.SelectedValue)) + 1)
                Command.Parameters.AddWithValue("@CreatedBy", Session("EzEditName"))
                Command.Parameters.AddWithValue("@UpdatedBy", Session("EzEditName"))

            End If


            Try
                Result = Emagine.ExecuteSQL(SqlBuilder.ToString, Command, ErrorMessage)

            Catch ex As Exception
                ex.HelpLink = "Error inserting or updating record."
                Emagine.LogError(ex)
            End Try

            If Result = True Then
                If RedirectID > 0 Then
                    lblAlert.Text = "The record has been updated successfully."
                Else
                    lblAlert.Text = "The record has been added successfully."
                End If

                gdvRedirects.DataBind()

                pnlRedirects.Visible = True
                pnlEditRedirect.Visible = False

            Else
                lblAlert.Text = "An error occurred: " & ErrorMessage
            End If

        End If
    End Sub

    Protected Sub btnAddRedirect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddRedirect.Click
        Me.AddRedirect()
    End Sub

    Protected Sub btnAddRedirect2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddRedirect2.Click
        Me.AddRedirect()
    End Sub


    Sub UpdateSortOrder(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ddlSortOrder As DropDownList = sender
        Dim Row As GridViewRow = ddlSortOrder.Parent.Parent
        Dim hdnRedirectID As HiddenField = Row.FindControl("hdnRedirectID")
        Dim hdnSortOrder As HiddenField = Row.FindControl("hdnSortOrder")
        Dim RedirectID As Integer = CInt(hdnRedirectID.Value)
        Dim OldSortOrder As Integer = CInt(hdnSortOrder.Value)
        Dim NewSortOrder As Integer = CInt(ddlSortOrder.SelectedValue)
        Dim Sql As String = ""

        If OldSortOrder > NewSortOrder Then
            Sql = "UPDATE Redirects SET SortOrder = SortOrder + 1 WHERE RedirectTypeID = " & ddlRedirectType.SelectedValue & " AND RedirectID <> " & RedirectID & " AND SortOrder >= " & NewSortOrder & " AND SortOrder < " & OldSortOrder
        Else
            Sql = "UPDATE Redirects SET SortOrder = SortOrder - 1 WHERE RedirectTypeID = " & ddlRedirectType.SelectedValue & " AND  RedirectID <> " & RedirectID & " AND SortOrder <= " & NewSortOrder & " AND SortOrder > " & OldSortOrder
        End If

        Emagine.ExecuteSQL(Sql)

        Emagine.ExecuteSQL("UPDATE Redirects SET SortOrder = " & NewSortOrder & " WHERE RedirectID = " & RedirectID)

        Me.ResetSortOrder()

        gdvRedirects.DataBind()

    End Sub

    Sub ResetSortOrder()
        Dim RedirectID As Integer = 0

        Dim DataTable As DataTable = Emagine.GetDataTable("SELECT * FROM Redirects WHERE RedirectTypeID = " & ddlRedirectType.SelectedValue & " ORDER BY SortOrder")
        For i As Integer = 0 To DataTable.Rows.Count - 1
            Dim Sql As String = "UPDATE Redirects SET SortOrder = " & (i + 1) & " WHERE RedirectID = " & DataTable.Rows(i).Item("RedirectID")
            Emagine.ExecuteSQL(Sql)
        Next

    End Sub

    Protected Sub ddlRedirectTypeID_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRedirectTypeID.Load
        If Not IsPostBack Then
            ddlRedirectTypeID.DataSource = Emagine.GetDataTable("SELECT * FROM RedirectTypes ORDER BY RedirectTypeID")
            ddlRedirectTypeID.DataValueField = "RedirectTypeID"
            ddlRedirectTypeID.DataTextField = "RedirectType"
            ddlRedirectTypeID.DataBind()
        End If
    End Sub

    Protected Sub cbxIsEnabled_CheckChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cbxIsEnabled As CheckBox = sender
        Dim btnEdit As ImageButton = cbxIsEnabled.Parent.Parent.FindControl("btnEdit")
        Dim ItemID As Integer = Emagine.GetNumber(btnEdit.CommandArgument)
        Dim EnabledText As String = "enabled"
        Dim ErrorMessage As String = ""

        If Not cbxIsEnabled.Checked Then EnabledText = "disabled"

        If ItemID > 0 Then
            If Emagine.ExecuteSQL("UPDATE Redirects SET IsEnabled = '" & cbxIsEnabled.Checked & "' WHERE RedirectID = " & ItemID, ErrorMessage) Then
                lblAlert.Text = "The redirect has been " & EnabledText & " successfully."
                gdvRedirects.DataBind()
            Else
                lblAlert.Text = "An error occurred: " & ErrorMessage
            End If
        End If
    End Sub
End Class
