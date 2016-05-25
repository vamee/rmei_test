Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Web
Imports System.Xml

Partial Class modules_Rss_EventRssFeed
    Inherits System.Web.UI.Page

    'Edit the following 3 lines to customize
    Dim CompanyName As String = "aPriori"
    Dim LogoUrl As String = "http://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & "/Collateral/templates/English-US/images/top_logo.gif"
    Dim Copyright As String = "(c) " & Date.Now.Year & ", aPriori Technologies. All rights reserved."

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim CategoryID As Integer = CInt(Request("CategoryID"))

        Dim Sql As String = "SELECT ModuleCategories.*, MediaContacts.ContactName, MediaContacts.Email AS ContactEmail FROM ModuleCategories INNER JOIN MediaContacts ON ModuleCategories.MediaContactID = MediaContacts.ContactID WHERE ModuleCategories.CategoryID = " & CategoryID
        Dim DataTable As DataTable = Emagine.GetDataTable(Sql)

        If DataTable.Rows.Count > 0 Then
            Dim CategoryName As String = DataTable.Rows(0).Item("CategoryName")
            Dim CategoryDescription As String = DataTable.Rows(0).Item("Description")
            Dim ContactName As String = DataTable.Rows(0).Item("ContactName")
            Dim ContactEmail As String = DataTable.Rows(0).Item("ContactEmail")

            Dim PageKey As String = Emagine.GetDbValue("SELECT PageKey FROM qryDisplayPageModules WHERE ModuleKey='PR01' AND ForeignKey='CategoryId' AND ForeignValue='" & CategoryID & "'")
            Dim LastPubDate As String = Emagine.GetDbValue("SELECT MAX(DisplayDate) AS MaxDisplayDate FROM qryArticles WHERE CategoryID = " & CategoryID)

            Response.Clear()
            Response.ContentType = "text/xml"

            Dim objX As New XmlTextWriter(Response.OutputStream, Encoding.UTF8)
            'objX.WriteStartDocument()
            objX.WriteStartElement("rss")
            objX.WriteAttributeString("version", "2.0")
            objX.WriteStartElement("channel")

            objX.WriteElementString("title", CompanyName & ": " & CategoryName)
            objX.WriteElementString("link", "http://" & Request.ServerVariables("SERVER_NAME") & "/" & PageKey & ".htm")
            objX.WriteElementString("description", CategoryDescription)
            objX.WriteElementString("copyright", Copyright)
            objX.WriteElementString("lastPubDate", LastPubDate)
            objX.WriteElementString("ttl", "5")
            objX.WriteElementString("managingEditor", ContactEmail)
            objX.WriteElementString("category", CategoryName)
            objX.WriteStartElement("image")
            objX.WriteElementString("title", CompanyName)
            objX.WriteElementString("width", "371")
            objX.WriteElementString("height", "43")
            objX.WriteElementString("link", "http://" & Request.ServerVariables("SERVER_NAME"))
            objX.WriteElementString("url", LogoUrl)
            objX.WriteEndElement()

            Dim Rs As SqlDataReader = Emagine.GetDataReader("SELECT Events.*, Resources.ResourceName As EventName, Resources.LastUpdated FROM Events INNER JOIN Resources ON Events.ResourceID = Resources.ResourceID ORDER BY SortOrder")
            While Rs.Read()
                Dim DisplayDate As DateTime = Rs("LastUpdated")
                objX.WriteStartElement("item")
                objX.WriteElementString("author", ContactName)
                objX.WriteElementString("title", Rs("EventName").ToString)
                objX.WriteElementString("description", Rs("EventSummary").ToString)
                objX.WriteElementString("link", Rs("ArchiveUrl"))
                objX.WriteElementString("pubDate", DisplayDate.ToString("r"))
                objX.WriteElementString("guid", Rs("ResourceID").ToString())
                objX.WriteEndElement()
            End While
            Rs.Close()

            objX.WriteEndElement()
            objX.WriteEndElement()
            'objX.WriteEndDocument()
            objX.Flush()
            objX.Close()
            Response.End()
        Else
            Response.Write("Rss Feed Error")
        End If
    End Sub
End Class
