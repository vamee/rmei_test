Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Web
Imports System.Xml

Partial Class modules_Rss_News
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim CategoryID As Integer = 45
        Dim LastPubDate As DateTime = CDate(Emagine.GetDbValue("SELECT MAX(DisplayDate) AS MaxDisplayDate FROM qryArticles WHERE CategoryID = " & CategoryID))

        Response.Clear()
        Response.ContentType = "text/xml"

        Dim objX As New XmlTextWriter(Response.OutputStream, Encoding.UTF8)
        'objX.WriteStartDocument()
        objX.WriteStartElement("rss")
        objX.WriteAttributeString("version", "2.0")
        objX.WriteStartElement("channel")

        objX.WriteElementString("title", "Active Endpoints: Open Source News")
        objX.WriteElementString("link", "http://" & Request.ServerVariables("SERVER_NAME") & "/news-rss.htm")
        objX.WriteElementString("description", "The latest open source news from the world of Active Endpoints")
        objX.WriteElementString("copyright", "(c) 2006, Active Endpoints, LLC. All rights reserved.")
        objX.WriteElementString("lastPubDate", LastPubDate)
        objX.WriteElementString("ttl", "5")
        objX.WriteElementString("managingEditor", "press@active-endpoints.com")
        objX.WriteElementString("category", "News")
        objX.WriteStartElement("image")
        objX.WriteElementString("title", "Active Endpoints Open Source News")
        objX.WriteElementString("width", "144")
        objX.WriteElementString("height", "48")
        objX.WriteElementString("link", "http://" & Request.ServerVariables("SERVER_NAME"))
        objX.WriteElementString("url", "http://" & Request.ServerVariables("SERVER_NAME") & "/Collateral/Templates/Common/images/top_logo.gif")
        objX.WriteEndElement()

        Dim Rs As SqlDataReader = PR01.GetArticles(CategoryID)
        While Rs.Read()
            Dim DisplayDate As DateTime = Rs("DisplayDate")
            objX.WriteStartElement("item")
            objX.WriteElementString("author", "Sonal Rajan")
            objX.WriteElementString("title", Rs("ResourceName").ToString)
            objX.WriteElementString("description", Rs("ArticleSummary").ToString)
            objX.WriteElementString("link", "http://" & Request.ServerVariables("SERVER_NAME") & "/" & Rs("ResourceID").ToString & "/" & Rs("DeliveryPageKey").ToString & ".htm")
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
    End Sub
End Class
