Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class Custom01
    Inherits Resources.Resource

    Public ApplicationID As Integer = -1
    Public ApplicationName As String = ""
    Public Description As String = ""
    Public FileName As String = ""

    Public Shared Function GetCustomApplications(ByVal intStatusID As Integer, ByVal intLanguageID As Integer) As SqlDataReader
        Return Emagine.GetDataReader("SELECT * FROM qryCustomApplications WHERE StatusID=" & intStatusID & " AND LanguageID=" & intLanguageID & " ORDER BY ApplicationName")
    End Function

    Public Shared Function GetCustomApplication(ByVal intApplicationId As Integer) As Custom01
        Dim Application As New Custom01
        Dim dtr As SqlDataReader = Emagine.GetDataReader("SELECT * FROM qryCustomApplications WHERE ApplicationID=" & intApplicationId)
        If dtr.Read Then
            Application.ApplicationID = dtr("ApplicationID")
            Application.LanguageID = dtr("LanguageID")
            Application.StatusID = dtr("StatusID")
            Application.ApplicationName = dtr("ResourceName").ToString
            Application.ResourceID = dtr("ResourceID").ToString
            Application.ResourceName = dtr("ResourceName").ToString
            Application.Description = dtr("Description").ToString
            Application.FileName = dtr("FileName").ToString
            Application.IsEnabled = dtr("IsEnabled")
            Application.DisplayStartDate = dtr("DisplayStartDate")
            Application.DisplayEndDate = dtr("DisplayEndDate")
            Application.CreatedDate = dtr("CreatedDate")
            Application.CreatedBy = dtr("CreatedBy").ToString
            Application.UpdatedDate = dtr("UpdatedDate")
            Application.UpdatedBy = dtr("UpdatedBy").ToString
        End If

        dtr.Close()

        Return Application
    End Function

    Public Shared Function Update(ByVal objCustomApp As Custom01) As Boolean
        Dim Result As Boolean = False
        Dim Sql As String = "UPDATE CustomApplications SET ResourceID=@ResourceID, Description=@Description, FileName=@FileName WHERE ApplicationID=@ApplicationID"

        Dim Command As New SqlCommand
        Command.Parameters.AddWithValue("@ApplicationID", objCustomApp.ApplicationID)
        Command.Parameters.AddWithValue("@ResourceID", objCustomApp.ResourceID)
        Command.Parameters.AddWithValue("@Description", objCustomApp.Description)
        Command.Parameters.AddWithValue("@FileName", objCustomApp.FileName)

        Result = Emagine.ExecuteSQL(SQL, Command)

        If Result Then
            Dim MyResource As Resources.Resource = Resources.Resource.GetResource(objCustomApp.ResourceID)

            If MyResource.ResourceID.Length > 0 Then
                MyResource.LanguageID = objCustomApp.LanguageID
                MyResource.StatusID = objCustomApp.StatusID
                MyResource.ResourceName = objCustomApp.ResourceName
                MyResource.IsEnabled = objCustomApp.IsEnabled
                MyResource.DisplayStartDate = objCustomApp.DisplayStartDate
                MyResource.DisplayEndDate = objCustomApp.DisplayEndDate
                MyResource.UpdatedBy = objCustomApp.UpdatedBy
                MyResource.UpdatedDate = Date.Now()

                Result = Resources.Resource.UpdateResource(MyResource)
            Else
                MyResource.ResourceID = objCustomApp.ResourceID
                MyResource.ResourceType = "Custom01"
                MyResource.LanguageID = objCustomApp.LanguageID
                MyResource.StatusID = objCustomApp.StatusID
                MyResource.ResourceName = objCustomApp.ResourceName
                MyResource.DisplayStartDate = objCustomApp.DisplayStartDate
                MyResource.DisplayEndDate = objCustomApp.DisplayEndDate
                MyResource.IsEnabled = objCustomApp.IsEnabled
                MyResource.CreatedBy = objCustomApp.CreatedBy
                MyResource.UpdatedBy = objCustomApp.UpdatedBy

                Result = Resources.Resource.AddResource(MyResource)
            End If

        End If

        Return Result
    End Function

    Public Shared Function Insert(ByVal objCustomApp As Custom01) As Custom01
        Dim Result As Boolean = False
        objCustomApp.ResourceID = Emagine.GetUniqueID()
        Dim Sql As String = "INSERT INTO CustomApplications (ResourceID,Description,FileName) VALUES (@ResourceID,@Description,@FileName)"

        Dim Command As New SqlCommand
        Command.Parameters.AddWithValue("@ResourceID", objCustomApp.ResourceID)
        Command.Parameters.AddWithValue("@Description", objCustomApp.Description)
        Command.Parameters.AddWithValue("@FileName", objCustomApp.FileName)

        Result = Emagine.ExecuteSQL(Sql, Command)

        If Result Then
            objCustomApp.ApplicationID = Emagine.GetNumber(Emagine.GetDbValue("SELECT MAX(ApplicationID) AS MaxID FROM CustomApplications"))

            Dim MyResource As New Resources.Resource
            MyResource.ResourceID = objCustomApp.ResourceID
            MyResource.ResourceType = "Custom01"
            MyResource.LanguageID = objCustomApp.LanguageID
            MyResource.StatusID = objCustomApp.StatusID
            MyResource.ResourceName = objCustomApp.ResourceName
            MyResource.DisplayStartDate = objCustomApp.DisplayStartDate
            MyResource.DisplayEndDate = objCustomApp.DisplayEndDate
            MyResource.IsEnabled = objCustomApp.IsEnabled
            MyResource.CreatedBy = objCustomApp.CreatedBy
            MyResource.UpdatedBy = objCustomApp.UpdatedBy

            Result = Resources.Resource.AddResource(MyResource)

            If Not Result Then Emagine.ExecuteSQL("DELETE FROM CustomApplications WHERE ResourceID = '" & objCustomApp.ResourceID & "'")
        End If

        Return objCustomApp
    End Function


    Public Shared Function Delete(ByVal objCustomApp As Custom01) As Boolean
        Dim Result As Boolean = False

        Result = Emagine.ExecuteSQL("DELETE FROM CustomApplications WHERE ApplicationID = " & objCustomApp.ApplicationID)

        If Result Then
            Emagine.ExecuteSQL("DELETE FROM Resources WHERE ResourceID = '" & objCustomApp.ResourceID & "'")
            Emagine.ExecuteSQL("DELETE FROM PageModuleProperties WHERE PageModuleID IN (SELECT PageModuleID FROM PageModules WHERE ModuleKey = 'Custom01' AND ForeignValue = " & objCustomApp.ApplicationID)
            Emagine.ExecuteSQL("DELETE FROM PageModules WHERE ModuleKey = 'Custom01' AND ForeignValue = " & objCustomApp.ApplicationID)
        End If

        Return Result
    End Function

End Class
