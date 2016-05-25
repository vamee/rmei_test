Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class Modules
    Inherits Emagine

    Public Shared Function GetModulePages(ByVal strModuleKey As String, ByVal strForeignValue As String) As SqlDataReader
        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim strSQL As String = "sp_Modules_GetModulePages"
        Dim objCommand As New SqlCommand(strSQL, objConn)

        objCommand.CommandType = CommandType.StoredProcedure
        objCommand.Parameters.AddWithValue("@ModuleKey", strModuleKey)
        objCommand.Parameters.AddWithValue("@ForeignValue", strForeignValue)
        objConn.Open()
        Dim dtrResults As SqlDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection)

        Return dtrResults
        objConn.Close()
    End Function

    Public Shared Function GetModuleTypes(ByVal strModuleKey As String) As SqlDataReader
        Dim objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim strSQL As String = "sp_Modules_GetModuleTypes"
        Dim dtrResults As SqlDataReader = Nothing
        Dim objCommand As New SqlCommand(strSQL, objConn)

        objCommand.CommandType = CommandType.StoredProcedure
        objCommand.Parameters.AddWithValue("@ModuleKey", strModuleKey)

        Try
            objConn.Open()
            dtrResults = objCommand.ExecuteReader(CommandBehavior.CloseConnection)
        Catch ex As Exception
            Emagine.LogError(ex)
	Finally
            'objConn.Close()
        End Try

        Return dtrResults
    End Function

    Public Shared Function GetApplications(Optional ByVal blnShowHidden As Boolean = True) As SqlDataReader
        Dim Sql As String = ""

        If Not blnShowHidden Then
            Sql = "SELECT ModuleKey, Name FROM Modules WHERE IsHidden = 'False' ORDER BY Name"
        Else
            Sql = "SELECT ModuleKey, Name FROM Modules ORDER BY Name"
        End If

        Dim dtr As SqlDataReader = Emagine.GetDataReader(Sql)
        Return dtr
    End Function

    Public Shared Function GetModuleName(ByVal strModuleKey As String) As String
        Return Emagine.GetDbValue("SELECT Name FROM Modules WHERE ModuleKey = '" & strModuleKey & "'")
    End Function

    Public Shared Function GetPropertyValue(ByVal intPageModuleId As Integer, ByVal intPropertyId As Integer) As String
        Dim strSQL As String = "SELECT PropertyValue FROM PageModuleProperties WHERE PageModuleID = " & intPageModuleId & " AND PropertyID = " & intPropertyId
        'HttpContext.Current.Response.Write(strSQL)
        'HttpContext.Current.Response.End()
        Return Emagine.GetDbValue(strSQL)
    End Function

    Public Shared Function GetModuleProperties(ByVal strModuleKey As String) As SqlDataReader
        Dim dtr As SqlDataReader = Emagine.GetDataReader("SELECT * FROM ModuleProperties WHERE ModuleKey = '" & strModuleKey & "' ORDER BY SortOrder")
        Return dtr
    End Function

    Public Shared Function GetModuleProperties(ByVal strModuleKey As String, ByVal intModuleTypeID As Integer) As SqlDataReader
        Dim dtr As SqlDataReader = Emagine.GetDataReader("SELECT * FROM ModuleProperties WHERE ModuleKey = '" & strModuleKey & "' AND ModuleTypeID = " & intModuleTypeID & " ORDER BY SortOrder")
        Return dtr
    End Function

    Public Shared Function GetModuleTypes() As Object
        Throw New NotImplementedException()
    End Function

    Public Shared Function GetModuleProperties(ByVal strModuleKey As String, ByVal intModuleTypeID As Integer, ByVal intCodeFileID As Integer) As SqlDataReader
        Dim dtr As SqlDataReader = Emagine.GetDataReader("SELECT * FROM ModuleProperties WHERE ModuleKey = '" & strModuleKey & "' AND (ModuleTypeID = " & intModuleTypeID & "OR ModuleTypeID = 0) AND (ModuleCodeFileID = " & intCodeFileID & " OR ModuleCodeFileID = 0) ORDER BY SortOrder")
        Return dtr
    End Function

    Public Shared Function GetPropertyOptions(ByVal intPropertyTypeID As Integer, ByVal strPropertyName As String, ByVal intPropertyID As Integer, ByVal strSelectedValue As String, ByVal strClass As String) As Control
        Dim Result As Control = Nothing
        Dim dtrProperties As SqlDataReader
        Dim arySelectedValues As Array = Nothing
        If Len(strSelectedValue) > 0 Then arySelectedValues = Split(strSelectedValue, "||")
        Dim Sql As String = "SELECT * FROM ModulePropertyOptions WHERE PropertyID = " & intPropertyID

        dtrProperties = Emagine.GetDataReader(Sql)
        If dtrProperties.HasRows() Then
            Select Case intPropertyTypeID
                Case 2
                    Dim ddl As New DropDownList
                    ddl.ID = strPropertyName
                    ddl.CssClass = strClass
                    While dtrProperties.Read
                        Dim Item As New ListItem
                        Item.Value = dtrProperties("OptionValue")
                        ddl.Items.Add(Item)
                    End While
                    ddl.SelectedValue = strSelectedValue
                    Result = ddl
                Case 3
                    Dim cbl As New CheckBoxList
                    cbl.ID = strPropertyName
                    cbl.CssClass = strClass
                    cbl.RepeatDirection = RepeatDirection.Horizontal
                    While dtrProperties.Read
                        Dim Item As New ListItem
                        Item.Value = dtrProperties("OptionValue")
                        If IsArray(arySelectedValues) Then
                            Dim i As Integer
                            For i = 0 To UBound(arySelectedValues)
                                If Trim(arySelectedValues(i)) = dtrProperties("OptionValue") Then
                                    Item.Selected = True
                                    Exit For
                                End If
                            Next
                        End If
                        cbl.Items.Add(Item)
                    End While
                    cbl.SelectedValue = strSelectedValue
                    Result = cbl
                    'Case Else
                    'Result = Nothing
            End Select
        End If

        dtrProperties.Close()

        Return Result
    End Function
End Class

Public Class ModuleCategory

    Public CategoryID As Integer = -1
    Public LanguageID As Integer = -1
    Public StatusID As Integer = 20
    Public ModuleKey As String = ""
    Public CategoryName As String = ""
    Public Description As String = ""
    Public PublishToRss As Boolean = False
    Public RssTitle As String = ""
    Public RssDescription As String = ""
    Public RssManagingEditor As String = ""
    Public RssImageUrl As String = ""

    Public Shared Function GetModuleCategories(ByVal strModuleKey As String) As SqlDataReader
        Dim Rs As SqlDataReader = Emagine.GetDataReader("SELECT * FROM ModuleCategories WHERE ModuleCategories.ModuleKey = '" & strModuleKey & "' ORDER BY CategoryName")
        Return Rs
    End Function

    Public Shared Function GetModuleCategories(ByVal strModuleKey As String, ByVal intStatusID As Integer, ByVal intLanguageID As Integer) As SqlDataReader
        Dim Rs As SqlDataReader = Emagine.GetDataReader("SELECT * FROM ModuleCategories WHERE ModuleKey = '" & strModuleKey & "' AND StatusID = " & intStatusID & " AND LanguageID = " & intLanguageID & " ORDER BY CategoryName")
        Return Rs
    End Function

    Public Shared Function GetModuleCategory(ByVal intCategoryID As Integer) As ModuleCategory
        Dim Category As New ModuleCategory
        Dim dtr As SqlDataReader = Emagine.GetDataReader("SELECT * FROM ModuleCategories WHERE CategoryID = " & intCategoryID)
        If dtr.Read Then
            Category.CategoryID = dtr("CategoryId")
            Category.ModuleKey = dtr("ModuleKey").ToString
            Category.CategoryName = dtr("CategoryName").ToString
            Category.Description = dtr("Description").ToString
            Category.PublishToRss = dtr("PublishToRss")
            Category.RssTitle = dtr("RssTitle").ToString
            Category.RssDescription = dtr("RssDescription").ToString
            Category.RssManagingEditor = dtr("RssManagingEditor").ToString
            Category.RssImageUrl = dtr("RssImageUrl").ToString
        Else
            Category = Nothing
        End If
        Return Category
    End Function

    Public Shared Function GetModuleCategoryName(ByVal intCategoryId As String) As String
        Return Emagine.GetDbValue("SELECT CategoryName FROM ModuleCategories WHERE CategoryID = " & intCategoryId)
    End Function

    Public Function InsertModuleCategory(ByVal Category As ModuleCategory) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim Result As Boolean = False
        Dim SqlBuilder As New StringBuilder

        SqlBuilder.Append("INSERT INTO ModuleCategories (ModuleKey, StatusID, LanguageID, CategoryName, Description, PublishToRss, RssTitle, RssDescription, RssManagingEditor, RssImageUrl) ")
        SqlBuilder.Append("VALUES (@ModuleKey, @StatusID, @LanguageID, @CategoryName, @Description, @PublishToRss, @RssTitle, @RssDescription, @RssManagingEditor, @RssImageUrl)")

        Dim Command As New SqlCommand(SqlBuilder.ToString, Conn)
        Command.Parameters.AddWithValue("@ModuleKey", Category.ModuleKey)
        Command.Parameters.AddWithValue("@StatusID", Category.StatusID)
        Command.Parameters.AddWithValue("@LanguageID", Category.LanguageID)
        Command.Parameters.AddWithValue("@CategoryName", Category.CategoryName)
        Command.Parameters.AddWithValue("@Description", Category.Description)
        Command.Parameters.AddWithValue("@PublishToRss", Category.PublishToRss)
        Command.Parameters.AddWithValue("@RssTitle", Category.RssTitle)
        Command.Parameters.AddWithValue("@RssDescription", Category.RssDescription)
        Command.Parameters.AddWithValue("@RssManagingEditor", Category.RssManagingEditor)
        Command.Parameters.AddWithValue("@RssImageUrl", Category.RssImageUrl)

        Try
            Conn.Open()
            Result = Command.ExecuteNonQuery()

        Catch ex As Exception
            Emagine.LogError(ex)

        Finally
            Conn.Close()
        End Try

        Return Result
    End Function

    Public Function UpdateModuleCategory(ByVal Category As ModuleCategory) As Boolean

        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim Result As Boolean = False
        Dim SqlBuilder As New StringBuilder

        SqlBuilder.Append("UPDATE ModuleCategories SET ")
        SqlBuilder.Append("CategoryName=@CategoryName, StatusID=@StatusID, LanguageID=@LanguageID, Description=@Description, PublishToRss=@PublishToRss, RssTitle=@RssTitle, RssDescription=@RssDescription, RssManagingEditor=@RssManagingEditor, RssImageUrl=@RssImageUrl ")
        SqlBuilder.Append("WHERE CategoryID = @CategoryID")

        Dim Command As New SqlCommand(SqlBuilder.ToString, Conn)
        Command.Parameters.AddWithValue("@CategoryID", Category.CategoryID)
        Command.Parameters.AddWithValue("@ModuleKey", Category.ModuleKey)
        Command.Parameters.AddWithValue("@StatusID", Category.StatusID)
        Command.Parameters.AddWithValue("@LanguageID", Category.LanguageID)
        Command.Parameters.AddWithValue("@CategoryName", Category.CategoryName)
        Command.Parameters.AddWithValue("@Description", Category.Description)
        Command.Parameters.AddWithValue("@PublishToRss", Category.PublishToRss)
        Command.Parameters.AddWithValue("@RssTitle", Category.RssTitle)
        Command.Parameters.AddWithValue("@RssDescription", Category.RssDescription)
        Command.Parameters.AddWithValue("@RssManagingEditor", Category.RssManagingEditor)
        Command.Parameters.AddWithValue("@RssImageUrl", Category.RssImageUrl)

        Try
            Conn.Open()
            Result = Command.ExecuteNonQuery()

        Catch ex As Exception
            Emagine.LogError(ex)

        Finally
            Conn.Close()
        End Try

        Return Result
    End Function

    Public Shared Sub DeleteModuleCategory(ByVal intCategoryId As Integer, ByVal strModuleKey As String)
        Emagine.ExecuteSQL("DELETE FROM ModuleCategories WHERE CategoryID = " & intCategoryId)
        Emagine.ExecuteSQL("DELETE FROM PageModuleProperties WHERE PageModuleID IN (SELECT PageModuleID FROM PageModules WHERE ModuleKey = '" & strModuleKey & "' AND ForeignValue = " & intCategoryId)
        Emagine.ExecuteSQL("DELETE FROM PageModules WHERE ModuleKey = '" & strModuleKey & "' AND ForeignValue = " & intCategoryId)

        'Dim Rs As SqlDataReader = Emagine.GetDataReader("SELECT * FROM qryTableSchema WHERE ModuleKey = '" & strModuleKey & "'")
        'Do While Rs.Read
        '    If Len(Rs("ParentTableName").ToString) > 0 Then
        '        Emagine.ExecuteSQL("DELETE FROM " & Rs("TableName") & " WHERE " & Rs("ParentKeyField") & " NOT IN (SELECT " & Rs("ParentKeyField") & " FROM " & Rs("ParentTableName") & ")")
        '        Emagine.ExecuteSQL("DELETE FROM " & Rs("TableName") & " WHERE " & Rs("ParentKeyField") & " IN (SELECT " & Rs("ParentKeyField") & " FROM " & Rs("ParentTableName") & " WHERE CategoryID = " & intCategoryId & ")")

        '    Else
        '        Emagine.ExecuteSQL("DELETE FROM " & Rs("TableName") & " WHERE CategoryID = " & intCategoryId)
        '    End If
        'Loop
        'Rs.Close()
        'Rs = Nothing

    End Sub
End Class

'Public Class ModuleItemImages
'    Public Shared Function GetItemImages(ByVal strModuleKey As String, ByVal intItemID As Integer) As DataTable
'        Return Emagine.GetDataTable("SELECT * FROM qryModuleItemImages WHERE ModuleKey = '" & strModuleKey & "' AND ItemID = " & intItemID & " ORDER BY SortOrder")
'    End Function
'End Class


'Public Class ModuleItemImage
'    Public ImageID As Integer = -1
'    Public ModuleImageID As Integer = -1
'    Public ModuleKey As String = ""
'    Public ItemID As Integer = -1
'    Public ImageUrl As String = ""
'    Public SortOrder As Integer = -1
'    Public IsEnabled As Boolean = False

'    Public Shared Function GetModuleItemImage(ByVal intImageID As Integer) As ModuleItemImage
'        Dim MyModuleItemImage As New ModuleItemImage

'        Dim ImageData As DataTable = Emagine.GetDataTable("SELECT * FROM ModuleItemImages WHERE ImageID = " & intImageID)
'        If ImageData.Rows.Count > 0 Then
'            MyModuleItemImage.ImageID = ImageData.Rows(0).Item("ImageID")
'            MyModuleItemImage.ModuleImageID = ImageData.Rows(0).Item("ModuleImageID")
'            MyModuleItemImage.ModuleKey = ImageData.Rows(0).Item("ModuleKey")
'            MyModuleItemImage.ItemID = ImageData.Rows(0).Item("ItemID")
'            MyModuleItemImage.ImageUrl = ImageData.Rows(0).Item("ImageUrl")
'            MyModuleItemImage.SortOrder = ImageData.Rows(0).Item("SortOrder")
'            MyModuleItemImage.IsEnabled = ImageData.Rows(0).Item("IsEnabled")
'        End If

'        Return MyModuleItemImage
'    End Function

'    Public Shared Function GetModuleItemImage(ByVal intModuleImageID As Integer, ByVal strModuleKey As String, ByVal intItemID As Integer) As ModuleItemImage
'        Dim MyModuleItemImage As New ModuleItemImage
'        Dim SqlBuilder As New StringBuilder

'        SqlBuilder.Append("SELECT * FROM ModuleItemImages WHERE ")
'        SqlBuilder.Append("ModuleImageID = " & intModuleImageID & " ")
'        SqlBuilder.Append("AND ModuleKey = '" & strModuleKey & "' ")
'        SqlBuilder.Append("AND ItemID = " & intItemID)

'        Dim ImageData As DataTable = Emagine.GetDataTable(SqlBuilder.ToString)
'        If ImageData.Rows.Count > 0 Then
'            MyModuleItemImage.ImageID = ImageData.Rows(0).Item("ImageID")
'            MyModuleItemImage.ModuleImageID = ImageData.Rows(0).Item("ModuleImageID")
'            MyModuleItemImage.ModuleKey = ImageData.Rows(0).Item("ModuleKey")
'            MyModuleItemImage.ItemID = ImageData.Rows(0).Item("ItemID")
'            MyModuleItemImage.ImageUrl = ImageData.Rows(0).Item("ImageUrl")
'            MyModuleItemImage.SortOrder = ImageData.Rows(0).Item("SortOrder")
'            MyModuleItemImage.IsEnabled = ImageData.Rows(0).Item("IsEnabled")
'        End If

'        Return MyModuleItemImage
'    End Function

'    Public Shared Function Add(ByVal objImage As ModuleItemImage) As Boolean
'        Dim SqlBuilder As New StringBuilder
'        SqlBuilder.Append("INSERT INTO ModuleItemImages ")
'        SqlBuilder.Append("(ModuleImageID,ModuleKey,ItemID,ImageUrl,SortOrder,IsEnabled) ")
'        SqlBuilder.Append("VALUES ")
'        SqlBuilder.Append("(@ModuleImageID,@ModuleKey,@ItemID,@ImageUrl,@SortOrder,@IsEnabled)")

'        Dim Command As New SqlCommand
'        Command.CommandType = CommandType.Text
'        Command.Parameters.AddWithValue("@ModuleImageID", objImage.ModuleImageID)
'        Command.Parameters.AddWithValue("@ModuleKey", objImage.ModuleKey)
'        Command.Parameters.AddWithValue("@ItemID", objImage.ItemID)
'        Command.Parameters.AddWithValue("@ImageUrl", objImage.ImageUrl)
'        Command.Parameters.AddWithValue("@SortOrder", objImage.SortOrder)
'        Command.Parameters.AddWithValue("@IsEnabled", objImage.IsEnabled)

'        Return Emagine.ExecuteSQL(SqlBuilder.ToString, Command)
'    End Function

'    Public Shared Function Update(ByVal objImage As ModuleItemImage) As Boolean
'        Dim SqlBuilder As New StringBuilder
'        SqlBuilder.Append("UPDATE ModuleItemImages SET ")
'        SqlBuilder.Append("ModuleImageID=@ModuleImageID,")
'        SqlBuilder.Append("ModuleKey=@ModuleKey,")
'        SqlBuilder.Append("ItemID=@ItemID,")
'        SqlBuilder.Append("ImageUrl=@ImageUrl,")
'        SqlBuilder.Append("SortOrder=@SortOrder,")
'        SqlBuilder.Append("IsEnabled=@IsEnabled ")
'        SqlBuilder.Append("WHERE ImageID=@ImageID")

'        Dim Command As New SqlCommand
'        Command.CommandType = CommandType.Text
'        Command.Parameters.AddWithValue("@ImageID", objImage.ImageID)
'        Command.Parameters.AddWithValue("@ModuleImageID", objImage.ModuleImageID)
'        Command.Parameters.AddWithValue("@ModuleKey", objImage.ModuleKey)
'        Command.Parameters.AddWithValue("@ItemID", objImage.ItemID)
'        Command.Parameters.AddWithValue("@ImageUrl", objImage.ImageUrl)
'        Command.Parameters.AddWithValue("@SortOrder", objImage.SortOrder)
'        Command.Parameters.AddWithValue("@IsEnabled", objImage.IsEnabled)

'        Return Emagine.ExecuteSQL(SqlBuilder.ToString, Command)
'    End Function
'End Class

Public Class PageModule

    Public PageModuleId As Integer
    Public PageID As Integer
    Public CodeFileID As Integer
    Public FormPageTypeID As Integer
    Public ModuleKey As String
    Public ForeignKey As String
    Public ForeignValue As String
    Public SortOrder As Integer

    Public Shared Function GetModulePages(ByVal strModuleKey As String, ByVal strForeignKey As String, ByVal strForeignValue As String) As SqlDataReader
        Dim strSQL As String = "SELECT * FROM qryDisplayPageModules WHERE ModuleKey = '" & strModuleKey & "' AND ForeignValue = '" & strForeignValue & "'"
        'HttpContext.Current.Response.Write(strSQL)
        'HttpContext.Current.Response.End()
        Dim dtr As SqlDataReader = Emagine.GetDataReader(strSQL)
        Return dtr
    End Function

    Public Function GetModulePage(ByVal intPageModuleId As Integer) As PageModule
        Dim PageModule As New PageModule
        Dim dtr As SqlDataReader = Emagine.GetDataReader("SELECT PageID, CodeFileID, FormPageTypeID, SortOrder FROM PageModules WHERE PageModuleID = " & intPageModuleId)
        If dtr.Read Then
            PageModule.PageID = dtr("PageID")
            PageModule.CodeFileID = dtr("CodeFileID")
            PageModule.FormPageTypeID = dtr("FormPageTypeID")
            PageModule.SortOrder = dtr("SortOrder")
        Else
            PageModule = Nothing
        End If
        Return PageModule
    End Function

    Public Shared Function GetPageModuleIDByPageKey(ByVal strResourcePageKey As String) As Integer
        Dim ModuleType As String = ""
        Dim ResourceID As String = ""
        Dim PageModuleID As Integer = 0

        If InStr(strResourcePageKey, "^") > 0 Then
            Dim aryPageKeys As Array = strResourcePageKey.Split("^")
            strResourcePageKey = aryPageKeys(0)
        End If

        If Len(Trim(strResourcePageKey)) > 0 Then
            Dim SQL As String = "SELECT ResourceType, ResourceID FROM qryResources WHERE ResourcePageKey = '" & strResourcePageKey & "'"
            Dim Rs As SqlDataReader = Emagine.GetDataReader(SQL)
            If Rs.Read Then
                ModuleType = Rs(0)
                ResourceID = Rs(1)
            End If
            Rs.Close()

            If Len(ModuleType) > 0 And Len(ResourceID) > 0 Then
                Select Case ModuleType
                    Case "PR01"
                        SQL = "SELECT PageModuleID FROM qryArticles WHERE ResourceID = '" & ResourceID & "'"

                    Case "DL01"
                        SQL = "SELECT PageModuleID FROM qryDownloads WHERE ResourceID = '" & ResourceID & "'"

                    Case "Careers01"
                        SQL = "SELECT PageModuleID FROM qryCareers WHERE ResourceID = '" & ResourceID & "'"

                    Case "Links01"
                        SQL = "SELECT PageModuleID FROM qryContentLinks WHERE ResourceID = '" & ResourceID & "'"

                    Case "Events01"
                        SQL = "SELECT PageModuleID FROM qryEvents WHERE ResourceID = '" & ResourceID & "'"
                End Select

                Rs = Emagine.GetDataReader(SQL)
                If Rs.Read Then PageModuleID = Rs(0)
                Rs.Close()
            End If
        End If

        Return PageModuleID
    End Function

    Public Shared Function GetPageModuleIDByResourceID(ByVal strResourceID As String) As Integer
        Dim ModuleType As String = ""
        Dim PageModuleID As Integer = 0

        If InStr(strResourceID, "^") > 0 Then
            Dim aryResources As Array = strResourceID.Split("^")
            strResourceID = aryResources(0)
        End If

        If Len(Trim(strResourceID)) > 0 Then
            Dim SQL As String = "SELECT ResourceType FROM qryResources WHERE ResourceID = '" & strResourceID & "'"
            Dim Rs As SqlDataReader = Emagine.GetDataReader(SQL)
            If Rs.Read Then
                ModuleType = Rs(0)
            End If
            Rs.Close()

            If Len(ModuleType) > 0 And Len(strResourceID) > 0 Then
                Select Case ModuleType
                    Case "PR01"
                        SQL = "SELECT PageModuleID FROM qryArticles WHERE ResourceID = '" & strResourceID & "'"

                    Case "DL01"
                        SQL = "SELECT PageModuleID FROM qryDownloads WHERE ResourceID = '" & strResourceID & "'"

                    Case "Careers01"
                        SQL = "SELECT PageModuleID FROM qryCareers WHERE ResourceID = '" & strResourceID & "'"

                    Case "Links01"
                        SQL = "SELECT PageModuleID FROM qryContentLinks WHERE ResourceID = '" & strResourceID & "'"

                    Case "Events01"
                        SQL = "SELECT PageModuleID FROM qryEvents WHERE ResourceID = '" & strResourceID & "'"

                    Case "Training"
                        SQL = "SELECT PageModuleID FROM qryTrainingEvents WHERE ResourceID = '" & strResourceID & "'"
                End Select

                Rs = Emagine.GetDataReader(SQL)
                If Rs.Read Then PageModuleID = Rs(0)
                Rs.Close()
            End If
        End If

        Return PageModuleID
    End Function


    Public Function Insert(ByVal objPageModule As PageModule) As Integer

        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_Pages01_AddModule"
        Dim PageModuleID As Integer = 0

        Dim Cmd As New SqlCommand(SQL, Conn)

        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.AddWithValue("@PageID", objPageModule.PageID)
        Cmd.Parameters.AddWithValue("@CodeFileID", objPageModule.CodeFileID)
        Cmd.Parameters.AddWithValue("@FormPageTypeID", objPageModule.FormPageTypeID)
        Cmd.Parameters.AddWithValue("@ModuleKey", objPageModule.ModuleKey)
        Cmd.Parameters.AddWithValue("@ForeignKey", objPageModule.ForeignKey)
        Cmd.Parameters.AddWithValue("@ForeignValue", objPageModule.ForeignValue)
        Cmd.Parameters.AddWithValue("@SortOrder", objPageModule.SortOrder)

        Try
            Conn.Open()
            Dim DataReader As SqlDataReader = Cmd.ExecuteReader()
            DataReader.Read()
            PageModuleID = DataReader(0)
            DataReader.Close()

        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            Conn.Close()
        End Try

        Return PageModuleID
    End Function

    Public Function Update(ByVal objPageModule As PageModule) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_Pages01_UpdateModule"
        Dim Result As Boolean = False

        Dim Cmd As New SqlCommand(SQL, Conn)

        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.AddWithValue("@PageModuleID", objPageModule.PageModuleId)
        Cmd.Parameters.AddWithValue("@PageID", objPageModule.PageID)
        Cmd.Parameters.AddWithValue("@CodeFileID", objPageModule.CodeFileID)
        Cmd.Parameters.AddWithValue("@FormPageTypeID", objPageModule.FormPageTypeID)
        Cmd.Parameters.AddWithValue("@ModuleKey", objPageModule.ModuleKey)
        Cmd.Parameters.AddWithValue("@ForeignKey", objPageModule.ForeignKey)
        Cmd.Parameters.AddWithValue("@ForeignValue", objPageModule.ForeignValue)
        Cmd.Parameters.AddWithValue("@SortOrder", objPageModule.SortOrder)

        Try
            Conn.Open()
            Result = Cmd.ExecuteNonQuery()
            'Result = True

        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            Conn.Close()
        End Try

        Return Result
    End Function

    'Public Shared Sub DeleteModuleCategory(ByVal intCategoryId As Integer, ByVal strModuleKey As String)
    '    Emagine.ExecuteSQL("DELETE FROM ModuleCategories WHERE CategoryID = " & intCategoryId)
    '    Emagine.ExecuteSQL("DELETE FROM PageModuleProperties WHERE PageModuleID IN (SELECT PageModuleID FROM PageModules WHERE ModuleKey = '" & strModuleKey & "' AND ForeignValue = " & intCategoryId)
    '    Emagine.ExecuteSQL("DELETE FROM PageModules WHERE ModuleKey = '" & strModuleKey & "' AND ForeignValue = " & intCategoryId)
    '    Emagine.ExecuteSQL("DELETE FROM Articles WHERE CategoryID = " & intCategoryId)
    'End Sub



End Class

Public Class PageModuleProperty

    Public PageModuleId As Integer
    Public PropertyID As Integer
    Public PropertyValue As String
    
    Public Shared Function Insert(ByVal intPageModuleID As Integer, ByVal intPropertyID As Integer, ByVal strPropertyValue As String) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_Pages01_AddModuleProperty"
        Dim Result As Boolean = False

        Dim Cmd As New SqlCommand(SQL, Conn)

        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.AddWithValue("@PageModuleID", intPageModuleID)
        Cmd.Parameters.AddWithValue("@PropertyID", intPropertyID)
        Cmd.Parameters.AddWithValue("@PropertyValue", strPropertyValue)

        Try
            Conn.Open()
            Cmd.ExecuteNonQuery()
            Result = True

        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            Conn.Close()
        End Try

        Return Result
    End Function

    Public Shared Function Insert(ByVal objPageModuleProperty As PageModuleProperty) As Boolean
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_Pages01_AddModuleProperty"
        Dim Result As Boolean = False

        Dim Cmd As New SqlCommand(SQL, Conn)

        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.AddWithValue("@PageModuleID", objPageModuleProperty.PageModuleId)
        Cmd.Parameters.AddWithValue("@PropertyID", objPageModuleProperty.PropertyID)
        Cmd.Parameters.AddWithValue("@PropertyValue", objPageModuleProperty.PropertyValue)

        Try
            Conn.Open()
            Cmd.ExecuteNonQuery()
            Result = True

        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            Conn.Close()
        End Try

        Return Result
    End Function

    Public Shared Function DeleteAll(ByVal intPageModuleID As Integer)
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim SQL As String = "sp_Pages01_DeleteAllModuleProperties"
        Dim Result As Boolean = False

        Dim Cmd As New SqlCommand(SQL, Conn)

        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.AddWithValue("@PageModuleID", intPageModuleID)

        Try
            Conn.Open()
            Cmd.ExecuteNonQuery()
            Result = True

        Catch ex As Exception
            Emagine.LogError(ex)
        Finally
            Conn.Close()
        End Try

        Return Result
    End Function

    Public Shared Function GetProperty(ByVal intPageModuleID As Integer, ByVal strPropertyName As String) As String
        Dim SQL As String = "SELECT PropertyValue FROM qryPageModuleProperties WHERE PageModuleID = " & intPageModuleID & " AND PropertyName = '" & strPropertyName & "'"

        Return Emagine.GetDbValue(SQL)
    End Function

End Class


