Imports PeterBlum.VAM

Partial Class Ezedit_Modules_PR01_EditModuleProperties
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim strModuleKey As String = ""
        Dim strForeignKey As String = ""
        Dim intForeignValue As Integer = 0
        Dim intPageModuleId As Integer = 0

        If Not IsPostBack Then
            strModuleKey = Trim(Request("ModuleKey"))
            strForeignKey = Trim(Request("ForeignKey"))
            intForeignValue = CInt(Request("ForeignValue"))
            intPageModuleId = CInt(Request("PageModuleId"))

            ViewState.Add("ModuleKey", strModuleKey)
            ViewState.Add("ForeignKey", strForeignKey)
            ViewState.Add("ForeignValue", intForeignValue)
            ViewState.Add("PageModuleId", intPageModuleId)
        Else
            strModuleKey = ViewState("ModuleKey")
            strForeignKey = ViewState("ForeignKey")
            intForeignValue = CInt(ViewState("ForeignValue"))
            intPageModuleId = CInt(ViewState("PageModuleId"))
        End If

        Dim strModuleName As String = Modules.GetModuleName(strModuleKey)

        If strModuleKey = "Forms01" Then
            lblModuleName.Text = "<a href='" & strModuleKey & "/Default.aspx' class='breadcrumb'>" & strModuleName & "</a>"
            Dim FormTypeID As Integer = Emagine.GetNumber(Emagine.GetDbValue("SELECT FormTypeID FROM Forms WHERE FormID = " & intForeignValue))

            If FormTypeID = 1 Or FormTypeID = 3 Then
                Me.BindRptFields(strModuleKey)
            End If

        ElseIf strModuleKey = "Custom01" Then
            lblModuleName.Text = "<a href='" & strModuleKey & "/Default.aspx' class='breadcrumb'>" & strModuleName & "</a>"

        Else
            lblModuleName.Text = "<a href='/ezedit/modules/Default.aspx?ModuleKey=" & strModuleKey & "' class='breadcrumb'>" & strModuleName & "</a>"
            Me.BindRptFields(strModuleKey)
        End If

        If intPageModuleId > 0 Then
            Me.Edit(intPageModuleId, strModuleKey, intForeignValue)
        Else
            Me.Add()
        End If

        btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete?');")

            'Response.Write("SELECT * FROM ModuleProperties WHERE ModuleKey = '" & strModuleKey & "' ORDER BY SortOrder")
    End Sub

    Sub BindRptFields(ByVal strModuleKey As String)
        Dim dtrFields As Data.SqlClient.SqlDataReader = Modules.GetModuleProperties(strModuleKey)
        If dtrFields.HasRows Then
            rptFields.DataSource = dtrFields
            rptFields.DataBind()
            rptFields.EnableViewState = False
        Else
            rptFields.Visible = True
        End If
        dtrFields.Close()
    End Sub

    Sub Add()
        lblPageTitle.Text = " > Add Module to Page"
    End Sub

    Sub Edit(ByVal intPageModuleId As Integer, ByVal strModuleKey As String, ByVal intCategoryId As String)
        lblPageTitle.Text = " > Edit Module Properties"
        Dim PageModule As New PageModule
        'Response.Write(intPageModuleId)
        PageModule = PageModule.GetModulePage(intPageModuleId)
    End Sub

    Protected Sub rptFields_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptFields.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim strModuleKey As String = DataBinder.Eval(e.Item.DataItem, "ModuleKey")
            Dim intPropertyId As Integer = CInt(DataBinder.Eval(e.Item.DataItem, "PropertyId"))
            Dim intPropertyTypeId As Integer = CInt(DataBinder.Eval(e.Item.DataItem, "PropertyTypeID"))
            Dim strPropertyName As String = DataBinder.Eval(e.Item.DataItem, "PropertyName")
            Dim strHelpText As String = DataBinder.Eval(e.Item.DataItem, "HelpText").ToString()
            Dim strSelectedValue As String = Modules.GetPropertyValue(ViewState("PageModuleId"), intPropertyId)
            'Response.Write(intPropertyId & " - " & intPropertyTypeId & " - " & strPropertyName & " - " & strSelectedValue & "<br>")

            Dim strLabel As String = DataBinder.Eval(e.Item.DataItem, "DisplayName").ToString
            Dim strLabelCss As String = "form-label"
            Dim tdFieldLabel As HtmlTableCell = CType(e.Item.FindControl("tdFieldLabel"), HtmlTableCell)
            Dim tdFormField As HtmlTableCell = CType(e.Item.FindControl("tdFormField"), HtmlTableCell)
            Dim trForms As HtmlTableRow = CType(e.Item.FindControl("trForms"), HtmlTableRow)
            Dim lblFieldLabel As Label = CType(e.Item.FindControl("lblFieldLabel"), Label)
            Dim plcFormField As PlaceHolder = CType(e.Item.FindControl("plcFormField"), PlaceHolder)
            Dim strBgColor As String

            'FormFields01.DisplayFormField(intPropertyId, Request(intPropertyId), CType(e.Item.FindControl("plcFormField"), PlaceHolder), True)
            Dim Formfield As New Control

            If intPropertyTypeId = 0 Then
                Dim lbl As New Label
                tdFieldLabel.Visible = False
                tdFormField.ColSpan = 2
                lbl.Text = DataBinder.Eval(e.Item.DataItem, "HelpText")
                Formfield = lbl
            Else
                lblFieldLabel.Text = strLabel & ":"
                lblFieldLabel.CssClass = "form_label"
            End If

            Select Case intPropertyTypeId
                Case 0
                    Formfield = FormFields01.GetContentBlock(intPropertyId, strHelpText)

                Case 1
                    Formfield = FormFields01.GetTextBox(intPropertyId, strSelectedValue, 300, 20, "form", 50)
                    Formfield.ID = intPropertyId
                Case 2, 3
                    Formfield = Modules.GetPropertyOptions(intPropertyTypeId, strPropertyName, intPropertyId, strSelectedValue, "main")
                    Formfield.ID = intPropertyId
                Case 101
                    Dim ddlDetailsPageId As New DropDownList
                    ddlDetailsPageId.AppendDataBoundItems = True
                    ddlDetailsPageId.Items.Add(New ListItem("<-- Please Choose -->", 0))
                    Formfield = Pages01.PopulatePageOptionsDDL(ddlDetailsPageId, 0, 0, 0, Session("EzEditStatusID"), Session("EzEditLanguageID"))
                    ddlDetailsPageId.SelectedValue = strSelectedValue
                    Formfield.ID = intPropertyId
                    'Dim Validator As New PeterBlum.VAM.RequiredListValidator
                    'Validator.ID = "rlv" & intPropertyId
                    'Validator.ControlIDToEvaluate = intPropertyId
                    'Validator.UnassignedIndex = 0
                    'Validator.SummaryErrorMessage = strLabel & " is required."
                    'plcFormField.Controls.Add(Validator)
                Case 102
                    Dim ddlRegistrationFormPages As New DropDownList
                    ddlRegistrationFormPages.CssClass = "main"
                    ddlRegistrationFormPages.Items.Add(New ListItem("None", 0))
                    ddlRegistrationFormPages.AppendDataBoundItems = True

                    Dim dtrFormPages As Data.SqlClient.SqlDataReader = Pages01.GetFormPages(2, Emagine.GetNumber(Session("EzEditStatusID")), Emagine.GetNumber(Session("EzEditLanguageID")))
                    If dtrFormPages.HasRows Then
                        ddlRegistrationFormPages.DataSource = dtrFormPages

                        ddlRegistrationFormPages.DataValueField = "PageID"
                        ddlRegistrationFormPages.DataTextField = "PageName"
                        'ddlRegistrationFormPages.SelectedValue = strSelectedValue
                        ddlRegistrationFormPages.DataBind()
                    End If
                    dtrFormPages.Close()

                    For i As Integer = 0 To ddlRegistrationFormPages.Items.Count - 1
                        If ddlRegistrationFormPages.Items.Item(i).Value = strSelectedValue Then
                            ddlRegistrationFormPages.SelectedValue = strSelectedValue
                            Exit For
                        End If
                    Next

                    Formfield = ddlRegistrationFormPages
                    'Formfield = Modules.GetFormPageOptionsDDL(strSelectedValue, False, "main")
                    Formfield.ID = intPropertyId
                    'Case Else
                    '   Formfield = FormFields01.GetTextBox(intPropertyId, "OK", 300, 20, "form", 50)
            End Select

            plcFormField.Controls.Add(Formfield)

            If e.Item.ItemType = ListItemType.Item Then strBgColor = "#FFFFFF" Else strBgColor = "#F3F2F7"

            trForms.BgColor = strBgColor
            trForms.Attributes("onmouseover") = "this.style.backgroundColor='#D2CEC2'"
            trForms.Attributes("onmouseout") = "this.style.backgroundColor='" & strBgColor & "'"

        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        PeterBlum.VAM.Globals.Page.Validate()
        If PeterBlum.VAM.Globals.Page.IsValid Then

            Dim PageModuleID As Integer = CInt(ViewState("PageModuleId"))

            Dim PageModule As New PageModule
            PageModule.PageModuleId = PageModuleID
            PageModule.PageID = 0
            PageModule.CodeFileID = 0
            PageModule.ModuleKey = ViewState("ModuleKey")
            PageModule.ForeignKey = ViewState("ForeignKey")
            PageModule.ForeignValue = ViewState("ForeignValue")
            PageModule.SortOrder = 0

            If PageModuleID > 0 Then
                PageModule.Update(PageModule)
                PageModuleProperty.DeleteAll(PageModuleID)
                Session("Alert") = "The module properties for this page have been updated succesfully."
            Else
                PageModuleID = PageModule.Insert(PageModule)
                Session("Alert") = "The module properties for this page have been added succesfully."
            End If

            If PageModuleID > 0 Then
                Me.InsertModuleProperties(Page, PageModuleID)
                Redirect()
            Else
                lblAlert.Text = "There was a problem saving page module data."
            End If

        End If
    End Sub

    Public Sub InsertModuleProperties(ByVal objParentControl As Control, ByVal intPageModuleID As Integer)
        Dim Control As New Control()

        For Each Control In objParentControl.Controls
            If IsNumeric(Control.ID) Then
                If TypeOf Control Is TextBox Then
                    'Response.Write(Control.ID & " : " & DirectCast(Control, TextBox).Text & " : " & Control.Parent.GetType().FullName & " : " & Control.Parent.ID & "<br>")
                    PageModuleProperty.Insert(intPageModuleID, Control.ID, DirectCast(Control, TextBox).Text)

                ElseIf TypeOf Control Is DropDownList Then
                    'Response.Write(Control.ID & " : " & DirectCast(Control, DropDownList).SelectedValue & "<br>")
                    PageModuleProperty.Insert(intPageModuleID, Control.ID, DirectCast(Control, DropDownList).SelectedValue)

                ElseIf TypeOf Control Is ListBox Then
                    Dim i As Integer
                    Dim ListBox As ListBox = DirectCast(Control, ListBox)
                    Dim arySelectedValues As Array = ListBox.GetSelectedIndices
                    Dim PropertyValue As String = ""

                    For i = 0 To UBound(arySelectedValues)
                        PropertyValue = PropertyValue & ListBox.Items(arySelectedValues(i)).Value
                        If i < UBound(arySelectedValues) Then PropertyValue = PropertyValue & "||"
                    Next

                    PageModuleProperty.Insert(intPageModuleID, Control.ID, PropertyValue)

                ElseIf TypeOf Control Is CheckBoxList Then
                    Dim CheckBoxList As CheckBoxList = DirectCast(Control, CheckBoxList)
                    Dim Item As ListItem
                    Dim PropertyValue As String = ""

                    For Each Item In CheckBoxList.Items
                        If Item.Selected Then
                            PropertyValue = PropertyValue & Item.Value & "||"
                        End If
                    Next

                    PageModuleProperty.Insert(intPageModuleID, Control.ID, PropertyValue)

                ElseIf TypeOf Control Is RadioButtonList Then
                    'Response.Write(Control.ID & " : " & DirectCast(Control, RadioButtonList).SelectedItem.ToString & "<br>")
                    PageModuleProperty.Insert(intPageModuleID, Control.ID, DirectCast(Control, RadioButtonList).SelectedItem.ToString)

                ElseIf TypeOf Control Is HiddenField Then
                    'Response.Write(Control.ID & " : " & DirectCast(Control, HiddenField).Value & "<br>")
                    PageModuleProperty.Insert(intPageModuleID, Control.ID, DirectCast(Control, HiddenField).Value)
                End If
            End If

            If Control.HasControls Then InsertModuleProperties(Control, intPageModuleID)
        Next
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Redirect()
    End Sub

    Sub OnItemCommand(ByVal Src As Object, ByVal Args As RepeaterCommandEventArgs)
        'Dim intUserId As HiddenField = Args.Item.FindControl("hidUserId")
        If Args.CommandName = "Delete" Then
            'If EzeditUser.DeleteUser(intUserId.Value) Then
            ' lblAlert.Text = "User Deleted"
            'Me.BindRpUsers()
            'Else
            '    lblAlert.Text = "Error Deleting User"
            'End If
        End If
    End Sub


    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim PageModuleID As Integer = CInt(ViewState("PageModuleId"))
        Dim SQL As String

        SQL = "DELETE FROM PageModuleProperties WHERE PageModuleID = " & PageModuleID
        Emagine.ExecuteSQL(SQL)

        SQL = "DELETE FROM PageModules WHERE PageModuleID = " & PageModuleID
        Emagine.ExecuteSQL(SQL)

        Redirect()
    End Sub

    Sub Redirect()
        Response.Redirect("Default.aspx")
    End Sub
End Class
