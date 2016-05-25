
Partial Class modules_Navigation_Breadcrumbs
    Inherits System.Web.UI.UserControl

    Dim _SeperatorHTML


    Property SeperatorHTML() As String
        Get
            Return _SeperatorHTML
        End Get
        Set(ByVal Value As String)
            _SeperatorHTML = Value
        End Set
    End Property

    Function GetBreadcrumbs(ByVal intPageID As Integer)
        Dim Breadcrumbs As String = ""
        Dim SQL As String = "SELECT PageID, PageTypeID, ParentPageID, PageName, PageKey, PageAction FROM Pages WHERE PageID = " & intPageID
        Dim ParentPageID As Integer = 0
        Dim PageData As Data.SqlClient.SqlDataReader
        PageData = Emagine.GetDataReader(SQL)

        If PageData.Read Then
            ParentPageID = PageData("ParentPageID")

            Breadcrumbs = "<span class='Breadcrumb-Selected'>" & PageData("PageName") & "</span>"
            If ParentPageID > 0 Then Breadcrumbs = SeperatorHTML & Breadcrumbs
        End If
        PageData.Close()

        Do While ParentPageID > 0
            SQL = "SELECT PageID, PageTypeID, ParentPageID, PageName, PageKey, PageAction FROM Pages WHERE PageID = " & ParentPageID
            PageData = Emagine.GetDataReader(SQL)

            If PageData.Read Then
                ParentPageID = PageData("ParentPageID")

                Select Case PageData("PageTypeID")
                    Case 0
                        'Breadcrumbs = "<span class='Breadcrumb-NoLink'>" & PageData("PageName") & "</span>" & Breadcrumbs
                        'Response.Write(Request.RawUrl & "<br>")
                        Dim Url As String = "/" & Emagine.GetDbValue("SELECT PageKey + '.htm' AS NavigateURL FROM Pages WHERE StatusID = 20 AND IsHidden = 0 AND ParentPageID = " & PageData("PageID") & " ORDER BY SortOrder")
                        If Request.RawUrl = Url Then
                            Breadcrumbs = "<span class='Breadcrumb-NoLink'>" & PageData("PageName") & "</span>" & Breadcrumbs
                        Else
                            Breadcrumbs = "<a href='" & Url & "' class='Breadcrumb-Link'>" & PageData("PageName") & "</a>" & Breadcrumbs
                        End If

                    Case 2
                        Breadcrumbs = "<a href='" & PageData("PageAction") & "' class='Breadcrumb-Link'>" & PageData("PageName") & "</a>" & Breadcrumbs
                    Case Else
                        Breadcrumbs = "<a href='/" & PageData("PageKey") & ".htm' class='Breadcrumb-Link'>" & PageData("PageName") & "</a>" & Breadcrumbs
                End Select

                If ParentPageID > 0 Then Breadcrumbs = SeperatorHTML & Breadcrumbs
            End If
            PageData.Close()
        Loop

        Return Breadcrumbs
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim PageID As Integer = CInt(Session("PageID"))
        If Not IsPostBack Then ltrBreadcrumbs.Text = GetBreadcrumbs(PageID)
    End Sub
End Class
