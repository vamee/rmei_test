Imports System.Data
Imports System.Data.SqlClient

Partial Class Ezedit_MasterPage
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'If CInt(Session("EzEditUserID")) = 0 Or CInt(Session("EzEditLevelID")) = 0 Or CInt(Session("EzEditLanguageID")) = 0 Or CInt(Session("EzEditStatusID")) = 0 Then
        Dim UserID As Integer = 0

        If Session("EzEditUserID") IsNot Nothing Then UserID = Emagine.GetNumber(Session("EzEditUserID"))
        
        If UserID = 0 Then
            
            FormsAuthentication.SignOut()
            FormsAuthentication.RedirectToLoginPage()

            Response.End()
        End If
    End Sub

    Public Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then PopulateMenu()
    End Sub

    Sub linkLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        FormsAuthentication.SignOut()
        FormsAuthentication.RedirectToLoginPage()
    End Sub

    Sub PopulateMenu()
        Dim Rs As DataSet = GetMenuData()
        For Each Row As DataRow In Rs.Tables("Modules").Rows()
            Dim HasPermissions As Boolean = False
            If Session("EzEditLevelID") = 1 Then
                HasPermissions = True
            Else
                HasPermissions = EzeditUser.HasModulePermissions(Session("EzEditUserID"), Row("ModuleKey").ToString())
            End If

            If HasPermissions Then
                Dim MenuItem As New MenuItem(Row("Name").ToString(), "", "", Row("EzeditMenuLink").ToString)
                EzEditNavigation.Items(2).ChildItems.Add(MenuItem)
            End If
        Next
    End Sub

    Function GetMenuData() As DataSet
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("WebsiteDB").ConnectionString)
        Dim ModuleAdapter As New SqlDataAdapter("SELECT ModuleKey, Name, EzeditMenuLink FROM Modules WHERE IsHidden = 0 AND (LanguageID = " & CInt(Session("EzEditLanguageID")) & " OR LanguageID = 0) ORDER BY Name", Conn)
        Dim Rs As New DataSet()
        ModuleAdapter.Fill(Rs, "Modules")
        Return Rs
    End Function

    Protected Sub EzEditNavigation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles EzEditNavigation.Load
        If Not IsPostBack Then
            If Session("EzEditLevelID") = 1 Then
                Dim MenuItem As MenuItem = Nothing
                MenuItem = New MenuItem("", "", "~/App_Themes/EzEdit/Images/navPipe.gif")
                MenuItem.Selectable = False
                EzEditNavigation.Items.Add(MenuItem)


                MenuItem = New MenuItem("", "", "~/App_Themes/EzEdit/Images/topNavAdmin.gif")
                MenuItem.Selectable = False

                MenuItem.ChildItems.Add(New MenuItem("Application Variables", "", "", "~/Ezedit/Admin/AppVariables/"))
                MenuItem.ChildItems.Add(New MenuItem("Error Log", "", "", "~/Ezedit/Admin/ErrorLog/"))
                MenuItem.ChildItems.Add(New MenuItem("EzEdit Users", "", "", "~/Ezedit/Admin/Users/"))
                MenuItem.ChildItems.Add(New MenuItem("Languages", "", "", "~/Ezedit/Admin/Languages/"))
                MenuItem.ChildItems.Add(New MenuItem("Redirects", "", "", "~/Ezedit/Admin/Redirects/"))
                MenuItem.ChildItems.Add(New MenuItem("Templates", "", "", "~/Ezedit/Admin/Templates/"))
                MenuItem.ChildItems.Add(New MenuItem("Template Headers", "", "", "~/Ezedit/Admin/Headers/"))

                EzEditNavigation.Items.Add(MenuItem)
            End If

        End If
    End Sub
End Class

