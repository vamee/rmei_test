Imports System.Data.SqlClient

Partial Class modules_Events01_DisplayCurrentEventDates
    Inherits System.Web.UI.UserControl

    Dim _PageModuleID As Integer = 0
    Private _ResourceID As String = ""
    

    Public Property PageModuleID() As Integer
        Get
            Return _PageModuleID
        End Get
        Set(ByVal value As Integer)
            _PageModuleID = value
        End Set
    End Property

#Region "OnLoad"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        _ResourceID = Resources.GetResourceID()

        
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
#End Region

#Region "Events"
    Protected Sub gdvList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvList.Load
        If Not Page.IsPostBack Then
            Dim MyCommand As New SqlCommand
            MyCommand.Parameters.AddWithValue("@PageModuleID", PageModuleID)
            MyCommand.Parameters.AddWithValue("@EventDate", Date.Now)

            Dim Sql As String = "SELECT * FROM qryEventDates WHERE EventDate >= @EventDate ORDER BY EventDate"

            gdvList.DataSource = Emagine.GetDataReader(Sql, MyCommand)
            gdvList.DataBind()
        End If
    End Sub

    Protected Sub gdvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim EventDateID As Integer = e.Row.DataItem("EventDateID")
            Dim ResourceID As String = e.Row.DataItem("ResourceID").ToString
            Dim DetailPageKey As String = e.Row.DataItem("DetailPageKey").ToString
            Dim FormPageKey As String = e.Row.DataItem("FormPageKey").ToString

            Dim hypDetail As HyperLink = e.Row.FindControl("hypDetail")
            Dim hypRegister As HyperLink = e.Row.FindControl("hypRegister")

            hypDetail.NavigateUrl = "/" & ResourceID & "/" & DetailPageKey & ".htm"
            hypRegister.NavigateUrl = "/" & ResourceID & "/" & FormPageKey & ".htm"
        End If
    End Sub
#End Region


End Class
