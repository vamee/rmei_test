<%@ Application Language="VB" %>

<script runat="server"> 
    Public Overrides Function GetVaryByCustomString(ByVal objContext As HttpContext, ByVal strParam As String) As String
        Dim InCookie As HttpCookie = objContext.Request.Cookies("MemberID")
        Dim MemberID As String = InCookie.Value
        
        Dim Result As String = objContext.Request("PageKey").ToString & "-" & Resources.GetResourceID() & " - " & MemberID
        
        'TO DO: Add ContentID and ContentPreviewID

        Return Result
    End Function
        
 
   

    
    
    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
        
        PeterBlum.VAM.Globals.Suite_LicenseKey = "32-8311203600|" & Server.MachineName
        PeterBlum.PetersDatePackage.Globals.LicenseKey = "17-2412100318|" & Server.MachineName
        Application("SalesForceService") = Nothing
        
        'If GlobalVariables.GetValue("SalesForceIntegrationType") = "API" Then
        '    Dim SalesForceService As New SalesForceApi.SforceService
        '    Try
        '        Dim LoginResult As SalesForceApi.LoginResult = SalesForceService.login(GlobalVariables.GetValue("SalesForceUsername"), GlobalVariables.GetValue("SalesForcePassword"))
        '        If LoginResult.passwordExpired = False Then
        '            SalesForceService.Url = LoginResult.serverUrl
        '            SalesForceService.SessionHeaderValue = New SalesForceApi.SessionHeader
        '            SalesForceService.SessionHeaderValue.sessionId = LoginResult.sessionId

        '            Application("SalesForceService") = SalesForceService
        '            'Dim UserInfo As sforce.GetUserInfoResult = LoginResult.userInfo
        '        End If
        '    Catch ex As Exception
        '        Emagine.LogError(ex)
        '    End Try
        'End If
    End Sub
    
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub
        
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub
</script>