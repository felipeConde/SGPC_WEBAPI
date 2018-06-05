Imports Microsoft.VisualBasic

Public Class cdr

    Private _cdrCodigo As String
    Private _nodeRamal As String
    Private _nodeTrunk As String
    Private _dataInicio As String
    Private _duracao As Double
    Private _dataFim As String
    Private _numero_b As String
    Private _rml_numero_a As String
    Private _valor_cdr2 As String
    Private _descricao As String
    Private _destino As String
    Private _tipoServ As String
    Private _tipoServ2 As String
    Private _valorAudit As Double
    Private _valorCdr As Double
    Private _valorOk As Integer
    Private _tipo_ligacao As Integer
    Private _tarif_codigo As Integer

    Public Sub New()

    End Sub

    Public Property CdrCodigo() As String
        Get
            Return _cdrCodigo
        End Get
        Set(ByVal value As String)
            _cdrCodigo = value
        End Set
    End Property

    Public Property NodeRamal() As String
        Get
            Return _nodeRamal
        End Get
        Set(ByVal value As String)
            _nodeRamal = value
        End Set
    End Property

    Public Property NodeTrunk() As String
        Get
            Return _nodeTrunk
        End Get
        Set(ByVal value As String)
            _nodeTrunk = value
        End Set
    End Property

    Public Property dataInicio() As String
        Get
            Return _dataInicio
        End Get
        Set(ByVal value As String)
            _dataInicio = value
        End Set
    End Property

    Public Property Duracao() As Double
        Get
            Return _duracao
        End Get
        Set(ByVal value As Double)
            _duracao = value
        End Set
    End Property

    Public Property DataFim() As String
        Get
            Return _dataFim
        End Get
        Set(ByVal value As String)
            _dataFim = value
        End Set
    End Property

    Public Property NumeroB() As String
        Get
            Return _numero_b
        End Get
        Set(ByVal value As String)
            _numero_b = value
        End Set
    End Property

    Public Property RmlNumeroA() As String
        Get
            Return _rml_numero_a
        End Get
        Set(ByVal value As String)
            _rml_numero_a = value
        End Set
    End Property

    Public Property ValorCdrs2() As String
        Get
            Return _valor_cdr2
        End Get
        Set(ByVal value As String)
            _valor_cdr2 = value
        End Set
    End Property

    Public Property Descricao() As String
        Get
            Return _descricao
        End Get
        Set(ByVal value As String)
            _descricao = value
        End Set
    End Property

    Public Property Tipo_Serv() As String
        Get
            Return _tipoServ
        End Get
        Set(ByVal value As String)
            _tipoServ = value
        End Set
    End Property

    Public Property Tipo_Serv2() As String
        Get
            Return _tipoServ2
        End Get
        Set(ByVal value As String)
            _tipoServ2 = value
        End Set
    End Property

    Public Property ValorAudit() As Double
        Get
            Return _valorAudit
        End Get
        Set(ByVal value As Double)
            _valorAudit = value
        End Set
    End Property

    Public Property ValorCDR() As Double
        Get
            Return _valorCdr
        End Get
        Set(ByVal value As Double)
            _valorCdr = value
        End Set
    End Property

    Public Property ValorOk() As Integer
        Get
            Return _valorOk
        End Get
        Set(ByVal value As Integer)
            _valorOk = value
        End Set
    End Property

    Public Property Tipo_ligacao() As Integer
        Get
            Return _tipo_ligacao
        End Get
        Set(ByVal value As Integer)
            _tipo_ligacao = value
        End Set
    End Property

    Public Property Tarif_Codigo() As Integer
        Get
            Return _tarif_codigo
        End Get
        Set(ByVal value As Integer)
            _tarif_codigo = value
        End Set
    End Property





End Class

