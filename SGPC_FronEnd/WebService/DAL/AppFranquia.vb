Imports Microsoft.VisualBasic

Public Class AppFranquia


    Public Sub New()

    End Sub

    Public Sub New(pCodigo As Integer, pNome As String, pCodigoOperadora As Integer, pValor As String, pTipoFranquia As Integer, pMaxMin As Integer, pCodigoFatura As Integer)
        _codigo = pCodigo
        _NOME = pNome
        _CODIGO_OPERADORA = pCodigoOperadora
        _VALOR = pValor
        _TIPO_FRANQUIA = pTipoFranquia
        _MAX_MIN = pMaxMin
        _CODIGO_FATURA = pCodigoFatura

    End Sub


    Private _codigo As Integer
    Public Property Codigo As String
        Get
            Return _codigo
        End Get
        Set(value As String)
            _codigo = value
        End Set
    End Property

    Private _NOME As String
    Public Property NOME As String
        Get
            Return _NOME
        End Get
        Set(value As String)
            _NOME = value
        End Set
    End Property

    Private _CODIGO_OPERADORA As Integer
    Public Property CODIGO_OPERADORA As Integer
        Get
            Return _CODIGO_OPERADORA
        End Get
        Set(value As Integer)
            _CODIGO_OPERADORA = value
        End Set
    End Property

    Private _VALOR As String
    Public Property VALOR As String
        Get
            Return _VALOR
        End Get
        Set(value As String)
            _VALOR = value
        End Set
    End Property

    Private _TIPO_FRANQUIA As Integer
    Public Property TIPO_FRANQUIA As Integer
        Get
            Return _TIPO_FRANQUIA
        End Get
        Set(value As Integer)
            _TIPO_FRANQUIA = value
        End Set
    End Property

    Private _MAX_MIN As Integer
    Public Property MAX_MIN As Integer
        Get
            Return _MAX_MIN
        End Get
        Set(value As Integer)
            _MAX_MIN = value
        End Set
    End Property

    Private _CODIGO_FATURA As Integer
    Public Property CODIGO_FATURA As Integer
        Get
            Return _CODIGO_FATURA
        End Get
        Set(value As Integer)
            _CODIGO_FATURA = value
        End Set
    End Property

    Private _codigo_plano As Integer
    Public Property CodigoPlano As Integer
        Get
            Return _codigo_plano
        End Get
        Set(value As Integer)
            _codigo_plano = value
        End Set
    End Property



End Class
