Public Class appUsuario

    Private _username As String

    Private _password As String

    Private _email As String

    Private _codigo As String

    Private _nome_usuario As String



    Public Property username As String
        Get
            Return _username
        End Get
        Set(value As String)
            _username = value
        End Set
    End Property

    Public Property password As String
        Get
            Return _password
        End Get
        Set(value As String)
            _password = value
        End Set
    End Property

    Public Property Email As String
        Get
            Return _email
        End Get
        Set(value As String)
            _email = value
        End Set
    End Property

    Public Property codigo As String
        Get
            Return _codigo
        End Get
        Set(value As String)
            _codigo = value
        End Set
    End Property

    Public Property nome_usuario As String
        Get
            Return _nome_usuario
        End Get
        Set(value As String)
            _nome_usuario = value
        End Set
    End Property
End Class
