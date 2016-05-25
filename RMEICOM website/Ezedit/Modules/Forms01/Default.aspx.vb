Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Partial Class Ezedit_Modules_Forms01_Default
    Inherits System.Web.UI.Page

#Region "Common"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblPageTitle.Text = "Forms"

        lblAlert.Text = ""
    End Sub

#End Region


#Region "Display"
    Protected Sub btnEditProperties_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim ItemID As Integer = Button.CommandArgument

        Me.DisplayEditPanel(ItemID)
    End Sub

    Protected Sub btnEditForm_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim ItemID As Integer = Button.CommandArgument

        Response.Redirect("BuildForm.aspx?FormID=" & ItemID)
    End Sub

    Protected Sub btnPreviewForm_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim ItemID As Integer = Button.CommandArgument

        ucPreviewForm.FormID = ItemID
        Me.DisplayPreviewPanel()
        lblPageTitle.Text = "<a href='Default.aspx' class='pageTitle'>Forms</a> > " & Emagine.GetDbValue("SELECT FormName FROM Forms WHERE FormID = " & ItemID)
    End Sub

    Protected Sub btnCopyForm_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim ItemID As Integer = Button.CommandArgument

        Me.DisplayEditPanel(ItemID)
        hdnAction.Value = "Copy"
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim Button As ImageButton = sender
        Dim ItemID As Integer = Button.CommandArgument

        If Forms01.DeleteForm(ItemID, "Forms01") Then
            gdvList.DataBind()
            lblAlert.Text = "The form has been removed successfully."
        Else
            lblAlert.Text = "An error occurred while attempting to remove this form."
        End If
    End Sub

    Protected Sub rptDisplayPages_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim PageModuleID As String = e.Item.DataItem("PageModuleID")
            Dim PageName As String = e.Item.DataItem("PageName").ToString
            Dim ForeignKey As String = e.Item.DataItem("ForeignKey").ToString
            Dim ForeignValue As String = e.Item.DataItem("ForeignValue").ToString

            Dim str As New StringBuilder
            str.Append("<li>")
            str.Append("<a href='EditModuleProperties.aspx?PageModuleID=" & PageModuleID & "&FormID=" & ForeignValue & "' class='main'>")
            str.Append(PageName)
            str.Append("</a>")
            str.Append("</li>")

            Dim lbl As Label = e.Item.FindControl("lblDisplayPages")
            lbl.Text = str.ToString
        End If
    End Sub

    Protected Sub gdvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim FormID As Integer = e.Row.DataItem("FormID")
            Dim FormTypeID As Integer = e.Row.DataItem("FormTypeID")

            Dim rptDisplayPages As Repeater = e.Row.FindControl("rptDisplayPages")
            Dim btnEditForm As ImageButton = e.Row.FindControl("btnEditForm")
            Dim btnPreviewForm As ImageButton = e.Row.FindControl("btnPreviewForm")
            Dim btnCopyForm As ImageButton = e.Row.FindControl("btnCopyForm")

            rptDisplayPages.DataSource = Modules.GetModulePages("Forms01", FormID)
            rptDisplayPages.DataBind()

            If FormTypeID <> 1 Then
                btnEditForm.Visible = False
                btnPreviewForm.Visible = False
                btnCopyForm.Visible = False
            End If

        End If
    End Sub

    Protected Sub lbtnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnAddNew.Click
        Me.DisplayEditPanel(0)
    End Sub

    Sub DisplayListPanel()
        gdvList.DataBind()

        pnlList.Visible = True
        pnlEdit.Visible = False
        pnlPreview.Visible = False
    End Sub
#End Region


#Region "Edit"
    Sub DisplayEditPanel(ByVal intFormID As Integer)
        ddlFormTypeID.SelectedIndex = -1
        hdnItemID.Value = intFormID

        If intFormID > 0 Then
            Dim MyForm As Forms01 = Forms01.GetFormInfo(intFormID)
            If MyForm.FormID > 0 Then
                For Each Item As ListItem In ddlFormTypeID.Items
                    If Item.Value = MyForm.FormTypeID Then
                        Item.Selected = True
                        Exit For
                    End If
                Next
                txtFormName.Text = MyForm.FormName
                txtDescription.Text = MyForm.Description
                txtLabelWidth.Text = MyForm.LabelWidth
                rblDisplayCaptcha.SelectedValue = MyForm.DisplayCaptcha
            End If
        End If

        pnlList.Visible = False
        pnlEdit.Visible = True
        pnlPreview.Visible = False
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DisplayListPanel()
    End Sub

    Sub ResetEditForm()
        ddlFormTypeID.SelectedIndex = -1
        txtFormName.Text = ""
        txtDescription.Text = ""
        txtLabelWidth.Text = ""
        rblDisplayCaptcha.SelectedIndex = 1
        hdnItemID.Value = -1
        hdnAction.Value = ""
    End Sub

    Protected Sub ddlFormTypeID_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFormTypeID.Load
        If Not Page.IsPostBack Then
            ddlFormTypeID.DataSource = Forms01.GetFormTypes()
            ddlFormTypeID.DataTextField = "FormType"
            ddlFormTypeID.DataValueField = "FormTypeID"
            ddlFormTypeID.DataBind()
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If PeterBlum.VAM.Globals.Page.IsValid Then
            Dim ItemID As Integer = Emagine.GetNumber(hdnItemID.Value)

            Dim MyForm As New Forms01
            If ItemID > 0 Then MyForm = Forms01.GetFormInfo(ItemID)
            MyForm.StatusID = 20
            MyForm.LanguageID = CInt(Session("EzEditLanguageID"))
            MyForm.FormTypeID = ddlFormTypeID.SelectedValue
            MyForm.FormName = txtFormName.Text
            MyForm.Description = txtDescription.Text
            MyForm.LabelWidth = txtLabelWidth.Text
            MyForm.DisplayCaptcha = rblDisplayCaptcha.SelectedValue
            MyForm.LastUpdated = Date.Now()
            MyForm.UpdatedBy = Session("EzEditUsername")

            If ItemID = 0 Or hdnAction.Value = "Copy" Then
                Dim FormID As Integer = MyForm.AddForm(MyForm)

                If FormID > 0 Then
                    If hdnAction.Value = "Copy" Then
                        Forms01.CopyFormFields(MyForm.FormID, FormID)
                    Else
                        Forms01.CopyDefaultFormFields(FormID, MyForm.FormTypeID)
                    End If

                    Me.ResetEditForm()
                    Me.DisplayListPanel()
                    lblAlert.Text = "The form has been added successfully."

                Else
                    lblAlert.Text = "An error occurred while attempting to add this form."
                End If

            Else
                If MyForm.UpdateForm(MyForm) Then
                    Me.ResetEditForm()
                    Me.DisplayListPanel()
                    lblAlert.Text = "The form has been updated successfully."
                Else
                    lblAlert.Text = "An error occurred while attempting to update this form."
                End If
            End If
        End If
    End Sub

#End Region

#Region "Preview"
    Sub DisplayPreviewPanel()
        pnlList.Visible = False
        pnlEdit.Visible = False
        pnlPreview.Visible = True
    End Sub
#End Region

End Class
