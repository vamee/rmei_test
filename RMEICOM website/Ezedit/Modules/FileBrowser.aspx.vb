Imports System.IO

Partial Class Ezedit_Modules_FileBrowser
    Inherits System.Web.UI.Page

    Dim RootPath As String = Server.MapPath("/modules/")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim FolderPath As String = RootPath

            If Request("FilePath") IsNot Nothing Then
                Dim FilePath As String = Request("FilePath").Replace("~", "")
                If FilePath.Length > 0 Then
                    If Directory.Exists(Server.MapPath(FilePath.Replace(Emagine.FormatFileName(FilePath), ""))) Then
                        FolderPath = Server.MapPath(FilePath.Replace(Emagine.FormatFileName(FilePath), ""))
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
        Dim FileList() As String = Directory.GetFiles(strPath, "*.ascx*")

        Dim DataTable As New DataTable
        DataTable.Columns.Add(New DataColumn("ItemName", System.Type.GetType("System.String")))
        DataTable.Columns.Add(New DataColumn("ItemType", System.Type.GetType("System.String")))
        DataTable.Columns.Add(New DataColumn("ItemPath", System.Type.GetType("System.String")))
        DataTable.Columns.Add(New DataColumn("ItemImage", System.Type.GetType("System.String")))

        Dim ParentPath As String = RootPath
        Dim FolderName As String = ""
        If Emagine.FormatFileName(strPath) IsNot Nothing Then FolderName = Emagine.FormatFileName(strPath).Replace("_", "-")
        If FolderName.Length > 0 Then
            ParentPath = strPath.Replace(FolderName, "")
        End If

        'Response.Write(strPath & "<br><br>" & RootPath)

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
            Row("ItemName") = Emagine.FormatFileName(FolderList(i))
            Row("ItemType") = "Folder"
            Row("ItemPath") = FolderList(i)
            Row("ItemImage") = "/App_Themes/EzEdit/Images/Folder.png"
            DataTable.Rows.Add(Row)
        Next

        For i As Integer = 0 To FileList.GetUpperBound(0)
            Dim Row As DataRow = DataTable.NewRow()
            Row("ItemName") = Emagine.FormatFileName(FileList(i))
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
            If ItemType = "Folder" Then
                'Dim FileList() As String = Directory.GetFiles(ItemPath)

                'If FileList.GetUpperBound(0) = 0 Then e.Row.Visible = False
            ElseIf ItemType = "File" Then
                Dim lbtnItemName As LinkButton = e.Row.FindControl("lbtnItemName")
                Dim lblItemName As Label = e.Row.FindControl("lblItemName")
                If Emagine.GetFileExtension(ItemPath) = ".ascx" Then
                    Dim FileUrl As String = ItemPath.Replace(Server.MapPath("/"), "").Replace("\", "/")
                    lbtnItemName.OnClientClick = "CloseAndRebind('/" & FileUrl & "');"
                    lblItemName.Visible = False
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
            Dim FileName As String = uplFileName.FileName


            If File.Exists(Server.MapPath(CurrentDirectory & FileName)) And cbxOverwriteFile.Checked = False Then
                lblAlert.Text = "This file already exists. If you would like to overwrite it, please check the 'Overwrite' box."
            Else
                uplFileName.SaveAs(Server.MapPath(CurrentDirectory & FileName))

                If uplCodeBehind.HasFile Then
                    uplCodeBehind.SaveAs(Server.MapPath(CurrentDirectory & uplCodeBehind.FileName))
                End If

                RadTabStrip1.Tabs(0).Selected = True
                RadMultiPage1.PageViews(0).Selected = True
                gdvFileNames.DataSource = Me.GetDataTable(Server.MapPath(lblCurrentDirectory.Text))
                gdvFileNames.DataBind()
            End If

        End If
    End Sub
End Class
