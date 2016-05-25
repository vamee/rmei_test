Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class Forms01
    Inherits Modules

    Public FormID As Integer = -1
    Public FormTypeID As Integer = -1
    Public FormType As String = ""
    Public StatusID As Integer = -1
    Public Status As String = ""
    Public LanguageID As Integer = -1
    Public LanguageName As String = ""
    Public FormName As String = ""
    Public Description As String = ""
    Public CodeFile As String = ""
    Public LabelWidth As Integer = -1
    Public DisplayCaptcha As Boolean = False
    Public DateCreated As DateTime
    Public CreatedBy As String = ""
    Public LastUpdated As DateTime
    Public UpdatedBy As String = ""

    Public Shared Function GetForms(ByVal StatusId As Integer, ByVal LanguageId As Integer) As SqlDataReader
        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim strSQL As String = "sp_Forms01_GetAllForms"

        Dim objCommand As New SqlCommand(strSQL, objConn)
        objCommand.CommandType = CommandType.StoredProcedure
        objCommand.Parameters.AddWithValue("@StatusId", StatusId)
        objCommand.Parameters.AddWithValue("@LanguageId", LanguageId)

        objConn.Open()
        Dim objDataReader As SqlDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection)

        Return objDataReader
    End Function

    'TO BE DELETED
    'Public Shared Function GetFormPages(ByVal StatusId As Integer, ByVal LanguageId As Integer) As SqlDataReader
    'Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
    'Dim strSQL As String = "sp_Forms01_GetAllFormPages"

    'Dim objCommand As New SqlCommand(strSQL, objConn)
    '    objCommand.CommandType = CommandType.StoredProcedure
    '    objCommand.Parameters.AddWithValue("@StatusId", StatusId)
    '    objCommand.Parameters.AddWithValue("@LanguageId", LanguageId)

    '    objConn.Open()
    'Dim objDataReader As SqlDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection)

    '    Return objDataReader
    'End Function

    Public Shared Function GetFormInfo(ByVal FormId As Integer) As Forms01
        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim strSQL As String = "sp_Forms01_GetFormInfo"
        Dim objCommand As New SqlCommand(strSQL, objConn)

        objCommand.CommandType = CommandType.StoredProcedure
        objCommand.Parameters.AddWithValue("@FormID", FormId)

        Dim dtrFormData As SqlDataReader
        Dim Form As New Forms01
        Try
            objConn.Open()
            dtrFormData = objCommand.ExecuteReader(CommandBehavior.SingleRow)
            If dtrFormData.Read Then
                Form.FormID = dtrFormData("FormID")
                Form.FormTypeID = dtrFormData("FormTypeID")
                Form.FormType = dtrFormData("FormType")
                Form.StatusID = dtrFormData("StatusID")
                Form.Status = dtrFormData("Status")
                Form.LanguageID = dtrFormData("LanguageID")
                Form.LanguageName = dtrFormData("LanguageName")
                Form.FormName = dtrFormData("FormName")
                Form.Description = dtrFormData("Description").ToString
                Form.CodeFile = dtrFormData("CodeFile").ToString
                Form.LabelWidth = dtrFormData("LabelWidth")
                Form.DisplayCaptcha = dtrFormData("DisplayCaptcha")
                Form.DateCreated = dtrFormData("DateCreated")
                Form.CreatedBy = dtrFormData("CreatedBy").ToString
                Form.LastUpdated = dtrFormData("LastUpdated")
                Form.UpdatedBy = dtrFormData("UpdatedBy").ToString
            Else
                Form = Nothing
            End If
            dtrFormData.Close()
        Finally
            objConn.Close()
        End Try
        Return Form
    End Function

    Public Function AddForm(ByVal Form As Forms01) As Integer

        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim strSQL As String = "sp_Forms01_AddForm"
        Dim intFormId As Integer = 0

        Dim objCommand As New SqlCommand(strSQL, objConn)
        objCommand.CommandType = CommandType.StoredProcedure
        objCommand.Parameters.AddWithValue("@FormTypeID", Form.FormTypeId)
        objCommand.Parameters.AddWithValue("@StatusID", Form.StatusId)
        objCommand.Parameters.AddWithValue("@LanguageID", Form.LanguageId)
        objCommand.Parameters.AddWithValue("@FormName", Form.FormName)
        objCommand.Parameters.AddWithValue("@Description", Form.Description)
        objCommand.Parameters.AddWithValue("@CodeFile", Form.CodeFile)
        objCommand.Parameters.AddWithValue("@LabelWidth", Form.LabelWidth)
        objCommand.Parameters.AddWithValue("@DisplayCaptcha", Form.DisplayCaptcha)
        objCommand.Parameters.AddWithValue("@UpdatedBy", Form.UpdatedBy)
        objCommand.Parameters.AddWithValue("@CreatedBy", Form.CreatedBy)

        Try
            objConn.Open()

            Dim DataReader As SqlDataReader = objCommand.ExecuteReader()
            If DataReader.Read() Then
                intFormId = DataReader(0)
            End If
            DataReader.Close()

            'If intFormId > 0 Then CopyDefaultFormFields(intFormId, Form.FormTypeId)


        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            If objConn.State = ConnectionState.Open Then objConn.Dispose()
        End Try

        Return intFormId
    End Function


    Public Function UpdateForm(ByVal Form As Forms01) As Boolean

        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim blnResult As Boolean = False
        Dim strSQL As String = "sp_Forms01_UpdateForm"

        Dim objCommand As New SqlCommand(strSQL, objConn)
        objCommand.Parameters.AddWithValue("@FormID", Form.FormId)
        objCommand.Parameters.AddWithValue("@FormTypeID", Form.FormTypeId)
        objCommand.Parameters.AddWithValue("@StatusID", Form.StatusId)
        objCommand.Parameters.AddWithValue("@LanguageID", Form.LanguageId)
        objCommand.Parameters.AddWithValue("@FormName", Form.FormName)
        objCommand.Parameters.AddWithValue("@Description", Form.Description)
        objCommand.Parameters.AddWithValue("@CodeFile", Form.CodeFile)
        objCommand.Parameters.AddWithValue("@LabelWidth", Form.LabelWidth)
        objCommand.Parameters.AddWithValue("@DisplayCaptcha", Form.DisplayCaptcha)
        objCommand.Parameters.AddWithValue("@UpdatedBy", Form.UpdatedBy)
        objCommand.Parameters.AddWithValue("@LastUpdated", Form.LastUpdated)

        Try
            objConn.Open()
            objCommand.CommandType = CommandType.StoredProcedure
            objCommand.ExecuteNonQuery()

            blnResult = True
        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            If objConn.State = ConnectionState.Open Then objConn.Close()
        End Try

        Return blnResult
    End Function

    Public Shared Function GetFormTypes() As SqlDataReader
        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim strSQL As String = "sp_Forms01_GetAllFormTypes"
        Dim objCommand As New SqlCommand(strSQL, objConn)
        objCommand.CommandType = CommandType.StoredProcedure
        objConn.Open()
        Dim dtrFormTypes As SqlDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection)
        Return dtrFormTypes
    End Function

    Public Shared Function DeleteForm(ByVal FormID As Integer, ByVal ModuleKey As String) As Boolean
        Dim blnResult As Boolean = False
        Dim strSQL As String = "sp_Forms01_DeleteForm"
        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim objCommand As New SqlCommand()

        objCommand.Connection = objConn
        objCommand.CommandText = strSQL
        objCommand.CommandType = CommandType.StoredProcedure
        objCommand.Parameters.AddWithValue("@FormID", FormID)
        objCommand.Parameters.AddWithValue("@ModuleKey", ModuleKey)

        Try
            objConn.Open()
            objCommand.ExecuteNonQuery()

            blnResult = True
        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            If objConn.State = ConnectionState.Open Then objConn.Close()
        End Try

        Return blnResult
    End Function

    Public Shared Sub CopyDefaultFormFields(ByVal FormId As Integer, ByVal FormTypeId As Integer)
        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim strSQL As String = "sp_Forms01_CopyDefaultFormFields"

        Dim objCommand As New SqlCommand(strSQL, objConn)
        objCommand.CommandType = CommandType.StoredProcedure

        objCommand.Parameters.AddWithValue("@FormID", FormId)
        objCommand.Parameters.AddWithValue("@FormTypeID", FormTypeId)

        Try
            objConn.Open()
            objCommand.ExecuteNonQuery()

        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            If objConn.State = ConnectionState.Open Then objConn.Close()
        End Try
    End Sub

    Public Shared Sub CopyFormFields(ByVal intFromFormId As Integer, ByVal intToFormId As Integer)
        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim strSQL As String = "sp_Forms01_CopyFormFields"

        Dim objCommand As New SqlCommand(strSQL, objConn)
        objCommand.CommandType = CommandType.StoredProcedure

        objCommand.Parameters.AddWithValue("@FromFormID", intFromFormId)
        objCommand.Parameters.AddWithValue("@ToFormID", intToFormId)

        Try
            objConn.Open()
            objCommand.ExecuteNonQuery()

        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            If objConn.State = ConnectionState.Open Then objConn.Close()
        End Try
    End Sub

    Public Shared Function GetFields(ByVal FormId As Integer) As SqlDataReader
        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim strSQL As String = "sp_Forms01_GetAllFields"

        Dim objCommand As New SqlCommand(strSQL, objConn)
        objCommand.CommandType = CommandType.StoredProcedure
        objCommand.Parameters.AddWithValue("@FormId", FormId)
        objConn.Open()
        Dim objDataReader As SqlDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection)
        Return objDataReader
    End Function

    Public Shared Function BuildFieldSortOrder(ByVal ddlFields As DropDownList, ByVal intFormID As Integer, ByVal intSortOrder As Integer) As DropDownList
        Dim i As Integer
        Dim intMaxSortOrder As Integer = GetMaxFieldSortOrder(intFormID)

        For i = 0 To intMaxSortOrder - 1
            ddlFields.Items.Add(New ListItem(i + 1, i + 1))
            If intSortOrder = i + 1 Then
                ddlFields.Items(i).Selected = True
            End If
        Next
        Return ddlFields
    End Function

    Public Shared Function GetMaxFieldSortOrder(ByVal intFormID As Integer) As Integer
        Dim intMaxSortOrder As Integer

        intMaxSortOrder = GetNumber(GetDbValue("EXECUTE sp_Forms01_GetMaxFieldSortOrder " & intFormID))

        Return intMaxSortOrder
    End Function

    Public Shared Function GetFormByPageID(ByVal intPageID As Integer) As Integer
        Dim FormID As Integer = 0

    End Function


    Public Shared Function GetControlValue(ByVal objFormField As Control) As String
        Dim Result As String = ""

        If TypeOf objFormField Is TextBox Then
            Result = DirectCast(objFormField, TextBox).Text

        ElseIf TypeOf objFormField Is PeterBlum.VAM.MultiSegmentDataEntry Then
            Result = DirectCast(objFormField, PeterBlum.VAM.MultiSegmentDataEntry).Text

        ElseIf TypeOf objFormField Is DropDownList Then
            Result = DirectCast(objFormField, DropDownList).SelectedValue

        ElseIf TypeOf objFormField Is ListBox Then
            Dim i As Integer
            Dim ListBox As ListBox = DirectCast(objFormField, ListBox)
            Dim arySelectedValues As Array = ListBox.GetSelectedIndices

            For i = 0 To UBound(arySelectedValues)
                Result = Result & ListBox.Items(arySelectedValues(i)).Value
                If i < UBound(arySelectedValues) Then Result = Result & "||"
            Next

        ElseIf TypeOf objFormField Is CheckBoxList Then
            Dim CheckBoxList As CheckBoxList = DirectCast(objFormField, CheckBoxList)
            Dim Item As ListItem

            For Each Item In CheckBoxList.Items
                If Item.Selected Then
                    If Item.Value = "OtherWithText" Then
                        Result = Result & Left(Item.Text, InStr(Item.Text, "<") - 1) & " " & HttpContext.Current.Request.Form("txtOther" & CheckBoxList.ID) & "||"
                    Else
                        Result = Result & Item.Value & "||"
                    End If

                End If
            Next

        ElseIf TypeOf objFormField Is RadioButtonList Then
            Dim RadioButtonList As RadioButtonList = DirectCast(objFormField, RadioButtonList)
            Dim Item As ListItem = RadioButtonList.SelectedItem

            If Item.Value = "OtherWithText" Then
                Result = Result & Left(Item.Text, InStr(Item.Text, "<") - 1) & " " & HttpContext.Current.Request.Form("txtOther" & RadioButtonList.ID) & "||"
            Else
                Result = RadioButtonList.SelectedValue
            End If

        ElseIf TypeOf objFormField Is HiddenField Then
            Result = DirectCast(objFormField, HiddenField).Value

        ElseIf TypeOf objFormField Is FileUpload Then
            Dim Upload As FileUpload = DirectCast(objFormField, FileUpload)
            Result = Upload.FileName
            If Upload.HasFile Then
                Upload.SaveAs(HttpContext.Current.Server.MapPath(GlobalVariables.GetValue("VirtualUserUploadPath") & Upload.FileName))
            End If
        End If

        If Result.IndexOf("{#") = 0 And Result.IndexOf("#}") > 0 Then
            Dim ItemName As String = Result.Replace("{#", "").Replace("#}", "")
            Dim UserID As String = Emagine.Users.User.GetUserID()

            Dim ResourceData As DataTable = Emagine.GetDataTable("SELECT ItemValue FROM UserResources WHERE ItemName = '" & ItemName & "' AND UserID = '" & UserID & "'")
            If ResourceData.Rows.Count > 0 Then
                Result = ""
                For i As Integer = 0 To (ResourceData.Rows.Count - 1)
                    Result = Result & ResourceData.Rows(i).Item(0).ToString
                    If i < (ResourceData.Rows.Count - 1) Then Result = Result & ", "
                Next
            End If
        End If


        If Result.IndexOf("{#") = 0 And Result.IndexOf("#}") > 0 Then
            Dim CampaignKey As String = Result.Replace("{#", "").Replace("#}", "")
            Dim ReadCookie As HttpCookie = HttpContext.Current.Request.Cookies(CampaignKey)
            If ReadCookie IsNot Nothing Then
                Result = ReadCookie.Value
            End If
        End If

        If Right(Result, 2) = "||" Then
            Result = Left(Result, Result.Length - 2)
        End If

        Return Result
    End Function

    Public Shared Function AutoSubmitForm(ByVal UserID As String, ByVal FormID As Integer, ByVal PageID As Integer, ByVal ResourceID As String, ByVal FormActions As String) As Boolean



    End Function

    Public Shared Function SaveToDB(ByVal objParentControl As Control, ByVal intSubmissionID As Integer) As Boolean
        Dim Result As Boolean = True
        Dim Control As New Control()

        For Each Control In objParentControl.Controls
            If IsNumeric(Control.ID) Then
                Dim FormField As New FormFields01
                FormField = FormField.GetFormFieldInfo(Control.ID)

                Dim SubmissionItem As New Forms01.Submissions.SubmissionValue
                SubmissionItem.SubmissionID = intSubmissionID
                SubmissionItem.FormFieldID = Control.ID
                SubmissionItem.FormFieldName = ""
                SubmissionItem.FormFieldLabel = ""
                SubmissionItem.FormFieldValue = GetControlValue(Control)

                If Not Forms01.Submissions.SubmissionValue.Add(SubmissionItem) Then
                    Result = False
                    Exit For
                End If

            End If

            If Control.HasControls Then Result = SaveToDB(Control, intSubmissionID)
        Next

        Return Result
    End Function

    Public Shared Function SaveResourceInfoToDB(ByVal intSubmissionID As Integer, ByVal strResourceID As String, ByVal strUserID As String) As Boolean
        Dim Result As Boolean = True
        If Len(strResourceID) > 0 Then
            Dim aryResources As Array = strResourceID.Split("^")
            Dim i As Integer = 0
            For i = 0 To aryResources.GetUpperBound(0)

                Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
                Dim SQL As String = "sp_Users_GetResourceItems"

                Dim objCommand As New SqlCommand(SQL, Conn)
                objCommand.CommandType = System.Data.CommandType.StoredProcedure
                objCommand.Parameters.AddWithValue("@UserID", strUserID)
                objCommand.Parameters.AddWithValue("@ResourceID", aryResources(i))

                Try
                    Conn.Open()

                    Dim Rs As System.Data.SqlClient.SqlDataReader = objCommand.ExecuteReader()
                    Do While Rs.Read
                        Dim SubmissionItem As New Forms01.Submissions.SubmissionValue
                        SubmissionItem.SubmissionID = intSubmissionID
                        SubmissionItem.FormFieldID = -1
                        SubmissionItem.FormFieldName = Rs("ItemName")
                        SubmissionItem.FormFieldLabel = Rs("ItemName")
                        SubmissionItem.FormFieldValue = Rs("ItemValue")

                        If Not Forms01.Submissions.SubmissionValue.Add(SubmissionItem) Then
                            Result = False
                            Exit For
                        End If
                    Loop
                    Rs.Close()
                    Rs = Nothing

                Catch ex As Exception
                    Emagine.LogError(ex)
                Finally
                    If Conn.State = System.Data.ConnectionState.Open Then Conn.Dispose()
                End Try

            Next
        End If

        Return Result
    End Function

    Public Shared Function PostToSalesForce(ByVal strPostData As String) As Boolean
        Dim Result As Boolean = False
        Dim StatusCode As Integer = 0
        Dim Writer As System.IO.StreamWriter
        Dim WebRequest As System.Net.HttpWebRequest
        Dim PostData As String = "encoding=UTF-8" & strPostData

        WebRequest = System.Net.WebRequest.Create("http://www.salesforce.com/servlet/servlet.WebToLead?")
        WebRequest.Method = "POST"
        WebRequest.ContentLength = PostData.Length
        WebRequest.Timeout = 3000
        WebRequest.KeepAlive = False
        WebRequest.ContentType = "application/x-www-form-urlencoded"

        Try
            Writer = New System.IO.StreamWriter(WebRequest.GetRequestStream())
            Writer.Write(PostData)
            Writer.Close()

            Dim WebResponse As System.Net.HttpWebResponse = WebRequest.GetResponse()
            StatusCode = CInt(WebResponse.StatusCode)

            If WebRequest.HaveResponse Then


                Dim ReceiveStream As System.IO.Stream = WebResponse.GetResponseStream()

                Dim ReadStream As New System.IO.StreamReader(ReceiveStream, Encoding.UTF8)
                Dim ResponseText As String = ReadStream.ReadToEnd()

                WebResponse.Close()
                ReceiveStream.Close()
                ReadStream.Close()

            End If
        Catch ex As Exception
            Emagine.LogError(ex)
        End Try

        WebRequest = Nothing

        If StatusCode = 200 Then Result = True

        Return Result
    End Function

    Public Shared Function PostToSalesForce(ByVal dtPostData As DataTable, ByVal strUserID As String, ByVal strResourceID As String, Optional ByRef strError As String = "") As Boolean 'SalesForce API
        Dim ErrorMessage As String = ""
        Dim strObjectID As String = ""
        Dim Result As Boolean = False

        strObjectID = SalesForceLead.Add(dtPostData, ErrorMessage)

        If strObjectID.Length > 0 Then
            Result = True

            Dim ActivityData As DataTable = Emagine.GetDataTable("SELECT * FROM UserResources WHERE UserID = '" & strUserID & "' AND ResourceID IN ('" & strResourceID.Replace("^", "','") & "')")
            For i As Integer = 0 To (ActivityData.Rows.Count - 1)
                Dim Activity As New SalesForceTask
                Activity.WhoID = strObjectID
                Activity.Subject = ActivityData.Rows(i).Item("ItemName")
                Activity.Description = ActivityData.Rows(i).Item("ItemValue")

                Dim TaskID As String = SalesForceTask.Add(Activity, strError)

                If TaskID.Length = 0 Then
                    Result = False
                    Exit For
                End If
            Next
        End If

        Return Result
    End Function

    Public Shared Function PostToSage(ByVal strPostData As String) As Boolean
        Dim Result As Boolean = False
        Dim StatusCode As Integer = 0
        Dim Writer As System.IO.StreamWriter
        Dim WebRequest As System.Net.HttpWebRequest
        Dim PostData As String = "encoding=UTF-8" & strPostData

        WebRequest = System.Net.WebRequest.Create("http://connect.peoplecube.com/ccare/trial/emailregister.cfm?lang=en")
        WebRequest.Method = "POST"
        WebRequest.ContentLength = PostData.Length
        WebRequest.Timeout = 3000
        WebRequest.KeepAlive = False
        WebRequest.ContentType = "application/x-www-form-urlencoded"

        Try
            Writer = New System.IO.StreamWriter(WebRequest.GetRequestStream())
            Writer.Write(PostData)
            Writer.Close()

            Dim WebResponse As System.Net.HttpWebResponse = WebRequest.GetResponse()
            StatusCode = CInt(WebResponse.StatusCode)

            If WebRequest.HaveResponse Then
                Dim ReceiveStream As System.IO.Stream = WebResponse.GetResponseStream()

                Dim ReadStream As New System.IO.StreamReader(ReceiveStream, Encoding.UTF8)
                Dim ResponseText As String = ReadStream.ReadToEnd()

                WebResponse.Close()
                ReceiveStream.Close()
                ReadStream.Close()

            End If
        Catch ex As Exception
            Emagine.LogError(ex)
        End Try

        WebRequest = Nothing

        If StatusCode = 200 Then Result = True

        Return Result
    End Function

    Public Shared Function EmailResults(ByVal intSubmissionID As Integer, ByVal intPageModuleID As Integer, ByVal objParentControl As Control, ByVal strResourceID As String, ByVal strUserID As String) As Boolean
        Dim Result As Boolean = False
        Dim Message As String = ""
        Dim EmailFrom As String = ""
        Dim EmailTo As String = ""
        Dim EmailCC As String = ""
        Dim EmailBCC As String = ""
        Dim EmailSubject As String = ""
        Dim Attachments As String = ""

        Dim SQL As String = "SELECT * FROM qryPageModuleProperties WHERE PageModuleID = " & intPageModuleID & " ORDER BY SortOrder"
        Dim Rs As Data.SqlClient.SqlDataReader = Emagine.GetDataReader(SQL)
        Do While Rs.Read
            Select Case Rs("PropertyName")
                Case "EmailFrom"
                    EmailFrom = Rs("PropertyValue")

                Case "EmailTo"
                    EmailTo = Rs("PropertyValue")

                Case "EmailCC"
                    EmailCC = Rs("PropertyValue")

                Case "EmailBCC"
                    EmailBCC = Rs("PropertyValue")

                Case "EmailSubject"
                    EmailSubject = Rs("PropertyValue")

            End Select
        Loop
        Rs.Close()

        Message = "<html>"
        Message = Message & "<head>"
        Message = Message & "<BASE href=""http://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & """>"
        Message = Message & "<link rel=""stylesheet"" href=""http://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & "/Collateral/Templates/common/styles.css"">"
        Message = Message & "</head>"
        Message = Message & "<body>"
        Message = Message & GetEmailBody(objParentControl, EmailFrom)

        If strResourceID.Length > 0 Then
            Dim aryResources As Array = strResourceID.Split("^")
            Dim i As Integer = 0
            For i = 0 To aryResources.GetUpperBound(0)

                Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
                Dim objCommand As New SqlCommand("sp_Users_GetResourceItems")
                objCommand.Connection = Conn
                objCommand.CommandType = System.Data.CommandType.StoredProcedure
                objCommand.Parameters.AddWithValue("@UserID", strUserID)
                objCommand.Parameters.AddWithValue("@ResourceID", aryResources(i))

                Try
                    Conn.Open()

                    Rs = objCommand.ExecuteReader()
                    Do While Rs.Read
                        If EmailSubject.IndexOf("{#") > -1 And EmailSubject.IndexOf("#}") > 0 Then
                            Dim ItemName As String = Right(EmailSubject, EmailSubject.Length - (EmailSubject.IndexOf("{#") + 2))
                            ItemName = Left(ItemName, ItemName.IndexOf("#}"))

                            If ItemName = Rs("ItemName").ToString Then
                                EmailSubject = EmailSubject.Replace("{#" & ItemName & "#}", Rs("ItemValue").ToString)
                            End If
                        End If
                        Message = Message & "<span class='main-bold'>" & Rs("ItemName").ToString & "</span> <span class='main'>" & Rs("ItemValue").ToString & "</span><br /><br />"
                    Loop
                    Rs.Close()
                    Rs = Nothing

                Catch ex As Exception
                    Emagine.LogError(ex)
                Finally
                    If Conn.State = System.Data.ConnectionState.Open Then Conn.Dispose()
                End Try

            Next
        End If

        Message = Message & "<span class='main-bold'>Form Submitted:</span> <span class='main'>" & Now() & "</span><br><br>"
        Message = Message & "<span class='main-bold'>UserID:</span> <span class='main'>" & strUserID & "</span><br><br>"
        Message = Message & "<span class='main-bold'>IP Address:</span> <span class='main'>" & HttpContext.Current.Request.ServerVariables("REMOTE_ADDR") & "</span><br><br>"
        Message = Message & "<span class='main-bold'>Browser Info:</span> <span class='main'>" & HttpContext.Current.Request.ServerVariables("HTTP_USER_AGENT") & "</span><br>"
        Message = Message & "</body>"
        Message = Message & "</html>"

        Attachments = GetEmailAttachments(objParentControl)

        If Attachments.Length > 0 Then
            If Right(Attachments, 1) = "," Then
                Attachments = Left(Attachments, Attachments.Length - 1)
            End If
        End If

        Result = Emagine.SendEmail(EmailFrom, EmailFrom, EmailTo, EmailTo, EmailCC, EmailBCC, EmailSubject, Message, Attachments, True)

        Return Result
    End Function

    Private Shared Function GetEmailBody(ByVal objParentControl As Control, ByRef strEmailFrom As String) As String
        Dim Control As New Control()
        Dim Result As String = ""

        For Each Control In objParentControl.Controls
            If IsNumeric(Control.ID) Then
                Dim FieldLabel As String = Emagine.GetDbValue("SELECT Label FROM FormFields WHERE FieldID = " & Control.ID)
                Dim FieldName As String = Emagine.GetDbValue("SELECT FieldName FROM FormFields WHERE FieldID = " & Control.ID)
                Dim FieldValue As String = Forms01.GetControlValue(Control)

                If FieldName = "EmailFrom" Then strEmailFrom = FieldValue

                Result = "<span class='main-bold'>"
                If Len(FieldLabel) > 0 Then
                    Result = Result & FieldLabel
                Else
                    Result = Result & FieldName & ":"
                End If
                Result = Result & "</span> <span class='main'>" & FieldValue & "</span><br><br>"
            End If

            If Control.HasControls Then
                Result = Result & GetEmailBody(Control, strEmailFrom)
            End If
        Next
        Return Result
    End Function

    Private Shared Function GetEmailAttachments(ByVal objParentControl As Control) As String
        Dim Control As New Control()
        Dim Result As String = ""

        For Each Control In objParentControl.Controls
            If IsNumeric(Control.ID) Then
                Dim FieldTypeID As Integer = Emagine.GetNumber(Emagine.GetDbValue("SELECT FieldTypeID FROM FormFields WHERE FieldID = " & Control.ID))
                If FieldTypeID = 7 Then
                    Dim FieldValue As String = Forms01.GetControlValue(Control)
                    If FieldValue.Trim.Length > 0 Then
                        Result = HttpContext.Current.Server.MapPath(GlobalVariables.GetValue("VirtualUserUploadPath") & FieldValue) & ","
                    End If
                End If

            End If

            If Control.HasControls Then
                Result = Result & GetEmailAttachments(Control)
            End If
        Next

        'If Right(Result, 1) = "," Then
        '    Result = Left(Result, Result.Length - 1)
        'End If

        Return Result
    End Function

    Public Shared Sub WriteResourceCookies(ByVal strResourceID As String)
        If strResourceID.Length > 0 Then
            Dim aryResourceIDs As Array = strResourceID.Split("^")
            Dim i As Integer

            For i = 0 To UBound(aryResourceIDs)
                Resources.Resource.WriteCookie(aryResourceIDs(i))
            Next

        End If
    End Sub

    

    Public Shared Function GetPostData(ByVal objParentControl As Control) As String
        Dim Control As New Control()
        Dim Result As String = ""

        For Each Control In objParentControl.Controls
            If IsNumeric(Control.ID) Then
                Dim FieldName As String = Emagine.GetDbValue("SELECT FieldName FROM FormFields WHERE FieldID = " & Control.ID)
                Dim FieldValue As String = Forms01.GetControlValue(Control)
                If FieldName.Length > 0 Then Result = "&" & HttpContext.Current.Server.UrlEncode(FieldName) & "=" & HttpContext.Current.Server.UrlEncode(FieldValue)
            End If

            If Control.HasControls Then
                Result = Result & GetPostData(Control)
            End If
        Next
        Return Result
    End Function

    Public Shared Function GetPostData(ByVal objParentControl As Control, ByVal dtPostData As DataTable) As DataTable
        Dim Control As New Control()

        For Each Control In objParentControl.Controls
            If IsNumeric(Control.ID) Then
                Dim FieldName As String = Emagine.GetDbValue("SELECT SalesForceFieldName FROM FormFields WHERE FieldID = " & Control.ID)

                If FieldName.Length > 0 Then
                    Dim FieldValue As String = Forms01.GetControlValue(Control)
                    Dim Row As DataRow = dtPostData.NewRow()
                    Row("FieldName") = FieldName
                    Row("FieldValue") = FieldValue
                    dtPostData.Rows.Add(Row)
                End If
            End If

            If Control.HasControls Then
                dtPostData = GetPostData(Control, dtPostData)
            End If
        Next
        Return dtPostData
    End Function

    Public Shared Function GetSubmissionData(ByVal intPageID As Integer, ByVal intFormID As Integer, ByVal strUserID As String) As String
        Dim Result As String = ""

        Dim SQL As String = "SELECT * FROM FormSubmissionValues WHERE SubmissionID IN (SELECT MAX(SubmissionID) FROM FormSubmissions WHERE PageID = " & intPageID & " AND FormID = " & intFormID & " AND UserID = '" & strUserID & "') AND FormFieldID <> -1"
        Dim Rs As SqlDataReader = Emagine.GetDataReader(SQL)
        Do While Rs.Read
            Result = Result & "&" & Rs("FormFieldName") & "=" & Rs("FormFieldValue")
        Loop
        Rs.Close()

        Return Result
    End Function

    'Public Class CustomForms

    '    Public Class CustomForm

    '        Public Shared Function GetPostData() As String
    '            Dim Result As String = ""
    '            Dim i As Integer
    '            Dim FirstPass As Boolean = True

    '            For i = 0 To HttpContext.Current.Request.Form.Count - 1
    '                If Left(HttpContext.Current.Request.Form.Keys(i).ToString, 2) <> "__" And HttpContext.Current.Request.Form.Keys(i).ToString <> "search" Then

    '                    If Not FirstPass Then Result = Result & "&"
    '                    Result = Result & HttpContext.Current.Request.Form.Keys(i).ToString & "=" & HttpContext.Current.Request.Form.Item(i).ToString
    '                    FirstPass = False
    '                End If

    '            Next
    '            Return Result
    '        End Function

    '        Public Shared Function SaveToDB(ByVal intSubmissionID As Integer) As Boolean
    '            Dim Result As Boolean = True
    '            Dim i As Integer = 0

    '            For i = 0 To HttpContext.Current.Request.Form.Count - 1
    '                If Left(HttpContext.Current.Request.Form.Keys(i).ToString, 2) <> "__" And HttpContext.Current.Request.Form.Keys(i).ToString <> "Keywords" And Left(HttpContext.Current.Request.Form.Keys(i).ToString, 3) <> "ctl" Then

    '                    Dim SubmissionItem As New Forms01.Submissions.SubmissionValue
    '                    SubmissionItem.SubmissionID = intSubmissionID
    '                    SubmissionItem.FormFieldID = 0
    '                    SubmissionItem.FormFieldName = HttpContext.Current.Request.Form.Keys(i).ToString
    '                    SubmissionItem.FormFieldLabel = HttpContext.Current.Request.Form.Keys(i).ToString
    '                    SubmissionItem.FormFieldValue = HttpContext.Current.Request.Form.Item(i).ToString

    '                    If Not Forms01.Submissions.SubmissionValue.Add(SubmissionItem) Then
    '                        Result = False
    '                        Exit For
    '                    End If

    '                End If

    '            Next

    '            Return Result
    '        End Function

    '        Public Shared Function EmailResults(ByVal intPageModuleID As Integer, ByVal strResourceID As String, ByVal strUserID As String) As Boolean
    '            Dim Result As Boolean = False
    '            Dim Message As String = ""
    '            Dim EmailFrom As String = ""
    '            Dim EmailTo As String = ""
    '            Dim EmailCC As String = ""
    '            Dim EmailBCC As String = ""
    '            Dim EmailSubject As String = ""

    '            Dim SQL As String = "SELECT * FROM qryPageModuleProperties WHERE PageModuleID = " & intPageModuleID & " ORDER BY SortOrder"
    '            Dim Rs As Data.SqlClient.SqlDataReader = Emagine.GetDataReader(SQL)
    '            Do While Rs.Read
    '                Select Case Rs("PropertyName")
    '                    Case "EmailFrom"
    '                        EmailFrom = Rs("PropertyValue")

    '                    Case "EmailTo"
    '                        EmailTo = Rs("PropertyValue")

    '                    Case "EmailCC"
    '                        EmailCC = Rs("PropertyValue")

    '                    Case "EmailBCC"
    '                        EmailBCC = Rs("PropertyValue")

    '                    Case "EmailSubject"
    '                        EmailSubject = Rs("PropertyValue")

    '                End Select
    '            Loop
    '            Rs.Close()

    '            Message = "<html>"
    '            Message = Message & "<head>"
    '            Message = Message & "<BASE href=""http://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & """>"
    '            Message = Message & "<link rel=""stylesheet"" href=""http://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & "/templates/common/styles.css"">"
    '            Message = Message & "</head>"
    '            Message = Message & "<body>"
    '            Message = Message & GetEmailBody()

    '            If strResourceID.Length > 0 Then
    '                Dim aryResources As Array = strResourceID.Split("^")
    '                Dim i As Integer = 0
    '                For i = 0 To aryResources.GetUpperBound(0)

    '                    Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
    '                    Dim objCommand As New SqlCommand("sp_Users_GetResourceItems")
    '                    objCommand.Connection = Conn
    '                    objCommand.CommandType = System.Data.CommandType.StoredProcedure
    '                    objCommand.Parameters.AddWithValue("@UserID", strUserID)
    '                    objCommand.Parameters.AddWithValue("@ResourceID", aryResources(i))

    '                    Try
    '                        Conn.Open()

    '                        Rs = objCommand.ExecuteReader()
    '                        Do While Rs.Read
    '                            Message = Message & "<span class='main-bold'>" & Rs("ItemName") & "</span> <span class='main'>" & Rs("ItemValue") & "</span><br><br>"
    '                        Loop
    '                        Rs.Close()
    '                        Rs = Nothing

    '                    Catch ex As Exception
    '                        Emagine.LogError(ex)
    '                    Finally
    '                        If Conn.State = System.Data.ConnectionState.Open Then Conn.Dispose()
    '                    End Try

    '                Next
    '            End If

    '            Message = Message & "<span class='main-bold'>Form Submitted:</span> <span class='main'>" & Now() & "</span><br><br>"
    '            Message = Message & "<span class='main-bold'>UserID:</span> <span class='main'>" & strUserID & "</span><br><br>"
    '            Message = Message & "<span class='main-bold'>IP Address:</span> <span class='main'>" & HttpContext.Current.Request.ServerVariables("REMOTE_ADDR") & "</span><br><br>"
    '            Message = Message & "<span class='main-bold'>Browser Info:</span> <span class='main'>" & HttpContext.Current.Request.ServerVariables("HTTP_USER_AGENT") & "</span><br>"
    '            Message = Message & "</body>"
    '            Message = Message & "</html>"

    '            Result = Emagine.SendEmail(EmailFrom, EmailFrom, EmailTo, EmailTo, EmailCC, EmailBCC, EmailSubject, Message, "", True)

    '            Return Result
    '        End Function

    '        Private Shared Function GetEmailBody() As String
    '            Dim Control As New Control()
    '            Dim Result As String = ""
    '            Dim i As Integer = 0

    '            For i = 0 To HttpContext.Current.Request.Form.Count - 1
    '                If Left(HttpContext.Current.Request.Form.Keys(i).ToString, 2) <> "__" And HttpContext.Current.Request.Form.Keys(i).ToString <> "Keywords" And Left(HttpContext.Current.Request.Form.Keys(i).ToString, 3) <> "ctl" Then
    '                    Result = Result & "<span class='main-bold'>"
    '                    Result = Result & HttpContext.Current.Request.Form.Keys(i).ToString
    '                    Result = Result & "</span>: "
    '                    Result = Result & "<span class='main'>"
    '                    Result = Result & HttpContext.Current.Request.Form.Item(i).ToString
    '                    Result = Result & "</span>"
    '                    Result = Result & "<br><br>"
    '                End If

    '            Next

    '            Return Result
    '        End Function
    '    End Class

    'End Class

    Public Class Submissions

        Public Class SubmissionValue
            Public SubmissionID As Integer
            Public FormFieldID As Integer
            Public UserID As String
            Public FormFieldName As String
            Public FormFieldLabel As String
            Public FormFieldValue As String

            Public Shared Function Add(ByVal SubmissionLineItem As Forms01.Submissions.SubmissionValue) As Boolean
                Dim Result As Boolean = False
                Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
                Dim SQL As String = "sp_Forms01_AddSubmissionItem"

                Dim objCommand As New SqlCommand(SQL, Conn)
                objCommand.CommandType = CommandType.StoredProcedure
                objCommand.Parameters.AddWithValue("@SubmissionID", SubmissionLineItem.SubmissionID)
                objCommand.Parameters.AddWithValue("@FormFieldID", SubmissionLineItem.FormFieldID)
                objCommand.Parameters.AddWithValue("@FormFieldName", SubmissionLineItem.FormFieldName)
                objCommand.Parameters.AddWithValue("@FormFieldLabel", SubmissionLineItem.FormFieldLabel)
                objCommand.Parameters.AddWithValue("@FormFieldValue", SubmissionLineItem.FormFieldValue)

                Try
                    Conn.Open()

                    Result = objCommand.ExecuteNonQuery()

                Catch ex As Exception
                    Emagine.LogError(ex)
                Finally
                    If Conn.State = ConnectionState.Open Then Conn.Dispose()
                End Try

                Return Result
            End Function

        End Class


        Public Shared Function Add(ByVal intPageID As Integer, ByVal intFormID As Integer, ByVal strUserID As String) As Integer
            Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
            Dim SQL As String = "sp_Forms01_AddSubmission"
            Dim SubmissionID As Integer = 0

            Dim objCommand As New SqlCommand(SQL, Conn)
            objCommand.CommandType = CommandType.StoredProcedure
            objCommand.Parameters.AddWithValue("@PageID", intPageID)
            objCommand.Parameters.AddWithValue("@FormID", intFormID)
            objCommand.Parameters.AddWithValue("@UserID", strUserID)

            Try
                Conn.Open()

                Dim DataReader As SqlDataReader = objCommand.ExecuteReader()
                If DataReader.Read() Then
                    SubmissionID = DataReader(0)
                End If
                DataReader.Close()

            Catch ex As Exception
                Emagine.LogError(ex)
            Finally
                If Conn.State = ConnectionState.Open Then Conn.Dispose()
            End Try

            Return SubmissionID
        End Function

        Public Shared Function Copy(ByVal intPageID As Integer, ByVal intFormID As Integer, ByVal strUserID As String) As Integer
            'RETURNS THE NEW SUBMISSION ID
            Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
            Dim SQL As String = "sp_Forms01_CopySubmission"
            Dim SubmissionID As Integer = 0

            Dim objCommand As New SqlCommand(SQL, Conn)
            objCommand.CommandType = CommandType.StoredProcedure
            objCommand.Parameters.AddWithValue("@PageID", intPageID)
            objCommand.Parameters.AddWithValue("@FormID", intFormID)
            objCommand.Parameters.AddWithValue("@UserID", strUserID)

            Try
                Conn.Open()

                Dim DataReader As SqlDataReader = objCommand.ExecuteReader()
                If DataReader.Read() Then
                    SubmissionID = DataReader(0)
                End If
                DataReader.Close()

            Catch ex As Exception
                Emagine.LogError(ex)
            Finally
                If Conn.State = ConnectionState.Open Then Conn.Dispose()
            End Try

            Return SubmissionID
        End Function

        Public Shared Function GetSubmissionCount(ByVal intFormID As Integer, ByVal strUserID As String) As Integer
            Dim Result As Integer = 0
            Dim SQL As String = "SELECT Count(SubmissionID) FROM FormSubmissions WHERE FormID = " & intFormID & " AND UserID = '" & strUserID & "'"
            Result = CInt(Emagine.GetDbValue(SQL))

            Return Result
        End Function

        Public Shared Function GetValue(ByVal intPageID As Integer, ByVal intFormID As Integer, ByVal strUserID As String, ByVal intFormFieldID As Integer) As String
            Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
            Dim SQL As String = "sp_Forms01_GetSubmissionValue"
            Dim Result As String = ""

            Dim objCommand As New SqlCommand(SQL, Conn)
            objCommand.CommandType = CommandType.StoredProcedure
            objCommand.Parameters.AddWithValue("@PageID", intPageID)
            objCommand.Parameters.AddWithValue("@FormID", intFormID)
            objCommand.Parameters.AddWithValue("@UserID", strUserID)
            objCommand.Parameters.AddWithValue("@FormFieldID", intFormFieldID)

            Try
                Conn.Open()

                Dim DataReader As SqlDataReader = objCommand.ExecuteReader()
                If DataReader.Read() Then
                    Result = DataReader("FormFieldValue")
                End If
                DataReader.Close()

            Catch ex As Exception
                Emagine.LogError(ex)
            Finally
                If Conn.State = ConnectionState.Open Then Conn.Dispose()
            End Try

            Return Result
        End Function

        Public Shared Function GetValueByFormFieldName(ByVal intFormFieldID As Integer, ByVal strUserID As String) As String
            Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
            Dim SQL As String = "sp_Forms01_GetSubmissionValueByFormFieldName"
            Dim Result As String = ""

            Dim objCommand As New SqlCommand(SQL, Conn)
            objCommand.CommandType = CommandType.StoredProcedure
            objCommand.Parameters.AddWithValue("@UserID", strUserID)
            objCommand.Parameters.AddWithValue("@FormFieldID", intFormFieldID)

            Try
                Conn.Open()

                Dim DataReader As SqlDataReader = objCommand.ExecuteReader()
                If DataReader.Read() Then Result = DataReader("FormFieldValue")
                DataReader.Close()

            Catch ex As Exception
                Emagine.LogError(ex)
            Finally
                If Conn.State = ConnectionState.Open Then Conn.Dispose()
            End Try

            Return Result
        End Function

        Public Shared Function EmailResults(ByVal intPageModuleID As Integer, ByVal strResourceID As String, ByVal intPageID As Integer, ByVal intFormID As Integer, ByVal strUserID As String) As Boolean
            Dim Result As Boolean = False
            Dim Message As String = ""
            Dim EmailFrom As String = ""
            Dim EmailTo As String = ""
            Dim EmailCC As String = ""
            Dim EmailBCC As String = ""
            Dim EmailSubject As String = ""

            Dim SQL As String = "SELECT * FROM qryPageModuleProperties WHERE PageModuleID = " & intPageModuleID & " ORDER BY SortOrder"
            Dim Rs As Data.SqlClient.SqlDataReader = Emagine.GetDataReader(SQL)
            Do While Rs.Read
                Select Case Rs("PropertyName")
                    Case "EmailFrom"
                        EmailFrom = Rs("PropertyValue")

                    Case "EmailTo"
                        EmailTo = Rs("PropertyValue")

                    Case "EmailCC"
                        EmailCC = Rs("PropertyValue")

                    Case "EmailBCC"
                        EmailBCC = Rs("PropertyValue")

                    Case "EmailSubject"
                        EmailSubject = Rs("PropertyValue")

                End Select
            Loop
            Rs.Close()

            Message = "<html>"
            Message = Message & "<head>"
            Message = Message & "<BASE href=""http://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & """>"
            Message = Message & "<link rel=""stylesheet"" href=""http://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & "/Collateral/Templates/common/styles.css"">"
            Message = Message & "</head>"
            Message = Message & "<body>"

            SQL = "SELECT * FROM FormSubmissionValues WHERE SubmissionID IN (SELECT MAX(SubmissionID) FROM FormSubmissions WHERE PageID = " & intPageID & " AND FormID = " & intFormID & " AND UserID = '" & strUserID & "') AND FormFieldID <> -1"
            Rs = Emagine.GetDataReader(SQL)
            Do While Rs.Read
                If Not IsDBNull(Rs("FormFieldLabel")) Then
                    Message = Message & "<span class='main-bold'>" & Rs("FormFieldLabel") & "</span> "
                Else
                    Message = Message & "<span class='main-bold'>" & Rs("FormFieldName") & ":</span> "
                End If
                Message = Message & "<span class='main'>" & Rs("FormFieldValue") & "</span><br><br>"
            Loop
            Rs.Close()

            If strResourceID.Length > 0 Then
                Dim aryResources As Array = strResourceID.Split("^")
                Dim i As Integer = 0
                For i = 0 To aryResources.GetUpperBound(0)

                    Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
                    Dim objCommand As New SqlCommand("sp_Users_GetResourceItems")
                    objCommand.Connection = Conn
                    objCommand.CommandType = System.Data.CommandType.StoredProcedure
                    objCommand.Parameters.AddWithValue("@UserID", strUserID)
                    objCommand.Parameters.AddWithValue("@ResourceID", aryResources(i))

                    Try
                        Conn.Open()

                        Rs = objCommand.ExecuteReader()
                        Do While Rs.Read
                            Message = Message & "<span class='main-bold'>" & Rs("ItemName") & "</span> <span class='main'>" & Rs("ItemValue") & "</span><br><br>"
                        Loop
                        Rs.Close()
                        Rs = Nothing

                    Catch ex As Exception
                        Emagine.LogError(ex)
                    Finally
                        If Conn.State = System.Data.ConnectionState.Open Then Conn.Dispose()
                    End Try

                Next
            End If

            Message = Message & "<span class='main-bold'>Form Submitted:</span> <span class='main'>" & Now() & "</span><br><br>"
            Message = Message & "<span class='main-bold'>UserID:</span> <span class='main'>" & strUserID & "</span><br><br>"
            Message = Message & "<span class='main-bold'>IP Address:</span> <span class='main'>" & HttpContext.Current.Request.ServerVariables("REMOTE_ADDR") & "</span><br><br>"
            Message = Message & "<span class='main-bold'>Browser Info:</span> <span class='main'>" & HttpContext.Current.Request.ServerVariables("HTTP_USER_AGENT") & "</span><br>"
            Message = Message & "</body>"
            Message = Message & "</html>"

            Result = Emagine.SendEmail(EmailFrom, EmailFrom, EmailTo, EmailTo, EmailCC, EmailBCC, EmailSubject, Message, "", True)

            Return Result
        End Function

    End Class

End Class