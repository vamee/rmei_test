
Partial Class modules_Programs_DisplayProgramsByType
    Inherits System.Web.UI.UserControl

    Dim _CategoryID As Integer = 0
    Dim _DiseaseID As String = ""
    Dim _PageModuleID As Integer = 0
    Dim _IsArchived As Boolean = False

    Public Property PageModuleID() As Integer
        Get
            Return _PageModuleID
        End Get
        Set(ByVal value As Integer)
            _PageModuleID = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _CategoryID = Emagine.GetNumber(Emagine.GetDbValue("SELECT ForeignValue FROM PageModules WHERE PageModuleID = " & PageModuleID))
        Dim ProgramTypeID As String = PageModuleProperty.GetProperty(_PageModuleID, "ProgramTypeID")
        _DiseaseID = PageModuleProperty.GetProperty(_PageModuleID, "DiseaseID")
        _IsArchived = CBool(Emagine.GetNumber(PageModuleProperty.GetProperty(_PageModuleID, "IsArchived")))

        If Right(ProgramTypeID, 2) = "||" Then ProgramTypeID = ProgramTypeID.Substring(0, ProgramTypeID.Length - 2)
        If Right(_DiseaseID, 2) = "||" Then _DiseaseID = _DiseaseID.Substring(0, _DiseaseID.Length - 2)

        Dim MyCommand As New SqlCommand
        MyCommand.Parameters.AddWithValue("@ModuleCategoryID", _CategoryID)
        MyCommand.Parameters.AddWithValue("@IsArchived", _IsArchived)
        MyCommand.Parameters.AddWithValue("@IsEnabled", True)

        Dim SqlBuilder As New StringBuilder
        SqlBuilder.Append("SELECT * FROM qryCustom_ProgramTypes WHERE ProgramTypeID IN (")
        SqlBuilder.Append("SELECT ProgramTypeID FROM qryCustom_Programs WHERE ")
        SqlBuilder.Append("ModuleCategoryID = @ModuleCategoryID ")
        SqlBuilder.Append("AND ProgramTypeID IN ('" & ProgramTypeID.Replace("||", "','") & "') ")
        SqlBuilder.Append("AND DiseaseID IN ('" & _DiseaseID.Replace("||", "','") & "') ")
        SqlBuilder.Append("AND IsArchived = @IsArchived ")
        SqlBuilder.Append("AND IsEnabled = @IsEnabled) ")
        SqlBuilder.Append("ORDER BY SortOrder")

        rptProgramTypes.DataSource = Emagine.GetDataReader(SqlBuilder.ToString, MyCommand)
        rptProgramTypes.DataBind()
    End Sub

    Protected Sub rptProgramTypes_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptProgramTypes.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim ProgramTypeID As String = e.Item.DataItem("ProgramTypeID").ToString

            Dim rptPrograms As Repeater = e.Item.FindControl("rptPrograms")

            Dim MyCommand As New SqlCommand
            MyCommand.Parameters.AddWithValue("@ModuleCategoryID", _CategoryID)
            MyCommand.Parameters.AddWithValue("@ProgramTypeID", ProgramTypeID)
            MyCommand.Parameters.AddWithValue("@IsArchived", _IsArchived)
            MyCommand.Parameters.AddWithValue("@IsEnabled", True)

            Dim SqlBuilder As New StringBuilder
            SqlBuilder.Append("SELECT * FROM qryCustom_Programs WHERE ")
            SqlBuilder.Append("ModuleCategoryID = @ModuleCategoryID ")
            SqlBuilder.Append("AND ProgramTypeID =@ProgramTypeID ")
            SqlBuilder.Append("AND DiseaseID IN ('" & _DiseaseID.Replace("||", "','") & "') ")
            SqlBuilder.Append("AND IsArchived = @IsArchived ")
            SqlBuilder.Append("AND IsEnabled = @IsEnabled ")
            SqlBuilder.Append("ORDER BY ProgramDate DESC, ResourceName")

            rptPrograms.DataSource = Emagine.GetDataReader(SqlBuilder.ToString, MyCommand)
            rptPrograms.DataBind()
        End If
    End Sub

    Protected Sub rptPrograms_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim ResourceName As String = e.Item.DataItem("ResourceName").ToString
            Dim ProgramUrl As String = e.Item.DataItem("ProgramUrl").ToString

            Dim lblProgramUrl As Label = e.Item.FindControl("lblProgramUrl")

            If ProgramUrl.Length > 0 Then
                lblProgramUrl.Text = "<a href='" & ProgramUrl & "' target='_blank'>" & ResourceName & "</a>"
            Else
                lblProgramUrl.Text = ResourceName
            End If

        End If
    End Sub
End Class
