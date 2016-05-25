Imports System.Data.SqlClient

Partial Class modules_Navigation_LeftNavigation
    Inherits System.Web.UI.UserControl

    Dim PageID As Integer = 0
    Dim ResourceID As String = Resources.GetResourceID()
    Dim PortfolioDeliveryPageID As Integer = 0
    Dim _MenuLevelCount As Integer = 3

    Public Property MenuLevelCount() As Integer
        Get
            Return _MenuLevelCount
        End Get
        Set(ByVal value As Integer)
            _MenuLevelCount = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PageID = CInt(Session("PageID"))

        If ResourceID.Length > 0 Then PortfolioDeliveryPageID = Emagine.GetNumber(Emagine.GetDbValue("SELECT DeliveryPageID FROM qryDeliveryPageModules WHERE ModuleKey = 'Portfolio' AND DeliveryPageID = '" & PageID & "'"))

        If PageID > 0 Then PopulateNavigation(Pages01.GetSectionID(PageID), 1)
    End Sub
    'mod vreeland 4/25/2012
    Function GetFirstChildUrl(ByVal intPageID As Integer) As String
        Dim PageID As Integer = intPageID
        Dim PageTypeID As Integer = 0
        Dim NavigateUrl As String = ""

        Do While PageTypeID = 0
            'mod vreeland 4/25/2012
            'Dim PageData As DataTable = Emagine.GetDataTable("SELECT PageID, PageTypeID, PageKey + '.htm' AS NavigateURL FROM Pages WHERE ParentPageID = " & PageID & " ORDER BY SortOrder")
            Dim PageData As DataTable = Emagine.GetDataTable("SELECT PageID, PageTypeID, PageKey + '.htm' AS NavigateURL FROM Pages WHERE ParentPageID = " & PageID & " AND IsHidden = 0 AND StatusID = 20 ORDER BY SortOrder")
            'end mod vreeland 4/25/2012
            If PageData.Rows.Count > 0 Then
                PageID = PageData.Rows(0).Item("PageID")
                PageTypeID = PageData.Rows(0).Item("PageTypeID")
                NavigateUrl = "~/" & PageData.Rows(0).Item("NavigateUrl")
            Else
                PageTypeID = -1
            End If
            PageData.Dispose()
        Loop

        Return NavigateUrl
    End Function
    'end mod vreeland 4/25/20121
    Function GetChildPages(ByVal intPageID As Integer) As SqlDataReader
        Dim SQLBuilder As New StringBuilder
        SQLBuilder.Append("SELECT  PageId, PageKey, ParentPageId, PageTypeId, MenuName, PageAction, IsSecure ")
        SQLBuilder.Append("FROM Pages ")
        'mod vreeland 4/25/2012
        'SQLBuilder.Append("WHERE ParentPageId = " & intPageID & " AND StatusId = 20 ")
        SQLBuilder.Append("WHERE ParentPageId = " & intPageID & " AND StatusId = 20 AND IsHidden = 0 ")
        'end mod vreeland 4/25/2012
        SQLBuilder.Append("ORDER BY SortOrder")

        Return Emagine.GetDataReader(SQLBuilder.ToString)
    End Function

    Sub PopulateNavigation(ByVal intPageID As Integer, ByVal intMenuLevel As Integer)
        Dim Rs As SqlDataReader = Nothing

        'If Pages01.IsRelated(PageID, 932) Then
        'Rs = Me.GetChildPages(intPageID)
        'Else
        Rs = Pages01.GetChildPages(intPageID)
        'End If


        Do While Rs.Read
            Dim TableRow As New TableRow
            Dim TableCell As New TableCell
            Dim TopTableRow As New TableRow
            Dim TopTableCell As New TableCell
            Dim BottomTableRow As New TableRow
            Dim BottomTableCell As New TableCell

            Dim IsRelated As Boolean = Pages01.IsRelated(PageID, Rs("PageID"))
            Select Case intMenuLevel
                Case 1
                    Dim SpacerRow As New TableRow
                    Dim SpacerCell As New TableCell
                    SpacerCell.CssClass = "leftnav-lvl1-spacer"
                    SpacerRow.Cells.Add(SpacerCell)
                    tblLeftNav.Rows.Add(SpacerRow)

                    If IsRelated Then


                        If PageID = Rs("PageID") Then
                            TableCell.Text = Rs("MenuName").ToString
                            TopTableCell.CssClass = "leftnav-bevel-top-on"
                            TableCell.CssClass = "leftnav-lvl1-on"
                            BottomTableCell.CssClass = "leftnav-bevel-btm-on"

                        Else
                            Dim PageLink As New HyperLink
                            Select Case Rs("PageTypeID")
                                Case 0
                                    TableCell.Text = Rs("MenuName").ToString
                                    'PageLink.NavigateUrl = Pages01.GetPageLink(Rs("PageTypeID"), Rs("PageKey").ToString, Rs("PageAction").ToString, Rs("IsSecure"), True)
                                    'PageLink.NavigateUrl = "/" & Emagine.GetDbValue("SELECT PageKey + '.htm' AS NavigateURL FROM Pages WHERE ParentPageID = " & Rs("PageID") & " ORDER BY SortOrder")
                                Case Else
                                    PageLink.NavigateUrl = Pages01.GetPageLink(Rs("PageTypeID"), Rs("PageKey").ToString, Rs("PageAction").ToString, Rs("IsSecure"))
                                    PageLink.Text = Rs("MenuName").ToString()
                                    TableCell.Controls.Add(PageLink)
                            End Select

                            TopTableCell.CssClass = "leftnav-bevel-top-on"
                            TableCell.CssClass = "leftnav-lvl1-on"
                            BottomTableCell.CssClass = "leftnav-bevel-btm-on"
                        End If
                    Else
                        Dim PageLink As New HyperLink

                        Select Case Rs("PageTypeID")
                            Case 0
                                'PageLink.NavigateUrl = Pages01.GetPageLink(Rs("PageTypeID"), Rs("PageKey").ToString, Rs("PageAction").ToString, Rs("IsSecure"), True)
                                'PageLink.NavigateUrl = "/" & Emagine.GetDbValue("SELECT PageKey + '.htm' AS NavigateURL FROM Pages WHERE ParentPageID = " & Rs("PageID") & " ORDER BY SortOrder")
                                ' PageLink.NavigateUrl = Pages01.GetPageLink(Rs("PageTypeID"), Rs("PageID"), Rs("PageAction").ToString, Rs("IsSecure"), True)
                               PageLink.NavigateUrl = Me.GetFirstChildUrl(CInt(Rs("PageID")))


                            Case Else
                                PageLink.NavigateUrl = Pages01.GetPageLink(Rs("PageTypeID"), Rs("PageKey").ToString, Rs("PageAction").ToString, Rs("IsSecure"))
                        End Select


                        PageLink.Text = Rs("MenuName").ToString()
                        TableCell.Controls.Add(PageLink)

                        TopTableCell.CssClass = "leftnav-bevel-top-off"
                        TableCell.CssClass = "leftnav-lvl1-off"
                        BottomTableCell.CssClass = "leftnav-bevel-btm-off"
                    End If

                    TopTableRow.Cells.Add(TopTableCell)
                    tblLeftNav.Rows.Add(TopTableRow)

                    TableRow.Cells.Add(TableCell)
                    tblLeftNav.Rows.Add(TableRow)

                    BottomTableRow.Cells.Add(BottomTableCell)
                    tblLeftNav.Rows.Add(BottomTableRow)

                Case 2

                    If IsRelated Then
                        If PageID = Rs("PageID") Then
                            TableCell.Text = Rs("MenuName").ToString
                            TableCell.CssClass = "leftnav-lvl2-on"
                        Else
                            Dim PageLink As New HyperLink

                            Select Case Rs("PageTypeID")
                                Case 0
                                    'PageLink.NavigateUrl = Pages01.GetPageLink(Rs("PageTypeID"), Rs("PageKey").ToString, Rs("PageAction").ToString, Rs("IsSecure"), True)
                                Case Else
                                    PageLink.NavigateUrl = Pages01.GetPageLink(Rs("PageTypeID"), Rs("PageKey").ToString, Rs("PageAction").ToString, Rs("IsSecure"))
                            End Select

                            PageLink.Text = Rs("MenuName").ToString()
                            TableCell.Controls.Add(PageLink)
                            TableCell.CssClass = "leftnav-lvl2-off"
                        End If
                    Else
                        Dim PageLink As New HyperLink

                        Select Case Rs("PageTypeID")
                            Case 0
                                'PageLink.NavigateUrl = Pages01.GetPageLink(Rs("PageTypeID"), Rs("PageKey").ToString, Rs("PageAction").ToString, Rs("IsSecure"), True)
                            Case Else
                                PageLink.NavigateUrl = Pages01.GetPageLink(Rs("PageTypeID"), Rs("PageKey").ToString, Rs("PageAction").ToString, Rs("IsSecure"))
                        End Select

                        PageLink.Text = Rs("MenuName").ToString()
                        TableCell.Controls.Add(PageLink)
                        TableCell.CssClass = "leftnav-lvl2-off"
                    End If
                    TableRow.Cells.Add(TableCell)
                    tblLeftNav.Rows.Add(TableRow)

                Case 3
                    If IsRelated Then
                        If PageID = Rs("PageID") Then
                            TableCell.Text = Rs("MenuName").ToString
                            TableCell.CssClass = "leftnav-lvl3-on"
                        Else
                            Dim PageLink As New HyperLink

                            Select Case Rs("PageTypeID")
                                Case 0
                                    'PageLink.NavigateUrl = Pages01.GetPageLink(Rs("PageTypeID"), Rs("PageKey").ToString, Rs("PageAction").ToString, Rs("IsSecure"), True)
                                Case Else
                                    PageLink.NavigateUrl = Pages01.GetPageLink(Rs("PageTypeID"), Rs("PageKey").ToString, Rs("PageAction").ToString, Rs("IsSecure"))
                            End Select

                            PageLink.Text = Rs("MenuName").ToString()
                            TableCell.Controls.Add(PageLink)
                            TableCell.CssClass = "leftnav-lvl3-off"
                        End If
                    Else
                        Dim PageLink As New HyperLink
                        PageLink.NavigateUrl = Pages01.GetPageLink(Rs("PageTypeID"), Rs("PageKey").ToString, Rs("PageAction").ToString, Rs("IsSecure"))
                        PageLink.Text = Rs("MenuName").ToString()
                        TableCell.Controls.Add(PageLink)
                        TableCell.CssClass = "leftnav-lvl3-off"
                    End If
                    TableRow.Cells.Add(TableCell)
                    tblLeftNav.Rows.Add(TableRow)

                Case 4
                    If IsRelated Then
                        If PageID = Rs("PageID") Then
                            TableCell.Text = Rs("MenuName").ToString
                            TableCell.CssClass = "leftnav-lvl4-on"
                        Else
                            Dim PageLink As New HyperLink

                            Select Case Rs("PageTypeID")
                                Case 0
                                    PageLink.NavigateUrl = Pages01.GetPageLink(Rs("PageTypeID"), Rs("PageID"), Rs("PageAction").ToString, Rs("IsSecure"), True)
                                Case Else
                                    PageLink.NavigateUrl = Pages01.GetPageLink(Rs("PageTypeID"), Rs("PageKey").ToString, Rs("PageAction").ToString, Rs("IsSecure"))
                            End Select

                            PageLink.Text = Rs("MenuName").ToString()
                            TableCell.Controls.Add(PageLink)
                            TableCell.CssClass = "leftnav-lvl4-off"
                        End If
                    Else
                        Dim PageLink As New HyperLink

                        Select Case Rs("PageTypeID")
                            Case 0
                                PageLink.NavigateUrl = Pages01.GetPageLink(Rs("PageTypeID"), Rs("PageID"), Rs("PageAction").ToString, Rs("IsSecure"), True)
                            Case Else
                                PageLink.NavigateUrl = Pages01.GetPageLink(Rs("PageTypeID"), Rs("PageKey").ToString, Rs("PageAction").ToString, Rs("IsSecure"))
                        End Select

                        PageLink.Text = Rs("MenuName").ToString()
                        TableCell.Controls.Add(PageLink)
                        TableCell.CssClass = "leftnav-lvl4-off"
                    End If
                    TableRow.Cells.Add(TableCell)
                    tblLeftNav.Rows.Add(TableRow)

            End Select

            'If PortfolioDeliveryPageID > 0 And IsRelated Then Me.DisplayPortfolioItems(Rs("PageID"), intMenuLevel + 1)
            If (IsRelated And intMenuLevel <= _MenuLevelCount) Then PopulateNavigation(Rs("PageID"), intMenuLevel + 1)
        Loop

        Rs.Close()
    End Sub


    Sub DisplayPortfolioItems(ByVal intPageID As Integer, ByVal intMenuLevel As Integer)
        Dim PortfolioData As DataTable = Emagine.GetDataTable("SELECT * FROM qryPortfolioItems WHERE IsEnabled = 'True' AND DeliveryPageID = '" & PortfolioDeliveryPageID & "' ORDER BY SortOrder")
        Dim ItemID As Integer = Emagine.GetNumber(Emagine.GetDbValue("SELECT ItemID FROM PortfolioItems WHERE ResourceID = '" & ResourceID & "'"))

        If PortfolioData.Rows.Count > 0 Then
            For i As Integer = 0 To (PortfolioData.Rows.Count - 1)
                Dim TableRow As New TableRow
                Dim TableCell As New TableCell
                Dim TopTableRow As New TableRow
                Dim TopTableCell As New TableCell
                Dim BottomTableRow As New TableRow
                Dim BottomTableCell As New TableCell
                Dim DisplayProducts As Boolean = False

                If PortfolioData.Rows(i).Item("ItemID") = ItemID Then
                    TableCell.Text = PortfolioData.Rows(i).Item("ResourceName").ToString
                    TableCell.CssClass = "leftnav-lvl" & intMenuLevel & "-on"
                Else
                    Dim PageLink As New HyperLink

                    PageLink.NavigateUrl = "/" & PortfolioData.Rows(i).Item("ResourceID") & "/" & PortfolioData.Rows(i).Item("DeliveryPageKey") & ".htm"

                    PageLink.Text = PortfolioData.Rows(i).Item("ResourceName").ToString.ToString()
                    TableCell.Controls.Add(PageLink)
                    TableCell.CssClass = "leftnav-lvl" & intMenuLevel & "-off"
                End If
                
                TableRow.Cells.Add(TableCell)
                tblLeftNav.Rows.Add(TableRow)
            Next
        End If
    End Sub
End Class


