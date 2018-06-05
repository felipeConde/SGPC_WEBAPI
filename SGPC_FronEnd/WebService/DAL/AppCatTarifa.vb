Imports Microsoft.VisualBasic

Public Class AppCatTarifa

    Private _codigo As String
    Private _descricao As String
    Private _codigo_tipo As String
    Private _operadora As String


    Public Sub New()

    End Sub

    Public Sub New(ByVal pcodigo As String, ByVal pdescricao As String)
        _codigo = pcodigo
        _descricao = pdescricao
    End Sub

    Public Sub New(ByVal pcodigo As String, ByVal pdescricao As String, ByVal pcodigo_tipo As String, ByVal poperadora As String)
        _codigo = pcodigo
        _descricao = pdescricao
        _codigo_tipo = pcodigo_tipo
        _operadora = poperadora
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

    Public Property Codigo_tipo As String
        Get
            Return _codigo_tipo
        End Get
        Set(ByVal value As String)
            _codigo_tipo = value
        End Set
    End Property

    Public Property Operadora As String
        Get
            Return _operadora
        End Get
        Set(ByVal value As String)
            _operadora = value
        End Set
    End Property

End Class
