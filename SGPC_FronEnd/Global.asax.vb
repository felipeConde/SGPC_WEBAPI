Imports System.Web.SessionState
Imports System.Globalization
Imports System.Web.Http

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Protected Sub Application_Start()
        GlobalConfiguration.Configure(AddressOf WebApiConfig.Register)
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

    Protected Sub Application_PreRequestHandlerExecute(sender As Object, e As EventArgs)
        If TypeOf Context.Handler Is IRequiresSessionState OrElse TypeOf Context.Handler Is IReadOnlySessionState Then
            ' Your Methods
            Dim context As HttpContext = HttpContext.Current
        End If
    End Sub

    Private Sub MvcApplication_PostAuthenticateRequest(sender As Object, e As EventArgs)
        System.Web.HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required)
    End Sub

    Public Overrides Sub Init()
        AddHandler Me.PostAuthenticateRequest, AddressOf MvcApplication_PostAuthenticateRequest
        MyBase.Init()
    End Sub


End Class