Imports Microsoft.VisualBasic

Public Class AppLinhasMoveis

    Private _codigo_linha As Integer
    Private _codigo_aparelho As Integer
    Private _codigo_sim As Integer
    Private _fleet As String
    Private _ip As String
    Private _codigo_operadora As Integer
    Private _fim_comodato As Date
    Private _natureza As Integer
    Private _mensalidade As Integer
    Private _cliente As String
    Private _obs As String
    Private _codigo_termo As Integer
    Private _codigo_usuario As Integer
    Private _codigo_tecnologia As Integer
    Private _nota_fiscal As String
    Private _codigo_cliente As String
    Private _protocolo_cancel As String
    Private _sucursal As String

    Public Sub New()
    End Sub

    Public Sub New(ByVal pcodigo_aparelho As Integer)
        _codigo_aparelho = pcodigo_aparelho
    End Sub


    Public Property Codigo_linha() As Integer
        Get
            Return _codigo_linha
        End Get
        Set(ByVal value As Integer)
            _codigo_linha = value
        End Set
    End Property

    Public Property Codigo_sim() As Integer
        Get
            Return _codigo_sim
        End Get
        Set(ByVal value As Integer)
            _codigo_sim = value
        End Set
    End Property

    Public Property Fleet() As String
        Get
            Return _fleet
        End Get
        Set(ByVal value As String)
            _fleet = value
        End Set
    End Property

    Public Property Ip() As String
        Get
            Return _ip
        End Get
        Set(ByVal value As String)
            _ip = value
        End Set
    End Property

    Public Property Codigo_operadora() As Integer
        Get
            Return _codigo_operadora
        End Get
        Set(ByVal value As Integer)
            _codigo_operadora = value
        End Set
    End Property

    Public Property Fim_comodato() As String
        Get
            Return _fim_comodato
        End Get
        Set(ByVal value As String)
            _fim_comodato = value
        End Set
    End Property

    Public Property Natureza() As Integer
        Get
            Return _natureza
        End Get
        Set(ByVal value As Integer)
            _natureza = value
        End Set
    End Property

    Public Property Mensalidade() As Integer
        Get
            Return _mensalidade
        End Get
        Set(ByVal value As Integer)
            _mensalidade = value
        End Set
    End Property

    Public Property Cliente() As String
        Get
            Return _cliente
        End Get
        Set(ByVal value As String)
            _cliente = value
        End Set
    End Property

    Public Property Obs() As String
        Get
            Return _obs
        End Get
        Set(ByVal value As String)
            _obs = value
        End Set
    End Property

    Public Property Codigo_termo() As Integer
        Get
            Return _codigo_termo
        End Get
        Set(ByVal value As Integer)
            _codigo_termo = value
        End Set
    End Property

    Public Property Codigo_usuario() As Integer
        Get
            Return _codigo_usuario
        End Get
        Set(ByVal value As Integer)
            _codigo_usuario = value
        End Set
    End Property

    Public Property Codigo_tecnologia() As Integer
        Get
            Return _codigo_tecnologia
        End Get
        Set(ByVal value As Integer)
            _codigo_tecnologia = value
        End Set
    End Property

    Public Property Nota_fiscal() As String
        Get
            Return _nota_fiscal
        End Get
        Set(ByVal value As String)
            _nota_fiscal = value
        End Set
    End Property

    Public Property Codigo_cliente() As Integer
        Get
            Return _codigo_cliente
        End Get
        Set(ByVal value As Integer)
            _codigo_cliente = value
        End Set
    End Property

    Public Property Protocolo_Cancel() As String
        Get
            Return _protocolo_cancel
        End Get
        Set(ByVal value As String)
            _protocolo_cancel = value
        End Set
    End Property

    Public Property Sucursal() As String
        Get
            Return _sucursal
        End Get
        Set(ByVal value As String)
            _sucursal = value
        End Set
    End Property

End Class
