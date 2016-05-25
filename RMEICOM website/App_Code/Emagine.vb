Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.IO
Imports System.Web


Public Class Emagine

    Public Shared Function GetSiteMapData() As DataTable
        Dim SQL As String = "SELECT PageID, PageName, PageKey, ParentPageID FROM Pages WHERE ParentPageID IN (SELECT PageID FROM Pages WHERE StatusID = 20) OR ParentPageID = 0"

        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SqlAdapter As New SqlDataAdapter(SQL, Conn)
        Dim Rs As New DataSet()
        SqlAdapter.Fill(Rs)

        For Each Row As DataRow In Rs.Tables(0).Rows
            If Emagine.GetNumber(Row.Item("ParentPageID").ToString) = 0 Then
                Row.Item("ParentPageID") = DBNull.Value
            End If
        Next

        Return Rs.Tables(0)
    End Function

    Public Shared Function GetPageKey() As String
        Dim RequestUrl As String = HttpContext.Current.Request.RawUrl.ToString.Replace("/print", "")
        Dim QueryString As String = HttpContext.Current.Request.QueryString.ToString

        Dim PageKey As String = RequestUrl.Replace("?" & QueryString, "")
        Dim FileExtension As String = Emagine.GetFileExtension(PageKey)
        PageKey.Replace(FileExtension, "")

        Return PageKey
    End Function

    Public Shared Function GetPageFileName() As String
        Dim RequestUrl As String = HttpContext.Current.Request.RawUrl.ToString
        Dim QueryString As String = HttpContext.Current.Request.QueryString.ToString

        Dim PageKey As String = RequestUrl.Replace("?" & QueryString, "")

        Return PageKey
    End Function

    Public Shared Function GetMimeType(ByVal strFileExtension As String) As String
        Dim SQL As String = "SELECT MimeType FROM MimeTypes WHERE FileExtension = '" & strFileExtension & "'"
        Dim Result As String = Emagine.GetDbValue(SQL)

        Return Result
    End Function

    Public Shared Function PopulateBooleanRBL(ByVal rbl As RadioButtonList) As RadioButtonList
        rbl.Items.Add(New ListItem("Yes", True))
        rbl.Items.Add(New ListItem("No", False))
        Return rbl
    End Function

    Public Shared Sub SelectRadioButtonListValues(ByVal rbl As RadioButtonList, ByVal cdValues As String)
        Dim ctr As Integer
        For ctr = 0 To rbl.Items.Count - 1
            If InStr(cdValues, rbl.Items(ctr).Value) > 0 Then
                rbl.Items(ctr).Selected() = True
            End If
        Next
    End Sub

    Public Shared Sub SelectDropDownListValues(ByVal ddl As DropDownList, ByVal cdValues As String)
        Dim ctr As Integer
        For ctr = 0 To ddl.Items.Count - 1
            If InStr(cdValues, ddl.Items(ctr).Value) > 0 Then
                ddl.Items(ctr).Selected() = True
            End If
        Next
    End Sub

    Public Shared Sub SelectListBoxValues(ByVal lst As ListBox, ByVal cdValues As String)
        Dim ctr As Integer
        For ctr = 0 To lst.Items.Count - 1
            If InStr(cdValues, lst.Items(ctr).Value) > 0 Then
                lst.Items(ctr).Selected() = True
            End If
        Next
    End Sub

    Public Shared Sub LogError(ByVal ex As Exception)
        Dim SqlBuilder As New StringBuilder
        SqlBuilder.Append("INSERT INTO ErrorLog ")
        SqlBuilder.Append("(ErrorDate,Message,Source,StackTrace,Data) ")
        SqlBuilder.Append("VALUES ")
        SqlBuilder.Append("(@ErrorDate, @Message, @Source, @StackTrace, @Data)")

        Dim DataBuilder As New StringBuilder
        If ex.Data IsNot Nothing Then
            For Each DictEntry As DictionaryEntry In ex.Data
                DataBuilder.AppendLine(DictEntry.Key & ": " & DictEntry.Value)
            Next
        End If

        Dim Command As New SqlCommand
        Command.Parameters.AddWithValue("@ErrorDate", Now())
        Command.Parameters.AddWithValue("@Message", ex.Message)
        Command.Parameters.AddWithValue("@Source", ex.Source)
        Command.Parameters.AddWithValue("@StackTrace", ex.StackTrace)
        Command.Parameters.AddWithValue("@Data", DataBuilder.ToString)

        Emagine.ExecuteSQL(SqlBuilder.ToString, Command)
    End Sub

    Public Shared Sub LogError(ByVal strMessage As String, ByVal strSource As String, ByVal strStackTrace As String)
        Dim SqlBuilder As New StringBuilder
        SqlBuilder.Append("INSERT INTO ErrorLog ")
        SqlBuilder.Append("(ErrorDate,Message,Source,StackTrace) ")
        SqlBuilder.Append("VALUES ")
        SqlBuilder.Append("(@ErrorDate, @Message, @Source, @StackTrace)")

        Dim Command As New SqlCommand
        Command.Parameters.AddWithValue("@ErrorDate", Now())
        Command.Parameters.AddWithValue("@Message", strMessage)
        Command.Parameters.AddWithValue("@Source", strSource)
        Command.Parameters.AddWithValue("@StackTrace", strStackTrace)

        Emagine.ExecuteSQL(SqlBuilder.ToString, Command)
    End Sub

    Public Shared Function GetDataSet(ByVal strSQL As String) As DataSet
        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim objAdapter As New SqlDataAdapter(strSQL, objConn)
        Dim objRs As New DataSet()

        Try
            objAdapter.Fill(objRs, "Data")

        Catch ex As Exception
            ex.HelpLink = "Calling Page: " & HttpContext.Current.Request.ServerVariables("SCRIPT_NAME") & vbCrLf & "SQL: " & strSQL
            Emagine.LogError(ex)

        End Try

        Return objRs
    End Function

    Public Shared Function GetDataSet(ByVal strSQL As String, ByVal objCommand As SqlCommand, Optional ByRef strErrorMessage As String = "") As DataSet
        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)

        Dim MyCommand As SqlCommand = objCommand
        MyCommand.Connection = objConn
        MyCommand.CommandText = strSQL

        Dim objAdapter As New SqlDataAdapter(MyCommand)

        Dim objRs As New DataSet()

        Try
            objAdapter.Fill(objRs, "Data")

        Catch ex As Exception
            ex.HelpLink = "Calling Page: " & HttpContext.Current.Request.ServerVariables("SCRIPT_NAME") & vbCrLf & "SQL: " & strSQL
            Emagine.LogError(ex)

        End Try

        Return objRs
    End Function

    Public Shared Function GetDataTable(ByVal strSQL As String) As DataTable
        Dim MyDataTable As New DataTable
        Dim MyDataSet As DataSet = GetDataSet(strSQL)
        If MyDataSet.Tables.Count > 0 Then MyDataTable = MyDataSet.Tables(0)

        Return MyDataTable
    End Function

    Public Shared Function GetDataTable(ByVal strSQL As String, ByVal objCommand As SqlCommand, Optional ByRef strErrorMessage As String = "") As DataTable
        Dim MyDataTable As New DataTable
        Dim MyDataSet As DataSet = GetDataSet(strSQL, objCommand, strErrorMessage)
        If MyDataSet.Tables.Count > 0 Then MyDataTable = MyDataSet.Tables(0)

        Return MyDataTable
    End Function

    Public Shared Function GetDbValue(ByVal strSQL As String, Optional ByRef strErrorMessage As String = "") As String
        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim objCommand As New SqlCommand(strSQL, objConn)
        Dim strResult As String = ""

        Try
            objConn.Open()
            Dim dtrRs As SqlDataReader = objCommand.ExecuteReader(CommandBehavior.SingleResult)
            If dtrRs.Read Then
                If Not IsDBNull(dtrRs(0)) Then strResult = dtrRs(0).ToString()
            End If
        Catch ex As Exception
            ex.HelpLink = "Calling Page: " & HttpContext.Current.Request.ServerVariables("SCRIPT_NAME") & vbCrLf & "SQL: " & strSQL
            Emagine.LogError(ex)
            strErrorMessage = ex.Message
        Finally
            objConn.Close()
        End Try
        Return strResult
    End Function

    Public Shared Function GetDbValue(ByVal strSQL As String, ByVal objCommand As SqlCommand, Optional ByRef strErrorMessage As String = "") As String
        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim strResult As String = ""
        objCommand.CommandText = strSQL
        objCommand.Connection = objConn
        Try
            objConn.Open()
            Dim dtrRs As SqlDataReader = objCommand.ExecuteReader(CommandBehavior.SingleResult)
            If dtrRs.Read Then
                If Not IsDBNull(dtrRs(0)) Then strResult = dtrRs(0).ToString()
            End If
        Catch ex As Exception
            ex.HelpLink = "Calling Page: " & HttpContext.Current.Request.ServerVariables("SCRIPT_NAME") & vbCrLf & "SQL: " & strSQL
            Emagine.LogError(ex)
            strErrorMessage = ex.Message
        Finally
            objConn.Close()
        End Try
        Return strResult
    End Function

    Public Shared Function GetFileContents(ByVal strFilePath As String) As String
        Dim Reader As New StreamReader(File.Open(strFilePath, FileMode.Open))
        Dim FileContent As String = Reader.ReadToEnd()
        Reader.Close()
        Return FileContent
    End Function

    Public Shared Function GetNumber(ByVal varNumber As String) As Integer
        Dim intResult As Integer

        'varNumber = varNumber.ToString()

        If String.IsNullOrEmpty(varNumber) Or Len(varNumber) = 0 Then
            intResult = 0
        ElseIf (varNumber.ToUpper = "TRUE") Or (varNumber.ToUpper = "YES") Then
            intResult = 1
        ElseIf (varNumber.ToUpper = "FALSE") Or (varNumber.ToUpper = "NO") Then
            intResult = 0
        ElseIf IsNumeric(varNumber) Then
            intResult = CInt(varNumber)
        Else
            intResult = 0
        End If

        Return intResult
    End Function

    Public Shared Function GetDataReader(ByVal strSQL As String) As SqlDataReader
        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim objCommand As New SqlCommand(strSQL, objConn)
        Dim dtrResults As SqlDataReader = Nothing

        Try
            objConn.Open()
            dtrResults = objCommand.ExecuteReader(CommandBehavior.CloseConnection)
        Catch ex As Exception
            ex.HelpLink = "Calling Page: " & HttpContext.Current.Request.ServerVariables("SCRIPT_NAME") & vbCrLf & "SQL: " & strSQL
            Emagine.LogError(ex)
        End Try

        Return dtrResults
        If objConn.State = ConnectionState.Open Then objConn.Close()
    End Function

    Public Shared Function GetDataReader(ByVal strSQL As String, ByVal objCommand As SqlCommand, Optional ByRef strErrorMessage As String = "") As SqlDataReader
        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        objCommand.Connection = objConn
        objCommand.CommandText = strSQL
        objCommand.CommandType = CommandType.Text

        Dim dtrResults As SqlDataReader = Nothing

        Try
            objConn.Open()
            dtrResults = objCommand.ExecuteReader(CommandBehavior.CloseConnection)
        Catch ex As Exception
            ex.HelpLink = "Calling Page: " & HttpContext.Current.Request.ServerVariables("SCRIPT_NAME") & vbCrLf & "SQL: " & strSQL
            Emagine.LogError(ex)
            strErrorMessage = ex.Message
        End Try

        Return dtrResults
        If objConn.State = ConnectionState.Open Then objConn.Close()
    End Function

    Public Shared Function SendEmail(ByVal strFromAddress As String, ByVal strFromName As String, ByVal strToAddress As String, _
                                ByVal strToName As String, ByVal strEmailCC As String, ByVal strEmailBCC As String, _
                                ByVal strSubject As String, ByVal strMessage As String, _
                                ByVal strAttachments As String, ByVal blnIsBodyHTML As Boolean) As Boolean

        Dim Result As Boolean = False
        Dim Msg As New MailMessage
        'Dim MailObj As New SmtpClient
        'MailObj.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis
        'Dim Msg As New MailMessage
        Dim MailObj As New SmtpClient(GlobalVariables.GetValue("EmailServer"))

        Msg.From = New MailAddress(strFromAddress, strFromName)
        Msg.To.Add(New MailAddress(strToAddress, strToName))
        If Len(strEmailCC) > 0 Then
            Msg.CC.Add(New MailAddress(strEmailCC, strEmailCC))
        End If
        If Len(strEmailBCC) > 0 Then
            Msg.Bcc.Add(New MailAddress(strEmailBCC, strEmailBCC))
        End If

        If strAttachments.Length > 0 Then
            Dim aryAttachments As Array = strAttachments.Split(",")
            Dim i As Integer
            For i = 0 To UBound(aryAttachments)
                Msg.Attachments.Add(New System.Net.Mail.Attachment(aryAttachments(i)))
            Next
        End If

        Msg.Subject = strSubject
        Msg.Body = strMessage
        Msg.IsBodyHtml = blnIsBodyHTML

        Try
            MailObj.Send(Msg)
            Result = True
        Catch ex As Exception
            Emagine.LogError(ex)
        End Try

        Msg.Dispose()
        Msg = Nothing
        MailObj = Nothing

        Return Result
    End Function

    Public Shared Function SendEmail(ByVal strFromAddress As String, ByVal strFromName As String, ByVal strReplyToAddress As String, ByVal strReplyToName As String, ByVal strToAddress As String, _
                                ByVal strToName As String, ByVal strEmailCC As String, ByVal strEmailBCC As String, _
                                ByVal strSubject As String, ByVal strMessage As String, _
                                ByVal strAttachments As String, ByVal blnIsBodyHTML As Boolean) As Boolean

        Dim Result As Boolean = False
        Dim Msg As New MailMessage
        Dim MailObj As New SmtpClient
        MailObj.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis

        Msg.From = New MailAddress(strFromAddress, strFromName)
        Msg.ReplyTo = New MailAddress(strReplyToAddress, strReplyToName)
        Msg.To.Add(New MailAddress(strToAddress, strToName))
        If Len(strEmailCC) > 0 Then
            Msg.CC.Add(New MailAddress(strEmailCC, strEmailCC))
        End If
        If Len(strEmailBCC) > 0 Then
            Msg.Bcc.Add(New MailAddress(strEmailBCC, strEmailBCC))
        End If

        If strAttachments.Length > 0 Then
            Dim aryAttachments As Array = strAttachments.Split(",")
            Dim i As Integer
            For i = 0 To UBound(aryAttachments)
                Msg.Attachments.Add(New System.Net.Mail.Attachment(aryAttachments(i)))
            Next
        End If

        Msg.Subject = strSubject
        Msg.Body = strMessage
        Msg.IsBodyHtml = blnIsBodyHTML

        Try
            MailObj.Send(Msg)
            Result = True
        Catch ex As Exception
            Emagine.LogError(ex)
        End Try

        Msg.Dispose()
        Msg = Nothing
        MailObj = Nothing

        Return Result
    End Function

    Public Shared Sub CloseReload()
        Dim strJScript As String
        strJScript = "<script language='javascript'>"
        strJScript += "window.opener.location.reload();"
        strJScript += "window.close();"
        strJScript += "</script>"
        HttpContext.Current.Response.Write(strJScript)
    End Sub

    Public Shared Sub CloseWindow()
        Dim strJScript As String
        strJScript = "<script language='javascript'>"
        strJScript += "window.close();"
        strJScript += "</script>"
        HttpContext.Current.Response.Write(strJScript)
    End Sub

    Public Shared Sub GoBack()
        Dim JavaScript As New StringBuilder
        JavaScript.Append("<script language='javascript'>")
        JavaScript.Append("window.history.go(-1);")
        JavaScript.Append("</script>")
        HttpContext.Current.Response.Write(JavaScript.ToString)
        HttpContext.Current.Response.End()
    End Sub

    Public Shared Function GetMd5Hash(ByVal input As String) As String
        Dim md5Hasher As New System.Security.Cryptography.MD5CryptoServiceProvider()
        Dim data As Byte() = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input))
        Dim sBuilder As New StringBuilder()
        Dim i As Integer

        For i = 0 To data.Length - 1
            sBuilder.Append(data(i).ToString("x2"))
        Next i

        Return sBuilder.ToString()

    End Function

    Public Shared Function ExecuteSQL(ByVal strSQL As String, Optional ByRef strErrorMessage As String = "") As Boolean
        Dim blnResult As Boolean = False
        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim objCommand As New SqlCommand(strSQL, objConn)

        Try
            objConn.Open()
            blnResult = objCommand.ExecuteNonQuery()
        Catch ex As Exception
            ex.HelpLink = "Calling Page: " & HttpContext.Current.Request.ServerVariables("SCRIPT_NAME") & vbCrLf & "SQL: " & strSQL
            Emagine.LogError(ex)
            strErrorMessage = ex.Message
        Finally
            If objConn.State = ConnectionState.Open Then objConn.Close()
        End Try

        Return blnResult
    End Function

    Public Shared Function ExecuteSql(ByVal strSql As String, ByVal objCommand As SqlCommand, Optional ByRef strErrorMessage As String = "") As Boolean
        Dim Result As Boolean = False
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        objCommand.Connection = Conn
        objCommand.CommandText = strSql

        Try
            Conn.Open()
            Result = objCommand.ExecuteNonQuery()
        Catch ex As Exception
            ex.HelpLink = "Calling Page: " & HttpContext.Current.Request.ServerVariables("SCRIPT_NAME") & vbCrLf & "SQL: " & strSql
            strErrorMessage = ex.Message
            Emagine.LogError(ex)
        Finally
            If Conn.State = ConnectionState.Open Then Conn.Close()
        End Try

        Return Result
    End Function

    Public Shared Function GetUniqueID() As String
        Dim Result As String

        Result = GetDbValue("SELECT NewID() AS UniqueID")

        Return Result
    End Function

    Public Shared Function GetFiles(ByVal strDirectoryPath As String) As Array
        Dim FileList As Array
        Dim Directory As New DirectoryInfo(strDirectoryPath)

        FileList = Directory.GetFiles()

        Return FileList
    End Function

    Public Shared Function FormatFileSize(ByVal dblFileSize As Double) As String
        Dim Result As String = ""

        If IsNumeric(dblFileSize) Then
            If (dblFileSize >= 1073741824) Then
                Result = CDbl(System.Math.Round(dblFileSize / 1073741824 * 100, 0) / 100) & " GB"
            ElseIf (dblFileSize >= 1048576) Then
                Result = CDbl(System.Math.Round(dblFileSize / 1048576 * 100, 0) / 100) & " MB"
            ElseIf (dblFileSize >= 1024) Then
                Result = CDbl(System.Math.Round(dblFileSize / 1024 * 100, 0) / 100) & " KB"
            Else
                Result = CDbl(dblFileSize) & " Bytes"
            End If
        Else
            Result = "0 Bytes"
        End If

        Return Result
    End Function

    Public Shared Function FormatFileName(ByVal strFileName As String) As String
        If Len(strFileName) > 0 Then
            If InStr(strFileName, "/") Then
                strFileName = Mid(strFileName, InStrRev(strFileName, "/") + 1)
            Else
                strFileName = Mid(strFileName, InStrRev(strFileName, "\") + 1)
            End If

            strFileName = Replace(strFileName, " ", "")
            strFileName = Replace(strFileName, "-", "_")
            strFileName = Replace(strFileName, "/", "_")
            strFileName = Replace(strFileName, Chr(34), "")
            strFileName = Replace(strFileName, "'", "")
        End If

        Return strFileName
    End Function

    Public Shared Function GetFileExtension(ByVal strFileName As String) As String
        Dim Result As String = ""

        If Not IsDBNull(strFileName) Then
            Result = Right(strFileName, Len(strFileName) - (InStrRev(strFileName, ".") - 1))
        End If

        Return Result
    End Function

    Public Shared Function DeleteFile(ByVal strFileName As String) As Boolean
        Dim File As FileInfo = New FileInfo(strFileName)
        If File.Exists Then File.Delete()
    End Function

    Public Shared Function GetFileSize(ByVal strFileName As String) As Long
        Dim FileSize As Long = 0
        Dim File As FileInfo = New FileInfo(strFileName)
        If File.Exists Then
            FileSize = File.Length
        End If
        Return FileSize
    End Function

    Public Shared Sub AlternateEzeditRepeaterRow(ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs, ByVal strRowName As String)
        Dim tr As HtmlTableRow = CType(e.Item.FindControl(strRowName), HtmlTableRow)
        Dim trBgColor As String
        If e.Item.ItemType = ListItemType.Item Then
            trBgColor = "#FFFFFF"
        Else
            trBgColor = "#F3F2F7"
        End If
        tr.BgColor = trBgColor
        tr.Attributes("onmouseover") = "this.style.backgroundColor='#D2CEC2'"
        tr.Attributes("onmouseout") = "this.style.backgroundColor='" & trBgColor & "'"
    End Sub

    Public Shared Function BuildSortOrderDDL(ByVal ddl As DropDownList, ByVal intFieldCount As Integer, ByVal FieldId As Integer, ByVal intSelectedIndex As Integer) As DropDownList
        Dim i As Integer
        For i = 1 To intFieldCount
            ddl.Items.Add(New ListItem(i, i))
            If i = intSelectedIndex Then
                ddl.Items(i - 1).Selected = True
            End If
        Next
        Return ddl
    End Function

    Public Shared Sub DisplayLookupOptions(ByVal intLookupID As Integer, ByVal strSelectedValue As String)
        Dim SQL As String = "SELECT * FROM LookupOptions WHERE LookupID = " & intLookupID
        Dim Rs As SqlDataReader = Emagine.GetDataReader(SQL)
        Do While Rs.Read
            If Rs("OptionValue") = strSelectedValue Then
                HttpContext.Current.Response.Write("<option value='" & Rs("OptionValue").ToString & "' selected>" & Rs("OptionText").ToString & "</option>")
            Else
                HttpContext.Current.Response.Write("<option value='" & Rs("OptionValue").ToString & "'>" & Rs("OptionText").ToString & "</option>")
            End If

        Loop
        Rs.Close()
    End Sub

    Public Shared Function GetLookupOptions(ByVal intLookupID As Integer) As SqlDataReader
        Dim SQL As String = "SELECT OptionText, OptionValue FROM LookupOptions WHERE LookupID = " & intLookupID & " ORDER BY SortOrder"
        Return Emagine.GetDataReader(SQL)
    End Function

    Public Shared Function GetLookupOptions(ByVal strLookupName As String) As SqlDataReader
        Dim SQL As String = "SELECT OptionText, OptionValue FROM LookupOptions WHERE LookupID IN (SELECT LookupID FROM Lookups WHERE LookupName = '" & strLookupName & "') ORDER BY SortOrder"
        Return Emagine.GetDataReader(SQL)
    End Function

    Public Class UserControls
        Public Shared Function GetPropertyValue(ByVal objControl As Control, ByVal strPropertyName As String) As String
            Dim _Result As String = ""
            Dim MyProperty As System.Reflection.PropertyInfo = Nothing
            Dim MyIndex As Object = Nothing

            MyProperty = objControl.GetType().GetProperty(strPropertyName)
            If Not MyProperty Is Nothing Then _Result = MyProperty.GetValue(objControl, MyIndex).ToString

            Return _Result
        End Function

        Public Shared Sub SetPropertyValue(ByVal objControl As Control, ByVal strPropertyName As String, ByVal varPropertyValue As Boolean)
            Dim MyProperty As System.Reflection.PropertyInfo
            MyProperty = objControl.GetType().GetProperty(strPropertyName)
            MyProperty.SetValue(objControl, varPropertyValue, Nothing)

        End Sub

        Public Shared Sub SetPropertyValue(ByVal objControl As Control, ByVal strPropertyName As String, ByVal varPropertyValue As String)
            Dim MyProperty As System.Reflection.PropertyInfo
            MyProperty = objControl.GetType().GetProperty(strPropertyName)
            MyProperty.SetValue(objControl, varPropertyValue, Nothing)
        End Sub

        Public Shared Sub SetPropertyValue(ByVal objControl As Control, ByVal strPropertyName As String, ByVal varPropertyValue As Integer)
            Dim MyProperty As System.Reflection.PropertyInfo
            MyProperty = objControl.GetType().GetProperty(strPropertyName)
            MyProperty.SetValue(objControl, varPropertyValue, Nothing)
        End Sub

        Public Shared Sub SetPropertyValue(ByVal objControl As Control, ByVal strPropertyName As String, ByVal varPropertyValue As Date)
            Dim MyProperty As System.Reflection.PropertyInfo
            MyProperty = objControl.GetType().GetProperty(strPropertyName)
            MyProperty.SetValue(objControl, varPropertyValue, Nothing)
        End Sub

    End Class

    Public Class Users
        Public Shared Function GetUsers() As SqlDataReader
            Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
            Dim SQL As String = "sp_Users_GetAllUsers"
            Dim UserRs As SqlDataReader = Nothing

            Dim Cmd As New SqlCommand(SQL, Conn)
            Cmd.CommandType = CommandType.StoredProcedure

            Try
                Conn.Open()
                UserRs = Cmd.ExecuteReader()

            Catch ex As Exception
                Emagine.LogError(ex)
            Finally
                Conn.Close()
            End Try

            Return UserRs
        End Function

        Public Class User
            Public UserID As String
            Public FirstLogin As Date
            Public LastLogin As Date
            Public LoginCount As Integer

            Public Shared Function GetUser(ByVal strUserID As String) As Emagine.Users.User
                Dim MyUser As New Emagine.Users.User
                Dim Rs As SqlDataReader = Emagine.GetDataReader("SELECT * FROM Users WHERE UserID = '" & strUserID & "'")

                If Rs.HasRows Then
                    MyUser.UserID = Rs("UserID")
                    MyUser.FirstLogin = Rs("FirstLogin")
                    MyUser.LastLogin = Rs("LastLogin")
                    MyUser.LoginCount = Rs("LoginCount")
                End If
                Rs.Close()
                Rs = Nothing

                Return MyUser
            End Function

            Public Shared Function GetUserID() As String
                Dim UserID As String = ""
                Dim InCookie As HttpCookie = HttpContext.Current.Request.Cookies("UserID")

                If Len(HttpContext.Current.Session("UserID")) > 0 Then
                    UserID = HttpContext.Current.Session("UserID").ToString()
                ElseIf Not InCookie Is Nothing Then
                    UserID = InCookie.Value.ToString()
                Else
                    UserID = AddUser()
                End If

                If Len(UserID) > 0 Then
                    Dim OutCookie As HttpCookie = HttpContext.Current.Response.Cookies("UserID")
                    OutCookie.Value = UserID
                    OutCookie.Expires = DateAdd(DateInterval.Year, 1, Now())
                    OutCookie.Domain = HttpContext.Current.Request.ServerVariables("SERVER_NAME")
                End If

                Return UserID
            End Function

            Public Shared Function AddUser() As String
                Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
                Dim SQL As String = "sp_Users_AddUser"
                Dim UserID As String = ""

                Dim Cmd As New SqlCommand(SQL, Conn)
                Cmd.CommandType = CommandType.StoredProcedure

                Try
                    Conn.Open()
                    Dim DataReader As SqlDataReader = Cmd.ExecuteReader()
                    If DataReader.Read() Then
                        UserID = DataReader(0).ToString()
                    End If
                    DataReader.Close()

                Catch ex As Exception
                    Emagine.LogError(ex)
                Finally
                    Conn.Close()
                End Try

                Return UserID
            End Function

            Public Shared Function UpdateUser(ByVal strUserID As String) As String
                Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
                Dim SQL As String = "sp_Users_UpdateUser"
                Dim Result As Boolean = False

                Dim Cmd As New SqlCommand(SQL, Conn)
                Cmd.CommandType = CommandType.StoredProcedure

                Dim Param As New SqlParameter
                Cmd.Parameters.AddWithValue("@UserID", strUserID)

                Try
                    Conn.Open()
                    Result = Cmd.ExecuteNonQuery()

                Catch ex As Exception
                    Emagine.LogError(ex)
                Finally
                    Conn.Close()
                End Try

                Return Result
            End Function

            Public Shared Sub DeleteTempResources(ByVal strUserID As String)
                Emagine.ExecuteSQL("DELETE FROM UserResources WHERE UserID = '" & strUserID & "'")
            End Sub

            Public Shared Sub SetResource(ByVal strUserID As String, ByVal strResourceID As String, ByVal strItemName As String, ByVal strItemValue As String)
                Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
                Dim SQL As String = "sp_Users_GetResource"

                Dim Cmd As New SqlCommand(SQL, Conn)
                Cmd.CommandType = CommandType.StoredProcedure

                Dim Param As New SqlParameter
                Cmd.Parameters.AddWithValue("@UserID", strUserID)
                Cmd.Parameters.AddWithValue("@BatchID", "")
                Cmd.Parameters.AddWithValue("@ResourceID", strResourceID)
                Cmd.Parameters.AddWithValue("@ItemName", strItemName)

                Try
                    Conn.Open()
                    Dim Rs As SqlDataReader = Cmd.ExecuteReader()
                    If Rs.HasRows Then
                        UpdateResource(strUserID, "", strResourceID, strItemName, strItemValue)
                    Else
                        AddResource(strUserID, "", strResourceID, strItemName, strItemValue)
                    End If
                    Rs.Close()

                Catch ex As Exception
                    Emagine.LogError(ex)
                Finally
                    Conn.Close()
                End Try
            End Sub

            Public Shared Sub SetResource(ByVal strUserID As String, ByVal strBatchID As String, ByVal strResourceID As String, ByVal strItemName As String, ByVal strItemValue As String)
                Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
                Dim SQL As String = "sp_Users_GetResource"

                Dim Cmd As New SqlCommand(SQL, Conn)
                Cmd.CommandType = CommandType.StoredProcedure

                Dim Param As New SqlParameter
                Cmd.Parameters.AddWithValue("@UserID", strUserID)
                Cmd.Parameters.AddWithValue("@BatchID", strBatchID)
                Cmd.Parameters.AddWithValue("@ResourceID", strResourceID)
                Cmd.Parameters.AddWithValue("@ItemName", strItemName)

                Try
                    Conn.Open()
                    Dim Rs As SqlDataReader = Cmd.ExecuteReader()
                    If Rs.HasRows Then
                        UpdateResource(strUserID, strBatchID, strResourceID, strItemName, strItemValue)
                    Else
                        AddResource(strUserID, strBatchID, strResourceID, strItemName, strItemValue)
                    End If
                    Rs.Close()

                Catch ex As Exception
                    Emagine.LogError(ex)
                Finally
                    Conn.Close()
                End Try
            End Sub

            Private Shared Sub UpdateResource(ByVal strUserID As String, ByVal strBatchID As String, ByVal strResourceID As String, ByVal strItemName As String, ByVal strItemValue As String)
                Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
                Dim SQL As String = "sp_Users_UpdateResource"

                Dim Cmd As New SqlCommand(SQL, Conn)
                Cmd.CommandType = CommandType.StoredProcedure

                Dim Param As New SqlParameter
                Cmd.Parameters.AddWithValue("@UserID", strUserID)
                Cmd.Parameters.AddWithValue("@BatchID", strBatchID)
                Cmd.Parameters.AddWithValue("@ResourceID", strResourceID)
                Cmd.Parameters.AddWithValue("@ItemName", strItemName)
                Cmd.Parameters.AddWithValue("@ItemValue", strItemValue)

                Try
                    Conn.Open()
                    Cmd.ExecuteNonQuery()

                Catch ex As Exception
                    Emagine.LogError(ex)
                Finally
                    Conn.Close()
                End Try

            End Sub

            Private Shared Sub AddResource(ByVal strUserID As String, ByVal strBatchID As String, ByVal strResourceID As String, ByVal strItemName As String, ByVal strItemValue As String)
                Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
                Dim SQL As String = "sp_Users_AddResource"

                Dim Cmd As New SqlCommand(SQL, Conn)
                Cmd.CommandType = CommandType.StoredProcedure

                Dim Param As New SqlParameter
                Cmd.Parameters.AddWithValue("@UserID", strUserID)
                Cmd.Parameters.AddWithValue("@BatchID", strBatchID)
                Cmd.Parameters.AddWithValue("@ResourceID", strResourceID)
                Cmd.Parameters.AddWithValue("@ItemName", strItemName)
                Cmd.Parameters.AddWithValue("@ItemValue", strItemValue)

                Try
                    Conn.Open()
                    Cmd.ExecuteNonQuery()

                Catch ex As Exception
                    Emagine.LogError(ex)
                Finally
                    Conn.Close()
                End Try
            End Sub

            Public Shared Sub DeleteResources(ByVal strUserID As String)
                Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
                Dim SQL As String = "sp_Users_DeleteResources"

                Dim Cmd As New SqlCommand(SQL, Conn)
                Cmd.CommandType = CommandType.StoredProcedure

                Dim Param As New SqlParameter
                Cmd.Parameters.AddWithValue("@UserID", strUserID)

                Try
                    Conn.Open()
                    Cmd.ExecuteNonQuery()

                Catch ex As Exception
                    Emagine.LogError(ex)
                Finally
                    Conn.Close()
                End Try
            End Sub

        End Class

    End Class
End Class







