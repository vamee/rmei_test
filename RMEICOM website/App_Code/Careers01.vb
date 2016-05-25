Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class Careers01
    Inherits Resources.Resource

    Public ModuleKey As String = "Careers01"
    Public CareerID As Integer = -1
    Public CategoryID As Integer = -1
    Public CategoryName As String = ""
    Public CareerSummary As String = ""
    Public CareerText As String = ""
    Public ExternalUrl As String = ""
    Public DisplayDate As String

    Public Shared Function GetCareer(ByVal intItemId As Integer) As Careers01
        Dim Career As New Careers01
        Dim Rs As SqlDataReader = Emagine.GetDataReader("SELECT * FROM qryCareers WHERE CareerId = " & intItemId)
        If Rs.Read Then
            Career.CareerID = Rs("CareerID")
            Career.CategoryID = Rs("CategoryID")
            Career.ModuleTypeID = Rs("ModuleTypeID")
            Career.CategoryName = Rs("CategoryName").ToString
            Career.DisplayDate = Rs("DisplayDate").ToString
            Career.ResourceID = Rs("ResourceID")
            Career.ResourceName = Rs("ResourceName").ToString
            Career.ResourceCategory = Rs("ResourceCategory").ToString
            Career.ResourcePageKey = Rs("ResourcePageKey").ToString
            Career.CareerSummary = Rs("CareerSummary").ToString
            Career.CareerText = Rs("CareerText").ToString
            Career.ExternalUrl = Rs("ExternalUrl").ToString
            Career.ResourceKeywords = Rs("ResourceKeywords").ToString
            Career.SortOrder = Rs("SortOrder")
            Career.IsEnabled = Rs("IsEnabled")
            Career.DisplayStartDate = Rs("DisplayStartDate")
            Career.DisplayEndDate = Rs("DisplayEndDate")
        Else
            Career = Nothing
        End If

        Rs.Close()

        Return Career
    End Function


    Public Shared Function Add(ByVal objCareer As Careers01, Optional ByRef strErrorMessage As String = "") As Careers01
        Dim Result As Boolean = False

        objCareer.ResourceID = Emagine.GetUniqueID()
        objCareer.SortOrder = Careers01.GetMaxSortOrder(objCareer.CategoryID) + 1

        Dim SqlBuilder As New StringBuilder
        SqlBuilder.Append("INSERT INTO Careers ")
        SqlBuilder.Append("(CategoryID,ResourceID,DisplayDate,CareerSummary,CareerText,ExternalUrl) ")
        SqlBuilder.Append("VALUES ")
        SqlBuilder.Append("(@CategoryID,@ResourceID,@DisplayDate,@CareerSummary,@CareerText,@ExternalUrl)")

        Dim Command As New SqlCommand
        Command.Parameters.AddWithValue("@CategoryID", objCareer.CategoryID)
        Command.Parameters.AddWithValue("@ResourceID", objCareer.ResourceID)
        Command.Parameters.AddWithValue("@DisplayDate", objCareer.DisplayDate)
        Command.Parameters.AddWithValue("@CareerSummary", objCareer.CareerSummary)
        Command.Parameters.AddWithValue("@CareerText", objCareer.CareerText)
        Command.Parameters.AddWithValue("@ExternalUrl", objCareer.ExternalUrl)

        Result = Emagine.ExecuteSQL(SqlBuilder.ToString, Command, strErrorMessage)
        Command.Dispose()

        If Result Then
            objCareer.CareerID = Emagine.GetNumber(Emagine.GetDbValue("SELECT MAX(CareerID) AS MaxItemID FROM Careers"))

            Dim MyResource As New Resources.Resource
            MyResource.ResourceID = objCareer.ResourceID
            MyResource.LanguageID = objCareer.LanguageID
            MyResource.ResourceName = objCareer.ResourceName
            MyResource.ResourceType = objCareer.ModuleKey
            MyResource.ModuleTypeID = objCareer.ModuleTypeID
            MyResource.DisplayStartDate = objCareer.DisplayStartDate
            MyResource.DisplayEndDate = objCareer.DisplayEndDate
            MyResource.SortOrder = objCareer.SortOrder
            MyResource.IsEnabled = objCareer.IsEnabled
            MyResource.CreatedBy = objCareer.CreatedBy
            MyResource.UpdatedBy = objCareer.UpdatedBy

            Result = Resources.Resource.AddResource(MyResource, strErrorMessage)

            If Not Result Then Emagine.ExecuteSQL("DELETE FROM Careers WHERE ResourceID = '" & objCareer.ResourceID & "'")
        End If

        Return objCareer
    End Function

    Public Shared Function Update(ByVal objCareer As Careers01, Optional ByRef strErrorMessage As String = "") As Boolean
        Dim Result As Boolean = False
        Dim SqlBuilder As New StringBuilder
        SqlBuilder.Append("UPDATE Careers SET ")
        SqlBuilder.Append("CategoryID=@CategoryID,")
        SqlBuilder.Append("DisplayDate=@DisplayDate,")
        SqlBuilder.Append("CareerSummary=@CareerSummary,")
        SqlBuilder.Append("CareerText=@CareerText,")
        SqlBuilder.Append("ExternalUrl=@ExternalUrl ")
        SqlBuilder.Append("WHERE CareerID = @CareerID")

        Dim Command As New SqlCommand
        Command.Parameters.AddWithValue("@CareerID", objCareer.CareerID)
        Command.Parameters.AddWithValue("@CategoryID", objCareer.CategoryID)
        Command.Parameters.AddWithValue("@DisplayDate", objCareer.DisplayDate)
        Command.Parameters.AddWithValue("@CareerSummary", objCareer.CareerSummary)
        Command.Parameters.AddWithValue("@CareerText", objCareer.CareerText)
        Command.Parameters.AddWithValue("@ExternalUrl", objCareer.ExternalUrl)

        Result = Emagine.ExecuteSQL(SqlBuilder.ToString, Command, strErrorMessage)
        Command.Dispose()

        If Result Then
            Dim MyResource As Resources.Resource = Resources.Resource.GetResource(objCareer.ResourceID)

            If MyResource.ResourceID.Length > 0 Then
                MyResource.ResourceName = objCareer.ResourceName
                MyResource.DisplayStartDate = objCareer.DisplayStartDate
                MyResource.DisplayEndDate = objCareer.DisplayEndDate
                MyResource.SortOrder = objCareer.SortOrder
                MyResource.IsEnabled = objCareer.IsEnabled
                MyResource.UpdatedBy = objCareer.UpdatedBy
                MyResource.UpdatedDate = Date.Now()

                Result = Resources.Resource.UpdateResource(MyResource, strErrorMessage)
            Else
                MyResource.ResourceID = objCareer.ResourceID
                MyResource.LanguageID = objCareer.LanguageID
                MyResource.ResourceName = objCareer.ResourceName
                MyResource.ResourceType = objCareer.ModuleKey
                MyResource.ModuleTypeID = objCareer.ModuleTypeID
                MyResource.DisplayStartDate = objCareer.DisplayStartDate
                MyResource.DisplayEndDate = objCareer.DisplayEndDate
                MyResource.IsEnabled = objCareer.IsEnabled
                MyResource.CreatedBy = objCareer.CreatedBy
                MyResource.UpdatedBy = objCareer.UpdatedBy

                Result = Resources.Resource.AddResource(MyResource, strErrorMessage)
            End If
        End If

        Return Result
    End Function

    Public Shared Function Delete(ByVal objCareer As Careers01, Optional ByRef strErrorMessage As String = "") As Boolean
        Dim Result As Boolean = False

        Dim Sql As String = "DELETE FROM Careers WHERE CareerID = @CareerID"
        Dim Command As New SqlCommand
        Command.Parameters.AddWithValue("@CareerID", objCareer.CareerID)

        Result = Emagine.ExecuteSQL(Sql, Command, strErrorMessage)

        If Result Then
            Dim MyResource As Resources.Resource = Resources.Resource.GetResource(objCareer.ResourceID)

            Resources.Resource.DeleteResource(MyResource)
        End If

        Return Result
    End Function

    Public Shared Function GetMaxSortOrder(ByVal intCategoryID As Integer) As Integer
        Return Emagine.GetNumber(Emagine.GetDbValue("SELECT COUNT(CareerID) As MaxSortOrder FROM qryCareers WHERE CategoryID = " & intCategoryID))
    End Function

End Class

