Imports System.Net
Imports System.Web.Http
Imports System.Web.HttpContext

Public Class LoginController
    Inherits ApiController

    Dim _dao As New DAOUsuario
    Dim session As  HttpSessionState = HttpContext.Current.Session

    ' GET api/<controller>
    Public Function GetValues() As IEnumerable(Of String)
        'Return New String() {"value1", "value2"}
        Return New String() {ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString}
    End Function

    ' GET api/<controller>/5
    Public Function GetValue(ByVal id As Integer) As String
        Return "value"
    End Function

    ' POST api/<controller>
    Public Function PostValue(<FromBody()> ByVal usuario As appUsuario) As IHttpActionResult
        Dim _usuario As appUsuario = Nothing
        If Not usuario Is Nothing Then
            _usuario = _dao.Login(usuario.username, usuario.password)
        End If


        If Not _usuario Is Nothing Then

            'session.Add("codigousuario", _usuario.codigo)
            'session("codigousuario") = _usuario.codigo
            'OK
            'Session("FirstName") = _usuario.Codigo
            Return Ok(_usuario)
        Else
            'Return httpres
            Return NotFound()
        End If




    End Function

    ' PUT api/<controller>/5
    Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

    End Sub

    ' DELETE api/<controller>/5
    Public Sub DeleteValue(ByVal id As Integer)

    End Sub
End Class
