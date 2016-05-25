
Imports System.Data.SqlClient

Partial Class modules_Events01_DisplayEvents
    Inherits System.Web.UI.UserControl

    Dim _PageModuleID As Integer = 0
    Private _ResourceID As String = ""
    Private _CategoryID As Integer = 0
    Private _DisplayPageID As Integer = 0
    Private _DisplayPageKey As String = ""
    Private _DetailPageID As Integer = 0
    Private _DetailPageKey As String = ""
    Private _RegistrationPageID As Integer = 0
    Private _RegistrationPageKey As String = ""
    Private _DeliveryPageID As Integer = 0
    Private _DeliveryPageKey As String = ""

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

        Dim MyCommand As New SqlCommand
        MyCommand.Parameters.AddWithValue("@PageModuleID", PageModuleID)
        MyCommand.Parameters.AddWithValue("@ResourceID", _ResourceID)

        _DisplayPageID = Emagine.GetNumber(Emagine.GetDbValue("SELECT PageID FROM PageModules WHERE PageModuleID = @PageModuleID", MyCommand))
        _DisplayPageKey = Pages01.GetPageInfo(_DisplayPageID).PageKey
        _CategoryID = Emagine.GetNumber(Emagine.GetDbValue("SELECT ForeignValue FROM PageModules WHERE PageModuleID = @PageModuleID", MyCommand))
        _DetailPageID = PageModuleProperty.GetProperty(PageModuleID, "DetailPage")
        _RegistrationPageID = PageModuleProperty.GetProperty(PageModuleID, "FormPage")
        _DeliveryPageID = PageModuleProperty.GetProperty(PageModuleID, "DeliveryPage")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Select Case Session("PageID")
            Case _DisplayPageID
                _DetailPageKey = Pages01.GetPageInfo(_DetailPageID).PageKey
                Me.ShowDisplayPanel(_CategoryID)

            Case _DetailPageID
                _RegistrationPageKey = Pages01.GetPageInfo(_RegistrationPageID).PageKey

                Dim MyCommand As New SqlCommand
                MyCommand.Parameters.AddWithValue("@ResourceID", _ResourceID)

                Dim EventID As Integer = Emagine.GetNumber(Emagine.GetDbValue("SELECT EventID FROM Events WHERE ResourceID = @ResourceID", MyCommand))

                If EventID > 0 Then
                    Me.ShowDetailPanel(EventID)
                Else
                    Response.Redirect("/" & _DisplayPageKey & ".htm")
                End If

            Case _RegistrationPageID
                _DeliveryPageKey = Pages01.GetPageInfo(_DeliveryPageID).PageKey

            Case _DeliveryPageID


            Case Else
                Response.Redirect("/" & _DisplayPageKey & ".htm")

        End Select
    End Sub
#End Region

#Region "Events"

    Protected Sub gdvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ResourceID As String = e.Row.DataItem("ResourceID").ToString
            Dim ResourceName As String = e.Row.DataItem("ResourceName").ToString

            Dim hypResourceName As HyperLink = e.Row.FindControl("hypResourceName")

            hypResourceName.NavigateUrl = "/" & ResourceID & "/" & _DetailPageKey & ".htm"
        End If
    End Sub

    Protected Sub btnRegister_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRegister.Click
        Response.Redirect("/" & _ResourceID & "/" & _RegistrationPageKey & ".htm")
    End Sub

#End Region

#Region "Custom Procedures"

    Sub ShowDisplayPanel(ByVal intCategoryID As Integer)
        lblCategoryName.Text = ModuleCategory.GetModuleCategory(intCategoryID).CategoryName

        Dim MyCommand As New SqlCommand
        MyCommand.Parameters.AddWithValue("@PageModuleID", PageModuleID)

        Dim Sql As String = "SELECT * FROM qryEvents WHERE PageModuleID = @PageModuleID ORDER BY SortOrder"

        gdvList.DataSource = Emagine.GetDataReader(Sql, MyCommand)
        gdvList.DataBind()

        pnlDisplay.Visible = True
    End Sub

    Sub ShowDetailPanel(ByVal intEventID As Integer)
        Dim MyEvent As Events01 = Events01.GetEvent(intEventID)
        ltrEventDescription.Text = MyEvent.EventDescription

        Dim MyCommand As New SqlCommand
        MyCommand.Parameters.AddWithValue("@EventID", intEventID)
        MyCommand.Parameters.AddWithValue("@EventDate", Date.Now)

        Dim Sql As String = "SELECT * FROM EventDates WHERE EventID = @EventID AND EventDate >= @EventDate"

        Dim MyEventDateData As DataTable = Emagine.GetDataTable(Sql, MyCommand)

        If MyEventDateData.Rows.Count > 0 Then btnRegister.Visible = True

        pnlDetail.Visible = True
    End Sub

    Sub ShowRegistrationPanel()

    End Sub

    Sub ShowDeliveryPanel()

    End Sub
#End Region

End Class
