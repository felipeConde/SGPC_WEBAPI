Imports Microsoft.VisualBasic

Public Class AppTipoLigacao
    
    Private _codigo As Integer
    Private _descricao As String
    Private _tipo_chamada As String
    Private _tipo As String
    Private _tarifa As String

    Public Sub New()

    End Sub

    Public Sub New(ByVal pcodigo As Integer, ByVal pdescricao As String, ByVal ptipo_chamada As String, ByVal ptipo As String, ByVal ptarifa As String)
        _codigo = pcodigo
        _descricao = pdescricao
        _tipo_chamada = ptipo_chamada
        _tipo = ptipo
        _tarifa = ptarifa
    End Sub

    Public Property Codigo As Integer
        Get
            Return _codigo
        End Get
        Set(ByVal value As Integer)
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

    Public Property Tipo_chamada As String
        Get
            Return _tipo_chamada
        End Get
        Set(ByVal value As String)
            _tipo_chamada = value
        End Set
    End Property

    Public Property Tipo As String
        Get
            Return _tipo
        End Get
        Set(ByVal value As String)
            _tipo = value
        End Set
    End Property

    Public Property Tarifa As String
        Get
            Return _tarifa
        End Get
        Set(ByVal value As String)
            _tarifa = value
        End Set
    End Property

End Class


