Imports Microsoft.VisualBasic
Imports SalesForceApi

Public Class SalesForceConnector

    Public Shared Function GetSalesForceService() As SalesForceApi.SforceService
        Dim SalesForceService As New SalesForceApi.SforceService
        If HttpContext.Current.Application("SalesForceService") IsNot Nothing Then
            If CDate(HttpContext.Current.Application("SalesForceLastLogin")).AddHours(2) < DateTime.Now Then
                HttpContext.Current.Application("SalesForceService") = Login()
                HttpContext.Current.Application("SalesForceLastLogin") = DateTime.Now
            End If
        Else
            HttpContext.Current.Application("SalesForceService") = Login()
            HttpContext.Current.Application("SalesForceLastLogin") = DateTime.Now
        End If

        SalesForceService = HttpContext.Current.Application("SalesForceService")
        SalesForceService.Timeout = 120000

        Return SalesForceService
    End Function

    Private Shared Function Login() As SalesForceApi.SforceService
        Dim SalesForceService As New SalesForceApi.SforceService
        Emagine.LogError("Logging into SalesForce...", "", "")

        Try
            Dim LoginResult As SalesForceApi.LoginResult = SalesForceService.login(GlobalVariables.GetValue("SalesForceUsername"), GlobalVariables.GetValue("SalesForcePassword"))
            If LoginResult.passwordExpired = False Then
                SalesForceService.Url = LoginResult.serverUrl
                SalesForceService.SessionHeaderValue = New SalesForceApi.SessionHeader
                SalesForceService.SessionHeaderValue.sessionId = LoginResult.sessionId

                Emagine.LogError("Successfully logged into SalesForce.", "", "")

            Else
                Emagine.LogError("SalesForce login failed.", "Password Expired", "")
                SalesForceService = Nothing

            End If

        Catch e As System.Web.Services.Protocols.SoapException
            Emagine.LogError("SalesForce login failed.", e.Message, e.StackTrace)
            SalesForceService = Nothing
            Emagine.LogError(e.Message, e.Source, e.StackTrace)

        Catch ex As Exception
            Emagine.LogError(ex)
            Emagine.SendEmail("errors@streambase.com", "errors@streambase.com", "production@emagineusa.net", "production@emagineusa.net", "", "", "SalesForce Login Failed.", ex.Message, "", False)
        End Try

        Return SalesForceService
    End Function

End Class

Public Class SalesForceObject

    Public Shared Function Describe(ByVal strObjectType As String) As Array
        Dim Results As Array = Nothing
        Dim SalesForceService As SalesForceApi.SforceService = SalesForceConnector.GetSalesForceService()

        If SalesForceService IsNot Nothing Then
            Dim ObjectResults() As DescribeSObjectResult = SalesForceService.describeSObjects(New String() {strObjectType})
            Dim ObjectResult As DescribeSObjectResult = ObjectResults(0)
            Dim ObjectFields() As Field = ObjectResult.fields
            Dim FieldNames As String = ""

            For i As Integer = 0 To ObjectFields.GetUpperBound(0)
                Dim Field As Field = ObjectFields(i)
                'FieldNames += Field.name & " - " & Field.label
                FieldNames += Field.name ' & " - " & Field.type.ToString
                If i < ObjectFields.GetUpperBound(0) Then FieldNames += ","
            Next

            Results = FieldNames.Split(",")
        End If
        Return Results
    End Function

    Public Shared Function Add(ByVal strObjectType As String, ByVal dtSfObjectData As DataTable, ByRef strError As String) As String
        'RETURNS THE ID OF THE SF OBJECT
        Dim Result As String = ""
        Dim SalesForceService As SalesForceApi.SforceService = SalesForceConnector.GetSalesForceService()

        If SalesForceService IsNot Nothing Then
            SalesForceService.AssignmentRuleHeaderValue = New SalesForceApi.AssignmentRuleHeader()
            SalesForceService.AssignmentRuleHeaderValue.useDefaultRule = True

            Dim SfdcObject(dtSfObjectData.Rows.Count) As System.Xml.XmlElement
            Dim SfdcObjects(1) As SalesForceApi.sObject
            Dim x As Integer = 0

            For i As Integer = 0 To (dtSfObjectData.Rows.Count - 1)
                'HttpContext.Current.Response.Write(i & " : " & dtSfObjectData.Rows(i).Item(0) & " : " & dtSfObjectData.Rows(i).Item(1) & "<br>")
                SfdcObject(i) = GetNewXmlElement(dtSfObjectData.Rows(i).Item(0), dtSfObjectData.Rows(i).Item(1))
            Next

            Dim acct As New SalesForceApi.sObject
            acct.type = strObjectType
            acct.Any = SfdcObject
            SfdcObjects(0) = acct

            Dim SaveResults() As SaveResult = SalesForceService.create(SfdcObjects)

            For i As Integer = 0 To SaveResults.GetUpperBound(0)
                If SaveResults(i).success Then
                    Result = SaveResults(i).id
                    Emagine.LogError("Create New " & strObjectType, "Success", "SessionID: " & HttpContext.Current.Session.SessionID & vbCrLf & Result)
                Else
                    For j As Integer = 0 To SaveResults(i).errors.GetUpperBound(0)
                        Dim ErrorMessage As String = ""

                        Dim SaveError As SalesForceApi.Error = SaveResults(i).errors(j)
                        ErrorMessage = "Error: " & SaveError.message & vbCrLf
                        strError = SaveError.message

                        For k As Integer = 0 To (dtSfObjectData.Rows.Count - 1)
                            ErrorMessage = ErrorMessage & dtSfObjectData.Rows(k).Item(0) & " : " & dtSfObjectData.Rows(k).Item(1) & vbCrLf
                            x += 1
                        Next

                        Emagine.LogError("SalesForce: Create New " & strObjectType, "Failed", "SessionID: " & HttpContext.Current.Session.SessionID & vbCrLf & ErrorMessage)
                    Next
                End If
            Next
        Else
            Emagine.LogError("SalesForce: Create New " & strObjectType, "Failed", "Unable connect to SalesForce service")
        End If

        Return Result
    End Function

    Private Shared Sub LogError(ByVal strError As String)
        Dim ex As New Exception
        ex.HelpLink = strError
        ex.Source = "Error creating salesforce object"
        Emagine.LogError(ex)
    End Sub

    Public Shared Function Update(ByVal strObjectType As String, ByVal strObjectID As String, ByVal dtSfObjectData As DataTable, ByRef strError As String) As String
        Dim Result As String = ""
        Dim SalesForceService As SalesForceApi.SforceService = SalesForceConnector.GetSalesForceService()

        If SalesForceService IsNot Nothing Then
            SalesForceService.AssignmentRuleHeaderValue = New SalesForceApi.AssignmentRuleHeader()
            SalesForceService.AssignmentRuleHeaderValue.useDefaultRule = True

            Dim FieldList As Array = SalesForceObject.Describe(strObjectType)

            'Dim ObjectType As Type = objSalesForceObject.GetType()

            Dim SfdcObject(dtSfObjectData.Rows.Count) As System.Xml.XmlElement
            Dim SfdcObjects(1) As SalesForceApi.sObject

            Dim UpdateObject As SalesForceApi.sObject = New SalesForceApi.sObject
            UpdateObject.type = strObjectType

            Dim x As Integer = 0

            For i As Integer = 0 To (dtSfObjectData.Rows.Count - 1)
                For j As Integer = 0 To FieldList.GetUpperBound(0)
                    If FieldList(j).ToString.ToUpper = dtSfObjectData.Rows(i).Item(0).ToString.ToUpper Then
                        If dtSfObjectData.Rows(i).Item(0).ToString.Length > 0 Then
                            SfdcObject(x) = GetNewXmlElement(dtSfObjectData.Rows(i).Item(0).ToString, dtSfObjectData.Rows(i).Item(1).ToString)
                            x += 1
                            'HttpContext.Current.Response.Write(ObjectProperty.Name & " : " & ObjectProperty.GetValue(objSalesForceObject, Nothing) & "<br />")
                            Exit For
                        End If
                    End If
                Next
            Next

            'HttpContext.Current.Response.End()

            Dim acct As New SalesForceApi.sObject
            acct.Id = strObjectID
            acct.type = strObjectType
            acct.Any = SfdcObject

            Dim SaveResults() As SaveResult = SalesForceService.update(New SalesForceApi.sObject() {acct})

            For i As Integer = 0 To SaveResults.GetUpperBound(0)
                If SaveResults(i).success Then
                    Result = SaveResults(i).id
                    Emagine.LogError("SalesForce: Update " & strObjectType, "Success", "SessionID: " & HttpContext.Current.Session.SessionID & vbCrLf & Result)
                Else
                    Dim ErrorMessage As String = ""

                    For j As Integer = 0 To SaveResults(i).errors.GetUpperBound(0)
                        Dim SaveError As SalesForceApi.Error = SaveResults(i).errors(j)
                        ErrorMessage = ErrorMessage & "Error: " & SaveError.message & vbCrLf
                        strError = strError & SaveError.message

                        For k As Integer = 0 To (dtSfObjectData.Rows.Count - 1)
                            ErrorMessage = ErrorMessage & dtSfObjectData.Rows(k).Item(0).ToString & " : " & dtSfObjectData.Rows(k).Item(1).ToString & vbCrLf
                        Next

                        Emagine.LogError("SalesForce: Update " & strObjectType, "Failed", "SessionID: " & HttpContext.Current.Session.SessionID & vbCrLf & ErrorMessage)
                    Next
                End If
            Next
        Else
            Emagine.LogError("SalesForce: Update " & strObjectType, "Failed", "Unable connect to SalesForce service")
        End If

        Return Result
    End Function

    Public Shared Function Query(ByVal strSql As String) As DataTable
        Dim SalesForceService As SalesForceApi.SforceService = SalesForceConnector.GetSalesForceService()
        Dim Rs As SalesForceApi.QueryResult = Nothing

        Dim ResultTable As New DataTable

        If SalesForceService IsNot Nothing Then
            Try
                Rs = SalesForceService.query(strSql)

                If Rs.size > 0 Then

                    For i As Integer = 0 To Rs.records.Length - 1
                        Dim Record As SalesForceApi.sObject = Rs.records(i)


                        If ResultTable.Columns.Count = 0 Then
                            For x As Integer = 0 To Record.Any.GetUpperBound(0)
                                ResultTable.Columns.Add(New DataColumn(Record.Any(x).Name))
                            Next
                        End If

                        Dim Row As DataRow = ResultTable.NewRow()

                        Dim j As Integer = 0
                        For j = 0 To Record.Any.GetUpperBound(0)
                            Row.Item(j) = Record.Any(j).InnerText
                        Next

                        ResultTable.Rows.Add(Row)
                    Next

                End If
            Catch ex As Exception
                Emagine.LogError(ex)
                Emagine.LogError("SalesForce: Query Failed", "File: SalesForce.vb" & vbCrLf & "Location: SalesForceObject.Query", "Query: " & strSql)
            End Try
        Else
            Emagine.LogError("SalesForce: Query", "Failed", "Unable connect to SalesForce service")
        End If

        Return ResultTable
    End Function

    Private Shared Function GetNewXmlElement(ByVal Name As String, ByVal nodeValue As String) As System.Xml.XmlElement
        Dim doc As System.Xml.XmlDocument = New System.Xml.XmlDocument
        Dim xmlel As System.Xml.XmlElement = doc.CreateElement(Name)
        xmlel.InnerText = nodeValue
        Return xmlel
    End Function
End Class

Public Class SalesForceTask
    Dim _Subject As String
    Dim _Description As String
    Dim _WhoID As String

    Public Property Subject() As String
        Get
            Return _Subject
        End Get
        Set(ByVal value As String)
            _Subject = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property
    Public Property WhoID() As String
        Get
            Return _WhoID
        End Get
        Set(ByVal value As String)
            _WhoID = value
        End Set
    End Property

    Public ReadOnly Property Priority() As String
        Get
            Return "Normal"
        End Get
    End Property

    Public ReadOnly Property Status() As String
        Get
            Return "Completed"
        End Get
    End Property

    Public Shared Function Add(ByVal objTask As SalesForceTask, Optional ByRef strError As String = "") As String
        Dim Result As String = ""

        Dim PostData As New DataTable("PostData")
        PostData.Columns.Add(New DataColumn("FieldName", System.Type.GetType("System.String")))
        PostData.Columns.Add(New DataColumn("FieldValue", System.Type.GetType("System.String")))

        Dim Row1 As DataRow = PostData.NewRow()
        Row1("FieldName") = "Subject"
        Row1("FieldValue") = objTask.Subject
        PostData.Rows.Add(Row1)

        Dim Row2 As DataRow = PostData.NewRow()
        Row2("FieldName") = "Description"
        Row2("FieldValue") = objTask.Description
        PostData.Rows.Add(Row2)

        Dim Row3 As DataRow = PostData.NewRow()
        Row3("FieldName") = "WhoID"
        Row3("FieldValue") = objTask.WhoID
        PostData.Rows.Add(Row3)

        Dim Row4 As DataRow = PostData.NewRow()
        Row4("FieldName") = "Priority"
        Row4("FieldValue") = objTask.Priority
        PostData.Rows.Add(Row4)

        Dim Row5 As DataRow = PostData.NewRow()
        Row5("FieldName") = "Status"
        Row5("FieldValue") = objTask.Status
        PostData.Rows.Add(Row5)

        Result = SalesForceObject.Add("task", PostData, strError)

        Return Result
    End Function

End Class

Public Class SalesForceLead
    Public Shared Function Add(ByVal dtLeadData As DataTable, Optional ByRef strError As String = "") As String
        Dim SfObjectID As String = ""
        Dim Email As String = ""

        For i As Integer = 0 To (dtLeadData.Rows.Count - 1)
            If dtLeadData.Rows(i).Item(0).ToString = "Email" Then
                Email = dtLeadData.Rows(i).Item(1).ToString
                Exit For
            End If
        Next

        Dim ContactData As DataTable = SalesForceObject.Query("SELECT id, CreatedDate FROM contact WHERE Email = '" & Email & "' AND IsDeleted = False ORDER BY CreatedDate ASC")

        'HttpContext.Current.Response.Write("SELECT id, CreatedDate FROM contact WHERE Email = '" & Email & "' AND IsDeleted = False ORDER BY CreatedDate ASC <br />")
        'HttpContext.Current.Response.Write("Contact Count: " & ContactData.Rows.Count & "<br />")

        If ContactData.Rows.Count > 0 Then 'Contact Exists
            'UPDATE CONTACT?
            'HttpContext.Current.Response.Write("Updating contact...<br />")
            SfObjectID = ContactData.Rows(0).Item(0).ToString
            SfObjectID = SalesForceObject.Update("contact", SfObjectID, dtLeadData, strError)

        Else
            Dim LeadData As DataTable = SalesForceObject.Query("SELECT id, ConvertedContactId, ConvertedOpportunityId, ConvertedAccountId, CreatedDate FROM lead WHERE Email = '" & Email & "' AND IsDeleted = False AND IsConverted = False ORDER BY CreatedDate ASC")

            'HttpContext.Current.Response.Write("SELECT id, ConvertedContactId, ConvertedOpportunityId, ConvertedAccountId, CreatedDate FROM lead WHERE Email = '" & Email & "' AND IsDeleted = False AND IsConverted = False ORDER BY CreatedDate ASC <br />")
            'HttpContext.Current.Response.Write("Lead Count: " & LeadData.Rows.Count & "<br />")

            If LeadData.Rows.Count > 0 Then 'Lead Exists
                'UPDATE LEAD?
                'HttpContext.Current.Response.Write("Updating lead...<br />")
                SfObjectID = LeadData.Rows(0).Item(0).ToString
                SfObjectID = SalesForceObject.Update("lead", SfObjectID, dtLeadData, strError)

            Else
                'ADD NEW LEAD
                'HttpContext.Current.Response.Write("Adding new lead...<br />")
                SfObjectID = SalesForceObject.Add("lead", dtLeadData, strError)
            End If
        End If

        'HttpContext.Current.Response.Write("SfObjectID: " & SfObjectID & "<br />")
        'HttpContext.Current.Response.Write("Error: " & strError & "<br />")
        'HttpContext.Current.Response.End()

        Return SfObjectID
    End Function

    'Public Shared Function Update(ByVal objLead As SalesForceLead) As String
    '    Dim Result As String = ""
    '    Dim LeadID As String = ""
    '    Dim ContactID As String = ""

    '    'HttpContext.Current.Response.Write("Update Lead")

    '    Dim LeadData As DataTable = SalesForceObject.Query("SELECT id, ConvertedContactId, ConvertedOpportunityId, ConvertedAccountId, Duplicate__c, CreatedDate FROM lead WHERE Email = '" & objLead.Email & "' AND IsDeleted = False ORDER BY CreatedDate ASC")
    '    Dim ContactData As DataTable = SalesForceObject.Query("SELECT id, CreatedDate FROM contact WHERE Email = '" & objLead.Email & "' AND IsDeleted = False ORDER BY CreatedDate ASC")

    '    Select Case (LeadData.Rows.Count + ContactData.Rows.Count)
    '        Case 0
    '            LeadID = SalesForceObject.Add("lead", objLead)

    '            Dim Activity As New SalesForceTask
    '            Activity.WhoID = LeadID
    '            Activity.Subject = "User Registered"
    '            If objLead.LeadSource.Length > 0 Then
    '                Activity.Description = "User registered with lead source: " & objLead.LeadSource
    '            Else
    '                Activity.Description = "User registered with no lead source."
    '            End If

    '            Dim TaskID As String = SalesForceTask.Add(Activity)

    '            If LeadID.Length > 0 Then Result = LeadID

    '            If LeadID.Length > 0 And objLead.LeadSource.Length > 0 Then
    '                'Check for a campaign with same name as lead source, create if not exists
    '                Dim Campaign As New SalesForceCampaign
    '                Campaign.Name = objLead.LeadSource
    '                Dim CampaignID As String = SalesForceCampaign.Add(Campaign)

    '                If CampaignID.Length > 0 Then
    '                    'Add Lead to Campaign
    '                    Dim CampaignMember As New SalesForceCampaignMember
    '                    CampaignMember.CampaignID = CampaignID
    '                    CampaignMember.LeadID = LeadID

    '                    SalesForceCampaignMember.Add(CampaignMember)
    '                End If
    '            End If

    '        Case 1
    '            objLead.LeadSource = ""          'DO NOT UPDATE THE LEAD SOURCE

    '            If ContactData.Rows.Count > 0 Then
    '                objLead.Id = ContactData.Rows(0).Item(0).ToString
    '                Result = SalesForceObject.Update("contact", objLead)
    '            Else
    '                objLead.Id = LeadData.Rows(0).Item(0).ToString
    '                Result = SalesForceObject.Update("lead", objLead)
    '            End If

    '            'HttpContext.Current.Response.Write("Lead ID: " & Result)
    '            'HttpContext.Current.Response.End()

    '        Case Is > 1
    '            objLead.LeadSource = ""          'DO NOT UPDATE THE LEAD SOURCE
    '            'LOOP THROUGH RESULTS AND FLAG ALL AS DUPLICATES
    '            For i As Integer = 0 To LeadData.Rows.Count - 1
    '                LeadID = LeadData.Rows(i).Item(0).ToString
    '                Dim ConvertedContactID As String = LeadData.Rows(i).Item(1).ToString
    '                Dim ConvertedOpportunityId As String = LeadData.Rows(i).Item(2).ToString
    '                Dim ConvertedAccountId As String = LeadData.Rows(i).Item(3).ToString
    '                Dim IsDuplicate As String = LeadData.Rows(i).Item(4).ToString

    '                'For j As Integer = 0 To LeadData.Columns.Count - 1
    '                '    HttpContext.Current.Response.Write(LeadData.Rows(i).Item(j).ToString & " - ")
    '                'Next

    '                'HttpContext.Current.Response.Write("<br />")

    '                If ConvertedContactID.Length = 0 And ConvertedOpportunityId.Length = 0 And ConvertedAccountId.Length = 0 And IsDuplicate = "false" Then
    '                    FlagDuplicate(LeadID)
    '                End If
    '            Next

    '            If ContactData.Rows.Count > 0 Then
    '                objLead.Id = ContactData.Rows(0).Item(0).ToString
    '                Result = SalesForceObject.Update("contact", objLead)
    '            Else
    '                objLead.Id = LeadData.Rows(0).Item(0).ToString
    '                Result = SalesForceObject.Update("lead", objLead)
    '            End If

    '            'HttpContext.Current.Response.End()
    '    End Select

    '    Return Result
    'End Function

    'Public Shared Sub FlagDuplicate(ByVal strLeadID As String)
    '    'Dim LeadData As DataTable = SalesForceObject.Query("SELECT id, CreatedDate FROM lead WHERE id = '" & strLeadID & "' AND IsDeleted = False ORDER BY CreatedDate ASC")

    '    'If LeadData.Rows.Count > 0 Then
    '    Dim Lead As New SalesForceLead
    '    Lead.Id = strLeadID
    '    Lead.Duplicate__c = True

    '    SalesForceObject.Update("lead", Lead)
    '    'End If
    'End Sub
End Class

'Public Class SalesForceCampaign

'    Dim _Name As String

'    Public Property Name() As String
'        Get
'            Return _Name
'        End Get
'        Set(ByVal value As String)
'            _Name = value
'        End Set
'    End Property

'    Public ReadOnly Property IsActive() As Boolean
'        Get
'            Return True
'        End Get
'    End Property

'    Public Shared Function Add(ByVal objCampaign As SalesForceCampaign) As String
'        Dim CampaignID As String = ""
'        Dim CampaignData As DataTable = SalesForceObject.Query("SELECT id FROM campaign WHERE Name = '" & objCampaign.Name & "'")

'        If CampaignData.Rows.Count > 0 Then
'            CampaignID = CampaignData.Rows(0).Item(0)

'        Else
'            CampaignID = SalesForceObject.Add("campaign", objCampaign)

'        End If

'        Return CampaignID
'    End Function

'End Class

'Public Class SalesForceCampaignMember
'    Dim _CampaignID As String
'    Dim _LeadID As String
'    Dim _ContactID As String

'    Public Property CampaignID() As String
'        Get
'            Return _CampaignID
'        End Get
'        Set(ByVal value As String)
'            _CampaignID = value
'        End Set
'    End Property

'    Public Property LeadID() As String
'        Get
'            Return _LeadID
'        End Get
'        Set(ByVal value As String)
'            _LeadID = value
'        End Set
'    End Property

'    Public Property ContactID() As String
'        Get
'            Return _ContactID
'        End Get
'        Set(ByVal value As String)
'            _ContactID = value
'        End Set
'    End Property

'    Public Shared Sub Add(ByVal objCampaignMember As SalesForceCampaignMember)
'        SalesForceObject.Add("CampaignMember", objCampaignMember)
'    End Sub
'End Class