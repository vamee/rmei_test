Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Web
Imports System.Xml

Partial Class modules_Rss_RssFeed
    Inherits System.Web.UI.Page

    

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim CategoryID As Integer = Emagine.GetNumber(Resources.GetResourceID())

        Dim Sql As String = "SELECT * FROM ModuleCategories WHERE PublishToRss = 1 AND CategoryID = " & CategoryID
        Dim DataTable As DataTable = Emagine.GetDataTable(Sql)

        If DataTable.Rows.Count > 0 Then
            Dim CategoryName As String = DataTable.Rows(0).Item("CategoryName")
            Dim RssTitle As String = DataTable.Rows(0).Item("RssTitle")
            Dim RssDescription As String = DataTable.Rows(0).Item("RssDescription")
            Dim RssManagingEditor As String = DataTable.Rows(0).Item("RssManagingEditor")
            Dim LogoUrl As String = DataTable.Rows(0).Item("RssImageUrl")
            Dim LogoWidth As Integer = 0
            Dim LogoHeight As Integer = 0
            If LogoUrl.Length > 0 Then
                Dim LogoPath As String = Server.MapPath(LogoUrl)
                If IO.File.Exists(LogoPath) Then
                    Dim Logo As System.Drawing.Image = System.Drawing.Image.FromFile(LogoPath)
                    LogoWidth = Logo.Width
                    LogoHeight = Logo.Height
                    LogoUrl = "http://" & Request.ServerVariables("SERVER_NAME") & LogoUrl.Remove(0, 1)
                Else
                    LogoUrl = ""
                End If
            End If

            Dim PageKey As String = ""
            Dim TableName As String = ""
            Dim LastPubDate As String = ""

            Dim PageModuleInfo As DataTable = Emagine.GetDataTable("SELECT PageKey, TableName FROM qryDisplayPageModules WHERE ModuleKey='PR01' AND ForeignKey='CategoryId' AND ForeignValue='" & CategoryID & "'")
            If PageModuleInfo.Rows.Count > 0 Then
                PageKey = PageModuleInfo.Rows(0).Item("PageKey")
                TableName = PageModuleInfo.Rows(0).Item("TableName")
            End If

            If TableName.Length > 0 Then
                Dim CompanyName As String = GlobalVariables.GetValue("CompanyName")
                Dim Copyright As String = "© " & Date.Now.Year & " " & CompanyName & " All Rights Reserved"
                LastPubDate = Emagine.GetDbValue("SELECT MAX(DisplayDate) AS MaxDisplayDate FROM " & TableName & " WHERE CategoryID = " & CategoryID)

                Response.Clear()
                Response.ContentType = "text/xml"

                Dim objX As New XmlTextWriter(Response.OutputStream, Encoding.UTF8)
                'objX.WriteStartDocument()
                objX.WriteStartElement("rss")
                objX.WriteAttributeString("version", "2.0")
                objX.WriteStartElement("channel")

                objX.WriteElementString("title", RssTitle)
                objX.WriteElementString("link", "http://" & Request.ServerVariables("SERVER_NAME") & "/" & PageKey & ".htm")
                objX.WriteElementString("description", RssDescription)
                objX.WriteElementString("copyright", Copyright)
                objX.WriteElementString("lastPubDate", LastPubDate)
                objX.WriteElementString("ttl", "5")
                objX.WriteElementString("managingEditor", RssManagingEditor)
                objX.WriteElementString("category", CategoryName)
                objX.WriteStartElement("image")
                objX.WriteElementString("title", CompanyName)
                objX.WriteElementString("width", "144")
                objX.WriteElementString("height", "48")
                objX.WriteElementString("link", "http://" & Request.ServerVariables("SERVER_NAME"))
                objX.WriteElementString("url", LogoUrl)
                objX.WriteEndElement()

                'Dim Rs As SqlDataReader = PR01.GetArticles(CategoryID)
                Dim Rs As DataTable = Emagine.GetDataTable("SELECT * FROM " & TableName & " WHERE CategoryID = " & CategoryID & " AND PublishToRss = 1")

                For i As Integer = 0 To (Rs.Rows.Count - 1)
                    Dim DisplayDate As DateTime = Rs.Rows(i).Item("DisplayDate")
                    objX.WriteStartElement("item")
                    objX.WriteElementString("author", Rs.Rows(i).Item("RssAuthor").ToString)
                    objX.WriteElementString("title", Rs.Rows(i).Item("ResourceName").ToString)
                    objX.WriteElementString("description", Rs.Rows(i).Item("RssDescription").ToString)
                    objX.WriteElementString("link", "http://" & Request.ServerVariables("SERVER_NAME") & "/" & Rs.Rows(i).Item("ResourceID").ToString & "/" & Rs.Rows(i).Item("DeliveryPageKey").ToString & ".htm")
                    objX.WriteElementString("pubDate", DisplayDate.ToString("r"))
                    objX.WriteElementString("guid", Rs.Rows(i).Item("ResourceID").ToString())
                    objX.WriteEndElement()
                Next
                Rs.Dispose()

                objX.WriteEndElement()
                objX.WriteEndElement()
                'objX.WriteEndDocument()
                objX.Flush()
                objX.Close()
                Response.End()
            Else
                Response.Write("Rss Feed Error")
            End If
        Else
            Response.Clear()
            Response.Status = "404 Not Found"
            Response.End()
        End If
    End Sub

End Class
