Imports System.Data.SqlClient
'Imports PeterBlum.VAM
Imports PeterBlum.PetersDatePackage

Partial Class Display_Forms01_SystemForm
    Inherits System.Web.UI.UserControl

    Dim _FormID As Integer = 0

    Public Property FormID() As Integer
        Get
            Return _FormID
        End Get
        Set(ByVal value As Integer)
            _FormID = value
            hdnFormID.Value = value
        End Set
    End Property

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.BindRptFields()
    End Sub
    
    Sub BindRptFields()
        Dim dtrFields As Data.SqlClient.SqlDataReader
        dtrFields = Forms01.GetFields(FormID)
        If dtrFields.HasRows Then
            rptFields.DataSource = dtrFields
            rptFields.DataBind()
            rptFields.EnableViewState = False
        Else
            rptFields.Visible = True
        End If
    End Sub

    Protected Sub rptFields_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptFields.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim intFormId As Integer = CInt(DataBinder.Eval(e.Item.DataItem, "FormID"))
            Dim intFieldId As Integer = CInt(DataBinder.Eval(e.Item.DataItem, "FieldID"))
            Dim intFieldTypeId As Integer = CInt(DataBinder.Eval(e.Item.DataItem, "FieldTypeID"))
            Dim strLabel As String = DataBinder.Eval(e.Item.DataItem, "Label").ToString
            Dim strLabelCss As String = DataBinder.Eval(e.Item.DataItem, "LabelCss").ToString
            Dim intLabelWidth As Integer = DataBinder.Eval(e.Item.DataItem, "LabelWidth")
            Dim intMinRequired As Integer = DataBinder.Eval(e.Item.DataItem, "MinRequired")
            Dim strValidationCss As String = "red-star" 'DataBinder.Eval(e.Item.DataItem, "ValidationCss")
            Dim tdFieldLabel As HtmlTableCell = CType(e.Item.FindControl("tdFieldLabel"), HtmlTableCell)
            Dim tdFormField As HtmlTableCell = CType(e.Item.FindControl("tdFormField"), HtmlTableCell)
            Dim trForms As HtmlTableRow = CType(e.Item.FindControl("trForms"), HtmlTableRow)
            Dim lblFieldLabel As Label = CType(e.Item.FindControl("lblFieldLabel"), Label)
            Dim strBgColor As String = ""

            If intFieldTypeId = 8 Then 'HIDDEN FORM FIELD
                FormFields01.DisplayFormField(intFieldId, "", CType(e.Item.FindControl("plcFormField"), PlaceHolder), False)
            Else
                If intFieldTypeId = 1 Then
                    tdFieldLabel.Visible = False
                    tdFormField.ColSpan = 2
                End If
                FormFields01.DisplayFormField(intFieldId, "", CType(e.Item.FindControl("plcFormField"), PlaceHolder), True)
            End If

            If intMinRequired > 0 Then strLabel = strLabel & "<span class='" & strValidationCss & "'>*</span>"

            tdFieldLabel.Width = intLabelWidth
            tdFieldLabel.Align = "Right"
            lblFieldLabel.Text = strLabel
            lblFieldLabel.CssClass = strLabelCss

        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim intFormId As Integer = CInt(Request("FormID"))
            Dim lblTableHeader As New Label
            lblTableHeader.Text = Emagine.GetDbValue("SELECT FormName FROM Forms WHERE FormID = " & intFormId).ToString
            Dim plcTableHeader As PlaceHolder = CType(e.Item.FindControl("plcTableHeader"), PlaceHolder)
            plcTableHeader.Controls.Add(lblTableHeader)

        ElseIf e.Item.ItemType = ListItemType.Footer Then
            Dim DisplayCaptcha As Boolean = Emagine.GetDbValue("SELECT DisplayCaptcha FROM Forms WHERE FormID = " & FormID)
            If DisplayCaptcha Then
                Dim trCaptcha As System.Web.UI.HtmlControls.HtmlTableRow = e.Item.FindControl("trCaptcha")
                trCaptcha.Visible = True
            End If

        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim pnlList As Panel = Me.Parent.Parent.FindControl("pnlList")
        Dim pnlEdit As Panel = Me.Parent.Parent.FindControl("pnlEdit")
        Dim pnlPreview As Panel = Me.Parent.Parent.FindControl("pnlPreview")

        pnlList.Visible = True
        pnlEdit.Visible = False
        pnlPreview.Visible = False
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        FormID = hdnFormID.Value
        Me.BindRptFields()

        lblAlert.Text = "The form was submitted successfully."
    End Sub
End Class