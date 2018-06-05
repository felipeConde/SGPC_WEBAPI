Imports Microsoft.VisualBasic

Public Class AppMoveis

    Private _codigo_aparelho As Integer
    Private _valor As Integer
    Private _acessorios As String
    Private _natureza As String
    Private _imei As String
    Private _hexa As String
    Private _garantia As Date
    Private _cod_modelo As Integer
    Private _notal_fiscal As String
    Private _pin_aparelho As String
    Private _estoque As String
    Private _backup As String
    Private _sucata As String
    Private _prop_estoque As String
    Private _ordem_servico As String
    Private _chamado_retirada As String
    Private _data_retirada As Date
    Private _emissao As Date
    Private _perdido As String

    Public Sub New()
    End Sub

    Public Sub New(ByVal pcodigo_aparelho As Integer)
        _codigo_aparelho = pcodigo_aparelho
    End Sub


    Public Property Valor() As Integer
        Get
            Return _valor
        End Get
        Set(ByVal value As Integer)
            _valor = value
        End Set
    End Property

    Public Property Acessorios() As String
        Get
            Return _acessorios
        End Get
        Set(ByVal value As String)
            _acessorios = value
        End Set
    End Property

    Public Property Natureza() As String
        Get
            Return _natureza
        End Get
        Set(ByVal value As String)
            _natureza = value
        End Set
    End Property

    Public Property Imei() As String
        Get
            Return _imei
        End Get
        Set(ByVal value As String)
            _imei = value
        End Set
    End Property

    Public Property Hexa() As String
        Get
            Return _hexa
        End Get
        Set(ByVal value As String)
            _hexa = value
        End Set
    End Property

    Public Property Garantia() As Date
        Get
            Return _garantia
        End Get
        Set(ByVal value As Date)
            _garantia = value
        End Set
    End Property

    Public Property cod_modelo() As Integer
        Get
            Return _cod_modelo
        End Get
        Set(ByVal value As Integer)
            _cod_modelo = value
        End Set
    End Property

    Public Property Nota_fiscal() As String
        Get
            Return _notal_fiscal
        End Get
        Set(ByVal value As String)
            _notal_fiscal = value
        End Set
    End Property

    Public Property Pin_Aparelho() As String
        Get
            Return _pin_aparelho
        End Get
        Set(ByVal value As String)
            _pin_aparelho = value
        End Set
    End Property

    Public Property Estoque() As String
        Get
            Return _estoque
        End Get
        Set(ByVal value As String)
            _estoque = value
        End Set
    End Property

    Public Property Backup() As String
        Get
            Return _backup
        End Get
        Set(ByVal value As String)
            _backup = value
        End Set
    End Property

    Public Property Sucata() As String
        Get
            Return _sucata
        End Get
        Set(ByVal value As String)
            _sucata = value
        End Set
    End Property


    Public Property Propriedade_estoque() As String
        Get
            Return _prop_estoque
        End Get
        Set(ByVal value As String)
            _prop_estoque = value
        End Set
    End Property

    Public Property Ordem_servico() As String
        Get
            Return _ordem_servico
        End Get
        Set(ByVal value As String)
            _ordem_servico = value
        End Set
    End Property

    Public Property Chamado_retirada() As String
        Get
            Return _chamado_retirada
        End Get
        Set(ByVal value As String)
            _chamado_retirada = value
        End Set
    End Property

    Public Property Data_retirada() As Date
        Get
            Return _data_retirada
        End Get
        Set(ByVal value As Date)
            _data_retirada = value
        End Set
    End Property

    Public Property Emissao() As Date
        Get
            Return _emissao
        End Get
        Set(ByVal value As Date)
            _emissao = value
        End Set
    End Property

    Public Property Perdido() As String
        Get
            Return _perdido
        End Get
        Set(ByVal value As String)
            _perdido = value
        End Set
    End Property
End Class