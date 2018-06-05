Imports Microsoft.VisualBasic

Public Class AppGrupo

    Private _codigo As String
    Private _grupo As String
    Private _tarifavel As String
    Private _ativo As String
    Private _responsavel As String
    Private _rateio As Double


    Public Sub New()

    End Sub

    Public Sub New(ByVal pCodigo As String, ByVal pGrupo As String)
        _codigo = pCodigo
        _grupo = pGrupo
    End Sub

    Public Sub New(ByVal pCodigo As String, ByVal pGrupo As String, ByVal pRateio As Double)
        _codigo = pCodigo
        _grupo = pGrupo
        _rateio = pRateio
    End Sub

    Public Sub New(ByVal pCodigo As String, ByVal pGrupo As String, ByVal pTarifavel As String, ByVal pAtivo As String, ByVal pResponsavel As String)
        _codigo = pCodigo
        _grupo = pGrupo
        _tarifavel = pTarifavel
        _ativo = pAtivo
        _responsavel = pResponsavel
    End Sub

    Public Property Codigo As String
        Get
            Return _codigo
        End Get
        Set(ByVal value As String)
            _codigo = value
        End Set
    End Property

    Public Property Grupo As String
        Get
            Return _grupo
        End Get
        Set(ByVal value As String)
            _grupo = value
        End Set
    End Property

    Public Property Tarifavel As String
        Get
            Return _tarifavel
        End Get
        Set(ByVal value As String)
            _tarifavel = value
        End Set
    End Property

    Public Property Ativo As String
        Get
            Return _ativo
        End Get
        Set(ByVal value As String)
            _ativo = value
        End Set
    End Property

    Public Property Responsavel As String
        Get
            Return _responsavel
        End Get
        Set(ByVal value As String)
            _responsavel = value
        End Set
    End Property

    Public Property Rateio As Double
        Get
            Return _rateio
        End Get
        Set(ByVal value As Double)
            _rateio = value
        End Set
    End Property

End Class
