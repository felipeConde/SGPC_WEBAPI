Imports System.Web
Imports System.Web.Services
Imports System.Web.HttpRequest
Imports System.IO
Imports System.Web.SessionState

Public Class user
    Implements System.Web.IHttpHandler, System.Web.SessionState.IRequiresSessionState

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        'context.Response.ContentType = "text/plain"
        'context.Response.Write("Hello World!")
        Dim session As HttpSessionState = HttpContext.Current.Session
        Dim values As String = ""
        'Dim codigousuario As String = context.Request.Params("codigousuario")
        'session.Add("codigousuario", codigousuario)
        Using reader = New StreamReader(context.Request.InputStream)
            ' This will equal to "charset = UTF-8 & param1 = val1 & param2 = val2 & param3 = val3 & param4 = val4"
            values = reader.ReadToEnd()
        End Using
        Dim codigousuario As String = values
        session.Add("codigousuario", codigousuario)

        session("conexao") = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString


    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class