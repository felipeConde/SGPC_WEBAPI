Imports Microsoft.VisualBasic

Public Class AppPesquisa

    Private _codigo As String
    Private _nome As String


    Public Sub New()
    End Sub

    Public Sub New(ByVal pcodigo As String, ByVal pnome As String)
        _codigo = pcodigo
        _nome = pnome
    End Sub

    Public Property Codigo() As String
        Get
            Return _codigo
        End Get
        Set(ByVal value As String)
            _codigo = value
        End Set
    End Property

    Public Property nome() As String
        Get
            Return _nome
        End Get
        Set(ByVal value As String)
            _nome = value
        End Set
    End Property

End Class
