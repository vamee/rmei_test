Imports System.Data.SqlClient

Partial Class Ezedit_Admin_Templates_Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblAlert.Text = ""
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim TemplateID As Integer = DataBinder.Eval(e.Row.DataItem, "TemplateID")
            Dim DeleteButton As ImageButton = e.Row.FindControl("btnDelete")
            Dim RecordCount As Integer = Emagine.GetNumber(Emagine.GetDbValue("SELECT COUNT(PageID) FROM Pages WHERE StatusID = 20 AND TemplateID = " & TemplateID))

            DeleteButton.OnClientClick = "return confirmDelete(" & RecordCount & ");"
        End If
    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim ItemID As Integer = Button.CommandArgument

        Me.PopulateEditForm(ItemID)

        pnlItemList.Visible = False
        pnlEdit.Visible = True
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim ItemID As Integer = Button.CommandArgument
        Dim ErrorMessage As String = ""
        Dim RecordCount As Integer = Emagine.GetNumber(Emagine.GetDbValue("SELECT COUNT(PageID) FROM Pages WHERE StatusID = 20 AND TemplateID = " & ItemID))

        If RecordCount = 0 Then
            If Emagine.ExecuteSQL("DELETE FROM Templates WHERE TemplateID = " & ItemID, ErrorMessage) Then
                lblAlert.Text = "The template has been removed successfully."
                gdvList.DataBind()
            Else
                lblAlert.Text = "The following error occurred while attempting to delete this record:<br /><br />" & ErrorMessage
            End If
        Else
            lblAlert.Text = "The following error occurred while attempting to delete this record:<br /><br />"
            lblAlert.Text = lblAlert.Text & "You cannot delete this template because " & RecordCount & " pages are currently using it. Remove these pages or choose a different template for the pages before attempting to delete this template."
        End If
    End Sub

    Protected Sub ddlLanguageID_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLanguageID.Load
        If Not Page.IsPostBack Then
            ddlLanguageID.DataSource = Emagine.GetDataTable("SELECT * FROM Languages")
            ddlLanguageID.DataTextField = "LanguageName"
            ddlLanguageID.DataValueField = "LanguageID"
            ddlLanguageID.DataBind()
        End If
    End Sub

    Sub PopulateEditForm(ByVal intItemID As Integer)
        If intItemID > 0 Then
            Dim ItemData As DataTable = Emagine.GetDataTable("SELECT * FROM Templates WHERE TemplateID = " & intItemID)
            If ItemData.Rows.Count > 0 Then
                txtTemplateName.Text = ItemData.Rows(0).Item("TemplateName").ToString
                txtDescription.Text = ItemData.Rows(0).Item("Description").ToString
                txtFileName.Text = ItemData.Rows(0).Item("FileName").ToString
                cbxSupportsLibraryItems.Checked = ItemData.Rows(0).Item("SupportsLibraryItems")
                cbxIsDefault.Checked = ItemData.Rows(0).Item("IsDefault")
                ddlLanguageID.SelectedIndex = -1
                For Each Item As ListItem In ddlLanguageID.Items
                    If Item.Value = ItemData.Rows(0).Item("LanguageID") Then
                        Item.Selected = True
                        Exit For
                    End If
                Next
                hdnItemID.Value = ItemData.Rows(0).Item("TemplateID")
                btnFileName.OnClientClick = "return ShowFileDialog(""" & txtFileName.ClientID & """, window.document.forms[0]." & txtFileName.ClientID & ".value);"
            End If
        Else
            Me.ResetEditForm()
        End If
    End Sub

    Sub ResetEditForm()
        ddlLanguageID.SelectedIndex = -1
        txtTemplateName.Text = ""
        txtDescription.Text = ""
        txtFileName.Text = ""
        cbxSupportsLibraryItems.Checked = False
        cbxIsDefault.Checked = False
        hdnItemID.Value = -1
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.ResetEditForm()
        pnlItemList.Visible = True
        pnlEdit.Visible = False
    End Sub

    Protected Sub btnFileName_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFileName.Load
        'If Not Page.IsPostBack Then
        btnFileName.OnClientClick = "return ShowFileDialog(""" & txtFileName.ClientID & """, """ & txtFileName.Text & """);"
        'End If
    End Sub

    Protected Sub btnAdd1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAdd1.Click
        Me.ResetEditForm()
        hdnItemID.Value = 0

        pnlItemList.Visible = False
        pnlEdit.Visible = True
    End Sub

    Protected Sub btnAdd2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd2.Click
        Me.ResetEditForm()
        hdnItemID.Value = 0

        pnlItemList.Visible = False
        pnlEdit.Visible = True
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim FileExists As Boolean = False

        If PeterBlum.VAM.Globals.Page.IsValid Then
            FileExists = System.IO.File.Exists(Server.MapPath(txtFileName.Text))

            If FileExists Then
                Dim ItemID As Integer = hdnItemID.Value
                Dim SqlBuilder As New StringBuilder
                Dim ErrorMessage As String = ""

                Dim Command As New SqlCommand
                Command.Parameters.AddWithValue("@TemplateID", ItemID)
                Command.Parameters.AddWithValue("@LanguageID", ddlLanguageID.SelectedValue)
                Command.Parameters.AddWithValue("@TemplateName", txtTemplateName.Text)
                Command.Parameters.AddWithValue("@Description", txtDescription.Text)
                Command.Parameters.AddWithValue("@FileName", txtFileName.Text)
                Command.Parameters.AddWithValue("@SupportsLibraryItems", cbxSupportsLibraryItems.Checked)
                Command.Parameters.AddWithValue("IsDefault", cbxIsDefault.Checked)

                If ItemID > 0 Then
                    SqlBuilder.Append("UPDATE Templates SET ")
                    SqlBuilder.Append("LanguageID=@LanguageID,")
                    SqlBuilder.Append("TemplateName=@TemplateName,")
                    SqlBuilder.Append("Description=@Description,")
                    SqlBuilder.Append("FileName=@FileName,")
                    SqlBuilder.Append("SupportsLibraryItems=@SupportsLibraryItems,")
                    SqlBuilder.Append("IsDefault=@IsDefault ")
                    SqlBuilder.Append("WHERE TemplateID=@TemplateID")

                    lblAlert.Text = "The template has been updated successfully."
                Else
                    SqlBuilder.Append("INSERT INTO Templates ")
                    SqlBuilder.Append("(LanguageID,TemplateName,Description,FileName,SupportsLibraryItems,IsDefault) ")
                    SqlBuilder.Append("VALUES ")
                    SqlBuilder.Append("(@LanguageID,@TemplateName,@Description,@FileName,@SupportsLibraryItems,@IsDefault)")

                    lblAlert.Text = "The template has been added successfully."
                End If

                If Emagine.ExecuteSQL(SqlBuilder.ToString, Command, ErrorMessage) Then
                    Me.ResetEditForm()
                    gdvList.DataBind()

                    pnlItemList.Visible = True
                    pnlEdit.Visible = False
                Else
                    lblAlert.Text = "The following error occurred while attempting to save this record:<br /><br />" & ErrorMessage
                End If
            Else
                lblAlert.Text = "Sorry, this template file does not exist. Please choose template using the browse tool."
            End If
        End If
    End Sub

    
End Class
