Imports Microsoft.VisualBasic

Public Class Estado

#Region "Construtor"

    Public Sub New()

    End Sub
    Public Sub New(ByVal pCodigo As Integer, ByVal pDescricao As String)
        _codigo = pCodigo
        _descricao = pDescricao
    End Sub
#End Region


#Region "Propriedades"
    Private _codigo As Integer
    Private _descricao As String

    Public Property Codigo() As Integer
        Get
            Return _codigo
        End Get
        Set(ByVal value As Integer)
            _codigo = value
        End Set
    End Property

    Public Property Descricao() As String
        Get
            Return _descricao
        End Get
        Set(ByVal value As String)
            _descricao = value
        End Set
    End Property


#End Region

End Class

