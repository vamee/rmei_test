Imports System.Data.SqlClient
'Imports PeterBlum.VAM
Imports PeterBlum.PetersDatePackage

Partial Class Display_Forms01_SystemForm
    Inherits System.Web.UI.UserControl

    Dim _PageModuleID As Integer = 0
    Dim _SubmissionCount As Integer = 0
    Dim PageID As Integer = 0
    Dim FormID As Integer = 0
    Dim UserID As String = Emagine.Users.User.GetUserID()
    Dim AutoPopulateForm As Boolean = False
    Dim AutoPopulateTypeID As Integer = 0
    Dim AutoSubmitForm As Boolean = False
    Dim ResourceID As String = Resources.GetResourceID()

    Public Property PageModuleID() As Integer
        Get
            Return _PageModuleID
        End Get
        Set(ByVal value As Integer)
            _PageModuleID = value
        End Set
    End Property

    Public Property SubmissionCount() As Integer
        Get
            Return _SubmissionCount
        End Get
        Set(ByVal value As Integer)
            _SubmissionCount = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        PageID = CInt(Session("PageID"))
        If ResourceID.Length > 0 Then
            Dim DbResourceID As String = Resources.GetResourceID(UserID, ResourceID)
            If DbResourceID.Length > 0 Then ResourceID = DbResourceID
        End If

        If PageID > 0 Then
            'If Len(ResourceID) > 0 Then PageModuleID = PageModule.GetPageModuleIDByResourceID(ResourceID)
            If ResourceID.Length > 0 Then
                PageModuleID = PageModule.GetPageModuleIDByResourceID(ResourceID)
                Dim DisplayPageID As Integer = -1
                Dim ModuleKey As String = Emagine.GetDbValue("SELECT ResourceType FROM Resources WHERE ResourceID = '" & ResourceID & "'")
                Dim Sql As String = ""

                Select Case ModuleKey.ToString.ToUpper
                    Case "Links01"
                        PageModuleID = PageModule.GetPageModuleIDByResourceID(ResourceID)

                    Case "DL01"
                        Sql = "SELECT ResourceName AS Title, Description FROM qryDownloads WHERE ResourceID = '" & ResourceID & "'"

                    Case "PR01"
                        Sql = "SELECT ResourceName AS Title, ArticleSummary AS Description FROM qryArticles WHERE ResourceID = '" & ResourceID & "'"

                    Case "Careers01"
                        Sql = "SELECT ResourceName AS Title, CareerSummary AS Description FROM qryCareers WHERE ResourceID = '" & ResourceID & "'"
                End Select

                If Sql.Length > 0 Then
                    Dim ResourceData As DataTable = Emagine.GetDataTable(Sql)
                    If ResourceData.Rows.Count > 0 Then
                        lblResourceTitle.Text = ResourceData.Rows(0).Item("Title")
                        lblResourceDescription.Text = ResourceData.Rows(0).Item("Description")
                        pnlResourceInfo.Visible = True
                    End If
                    ResourceData.Dispose()
                End If

            End If

            FormID = Emagine.GetNumber(Emagine.GetDbValue("SELECT ForeignValue FROM qryDisplayPageModules WHERE ModuleKey = 'Forms01' AND PageID = " & PageID))

            SubmissionCount = Forms01.Submissions.GetSubmissionCount(FormID, UserID)

            If SubmissionCount > 0 Then
                Dim FormBehavior As String = PageModuleProperty.GetProperty(PageModuleID, "SubsequentFormBehavior")

                If FormBehavior = "Pre-Populate Form" Then
                    AutoPopulateForm = True
                    AutoPopulateTypeID = 1
                ElseIf FormBehavior = "Auto Submit Form" Then
                    AutoSubmitForm = True
                End If
            Else
                Dim FormBehavior As String = PageModuleProperty.GetProperty(PageModuleID, "InitialFormBehavior")
                If FormBehavior = "Pre-Populate w/ Data from Another Instance of THIS Form" Then
                    AutoPopulateForm = True
                    AutoPopulateTypeID = 2
                ElseIf FormBehavior = "Pre-Populate w/ Data from ANY Form" Then
                    AutoPopulateForm = True
                    AutoPopulateTypeID = 3
                End If
            End If

            If AutoSubmitForm Then
                Dim FormActions As String = PageModuleProperty.GetProperty(PageModuleID, "SubsequentFormAction")
                Forms01.AutoSubmitForm(UserID, FormID, PageID, ResourceID, FormActions)
            Else
                BindRptFields(FormID)
            End If

            lblAlert.Text = Session("Alert")
            Session("Alert") = ""
        End If
    End Sub

    Sub Redirect(ByVal intFormID As Integer)
        Response.Redirect(Request.ServerVariables("SCRIPT_NAME") & "?FormID=" & intFormID)
    End Sub

    Sub BindRptFields(ByVal FormID As Integer)
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
                
                If AutoPopulateForm And AutoPopulateTypeID > 0 Then
                    If AutoPopulateTypeID = 1 Then
                        FormFields01.DisplayFormField(intFieldId, Forms01.Submissions.GetValue(PageID, intFormId, UserID, intFieldId), CType(e.Item.FindControl("plcFormField"), PlaceHolder), True)
                    ElseIf AutoPopulateTypeID = 2 Then
                        FormFields01.DisplayFormField(intFieldId, Forms01.Submissions.GetValue(0, intFormId, UserID, intFieldId), CType(e.Item.FindControl("plcFormField"), PlaceHolder), True)
                    ElseIf AutoPopulateTypeID = 3 Then
                        FormFields01.DisplayFormField(intFieldId, Forms01.Submissions.GetValueByFormFieldName(intFieldId, UserID), CType(e.Item.FindControl("plcFormField"), PlaceHolder), True)
                    End If
                Else
                    If intFieldTypeId = 1 Then
                        tdFieldLabel.Visible = False
                        tdFormField.ColSpan = 2
                    End If
                    FormFields01.DisplayFormField(intFieldId, "", CType(e.Item.FindControl("plcFormField"), PlaceHolder), True)
                End If
            End If

            'If e.Item.ItemType = ListItemType.Item Then strBgColor = "#FFFFFF" Else strBgColor = "#F3F2F7"
            'strBgColor = "#FFFFFF"

            'trForms.BgColor = strBgColor
            'trForms.Attributes("onmouseover") = "this.style.backgroundColor='#D2CEC2'"
            'trForms.Attributes("onmouseout") = "this.style.backgroundColor='" & strBgColor & "'"

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
            'Dim btnSave As PeterBlum.VAM.ImageButton = CType(e.Item.FindControl("btnSave"), PeterBlum.VAM.ImageButton)
            Dim btnSave As PeterBlum.VAM.Button = CType(e.Item.FindControl("btnSave"), PeterBlum.VAM.Button)
            Dim SubmitButtonText As String = PageModuleProperty.GetProperty(_PageModuleID, "SubmitButtonText")
            If SubmitButtonText.Length > 0 Then btnSave.Text = SubmitButtonText

            'btnSave.PostBackUrl = Request.RawUrl
            AddHandler btnSave.Click, AddressOf btnSave_Click

            Dim DisplayCaptcha As Boolean = Emagine.GetDbValue("SELECT DisplayCaptcha FROM Forms WHERE FormID = " & FormID)
            If DisplayCaptcha Then
                Dim trCaptcha As System.Web.UI.HtmlControls.HtmlTableRow = e.Item.FindControl("trCaptcha")
                trCaptcha.Visible = True
            End If

        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ValidCaptcha As Boolean = True

        Dim FormID As Integer = Emagine.GetDbValue("SELECT ForeignValue FROM qryDisplayPageModules WHERE ModuleKey = 'Forms01' AND PageID = " & PageID)
        Dim DisplayCaptcha As Boolean = Emagine.GetDbValue("SELECT DisplayCaptcha FROM Forms WHERE FormID = " & FormID)
        If DisplayCaptcha Then
            Dim Button As PeterBlum.VAM.Button = sender
            Dim Captcha As WebControlCaptcha.CaptchaControl = Button.Parent.FindControl("Captcha")
            ValidCaptcha = Captcha.UserValidated
        End If

        If ValidCaptcha Then
            If PeterBlum.VAM.Globals.Page.IsValid Then
                Dim Result As Boolean = True
                Dim FormActions As String = ""
                Dim DeliveryPageURL As String = ""

                If SubmissionCount > 0 Then
                    FormActions = PageModuleProperty.GetProperty(PageModuleID, "SubsequentFormAction")
                Else
                    FormActions = PageModuleProperty.GetProperty(PageModuleID, "InitialFormAction")
                End If

                Dim SubmissionID As Integer = Forms01.Submissions.Add(PageID, FormID, UserID)

                Resources.Resource.Register(UserID, ResourceID, SubmissionID)
                'Forms01.WriteResourceCookies(ResourceID)


                If Result = True And InStr(FormActions, "Save to DB") > 0 Then
                    Result = Forms01.SaveToDB(Page, SubmissionID)
                    Result = Forms01.SaveResourceInfoToDB(SubmissionID, ResourceID, UserID)
                End If

                If Result = True And InStr(FormActions, "Send Email") > 0 Then
                    Result = Forms01.EmailResults(SubmissionID, PageModuleID, Page, ResourceID, UserID)
                End If

                If Result = True And InStr(FormActions, "Post to SalesForce") > 0 Then
                    If GlobalVariables.GetValue("SalesForceIntegrationType") = "API" Then
                        Dim PostData As New DataTable("PostData")
                        PostData.Columns.Add(New DataColumn("FieldName", System.Type.GetType("System.String")))
                        PostData.Columns.Add(New DataColumn("FieldValue", System.Type.GetType("System.String")))

                        PostData = Forms01.GetPostData(Page, PostData)
                        'For i As Integer = 0 To PostData.Rows.Count - 1
                        '    Response.Write(PostData.Rows(i).Item(0) & " : " & PostData.Rows(i).Item(1) & "<br />")
                        'Next

                        'Response.End()
                        Result = Forms01.PostToSalesForce(PostData, UserID, ResourceID)

                    Else
                        Result = Forms01.PostToSalesForce(Forms01.GetPostData(Page))
                    End If
                End If

                'If InStr(FormActions, "Post to SugarCRM") > 0 Then
                'Forms01.PostToSugarCRM(Forms01.GetPostData(Page))
                'End If 

                If Result = True Then

                    'Emagine.Users.User.DeleteResources(UserID)

                    Dim ThankYouPageID As Integer = 0
                    Dim DeliveryPageID As Integer = 0

                    Dim SQL As String = "SELECT * FROM qryPageModuleProperties WHERE PageModuleID = " & PageModuleID & " ORDER BY SortOrder"
                    Dim Rs As Data.SqlClient.SqlDataReader = Emagine.GetDataReader(SQL)
                    Do While Rs.Read
                        Select Case Rs("PropertyName")
                            Case "ThankYouPage"
                                ThankYouPageID = Emagine.GetNumber(Rs("PropertyValue"))

                            Case "DeliveryPage"
                                DeliveryPageID = Emagine.GetNumber(Rs("PropertyValue"))
                        End Select
                    Loop
                    Rs.Close()
                    Rs = Nothing

                    If ThankYouPageID > 0 Then DeliveryPageID = ThankYouPageID

                    If DeliveryPageID > 0 Then
                        SQL = "SELECT PageKey FROM Pages WHERE PageID = " & DeliveryPageID
                        DeliveryPageURL = Emagine.GetDbValue(SQL)
                        If DeliveryPageURL.Length > 0 Then
                            DeliveryPageURL = "/" & DeliveryPageURL & ".htm"
                        End If
                    End If

                    If Len(DeliveryPageURL) > 0 Then
                        If Resources.GetResourceID().Length > 0 Then DeliveryPageURL = "/" & Resources.GetResourceID() & DeliveryPageURL

                        Response.Redirect(DeliveryPageURL)
                    End If

                Else
                    lblAlert.Text = "We're sorry. An error has occured while processing your information. Please try again later."
                End If

            End If
        Else
            lblAlert.Text = "CAPTCHA FAILED"
        End If
    End Sub
End Class