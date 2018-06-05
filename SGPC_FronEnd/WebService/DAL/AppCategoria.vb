Imports Microsoft.VisualBasic

Public Class AppCategoria
    Public Sub New()

    End Sub

    Private _codigo As String

    Public Property Codigo As String
        Get
            Return _codigo
        End Get
        Set(ByVal value As String)
            _codigo = value
        End Set
    End Property

    Private _descricao As String

    Public Property Descricao As String
        Get
            Return _descricao
        End Get
        Set(ByVal value As String)
            _descricao = value
        End Set
    End Property

    Private _ativo As String

    Public Property Ativo As String
        Get
            Return _ativo
        End Get
        Set(ByVal value As String)
            _ativo = value
        End Set
    End Property

    Private _flagCCusto As String

    Public Property FlagCCusto As String
        Get
            Return _flagCCusto
        End Get
        Set(ByVal value As String)
            _flagCCusto = value
        End Set
    End Property

End Class
