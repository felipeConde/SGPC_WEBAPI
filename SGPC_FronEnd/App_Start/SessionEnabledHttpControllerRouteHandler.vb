Imports System.Web.Http.WebHost
Imports System.Web.Routing


Public Class SessionEnabledHttpControllerRouteHandler
    Inherits HttpControllerRouteHandler
    Protected Overrides Function GetHttpHandler(requestContext As RequestContext) As IHttpHandler
        Return New SessionEnabledControllerHandler(requestContext.RouteData)
    End Function
End Class

