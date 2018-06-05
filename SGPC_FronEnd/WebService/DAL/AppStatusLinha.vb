Imports Microsoft.VisualBasic

Public Class AppStatusLinha

    Private _codigo_status As Integer
    Private _Descricao As String

    Public Sub New()

    End Sub

    Public Sub New(ByVal pCodigoStatus As Integer, ByVal PDescricao As String)
        _codigo_status = pCodigoStatus
        _Descricao = PDescricao
    End Sub

    Public Property CodigoStatus As Integer
        Get
            Return _codigo_status
        End Get
        Set(ByVal value As Integer)
            _codigo_status = value
        End Set
    End Property

    Public Property Descricao As String
        Get
            Return _Descricao
        End Get
        Set(ByVal value As String)
            _Descricao = value
        End Set
    End Property

End Class
