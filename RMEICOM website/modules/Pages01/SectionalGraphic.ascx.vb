
Partial Class modules_Pages01_SectionalGraphic
    Inherits System.Web.UI.UserControl

    Dim _MasterControlID As String = ""
    Dim _GraphicNumber As Integer = 0
    Dim _SectionalGraphic As String = ""

    Public WriteOnly Property MasterControlID() As String
        Set(ByVal value As String)
            _MasterControlID = value
        End Set
    End Property

    Public Property SectionalGraphic() As String
        Get
            Return Me.GetSectionalGraphic()
        End Get
        Set(ByVal value As String)
            _SectionalGraphic = value
        End Set
    End Property

    Public Property GraphicNumber() As Integer
        Get
            Return _GraphicNumber
        End Get
        Set(ByVal value As Integer)
            _GraphicNumber = value
        End Set
    End Property

    Function GetSectionalGraphic() As String
        Dim PageID As Integer = CInt(Session("PageID"))
        Dim SectionID As Integer = Pages01.GetSectionID(PageID)
        Dim SectionGraphic As String = ""

        Dim PageInfo As New Pages01
        PageInfo = PageInfo.GetPageInfo(PageID)
        SectionGraphic = PageInfo.HeaderGraphic

        If SectionGraphic.Length = 0 Then
            PageInfo = PageInfo.GetPageInfo(SectionID)
            SectionGraphic = PageInfo.HeaderGraphic
        End If

        Return SectionGraphic
    End Function

    Protected Sub imgSectionalGraphic_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgSectionalGraphic.Load
        Dim SectionalGraphic As String = GetSectionalGraphic()

        If SectionalGraphic.Length > 0 Then
            Dim arySectionalGraphic As Array = SectionalGraphic.Split("^")
            If _GraphicNumber <= UBound(arySectionalGraphic) Then
                imgSectionalGraphic.ImageUrl = arySectionalGraphic(_GraphicNumber)
            End If
        End If

    End Sub

    
End Class
