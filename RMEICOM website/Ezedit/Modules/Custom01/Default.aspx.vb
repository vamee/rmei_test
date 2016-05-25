
Partial Class Ezedit_Modules_Custom01_Default
    Inherits System.Web.UI.Page

    Dim _ModuleKey As String = "Custom01"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblAlert.Text = ""
    End Sub

    Protected Sub cbxIsEnabled_CheckChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cbxIsEnabled As CheckBox = sender
        Dim btnEdit As ImageButton = cbxIsEnabled.Parent.Parent.FindControl("btnEdit")
        Dim ItemID As Integer = Emagine.GetNumber(btnEdit.CommandArgument)
        Dim EnabledText As String = "enabled"

        If Not cbxIsEnabled.Checked Then EnabledText = "disabled"

        If ItemID > 0 Then
            Dim MyApp As Custom01 = Custom01.GetCustomApplication(ItemID)

            Dim MyResource As Resources.Resource = Resources.Resource.GetResource(MyApp.ResourceID)
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

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim ItemID As Integer = Button.CommandArgument

        Me.DisplayEditPanel(ItemID)
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim ItemID As Integer = Button.CommandArgument
        If ItemID > 0 Then
            If Custom01.Delete(Custom01.GetCustomApplication(ItemID)) Then
                lblAlert.Text = "The application has been removed successfully."
                Me.DisplayListPanel()
            Else
                lblAlert.Text = "An error occurred while attempting to delete this application."
            End If
        End If
    End Sub

    Sub DisplayEditPanel(ByVal intItemID As Integer)
        Dim MyApp As Custom01 = Custom01.GetCustomApplication(intItemID)

        If MyApp.ApplicationID > 0 Then
            txtApplicationName.Text = MyApp.ResourceName
            rblIsEnabled.SelectedIndex = -1
            For Each Item As ListItem In rblIsEnabled.Items
                If Item.Value = MyApp.IsEnabled Then
                    Item.Selected = True
                    Exit For
                End If
            Next
            If MyApp.DisplayStartDate.ToShortDateString <> "1/1/1900" Then txtDisplayStartDate.Text = MyApp.DisplayStartDate
            If MyApp.DisplayEndDate.ToShortDateString <> "1/1/1900" Then txtDisplayEndDate.Text = MyApp.DisplayEndDate
            txtDescription.Text = MyApp.Description
            txtFileName.Text = MyApp.FileName
            hdnItemID.Value = intItemID
            btnFileName.OnClientClick = "return ShowFileDialog(""" & txtFileName.ClientID & """, window.document.forms[0]." & txtFileName.ClientID & ".value);"
        Else
            Me.ResetEditForm()
            hdnItemID.Value = 0
        End If

        pnlList.Visible = False
        pnlEdit.Visible = True
        txtApplicationName.Focus()
    End Sub

    Sub DisplayListPanel()
        pnlList.Visible = True
        pnlEdit.Visible = False
        gdvList.DataBind()
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DisplayListPanel()
    End Sub

    Protected Sub ResetEditForm()
        txtApplicationName.Text = ""
        rblIsEnabled.SelectedIndex = 0
        txtDisplayStartDate.Text = ""
        txtDisplayEndDate.Text = ""
        txtDescription.Text = ""
        txtFileName.Text = ""
        hdnItemID.Value = -1
    End Sub

    Protected Sub lbtnAddItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnAddItem.Click
        Me.DisplayEditPanel(0)
    End Sub

    Protected Sub btnFileName_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFileName.Load
        If Not Page.IsPostBack Then
            btnFileName.OnClientClick = "return ShowFileDialog('" & txtFileName.ClientID & "', '" & txtFileName.Text & "');"
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If PeterBlum.VAM.Globals.Page.IsValid Then
            Dim ItemID As Integer = Emagine.GetNumber(hdnItemID.Value)
            Dim MyApp As New Custom01
            If ItemID > 0 Then MyApp = Custom01.GetCustomApplication(ItemID)
            MyApp.ResourceName = txtApplicationName.Text
            MyApp.IsEnabled = rblIsEnabled.SelectedValue
            If IsDate(txtDisplayStartDate.Text) Then MyApp.DisplayStartDate = txtDisplayStartDate.Text Else MyApp.DisplayStartDate = "1/1/1900"
            If IsDate(txtDisplayEndDate.Text) Then MyApp.DisplayEndDate = txtDisplayEndDate.Text Else MyApp.DisplayEndDate = "1/1/1900"
            MyApp.Description = txtDescription.Text
            MyApp.FileName = txtFileName.Text
            MyApp.LanguageID = Session("EzEditLanguageID")
            MyApp.StatusID = Session("EzEditStatusID")
            MyApp.UpdatedBy = Session("EzEditUsername")
            MyApp.UpdatedDate = Date.Now

            If MyApp.ApplicationID > 0 Then
                If Custom01.Update(MyApp) Then
                    lblAlert.Text = "The application has been updated successfully."
                    Me.DisplayListPanel()
                Else
                    lblAlert.Text = "An error occurred while attempting to update this application."
                End If

            Else
                MyApp.CreatedBy = Session("EzEditUsername")

                If Custom01.Insert(MyApp).ApplicationID > 0 Then
                    lblAlert.Text = "The application has been added successfully."
                    Me.DisplayListPanel()
                Else
                    lblAlert.Text = "An error occurred while attempting to insert this application."
                End If

            End If
        End If
    End Sub

    
    Protected Sub gdvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ApplicationID As Integer = e.Row.DataItem("ApplicationID")
            Dim DisplayStartDate As Date = e.Row.DataItem("DisplayStartDate").ToString
            Dim DisplayEndDate As Date = e.Row.DataItem("DisplayEndDate").ToString
            Dim IsEnabled As Boolean = e.Row.DataItem("IsEnabled")

            Dim rpDisplayPages As Repeater = e.Row.FindControl("rpDisplayPages")
            Dim lblStatus As Label = e.Row.FindControl("lblStatus")
            Dim lblDisplayDates As Label = e.Row.FindControl("lblDisplayDates")

            rpDisplayPages.DataSource = PageModule.GetModulePages(_ModuleKey, "ApplicationID", ApplicationID)
            rpDisplayPages.DataBind()

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
        End If
    End Sub

    Protected Sub rpDisplayPages_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim PageModuleId As String = e.Item.DataItem("PageModuleID")
            Dim PageName As String = e.Item.DataItem("PageName").ToString
            Dim ForeignKey As String = e.Item.DataItem("ForeignKey").ToString
            Dim ForeignValue As String = e.Item.DataItem("ForeignValue").ToString

            Dim str As New StringBuilder
            str.Append("<li>")
            str.Append("<a href='../EditModuleProperties.aspx?ModuleKey=" & _ModuleKey & "&PageModuleID=" & PageModuleId & "&ForeignKey=" & ForeignKey & "&ForeignValue=" & ForeignValue & "' class='main'>")
            str.Append(PageName)
            str.Append("</a>")
            str.Append("</li>")

            Dim lbl As Label = e.Item.FindControl("lblDisplayPages")
            lbl.Text = str.ToString
        End If
    End Sub
End Class
