Imports System.Data
Imports System.Data.SqlClient

Partial Class Display_Modules
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim PageID As Integer = CInt(Session("PageID"))
        DisplayModules(PageID)
    End Sub

    Sub DisplayModules(ByVal intPageID As Integer)
        Dim ModuleData As DataTable = Emagine.GetDataTable("SELECT  PageModuleID, ForeignValue, CodeFileName, DisplayPageSortOrder AS SortOrder FROM qryDisplayPageModules WHERE PageID = " & intPageID & " AND CodeFileName IS NOT NULL ORDER BY SortOrder")
        Dim DeliveryPageData As DataTable = Emagine.GetDataTable("SELECT DISTINCT PageModuleID, CodeFileName, DeliveryPageSortOrder AS SortOrder FROM qryDeliveryPageModules WHERE DeliveryPageID = '" & intPageID & "' AND CodeFileName IS NOT NULL ORDER BY SortOrder")

        For Each Row As DataRow In DeliveryPageData.Rows
            ModuleData.ImportRow(Row)
        Next

        ModuleData.DefaultView.Sort = "SortOrder"

        For i As Integer = 0 To (ModuleData.DefaultView.Count - 1)

            'Response.Write(ModuleData.DefaultView(i).Item(0) & " : " & ModuleData.DefaultView(i).Item(2) & " : " & ModuleData.DefaultView(i).Item(3) & "<br>")

            Dim File As System.IO.FileInfo = New System.IO.FileInfo(Server.MapPath(ModuleData.DefaultView(i).Item("CodeFileName")))

            If File.Exists Then
                Dim ucPageModule As Control = LoadControl(ModuleData.DefaultView(i).Item("CodeFileName"))

                Dim MyProperty As System.Reflection.PropertyInfo
                MyProperty = ucPageModule.GetType().GetProperty("PageModuleID")
                If Not MyProperty Is Nothing Then MyProperty.SetValue(ucPageModule, ModuleData.DefaultView(i).Item("PageModuleID"), Nothing)
                plcModules.Controls.Add(ucPageModule)
            End If
            File = Nothing

        Next
        'Response.End()

    End Sub


    Sub DisplayModules2(ByVal intPageID As Integer)
        'Dim plcPageModule As PlaceHolder = CType(Page.Master.FindControl("plcModule"), PlaceHolder)
        Dim Rs As System.Data.SqlClient.SqlDataReader = Emagine.GetDataReader("SELECT  PageModuleID, ForeignValue, CodeFileName FROM qryDisplayPageModules WHERE PageID = " & intPageID & " AND CodeFileName IS NOT NULL ORDER BY SortOrder")

        Do While Rs.Read
            Dim File As System.IO.FileInfo = New System.IO.FileInfo(Server.MapPath(Rs("CodeFileName")))

            If File.Exists Then
                Dim ucPageModule As Control = LoadControl(Rs("CodeFileName"))

                Dim MyProperty As System.Reflection.PropertyInfo
                MyProperty = ucPageModule.GetType().GetProperty("PageModuleID")
                If Not MyProperty Is Nothing Then MyProperty.SetValue(ucPageModule, Rs("PageModuleID"), Nothing)
                plcModules.Controls.Add(ucPageModule)
            End If
            File = Nothing
        Loop
        Rs.Close()

        Dim aryProcessedFiles As New ArrayList()

        Rs = Emagine.GetDataReader("SELECT DISTINCT CodeFileName, PageModuleID FROM qryDeliveryPageModules WHERE DeliveryPageID = '" & intPageID & "' AND CodeFileName IS NOT NULL")
        Do While Rs.Read
            Dim FoundMatch As Boolean = False

            For i As Integer = 0 To aryProcessedFiles.Count - 1
                If aryProcessedFiles(i).ToString = Rs("CodeFileName") Then
                    FoundMatch = True
                End If
            Next

            If Not FoundMatch Then
                aryProcessedFiles.Add(Rs("CodeFileName"))
                Dim File As System.IO.FileInfo = New System.IO.FileInfo(Server.MapPath(Rs("CodeFileName")))

                If File.Exists Then
                    Dim ucPageModule As Control = LoadControl(Rs("CodeFileName"))

                    Dim MyProperty As System.Reflection.PropertyInfo
                    MyProperty = ucPageModule.GetType().GetProperty("PageModuleID")
                    If Not MyProperty Is Nothing Then MyProperty.SetValue(ucPageModule, Rs("PageModuleID"), Nothing)
                    plcModules.Controls.Add(ucPageModule)
                End If
                File = Nothing
            End If
        Loop
        Rs.Close()
    End Sub

    
End Class
