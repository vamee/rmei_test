Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI.WebControls

Partial Class Ezedit_Admin_Languages_Default
    Inherits System.Web.UI.Page

    Dim intMaxDomainRows As Integer = 10

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        DisplayDomainRows()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack Then
            Dim FoundMatch As Boolean = False
            For i As Integer = 0 To Request.Form.Keys.Count - 1
                If Request.Form.Keys(i).IndexOf("btnSave") > -1 Then
                    FoundMatch = True
                    Exit For
                End If
            Next

            If FoundMatch Then

            End If
        End If

        lblAlert.Text = ""
    End Sub

    Sub DisplayDomainRows()
        Dim RowStyle As String = "table-row"

        For i As Integer = 0 To (intMaxDomainRows - 1)

            Dim Cell1 As New TableCell
            Cell1.BorderWidth = 1
            Dim hdnDomainID As New HiddenField
            hdnDomainID.ID = "hdnDomainID" & i
            hdnDomainID.Value = 0
            Dim txtDomainName As New TextBox
            txtDomainName.ID = "txtDomainName" & i
            txtDomainName.Width = 250
            Cell1.Controls.Add(txtDomainName)
            Cell1.Controls.Add(hdnDomainID)

            Dim Cell2 As New TableCell
            Cell2.BorderWidth = 1
            Cell2.HorizontalAlign = HorizontalAlign.Center
            Dim cbxIsEnabled As New CheckBox
            cbxIsEnabled.ID = "cbxIsEnabled" & i
            Cell2.Controls.Add(cbxIsEnabled)

            Dim Row As New TableRow
            If RowStyle = "table-altrow" Then
                RowStyle = "table-row"

            Else
                RowStyle = "table-altrow"
            End If
            Row.CssClass = RowStyle
            Row.Cells.Add(Cell1)
            Row.Cells.Add(Cell2)

            tblDomains.Rows.Add(Row)
        Next
    End Sub

    Sub PopulateDomains(ByVal intLanguageID As Integer)
        Dim DomainData As DataTable = Emagine.GetDataTable("SELECT * FROM Domains WHERE LanguageID = " & intLanguageID)

        For i As Integer = 0 To (DomainData.Rows.Count - 1)
            Dim hdnDomainID As HiddenField = tblDomains.Rows(i).Cells(0).FindControl("hdnDomainID" & i)
            Dim txtDomainName As TextBox = tblDomains.Rows(i).Cells(0).FindControl("txtDomainName" & i)
            Dim cbxIsEnabled As CheckBox = tblDomains.Rows(i).Cells(0).FindControl("cbxIsEnabled" & i)

            hdnDomainID.Value = DomainData.Rows(i).Item("DomainID")
            txtDomainName.Text = DomainData.Rows(i).Item("DomainName")
            cbxIsEnabled.Checked = DomainData.Rows(i).Item("IsEnabled")
        Next

        DomainData.Dispose()
        
    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim LanguageID As Integer = Button.CommandArgument
        Me.EditItem(LanguageID, 0)
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim LanguageID As Integer = Button.CommandArgument
        Dim IsDefault As Boolean = Emagine.GetDbValue("SELECT DefaultLanguage FROM Languages WHERE LanguageID = " & LanguageID)

        If Not IsDefault Then
            If Emagine.ExecuteSQL("DELETE FROM Languages WHERE LanguageID = " & LanguageID) Then
                gdvList.DataBind()
                lblAlert.Text = "The language has been removed successfully."
            Else
                lblAlert.Text = "An error occurred while attempting to delete the language."
            End If
        Else
            lblAlert.Text = "You cannot delete the default language."
        End If
    End Sub

    Protected Sub btnCopy_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim LanguageID As Integer = Button.CommandArgument
        Me.EditItem(0, LanguageID)
    End Sub

    Protected Sub ibtnAddNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnAddNew.Click
        Me.EditItem(0, 0)
    End Sub

    Protected Sub lbtnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnAddNew.Click
        Me.EditItem(0, 0)
    End Sub

    Sub EditItem(ByVal intLanguageID As Integer, ByVal intCopyLanguageID As Integer)
        If intLanguageID > 0 Then
            Dim LanguageData As DataTable = Emagine.GetDataTable("SELECT * FROM Languages WHERE LanguageID = " & intLanguageID)
            If LanguageData.Rows.Count > 0 Then
                txtLanguageName.Text = LanguageData.Rows(0).Item("LanguageName")

                hdnLanguageID.Value = intLanguageID
                hdnCopyLanguageID.Value = intCopyLanguageID

                pnlList.Visible = False
                pnlEdit.Visible = True

                Me.PopulateDomains(intLanguageID)
            End If


        ElseIf intCopyLanguageID > 0 Then
            Dim LanguageData As DataTable = Emagine.GetDataTable("SELECT * FROM Languages WHERE LanguageID = " & intCopyLanguageID)
            If LanguageData.Rows.Count > 0 Then
                txtLanguageName.Text = LanguageData.Rows(0).Item("LanguageName")

                hdnLanguageID.Value = intLanguageID
                hdnCopyLanguageID.Value = intCopyLanguageID

                pnlList.Visible = False
                pnlEdit.Visible = True
            End If

        Else
            hdnLanguageID.Value = 0
            hdnCopyLanguageID.Value = 0
            txtLanguageName.Text = ""

            pnlList.Visible = False
            pnlEdit.Visible = True
        End If

        txtLanguageName.Focus()
    End Sub

    Protected Sub rdoIsDefault_CheckChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim RadioButton As Vladsm.Web.UI.WebControls.GroupRadioButton = sender
        Dim Button As ImageButton = RadioButton.Parent.Parent.FindControl("btnEdit")
        Dim LanguageID As Integer = Button.CommandArgument
        Dim LanguageName As String = Emagine.GetDbValue("SELECT LanguageName FROM Languages WHERE LanguageID = " & LanguageID)

        Emagine.ExecuteSQL("UPDATE Languages SET DefaultLanguage = 'True' WHERE LanguageID = " & LanguageID)
        Emagine.ExecuteSQL("UPDATE Languages SET DefaultLanguage = 'False' WHERE LanguageID <> " & LanguageID)

        lblAlert.Text = "The default language has been successfully changed to " & LanguageName & "."
    End Sub

    Protected Sub cbxIsEnabled_CheckChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim CheckBox As CheckBox = sender
        Dim Button As ImageButton = CheckBox.Parent.Parent.FindControl("btnEdit")
        Dim LanguageID As Integer = Button.CommandArgument
        Dim LanguageName As String = Emagine.GetDbValue("SELECT LanguageName FROM Languages WHERE LanguageID = " & LanguageID)

        Emagine.ExecuteSQL("UPDATE Languages SET IsEnabled = '" & CheckBox.Checked & "' WHERE LanguageID = " & LanguageID)

        If CheckBox.Checked Then
            lblAlert.Text = LanguageName & " has been successfully enabled."
        Else
            lblAlert.Text = LanguageName & " has been successfully disabled."
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        hdnLanguageID.Value = 0
        hdnCopyLanguageID.Value = 0

        pnlList.Visible = True
        pnlEdit.Visible = False
    End Sub

    Protected Sub gdvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim LanguageID As Integer = e.Row.DataItem("LanguageID")
            Dim IsDefault As Boolean = e.Row.DataItem("DefaultLanguage")
            Dim IsEnabled As Boolean = e.Row.DataItem("IsEnabled")

            Dim lstDomains As BulletedList = e.Row.FindControl("lstDomains")
            Dim rdoIsDefault As Vladsm.Web.UI.WebControls.GroupRadioButton = e.Row.FindControl("rdoIsDefault")
            Dim cbxIsEnabled As CheckBox = e.Row.FindControl("cbxIsEnabled")
            Dim btnEdit As ImageButton = e.Row.FindControl("btnEdit")
            Dim btnCopy As ImageButton = e.Row.FindControl("btnCopy")
            Dim btnDelete As ImageButton = e.Row.FindControl("btnDelete")

            rdoIsDefault.Checked = IsDefault
            cbxIsEnabled.Checked = IsEnabled
            btnEdit.CommandArgument = LanguageID
            btnCopy.CommandArgument = LanguageID
            btnDelete.CommandArgument = LanguageID

            Dim DomainData As DataTable = Emagine.GetDataTable("SELECT * FROM Domains WHERE LanguageID = " & LanguageID)
            For i As Integer = 0 To (DomainData.Rows.Count - 1)
                lstDomains.Items.Add(New ListItem(DomainData.Rows(i).Item("DomainName")))
            Next
            DomainData.Dispose()
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If PeterBlum.VAM.Globals.Page.IsValid Then
            Dim LanguageID As Integer = hdnLanguageID.Value
            Dim CopyLanguageID As Integer = hdnCopyLanguageID.Value
            Dim SqlBuilder As New StringBuilder
            Dim Command As New System.Data.SqlClient.SqlCommand

            If LanguageID > 0 Then
                Dim OldLanguageName As String = Emagine.GetDbValue("SELECT LanguageName FROM Languages WHERE LanguageID = " & LanguageID)

                SqlBuilder.Append("UPDATE Languages SET ")
                SqlBuilder.Append("Languagename=@LanguageName ")
                SqlBuilder.Append("WHERE LanguageID=@LanguageID")

                Command.Parameters.AddWithValue("@LanguageID", LanguageID)
                Command.Parameters.AddWithValue("@LanguageName", txtLanguageName.Text)

                If Emagine.ExecuteSQL(SqlBuilder.ToString, Command) Then
                    Me.CreateFolders(OldLanguageName, txtLanguageName.Text, "MOVE")
                    Me.UpdateDomainInfo(LanguageID)

                    txtLanguageName.Text = ""
                    hdnLanguageID.Value = 0
                    hdnCopyLanguageID.Value = 0
                    Me.ClearDomainInfo()

                    gdvList.DataBind()

                    pnlList.Visible = True
                    pnlEdit.Visible = False

                    lblAlert.Text = "The language has been updated successfully."
                Else
                    lblAlert.Text = "An error occurred while updating this language."
                End If

            ElseIf CopyLanguageID > 0 Then
                If Emagine.GetDataTable("SELECT * FROM Languages WHERE LanguageName = '" & txtLanguageName.Text & "'").Rows.Count = 0 Then
                    SqlBuilder.Append("INSERT INTO Languages ")
                    SqlBuilder.Append("(LanguageName) ")
                    SqlBuilder.Append("VALUES ")
                    SqlBuilder.Append("(@LanguageName)")

                    Command.Parameters.AddWithValue("@LanguageName", txtLanguageName.Text)

                    If Emagine.ExecuteSQL(SqlBuilder.ToString, Command) Then
                        LanguageID = Emagine.GetNumber(Emagine.GetDbValue("SELECT MAX(LanguageID) AS MaxLanguageID FROM Languages"))
                        Me.UpdateDomainInfo(LanguageID)
                        Me.CopyContent(CopyLanguageID, LanguageID)
                        Me.UpdateParentPages(CopyLanguageID, LanguageID)
                        Me.UpdateContentPages(CopyLanguageID, LanguageID)
                        Me.CopyArticles(CopyLanguageID, LanguageID)
                        Me.CopyDownloads(CopyLanguageID, LanguageID)
                        Me.CopyCareers(CopyLanguageID, LanguageID)
                        Me.CopyForms(CopyLanguageID, LanguageID)
                        Me.CopyEvents(CopyLanguageID, LanguageID)
                        Me.CreateFolders("", txtLanguageName.Text, "COPY")

                        txtLanguageName.Text = ""
                        hdnLanguageID.Value = 0
                        hdnCopyLanguageID.Value = 0
                        Me.ClearDomainInfo()

                        gdvList.DataBind()

                        pnlList.Visible = True
                        pnlEdit.Visible = False

                        lblAlert.Text = "The language has been copied successfully."
                    Else
                        lblAlert.Text = "An error occurred while updating this language."
                    End If

                Else
                    lblAlert.Text = "A language with this name already exists."
                End If

            Else
                If Emagine.GetDataTable("SELECT * FROM Languages WHERE LanguageName = '" & txtLanguageName.Text & "'").Rows.Count = 0 Then

                    SqlBuilder.Append("INSERT INTO Languages ")
                    SqlBuilder.Append("(LanguageName) ")
                    SqlBuilder.Append("VALUES ")
                    SqlBuilder.Append("(@LanguageName)")

                    Command.Parameters.AddWithValue("@LanguageName", txtLanguageName.Text)

                    If Emagine.ExecuteSQL(SqlBuilder.ToString, Command) Then
                        LanguageID = Emagine.GetNumber(Emagine.GetDbValue("SELECT MAX(LanguageID) AS MaxLanguageID FROM Languages"))
                        Me.UpdateDomainInfo(LanguageID)
                        Me.CreateFolders("", txtLanguageName.Text, "NONE")

                        txtLanguageName.Text = ""
                        hdnLanguageID.Value = 0
                        hdnCopyLanguageID.Value = 0
                        Me.ClearDomainInfo()

                        gdvList.DataBind()

                        pnlList.Visible = True
                        pnlEdit.Visible = False

                        lblAlert.Text = "The language has been added successfully."
                    Else
                        lblAlert.Text = "An error occurred while updating this language."
                    End If
                Else
                    lblAlert.Text = "A language with this name already exists."
                End If
            End If
        End If
    End Sub

    Sub CreateFolders(ByVal strOldFolderName As String, ByVal strNewFolderName As String, ByVal strCopyType As String)
        If strOldFolderName <> strNewFolderName Then

            If strOldFolderName.Length > 0 Then
                Select Case strCopyType

                    Case "MOVE"
                        If System.IO.Directory.Exists(Server.MapPath(GlobalVariables.GetValue("VirtualDocumentUploadPath") & strOldFolderName)) = True And System.IO.Directory.Exists(Server.MapPath(GlobalVariables.GetValue("VirtualDocumentUploadPath") & strNewFolderName)) = False Then
                            System.IO.Directory.Move(Server.MapPath(GlobalVariables.GetValue("VirtualDocumentUploadPath") & strOldFolderName), Server.MapPath(GlobalVariables.GetValue("VirtualDocumentUploadPath") & strNewFolderName))
                        End If

                        If System.IO.Directory.Exists(Server.MapPath(GlobalVariables.GetValue("VirtualImageUploadPath") & strOldFolderName)) = True And System.IO.Directory.Exists(Server.MapPath(GlobalVariables.GetValue("VirtualImageUploadPath") & strNewFolderName)) = False Then
                            System.IO.Directory.Move(Server.MapPath(GlobalVariables.GetValue("VirtualImageUploadPath") & strOldFolderName), Server.MapPath(GlobalVariables.GetValue("VirtualImageUploadPath") & strNewFolderName))
                        End If

                        If System.IO.Directory.Exists(Server.MapPath(GlobalVariables.GetValue("VirtualMediaUploadPath") & strOldFolderName)) = True And System.IO.Directory.Exists(Server.MapPath(GlobalVariables.GetValue("VirtualMediaUploadPath") & strNewFolderName)) = False Then
                            System.IO.Directory.Move(Server.MapPath(GlobalVariables.GetValue("VirtualMediaUploadPath") & strOldFolderName), Server.MapPath(GlobalVariables.GetValue("VirtualMediaUploadPath") & strNewFolderName))
                        End If

                        If System.IO.Directory.Exists(Server.MapPath(GlobalVariables.GetValue("VirtualFlashUploadPath") & strOldFolderName)) = True And System.IO.Directory.Exists(Server.MapPath(GlobalVariables.GetValue("VirtualFlashUploadPath") & strNewFolderName)) = False Then
                            System.IO.Directory.Move(Server.MapPath(GlobalVariables.GetValue("VirtualFlashUploadPath") & strOldFolderName), Server.MapPath(GlobalVariables.GetValue("VirtualFlashUploadPath") & strNewFolderName))
                        End If

                    Case "COPY"
                        If Not System.IO.Directory.Exists(Server.MapPath(GlobalVariables.GetValue("VirtualDocumentUploadPath") & strNewFolderName)) Then
                            System.IO.Directory.CreateDirectory(Server.MapPath(GlobalVariables.GetValue("VirtualDocumentUploadPath") & strNewFolderName))
                        End If

                        If Not System.IO.Directory.Exists(Server.MapPath(GlobalVariables.GetValue("VirtualImageUploadPath") & strNewFolderName)) Then
                            System.IO.Directory.CreateDirectory(Server.MapPath(GlobalVariables.GetValue("VirtualImageUploadPath") & strNewFolderName))
                        End If

                        If Not System.IO.Directory.Exists(Server.MapPath(GlobalVariables.GetValue("VirtualMediaUploadPath") & strNewFolderName)) Then
                            System.IO.Directory.CreateDirectory(Server.MapPath(GlobalVariables.GetValue("VirtualMediaUploadPath") & strNewFolderName))
                        End If

                        If Not System.IO.Directory.Exists(Server.MapPath(GlobalVariables.GetValue("VirtualFlashUploadPath") & strNewFolderName)) Then
                            System.IO.Directory.CreateDirectory(Server.MapPath(GlobalVariables.GetValue("VirtualFlashUploadPath") & strNewFolderName))
                        End If
                End Select


            Else
                If Not System.IO.Directory.Exists(Server.MapPath(GlobalVariables.GetValue("VirtualDocumentUploadPath") & strNewFolderName)) Then
                    System.IO.Directory.CreateDirectory(Server.MapPath(GlobalVariables.GetValue("VirtualDocumentUploadPath") & strNewFolderName))
                End If

                If Not System.IO.Directory.Exists(Server.MapPath(GlobalVariables.GetValue("VirtualImageUploadPath") & strNewFolderName)) Then
                    System.IO.Directory.CreateDirectory(Server.MapPath(GlobalVariables.GetValue("VirtualImageUploadPath") & strNewFolderName))
                End If

                If Not System.IO.Directory.Exists(Server.MapPath(GlobalVariables.GetValue("VirtualMediaUploadPath") & strNewFolderName)) Then
                    System.IO.Directory.CreateDirectory(Server.MapPath(GlobalVariables.GetValue("VirtualMediaUploadPath") & strNewFolderName))
                End If

                If Not System.IO.Directory.Exists(Server.MapPath(GlobalVariables.GetValue("VirtualFlashUploadPath") & strNewFolderName)) Then
                    System.IO.Directory.CreateDirectory(Server.MapPath(GlobalVariables.GetValue("VirtualFlashUploadPath") & strNewFolderName))
                End If
            End If

        End If
    End Sub

    Sub CopyContent(ByVal intOldLanguageID As Integer, ByVal intNewLanguageID As Integer)
        Dim PageData As DataTable = Emagine.GetDataTable("SELECT * FROM Pages WHERE LanguageID = " & intOldLanguageID)
        For i As Integer = 0 To (PageData.Rows.Count - 1)
            Dim SqlBuilder As New StringBuilder
            Dim Command As New SqlCommand

            SqlBuilder.Append("INSERT INTO Pages (")

            For j As Integer = 0 To (PageData.Columns.Count - 1)
                Dim ColumnName As String = PageData.Columns(j).ColumnName.ToString
                If ColumnName.ToUpper <> "PAGEID" Then
                    SqlBuilder.Append(ColumnName)
                    If j < (PageData.Columns.Count - 1) Then SqlBuilder.Append(",")
                End If
            Next
            SqlBuilder.Append(") VALUES (")
            For j As Integer = 0 To (PageData.Columns.Count - 1)
                Dim ColumnName As String = PageData.Columns(j).ColumnName.ToString
                If ColumnName.ToUpper <> "PAGEID" Then
                    SqlBuilder.Append("@" & ColumnName)
                    If j < (PageData.Columns.Count - 1) Then SqlBuilder.Append(",")
                End If

                If ColumnName.ToUpper = "LANGUAGEID" Then
                    Command.Parameters.AddWithValue("@" & ColumnName, intNewLanguageID)
                Else
                    Command.Parameters.AddWithValue("@" & ColumnName, PageData.Rows(i).Item(j).ToString)
                End If

            Next
            SqlBuilder.Append(")")

            Emagine.ExecuteSQL(SqlBuilder.ToString, Command)
            SqlBuilder.Remove(0, SqlBuilder.Length)

            Command.Parameters.Clear()

            Dim OldPageID As Integer = PageData.Rows(i).Item("PageID")
            Dim NewPageID As Integer = Emagine.GetDbValue("SELECT MAX(PageID) AS MaxPageID FROM Pages")

            Dim ContentData As DataTable = Emagine.GetDataTable("SELECT * FROM [Content] WHERE ModuleKey = 'Pages01' and ForeignKey = '" & OldPageID & "'")
            For k As Integer = 0 To (ContentData.Rows.Count - 1)
                SqlBuilder.Append("INSERT INTO Content (")

                For l As Integer = 0 To (ContentData.Columns.Count - 1)
                    Dim ColumnName As String = ContentData.Columns(l).ColumnName.ToString
                    If ColumnName.ToUpper <> "CONTENTID" Then
                        SqlBuilder.Append(ColumnName)
                        If l < (ContentData.Columns.Count - 1) Then SqlBuilder.Append(",")
                    End If
                Next
                SqlBuilder.Append(") VALUES (")
                For l As Integer = 0 To (ContentData.Columns.Count - 1)
                    Dim ColumnName As String = ContentData.Columns(l).ColumnName.ToString
                    If ColumnName.ToUpper <> "CONTENTID" Then
                        SqlBuilder.Append("@" & ColumnName)
                        If l < (ContentData.Columns.Count - 1) Then SqlBuilder.Append(",")
                    End If

                    If ColumnName.ToUpper = "FOREIGNKEY" Then
                        Command.Parameters.AddWithValue("@" & ColumnName, NewPageID)
                    ElseIf ColumnName.ToUpper = "DATECREATED" Then
                        Command.Parameters.AddWithValue("@" & ColumnName, Date.Now)
                    ElseIf ColumnName.ToUpper = "CREATEDBY" Then
                        Command.Parameters.AddWithValue("@" & ColumnName, Session("EzEditName").ToString)
                    ElseIf ColumnName.ToUpper = "LASTUPDATED" Then
                        Command.Parameters.AddWithValue("@" & ColumnName, Date.Now)
                    ElseIf ColumnName.ToUpper = "UPDATEDBY" Then
                        Command.Parameters.AddWithValue("@" & ColumnName, Session("EzEditName").ToString)
                    Else
                        Command.Parameters.AddWithValue("@" & ColumnName, ContentData.Rows(k).Item(l).ToString)
                    End If
                Next
                SqlBuilder.Append(")")

                Emagine.ExecuteSQL(SqlBuilder.ToString, Command)
            Next
        Next
    End Sub

    Sub UpdateParentPages(ByVal intOldLanguageID As Integer, ByVal intNewLanguageID As Integer)
        Dim PageData As DataTable = Emagine.GetDataTable("SELECT * FROM Pages WHERE LanguageID = " & intNewLanguageID & " AND ParentPageID > 0")
        For i As Integer = 0 To (PageData.Rows.Count - 1)
            Dim Sql As String = ""
            Dim PageKey As String = PageData.Rows(i).Item("PageKey")
            Dim ParentPageInfo As New Pages01  'Emagine.GetDbValue("SELECT PageKey FROM [Pages] WHERE PageID = " & PageData.Rows(i).Item("ParentPageID"))
            ParentPageInfo = ParentPageInfo.GetPageInfo(PageData.Rows(i).Item("ParentPageID"))
            Dim ParentPageID As Integer = 0

            If ParentPageInfo.PageID > 0 Then
                If ParentPageInfo.PageKey.Length > 0 Then
                    Sql = "SELECT PageID FROM Pages WHERE LanguageID = " & intNewLanguageID & " AND PageKey = '" & ParentPageInfo.PageKey & "'"
                Else
                    Sql = "SELECT PageID FROM Pages WHERE LanguageID = " & intNewLanguageID & " AND PageName = '" & ParentPageInfo.PageName.Replace("'", "''") & "' AND MenuName = '" & ParentPageInfo.PageName.Replace("'", "''") & "' AND SortOrder = " & ParentPageInfo.SortOrder
                End If

                ParentPageID = Emagine.GetNumber(Emagine.GetDbValue(Sql))
                If ParentPageID > 0 Then Emagine.ExecuteSQL("UPDATE Pages SET ParentPageID = " & ParentPageID & " WHERE PageID = " & PageData.Rows(i).Item("PageID"))
            End If
        Next
    End Sub

    Sub UpdateContentPages(ByVal intOldLanguageID As Integer, ByVal intNewLanguageID As Integer)
        Dim PageData As DataTable = Emagine.GetDataTable("SELECT * FROM Pages WHERE LanguageID = " & intNewLanguageID & " AND ContentPageID > 0")
        For i As Integer = 0 To (PageData.Rows.Count - 1)
            Dim Sql As String = ""
            Dim PageKey As String = PageData.Rows(i).Item("PageKey")
            Dim ParentPageInfo As New Pages01  'Emagine.GetDbValue("SELECT PageKey FROM [Pages] WHERE PageID = " & PageData.Rows(i).Item("ParentPageID"))
            ParentPageInfo = ParentPageInfo.GetPageInfo(PageData.Rows(i).Item("ContentPageID"))
            Dim ParentPageID As Integer = 0

            If ParentPageInfo.PageID > 0 Then
                If ParentPageInfo.PageKey.Length > 0 Then
                    Sql = "SELECT PageID FROM Pages WHERE LanguageID = " & intNewLanguageID & " AND PageKey = '" & ParentPageInfo.PageKey & "'"
                Else
                    Sql = "SELECT PageID FROM Pages WHERE LanguageID = " & intNewLanguageID & " AND PageName = '" & ParentPageInfo.PageName.Replace("'", "''") & "' AND MenuName = '" & ParentPageInfo.PageName.Replace("'", "''") & "' AND SortOrder = " & ParentPageInfo.SortOrder
                End If


                ParentPageID = Emagine.GetNumber(Emagine.GetDbValue(Sql))
                If ParentPageID > 0 Then Emagine.ExecuteSQL("UPDATE Pages SET ContentPageID = " & ParentPageID & " WHERE PageID = " & PageData.Rows(i).Item("PageID"))
            End If
        Next
    End Sub

    Sub UpdateDomainInfo(ByVal intLanguageID As Integer)
        Me.PopulateDomains(0)

        For i As Integer = 0 To (intMaxDomainRows - 1)
            Dim Sql As String = ""
            Dim hdnDomainID As HiddenField = tblDomains.Rows(i).Cells(0).FindControl("hdnDomainID" & i)
            Dim txtDomainName As TextBox = tblDomains.Rows(i).Cells(0).FindControl("txtDomainName" & i)
            Dim cbxIsEnabled As CheckBox = tblDomains.Rows(i).Cells(0).FindControl("cbxIsEnabled" & i)
            Dim DomainID As Integer = Emagine.GetNumber(hdnDomainID.Value)
            Dim DomainName As String = txtDomainName.Text.Trim
            Dim IsEnabled As Boolean = cbxIsEnabled.Checked

            If DomainName.Length > 0 Then
                If DomainID > 0 Then
                    Sql = "UPDATE Domains SET DomainName = '" & DomainName.Replace("'", "''") & "', IsEnabled = '" & cbxIsEnabled.Checked & "' WHERE DomainID = " & DomainID
                Else
                    Sql = "INSERT INTO Domains (LanguageID,DomainName,IsEnabled) VALUES (" & intLanguageID & ", '" & txtDomainName.Text.Replace("'", "''") & "', '" & cbxIsEnabled.Checked & "')"
                End If
                Emagine.ExecuteSQL(Sql)

            Else
                If DomainID > 0 Then
                    Sql = "DELETE FROM Domains WHERE DomainID = " & DomainID
                    Emagine.ExecuteSQL(Sql)
                End If
            End If
        Next
    End Sub

    Sub CopyArticles(ByVal intOldLanguageID As Integer, ByVal intNewLanguageID As Integer)
        Dim ArticleCategoryData As DataTable = Emagine.GetDataTable("SELECT * FROM ModuleCategories WHERE LanguageID = " & intOldLanguageID & " AND CategoryID IN (SELECT CategoryID FROM Articles)")

        For i As Integer = 0 To (ArticleCategoryData.Rows.Count - 1)
            Dim OldCategoryID As Integer = ArticleCategoryData.Rows(i).Item("CategoryID")
            Dim NewCategoryID As Integer = Me.CopyModuleCategory(OldCategoryID, intNewLanguageID)

            If NewCategoryID > 0 Then
                Dim ArticleData As DataTable = Emagine.GetDataTable("SELECT * FROM Articles WHERE CategoryID = " & OldCategoryID)

                For x As Integer = 0 To (ArticleData.Rows.Count - 1)
                    Dim NewResourceID As String = Emagine.GetUniqueID()
                    Dim OldResourceID As String = ArticleData.Rows(x).Item("ResourceID")

                    Dim SqlBuilder As New StringBuilder
                    SqlBuilder.Append("INSERT INTO Articles (")

                    For j As Integer = 1 To (ArticleData.Columns.Count - 1)
                        SqlBuilder.Append(ArticleData.Columns(j).ColumnName)
                        If j < (ArticleData.Columns.Count - 1) Then SqlBuilder.Append(",")
                    Next
                    SqlBuilder.Append(") VALUES (")
                    For j As Integer = 1 To (ArticleData.Columns.Count - 1)
                        SqlBuilder.Append("@" & ArticleData.Columns(j).ColumnName)
                        If j < (ArticleData.Columns.Count - 1) Then SqlBuilder.Append(",")
                    Next
                    SqlBuilder.Append(")")

                    Dim Command As New SqlCommand

                    For j As Integer = 1 To (ArticleData.Columns.Count - 1)
                        If ArticleData.Columns(j).ColumnName = "ResourceID" Then
                            Command.Parameters.AddWithValue("@ResourceID", NewResourceID)
                        ElseIf ArticleData.Columns(j).ColumnName = "CategoryID" Then
                            Command.Parameters.AddWithValue("@CategoryID", NewCategoryID)
                        Else
                            Command.Parameters.AddWithValue("@" & ArticleData.Columns(j).ColumnName, ArticleData.Rows(x).Item(j))
                        End If
                    Next

                    If Emagine.ExecuteSQL(SqlBuilder.ToString, Command) Then
                        Me.CopyResource(OldResourceID, NewResourceID)
                    End If
                Next
            End If
        Next
    End Sub


    Sub CopyDownloads(ByVal intOldLanguageID As Integer, ByVal intNewLanguageID As Integer)
        Dim DownloadCategoryData As DataTable = Emagine.GetDataTable("SELECT * FROM ModuleCategories WHERE LanguageID = " & intOldLanguageID & " AND CategoryID IN (SELECT CategoryID FROM Downloads)")

        For i As Integer = 0 To (DownloadCategoryData.Rows.Count - 1)
            Dim OldCategoryID As Integer = DownloadCategoryData.Rows(i).Item("CategoryID")
            Dim NewCategoryID As Integer = Me.CopyModuleCategory(OldCategoryID, intNewLanguageID)

            If NewCategoryID > 0 Then
                Dim DownloadData As DataTable = Emagine.GetDataTable("SELECT * FROM Downloads WHERE CategoryID = " & OldCategoryID)

                For x As Integer = 0 To (DownloadData.Rows.Count - 1)
                    Dim NewResourceID As String = Emagine.GetUniqueID()
                    Dim OldResourceID As String = DownloadData.Rows(x).Item("ResourceID")

                    Dim SqlBuilder As New StringBuilder
                    SqlBuilder.Append("INSERT INTO Downloads (")

                    For j As Integer = 1 To (DownloadData.Columns.Count - 1)
                        SqlBuilder.Append(DownloadData.Columns(j).ColumnName)
                        If j < (DownloadData.Columns.Count - 1) Then SqlBuilder.Append(",")
                    Next
                    SqlBuilder.Append(") VALUES (")
                    For j As Integer = 1 To (DownloadData.Columns.Count - 1)
                        SqlBuilder.Append("@" & DownloadData.Columns(j).ColumnName)
                        If j < (DownloadData.Columns.Count - 1) Then SqlBuilder.Append(",")
                    Next
                    SqlBuilder.Append(")")

                    Dim Command As New SqlCommand

                    For j As Integer = 1 To (DownloadData.Columns.Count - 1)
                        If DownloadData.Columns(j).ColumnName = "ResourceID" Then
                            Command.Parameters.AddWithValue("@ResourceID", NewResourceID)
                        ElseIf DownloadData.Columns(j).ColumnName = "CategoryID" Then
                            Command.Parameters.AddWithValue("@CategoryID", NewCategoryID)
                        Else
                            Command.Parameters.AddWithValue("@" & DownloadData.Columns(j).ColumnName, DownloadData.Rows(x).Item(j))
                        End If
                    Next

                    If Emagine.ExecuteSQL(SqlBuilder.ToString, Command) Then
                        Me.CopyResource(OldResourceID, NewResourceID)
                    End If
                Next
            End If
        Next
    End Sub

    Sub CopyCareers(ByVal intOldLanguageID As Integer, ByVal intNewLanguageID As Integer)
        Dim CareerCategoryData As DataTable = Emagine.GetDataTable("SELECT * FROM ModuleCategories WHERE LanguageID = " & intOldLanguageID & " AND CategoryID IN (SELECT CategoryID FROM Careers)")

        For i As Integer = 0 To (CareerCategoryData.Rows.Count - 1)
            Dim OldCategoryID As Integer = CareerCategoryData.Rows(i).Item("CategoryID")
            Dim NewCategoryID As Integer = Me.CopyModuleCategory(OldCategoryID, intNewLanguageID)

            If NewCategoryID > 0 Then
                Dim CareerData As DataTable = Emagine.GetDataTable("SELECT * FROM Careers WHERE CategoryID = " & OldCategoryID)

                For x As Integer = 0 To (CareerData.Rows.Count - 1)
                    Dim NewResourceID As String = Emagine.GetUniqueID()
                    Dim OldResourceID As String = CareerData.Rows(x).Item("ResourceID")

                    Dim SqlBuilder As New StringBuilder
                    SqlBuilder.Append("INSERT INTO Careers (")

                    For j As Integer = 1 To (CareerData.Columns.Count - 1)
                        SqlBuilder.Append(CareerData.Columns(j).ColumnName)
                        If j < (CareerData.Columns.Count - 1) Then SqlBuilder.Append(",")
                    Next
                    SqlBuilder.Append(") VALUES (")
                    For j As Integer = 1 To (CareerData.Columns.Count - 1)
                        SqlBuilder.Append("@" & CareerData.Columns(j).ColumnName)
                        If j < (CareerData.Columns.Count - 1) Then SqlBuilder.Append(",")
                    Next
                    SqlBuilder.Append(")")

                    Dim Command As New SqlCommand

                    For j As Integer = 1 To (CareerData.Columns.Count - 1)
                        If CareerData.Columns(j).ColumnName = "ResourceID" Then
                            Command.Parameters.AddWithValue("@ResourceID", NewResourceID)
                        ElseIf CareerData.Columns(j).ColumnName = "CategoryID" Then
                            Command.Parameters.AddWithValue("@CategoryID", NewCategoryID)
                        Else
                            Command.Parameters.AddWithValue("@" & CareerData.Columns(j).ColumnName, CareerData.Rows(x).Item(j))
                        End If
                    Next

                    If Emagine.ExecuteSQL(SqlBuilder.ToString, Command) Then
                        Me.CopyResource(OldResourceID, NewResourceID)
                    End If
                Next
            End If
        Next
    End Sub


    Sub CopyForms(ByVal intOldLanguageID As Integer, ByVal intNewLanguageID As Integer)
        Dim FormData As DataTable = Emagine.GetDataTable("SELECT * FROM Forms WHERE LanguageID = " & intOldLanguageID)

        For i As Integer = 0 To (FormData.Rows.Count - 1)
            Dim OldFormID As Integer = FormData.Rows(i).Item("FormID")
            Dim NewFormID As Integer = Me.CopyForm(OldFormID, intNewLanguageID)

            If NewFormID > 0 Then
                Dim FormFieldData As DataTable = Emagine.GetDataTable("SELECT * FROM FormFields WHERE FormID = " & OldFormID)

                For x As Integer = 0 To (FormFieldData.Rows.Count - 1)

                    Dim SqlBuilder As New StringBuilder
                    SqlBuilder.Append("INSERT INTO FormFields (")

                    For j As Integer = 1 To (FormFieldData.Columns.Count - 1)
                        SqlBuilder.Append(FormFieldData.Columns(j).ColumnName)
                        If j < (FormFieldData.Columns.Count - 1) Then SqlBuilder.Append(",")
                    Next
                    SqlBuilder.Append(") VALUES (")
                    For j As Integer = 1 To (FormFieldData.Columns.Count - 1)
                        SqlBuilder.Append("@" & FormFieldData.Columns(j).ColumnName)
                        If j < (FormFieldData.Columns.Count - 1) Then SqlBuilder.Append(",")
                    Next
                    SqlBuilder.Append(")")

                    Dim Command As New SqlCommand

                    For j As Integer = 1 To (FormFieldData.Columns.Count - 1)
                        If FormFieldData.Columns(j).ColumnName = "FormID" Then
                            Command.Parameters.AddWithValue("@FormID", NewFormID)
                        Else
                            Command.Parameters.AddWithValue("@" & FormFieldData.Columns(j).ColumnName, FormFieldData.Rows(x).Item(j))
                        End If
                    Next

                    Emagine.ExecuteSQL(SqlBuilder.ToString, Command)
                Next
            End If
        Next
    End Sub

    Function CopyForm(ByVal intOldFormID As Integer, ByVal intNewLanguageID As Integer) As Integer
        Dim NewFormID As Integer = 0
        Dim FormData As DataTable = Emagine.GetDataTable("SELECT * FROM Forms WHERE FormID = " & intOldFormID)
        Dim SqlBuilder As New StringBuilder
        SqlBuilder.Append("INSERT INTO Forms (")
        For i As Integer = 1 To (FormData.Columns.Count - 1)
            SqlBuilder.Append(FormData.Columns(i).ColumnName)
            If i < (FormData.Columns.Count - 1) Then SqlBuilder.Append(",")
        Next
        SqlBuilder.Append(") VALUES (")
        For i As Integer = 1 To (FormData.Columns.Count - 1)
            SqlBuilder.Append("@" & FormData.Columns(i).ColumnName)
            If i < (FormData.Columns.Count - 1) Then SqlBuilder.Append(",")
        Next
        SqlBuilder.Append(")")

        Dim Command As New SqlCommand
        For i As Integer = 1 To (FormData.Columns.Count - 1)
            If FormData.Columns(i).ColumnName = "LanguageID" Then
                Command.Parameters.AddWithValue("@" & FormData.Columns(i).ColumnName, intNewLanguageID)
            Else
                Command.Parameters.AddWithValue("@" & FormData.Columns(i).ColumnName, FormData.Rows(0).Item(i))
            End If
        Next

        If Emagine.ExecuteSQL(SqlBuilder.ToString, Command) Then
            NewFormID = Emagine.GetNumber(Emagine.GetDbValue("SELECT MAX(FormID) AS MaxFormID FROM Forms"))
        End If

        Return NewFormID
    End Function

    Sub CopyEvents(ByVal intOldLanguageID As Integer, ByVal intNewLanguageID As Integer)
        Dim EventCategoryData As DataTable = Emagine.GetDataTable("SELECT * FROM ModuleCategories WHERE LanguageID = " & intOldLanguageID & " AND CategoryID IN (SELECT CategoryID FROM Events)")

        For i As Integer = 0 To (EventCategoryData.Rows.Count - 1)
            Dim OldCategoryID As Integer = EventCategoryData.Rows(i).Item("CategoryID")
            Dim NewCategoryID As Integer = Me.CopyModuleCategory(OldCategoryID, intNewLanguageID)

            If NewCategoryID > 0 Then
                Dim EventData As DataTable = Emagine.GetDataTable("SELECT * FROM Events WHERE CategoryID = " & OldCategoryID)

                For x As Integer = 0 To (EventData.Rows.Count - 1)
                    Dim NewResourceID As String = Emagine.GetUniqueID()
                    Dim OldResourceID As String = EventData.Rows(x).Item("ResourceID")

                    Dim SqlBuilder As New StringBuilder
                    SqlBuilder.Append("INSERT INTO Events (")

                    For j As Integer = 1 To (EventData.Columns.Count - 1)
                        SqlBuilder.Append(EventData.Columns(j).ColumnName)
                        If j < (EventData.Columns.Count - 1) Then SqlBuilder.Append(",")
                    Next
                    SqlBuilder.Append(") VALUES (")
                    For j As Integer = 1 To (EventData.Columns.Count - 1)
                        SqlBuilder.Append("@" & EventData.Columns(j).ColumnName)
                        If j < (EventData.Columns.Count - 1) Then SqlBuilder.Append(",")
                    Next
                    SqlBuilder.Append(")")

                    Dim Command As New SqlCommand

                    For j As Integer = 1 To (EventData.Columns.Count - 1)
                        If EventData.Columns(j).ColumnName = "ResourceID" Then
                            Command.Parameters.AddWithValue("@ResourceID", NewResourceID)
                        ElseIf EventData.Columns(j).ColumnName = "CategoryID" Then
                            Command.Parameters.AddWithValue("@CategoryID", NewCategoryID)
                        Else
                            Command.Parameters.AddWithValue("@" & EventData.Columns(j).ColumnName, EventData.Rows(x).Item(j))
                        End If
                    Next

                    If Emagine.ExecuteSQL(SqlBuilder.ToString, Command) Then
                        Me.CopyResource(OldResourceID, NewResourceID)

                        Dim NewEventID As Integer = Emagine.GetNumber(Emagine.GetDbValue("SELECT MAX(EventID) AS MaxEventID FROM Events"))
                        Dim OldEventID As Integer = EventData.Rows(x).Item("EventID")

                        Me.CopyEventDates(OldEventID, NewEventID)
                    End If
                Next
            End If
        Next
    End Sub

    Sub CopyEventDates(ByVal intOldEventID As Integer, ByVal intNewEventID As Integer)
        Dim EventDateData As DataTable = Emagine.GetDataTable("SELECT * FROM EventDates WHERE EventID = " & intOldEventID)

        For x As Integer = 0 To (EventDateData.Rows.Count - 1)
            Dim SqlBuilder As New StringBuilder
            SqlBuilder.Append("INSERT INTO EventDates (")

            For j As Integer = 1 To (EventDateData.Columns.Count - 1)
                SqlBuilder.Append(EventDateData.Columns(j).ColumnName)
                If j < (EventDateData.Columns.Count - 1) Then SqlBuilder.Append(",")
            Next
            SqlBuilder.Append(") VALUES (")
            For j As Integer = 1 To (EventDateData.Columns.Count - 1)
                SqlBuilder.Append("@" & EventDateData.Columns(j).ColumnName)
                If j < (EventDateData.Columns.Count - 1) Then SqlBuilder.Append(",")
            Next
            SqlBuilder.Append(")")

            Dim Command As New SqlCommand

            For j As Integer = 1 To (EventDateData.Columns.Count - 1)
                If EventDateData.Columns(j).ColumnName = "EventID" Then
                    Command.Parameters.AddWithValue("@EventID", intNewEventID)
                Else
                    Command.Parameters.AddWithValue("@" & EventDateData.Columns(j).ColumnName, EventDateData.Rows(x).Item(j))
                End If
            Next

            Emagine.ExecuteSQL(SqlBuilder.ToString, Command)
        Next
    End Sub


    Function CopyModuleCategory(ByVal intOldCategoryID As Integer, ByVal intNewLanguageID As Integer) As Integer
        Dim NewCategoryID As Integer = 0
        Dim CategoryData As DataTable = Emagine.GetDataTable("SELECT * FROM ModuleCategories WHERE CategoryID = " & intOldCategoryID)
        Dim SqlBuilder As New StringBuilder
        SqlBuilder.Append("INSERT INTO ModuleCategories (")
        For i As Integer = 1 To (CategoryData.Columns.Count - 1)
            SqlBuilder.Append(CategoryData.Columns(i).ColumnName)
            If i < (CategoryData.Columns.Count - 1) Then SqlBuilder.Append(",")
        Next
        SqlBuilder.Append(") VALUES (")
        For i As Integer = 1 To (CategoryData.Columns.Count - 1)
            SqlBuilder.Append("@" & CategoryData.Columns(i).ColumnName)
            If i < (CategoryData.Columns.Count - 1) Then SqlBuilder.Append(",")
        Next
        SqlBuilder.Append(")")

        Dim Command As New SqlCommand
        For i As Integer = 1 To (CategoryData.Columns.Count - 1)
            If CategoryData.Columns(i).ColumnName = "LanguageID" Then
                Command.Parameters.AddWithValue("@" & CategoryData.Columns(i).ColumnName, intNewLanguageID)
            Else
                Command.Parameters.AddWithValue("@" & CategoryData.Columns(i).ColumnName, CategoryData.Rows(0).Item(i))
            End If
        Next

        If Emagine.ExecuteSQL(SqlBuilder.ToString, Command) Then
            NewCategoryID = Emagine.GetNumber(Emagine.GetDbValue("SELECT MAX(CategoryID) AS MaxCategoryID FROM ModuleCategories"))
        End If

        Return NewCategoryID
    End Function

    Sub CopyResource(ByVal strOldResourceID As String, ByVal strNewResourceID As String)
        Dim SqlBuilder As New StringBuilder
        Dim ResourceData As DataTable = Emagine.GetDataTable("SELECT * FROM Resources WHERE ResourceID = '" & strOldResourceID & "'")
        Dim aryFieldsToIgnore As Array = Split("ClickCount,LastClickdate,DownloadCount,LastDownloadDate", ",")

        If ResourceData.Rows.Count > 0 Then
            SqlBuilder.Append("INSERT INTO Resources (")

            For i As Integer = 0 To (ResourceData.Columns.Count - 1)
                Dim FoundMatch As Boolean = True
                For j As Integer = 0 To aryFieldsToIgnore.GetUpperBound(0)
                    If ResourceData.Columns(i).ColumnName.ToLower = aryFieldsToIgnore(j).ToString.ToLower Then
                        FoundMatch = False
                        Exit For
                    End If
                Next

                If FoundMatch Then
                    SqlBuilder.Append(ResourceData.Columns(i).ColumnName)
                    If i < (ResourceData.Columns.Count - 1) Then SqlBuilder.Append(",")
                End If
            Next
            SqlBuilder.Append(") VALUES (")

            For i As Integer = 0 To (ResourceData.Columns.Count - 1)
                Dim FoundMatch As Boolean = True
                For j As Integer = 0 To aryFieldsToIgnore.GetUpperBound(0)
                    If ResourceData.Columns(i).ColumnName.ToLower = aryFieldsToIgnore(j).ToString.ToLower Then
                        FoundMatch = False
                        Exit For
                    End If
                Next

                If FoundMatch Then
                    SqlBuilder.Append("@" & ResourceData.Columns(i).ColumnName)
                    If i < (ResourceData.Columns.Count - 1) Then SqlBuilder.Append(",")
                End If
            Next
            SqlBuilder.Append(")")


            Dim Command As New SqlCommand

            For i As Integer = 0 To (ResourceData.Columns.Count - 1)
                Dim FoundMatch As Boolean = True
                For j As Integer = 0 To aryFieldsToIgnore.GetUpperBound(0)
                    If ResourceData.Columns(i).ColumnName.ToLower = aryFieldsToIgnore(j).ToString.ToLower Then
                        FoundMatch = False
                        Exit For
                    End If
                Next

                If FoundMatch Then
                    If ResourceData.Columns(i).ColumnName = "ResourceID" Then
                        Command.Parameters.AddWithValue("@ResourceID", strNewResourceID)
                    Else
                        Command.Parameters.AddWithValue("@" & ResourceData.Columns(i).ColumnName, ResourceData.Rows(0).Item(i).ToString)
                    End If
                End If
            Next

            Emagine.ExecuteSQL(SqlBuilder.ToString, Command)
        End If

    End Sub

    Sub ClearDomainInfo()
        For i As Integer = 0 To (intMaxDomainRows - 1)
            Dim Sql As String = ""
            Dim hdnDomainID As HiddenField = tblDomains.Rows(i).Cells(0).FindControl("hdnDomainID" & i)
            Dim txtDomainName As TextBox = tblDomains.Rows(i).Cells(0).FindControl("txtDomainName" & i)
            Dim cbxIsEnabled As CheckBox = tblDomains.Rows(i).Cells(0).FindControl("cbxIsEnabled" & i)

            hdnDomainID.Value = 0
            txtDomainName.Text = ""
            cbxIsEnabled.Checked = False
        Next
    End Sub
End Class
