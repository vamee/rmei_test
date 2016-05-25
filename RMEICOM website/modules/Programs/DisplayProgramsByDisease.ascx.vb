
Partial Class modules_Programs_DisplayProgramsByDisease
    Inherits System.Web.UI.UserControl

    Dim _CategoryID As Integer = 0
    Dim _ProgramTypeID As String = ""
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
        Dim DiseaseID As String = PageModuleProperty.GetProperty(_PageModuleID, "DiseaseID")
        _ProgramTypeID = PageModuleProperty.GetProperty(_PageModuleID, "ProgramTypeID")
        _IsArchived = CBool(Emagine.GetNumber(PageModuleProperty.GetProperty(_PageModuleID, "IsArchived")))

        If Right(_ProgramTypeID, 2) = "||" Then _ProgramTypeID = _ProgramTypeID.Substring(0, _ProgramTypeID.Length - 2)
        If Right(DiseaseID, 2) = "||" Then DiseaseID = DiseaseID.Substring(0, DiseaseID.Length - 2)

        Dim MyCommand As New SqlCommand
        MyCommand.Parameters.AddWithValue("@ModuleCategoryID", _CategoryID)
        MyCommand.Parameters.AddWithValue("@IsArchived", _IsArchived)
        MyCommand.Parameters.AddWithValue("@IsEnabled", True)

        Dim SqlBuilder As New StringBuilder
        SqlBuilder.Append("SELECT * FROM qryCustom_Diseases WHERE DiseaseID IN (")
        SqlBuilder.Append("SELECT DiseaseID FROM qryCustom_Programs WHERE ")
        SqlBuilder.Append("ModuleCategoryID = @ModuleCategoryID ")
        SqlBuilder.Append("AND ProgramTypeID IN ('" & _ProgramTypeID.Replace("||", "','") & "') ")
        SqlBuilder.Append("AND DiseaseID IN ('" & DiseaseID.Replace("||", "','") & "') ")
        SqlBuilder.Append("AND IsArchived = @IsArchived ")
        SqlBuilder.Append("AND IsEnabled = @IsEnabled) ")
        SqlBuilder.Append("ORDER BY ResourceName")

        rptDiseases.DataSource = Emagine.GetDataReader(SqlBuilder.ToString, MyCommand)
        rptDiseases.DataBind()
    End Sub

    Protected Sub rptProgramTypes_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptDiseases.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim DiseaseID As String = e.Item.DataItem("DiseaseID").ToString

            Dim rptPrograms As Repeater = e.Item.FindControl("rptPrograms")

            Dim MyCommand As New SqlCommand
            MyCommand.Parameters.AddWithValue("@ModuleCategoryID", _CategoryID)
            MyCommand.Parameters.AddWithValue("@ProgramTypeID", _ProgramTypeID)
            MyCommand.Parameters.AddWithValue("@IsArchived", _IsArchived)
            MyCommand.Parameters.AddWithValue("@IsEnabled", True)

            Dim SqlBuilder As New StringBuilder
            SqlBuilder.Append("SELECT * FROM qryCustom_Programs WHERE ")
            SqlBuilder.Append("ModuleCategoryID = @ModuleCategoryID ")
            SqlBuilder.Append("AND ProgramTypeID =@ProgramTypeID ")
            SqlBuilder.Append("AND DiseaseID IN ('" & DiseaseID.Replace("||", "','") & "') ")
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
