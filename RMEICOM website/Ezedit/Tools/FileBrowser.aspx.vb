Imports System.IO

Partial Class Ezedit_Tools_FileBrowser
    Inherits System.Web.UI.Page

    Dim RootPath As String = Server.MapPath(GlobalVariables.GetValue("VirtualDefaultFileBrowserPath"))
    Dim _DefaultPath As String = ""
    Dim _FileExtensions As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request("DefaultPath") IsNot Nothing Then
            _DefaultPath = Request("DefaultPath")
            RootPath = Server.MapPath(Request("DefaultPath"))
        Else
            pnlDisplay.Visible = False
            pnlNoPath.Visible = True
            lblError.Text = "DefaultPath was not specified."
        End If

        If Request("FileExtensions") IsNot Nothing Then _FileExtensions = Request("FileExtensions")

        If _FileExtensions.Length > 0 Then
            CompareToStringsValidator1.Enabled = True
            CompareToStringsValidator1.SummaryErrorMessage = "Only files with (" & _FileExtensions & ") extensions are allowed."

            Dim aryFileExtensions As Array = _FileExtensions.Split(",")
            For i As Integer = 0 To aryFileExtensions.GetUpperBound(0)
                CompareToStringsValidator1.Items.Add(New PeterBlum.VAM.CompareToStringsItem(aryFileExtensions(i)))
            Next
        End If

        If Not Page.IsPostBack Then
            Dim FolderPath As String = RootPath

            If _DefaultPath.Length > 0 Then
                If Directory.Exists(Server.MapPath(_DefaultPath)) Then
                    FolderPath = Server.MapPath(_DefaultPath)
                End If
            End If

            If Request("FilePath") IsNot Nothing Then
                Dim FilePath As String = Request("FilePath").Replace("~", "")

                'Response.Write(FilePath)
                If FilePath.Length > 0 Then
                    If IO.File.Exists(Server.MapPath(FilePath)) Then
                        If Directory.Exists(Server.MapPath(FilePath.Replace(Me.FormatFileName(FilePath), ""))) Then
                            FolderPath = Server.MapPath(FilePath.Replace(Me.FormatFileName(FilePath), ""))
                        End If
                    End If
                End If
            End If

            gdvFileNames.DataSource = Me.GetDataTable(FolderPath)
            gdvFileNames.DataBind()
            lblCurrentDirectory.Text = Me.GetVirtualDirectory(FolderPath)
        End If
    End Sub

    Protected Sub lbtnItemName_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Button As LinkButton = sender
        Dim ItemPath As String = Button.CommandArgument
        Dim ItemType As String = Button.CommandName

        If ItemType = "Folder" Then
            gdvFileNames.DataSource = Me.GetDataTable(ItemPath)
            gdvFileNames.DataBind()
            lblCurrentDirectory.Text = Me.GetVirtualDirectory(ItemPath)
        End If
    End Sub

    Function GetDataTable(ByVal strPath As String) As DataTable
        Dim FolderList() As String = Directory.GetDirectories(strPath)
        Dim FileList() As String = Directory.GetFiles(strPath)

        Dim DataTable As New DataTable
        DataTable.Columns.Add(New DataColumn("ItemName", System.Type.GetType("System.String")))
        DataTable.Columns.Add(New DataColumn("ItemType", System.Type.GetType("System.String")))
        DataTable.Columns.Add(New DataColumn("ItemPath", System.Type.GetType("System.String")))
        DataTable.Columns.Add(New DataColumn("ItemImage", System.Type.GetType("System.String")))

        Dim ParentPath As String = RootPath
        Dim FolderName As String = ""
        If Me.FormatFileName(strPath) IsNot Nothing Then FolderName = Me.FormatFileName(strPath).Replace("_", "-")
        If FolderName.Length > 0 Then
            ParentPath = strPath.Replace(FolderName, "")
        End If

        'Response.Write(strPath & "<br><br>" & RootPath)

        'Response.Write(RootPath & " : " & ParentPath)

        If strPath <> RootPath Then
            Dim Row As DataRow = DataTable.NewRow()
            Row("ItemName") = "Up"
            Row("ItemType") = "Folder"
            Row("ItemPath") = ParentPath
            Row("ItemImage") = "/App_Themes/EzEdit/Images/Arrow_Up.png"
            DataTable.Rows.Add(Row)
        End If

        For i As Integer = 0 To FolderList.GetUpperBound(0)
            Dim Row As DataRow = DataTable.NewRow()
            Row("ItemName") = Me.FormatFileName(FolderList(i))
            Row("ItemType") = "Folder"
            Row("ItemPath") = FolderList(i)
            Row("ItemImage") = "/App_Themes/EzEdit/Images/Folder.png"
            DataTable.Rows.Add(Row)
        Next

        For i As Integer = 0 To FileList.GetUpperBound(0)
            Dim Row As DataRow = DataTable.NewRow()
            Row("ItemName") = Me.FormatFileName(FileList(i))
            Row("ItemType") = "File"
            Row("ItemPath") = FileList(i)
            Row("ItemImage") = "/App_Themes/EzEdit/Images/Page_White.png"
            DataTable.Rows.Add(Row)
        Next

        Return DataTable
    End Function

    Protected Sub gdvFileNames_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvFileNames.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ItemPath As String = e.Row.DataItem("ItemPath")
            Dim ItemType As String = e.Row.DataItem("ItemType")

            If ItemType = "File" Then
                Dim lbtnItemName As LinkButton = e.Row.FindControl("lbtnItemName")
                Dim lblItemName As Label = e.Row.FindControl("lblItemName")
                Dim hypPreview As HyperLink = e.Row.FindControl("hypPreview")

                Dim FoundMatch As Boolean = False
                If _FileExtensions.Length > 0 Then
                    Dim aryFileExtensions As Array = _FileExtensions.Split(",")
                    For i As Integer = 0 To aryFileExtensions.GetUpperBound(0)
                        If Emagine.GetFileExtension(ItemPath) = aryFileExtensions(i) Then
                            FoundMatch = True
                            Exit For
                        End If
                    Next
                Else
                    FoundMatch = True
                End If

                If FoundMatch Then
                    Dim FileUrl As String = ItemPath.Replace(Server.MapPath("/"), "").Replace("\", "/")
                    lbtnItemName.OnClientClick = "CloseAndRebind('/" & FileUrl & "');"
                    lblItemName.Visible = False
                    hypPreview.NavigateUrl = "/" & FileUrl
                    hypPreview.Visible = True
                Else
                    lbtnItemName.Visible = False
                    lblItemName.Visible = True
                End If
            End If
        End If
    End Sub

    Function GetVirtualDirectory(ByVal strPath As String) As String
        Return "/" & strPath.Replace(Server.MapPath("/"), "").Replace("\", "/")
    End Function

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        If PeterBlum.VAM.Globals.Page.IsValid Then
            Dim CurrentDirectory As String = lblCurrentDirectory.Text
            If Right(CurrentDirectory, 1) <> "\" Then CurrentDirectory = CurrentDirectory & "\"
            Dim FileName As String = uplFileName.FileName


            If File.Exists(Server.MapPath(CurrentDirectory & FileName)) And cbxOverwriteFile.Checked = False Then
                lblAlert.Text = "This file already exists. If you would like to overwrite it, please check the 'Overwrite' box."
            Else
                uplFileName.SaveAs(Server.MapPath(CurrentDirectory & FileName))

                RadTabStrip1.Tabs(0).Selected = True
                RadMultiPage1.PageViews(0).Selected = True
                gdvFileNames.DataSource = Me.GetDataTable(Server.MapPath(lblCurrentDirectory.Text))
                gdvFileNames.DataBind()
            End If

        End If
    End Sub

    Function FormatFileName(ByVal strFileName As String) As String
        If Len(strFileName) > 0 Then
            If InStr(strFileName, "/") Then
                strFileName = Mid(strFileName, InStrRev(strFileName, "/") + 1)
            Else
                strFileName = Mid(strFileName, InStrRev(strFileName, "\") + 1)
            End If
        End If

        Return strFileName
    End Function
End Class
