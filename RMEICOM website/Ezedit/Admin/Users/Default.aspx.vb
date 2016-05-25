
Partial Class Ezedit_Admin_Default
    Inherits System.Web.UI.Page

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        csvUsername.ServerCondition = New PeterBlum.VAM.ServerConditionEventHandler(AddressOf DuplicateUsernameCheck)
        lblAlert.Text = ""

        If Session("Alert") IsNot Nothing Then
            lblAlert.Text = Session("Alert")
            Session("Alert") = ""
        End If
    End Sub

    Protected Sub ddlEzEditLevelID_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlEzEditLevelID.Load
        If Not Page.IsPostBack Then
            ddlEzEditLevelID.DataSource = EzeditUser.GetUserLevels
            ddlEzEditLevelID.DataTextField = "EzEditLevel"
            ddlEzEditLevelID.DataValueField = "EzEditLevelID"
            ddlEzEditLevelID.DataBind()
        End If
    End Sub

    Protected Sub ddlLanguageID_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLanguageID.Load
        If Not Page.IsPostBack Then
            ddlLanguageID.DataSource = Emagine.GetDataTable("SELECT LanguageID, LanguageName FROM Languages")
            ddlLanguageID.DataTextField = "LanguageName"
            ddlLanguageID.DataValueField = "LanguageID"
            ddlLanguageID.DataBind()
        End If
    End Sub

    Protected Sub lbtnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnAddNew.Click
        Me.DisplayEditPanel(0)
    End Sub

    Protected Sub cbxIsEnabled_CheckChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cbxIsEnabled As CheckBox = sender
        Dim btnEdit As ImageButton = cbxIsEnabled.Parent.Parent.FindControl("btnEdit")
        Dim ItemID As Integer = Emagine.GetNumber(btnEdit.CommandArgument)
        Dim EnabledText As String = "enabled"

        If Not cbxIsEnabled.Checked Then EnabledText = "disabled"

        If ItemID > 0 Then
            Dim MyUser As EzeditUser = EzeditUser.GetUser(ItemID)
            MyUser.IsEnabled = cbxIsEnabled.Checked

            If MyUser.UpdateEzEditUser(MyUser) Then
                lblAlert.Text = "The item has been " & EnabledText & " successfully."
                Me.DisplayListPanel()
            Else
                lblAlert.Text = "An error occurred while attempting to update this user."
            End If
        End If
    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim UserID As Integer = Button.CommandArgument

        Me.DisplayEditPanel(UserID)
    End Sub

    Protected Sub btnPermissions_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim UserID As Integer = Button.CommandArgument

        Response.Redirect("Permissions.aspx?UserID=" & UserID)
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim ErrorMessage As String = ""
        Dim UserID As Integer = Button.CommandArgument
        Dim MyUser As EzeditUser = EzeditUser.GetUser(UserID)

        If EzeditUser.DeleteUser(MyUser, ErrorMessage) Then
            lblAlert.Text = "The user has been removed successfully."
            Me.DisplayListPanel()
        Else
            lblAlert.Text = "An error occurred while attempting to delete this user.<br /><i>(" & ErrorMessage & ")</i>"
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DisplayListPanel()
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If PeterBlum.VAM.Globals.Page.IsValid Then
            Dim UserID As Integer = Emagine.GetNumber(hdnUserID.Value)
            Dim OldPassword As String = Emagine.GetDbValue("SELECT Password FROM EzEditUsers WHERE UserID = " & UserID)
            Dim NewPassword = txtPassword.Text
            If NewPassword <> OldPassword Then
                NewPassword = Emagine.GetMd5Hash(txtPassword.Text)
            End If
            Dim User As New EzeditUser
            User.UserID = UserID
            User.EzEditLevelID = ddlEzEditLevelID.SelectedValue
            User.LanguageID = ddlLanguageID.SelectedValue
            User.FirstName = txtFirstName.Text
            User.LastName = txtLastName.Text
            User.Email = txtEmail.Text
            User.Username = txtUsername.Text
            User.Password = NewPassword
            User.IsEnabled = rblIsEnabled.SelectedValue
            If User.UpdateEzEditUser(User) Then
                lblAlert.Text = "The user has been updated successfully."
                Me.DisplayListPanel()
            Else
                lblAlert.Text = "An error occurred while attempting to update this user."
            End If
        End If
    End Sub


#End Region



    Sub DisplayListPanel()
        gdvList.DataBind()

        pnlList.Visible = True
        pnlEdit.Visible = False
    End Sub

    Sub DisplayEditPanel(ByVal intItemID As Integer)
        Dim User As EzeditUser = EzeditUser.GetUser(intItemID)
        If User.UserID <> 0 Then
            ddlEzEditLevelID.SelectedIndex = -1
            For Each Item As ListItem In ddlEzEditLevelID.Items
                If Item.Value = User.EzEditLevelID Then
                    Item.Selected = True
                    Exit For
                End If
            Next

            ddlLanguageID.SelectedIndex = -1
            For Each Item As ListItem In ddlLanguageID.Items
                If Item.Value = User.LanguageID Then
                    Item.Selected = True
                    Exit For
                End If
            Next

            txtFirstName.Text = User.FirstName
            txtLastName.Text = User.LastName
            txtEmail.Text = User.Email
            txtUsername.Text = User.Username
            txtPassword.Attributes.Add("value", User.Password)
            txtPasswordConfirm.Attributes.Add("value", User.Password)
            rblIsEnabled.SelectedIndex = -1
            For Each Item As ListItem In rblIsEnabled.Items
                If Item.Value = User.IsEnabled Then
                    Item.Selected = True
                    Exit For
                End If
            Next

            hdnUserID.Value = User.UserID
            csvUsername.Enabled = False
        Else
            ddlEzEditLevelID.SelectedIndex = -1
            ddlLanguageID.SelectedIndex = -1
            txtFirstName.Text = ""
            txtLastName.Text = ""
            txtEmail.Text = ""
            txtUsername.Text = ""
            txtPassword.Text = ""
            txtPasswordConfirm.Text = ""
            rblIsEnabled.SelectedIndex = 0
            hdnUserID.Value = 0
        End If

        pnlList.Visible = False
        pnlEdit.Visible = True
    End Sub

    Protected Sub DuplicateUsernameCheck(ByVal sourceCondition As PeterBlum.VAM.BaseCondition, ByVal args As PeterBlum.VAM.ConditionEventArgs)
        Dim vArgs As PeterBlum.VAM.ConditionTwoFieldEventArgs = CType(args, PeterBlum.VAM.ConditionTwoFieldEventArgs)

        Dim UserID As Integer = Emagine.GetNumber(hdnUserID.Value)

        args.IsMatch = IsUniqueUsername(txtUsername.Text, UserID)
    End Sub

    Public Shared Function IsUniqueUsername(ByVal strUsername As String, ByVal intUserId As Integer) As Boolean
        If Emagine.GetDbValue("SELECT Count(*) FROM EzEditUsers WHERE UserId <> " & intUserId & " AND Username = '" & strUsername & "'") = 0 Then
            Return True
        Else
            Return False
        End If
    End Function






End Class
