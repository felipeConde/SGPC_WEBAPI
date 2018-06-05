Imports System.Net
Imports System.Net.Http
Imports System.Web.Http

Namespace Controllers
    Public Class authenticateController
        Inherits ApiController

        ' GET: authenticate
        Function GetValues() As IHttpActionResult
            Return Ok("Olá")
        End Function


        ' POST api/values
        Public Function PostValue(<FromBody()> user As User) As IHttpActionResult
            'Return Ok(user.Username)

            Dim response = New With {
                 .success = True,
                 .message = "Usuário Ok"
            }

            If user.Password <> "teste" Then
                'negado
                response.success = False
                response.message = "Usuário inválido"

            End If

            Return Ok(response)

        End Function

    End Class
End Namespace