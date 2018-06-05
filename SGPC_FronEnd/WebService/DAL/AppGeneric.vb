Imports Microsoft.VisualBasic

Public Class AppGeneric
    
    Private _codigo As String
    Private _descricao As String
    Private _descricao2 As String


    Public Sub New()

    End Sub

    Public Sub New(ByVal pCodigo As String, ByVal pDescricao As String)
        _codigo = pCodigo
        _descricao = pDescricao
    End Sub

    Public Sub New(ByVal pCodigo As String, ByVal pDescricao As String, ByVal pDescricao2 As String)
        _codigo = pCodigo
        _descricao = pDescricao
        _descricao2 = pDescricao2
    End Sub

    Public Property Codigo As String
        Get
            Return _codigo
        End Get
        Set(ByVal value As String)
            _codigo = value
        End Set
    End Property

    Public Property Descricao As String
        Get
            Return _descricao
        End Get
        Set(ByVal value As String)
            _descricao = value
        End Set
    End Property

    Public Property Descricao2 As String
        Get
            Return _descricao2
        End Get
        Set(ByVal value As String)
            _descricao2 = value
        End Set
    End Property

End Class

