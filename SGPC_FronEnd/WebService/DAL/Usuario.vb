Imports Microsoft.VisualBasic



Public Class Usuario
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

End Class
