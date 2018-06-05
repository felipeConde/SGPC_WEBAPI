Imports System.Web.Http.WebHost
Imports System.Web.Routing


Public Class SessionEnabledControllerHandler
        Inherits HttpControllerHandler
        Implements IRequiresSessionState
        Public Sub New(routeData As RouteData)
            MyBase.New(routeData)
        End Sub
    End Class


