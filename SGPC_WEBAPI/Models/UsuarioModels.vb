Public Class UsuarioModels

    Private _codigoUsuario As Integer
    Private _nomeUsuario As String

    Public Property CodigoUsuario() As Integer
        Get
            Return _codigoUsuario
        End Get
        Set(ByVal value As Integer)
            _codigoUsuario = value
        End Set
    End Property

    Public Property NomeUsuario() As String
        Get
            Return _nomeUsuario
        End Get
        Set(ByVal value As String)
            _nomeUsuario = value
        End Set
    End Property

    Public Property Login As String
        Get
            Return _login
        End Get
        Set(value As String)
            _login = value
        End Set
    End Property

    Public Property Password As String
        Get
            Return _password
        End Get
        Set(value As String)
            _password = value
        End Set
    End Property

    Private _login As String

    Private _password As String


End Class
