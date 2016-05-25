Imports System.Data
Imports System.Data.SqlClient

Partial Class modules_Navigation_HorizontalNavigation
    Inherits System.Web.UI.UserControl

    Dim _MenuLevelCount As Integer = 0
    Dim _LanguageID As Integer = 0
    Dim _TemplateID As Integer = 0

    Public WriteOnly Property MenuLevels() As Integer
        Set(ByVal value As Integer)
            _MenuLevelCount = value
        End Set
    End Property

    Public WriteOnly Property LanguageID() As Integer
        Set(ByVal value As Integer)
            _LanguageID = value
        End Set
    End Property

    Public WriteOnly Property TemplateID() As Integer
        Set(ByVal value As Integer)
            _TemplateID = value
        End Set
    End Property

    Protected Sub Menu_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Menu.Load
        'Page.ClientScript.RegisterStartupScript(Page.GetType, "Menu", String.Format("{0}.FixListWidth({0});", Menu.ClientID), True)

        Dim PageID As Integer = Emagine.GetNumber(Session("PageID"))
        'If Not IsPostBack Then

        'End If
        Menu.Items.Clear()
        PopulateMenu(PageID)
    End Sub

    Sub PopulateMenu(ByVal intPageID As Integer)
        If _MenuLevelCount >= 0 Then
            Dim Rs As DataSet = GetMenuData()
            Dim Counter As Integer = 0

            For Each MasterRow As DataRow In Rs.Tables("Pages1").Rows()
                Counter += 1

                Dim MasterItem As New Telerik.WebControls.RadMenuItem
                If MasterRow("PageTypeID") > 0 Then
                    MasterItem.NavigateUrl = Pages01.GetPageLink(MasterRow("PageTypeID"), MasterRow("PageKey").ToString(), MasterRow("PageAction").ToString, MasterRow("IsSecure"))
                End If

                MasterItem.ImageOverUrl = MasterRow("MenuImageSelected").ToString
                'MasterItem.CssClass = "Menu-Horizontal-Dynamic-Menu"
                MasterItem.GroupSettings.OffsetX = 0
                MasterItem.GroupSettings.OffsetY = 0

                Select Case MasterRow("PageTypeID")
                    Case 0
                        'MasterItem.Selectable = False
                        Dim NavigateUrl As String = Me.GetFirstChildUrl(MasterRow("PageID"))
                        If NavigateUrl.Length > 0 Then MasterItem.NavigateUrl = NavigateUrl
                    Case 1
                        MasterItem.NavigateUrl = "~/" & MasterRow("PageKey").ToString() & ".htm"
                    Case 2
                        MasterItem.NavigateUrl = MasterRow("PageAction").ToString()
                End Select

                'MasterItem.Text = MasterRow("MenuName")

                If Pages01.IsPageRelated(intPageID, MasterRow("PageID")) Then
                    MasterItem.ImageUrl = MasterRow("MenuImageOn").ToString
                Else
                    MasterItem.ImageUrl = MasterRow("MenuImageOff").ToString().ToString
                End If

                Menu.Items.Add(MasterItem)

                'If Counter < Rs.Tables("Pages1").Rows.Count Then
                    'Dim SeperatorItem As New Telerik.WebControls.RadMenuItem
                    'SeperatorItem.Text = "|"
                    'Menu.Items.Add(SeperatorItem)
                'End If

                If _MenuLevelCount >= 1 And MasterRow("DisplaySubMenu") Then
                    For Each ChildRow1 As DataRow In MasterRow.GetChildRows("Children1")
                        Dim ChildItem1 As New Telerik.WebControls.RadMenuItem
                        ChildItem1.Text = ChildRow1("MenuName")

                        Select Case ChildRow1("PageTypeID")
                            Case 0
                                ChildItem1.NavigateUrl = GetFirstChildUrl(ChildRow1("PageID"))
                            Case Is > 0
                                ChildItem1.NavigateUrl = Pages01.GetPageLink(ChildRow1("PageTypeID"), ChildRow1("PageKey").ToString(), ChildRow1("PageAction").ToString, ChildRow1("IsSecure"))
                        End Select

                        MasterItem.Items.Add(ChildItem1)
                        ChildItem1.GroupSettings.OffsetX = 0
                        ChildItem1.GroupSettings.OffsetY = -1

                        If _MenuLevelCount >= 2 And ChildRow1("DisplaySubMenu") Then
                            For Each ChildRow2 As DataRow In ChildRow1.GetChildRows("Children2")
                                Dim ChildItem2 As New Telerik.WebControls.RadMenuItem
                                ChildItem2.Text = ChildRow2("MenuName")

                                Select Case ChildRow2("PageTypeID")
                                    Case Is > 0
                                        ChildItem2.NavigateUrl = Pages01.GetPageLink(ChildRow2("PageTypeID"), ChildRow2("PageKey").ToString(), ChildRow2("PageAction").ToString, ChildRow2("IsSecure"))
                                End Select

                                ChildItem1.Items.Add(ChildItem2)

                                If _MenuLevelCount >= 3 And ChildRow2("DisplaySubMenu") Then
                                    For Each ChildRow3 As DataRow In ChildRow2.GetChildRows("Children3")
                                        Dim ChildItem3 As New Telerik.WebControls.RadMenuItem
                                        ChildItem3.Text = ChildRow3("MenuName")

                                        Select Case ChildRow3("PageTypeID")
                                            Case Is > 0
                                                ChildItem3.NavigateUrl = Pages01.GetPageLink(ChildRow3("PageTypeID"), ChildRow3("PageKey").ToString(), ChildRow3("PageAction").ToString, ChildRow3("IsSecure"))
                                        End Select

                                        ChildItem2.Items.Add(ChildItem3)
                                    Next
                                End If
                            Next
                        End If
                    Next
                End If
            Next
        End If
    End Sub


    Function GetMenuData() As DataSet
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim CategoryAdapter1 As New SqlDataAdapter("SELECT PageID, PageTypeID, MenuName, PageKey, ParentPageID, MenuImageOn, MenuImageOff, MenuImageSelected, IsSecure, PageAction, DisplaySubMenu FROM qryTemplateNavigation WHERE NavigationType = 'TOP' AND TemplateID = " & _TemplateID & " AND ParentPageID = 0 AND IsHidden = 0 AND StatusID = 20 ORDER BY SortOrder", Conn)
        Dim Rs As New DataSet()

        CategoryAdapter1.Fill(Rs, "Pages1")

        If _MenuLevelCount >= 1 Then
            Dim CategoryAdapter2 As New SqlDataAdapter("SELECT PageID, PageTypeID, MenuName, PageKey, ParentPageID, PageAction, IsSecure, DisplaySubMenu FROM Pages WHERE ParentPageID IN (SELECT PageID FROM qryTemplateNavigation WHERE NavigationType = 'TOP' AND TemplateID = " & _TemplateID & " AND ParentPageID = 0 AND IsHidden = 0 AND StatusID = 20) AND IsHidden = 0 AND StatusID = 20 ORDER BY SortOrder", Conn)
            CategoryAdapter2.Fill(Rs, "Pages2")
            Try
                Rs.Relations.Add("Children1", Rs.Tables("Pages1").Columns("PageID"), Rs.Tables("Pages2").Columns("ParentPageID"))
            Catch ex As Exception
                Emagine.LogError(ex)
            End Try
        End If

        If _MenuLevelCount >= 2 Then
            Dim CategoryAdapter3 As New SqlDataAdapter("SELECT PageID, PageTypeID, MenuName, PageKey, ParentPageID, PageAction, IsSecure, DisplaySubMenu FROM Pages WHERE ParentPageID IN (SELECT PageID FROM Pages WHERE IsHidden = 0 AND StatusID = 20 AND ParentPageID IN (SELECT PageID FROM qryTemplateNavigation WHERE NavigationType = 'TOP' AND TemplateID = " & _TemplateID & " AND ParentPageID = 0 AND IsHidden = 0 AND StatusID = 20)) AND IsHidden = 0 AND StatusID = 20 ORDER BY SortOrder", Conn)
            CategoryAdapter3.Fill(Rs, "Pages3")
            Try
                Rs.Relations.Add("Children2", Rs.Tables("Pages2").Columns("PageID"), Rs.Tables("Pages3").Columns("ParentPageID"))
            Catch ex As Exception
                Emagine.LogError(ex)
            End Try

        End If

        If _MenuLevelCount >= 3 Then
            Dim CategoryAdapter4 As New SqlDataAdapter("SELECT PageID, PageTypeID, MenuName, PageKey, ParentPageID, PageAction, IsSecure, DisplaySubMenu FROM Pages WHERE ParentPageID IN (SELECT PageID FROM Pages WHERE IsHidden=0 AND StatusID=20 AND ParentPageID IN (SELECT PageID FROM Pages WHERE IsHidden = 0 AND StatusID = 20 AND ParentPageID IN (SELECT PageID FROM qryTemplateNavigation WHERE NavigationType = 'TOP' AND TemplateID = " & _TemplateID & " AND ParentPageID = 0 AND IsHidden = 0 AND StatusID = 20))) AND IsHidden = 0 AND StatusID = 20 ORDER BY SortOrder", Conn)
            CategoryAdapter4.Fill(Rs, "Pages4")
            Try
                Rs.Relations.Add("Children3", Rs.Tables("Pages3").Columns("PageID"), Rs.Tables("Pages4").Columns("ParentPageID"))
            Catch ex As Exception
                Emagine.LogError(ex)
            End Try
        End If
        Conn.Close()

        Return Rs
    End Function

    Function GetFirstChildUrl(ByVal intPageID As Integer) As String
        Dim PageID As Integer = intPageID
        Dim PageTypeID As Integer = 0
        Dim NavigateUrl As String = ""

        Do While PageTypeID = 0
            'mod vreeland 4/25/2012
            'Dim PageData As DataTable = Emagine.GetDataTable("SELECT PageID, PageTypeID, PageKey + '.htm' AS NavigateURL FROM Pages WHERE ParentPageID = " & PageID & " ORDER BY SortOrder")
            Dim PageData As DataTable = Emagine.GetDataTable("SELECT PageID, PageTypeID, PageKey + '.htm' AS NavigateURL FROM Pages WHERE ParentPageID = " & PageID & " AND IsHidden = 0 AND StatusID = 20 ORDER BY SortOrder")
            'end mod vreeland 4/25/2012
            If PageData.Rows.Count > 0 Then
                PageID = PageData.Rows(0).Item("PageID")
                PageTypeID = PageData.Rows(0).Item("PageTypeID")
                NavigateUrl = "~/" & PageData.Rows(0).Item("NavigateUrl")
            Else
                PageTypeID = -1
            End If
            PageData.Dispose()
        Loop

        Return NavigateUrl
    End Function

    
End Class
