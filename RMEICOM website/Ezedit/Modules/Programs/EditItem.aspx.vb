
Partial Class Ezedit_Modules_Programs_EditItem
    Inherits System.Web.UI.Page

    Dim _ModuleKey As String = "Programs"
    Dim _CategoryID As Integer = 0
    Dim _ProgramID As String = ""

#Region "Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request("CategoryID") IsNot Nothing Then _CategoryID = Emagine.GetNumber(Request("CategoryID"))
        If Request("ProgramID") IsNot Nothing Then _ProgramID = Request("ProgramID")

        If Not Page.IsPostBack Then
            ddlProgramTypeID.DataBind()
            ddlDiseaseID.DataBind()

            If _CategoryID > 0 Then
                Me.DisplayEditPanel(_ProgramID)
            Else
                Response.Redirect("Default.aspx")
            End If
        End If

        

    End Sub
#End Region

#Region "Events"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If PeterBlum.VAM.Globals.Page.IsValid Then
            Dim Result As Boolean = False
            Dim ErrorMessage As String = ""
            Dim ProgramID As String = hdnItemID.Value
            Dim SqlBuilder As New StringBuilder

            Dim MyCommand As New SqlCommand
            MyCommand.Parameters.AddWithValue("@ProgramID", ProgramID)
            MyCommand.Parameters.AddWithValue("@ProgramTypeID", ddlProgramTypeID.SelectedValue)
            MyCommand.Parameters.AddWithValue("@DiseaseID", ddlDiseaseID.SelectedValue)
            MyCommand.Parameters.AddWithValue("@ProgramDate", txtProgramDate.Text)
            MyCommand.Parameters.AddWithValue("@Description", txtDescription.EditorContent)
            MyCommand.Parameters.AddWithValue("@ProgramUrl", txtProgramUrl.Text)

            ProgramID = Emagine.GetDbValue("SELECT ProgramID FROM Custom_Programs WHERE ProgramID = @ProgramID", MyCommand)

            If ProgramID.Length > 0 Then
                SqlBuilder.Append("UPDATE Custom_Programs SET ")
                SqlBuilder.Append("ProgramTypeID = @ProgramTypeID, ")
                SqlBuilder.Append("DiseaseID = @DiseaseID, ")
                SqlBuilder.Append("ProgramDate = @ProgramDate, ")
                SqlBuilder.Append("Description = @Description, ")
                SqlBuilder.Append("ProgramUrl = @ProgramUrl ")
                SqlBuilder.Append("WHERE ProgramID = @ProgramID")

                If Emagine.ExecuteSQL(SqlBuilder.ToString, MyCommand, ErrorMessage) Then
                    Dim ResourceID As String = Emagine.GetDbValue("SELECT ResourceID FROM Custom_Programs WHERE ProgramID = @ProgramID", MyCommand)
                    Dim MyResource As Resources.Resource = Resources.Resource.GetResource(ResourceID)
                    MyResource.ResourceName = txtResourceName.Text
                    MyResource.UpdatedDate = Date.Now()
                    MyResource.UpdatedBy = Session("EzEditUsername")

                    Result = Resources.Resource.UpdateResource(MyResource, ErrorMessage)
                End If

                lblAlert.Text = "The program has been updated successfully."
            Else
                Dim MyResource As New Resources.Resource
                MyResource.ResourceID = Emagine.GetUniqueID()
                MyResource.ResourceType = _ModuleKey
                MyResource.ResourceName = txtResourceName.Text
                MyResource.CreatedBy = Session("EzEditUsername")
                MyResource.UpdatedBy = Session("EzEditUsername")

                ProgramID = Emagine.GetUniqueID()
                MyCommand.Parameters("@ProgramID").Value = ProgramID
                MyCommand.Parameters.AddWithValue("@ResourceID", MyResource.ResourceID)
                MyCommand.Parameters.AddWithValue("@ModuleCategoryID", _CategoryID)

                SqlBuilder.Append("INSERT INTO Custom_Programs ")
                SqlBuilder.Append("(ProgramID,ResourceID,ModuleCategoryID,ProgramTypeID,DiseaseID,ProgramDate,Description,ProgramUrl) ")
                SqlBuilder.Append("VALUES ")
                SqlBuilder.Append("(@ProgramID,@ResourceID,@ModuleCategoryID,@ProgramTypeID,@DiseaseID,@ProgramDate,@Description,@ProgramUrl)")

                If Emagine.ExecuteSQL(SqlBuilder.ToString, MyCommand, ErrorMessage) Then
                    Result = Resources.Resource.AddResource(MyResource, ErrorMessage)
                End If

                lblAlert.Text = "The program has been added successfully."

            End If

            If Result Then
                Session("Alert") = lblAlert.Text
                'Response.Redirect("ItemList.aspx?CategoryID=" & _CategoryID)
                Me.Redirect()

            Else
                lblAlert.Text = "An error occurred: " & ErrorMessage
            End If
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Redirect()
    End Sub
#End Region

#Region "Custom Procedures"
    Sub DisplayEditPanel(ByVal strProgramID As String)
        Dim MyCommand As New SqlCommand
        MyCommand.Parameters.AddWithValue("@ProgramID", strProgramID)

        Dim Sql As String = "SELECT * FROM qryCustom_Programs WHERE ProgramID = @ProgramID"

        Dim MyProgramData As DataTable = Emagine.GetDataTable(Sql, MyCommand)
        If MyProgramData.Rows.Count > 0 Then

            For Each Item As ListItem In ddlProgramTypeID.Items
                If Item.Value = MyProgramData.Rows(0).Item("ProgramTypeID").ToString Then
                    Item.Selected = True
                    Exit For
                End If
            Next

            For Each Item As ListItem In ddlDiseaseID.Items
                If Item.Value = MyProgramData.Rows(0).Item("DiseaseID").ToString Then
                    Item.Selected = True
                    Exit For
                End If
            Next

            txtResourceName.Text = MyProgramData.Rows(0).Item("ResourceName").ToString
            txtProgramDate.Text = MyProgramData.Rows(0).Item("ProgramDate").ToString
            txtDescription.EditorContent = MyProgramData.Rows(0).Item("Description").ToString
            txtProgramUrl.Text = MyProgramData.Rows(0).Item("ProgramUrl").ToString
            hdnItemID.Value = strProgramID
        End If
        MyProgramData.Dispose()
        MyCommand.Dispose()
    End Sub

    Sub Redirect()
        Dim OrderBy As String = ""
        Dim OrderByDirection As String = "ASC"
        Dim PageIndex As Integer = 0
        Dim IsArchived As String = ""

        If Request.QueryString("OrderBy") IsNot Nothing Then OrderBy = Request.QueryString("OrderBy").ToString
        If Request.QueryString("OrderByDirection") IsNot Nothing Then OrderByDirection = Request.QueryString("OrderByDirection").ToString
        If Request.QueryString("PageIndex") IsNot Nothing Then PageIndex = Emagine.GetNumber(Request.QueryString("PageIndex").ToString)
        If Request.QueryString("IsArchived") IsNot Nothing Then IsArchived = Request.QueryString("IsArchived").ToString

        Response.Redirect("ItemList.aspx?CategoryID=" & _CategoryID & "&OrderBy=" & OrderBy & "&OrderByDirection=" & OrderByDirection & "&PageIndex=" & PageIndex & "&IsArchived=" & IsArchived)
    End Sub
#End Region

    
    
    
    
End Class
