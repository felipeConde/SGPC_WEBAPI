Imports Microsoft.VisualBasic

Public Class AppRateio

    Private _codigo As String
    'Private _fatura As String
    Private _descricao As String
    'Private _codigo_operadora As String
    'Private _codigo_tipo As String
    Private _linha_tipo As String
    'Private _competencia As String
    Private _valor As String
    Private _individual As String
    Private _data_criacao As String
    Private _codigo_fatura As String
    Private _num_linha As String
    Private _rateio_tipo As String
    Private _servicos As New List(Of String)


    Public Sub New()

    End Sub

    Public Sub New(ByVal pcodigo As String, ByVal pdescricao As String, ByVal pvalor As String, ByVal pindividual As String, ByVal pdata_criacao As String, ByVal pcodigo_fatura As String, ByVal plinha_tipo As String, ByVal pnum_linha As String, ByVal prateio_tipo As String)
        'ByVal pfatura As String, ByVal pdescricao As String, ByVal pcodigo_operadora As String, ByVal pcodigo_tipo As String, ByVal plinha_tipo As String, ByVal pcompetencia As String, 

        _codigo = pcodigo
        '_fatura = pfatura
        _descricao = pdescricao
        '_codigo_operadora = pcodigo_operadora
        '_codigo_tipo = pcodigo_tipo
        _linha_tipo = plinha_tipo
        '_competencia = pcompetencia
        _valor = pvalor
        _individual = pindividual
        _data_criacao = pdata_criacao
        _codigo_fatura = pcodigo_fatura
        _num_linha = pnum_linha
        _rateio_tipo = prateio_tipo

    End Sub

    Public Sub New(ByVal pcodigo As String, ByVal pdescricao As String, ByVal pvalor As String, ByVal pindividual As String, ByVal pdata_criacao As String, ByVal pcodigo_fatura As String, ByVal plinha_tipo As String, ByVal pnum_linha As String, ByVal prateio_tipo As String, pServicos As List(Of String), Optional pcodigoFranquia As String = "")
        'ByVal pfatura As String, ByVal pdescricao As String, ByVal pcodigo_operadora As String, ByVal pcodigo_tipo As String, ByVal plinha_tipo As String, ByVal pcompetencia As String, 

        _codigo = pcodigo
        '_fatura = pfatura
        _descricao = pdescricao
        '_codigo_operadora = pcodigo_operadora
        '_codigo_tipo = pcodigo_tipo
        _linha_tipo = plinha_tipo
        '_competencia = pcompetencia
        _valor = pvalor
        _individual = pindividual
        _data_criacao = pdata_criacao
        _codigo_fatura = pcodigo_fatura
        _num_linha = pnum_linha
        _rateio_tipo = prateio_tipo
        _servicos = pServicos
        _codigo_franquia = pcodigoFranquia

    End Sub

    Public Property Codigo() As String
        Get
            Return _codigo
        End Get
        Set(ByVal value As String)
            _codigo = value
        End Set
    End Property

    Public Property Num_Linha() As String
        Get
            Return _num_linha
        End Get
        Set(ByVal value As String)
            _num_linha = value
        End Set
    End Property

    'Public Property Fatura() As String
    '    Get
    '        Return _fatura
    '    End Get
    '    Set(ByVal value As String)
    '        _fatura = value
    '    End Set
    'End Property

    Public Property Descricao() As String
        Get
            Return _descricao
        End Get
        Set(ByVal value As String)
            _descricao = value
        End Set
    End Property

    'Public Property Codigo_operadora() As String
    '    Get
    '        Return _codigo_operadora
    '    End Get
    '    Set(ByVal value As String)
    '        _codigo_operadora = value
    '    End Set
    'End Property

    'Public Property Codigo_tipo() As String
    '    Get
    '        Return _codigo_tipo
    '    End Get
    '    Set(ByVal value As String)
    '        _codigo_tipo = value
    '    End Set
    'End Property

    Public Property Linha_tipo() As String
        Get
            Return _linha_tipo
        End Get
        Set(ByVal value As String)
            _linha_tipo = value
        End Set
    End Property

    'Public Property Competencia() As String
    '    Get
    '        Return _competencia
    '    End Get
    '    Set(ByVal value As String)
    '        _competencia = value
    '    End Set
    'End Property

    Public Property Valor() As String
        Get
            Return _valor
        End Get
        Set(ByVal value As String)
            _valor = value
        End Set
    End Property

    Public Property Individual() As String
        Get
            Return _individual
        End Get
        Set(ByVal value As String)
            _individual = value
        End Set
    End Property

    Public Property data_criacao() As String
        Get
            Return _data_criacao
        End Get
        Set(ByVal value As String)
            _data_criacao = value
        End Set
    End Property

    Public Property Codigo_fatura() As String
        Get
            Return _codigo_fatura
        End Get
        Set(ByVal value As String)
            _codigo_fatura = value
        End Set
    End Property

    Public Property Rateio_tipo() As String
        Get
            Return _rateio_tipo
        End Get
        Set(ByVal value As String)
            _rateio_tipo = value
        End Set
    End Property

    Public Property Servicos As List(Of String)
        Get
            Return _servicos
        End Get
        Set(value As List(Of String))
            _servicos = value
        End Set
    End Property

    Private _codigo_franquia As String
    Public Property codigo_franquia() As String
        Get
            Return _codigo_franquia
        End Get
        Set(ByVal value As String)
            _codigo_franquia = value
        End Set
    End Property


End Class


