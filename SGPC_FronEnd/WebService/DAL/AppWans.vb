Imports Microsoft.VisualBasic

Public Class AppWans
#Region "Construtor"
    Public Sub New()

    End Sub

    Public Sub New(ByVal pcodigo_wan As String, ByVal pnome_wan As String, ByVal pconcentrador As String, ByVal pwan_matriz As String, ByVal pwan_remota As String, ByVal pmascara As String, ByVal prange As String)
        _codigo_wan = pcodigo_wan
        _nome_wan = pnome_wan
        _concentrador = pconcentrador
        _wan_matriz = pwan_matriz
        _wan_remota = pwan_remota
        _mascara = pmascara
        _range = prange

    End Sub

#End Region

#Region "Propriedades"

    Private _nome_wan As String
    Private _codigo_wan As String
    Private _concentrador As String
    Private _wan_matriz As String
    Private _wan_remota As String
    Private _mascara As String
    Private _range As String
    Private _end_ip_operadora As String
    Private _end_ip_cliente As String
    Private _ip_inicial As String
    Private _ip_final As String
    Private _gateway As String
    Private _dns_1 As String
    Private _dns_2 As String
    Private _dominio As String
    Private _rede As String

    Public Property Rede() As String
        Get
            Return _rede
        End Get
        Set(ByVal value As String)
            _rede = value
        End Set
    End Property

    Public Property End_ip_operadora() As String
        Get
            Return _end_ip_operadora
        End Get
        Set(ByVal value As String)
            _end_ip_operadora = value
        End Set
    End Property

    Public Property End_ip_cliente() As String
        Get
            Return _end_ip_cliente
        End Get
        Set(ByVal value As String)
            _end_ip_cliente = value
        End Set
    End Property

    Public Property Ip_Inicial() As String
        Get
            Return _ip_inicial
        End Get
        Set(ByVal value As String)
            _ip_inicial = value
        End Set
    End Property

    Public Property Ip_Final() As String
        Get
            Return _ip_final
        End Get
        Set(ByVal value As String)
            _ip_final = value
        End Set
    End Property

    Public Property Gateway() As String
        Get
            Return _gateway
        End Get
        Set(ByVal value As String)
            _gateway = value
        End Set
    End Property

    Public Property DNS_1() As String
        Get
            Return _dns_1
        End Get
        Set(ByVal value As String)
            _dns_1 = value
        End Set
    End Property

    Public Property DNS_2() As String
        Get
            Return _dns_2
        End Get
        Set(ByVal value As String)
            _dns_2 = value
        End Set
    End Property

    Public Property Dominio() As String
        Get
            Return _dominio
        End Get
        Set(ByVal value As String)
            _dominio = value
        End Set
    End Property

    Public Property Codigo_Wan() As String
        Get
            Return _codigo_wan
        End Get
        Set(ByVal value As String)
            _codigo_wan = value
        End Set
    End Property

    Public Property Nome_Wan() As String
        Get
            Return _nome_wan
        End Get
        Set(ByVal value As String)
            _nome_wan = value
        End Set
    End Property


    Public Property concentrador() As String
        Get
            Return _concentrador
        End Get
        Set(ByVal value As String)
            _concentrador = value
        End Set
    End Property

    Public Property wan_matriz() As String
        Get
            Return _wan_matriz
        End Get
        Set(ByVal value As String)
            _wan_matriz = value
        End Set
    End Property

    Public Property wan_remota() As String
        Get
            Return _wan_remota
        End Get
        Set(ByVal value As String)
            _wan_remota = value
        End Set
    End Property

    Public Property Mascara() As String
        Get
            Return _mascara
        End Get
        Set(ByVal value As String)
            _mascara = value
        End Set
    End Property

    Public Property Range() As String
        Get
            Return _range
        End Get
        Set(ByVal value As String)
            _range = value
        End Set
    End Property

#End Region

End Class
