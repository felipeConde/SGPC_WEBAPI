Imports Microsoft.VisualBasic

Public Class AppPlano

    Private _codigoPlano As Integer
    Private _plano As String
    Private _codigoOperadora As Integer

    Public Sub New()

    End Sub

    Public Sub New(ByVal pCodigoPlano As Integer, ByVal pPlano As String, ByVal pCodigoOperadora As Integer)

        _codigoPlano = pCodigoPlano
        _plano = pPlano
        _codigoOperadora = pCodigoOperadora

    End Sub

    Public Property CodigoPlano As Integer
        Get
            Return _codigoPlano
        End Get
        Set(ByVal value As Integer)
            _codigoPlano = value
        End Set
    End Property

    Public Property Plano As String
        Get
            Return _plano
        End Get
        Set(ByVal value As String)
            _plano = value
        End Set
    End Property

    Public Property CodigoOperadora As Integer
        Get
            Return _codigoOperadora
        End Get
        Set(ByVal value As Integer)
            _codigoOperadora = value
        End Set
    End Property

    Private _trafego As String
    Public Property Trafego As String
        Get
            Return _trafego
        End Get
        Set(ByVal value As String)
            _trafego = value
        End Set
    End Property

    Private _inicioValidade As String
    Public Property InicioValidade As String
        Get
            Return _inicioValidade
        End Get
        Set(ByVal value As String)
            _inicioValidade = value
        End Set
    End Property

    Private _fimValidade As String
    Public Property FimValidade As String
        Get
            Return _fimValidade
        End Get
        Set(ByVal value As String)
            _fimValidade = value
        End Set
    End Property

    Private _contrato As String
    Public Property Contrato As String
        Get
            Return _contrato
        End Get
        Set(ByVal value As String)
            _contrato = value
        End Set
    End Property


End Class
