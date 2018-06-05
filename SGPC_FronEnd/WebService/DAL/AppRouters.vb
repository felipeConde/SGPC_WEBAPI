Imports Microsoft.VisualBasic

Public Class AppRouters
#Region "Construtor"
    Public Sub New()

    End Sub

    Public Sub New(ByVal pcodigo_router As String, ByVal pnome As String, ByVal pmodelo As String, ByVal pversao As String, ByVal prelease As String, ByVal pbootrom As String, ByVal pativo As String, ByVal pcanal As String, ByVal pip_pabx As String, ByVal pmarca As String)
        _codigo_router = pcodigo_router
        _nome = pnome
        _modelo = pmodelo
        _versao = pversao
        _release = prelease
        _bootrom = pbootrom
        _ativo = pativo
        _canal = pcanal
        _ip_pabx = pip_pabx
        _marca = pmarca

    End Sub

#End Region

#Region "Propriedades"

    Private _codigo_router As String
    Private _nome As String
    Private _modelo As String
    Private _versao As String
    Private _release As String
    Private _bootrom As String
    Private _ativo As String
    Private _canal As String
    Private _ip_pabx As String
    Private _marca As String


    Public Property Codigo_Router() As String
        Get
            Return _codigo_router
        End Get
        Set(ByVal value As String)
            _codigo_router = value
        End Set
    End Property

    Public Property Nome() As String
        Get
            Return _nome
        End Get
        Set(ByVal value As String)
            _nome = value
        End Set
    End Property

    Public Property Marca() As String
        Get
            Return _marca
        End Get
        Set(ByVal value As String)
            _marca = value
        End Set
    End Property

        Public Property Modelo() As String
        Get
            Return _modelo
        End Get
        Set(ByVal value As String)
            _modelo = value
        End Set
    End Property
        Public Property Versao() As String
        Get
            Return _versao
        End Get
        Set(ByVal value As String)
            _versao = value
        End Set
    End Property
        Public Property Release() As String
        Get
            Return _release
        End Get
        Set(ByVal value As String)
            _release = value
        End Set
    End Property
        Public Property BootRom() As String
        Get
            Return _bootrom
        End Get
        Set(ByVal value As String)
            _bootrom = value
        End Set
    End Property
        Public Property Ativo() As String
        Get
            Return _ativo
        End Get
        Set(ByVal value As String)
            _ativo = value
        End Set
    End Property

    Public Property Canal() As String
        Get
            Return _canal
        End Get
        Set(ByVal value As String)
            _canal = value
        End Set
    End Property

   Public Property IP_PABX() As String
        Get
            Return _ip_pabx
        End Get
        Set(ByVal value As String)
            _ip_pabx = value
        End Set
    End Property
#End Region

End Class
