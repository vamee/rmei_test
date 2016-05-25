
Partial Class PR01_DisplayNews_SuccessStories
    Inherits System.Web.UI.UserControl

    Dim _PageModuleID As Integer = 0

    Public Property PageModuleID() As Integer
        Get
            Return _PageModuleID
        End Get
        Set(ByVal value As Integer)
            _PageModuleID = value
        End Set
    End Property

    Sub DisplayArticleList(ByVal intCategoryId As Integer, ByVal strDisplayPageKey As String)
        Dim dtrPressReleases As Data.SqlClient.SqlDataReader = PR01.GetArticles(intCategoryId, 1, "DisplayDate", "DESC")
        If dtrPressReleases.HasRows Then
            rptPR01.DataSource = dtrPressReleases
            rptPR01.DataBind()
            rptPR01.EnableViewState = False
        Else
            rptPR01.Visible = False
        End If
        dtrPressReleases.Close()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim CategoryID As Integer = Emagine.GetDbValue("SELECT ForeignValue FROM PageModules WHERE PageModuleID = " & PageModuleID)
        Dim ResourceID As String = Resources.GetResourceID()
        Dim ArticleID As Integer = 0
        Dim DisplayPageKey As String = ""

        If Len(ResourceID) > 0 Then
            Dim SQL As String = "SELECT * FROM qryArticles WHERE ResourceID = '" & ResourceID & "'"
            Dim Rs As Data.SqlClient.SqlDataReader = Emagine.GetDataReader(SQL)
            If Rs.Read Then
                ArticleID = Rs("ArticleID")
                DisplayPageKey = Rs("DisplayPageKey")
            End If
            Rs.Close()
            Rs = Nothing
        End If

        If ArticleID > 0 Then
            DisplayArticle(ArticleID)
        Else
            DisplayArticleList(CategoryID, DisplayPageKey)
        End If
    End Sub

    Sub DisplayArticle(ByVal intArticleID As Integer)

        Dim FormRedirect As String = ""
        Dim ResourcePageKey As String = ""
        Dim ResourceID As String = ""
        Dim UserID As String = Emagine.Users.User.GetUserID()
        Dim SQL As String = "SELECT FormPageID, FormPageKey, ResourceID FROM qryArticles WHERE ArticleID = " & intArticleID
        Dim Rs As Data.SqlClient.SqlDataReader = Emagine.GetDataReader(SQL)

        If Rs.Read Then
            Resources.UpdateClickCount(Rs("ResourceID"))
            ResourceID = Rs("ResourceID")
            'ResourcePageKey = Rs("ResourcePageKey")
            If Emagine.GetNumber(Rs("FormPageID").ToString) > 0 Then
                FormRedirect = "/" & Rs("ResourceID") & "/" & Rs("FormPageKey") & ".htm"
            End If
        End If
        Rs.Close()
        Rs = Nothing

        If Len(FormRedirect) = 0 Or Resources.UserHasRegistered(UserID, ResourceID) Then
            SQL = "SELECT ResourceName, ArticleText FROM qryArticles WHERE ArticleID = " & intArticleID
            Rs = Emagine.GetDataReader(SQL)
            If Rs.Read Then
                lblArticleTitle.Text = Rs("ResourceName")
                lblArticleText.Text = Rs("ArticleText")
                ArticleDetail.Visible = True
            End If
            Rs.Close()
            Rs = Nothing
        Else
            Response.Redirect(FormRedirect)
        End If
    End Sub

    Protected Sub rptPR01_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptPR01.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim ModuleType As String = DataBinder.Eval(e.Item.DataItem, "ModuleType").ToString()
            Dim ArticleURL As String = DataBinder.Eval(e.Item.DataItem, "ArticleURL").ToString()
            Dim ImageURL As String = DataBinder.Eval(e.Item.DataItem, "ImageURL").ToString()
            Dim Filename As String = DataBinder.Eval(e.Item.DataItem, "Filename").ToString()
            Dim ResourceID As String = DataBinder.Eval(e.Item.DataItem, "ResourceID").ToString()
            Dim ResourceName As String = DataBinder.Eval(e.Item.DataItem, "ResourceName").ToString()
            Dim DeliveryPageKey As String = DataBinder.Eval(e.Item.DataItem, "DeliveryPageKey").ToString()

            Dim imgImageUrl As Image = e.Item.FindControl("imgImageUrl")
            Dim hypArticle As HyperLink = CType(e.Item.FindControl("hypArticle"), HyperLink)

            If ImageURL.Length > 0 Then
                imgImageUrl.ImageUrl = ImageURL
                imgImageUrl.Visible = True
            End If

            hypArticle.Text = ResourceName

            If ModuleType = "External Link" Then
                hypArticle.NavigateUrl = ArticleURL
                hypArticle.Target = "_blank"
            ElseIf ModuleType = "File Download" Then
                hypArticle.NavigateUrl = Filename
                hypArticle.Target = "_blank"
            Else
                hypArticle.NavigateUrl = "/" & ResourceID & "/" & DeliveryPageKey & ".htm"
            End If


        End If
    End Sub
End Class
