Imports PeterBlum.VAM

Partial Class Ezedit_Modules_PR01_EditModuleProperties
    Inherits System.Web.UI.Page

    Dim _ModuleKey As String = ""
    Dim _ForeignKey As String = ""
    Dim _ForeignValue As Integer = 0
    Dim _PageModuleID As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request("ModuleKey") IsNot Nothing Then _ModuleKey = Request("ModuleKey")
        If Request("ForeignKey") IsNot Nothing Then _ForeignKey = Request("ForeignKey")
        If Request("ForeignValue") IsNot Nothing Then _ForeignValue = Emagine.GetNumber(Request("ForeignValue"))
        If Request("PageModuleID") IsNot Nothing Then _PageModuleID = Emagine.GetNumber(Request("PageModuleID"))

        If Not IsPostBack Then
            Dim strModuleName As String = Modules.GetModuleName(_ModuleKey)

            If _ModuleKey = "Custom01" Then
                lblModuleName.Text = "<a href='" & _ModuleKey & "/Default.aspx' class='breadcrumb'>" & strModuleName & "</a>"

            Else
                lblModuleName.Text = "<a href='/ezedit/modules/Default.aspx?ModuleKey=" & _ModuleKey & "' class='breadcrumb'>" & strModuleName & "</a>"
            End If
        Else
            If ddlCodeFileID.SelectedIndex > 0 Then
                rptFields.DataSource = Modules.GetModuleProperties(_ModuleKey, 0, ddlCodeFileID.SelectedValue)
                rptFields.DataBind()
                rptFields.EnableViewState = False
            End If
        End If

        ddlDisplayPageId = Pages01.PopulatePageOptionsDDL(ddlDisplayPageId, 0, 0, 0, Session("EzEditStatusID"), Session("EzEditLanguageID"))

        If _PageModuleID > 0 Then
            Me.Edit(_PageModuleID, _ModuleKey, _ForeignValue)
        Else
            Me.Add()
        End If
    End Sub

    Sub Add()
        lblPageTitle.Text = " > Add Module to Page"
    End Sub

    Sub Edit(ByVal intPageModuleID As Integer, ByVal strModuleKey As String, ByVal intCategoryID As String)
        lblPageTitle.Text = " > Edit Module Properties"
        Dim PageModule As New PageModule

        PageModule = PageModule.GetModulePage(intPageModuleID)
        If Not IsPostBack Then
            ddlDisplayPageId.SelectedValue = PageModule.PageID
            ddlCodeFileID.SelectedValue = PageModule.CodeFileID
            txtSortOrder.Text = PageModule.SortOrder

            rptFields.DataSource = Modules.GetModuleProperties(_ModuleKey, 0, PageModule.CodeFileID)
            rptFields.DataBind()
            rptFields.EnableViewState = False
        End If
    End Sub

    Protected Sub rptFields_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptFields.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim strModuleKey As String = DataBinder.Eval(e.Item.DataItem, "ModuleKey")
            Dim intPropertyID As Integer = CInt(DataBinder.Eval(e.Item.DataItem, "PropertyId"))
            Dim intPropertyTypeId As Integer = CInt(DataBinder.Eval(e.Item.DataItem, "PropertyTypeID"))
            Dim strPropertyName As String = DataBinder.Eval(e.Item.DataItem, "PropertyName")
            Dim strHelpText As String = DataBinder.Eval(e.Item.DataItem, "HelpText").ToString()
            Dim strSelectedValue As String = Modules.GetPropertyValue(_PageModuleID, intPropertyId)
            Dim strLabel As String = DataBinder.Eval(e.Item.DataItem, "DisplayName").ToString
            Dim IsRequired As Boolean = e.Item.DataItem("IsRequired")
            Dim strLabelCss As String = "form-label"

            Dim tdFieldLabel As HtmlTableCell = CType(e.Item.FindControl("tdFieldLabel"), HtmlTableCell)
            Dim tdFormField As HtmlTableCell = CType(e.Item.FindControl("tdFormField"), HtmlTableCell)
            Dim trForms As HtmlTableRow = CType(e.Item.FindControl("trForms"), HtmlTableRow)
            Dim lblFieldLabel As Label = CType(e.Item.FindControl("lblFieldLabel"), Label)
            Dim plcFormField As PlaceHolder = CType(e.Item.FindControl("plcFormField"), PlaceHolder)

            Dim Formfield As New Control

            If intPropertyTypeId = 0 Then
                Dim lbl As New Label
                tdFieldLabel.Visible = False
                tdFormField.ColSpan = 2
                lbl.Text = DataBinder.Eval(e.Item.DataItem, "HelpText")
                Formfield = lbl
            Else
                lblFieldLabel.Text = strLabel & ":"
                lblFieldLabel.CssClass = "form-label"
            End If

            Select Case intPropertyTypeId
                Case 0
                    Formfield = FormFields01.GetContentBlock(intPropertyId, strHelpText)

                Case 1
                    Formfield = FormFields01.GetTextBox(intPropertyId, strSelectedValue, 300, 20, "form", 50)
                    Formfield.ID = intPropertyID

                    If IsRequired Then
                        Dim Validator As New PeterBlum.VAM.RequiredTextValidator
                        Validator.ControlIDToEvaluate = intPropertyID
                        Validator.SummaryErrorMessage = strLabel & " is required."
                        plcFormField.Controls.Add(Validator)
                    End If

                Case 2, 3
                    Formfield = Modules.GetPropertyOptions(intPropertyTypeId, strPropertyName, intPropertyId, strSelectedValue, "main")
                    Formfield.ID = intPropertyID

                    

                Case 101
                    Dim ddlDetailsPageId As New DropDownList
                    ddlDetailsPageId.AppendDataBoundItems = True
                    ddlDetailsPageId.Items.Add(New ListItem("<-- Please Choose -->", -1))
                    Formfield = Pages01.PopulatePageOptionsDDL(ddlDetailsPageId, 0, 0, 0, Session("EzEditStatusID"), Session("EzEditLanguageID"))
                    ddlDetailsPageId.SelectedValue = strSelectedValue
                    Formfield.ID = intPropertyID

                    If IsRequired Then
                        Dim Validator As New PeterBlum.VAM.RequiredListValidator
                        Validator.ID = "rlv" & intPropertyID
                        Validator.ControlIDToEvaluate = intPropertyID
                        Validator.UnassignedIndex = 0
                        Validator.SummaryErrorMessage = strLabel & " is required."
                        plcFormField.Controls.Add(Validator)
                    End If

                Case 102
                    Dim ddlRegistrationFormPages As New DropDownList
                    ddlRegistrationFormPages.CssClass = "form-textbox"
                    ddlRegistrationFormPages.Items.Add(New ListItem("None", 0))
                    ddlRegistrationFormPages.AppendDataBoundItems = True

                    Dim dtrFormPages As Data.SqlClient.SqlDataReader = Pages01.GetFormPages(2, Emagine.GetNumber(Session("EzEditStatusID")), Emagine.GetNumber(Session("EzEditLanguageID")))
                    If dtrFormPages.HasRows Then
                        ddlRegistrationFormPages.DataSource = dtrFormPages

                        ddlRegistrationFormPages.DataValueField = "PageID"
                        ddlRegistrationFormPages.DataTextField = "PageName"
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
                    Formfield.ID = intPropertyID
                    
                Case 200
                    Dim ddlLookup As New DropDownList
                    ddlLookup.AppendDataBoundItems = True
                    ddlLookup.CssClass = "form-dropdown"

                    If IsRequired Then
                        ddlLookup.Items.Add(New ListItem("<- Please Choose ->", -1))
                        Dim Validator As New PeterBlum.VAM.RequiredListValidator
                        Validator.ControlIDToEvaluate = intPropertyID
                        Validator.UnassignedIndex = 0
                        Validator.SummaryErrorMessage = strLabel & " is required."
                        plcFormField.Controls.Add(Validator)
                    End If

                    ddlLookup.DataSource = Emagine.GetLookupOptions(strPropertyName)
                    ddlLookup.DataValueField = "OptionValue"
                    ddlLookup.DataTextField = "OptionText"
                    ddlLookup.DataBind()

                    For i As Integer = 0 To ddlLookup.Items.Count - 1
                        If ddlLookup.Items.Item(i).Value = strSelectedValue Then
                            ddlLookup.SelectedValue = strSelectedValue
                            Exit For
                        End If
                    Next

                    Formfield = ddlLookup
                    Formfield.ID = intPropertyID
            End Select

            plcFormField.Controls.Add(Formfield)
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        PeterBlum.VAM.Globals.Page.Validate()
        If PeterBlum.VAM.Globals.Page.IsValid Then

            Dim PageModule As New PageModule
            PageModule.PageModuleId = _PageModuleID
            PageModule.PageID = ddlDisplayPageId.SelectedValue
            PageModule.CodeFileID = ddlCodeFileID.SelectedValue
            PageModule.ModuleKey = _ModuleKey
            PageModule.ForeignKey = _ForeignKey
            PageModule.ForeignValue = _ForeignValue
            PageModule.SortOrder = Emagine.GetNumber(txtSortOrder.Text)

            If _PageModuleID > 0 Then
                PageModule.Update(PageModule)
                PageModuleProperty.DeleteAll(_PageModuleID)
                Session("Alert") = "The module properties for this page have been updated succesfully."
            Else
                _PageModuleID = PageModule.Insert(PageModule)
                Session("Alert") = "The module properties for this page have been added succesfully."
            End If

            If _PageModuleID > 0 Then
                Me.InsertModuleProperties(Page, _PageModuleID)
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
        Dim SQL As String = ""

        SQL = "DELETE FROM PageModuleProperties WHERE PageModuleID = " & _PageModuleID
        Emagine.ExecuteSQL(SQL)

        SQL = "DELETE FROM PageModules WHERE PageModuleID = " & _PageModuleID
        Emagine.ExecuteSQL(SQL)

        Redirect()
    End Sub

    Sub Redirect()
        If _ModuleKey = "Forms01" Or _ModuleKey = "Custom01" Then
            Response.Redirect(_ModuleKey & "/Default.aspx")
        Else
            Response.Redirect("/ezedit/modules/Default.aspx?ModuleKey=" & _ModuleKey)
        End If
    End Sub

    Protected Sub ddlCodeFileID_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCodeFileID.Load
        If Not Page.IsPostBack Then
            Dim Sql As String = "SELECT CodeFileID, Description FROM ModuleCodeFiles WHERE ModuleKey = '" & _ModuleKey & "'"
            ddlCodeFileID.DataSource = Emagine.GetDataTable(Sql)
            ddlCodeFileID.DataValueField = "CodeFileID"
            ddlCodeFileID.DataTextField = "Description"
            ddlCodeFileID.DataBind()
        End If
    End Sub

    


End Class
