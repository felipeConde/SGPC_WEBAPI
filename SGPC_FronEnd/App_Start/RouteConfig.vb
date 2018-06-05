Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Http
Imports System.Web.Mvc
Imports System.Web.Routing

Public Module RouteConfig
    Public Sub RegisterRoutes(ByVal routes As RouteCollection)
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}")

        ' Web API Session Enabled Route Configurations
        'routes.MapHttpRoute(
        '        name:="SessionsRoute",
        '        routeTemplate:="api/sessions/{controller}/{id}",
        '        defaults:=New With {.id = RouteParameter.Optional}
        '    ).RouteHandler = New SessionEnabledHttpControllerRouteHandler()

        routes.MapRoute(
            name:="Default",
            url:="{controller}/{action}/{id}",
            defaults:=New With {.action = "Index", .id = UrlParameter.Optional}
        )
    End Sub
End Module