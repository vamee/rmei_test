Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class EzeditUser

    Public UserID As Integer = 0
    Public EzEditLevelID As Integer = -1
    Public LanguageID As Integer = -1
    Public FirstName As String = ""
    Public LastName As String = ""
    Public Email As String = ""
    Public Username As String = ""
    Public Password As String = ""
    Public IsEnabled As Boolean = True
    Public DateCreated As DateTime = "1/1/1900"
    Public FirstLogin As String = ""
    Public LastLogin As String = ""
    Public LoginCount As Integer = 0

    Public Shared Function GetAllUsers() As SqlDataReader
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim selectString As String
        selectString = "SELECT * FROM qryEzEditUsers WHERE UserID > 1"
        Dim cmd As New SqlCommand(selectString, con)
        con.Open()
        Dim dtr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
        Return dtr
        con.Close()
    End Function

    Public Shared Function GetAllUsers(ByVal intLanguageID As Integer) As SqlDataReader
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim selectString As String
        selectString = "SELECT * FROM qryEzEditUsers WHERE UserID > 1 AND LanguageID = " & intLanguageID
        Dim cmd As New SqlCommand(selectString, con)
        con.Open()
        Dim dtr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
        Return dtr
        con.Close()
    End Function

    Public Shared Function GetUser(ByVal intUserID As Integer) As EzeditUser
        Dim User As New EzeditUser
        Dim dtr As SqlDataReader = Emagine.GetDataReader("SELECT * FROM EzEditUsers WHERE UserId = " & intUserID)
        If dtr.Read Then
            User.UserID = dtr("UserId")
            User.EzEditLevelID = dtr("EzEditLevelID")
            User.LanguageID = dtr("LanguageID")
            User.FirstName = dtr("FirstName").ToString
            User.LastName = dtr("LastName").ToString
            User.Email = dtr("Email").ToString
            User.Username = dtr("Username").ToString
            User.Password = dtr("Password").ToString
            User.IsEnabled = dtr("IsEnabled")
            User.FirstLogin = dtr("FirstLogin").ToString
            User.LastLogin = dtr("LastLogin").ToString
            User.LoginCount = dtr("LoginCount")
        End If
        dtr.Close()

        If User.UserID = 0 And GlobalVariables.GetValue("EnableEmagineWebServices").ToUpper = "TRUE" Then
            Try
                Dim MyWebService As New EmagineUsers.EzEditUsers
                Dim MyUser As EmagineUsers.EzeditUser = MyWebService.GetUserByID(intUserID)

                If MyUser.UserID <> 0 Then
                    User.UserID = MyUser.UserID
                    User.EzEditLevelID = 1
                    User.LanguageID = MyUser.LanguageID
                    User.FirstName = MyUser.FirstName
                    User.LastName = MyUser.LastName
                    User.Email = MyUser.Email
                    User.Username = MyUser.Username
                    User.Password = MyUser.Password
                    User.IsEnabled = MyUser.IsEnabled
                    User.FirstLogin = MyUser.FirstLogin
                    User.LastLogin = MyUser.LastLogin
                    User.LoginCount = MyUser.LoginCount
                End If
            Catch ex As Exception
                Emagine.LogError(ex)
            End Try

        End If

        Return User
    End Function

    Public Shared Function GetUser(ByVal strUsername As String, ByVal strPassword As String) As EzeditUser
        Dim User As New EzeditUser
        Dim UserData As DataTable = Emagine.GetDataTable("SELECT * FROM EzEditUsers WHERE Username='" & strUsername.Replace("'", "''") & "' AND Password = '" & Emagine.GetMd5Hash(strPassword) & "' AND IsEnabled = 'True'")
        If UserData.Rows.Count > 0 Then
            User.UserID = UserData.Rows(0).Item("UserId")
            User.EzEditLevelID = UserData.Rows(0).Item("EzEditLevelID")
            User.LanguageID = UserData.Rows(0).Item("LanguageID")
            User.FirstName = UserData.Rows(0).Item("FirstName").ToString
            User.LastName = UserData.Rows(0).Item("LastName").ToString
            User.Email = UserData.Rows(0).Item("Email").ToString
            User.Username = UserData.Rows(0).Item("Username").ToString
            User.Password = UserData.Rows(0).Item("Password").ToString
            User.IsEnabled = UserData.Rows(0).Item("IsEnabled")
            User.FirstLogin = UserData.Rows(0).Item("FirstLogin").ToString
            User.LastLogin = UserData.Rows(0).Item("LastLogin").ToString
            User.LoginCount = UserData.Rows(0).Item("LoginCount")
        End If
        UserData.Dispose()

        If User.UserID = 0 And GlobalVariables.GetValue("EnableEmagineWebServices").ToUpper = "TRUE" Then
            Try
                Dim MyWebService As New EmagineUsers.EzEditUsers
                Dim MyUser As EmagineUsers.EzeditUser = MyWebService.GetUser(strUsername, strPassword)

                If MyUser.UserID <> 0 Then
                    User.UserID = MyUser.UserID
                    User.EzEditLevelID = 1
                    User.LanguageID = MyUser.LanguageID
                    User.FirstName = MyUser.FirstName
                    User.LastName = MyUser.LastName
                    User.Email = MyUser.Email
                    User.Username = MyUser.Username
                    User.Password = MyUser.Password
                    User.IsEnabled = MyUser.IsEnabled
                    User.FirstLogin = MyUser.FirstLogin
                    User.LastLogin = MyUser.LastLogin
                    User.LoginCount = MyUser.LoginCount
                End If
            Catch ex As Exception
                Emagine.LogError(ex)
            End Try

        End If

        Return User
    End Function

    Public Shared Function GetUserLevels() As SqlDataReader
        Return Emagine.GetDataReader("SELECT * FROM EzEditUserLevels ORDER BY EzEditLevelID")
        ' WHERE EzEditLevelID >= " & Session("EzEditLevelID") & "
    End Function

    Public Function UpdateEzEditUser(ByVal User As EzeditUser) As Boolean

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim blnSuccess As Boolean
        Dim strSelect As String

        If User.UserId = 0 Then
            strSelect = "INSERT INTO EzEditUsers "
            strSelect += "(EzEditLevelID, LanguageID, FirstName, LastName, Email, Username, Password, IsEnabled)"
            strSelect += "VALUES (@EzEditLevelID, @LanguageID, @FirstName, @LastName, @Email, @Username, @Password, @IsEnabled)"
            HttpContext.Current.Session("ALERT") = "User Added Successfully"
        Else ' UPDATE
            strSelect = "UPDATE EzEditUsers "
            strSelect += "SET EzEditLevelID = @EzEditLevelID, LanguageID = @LanguageID, FirstName = @FirstName, LastName = @LastName, Email = @Email, "
            If HasPasswordChanged(User.UserId, User.Password) Then
                strSelect += "Password = @Password, "
            End If
            strSelect += "Username = @Username, IsEnabled = @IsEnabled "
            strSelect += "WHERE UserId = @UserId"
            HttpContext.Current.Session("ALERT") = "User Updated Successfully"
        End If

        Dim cmd As New SqlCommand(strSelect, con)

        cmd.Parameters.AddWithValue("@UserId", User.UserId)
        cmd.Parameters.AddWithValue("@EzEditLevelID", User.EzEditLevelID)
        cmd.Parameters.AddWithValue("@LanguageID", User.LanguageID)
        cmd.Parameters.AddWithValue("@FirstName", User.FirstName)
        cmd.Parameters.AddWithValue("@LastName", User.LastName)
        cmd.Parameters.AddWithValue("@Email", User.Email)
        cmd.Parameters.AddWithValue("@Username", User.Username)
        cmd.Parameters.AddWithValue("@Password", User.Password)
        cmd.Parameters.AddWithValue("@IsEnabled", User.IsEnabled)

        Try
            con.Open()
            cmd.ExecuteNonQuery()
            blnSuccess = True
        Catch ex As Exception
            Emagine.LogError(ex)
            blnSuccess = False
        Finally
            con.Close()
        End Try

        Return blnSuccess

    End Function

    Function HasPasswordChanged(ByVal intUserID As Integer, ByVal strPassword As String) As Boolean
        If Emagine.GetDbValue("SELECT Password FROM EzEditUsers WHERE UserID = " & intUserID) = strPassword Then
            ' HttpContext.Current.Response.Write("true")
            Return False
        Else
            Return True
            'HttpContext.Current.Response.Write("false")
        End If
        'HttpContext.Current.Response.End()
    End Function

    Public Shared Function DeleteUser(ByVal intUserId As Integer) As Boolean
        Return Emagine.ExecuteSQL("DELETE FROM EzEditUsers WHERE UserId = " & intUserId)
    End Function

    Public Shared Function DeleteUser(ByVal objUser As EzeditUser, Optional ByRef strError As String = "") As Boolean
        Dim Result As Boolean = False
        Dim UserID As Integer = objUser.UserID

        Result = Emagine.ExecuteSQL("DELETE FROM EzEditUsers WHERE UserId = " & UserID, strError)

        If Result Then
            Emagine.ExecuteSQL("DELETE FROM PagePermissions WHERE EzUserID = " & UserID)
            Emagine.ExecuteSQL("DELETE FROM ModulePermissions WHERE EzUserID = " & UserID)
        End If

        Return Result
    End Function

    Public Shared Function IsUniqueUsername(ByVal strUsername As String, ByVal intUserId As Integer) As Boolean
        If Emagine.GetDbValue("SELECT Count(*) FROM EzEditUsers WHERE UserId <> " & intUserId & " AND Username = '" & strUsername & "'") = 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function ValidateUser(ByVal strUsername As String, ByVal strPassword As String) As Boolean
        Dim strComparePassword As String
        strComparePassword = Emagine.GetDbValue("SELECT Password FROM EzeditUsers WHERE Username = '" & strUsername & "'")
        If Emagine.GetMd5Hash(strPassword) = strComparePassword Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Sub UpdateUserLogin(ByVal User As EzeditUser)
        Dim strSQL As String
        strSQL = "UPDATE EzEditUsers SET "
        strSQL += "FirstLogin = '" & User.FirstLogin & "', "
        strSQL += "LastLogin = '" & Now() & "', "
        strSQL += "LoginCount = " & User.LoginCount + 1 & " "
        strSQL += "WHERE UserId = " & User.UserId
        HttpContext.Current.Response.Write(strSQL)
        Emagine.ExecuteSQL(strSQL)
    End Sub

    Public Shared Function HasPagePermissions(ByVal intUserID As Integer, ByVal intPageID As Integer) As Boolean
        Dim Result As Boolean = False
        Dim User As EzeditUser = EzeditUser.GetUser(intUserID)
        If User.EzEditLevelID = 1 Then
            Result = True
        ElseIf Emagine.GetNumber(Emagine.GetDbValue("SELECT Count(*) FROM PagePermissions WHERE EzUserID = " & intUserID & " AND PageId = " & intPageID)) > 0 Then
            Result = True
        End If

        Return Result
    End Function

    Public Shared Function HasModulePermissions(ByVal intUserID As Integer, ByVal strModuleKey As String) As Boolean
        Dim Result As Boolean = False
        Dim User As EzeditUser = EzeditUser.GetUser(intUserID)

        If User.EzEditLevelID = 1 Then
            Result = True
        ElseIf Emagine.GetNumber(Emagine.GetDbValue("SELECT Count(*) FROM ModulePermissions WHERE EzUserID = " & intUserID & " AND ModuleKey = '" & strModuleKey.Replace("'", "''") & "'")) > 0 Then
            Result = True
        End If

        Return Result
    End Function

End Class
