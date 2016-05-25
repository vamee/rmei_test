Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Namespace Membership
    Public Class Members

    End Class

    Public Class Member

        Public MemberID As String = ""
        Public CategoryID As Integer = 0
        Public MemberType As String = ""
        Public FirstName As String = ""
        Public LastName As String = ""
        Public Username As String = ""
        Public Password As String = ""
        Public Email As String = ""
        Public Description As String = ""
        Public LoggedIn As Boolean = False
        Public FirstLoginDate As Date = "1/1/1900"
        Public LastLoginDate As Date = "1/1/1900"
        Public LoginCount As Integer = 0
        Public CreatedDate As Date = "1/1/1900"
        Public CreatedBy As String = ""
        Public UpdatedDate As Date = "1/1/1900"
        Public UpdatedBy As String = ""

        Public Shared Function Add(ByVal objUser As Membership.Member) As Boolean
            Dim Result As Boolean = False
            Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
            Dim SqlBuilder As New StringBuilder
            SqlBuilder.Append("INSERT INTO Members ")
            SqlBuilder.Append("(CategoryID,FirstName,LastName,Username,Password,Email,Description,CreatedBy,UpdatedBy)")
            SqlBuilder.Append(" VALUES ")
            SqlBuilder.Append("(@CategoryID,@FirstName,@LastName,@Username,@Password,@Email,@Description,@CreatedBy,@UpdatedBy)")

            Dim Command As New SqlCommand(SqlBuilder.ToString, Conn)
            Command.Parameters.AddWithValue("@CategoryID", objUser.CategoryID)
            Command.Parameters.AddWithValue("@FirstName", objUser.FirstName)
            Command.Parameters.AddWithValue("@LastName", objUser.LastName)
            Command.Parameters.AddWithValue("@Username", objUser.Username)
            Command.Parameters.AddWithValue("@Password", objUser.Password)
            Command.Parameters.AddWithValue("@Email", objUser.Email)
            Command.Parameters.AddWithValue("@Description", objUser.Description)
            Command.Parameters.AddWithValue("@CreatedBy", objUser.CreatedBy)
            Command.Parameters.AddWithValue("@UpdatedBy", objUser.UpdatedBy)

            Try
                Conn.Open()
                If Command.ExecuteNonQuery() > 0 Then Result = True

            Catch ex As Exception
                ex.HelpLink = "Error inserting new member. [App_Code/Membership.vb]"
                Emagine.LogError(ex)

            Finally
                Conn.Close()
            End Try

            Return Result
        End Function

        Public Shared Function Update(ByVal objUser As Membership.Member) As Boolean
            Dim Result As Boolean = False
            Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
            Dim SqlBuilder As New StringBuilder
            SqlBuilder.Append("UPDATE Members SET ")
            SqlBuilder.Append("CategoryID=@CategoryID,")
            SqlBuilder.Append("FirstName=@FirstName,")
            SqlBuilder.Append("LastName=@LastName,")
            SqlBuilder.Append("Username=@Username,")
            SqlBuilder.Append("Password=@Password,")
            SqlBuilder.Append("Email=@Email,")
            SqlBuilder.Append("Description=@Description,")
            SqlBuilder.Append("IsLoggedIn=@IsLoggedIn,")
            SqlBuilder.Append("FirstLoginDate=@FirstLoginDate,")
            SqlBuilder.Append("LastLoginDate=@LastLoginDate,")
            SqlBuilder.Append("LoginCount=@LoginCount,")
            SqlBuilder.Append("UpdatedDate=@UpdatedDate,")
            SqlBuilder.Append("UpdatedBy=@UpdatedBy ")
            SqlBuilder.Append("WHERE MemberID = @MemberID")

            Dim Command As New SqlCommand(SqlBuilder.ToString, Conn)
            Command.Parameters.AddWithValue("@MemberID", objUser.MemberID)
            Command.Parameters.AddWithValue("@CategoryID", objUser.CategoryID)
            Command.Parameters.AddWithValue("@FirstName", objUser.FirstName)
            Command.Parameters.AddWithValue("@LastName", objUser.LastName)
            Command.Parameters.AddWithValue("@Username", objUser.Username)
            Command.Parameters.AddWithValue("@Password", objUser.Password)
            Command.Parameters.AddWithValue("@Email", objUser.Email)
            Command.Parameters.AddWithValue("@Description", objUser.Description)
            Command.Parameters.AddWithValue("@IsLoggedIn", objUser.LoggedIn)
            Command.Parameters.AddWithValue("@FirstLoginDate", objUser.FirstLoginDate)
            Command.Parameters.AddWithValue("@LastLoginDate", objUser.LastLoginDate)
            Command.Parameters.AddWithValue("@LoginCount", objUser.LoginCount)
            Command.Parameters.AddWithValue("@UpdatedDate", objUser.UpdatedDate)
            Command.Parameters.AddWithValue("@UpdatedBy", objUser.UpdatedBy)

            Try
                Conn.Open()
                If Command.ExecuteNonQuery() > 0 Then Result = True

            Catch ex As Exception
                ex.HelpLink = "Error updating member. [App_Code/Membership.vb]"
                Emagine.LogError(ex)

            Finally
                Conn.Close()
            End Try

            Return Result
        End Function

        Public Shared Function GetMembershipUser(ByVal strMemberID As String) As Membership.Member
            Dim User As New Membership.Member
            Dim Sql As String = "SELECT * FROM qryMembers WHERE MemberID = '" & strMemberID.Replace("'", "''") & "'"
            Dim MemberData As DataTable = Emagine.GetDataTable(Sql)

            If MemberData.Rows.Count > 0 Then
                User.MemberID = MemberData.Rows(0).Item("MemberID").ToString
                User.CategoryID = MemberData.Rows(0).Item("CategoryID").ToString
                User.MemberType = MemberData.Rows(0).Item("MemberType").ToString
                User.FirstName = MemberData.Rows(0).Item("FirstName").ToString
                User.LastName = MemberData.Rows(0).Item("LastName").ToString
                User.Username = MemberData.Rows(0).Item("Username").ToString
                User.Password = MemberData.Rows(0).Item("Password").ToString
                User.Email = MemberData.Rows(0).Item("Email").ToString
                User.Description = MemberData.Rows(0).Item("Description").ToString
                User.LoggedIn = MemberData.Rows(0).Item("IsLoggedIn")
                If Not String.IsNullOrEmpty(MemberData.Rows(0).Item("FirstLoginDate").ToString) Then User.FirstLoginDate = MemberData.Rows(0).Item("FirstLoginDate").ToString
                If Not String.IsNullOrEmpty(MemberData.Rows(0).Item("LastLoginDate").ToString) Then User.LastLoginDate = MemberData.Rows(0).Item("LastLoginDate").ToString
                User.LoginCount = Emagine.GetNumber(MemberData.Rows(0).Item("LoginCount"))
                If Not String.IsNullOrEmpty(MemberData.Rows(0).Item("CreatedDate").ToString) Then User.CreatedDate = MemberData.Rows(0).Item("CreatedDate").ToString
                User.CreatedBy = MemberData.Rows(0).Item("CreatedBy").ToString
                If Not String.IsNullOrEmpty(MemberData.Rows(0).Item("UpdatedDate").ToString) Then User.UpdatedDate = MemberData.Rows(0).Item("UpdatedDate").ToString
                User.UpdatedBy = MemberData.Rows(0).Item("UpdatedBy").ToString
            End If

            Return User
        End Function

        Public Shared Function GetMembershipUser(ByVal strUsername As String, ByVal strPassword As String) As Membership.Member
            Dim User As New Membership.Member
            Dim Sql As String = "SELECT * FROM qryMembers WHERE Username='" & strUsername.Replace("'", "''") & "' AND Password='" & strPassword.Replace("'", "''") & "'"
            Dim MemberData As DataTable = Emagine.GetDataTable(Sql)

            If MemberData.Rows.Count > 0 Then
                User.MemberID = MemberData.Rows(0).Item("MemberID").ToString
                User.CategoryID = MemberData.Rows(0).Item("CategoryID").ToString
                User.MemberType = MemberData.Rows(0).Item("MemberType").ToString
                User.FirstName = MemberData.Rows(0).Item("FirstName").ToString
                User.LastName = MemberData.Rows(0).Item("LastName").ToString
                User.Username = MemberData.Rows(0).Item("Username").ToString
                User.Password = MemberData.Rows(0).Item("Password").ToString
                User.Email = MemberData.Rows(0).Item("Email").ToString
                User.Description = MemberData.Rows(0).Item("Description").ToString
                User.LoggedIn = MemberData.Rows(0).Item("IsLoggedIn")
                If Not String.IsNullOrEmpty(MemberData.Rows(0).Item("FirstLoginDate").ToString) Then User.FirstLoginDate = MemberData.Rows(0).Item("FirstLoginDate").ToString
                If Not String.IsNullOrEmpty(MemberData.Rows(0).Item("LastLoginDate").ToString) Then User.LastLoginDate = MemberData.Rows(0).Item("LastLoginDate").ToString
                User.LoginCount = MemberData.Rows(0).Item("LoginCount")
                If Not String.IsNullOrEmpty(MemberData.Rows(0).Item("CreatedDate").ToString) Then User.CreatedDate = MemberData.Rows(0).Item("CreatedDate").ToString
                User.CreatedBy = MemberData.Rows(0).Item("CreatedBy").ToString
                If Not String.IsNullOrEmpty(MemberData.Rows(0).Item("UpdatedDate").ToString) Then User.UpdatedDate = MemberData.Rows(0).Item("UpdatedDate").ToString
                User.UpdatedBy = MemberData.Rows(0).Item("UpdatedBy").ToString
            End If

            Return User
        End Function

        Public Shared Function GetMembershipUserID() As String
            Dim MemberID As String = ""
            Dim InCookie As HttpCookie = HttpContext.Current.Request.Cookies("MemberID")

            If HttpContext.Current.Session("MemberID") IsNot Nothing Then
                MemberID = HttpContext.Current.Session("MemberID").ToString

            ElseIf InCookie IsNot Nothing Then
                MemberID = InCookie.Value.ToString
            End If

            Dim OutCookie As HttpCookie = HttpContext.Current.Response.Cookies("MemberID")
            OutCookie.Value = MemberID
            OutCookie.Expires = DateAdd(DateInterval.Year, 1, Now())
            OutCookie.Domain = HttpContext.Current.Request.ServerVariables("SERVER_NAME")

            Return MemberID
        End Function

        Private Shared Function IsLoggedIn(ByVal strMemberID As String) As Boolean
            Dim Result As Boolean = False
            Dim SQL = "SELECT IsLoggedIn FROM Members WHERE MemberID = '" & strMemberID & "'"
            Result = CBool(Emagine.GetNumber(Emagine.GetDbValue(SQL).ToString))
            Return Result
        End Function

    End Class
End Namespace


