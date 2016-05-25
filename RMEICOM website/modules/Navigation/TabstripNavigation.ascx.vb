Imports System.Data
Imports System.Data.SqlClient

Partial Class modules_Navigation_TabstripNavigation
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


    Sub PopulateMenu(ByVal intPageID As Integer)
        If _MenuLevelCount > 0 Then
            Dim Rs As DataSet = GetMenuData()

            For Each MasterRow As DataRow In Rs.Tables("Pages1").Rows()
                Dim MasterItem As New Telerik.WebControls.Tab
                'If MasterRow("PageTypeID") > 0 Then
                '    MasterItem.NavigateUrl = Pages01.GetPageLink(MasterRow("PageTypeID"), MasterRow("PageKey").ToString(), MasterRow("PageAction").ToString, MasterRow("IsSecure"))
                'End If

                MasterItem.ImageOverUrl = MasterRow("MenuImageSelected").ToString
                'MasterItem.CssClass = "Menu-Horizontal-Dynamic-Menu"

                Select Case MasterRow("PageTypeID")
                    Case 0
                        'MasterItem.NavigateUrl = "~/" & Emagine.GetDbValue("SELECT PageKey + '.htm' AS NavigateURL FROM Pages WHERE ParentPageID = " & MasterRow("PageID") & " ORDER BY SortOrder")
                    Case 1
                        'MasterItem.NavigateUrl = "~/" & MasterRow("PageKey").ToString() & ".htm"
                    Case 2
                        'MasterItem.NavigateUrl = MasterRow("PageAction").ToString()
                End Select

                If Pages01.IsPageRelated(intPageID, MasterRow("PageID")) Then
                    MasterItem.ImageUrl = MasterRow("MenuImageOn").ToString
                Else
                    MasterItem.ImageUrl = MasterRow("MenuImageOff").ToString().ToString
                End If

                'MasterItem.Text = MasterRow("MenuName").ToString()


                Menu.Tabs.Add(MasterItem)

                For Each ChildRow1 As DataRow In MasterRow.GetChildRows("Children1")
                    Dim ChildItem1 As New Telerik.WebControls.Tab
                    ChildItem1.Text = ChildRow1("MenuName")

                    ChildItem1.NavigateUrl = Pages01.GetPageLink(ChildRow1("PageTypeID"), ChildRow1("PageKey").ToString(), ChildRow1("PageAction").ToString, ChildRow1("IsSecure"))
                    MasterItem.Tabs.Add(ChildItem1)
                Next
            Next
        End If
    End Sub


    Function GetMenuData() As DataSet
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim CategoryAdapter1 As New SqlDataAdapter("SELECT PageID, PageTypeID, MenuName, PageKey, ParentPageID, MenuImageOn, MenuImageOff, MenuImageSelected, IsSecure, PageAction FROM qryTemplateNavigation WHERE NavigationType = 'TOP' AND TemplateID = " & _TemplateID & " AND ParentPageID = 0 AND IsHidden = 0 AND StatusID = 20 ORDER BY SortOrder", Conn)
        Dim Rs As New DataSet()

        CategoryAdapter1.Fill(Rs, "Pages1")

        Dim CategoryAdapter2 As New SqlDataAdapter("SELECT PageID, PageTypeID, MenuName, PageKey, ParentPageID, PageAction, IsSecure FROM Pages WHERE ParentPageID IN (SELECT PageID FROM qryTemplateNavigation WHERE NavigationType = 'TOP' AND TemplateID = " & _TemplateID & " AND ParentPageID = 0 AND IsHidden = 0 AND StatusID = 20) AND IsHidden = 0 AND StatusID = 20 ORDER BY SortOrder", Conn)
        CategoryAdapter2.Fill(Rs, "Pages2")
        Try
            Rs.Relations.Add("Children1", Rs.Tables("Pages1").Columns("PageID"), Rs.Tables("Pages2").Columns("ParentPageID"))
        Catch ex As Exception
            Emagine.LogError(ex)
        End Try

        Return Rs
    End Function



    Protected Sub Menu_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Menu.Load
        'Page.ClientScript.RegisterStartupScript(Page.GetType, "Menu", String.Format("{0}.FixListWidth({0});", Menu.ClientID), True)

        Dim PageID As Integer = Emagine.GetNumber(Session("PageID"))
        If Not IsPostBack Then PopulateMenu(PageID)
    End Sub
End Class
