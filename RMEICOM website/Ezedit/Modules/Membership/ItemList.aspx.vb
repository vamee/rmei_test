
Partial Class Ezedit_Modules_Members_Default
    Inherits System.Web.UI.Page

    Dim _ModuleKey As String = "Membership"

    Sub EditMember(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim MemberID As String = Button.CommandArgument.ToString

        If MemberID.Length > 0 Then
            Dim Member As New Membership.Member
            Member = Membership.Member.GetMembershipUser(MemberID)

            If Member.MemberID.Length > 0 Then
                hdnMemberID.Value = Member.MemberID
                ddlCategoryID.SelectedIndex = -1
                For Each Item As ListItem In ddlCategoryID.Items
                    If Item.Value = Member.CategoryID Then
                        Item.Selected = True
                        Exit For
                    End If
                Next

                txtFirstName.Text = Member.FirstName
                txtLastName.Text = Member.LastName
                txtEmail.Text = Member.Email
                txtUsername.Text = Member.Username
                txtPassword.Text = Member.Password
                txtDescription.Text = Member.Description
            End If
        End If

        pnlEditMember.Visible = True
        pnlMembers.Visible = False
    End Sub

    Sub DeleteMember(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim MemberID As String = Button.CommandArgument.ToString

        Emagine.ExecuteSQL("DELETE FROM Members WHERE MemberID = '" & MemberID & "'")

        lblAlert.Text = "The member has been removed successfully."

        Me.RefreshData()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim CategoryID As Integer = Emagine.GetNumber(Request("CategoryID").ToString)
            Dim CategoryName As String = Emagine.GetDbValue("SELECT Categoryname FROM ModuleCategories WHERE CategoryID = " & CategoryID)
            Dim ModuleName As String = ""
            Dim ModuleUrl As String = ""
            Dim ModuleData As DataTable = Emagine.GetDataTable("SELECT * FROM Modules WHERE ModuleKey = '" & _ModuleKey & "'")

            If ModuleData.Rows.Count > 0 Then
                ModuleName = ModuleData.Rows(0).Item("Name")
                ModuleUrl = ModuleData.Rows(0).Item("EzEditMenuLink")
                lblPageTitle.Text = "<a href='" & ModuleUrl & "' class='pageTitle'>" & ModuleName & "</a> > "
            End If

            pnlMembers.Visible = True
            lblPageTitle.Text = lblPageTitle.Text & CategoryName

            Me.RefreshData()
        End If
    End Sub

    Sub RefreshData()
        Dim CategoryID As Integer = Emagine.GetNumber(Request("CategoryID").ToString)
        Dim Sql As String = "SELECT MemberID, LastName + ', ' + FirstName As MemberName, Username, Description FROM Members WHERE CategoryID = " & CategoryID & " ORDER BY LastName, FirstName"

        gdvMembers.DataSource = Emagine.GetDataTable(Sql)
        gdvMembers.DataBind()
    End Sub

    Protected Sub btnAddmember_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddmember.Click
        Dim CategoryID As Integer = Emagine.GetNumber(Request("CategoryID").ToString)
        pnlEditMember.Visible = True
        pnlMembers.Visible = False
        hdnMemberID.Value = ""
        txtFirstName.Focus()

        ddlCategoryID.SelectedIndex = -1
        For Each Item As ListItem In ddlCategoryID.Items
            If Item.Value = CategoryID Then
                Item.Selected = True
                Exit For
            End If
        Next

        Me.ResetFormFields()
    End Sub

    Protected Sub btnAddMember2_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddMember2.Click
        Dim CategoryID As Integer = Emagine.GetNumber(Request("CategoryID").ToString)
        pnlEditMember.Visible = True
        pnlMembers.Visible = False
        hdnMemberID.Value = ""
        txtFirstName.Focus()

        ddlCategoryID.SelectedIndex = -1
        For Each Item As ListItem In ddlCategoryID.Items
            If Item.Value = CategoryID Then
                Item.Selected = True
                Exit For
            End If
        Next

        Me.ResetFormFields()
    End Sub

    Sub ResetFormFields()
        txtFirstName.Text = ""
        txtLastName.Text = ""
        txtEmail.Text = ""
        txtUsername.Text = ""
        txtPassword.Text = ""
        txtDescription.Text = ""
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        pnlMembers.Visible = True
        pnlEditMember.Visible = False
    End Sub

    
    
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If PeterBlum.VAM.Globals.Page.IsValid Then
            Dim Result As Boolean = False
            Dim UserExists As Boolean = False
            Dim MemberID As String = hdnMemberID.Value.ToString

            If MemberID.Length = 0 Then
                Dim MemberData As DataTable = Emagine.GetDataTable("SELECT * FROM Members WHERE Username = '" & txtUsername.Text.Replace("'", "''") & "'")

                If MemberData.Rows.Count > 0 Then UserExists = True
            End If

            If Not UserExists Then
                Dim Member As New Membership.Member
                Member.MemberID = MemberID
                Member.CategoryID = ddlCategoryID.SelectedValue
                Member.FirstName = txtFirstName.Text
                Member.LastName = txtLastName.Text
                Member.Email = txtEmail.Text
                Member.Username = txtUsername.Text
                Member.Password = txtPassword.Text
                Member.Description = txtDescription.Text
                Member.CreatedDate = Date.Now
                Member.CreatedBy = Session("EzEditName")
                Member.UpdatedDate = Date.Now
                Member.UpdatedBy = Session("EzEditName")

                If MemberID.Length > 0 Then
                    Result = Membership.Member.Update(Member)
                Else
                    Result = Membership.Member.Add(Member)
                End If

                If Result = True Then
                    If MemberID.Length > 0 Then
                        lblAlert.Text = "The member info has been updated successfully."
                    Else
                        lblAlert.Text = "The member info has been added successfully."
                    End If

                    Me.RefreshData()

                    pnlMembers.Visible = True
                    pnlEditMember.Visible = False
                Else
                    lblAlert.Text = "Error. Unable to save record."
                End If

            Else
                lblAlert.Text = "This username already exists in the database. Please choose another."
            End If
        End If
    End Sub

    Protected Sub gdvMembers_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvMembers.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim MemberID As String = DataBinder.Eval(e.Row.DataItem, "MemberID")

            Dim btnEdit As ImageButton = e.Row.FindControl("btnEdit")
            Dim btnDelete As ImageButton = e.Row.FindControl("btnDelete")

            btnEdit.CommandArgument = MemberID
            btnDelete.CommandArgument = MemberID
        End If
    End Sub

    Protected Sub ddlCategoryID_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCategoryID.Load
        If Not Page.IsPostBack Then
            ddlCategoryID.DataSource = Emagine.GetDataTable("SELECT CategoryID, CategoryName FROM ModuleCategories WHERE ModuleKey = '" & _ModuleKey & "'")
            ddlCategoryID.DataTextField = "CategoryName"
            ddlCategoryID.DataValueField = "CategoryID"
            ddlCategoryID.DataBind()
        End If
    End Sub
End Class
