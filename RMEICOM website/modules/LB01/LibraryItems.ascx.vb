
Partial Class UserControls_LibraryItems
    Inherits System.Web.UI.UserControl

    Dim _Orientation As String

    Property Orientation() As String
        Get
            Return _Orientation
        End Get
        Set(ByVal Value As String)
            _Orientation = Value
        End Set
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim PageID As Integer = Emagine.GetNumber(Session("PageID"))
        Dim LibraryItemData As DataTable = Emagine.GetDataTable("SELECT * FROM qryPageLibraryItems WHERE PageID = " & PageID & " AND IsEnabled = 'True' AND DisplayStartDate <= '" & Date.Now() & "' AND (DisplayEndDate <= '1/2/1900' OR DisplayEndDate >= '" & Date.Now() & "') ORDER BY SortOrder")

        If LibraryItemData.Rows.Count > 0 Then
            If Trim(UCase(Orientation)) = "HORIZONTAL" Then
                HorizontalItems.DataSource = LibraryItemData
                HorizontalItems.DataBind()
                HorizontalItems.Visible = True
            Else
                VerticalItems.DataSource = LibraryItemData
                VerticalItems.DataBind()
                VerticalItems.Visible = True
            End If

        Else
            VerticalItems.Visible = False
            HorizontalItems.Visible = False
        End If

    End Sub
End Class
