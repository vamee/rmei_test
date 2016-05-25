
Partial Class modules_Programs_DisplayPrograms
    Inherits System.Web.UI.UserControl

    Dim CategoryName As String = ""
    Dim _PageModuleID As Integer = 0

    Public Property PageModuleID() As Integer
        Get
            Return _PageModuleID
        End Get
        Set(ByVal value As Integer)
            _PageModuleID = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim CategoryID As Integer = Emagine.GetNumber(Emagine.GetDbValue("SELECT ForeignValue FROM PageModules WHERE PageModuleID = " & PageModuleID))
        Dim ProgramTypeID As String = PageModuleProperty.GetProperty(_PageModuleID, "ProgramTypeID")
        Dim DiseaseID As String = PageModuleProperty.GetProperty(_PageModuleID, "DiseaseID")
        Dim IsArchived As Boolean = CBool(Emagine.GetNumber(PageModuleProperty.GetProperty(_PageModuleID, "IsArchived")))

        If Right(ProgramTypeID, 2) = "||" Then ProgramTypeID = ProgramTypeID.Substring(0, ProgramTypeID.Length - 2)
        If Right(DiseaseID, 2) = "||" Then DiseaseID = DiseaseID.Substring(0, DiseaseID.Length - 2)

        Dim MyCommand As New SqlCommand
        MyCommand.Parameters.AddWithValue("@ModuleCategoryID", CategoryID)
        'MyCommand.Parameters.AddWithValue("@ProgramTypeID", "'" & ProgramTypeID.Replace("||", "','") & "'")
        'MyCommand.Parameters.AddWithValue("@DiseaseID", "'" & DiseaseID.Replace("||", "','") & "'")
        MyCommand.Parameters.AddWithValue("@IsArchived", IsArchived)
        MyCommand.Parameters.AddWithValue("@IsEnabled", True)

        Dim SqlBuilder As New StringBuilder
        SqlBuilder.Append("SELECT * FROM qryCustom_Programs WHERE ")
        SqlBuilder.Append("ModuleCategoryID = @ModuleCategoryID ")
        SqlBuilder.Append("AND ProgramTypeID IN ('" & ProgramTypeID.Replace("||", "','") & "') ")
        SqlBuilder.Append("AND DiseaseID IN ('" & DiseaseID.Replace("||", "','") & "') ")
        SqlBuilder.Append("AND IsArchived = @IsArchived ")
        SqlBuilder.Append("AND IsEnabled = @IsEnabled ")
        SqlBuilder.Append("ORDER BY ProgramDate, ResourceName")

        rptPrograms.DataSource = Emagine.GetDataReader(SqlBuilder.ToString, MyCommand)
        rptPrograms.DataBind()
    End Sub
End Class
