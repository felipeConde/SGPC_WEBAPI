Imports Microsoft.VisualBasic

Public Class AppLans
#Region "Construtor"
    Public Sub New()

    End Sub

    Public Sub New(ByVal pcodigo_lan As String, ByVal pnome_lan As String, ByVal prede As String, ByVal pip As String, ByVal pbroadcast As String, ByVal pmascara As String, ByVal prange As String, ByVal prange_2 As String)
        _codigo_lan = pcodigo_lan
        _nome_lan = pnome_lan
        _rede = prede
        _ip = pip
        _broadcast = pbroadcast
        _mascara = pmascara
        _range = prange
        _range_2 = prange_2

    End Sub

#End Region

#Region "Propriedades"

    Private _codigo_lan As String
    Private _nome_lan As String
    Private _rede As String
    Private _ip As String
    Private _broadcast As String
    Private _mascara As String
    Private _range As String
    Private _range_2 As String


    Public Property Nome_Lan() As String
        Get
            Return _nome_lan
        End Get
        Set(ByVal value As String)
            _nome_lan = value
        End Set
    End Property

    Public Property Codigo_Lan() As String
        Get
            Return _codigo_lan
        End Get
        Set(ByVal value As String)
            _codigo_lan = value
        End Set
    End Property

    Public Property Rede() As String
        Get
            Return _rede
        End Get
        Set(ByVal value As String)
            _rede = value
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

    Public Property Broadcast() As String
        Get
            Return _broadcast
        End Get
        Set(ByVal value As String)
            _broadcast = value
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

    Public Property Range_2() As String
        Get
            Return _range_2
        End Get
        Set(ByVal value As String)
            _range_2 = value
        End Set
    End Property

#End Region

End Class
