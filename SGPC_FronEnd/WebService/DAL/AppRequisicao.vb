Imports Microsoft.VisualBasic

Public Class AppRequisicao

    Private _codigo As String
    Private _descricao As String
    Private _autor As String
    Private _autorizador As String
    Private _aprovada As String
    Private _operador As String
    Private _concluida As String

    Public Sub New()

    End Sub

    Public Sub New(ByVal pCodigo As String, ByVal pdescricao As String)
        _codigo = pCodigo
        _descricao = pdescricao
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

    Public Property Autor As String
        Get
            Return _autor
        End Get
        Set(ByVal value As String)
            _autor = value
        End Set
    End Property

    Public Property Autorizador As String
        Get
            Return _autorizador
        End Get
        Set(ByVal value As String)
            _autorizador = value
        End Set
    End Property

    Public Property Aprovada As String
        Get
            Return _aprovada
        End Get
        Set(ByVal value As String)
            _aprovada = value
        End Set
    End Property

    Public Property Operador As String
        Get
            Return _operador
        End Get
        Set(ByVal value As String)
            _operador = value
        End Set
    End Property

    Public Property Concluida As String
        Get
            Return _concluida
        End Get
        Set(ByVal value As String)
            _concluida = value
        End Set
    End Property


End Class
